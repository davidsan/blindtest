﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BlindTestServer.Tools;
using BlindTestServer.Model;
using BlindTestServer.Service;

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
        //private int score;
        private Score score;
        private ScoreContext scoreContext;

        public ScoreContext ScoreContext { get; set; }

        public Score Score
        {
            get { return score; }
            set { score = value; }
        }


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

        //public int Score
        //{
        //    get { return score; }
        //    set { score = value; }
        //}

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
            this.score = new Score();
            this.scoreContext = new ScoreContext();
            foreach (Score s in scoreContext.DbSetScores)
            {
                Console.WriteLine(string.Format("Score !!!!!!!!!!!! : {0} - {1}", s.Name, s.Points));
            }
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
                    case "exit":
                        disconnect();
                        sock.Shutdown(SocketShutdown.Both);
                        sock.Close();
                        if (donnee.UserAnswer == donnee.NumberUserReady)
                        {
                            donnee.roundOver.Set();
                        }
                        break;
                    case "disconnect":
                        disconnect();
                        Message.sendMessage("disconnected;", sock);
                        sock.Shutdown(SocketShutdown.Both);
                        sock.Close();
                        break;
                    case "connect":
                        string arg1 = reponseSplit[1];
                        connect(arg1);
                        donnee.UserList.Add(username);
                        score.Name = Username;
                        var scores = from s in scoreContext.DbSetScores
                                     where s.Name.StartsWith(username)
                                     select s;
                        if (scores.Count() == 0)
                        {
                            score.Points = 0;
                            scoreContext.DbSetScores.Add(score);
                            scoreContext.SaveChanges();
                        }
                        else
                        {
                            score = scores.First();
                        }

                        Message.sendMessage("connected;" + Username + ";", sock);
                        Message.sendMessageExceptOne("connectednew;" + Username + ";", sock);
                        Console.WriteLine(Username + " join the server !!");
                        break;
                    case "ready":
                        donnee.NumberUserReady++;
                        if (donnee.NumberUserReady >= donnee.MinJoueur)
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
                                foreach (Score s in scoreContext.DbSetScores)
                                {
                                    Console.WriteLine(string.Format("Score Guess : {0} - {1}", s.Name, s.Points));
                                }
                                donnee.UserWhoFindList.Add(this);
                                scoreContext.SaveChanges();
                            }
                            else
                            {
                                Message.broadcastOne("Sorry not the good answer !!", sock);
                            }
                        }
                        if (donnee.UserAnswer == donnee.NumberUserReady)
                        {
                            donnee.roundOver.Set();
                        }
                        break;
                    case "chat":
                        string msg = reponseSplit[1];
                        Message.sendMessageToAll("chatR;" + username + ";" + msg + ";");
                        break;
                    case "categoryandlevel":
                        String category = reponseSplit[1];
                        String level = reponseSplit[2];
                        donnee.ChooseCategoryList.Add(category);
                        donnee.ChooseLevelList.Add(level);
                        break;
                    case "timesup":
                        donnee.NumberOfTimesUp++;
                        if (donnee.NumberOfTimesUp + donnee.UserAnswer == donnee.NumberUserReady)
                        {
                            donnee.roundOver.Set();
                        }
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
        /// Verfie si le nom d'utilisateur est utilise ou non
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

        /// <summary>
        /// Deconnect un client
        /// </summary>
        private void disconnect()
        {
            Console.WriteLine(username + " leave the server !!");
            donnee.NumberUserReady--;
            donnee.UserList.Remove(username);
            donnee.SockList.Remove(sock);
            donnee.UserControlList.Remove(this);
        }

        /// <summary>
        /// Test si la chanson envoyer par le client est la bonne
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        private bool checkRightTitle(String title)
        {
            return donnee.Quiz.CorrectSong.Title.Equals(title);
        }
        #endregion

    }
}
