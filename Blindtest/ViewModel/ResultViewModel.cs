using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blindtest.View;
using System.Windows.Input;
using Blindtest.Service;
using System.Windows.Controls;

namespace Blindtest.ViewModel
{
    class ResultViewModel : ViewModelBase
    {
        #region Constructor
        public ResultViewModel(String score)
        {
            this.scorefinal = score;
            BtnReplay = new RelayCommand(new Action<object>(Replay));
            BtnSettings = new RelayCommand(new Action<object>(Settings));
        }
        #endregion // Constructor

        #region Field
        private NetworkManager nm = NetworkManager.Instance;
        private bool isOnline;
        private String scorefinal;
        private ICommand btnReplay;
        private ICommand btnSettings;
        #endregion // Field

        #region Properties / Command

        public PlayViewModel pvm { get; set; }
        public bool IsOnline
        {
            get { return isOnline; }
            set { isOnline = value; }
        }
        
        public ICommand BtnReplay
        {
            get { return btnReplay; }
            set { btnReplay = value; }
        }

        public String ScoreFinal
        {
            get { return scorefinal; }
            set { scorefinal = value; OnPropertyChanged("ScoreFinal"); }
        }

        public ICommand BtnSettings
        {
            get { return btnSettings; }
            set { btnSettings = value; }
        }
        #endregion // Properties / Command

        #region Action / Function
        /// <summary>
        /// Affiche la fentre d'option
        /// </summary>
        /// <param name="obj"></param>
        private void Settings(object obj)
        {
            OptionViewModel opt = OptionViewModel.Instance;
            opt.PreviousView = MainWindow.Instance.contentControl.Content as UserControl;
            opt.PreviousViewModel = this;
            MainWindow.Instance.DataContext = opt;
            MainWindow.Instance.contentControl.Content = new OptionView();
        }

        /// <summary>
        /// Permet de revenir sur la fenetre de choix de jeu
        /// </summary>
        /// <param name="obj"></param>
        private void Replay(object obj)
        {
            MainWindow.Instance.contentControl.Content = new PlayView();
            pvm = new PlayViewModel(nm.Username);
            if (IsOnline)
            {
                pvm.hasConnected = true;
                pvm.hasClickedOnline = true;
            }
            MainWindow.Instance.DataContext = pvm;
            nm.IsInGame = false;

        }
        #endregion // Action / Function

    }
}
