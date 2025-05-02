using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament_manager.Model;

namespace Tournament_manager.ViewModel
{
    internal class AddPlayersViewModel
    {
        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
        public AddPlayersViewModel()
        {

        }
    }
}
