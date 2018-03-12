using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using log4net;
using ServiceModelEx;

namespace SelfHostedWCFWindowsService.Service.Helpers
{
    public class DuplexErrorHandlerBehaviorAttribute : Attribute, IServiceBehavior, IErrorHandler
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DuplexErrorHandlerBehaviorAttribute));

        private Type _serviceType;

        #region IErrorHandler Members

        bool IErrorHandler.HandleError(Exception error)
        {
            try
            {
                _log.Warn($"Error handler has found an error: {error.Message}");

                //var sbError = new StringBuilder();

                //sbError.AppendLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");

                //sbError.AppendFormat("Error occurred @ {1:dd MMM yyyy HH:mm:ss}{0}{0}", Environment.NewLine, DateTime.Now);

                //sbError.AppendFormat("Message:{0}{1}{0}", Environment.NewLine, error.Message);

                //sbError.AppendFormat("Type: {1}{0}", Environment.NewLine, error.GetType().ToString());
                //sbError.AppendFormat("Stack Trace: {0}{1}{0}", Environment.NewLine, error.StackTrace);
                //sbError.AppendFormat("Source: {0} ({1})", error.Source, Environment.MachineName);

                //var innerException = error.InnerException;
                //while (innerException != null)
                //{
                //    sbError.AppendLine();
                //    sbError.AppendFormat("-Message:{0}-{1}{0}", Environment.NewLine, error.Message);

                //    sbError.AppendFormat("-Type: {1}{0}", Environment.NewLine, error.GetType().ToString());
                //    sbError.AppendFormat("-Stack Trace: {0}{1}{0}", Environment.NewLine, error.StackTrace);
                //    sbError.AppendFormat("-Source: {1} ({2}){0}", Environment.NewLine, error.Source, Environment.MachineName);
                //    innerException = innerException.InnerException;
                //    sbError.AppendLine();
                //}

                //sbError.AppendLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                //sbError.AppendLine();
                //sbError.AppendLine();

                //Log.Warn(sbError.ToString());
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            // false means the other HandleErrors on other ErrorHandlers for the ChannelDispatcher will fire. True to stop subsequent. 
            // pg. 280 "Programming WCF Services"
            return false;
        }

        void IErrorHandler.ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            try
            {
                ErrorHandlerHelper.PromoteException(_serviceType, error, version, ref fault);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        #endregion

        #region IServiceBehavior Members

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription descr, ServiceHostBase host)
        {
            try
            {
                _serviceType = descr.ServiceType;
                foreach (ChannelDispatcher dispatcher in host.ChannelDispatchers)
                    dispatcher.ErrorHandlers.Add(this);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase host, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        { }

        void IServiceBehavior.Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        { }

        #endregion
    }
}
