using System.Text;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Xml.Serialization;

namespace Common
{
    public class MessageSerializer : ISerializer
    {
        public byte[] Serialize(Message message)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Message));
            MemoryStream messageContainer = new MemoryStream();
            serializer.Serialize(messageContainer, message);
            return messageContainer.GetBuffer();
        }

        public Message Deserialize(byte[] data, int amount)
        {
            MemoryStream messageContainer = new MemoryStream();
            messageContainer.Write(data, 0, amount);
            XmlSerializer serializer = new XmlSerializer(typeof(Message));
            messageContainer.Position = 0;
            Message message = (Message)serializer.Deserialize(messageContainer);
            return message;
        }
    }
}
