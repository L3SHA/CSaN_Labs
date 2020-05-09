using Common;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Client
{
    interface IClientRepository
    {
        void ClearRepository();

        void SaveMessage(Message message, int id);

        void SaveClientID(int id);

        void AddUserToList(int id, string name);

        void DeleteUserFromList(int id);

        int GetClientID();

        List<Message> GetMessageList(int id);

        Dictionary<int, string> GetUsers();

        void SaveClientSocket(Socket client);

        Socket GetClientSocket();

        void SaveClientName(string name);

        string GetClientName();

        void SaveEndPointAddress(IPEndPoint ipEndPoint);

        IPEndPoint GetEndPointAddress();

        void AddConversation(int id);

        void DeleteConversation(int id);
    }
}
