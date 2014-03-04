using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using LP2.Service.Common;


namespace LP2Service
{
    public class LoanPost
    {
        short Category = 90;
        static readonly DataAccess.DataAccess da = new DataAccess.DataAccess();
        static readonly Dictionary<int, string> loanFields = new Dictionary<int, string>();

        static LoanPost()
        {
            loanFields.Add(327, "loannumber");
            loanFields.Add(7339, "CommitmentNumber");
            loanFields.Add(101, "borrower_lastname");
            loanFields.Add(2836, "FICO");
            loanFields.Add(31, "PropertyAddr");
            loanFields.Add(32, "PropertyCity");
            loanFields.Add(33, "PropertyState");
            loanFields.Add(34, "PropertyZip");
            loanFields.Add(99902, "LoanProgram");
            loanFields.Add(13, "LoanTerm");
            loanFields.Add(12, "Rate");
            loanFields.Add(819, "LoanAmount");
            loanFields.Add(800, "SalesPrice");
            loanFields.Add(801, "AppraisedValue");
            loanFields.Add(99903, "Purpose");
            loanFields.Add(99904, "LoanType");
            loanFields.Add(4001, "FullDoc");
            loanFields.Add(2797, "AU");
            loanFields.Add(543, "BottomRatio");
            loanFields.Add(99906, "Occupancy");
            loanFields.Add(99905, "PropertyType");
            loanFields.Add(36, "Units");
            loanFields.Add(4003, "EscrowTaxes");
            loanFields.Add(4004, "EscrowInsurance");
            loanFields.Add(540, "LTV");
            loanFields.Add(541, "CLTV");
            loanFields.Add(7341, "Investor");
            loanFields.Add(7336, "InvestorLoanNumber");
            loanFields.Add(11841, "LockType");
            loanFields.Add(11452, "ShipDate");
            loanFields.Add(99901, "LoanStatus");
            loanFields.Add(6170, "Yield");
            loanFields.Add(11604, "Yield2");
            loanFields.Add(6064, "LenderFee");
            loanFields.Add(3287, "TPO_YSP");
            loanFields.Add(6075, "EstCloseDate");
            loanFields.Add(6055, "DateClosed");
            loanFields.Add(19, "LoanRep");
            loanFields.Add(6031, "InvestorSpec");
            loanFields.Add(3285, "MandatoryPrice");
            loanFields.Add(6025, "DateSubmited");
            loanFields.Add(6030, "DateApproved");
            loanFields.Add(6027, "CTC");
            loanFields.Add(6061, "DateLocked");
            loanFields.Add(6063, "DateLockExpires");
            loanFields.Add(11439, "LockExtension");
            loanFields.Add(1023, "LoanOriginationFeeType");
            loanFields.Add(1024, "DiscountFee");
            loanFields.Add(6040, "DateFunded");
            loanFields.Add(61, "DatePurchased");
        }

        /// <summary>
        /// Posts the active loan.
        /// </summary>
        public int PostActiveLoan()
        {
            Common.Table.Company_MCT companyMct = null;

            da.GetCompanyMCT(ref companyMct);
            int numLoansPosted = 0;
            if (companyMct == null || companyMct.PostDataEnabled.GetValueOrDefault(false) == false || string.IsNullOrEmpty(companyMct.PostURL) || string.IsNullOrEmpty(companyMct.ClientID))
            {
                int Event_id = 8010;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "Post Active Loan configuration data error.", EventLogEntryType.Warning, Event_id, Category);
                return numLoansPosted;
            }

            if (!IsValidURL(companyMct.PostURL))
            {
                return numLoansPosted;
            }

            DataSet dsActiveLoans = da.GetPostActiveLoan();

            if (dsActiveLoans == null || dsActiveLoans.Tables.Count < 1 || dsActiveLoans.Tables[0].Rows.Count < 1)
            {
                return numLoansPosted;
            }

            var sb = new StringBuilder();
            var stringWriter = new StringWriterUtf8(sb);
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("client");//start client
            xmlWriter.WriteElementString("client_id", companyMct.ClientID);//client_id
            xmlWriter.WriteElementString("data_source", "Pulse");//data_source
            xmlWriter.WriteElementString("data_collection", "Current");//data_collection

            xmlWriter.WriteStartElement("loandata");//start loandata
            //for loans start
            var loanData = from item in dsActiveLoans.Tables[0].AsEnumerable()
                           select new
                           {
                               FileId = item.Field<int>("FileId"),
                               PointFieldId = item.Field<int>("PointFieldId"),
                               CurrentValue = item.Field<string>("CurrentValue")
                           };
            var gLoans = from item in loanData
                         group item by item.FileId into g
                         select new { FileId = g.Key, LoanData = g };

            foreach (var gLoan in gLoans)
            {
                xmlWriter.WriteStartElement("loan");//start loan

                string eleName = string.Empty;
                string eleValue = string.Empty;
                foreach (var loan in gLoan.LoanData)
                {
                    eleName = string.Empty;
                    eleValue = string.Empty;

                    loanFields.TryGetValue(loan.PointFieldId, out eleName);
                    eleValue = loan.CurrentValue;
                    if (!string.IsNullOrEmpty(eleName))
                    {
                        if (loan.PointFieldId == 4001)
                            eleValue = eleValue == "X" ? "Yes" : "No";
                        xmlWriter.WriteElementString(eleName, eleValue);
                    }
                }

                xmlWriter.WriteEndElement();//end loan
                numLoansPosted++;
            }
            //for loans end
            xmlWriter.WriteEndElement();//end loandata

