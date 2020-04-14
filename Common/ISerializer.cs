using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    interface ISerializer
    {
        byte[] Serialize(Message message);
        Message Deserialize(byte[] buffer, int amount);
    }
}
