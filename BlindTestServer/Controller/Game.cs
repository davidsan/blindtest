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
        /// <summary>
        /// Fonction Threader qui lance une partie lorsque 
        /// l'evenement startGame est reçu
        /// </summary>
        public void Run()
        {
            while (true)
            {
                donnee.startGame.WaitOne();
                InitGame();
                while (donnee.CurrentRound != donnee.MaxRound+1)
                {
                    donnee.CurrentRound++;
                    InitRound();
                    donnee.roundOver.WaitOne();
                    RoundFinish();
                }
                GameOver();
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Initialise une partie
        /// Mise a zero des scores
        /// </summary>
        private void InitGame()
        {
            category = donnee.randomCategory();
            SongManager.Instance.SelectCategoryList(category);
            donnee.randomLevel();
            Message.SendMessageToAll("newgame;");
            ResetAllScore();
            donnee.CurrentRound = 0;
        }

        /// <summary>
        /// Initialise un round
        /// Choix aléatoire des chansons
        /// List des personnes ayant trouvés vide
        /// Envoi round;numRound;s1;s2;s3;s4;url;
        /// </summary>
        private void InitRound()
        {
            lock (Message)
            {
                Console.WriteLine("Init du round " + donnee.CurrentRound);
                donnee.initQuiz();
                ResetPlayer();
                donnee.UserAnswer = 0;
                donnee.NumberOfTimesUp = 0;
                donnee.UserWhoFindList.RemoveAll(x => true);
                StringBuilder sb = new StringBuilder("");
                sb.Append("round;" + donnee.CurrentRound + ";" + donnee.NumberOfSong + ";");
                for (int i = 0; i < donnee.NumberOfSong; i++)
                {
                    sb.Append(donnee.Quiz.Songs.ElementAt(i).Title + ";");
                }
                sb.Append(donnee.Quiz.CorrectSong.Link + ";");
                Message.SendMessageToAll(sb.ToString());
                Console.WriteLine("La bonne chanson est :" + donnee.Quiz.CorrectSong.Title);
            }
        }

        /// <summary>
        /// Traitement fin de round
        /// Envoi a tout les clients "score;score du client;"
        /// </summary>
        private void RoundFinish()
        {
            lock (Message)
            {
                Console.WriteLine("Fin du round " + donnee.CurrentRound);
                Message.SendMessageToAll("roundover;" + donnee.Quiz.CorrectSong.Title+ ";");
                int score = 10;
                //  maj du score
                foreach (Listener e in donnee.UserWhoFindList)
                {
                    e.ScoreRound += score;
                    if (score > 5)
                    {
                        score--;
                    }
                        
                }
                // envoie du score
                foreach (Listener e in donnee.UserControlList)
                {
                    Message.SendMessage("score;" + e.ScoreRound + ";", e.Sock);
                }
            }
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// Traitement de fin de partie
        /// Affiche le score pour les clients
        /// </summary>
        private void GameOver()
        {
            Console.WriteLine("Game Over !!");
            lock (Message)
            {
                Message.SendMessageToAll("gameover;\n");

                StringBuilder sb = new StringBuilder("");
                sb.Append("Score : ");
                foreach (Listener e in donnee.UserControlList)
                {
                    e.IsReady = false;
                    sb.Append(e.Username + " : " + e.ScoreRound + "\n");
                    e.ScoreUser.Points += e.ScoreRound;
                    e.ScoreContext.SaveChanges();
                }
                Message.BroadcastToAll(sb.ToString());
                System.Threading.Thread.Sleep(500);
                WriteAllTimeScore();
            }
            donnee.NumberUserReady = 0;
            donnee.ChooseCategoryList.Clear();
            donnee.ChooseLevelList.Clear();
            donnee.NumberOfSong = -1;
        }

        /// <summary>
        /// Reset a false la valeur de guessed des clients
        /// </summary>
        private void ResetPlayer() 
        {
            foreach (Listener e in donnee.UserControlList) {
                e.Guessed = false;
            }
        }

        /// <summary>
        /// Remet les scores a 0 des clients
        /// </summary>
        private void ResetAllScore()
        {
           foreach (Listener e in donnee.UserControlList)
            {
                e.ScoreRound = 0;
            }
        }

        /// <summary>
        /// Write all time score to all user
        /// </summary>
        private void WriteAllTimeScore()
        {
            StringBuilder sb = new StringBuilder("");
            ScoreContext sc = new ScoreContext();
            IEnumerable<Score> dbSet = sc.DbSetScores.OrderByDescending(item => item.Points);
            sb.Append("All time score : ");
            Score s;
            for (int i = 0; i < dbSet.Count() && i < 5; i++)
            {
                s = dbSet.ElementAt(i);
                Console.WriteLine(sb.Append(string.Format("{0} : {1}\n", s.Name, s.Points)));
            }
            Message.BroadcastToAll(sb.ToString());
        }
        #endregion
    }
}
