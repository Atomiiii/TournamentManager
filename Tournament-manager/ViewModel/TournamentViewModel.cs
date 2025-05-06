using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tournament_manager.Model;

namespace Tournament_manager.ViewModel
{
    public class TournamentViewModel : INotifyPropertyChanged
    {
        public Tournament Tournament { get; }
        public string ShowTournamentName => $"Tournament: {Tournament.Name}";

        private ObservableCollection<Player> _matches;
        public ObservableCollection<Player> Matches
        {
            get => _matches;
            set
            {
                _matches = value;
                OnPropertyChanged();
            }
        }

        public TournamentViewModel(Tournament tournament)
        {
            Tournament = tournament;
            Matches = new ObservableCollection<Player>(Tournament.Rounds[0].Matches.SelectMany(match => new[] { match.Player1, match.Player2 }.Where(player => player != null)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
