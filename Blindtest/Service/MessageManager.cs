using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blindtest.Service
{
    class MessageManager
    {
        private static NetworkManager nm = NetworkManager.Instance;

        /// <summary>
        /// Envoi d'une commande au serveur
        /// </summary>
        /// <param name="message"></param>
        public static void sendMessageToServer(String message)
        {
            String m = message + "\n";
            byte[] reponseByServer = ASCIIEncoding.ASCII.GetBytes(m);
            nm.Sock.Send(reponseByServer);
        }
    }
}