            xmlWriter.WriteEndElement();//end client
            xmlWriter.WriteEndDocument();//end document

            PostDataToServer(sb.ToString(), companyMct.PostURL);
            return numLoansPosted;
        }

        /// <summary>
        /// Posts the archived loan.
        /// </summary>
        public int PostArchivedLoan()
        {
            Common.Table.Company_MCT companyMct = null;
            da.GetCompanyMCT(ref companyMct);
            int numLoansPosted = 0;
            if (companyMct == null || companyMct.PostDataEnabled.GetValueOrDefault(false) == false || string.IsNullOrEmpty(companyMct.PostURL) || string.IsNullOrEmpty(companyMct.ClientID))
            {
                int Event_id = 8011;                
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "Post Active Loan CompanyMCT data not configured.", EventLogEntryType.Warning, Event_id, Category);
                return numLoansPosted;
            }

            if (!IsValidURL(companyMct.PostURL))
            {
                int Event_id = 8012;                
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "Post Active Loan CompanyMCT.PostURL not configured.", EventLogEntryType.Warning, Event_id, Category);
                return numLoansPosted;
            }

            DataSet activeLoans = da.GetPostArchivedLoan();

            if (activeLoans == null || activeLoans.Tables.Count < 1 || activeLoans.Tables[0].Rows.Count < 1)
            {
                return numLoansPosted;
            }

            var sb = new StringBuilder();
            var stringWriter = new StringWriterUtf8(sb);
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("client");//start client
            xmlWriter.WriteElementString("client_id", companyMct.ClientID);//client_id
            xmlWriter.WriteElementString("data_source", "Pulse");//data_source
            xmlWriter.WriteElementString("data_collection", "Archive");//data_collection

            xmlWriter.WriteStartElement("loandata");//start loandata
            //for loans start
            var loanData = from item in activeLoans.Tables[0].AsEnumerable()
                           select new
                           {
                               FileId = item.Field<int>("FileId"),
                               PointFieldId = item.Field<int>("PointFieldId"),
                               CurrentValue = item.Field<string>("CurrentValue")
                           };
            var gLoans = from item in loanData
                         group item by item.FileId into g
                         select new { FileId = g.Key, LoanData = g };

            foreach (var gLoan in gLoans)
            {
                xmlWriter.WriteStartElement("loan");//start loan

                string eleName = string.Empty;
                string eleValue = string.Empty;
                foreach (var loan in gLoan.LoanData)
                {
                    eleName = string.Empty;
                    eleValue = string.Empty;

                    loanFields.TryGetValue(loan.PointFieldId, out eleName);
                    eleValue = loan.CurrentValue;
                    if (!string.IsNullOrEmpty(eleName))
                    {
                        if (loan.PointFieldId == 4001)
                            eleValue = eleValue == "X" ? "Yes" : "No";
                        xmlWriter.WriteElementString(eleName, eleValue);
                    }
                }

                xmlWriter.WriteEndElement();//end loan
                numLoansPosted++;
            }
            //for loans end
            xmlWriter.WriteEndElement();//end loandata

            xmlWriter.WriteEndElement();//end client
            xmlWriter.WriteEndDocument();//end document

            PostDataToServer(sb.ToString(), companyMct.PostURL);
            return numLoansPosted;
        }

        /// <summary>
        /// Determines whether [is valid URL] [the specified URL].
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        ///   <c>true</c> if [is valid URL] [the specified URL]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidURL(string url)
        {
            bool foundMatch = false;
            foundMatch = Regex.IsMatch(url, @"\b(https?)://[-A-Z0-9+&@#/%?=~_|!:,.;]*[A-Z0-9+&@#/%=~_|]", RegexOptions.IgnoreCase);
            return foundMatch;
        }

        /// <summary>
        /// Posts the data to server.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="serverUrl">The server URL.</param>
        /// <returns></returns>
        private string PostDataToServer(string data, string serverUrl)
        {

            var buffer = Encoding.UTF8.GetBytes(data);
            string response;

            var webReq = (HttpWebRequest)WebRequest.Create(serverUrl);
            webReq.Timeout = 60 * 1000;//60 seconds
            webReq.Method = "POST";

            webReq.ContentLength = buffer.Length;

            using (var postData = webReq.GetRequestStream())
            {
                postData.Write(buffer, 0, buffer.Length);
                postData.Close();
            }



            var httpWebResponse = (HttpWebResponse)webReq.GetResponse();

            using (var responseStream = httpWebResponse.GetResponseStream())
            {
                using (var streamReader = new StreamReader(responseStream))
                {
                    response = streamReader.ReadToEnd();
                }
            }

            return response;
        }
    }

    internal class StringWriterUtf8 : StringWriter
    {
        public StringWriterUtf8(StringBuilder sb)
            : base(sb)
        {
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

    }
}
