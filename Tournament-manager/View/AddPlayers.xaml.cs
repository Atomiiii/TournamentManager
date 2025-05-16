using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Tournament_manager.Model;
using Tournament_manager.View;
using Tournament_manager.ViewModel;

namespace Tournament_manager.Pages
{
    public partial class AddPlayersPage : Page
    {
        private AddPlayersViewModel viewModel;

        public AddPlayersPage()
        {
            InitializeComponent();
            viewModel = new AddPlayersViewModel();
            viewModel.TournamentStarted += OnTournamentStarted;
            DataContext = viewModel;
        }

        private void OnTournamentStarted(Tournament tournament)
        {
            var tournamentPage = new TournamentPage(tournament);
            NavigationService.Navigate(tournamentPage);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = e.OriginalSource as NavButton;
        }
        private void LoadedPlayersGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid grid && grid.SelectedItem is Player selectedPlayer)
            {
                if (DataContext is AddPlayersViewModel vm)
                {
                    vm.AddLoadedPlayerCommand.Execute(selectedPlayer);
                }
            }
        }
    }
}
