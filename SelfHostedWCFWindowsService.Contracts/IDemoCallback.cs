using System.ServiceModel;

namespace SelfHostedWCFWindowsService.Contracts
{
    [ServiceContract]
    public interface IDemoCallback
    {
        [OperationContract]
        void Heartbeat();
    }
}