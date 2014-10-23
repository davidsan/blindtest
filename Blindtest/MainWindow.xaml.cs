﻿using Blindtest.View;
using Blindtest.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace Blindtest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static MainWindow Instance { get; set; }

        static MainWindow()
        {
            Instance = new MainWindow();

        }
        private MainWindow()
        {
            InitializeComponent();
            this.DataContext = new PlayViewModel();
            var viewModel = DataContext as PlayViewModel;
            this.contentControl.Content = new PlayView();
        }
    }
}
