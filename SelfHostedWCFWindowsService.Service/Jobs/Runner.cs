using System;
using System.Threading;
using log4net;

namespace SelfHostedWCFWindowsService.Service.Jobs
{
    public class Runner : IDisposable
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Runner));

        private readonly Job _job;
        private readonly Timer _timer;

        public Runner(Job job)
        {
            _job = job;
            _timer = new Timer(_ => Run(), null, job.RunEvery, Timeout.InfiniteTimeSpan);
        }

        private void Run()
        {
            try
            {
                _job.Delegate.Invoke();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            finally
            {
                _timer?.Change(_job.RunEvery, Timeout.InfiniteTimeSpan);
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}