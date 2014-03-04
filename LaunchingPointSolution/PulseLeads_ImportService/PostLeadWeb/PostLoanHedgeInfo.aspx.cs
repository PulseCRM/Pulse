using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Net;

namespace PulseLeads
{
    public partial class PostLoanHedgeInfo : System.Web.UI.Page
    {
        private static readonly DataAccess.DataAccess _DataAccess = new DataAccess.DataAccess();
        private string MCT_ClientID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            bool status = false;
            #region for test
            if (Request.QueryString["test"] != null && Request.QueryString["test"].ToString() == "1")
            {
                return;
            }
            if (Request.InputStream.Length == 0)
            {
                WriteStatus(status, "No XML data.");
                return;
            }
            #endregion

            string err = string.Empty;
            try
            {
                var streamReader = new StreamReader(Request.InputStream);

                XDocument xmlDoc = XDocument.Load(streamReader);
                ProcessRequest(xmlDoc);
            }
            catch (Exception exception)
            {
                err = string.Format("Parse request xml error. Exception: {0}", exception.ToString());
                WriteStatus(status, err);
            }
            finally
            { }

        }
        private bool CheckDecimal(string input, out decimal output)
        {
            bool success = false;
            output = 0;

            success = decimal.TryParse(input, out output);
            return success;
        }

        private bool UpdatePoint(int fileId, out string err)
        {
            bool success = false;
            err = string.Empty;
            FocusIT.Pulse.ServiceManager sm = new FocusIT.Pulse.ServiceManager();
            using (PostLeadWeb.LP2Service.LP2ServiceClient client = sm.StartServiceClient())
            {
                PostLeadWeb.LP2Service.UpdateLockInfoRequest req = new PostLeadWeb.LP2Service.UpdateLockInfoRequest();
                req.hdr = new PostLeadWeb.LP2Service.ReqHdr();
                req.FileId = fileId;
                PostLeadWeb.LP2Service.UpdateLockInfoResponse resp = client.UpdateLoanProfitability(req);
                if (resp != null && resp.hdr != null)
                {
                    if (!resp.hdr.Successful)
                    {
                        err = resp.hdr.StatusInfo;
                    }

                    success = resp.hdr.Successful;
                }
            }
            return success;
        }
        private string GetElementValue(XElement el, string eleName)
        {
            string value = string.Empty;
            if (el == null || string.IsNullOrEmpty(eleName))
                return value;
            try
            {
                string tempValue = el.Elements(eleName).FirstOrDefault().Value;
                if (string.IsNullOrEmpty(tempValue))
                    return value;

                tempValue = System.Web.HttpUtility.UrlDecode(tempValue);
                value = tempValue.Trim();
            }
            catch (Exception Ex)
            { }
            return value;
        }
 
