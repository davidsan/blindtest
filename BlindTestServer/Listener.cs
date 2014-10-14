using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BlindTestServer
{
    class Listener
    {
        #region Param
        Socket sock;
        Donnee donnee;
        byte[] reponseByServer = new byte[32767];
        byte[] rep = new Byte[32767];
        String[] reponseSplit;
        String username;
        #endregion

        #region Construcor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="donnee"></param>
        public Listener(Socket client, Donnee donnee)
        {
            this.sock = client;
            this.donnee = donnee;
        }
        #endregion

        #region Listenner
        /// <summary>
        /// Listen user command
        /// </summary>
        public void Listen()
        {
            while (sock.Connected != false)
            {
                int count = sock.Receive(rep, rep.Length, 0);
                string srep = Encoding.ASCII.GetString(rep);
                string reponse = srep.Substring(0, count);
                reponseSplit = reponse.Split(';');
                switch (reponseSplit[0])
                {
                    case "exit":
                        Console.WriteLine(username + " leave the server !!");
                        donnee.NumberUserReady--;
                        donnee.removeSocket(sock);
                        sock.Shutdown(SocketShutdown.Both);
                        sock.Close();
                        break;
                    case "connect":
                        string arg1 = reponseSplit[1];
                        if (connect(arg1))
                        {
                            sendMessage("This username is already in use : "
                                + arg1 + " !!\n");
                        }
                        else
                        {
                            username = arg1;
                            donnee.addUser(username);
                            sendMessage("Welcome " + username + " !!");
                            sendMessageExcept(username + " join the server !!", sock);
                            Console.WriteLine(username + " join the server !!");
                            donnee.incrUserReady();
                            if (donnee.NumberUserReady == donnee.MinJoueur)
                            {
                                sendMessage("A game will start soon !!");
                                donnee.startGame.Set();
                            }
                        }
                        break;
                    default:
                        Console.WriteLine(reponse);
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
        private bool connect(String name)
        {
            return donnee.exist(name);
        }

        /// <summary>
        /// Broadcast to all client
        /// </summary>
        /// <param name="message"></param>
        private void sendMessage(String message)
        {
            StringBuilder broadcast = new StringBuilder("broadcast;");
            broadcast.Append(message);
            broadcast.Append(";\n");
            reponseByServer = ASCIIEncoding.ASCII.GetBytes(broadcast.ToString());
            sock.Send(reponseByServer);
        }

        /// <summary>
        /// Broadcast to all client except the one in param
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sock"></param>
        private void sendMessageExcept(String message, Socket sock)
        {
            StringBuilder broadcast = new StringBuilder("broadcast;");
            broadcast.Append(message);
            broadcast.Append(";\n");
            foreach (Socket s in donnee.SockList) {
                if (s != sock)
                {
                    reponseByServer = ASCIIEncoding.ASCII.GetBytes(broadcast.ToString());
                    s.Send(reponseByServer);
                }
            }
        }
        #endregion

    }
}
