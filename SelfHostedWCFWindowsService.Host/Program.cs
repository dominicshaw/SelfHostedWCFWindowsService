using System;
using System.ServiceModel;
using System.ServiceProcess;
using log4net;
using SelfHostedWCFWindowsService.Host.Service;
using SelfHostedWCFWindowsService.Service;
using SelfHostedWCFWindowsService.Service.Workers;

namespace SelfHostedWCFWindowsService.Host
{
    class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "/service")
            {
                ServiceBase.Run(new WindowsService());
            }
            else
            {
                _log.Info("Starting Demo Service...");

                using (var host = new ServiceHost(typeof(Demo)))
                {
                    host.Open();

                    _log.Info("The service is ready.");
                    _log.Info("Press <Enter> to stop the service.");

                    _log.Info(Scheduler.Instance.Jobs.Count + " jobs found."); // to trigger the startup

                    Console.ReadLine();

                    // Close the ServiceHost.
                    host.Close();
                }
            }
        }
    }
}