        private void ProcessRequest(XDocument xmlDoc)
        {
            bool status = false;
            string err = string.Empty;
            try
            {
                var mctPost = xmlDoc.Elements("client");
                MCT_ClientID = mctPost.Elements("name").FirstOrDefault().Value;
                if (string.IsNullOrEmpty(MCT_ClientID))
                {
                    err = "The name element is missing in the XML request.";
                    WriteStatus(status, err);
                    return;
                }
                string clientID = _DataAccess.GetMCT_ClientID();
                if (string.IsNullOrEmpty(clientID))
                {
                    err = "The ClientID is not configured in Company MCT settings.";
                    WriteStatus(status, err);
                    return;
                }
                if (clientID.ToUpper().Trim() != MCT_ClientID.ToUpper().Trim())
                {
                    err = "The name element in the XML request does not match Pulse ClientID configruation.";
                    WriteStatus(status, err);
                    return;
                }
                Common.Record.LoanProfit loanProfit = new Common.Record.LoanProfit();
                int loanIdx = 0; int iFileId = 0;
                decimal tempDecimal;
                int tempInt = 0;
                DateTime tempExtDate = DateTime.MinValue;
                DateTime tempDate = DateTime.MinValue;
                string errMsg = string.Empty;

                foreach (var loan in mctPost.Elements("loan"))
                {
                    loanIdx++;
                    if (loan == null)
                        continue;
                    #region process Loan Number & FileID
                    string lid = GetElementValue(loan, "id");
                    if (string.IsNullOrEmpty(lid))
                    {
                        err += string.Format("loan at index: {0} does not contain an id element which is used for the Loan Number.", loanIdx);
                        continue;
                    }
                    string sFileId = _DataAccess.GetFileIdByLoanNumber(lid);
                    if (string.IsNullOrEmpty(sFileId))
                    {
                        err += string.Format("Unable to find the loan number, {0} specified in the id element within loan at index {1}. \r\n", lid, loanIdx);
                        continue;
                    }

                    if (!int.TryParse(sFileId, out iFileId))
                    {
                        err += string.Format("Invalid FileID {0} returned for the loan number, {1} specified in the id element within loan at index {2}. \r\n", sFileId, lid, loanIdx);
                    }
                    #endregion
                    //get the elements from the XML
                    string netsell = GetElementValue(loan, "netsell");
                    string investor = GetElementValue(loan, "investor");
                    string srp = GetElementValue(loan, "srp");
                    string llpa = GetElementValue(loan, "llpas");
                    string hedge_cost = GetElementValue(loan, "hedge_cost");
                    string mandatory_final_price = GetElementValue(loan, "mandatory_final_price");
                    string commitment_number = GetElementValue(loan, "commitment_number");
                    string commitment_exp_date = GetElementValue(loan, "commitment_exp_date");
                    string commitment_date = GetElementValue(loan, "commitment_date");
                    loanProfit.FileId = iFileId;

                    #region Check if fields are valid and assign them to loanProfit if valid
                    if (!string.IsNullOrEmpty(netsell))
                    {
                        if (CheckDecimal(netsell, out tempDecimal))
                            loanProfit.NetSell = netsell;
                        else
                            err += string.Format("loan at index {0}, Invalid netsell {1} not decimal.\r\n", loanIdx, netsell);
                    }

                    if (!string.IsNullOrEmpty(srp))
                    {
                        if (CheckDecimal(srp, out tempDecimal))
                            loanProfit.SRP = srp;
                        else
                            err += string.Format("loan at index {0}, Invalid srp {1} not decimal.\r\n", loanIdx, srp);
                    }

                    if (!string.IsNullOrEmpty(llpa))
                    {
                        if (CheckDecimal(llpa, out tempDecimal))
                            loanProfit.LLPA = llpa;
                        else
                            err += string.Format("loan at index {0}, Invalid llpa {1} not decimal.\r\n", loanIdx, llpa);
                    }

                    if (!string.IsNullOrEmpty(hedge_cost))
                    {
                        if (CheckDecimal(hedge_cost, out tempDecimal))
                        {
                            tempDecimal = -(tempDecimal);
                            loanProfit.HedgeCost = tempDecimal.ToString();
                        }
                        else
                            err += string.Format("loan at index {0}, Invalid hedge_cost {1} not decimal.\r\n", loanIdx, hedge_cost);
                    }

                    if (!string.IsNullOrEmpty(mandatory_final_price))
                    {
                        if (CheckDecimal(mandatory_final_price, out tempDecimal))
                            loanProfit.MandatoryFinalPrice = mandatory_final_price;
                        else
                            err += string.Format("loan at index {0}, Invalid mandatory_final_price {1} not decimal.\r\n", loanIdx, mandatory_final_price);
                    }

                    if (!string.IsNullOrEmpty(commitment_exp_date))
                    {
                        if (DateTime.TryParse(commitment_exp_date, out tempExtDate))
                            loanProfit.CommitmentExpDate = commitment_exp_date;
                        else
                            err += string.Format("loan at index {0}, Invalid commitment_exp_date {1} not a date.\r\n", loanIdx, commitment_exp_date);
                    }

                    if (!string.IsNullOrEmpty(commitment_date))
                    {
                        if (DateTime.TryParse(commitment_date, out tempDate))
                            loanProfit.CommitmentDate = commitment_date;
                        else
                            err += string.Format("loan at index {0}, Invalid commitment_date {1} not a date.\r\n", loanIdx, commitment_date);
                    }

                    if (tempExtDate != DateTime.MinValue && tempDate != DateTime.MinValue)
                    {
                        tempInt = (tempExtDate - tempDate).Days;
                        loanProfit.CommitmentTerm = tempInt.ToString();
                    }
                    loanProfit.Investor = investor;
                    loanProfit.CommitmentNumber = commitment_number;
                    #endregion

                    try
                    {
                        if (_DataAccess.SaveLoanProfit(loanProfit, out errMsg) == false)
                        {
                            if (!string.IsNullOrEmpty(errMsg))
                            {
                                err += string.Format("Unable to process loan at index {0}, Error: {1}.\r\n", loanIdx, errMsg);
                            }
                            continue;
                        }
                        if (UpdatePoint(iFileId, out errMsg) == false)
                        {
                            err += string.Format("Unable to Update Point File for loan at index {0}, Error: {1}.\r\n", loanIdx, errMsg);
                            loanProfit.Error = errMsg;
                            _DataAccess.SaveLoanProfit(loanProfit, out errMsg);
                        }
                    }
                    catch (Exception ex1)
                    {
                        err += string.Format("Unable to process loan at index {0}, Exception: {1}.\r\n", loanIdx, ex1.ToString());
                    }
                }
                if (string.IsNullOrEmpty(err))
                    status = true;

                WriteStatus(status, err);
                return;
            }
            catch (Exception ex)
            {
                err = string.Format("Error processing XML request. Exception: {0}", ex.ToString());
                WriteStatus(status, err);
                return;
            }
        }

