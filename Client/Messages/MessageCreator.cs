using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Client
{
    public static class MessageCreator
    {
        private static ClientRepositoryService clientRepositoryService = ClientRepositoryService.GetInstance();
        public static Message CreateMessageToAll(string messageText, List<int> files)
        {
            return new Message(MessageTypes.ToAllMsg, -1, "[Public Message]" + DateTime.Now + " From " + clientRepositoryService.GetClientName() + ": " + messageText, files);    
        }

        public static Message CreatePrivateMessage(string messageText, List<int> files, int destID)
        {
            return new Message(MessageTypes.PrivateMsg, clientRepositoryService.GetClientID(), destID, "[Private Message]" + DateTime.Now + " From " + clientRepositoryService.GetClientName() + ": " + messageText, files);
        }

        public static Message CreateServiceMessage(MessageTypes messageType)//regrequest, searchrequest
        {
            switch(messageType)
            {
                case MessageTypes.RegRequest:
                    return new Message(messageType, clientRepositoryService.GetClientName());
                case MessageTypes.UserJoinOrLeft:
                    return new Message(messageType, clientRepositoryService.GetClientID(), clientRepositoryService.GetClientName(), clientRepositoryService.GetClientName() + " left the chat", false);
                case MessageTypes.SearchRequest:
                    return new Message();
                default:
                    return new Message();
            }
        }
    }
}
