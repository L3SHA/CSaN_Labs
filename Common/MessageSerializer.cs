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
            return Encoding.Default.GetBytes(JsonConvert.SerializeObject(message));
        }

        public Message Deserialize(byte[] data, int size)
        {
            var temp = new byte[size];
            Array.Copy(data, 0, temp, 0, size);
            Console.WriteLine(Encoding.Default.GetString(temp));
            return JsonConvert.DeserializeObject<Message>(Encoding.Default.GetString(temp));
        }
    }
}
