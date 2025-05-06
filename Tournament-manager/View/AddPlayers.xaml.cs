using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            // Navigate to the TournamentPage with the tournament passed in
            var tournamentPage = new TournamentPage(tournament);
            NavigationService.Navigate(tournamentPage);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = e.OriginalSource as NavButton;
            if (clickedButton?.NavUri != null)
            {
                // You can remove this old nav behavior if switching to event-based nav
            }
        }
    }
}
