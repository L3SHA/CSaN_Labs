using System.Text;
using Newtonsoft.Json;
using System.IO;
using System;

namespace Common
{
    public class MessageSerializer : IMessageSerializer
    {
        private static MessageSerializer messageSerializer;

        private MessageSerializer()
        {

        }

        public static MessageSerializer GetInstance()
        {
            if(messageSerializer == null)
            {
                messageSerializer = new MessageSerializer();
            }
            return messageSerializer;
        }

        public byte[] Serialize(Message message)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        }

        public Message Deserialize(byte[] data)
        {
            return JsonConvert.DeserializeObject<Message>(Encoding.UTF8.GetString(data));
        }
    }
}
