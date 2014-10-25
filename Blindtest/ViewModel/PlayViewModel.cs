﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blindtest.View;
using System.Windows.Input;
using Blindtest.Service;
using System.Threading;
using System.Windows.Controls;

namespace Blindtest.ViewModel
{
    class PlayViewModel : ViewModelBase
    {
        #region Constructor
        public PlayViewModel(String username)
        {
            Username = username;
            BtnPlayOffline = new RelayCommand(new Action<object>(PlayOffline), PredicateOffline);
            BtnPlayOnline = new RelayCommand(new Action<object>(PlayOnline), PredicateOnline);
            BtnDisconnect = new RelayCommand(new Action<object>(Disconnect), x => hasConnected);
            BtnReady = new RelayCommand(new Action<object>(Ready), x => hasConnected);
            BtnSettings = new RelayCommand(new Action<object>(Settings));
        }
        #endregion // Constructor

        #region Field
        
        private NetworkManager nm = NetworkManager.Instance;
        private String username;
        private String messageError;
        private ICommand btnPlayOffline;
        private ICommand btnPlayOnline;
        private ICommand btnReady;
        private ICommand btnDisconnect;
        private ICommand btnSettings;
        #endregion // Field

        #region Properties / Command
        public QuizViewModel qvm { get; set; }
        public QuizOnlineViewModel qovm { get; set; }
        public bool hasClickedOnline = false;
        public bool hasConnected { get; set; }

        public String Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged("Username"); }
        }

        public ICommand BtnPlayOffline
        {
            get { return btnPlayOffline; }
            set { btnPlayOffline = value; }
        }

        public ICommand BtnPlayOnline
        {
            get { return btnPlayOnline; }
            set { btnPlayOnline = value; }
        }

        public ICommand BtnReady
        {
            get { return btnReady; }
            set { btnReady = value; }
        }

        public ICommand BtnDisconnect
        {
            get { return btnDisconnect; }
            set { btnDisconnect = value; }
        }

        public ICommand BtnSettings
        {
            get { return btnSettings; }
            set { btnSettings = value; }
        }

        public String MessageError
        {
            get { return messageError; }
            set { messageError = value; OnPropertyChanged("MessageError"); }
        }
        #endregion // Properties / Command

        #region Action / Function 
        private bool PredicateOnline(object obj)
        {
            return !hasClickedOnline;
        }

        private bool PredicateReady(object obj)
        {
            return hasClickedOnline;
        }

        private bool PredicateOffline(object obj)
        {
            return !hasClickedOnline;
        }

        private void PlayOnline(object obj)
        {
            if (!hasConnected)
            {
                String address = "127.0.0.1";
                int port = 8888;

                if (nm.InitSock(address, port))
                {
                // will create a new thread for listening
                // will also send a connect request
                Thread thr = new Thread(new ThreadStart(nm.Listen));
                thr.Start();

                String connectStr = "connect;" + Username + ";\n";

                byte[] reponseByServer = ASCIIEncoding.ASCII.GetBytes(connectStr.ToString());
                nm.Sock.Send(reponseByServer);

                hasClickedOnline = true;
                nm.IsOnline = true;
                }
                else
                {
                    MessageError = "Connection error. Server may be offline";
                }
            }
        }

        private void PlayOffline(object obj)
        {
            SongManager.Instance.SelectCategoryList(nm.Category);
            nm.Username = Username;
            MainWindow.Instance.contentControl.Content = new QuizView();
            qvm = new QuizViewModel();
            MainWindow.Instance.DataContext = qvm;
            qvm.Play();
        }

        private void Ready(object obj)
        {
            String connectStr = "ready;";
            byte[] reponseByServer = ASCIIEncoding.ASCII.GetBytes(connectStr.ToString());
            nm.Sock.Send(reponseByServer);

            nm.IsInGame = true;
            MainWindow.Instance.contentControl.Content = new WaitingView();
            MainWindow.Instance.DataContext = new WaitingViewModel(); ;
        }

        private void Disconnect(object obj)
        {
            nm.IsOnline = false;
            String connectStr = "disconnect;";
            byte[] reponseByServer = ASCIIEncoding.ASCII.GetBytes(connectStr.ToString());
            nm.Sock.Send(reponseByServer);
            hasClickedOnline = false;
            hasConnected = false;
        }

        private void Settings(object obj)
        {
            OptionViewModel opt = OptionViewModel.Instance;
            opt.PreviousView = MainWindow.Instance.contentControl.Content as UserControl;
            opt.PreviousViewModel = this;
            MainWindow.Instance.DataContext = opt;
            MainWindow.Instance.contentControl.Content = new OptionView();
        }
        #endregion // Action / Function

    }
}
