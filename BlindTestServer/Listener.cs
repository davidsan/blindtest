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
                reponse = reponse.Trim();
                switch (reponseSplit[0])
                {
                    case "exit":
                        Console.WriteLine(username + " leave the server !!");
                        sock.Shutdown(SocketShutdown.Both);
                        sock.Close();
                        break;
                    case "connect":
                        string arg1 = reponseSplit[1].Trim();
                        if (connect(arg1))
                        {
                            sendMessage("This username is already in use : "
                                + arg1 + " !!\n");
                        }
                        else
                        {
                            username = arg1;
                            donnee.addUser(username);
                            sendMessage("Welcome " + username + " !!\n");
                            Console.WriteLine(username + " join the server !!");
                            sendMessage("xml;" + donnee.getXmlUrl() + "\n");
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
        /// Broadcast to client
        /// </summary>
        /// <param name="message"></param>
        private void sendMessage(String message)
        {
            reponseByServer = ASCIIEncoding.ASCII.GetBytes(message);
            sock.Send(reponseByServer);
        }
        #endregion

    }
}
