using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;
using Blindtest.View;
using System.Windows.Controls;

namespace Blindtest.ViewModel
{
    class LandingViewModel : ViewModelBase
    {
        #region Field
        private Random r = new Random();
        private String username;
        private ICommand btnGo;
        private ICommand btnSettings;
        #endregion // Field

        #region Constructor
        public LandingViewModel()
        {
            Username = "user" + r.Next(10000);
            BtnGo = new RelayCommand(new Action<object>(Go));
            BtnSettings = new RelayCommand(new Action<object>(Settings));
        }
        #endregion // Constructor

        #region Properties / Command
        public String Username
        {
            get { return username; }
            set { username = value; }
        }
        
        public ICommand BtnGo
        {
            get { return btnGo; }
            set { btnGo = value; }
        }

       
        public ICommand BtnSettings
        {
            get { return btnSettings; }
            set { btnSettings = value; }
        }
        #endregion // Properties / Command

        #region Action
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
        /// Donne un nouveau nom si le champs est vide
        /// Sinon affiche la fenetre de choix de jeu.
        /// Set le nom dans la Title bar
        /// </summary>
        /// <param name="obj"></param>
        private void Go(object obj)
        {
            if (Username == null || Username.Equals(""))
            {
                Username = "user" + r.Next(10000);
            }
            if (Username.Contains(";"))
            {
                Username = Username.Replace(";", "-");
            }
            MainWindow.Instance.contentControl.Content = new PlayView();
            MainWindow.Instance.Title += " - " + Username;
            MainWindow.Instance.DataContext = new PlayViewModel(Username);
        }
        #endregion // Action
    }
}
