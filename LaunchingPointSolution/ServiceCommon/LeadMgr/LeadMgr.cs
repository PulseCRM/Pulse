using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Web;
using LP2.Service.Common;
using DataAccess;
using Common;
using FocusIT.Pulse;
using focusIT;
using System.Threading;

namespace LeadMgr
{
    public class PostArgs
    {
        public int LoanId { get; set; }
        public int LoanOfficerId { get; set; }
        public int ContactId { get; set; }
        public int BranchId { get; set; }

        //gdc CR42
        public int CoBorrowerContactId { get; set; }
    }
    public class InternalLeadReq
    {
        public PostLeadRequest req;
        public PostArgs _postArgs;
    }
    public class InternalLR_GetNextLoanOfficerReq
    {
        public LR_GetNextLoanOfficerReq req;
        public PostArgs _postArgs;
    }

    public class LeadMgr
    {
        short Category = 80;

        public bool PostLead(PostLeadRequest req, out string err)
        {
            bool status = false;
            err = string.Empty;
            try
            {
                status = CheckData(ref req, ref err);
                if (!status)
                    return status;
                InternalLeadReq leadReq = new InternalLeadReq();
                leadReq.req = req;
                status = ThreadPool.QueueUserWorkItem(new WaitCallback(ImportLead), (object)leadReq);
            }
            catch (Exception ex)
            {
                err = "PostLead Error:" + ex.ToString();
                int Event_id = 8001;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.ToString(), EventLogEntryType.Error, Event_id, Category);
            }
            return status;
        }

        private bool CheckData(ref PostLeadRequest req, ref string err)
        {
            err = string.Empty;
            if (req == null || req.RequestHeader == null)
            {
                err = "The Request ore Request Header cannot be empty.";
                return false;
            }

            if (string.IsNullOrEmpty(req.CellPhone) &&
                string.IsNullOrEmpty(req.Email) &&
                string.IsNullOrEmpty(req.BusinessPhone) &&
                string.IsNullOrEmpty(req.HomePhone))
            {
                err = "Must have at least one contact method specified, Email, Cell Phone, Business Phone or Home Phone.";
                return false;
            }

            try
            {
                string SqlCmd = string.Format("Select top 1 GlobalId from Company_General ");
                object obj = DbHelperSQL.GetSingle(SqlCmd);
                string CompanySecurityToken = (obj == null || obj == DBNull.Value) ? string.Empty : (string)obj;

                if (CompanySecurityToken != req.RequestHeader.SecurityToken)
                {
                    err = "Could not authenticate the company security token.";
                    return false;
                }
                if (string.IsNullOrEmpty(req.BorrowerFirstName))
                    req.BorrowerFirstName = "TBD";
                if (string.IsNullOrEmpty(req.BorrowerLastName))
                {
                    req.BorrowerLastName = "TBD";
                    req.BorrowerLastName += DateTime.Now.Ticks.ToString();
                }

                if (req.LoanAmount == null)
                    req.LoanAmount = 0;
                if (req.PropertyValue == null)
                    req.PropertyValue = 0;

                return true;
            }
            catch (Exception ex)
            {
                err = ex.ToString();
                int Event_id = 8002;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
                return false;
            }
        }

        private void LogXMLRequest(object req)
        {
            var settings = new XmlWriterSettings { Indent = true };

            using (var xw = XmlWriter.Create("PulseLead.xml", settings))
            {
                DataContractSerializer ds = null;
                PostLeadRequest req1 = req as PostLeadRequest;
                if (req1 != null)
                    ds = new DataContractSerializer(typeof(PostLeadRequest));

                ds.WriteObject(xw, req);
            }
        }

