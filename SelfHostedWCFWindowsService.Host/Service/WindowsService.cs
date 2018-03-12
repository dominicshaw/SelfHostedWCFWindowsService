using System.ServiceModel;
using System.ServiceProcess;
using SelfHostedWCFWindowsService.Service;

namespace SelfHostedWCFWindowsService.Host.Service
{
    partial class WindowsService : ServiceBase
    {
        private ServiceHost _complyHost;

        public WindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _complyHost?.Close();

            _complyHost = new ServiceHost(typeof(Demo));

            _complyHost.Open();
        }

        protected override void OnStop()
        {
            if (_complyHost != null)
            {
                _complyHost.Close();
                _complyHost = null;
            }
        }
    }
}
