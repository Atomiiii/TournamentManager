using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tournament_manager.Pages
{
    /// <summary>
    /// Interaction logic for TournamentType.xaml
    /// </summary>
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
