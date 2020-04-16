using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IMessageSerializer
    {
        byte[] Serialize(Message message);
        Message Deserialize(byte[] buffer, int size);
    }
}
