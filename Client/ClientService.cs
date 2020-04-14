using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System;
using Common;
using System.IO;


namespace Client
{
    class ClientService
    {
        private string broadcastIP = "192.168.99.255";
        private Socket client, socketUdpHandler;
        private MessageSerializer messageSerializer;
        public int ServerPort { private set; get;}
        public string ServerIPAddress { private set; get;}
        public int ID { private set; get; }
        public Dictionary<int, string> Conversations { private set; get; }
        public Dictionary<int, string> Users { private set; get; }
        public delegate void HandleEvent();
        private event HandleEvent UpdateUI;
        private Thread thread;

        public ClientService()
        {
            messageSerializer = new MessageSerializer();
            Users = new Dictionary<int, string>();
            Conversations = new Dictionary<int, string>();
            Conversations.Add(-1, "");
            SetUdpEndPoint();
        }

        public void StartClient(IPAddress ipAddress, int port)
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(ipAddress, port);
            client.Connect(ipPoint); 
            thread = new Thread(RecieveMessage);
            thread.Start();
        }

        public void SendMessage(Message message)
        {  
            client.Send(messageSerializer.Serialize(message));
        }

        public void RecieveMessage()
        {
            
            while (client.Connected)
            {
                byte[] data = new byte[1024];
                int amount;
                MemoryStream messageContainer = new MemoryStream();
                do
                {
                    try
                    {
                        amount = client.Receive(data);
                        messageContainer.Write(data, 0, amount);
                    }
                    catch
                    {

                    }
                    
                } while (client.Available > 0);
                Message message = messageSerializer.Deserialize(messageContainer.GetBuffer(), messageContainer.GetBuffer().Length);
                HandleMessage(message);
            }
        }

        private void HandleMessage(Message message)
        {
            switch (message.messageType)
            {
                case MessageTypes.RegResponse:
                    ID = message.id;
                    Conversations[-1] = message.message + "\r\n";
                    foreach (UserInfo userInfo in message.users)
                    {
                        if (ID != userInfo.ID)
                        {
                            Users.Add(userInfo.ID, userInfo.Name);
                            Conversations.Add(userInfo.ID, "");
                        }
                    }
                    break;
                case MessageTypes.UserJoinOrLeft:
                    if(message.isJoin)
                    {
                        if (message.id != ID)
                        {
                            Conversations.Add(message.id, "");
                            Conversations[-1] += message.message + "\r\n";
                            Users.Add(message.id, message.name);
                        }
                    }
                    else 
                    {
                        Conversations[-1] += message.message + "\r\n";
                        Conversations.Remove(message.id);
                        Users.Remove(message.id);
                    }  
                    break;
                case MessageTypes.PrivateMsg:
                    Conversations[message.sourceID] += message.message + "\r\n";
                    break;
                case MessageTypes.ToAllMsg:
                    Conversations[message.id] += message.message + "\r\n";
                    break;
                case MessageTypes.SearchResponse:
                    ServerPort = message.port;
                    ServerIPAddress = message.ipAddress;
                    break;
            }
            UpdateUI?.Invoke();
        }

        public void Subscribe(HandleEvent handleEvent)
        {
            UpdateUI += handleEvent; 
        }

        public void CloseClient()
        {
            thread.Abort();
            thread.Join(500);
            Users.Clear();
            Conversations.Clear();
            client.Close();
        }

        public void SetUdpEndPoint()
        {
            socketUdpHandler = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socketUdpHandler.EnableBroadcast = true;
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 0);
            socketUdpHandler.Bind(localEndPoint);
        }

        public void ReceiveMessagesUdp()
        {
            byte[] data = new byte[1024];
            EndPoint endPoint = socketUdpHandler.LocalEndPoint;
            while (true)
            {
                int amount = socketUdpHandler.ReceiveFrom(data, ref endPoint);
                Message message = messageSerializer.Deserialize(data, amount);
                if (message.messageType == MessageTypes.SearchResponse)
                {
                    HandleMessage(message);
                    return;
                }
            }
        }

        public void UdpBroadcastRequest()
        {
            Message message = new Message(MessageTypes.SearchRequest);
            message.port = ((IPEndPoint)socketUdpHandler.LocalEndPoint).Port;
            message.ipAddress = NetNodeInfo.GetCurrentIP().ToString();
            IPEndPoint IPendPoint = new IPEndPoint(IPAddress.Parse(broadcastIP), 8005);
            Socket sendRequest = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sendRequest.SendTo(messageSerializer.Serialize(message), IPendPoint);
            Thread threadReceiveUdp = new Thread(ReceiveMessagesUdp);
            threadReceiveUdp.Start();
        }
    }
}
