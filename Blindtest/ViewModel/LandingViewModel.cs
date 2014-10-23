using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;
using Blindtest.View;

namespace Blindtest.ViewModel
{
    class LandingViewModel : ViewModelBase
    {
        Random r = new Random();
        public LandingViewModel()
        {
            Username = "user" + r.Next(10000);
            BtnGo = new RelayCommand(new Action<object>(Go));
        }

        private String username;

        public String Username
        {
            get { return username; }
            set { username = value; }
        }
        
        private ICommand btnGo;

        public ICommand BtnGo
        {
            get { return btnGo; }
            set { btnGo = value; }
        }


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
    }
}
