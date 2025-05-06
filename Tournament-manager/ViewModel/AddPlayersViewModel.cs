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
        private Tournament tournament = new Tournament { Id = DateTime.Now.ToString("s")};

        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();

        public Tournament Tournament
        {
            get => tournament;
            set { tournament = value; OnPropertyChanged(); }
        }
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
        public ICommand StartTournamentCommand { get; }

        public AddPlayersViewModel()
        {
            AddPlayerCommand = new RelayCommand(AddPlayer);
            StartTournamentCommand = new RelayCommand(StartTournament);
        }

        private void AddPlayer()
        {
            if (!string.IsNullOrWhiteSpace(PlayerName))
            {
                Players.Add(new Player(PlayerName, SelectedDivision));
                PlayerName = string.Empty;
            }
        }

        public event Action<Tournament> TournamentStarted;

        private void StartTournament()
        {
            if (!string.IsNullOrWhiteSpace(TournamentName))
            {
                Tournament.Name = TournamentName;
                Tournament.RoundDurations = RoundDuration;
                Tournament.Players = Players.ToList();
                Tournament.Rounds.Add(RoundOne.MakeRoundOne(Tournament.Players));
            TournamentStarted?.Invoke(Tournament);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
