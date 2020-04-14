using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Common
{
    public class NetNodeInfo
    {
        public static IPAddress GetCurrentIP()
        {
            IPAddress[] adresses = Dns.GetHostAddresses(Dns.GetHostName());
            IPAddress currentIPAdress = null;
            bool IsFound = false;
            foreach (var adress in adresses)
            {
                if (adress.GetAddressBytes().Length == 4 && !IsFound)
                {
                    currentIPAdress = adress;
                    IsFound = true;
                }
            }
            return currentIPAdress;
        }
    }
}
