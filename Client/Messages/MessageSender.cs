using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Client
{
    public static class MessageSender
    {
        public static void SendMessage(Message message)
        {
            ClientRepositoryService.GetInstance().GetClientSocket().Send(MessageSerializer.GetInstance().Serialize(message));
        }
    }
}
