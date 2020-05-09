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
    class ClientService : IClientService
    {
        private string broadcastIP = "192.168.99.255";
        private Socket socketUdpHandler;
        private IClientRepositoryService clientRepositoryService;
        private Thread thread;

        public ClientService()
        {
            clientRepositoryService = ClientRepositoryService.GetInstance();
            SetUdpEndPoint();
        }

        public void StartClient()
        {
            var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientRepositoryService.SetClientSocket(client);
            client.Connect(clientRepositoryService.GetEndPointAddress());
            MessageReciever messageReciever = new MessageReciever(client);
            thread = new Thread(messageReciever.RecieveMessage);
            thread.Start();
            MessageSender.SendMessage(MessageCreator.CreateServiceMessage(MessageTypes.RegRequest));
        }

        public void CloseClient()
        {
            thread.Abort();
            thread.Join(500);
            MessageSender.SendMessage(MessageCreator.CreateServiceMessage(MessageTypes.UserJoinOrLeft));
            clientRepositoryService.GetClientSocket().Close();
        }

        public void SetUdpEndPoint()
        {
            socketUdpHandler = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socketUdpHandler.EnableBroadcast = true;
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 0);
            socketUdpHandler.Bind(localEndPoint);
        }

        private void ReceiveMessagesUdp()
        {
            byte[] data = new byte[1024];
            EndPoint endPoint = socketUdpHandler.LocalEndPoint;
            while (true)
            {
                int amount = socketUdpHandler.ReceiveFrom(data, ref endPoint);
                Message message = MessageSerializer.GetInstance().Deserialize(data, amount);
                if (message.messageType == MessageTypes.SearchResponse)
                {
                    MessageHandler.HandleMessage(message);
                    return;
                }
            }
        }

        public void FindServerRequest()
        {
            Message message = new Message(MessageTypes.SearchRequest);
            message.port = ((IPEndPoint)socketUdpHandler.LocalEndPoint).Port;
            message.ipAddress = NetNodeInfo.GetCurrentIP().ToString();
            IPEndPoint IPendPoint = new IPEndPoint(IPAddress.Parse(broadcastIP), 8005);
            Socket sendRequest = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sendRequest.SendTo(MessageSerializer.GetInstance().Serialize(message), IPendPoint);
            Thread threadReceiveUdp = new Thread(ReceiveMessagesUdp);
            threadReceiveUdp.Start();
        }
    }
}
