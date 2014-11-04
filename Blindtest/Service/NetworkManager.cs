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
    /// <summary>
    /// Classe permettant d'ecouter le serveur.
    /// Un NetworkManager par client
    /// Contient les informations utiles a un client
    /// </summary>
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
        private int port;
        private String adresse;
        private String username;
        private Socket sock;
        private String category = "All";
        private String level = "Easy";
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

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public String Adresse
        {
            get { return adresse; }
            set { adresse = value; }
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

        public String Level
        {
            get { return level; }
            set { level = value; }
        }
        
        public Socket Sock
        {
            get { return sock; }
            set { sock = value; }
        }
        #endregion // Properties

        /// <summary>
        /// Etablie la connexion avec le serveur
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool InitSock(String address, int port)
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Blocking = true;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(address), port);
            try { sock.Connect(ipep); }
            catch (Exception e) { return false; }
            return true;
        }

        /// <summary>
        /// Fonction qui est threader, 
        /// permet la reception des messages du serveur
        /// et fait le traitement qui va avec.
        /// </summary>
        public void Listen()
        {
            while (sock.Connected)
            {
                try
                {
                    /* Reception du message */
                    int count = sock.Receive(rep, rep.Length, 0);
                    string srep = Encoding.ASCII.GetString(rep);
                    string reponse = srep.Substring(0, count);
                    /* Fin reception */
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
                            string maxSongs = reponseSplit[2];
                            string song1 = reponseSplit[3];
                            string song2 = reponseSplit[4];
                            string song3 = reponseSplit[5];
                            string song4 = reponseSplit[6];
                            string song5 = null;
                            string song6 = null;
                            string song7 = null;
                            string song8 = null;
                            string songUrl = null;
                            switch (Convert.ToInt32(maxSongs))
                            {
                                case 4 :
                                    songUrl = reponseSplit[7];
                                    break;
                                case 6:
                                    song5 = reponseSplit[7];
                                    song6 = reponseSplit[8];
                                    songUrl = reponseSplit[9];
                                    break;
                                case 8 :
                                    song5 = reponseSplit[7];
                                    song6 = reponseSplit[8];
                                    song7 = reponseSplit[9];
                                    song8 = reponseSplit[10];
                                    songUrl = reponseSplit[11];
                                    break;
                                default :
                                    break;
                            }
                            
                            MainWindow.Instance.Dispatcher.Invoke(() =>
                            {
                                qvm = MainWindow.Instance.DataContext as QuizOnlineViewModel;
                                Console.WriteLine("NEW SONG COMMING");
                                qvm.Songs.Clear();
                                qvm.Songs.Add(song1);
                                qvm.Songs.Add(song2);
                                qvm.Songs.Add(song3);
                                qvm.Songs.Add(song4);
                                if (song5 != null)
                                {
                                    qvm.Songs.Add(song5);
                                    qvm.Songs.Add(song6);
                                }
                                if (song7 != null)
                                {
                                    qvm.Songs.Add(song7);
                                    qvm.Songs.Add(song8);
                                }
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
                                    rvm.ScoreFinal = message.Replace("Score : ", "");
                                });
                            }

                            if (message.Contains("All time score : "))
                            {
                                ResultViewModel rvm;
                                MainWindow.Instance.Dispatcher.Invoke(() =>
                                {
                                    Console.WriteLine("Cool");
                                    rvm = MainWindow.Instance.DataContext as ResultViewModel;
                                    Console.WriteLine("Cool2");
                                    rvm.ScoreAllTime = message.Replace("All time score : ", "");
                                });
                            }

                            Console.WriteLine(message);
                            break;
                        default:
                            Console.WriteLine("Command error, please retry\n");
                            break;
                    }
                }
                catch (Exception e) { Console.WriteLine(e.ToString()); }
            }
                
            Thread.CurrentThread.Abort();
        }
    }
}
