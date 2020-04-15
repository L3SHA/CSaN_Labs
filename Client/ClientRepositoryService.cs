using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Client
{
    class ClientRepositoryService : IClientRepositoryService
    {
        private static ClientRepositoryService clientRepositoryService;

        private IClientRepository clientRepository;

        private ClientRepositoryService()
        {
            
        }

        public static ClientRepositoryService GetInstance()
        {
            if(clientRepositoryService == null)
            {
                clientRepositoryService = new ClientRepositoryService();
            }
            return clientRepositoryService;
        }

        public void AddUser(int id, string name)
        {
            clientRepository = ClientRepository.GetInstance();
            clientRepository.AddUserToList(id, name);
        }

        public void DeleteUser(int id)
        {
            clientRepository.DeleteUserFromList(id);
        }

        public int GetClientID()
        {
            return clientRepository.GetClientID();
        }

        public string GetMessagesText(int id)
        {
            List<Message> messageList = clientRepository.GetMessageList(id);
            var messagesText = new StringBuilder();
            foreach(Message message in messageList)
            {
                messagesText.Append(message.message);
            }
            return messagesText.ToString();
        }

        public List<string> GetUsersList()
        {
            var usersList = new List<string>();
            Dictionary<int, string> users = clientRepository.GetUsers();
            foreach(int id in users.Keys)
            {
                usersList.Add(id + " " + users[id]);
            }
            return usersList;
        }

        public void SaveMessage(Message message, int id)
        {            
            clientRepository.SaveMessage(message, id);
        }

        public void SetClientID(int id)
        {
            clientRepository.SaveClientID(id);
        }
    }
}
