using System.Collections.Generic;
using Common;
using System;

namespace Client
{
    class ClientRepository : IClientRepository
    {
        private static ClientRepository clientRepository;
        private delegate void HandleDataUpdate(DataUpdateEvents.Events dataUpdateEvents);
        private event HandleDataUpdate UpdateUI;

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

        private int id;

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
            if (conversations.ContainsKey(id))
            {
                conversations[id].Add(message);
            }
            else
            {
                conversations.Add(id, new List<Message>());
            }
            UpdateUI?.Invoke(DataUpdateEvents.Events.MessagesUpdate);
        }

        public Dictionary<int, string> GetUsers()
        {
            return users;
        }

        void SubscribeUIUpdate(HandleDataUpdate handleDataUpdate)
        {
            UpdateUI += handleDataUpdate;
        }
    }
}
