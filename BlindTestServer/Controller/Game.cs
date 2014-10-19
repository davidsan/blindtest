using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using BlindTestServer.Model;
using BlindTestServer.Tools;

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
        private Message Message;
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
            resetAllScore();
            donnee.CurrentRound = 0;
            Message.broadcastToAll("A new game will start !!");
        }

        /// <summary>
        /// Initialise un round
        /// Choix aléatoire des chansons
        /// List des personnes ayant trouvés vide
        /// Envoi round;numRound;s1;s2;s3;s4;url;
        /// </summary>
        private void initRound()
        {
            Console.WriteLine("Init du round " + donnee.CurrentRound);
            donnee.initQuiz();
            resetPlayer();
            donnee.UserAnswer = 0;
            donnee.UserWhoFindList.RemoveAll(x => true);
            List<String> songList = new List<String>();
            for (int i = 0; i < 4; i++)
            {
                songList.Add(donnee.Quiz.Songs.ElementAt(i).Title);
            }
            String res = "round;" + donnee.CurrentRound + ";"
                + songList.ElementAt(0) + ";"
                + songList.ElementAt(1) + ";"
                + songList.ElementAt(2) + ";"
                + songList.ElementAt(3) + ";"
                + donnee.Quiz.CorrectSong.Link + ";\n";
            Message.sendMessageToAll(res);
            Console.WriteLine("La bonne chanson est :" + donnee.Quiz.CorrectSong.Title);
            Message.broadcastToAll("Round " + donnee.CurrentRound +" will starting !!");
        }

        /// <summary>
        /// Traitement fin de round
        /// Envoi a tout les clients "score;score du client;"
        /// </summary>
        private void roundFinish()
        {
            Console.WriteLine("Fin du round " + donnee.CurrentRound);
            Message.broadcastToAll("Round " + donnee.CurrentRound + " is over !!");
            Message.broadcastToAll("La bonne chanson etait : " + donnee.Quiz.CorrectSong.Title);
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

        /// <summary>
        /// Traitement de fin de partie
        /// Affiche le score pour les clients
        /// </summary>
        private void gameOver()
        {
            Console.WriteLine("Game Over !!");
            Message.broadcastToAll("The game is over !!");
            Message.broadcastToAll("Score : ");
            StringBuilder sb = new StringBuilder("");
            foreach (Listener e in donnee.UserControlList)
            {
                e.IsReady = false;
                sb.Append(e.Username + " : " + e.Score +"\n");
            }
            Message.broadcastToAll(sb.ToString());
            donnee.NumberUserReady = 0;
          
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
