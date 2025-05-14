using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Tournament_manager.Helpers;
using Tournament_manager.Model;
using Tournament_manager.View;
using System.Windows.Navigation;

namespace Tournament_manager.ViewModel
{
    public class TournamentViewModel : INotifyPropertyChanged
    {
        public enum MatchFilter
        {
            Unfinished,
            Finished
        }
        public Tournament Tournament { get; }
        public string ShowTournamentName => $"Tournament: {Tournament.Name}";
        public string ShowRoundNumber => $"Round {Tournament.currentRoundIndex}/{Tournament.RoundCount}";

        public ICommand WinCommand { get; }
        public ICommand LoseCommand { get; }
        public ICommand DrawCommand { get; }
        public ICommand WarningCommand { get; }
        public ICommand DropCommand { get; }
        public ICommand PrintPairingsCommand { get; }

        public ICommand ShowUnfinishedMatchesCommand { get; }
        public ICommand ShowFinishedMatchesCommand { get; }
        public ICommand NextRoundCommand { get; }
        public ICommand SaveTournamentCommand { get; }

        private ObservableCollection<Match> allMatches;

        private ObservableCollection<Match> displayedMatches;
        public ObservableCollection<Match> DisplayedMatches
        {
            get => displayedMatches;
            set
            {
                displayedMatches = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Match> Matches => DisplayedMatches;

        private MatchFilter currentFilter = MatchFilter.Unfinished;

        // Constructor
        public TournamentViewModel(Tournament tournament)
        {
            Tournament = tournament;
            allMatches = new ObservableCollection<Match>(Tournament.Rounds[Tournament.currentRoundIndex - 1].Matches);
            DisplayedMatches = new ObservableCollection<Match>(allMatches);


            WinCommand = new RelayCommand(p => SetResult(p, Result.Win));
            LoseCommand = new RelayCommand(p => SetResult(p, Result.Lose));
            DrawCommand = new RelayCommand(p => SetResult(p, Result.Draw));
            NextRoundCommand = new RelayCommand(_ => NextRoundAsync());
            SaveTournamentCommand = new RelayCommand(_ => SaveTournamentAsync());
            PrintPairingsCommand = new RelayCommand(_ => PrintPairings());


            ShowUnfinishedMatchesCommand = new RelayCommand(_ => FilterMatches(MatchFilter.Unfinished));
            ShowFinishedMatchesCommand = new RelayCommand(_ => FilterMatches(MatchFilter.Finished));
        }

        // methods
        private async void PrintPairings()
        {
            ToPdf pdfGenerator = new ToPdf();
            pdfGenerator.GeneratePairingsPdf(Tournament.Rounds[Tournament.currentRoundIndex - 1]);
        }
        private async void SaveTournamentAsync()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Json files (*.json)|*.json",
                Title = "Save Tournament"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                };
                string json = JsonSerializer.Serialize(Tournament, options);
                File.WriteAllText(filePath, json);
            }
        }
        private async void NextRoundAsync()
        {
            
                if (allMatches.Any(m => m.Player1Result == null))
                {
                    return;
                }
                foreach (Match match in allMatches)
                {
                    if (match.Bye)
                    {
                        match.Player1.Wins++;
                        match.Player1.Score += 3;
                    }
                    else if (match.Player1Result == Result.Draw)
                    {
                        match.Player1.Draws++;
                        match.Player2.Draws++;
                        match.Player1.Score += 1;
                        match.Player2.Score += 1;
                        match.Player1.Points += match.Player1Pts;
                        match.Player2.Points += match.Player2Pts;
                    }
                    else
                    {
                        Player winner = match.Player1Result == Result.Win ? match.Player1 : match.Player2;
                        Player loser = match.Player1Result == Result.Lose ? match.Player1 : match.Player2;

                        loser.Losses++;
                        winner.Wins++;
                        winner.Score += 3;
                        match.Player1.Points += match.Player1Pts;
                        match.Player2.Points += match.Player2Pts;
                    }
                }
            if (!(Tournament.RoundCount == Tournament.Rounds.Count))
            {
                Tournament.currentRoundIndex++;
                OnPropertyChanged(nameof(ShowRoundNumber));
                Round currentRound = await Pairing.MakePairingAsynch(Tournament, Tournament.Rounds.Count + 1, Tournament.Players);
                currentRound.Matches.Sort((x, y) => x.TableNumber.CompareTo(y.TableNumber));
                Tournament.Rounds.Add(currentRound);
                allMatches = new ObservableCollection<Match>(currentRound.Matches);
                DisplayedMatches = new ObservableCollection<Match>(allMatches);
            } else
            {
                await Pairing.SortPlayers(Tournament.Players);
                for (int i = 0; i < Tournament.Players.Count; i++)
                {
                    Tournament.Players[i].Result = i + 1;
                }
                NavigateToResultPage?.Invoke(Tournament);
            }
        }
        public event Action<Tournament> NavigateToResultPage;

        private void FilterMatches(MatchFilter filter)
        {
            currentFilter = filter;

            switch (filter)
            {
                case MatchFilter.Unfinished:
                    DisplayedMatches = new ObservableCollection<Match>(
                        allMatches.Where(m => m.Player1Result == null));
                    break;
                case MatchFilter.Finished:
                    DisplayedMatches = new ObservableCollection<Match>(
                        allMatches.Where(m => m.Player1Result != null));
                    break;
            }
        }

        public void SetResult(object? parameter, Result result)
        {
            if (parameter is not Player targetPlayer)
                return;

            var match = DisplayedMatches.FirstOrDefault(m => m.Player1 == targetPlayer || m.Player2 == targetPlayer);
            if (match == null) return;

            bool isPlayer1 = parameter == match.Player1;

            Result result1;
            Result result2;
            if (isPlayer1)
            {
                result1 = result;
                result2 = result == Result.Win ? Result.Lose : (result == Result.Draw ? Result.Draw : Result.Lose);
            }
            else
            {
                result2 = result;
                result1 = result == Result.Win ? Result.Lose : (result == Result.Draw ? Result.Draw : Result.Lose);
            }
            match.Player1Result = result1;
            match.Player2Result = result2;

            DisplayedMatches.Remove(match);

            FilterMatches(currentFilter);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}