        private void Find_Setup_Branch_LO(string branchName, string loFirstName, string loLastName, ref PostArgs _PostArgs)
        {
            int BranchId = 0;
            string err = "";
            // if there is a branch name, get the branchId
            if (!string.IsNullOrEmpty(branchName))
                BranchId = FindBranch(branchName, ref err);

            // try to find the Loan Officer UserId if specified
            string LoanOfficer = string.IsNullOrEmpty(loFirstName) || string.IsNullOrEmpty(loLastName)
                                     ? string.Empty
                                     : loFirstName.Trim() + " " + loLastName.Trim();
            int LO_Id = 0;
            if (!string.IsNullOrEmpty(LoanOfficer))
                LO_Id = FindLoanOfficer(LoanOfficer, BranchId, ref err);

            /*   No need CR072
                      //If Branch is found but there is no LoanOfficer specified, use the Branch Manager instead
                        if (LO_Id <= 0 && BranchId > 0)
                        {
                            string sqlCmd = string.Format("Select TOP 1 BranchMgrId from BranchManagers where BranchId={0}",
                                                          BranchId);
                            object obj = focusIT.DbHelperSQL.GetSingle(sqlCmd);
                            LO_Id = obj == null ? 0 : (int)obj;
                        }*/
            if (_PostArgs == null)
                _PostArgs = new PostArgs();
            _PostArgs.BranchId = BranchId;
            _PostArgs.LoanOfficerId = LO_Id;
        }
        private void ImportLead(object obj)
        {
            string err = string.Empty;
            int LoanId = 0;
            bool isLeadRouting = false;
            bool isLeadRoutingError = false;
            #region checks for nulls
            InternalLeadReq leadReq = obj as InternalLeadReq;
            if (leadReq == null)
            {
                int Event_id = 8003;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "ImportLead, PostLeadRequest or RequestHeader is null.", EventLogEntryType.Warning, Event_id, Category);
                return;
            }
            PostLeadRequest req = leadReq.req;
            if (req == null || req.RequestHeader == null)
            {
                int Event_id = 8004;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "ImportLead, PostLeadRequest or RequestHeader is null.", EventLogEntryType.Warning, Event_id, Category);
                return;
            }
            if (leadReq._postArgs == null)
                leadReq._postArgs = new PostArgs();
            #endregion
            #region check duplicates
            int BorrowerId = 0;
            DataAccess.DataAccess da = new DataAccess.DataAccess();
            //gdc 20130303 CR61
            try
            {
                if (!string.IsNullOrEmpty(req.LeadId) && req.CheckDuplicate == true)
                {
                    string sqlCmd = string.Format("Select TOP 1 FileId from Loans where GlobalId='{0}'",
                                                  req.LeadId);
                    object obj1 = focusIT.DbHelperSQL.GetSingle(sqlCmd);
                    LoanId = (obj1 == null || obj1 == DBNull.Value) ? 0 : (int)obj1;
                }
                //gdc 20130303 CR61 end
            #endregion
                if (!string.IsNullOrEmpty(req.BranchName) ||
                    (!string.IsNullOrEmpty(req.LoanOfficerFirstName) && !string.IsNullOrEmpty(req.LoanOfficerLastName)))
                {
                    Find_Setup_Branch_LO(req.BranchName, req.LoanOfficerFirstName, req.LoanOfficerLastName, ref leadReq._postArgs);
                }

                if ((string.IsNullOrEmpty(req.LoanOfficerFirstName) && string.IsNullOrEmpty(req.LoanOfficerLastName)) || leadReq._postArgs.LoanOfficerId <= 0)
                {
                    err = string.Empty;
                    leadReq._postArgs.LoanOfficerId = GetNextLoanofficer(req.LeadSource, req.Property_State,
                                                                         req.PurposeOfLoan.ToString(), req.LoanType, ref err);
                    isLeadRouting = true;
                    if (!string.IsNullOrEmpty(err))
                    {
                        isLeadRoutingError = true;
                    }
                }

                if (LoanId > 0)
                    BorrowerId = da.GetBorrowerContactID(LoanId);
            }
            catch (Exception ex1)
            {
                int Event_id = 8005;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "ImportLead, Exception: " + ex1.ToString(), EventLogEntryType.Warning, Event_id, Category);
            }
            if (BorrowerId <= 0)
            {
                BorrowerId = CreateContact(req, leadReq._postArgs.LoanOfficerId, BorrowerId, true, ref err);
                if (BorrowerId <= 0)
                {
                    int Event_id = 8006;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, "ImportLead, failed to create Borrower Contact records, Error:" + err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }
            }
            // save the loan record
            if (LoanId <= 0)
            {
                LoanId = CreateLoan(req, BorrowerId, leadReq._postArgs.BranchId, leadReq._postArgs.LoanOfficerId, ref err);
                if (LoanId <= 0)
                {
                    int Event_id = 8007;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, "ImportLead, failed to create Loan records, Error:" + err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }
            }
            if (isLeadRouting)
            {
                LR_AssignLoanOfficerReq aLoReq = new LR_AssignLoanOfficerReq();
                aLoReq.LoanId = LoanId;
                aLoReq.LoanOfficerId = leadReq._postArgs.LoanOfficerId;
                err = string.Empty;
                AssignLoanOfficer(aLoReq, out err);
                if ((!string.IsNullOrEmpty(err) || isLeadRoutingError) && !string.IsNullOrEmpty(req.LeadSource))
                {
                    int newLoanOfficerId = GetLOIDByLeadSource(req.LeadSource, ref err);
                    UpdateLoan(LoanId, newLoanOfficerId, ref err);
                    if (!string.IsNullOrEmpty(err))
                    {
                        int Event_id = 8007;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, "ImportLead, failed to update Loan records, Error:" + err, EventLogEntryType.Warning, Event_id, Category);
                    }
                }
            }

            leadReq._postArgs.ContactId = BorrowerId;
            leadReq._postArgs.LoanId = LoanId;
            SendEmailLeadCreated(LoanId);

            if (string.IsNullOrEmpty(req.CoBorrowerFirstName) || string.IsNullOrEmpty(req.CoBorrowerLastName))
                return;
            int CoBorrowerId = 0;
            CoBorrowerId = CreateContact(req, leadReq._postArgs.LoanOfficerId, CoBorrowerId, false, ref err);
            if (CoBorrowerId <= 0)
            {
                int Event_id = 8008;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, "ImportLead, failed to create Coborrower Contact records, Error:" + err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }

            return;
        }



        private void SendEmailLeadCreated(int LoanId)
        {
            DataAccess.DataAccess da = new DataAccess.DataAccess();
            //int fromUser=da.ge
            int BorrowerContactID = 0;
            try
            {
                BorrowerContactID = da.GetBorrowerContactID(LoanId);
            }
            catch (Exception e)
            {
                int Event_id = 8009;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, e.ToString(), EventLogEntryType.Warning, Event_id, Category);
            }
            List<Table.TemplateEmail> templateEmails = da.GetTemplateEmail();

            if (templateEmails == null)
                return;

            foreach (var templateEmail in templateEmails)
            {
                try
                {
                    if (templateEmail == null)
                        continue;

                    EmailManager.EmailMgr em = EmailManager.EmailMgr.Instance;

                    #region SendEmailRequest
                    SendEmailRequest req = new SendEmailRequest();
                    req.EmailTemplId = templateEmail.TemplEmailId;
                    req.FileId = LoanId;
                    req.hdr = new LP2.Service.Common.ReqHdr();
                    #endregion
                    bool status = em.SendEmail(req);
                    if (!status)
                    {
                        string err =
                            string.Format(
                                "Faild to send email for EmailTemplId={0} FileId={1} BorrowerContactId={2} ",
                                templateEmail.TemplEmailId, LoanId, BorrowerContactID);
                        int Event_id = 8010;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }

                }
                catch (Exception ex)
                {
                    int Event_id = 8011;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, ex.ToString(), EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        private int FindBranch(string BranchName, ref string err)
        {
            int BranchId = 0;
            err = string.Empty;
            if (string.IsNullOrEmpty(BranchName))
            {
                err = "Branch Name is blank.";
                return BranchId;
            }
            string sqlCmd = string.Format("select BranchId from Branches where [Name]='{0}'", BranchName);
            object obj = focusIT.DbHelperSQL.GetSingle(sqlCmd);
            BranchId = obj == null ? 0 : (int)obj;
            return BranchId;
        }

        private int FindLoanOfficer(string LoanOfficer, int BranchId, ref string err)
        {
            int LO_Id = 0;
            err = string.Empty;
            if (string.IsNullOrEmpty(LoanOfficer))
            {
                err = "Loan Officer name is blank.";
                return LO_Id;
            }

            //DataAccess.DataAccess da = new DataAccess.DataAccess();

            string sqlCmd = string.Format("select dbo.lpfn_FindUserByFullname('{0}', '{1}', '{2}')", LoanOfficer,
                                          "Loan Officer", BranchId);
            object obj = focusIT.DbHelperSQL.GetSingle(sqlCmd);
            LO_Id = obj == null ? 0 : (int)obj;
            return LO_Id;
        }

        public int CreateContact(PostLeadRequest req, int LoanOfficerId, int ContactId, bool IsBorrower, ref string err)
        {
            err = string.Empty;
            string roleName = (IsBorrower) ? ContactRoles.ContactRole_Borrower : ContactRoles.ContactRole_Coborrower;
            FocusIT.Pulse.Address MailingAddress_temp = null;

            MailingAddress_temp = new FocusIT.Pulse.Address();
            MailingAddress_temp = req.MailingAddress;
            Common.Record.Contacts contactRec = new Common.Record.Contacts();
            Employment[] employments = null;
            OtherIncome[] otherIncomes = null;
            LiquidAssets[] liquidAssets = null;
            //contactRec.ContactId = 0;
            contactRec.ContactId = ContactId; //CR61
            // if it's borrower, use the borrower's fields
            #region Borrower, Coborrower
            if (IsBorrower)
            {
                contactRec.FirstName = req.BorrowerFirstName;
                contactRec.MiddleName = req.BorrowerMiddleName;
                contactRec.LastName = req.BorrowerLastName;
                contactRec.DOB = req.DOB;
                contactRec.SSN = req.SSN;

                contactRec.HomePhone = string.IsNullOrEmpty(req.HomePhone) ? string.Empty : req.HomePhone;
                contactRec.BusinessPhone = string.IsNullOrEmpty(req.BusinessPhone) ? string.Empty : req.BusinessPhone;
                contactRec.CellPhone = string.IsNullOrEmpty(req.CellPhone) ? string.Empty : req.CellPhone;
                contactRec.Email = string.IsNullOrEmpty(req.Email) ? string.Empty : req.Email;
                if (req.Employment != null)
                    employments = req.Employment;
                if (req.OtherIncome != null)
                    otherIncomes = req.OtherIncome;
                if (req.LiquidAssets != null)
                    liquidAssets = req.LiquidAssets;
            }
            else   // Coborrower
            {
                if (req.CoBorrowerFirstName != null && req.CoBorrowerLastName != null)
                {
                    contactRec.FirstName = req.CoBorrowerFirstName;
                    contactRec.LastName = req.CoBorrowerLastName;
                    contactRec.MiddleName = req.CoBorrowerMiddleName;
                    //gdc CR42 contactRec.CellPhone = string.IsNullOrEmpty(req.CoBorrowerPhone) ? string.Empty : req.CoBorrowerPhone;
                    contactRec.Email = string.IsNullOrEmpty(req.CoBorrowerEmail) ? string.Empty : req.CoBorrowerEmail;

                    //gdc CR42
                    contactRec.BusinessPhone = string.IsNullOrEmpty(req.CoBorrowerBusinessPhone) ? string.Empty : req.CoBorrowerBusinessPhone;
                    contactRec.CellPhone = string.IsNullOrEmpty(req.CoBorrowerCellPhone) ? string.Empty : req.CoBorrowerCellPhone;
                    contactRec.HomePhone = string.IsNullOrEmpty(req.CoBorrowerPhone) ? string.Empty : req.CoBorrowerPhone;
                }
                if (req.CoBorrowerEmployers != null)
                    employments = req.CoBorrowerEmployers;
                if (req.CoBorrowerOtherIncome != null)
                    otherIncomes = req.CoBorrowerOtherIncome;
            }
            if (MailingAddress_temp != null)
            {
                contactRec.MailingAddr = MailingAddress_temp.Street;
                contactRec.MailingCity = MailingAddress_temp.City;
                contactRec.MailingState = MailingAddress_temp.State;
                contactRec.MailingZip = MailingAddress_temp.Zip;
            }
            else
            {
                contactRec.MailingAddr = string.Empty;
                contactRec.MailingCity = string.Empty;
                contactRec.MailingState = string.Empty;
                contactRec.MailingZip = string.Empty;
            }
            #endregion
            //contactRec.Enable = true;

            DataAccess.DataAccess da = new DataAccess.DataAccess();

            ContactId = da.Save_Contacts(contactRec, roleName, ContactId, ref err);
            if (ContactId <= 0)
                return ContactId;
            #region Save Prospect Record
            Common.Table.Prospect prospectRec = new Common.Table.Prospect();
            switch (req.PreferredContactMethod)
            {
                case PreferredContactMethod.BusinessPhone:
                    prospectRec.PreferredContact = "Business Phone";
                    break;
                case PreferredContactMethod.CellPhone:
                    prospectRec.PreferredContact = "Cell Phone";
                    break;
                case PreferredContactMethod.HomePhone:
                    prospectRec.PreferredContact = "Home Phone";
                    break;
                case PreferredContactMethod.Email:
                    prospectRec.PreferredContact = "Email";
                    break;
            }
            prospectRec.CreditRanking = req.CreditRanking == null ? string.Empty : req.CreditRanking.ToString();
            if (req.CreditRanking == CreditRanking.VeryGood)
                prospectRec.CreditRanking = "Very Good";

            if (LoanOfficerId > 0)
                prospectRec.LoanOfficer = LoanOfficerId;
            prospectRec.ContactId = ContactId;
            prospectRec.Dependents = req.HaveDependents;
            prospectRec.LeadSource = string.IsNullOrEmpty(req.LeadSource) ? "Internet" : req.LeadSource;
            da.Save_Prospect(prospectRec, ref err);
            string sqlCommand = string.Format("Update Prospect set Dependents='{0}', LeadSource='{2}' WHERE ContactId={1}", req.HaveDependents ? 1 : 0, ContactId, prospectRec.LeadSource);

            DbHelperSQL.ExecuteSql(sqlCommand);

            #endregion
            #region Employment
            if (employments != null)
            {
                foreach (Employment employment in employments)
                {
                    try
                    {
                        SaveEmployment(employment, ContactId, ref err);
                    }
                    catch (Exception ex_Em)
                    {
                        int Event_id = 8012;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, "CreateContact, Exception in Employment: " + ex_Em.ToString(), EventLogEntryType.Warning, Event_id, Category);
                    }
                }
            }
            #endregion
            #region OtherIncome
            if (otherIncomes != null)
            {
                foreach (OtherIncome otherIncome in otherIncomes)
                {
                    try
                    {
                        SaveOtherIncome(otherIncome, ContactId, ref err);
                    }
                    catch (Exception ex_OI)
                    {
                        int Event_id = 8013;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, "CreateContact, Exception in SaveOtherIncome: " + ex_OI.ToString(), EventLogEntryType.Warning, Event_id, Category);
                    }
                }
            }
            #endregion
            #region LiquidAssets
            if (liquidAssets != null)
            {
                foreach (LiquidAssets liquidAssetse in liquidAssets)
                {
                    try
                    {
                        SaveLiquidAssets(liquidAssetse, ContactId, ref err);
                    }
                    catch (Exception ex_LA)
                    {
                        int Event_id = 8014;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, "CreateContact, Exception in SaveOtherIncome: " + ex_LA.ToString(), EventLogEntryType.Warning, Event_id, Category);
                    }
                }
            }
            #endregion
            if (!IsBorrower)
                return ContactId;
            #region Update Borrower Contact Middlename, DOB, and SSN
            if (ContactId > 0 && (!string.IsNullOrEmpty(req.BorrowerMiddleName) ||
                !string.IsNullOrEmpty(req.DOB) || !string.IsNullOrEmpty(req.SSN)))
            {
                string sqlCmd =
                    string.Format("Update [dbo].[Contacts] set MiddleName='{0}', DOB='{1}', SSN='{2}' WHERE ContactId={3}",
                                            req.BorrowerMiddleName, req.DOB, req.SSN, ContactId);
                DbHelperSQL.ExecuteSql(sqlCmd);
            }
            #endregion
            return ContactId;
        }

        private int CreateLoan(PostLeadRequest req, int ContactId, int BranchId, int LoanOfficerId, ref string err)
        {
            int LoanId = 0;

            //CR61
            #region Create a Loan Record
            SqlParameter[] parameters =
                {
                    new SqlParameter("@FileId", SqlDbType.Int, 4), //0
                    new SqlParameter("@FolderId", SqlDbType.Int, 4), //1
                    new SqlParameter("@FileName", SqlDbType.NVarChar, 255), //2
                    new SqlParameter("@EstCloseDate", SqlDbType.DateTime), //3
                    new SqlParameter("@LoanAmount", SqlDbType.Money), //4
                    new SqlParameter("@Program", SqlDbType.NVarChar, 255), //5
                    new SqlParameter("@PropertyAddr", SqlDbType.NVarChar, 50), //6
                    new SqlParameter("@PropertyCity", SqlDbType.NVarChar, 50), //7
                    new SqlParameter("@PropertyState", SqlDbType.NVarChar, 2), //8
                    new SqlParameter("@PropertyZip", SqlDbType.NVarChar, 10), //9
                    new SqlParameter("@Rate", SqlDbType.SmallMoney), //10
                    new SqlParameter("@Status", SqlDbType.NVarChar, 50), //11
                    new SqlParameter("@ProspectLoanStatus", SqlDbType.NVarChar, 50), //12
                    new SqlParameter("@Ranking", SqlDbType.NVarChar, 20), //13
                    new SqlParameter("@Created", SqlDbType.DateTime), //14
                    new SqlParameter("@CreatedBy", SqlDbType.Int, 4), //15
                    new SqlParameter("@Modifed", SqlDbType.DateTime), //16
                    new SqlParameter("@ModifiedBy", SqlDbType.Int, 4), //17
                    new SqlParameter("@BoID", SqlDbType.Int, 4), //18
                    new SqlParameter("@CoBoID", SqlDbType.Int, 4), //19
                    new SqlParameter("@UserId", SqlDbType.Int, 4), //20
                    new SqlParameter("@Lien", SqlDbType.NVarChar, 50), //21
                    new SqlParameter("@Purpose", SqlDbType.NVarChar, 50), //22
                    new SqlParameter("@LoanOfficerId", SqlDbType.Int, 4), //23
                    new SqlParameter("@BranchId", SqlDbType.Int, 4), //24
                    new SqlParameter("@PropertyType", SqlDbType.NVarChar, 255), //25
                    new SqlParameter("@HousingStatus", SqlDbType.NVarChar, 255), //26
                    new SqlParameter("@IncludeEscrows", SqlDbType.Bit), //27
                    new SqlParameter("@InterestOnly", SqlDbType.Bit), //28
                    new SqlParameter("@CoBrwType", SqlDbType.NVarChar, 255), //29
                    new SqlParameter("@RentAmount", SqlDbType.Decimal ),  //30
                    new SqlParameter("@Occupancy", SqlDbType.NVarChar, 50)  //31
               
                };
            // File Id
            if (LoanId > 0) //CR61
            {
                parameters[0].Value = LoanId;
            }
            else
            {
                parameters[0].Value = DBNull.Value;
            }
            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1].Value = DBNull.Value;
            parameters[2].Value = DBNull.Value;
            parameters[3].Value = DBNull.Value;
            if (req.LoanAmount != null)
                parameters[4].Value = req.LoanAmount;
            else
                parameters[4].Value = DBNull.Value;
            if (req.LoanProgram != null)
                parameters[5].Value = req.LoanProgram;
            else
                parameters[5].Value = DBNull.Value;
            if (req.Property_Street != null)
            {
                parameters[6].Value = req.Property_Street;
            }
            else
            {
                parameters[6].Value = DBNull.Value;
            }
            if (req.Property_City != null)
            {
                parameters[7].Value = req.Property_City;
            }
            else
            {
                parameters[7].Value = DBNull.Value;
            }
            if (req.Property_State != null)
            {
                parameters[8].Value = req.Property_State;
            }
            else
            {
                parameters[8].Value = DBNull.Value;
            }
            if (req.Property_Zip != null)
            {
                parameters[9].Value = req.Property_Zip;
            }
            else
            {
                parameters[9].Value = DBNull.Value;
            }
            parameters[10].Value = DBNull.Value;
            parameters[11].Value = "Prospect";
            parameters[12].Value = "Active";
            parameters[13].Value = "Hot";
            parameters[14].Value = DateTime.Now;
            parameters[15].Value = DBNull.Value;
            parameters[16].Value = DateTime.Now;
            parameters[17].Value = DBNull.Value;
            parameters[18].Value = ContactId;
            parameters[19].Value = DBNull.Value;
            parameters[20].Value = DBNull.Value;
            parameters[21].Value = "First";
            parameters[22].Value = req.PurposeOfLoan;
            parameters[23].Value = LoanOfficerId;
            parameters[24].Value = BranchId;

            if (req.PropertyType == null)
            {
                parameters[25].Value = DBNull.Value;
            }
            else
            {
                parameters[25].Value = req.PropertyType;
            }

            if (req.HousingStatus == null)
            {
                parameters[26].Value = DBNull.Value;
            }
            else
            {
                parameters[26].Value = req.HousingStatus;
            }

            if (req.IncludeEscrows == null)
            {
                parameters[27].Value = DBNull.Value;
            }
            else
            {
                parameters[27].Value = req.IncludeEscrows;
            }

            if (req.InterestOnly == null)
            {
                parameters[28].Value = DBNull.Value;
            }
            else
            {
                parameters[28].Value = req.InterestOnly;
            }

            if (req.CoBorrowerType == null)
            {
                parameters[29].Value = DBNull.Value;
            }
            else
            {
                parameters[29].Value = req.CoBorrowerType;
            }

            if (req.RentAmount == null)
            {
                parameters[30].Value = DBNull.Value;
            }
            else
            {
                parameters[30].Value = req.RentAmount;
            }

            if (req.OccupancyType == null)
            {
                parameters[31].Value = DBNull.Value;
            }
            else
            {
                if (req.OccupancyType == OccupancyType.InvestmentProperty)
                {
                    parameters[31].Value = "Investment Property";
                }

                if (req.OccupancyType == OccupancyType.PrimaryResidence)
                {
                    parameters[31].Value = "Primary Residence";
                }

                if (req.OccupancyType == OccupancyType.SecondHome)
                {
                    parameters[31].Value = "Second Home";
                }

            }

            int rows = 0;
            LoanId = DbHelperSQL.RunProcedure("dbo.[pl_LoanDetailSave]", parameters, out rows);
            #endregion
            if (LoanId <= 0)
                return LoanId;

            #region Update the loan with additional info

            string sqlCommand = @"Update [dbo].[Loans]
                                               set PropertyType=@PropertyType,
                                                   HousingStatus=@HousingStatus,
                                                   RentAmount=@RentAmount,
                                                   InterestOnly=@InterestOnly,
                                                   IncludeEscrows=@IncludeEscrows,
                                                    GlobalId=@GlobalId,
                                                    MonthlyPMI=@MonthlyPMI,
                                                    County =@County,
                                                    LoanType=@LoanType,
                                                    Term = @Term,
                                                    Rate =@Rate 
                                          WHERE FileId=@LoanId";

            SqlParameter[] parameters1 =
                {
                       new SqlParameter("@PropertyType", SqlDbType.NVarChar,100)
                       ,new SqlParameter("@HousingStatus", SqlDbType.NVarChar,50)
                       ,new SqlParameter("@RentAmount", SqlDbType.Decimal)
                       ,new SqlParameter("@InterestOnly", SqlDbType.Bit)
                       ,new SqlParameter("@IncludeEscrows",SqlDbType.Bit)
                       ,new SqlParameter("@LoanId",SqlDbType.Int)
                        ,new SqlParameter("@GlobalId", SqlDbType.NVarChar,255)
                       ,new SqlParameter("@MonthlyPMI", SqlDbType.Decimal)
                       ,new SqlParameter("@County",SqlDbType.NVarChar,50)
                       ,new SqlParameter("@LoanType",SqlDbType.NVarChar,50)
                       ,new SqlParameter("@Term",SqlDbType.SmallInt)
                       ,new SqlParameter("@Rate",SqlDbType.SmallMoney)
                };

            parameters1[0].Value = req.PropertyType.ToString();
            parameters1[1].Value = req.HousingStatus.ToString();
            parameters1[2].Value = req.RentAmount;
            parameters1[3].Value = req.InterestOnly ? 1 : 0;
            parameters1[4].Value = req.IncludeEscrows ? 1 : 0;
            parameters1[5].Value = LoanId;

            //CR61
            parameters1[6].Value = req.LeadId == null ? "" : req.LeadId;
            parameters1[7].Value = req.MonthlyPayment;
            parameters1[8].Value = req.County == null ? "" : req.County;
            parameters1[9].Value = req.LoanType == null ? "" : req.LoanType;
            parameters1[10].Value = req.Term;
            parameters1[11].Value = req.Rate;
            DbHelperSQL.ExecuteSql(sqlCommand, parameters1);

            #endregion

            #region Add Loan Notes
            if (!string.IsNullOrEmpty(req.Notes))
            {
                string sqlCommandNotes = @"INSERT INTO [LoanNotes]
                                            ([FileId]
                                            ,[Created]
                                            ,[Sender]
                                            ,[Note]
                                            ,[Exported])
                                        VALUES
                                            (@FileId
                                            ,@Created
                                            ,@Sender
                                            ,@Note
                                            ,@Exported)";

                SqlParameter[] parametersNotes =
                {
                        new SqlParameter("@FileId", SqlDbType.Int)
                        ,new SqlParameter("@Created", SqlDbType.DateTime)
                        ,new SqlParameter("@Sender", SqlDbType.NVarChar,255)
                        ,new SqlParameter("@Note", SqlDbType.NVarChar)
                        ,new SqlParameter("@Exported",SqlDbType.Bit)
                };

                parametersNotes[0].Value = LoanId;
                parametersNotes[1].Value = DateTime.Now;
                parametersNotes[2].Value = "Internet";//Sender
                parametersNotes[3].Value = req.Notes;
                parametersNotes[4].Value = false;

                DbHelperSQL.ExecuteSql(sqlCommandNotes, parametersNotes);
            }
            #endregion
            return LoanId;
        }
        private void SaveLiquidAssets(LiquidAssets liquidAssetse, int ContactId, ref string err)
        {
            string sqlCommand = @"INSERT INTO [dbo].[ProspectAssets]
                                               ([ContactId]
                                               ,[Name]
                                               ,[Account]
                                               ,[Amount]
                                               ,[Type])
                                         VALUES
                                               (@ContactId
                                               ,@Name
                                               ,@Account
                                               ,@Amount
                                               ,@Type)";
            SqlParameter[] parameters =
            {
                   new SqlParameter("@ContactId", SqlDbType.Int, 4)
                   ,new SqlParameter("@Name", SqlDbType.NVarChar,50)
                   ,new SqlParameter("@Account", SqlDbType.NVarChar,50)
                   ,new SqlParameter("@Amount", SqlDbType.Decimal)
                   ,new SqlParameter("@Type",SqlDbType.NVarChar,100)
            };

            parameters[0].Value = ContactId;
            parameters[1].Value = liquidAssetse.NameOfAccount;
            parameters[2].Value = liquidAssetse.AccountNo;
            parameters[3].Value = liquidAssetse.Amount;
            parameters[4].Value = DBNull.Value;

            try
            {
                DbHelperSQL.ExecuteSql(sqlCommand, parameters);
            }
            catch (Exception exception)
            {
                err = exception.Message;
                int Event_id = 8015;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
        }

        //gdc CR42
        private void SaveOtherIncome(OtherIncome otherIncome, int contactId, ref string err)
        {
            //gdc CR42 
            if (contactId <= 0)
            {
                return;
            }

            string sqlCommand = @"INSERT INTO [dbo].[ProspectOtherIncome]
                                                       ([ContactId]
                                                       ,[Type]
                                                       ,[MonthlyIncome])
                                                 VALUES
                                                       (@ContactId
                                                       ,@Type
                                                       ,@MonthlyIncome)";
            SqlParameter[] parameters =
            {
                   new SqlParameter("@ContactId", SqlDbType.Int, 4)
                   ,new SqlParameter("@Type", SqlDbType.NVarChar,100)
                   ,new SqlParameter("@MonthlyIncome", SqlDbType.Decimal)
            };

            parameters[0].Value = contactId;//IsBorrower ? postArgs.ContactId : postArgs.CoBorrowerContactId;//postArgs.ContactId;
            parameters[1].Value = otherIncome.Type;
            parameters[2].Value = otherIncome.Amount;

            try
            {
                DbHelperSQL.ExecuteSql(sqlCommand, parameters);
            }
            catch (Exception exception)
            {
                err = exception.Message;
                int Event_id = 8016;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
        }

        //gdc CR42
        private void SaveEmployment(Employment employment, int contactId, ref string err)
        {
            //gdc CR42 
            //int contactId = IsBorrower ? postArgs.ContactId : postArgs.CoBorrowerContactId;
            if (contactId <= 0)
            {
                return;
            }

            int emplid = 0;
            string sqlCommand = @"INSERT INTO [dbo].[ProspectEmployment]
                                                   ([ContactId]
                                                   ,[SelfEmployed]
                                                   ,[Position]
                                                   ,[StartYear]
                                                   ,[StartMonth]
                                                   ,[EndYear]
                                                   ,[EndMonth]
                                                   ,[YearsOnWork]
                                                   ,[Phone]
                                                   ,[ContactBranchId]
                                                   ,[CompanyName]
                                                   ,[Address]
                                                   ,[City]
                                                   ,[State]
                                                   ,[Zip]
                                                   ,[BusinessType]
                                                   ,[VerifyYourTaxes])
                                             VALUES
                                                   (@ContactId
                                                   ,@SelfEmployed
                                                   ,@Position
                                                   ,@StartYear
                                                   ,@StartMonth
                                                   ,@EndYear
                                                   ,@EndMonth
                                                   ,@YearsOnWork
                                                   ,@Phone
                                                   ,@ContactBranchId
                                                   ,@CompanyName
                                                   ,@Address
                                                   ,@City
                                                   ,@State
                                                   ,@Zip
                                                   ,@BusinessType
                                                   ,@VerifyYourTaxes);SELECT @@IDENTITY";
            SqlParameter[] parameters =
            {
                   new SqlParameter("@ContactId", SqlDbType.Int, 4)            //0
                   ,new SqlParameter("@SelfEmployed", SqlDbType.Bit)           //1
                   ,new SqlParameter("@Position", SqlDbType.NVarChar,50)       //2
                   ,new SqlParameter("@StartYear", SqlDbType.Decimal)          //3
                   ,new SqlParameter("@StartMonth", SqlDbType.Decimal)         //4
                   ,new SqlParameter("@EndYear", SqlDbType.Decimal)            //5
                   ,new SqlParameter("@EndMonth", SqlDbType.Decimal)           //6 
                   ,new SqlParameter("@YearsOnWork", SqlDbType.Decimal)        //7
                   ,new SqlParameter("@Phone", SqlDbType.NVarChar,20)          //8
                   ,new SqlParameter("@ContactBranchId", SqlDbType.Int, 4)     //9
                   ,new SqlParameter("@CompanyName", SqlDbType.NVarChar,255)   //10
                   ,new SqlParameter("@Address", SqlDbType.NVarChar,255)       //11
                   ,new SqlParameter("@City", SqlDbType.NVarChar, 100)          //12 
                   ,new SqlParameter("@State", SqlDbType.NVarChar, 2)         //13
                   ,new SqlParameter("@Zip", SqlDbType.NVarChar, 20)           //14
                   ,new SqlParameter("@BusinessType", SqlDbType.NVarChar, 255) //15
                   ,new SqlParameter("@VerifyYourTaxes", SqlDbType.Bit)        //16
            };

            parameters[0].Value = contactId;
            parameters[1].Value = employment.SelfEmployed;
            parameters[2].Value = employment.Position;
            parameters[3].Value = Convert.ToDecimal(employment.StartYear % 100);//gdc 20111206     
            parameters[4].Value = Convert.ToDecimal(employment.StartMonth);
            parameters[5].Value = Convert.ToDecimal(employment.EndYear % 100);//gdc 20111206     
            parameters[6].Value = Convert.ToDecimal(employment.EndMonth);
            parameters[7].Value = Convert.ToDecimal(employment.YearsOnWork);
            parameters[8].Value = employment.Phone == null ? "" : employment.Phone;    //gdc 20111206       
            parameters[9].Value = DBNull.Value;
            parameters[10].Value = employment.Company;
            if (employment.Address != null)
            {
                parameters[11].Value = employment.Address.Street;
                parameters[12].Value = employment.Address.City;
                parameters[13].Value = employment.Address.State;
                parameters[14].Value = employment.Address.Zip;
            }
            else
            {
                parameters[11].Value = DBNull.Value;
                parameters[12].Value = DBNull.Value;
                parameters[13].Value = DBNull.Value;
                parameters[14].Value = DBNull.Value;
            }

            parameters[15].Value = employment.BusinessType;
            parameters[16].Value = employment.VerifyYourTaxes;

            try
            {

                object obj = DbHelperSQL.GetSingle(sqlCommand, parameters);
                if (obj == null)
                {
                    return;
                }
                emplid = Convert.ToInt32(obj);

            }
            catch (Exception exception)
            {
                err = exception.Message;
                int Event_id = 8017;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }

            if (emplid > 0 && employment.MonthlySalary > 0)
            {
                try
                {
                    SaveIncome(emplid, employment.MonthlySalary, contactId, ref err); //gdc CR42 
                }
                catch (Exception exception)
                {
                    err = exception.Message;
                    int Event_id = 8018;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        //gdc CR42
        private void SaveIncome(int emplid, decimal monthlySalary, int contactId, ref string err)
        {
            //int contactId = IsBorrower ? postArgs.ContactId : postArgs.CoBorrowerContactId;
            if (contactId <= 0)
            {
                return;
            }

            string sqlCommand = @"INSERT INTO [dbo].[ProspectIncome]
                                           ([ContactId]
                                           ,[Salary]
                                           ,[Overtime]
                                           ,[Bonuses]
                                           ,[Commission]
                                           ,[Div_Int]
                                           ,[NetRent]
                                           ,[Other]
                                           ,[EmplId])
                                     VALUES
                                           (@ContactId
                                           ,@Salary
                                           ,@Overtime
                                           ,@Bonuses
                                           ,@Commission
                                           ,@Div_Int
                                           ,@NetRent
                                           ,@Other
                                           ,@EmplId)";
            SqlParameter[] parameters =
            {
                 new SqlParameter("@ContactId", SqlDbType.Int, 4)
               ,new SqlParameter("@Salary", SqlDbType.Decimal)
               ,new SqlParameter("@Overtime", SqlDbType.Decimal)
               ,new SqlParameter("@Bonuses", SqlDbType.Decimal)
               ,new SqlParameter("@Commission", SqlDbType.Decimal)
               ,new SqlParameter("@Div_Int", SqlDbType.Decimal)
               ,new SqlParameter("@NetRent", SqlDbType.Decimal)
               ,new SqlParameter("@Other", SqlDbType.Decimal)
               ,new SqlParameter("@EmplId", SqlDbType.Int, 4)
            };

            parameters[0].Value = contactId; //IsBorrower ? postArgs.ContactId : postArgs.CoBorrowerContactId;
            parameters[1].Value = monthlySalary;
            parameters[2].Value = DBNull.Value;
            parameters[3].Value = DBNull.Value;
            parameters[4].Value = DBNull.Value;
            parameters[5].Value = DBNull.Value;
            parameters[6].Value = DBNull.Value;
            parameters[7].Value = DBNull.Value;
            parameters[8].Value = emplid;
            try
            {
                DbHelperSQL.ExecuteSql(sqlCommand, parameters);
            }
            catch (Exception e)
            {
                err = e.ToString();
                int Event_id = 8019;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
        }

        /// <summary>
        /// Gets the next loanofficer.
        /// </summary>
        /// <param name="leadSource">The lead source.</param>
        /// <param name="state">The state.</param>
        /// <param name="purpose">The purpose.</param>
        /// <param name="loanType">Type of the loan.</param>
        /// <param name="err">The err.</param>
        /// <returns>System.Int32.</returns>
        public int GetNextLoanofficer(string leadSource, string state, string purpose, string loanType, ref string err)
        {
            int userId = 0;
            string sqlCommand = @"lpsp_GetNextLoanofficer";
            SqlParameter[] parameters =
            {       new SqlParameter("@LeadSource", SqlDbType.NVarChar, 150)
                   ,new SqlParameter("@STATE", SqlDbType.NVarChar,2)
                   ,new SqlParameter("@Purpose", SqlDbType.NVarChar,255)
                   ,new SqlParameter("@LoanType", SqlDbType.NVarChar,255)
            };
            if (!string.IsNullOrEmpty(purpose) & purpose.ToUpper().Contains("REFI"))
                purpose = "refi";

            if (!string.IsNullOrEmpty(loanType) &&
                !loanType.ToUpper().Contains("VA") &&
                !loanType.ToUpper().Contains("FHA") &&
                !loanType.ToUpper().Contains("OTHER"))
                loanType = "conv";

            parameters[0].Value = !string.IsNullOrEmpty(leadSource) ? (object)leadSource : DBNull.Value;

            parameters[1].Value = !string.IsNullOrEmpty(state) ? (object)state : DBNull.Value;

            parameters[2].Value = !string.IsNullOrEmpty(purpose) ? (object)purpose : DBNull.Value;

            parameters[3].Value = !string.IsNullOrEmpty(loanType) ? (object)loanType : DBNull.Value;


            try
            {
                int rowAffected = 0;
                object returnValue = DbHelperSQL.RunProcedure(sqlCommand, parameters, out rowAffected);
                if (returnValue != null)
                    userId = Convert.ToInt32(returnValue);
            }
            catch (Exception exception)
            {
                err = exception.Message;
                int Event_id = 8015;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }

            return userId;
        }

        /// <summary>
        /// Gets the lead source id.
        /// </summary>
        /// <param name="leadSource">The lead source.</param>
        /// <param name="err">The err.</param>
        /// <returns>System.Int32.</returns>
        public int GetLOIDByLeadSource(string leadSource, ref string err)
        {
            int userId = 0;
            string sqlCommand = @"lpsp_GetLOIDByLeadSourceOrDefault";
            SqlParameter[] parameters =
            {   
                new SqlParameter("@LeadSource", SqlDbType.NVarChar, 150)
            };

            parameters[0].Value = !string.IsNullOrEmpty(leadSource) ? (object)leadSource : DBNull.Value;
            try
            {
                int rowAffected = 0;
                object returnValue = DbHelperSQL.RunProcedure(sqlCommand, parameters, out rowAffected);
                if (returnValue != null)
                    userId = Convert.ToInt32(returnValue);
            }
            catch (Exception exception)
            {
                err = exception.Message;
                int Event_id = 8015;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }

            return userId;
        }

        /// <summary>
        /// Assigns the loan officer.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <param name="err">The err.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool AssignLoanOfficer(LR_AssignLoanOfficerReq req, out string err)
        {
            var status = false;
            err = string.Empty;
            string sqlCommand = @"UPDATE dbo.UserLeadDist SET LeadsAssignedToday = ISNULL(LeadsAssignedToday,0) + 1,LastLeadAssigned=@FileId,LastAssigned=GETDATE() WHERE UserID = @UserId";
            SqlParameter[] parameters =
            {
                 new SqlParameter("@FileId", SqlDbType.Int, 4),
                 new SqlParameter("@UserId", SqlDbType.Int, 4)

            };

            parameters[0].Value = req.LoanId;
            parameters[1].Value = req.LoanOfficerId;

            try
            {
                DbHelperSQL.ExecuteSql(sqlCommand, parameters);
                status = true;
            }
            catch (Exception e)
            {
                err = e.ToString();
                status = false;
                int Event_id = 8019;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            return status;
        }

        /// <summary>
        /// Updates the loan with new loan offericer.
        /// </summary>
        /// <param name="loanId">The loan id.</param>
        /// <param name="loanOfficerId">The loan officer id.</param>
        /// <param name="err">The err.</param>
        private void UpdateLoan(int loanId, int loanOfficerId, ref string err)
        {
            err = string.Empty;
            int roleId = GetRoleId(UserRoles.UserRole_LoanOfficer, ref err);
            if(roleId <= 0)
                return;

            DataAccess.DataAccess da = new DataAccess.DataAccess();
            da.ReassignLoan(loanId, roleId, loanOfficerId, 0, ref err);
        }

        /// <summary>
        /// Updates the user leads dist.
        /// </summary>
        /// <param name="err">The err.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public bool UpdateUserLeadsDist(ref string err)
        {
            bool status = false;
            string sqlCommand = @"UPDATE dbo.UserLeadDist SET LeadsAssignedToday = 0";
            try
            {
                DbHelperSQL.ExecuteSql(sqlCommand);
                status = true;
            }
            catch (Exception e)
            {
                status = false;
                err = e.ToString();
                int Event_id = 8019;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }

            return status;
        }

        /// <summary>
        /// Gets the role id.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="err">The err.</param>
        /// <returns>System.Int32.</returns>
        private int GetRoleId(string roleName, ref string err)
        {
            int roleId = 0;
            err = string.Empty;
            if (string.IsNullOrEmpty(roleName))
            {
                err = "Role name is blank.";
                return roleId;
            }

            //DataAccess.DataAccess da = new DataAccess.DataAccess();

            string sqlCmd = string.Format("SELECT RoleId FROM dbo.Roles WHERE Name = '{0}'", roleName);
            object obj = focusIT.DbHelperSQL.GetSingle(sqlCmd);
            roleId = obj == null ? 0 : (int)obj;
            return roleId;
        }
    }
}
