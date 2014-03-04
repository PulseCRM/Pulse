using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using Common;
using PulseLeads.LP2Service;

namespace FocusIT.Pulse
{
    /// <summary>
    /// The Service Manager
    /// </summary>
    public partial class ServiceManager
    {

        string WcfUrl;
        BasicHttpBinding WcfBinding;
        EndpointAddress WcfEndPointAddress;
        LP2ServiceClient WcfServiceClient;
        /// <summary>
        /// Get the WCF URL.
        /// </summary>
        /// <returns></returns>
        private string GetWCFServiceURL()
        {
            string err = string.Empty;
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
            Table.CompanyWeb setting;
            try
            {
                setting = dataAccess.GetEmailServerSetting(out err);
                if (setting != null)
                {
                    return setting.BackgroundWCFURL;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return string.Empty;
        }
        /// <summary>
        /// Service Manager
        /// </summary>

        /// <returns></returns>
        /// 
        public ServiceManager()
        {
            try
            {
                WcfUrl = GetWCFServiceURL();
                if ((WcfUrl == null) || (WcfUrl == String.Empty))
                    throw new Exception("No Pulse WCF Service URL configured in the database.");
                if (WcfBinding == null)
                    WcfBinding = new BasicHttpBinding();
                int bufSize = WcfBinding.MaxBufferSize;
                bufSize = 650000000;
                WcfBinding.MaxBufferPoolSize = bufSize;
                WcfBinding.MaxBufferSize = bufSize;
                WcfBinding.MaxReceivedMessageSize = bufSize;
                WcfBinding.ReaderQuotas.MaxArrayLength = bufSize;
                if (WcfEndPointAddress == null)
                    WcfEndPointAddress = new EndpointAddress(WcfUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Start the Service Client.
        /// </summary>

        /// <returns></returns>
        /// 
        public LP2ServiceClient StartServiceClient()
        {
            try
            {
                if (WcfServiceClient == null)
                {
                    WcfServiceClient = new LP2ServiceClient(WcfBinding, WcfEndPointAddress);
                }
                return WcfServiceClient;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
