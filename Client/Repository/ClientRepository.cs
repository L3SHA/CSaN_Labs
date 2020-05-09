using System.Collections.Generic;
using Common;
using System;
using System.Net.Sockets;
using System.Net;

namespace Client
{
    class ClientRepository : IClientRepository
    {
        private static ClientRepository clientRepository;
        public delegate void HandleDataUpdate(DataUpdateEvents.Events dataUpdateEvents);
        public event HandleDataUpdate UpdateUI;

        private ClientRepository()
        {
            conversations = new Dictionary<int, List<Message>>();
            users = new Dictionary<int, string>();
        }

        public static ClientRepository GetInstance()
        {
            if(clientRepository == null)
            {
                clientRepository = new ClientRepository();
            }
            return clientRepository;
        }

        public void ClearRepository()
        {
            id = 0;
            client = null;
            name = "";
            conversations.Clear();
            users.Clear();
        }

        private int id;
        private Socket client;
        private IPEndPoint ipEndPoint;
        private string name;
        private Dictionary<int, List<Message>> conversations;
        private Dictionary<int, string> users;

        public void AddUserToList(int id, string name)
        {
            users.Add(id, name);
            UpdateUI?.Invoke(DataUpdateEvents.Events.UsersListUpdate);
        }

        public void DeleteUserFromList(int id)
        {
            users.Remove(id);
            UpdateUI?.Invoke(DataUpdateEvents.Events.UsersListUpdate);
        }

        public int GetClientID()
        {
            return id;
        }

        public List<Message> GetMessageList(int id)
        {
            return conversations[id];
        }

        public void SaveClientID(int id)
        {
            this.id = id;
        }

        public void SaveMessage(Message message, int id)
        {
            conversations[id].Add(message);
            UpdateUI?.Invoke(DataUpdateEvents.Events.MessagesUpdate);
        }

        public Dictionary<int, string> GetUsers()
        {
            return users;
        }

        public void SubscribeUIUpdate(HandleDataUpdate handleDataUpdate)
        {
            UpdateUI += handleDataUpdate;
        }

        public void SaveClientSocket(Socket client)
        {
            if (client == null)
            {
                UpdateUI?.Invoke(DataUpdateEvents.Events.ServerConnectionUpdate);
            }
            else 
            {
                this.client = client;
            }
        }

        public Socket GetClientSocket()
        {
            return client;
        }

        public void SaveClientName(string name)
        {
            this.name = name;
        }

        public string GetClientName()
        {
            return name;
        }

        public void SaveEndPointAddress(IPEndPoint ipEndPoint)
        {
            this.ipEndPoint = ipEndPoint;
            UpdateUI?.Invoke(DataUpdateEvents.Events.ServerInfoUpdate);
        }

        public IPEndPoint GetEndPointAddress()
        {
            return ipEndPoint;
        }

        public void AddConversation(int id)
        {
            conversations.Add(id, new List<Message>());
        }

        public void DeleteConversation(int id)
        {
            conversations.Remove(id);
        }
    }
}
