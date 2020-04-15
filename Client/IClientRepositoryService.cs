using System.Collections.Generic;
using Common;

namespace Client
{
    interface IClientRepositoryService
    {
        void SaveMessage(Message message, int id);

        string GetMessagesText(int id);

        void SetClientID(int id);

        int GetClientID();

        List<string> GetUsersList();

        void AddUser(int id, string name);

        void DeleteUser(int id);
    }
}
