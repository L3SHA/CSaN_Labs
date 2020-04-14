using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var port = 8005;
            var server = new ServerService(port);
            server.StartServer();
        }
    }
}
