using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Globalization;
using PostLeadWeb.LP2Service;
using FocusIT.Pulse;
using System.Configuration;

namespace PulseLeads
{
    public partial class LT_PostLeads : System.Web.UI.Page
    {
        //private const string  EventLogName = "PulseLeadService-LT_PostLeads";
        protected void Page_Load(object sender, EventArgs e)
        {
            #region for test
            if (Request.QueryString["test"] != null && Request.QueryString["test"].ToString() == "1")
            {
                return;
            }

            #endregion
            Response.Clear();

            if (Request.InputStream.Length == 0)
            {
                Log("No XML data.", 0);
                WriteStatus(0, "No XML data.");
                return;
            }
            string token = string.Empty;
            try
            {
                string sqlCmd = "Select top 1 GlobalID from Company_General";
                object obj = focusIT.DbHelperSQL.GetSingle(sqlCmd);
                token = (obj == null || obj == DBNull.Value) ? string.Empty : (string)obj;
            }
            catch (Exception ex)
            { }
            if (string.IsNullOrEmpty(token))
            {
                token = System.Configuration.ConfigurationManager.AppSettings["SecurityToken"] == null ? ""
                    : System.Configuration.ConfigurationManager.AppSettings["SecurityToken"].ToString();

                if (string.IsNullOrEmpty(token))
                {
                    Log("SecurityToken is Empty!", 0);
                    WriteStatus(0, "SecurityToken is Empty!");
                    return;
                }
            }
            PostLeadRequest req_lead = new PostLeadRequest();

            req_lead.RequestHeader = new PostLeadWeb.LP2Service.Lead_ReqHdr();
            req_lead.RequestHeader.SecurityToken = token;

            string err = string.Empty;
            bool checkDuplicates = false;
            #region mapping  field
            try
            {

                var streamReader = new StreamReader(Request.InputStream);
                //string str = streamReader.ReadToEnd();
                XDocument xmlDoc = XDocument.Load(streamReader);
                var leadApp = xmlDoc.Elements("LeadInformation").Elements("LeadApplication");

                var applicantID = leadApp.Elements("ApplicantID").FirstOrDefault().Value;

                req_lead.LeadId = applicantID;
                if (ConfigurationManager.AppSettings["CheckDuplicates"] != null)
                    Boolean.TryParse(ConfigurationManager.AppSettings["CheckDuplicates"].ToString(), out checkDuplicates);
                if (!string.IsNullOrEmpty(req_lead.LeadId))
                {
                    req_lead.CheckDuplicate = checkDuplicates;
                }
                PostLeadWeb.LP2Service.Address address = new PostLeadWeb.LP2Service.Address();
                address.City = leadApp.Elements("City").FirstOrDefault().Value;
                address.State = leadApp.Elements("State").FirstOrDefault().Value;
                address.Zip = leadApp.Elements("Zip").FirstOrDefault().Value;
                address.Street = leadApp.Elements("Address1").FirstOrDefault().Value;
                req_lead.MailingAddress = address;

                req_lead.Email = leadApp.Elements("EmailAddress").FirstOrDefault().Value;
                req_lead.HomePhone = leadApp.Elements("HomePhone").FirstOrDefault().Value;
                req_lead.BusinessPhone = leadApp.Elements("WorkPhone").FirstOrDefault().Value;
                req_lead.BorrowerFirstName = leadApp.Elements("FirstName").FirstOrDefault().Value;
                req_lead.BorrowerLastName = leadApp.Elements("LastName").FirstOrDefault().Value;
                req_lead.BorrowerMiddleName = "";
                req_lead.SSN = leadApp.Elements("SSN").FirstOrDefault().Value;
                req_lead.PreferredContactMethod = PreferredContactMethod.Email;
                //var oIncomeList = new List<OtherIncome>();
                //if (!string.IsNullOrEmpty(leadApp.Elements("MonthlyIncome").FirstOrDefault().Value))
                //{
                //    OtherIncome oIncome = new OtherIncome();
                //    oIncome.Amount = Convert.ToDecimal(leadApp.Elements("MonthlyIncome").FirstOrDefault().Value);
                //    oIncomeList.Add(oIncome);
                //}
                //req_lead.OtherIncome = oIncomeList.ToArray();

                // into  loannotes 


                #region CreditRating
                switch (leadApp.Elements("CreditRating").FirstOrDefault().Value.ToLower())
                {
                    case "good":
                        req_lead.CreditRanking = PostLeadWeb.LP2Service.CreditRanking.Good;
                        break;
                    case "excellent":
                        req_lead.CreditRanking = PostLeadWeb.LP2Service.CreditRanking.Excellent;
                        break;
                    case "fair":
                        req_lead.CreditRanking = PostLeadWeb.LP2Service.CreditRanking.Fair;
                        break;
                    case "poor":
                        req_lead.CreditRanking = PostLeadWeb.LP2Service.CreditRanking.Poor;
                        break;
                    case "verygood":
                        req_lead.CreditRanking = PostLeadWeb.LP2Service.CreditRanking.VeryGood;
                        break;
                    default: req_lead.CreditRanking = PostLeadWeb.LP2Service.CreditRanking.Good;
                        break;
                }
                #endregion



                //IsMilitary

                //TimeToContact




                //WorkingWithRealtor

                #region PropertyType
                switch (leadApp.Elements("PropertyType").FirstOrDefault().Value.ToLower())
                {
                    case "single-family":
                        req_lead.PropertyType = "SFR";
                        break;
                    case "town home":
                        req_lead.PropertyType = "Townhome";
                        break;

                    case "condominium":
                        req_lead.PropertyType = "Condo";
                        break;
                    case "Cooperative":
                        req_lead.PropertyType = "Other";
                        break;
                    case "multiple-family":
                        req_lead.PropertyType = "TwotoFourUnit";
                        break;
                    case "mobile home":
                        req_lead.PropertyType = "Other";
                        break;
                    default: req_lead.PropertyType = "Other";
                        break;
                }
                #endregion

                switch (leadApp.Elements("PropertyUse").FirstOrDefault().Value.ToLower())
                {
                    case "primary residence":
                        req_lead.OccupancyType = PostLeadWeb.LP2Service.OccupancyType.PrimaryResidence;
                        break;
                    case "vacation property ":
                        req_lead.OccupancyType = PostLeadWeb.LP2Service.OccupancyType.SecondHome;
                        break;
                    case "investment property":
                        req_lead.OccupancyType = PostLeadWeb.LP2Service.OccupancyType.InvestmentProperty;
                        break;
                }

                //PropertyCounty

                req_lead.Property_Zip = leadApp.Elements("PropertyZip").FirstOrDefault().Value;

                //PropertyMSA

                req_lead.Property_State = leadApp.Elements("PropertyState").FirstOrDefault().Value;

                //LoanType
                if (!string.IsNullOrEmpty(leadApp.Elements("LoansToBeFinanced").FirstOrDefault().Value))
                    req_lead.Notes += "Loans to be financed=" + leadApp.Elements("LoansToBeFinanced").FirstOrDefault().Value;

                //DownPayment
                req_lead.LoanAmount = string.IsNullOrEmpty(leadApp.Elements("LoanAmount").FirstOrDefault().Value) ? 0 : Convert.ToInt32(leadApp.Elements("LoanAmount").FirstOrDefault().Value);
                req_lead.PropertyValue = string.IsNullOrEmpty(leadApp.Elements("PropertyPrice").FirstOrDefault().Value) ? 0 : Convert.ToInt32(leadApp.Elements("PropertyPrice").FirstOrDefault().Value);

                //AddlCashOut
                if (!string.IsNullOrEmpty(leadApp.Elements("AddlCashOut").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n AdditionalCashout=" + leadApp.Elements("AddlCashOut").FirstOrDefault().Value;

                //MortgageType
                if (!string.IsNullOrEmpty(leadApp.Elements("MortgageType").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n MortgageType=" + leadApp.Elements("MortgageType").FirstOrDefault().Value;

                //TimeLine
                if (!string.IsNullOrEmpty(leadApp.Elements("TimeLine").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n TimeLine=" + leadApp.Elements("TimeLine").FirstOrDefault().Value;

                //<AnnualIncome/>
                if (!string.IsNullOrEmpty(leadApp.Elements("AnnualIncome").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n AnnualIncome=" + leadApp.Elements("AnnualIncome").FirstOrDefault().Value;

                //MonthlyPayment
                //req_lead.MonthlyPayment = string.IsNullOrEmpty(leadApp.Elements("MonthlyPayment").FirstOrDefault().Value) ? 0 : Convert.ToDecimal(leadApp.Elements("MonthlyPayment").FirstOrDefault().Value);

                //FirstMortgageMonthlyPayment = leadApp.Elements("FirstMortgageBalance").FirstOrDefault().Value;

                //FirstMortgageBalance
                if (!string.IsNullOrEmpty(leadApp.Elements("FirstMortgageBalance").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n FirstMortgageBalance=" + leadApp.Elements("FirstMortgageBalance").FirstOrDefault().Value;

                //SecondMortgageBalance
                if (!string.IsNullOrEmpty(leadApp.Elements("SecondMortgageBalance").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n SecondMortgageBalance=" + leadApp.Elements("SecondMortgageBalance").FirstOrDefault().Value;

                //AddlMortgagePayment
                if (!string.IsNullOrEmpty(leadApp.Elements("AddlMortgagePayment").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n AddlMortgagePayment=" + leadApp.Elements("AddlMortgagePayment").FirstOrDefault().Value;

                //MonthlyObligations
                if (!string.IsNullOrEmpty(leadApp.Elements("MonthlyObligations").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n MonthlyObligations=" + leadApp.Elements("MonthlyObligations").FirstOrDefault().Value;

                //Bankruptcy
                if (!string.IsNullOrEmpty(leadApp.Elements("Bankruptcy").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n Bankruptcy=" + leadApp.Elements("Bankruptcy").FirstOrDefault().Value;


                #region LoanRequested
                switch (leadApp.Elements("LoanRequested").FirstOrDefault().Value.ToLower())
                {
                    case "purchase":
                        req_lead.PurposeOfLoan = PostLeadWeb.LP2Service.PurposeOfLoan.Purchase;
                        break;
                    case "refinance":
                        if (string.IsNullOrEmpty(leadApp.Elements("AddlCashOut").FirstOrDefault().Value))
                        {
                            req_lead.PurposeOfLoan = PostLeadWeb.LP2Service.PurposeOfLoan.Refinance_Cashout;
                        }
                        else
                        {
                            req_lead.PurposeOfLoan = PostLeadWeb.LP2Service.PurposeOfLoan.Refinance_No_Cashout;
                        }

                        break;
                }
                #endregion

                //req_lead.LoanProgram = leadApp.Elements("LoanProgram").FirstOrDefault().Value;

                //MonthlyIncome-->LoanNotes
                if (!string.IsNullOrEmpty(leadApp.Elements("MonthlyIncome").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n MonthlyIncome=" + leadApp.Elements("MonthlyIncome").FirstOrDefault().Value;

                //AnnualIncome-->LoanNotes
                if (!string.IsNullOrEmpty(leadApp.Elements("AnnualIncome").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n AnnualIncome=" + leadApp.Elements("AnnualIncome").FirstOrDefault().Value;

                //IsMilitary-->LoanNotes
                if (!string.IsNullOrEmpty(leadApp.Elements("IsMilitary").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n IsMilitary=" + leadApp.Elements("IsMilitary").FirstOrDefault().Value;

                //TimeToContact-->LoanNotes
                if (!string.IsNullOrEmpty(leadApp.Elements("IsMilitary").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n TimeToContact=" + leadApp.Elements("IsMilitary").FirstOrDefault().Value;

                //WorkingWithRealtor-->LoanNotes
                if (!string.IsNullOrEmpty(leadApp.Elements("WorkingWithRealtor").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n WorkingWithRealtor=" + leadApp.Elements("WorkingWithRealtor").FirstOrDefault().Value;

                //PropertyMSA-->LoanNotes
                if (!string.IsNullOrEmpty(leadApp.Elements("PropertyMSA").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n PropertyMSA=" + leadApp.Elements("PropertyMSA").FirstOrDefault().Value;

                //PropertyCounty-->Loans.County 
                if (!string.IsNullOrEmpty(leadApp.Elements("PropertyCounty").FirstOrDefault().Value))
                    req_lead.County = leadApp.Elements("PropertyCounty").FirstOrDefault().Value;

                //LoanType-->Loans.LoanType 
                if (!string.IsNullOrEmpty(leadApp.Elements("LoanType").FirstOrDefault().Value))
                    req_lead.LoanType = leadApp.Elements("LoanType").FirstOrDefault().Value;

                //MortgageTerm-->Loans.Term
                int tempInt = 0;
                if (int.TryParse(leadApp.Elements("MortgageTerm").FirstOrDefault().Value, out tempInt))
                    req_lead.Term = tempInt;

                //CurrentLoanRate-->Loans.Rate 
                decimal tempDecimal = 0;
                if (decimal.TryParse(leadApp.Elements("CurrentLoanRate").FirstOrDefault().Value, out tempDecimal))
                    req_lead.Rate = tempDecimal;

                //PartnerUID

                //NameOfPartner-->LoanNote
                if (!string.IsNullOrEmpty(leadApp.Elements("NameOfPartner").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n NameOfPartner=" + leadApp.Elements("NameOfPartner").FirstOrDefault().Value;

                //TrackingNumber

                //DTI-->LoanNotes
                if (!string.IsNullOrEmpty(leadApp.Elements("DTI").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n DTI=" + leadApp.Elements("DTI").FirstOrDefault().Value;

                //LTV-->Loans.LTV
                if (!string.IsNullOrEmpty(leadApp.Elements("LTV").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n LTV=" + leadApp.Elements("LTV").FirstOrDefault().Value;

                //ApplicationDate
                //SourceID
                //FilterClass
                //FilterName
                //IsVerified
                //VerifiedHomePhone

                //PromotionalProgram-->LoanNotes
                if (!string.IsNullOrEmpty(leadApp.Elements("PromotionalProgram").FirstOrDefault().Value))
                    req_lead.Notes += "\r\n PromotionalProgram=" + leadApp.Elements("PromotionalProgram").FirstOrDefault().Value;

                string leadSource = ConfigurationManager.AppSettings["LeadSource"] == null ? string.Empty : ConfigurationManager.AppSettings["LeadSource"].ToString();
                req_lead.LeadSource = string.IsNullOrEmpty(leadSource) ? "Lending Tree" : leadSource;
                //OtherLoanProgram 
                //MatchFee
                //FilterRoutingID

            #endregion

                ServiceManager sm = new ServiceManager();
                using (PostLeadWeb.LP2Service.LP2ServiceClient client = sm.StartServiceClient())
                {
                    err = string.Empty;
                    RespHdr resp = client.PostLead(req_lead);
                    if (resp == null)
                    {
                        WriteStatus(99, "Failed to Post Lead, Service returned no response.");
                        return;
                    }
                    if (!resp.Successful)
                    {
                        //Log("Pulse Service Post Lead Result:" + err, 0);
                        err = "Failed to Post Lead, Service error: " + resp.StatusInfo;
                        WriteStatus(1, err);
                    }
                    else
                        WriteStatus(0, "Success");
                }
            }
            catch (Exception ex)
            {
                //Log(ex.Message, 0);
                WriteStatus(0, ex.ToString());
            }

        }


        public void WriteStatus(int ErrorNum, string ErrorMsg)
        {
            StringBuilder sb = new StringBuilder();


            Utf8StringWriter stringWriter = new Utf8StringWriter(sb);
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);


            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("LTACK");

            xmlWriter.WriteStartElement("ERRSTATUS");
            xmlWriter.WriteElementString("ERRORNUM", ErrorNum.ToString());
            xmlWriter.WriteElementString("ERRORDESCRIPTION", ErrorMsg);

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();//end LTACK
            xmlWriter.WriteEndDocument();

            Response.ContentType = "text/xml";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(sb.ToString());
        }


        public void Log(string Message, int Type)
        {
            //type default =0 error
            //EventLog.WriteEntry(EventLogName, Message, EventLogEntryType.Error);
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
            //url = @"http://api.launchingpoint.com/LT_PostLeads.aspx";
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

                //Congratulations, you just requested your first POST page, you
                //can now start logging into most login forms, with your application
                //Or other examples.
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
            return vystup.Trim();
        }
        #endregion



    }

    #region Utf8StringWriter
    public class Utf8StringWriter : StringWriter
    {
        public Utf8StringWriter(StringBuilder sb)
            : base(sb)
        {
        }
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }
    #endregion
}