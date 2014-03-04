using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using PostLeadWeb.LP2Service;
using focusIT;


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
            string Url = string.Empty;
            
            string sqlCmd = "Select top 1 BackgroundWCFURL from Company_Web";
            try
            {
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                Url = obj == null || obj == DBNull.Value ? string.Empty : (string)obj;
            }
            catch (Exception ex)
            {
                return Url;
            }

            return Url;
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
