﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Blindtest.Service
{
    class NetworkManager
    {

        private static NetworkManager instance;
        private NetworkManager() { }
        public static NetworkManager Instance
        {
            get
            {
                if (instance == null) { instance = new NetworkManager(); }
                return instance;
            }
        }
        private Socket sock;
        public Socket Sock
        {
            get { return sock; }
            set { sock = value; }
        }

        private byte[] rep = new Byte[32767];
        public void InitSock(String address, int port)
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Blocking = true;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(address), port);
            sock.Connect(ipep);
        }
        public void Listen()
        {

            while (sock.Connected != false)
            {
                int count = sock.Receive(rep, rep.Length, 0);
                string srep = Encoding.ASCII.GetString(rep);
                string reponse = srep.Substring(0, count);
                String[] reponseSplit = reponse.Split(';');
                switch (reponseSplit[0])
                {
                    case "chatR":
                        string arg1 = reponseSplit[1];
                        Console.WriteLine(arg1);
                        break;
                    case "round":
                        string numRound = reponseSplit[1];
                        string song1 = reponseSplit[2];
                        string song2 = reponseSplit[3];
                        string song3 = reponseSplit[4];
                        string song4 = reponseSplit[5];
                        string songUrl = reponseSplit[6];
                        Console.WriteLine(numRound, song1, song2, song3, song4, songUrl);
                        break;
                    case "score":
                        string value = reponseSplit[2];
                        Console.WriteLine(value);
                        break;
                    case "broadcast":
                        string message = reponseSplit[2];
                        Console.WriteLine(message);
                        break;
                    default:
                        Console.WriteLine("Command error, please retry\n");
                        break;
                }
            }
            Thread.CurrentThread.Abort();
        }
    }
}