using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace BlindTestServer
{
    class Server
    {
        static void Main(string[] args)
        {
            TcpListener serverSocket = new TcpListener(IPAddress.Any, 8888);
            Donnee donnee = new Donnee();
            serverSocket.Start();
            Console.WriteLine("Server Started !!");
            Console.WriteLine("Waiting for users !!");
            Game game = new Game(donnee);
            Thread thrGame = new Thread(new ThreadStart(game.Run));
            thrGame.Start();

            while (true)
            {
                Socket client = serverSocket.AcceptSocket();
                if (client.Connected)
                {
                    donnee.addSocket(client);
                    Listener listener = new Listener(client, donnee);
                    Thread thr = new Thread(new ThreadStart(listener.Listen));
                    thr.Start();
                }
            }

        }
    }
}
