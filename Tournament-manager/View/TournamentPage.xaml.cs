﻿using System;
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
using Tournament_manager.Model;
using Tournament_manager.ViewModel;

namespace Tournament_manager.View
{
    public partial class TournamentPage : Page
    {
        private TournamentViewModel viewModel;

        public TournamentPage(Tournament tournament)
        {
            InitializeComponent();
            viewModel = new TournamentViewModel(tournament);
            viewModel.NavigateToResultPage += OnNavigateToResultPage;
            DataContext = viewModel;
        }
        private void OnNavigateToResultPage(Tournament tournament)
        {
            NavigationService.Navigate(new ResultPage(tournament));
        }
    }
}
