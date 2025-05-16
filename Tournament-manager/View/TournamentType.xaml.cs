using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Tournament_manager.Model;
using Tournament_manager.View;
using Tournament_manager.ViewModel;

namespace Tournament_manager.Pages
{
    public partial class TournamentType : Page
    {
        private TournamentTypeViewModel viewModel;
        public TournamentType()
        {
            InitializeComponent();
            viewModel = new TournamentTypeViewModel();
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
            if (clickedButton?.NavUri != null)
            {
                NavigationService.Navigate(clickedButton.NavUri);
            }
        }
    }
}
