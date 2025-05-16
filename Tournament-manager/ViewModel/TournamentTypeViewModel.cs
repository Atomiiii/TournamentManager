using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Tournament_manager.Helpers;
using Tournament_manager.Model;





namespace Tournament_manager.ViewModel
{
    internal class TournamentTypeViewModel
    {
        public ICommand LoadTournamentCommand { get; }
        public TournamentTypeViewModel() {
            LoadTournamentCommand = new RelayCommand(_ => LoadTournament());
        }

        public event Action<Tournament> TournamentStarted;
        public void LoadTournament()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Json files (*.json)|*.json",
                Title = "Load Tournament"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    string json = File.ReadAllText(filePath);

                    var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                        PropertyNameCaseInsensitive = true
                    };

                    Tournament loadedTournament = JsonSerializer.Deserialize<Tournament>(json, options);

                    if (loadedTournament != null)
                    {
                        TournamentStarted?.Invoke(loadedTournament);
                    }
                    else
                    {
                        MessageBox.Show("Failed to load tournament data.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading tournament: {ex.Message}");
                }
            }
        }
    }
}
