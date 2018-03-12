using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceModel;
using log4net;
using SelfHostedWCFWindowsService.Proxy;

namespace SelfHostedWCFWindowsService.Client
{
    class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        static void Main()
        {
            var callback = new DemoCallback();
            var context = new InstanceContext(callback);
            var proxy = new TwoWay(context);

            callback.ReceiveHeartbeat += Callback_ReceiveHeartbeat;

            var user = proxy.Login(Environment.UserName, GetIPAddress(), Assembly.GetExecutingAssembly().GetName().Version.ToString());
            
            _log.Info("Logged In: " + user);

            _log.Info("Press any key to close.");
            Console.Read();

            callback.ReceiveHeartbeat -= Callback_ReceiveHeartbeat;

            proxy.Logout();
            proxy.Close();
        }

        private static void Callback_ReceiveHeartbeat(object sender, EventArgs e)
        {
            _log.Info("Heartbeat");
        }

        private static string GetIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var localIP = string.Empty;

            foreach (var ip in host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork))
            {
                localIP = ip.ToString();
                break;
            }

            return localIP;
        }
    }
}