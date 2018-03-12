using System;

namespace SelfHostedWCFWindowsService.Service.Jobs
{
    public class Job : IDisposable
    {
        private readonly Runner _runner;

        public Job(Guid id, string name, Action del, TimeSpan runEvery)
        {
            Id = id;
            Name = name;
            Delegate = del;
            RunEvery = runEvery;

            _runner = new Runner(this);
        }

        public Guid Id { get; }
        public string Name { get; }
        public Action Delegate { get; }
        public TimeSpan RunEvery { get; }

        public void Dispose()
        {
            _runner?.Dispose();
        }
    }
}
