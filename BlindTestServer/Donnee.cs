using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace BlindTestServer
{
    class Donnee
    {
        public Donnee()
        {
            userList = new List<string>();
            sockList = new List<Socket>();
        }

        public AutoResetEvent startGame = new AutoResetEvent(false);
        private List<String> userList;
        private String xmlUrl = "https://itunes.apple.com/us/rss/topsongs/limit=100/xml";
        private Quiz quiz;  
        private int numberUserReady = 0;
        private List<Socket> sockList;
        private int minJoueur = 2;
        private int currentRound = 0;

        public Quiz Quiz
        {
            get { return quiz; }
            private set { quiz = value; }
        }

        public int CurrentRound
        {
            get { return currentRound; }
            set { currentRound = value; }
        }
        
        public List<Socket> SockList
        {
            get { return sockList; }
            set { sockList = value; }
        }

	    public int MinJoueur
	    {
		    get { return minJoueur;}
		    set { minJoueur = value;}
	    }

        public void addSocket(Socket client)
        {
            this.sockList.Add(client);
        }

        public void removeSocket(Socket client)
        {
            this.sockList.Remove(client);
        }
        
        public int NumberUserReady
        {
            get { return numberUserReady ; }
            set { numberUserReady = value; }
        }
        
        public bool exist(String name)
        {
            return userList.Exists(x => x.Equals(name));
        }

        public void addUser(String name)
        {
            userList.Add(name);
        }

        public void removeUser(String name)
        {
            userList.Remove(name);
        }

        public String getXmlUrl()
        {
            return xmlUrl;
        }

        public void newRound()
        {
            quiz = new Quiz(4);
        }

        public void incrUserReady()
        {
            this.numberUserReady++;
        }

        /// <summary>
        /// Init le round
        /// </summary>
        public void initRound()
        {
            quiz = new Quiz(4); // Pour le moment 4 chanson
        }
    }
}
