using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

namespace BlindTestServer
{
    class Game
    {
        private Donnee donnee;
        private byte[] reponseByServer = new byte[32767];

        public Game(Donnee donnee)
        {
            this.donnee = donnee;
        }



        public void Run()
        {
            donnee.startGame.WaitOne();
            donnee.initRound();
            String song1 = donnee.Quiz.Songs.ElementAt(0).Title;
            String song2 = donnee.Quiz.Songs.ElementAt(1).Title;
            String song3 = donnee.Quiz.Songs.ElementAt(2).Title;
            String song4 = donnee.Quiz.Songs.ElementAt(3).Title;
            String message = "round;" 
                + song1 + ";" + song2 + ";" + song3 + ";" + song4 + ";" 
                + donnee.Quiz.CorrectSong.Link + ";\n";
            sendMessageToAll(message);
        }

        private void sendMessageToAll(String message)
        {
            foreach (Socket s in donnee.SockList)
            {
                reponseByServer = ASCIIEncoding.ASCII.GetBytes(message);
                s.Send(reponseByServer);
            }
        }
    }
}
