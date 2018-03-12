using System;
using System.ServiceModel;
using log4net;
using SelfHostedWCFWindowsService.Contracts;
using SelfHostedWCFWindowsService.Service.Helpers;
using SelfHostedWCFWindowsService.Service.Workers;
using ServiceModelEx;

namespace SelfHostedWCFWindowsService.Service
{
    [SecurityBehavior(ServiceSecurity.None)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    [DuplexErrorHandlerBehavior]
    public class Demo : IDemo, IDisposable
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Demo));

        private readonly Guid _serviceID = Guid.NewGuid();
        
        private string _username;

        public Demo()
        {
            _log.Debug($"New service instance ({_serviceID})");

            var callback = OperationContext.Current.GetCallbackChannel<IDemoCallback>();

            Scheduler.Instance.Callbacks.AddOrUpdate(_serviceID, callback, (id, cb) => callback);
        }

        public string Login(string username, string ipAddress, string version)
        {
            _log.Info("Logged in user: " + username);
            _username = username;
            return _username;
        }

        public void Logout()
        {
            Scheduler.Instance.Callbacks.TryRemove(_serviceID, out _);
            _log.Info("Logged out user: " + _username);
        }

        public IAsyncResult BeginLogin(string username, string ipAddress, string version, AsyncCallback callback, object asyncState)
        {
            return new CompletedAsyncResult<string>(Login(username, ipAddress, version));
        }

        public string EndLogin(IAsyncResult r)
        {
            return ((CompletedAsyncResult<string>) r).Data;
        }

        public void Dispose()
        {
            Scheduler.Instance.Callbacks.TryRemove(_serviceID, out _);
        }
    }
}
