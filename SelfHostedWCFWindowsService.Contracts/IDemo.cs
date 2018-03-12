using System;
using System.ServiceModel;

namespace SelfHostedWCFWindowsService.Contracts
{
    [ServiceContract(Namespace = "http://SelfHostedWCFWindowsService.Contracts", CallbackContract = typeof(IDemoCallback))]
    public interface IDemo
    {
        [OperationContract]
        string Login(string username, string ipAddress, string version);

        [OperationContract]
        void Logout();
        
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginLogin(string username, string ipAddress, string version, AsyncCallback callback, object asyncState);

        string EndLogin(IAsyncResult r);
    }
}
