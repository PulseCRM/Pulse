using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using ShortLeadForm.PulseLeadService;

namespace ShortLeadForm
{
    /// <summary>
    /// Process 的摘要说明
    /// </summary>
    public class Process : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(ProcessLead());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        public string ProcessLead()
        {

            string messageJson = "{\"type\":\"{0}\",\"data\":\"{1}\"}";
            bool isErr = false;

            string LoanOfficerFirstName = string.Empty;
            string LoanOfficerLastName = string.Empty;
            string LeadSource = string.Empty;

            string errorMessage = string.Empty;

            try
            {

                #region LoanOfficer And LeadSource
                if (!string.IsNullOrEmpty(GetForm("LeadSource")) && !string.IsNullOrEmpty(GetForm("LoanOfficerFirstName"))
                    && !string.IsNullOrEmpty(GetForm("LoanOfficerLastName"))
                    )
                {
                    LoanOfficerFirstName = GetForm("LoanOfficerFirstName");
                    LoanOfficerLastName = GetForm("LoanOfficerLastName");
                    LeadSource = GetForm("LeadSource");
                }
                else
                {
                    return "{\"type\":\"fatal\",\"data\":\"Missing required query string(LoanOfficer/LeadSource).\"}";
                }
                #endregion

                #region Post Loan To Service

                string serviceUrl = System.Configuration.ConfigurationManager.AppSettings["PulseLeadServiceURL"].ToString();

                if (string.IsNullOrEmpty(serviceUrl))
                {
                    return "{\"type\":\"fatal\",\"data\":\"Service URl Error.\"}";
                }

                using (PulseLeadServiceClient client = new PulseLeadServiceClient())
                {

                    PostLoanAppRequest PostLoanApp_req = new PostLoanAppRequest();

                    PostLoanApp_req.RequestHeader = new ReqHdr();

                    PostLoanApp_req.RequestHeader.SecurityToken = System.Configuration.ConfigurationManager.AppSettings["SecurityToken"].ToString();

                    

                    #region LoanOfficer / Branch
                    PostLoanApp_req.LoanOfficerFirstName = LoanOfficerFirstName;
                    PostLoanApp_req.LoanOfficerLastName = LoanOfficerLastName;
                    //PostLoanApp_req.BranchName = BranchName;
                    PostLoanApp_req.LeadSource = LeadSource;
                    #endregion

                    #region Borrower Information
                    PostLoanApp_req.BorrowerFirstName = GetForm("txtFirstName");
                    PostLoanApp_req.BorrowerMiddleName = "";
                    PostLoanApp_req.BorrowerLastName = GetForm("txtLastName");
                    PostLoanApp_req.HomePhone = GetForm("txtPhone");
                    PostLoanApp_req.Email = GetForm("txtEmail");

                    PostLoanApp_req.CreditRanking = CreditRanking.Good;

                    PostLoanApp_req.PreferredContactMethod = PreferredContactMethod.Email;
                    #endregion

                    #region LoanAmount / PurposeOfLoan
                    PostLoanApp_req.LoanAmount = 0;
                    PostLoanApp_req.PurposeOfLoan = PurposeOfLoan.Other;

                    #endregion


                    #region Comments/Notes
                    string comment = GetForm("txtComments");
                    string area = GetForm("selArea");
                    if (!string.IsNullOrEmpty(area))
                    {
                        if (!area.Contains("--Select an area"))
                        {
                            PostLoanApp_req.Notes = string.Format("Area Of Interest={0}", area, comment);
                        }
                    }
                    if (!string.IsNullOrEmpty(comment))
                    {
                        if (!string.IsNullOrEmpty(PostLoanApp_req.Notes))
                        {
                            PostLoanApp_req.Notes += "&";
                        }

                        PostLoanApp_req.Notes += string.Format("Comments={0}", comment);
                    }
                    #endregion

                    PostLoanApp_req.OccupancyType = OccupancyType.PrimaryResidence;

                    RespHdr resp1 = null;
                    //resp1 = client.PostLoanApp2(PostLoanApp_req);
                    resp1 = client.PostLoanApp(PostLoanApp_req);
                    
                    if (!resp1.Successful)
                    {
                        isErr = true;
                        errorMessage = resp1.Error;
                        messageJson = "{\"type\":\"fatal\",\"data\":\"" + errorMessage + "\"}";//error
                    }
                    else
                    {
                        messageJson = "{\"type\":\"success\",\"data\":\"Your application has been receieved. Your Loan Officer will contact you shortly.\"}";
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                isErr = true;
                errorMessage = "Server exception:" + ex.Message;
                messageJson = "{\"type\":\"fatal\",\"data\":\"" + errorMessage + "\"}";
            }
            finally
            {

            }

            return messageJson;

        }


        /// <summary>
        /// GetForm
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        private string GetForm(string Name)
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form[Name]))
            {
                return HttpContext.Current.Request.Form[Name].ToString().Trim();
            }
            return string.Empty;
        }
    }
}