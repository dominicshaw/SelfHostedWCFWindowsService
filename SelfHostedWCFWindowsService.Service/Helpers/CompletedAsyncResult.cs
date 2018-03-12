using System;
using System.Threading;

namespace SelfHostedWCFWindowsService.Service.Helpers
{
    public class CompletedAsyncResult<T> : IAsyncResult
    {
        public CompletedAsyncResult(T data)
        {
            Data = data;
        }

        public T Data { get; }
        public object AsyncState => Data;
        public WaitHandle AsyncWaitHandle => throw new Exception("The method or operation is not implemented.");
        public bool CompletedSynchronously => true;
        public bool IsCompleted => true;
    }
}