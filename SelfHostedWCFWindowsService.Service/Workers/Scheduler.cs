using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using log4net;
using SelfHostedWCFWindowsService.Contracts;
using SelfHostedWCFWindowsService.Service.Jobs;

namespace SelfHostedWCFWindowsService.Service.Workers
{
    public sealed class Scheduler
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Scheduler));

        private static volatile Scheduler _instance;
        private static readonly object _syncRoot = new object();

        public ConcurrentDictionary<Guid, IDemoCallback> Callbacks { get; } = new ConcurrentDictionary<Guid, IDemoCallback>();

        public List<Job> Jobs { get; } = new List<Job>();

        private Scheduler()
        {
            Jobs.Add(new Job(Guid.NewGuid(), "Running", Running, TimeSpan.FromSeconds(20)));
            Jobs.Add(new Job(Guid.NewGuid(), "Heartbeat", Heartbeat, TimeSpan.FromSeconds(10)));
        }

        private static void Running()
        {
            try
            {
                _log.Info("I am running.");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        private void Heartbeat()
        {
            try
            {
                var callbacks = Callbacks.Values.ToArray();
                
                _log.Debug($"Calling {callbacks.Length} heartbeats...");

                foreach (var cb in callbacks)
                {
                    try
                    {
                        cb.Heartbeat();
                    }
                    catch (Exception ex)
                    {
                        _log.Warn(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        public static Scheduler Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new Scheduler();
                    }
                }

                return _instance;
            }
        }
    }
}
