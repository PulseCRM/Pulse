using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using PulseLeads.Zillow.Controllers;
using PulseLeads.Zillow.Models;
using PulseLeads.Zillow._Code;
using PostLeadRequest = PulseLeads.Zillow.Models.PostLeadRequest;

namespace PulseLeads.Zillow
{
    public partial class ContactAPI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Read XML posted via HTTP
                Page.Response.ContentType = "text/xml";
                StreamReader reader = new StreamReader(Page.Request.InputStream);
                String xml_data = reader.ReadToEnd();

                // Post
                string response_text = string.Empty;
                Page.Response.StatusCode = PostLead(xml_data, out response_text)
                    ? 200
                    : 500;
                Page.Response.Write(response_text);
                Page.Response.Flush();
            }
            catch (Exception ex)
            {
                Page.Response.StatusCode = 500;
                Page.Response.Write(ex.Message);
                Page.Response.Flush();
                NLogger.Error(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ex);
            }
        }

        private bool PostLead(string xml_data, out string response_text)
        {
            bool ret_status = false;

            try
            {
                DataSet ds = new DataSet();
                ZillowAttributeCollection zcol = new ZillowAttributeCollection();

                // Parse the xml and populate the ZillowAttributeCollection
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml_data)))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        // copy data into collection
                        ds.ReadXml(reader, XmlReadMode.Auto);
                        zcol = ImportManager.ImportLeadFromZillowv5(ds);

                        // re-read the xml
                        stream.Seek(0, SeekOrigin.Begin);
                        zcol.ZillowXml = reader.ReadToEnd();
                    }
                }

                // Read security token
                zcol.PulseSecurityToken = Helpers.ReadSecurityToken();

                // Get Pulse Lead Request object from ZillowAttributeCollection
                PostLeadRequest pulse_lead_req = PulseLeadClient.GetPulseLeadRequest(zcol);

                // post request 
                ResponseHdr response_hdr = PulseLeadClient.PostPulseLeadRequest(pulse_lead_req, zcol.ImportTransId.ToString());
                if (response_hdr.Successful)
                {
                    zcol.ReturnStatus = "Success";
                    ret_status = true;
                    zcol.BorrowerId = response_hdr._postArgs.ContactId;
                    zcol.CoborrowerId = response_hdr._postArgs.CoBorrowerContactId;
                    zcol.LoanId = response_hdr._postArgs.LoanId;
                }
                else
                {
                    zcol.ReturnStatus = "Failed.";
                }
                zcol.Save();
                response_text = zcol.ReturnStatus;

            }
            catch (Exception ex)
            {
                response_text = "Application Error<br/>" + ex.Message;
                NLogger.Error(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ex);
            }

            return ret_status;
        }

    }
}