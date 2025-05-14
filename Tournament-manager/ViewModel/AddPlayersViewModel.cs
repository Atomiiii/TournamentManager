using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tournament_manager.Helpers;
using Tournament_manager.Model;
using System.Text.Json;
using System.IO;
using System.Windows;

namespace Tournament_manager.ViewModel
{
    public class AddPlayersViewModel : INotifyPropertyChanged
    {
        // parameters
        private readonly string savePlayersPath;

        private string playerName;
        private PlayerDivision selectedDivision;
        private string tournamentName;
        private int roundDuration;
        private Tournament tournament = new Tournament { Id = DateTime.Now.ToString("s") };

        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
        public ObservableCollection<Player> LoadedPlayers { get; set; } = new ObservableCollection<Player>();

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

        private string roundDurationText;
        public string RoundDurationText
        {
            get => roundDurationText;
            set
            {
                roundDurationText = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddPlayerCommand { get; }
        public ICommand SavePlayerCommand { get; }
        public ICommand DeletePlayerCommand { get; }
        public ICommand SaveTournamentCommand { get; }
        public ICommand StartTournamentCommand { get; }
        public ICommand DeleteLoadedPlayerCommand { get; }
        public ICommand AddLoadedPlayerCommand { get; }

        // constructor
        public AddPlayersViewModel()
        {
            var appDataPath = Path.Combine(
                   Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                   "TournamentManager",
                   "SavedPlayersData"
               );
            Directory.CreateDirectory(appDataPath);
            savePlayersPath = Path.Combine(appDataPath, "players.json");


            TournamentName = Tournament.Name;
            RoundDurationText = Tournament.RoundDurations.ToString();
            List<Player> loadedPlayers = LoadPlayersAsync(savePlayersPath).Result;
            LoadedPlayers = new ObservableCollection<Player>(loadedPlayers);

            AddPlayerCommand = new RelayCommand(_ => AddPlayer());
            DeletePlayerCommand = new RelayCommand(p => DeletePlayer(p as Player));
            SavePlayerCommand = new RelayCommand(_ => SavePlayer());
            StartTournamentCommand = new RelayCommand(_ => StartTournament());
            DeleteLoadedPlayerCommand = new RelayCommand(p => DeleteLoadedPlayer(p as Player));
            AddLoadedPlayerCommand = new RelayCommand(p => AddLoadedPlayer(p as Player));
        }

        // methods
        private void DeletePlayer(Player player)
        {
            if (player != null && Players.Contains(player))
            {
                Players.Remove(player);
            }
        }
        private async void DeleteLoadedPlayer(Player player)
        {
            if (player != null)
            {
                LoadedPlayers.Remove(player);
                string json = JsonSerializer.Serialize(LoadedPlayers);
                await File.WriteAllTextAsync(savePlayersPath, json);
            }
        }
        private void AddLoadedPlayer(Player player)
        {
            if (player != null && !Players.Contains(player))
            {
                Players.Add(player);
            }
        }

        public static async Task<List<Player>> LoadPlayersAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<Player>();
            }
            string loadedData = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Player>>(loadedData) ?? new List<Player>();
        }
        private void AddPlayer()
        {
            if (!string.IsNullOrWhiteSpace(PlayerName))
            {
                Players.Add(new Player(PlayerName, SelectedDivision));
                PlayerName = string.Empty;
            }
        }
        private async void SavePlayer()
        {
            if (!string.IsNullOrWhiteSpace(PlayerName))
            {
                Player player = new Player(PlayerName, SelectedDivision);
                LoadedPlayers.Add(player);
                var save = JsonSerializer.Serialize(LoadedPlayers);
                await File.WriteAllTextAsync(savePlayersPath, save);
            }
        }

        public event Action<Tournament> TournamentStarted;

        private async Task StartTournament()
        {
            if (!string.IsNullOrWhiteSpace(TournamentName))
            {
                if (Players.Count <= 4)
                {
                    MessageBox.Show("Minimum number of players for a swiss tournament is 5.");
                    return;
                }
                else if (int.TryParse(RoundDurationText, out int roundDuration))
                {
                    Tournament.Name = TournamentName;
                    Tournament.RoundDurations = roundDuration;
                    Tournament.Players = Players.ToList();
                    Round round = await RoundOne.MakeRoundOne(Tournament.Players);
                    Tournament.Rounds.Add(round);
                    TournamentStarted?.Invoke(Tournament);
                } else
                {
                    MessageBox.Show("Please enter a valid number for round duration.");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
