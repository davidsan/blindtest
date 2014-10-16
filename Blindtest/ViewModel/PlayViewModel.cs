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
        bool hasClickedOnline = false;

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

        public PlayViewModel()
        {
            BtnPlayOffline = new RelayCommand(new Action<object>(PlayOffline), PredicateOffline);
            BtnPlayOnline = new RelayCommand(new Action<object>(PlayOnline), PredicateOnline);
            BtnReady = new RelayCommand(new Action<object>(Ready), PredicateReady);
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
            hasClickedOnline = true;
            Random r = new Random();
            String pseudo = "user" + r.Next(10000);
            String address = "localhoast";
            int port = 8888;

            NetworkManager nm = NetworkManager.Instance;
            nm.InitSock(address, port);
            // will create a new thread for listening
            // will also send a connect request
            Thread thr = new Thread(new ThreadStart(nm.Listen));
            thr.Start();


            String connectStr = "connect;" + pseudo + ";\n";

            byte[] reponseByServer = ASCIIEncoding.ASCII.GetBytes(connectStr.ToString());
            nm.Sock.Send(reponseByServer);

        }

        private void PlayOffline(object obj)
        {
            MainWindow.Instance.contentControl.Content = new QuizView();
            qvm = new QuizViewModel();
            MainWindow.Instance.DataContext = qvm;
            qvm.Play();
        }

        private void Ready(object obj)
        {

        }

        public QuizViewModel qvm { get; set; }
    }
}
