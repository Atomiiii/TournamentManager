using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tournament_manager.Helpers;
using Tournament_manager.Model;

namespace Tournament_manager.ViewModel
{
    public class ResultPageViewModel
    {
        public Tournament Tournament { get; }

        public ICommand PrintResultsCommand { get; }
        public ResultPageViewModel(Tournament tournament) 
        {
            Tournament = tournament;
            PrintResultsCommand = new RelayCommand(_ => PrintResults());
        }

        private async void PrintResults()
        {
            ToPdf pdfGenerator = new ToPdf();
            pdfGenerator.GenerateResultsPdf(Tournament);
        }
    }
}
