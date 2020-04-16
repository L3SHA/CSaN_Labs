using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using Common;

namespace Client
{
    class MessageReciever
    {
        private MessageSerializer messageSerializer;
        private Message message;
        private Socket socket;

        public MessageReciever(Socket socket)
        {
            this.socket = socket;
            messageSerializer = MessageSerializer.GetInstance();
        }

        public void RecieveMessage()
        {
            while (socket.Connected)
            {
                byte[] data = new byte[1024];
                int amount;
                MemoryStream messageContainer = new MemoryStream();
                do
                {
                    try
                    {
                        amount = socket.Receive(data);
                        messageContainer.Write(data, 0, amount);
                    }
                    catch
                    {
                        //add some exception to handle it on higher level
                    }
                } while (socket.Available > 0);
                message = messageSerializer.Deserialize(messageContainer.GetBuffer());
                MessageHandler.HandleMessage(message);
            }
        }
    }
}
