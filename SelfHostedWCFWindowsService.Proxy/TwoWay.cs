using System;
using System.ServiceModel;
using SelfHostedWCFWindowsService.Contracts;

namespace SelfHostedWCFWindowsService.Proxy
{
    public class TwoWay : DuplexClientBase<IDemo>, IDemo
    {
        public TwoWay(InstanceContext callbackInstance)
            : base(callbackInstance)
        {
            
        }
        public TwoWay(InstanceContext callbackInstance, string endpointConfigurationName)
            : base(callbackInstance, endpointConfigurationName)
        {
            
        }

        public string Login(string username, string ipAddress, string version)
        {
            return Channel.Login(username, ipAddress, version);
        }

        public void Logout()
        {
            Channel.Logout();
        }

        public IAsyncResult BeginLogin(string username, string ipAddress, string version, AsyncCallback callback, object asyncState)
        {
            return Channel.BeginLogin(username, ipAddress, version, callback, asyncState);
        }

        public string EndLogin(IAsyncResult r)
        {
            return Channel.EndLogin(r);
        }
    }
}
