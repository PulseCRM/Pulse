using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using PulseLeads.Zillow.Controllers;
using PulseLeads.Zillow.Models;
using PulseLeads.Zillow._Code;
using Employment = PulseLeads.Zillow.Models.Employment;
using LiquidAssets = PulseLeads.Zillow.Models.LiquidAssets;
using OtherIncome = PulseLeads.Zillow.Models.OtherIncome;
using PostLeadRequest = PulseLeads.Zillow.Models.PostLeadRequest;

namespace PulseLeads.Zillow
{
    public partial class ContactAPITest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                ZillowAttributeCollection zcol = new ZillowAttributeCollection();

                // Parse the xml and populate the ZillowAttributeCollection
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(TextBoxZillowXml.Text)))
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
                    zcol.BorrowerId = response_hdr._postArgs.ContactId;
                    zcol.CoborrowerId = response_hdr._postArgs.CoBorrowerContactId;
                    zcol.LoanId = response_hdr._postArgs.LoanId;
                }
                else
                {
                    zcol.ReturnStatus = "Failed.";
                }
                zcol.Save();

                // display html formatted
                LiteralResults.Text = "<p>Response Status: " + zcol.ReturnStatus + "</p><p>" +  Helpers.FriendlyHtml(zcol) + "</p>";

            }
            catch (Exception ex)
            {
                LiteralResults.Text = "Application Error<br/>" + ex.Message;
            }
        }

        protected void ButtonParse_Click(object sender, EventArgs e)
        {
            try
            {
                // Parse the xml and populate the ZillowAttributeCollection
                StringReader tr = new StringReader(TextBoxZillowXml.Text);

                // move data into collection
                DataSet ds = new DataSet();
                ds.ReadXml(tr, XmlReadMode.Auto);
                ZillowAttributeCollection zcoll = ImportManager.ImportLeadFromZillowv5(ds);

                // display html formatted
                LiteralResults.Text = Helpers.FriendlyHtml(zcoll);

            }
            catch (Exception ex)
            {
                LiteralResults.Text = "Error: " + ex.Message;
            }
        }

        protected void LinkButtonLoadSample_Click(object sender, EventArgs e)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                string xml_file_path = Server.MapPath("~/Zillow/SupportDocuments/Sample1_V50.xml");

                if (File.Exists(xml_file_path))
                {
                    TextBoxZillowXml.Text = File.ReadAllText(xml_file_path);
                }
            }
            catch (Exception ex)
            {
                LiteralResults.Text = "Error: " + ex.Message;
            }
        }


    }
}