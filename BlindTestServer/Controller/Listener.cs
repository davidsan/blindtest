using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BlindTestServer.Tools;
using BlindTestServer.Model;

namespace BlindTestServer.Controller
{
    class Listener
    {
        #region Param
        private Socket sock;
        private Donnee donnee;
        private byte[] rep = new Byte[32767];
        private String[] reponseSplit;
        private String username;
        private Message Message;
        private bool guessed = false;
        private bool isReady = false;
        private int score = 0;

        public String Username
        {
            get { return username; }
            private set { username = value; }
        }
        public bool Guessed
        {
            get { return guessed; }
            set { guessed = value; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public Socket Sock
        {
            get { return sock; }
            private set { sock = value; }
        }

        public bool IsReady
        {
            get { return isReady; }
            set { isReady = value; }
        }
        #endregion

        #region Construcor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="donnee"></param>
        public Listener(Socket client, Donnee donnee, Message messages)
        {
            this.Sock = client;
            this.donnee = donnee;
            this.Message = messages;
            donnee.UserControlList.Add(this);
        }
        #endregion

        #region Listenner
        /// <summary>
        /// Listen user command
        /// </summary>
        public void Listen()
        {
            while (sock.Connected)
            {
                int count = sock.Receive(rep, rep.Length, 0);
                string srep = Encoding.ASCII.GetString(rep);
                string reponse = srep.Substring(0, count);
                reponseSplit = reponse.Split(';');
                switch (reponseSplit[0])
                {
                    case "exit" :
                        Console.WriteLine(username + " leave the server !!");
                        donnee.NumberUserReady--;
                        donnee.UserList.Remove(username);
                        donnee.SockList.Remove(sock);
                        donnee.UserControlList.Remove(this);
                        sock.Shutdown(SocketShutdown.Both);
                        sock.Close();
                        break;
                    case "connect" :
                        string arg1 = reponseSplit[1];
                        connect(arg1);
                        donnee.UserList.Add(username);
                        Message.sendMessage("connected;" + Username + ";", sock);
                        Message.sendMessageExceptOne("connectednew;" + Username + ";", sock);
                        Console.WriteLine(Username + " join the server !!");
                        break;
                    case "ready":
                        if (!IsReady)
                        {
                            donnee.NumberUserReady++;
                            IsReady = true;
                        }
                        else
                        {
                            donnee.NumberUserReady--;
                            IsReady = false;
                        }
                        if (donnee.NumberUserReady == donnee.MinJoueur)
                        {
                            donnee.startGame.Set();
                        }
                        break;
                    case "guess":
                        string title = reponseSplit[1];
                        if (guessed)
                        {
                            Message.broadcastOne("You already guess !!", sock);
                        }
                        else
                        {
                            guessed = true;
                            donnee.UserAnswer++;
                            if (checkRightTitle(title))
                            {
                                Message.broadcastOne("Your a champion !!", sock);
                                donnee.UserWhoFindList.Add(this);
                            }
                            else
                            {
                                Message.broadcastOne("Sorry not the good answer !!", sock);
                            }
                        }
                        if (donnee.UserAnswer == donnee.MinJoueur)
                        {
                            donnee.roundOver.Set();
                        }
                        break;
                    case "chat":
                        string msg = reponseSplit[1];
                        Message.sendMessageToAll("chatR;" + username + ";" + msg + ";");
                        break;
                    default:
                        Console.WriteLine("Command error, please retry\n");
                        break;
                }
            }
            Thread.CurrentThread.Abort();
        }
        #endregion

        #region Private Function
        /// <summary>
        /// True if the username exist on the server
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private void connect(String name)
        {
            int compteur = 0;
            String username = name;
            String testname = username;
            while (donnee.UserList.Exists(x => x.Equals(testname)))
            {
                testname = username + "#" + compteur;
                compteur++;
            }
            Username = testname;
        }

        private bool checkRightTitle(String title)
        {
            return donnee.Quiz.CorrectSong.Title.Equals(title);
        }
        #endregion

    }
}
