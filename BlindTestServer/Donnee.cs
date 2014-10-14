using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace BlindTestServer
{
    class Donnee
    {
        private List<String> userList;
        private String xmlUrl = "https://itunes.apple.com/us/rss/topsongs/limit=100/xml";
        private Quiz quiz;
        private int numberUserReady = 0;
        private List<Socket> listSock;

        private int minJoueur;

        public Donnee()
        {
            userList = new List<string>();
        }

	    public int MinJoueur
	    {
		    get { return minJoueur;}
		    set { minJoueur = value;}
	    }

        public void addSocket(Socket client)
        {

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
    }
}
