using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public struct UserInfo
    {
        public int ID;
        public string Name;
    }

    public enum MessageTypes
    {
        RegRequest,
        RegResponse,
        ToAllMsg,
        PrivateMsg,
        UserJoinOrLeft,
        SearchRequest,
        SearchResponse
    }
   // [Serializable]
    public class Message
    {

        public bool isJoin;
        public int id;
        public int sourceID, destID;
        public string name;
        public string message;
        public string ipAddress;
        public int port;
        public MessageTypes messageType; 
        public List<UserInfo> users;


        public Message()
        {

        }

        public Message(MessageTypes messageType)
        {
            this.messageType = messageType;
        }

        public Message(MessageTypes messageType, string name)
        {
            this.messageType = messageType;
            this.name = name;
        }

        public Message(MessageTypes messageType, int id, List<UserInfo> users, string messages)
        {
            this.messageType = messageType;
            this.id = id;
            this.users = users;
            message = messages;
        }

        public Message(MessageTypes messageType, int id, string message)
        {
            this.messageType = messageType;
            this.id = id;
            this.message = message;
        }

        public Message(MessageTypes messageType, int sourceID, int destID, string message)
        {
            this.messageType = messageType;
            this.sourceID = sourceID;
            this.destID = destID;
            this.message = message;
        }

        public Message(MessageTypes messageType, int id, string name, string message, bool isJoin)
        {
            this.messageType = messageType;
            this.id = id;
            this.name = name;
            this.message = message;
            this.isJoin = isJoin;
        }

        public Message(MessageTypes messageType, string ipAddress, int port)
        {
            this.messageType = messageType;
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public Message(string message)
        {
            this.message = message;
        }
    }
}
