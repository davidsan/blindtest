using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blindtest.View;
using System.Windows.Input;

namespace Blindtest.ViewModel
{
    class PlayViewModel : ViewModelBase
    {
        private ICommand btnPlay;
        public ICommand BtnPlay
        {
            get { return btnPlay; }
            set { btnPlay = value; }
        }

        public PlayViewModel()
        {
            BtnPlay = new RelayCommand(new Action<object>(Play));
        }

        private void Play(object obj)
        {
            MainWindow.Instance.contentControl.Content = new QuizView();
            qvm = new QuizViewModel();
            MainWindow.Instance.DataContext = qvm;
            qvm.Play();
        }

        public QuizViewModel qvm { get; set; }
    }
}
