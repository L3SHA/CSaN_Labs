using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using Common;

namespace Client
{
    interface IClientRepositoryService
    {
        void ClearClientRepository(); 

        void SaveMessage(Message message, int id);

        string GetMessagesText(int id);

        void SetClientID(int id);

        int GetClientID();

        List<string> GetUsersList();

        void AddUser(int id, string name);

        void DeleteUser(int id);

        void SetClientSocket(Socket client);

        Socket GetClientSocket();

        void SetClientName(string name);

        string GetClientName();

        void SetEndPointAddress(string ipAddress, string port);

        IPEndPoint GetEndPointAddress();

        void AddConversation(int id);

        void DeleteConversation(int id);
    }
}
