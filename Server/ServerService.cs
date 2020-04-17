using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Common;
using System.IO;

namespace Server
{
    class ServerService
    {
        public ServerService(int port)
        {
            this.port = port;
            id = 0;
            Clients = new Dictionary<int, string>();
            Conversations = new Dictionary<int, string>();
            Connections = new Dictionary<int, Socket>();
            Conversations.Add(-1, "");
            messageSerializer = MessageSerializer.GetInstance();
        }
        private Socket server, client;
        private const int MaxConnectionAmount = 10;
        private int id;
        private MessageSerializer messageSerializer;
        private int port;

        public Dictionary<int, string> Clients { private set; get; }
        public Dictionary<int, Socket> Connections { private set; get; }
        public Dictionary<int, string> Conversations { private set; get; }

        public void StartServer()
        {
            IPEndPoint endPoint = new IPEndPoint(NetNodeInfo.GetCurrentIP(), port);
            Console.WriteLine(endPoint);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(endPoint);
            server.Listen(MaxConnectionAmount);
            Thread handleUDP = new Thread(ListenUDPBroadcast);
            handleUDP.Start();
            while (true)
            {
                client = server.Accept();
                Connections.Add(id, client);
                var connectionInfo = new ConnectionInfo(client, id++);
                Thread thread = new Thread(new ParameterizedThreadStart(RecieveMessage));
                thread.Start(connectionInfo);
            }
        }

        public void RecieveMessage(object connectionInfo)
        {
            ConnectionInfo clientInfo = (ConnectionInfo)connectionInfo;
            while (clientInfo.Client.Connected)
            {
                byte[] data = new byte[1024];
                int amount;
                MemoryStream messageContainer = new MemoryStream();                    
                do
                {
                    try
                    {
                        amount = clientInfo.Client.Receive(data);
                        messageContainer.Write(data, 0, amount);
                    }
                    catch
                    {
                        Console.WriteLine("Connection lost");
                    }
                } while (clientInfo.Client.Available > 0);
                if (messageContainer.GetBuffer().Length > 0)
                {
                    Message message = messageSerializer.Deserialize(messageContainer.GetBuffer(), messageContainer.GetBuffer().Length);
                    HandleMessage(message, clientInfo);
                }
            }
        }

        public void HandleMessage(Message message, ConnectionInfo clientInfo)
        {
            switch (message.messageType)
            {
                case MessageTypes.RegRequest:
                    RegisterClient(clientInfo.ID, message.name);
                    var users = new List<UserInfo>();
                    UserInfo ui;
                    foreach (int id in Clients.Keys)
                    {
                        ui.ID = id;
                        ui.Name = Clients[id];
                        users.Add(ui);
                    }
                    SendMessage(new Message(MessageTypes.RegResponse, clientInfo.ID, users, Conversations[-1]));
                    SendMessage(new Message(MessageTypes.UserJoinOrLeft, clientInfo.ID,  Clients[clientInfo.ID], Clients[clientInfo.ID] + " join chat", true));
                    break;
                case MessageTypes.ToAllMsg:
                    Conversations[-1] += message.message;
                    SendMessage(message);
                    break;
                case MessageTypes.PrivateMsg:
                    SendMessage(message);
                    break;
                case MessageTypes.UserJoinOrLeft:
                    DeleteClient(message.id);
                    SendMessage(message);
                    break;
            }
        }

        public void SendMessage(Message message)
        {
            switch (message.messageType)
            {
                case MessageTypes.RegResponse:
                case MessageTypes.PrivateMsg:
                    SendPrivateMessage(message);
                    break;
                case MessageTypes.UserJoinOrLeft:
                    SendMessageJoinLeft(message);
                    break;
                case MessageTypes.ToAllMsg:
                    SendMessageToAll(message);
                    break;
            }
        }

        public void SendMessageJoinLeft(Message message)
        {
            foreach (int id in Connections.Keys)
            {
                if (message.id != id)
                {
                    Connections[id].Send(messageSerializer.Serialize(message));
                }
            }
        }

        public void SendPrivateMessage(Message message)
        {
            if (message.messageType == MessageTypes.PrivateMsg)
            {
                Connections[message.destID].Send(messageSerializer.Serialize(message));
            }
            else
            {
                Connections[message.id].Send(messageSerializer.Serialize(message));
            }
        }

        public void SendMessageToAll(Message message)
        {
            foreach (int id in Connections.Keys)
            {
                Connections[id].Send(messageSerializer.Serialize(message));
            }
        }

        private void RegisterClient(int id, string name)
        {
            Clients.Add(id, name);
            Conversations.Add(id, "");
        }

        private void DeleteClient(int id)
        {
            Clients.Remove(id);
            Conversations.Remove(id);
            Connections.Remove(id);
        }

        private void ListenUDPBroadcast()
        {
            Socket socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socketListener.EnableBroadcast = true;
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
            socketListener.Bind(localEndPoint);
            byte[] data = new byte[1024];
            EndPoint endPoint = localEndPoint;
            while (true)
            {
                int amount = socketListener.ReceiveFrom(data, ref endPoint);
                Message message = messageSerializer.Deserialize(data, amount);
                if (message.messageType == MessageTypes.SearchRequest)
                    HandleSearchMessage(message);
            }
        }

        private void HandleSearchMessage(Message message)
        {
            Message messageResponse = new Message(MessageTypes.SearchResponse, NetNodeInfo.GetCurrentIP().ToString(), port);
            Socket socketSetAdress = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(message.ipAddress), message.port);
            Console.WriteLine(messageResponse.ipAddress);
            socketSetAdress.SendTo(messageSerializer.Serialize(messageResponse), endPoint);
        }

    }
}
