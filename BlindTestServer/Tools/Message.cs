using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using BlindTestServer.Model;

namespace BlindTestServer.Tools
{
    class Message
    {
        private Donnee donnee;
        private byte[] reponseByServer = new byte[32767];

        public Message(Donnee donnee)
        {
            this.donnee = donnee;
        }

        #region Funtion
        /// <summary>
        /// Broadcast to one client
        /// Envoi "broadcast;message;"
        /// </summary>
        /// <param name="message"></param>
        public void BroadcastOne(String message, Socket sock)
        {
            StringBuilder broadcast = new StringBuilder("broadcast;");
            broadcast.Append(message);
            broadcast.Append(";\n");
            reponseByServer = ASCIIEncoding.ASCII.GetBytes(broadcast.ToString());
            sock.Send(reponseByServer);
        }

        /// <summary>
        /// Broadcast to all client except the one in param
        /// Envoi "broadcast;message;"
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sock"></param>
        public void BroadcastExceptOne(String message, Socket sock)
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

        /// <summary>
        /// Broadcast to all client
        /// Envoi "broadcast;message;"
        /// </summary>
        /// <param name="message"></param>
        public void BroadcastToAll(String message)
        {
            StringBuilder broadcast = new StringBuilder("broadcast;");
            broadcast.Append(message);
            broadcast.Append(";\n");
            foreach (Socket s in donnee.SockList)
            {
                reponseByServer = ASCIIEncoding.ASCII.GetBytes(broadcast.ToString());
                s.Send(reponseByServer);
            }
        }

        /// <summary>
        /// Pour envoyer une commade a tout les clients
        /// Exemple : "score;10;" "chatR;David;Salut sava !;"
        /// </summary>
        /// <param name="message"></param>
        public void SendMessageToAll(String message)
        {
            String m = message + "\n";
            foreach (Socket s in donnee.SockList)
            {
                reponseByServer = ASCIIEncoding.ASCII.GetBytes(m);
                s.Send(reponseByServer);
                Console.WriteLine(message);
            }
        }

        /// <summary>
        /// Pour envoyer une commade a un client
        /// Exemple : "score;10;" "chatR;David;Salut sava !;"
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sock"></param>
        public void SendMessage(String message, Socket sock)
        {
            String m = message + "\n";
            reponseByServer = ASCIIEncoding.ASCII.GetBytes(m);
            sock.Send(reponseByServer);
        }

        /// <summary>
        /// Pour envoyer une commande excepter le client en param
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sock"></param>
        public void SendMessageExceptOne(String message, Socket sock)
        {
            String m = message + "\n";
            foreach (Socket s in donnee.SockList)
            {
                if (s != sock)
                {
                    reponseByServer = ASCIIEncoding.ASCII.GetBytes(m);
                    s.Send(reponseByServer);
                }
            }
        }
        #endregion

    }
}