        private void WriteStatus(bool success, string ErrorMsg)
        {
            StringBuilder sb = new StringBuilder();
            Utf8StringWriter stringWriter = new Utf8StringWriter(sb);
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("client");

            xmlWriter.WriteElementString("name", MCT_ClientID);
            xmlWriter.WriteElementString("id", "Pulse");
            xmlWriter.WriteElementString("success", success.ToString());
            xmlWriter.WriteElementString("errorMsg", ErrorMsg);

            xmlWriter.WriteEndElement();  // end client
            xmlWriter.WriteEndDocument();

            Response.ContentType = "text/xml";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(sb.ToString());
        }
        #region for test

        protected void btnTest_Click(object sender, EventArgs e)
        {
            string url = Request.Url.AbsoluteUri.Replace("?test=1", "");
            //Response.Write(Request.Url.AbsoluteUri);
            //Response.Write("<br />");
            //Response.Write(url);

            string data = xmldata.Value.Replace("\r\n", "").Replace("\t", "");
            //string token = txbToken.Text;
            // url = url + "?SecurityToken=" + token;
            //url = @"http://api.launchingpoint.com/PostLoanHedgeInfo.aspx";
            txtareResult.Value = Post(url, data);
        }

        public static string Post(string url, string data)
        {
            string vystup = null;
            try
            {
                //Our postvars
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                //Initialisation, we use localhost, change if appliable
                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(url);
                //Our method is post, otherwise the buffer (postvars) would be useless
                WebReq.Method = "POST";
                //We use form contentType, for the postvars.
                WebReq.ContentType = "text/xml";
                //The length of the buffer (postvars) is used as contentlength.
                WebReq.ContentLength = buffer.Length;
                //We open a stream for writing the postvars
                Stream PostData = WebReq.GetRequestStream();
                //Now we write, and afterwards, we close. Closing is always important!
                PostData.Write(buffer, 0, buffer.Length);
                PostData.Close();
                //Get the response handle, we have no true response yet!
                HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
                //Let's show some information about the response
                Console.WriteLine(WebResp.StatusCode);
                Console.WriteLine(WebResp.Server);

                //Now, we read the response (the string), and output it.
                Stream Answer = WebResp.GetResponseStream();
                StreamReader _Answer = new StreamReader(Answer);
                vystup = _Answer.ReadToEnd();
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
            return vystup.Trim();
        }
        #endregion
    }
}