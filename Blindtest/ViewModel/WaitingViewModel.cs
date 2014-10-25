using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Blindtest.View;

namespace Blindtest.ViewModel
{
    class WaitingViewModel : ViewModelBase
    {
        public WaitingViewModel()
        {
            btnSettings = new RelayCommand(new Action<object>(Settings));
        }
        private ICommand btnSettings;

        #region Command
        public ICommand BtnSettings
        {
            get { return btnSettings; }
            set { btnSettings = value; }
        }
        #endregion // Properties / Command

        #region Action
        private void Settings(object obj)
        {
            OptionViewModel opt = OptionViewModel.Instance;
            opt.PreviousView = MainWindow.Instance.contentControl.Content as UserControl;
            opt.PreviousViewModel = this;
            MainWindow.Instance.DataContext = opt;
            MainWindow.Instance.contentControl.Content = new OptionView();
        }
        #endregion
    }
}
