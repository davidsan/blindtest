using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Blindtest.ViewModel;
using System.Windows.Threading;

namespace Blindtest.Service
{
    class NetworkManager
    {

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
        private Socket sock;
        public Socket Sock
        {
            get { return sock; }
            set { sock = value; }
        }

        private byte[] rep = new Byte[32767];
        public void InitSock(String address, int port)
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Blocking = true;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(address), port);
            sock.Connect(ipep);
        }
        public void Listen()
        {


            while (sock.Connected != false)
            {
                int count = sock.Receive(rep, rep.Length, 0);
                string srep = Encoding.ASCII.GetString(rep);
                string reponse = srep.Substring(0, count);
                String[] reponseSplit = reponse.Split(';');
                switch (reponseSplit[0])
                {
                    case "chatR":
                        string arg1 = reponseSplit[1];
                        Console.WriteLine(arg1);
                        break;
                    case "round":
                        string numRound = reponseSplit[1];
                        string song1 = reponseSplit[2];
                        string song2 = reponseSplit[3];
                        string song3 = reponseSplit[4];
                        string song4 = reponseSplit[5];
                        string songUrl = reponseSplit[6];
                        QuizOnlineViewModel qvm;
                        MainWindow.Instance.Dispatcher.Invoke(() =>
                        {
                            qvm = MainWindow.Instance.DataContext as QuizOnlineViewModel;
                            qvm.Songs.Add(song1);
                            qvm.Songs.Add(song2);
                            qvm.Songs.Add(song3);
                            qvm.Songs.Add(song4);
                            qvm.CorrectSongUrl = songUrl;
                            qvm.RoundsCount = Convert.ToInt32(numRound); 
                        });
                        Console.WriteLine(numRound, song1, song2, song3, song4, songUrl);
                        break;
                    case "score":
                        string value = reponseSplit[1];
                        QuizOnlineViewModel q2vm;
                        MainWindow.Instance.Dispatcher.Invoke(() =>
                        {
                            q2vm = MainWindow.Instance.DataContext as QuizOnlineViewModel;
                            q2vm.Score = Convert.ToInt32(value);
                        });
                        Console.WriteLine(value);
                        break;
                    case "broadcast":
                        string message = reponseSplit[1];
                        Console.WriteLine("message  :" + message);

                        string message1 = reponseSplit[2];
                        Console.WriteLine("message 1 :" + message1);

                        String[] reponseSplit2 = message.Split(' ');
                        string message2 = reponseSplit[0];
                        Console.WriteLine("message 2 :" +message2);

                        if (message2.CompareTo("Welcome") == 0)
                        {
                            PlayViewModel pvm;
                            MainWindow.Instance.Dispatcher.Invoke(() =>
                            {
                                pvm = MainWindow.Instance.DataContext as PlayViewModel;
                                pvm.hasConnected = true;
                            });
                        }

                       // QuizOnlineViewModel q3vm;
                       // MainWindow.Instance.Dispatcher.Invoke(() =>
                       // {
                        //    q3vm = MainWindow.Instance.DataContext as QuizOnlineViewModel;
                         //   q3vm.CorrectSongTitre = message;
                       // });

                        
                        Console.WriteLine(message);
                        break;
                    default:
                        Console.WriteLine("Command error, please retry\n");
                        break;
                }
            }
            Thread.CurrentThread.Abort();
        }
    }
}
