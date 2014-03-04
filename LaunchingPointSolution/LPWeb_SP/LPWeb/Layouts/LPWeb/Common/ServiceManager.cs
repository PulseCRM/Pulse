using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using LPWeb.LP_Service;
using LPWeb.BLL;

namespace LPWeb.Layouts.LPWeb.Common
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
           
            Company_Web company_web = new Company_Web();
            return company_web.GetWcfUrl();
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
