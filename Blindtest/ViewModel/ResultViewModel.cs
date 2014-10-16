﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blindtest.View;
using System.Windows.Input;

namespace Blindtest.ViewModel
{
    class ResultViewModel : ViewModelBase
    {

        private ICommand btnReplay;
        public ICommand BtnReplay
        {
            get { return btnReplay; }
            set { btnReplay = value; }
        }

        public int scorefinal;
        public int ScoreFinal
        {
            get { return scorefinal; }
            set { scorefinal = value; OnPropertyChanged("ScoreFinal"); }
        }

        public ResultViewModel(int score)
        {
            this.scorefinal = score;
            BtnReplay = new RelayCommand(new Action<object>(Replay));
        }

        private void Replay(object obj)
        {
            MainWindow.Instance.contentControl.Content = new PlayView();
            pvm = new PlayViewModel();
            MainWindow.Instance.DataContext = pvm;

        }

        public PlayViewModel pvm { get; set; }
    }
}
