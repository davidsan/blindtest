using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using BlindTestServer.Controller;

namespace BlindTestServer.Model
{
    /// <summary>
    /// Information utile pour le serveur
    /// </summary>
    class Donnee
    {
        #region Contrutor
        public Donnee()
        {
            userList = new List<string>();
            sockList = new List<Socket>();
            UserControlList = new List<Listener>();
            UserWhoFindList = new List<Listener>();
            chooseCategoryList = new List<String>();
            chooseLevelList = new List<String>();
        }
        #endregion

        #region Field
        public enum LevelEnum { EASY, MEDIUM, HARDCORE };
        // event leve pour demarre une partie
        public AutoResetEvent startGame = new AutoResetEvent(false); 
        // event leve pour la fin d'un round
        public AutoResetEvent roundOver = new AutoResetEvent(false);
        private List<String> userList; // List des utilisateurs
        private List<Socket> sockList; // List des sockets
        private List<Listener> userControlList; // Listener des gens connecte
        private List<Listener> userWhoFindList; // Joueur ayant la bonne reponse
        private List<String> chooseCategoryList; // Choix de categorie des joueurs
        private List<String> chooseLevelList; // Choix de difficulte des joueurs
        private int numberUserReady = 0; // Nombre de joueur pret
        private int minJoueur = 2; // Nombre de joueur minimum pour commence
        private int currentRound = 0; // Round courant
        private int maxRound = 2; // Nombre max de round
        private int userAnswer = 0; // Personne ayant repondu
        private int numberOfTimesUp = 0; // Nombre de personne ayant atteint la limite de temps
        private int numberOfSong = -1; // Nombre de chanson par round
        private LevelEnum level = LevelEnum.EASY;
        private Quiz quiz;
        #endregion

        #region Properties
        public List<String> UserList
        {
            get { return userList; }
            private set { userList = value; }
        }

        public List<Socket> SockList
        {
            get { return sockList; }
            private set { sockList = value; }
        }

        public List<Listener> UserWhoFindList
        {
            get { return userWhoFindList; }
            private set { userWhoFindList = value; }
        }
        
        public List<Listener> UserControlList
        {
            get { return userControlList; }
            private set { userControlList = value; }
        }

        public List<String> ChooseCategoryList
        {
            get { return chooseCategoryList; }
            set { chooseCategoryList = value; }
        }

        public List<String> ChooseLevelList
        {
            get { return chooseLevelList; }
            set { chooseLevelList = value; }
        }

        public int NumberUserReady
        {
            get { return numberUserReady; }
            set { numberUserReady = value; }
        }

        public int MinJoueur
        {
            get { return minJoueur; }
            set { minJoueur = value; }
        }

        public int CurrentRound
        {
            get { return currentRound; }
            set { currentRound = value; }
        }

        public int MaxRound
        {
            get { return maxRound; }
            private set { maxRound = value; }
        }

        public int UserAnswer
        {
            get { return userAnswer; }
            set { userAnswer = value; }
        }

        public int NumberOfTimesUp
        {
            get { return numberOfTimesUp; }
            set { numberOfTimesUp = value; }
        }

        public int NumberOfSong
        {
            get { return numberOfSong; }
            set { numberOfSong = value; }
        }

        public LevelEnum Level
        {
            get { return level; }
            set { level = value; }
        }

        public Quiz Quiz
        {
            get { return quiz; }
            private set { quiz = value; }
        }
        
        #endregion //Params

        #region Functions
   
        /// <summary>
        /// Init le round
        /// </summary>
        public void initQuiz()
        {
            if (NumberOfSong < 0)
            {
                switch (Level)
                {
                    case LevelEnum.EASY:
                        NumberOfSong = 4;
                        break;
                    case LevelEnum.MEDIUM:
                        NumberOfSong = 6;
                        break;
                    case LevelEnum.HARDCORE:
                        NumberOfSong = 8;
                        break;
                    default: 
                        NumberOfSong = 4;
                        break;
                }
            }
            quiz = new Quiz(NumberOfSong); // Pour le moment 4 chanson
        }

        /// <summary>
        /// Choisi parmis les categories envoyé une categorie
        /// </summary>
        /// <returns></returns>
        public String randomCategory()
        {
            Random rnd = new Random();
            if (ChooseCategoryList.Count == 0)
            {
                return "All";
            }
            else
            {
                return ChooseCategoryList[rnd.Next(ChooseCategoryList.Count)];
            }
        }

        /// <summary>
        /// Choisi un mode de difficulte parmis celle envoye
        /// </summary>
        public void randomLevel()
        {
            Random rnd = new Random();
            if (ChooseLevelList.Count == 0)
            {
                Level = LevelEnum.EASY;
            }
            else
            {
                String choose = ChooseLevelList[rnd.Next(ChooseLevelList.Count)];
                if (choose.Equals("Easy"))
                {
                    Level = LevelEnum.EASY;
                }
                else if (choose.Equals("Medium"))
                {
                    Level = LevelEnum.MEDIUM;
                }
                else
                {
                    Level = LevelEnum.HARDCORE;
                }
            }
        }

        #endregion
    }
}
