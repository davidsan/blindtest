using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Blindtest.ViewModel;
using Blindtest.View;
using System.Windows.Threading;

namespace Blindtest.Service
{
    class NetworkManager
    {
        #region Constructor
        private static NetworkManager instance;
        private NetworkManager() { }
        public static NetworkManager Instance
        {
            get
            {
                if (instance == null) { instance = new NetworkManager(); }
                return instance;
            }
        }
        #endregion // Constructor

        #region Field
        private bool isInGame;
        private bool isOnline;
        private String username;
        private Socket sock;
        private String category = "All";
        private byte[] rep = new Byte[32767];
        #endregion // Field

        #region Properties
        public bool IsInGame
        {
            get { return isInGame; }
            set { isInGame = value; }
        }

        
        public bool IsOnline
        {
            get { return isOnline; }
            set { isOnline = value; }
        }
        
        
        public String Username
        {
            get { return username; }
            set { username = value; }
        }

        public String Category
        {
            get { return category; }
            set { category = value; }
        }
        
        public Socket Sock
        {
            get { return sock; }
            set { sock = value; }
        }
        #endregion // Properties


        public bool InitSock(String address, int port)
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Blocking = true;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(address), port);
            try { sock.Connect(ipep); }
            catch (Exception e) { return false; }
            return true;
        }

        public void Listen()
        {
            while (sock.Connected)
            {
                try
                {
                    int count = sock.Receive(rep, rep.Length, 0);

                    string srep = Encoding.ASCII.GetString(rep);
                    string reponse = srep.Substring(0, count);
                    Console.WriteLine("Client : " + reponse);
                    String[] reponseSplit = reponse.Split(';');
                    QuizOnlineViewModel qvm;
                    switch (reponseSplit[0])
                    {
                        case "connected":
                            Username = reponseSplit[1];
                            PlayViewModel pvm;
                            MainWindow.Instance.Dispatcher.Invoke(() =>
                            {
                                Console.WriteLine("Set new Title for " + Username);
                                MainWindow.Instance.Title = "THE BLINDTEST - " + Username;
                                pvm = MainWindow.Instance.DataContext as PlayViewModel;
                                pvm.hasConnected = true;
                                pvm.Username = Username;
                            });
                            break;
                        case "connectednew":
                            Console.WriteLine(Username + " join the server !!");
                            break;
                        case "disconnected":
                            sock.Close();
                            break;
                        case "chatR":
                            string arg1 = reponseSplit[1];
                            Console.WriteLine(arg1);
                            break;
                        case "newgame" :
                             MainWindow.Instance.Dispatcher.Invoke(() =>
                            {
                                MainWindow.Instance.DataContext = new QuizOnlineViewModel();
                                MainWindow.Instance.contentControl.Content = new QuizOnlineView();
                            });
                            break;
                        case "round":
                            string numRound = reponseSplit[1];
                            string song1 = reponseSplit[2];
                            string song2 = reponseSplit[3];
                            string song3 = reponseSplit[4];
                            string song4 = reponseSplit[5];
                            string songUrl = reponseSplit[6];
                            MainWindow.Instance.Dispatcher.Invoke(() =>
                            {
                                qvm = MainWindow.Instance.DataContext as QuizOnlineViewModel;
                                Console.WriteLine("NEw SONG COMMING");
                                qvm.Songs.Clear();
                                qvm.Songs.Add(song1);
                                qvm.Songs.Add(song2);
                                qvm.Songs.Add(song3);
                                qvm.Songs.Add(song4);
                                qvm.CorrectSongUrl = songUrl.Trim();
                                qvm.RoundsCount = Convert.ToInt32(numRound);
                                qvm.Play();
                            });
                            Console.WriteLine(numRound, song1, song2, song3, song4, songUrl);
                            break;
                        case "roundover":
                            String correctSongTitle = reponseSplit[1];
                            MainWindow.Instance.Dispatcher.Invoke(() =>
                            {
                                qvm = MainWindow.Instance.DataContext as QuizOnlineViewModel;
                                qvm.CorrectSongTitre = correctSongTitle;
                            });
                            break;
                        case "score":
                            string value = reponseSplit[1];
                            MainWindow.Instance.Dispatcher.Invoke(() =>
                            {
                                qvm = MainWindow.Instance.DataContext as QuizOnlineViewModel;
                                qvm.Score = Convert.ToInt32(value);
                            });
                            Console.WriteLine(value);
                            break;
                        case "gameover":
                            MainWindow.Instance.Dispatcher.Invoke(() =>
                            {
                                MainWindow.Instance.contentControl.Content = new ResultView();
                                ResultViewModel rvm = new ResultViewModel("");
                                rvm.IsOnline = true;
                                MainWindow.Instance.DataContext = rvm;

                            });
                            break;
                        case "broadcast":
                            string message = reponseSplit[1];
                            if (message.Contains("Score : "))
                            {
                                ResultViewModel rvm;
                                MainWindow.Instance.Dispatcher.Invoke(() =>
                                {
                                    Console.WriteLine("Cool");
                                    rvm = MainWindow.Instance.DataContext as ResultViewModel;
                                    Console.WriteLine("Cool2");
                                    rvm.ScoreFinal = message;
                                });
                            }

                            Console.WriteLine(message);
                            break;
                        default:
                            Console.WriteLine("Command error, please retry\n");
                            break;
                    }
                }
                catch (Exception e) { }
            }
                
            Thread.CurrentThread.Abort();
        }
    }
}
