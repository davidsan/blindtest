using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using BlindTestServer.Model;
using BlindTestServer.Tools;
using BlindTestServer.Service;

namespace BlindTestServer.Controller
{
    class Game
    {
        #region Constructor & Params
        public Game(Donnee donnee, Message message)
        {
            this.donnee = donnee;
            this.Message = message;
        }

        private Donnee donnee;
        private byte[] reponseByServer = new byte[32767];
        private SongManager sm = SongManager.Instance;
        private Message Message;
        private String category;
        #endregion

        #region Main
        public void Run()
        {
            while (true)
            {
                donnee.startGame.WaitOne();
                initGame();
                while (donnee.CurrentRound != donnee.MaxRound+1)
                {
                    donnee.CurrentRound++;
                    initRound();
                    donnee.roundOver.WaitOne();
                    roundFinish();
                }
                gameOver();
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Initialise une partie
        /// Mise a zero des scores
        /// </summary>
        private void initGame()
        {
            category = donnee.randomCategory();
            SongManager.Instance.SelectCategoryList(category);
            donnee.randomLevel();
            Message.sendMessageToAll("newgame;");
            resetAllScore();
            donnee.CurrentRound = 0;
        }

        /// <summary>
        /// Initialise un round
        /// Choix aléatoire des chansons
        /// List des personnes ayant trouvés vide
        /// Envoi round;numRound;s1;s2;s3;s4;url;
        /// </summary>
        private void initRound()
        {
            lock (Message)
            {
                Console.WriteLine("Init du round " + donnee.CurrentRound);
                donnee.initQuiz();
                resetPlayer();
                donnee.UserAnswer = 0;
                donnee.NumberOfTimesUp = 0;
                donnee.UserWhoFindList.RemoveAll(x => true);
                List<String> songList = new List<String>();
                StringBuilder sb = new StringBuilder("");
                sb.Append("round;" + donnee.CurrentRound + ";" + donnee.NumberOfSong + ";");
                for (int i = 0; i < donnee.NumberOfSong; i++)
                {
                    sb.Append(donnee.Quiz.Songs.ElementAt(i).Title + ";");
                }
                sb.Append(donnee.Quiz.CorrectSong.Link + ";\n");
                Message.sendMessageToAll(sb.ToString());
                Console.WriteLine(sb.ToString());
                Console.WriteLine("La bonne chanson est :" + donnee.Quiz.CorrectSong.Title);
            }
        }

        /// <summary>
        /// Traitement fin de round
        /// Envoi a tout les clients "score;score du client;"
        /// </summary>
        private void roundFinish()
        {
            lock (Message)
            {
                Console.WriteLine("Fin du round " + donnee.CurrentRound);
                Message.sendMessageToAll("roundover;" + donnee.Quiz.CorrectSong.Title+ ";");
                int score = 10;
                //  maj du score
                foreach (Listener e in donnee.UserWhoFindList)
                {
                    e.Score += score;
                    if (score > 5)
                        score--;
                }
                // envoie du score
                foreach (Listener e in donnee.UserControlList)
                {
                    Message.sendMessage("score;" + e.Score + ";", e.Sock);
                }
            }
        }

        /// <summary>
        /// Traitement de fin de partie
        /// Affiche le score pour les clients
        /// </summary>
        private void gameOver()
        {
            Console.WriteLine("Game Over !!");
            lock (Message)
            {
                Message.sendMessageToAll("gameover;");

                StringBuilder sb = new StringBuilder("");
                sb.Append("Score : \n");
                foreach (Listener e in donnee.UserControlList)
                {
                    e.IsReady = false;
                    sb.Append(e.Username + " : " + e.Score + "\n");
                }
                Message.broadcastToAll(sb.ToString());
            }
            donnee.NumberUserReady = 0;
            donnee.ChooseCategoryList.Clear();
            donnee.ChooseLevelList.Clear();          
        }

        private void resetPlayer() 
        {
            foreach (Listener e in donnee.UserControlList) {
                e.Guessed = false;
            }
        }

        private void resetAllScore()
        {
            foreach (Listener e in donnee.UserControlList)
            {
                e.Score = 0;
            }
        }
        #endregion
    }
}
