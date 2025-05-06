using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Tournament_manager.Pages
{
    public partial class TournamentType : Page
    {
        public TournamentType()
        {
            InitializeComponent();
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
