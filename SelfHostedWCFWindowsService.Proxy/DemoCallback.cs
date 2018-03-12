using System;
using SelfHostedWCFWindowsService.Contracts;

namespace SelfHostedWCFWindowsService.Proxy
{
    public class DemoCallback : IDemoCallback
    {
        public event EventHandler ReceiveHeartbeat;
        
        public void Heartbeat()
        {
            ReceiveHeartbeat?.Invoke(this, null);
        }
    }
}