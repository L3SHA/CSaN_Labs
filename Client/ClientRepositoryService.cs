using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Common;

namespace Client
{
    class ClientRepositoryService : IClientRepositoryService
    {
        private static ClientRepositoryService clientRepositoryService;

        private static IClientRepository clientRepository;

        private ClientRepositoryService()
        {
            
        }

        public static ClientRepositoryService GetInstance()
        {
            if(clientRepositoryService == null)
            {
                clientRepositoryService = new ClientRepositoryService();
            }
            clientRepository = ClientRepository.GetInstance();
            return clientRepositoryService;
        }

        public void ClearClientRepository()
        {
            clientRepository.ClearRepository();
        }

        public void AddUser(int id, string name)
        {
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

        public Socket GetClientSocket()
        {
            return clientRepository.GetClientSocket();
        }

        public string GetMessagesText(int id)
        {
            List<Message> messageList = clientRepository.GetMessageList(id);
            var messagesText = new StringBuilder();
            foreach(Message message in messageList)
            {
                messagesText.Append(message.message + "\r\n");
            }
            return messagesText.ToString();
        }

        public List<string> GetUsersList()
        {
            var usersList = new List<string>();
            Dictionary<int, string> users = clientRepository.GetUsers();
            foreach(int id in users.Keys)
            {
                usersList.Add("User id:" + id + " " + users[id]);
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

        public void SetClientSocket(Socket client)
        {
            clientRepository.SaveClientSocket(client);
        }

        public void SetClientName(string name)
        {
            clientRepository.SaveClientName(name);
        }

        public string GetClientName()
        {
            return clientRepository.GetClientName();
        }

        public void SetEndPointAddress(string ipAddress, string port)
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), int.Parse(port));
            clientRepository.SaveEndPointAddress(ipEndPoint);
        }

        public IPEndPoint GetEndPointAddress()
        {
            return clientRepository.GetEndPointAddress();
        }
    }
}
