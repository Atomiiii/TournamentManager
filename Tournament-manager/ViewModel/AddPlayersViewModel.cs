using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tournament_manager.Helpers;
using Tournament_manager.Model;

namespace Tournament_manager.ViewModel
{
    public class AddPlayersViewModel : INotifyPropertyChanged
    {
        private string playerName;
        private PlayerDivision selectedDivision;
        private string tournamentName;
        private int roundDuration;

        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();

        public string PlayerName
        {
            get => playerName;
            set { playerName = value; OnPropertyChanged(); }
        }

        public PlayerDivision SelectedDivision
        {
            get => selectedDivision;
            set { selectedDivision = value; OnPropertyChanged(nameof(SelectedDivision)); }
        }
        public Array Divisions { get; } = Enum.GetValues(typeof(PlayerDivision));


        public string TournamentName
        {
            get => tournamentName;
            set { tournamentName = value; OnPropertyChanged(); }
        }

        public int RoundDuration
        {
            get => roundDuration;
            set { roundDuration = value; OnPropertyChanged(); }
        }

        public ICommand AddPlayerCommand { get; }
        public ICommand SaveTournamentCommand { get; }

        public AddPlayersViewModel()
        {
            AddPlayerCommand = new RelayCommand(AddPlayer);
            SaveTournamentCommand = new RelayCommand(SaveTournament);
        }

        private void AddPlayer()
        {
            if (!string.IsNullOrWhiteSpace(PlayerName))
            {
                Players.Add(new Player(PlayerName, SelectedDivision));
                PlayerName = string.Empty;
            }
        }

        private void SaveTournament()
        {
            Tournament tournament = new Tournament
            {
                Name = TournamentName,
                RoundDurations = RoundDuration,
                Players = Players.ToList()
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
