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
        }
        #endregion

        #region Params
        public AutoResetEvent startGame = new AutoResetEvent(false);
        public AutoResetEvent roundOver = new AutoResetEvent(false);
        private List<String> userList;
        private List<Socket> sockList;
        private List<Listener> userControlList;
        private List<Listener> userWhoFindList;
        private List<String> chooseCategoryList;
        private int numberUserReady = 0;
        private int minJoueur = 2;
        private int currentRound = 0;
        private int maxRound = 2;
        private int userAnswer = 0;
        private Quiz quiz;
        private String xmlUrl = "https://itunes.apple.com/us/rss/topsongs/limit=100/xml";
        
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

        public Quiz Quiz
        {
            get { return quiz; }
            private set { quiz = value; }
        }
        
        public String XmlUrl
        {
            get { return xmlUrl; }
            private set { xmlUrl = value; }
        }
        #endregion //Params

        #region Functions
   
        /// <summary>
        /// Init le round
        /// </summary>
        public void initQuiz()
        {
            quiz = new Quiz(4); // Pour le moment 4 chanson
        }

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

        #endregion
    }
}
