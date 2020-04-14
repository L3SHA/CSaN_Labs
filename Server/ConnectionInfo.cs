using System;
using System.Net.Sockets;
using Common;

namespace Server
{
    class ConnectionInfo
    {
        public ConnectionInfo(Socket client, int id)
        {
            Client = client;
            ID = id;
        }
        public Socket Client { private set; get; }
        public int ID { private set; get; }
    }
}

