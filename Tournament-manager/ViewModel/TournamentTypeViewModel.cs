using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Tournament_manager.Helpers;
using Tournament_manager.Model;

using Tournament_manager.View;





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
                        // Now navigate to the page and pass the tournament
                        
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
