using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
namespace MaterialDesign2.Classes
{
    public class InternetConnect
    {
        public bool Connected;

        public InternetConnect()
        {
            Connected = false;
        }
        public  bool IsNetworkAvailable()
        {
            return IsNetworkAvailable(0);
        }

        public static bool IsNetworkAvailable(long minimumSpeed)
        {
            try
            {
                System.Net.Sockets.TcpClient client =
                    new System.Net.Sockets.TcpClient("www.google.com", 80);
                client.Close();
                return true;
            }
            catch 
            {
                return false;
            }
          
        }
    }
}
