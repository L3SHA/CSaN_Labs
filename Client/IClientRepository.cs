using System;
using System.Collections.Generic;
using Common;

namespace Client
{
    interface IClientRepository
    {
        void SaveMessage(Message message, int id);

        void SaveClientID(int id);

        void AddUserToList(int id, string name);

        void DeleteUserFromList(int id);

        int GetClientID();

        List<Message> GetMessageList(int id);

        Dictionary<int, string> GetUsers();
    }
}
