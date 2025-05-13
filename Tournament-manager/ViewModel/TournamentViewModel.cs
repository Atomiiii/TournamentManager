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
using System.Windows.Input;
using Tournament_manager.Helpers;
using Tournament_manager.Model;

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
        public string ShowRoundNumber => $"Round {Tournament.currentRoundIndex}";

        public ICommand WinCommand { get; }
        public ICommand LoseCommand { get; }
        public ICommand DrawCommand { get; }
        public ICommand WarningCommand { get; }
        public ICommand DropCommand { get; }

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


            ShowUnfinishedMatchesCommand = new RelayCommand(_ => FilterMatches(MatchFilter.Unfinished));
            ShowFinishedMatchesCommand = new RelayCommand(_ => FilterMatches(MatchFilter.Finished));
        }

        // methods
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
            if (!(Tournament.RoundCount == Tournament.Rounds.Count))
            {
                if (allMatches.Any(m => m.Player1Result == null || m.Player2Result == null))
                {
                    MessageBox.Show("You must complete all matches before proceeding to the next round.");
                    return;
                }
                foreach (Match match in allMatches)
                {

                    if (match.Player1.HadBye)
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
                Tournament.currentRoundIndex++;
                OnPropertyChanged(nameof(ShowRoundNumber));
                Round currentRound = await Pairing.MakePairingAsynch(Tournament.Rounds.Count + 1, Tournament.Players);
                Tournament.Rounds.Add(currentRound);
                allMatches = new ObservableCollection<Match>(currentRound.Matches);
                DisplayedMatches = new ObservableCollection<Match>(allMatches);
            } else
            {

            }
 
        }
        // Method to filter matches based on their status
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
            if (isPlayer1 && !match.Player1.HadBye)
            {
                match.Player1Result = result;
                if (result == Result.Win)
                {
                    match.Player2Result = Result.Lose;
                }
                else if (result == Result.Lose)
                {
                    match.Player2Result = Result.Win;
                }
                else if (result == Result.Draw)
                {
                    match.Player2Result = Result.Draw;
                }
            }
            else
            {
                if (!(match.Player2 == null || match.Player2.HadBye))
                {
                    match.Player2Result = result;
                    if (result == Result.Win)
                    {
                        match.Player1Result = Result.Lose;
                    }
                    else if (result == Result.Lose)
                    {
                        match.Player1Result = Result.Win;
                    }
                    else if (result == Result.Draw)
                    {
                        match.Player1Result = Result.Draw;
                    }
                }
            }

            DisplayedMatches.Remove(match);

            FilterMatches(currentFilter);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}