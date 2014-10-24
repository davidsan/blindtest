using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blindtest.View;
using System.Windows.Input;
using Blindtest.Service;
using System.Threading;

namespace Blindtest.ViewModel
{
    class PlayViewModel : ViewModelBase
    {
        public bool hasClickedOnline = false;
        private NetworkManager nm = NetworkManager.Instance;

        public QuizViewModel qvm { get; set; }
        public QuizOnlineViewModel qovm { get; set; }

        private String username;
        public String Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged("Username"); }
        }

        public bool hasConnected { get; set; }

        private ICommand btnPlayOffline;
        public ICommand BtnPlayOffline
        {
            get { return btnPlayOffline; }
            set { btnPlayOffline = value; }
        }

        private ICommand btnPlayOnline;
        public ICommand BtnPlayOnline
        {
            get { return btnPlayOnline; }
            set { btnPlayOnline = value; }
        }

        private ICommand btnReady;
        public ICommand BtnReady
        {
            get { return btnReady; }
            set { btnReady = value; }
        }

        private ICommand btnDisconnect;

        public ICommand BtnDisconnect
        {
            get { return btnDisconnect; }
            set { btnDisconnect = value; }
        }
        
        private String messageError;

        public String MessageError
        {
            get { return messageError; }
            set { messageError = value; OnPropertyChanged("MessageError"); }
        }
         
        public PlayViewModel(String username)
        {
            Username = username;
            BtnPlayOffline = new RelayCommand(new Action<object>(PlayOffline), PredicateOffline);
            BtnPlayOnline = new RelayCommand(new Action<object>(PlayOnline), PredicateOnline);
            BtnDisconnect = new RelayCommand(new Action<object>(Disconnect), x => hasConnected);
            BtnReady = new RelayCommand(new Action<object>(Ready), x => hasConnected);
        }

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

            MainWindow.Instance.contentControl.Content = new QuizOnlineView();
            qovm = new QuizOnlineViewModel();
            MainWindow.Instance.DataContext = qovm;
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

    }
}
