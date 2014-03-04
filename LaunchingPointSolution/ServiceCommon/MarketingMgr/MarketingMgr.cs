using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Specialized;
using Common;
using focusIT;
using DataAccess;
using LP2.Service.Common;
using MarketingMgr.LeadStar;
using MarketingMgr;
using Utilities;

namespace MarketingManager
{
    public class LoanMarketingEvent
    {
        public int LoanMarketingEventId;
        public string Action; 
        public DateTime ExecutionDate; 
        public int LoanMarketingId;
        public string LeadStarEventId;
        public bool Completed; 
        public int WeekNo; 
        public string EventContent; 
        public string EventUrl; 
        public int FileId;  
    }
    
    public class MkgMgr : IMkgMgr
    {
        static DataAccess.DataAccess m_da = null;
        static int m_refCount = 0;
        static MkgMgr m_Instance;
        Utilities.SingleThreadedContext m_ThreadContext = null;
        public const string LeadStar_APIKey = "71A0C6DE-9B3A-4788-BB49-72117EEC51C8";
        public const string LeadStar_InstanceId = "My";
        public static string LeadStar_CompanyKey = "";
        bool SyncQueued = false;

        public static MkgMgr Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new MkgMgr();
 
                }
                lock (m_Instance)
                {
                    m_refCount++;
                }
                return m_Instance;
            }
        }

        private MkgMgr()
        {
            if (m_da == null)
                m_da = new DataAccess.DataAccess();

            if (m_Instance != null)
                return;

            if (m_ThreadContext == null)
            {
                m_ThreadContext = new Utilities.SingleThreadedContext();
                m_ThreadContext.ExceptionEvent += new ExceptionEventHandler(ThreadExceptionHandler);
                m_ThreadContext.Init("Marketing Manager");
            }
        }

        public void ThreadExceptionHandler(object sender, ExceptionEventArgs args)
        {
            Trace.TraceError(args.Exception.Message);
            EventLog.WriteEntry("InfoHub Service", args.Exception.Message, EventLogEntryType.Error);
        }

        public string FindExtension(string phoneNumber)
        {
            string Extension = string.Empty;
            string Exp = @"(?<phoneGroup>\d\d?\d?\d?)";
         
            int index = 0;

            index = phoneNumber.LastIndexOf("e");
            if (index >= 0)
            {
                Extension = phoneNumber.Substring(index);
                Match result = Regex.Match(Extension, Exp);
                if (result.Success)
                {
                    Extension = result.Value;
                }
            }
            
            return Extension;
        }

        public string FindNXX(string phoneNumber)
        {
            string NXX = string.Empty;
            string Exp = @"(?<phoneGroup>\d\d\d\-\d\d\d\d)";
            string Exp1 = @"(?<phoneGroup>\d\d\d\d)";
            Match result = Regex.Match(phoneNumber, Exp);
            if (result.Success)
            {
                NXX = result.Value;
            }
            Match result1 = Regex.Match(NXX, Exp1);
            if (result1.Success)
            {
                NXX = result1.Value;
            }

            return NXX;
        }

        public string FindNPA(string phoneNumber)
        {
            string NPA = string.Empty;
            string Exp = @"(?<phoneGroup>\d\d\d\-\d\d\d\d)";
            string Exp1 = @"(?<phoneGroup>\d\d\d\-)";
            Match result = Regex.Match(phoneNumber, Exp);
            if (result.Success)
            {
                NPA = result.Value;
            }
            Match result1 = Regex.Match(NPA, Exp1);
            if (result1.Success)
            {
                NPA = result1.Value;
            }

            if (NPA.Length >= 3)
            {
                NPA = NPA.Substring(0, 3);
            }

            return NPA;
        }

        public string FindAreaCode(string phoneNumber)
        {
            string Area_Code = string.Empty;
            string Exp =  @"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]";
            string Exp1 = @"(?<phoneGroup>\d\d\d)";
            Match result = Regex.Match(phoneNumber, Exp);
            if (result.Success)
            {
                Area_Code = result.Value;
            }
            Match result1 = Regex.Match(Area_Code, Exp1);
            if (result1.Success)
            {
                Area_Code = result1.Value;
            }
            return Area_Code;
        }

        private MarketingMgr.LeadStar.Company SetupCompanyInfo(ref string err)
        {
            err = "";
            MarketingMgr.LeadStar.Company companyInfo = null;
            string Phone = "";
            string Fax = "";     
            string cgSql = "select * from company_general";
            DataTable CompanyGeneral = DbHelperSQL.ExecuteDataTable(cgSql);

            if (CompanyGeneral == null || CompanyGeneral.Rows.Count == 0)
            {
                err = "Unable to find the company record.";
                return companyInfo;
            }
                        
            string strCompanyKey = CompanyGeneral.Rows[0]["GlobalId"] == DBNull.Value ? string.Empty : CompanyGeneral.Rows[0]["GlobalId"].ToString();
            string IntegrationID = CompanyGeneral.Rows[0]["IntegrationID"] == DBNull.Value ? string.Empty : CompanyGeneral.Rows[0]["IntegrationID"].ToString();
            string APIKey = CompanyGeneral.Rows[0]["APIKey"] == DBNull.Value ? string.Empty : CompanyGeneral.Rows[0]["APIKey"].ToString();
            string LeadStar_ID = CompanyGeneral.Rows[0]["LeadStar_ID"] == DBNull.Value ? string.Empty : CompanyGeneral.Rows[0]["LeadStar_ID"].ToString();
            bool EnableMarketing = CompanyGeneral.Rows[0]["EnableMarketing"] == DBNull.Value ? false : (bool)CompanyGeneral.Rows[0]["EnableMarketing"];

            if (!EnableMarketing && string.IsNullOrEmpty(LeadStar_ID))
                return companyInfo;
  
            APIKey = LeadStar_APIKey;
            if (!EnableMarketing && APIKey.ToUpper() == "INACTIVE")
                return companyInfo;
            else
                if (!EnableMarketing)
                    APIKey = "Inactive";

            string CompanyName = CompanyGeneral.Rows[0]["Name"] == DBNull.Value ? string.Empty : CompanyGeneral.Rows[0]["Name"].ToString();
            if (string.IsNullOrEmpty(CompanyName))
            {
                err = "Company Name is not specified.";
                return companyInfo;
            }

            companyInfo = new MarketingMgr.LeadStar.Company();

            companyInfo.Company_ID = LeadStar_ID;

            string Address1 = CompanyGeneral.Rows[0]["Address"] == DBNull.Value ? string.Empty : CompanyGeneral.Rows[0]["Address"].ToString();            
            string City = CompanyGeneral.Rows[0]["City"] == DBNull.Value ? string.Empty : CompanyGeneral.Rows[0]["City"].ToString();
            string State = CompanyGeneral.Rows[0]["State"] == DBNull.Value ? string.Empty : CompanyGeneral.Rows[0]["State"].ToString();
            string Zip = CompanyGeneral.Rows[0]["Zip"] == DBNull.Value ? string.Empty : CompanyGeneral.Rows[0]["Zip"].ToString();
            string Address2 = string.Empty;

            companyInfo.Company_Name = CompanyName;
            companyInfo.Company_Status = EnableMarketing ? "Active" : "Inactive";
            companyInfo.Address1 = Address1;
            companyInfo.Address2 = Address2;
            companyInfo.City = City;
            companyInfo.State = State;
            companyInfo.Zip = Zip;
            companyInfo.Integration_ID = IntegrationID;
            companyInfo.Company_Key = strCompanyKey;
            companyInfo.Phone_Area_Code = string.Empty;
            companyInfo.Phone_Prefix = string.Empty;
            companyInfo.Phone_Number = string.Empty;
            companyInfo.Phone_Extension = string.Empty;
            companyInfo.Fax_Area_Code = string.Empty;
            companyInfo.Fax_Prefix = string.Empty;
            companyInfo.Fax_Number = string.Empty;
            
            Phone = CompanyGeneral.Rows[0]["Phone"] == null ? string.Empty : CompanyGeneral.Rows[0]["Phone"].ToString();
            if (!string.IsNullOrEmpty(Phone))
            {
                companyInfo.Phone_Area_Code = FindAreaCode(Phone);
                companyInfo.Phone_Prefix = FindNPA(Phone);
                companyInfo.Phone_Number = FindNXX(Phone);
                companyInfo.Phone_Extension = FindExtension(Phone);
            }
            Fax = CompanyGeneral.Rows[0]["Fax"] == null ? string.Empty : CompanyGeneral.Rows[0]["Fax"].ToString();
            if (!string.IsNullOrEmpty(Fax))
            {
                companyInfo.Fax_Area_Code = FindAreaCode(Fax);
                companyInfo.Fax_Prefix = FindNPA(Fax);
                companyInfo.Fax_Number = FindNXX(Fax);
            }
            companyInfo.Company_Manager_ID = string.Empty;
            companyInfo.Company_Manager_Username = string.Empty;
            if (CompanyGeneral.Rows[0]["LeadStar_username"] == DBNull.Value)
            {
                companyInfo.Company_Manager_Username = null;
                return companyInfo;
            }

            companyInfo.Company_Manager_Username = CompanyGeneral.Rows[0]["LeadStar_username"].ToString();
            companyInfo.Company_Manager_ID = CompanyGeneral.Rows[0]["LeadStar_userid"] == DBNull.Value ? string.Empty : CompanyGeneral.Rows[0]["LeadStar_userid"].ToString();

            return companyInfo;
        }

        private void UpdateMarketingSettings(string ServiceUrl, string CampaignUrl)
        {
            if (!string.IsNullOrEmpty(CampaignUrl))
            {
                object obj = DbHelperSQL.GetSingle("Select top 1 CampaignDetailUrl from MarketingSettings");
                if (obj == null)
                {
                    string esql = string.Format("Insert into MarketingSettings (WebServiceURL, CampaignDetailURL, ReconcileInterval) values ('{0}','{1}', '{2}')",
                                        ServiceUrl, CampaignUrl, 24);
                    int status = DbHelperSQL.ExecuteSql(esql);
                }
                else
                {
                    string esql1 = string.Format("Update MarketingSettings set WebServiceURL='{0}', CampaignDetailURL='{1}'",
                                           ServiceUrl, CampaignUrl);
                    int status = DbHelperSQL.ExecuteSql(esql1);
                }
            }
        }
        private int[] GetFileIdsForAutoCampaigns(string loanType, string stageName, ref string err)
        {
            List<int> FileIds = new List<int>();
            err = string.Empty;
            bool logErr = false;
            switch (loanType)
            {
                case MarketingLoanType.Leads:
                case MarketingLoanType.Opportunities:
                    loanType = " AND Status = 'Prospect'"; break;
                case MarketingLoanType.ActiveLoans:
                    loanType = " AND Status ='Processing'"; break;
                case MarketingLoanType.ArchivedLoans:
                    loanType = " AND Status <> 'Processing' AND Status<> 'Prospect'"; break;
                default: return FileIds.ToArray();
            }
            try
            {
                string sqlCmd = string.Format("Select FileId, Completed from LoanStages ls inner join Loans l on ls.FileId=l.FileId where StageName='{0}' AND Completed IS NOT NULL {1} ", stageName, loanType);
                DataSet ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                    return FileIds.ToArray();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int fileId = (int)dr["FileId"];
                    DateTime CompletionDate = (DateTime)dr["Completed"];
                    TimeSpan ts = DateTime.Today - CompletionDate;
                    int NumDays = ts.Days;
                    if (NumDays > 1)
                        continue;
                    FileIds.Add(fileId);
                }
                return FileIds.ToArray();
            }
            catch (Exception ex)
            {
                err = "Failed to GetFileIdsForAutoCampaigns, Exception:" + ex.Message;
                logErr = true;
                return FileIds.ToArray();
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }

        private List<AutoCampaigns> GetAutoCampaigns(ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            DataSet ds = null;
            List<AutoCampaigns> AutoCampaignList = null;
            try
            {
                string sqlCmd = "select ac.*, ts.Name as StageName from AutoCampaigns ac left join Template_Stages ts on ac.TemplStageId=ts.TemplStageId Where Enabled=1";
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, "No Auto Campaign records to process.");
                    return AutoCampaignList;
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr == null || dr["SelectedBy"] == DBNull.Value || dr["StageName"] == DBNull.Value 
                        || dr["LoanType"] == DBNull.Value)
                        continue;
                    string loanType = dr["LoanType"] == DBNull.Value ? string.Empty : dr["LoanType"].ToString();
                    string StageName = dr["StageName"].ToString();
                    int[] FileIds = GetFileIdsForAutoCampaigns(loanType, StageName, ref err);
                    if (FileIds.Length <= 0)
                        continue;
                    AutoCampaigns ac = new AutoCampaigns()
                    {
                        CampaignId = (int)dr["CampaignId"],
                        PaymentType = (MarketingPaymentType) (short)dr["PaidBy"],
                        Enabled = true,
                        SelectedBy = (int)dr["SelectedBy"] ,
                        LoanType = dr["LoanType"].ToString(),
                        TemplStageId = (int)dr["TemplStageid"],
                        TemplStageName = dr["StageName"].ToString(),
                        FileIds = FileIds
                    };

                    AutoCampaignList.Add(ac);
                }

                return AutoCampaignList;
            }
            catch (Exception ex)
            {
                err = "Failed to GetAutoCampaigns, Exception:" + ex.Message;
                logErr = true;
                return AutoCampaignList;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }

        }

        private void StartLoanAutoCampaign (object obj)
        {
            string err = string.Empty;
            bool logErr = false;
            try
            {
                StartCampaignRequest req = (StartCampaignRequest)obj;
                if (req == null)
                {
                    err = "StartLoanAutoCampaign, Failed to instantiate StartCampaignRequest from object.";
                    logErr = false;
                    return;
                }
                StartCampaignResponse resp = null;
                resp = StartCampaign(req, ref err);
                if (!resp.hdr.Successful)
                {
                    err = string.Format("StartLoanAutoCampaign, failed with ErrorCode {0}, Status Detail: {1}, Error Msg: {2}.", resp.ErrorCode, resp.hdr.StatusInfo, err);
                    logErr = false;    
                }
            }
            catch (Exception ee)
            {
                err = "StartLoanAutoCampaign, failed with Exception: " + ee.Message + "\n" + ee.StackTrace;
                logErr = false;
            }
            finally
            {
                if (logErr)
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
            }

        }

        public bool StartAutoCampaigns(StartCampaignRequest req, ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            DataSet ds = null;
            try
            {
                //List<AutoCampaigns> campaignList = GetAutoCampaigns(ref err);
                //if (campaignList == null || campaignList.Count <= 0)
                //    return true;
                //foreach (AutoCampaigns ac in campaignList)
                //{
                //    if (ac == null || ac.FileIds.Length <= 0)
                //        continue;
                //    if (ac.FileIds == null || ac.FileIds.Length <= 0)
                //        continue;
                //    foreach (int FileId in ac.FileIds)
                //        if (FileId > 0)
                //            StartLoanAutoCampaign(FileId, ac.CampaignId, ac.SelectedBy, ac.PaymentType, ref err);
                //}
                m_ThreadContext.Post(StartLoanAutoCampaign, req);
                return true;
            }
            catch (Exception ex)
            {
                err = "Failed to update Leadstar Company, Exception:" + ex.Message;
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }

        public bool UpdateCompany(ref string err)
        {
            err = string.Empty;
            string err1 = string.Empty;
            bool logErr = false;
            int Event_id = 6010;
            bool ShouldUpdateCompany = false;
            try
            {
                string LeadStarId = string.Empty;
                MarketingMgr.LeadStar.Company lsCompany = SetupCompanyInfo(ref err);
                if (lsCompany == null)             // if there is no company info, we don't have to send
                {
                    if (string.IsNullOrEmpty(err))  // no error meaning we don't have to send
                        return true;
                    logErr = true;
                    return false;
                }
                string result = string.Empty;
                using (LeadStarServiceClient LS = new LeadStarServiceClient())
                {
                    string instance_ID = "MY";
                    // this company hasn't been sent because there is no IntegrationID in the DB
                    if ( string.IsNullOrEmpty(lsCompany.Integration_ID) || 
                         string.IsNullOrEmpty(lsCompany.Company_Key) ||
                         string.IsNullOrEmpty(lsCompany.Company_ID) ||
                         (lsCompany.Company_ID.Length < 32) )
                    {
                        string strCompanyKey = "";
                        if (string.IsNullOrEmpty(lsCompany.Company_Key) ||
                             (lsCompany.Company_Key.Length < 32))
                        {
                            strCompanyKey = Guid.NewGuid().ToString();
                            lsCompany.Company_Key = strCompanyKey;
                        }

                        lsCompany.Integration_ID = lsCompany.Company_Key.Replace("-", "_");
                        lsCompany.Company_ID = string.Empty;

                        lsCompany.Company_Manager_Username = "";
                        err = LS.InsertCompany(instance_ID, lsCompany, LeadStar_APIKey);

                        err1 = string.Format("  [Message detail]: " +
                                             " company.Address1 = '{0}'," +
                                             " company.Address2 = '{1}'," +
                                             " company.City = '{2}'," +
                                             " company.Company_ID = '{3}'," +
                                             " company.Company_Key = '{4}'," +
                                             " company.Company_Manager_ID = '{5}'," +
                                             " company.Company_Manager_Username = '{6}'," +
                                             " company.Company_Name = '{7}'," +
                                             " company.Company_Status = '{8}'," +
                                             " company.Fax_Area_Code = '{9}'," +
                                             " company.Fax_Prefix = '{10}'," +
                                             " company.Fax_Number = '{11}'," +
                                             " company.Integration_ID = '{12}'," +
                                             " company.Phone_Area_Code = '{13}'," +
                                             " company.Phone_Prefix = '{14}'," +
                                             " company.Phone_Number = '{15}'," +
                                             " company.State = '{16}'," +
                                             " company.Zip = '{17}'",                                          
                                             lsCompany.Address1,
                                             lsCompany.Address2,
                                             lsCompany.City,
                                             lsCompany.Company_ID,
                                             lsCompany.Company_Key,
                                             lsCompany.Company_Manager_ID,
                                             lsCompany.Company_Manager_Username,
                                             lsCompany.Company_Name,
                                             lsCompany.Company_Status,
                                             lsCompany.Fax_Area_Code,
                                             lsCompany.Fax_Prefix,
                                             lsCompany.Fax_Number,
                                             lsCompany.Integration_ID,
                                             lsCompany.Phone_Area_Code,
                                             lsCompany.Phone_Prefix,
                                             lsCompany.Phone_Number,
                                             lsCompany.State,
                                             lsCompany.Zip);                                            

                        switch (err.ToLower())
                        {
                            case "1000":
                                err = "";
                                logErr = false;
                                Event_id = 6010;
                                lsCompany.Company_Status = "Active";
                                string cgSql =
                                string.Format("update company_general set GlobalId='{0}', IntegrationID='{1}', APIKey='{2}', LeadStar_ID='{3}'",
                                          lsCompany.Company_Key
                                        , lsCompany.Integration_ID
                                        , lsCompany.Company_Status 
                                        , lsCompany.Company_ID );
                                int st = DbHelperSQL.ExecuteSql(cgSql);                                
                                break;

                            case "1201":
                                logErr = true;
                                Event_id = 6011;
                                err = "Leadstar.InsertCompany Fails(1201): Empty IntegrationID";                                
                                return false;

                            case "1202":
                                logErr = true;
                                Event_id = 6012;
                                err = "Leadstar.InsertCompany Fails(1202): Bad Instance ID";
                                return false;

                            case "1203":
                                logErr = true;
                                Event_id = 6013;
                                err = "Leadstar.InsertCompany Fails(1203): Bad API_Key";
                                return false;

                            case "1204":
                                logErr = true;
                                Event_id = 6014;
                                err = "Leadstar.InsertCompany Fails(1204): Company Manager Username Not Found";
                                return false;

                            case "1205":
                                logErr = true;
                                Event_id = 6015;
                                err = "Leadstar.InsertCompany Fails(1205): Bad Company Key";
                                return false;

                            case "1206":
                                logErr = true;
                                Event_id = 6016;
                                err = "Leadstar.InsertCompany Fails(1206): Integration_ID must be unique across company";
                                return false;

                            case "1207":
                                logErr = true;
                                Event_id = 6017;
                                err = "Leadstar.InsertCompany Fails(1207): Company Name and Integration ID are both required";
                                return false;

                            case "1208":
                                logErr = true;
                                Event_id = 6018;
                                err = "Leadstar.InsertCompany Fails(1208): Company Key Must Be Unique Within System";
                                break;

                            case "1209":
                                logErr = true;
                                Event_id = 6019;
                                err = "Leadstar.InsertCompany Fails(1209): Company Key Must Be Valid GUID";
                                return false;

                            default:
                                logErr = true;
                                Event_id = 6019;
                                err = "Leadstar.InsertCompany Fails: " + err;
                                return false;
                        }
                        ShouldUpdateCompany = false;
                    }
                    else
                        ShouldUpdateCompany = true;

                    if (string.IsNullOrEmpty(LeadStarId))
                        LeadStarId = LeadStar_APIKey;

                    Company company = LS.GetCompanyByIntegrationID(instance_ID, lsCompany.Integration_ID, LeadStarId, lsCompany.Company_Key);
                    if (company == null)
                    {
                        if (ShouldUpdateCompany == true)
                        {
                            err = "Failed to Leadstar.GetCompanyByIntegrationID from Leadstar.";
                            Event_id = 6029;
                            logErr = true;
                            return false;
                        }
                        else
                        {
                            logErr = false;
                            return true;
                        }
                    }

                    if (lsCompany.Company_Status.Trim().ToLower() == "active")
                    {
                        string ServiceUrl = LS.Endpoint.Address.Uri.ToString();
                        string CampaignUrl = LS.GetBaseURL(instance_ID, LeadStarId);
                        UpdateMarketingSettings(ServiceUrl, CampaignUrl);
                    }

                    lsCompany.Company_ID = company.Company_ID;

                    if (ShouldUpdateCompany == false)
                    {
                        err = "";
                        logErr = false;
                        Event_id = 6010;
                        lsCompany.Company_Status = "Active";
                        string cgSql1 =
                        string.Format("update company_general set GlobalId='{0}', IntegrationID='{1}', APIKey='{2}', LeadStar_ID='{3}'",
                                  lsCompany.Company_Key
                                , lsCompany.Integration_ID
                                , lsCompany.Company_Status
                                , lsCompany.Company_ID);
                        int st1 = DbHelperSQL.ExecuteSql(cgSql1);
                        return true;
                    }

                    if (ShouldUpdateCompany)
                    {
                        company.Address1 = lsCompany.Address1;
                        company.Address2 = lsCompany.Address2;
                        company.City = lsCompany.City;
                        company.Company_Manager_ID = lsCompany.Company_Manager_ID;
                        company.Company_Manager_Username = lsCompany.Company_Manager_Username;
                        company.Company_Status = lsCompany.Company_Status;
                        company.Phone_Area_Code = lsCompany.Phone_Area_Code;
                        company.Phone_Prefix = lsCompany.Phone_Prefix;
                        company.Phone_Number = lsCompany.Phone_Number;
                        company.Fax_Area_Code = lsCompany.Fax_Area_Code;
                        company.Fax_Prefix = lsCompany.Fax_Prefix;
                        company.Fax_Number = lsCompany.Fax_Number;

                        if ((lsCompany.Company_Manager_ID != string.Empty) &&
                            (lsCompany.Company_Manager_Username != string.Empty))
                        {
                              company.Company_Manager_ID = lsCompany.Company_Manager_ID;
                              company.Company_Manager_Username = lsCompany.Company_Manager_Username;
                        }                  

                        company.Company_Status = lsCompany.Company_Status;

                        err = LS.UpdateCompany(instance_ID, company, LeadStarId);                        
                        
                        err1 = string.Format("  [Message detail]: " +
                                            " company.Address1 = '{0}'," +
                                            " company.Address2 = '{1}'," +
                                            " company.City = '{2}'," +
                                            " company.Company_ID = '{3}'," +
                                            " company.Company_Key = '{4}'," +
                                            " company.Company_Manager_ID = '{5}'," +
                                            " company.Company_Manager_Username = '{6}'," +
                                            " company.Company_Name = '{7}'," +
                                            " company.Company_Status = '{8}'," +
                                            " company.Fax_Area_Code = '{9}'," +
                                            " company.Fax_Prefix = '{10}'," +
                                            " company.Fax_Number = '{11}'," +
                                            " company.Integration_ID = '{12}'," +
                                            " company.Phone_Area_Code = '{13}'," +
                                            " company.Phone_Prefix = '{14}'," +
                                            " company.Phone_Number = '{15}'," +
                                            " company.State = '{16}'," +
                                            " company.Zip = '{17}'",
                                            company.Address1,
                                            company.Address2,
                                            company.City,
                                            company.Company_ID,
                                            company.Company_Key,
                                            company.Company_Manager_ID,
                                            company.Company_Manager_Username,
                                            company.Company_Name,
                                            company.Company_Status,
                                            company.Fax_Area_Code,
                                            company.Fax_Prefix,
                                            company.Fax_Number,
                                            company.Integration_ID,
                                            company.Phone_Area_Code,
                                            company.Phone_Prefix,
                                            company.Phone_Number,
                                            company.State,
                                            company.Zip);

                        switch (err.ToLower())
                        {
                            case "1000":
                                err = "";
                                Event_id = 6020;
                                logErr = false;
                                company.Company_Status = "Active";
                                string ucgSql =
                                string.Format("update company_general set GlobalId='{0}', IntegrationID='{1}', APIKey='{2}', LeadStar_ID='{3}'",
                                          company.Company_Key
                                        , company.Integration_ID
                                        , company.Company_Status 
                                        , company.Company_ID);

                                int sta = DbHelperSQL.ExecuteSql(ucgSql);
                                break;

                            case "1201":
                                logErr = true;
                                Event_id = 6021;
                                err = "Leadstar.UpdateCompany Fails(1201): Empty IntegrationID";
                                return false;

                            case "1202":
                                logErr = true;
                                Event_id = 6022;
                                err = "Leadstar.UpdateCompany Fails(1202): Bad Instance ID";
                                return false;

                            case "1203":
                                logErr = true;
                                Event_id = 6023;
                                err = "Leadstar.UpdateCompany Fails(1203): Bad API_Key";
                                return false;

                            case "1204":
                                logErr = true;
                                Event_id = 6024;
                                err = "Leadstar.UpdateCompany Fails(1204): Username not Found";
                                return false;

                            case "1205":
                                logErr = true;
                                Event_id = 6025;
                                err = "Leadstar.UpdateCompany Fails(1205): Bad Company Key";
                                return false;

                            case "1210":
                                logErr = true;
                                Event_id = 6026;
                                err = "Leadstar.UpdateCompany Fails(1210): Integration_ID not found or bad Company Key";
                                return false;

                            default:
                                logErr = true;
                                Event_id = 6027;
                                err = "Leadstar.UpdateCompany Fails: " + err;
                                return false;
                        }
                    return true;
                    } 
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Failed to update Leadstar Company, Exception:" + ex.Message;
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    err = err + err1;
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id);
                }
                else
                {
                    err = err1;
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id);
                }
            }
        }

        //public UpdateRegionResponse UpdateRegion(UpdateRegionRequest req, ref string err)
        //{
        //    err = string.Empty;
        //    UpdateRegionResponse resp = new UpdateRegionResponse();
        //    resp.hdr = new RespHdr();
        //    resp.hdr.Successful = true;
        //    resp.hdr.StatusInfo = "";
        //    return resp;
        //}

        private bool MarketingEnabled ()
        {
            string cSql = "Select top 1 EnableMarketing from Company_General";
            object obj = DbHelperSQL.GetSingle(cSql);
            if (obj == null)
                return false;
            return (bool)obj;
        }

        private void GetCompanyLeadStarInfo (ref bool MarketingEnabled, ref string LeadStarId, ref string CompanyKey, ref string IntegrationId, ref string APIKey)
        {
            MarketingEnabled = false;
            LeadStarId = string.Empty;
            CompanyKey = string.Empty;
            IntegrationId = string.Empty;

            string cgSql = "select * from company_general";
            DataTable CompanyGeneral = DbHelperSQL.ExecuteDataTable(cgSql);

            if (CompanyGeneral.Rows.Count == 0)
                return;
            APIKey = CompanyGeneral.Rows[0]["APIKey"] == DBNull.Value ? string.Empty : CompanyGeneral.Rows[0]["APIKey"].ToString();
            MarketingEnabled = CompanyGeneral.Rows[0]["EnableMarketing"] == DBNull.Value ? false : (bool)CompanyGeneral.Rows[0]["EnableMarketing"];

            if (!MarketingEnabled)
                return;
            CompanyKey = CompanyGeneral.Rows[0]["GlobalId"].ToString();
            IntegrationId = CompanyGeneral.Rows[0]["IntegrationID"].ToString();           

            if (string.IsNullOrEmpty(APIKey))
            {
                APIKey = LeadStar_APIKey;
            }
            LeadStarId = CompanyGeneral.Rows[0]["LeadStar_ID"] == DBNull.Value ? string.Empty : CompanyGeneral.Rows[0]["LeadStar_ID"].ToString();
        }
        #region Update LeadStar Methods

        private bool SetupBranch(int BranchId, ref MarketingMgr.LeadStar.Branch lsBranch, string CompanyKey, string IntegrationId, string LeadStarId, ref string err)
        {
            err = "";
          
            string brSql = "select * from Branches where BranchId=" + BranchId;
            DataTable Branches = DbHelperSQL.ExecuteDataTable(brSql);

            if (Branches == null || Branches.Rows.Count == 0)
            {
                err = "Unable to find the branch record.";
                return false;
            }

            if (lsBranch == null)
                lsBranch = new MarketingMgr.LeadStar.Branch();

            if (Branches.Rows[0]["Name"] == DBNull.Value)
            {
                err = "Branch Name is blank.";
                return false;
            }
            lsBranch.Integration_ID = Branches.Rows[0]["GlobalId"].ToString();
            if (lsBranch.Integration_ID.Length <= IntegrationId.Length)
                lsBranch.Integration_ID = "";
            lsBranch.Company_Key = CompanyKey;
            lsBranch.Company_ID = LeadStarId;
            lsBranch.Branch_Name = Branches.Rows[0]["Name"].ToString();
            bool Enabled = Branches.Rows[0]["Enabled"] == DBNull.Value ? false : (bool)Branches.Rows[0]["Enabled"];
            if (Enabled)
                lsBranch.Branch_Status = "Active";
            else
                lsBranch.Branch_Status = "Inactive";
 
            lsBranch.Address1 = Branches.Rows[0]["BranchAddress"] == DBNull.Value ? string.Empty : Branches.Rows[0]["BranchAddress"].ToString();

            lsBranch.City = Branches.Rows[0]["City"] == DBNull.Value ? string.Empty : Branches.Rows[0]["City"].ToString();
            lsBranch.State = Branches.Rows[0]["BranchState"] == DBNull.Value ? string.Empty : Branches.Rows[0]["BranchState"].ToString();
            lsBranch.Zip = Branches.Rows[0]["Zip"] == DBNull.Value ? string.Empty : Branches.Rows[0]["Zip"].ToString();
            lsBranch.Address2 = string.Empty;
            lsBranch.Phone_Area_Code = string.Empty;
            lsBranch.Phone_Prefix = string.Empty;
            lsBranch.Phone_Number = string.Empty;
            lsBranch.Phone_Extension = string.Empty;

            string Phone = Branches.Rows[0]["Phone"] == DBNull.Value ? string.Empty : Branches.Rows[0]["Phone"].ToString();

            if (!string.IsNullOrEmpty(Phone))
            {
                lsBranch.Phone_Area_Code = FindAreaCode(Phone);
                lsBranch.Phone_Prefix = FindNPA(Phone);
                lsBranch.Phone_Number = FindNXX(Phone);
                lsBranch.Phone_Extension = FindExtension(Phone);
            }

            lsBranch.Fax_Area_Code = string.Empty;
            lsBranch.Fax_Prefix = string.Empty;
            lsBranch.Fax_Number = string.Empty;

            string Fax = Branches.Rows[0]["Fax"] == DBNull.Value ? string.Empty : Branches.Rows[0]["Fax"].ToString();

            if (!string.IsNullOrEmpty(Fax))
            {
                lsBranch.Fax_Area_Code = FindAreaCode(Fax);
                lsBranch.Fax_Prefix = FindNPA(Fax);
                lsBranch.Fax_Number = FindNXX(Fax);
            }

            lsBranch.Branch_License_1 = Branches.Rows[0]["License1"] == DBNull.Value ? string.Empty : Branches.Rows[0]["License1"].ToString();
            lsBranch.Branch_License_2 = Branches.Rows[0]["License2"] == DBNull.Value ? string.Empty : Branches.Rows[0]["License2"].ToString();
            lsBranch.Branch_License_3 = Branches.Rows[0]["License3"] == DBNull.Value ? string.Empty : Branches.Rows[0]["License3"].ToString();
            lsBranch.Branch_License_4 = Branches.Rows[0]["License4"] == DBNull.Value ? string.Empty : Branches.Rows[0]["License4"].ToString();
            lsBranch.Branch_License_5 = Branches.Rows[0]["License5"] == DBNull.Value ? string.Empty : Branches.Rows[0]["License5"].ToString();

            lsBranch.Disclaimer = Branches.Rows[0]["Disclaimer"] == DBNull.Value ? string.Empty : Branches.Rows[0]["Disclaimer"].ToString();

            lsBranch.Branch_Manager_ID = string.Empty;
            lsBranch.Branch_Manager_Username = string.Empty;

            if (Branches.Rows[0]["Leadstar_Username"] == DBNull.Value)
                return true;

            lsBranch.Branch_Manager_Username = Branches.Rows[0]["Leadstar_Username"].ToString();
            lsBranch.Branch_Manager_ID = Branches.Rows[0]["Leadstar_Userid"] == DBNull.Value ? string.Empty : Branches.Rows[0]["Leadstar_Userid"].ToString();  

            return true;
        }

        public bool UpdateBranch(int BranchId, ref string err)
        {
           err = string.Empty;
           string err1 = string.Empty;
           int Event_id = 6030;
           bool logErr = false;
           bool ShouldUpdateBranch = false;
           try
           {      
               string instance_ID = "MY";
               string LeadStarId = string.Empty;
               string APIKey = string.Empty;
               string Company_IntegrationID = string.Empty;
               string strCompanyKey = string.Empty;
               MarketingMgr.LeadStar.Branch lsBranch = null;
               MarketingMgr.LeadStar.Company lsCompany = null;
               lsCompany = SetupCompanyInfo(ref err);

               if (lsCompany == null)
               {
                   if (!string.IsNullOrEmpty(err))
                   {
                       logErr = true;
                       return false;
                   }
                   return true;
               }

               if (SetupBranch(BranchId, ref lsBranch, lsCompany.Company_Key, lsCompany.Integration_ID, lsCompany.Company_ID, ref err) == false)  
               {
                       logErr = true;
                       return false;
               }

               if (lsBranch == null)
               {
                   err = string.Format("Cannot get Branch information, BranchId={0}.", BranchId);
                   logErr = true;
                   return false;
               }

               if (string.IsNullOrEmpty(APIKey))
               APIKey = LeadStar_APIKey;

               strCompanyKey = lsCompany.Company_Key;
               Company_IntegrationID = lsCompany.Integration_ID;

               using (LeadStarServiceClient LS = new LeadStarServiceClient())
               {
                   if (string.IsNullOrEmpty(lsBranch.Integration_ID))                      
                   {
                       lsBranch.Integration_ID = Company_IntegrationID + "_b" + BranchId.ToString();
                      
                       err = LS.InsertBranch(instance_ID, lsBranch, APIKey);

                       err1 = string.Format("  [Message detail]: " +                                            
                                            " branch.Address1 = '{0}'," +
                                            " branch.Address2 = '{1}'," +
                                            " branch.Branch_ID = '{2}'," +
                                            " branch.Branch_License_1 = '{3}'," +
                                            " branch.Branch_License_2 = '{4}'," +
                                            " branch.Branch_License_3 = '{5}'," +
                                            " branch.Branch_License_4 = '{6}'," +
                                            " branch.Branch_License_5 = '{7}'," +
                                            " branch.Branch_Manager_ID = '{8}'," +
                                            " branch.Branch_Manager_Username = '{9}'," +
                                            " branch.Branch_Name = '{10}'," +                                            
                                            " branch.Branch_Status = '{11}'," +
                                            " branch.City = '{12}'," +
                                            " branch.Company_ID = '{13}'," +
                                            " branch.Company_Key = '{14}'," +
                                            " branch.Disclaimer = '{15}'," +
                                            " branch.Fax_Area_Code = '{16}'," +
                                            " branch.Fax_Number = '{17}'," +
                                            " branch.Fax_Prefix = '{18}'," +
                                            " branch.Integration_ID = '{19}'," +
                                            " branch.Phone_Area_Code = '{20}'," +
                                            " branch.Phone_Extension = '{21}'," +
                                            " branch.Phone_Number = '{22}'," +
                                            " branch.Phone_Prefix = '{23}'," +
                                            " branch.State = '{24}'," +
                                            " branch.Zip = '{25}' ",
                                            lsBranch.Address1,
                                            lsBranch.Address2,
                                            lsBranch.Branch_ID,
                                            lsBranch.Branch_License_1,
                                            lsBranch.Branch_License_2,
                                            lsBranch.Branch_License_3,
                                            lsBranch.Branch_License_4,
                                            lsBranch.Branch_License_5,
                                            lsBranch.Branch_Manager_ID,
                                            lsBranch.Branch_Manager_Username,
                                            lsBranch.Branch_Name,
                                            lsBranch.Branch_Status,
                                            lsBranch.City,
                                            lsBranch.Company_ID,
                                            lsBranch.Company_Key,
                                            lsBranch.Disclaimer,
                                            lsBranch.Fax_Area_Code,
                                            lsBranch.Fax_Number,
                                            lsBranch.Fax_Prefix,
                                            lsBranch.Integration_ID,
                                            lsBranch.Phone_Area_Code,
                                            lsBranch.Phone_Extension,
                                            lsBranch.Phone_Number,
                                            lsBranch.Phone_Prefix,
                                            lsBranch.State,                                           
                                            lsBranch.Zip);

                       switch (err.ToLower())
                       {
                           case "1000":
                               err = "";
                               logErr = true;
                               Event_id = 6030;
                               string ucgSql = "update Branches set GlobalId='" + lsBranch.Integration_ID + "' where BranchId=" + BranchId;
                               int status = DbHelperSQL.ExecuteSql(ucgSql);
                               break;

                           case "1201":
                               logErr = true;
                               Event_id = 6031;
                               err = "Leadstar.InsertBranch Fails(1201): Empty IntegrationID";
                               return false;

                           case "1202":
                               logErr = true;
                               Event_id = 6032;
                               err = "Leadstar.InsertBranch Fails(1202): Bad Instance ID";
                               return false;

                           case "1203":
                               logErr = true;
                               Event_id = 6033;
                               err = "Leadstar.InsertBranch Fails(1203): Bad API_Key";
                               return false;

                           case "1204":
                               logErr = true;
                               Event_id = 6034;
                               err = "Leadstar.InsertBranch Fails(1204): Company Manager Username Not Found";
                               return false;

                           case "1205":
                               logErr = true;
                               Event_id = 6035;
                               err = "Leadstar.InsertBranch Fails(1205): Bad Company Key";
                               return false;

                           case "1206":
                               logErr = true;
                               Event_id = 6036;
                               err = "Leadstar.InsertBranch Fails(1206): Branch Integration_ID must be unique across company";
                               return false;

                           case "1207":
                               logErr = true;
                               Event_id = 6037;
                               err = "Leadstar.InsertBranch Fails(1207): Company Name and Integration ID are both required";
                               return false;

                           case "1208":
                               logErr = true;
                               Event_id = 6038;
                               err = "Leadstar.InsertBranch Fails(1208): Company Key Must Be Unique Within System";
                               return false;

                           case "1209":
                               logErr = true;
                               Event_id = 6039;
                               err = "Leadstar.InsertBranch Fails(1209): Company Key Must Be Valid GUID";
                               return false;

                           case "1211":
                               logErr = true;
                               Event_id = 6039;
                               err = "Leadstar.InsertBranch Fails(1211): Company Not Found";
                               return false;

                           case "1212":
                               logErr = true;
                               Event_id = 6039;
                               err = "Leadstar.InsertBranch Fails(1212): Leadstar Internal Error";
                               return false;

                           default:
                               logErr = true;
                               Event_id = 6039;
                               err = "Leadstar.InsertBranch Fails: " + err;
                               return false;
                       }
                       ShouldUpdateBranch = false;
                   } 
                   else
                       ShouldUpdateBranch = true;

                    Branch branch = LS.GetBranchByIntegrationID("MY", lsBranch.Integration_ID, APIKey, strCompanyKey);
                    if (branch == null)
                    {
                        if (ShouldUpdateBranch == true)
                        {
                            logErr = true;
                            Event_id = 6049;
                            err = "LeadStar GetBranchByIntegrationID cannot find the branch record.";
                            return false;
                        }
                        else
                        {
                            logErr = false;
                            return true;
                        }
                    }

                    string sqlcmd = string.Format("Update Branches set LeadStar_ID='{0}' where BranchId={1}", branch.Branch_ID, BranchId);
                    DbHelperSQL.ExecuteSql(sqlcmd);

                    if (!ShouldUpdateBranch)
                    {
                        return true;
                    }

                    branch.Branch_Name = lsBranch.Branch_Name;
                    branch.Branch_Status = lsBranch.Branch_Status;
                    branch.Address1 = lsBranch.Address1;
                    branch.Address2 = lsBranch.Address2;
                    branch.City = lsBranch.City;
                    branch.State = lsBranch.State;
                    branch.Zip = lsBranch.Zip;
                    branch.Branch_License_1 = lsBranch.Branch_License_1;
                    branch.Branch_License_2 = lsBranch.Branch_License_2;
                    branch.Branch_License_3 = lsBranch.Branch_License_3;
                    branch.Branch_License_4 = lsBranch.Branch_License_4;
                    branch.Branch_License_5 = lsBranch.Branch_License_5;

                    if ((lsBranch.Branch_Manager_ID != string.Empty) &&
                        (lsBranch.Branch_Manager_Username != string.Empty))
                    {
                        branch.Branch_Manager_ID = lsBranch.Branch_Manager_ID;
                        branch.Branch_Manager_Username = lsBranch.Branch_Manager_Username;   
                    } 

                    branch.Phone_Area_Code = lsBranch.Phone_Area_Code;
                    branch.Phone_Prefix = lsBranch.Phone_Prefix;
                    branch.Phone_Number = lsBranch.Phone_Number;
                    branch.Phone_Extension = lsBranch.Phone_Extension;
                    branch.Fax_Area_Code = lsBranch.Fax_Area_Code;
                    branch.Fax_Prefix = lsBranch.Fax_Prefix;
                    branch.Fax_Number = lsBranch.Fax_Number;

                    err = LS.UpdateBranch(instance_ID, branch, APIKey);

                    err1 = string.Format("  [Message detail]: " +
                                             " branch.Address1 = '{0}'," +
                                             " branch.Address2 = '{1}'," +
                                             " branch.Branch_ID = '{2}'," +
                                             " branch.Branch_License_1 = '{3}'," +
                                             " branch.Branch_License_2 = '{4}'," +
                                             " branch.Branch_License_3 = '{5}'," +
                                             " branch.Branch_License_4 = '{6}'," +
                                             " branch.Branch_License_5 = '{7}'," +
                                             " branch.Branch_Manager_ID = '{8}'," +
                                             " branch.Branch_Manager_Username = '{9}'," +
                                             " branch.Branch_Name = '{10}'," +
                                             " branch.Branch_Status = '{11}'," +
                                             " branch.City = '{12}'," +
                                             " branch.Company_ID = '{13}'," +
                                             " branch.Company_Key = '{14}'," +
                                             " branch.Disclaimer = '{15}'," +
                                             " branch.Fax_Area_Code = '{16}'," +
                                             " branch.Fax_Number = '{17}'," +
                                             " branch.Fax_Prefix = '{18}'," +
                                             " branch.Integration_ID = '{19}'," +
                                             " branch.Phone_Area_Code = '{20}'," +
                                             " branch.Phone_Extension = '{21}'," +
                                             " branch.Phone_Number = '{22}'," +
                                             " branch.Phone_Prefix = '{23}'," +
                                             " branch.State = '{24}'," +
                                             " branch.Zip = '{25}' ",
                                             branch.Address1,
                                             branch.Address2,
                                             branch.Branch_ID,
                                             branch.Branch_License_1,
                                             branch.Branch_License_2,
                                             branch.Branch_License_3,
                                             branch.Branch_License_4,
                                             branch.Branch_License_5,
                                             branch.Branch_Manager_ID,
                                             branch.Branch_Manager_Username,
                                             branch.Branch_Name,
                                             branch.Branch_Status,
                                             branch.City,
                                             branch.Company_ID,
                                             branch.Company_Key,
                                             branch.Disclaimer,
                                             branch.Fax_Area_Code,
                                             branch.Fax_Number,
                                             branch.Fax_Prefix,
                                             branch.Integration_ID,
                                             branch.Phone_Area_Code,
                                             branch.Phone_Extension,
                                             branch.Phone_Number,
                                             branch.Phone_Prefix,
                                             branch.State,
                                             branch.Zip);

                    switch (err.ToLower())
                    {
                        case "1000":
                            err = "";
                            Event_id = 6040;
                            break;

                        case "1201":
                            logErr = true;
                            Event_id = 6041;
                            err = "Leadstar.UpdateBranch Fails(1201): Empty IntegrationID";
                            return false;

                        case "1202":
                            logErr = true;
                            Event_id = 6042;
                            err = "Leadstar.UpdateBranch Fails(1202): Bad Instance ID";
                            return false;

                        case "1203":
                            logErr = true;
                            Event_id = 6043;
                            err = "Leadstar.UpdateBranch Fails(1203): Bad API_Key";
                            return false;

                        case "1204":
                            logErr = true;
                            Event_id = 6044;
                            err = "Leadstar.UpdateBranch Fails(1204): Username not Found";
                            return false;

                        case "1205":
                            logErr = true;
                            Event_id = 6045;
                            err = "Leadstar.UpdateBranch Fails(1205): Bad Company Key";
                            return false;

                        case "1209":
                            logErr = true;
                            Event_id = 6046;
                            err = "Leadstar.UpdateBranch Fails(1209): Company Key Must Be Valid GUID";
                            return false;

                        case "1210":
                            logErr = true;
                            Event_id = 6047;
                            err = "Leadstar.UpdateBranch Fails(1210): Branch Integration ID Not Found";
                            return false;

                        case "1211":
                            logErr = true;
                            Event_id = 6048;
                            err = "Leadstar.UpdateBranch Fails(1211): Company Not Found";
                            return false;

                        case "1212":
                            logErr = true;
                            Event_id = 6049;
                            err = "Leadstar.UpdateBranch Fails(1212): Leadstar internal Error";
                            return false;

                        default:
                            logErr = true;
                            Event_id = 6049;
                            err = "Leadstar.UpdateBranch Fails: " + err;
                            return false;
                    }                   
               }
               return true;
           }
           catch (Exception ex)
           {
               err = string.Format("Failed to update Leadstar Branch, BranchId={0}, Exception:{1}", BranchId, ex.Message);
               logErr  = true;
               return false;
           }
           finally
           {
               if (logErr)
               {
                   err = err + err1;
                   Trace.TraceError(err);
                   EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id);
               }
               else
               {
                   err = err1;
                   //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id);
               }
           }
        }

        private bool UpdateCompanyAdmin(MarketingMgr.LeadStar.User lsUser, ref string err)
        {
            err = "";
            string ugSql = string.Format("Update Company_General set LeadStar_username='{0}', LeadStar_userid='{1}'", lsUser.Username, lsUser.User_ID);
            DbHelperSQL.ExecuteSql(ugSql);
            
            return true;
        }

        private bool UpdateBranchMgr(int BranchId, MarketingMgr.LeadStar.User lsUser, ref string err)        
        {
            err = "";
            string ubSql = string.Format("Update Branches set LeadStar_username='{0}', LeadStar_userid='{1}' where BranchId={2}", lsUser.Username, lsUser.Integration_ID, BranchId);
            DbHelperSQL.ExecuteSql(ubSql);

            return true;
        }

        private bool SetupUser(int userId, ref MarketingMgr.LeadStar.User lsUser, MarketingMgr.LeadStar.Company companyInfo, ref bool CompanyAdmin, ref bool BranchMgr, ref int BranchId, ref string err)
        {
            CompanyAdmin = false;
            BranchMgr = false;
            BranchId = 0;
            err = "";
            bool st = true;
  
            string uSql = "select top 1 * from Users where UserId=" + userId;
            DataTable Users = DbHelperSQL.ExecuteDataTable(uSql);

            if (Users.Rows.Count == 0)
            {
                err = "Unable to find the user record.";
                return false;
            }

            int RoleId = 0;

            if (Users.Rows[0]["RoleId"] == DBNull.Value)
            {
                RoleId = 0;
            }
            else
            {
                RoleId = (int)Users.Rows[0]["RoleId"];
            }

            string username = Users.Rows[0]["username"] == DBNull.Value ? string.Empty : Users.Rows[0]["username"].ToString();
            string firstName = Users.Rows[0]["FirstName"] == DBNull.Value ? string.Empty : Users.Rows[0]["FirstName"].ToString();
            string lastName = Users.Rows[0]["LastName"] == DBNull.Value ? string.Empty : Users.Rows[0]["LastName"].ToString();
           
            string roleName = "";

            if (RoleId > 0)
            {
                string rSql = "select top 1 * from Roles where RoleId=" + RoleId;
                DataTable Roles = DbHelperSQL.ExecuteDataTable(rSql);
                if (Roles.Rows.Count > 0)
                {
                    roleName = Roles.Rows[0]["Name"] == DBNull.Value ? string.Empty : Roles.Rows[0]["Name"].ToString();           
                }
            }

            object obj = null;
            if (roleName.ToLower() == "executive")
            {
                uSql = "select Top 1 ExecutiveId from CompanyExecutives ce inner join users u on ce.ExecutiveId=u.UserId where u.UserEnabled=1";
                obj = DbHelperSQL.GetSingle(uSql);
                int compAdminUser = obj == null ? 0 : (int)obj;
                if (compAdminUser == userId)
                {
                    CompanyAdmin = true;
                }
            }
            else if (roleName.ToLower() == "branch manager")
            {
                uSql = "select Top 1 BranchId from BranchManagers bm inner join users u on bm.BranchMgrId=u.UserId where BranchMgrId=" + userId;
                obj = DbHelperSQL.GetSingle(uSql);
                BranchId = obj == null ? 0 : (int)obj;
                if (BranchId > 0)
                {
                    BranchMgr = true;
                }
            }
            else
            {
                uSql = "select Top 1 b.BranchId from GroupUsers gu inner join Groups g on gu.GroupId=g.GroupId inner join Branches b on g.BranchID=b.BranchId where gu.UserId=" + userId;
                obj = DbHelperSQL.GetSingle(uSql);
                BranchId = obj == null ? 0 : (int)obj;
            }

            if (lsUser == null)
                lsUser = new MarketingMgr.LeadStar.User();

            lsUser.Username = username;
            lsUser.First_Name = firstName;
            lsUser.Last_Name = lastName;
            lsUser.Email_Address = Users.Rows[0]["EmailAddress"] == DBNull.Value ? string.Empty : Users.Rows[0]["EmailAddress"].ToString();
            lsUser.Password = Users.Rows[0]["Password"] == DBNull.Value ? string.Empty : Users.Rows[0]["Password"].ToString();
            lsUser.Phone1_Area_Code = string.Empty;
            lsUser.Phone1_Prefix = string.Empty;
            lsUser.Phone1_Number = string.Empty;
            lsUser.Phone1_Type = string.Empty;
            lsUser.Phone2_Area_Code = string.Empty;
            lsUser.Phone2_Prefix = string.Empty;
            lsUser.Phone2_Number = string.Empty;
            lsUser.Phone2_Type = string.Empty;
            string Phone = Users.Rows[0]["Phone"] == DBNull.Value ? string.Empty : Users.Rows[0]["Phone"].ToString();
            if (!string.IsNullOrEmpty(Phone))
            {
                lsUser.Phone1_Type = "Direct";
                lsUser.Phone1_Area_Code = FindAreaCode(Phone);
                lsUser.Phone1_Prefix = FindNPA(Phone);
                lsUser.Phone1_Number = FindNXX(Phone);
            }

            string Cell = Users.Rows[0]["Cell"] == null ? string.Empty : Users.Rows[0]["Cell"].ToString();

            if (!string.IsNullOrEmpty(Cell))
            {
                lsUser.Phone2_Type = "Cell";
                lsUser.Phone2_Area_Code = FindAreaCode(Cell);
                lsUser.Phone2_Prefix = FindNPA(Cell);
                lsUser.Phone2_Number = FindNXX(Cell);
            }

            lsUser.License_Number = Users.Rows[0]["LicenseNumber"] == null ? string.Empty : Users.Rows[0]["LicenseNumber"].ToString();
            lsUser.Signature = Users.Rows[0]["Signature"] == null ? string.Empty : Users.Rows[0]["Signature"].ToString();
            lsUser.Photo = Users.Rows[0]["UserPictureFile"] == null ? string.Empty : Users.Rows[0]["UserPictureFile"].ToString();
            //model.UserPictureFile = DBNull.Value == dt.Rows[n]["UserPictureFile"] ? null : (byte[])dt.Rows[n]["UserPictureFile"];

            lsUser.Logo = "";
            lsUser.Secret_Question = "";
            lsUser.Secret_Answer = "";

            lsUser.Suffix = "";
            lsUser.Title = "";
            lsUser.Web_Address = "";

            bool MarketingAcctEnabled = Users.Rows[0]["MarketingAcctEnabled"] == DBNull.Value ? false : (bool)Users.Rows[0]["MarketingAcctEnabled"];
            bool UserEnabled = Users.Rows[0]["UserEnabled"] == DBNull.Value ? false : (bool)Users.Rows[0]["UserEnabled"];

            if (MarketingAcctEnabled && UserEnabled)
            {
                lsUser.Status = "Active";
            }
            else
            {
                lsUser.Status = "Inactive";
            }

            lsUser.Branch_Integration_ID = string.Empty;

            if (BranchId > 0)
            {
                MarketingMgr.LeadStar.Branch lsBranch = null;
                st = SetupBranch(BranchId, ref lsBranch, companyInfo.Company_Key, companyInfo.Integration_ID, companyInfo.Company_ID, ref err);

                if (lsBranch != null)
                {
                    lsUser.Branch_Integration_ID = lsBranch.Integration_ID;
                }
            }

            lsUser.Integration_ID = Users.Rows[0]["GlobalId"] == DBNull.Value ? string.Empty : Users.Rows[0]["GlobalId"].ToString();

            if (lsUser.Integration_ID.Length <= companyInfo.Integration_ID.Length)
                lsUser.Integration_ID = "";

            if (lsUser.Branch_Integration_ID.Length <= companyInfo.Integration_ID.Length)
            {
                object objb = DbHelperSQL.GetSingle("Select top 1 GlobalId from Branches where Enabled=1");
                if (objb != null)
                {
                    lsUser.Branch_Integration_ID = (string)objb;
                }   
            }

            return true;
        }

        public bool UpdateUser(int UserId, ref string err)
        {
           err = string.Empty;
           string err1 = string.Empty;
           int Event_id = 6050;
           int BranchId = 0;
           bool logErr = false;
           bool CompanyAdmin = false;
           bool BranchMgr = false;
           bool ShouldUpdateUser = false;
           bool st = true;
           string str = string.Empty;
           string ugSql = string.Empty;

           try
           {             
               MarketingMgr.LeadStar.User lsUser = null;
               MarketingMgr.LeadStar.Company lsCompany = null;

               lsCompany = SetupCompanyInfo(ref err);
               if (lsCompany == null)
               {
                   logErr = true;
                   return false;
               }

               st = SetupUser(UserId, ref lsUser, lsCompany, ref CompanyAdmin, ref BranchMgr, ref BranchId, ref err);              
               if (lsUser == null)
               {
                    err = string.Format("Failed to set up Leadstar user record, userId={0}, error='{1}'.", UserId, err);
                    logErr = true;
                    return false;
               }

               if (string.IsNullOrEmpty(lsUser.Password))
               {
                   lsUser.Password = "testingpassword";
               }

               if (CompanyAdmin)
               {
                   lsUser.User_Type = UserType.CompanyManager;                   
               }
               else if (BranchMgr)
               {
                   lsUser.User_Type = UserType.BranchManager;                  
               }
               else
               {
                   lsUser.User_Type = UserType.BranchUser;
               }              

               using (LeadStarServiceClient LS = new LeadStarServiceClient())
               {
                   if (string.IsNullOrEmpty(lsUser.Integration_ID))
                   {
                       lsUser.Integration_ID = lsCompany.Integration_ID + "_u" + UserId.ToString();
                       
                       err = LS.InsertUser("MY", lsUser, LeadStar_APIKey, lsCompany.Company_Key);

                       err1 = string.Format("  [Message detail]: " +
                                            " user.Branch_ID = '{0}'," +
                                            " user.Branch_Integration_ID = '{1}'," +
                                            " user.Company_ID = '{2}'," +
                                            " user.Company_Integration_ID = '{3}'," +
                                            " user.Email_Address = '{4}'," +
                                            " user.First_Name = '{5}'," +
                                            " user.Integration_ID = '{6}'," +
                                            " user.Last_Name = '{7}'," +
                                            " user.License_Number = '{8}'," +
                                            " user.Logo = '{9}'," +
                                            " user.Password = '{10}'," +
                                            " user.Phone1_Area_Code = '{11}'," +
                                            " user.Phone1_Number = '{12}'," +
                                            " user.Phone1_Prefix = '{13}'," +
                                            " user.Phone1_Type = '{14}'," +
                                            " user.Phone2_Area_Code = '{15}'," +
                                            " user.Phone2_Number = '{16}'," +
                                            " user.Phone2_Prefix = '{17}'," +
                                            " user.Phone2_Type = '{18}'," +
                                            " user.Photo = '{19}'," +
                                            " user.Secret_Answer = '{20}'," +
                                            " user.Secret_Question = '{21}'," +
                                            " user.Status = '{22}'," +
                                            " user.Suffix = '{23}'," +
                                            " user.Title = '{24}'," +
                                            " user.User_ID = '{25}'," +
                                            " user.User_Type = '{26}'," +
                                            " user.Username = '{27}'," +
                                            " user.Web_Address = '{28}' ",
                                            lsUser.Branch_ID,
                                            lsUser.Branch_Integration_ID,
                                            lsUser.Company_ID,
                                            lsUser.Company_Integration_ID,
                                            lsUser.Email_Address,
                                            lsUser.First_Name,
                                            lsUser.Integration_ID,
                                            lsUser.Last_Name,
                                            lsUser.License_Number,
                                            lsUser.Logo,
                                            lsUser.Password,
                                            lsUser.Phone1_Area_Code,
                                            lsUser.Phone1_Number,
                                            lsUser.Phone1_Prefix,
                                            lsUser.Phone1_Type,
                                            lsUser.Phone2_Area_Code,
                                            lsUser.Phone2_Number,
                                            lsUser.Phone2_Prefix,
                                            lsUser.Phone2_Type,
                                            lsUser.Photo,
                                            lsUser.Secret_Answer,
                                            lsUser.Secret_Question,
                                            lsUser.Status,
                                            lsUser.Suffix,
                                            lsUser.Title,
                                            lsUser.User_ID,
                                            lsUser.User_Type,
                                            lsUser.Username,
                                            lsUser.Web_Address);
                                           
                       switch (err.ToLower())
                       {
                           case "1000":
                               err = "";
                               logErr = false;
                               Event_id = 6050;
                               ugSql = "update Users set GlobalId='" + lsUser.Integration_ID + "' where UserId=" + UserId;
                               int status = DbHelperSQL.ExecuteSql(ugSql);
                               break;
                               
                           case "1201":
                               logErr = true;
                               Event_id = 6051;
                               err = "Leadstar.InsertUser Fails(1201): Empty IntegrationID";
                               return false;

                           case "1202":
                               logErr = true;
                               Event_id = 6052;
                               err = "Leadstar.InsertUser Fails(1202): Bad Instance ID";
                               return false;

                           case "1203":
                               logErr = true;
                               Event_id = 6053;
                               err = "Leadstar.InsertUser Fails(1203): Bad API_Key";
                               return false;

                           case "1204":
                               logErr = true;
                               Event_id = 6054;
                               err = "Leadstar.InsertUser Fails(1204): Username not Found";
                               return false;

                           case "1205":
                               logErr = true;
                               Event_id = 6055;
                               err = "Leadstar.InsertUser Fails(1205): Bad Company Key";
                               return false;

                           case "1206":
                               logErr = true;
                               Event_id = 6056;
                               err = "Leadstar.InsertUser Fails(1206): Integration_ID must be unique across company";
                               return false;

                           case "1211":
                               logErr = true;
                               Event_id = 6057;
                               err = "Leadstar.InsertUser Fails(1211): Company or Branch Not Found";
                               return false;

                           case "1212":
                               logErr = true;
                               Event_id = 6058;
                               err = "Leadstar.InsertUser Fails(1212): Leadstar Internal Error";
                               return false;

                           case "1213":
                               logErr = true;
                               Event_id = 6059;
                               err = "Leadstar.InsertUser Fails(1213): Integration_ID, Username, Password & Branch Integration ID Are All Required";
                               return false;

                           case "1214":
                               logErr = true;
                               Event_id = 6059;
                               err = "Leadstar.InsertUser Fails(1214): Username is already taken in system";
                               return false;

                           case "1215":
                               logErr = true;
                               Event_id = 6059;
                               err = "Leadstar.InsertUser Fails(1215): This Branch Already Has a Manager";
                               return false;

                           case "1216":
                               logErr = true;
                               Event_id = 6059;
                               err = "Leadstar.InsertUser Fails(1216): This Company Already Has a Manager";
                               return false;

                           default:
                               logErr = true;
                               Event_id = 6059;
                               err = "Leadstar.InsertUser Fails: " + err;
                               return false;
                       }
                       ShouldUpdateUser = false;
                   }
                   else
                       ShouldUpdateUser = true;

                   MarketingMgr.LeadStar.User user = LS.GetUserByIntegrationID("MY", lsUser.Integration_ID, LeadStar_APIKey, lsCompany.Company_Key);
                   if (user == null)
                   {  
                       if ( ShouldUpdateUser == true)
                       {
                       logErr = true;
                       Event_id = 6069;
                       err = string.Format("Failed to get LeadStar User Record via GetUserByIntegrationID, UserId={0}", UserId);
                       return false;
                       }
                       else
                       {
                           err = "";
                           logErr = false;
                           return true;
                       }
                   }

                   ugSql = string.Format("Update users set Leadstar_ID='{0}' where UserId={1}", user.User_ID, UserId);
                   DbHelperSQL.ExecuteSql(ugSql);

                   if (ShouldUpdateUser != true)
                   {
                       err = "";
                       return true;
                   }

                   lsUser.User_ID = user.User_ID;
                   user.Username = lsUser.Username;
                   user.First_Name = lsUser.First_Name;
                   user.Last_Name = lsUser.Last_Name;
                   user.Email_Address = lsUser.Email_Address;
                   user.Password = lsUser.Password;
                   user.Phone1_Area_Code = lsUser.Phone1_Area_Code;
                   user.Phone1_Prefix = lsUser.Phone1_Prefix;
                   user.Phone1_Number = lsUser.Phone1_Number;
                   user.Phone1_Type = lsUser.Phone1_Type;
                   user.Phone2_Area_Code = lsUser.Phone2_Area_Code;
                   user.Phone2_Prefix = lsUser.Phone2_Prefix;
                   user.Phone2_Number = lsUser.Phone2_Number;
                   user.Phone2_Type = lsUser.Phone2_Type;
                   user.License_Number = lsUser.License_Number;
                   user.Signature = lsUser.Signature;
                   user.Photo = lsUser.Photo;
                   user.Status = lsUser.Status;

                   user.Logo = "";
                   user.Secret_Question = "";
                   user.Secret_Answer = "";

                   user.Suffix = "";
                   user.Title = "";
                   user.Web_Address = "";

                   err = LS.UpdateUser("MY", user, LeadStar_APIKey, lsCompany.Company_Key);

                   err1 = string.Format("  [Message detail]: " +
                                            " user.Branch_ID = '{0}'," +
                                            " user.Branch_Integration_ID = '{1}'," +
                                            " user.Company_ID = '{2}'," +
                                            " user.Company_Integration_ID = '{3}'," +
                                            " user.Email_Address = '{4}'," +
                                            " user.First_Name = '{5}'," +
                                            " user.Integration_ID = '{6}'," +
                                            " user.Last_Name = '{7}'," +
                                            " user.License_Number = '{8}'," +
                                            " user.Logo = '{9}'," +
                                            " user.Password = '{10}'," +
                                            " user.Phone1_Area_Code = '{11}'," +
                                            " user.Phone1_Number = '{12}'," +
                                            " user.Phone1_Prefix = '{13}'," +
                                            " user.Phone1_Type = '{14}'," +
                                            " user.Phone2_Area_Code = '{15}'," +
                                            " user.Phone2_Number = '{16}'," +
                                            " user.Phone2_Prefix = '{17}'," +
                                            " user.Phone2_Type = '{18}'," +
                                            " user.Photo = '{19}'," +
                                            " user.Secret_Answer = '{20}'," +
                                            " user.Secret_Question = '{21}'," +
                                            " user.Status = '{22}'," +
                                            " user.Suffix = '{23}'," +
                                            " user.Title = '{24}'," +
                                            " user.User_ID = '{25}'," +
                                            " user.User_Type = '{26}'," +
                                            " user.Username = '{27}'," +
                                            " user.Web_Address = '{28}' ",
                                            user.Branch_ID,
                                            user.Branch_Integration_ID,
                                            user.Company_ID,
                                            user.Company_Integration_ID,
                                            user.Email_Address,
                                            user.First_Name,
                                            user.Integration_ID,
                                            user.Last_Name,
                                            user.License_Number,
                                            user.Logo,
                                            user.Password,
                                            user.Phone1_Area_Code,
                                            user.Phone1_Number,
                                            user.Phone1_Prefix,
                                            user.Phone1_Type,
                                            user.Phone2_Area_Code,
                                            user.Phone2_Number,
                                            user.Phone2_Prefix,
                                            user.Phone2_Type,
                                            user.Photo,
                                            user.Secret_Answer,
                                            user.Secret_Question,
                                            user.Status,
                                            user.Suffix,
                                            user.Title,
                                            user.User_ID,
                                            user.User_Type,
                                            user.Username,
                                            user.Web_Address);

                   switch (err.ToLower())
                   {
                       case "1000":
                           err = "";
                           logErr = false;
                           Event_id = 6060;
                           break;

                       case "1201":
                           logErr = true;
                           Event_id = 6061;
                           err = "Leadstar.UpdateUser Fails(1201): Empty IntegrationID";
                           return false;

                       case "1202":
                           logErr = true;
                           Event_id = 6062;
                           err = "Leadstar.UpdateUser Fails(1202): Bad Instance ID";
                           return false;

                       case "1203":
                           logErr = true;
                           Event_id = 6063;
                           err = "Leadstar.UpdateUser Fails(1203): Bad API_Key";
                           return false;

                       case "1204":
                           logErr = true;
                           Event_id = 6064;
                           err = "Leadstar.UpdateUser Fails(1204): Username not Found";
                           return false;

                       case "1205":
                           logErr = true;
                           Event_id = 6065;
                           err = "Leadstar.UpdateUser Fails(1205): Bad Company Key";
                           return false;

                       case "1206":
                           logErr = true;
                           Event_id = 6066;
                           err = "Leadstar.UpdateUser Fails(1206): User Integration_ID must be unique across company";
                           return false;

                       case "1211":
                           logErr = true;
                           Event_id = 6067;
                           err = "Leadstar.UpdateUser Fails(1211): Company or Branch Not Found";
                           return false;

                       case "1212":
                           logErr = true;
                           Event_id = 6068;
                           err = "Leadstar.UpdateUser Fails(1212): Leadstar Internal Error";
                           return false;

                       case "1213":
                           logErr = true;
                           Event_id = 6069;
                           err = "Leadstar.UpdateUser Fails(1213): Integration_ID, Username & Password Are Required";
                           return false;

                       case "1214":
                           logErr = true;
                           Event_id = 6069;
                           err = "Leadstar.UpdateUser Fails(1214): Username is already taken in system";
                           return false;

                       case "1215":
                           logErr = true;
                           Event_id = 6069;
                           err = "Leadstar.UpdateUser Fails(1215): This Branch Already Has a Manager";
                           return false;

                       case "1216":
                           logErr = true;
                           Event_id = 6069;
                           err = "Leadstar.UpdateUser Fails(1216): This Company Already Has a Manager";
                           return false;

                       case "1217":
                           logErr = true;
                           Event_id = 6069;
                           err = "Leadstar.UpdateUser Fails(1217): Both Company Integration ID and Branch Integration ID are required";
                           return false;

                       case "1218":
                           logErr = true;
                           Event_id = 6069;
                           err = "Leadstar.UpdateUser Fails(1218): User not in system";
                           return false;

                       default:
                           logErr = true;
                           Event_id = 6069;
                           err = "Leadstar.UpdateUser Fails: " + err;
                           return false;
                   }                  

                   if (CompanyAdmin)
                   {
                       bool status = UpdateCompanyAdmin(lsUser, ref err);                       
                   }
                   else if (BranchMgr)
                   {
                       bool status = UpdateBranchMgr( BranchId, lsUser, ref err);                      
                   }

                   return true; 
               }
           }
           catch (Exception ex)
           {
               err = string.Format("Failed to update Leadstar User, UserId={0}. Exception:{1}", UserId, ex.Message);
               logErr = true;
               return false;
           }
           finally
           {
               if (logErr)
               {
                   err = err + err1;
                   Trace.TraceError(err);
                   EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id);
               }
               else
               {
                   err = err1;
                   //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id);
               }
           }
        }

        private bool SetupProspect(int FileId, ref MarketingMgr.LeadStar.Prospect psNewProspect, MarketingMgr.LeadStar.Company companyInfo, ref string err)
        {           
            string strCompanyKey = companyInfo.Company_Key;
            DateTime dtNull = new DateTime(1800, 1, 1);
            
            string cSql = "select * from Loans where FileId=" + FileId;
            DataTable Loans = DbHelperSQL.ExecuteDataTable(cSql);

            if (Loans == null || Loans.Rows.Count == 0)
            {
                err = "Unable to find the loan record.";
                return false;
            }
                        
            if (Loans.Rows[0]["AppraisedValue"] != DBNull.Value)
            {
                psNewProspect.Appraised_Value = (decimal)Loans.Rows[0]["AppraisedValue"];
            }

            if (Loans.Rows[0]["DateClose"] != DBNull.Value)
            {
                psNewProspect.Closing_Date = (DateTime)Loans.Rows[0]["DateClose"];
            }
            else
            {
                psNewProspect.Closing_Date = dtNull;
            }

            psNewProspect.Company = companyInfo.Company_Name;

            if (Loans.Rows[0]["GlobalId"].ToString().Length < companyInfo.Integration_ID.Length)
            {
                psNewProspect.Integration_ID = null;
            }
            else
            {
                psNewProspect.Integration_ID = Loans.Rows[0]["GlobalId"].ToString();
            }

            if (Loans.Rows[0]["Rate"] != DBNull.Value)
            {
                psNewProspect.Interest_Rate = (decimal)Loans.Rows[0]["Rate"];
            }

            if (Loans.Rows[0]["LoanAmount"] != DBNull.Value)
            {
                psNewProspect.Loan_Amount = (decimal)Loans.Rows[0]["LoanAmount"];
            }

            psNewProspect.Loan_Number = Loans.Rows[0]["LoanNumber"] == DBNull.Value ? string.Empty : Loans.Rows[0]["LoanNumber"].ToString();
            psNewProspect.Loan_Type = Loans.Rows[0]["LoanType"] == DBNull.Value ? string.Empty : Loans.Rows[0]["LoanType"].ToString();

            if (Loans.Rows[0]["LTV"] != DBNull.Value)
            {
                psNewProspect.LTV_Ratio = (decimal)Loans.Rows[0]["LTV"];
            }

            if (Loans.Rows[0]["MonthlyPayment"] != DBNull.Value)
            {
                psNewProspect.Monthly_Payment = (decimal)Loans.Rows[0]["MonthlyPayment"];
            }

            psNewProspect.Occupancy = Loans.Rows[0]["Occupancy"] == DBNull.Value ? string.Empty : Loans.Rows[0]["Occupancy"].ToString();

            psNewProspect.Prospect_Type = Loans.Rows[0]["ProspectLoanStatus"] == DBNull.Value ? string.Empty : Loans.Rows[0]["ProspectLoanStatus"].ToString();

            if (Loans.Rows[0]["SalesPrice"] != DBNull.Value)
            {
                psNewProspect.Purchase_Price = (decimal)Loans.Rows[0]["SalesPrice"];
            }

            if (Loans.Rows[0]["LeadStar_username"] != DBNull.Value)
            {
                psNewProspect.Username = Loans.Rows[0]["LeadStar_username"].ToString();
            }
            else
            {
                psNewProspect.Username = string.Empty;
            }

            if (Loans.Rows[0]["LeadStar_userid"] != DBNull.Value)
            {
                psNewProspect.User_ID = Loans.Rows[0]["LeadStar_userid"].ToString();
            }
            else
            {
                psNewProspect.User_ID = string.Empty;
            }

            psNewProspect.Property_Type = string.Empty;

            psNewProspect.Referral_Company = string.Empty; 
            psNewProspect.Referral_Email = string.Empty; 
            psNewProspect.Referral_First_Name = string.Empty; 
            psNewProspect.Referral_Last_Name = string.Empty;
 
            psNewProspect.Referral_Phone_Area_Code = string.Empty; 
            psNewProspect.Referral_Phone_Number = string.Empty; 
            psNewProspect.Referral_Phone_Prefix = string.Empty; 
            psNewProspect.Referral_Type = string.Empty;
 
            psNewProspect.Salutation = string.Empty;
            psNewProspect.Secondary_Birthdate = dtNull;
            
            psNewProspect.Secondary_First_Name = string.Empty;
            psNewProspect.Secondary_Last_Name = string.Empty;

            decimal trr = 2;

            psNewProspect.Target_Refi_Rate = trr;
            psNewProspect.Website = string.Empty;
        
            return true;
        }

        private bool SetupContact(int FileId, ref MarketingMgr.LeadStar.Prospect psNewProspect, ref string err)        
        {            
            err = string.Empty;            
            bool logErr = false;
            DateTime dtNull = new DateTime(1800, 1, 1);
            string str = string.Empty;            
            int ContactRoleId = 0;
            int ContactId = 0;            

            try
            {
                ContactRoleId = 1;
                object obj = DbHelperSQL.GetSingle("Select top 1 ContactId from LoanContacts where FileId=" + FileId + " and ContactRoleId=" + ContactRoleId);
                if (obj != null)
                {
                    ContactId = (int)obj;
                }
                else
                {
                    ContactRoleId = 2;
                    object obj1 = DbHelperSQL.GetSingle("Select top 1 ContactId from LoanContacts where FileId=" + FileId + " and ContactRoleId=" + ContactRoleId);
                    if (obj1 != null)
                    {
                    ContactId = (int)obj1;
                    }
                    else 
                    {
                    return false;
                    }
                }
         
                psNewProspect.Contact_ID = ContactId.ToString();

                string cSql = "select * from Contacts where ContactId=" + ContactId;
                DataTable Contacts = DbHelperSQL.ExecuteDataTable(cSql);

                if (Contacts == null || Contacts.Rows.Count == 0)
                {
                    err = "Unable to find the contact record.";
                    return false;
                }                

                psNewProspect.First_Name = Contacts.Rows[0]["FirstName"] == DBNull.Value ? string.Empty : Contacts.Rows[0]["FirstName"].ToString();
                psNewProspect.Last_Name = Contacts.Rows[0]["LastName"] == DBNull.Value ? string.Empty : Contacts.Rows[0]["LastName"].ToString();             
                psNewProspect.Suffix = Contacts.Rows[0]["GenerationCode"] == DBNull.Value ? string.Empty : Contacts.Rows[0]["GenerationCode"].ToString();

                string HomePhone = Contacts.Rows[0]["HomePhone"] == DBNull.Value ? string.Empty : Contacts.Rows[0]["HomePhone"].ToString();
                
                psNewProspect.Home_Phone_Area_Code = string.Empty;
                psNewProspect.Home_Phone_Prefix = string.Empty;
                psNewProspect.Home_Phone_Number = string.Empty;

                if (!string.IsNullOrEmpty(HomePhone))
                {
                    psNewProspect.Home_Phone_Area_Code = FindAreaCode(HomePhone);
                    psNewProspect.Home_Phone_Prefix = FindNPA(HomePhone); 
                    psNewProspect.Home_Phone_Number = FindNXX(HomePhone);                    
                }

                string CellPhone = Contacts.Rows[0]["CellPhone"] == DBNull.Value ? string.Empty : Contacts.Rows[0]["CellPhone"].ToString();

                psNewProspect.Cell_Phone_Area_Code = string.Empty;
                psNewProspect.Cell_Phone_Prefix = string.Empty;
                psNewProspect.Cell_Phone_Number = string.Empty;

                if (!string.IsNullOrEmpty(CellPhone))
                {
                    psNewProspect.Cell_Phone_Area_Code = FindAreaCode(CellPhone);
                    psNewProspect.Cell_Phone_Prefix = FindNPA(CellPhone);
                    psNewProspect.Home_Phone_Number = FindNXX(CellPhone);
                }

                string BusinessPhone = Contacts.Rows[0]["BusinessPhone"] == DBNull.Value ? string.Empty : Contacts.Rows[0]["BusinessPhone"].ToString();

                psNewProspect.Office_Phone_Area_Code = string.Empty;
                psNewProspect.Office_Phone_Prefix = string.Empty;
                psNewProspect.Office_Phone_Number = string.Empty;
                psNewProspect.Office_Phone_Ext = string.Empty;

                if (!string.IsNullOrEmpty(BusinessPhone))
                {
                    psNewProspect.Office_Phone_Area_Code = FindAreaCode(BusinessPhone);
                    psNewProspect.Office_Phone_Prefix = FindNPA(BusinessPhone); 
                    psNewProspect.Office_Phone_Number = FindNXX(BusinessPhone);
                    psNewProspect.Office_Phone_Ext = FindExtension(BusinessPhone);                    
                }

                string Fax = Contacts.Rows[0]["Fax"] == DBNull.Value ? string.Empty : Contacts.Rows[0]["Fax"].ToString();

                psNewProspect.Fax_Area_Code = string.Empty;
                psNewProspect.Fax_Prefix = string.Empty;
                psNewProspect.Fax_Number = string.Empty;

                if (!string.IsNullOrEmpty(Fax))
                {
                    psNewProspect.Fax_Area_Code = FindAreaCode(Fax);
                    psNewProspect.Fax_Prefix = FindNPA(Fax);
                    psNewProspect.Fax_Number = FindNXX(Fax);
                }

                psNewProspect.Email_Address = Contacts.Rows[0]["Email"] == DBNull.Value ? string.Empty : Contacts.Rows[0]["Email"].ToString();

                if (Contacts.Rows[0]["DOB"] != DBNull.Value)
                {
                    psNewProspect.Birthdate = (DateTime)Contacts.Rows[0]["DOB"];
                }
                else
                {
                    psNewProspect.Birthdate = dtNull;
                }

                psNewProspect.Address1 = Contacts.Rows[0]["MailingAddr"] == DBNull.Value ? string.Empty : Contacts.Rows[0]["MailingAddr"].ToString();
                psNewProspect.Address2 = string.Empty;

                psNewProspect.City = Contacts.Rows[0]["MailingCity"] == DBNull.Value ? string.Empty : Contacts.Rows[0]["MailingCity"].ToString();
                psNewProspect.State = Contacts.Rows[0]["MailingState"] == DBNull.Value ? string.Empty : Contacts.Rows[0]["MailingState"].ToString();
                psNewProspect.Zip = Contacts.Rows[0]["MailingZip"] == DBNull.Value ? string.Empty : Contacts.Rows[0]["MailingZip"].ToString();

                psNewProspect.Contact_Status = Contacts.Rows[0]["Enabled"].ToString() == "True" ? "Active" : "Inactive";

                psNewProspect.Date_Created = dtNull;                
                psNewProspect.Date_Modified = dtNull;
            }
            catch (Exception ex)
            {
                err = string.Format("Failed to setup Contact, ContactId={0}. Exception:{1}", ContactId, ex.Message);
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }

            return true;
        }

        private bool SetupUsername(int FileId, MarketingMgr.LeadStar.Company lsCompany, ref MarketingMgr.LeadStar.Prospect psNewProspect, ref string err)
        {
            int UserId = 0;
            int BranchId = 0;
            bool st = true;

            object obju = DbHelperSQL.GetSingle("Select top 1 UserId from LoanTeam where RoleId=3 and FileId=" + FileId);
            if (obju != null)
            {
                UserId = (int)obju;
            }
            else
            {
                object objua = DbHelperSQL.GetSingle("Select top 1 UserId from LoanTeam where FileId=" + FileId);
                if (objua != null)
                {
                    UserId = (int)objua;
                }
                else
                {
                    object objuab = DbHelperSQL.GetSingle("Select top 1 BranchID from Loans where FileId=" + FileId);
                    if (objuab != null)
                    {
                        BranchId = (int)objuab;
                    }
                    else
                    {
                        return false;
                    }                   
                }             
            }

            if (BranchId > 0)
            {
                object objb = DbHelperSQL.GetSingle("Select top 1 Leadstar_Username from Branches where BranchId=" + BranchId);
                if (objb != null)
                {
                    psNewProspect.Username = (string)objb;
                }

                object objb1 = DbHelperSQL.GetSingle("Select top 1 Leadstar_Userid from Branches where BranchId=" + BranchId);
                if (objb1 != null)
                {
                    psNewProspect.User_ID = (string)objb1;
                }

                string ulSql1 = "update Loans set LeadStar_username='" + psNewProspect.Username + "', LeadStar_userid='" + psNewProspect.User_ID + "' where FileId=" + FileId;
                int sta = DbHelperSQL.ExecuteSql(ulSql1);

                return true;
            }

            object objun = DbHelperSQL.GetSingle("Select top 1 Username from Users where UserId=" + UserId);
            if (objun != null)
            {
                psNewProspect.Username = (string)objun;
            }
            else
            {
                return false;
            }

            object objui = DbHelperSQL.GetSingle("Select top 1 GlobalId from Users where UserId=" + UserId);
            if (objui != null)
            {
                psNewProspect.User_ID = (string)objui;
            }
            else
            {
                return false;
            }

            string ulSql = "update Loans set LeadStar_username='" + psNewProspect.Username + "', LeadStar_userid='" + psNewProspect.User_ID + "' where FileId=" + FileId;
            int status = DbHelperSQL.ExecuteSql(ulSql);

            return true;
        }

        private bool SetUp_psGetProspect(ref MarketingMgr.LeadStar.Prospect psGetProspect, MarketingMgr.LeadStar.Prospect psNewProspect, ref string err)
        {
            psGetProspect.Address1 = psNewProspect.Address1;
            psGetProspect.Address2 = psNewProspect.Address2;
            psGetProspect.Appraised_Value = psNewProspect.Appraised_Value;
            psGetProspect.Birthdate = psNewProspect.Birthdate;
            psGetProspect.Cell_Phone_Area_Code = psNewProspect.Cell_Phone_Area_Code;
            psGetProspect.Cell_Phone_Number = psNewProspect.Cell_Phone_Number;
            psGetProspect.Cell_Phone_Prefix = psNewProspect.Cell_Phone_Prefix;
            psGetProspect.City = psNewProspect.City;
            psGetProspect.Closing_Date = psNewProspect.Closing_Date;
            psGetProspect.Company = psNewProspect.Company;
            psGetProspect.Email_Address = psNewProspect.Email_Address;
            psGetProspect.Email_Notification = psNewProspect.Email_Notification;
            psGetProspect.Fax_Area_Code = psNewProspect.Fax_Area_Code;
            psGetProspect.Fax_Number = psNewProspect.Fax_Number;
            psGetProspect.Fax_Prefix = psNewProspect.Fax_Prefix;
            psGetProspect.First_Name = psNewProspect.First_Name;
            psGetProspect.Home_Phone_Area_Code = psNewProspect.Home_Phone_Area_Code;
            psGetProspect.Home_Phone_Number = psNewProspect.Home_Phone_Number;
            psGetProspect.Home_Phone_Prefix = psNewProspect.Home_Phone_Prefix;
            psGetProspect.Integration_ID = psNewProspect.Integration_ID;
            psGetProspect.Interest_Rate = psNewProspect.Interest_Rate;
            psGetProspect.Last_Name = psNewProspect.Last_Name;
            psGetProspect.Loan_Amount = psNewProspect.Loan_Amount;
            psGetProspect.Loan_Number = psNewProspect.Loan_Number;
            psGetProspect.Loan_Type = psNewProspect.Loan_Type;
            psGetProspect.LTV_Ratio = psNewProspect.LTV_Ratio;
            psGetProspect.Monthly_Payment = psNewProspect.Monthly_Payment;
            psGetProspect.Occupancy = psNewProspect.Occupancy;
            psGetProspect.Office_Phone_Area_Code = psNewProspect.Office_Phone_Area_Code;
            psGetProspect.Office_Phone_Ext = psNewProspect.Office_Phone_Ext;
            psGetProspect.Office_Phone_Number = psNewProspect.Office_Phone_Number;
            psGetProspect.Office_Phone_Prefix = psNewProspect.Office_Phone_Prefix;
            psGetProspect.Property_Type = psNewProspect.Property_Type;
            psGetProspect.Prospect_Type = psNewProspect.Prospect_Type;
            psGetProspect.Purchase_Price = psNewProspect.Purchase_Price;
            psGetProspect.Referral_Company = psNewProspect.Referral_Company;
            psGetProspect.Referral_Email = psNewProspect.Referral_Email;
            psGetProspect.Referral_First_Name = psNewProspect.Referral_First_Name;
            psGetProspect.Referral_Last_Name = psNewProspect.Referral_Last_Name;
            psGetProspect.Referral_Phone_Area_Code = psNewProspect.Referral_Phone_Area_Code;
            psGetProspect.Referral_Phone_Number = psNewProspect.Referral_Phone_Number;
            psGetProspect.Referral_Phone_Prefix = psNewProspect.Referral_Phone_Prefix;
            psGetProspect.Referral_Type = psNewProspect.Referral_Type;
            psGetProspect.Salutation = psNewProspect.Salutation;
            psGetProspect.Secondary_Birthdate = psNewProspect.Secondary_Birthdate;
            psGetProspect.Secondary_First_Name = psNewProspect.Secondary_First_Name;
            psGetProspect.Secondary_Last_Name = psNewProspect.Secondary_Last_Name;
            psGetProspect.State = psNewProspect.State;
            psGetProspect.Suffix = psNewProspect.Suffix;
            psGetProspect.Target_Refi_Rate = psNewProspect.Target_Refi_Rate;
            psGetProspect.Username = psNewProspect.Username;
            psGetProspect.Website = psNewProspect.Website;
            psGetProspect.Zip = psNewProspect.Zip;

            return true;
        }

        public bool UpdateProspect(int FileId, ref string err)        
        {
            bool st = true;
            bool ShouldUpdateUser = false;
            err = string.Empty;
            string err1 = string.Empty;
            int Event_id = 6070;
            bool logErr = false;            
            string str = string.Empty;
            string ugSql = string.Empty;

            try
            {
                MarketingMgr.LeadStar.Company lsCompany = null;

                lsCompany = SetupCompanyInfo(ref err);

                if (lsCompany == null)
                {
                    logErr = true;
                    Event_id = 6079;
                    return false;
                }

                MarketingMgr.LeadStar.Prospect psNewProspect = new MarketingMgr.LeadStar.Prospect();

                if (SetupProspect(FileId, ref psNewProspect, lsCompany, ref err) == false)
                {
                    if (!string.IsNullOrEmpty(err))
                    {
                        logErr = true;
                        Event_id = 6079;
                        return false;
                    }
                }

                if (psNewProspect == null)
                {
                    err = string.Format("Failed to set up Leadstar prospect record, FileId={0}.", FileId);
                    logErr = true;
                    Event_id = 6079;
                    return false;
                }

                st = SetupContact( FileId, ref psNewProspect, ref err);

                st = SetupUsername(FileId, lsCompany, ref psNewProspect, ref err);
        
                using (LeadStarServiceClient LS = new LeadStarServiceClient())
                {
                    if (string.IsNullOrEmpty(psNewProspect.Integration_ID))
                    {
                        psNewProspect.Integration_ID = lsCompany.Integration_ID + "_l" + FileId.ToString();

                        err1 = string.Format("  [Message detail]: " +
                                                  " prospect.Address1 = '{0}'," +
                                                  " prospect.Address2 = '{1}'," +
                                                  " prospect.Appraised_Value = '{2}'," +
                                                  " prospect.Birthdate = '{3}'," +
                                                  " prospect.Cell_Phone_Area_Code = '{4}'," +
                                                  " prospect.Cell_Phone_Number = '{5}'," +
                                                  " prospect.Cell_Phone_Prefix = '{6}'," +
                                                  " prospect.City = '{7}'," +
                                                  " prospect.Closing_Date = '{8}'," +
                                                  " prospect.Company = '{9}'," +
                                                  " prospect.Contact_ID = '{10}'," +
                                                  " prospect.Contact_Status = '{11}'," +
                                                  " prospect.Date_Created = '{12}'," +
                                                  " prospect.Date_Modified = '{13}'," +
                                                  " prospect.Email_Address = '{14}'," +
                                                  " prospect.Email_Notification = '{15}'," +
                                                  " prospect.Fax_Area_Code = '{16}'," +
                                                  " prospect.Fax_Number = '{17}'," +
                                                  " prospect.Fax_Prefix = '{18}'," +
                                                  " prospect.First_Name = '{19}'," +
                                                  " prospect.Home_Phone_Area_Code = '{20}'," +
                                                  " prospect.Home_Phone_Number = '{21}'," +
                                                  " prospect.Home_Phone_Prefix = '{22}'," +
                                                  " prospect.Integration_ID = '{23}'," +
                                                  " prospect.Interest_Rate = '{24}'," +
                                                  " prospect.Last_Name = '{25}'," +
                                                  " prospect.Loan_Amount = '{26}'," +
                                                  " prospect.Loan_Number = '{27}'," +
                                                  " prospect.Loan_Type = '{28}'," +
                                                  " prospect.LTV_Ratio = '{29}'," +
                                                  " prospect.Monthly_Payment = '{30}'," +
                                                  " prospect.Occupancy = '{31}'," +
                                                  " prospect.Office_Phone_Area_Code = '{32}'," +
                                                  " prospect.Office_Phone_Ext = '{33}'," +
                                                  " prospect.Office_Phone_Number = '{34}'," +
                                                  " prospect.Office_Phone_Prefix = '{35}'," +
                                                  " prospect.Property_Type = '{36}'," +
                                                  " prospect.Prospect_Type = '{37}'," +
                                                  " prospect.Purchase_Price = '{38}'," +
                                                  " prospect.Referral_Company = '{39}'," +
                                                  " prospect.Referral_Email = '{40}'," +
                                                  " prospect.Referral_First_Name = '{41}'," +
                                                  " prospect.Referral_Last_Name = '{42}'," +
                                                  " prospect.Referral_Phone_Area_Code = '{43}'," +
                                                  " prospect.Referral_Phone_Number = '{44}'," +
                                                  " prospect.Referral_Phone_Prefix = '{45}'," +
                                                  " prospect.Referral_Type = '{46}'," +
                                                  " prospect.Salutation = '{47}'," +
                                                  " prospect.Secondary_Birthdate = '{48}'," +
                                                  " prospect.Secondary_First_Name = '{49}'," +
                                                  " prospect.Secondary_Last_Name = '{50}'," +
                                                  " prospect.State = '{51}'," +
                                                  " prospect.Suffix = '{52}'," +
                                                  " prospect.Target_Refi_Rate = '{53}'," +
                                                  " prospect.User_ID = '{54}'," +
                                                  " prospect.Username = '{55}'," +
                                                  " prospect.Website = '{56}'," +
                                                  " prospect.Zip = '{57}' ",
                                                  psNewProspect.Address1,
                                                  psNewProspect.Address2,
                                                  psNewProspect.Appraised_Value,
                                                  psNewProspect.Birthdate,
                                                  psNewProspect.Cell_Phone_Area_Code,
                                                  psNewProspect.Cell_Phone_Number,
                                                  psNewProspect.Cell_Phone_Prefix,
                                                  psNewProspect.City,
                                                  psNewProspect.Closing_Date,
                                                  psNewProspect.Company,
                                                  psNewProspect.Contact_ID,
                                                  psNewProspect.Contact_Status,
                                                  psNewProspect.Date_Created,
                                                  psNewProspect.Date_Modified,
                                                  psNewProspect.Email_Address,
                                                  psNewProspect.Email_Notification,
                                                  psNewProspect.Fax_Area_Code,
                                                  psNewProspect.Fax_Number,
                                                  psNewProspect.Fax_Prefix,
                                                  psNewProspect.First_Name,
                                                  psNewProspect.Home_Phone_Area_Code,
                                                  psNewProspect.Home_Phone_Number,
                                                  psNewProspect.Home_Phone_Prefix,
                                                  psNewProspect.Integration_ID,
                                                  psNewProspect.Interest_Rate,
                                                  psNewProspect.Last_Name,
                                                  psNewProspect.Loan_Amount,
                                                  psNewProspect.Loan_Number,
                                                  psNewProspect.Loan_Type,
                                                  psNewProspect.LTV_Ratio,
                                                  psNewProspect.Monthly_Payment,
                                                  psNewProspect.Occupancy,
                                                  psNewProspect.Office_Phone_Area_Code,
                                                  psNewProspect.Office_Phone_Ext,
                                                  psNewProspect.Office_Phone_Number,
                                                  psNewProspect.Office_Phone_Prefix,
                                                  psNewProspect.Property_Type,
                                                  psNewProspect.Prospect_Type,
                                                  psNewProspect.Purchase_Price,
                                                  psNewProspect.Referral_Company,
                                                  psNewProspect.Referral_Email,
                                                  psNewProspect.Referral_First_Name,
                                                  psNewProspect.Referral_Last_Name,
                                                  psNewProspect.Referral_Phone_Area_Code,
                                                  psNewProspect.Referral_Phone_Number,
                                                  psNewProspect.Referral_Phone_Prefix,
                                                  psNewProspect.Referral_Type,
                                                  psNewProspect.Salutation,
                                                  psNewProspect.Secondary_Birthdate,
                                                  psNewProspect.Secondary_First_Name,
                                                  psNewProspect.Secondary_Last_Name,
                                                  psNewProspect.State,
                                                  psNewProspect.Suffix,
                                                  psNewProspect.Target_Refi_Rate,
                                                  psNewProspect.User_ID,
                                                  psNewProspect.Username,
                                                  psNewProspect.Website,
                                                  psNewProspect.Zip);

                        if (string.IsNullOrEmpty(psNewProspect.Username))
                        {
                            logErr = true;
                            Event_id = 6079;
                            err = "Leadstar.InsertProspect Fails(1204): Username is null";
                            return false;
                        }
                        else
                        {
                            err = LS.InsertProspect("MY", psNewProspect, LeadStar_APIKey, lsCompany.Company_Key);
                        }

                        switch (err.ToLower())
                        {
                            case "1000":
                                err = "";
                                logErr = false;
                                Event_id = 6070;
                                string ulSql = "update Loans set GlobalId='" + psNewProspect.Integration_ID + "' where FileId=" + FileId;
                                int status = DbHelperSQL.ExecuteSql(ulSql);
                                break;

                            case "1201":
                                logErr = true;
                                Event_id = 6071;
                                err = "Leadstar.InsertProspect Fails(1201): Empty IntegrationID";
                                return false;

                            case "1202":
                                logErr = true;
                                Event_id = 6072;
                                err = "Leadstar.InsertProspect Fails(1202): Bad Instance ID";
                                return false;

                            case "1203":
                                logErr = true;
                                Event_id = 6073;
                                err = "Leadstar.InsertProspect Fails(1203): Bad API_Key";
                                return false;

                            case "1204":
                                logErr = true;
                                Event_id = 6074;
                                err = "Leadstar.InsertProspect Fails(1204): Username not Found";
                                return false;

                            case "1205":
                                logErr = true;
                                Event_id = 6075;
                                err = "Leadstar.InsertProspect Fails(1205): Bad Company Key";
                                return false;

                            case "1206":
                                logErr = true;
                                Event_id = 6076;
                                err = "Leadstar.InsertProspect Fails(1206): Integration_ID must be unique across company";                                  
                                return false;

                            default:
                                logErr = true;
                                Event_id = 6077;
                                err = "Leadstar.InsertProspect Fails: " + err;
                                return false;
                        }
                        ShouldUpdateUser = false;
                    }
                    else
                        ShouldUpdateUser = true;

                    MarketingMgr.LeadStar.Prospect psGetProspect = LS.GetProspectByIntegrationID("MY", psNewProspect.Integration_ID, LeadStar_APIKey, lsCompany.Company_Key);
                    
                    if (psGetProspect == null)
                    {
                        if (ShouldUpdateUser == true)
                        {
                            logErr = true;
                            Event_id = 6089;
                            err = string.Format("Failed to get Prospect Record via GetProspectByIntegrationID, FileId={0}", FileId);
                            return false;
                        }
                        else
                        {
                            logErr = false;
                            return true;
                        }
                    }

                    psNewProspect.User_ID = psGetProspect.User_ID;
                    
                    st = SetUp_psGetProspect(ref psGetProspect,  psNewProspect, ref err);
                 
                    err = LS.UpdateProspect("MY", psGetProspect, LeadStar_APIKey, lsCompany.Company_Key);

                    err1 = string.Format("  [Message detail]: " +
                                                  " prospect.Address1 = '{0}'," +
                                                  " prospect.Address2 = '{1}'," +
                                                  " prospect.Appraised_Value = '{2}'," +
                                                  " prospect.Birthdate = '{3}'," +
                                                  " prospect.Cell_Phone_Area_Code = '{4}'," +
                                                  " prospect.Cell_Phone_Number = '{5}'," +
                                                  " prospect.Cell_Phone_Prefix = '{6}'," +
                                                  " prospect.City = '{7}'," +
                                                  " prospect.Closing_Date = '{8}'," +
                                                  " prospect.Company = '{9}'," +
                                                  " prospect.Contact_ID = '{10}'," +
                                                  " prospect.Contact_Status = '{11}'," +
                                                  " prospect.Date_Created = '{12}'," +
                                                  " prospect.Date_Modified = '{13}'," +
                                                  " prospect.Email_Address = '{14}'," +
                                                  " prospect.Email_Notification = '{15}'," +
                                                  " prospect.Fax_Area_Code = '{16}'," +
                                                  " prospect.Fax_Number = '{17}'," +
                                                  " prospect.Fax_Prefix = '{18}'," +
                                                  " prospect.First_Name = '{19}'," +
                                                  " prospect.Home_Phone_Area_Code = '{20}'," +
                                                  " prospect.Home_Phone_Number = '{21}'," +
                                                  " prospect.Home_Phone_Prefix = '{22}'," +
                                                  " prospect.Integration_ID = '{23}'," +
                                                  " prospect.Interest_Rate = '{24}'," +
                                                  " prospect.Last_Name = '{25}'," +
                                                  " prospect.Loan_Amount = '{26}'," +
                                                  " prospect.Loan_Number = '{27}'," +
                                                  " prospect.Loan_Type = '{28}'," +
                                                  " prospect.LTV_Ratio = '{29}'," +
                                                  " prospect.Monthly_Payment = '{30}'," +
                                                  " prospect.Occupancy = '{31}'," +
                                                  " prospect.Office_Phone_Area_Code = '{32}'," +
                                                  " prospect.Office_Phone_Ext = '{33}'," +
                                                  " prospect.Office_Phone_Number = '{34}'," +
                                                  " prospect.Office_Phone_Prefix = '{35}'," +
                                                  " prospect.Property_Type = '{36}'," +
                                                  " prospect.Prospect_Type = '{37}'," +
                                                  " prospect.Purchase_Price = '{38}'," +
                                                  " prospect.Referral_Company = '{39}'," +
                                                  " prospect.Referral_Email = '{40}'," +
                                                  " prospect.Referral_First_Name = '{41}'," +
                                                  " prospect.Referral_Last_Name = '{42}'," +
                                                  " prospect.Referral_Phone_Area_Code = '{43}'," +
                                                  " prospect.Referral_Phone_Number = '{44}'," +
                                                  " prospect.Referral_Phone_Prefix = '{45}'," +
                                                  " prospect.Referral_Type = '{46}'," +
                                                  " prospect.Salutation = '{47}'," +
                                                  " prospect.Secondary_Birthdate = '{48}'," +
                                                  " prospect.Secondary_First_Name = '{49}'," +
                                                  " prospect.Secondary_Last_Name = '{50}'," +
                                                  " prospect.State = '{51}'," +
                                                  " prospect.Suffix = '{52}'," +
                                                  " prospect.Target_Refi_Rate = '{53}'," +
                                                  " prospect.User_ID = '{54}'," +
                                                  " prospect.Username = '{55}'," +
                                                  " prospect.Website = '{56}'," +
                                                  " prospect.Zip = '{57}' ",
                                                  psGetProspect.Address1,
                                                  psGetProspect.Address2,
                                                  psGetProspect.Appraised_Value,
                                                  psGetProspect.Birthdate,
                                                  psGetProspect.Cell_Phone_Area_Code,
                                                  psGetProspect.Cell_Phone_Number,
                                                  psGetProspect.Cell_Phone_Prefix,
                                                  psGetProspect.City,
                                                  psGetProspect.Closing_Date,
                                                  psGetProspect.Company,
                                                  psGetProspect.Contact_ID,
                                                  psGetProspect.Contact_Status,
                                                  psGetProspect.Date_Created,
                                                  psGetProspect.Date_Modified,
                                                  psGetProspect.Email_Address,
                                                  psGetProspect.Email_Notification,
                                                  psGetProspect.Fax_Area_Code,
                                                  psGetProspect.Fax_Number,
                                                  psGetProspect.Fax_Prefix,
                                                  psGetProspect.First_Name,
                                                  psGetProspect.Home_Phone_Area_Code,
                                                  psGetProspect.Home_Phone_Number,
                                                  psGetProspect.Home_Phone_Prefix,
                                                  psGetProspect.Integration_ID,
                                                  psGetProspect.Interest_Rate,
                                                  psGetProspect.Last_Name,
                                                  psGetProspect.Loan_Amount,
                                                  psGetProspect.Loan_Number,
                                                  psGetProspect.Loan_Type,
                                                  psGetProspect.LTV_Ratio,
                                                  psGetProspect.Monthly_Payment,
                                                  psGetProspect.Occupancy,
                                                  psGetProspect.Office_Phone_Area_Code,
                                                  psGetProspect.Office_Phone_Ext,
                                                  psGetProspect.Office_Phone_Number,
                                                  psGetProspect.Office_Phone_Prefix,
                                                  psGetProspect.Property_Type,
                                                  psGetProspect.Prospect_Type,
                                                  psGetProspect.Purchase_Price,
                                                  psGetProspect.Referral_Company,
                                                  psGetProspect.Referral_Email,
                                                  psGetProspect.Referral_First_Name,
                                                  psGetProspect.Referral_Last_Name,
                                                  psGetProspect.Referral_Phone_Area_Code,
                                                  psGetProspect.Referral_Phone_Number,
                                                  psGetProspect.Referral_Phone_Prefix,
                                                  psGetProspect.Referral_Type,
                                                  psGetProspect.Salutation,
                                                  psGetProspect.Secondary_Birthdate,
                                                  psGetProspect.Secondary_First_Name,
                                                  psGetProspect.Secondary_Last_Name,
                                                  psGetProspect.State,
                                                  psGetProspect.Suffix,
                                                  psGetProspect.Target_Refi_Rate,
                                                  psGetProspect.User_ID,
                                                  psGetProspect.Username,
                                                  psGetProspect.Website,
                                                  psGetProspect.Zip);

                    switch (err.ToLower())
                    {
                        case "1000":
                            err = "";
                            logErr = false;
                            Event_id = 6080;
                            break;

                        case "1201":
                            logErr = true;
                            Event_id = 6081;
                            err = "Leadstar.UpdateProspect Fails(1201): Empty IntegrationID";
                            return false;                            

                        case "1202":
                            logErr = true;
                            Event_id = 6082;
                            err = "Leadstar.UpdateProspect Fails(1202): Bad Instance ID";
                            return false;
                        
                        case "1203":
                            logErr = true;
                            Event_id = 6083;
                            err = "Leadstar.UpdateProspect Fails(1203): Bad API_Key";
                            return false;

                        case "1204":
                            logErr = true;
                            Event_id = 6084;
                            err = "Leadstar.UpdateProspect Fails(1204): Username not Found";
                            return false;                            

                        case "1205":
                            logErr = true;
                            Event_id = 6085;
                            err = "Leadstar.UpdateProspect Fails(1205): Bad Company Key";
                            return false;                        

                        default:
                            logErr = true;
                            Event_id = 6086;
                            err = "Leadstar.UpdateProspect Fails: " + err;
                            return false;
                    }
           
                    return true;
                }
                      
            }
            catch (Exception ex)
            {
                err = string.Format("Failed to update Leadstar prospect, FileId={0}. Exception:{1}", FileId, ex.Message);
                logErr = false;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    err = err + err1;
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id);
                }
                else
                {
                    err = err1;
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id);
                }
            }
        }

        #endregion

        #region Update Pulse Campaign Info

        private int UpdateCampaignCategory(MarketingMgr.LeadStar.CampaignCategory category)
        {
            bool logErr = false;
            string err = string.Empty;
            string sqlCmd = string.Empty;
            int CategoryId = 0;
            try
            {
                sqlCmd = string.Format("select CategoryId from MarketingCategory where GlobalId='{0}'", category.CampaignCategoryKey);
                object objc = DbHelperSQL.GetSingle(sqlCmd);

                if (objc == null)
                {
                    sqlCmd = string.Format("Insert into MarketingCategory (CategoryName, GlobalId, [Description]) values ('{0}', '{1}', '{2}')",
                             category.CampaignCategoryValue, category.CampaignCategoryKey, category.CampaignCategoryDescription);
                    DbHelperSQL.ExecuteSql(sqlCmd);
                    sqlCmd = string.Format("select CategoryId from MarketingCategory where GlobalId='{0}'", category.CampaignCategoryKey);
                    objc = DbHelperSQL.GetSingle(sqlCmd);
                    CategoryId = objc == null || objc == DBNull.Value ? 0 : (int)objc;
                    return CategoryId;
                }
                else
                {
                    CategoryId = (int)objc;
                }

                sqlCmd = string.Format("Update MarketingCategory set CategoryName='{0}', GlobalId='{1}', [Description]='{2}' where CategoryId={3}",
                             category.CampaignCategoryValue, category.CampaignCategoryKey, category.CampaignCategoryDescription, CategoryId);
                DbHelperSQL.ExecuteSql(sqlCmd);
                return CategoryId;
            }
            catch (Exception ex)
            {
                logErr = true;
                err = "Failed to update Marketing Campaign Category, Exception: " + ex.Message;
                return CategoryId;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }  
        }

        private int UpdateCampaign(MarketingMgr.LeadStar.MarketingCampaign campaign, int Cid)
        {
            bool logErr = false;
            string err = string.Empty;
            string sqlCmd = string.Empty;
            int CampaignId = 0;
            
            try
            {
                sqlCmd = string.Format("Select CampaignId from MarketingCampaigns where GlobalId='{0}'", campaign.Marketing_Campaign_ID);
                object objc = DbHelperSQL.GetSingle(sqlCmd);
                if (objc == null || objc == DBNull.Value)
                {
                    sqlCmd = string.Format("Insert into MarketingCampaigns (CampaignName, GlobalId, [Description], Price, [Status], CategoryId) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                                            campaign.Campaign_Name, campaign.Marketing_Campaign_ID, campaign.Campaign_Description, campaign.Price, campaign.Status, Cid);
                    DbHelperSQL.ExecuteSql(sqlCmd);
                    sqlCmd = string.Format("Select CampaignId from MarketingCampaigns where GlobalId='{0}'", campaign.Marketing_Campaign_ID);
                    objc = DbHelperSQL.GetSingle(sqlCmd);
                    CampaignId = objc == null || objc == DBNull.Value ? 0 : (int)objc;
                    return CampaignId;
                }
                CampaignId = (int)objc;

                sqlCmd = string.Format("Update MarketingCampaigns set CampaignName='{0}', GlobalId='{1}', [Description]='{2}', Price='{3}', [Status]='{4}', CategoryId='{5}' where CampaignId='{6}'",
                         campaign.Campaign_Name, campaign.Marketing_Campaign_ID, campaign.Campaign_Description, campaign.Price, campaign.Status, Cid, CampaignId);
                DbHelperSQL.ExecuteSql(sqlCmd);
                return CampaignId;
            }
            catch (Exception ex)
            {
                logErr = true;
                err = "Failed to update Marketing Campaign, Exception: " + ex.Message;
                return CampaignId;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }  
        }
        
        private bool UpdateCampaignEvent (MarketingMgr.LeadStar.CampaignEvent campaignEvent)
        {
            bool logErr = false;
            string err = string.Empty;
            string sqlCmd = string.Empty;
            int CampId = 0;
            try
            {
                string Url = string.Empty;

                Url = @"CampaignImages/" + campaignEvent.EventIcon;
                string EventContent = campaignEvent.EventContent;
                if (campaignEvent.Action == "Email")
                    EventContent = @"CampaignEmails/" + EventContent;
                else
                    if (campaignEvent.Action == "Mail")
                        EventContent = @"CampaignImages/"+EventContent;
                sqlCmd = string.Format("Select CampaignEventId from MarketingCampaignEvents where GlobalId='{0}'", campaignEvent.Campaign_Event_ID);
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                if (obj == null)
                {
                    string sC = string.Format("select CampaignId from MarketingCampaigns where GlobalId='{0}' ", campaignEvent.Marketing_Campaign_ID);                    
                    object obj1 = DbHelperSQL.GetSingle(sC);
                    if (obj1 != null)
                    {
                        CampId = (int)obj1;
                    }
                    
                    sqlCmd = "Insert into MarketingCampaignEvents (Action, GlobalId, EventContent, EventURL, WeekNo, CampaignId) values (@Action, @GlobalId, @EventContent, @EventURL, @WeekNo, @CampaignId)";
                    SqlParameter[] parameters = 
                    {
                        //@Action, @GlobalId, @EventContent, @EventURL, @WeekNo, @CampaignId
                        new SqlParameter ("@Action", SqlDbType.NVarChar, 50),
                        new SqlParameter ("@GlobalId", SqlDbType.NVarChar, 255),
                        new SqlParameter ("@EventContent", SqlDbType.NVarChar),
                        new SqlParameter("@EventURL", SqlDbType.NVarChar, 255),
                        new SqlParameter("@WeekNo", SqlDbType.Int),                    
                        new SqlParameter("@CampaignId", SqlDbType.Int)
                    };

                    if (!string.IsNullOrEmpty(campaignEvent.Action))
                        parameters[0].Value = campaignEvent.Action;
                    else
                        parameters[0].Value = DBNull.Value;
                    if (!string.IsNullOrEmpty(campaignEvent.Campaign_Event_ID))
                        parameters[1].Value = campaignEvent.Campaign_Event_ID;
                    else
                        parameters[1].Value = DBNull.Value;
                    if (!string.IsNullOrEmpty(EventContent))
                        parameters[2].Value = EventContent;
                    else
                        parameters[2].Value = DBNull.Value;
                    if (!string.IsNullOrEmpty(Url))
                        parameters[3].Value = Url;
                    else
                        parameters[3].Value = DBNull.Value;
                    
                    parameters[4].Value = campaignEvent.Week;                  
                    parameters[5].Value = CampId;
                    DbHelperSQL.ExecuteSql(sqlCmd, parameters);
                    return true;
                }
                int EventId = (int)obj;
                sqlCmd = "Update MarketingCampaignEvents set Action=@Action, GlobalId=@GlobalId, WeekNo=@WeekNo, EventContent=@EventContent, EventURL=@EventURL where CampaignEventId=@CampaignEventId";
                SqlParameter [] Params1 = 
                {
                       new SqlParameter ("@Action", SqlDbType.NVarChar, 50),
                       new SqlParameter ("@GlobalId", SqlDbType.NVarChar, 255),
                       new SqlParameter("@WeekNo", SqlDbType.Int),                 
                       new SqlParameter ("@EventContent", SqlDbType.NVarChar),
                       new SqlParameter("@EventURL", SqlDbType.NVarChar, 255),
                       new SqlParameter("@CampaignEventId", SqlDbType.Int)           
                };
                if (!string.IsNullOrEmpty(campaignEvent.Action))
                    Params1[0].Value = campaignEvent.Action;
                else
                    Params1[0].Value = DBNull.Value;
                if (!string.IsNullOrEmpty(campaignEvent.Campaign_Event_ID))
                    Params1[1].Value = campaignEvent.Campaign_Event_ID;
                else
                    Params1[1].Value = DBNull.Value;
                Params1[2].Value = campaignEvent.Week;
                if (!string.IsNullOrEmpty(EventContent))
                    Params1[3].Value = EventContent;
                else
                    Params1[3].Value = DBNull.Value;
                if (!string.IsNullOrEmpty(Url))
                    Params1[4].Value = Url;
                else
                    Params1[4].Value = DBNull.Value;
                    
                Params1[5].Value = EventId;
                DbHelperSQL.ExecuteSql(sqlCmd, Params1);
                return true;
            }
            catch (Exception ex)
            {
                logErr = true;
                err = "Failed to update Marketing Campaign Event, Exception: " + ex.Message;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }    
        }

        private List<MarketingMgr.LeadStar.CampaignCategory> GetCampaignCategoryList()
        {
            List<MarketingMgr.LeadStar.CampaignCategory> categoryList = null;
            bool logErr = false;
            string err = string.Empty;
            string sqlCmd = string.Empty;
            try
            {
                using (MarketingMgr.LeadStar.LeadStarServiceClient LS = new LeadStarServiceClient())
                {
                    MarketingMgr.LeadStar.CampaignCategory[] campaignList =
                        LS.GetAllCampaignCategories(LeadStar_InstanceId, LeadStar_APIKey);
                    if (campaignList == null || campaignList.Length <= 0)
                    {
                        err = string.Format("No CampaignCategory has been returned from Leadstar, InstanceId={0}, APIKey={1}.",
                                            LeadStar_InstanceId, LeadStar_APIKey);
                        logErr = true;
                        return categoryList;
                    }
                    categoryList = campaignList.ToList<CampaignCategory>();
                }
                return categoryList;
            }
            catch (Exception ex)
            {
                logErr = true;
                err = "Failed to update Marketing Campaign, Exception: " + ex.Message;
                return categoryList;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }  
        }

        private List<MarketingMgr.LeadStar.CampaignEvent> GetMarketingEvents (string CampaignId)
        {
            List<MarketingMgr.LeadStar.CampaignEvent> eventList = null;
            bool logErr = false;
            string err = string.Empty;
            string CompanyKey = string.Empty;
            string LeadStarId = string.Empty;
            string IntegrationId = string.Empty;
            string APIKey = string.Empty;
            try
            {
                using (MarketingMgr.LeadStar.LeadStarServiceClient LS = new LeadStarServiceClient())
                {
                    bool MarketingEnabled = false;
                    GetCompanyLeadStarInfo(ref MarketingEnabled, ref LeadStarId, ref CompanyKey, ref IntegrationId, ref APIKey);
                    if (!MarketingEnabled)
                        return eventList;
                    MarketingMgr.LeadStar.CampaignEvent[] events =
                        LS.GetCampaignEventsByMarketingCampaign(LeadStar_InstanceId, LeadStar_APIKey, CampaignId);
                    if (events == null || events.Length <= 0)
                    {
                        err = string.Format("No Marketing Campaign has been returned from Leadstar, InstanceId={0}, APIKey={1}, Companykey={2}, Campaign={3}.",
                                            LeadStar_InstanceId, LeadStar_APIKey, CompanyKey, CampaignId);
                        logErr = true;
                        return eventList;
                    }
                    eventList = events.ToList<CampaignEvent>();
                }
                return eventList;
            }
            catch (Exception ex)
            {
                logErr = true;
                err = string.Format("Failed to get Marketing Campaigns from Leadstar, InstanceId={0}, APIKey={1}, Companykey={2}, Campaign={3}, Exception:{4}.",
                                   LeadStar_InstanceId, LeadStar_APIKey, CompanyKey, CampaignId, ex.Message);
                return eventList;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }  
        }

        private List<MarketingMgr.LeadStar.MarketingCampaign> GetMarketingCampaignList(string category)
        {
            List<MarketingMgr.LeadStar.MarketingCampaign> campaignList = null;
            bool logErr = false;
            string err = string.Empty;
            string CompanyKey = string.Empty;
            string LeadStarId = string.Empty;
            string IntegrationId = string.Empty;
            string APIKey = string.Empty;
            try
            {
                using (MarketingMgr.LeadStar.LeadStarServiceClient LS = new LeadStarServiceClient())
                {
                    bool MarketingEnabled = false;
                    GetCompanyLeadStarInfo(ref MarketingEnabled, ref LeadStarId, ref CompanyKey, ref IntegrationId, ref APIKey);
                    if (!MarketingEnabled)
                        return campaignList;
                    MarketingMgr.LeadStar.MarketingCampaign[] campaigns =
                        LS.GetMarketingCampaignsByCategory(LeadStar_InstanceId, LeadStar_APIKey, category, CompanyKey);
                    if (campaigns == null || campaigns.Length <= 0)
                    {
                        err = string.Format("No Marketing Campaign has been returned from Leadstar, InstanceId={0}, APIKey={1}, Companykey={2}, Category={3}.",
                                            LeadStar_InstanceId, LeadStar_APIKey, CompanyKey, category);
                        logErr = true;
                        return campaignList;
                    }
                    campaignList = campaigns.ToList<MarketingCampaign>();
                }
                return campaignList;
            }
            catch (Exception ex)
            {
                logErr = true;
                err = string.Format("Failed to get Marketing Campaigns from Leadstar, InstanceId={0}, APIKey={1}, Companykey={2}, Category={3}, Exception:{4}.",
                                   LeadStar_InstanceId, LeadStar_APIKey, CompanyKey, category, ex.Message);
                return campaignList;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }  

        }

        private bool LoadMarketingEvents(string campaignkey)
        {
            bool logErr = false;
            string err = string.Empty;
            string CompanyKey = string.Empty;
            try
            {
                List<MarketingMgr.LeadStar.CampaignEvent> eventList = GetMarketingEvents(campaignkey);
                if (eventList == null || eventList.Count <= 0)
                    return false;
                foreach (CampaignEvent ev in eventList)
                {
                    if (UpdateCampaignEvent(ev) == false)
                        continue;
                }
                return true;
            }
            catch (Exception ex)
            {
                logErr = true;
                err = string.Format("Failed to load Marketing Campaign Events from Leadstar, InstanceId={0}, APIKey={1}, Companykey={2},  Campaign={3}, Exception:{4}.",
                                   LeadStar_InstanceId, LeadStar_APIKey, CompanyKey, campaignkey, ex.Message);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }  
        }

        private bool LoadMarketingCampaigns(string categorykey, int Cid)
        {
            bool logErr = false;
            string err = string.Empty;
            string CompanyKey = string.Empty;
            int CampaignId = 0;
            try
            {
                List<MarketingCampaign> campaignList = GetMarketingCampaignList(categorykey);
                if (campaignList == null || campaignList.Count <= 0)
                    return false;
                foreach (MarketingCampaign mc in campaignList)
                {
                    if (mc.CoBranding.ToUpper() == "TRUE")
                        continue;
                    CampaignId = UpdateCampaign(mc,Cid);
                    if (CampaignId <= 0)
                        continue;
                    if (LoadMarketingEvents(mc.Marketing_Campaign_ID) == false)
                        continue;
                }

                return true;
            }
            catch (Exception ex)
            {
                logErr = true;
                err = string.Format("Failed to load Marketing Campaigns from Leadstar, InstanceId={0}, APIKey={1}, Companykey={2}, Category={3}, Exception:{4}.",
                                   LeadStar_InstanceId, LeadStar_APIKey, CompanyKey, categorykey, ex.Message);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }  
        }

        public bool LoadLeadStarMarketing()
        {
            bool logErr = false;
            int Event_id = 6091;
            string err = string.Empty;
            string CompanyKey = string.Empty;
            int CategoryId = 0;
            try
            {
                List<CampaignCategory> categoryList = GetCampaignCategoryList();
                if (categoryList == null || categoryList.Count <= 0)
                    return false;

                foreach (CampaignCategory cc in categoryList)
                {
                    CategoryId = UpdateCampaignCategory(cc);
                    if (CategoryId <= 0)
                        continue;
                    if (LoadMarketingCampaigns(cc.CampaignCategoryKey, CategoryId) == false)
                        continue;
                }

                return true;
            }
            catch (Exception ex)
            {
                logErr = true;
                err = string.Format("Failed to load Marketing Categories from Leadstar, InstanceId={0}, APIKey='{1}', Exception:'{2}'.",
                                   LeadStar_InstanceId, LeadStar_APIKey, ex.Message);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id);
                }
            }  
        }
        #endregion

        #region Prospect Campaign Management
        private int UpdateProspectCampaign(int FileId, ContactCampaign ContactCampaign, ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            int LoanMkgId = 0;
            try
            {
                string sqlCmd = string.Format("Select CampaignId from MarketingCampaigns where GlobalId='{0}'", ContactCampaign.Marketing_Campaign_ID);
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                int CampaignId = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
                if (CampaignId <= 0)
                {
                    err = string.Format("Cannot find the Marketing Campaign with LeadStarID='{0}'.", ContactCampaign.Marketing_Campaign_ID);
                    logErr = true;
                    return LoanMkgId;
                }
                sqlCmd = string.Format("Select LoanMarketingId from LoanMarketing where FileId='{0}' AND (LeadStarId='{1}' OR CampaignId='{2}')", FileId, ContactCampaign.Contact_Campaign_ID, CampaignId);
                obj = DbHelperSQL.GetSingle(sqlCmd);
 
                LoanMkgId = (obj == DBNull.Value || obj == null) ? 0 : (int)obj;
                if (LoanMkgId <= 0)
                {
                    sqlCmd = string.Format("Select UserId from Users where GlobalId='{0}'", ContactCampaign.User_Integration_ID);
                    obj = DbHelperSQL.GetSingle(sqlCmd);
                    int UserId = obj == DBNull.Value ? 0 : (int)obj;
                    sqlCmd = "Insert into LoanMarketing (Selected, Type, Started, StartedBy, CampaignId, Status, FileId, SelectedBy, LeadStarId) VALUES ";
                    sqlCmd += string.Format("('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')",
                                             ContactCampaign.Campaign_Start_Date.ToString(),
                                             "Auto",
                                             ContactCampaign.Campaign_Start_Date.ToString(),
                                             UserId,
                                             CampaignId,
                                             ContactCampaign.Campaign_Status,
                                             FileId,
                                             UserId,
                                             ContactCampaign.Contact_Campaign_ID
                                             );
                    DbHelperSQL.ExecuteSql(sqlCmd);
                    sqlCmd = string.Format("Select LoanMarketingId from LoanMarketing where FileId='{0}' AND (LeadStarId='{1}' OR CampaignId='{2}')", FileId, ContactCampaign.Contact_Campaign_ID, CampaignId);
                    obj = DbHelperSQL.GetSingle(sqlCmd);
                    LoanMkgId = (obj == DBNull.Value || obj == null) ? 0 : (int)obj;
                    return LoanMkgId;
                }
                sqlCmd = string.Format("Update LoanMarketing set Started='{0}', Status='{1}', LeadStarId='{2}' WHERE FileId='{3}' and LoanMarketingId='{4}'",
                                           ContactCampaign.Campaign_Start_Date.ToString(),
                                           ContactCampaign.Campaign_Status,
                                           ContactCampaign.Contact_Campaign_ID,
                                           FileId,
                                           LoanMkgId);
                DbHelperSQL.ExecuteSql(sqlCmd);
                return LoanMkgId;
            }
            catch (Exception ex)
            {
                err = string.Format("Failed to GetProspectCampaignEvents, FileId={0}. Exception:{1}", FileId, ex.Message);
                logErr = true;
                return LoanMkgId;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }  
        }

        private List<MarketingMgr.LeadStar.ContactCampaignEvent> GetProspectCampaignEvents(int FileId, string LeadStarCompanyKey, string LeadStarCampaignId, ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            List<MarketingMgr.LeadStar.ContactCampaignEvent> CampaignEvents_List = new List<MarketingMgr.LeadStar.ContactCampaignEvent>();
            MarketingMgr.LeadStar.ContactCampaignEvent[] CampaignEvents = null;
            try
            {
                using (MarketingMgr.LeadStar.LeadStarServiceClient lsClient = new MarketingMgr.LeadStar.LeadStarServiceClient())
                {
                    CampaignEvents = lsClient.GetEventsByContactCampaign(LeadStar_InstanceId, LeadStarCampaignId, LeadStar_APIKey, LeadStarCompanyKey);                          
                }
                return CampaignEvents_List;              
            }
            catch (Exception ex)
            {
                err = string.Format("Failed to GetProspectCampaignEvents, FileId={0}. Exception:{1}", FileId, ex.Message);
                logErr = true;
                return CampaignEvents_List;               
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }  
        }

        private int UpdateProspectCampaignEvents (int FileId, int LoanMarketingId, string LeadStarCampaignId, ContactCampaignEvent cce, ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            int LoanMarketingEventid = 0;
            string sqlCmd = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(cce.Contact_Campaign_Event_ID))
                {
                    err = string.Format("UpdateProspectCampaignEvents, Contact_Campaign_Event_ID is NULL, FileId={0}, LoanMarketingId={1}, LeadStarCampaignId={2}, WeekNo={3}",
                                        FileId, LoanMarketingId, LeadStarCampaignId, cce.Week);
                    return LoanMarketingEventid;
                }
                string Url = @"CampaignImages/" + cce.EventIcon;
                string EventContent = cce.EventContent;
                if (cce.Action == "Email")
                    EventContent = @"CampaignEmails/" + EventContent;
                else
                    if (cce.Action == "Mail")
                        EventContent = @"CampaignImages/" + EventContent;
                sqlCmd = string.Format("Select LoanMarketingEventId from LoanMarketingEvents where FileId='{0}' AND LoanMarketingId = '{1}' AND LeadStarEventId='{2}' ",
                                              FileId, LoanMarketingId, cce.Contact_Campaign_Event_ID);
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                LoanMarketingEventid = (obj == DBNull.Value || obj == null) ? 0 : (int)obj;
                if (LoanMarketingEventid <= 0)
                {
                    string Sql = "Insert into LoanMarketingEvents (Action, ExecutionDate, LoanMarketingId, Completed, WeekNo, EventContent, EventURL, LeadStarEventId, FileId) VALUES " +
                                 "(@Action, @ExecutionDate, @LoanMarketingId, @Completed, @WeekNo, @EventContent, @EventUrl, @LeadStarEventId, @FileId)";

                    SqlCommand SqlCmd = new SqlCommand(Sql);

                    if (!string.IsNullOrEmpty(cce.Action))
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@Action", SqlDbType.NVarChar, cce.Action);
                    else
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@Action", SqlDbType.NVarChar, DBNull.Value);

                    if (cce.Execution_Date != DateTime.MinValue)
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@ExecutionDate", SqlDbType.DateTime, cce.Execution_Date);
                    else
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@ExecutionDate", SqlDbType.DateTime, DBNull.Value);

                    DbHelperSQL.AddSqlParameter(SqlCmd, "@LoanMarketingId", SqlDbType.Int, LoanMarketingId);

                    if (cce.Completed)
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@Completed", SqlDbType.Bit, 1);
                    else
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@Completed", SqlDbType.Bit, 0);

                    DbHelperSQL.AddSqlParameter(SqlCmd, "@WeekNo", SqlDbType.Int, cce.Week);

                    if (!string.IsNullOrEmpty(EventContent))
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@EventContent", SqlDbType.NVarChar, EventContent);
                    else
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@EventContent", SqlDbType.NVarChar, DBNull.Value);

                    if (!string.IsNullOrEmpty(cce.EventIcon))
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@EventUrl", SqlDbType.NVarChar, cce.EventIcon);
                    else
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@EventUrl", SqlDbType.NVarChar, DBNull.Value);

                    if (!string.IsNullOrEmpty(cce.Contact_Campaign_Event_ID))
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@LeadStarEventId", SqlDbType.NVarChar, cce.Contact_Campaign_Event_ID);                        
                    else
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@LeadStarEventId", SqlDbType.NVarChar, DBNull.Value);

                    DbHelperSQL.AddSqlParameter(SqlCmd, "@FileId", SqlDbType.Int, FileId);
                 
                    DbHelperSQL.ExecuteNonQuery(SqlCmd);   

                    sqlCmd = string.Format("Select LoanMarketingEventId from LoanMarketingEvents where FileId={0} AND LeadStarEventId='{1}' ",
                              FileId, cce.Contact_Campaign_Event_ID);
                    obj = DbHelperSQL.GetSingle(sqlCmd);
                    LoanMarketingEventid = obj == DBNull.Value ? 0 : (int)obj;
                    return LoanMarketingEventid;
                }
                sqlCmd = string.Format("Update LoanMarketingEvents set ExecutionDate='{0}', Completed='{1}', LoanMarketingId='{2}', LeadStarEventId='{3}' WHERE LoanMarketingEventId={4} AND FileID={5}",
                    cce.Execution_Date.ToString(), cce.Completed.ToString(), LoanMarketingId, cce.Contact_Campaign_Event_ID, LoanMarketingEventid, FileId);
                DbHelperSQL.ExecuteSql(sqlCmd);
                return LoanMarketingEventid;
            }
            catch (Exception ex)
            {
                err = string.Format("Failed to UpdateProspectCampaignEvents, FileId={0}, LeadStarEventId={1}. Exception:{1}", FileId, cce.Contact_Campaign_Event_ID, ex.Message);
                logErr = true;
                return LoanMarketingEventid;  
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }  
        }

        private List<string> GetProspectCampaigns(int FileId, ref string err)
        {
            List<string> ContactCampaignList = null;
            err = string.Empty;
            bool logErr = false;
            try
            {
                MarketingMgr.LeadStar.Company lsCompany = SetupCompanyInfo(ref err);
                if (lsCompany == null)
                    return ContactCampaignList;
                string sqlCmd = string.Format("Select GlobalId from Loans where FileId={0} ", FileId);
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                string prospectIntegrationId = (string)obj;
                if (string.IsNullOrEmpty(prospectIntegrationId))
                {
                    //logErr = true;
                    //err = "No Integration Id found for the specified loan, FileId=" + FileId;
                    //return ContactCampaignList;
                    prospectIntegrationId = lsCompany.Integration_ID + "_l" + FileId.ToString();                   
                }

                using (MarketingMgr.LeadStar.LeadStarServiceClient lsClient = new MarketingMgr.LeadStar.LeadStarServiceClient())
                {
                    string strContactCampaignID = "5312bae4-9167-4e56-a916-e366ba080f88";
                    MarketingMgr.LeadStar.ContactCampaignEvent[] arContactCampaignevents = lsClient.GetEventsByContactCampaign(LeadStar_InstanceId, strContactCampaignID, LeadStar_APIKey, lsCompany.Company_Key);
                    List<MarketingMgr.LeadStar.ContactCampaignEvent> ContactCampaignEvent_List = new List<MarketingMgr.LeadStar.ContactCampaignEvent>();
                    if ((arContactCampaignevents != null) && (arContactCampaignevents.Length > 0))
                    ContactCampaignEvent_List = arContactCampaignevents.ToList<MarketingMgr.LeadStar.ContactCampaignEvent>();


                    prospectIntegrationId = "5598a3e7_46c4_49c5_bc60_a96d5484786f_l289";
                    lsCompany.Company_Key = "5598a3e7-46c4-49c5-bc60-a96d5484786f";
                        MarketingMgr.LeadStar.ContactCampaign[] ContactCampaigns = lsClient.GetContactCampaignsByProspect(LeadStar_InstanceId, prospectIntegrationId, LeadStar_APIKey, lsCompany.Company_Key);
               
                    if (ContactCampaigns == null || ContactCampaigns.Length <= 0)
                        return ContactCampaignList;
                //    ContactCampaignList = ContactCampaigns.ToList<ContactCampaign>();
                }
                return ContactCampaignList;
            }
            catch (Exception ex)
            {
                err = string.Format("Failed to GetProspectCampaign, FileId={0}. Exception:{1}", FileId, ex.Message);
                logErr = true;
                return ContactCampaignList;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }  
        }

        public bool LoadProspectCampaigns(int FileId, ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            int LoanMarketingId = 0;
            string LoanMarketing_LeadStarId = "";

            try
            {
                MarketingMgr.LeadStar.Company lsCompany = null;

                lsCompany = SetupCompanyInfo(ref err);
            
                if (lsCompany == null)
                {
                    logErr = true;
                    return false;
                }

                string lmSql = "select LeadStarId, LoanMarketingId from LoanMarketing where FileId=" + FileId;
                DataTable LoanMarketing = DbHelperSQL.ExecuteDataTable(lmSql);

                if (LoanMarketing == null || LoanMarketing.Rows.Count == 0)
                {                   
                    return true;
                }                

                foreach (DataRow dr in LoanMarketing.Rows)
                {              
                if (dr["LeadStarId"] == DBNull.Value)
                {
                    continue;
                }

                LoanMarketing_LeadStarId = dr["LeadStarId"].ToString().Trim();
                LoanMarketingId = (int) dr["LoanMarketingId"];
                
                using (MarketingMgr.LeadStar.LeadStarServiceClient lsClient = new MarketingMgr.LeadStar.LeadStarServiceClient())
                {
                //string strContactCampaignID = "5312bae4-9167-4e56-a916-e366ba080f88";
                
                List<MarketingMgr.LeadStar.ContactCampaignEvent> ContactCampaignEvent_List = new List<MarketingMgr.LeadStar.ContactCampaignEvent>();
                MarketingMgr.LeadStar.ContactCampaignEvent[] arContactCampaignevents = lsClient.GetEventsByContactCampaign(LeadStar_InstanceId, LoanMarketing_LeadStarId, LeadStar_APIKey, lsCompany.Company_Key);
                
                if ((arContactCampaignevents == null) || (arContactCampaignevents.Length == 0))
                {
                    continue;
                }

                ContactCampaignEvent_List = arContactCampaignevents.ToList<MarketingMgr.LeadStar.ContactCampaignEvent>();

                int LoanMarketingEventId = 0;

                foreach (ContactCampaignEvent ce in ContactCampaignEvent_List )
                {
                    if ((ce == null) || (string.IsNullOrEmpty(ce.Contact_Campaign_Event_ID)))
                    {
                        continue;
                    }

                    LoanMarketingEventId = UpdateProspectCampaignEvents(FileId, LoanMarketingId, LoanMarketing_LeadStarId, ce, ref err);
                }

                }

                }

                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("Failed to LoadProspectCampaigns, FileId={0}. Exception:{1}", FileId, ex.Message);
                logErr = true;
                return false;               
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }  
        }

        public bool Scheduled_MarketingEvents(ref string err)
        {
            bool success = true;
            int FileId = 0;           
            err = string.Empty;
            bool logErr = false;
            DataSet ds = null;
            List<int> fileList = new List<int>();
           
            try
            {
                string sqlCmd = "select distinct fileid from loanmarketing";
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                {                   
                    return true;
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr == null)
                        continue;

                    FileId = (int)dr["FileId"];

                    success = LoadProspectCampaigns(FileId, ref err);                   
                }

                return true;
            }
            catch (Exception ex)
            {
                err = "Failed to Complete Scheduled_MarketingEvents, Exception:" + ex.Message;
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }  

        }

        public bool GetAllProspectCampaigns(ref string err)
        {
            return true;
        }

        private bool Setup_lsCCOrder(StartCampaignRequest req, ref ContactCampaignsOrder lsCCOrder, ref int LoanMarketingId, ref string err)
        {
            err = string.Empty;
            bool success = true;
            int Group_id = 0;
            int Branch_id = 0;
            DateTime DTN = DateTime.Now;
            string Company_Username = string.Empty;
            string LeadStar_CampaignId = string.Empty;
            string LeadStar_UserId = string.Empty;
            string LeadStar_ProspectId = string.Empty;
            string CampaignType = "Auto";
            LoanMarketingId = 0;

            if (req.Auto == false)
            {
                CampaignType = "Manual";
            }
            else
            {
                req.payer = Payer_Type.Company;
            }

            if (req.hdr == null || req.CampaignId <= 0 || req.FileId.Length <= 0)
            {
                err = "Invalid StartCampaignRequest, missing req hdr, campaignId or FileId";
                return false;
            }

            MarketingMgr.LeadStar.Company lsCompany = null;

            lsCompany = SetupCompanyInfo(ref err);

            if (lsCompany == null)
            {
                err = "Failed to read company_general";
                return false;
            }

            LeadStar_CompanyKey = lsCompany.Company_Key;           
            Company_Username = lsCompany.Company_Manager_Username;

            object obj = DbHelperSQL.GetSingle("Select top 1 GlobalId from MarketingCampaigns where CampaignId=" + req.CampaignId.ToString());
            if (obj == null)
            {
                err = "Failed to read Leadstar MarketingCampaign ID from MarketingCampaigns where CampaignId=" + req.CampaignId.ToString();
                return false;
            }
            else
            {
                LeadStar_CampaignId = (string)obj;
            }

            if (req.payer == Payer_Type.Company)
            {
                if (Company_Username == null)
                {
                    err = "Failed to read LeadStar_username from Company_General where username = null";
                    return false;
                }

                string cmd1 = string.Format("Select top 1 GlobalId from Users where Username='{0}'",  Company_Username.ToString());

                object obj1 = DbHelperSQL.GetSingle(cmd1);
                if (obj1 == null)
                {
                    err = "Failed to read Leadstar User ID from Users where Username=" + Company_Username.ToString();
                    return false;
                }
                else
                {
                    LeadStar_UserId = (string)obj1;
                }
            }
       
            if (req.payer == Payer_Type.Branch)
            {                
                string cmd2 = string.Format("Select top 1 GroupID from GroupUsers where UserID='{0}'", req.hdr.UserId.ToString());
                object obj1 = DbHelperSQL.GetSingle(cmd2);
                if (obj1 == null)
                {
                    err = "Failed to read GroupID from GroupUsers where UserID=" + req.hdr.UserId.ToString();
                    return false;
                }
                else
                {
                    Group_id = (int)obj1;
                }

                string cmd3 = string.Format("Select top 1 BranchID from Groups where GroupId='{0}'", Group_id.ToString());
                object obj2 = DbHelperSQL.GetSingle(cmd3);
                if (obj2 == null)
                {
                    err = "Failed to read BranchID from Groups where GroupId=" + Group_id.ToString();
                    return false;
                }
                else
                {
                    Branch_id = (int)obj2;
                }

                string cmd4 = string.Format("Select top 1 Leadstar_Userid from Branches where BranchId='{0}'" + Branch_id.ToString());
                object obj3 = DbHelperSQL.GetSingle(cmd4);
                if (obj3 == null)
                {
                    err = "Failed to read Leadstar User ID from Branches where BranchId=" + Branch_id.ToString();
                    return false;
                }
                else
                {
                    LeadStar_UserId = (string)obj3;
                }
            }

            if (req.payer == Payer_Type.User)
            {
                object obj1 = DbHelperSQL.GetSingle("Select top 1 GlobalId from Users where UserId=" + req.hdr.UserId.ToString());
                if (obj1 == null)
                {
                    err = "Failed to read Leadstar User ID from Users where UserId=" + req.hdr.UserId.ToString();
                    return false;
                }
                else
                {
                    LeadStar_UserId = (string)obj1;
                }
            }   

            lsCCOrder.API_Key = LeadStar_APIKey;

            if (req.StartDate == DateTime.MinValue)
                lsCCOrder.Campaign_Start_Date = DateTime.Now;
            else
                lsCCOrder.Campaign_Start_Date = req.StartDate;

            lsCCOrder.Instance_ID = LeadStar_InstanceId;
            lsCCOrder.Marketing_Campaign_ID = LeadStar_CampaignId;
            lsCCOrder.Marketing_Partner_ID = "";
            lsCCOrder.User_Integration_ID = LeadStar_UserId;
            lsCCOrder.Company_Key = LeadStar_CompanyKey;

            int idx = req.FileId.Length;

            List<string> Prospects_List = new List<string>();

            for (int i = 0; i < idx; i++)
            {

                object obj2 = DbHelperSQL.GetSingle("Select top 1 GlobalId from Loans where FileId=" + req.FileId[i].ToString());
                if (obj2 == null)
                {
                    continue;
                }
                else
                {
                    LeadStar_ProspectId = (string)obj2;
                }

                Prospects_List.Add(LeadStar_ProspectId);

                string SqlCmd3 = string.Format("Select top 1 LoanMarketingId from LoanMarketing where FileId={0} and CampaignId={1}", req.FileId[i], req.CampaignId);
                object obj3 = DbHelperSQL.GetSingle(SqlCmd3);
                if (obj3 == null)
                {
                    string SqlCmd4 = "Insert into LoanMarketing (Selected, Type, Started, StartedBy, CampaignId, Status, FileId, SelectedBy) VALUES";
                    SqlCmd4 += string.Format("('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}' )",
                                 DTN, CampaignType, lsCCOrder.Campaign_Start_Date, req.hdr.UserId, req.CampaignId, "Active", req.FileId[i], req.hdr.UserId);
                    DbHelperSQL.ExecuteSql(SqlCmd4);

                    string SqlCmd5 = string.Format("Select top 1 LoanMarketingId from LoanMarketing where FileId={0} and CampaignId={1}", req.FileId[i], req.CampaignId);
                    object obj5 = DbHelperSQL.GetSingle(SqlCmd5);
                    if (obj5 != null)
                    {
                        LoanMarketingId = (int)obj5;
                    }
                    else
                    {
                        LoanMarketingId = 0;
                        err = string.Format("Failed to read LoanMarketingId from LoanMarketing where FileId={0} and CampaignId={1} ", req.FileId[i], req.CampaignId);                        
                    }
                }
                else
                {
                    LoanMarketingId = (int)obj3;
                }
            }

            lsCCOrder.LstProspectIds = Prospects_List.ToArray();

            if (LoanMarketingId > 0)
            {
                return true;
            }
            else
            {
                err = string.Format("Failed to read LoanMarketingId from LoanMarketing where FileId={0} and CampaignId={1}", req.FileId[0], req.CampaignId);
                return false;
            }
        }

        public StartCampaignResponse StartCampaign(StartCampaignRequest req, ref string err)
        {
            err = string.Empty;
            string err1 = string.Empty;
            int Event_id = 6501;
            bool logErr = false;
            bool queCampaign = false;
            int LoanMarketingId = 0;
            bool success = true;
            bool bIsAnyError = false;
            string CampaignType = "Auto";

            StartCampaignResponse resp = new StartCampaignResponse();
            resp.hdr = new RespHdr();
            resp.ErrorCode = "";

            if (req.Auto == false)
            {
                CampaignType = "Manual";
            }

            MarketingMgr.LeadStar.ContactCampaignsOrder lsCCOrder = new MarketingMgr.LeadStar.ContactCampaignsOrder();
            MarketingMgr.LeadStar.PayerType LeadStar_Payer = PayerType.User;

            if (req.Auto == true)
            {
                req.payer = Payer_Type.Company;
            }

            if (req.payer == Payer_Type.Company)
            {
                LeadStar_Payer = PayerType.Company;
            }
            else
                if (req.payer == Payer_Type.Branch)
                {
                    LeadStar_Payer = PayerType.Branch;
                }

            try
            {
                success = Setup_lsCCOrder(req, ref lsCCOrder, ref LoanMarketingId, ref err);

                if (success == false)
                {
                    logErr = false;
                    Event_id = 6501;
                    resp.hdr.Successful = false;
                    resp.ErrorCode = "9999";
                    return resp;
                }

                using (LeadStarServiceClient LS = new LeadStarServiceClient())
                {
                    string[] scco_array = new string[20];

                    scco_array = LS.SaveContactCampaignsOrder(lsCCOrder, LeadStar_Payer);

                    err1 = string.Format("  [Message detail]: " +
                                             " lsCCOrder.API_Key = '{0}'," +
                                             " lsCCOrder.Campaign_Start_Date = '{1}'," +
                                             " lsCCOrder.Company_Key = '{2}'," +
                                             " lsCCOrder.Instance_ID = '{3}'," +
                                             " lsCCOrder.LstProspectIds = '{4}'," +
                                             " lsCCOrder.Marketing_Campaign_ID = '{5}'," +
                                             " lsCCOrder.Marketing_Partner_ID = '{6}'," +
                                             " lsCCOrder.User_Integration_ID = '{7}'",
                                             lsCCOrder.API_Key,
                                             lsCCOrder.Campaign_Start_Date,
                                             lsCCOrder.Company_Key,
                                             lsCCOrder.Instance_ID,
                                             lsCCOrder.LstProspectIds[0].ToString(),
                                             lsCCOrder.Marketing_Campaign_ID,
                                             lsCCOrder.Marketing_Partner_ID,
                                             lsCCOrder.User_Integration_ID);
      
                    if ((scco_array == null) || (scco_array.Length == 0))
                    {
                        logErr = true;
                        success = false;
                        resp.ErrorCode = "9999";
                        Event_id = 6598;
                        err = string.Format("CampaignType: {0} StartCampaign Leadstar.SaveContactCampaignsOrder Fails: return null", CampaignType.ToString());                       
                        resp.hdr.Successful = false;
                        resp.hdr.StatusInfo = err;
                        return resp;
                    }

                    resp.ErrorCode = "";
                    success = true;
                    err = "";

                    int idx = scco_array.Length;

                    for (int i = 0; i < idx; i++)
                    {

                        string scco = scco_array[i];

                        switch (scco.Trim().ToLower())
                        {
                            case "1202":
                                logErr = true;
                                resp.ErrorCode = "1202";
                                success = false;
                                bIsAnyError = true;
                                Event_id = 6502;
                                err = string.Format("CampaignType: {0} StartCampaign Leadstar.SaveContactCampaignsOrder Fails(1202): Bad Instance ID", CampaignType.ToString());                                
                                break;

                            case "1203":
                                logErr = true;
                                resp.ErrorCode = "1203";
                                success = false;
                                bIsAnyError = true;
                                Event_id = 6503;
                                err = string.Format("CampaignType: {0} StartCampaign Leadstar.SaveContactCampaignsOrder Fails(1203): Bad API_Key", CampaignType.ToString());                               
                                break;

                            case "1213":
                                logErr = true;
                                resp.ErrorCode = "1213";
                                success = false;
                                bIsAnyError = true;
                                Event_id = 6513;
                                err = string.Format("CampaignType: {0} StartCampaign Leadstar.SaveContactCampaignsOrder Fails(1213): User Integration ID, at least one Prospect and Marketing Campaign required", CampaignType.ToString());                                 
                                break;

                            case "1219":
                                logErr = true;
                                resp.ErrorCode = "1219";
                                success = false;
                                bIsAnyError = true;
                                Event_id = 6519;
                                err = string.Format("CampaignType: {0} StartCampaign Leadstar.SaveContactCampaignsOrder Fails(1219): Insufficient Funds", CampaignType.ToString());                                
                                break;

                            default:
                                Guid guidTemp = Guid.Empty;
                                try
                                {
                                    guidTemp = new Guid(scco);
                                }
                                catch
                                {
                                    guidTemp = Guid.Empty;
                                }

                                if (guidTemp != Guid.Empty)
                                {
                                    string SqlCmd3 = string.Format("Select top 1 LoanMarketingId from LoanMarketing where FileId={0} and CampaignId={1} and Status='Active'", req.FileId[i].ToString(), req.CampaignId.ToString());
                                    object o3 = DbHelperSQL.GetSingle(SqlCmd3);
                                    if (o3 != null)
                                    {
                                        success = true;
                                        LoanMarketingId = (int)o3;
                                        string SqlCmd5 = "Update LoanMarketing set LeadStarId ='" + scco + "' where LoanMarketingId=" + LoanMarketingId;
                                        DbHelperSQL.ExecuteSql(SqlCmd5);
                                    }
                                    else
                                    {
                                        success = false;
                                        bIsAnyError = true;
                                        Event_id = 6520;              
                                        err = string.Format("CampaignType: {0} StartCampaign Failed to update LoanMarketing where FileId={1} and CampaignId={2} ", CampaignType.ToString(), req.FileId[i].ToString(), req.CampaignId.ToString());                                        
                                    }
                                }
                                else
                                {
                                    success = false;
                                    bIsAnyError = true;
                                    err = string.Format("CampaignType: {0} StartCampaign Leadstar.SaveContactCampaignsOrder Fails(1219): Unknown error from LeadStar Service: {1} ", CampaignType.ToString(), scco.Trim().ToLower().ToString());
                                }

                                break;
                        }

                        if (success == false)
                        {
                            string SqlCmd6 = "Update LoanMarketing set Status = 'Error' where LoanMarketingId=" + LoanMarketingId;
                            DbHelperSQL.ExecuteSql(SqlCmd6);
                        }

                        string SqlCmd11 = "Insert into MarketingLog (LoanMarketingId, EventTime, Success, Error) VALUES";
                        SqlCmd11 += string.Format("('{0}', '{1}', '{2}', '{3}' )",
                                           LoanMarketingId, DateTime.Now, success, err);
                        DbHelperSQL.ExecuteSql(SqlCmd11);
                    }
                }

                resp.hdr.Successful = !bIsAnyError;
                resp.hdr.StatusInfo = err;
                return resp;

            }
            catch (Exception ex)
            {
                logErr = true;
                err = string.Format("CampaignType:{0} StartCampaign Leadstar.SaveContactCampaignsOrder Exception: ", CampaignType.ToString(), ex.Message);
                resp.ErrorCode = "9999";
                Event_id = 6599;
                resp.hdr.Successful = false;
                resp.hdr.StatusInfo = err;
                return resp;
            }
            finally
            {
                if (logErr)
                {
                    err = err + err1;
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id);
                }
                else
                {
                    err = err1;
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id);
                }

                if (queCampaign && LoanMarketingId > 0)
                {
                    string sql = "Insert into MarketingQue (QueTime, Type, LoanMarketingId) values ";
                    sql += string.Format("(GetDate, '{0}','{1}'", CampaignType, LoanMarketingId);
                    DbHelperSQL.ExecuteSql(sql);
                }
            }
        }

        public bool RemoveCampaign(RemoveCampaignRequest req, ref string err)
        {
            err = string.Empty;
            bool success = true;
            bool logErr = false;
            string contact_Campaign_ID = "";
            int LoanMarketingId = 0;

            try
            {
                if (req.hdr == null || req.CampaignId <= 0 || req.FileId <= 0)
                {
                    logErr = true;
                    err = "Invalid StartCampaignRequest, missing req hdr, campaignId or FileId";
                    return false;
                }

                MarketingMgr.LeadStar.Company lsCompany = null;

                lsCompany = SetupCompanyInfo(ref err);

                if (lsCompany == null)
                {
                    logErr = true;
                    err = "Failed to read company_general";
                    return false;
                }

                if (lsCompany.Company_Key.Length < 32)
                {
                    success = UpdateCompany(ref err);
                    lsCompany = SetupCompanyInfo(ref err);
                }

                LeadStar_CompanyKey = lsCompany.Company_Key;

                string SqlCmd1 = string.Format("Select top 1 LoanMarketingId from LoanMarketing where FileId={0} and CampaignId={1} and Status='Active'", req.FileId, req.CampaignId);
                object obj1 = DbHelperSQL.GetSingle(SqlCmd1);
                if (obj1 == null)
                {
                    logErr = true;
                    err = "Failed to read LoanMarketing";                  
                    return false;                    
                }
                else
                {
                    LoanMarketingId = (int)obj1;
                }

                string SqlCmd2 = string.Format("Select top 1 LeadStarId from LoanMarketing where LoanMarketingId={0}", LoanMarketingId);
                object obj2 = DbHelperSQL.GetSingle(SqlCmd2);
                if (obj2 == null)
                {
                    logErr = true;
                    err = "Failed to read LoanMarketing";
                    return false;
                }
                else
                {
                    contact_Campaign_ID = (string)obj2;
                }

                using (LeadStarServiceClient LS = new LeadStarServiceClient())
                {
                    MarketingMgr.LeadStar.ContactCampaignEvent[] arContactCampaignevents = LS.DeleteContactCampaign(LeadStar_InstanceId, contact_Campaign_ID, LeadStar_APIKey, LeadStar_CompanyKey);                  
                    var sortedCCEList = from cl in arContactCampaignevents 
                                        orderby cl.Execution_Date
                                        select cl;
                    List<MarketingMgr.LeadStar.ContactCampaignEvent> CCE = sortedCCEList.ToList<MarketingMgr.LeadStar.ContactCampaignEvent>();
                   
                    string SqlCmd3 = "Update LoanMarketing set Status = 'Removed' where LoanMarketingId=" + LoanMarketingId;
                    DbHelperSQL.ExecuteSql(SqlCmd3);                   

                    string SqlCmd4 = "Insert into MarketingLog (LoanMarketingId, EventTime, Success, Error) VALUES";
                    SqlCmd4 += string.Format("('{0}', '{1}', '{2}', '{3}' )",
                                       LoanMarketingId, DateTime.Now, success, err);
                    DbHelperSQL.ExecuteSql(SqlCmd4);    
                }
            }
            catch (Exception ex)
            {
                err = "Failed to remove Marketing Campaign, Exception: " + ex.Message;
                // return false;
                return true;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }  
            return true;
        }

        public CompleteCampaignEventResponse CompleteCampaignEvent(CompleteCampaignEventRequest req, ref string err)
        {
            CompleteCampaignEventResponse resp = new CompleteCampaignEventResponse();
            resp.hdr = new RespHdr();
            return resp;
        }

        #endregion

        #region Sync Marketing Data

        private List<int> GetMarketingBranchList(ref string err)    
        {
            err = string.Empty;
            bool logErr = false;
            DataSet ds = null;
            List<int> branchList = new List<int>();
            int BranchId = 0;

            try
            {
                string sqlCmd = "select BranchId from Branches";
                ds = DbHelperSQL.Query(sqlCmd);

                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, "No Branch records to process.");
                    return branchList;
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {                 
                    if (dr == null)                  
                        continue;

                    BranchId = (int)dr["BranchId"];

                    if (BranchId > 0)
                    {
                        branchList.Add(BranchId);
                    }
                }

                return branchList;
            }
            catch (Exception ex)
            {
                err = "Failed to GetMarketingBranchList, Exception:" + ex.Message;
                logErr = true;
                return branchList;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }
  
        private bool ProcessMarketingBranchList(ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            bool status = true;
       
            try
            {
                List<int> branchList = GetMarketingBranchList(ref err);
                if (branchList == null || branchList.Count <= 0)
                    return true;
                foreach (int branchid in branchList)
                {
                    if (branchid > 0)
                    {
                        status = UpdateBranch(branchid, ref err);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Failed to update Leadstar branch, Exception:" + ex.Message;
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }

        private List<int> GetMarketingUserList(ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            bool UserEnabled = true;
            DataSet ds = null;
            List<int> userList = new List<int>();
            int UserId = 0;

            try
            {
                string sqlCmd = "select UserId, UserEnabled from Users";
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, "No User records to process.");
                    return userList;
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr == null)                  
                        continue;

                    UserId = (int)dr["UserId"];

                    if (UserId > 0)
                    {
                        if (dr["UserEnabled"] == DBNull.Value)
                            UserEnabled = false;
                        else
                            UserEnabled = Convert.ToBoolean(dr["UserEnabled"]);
                        if (UserEnabled == true)
                        {
                            userList.Add(UserId);
                        }
                    }
                }

                return userList;
            }
            catch (Exception ex)
            {
                err = "Failed to GetMarketingUserList, Exception:" + ex.Message;
                logErr = true;
                return userList;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }

        }

        private bool ProcessMarketingUserList(ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            bool status = true;
       
            try
            {
                List<int> userList = GetMarketingUserList(ref err);
                if (userList == null || userList.Count <= 0)
                    return true;
                foreach (int userid in userList)
                {               
                    if (userid > 0)
                    {
                        status = UpdateUser(userid, ref err);                       
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Failed to update Leadstar user, Exception:" + ex.Message;
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }

        private List<int> GetMarketingProspectList(ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            DataSet ds = null;
            List<int> fileList = new List<int>();
            int FileId = 0;

            try
            {
                string sqlCmd = "select FileId from Loans";
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                {
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, "No User records to process.");
                    return fileList;
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr == null)
                        continue;

                    FileId = (int)dr["FileId"];

                    if (FileId > 0)
                    {
                        fileList.Add(FileId);
                    }
                }

                return fileList;
            }
            catch (Exception ex)
            {
                err = "Failed to GetMarketingProspectList, Exception:" + ex.Message;
                logErr = true;
                return fileList;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }

        private bool ProcessMarketingProspectList(ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            bool status = true;

            try
            {
                List<int> fileList = GetMarketingProspectList(ref err);
                if (fileList == null || fileList.Count <= 0)
                    return true;
                foreach (int fileid in fileList)
                {
                    if (fileid > 0)
                    {
                        status = UpdateProspect(fileid, ref err);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Failed to update Leadstar prospect, Exception:" + ex.Message;
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }

        public bool ProcessSyncMarketingData(ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            bool StartMarketingSync = false;

            try
            {
                object obj = DbHelperSQL.GetSingle("Select top 1 StartMarketingSync from dbo.Company_General");
                StartMarketingSync = (obj == null || obj == DBNull.Value) ? false : (bool)obj;
                if (!StartMarketingSync || SyncQueued)
                {
                    if (SyncQueued)
                    {
                        err = "There is already a Sync Marketing Request queued.";
                        Trace.TraceInformation(err);
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information);
                        return false;
                    }
                    return true;
                }
                err = "Starting to sync marketing data now...";
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information);
                //ThreadPool.QueueUserWorkItem(new WaitCallback(SyncMarketingData), (object)err);
                m_ThreadContext.Post(new SendOrPostCallback(SyncMarketingData), err);
                return true;
            }
            catch (Exception ex)
            {
                err = "Failed to Sync Marketing Data, Exception:" + ex.Message+"\n"+ex.StackTrace;
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }
        private void SyncMarketingData(object obj)
        {
            string err = string.Empty;
            bool logErr = false;
            bool status = true;
       
            try
            {
                SyncQueued = true;
                status = UpdateCompany(ref err);
                err = "Sync marketing data, completed UpdateCompany...";
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information);
                status = LoadLeadStarMarketing();
                err = "Sync marketing data, completed LoadLeadStarMarketing... status = "+status.ToString();
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information);
                SyncQueued = false;
            }
            catch (Exception ex)
            {
                err = "Failed to Sync Marketing Data with LeadStar, Exception:" + ex.Message + "\n"+ex.StackTrace;
                logErr = true;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }

        public bool Scheduled_SyncMarketingData(ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            bool status = true;
            bool StartMarketingSync = false;

            try
            {
                object obj = DbHelperSQL.GetSingle("Select top 1 StartMarketingSync from Company_General");
                if (obj == null)
                {
                    err = "Failed to read company_general";
                    return true;
                }
                else
                {
                    StartMarketingSync = (bool)obj;
                    if (StartMarketingSync != true)
                    {
                        return true;
                    }
                }

                status = UpdateCompany(ref err);                
                status = ProcessMarketingBranchList(ref err);
                status = ProcessMarketingUserList(ref err);
                status = ProcessMarketingProspectList(ref err);
                status = LoadLeadStarMarketing();
                
                return true;
            }
            catch (Exception ex)
            {
                err = "Failed to Sync Marketing Data, Exception:" + ex.Message;
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }

        #endregion Sync Marketing Data

        #region Update Credit Card

        public UpdateCreditCardResponse UpdateCreditCard(UpdateCreditCardRequest req, ref string err)
        {
            err = string.Empty;
            string err1 = string.Empty;
            int Event_id = 6701;
            UpdateCreditCardResponse resp = new UpdateCreditCardResponse();
            resp.hdr = new RespHdr();
            string User_Integration_ID = string.Empty;          
            bool logErr = false;
            bool success = true;
          
            try
            {

                if ((LeadStar_CompanyKey == "") || (LeadStar_CompanyKey == string.Empty))
                {
                    string SqlCmd1 = string.Format("Select top 1 GlobalId from Company_General");
                    object obj1 = DbHelperSQL.GetSingle(SqlCmd1);
                    if (obj1 == null)
                    {
                        logErr = true;
                        err = "Failed to get Company Key from Company_General";
                        resp.hdr.Successful = false;
                        resp.hdr.StatusInfo = err;
                        return resp;
                    }
                    else
                    {
                        LeadStar_CompanyKey = (string)obj1;
                    }
                }

                string SqlCmd = string.Format("Select top 1 GlobalId from Users where UserId={0}", req.hdr.UserId);
                object obj = DbHelperSQL.GetSingle(SqlCmd);
                if (obj == null)
                {
                    object enableMarketing = DbHelperSQL.GetSingle("Select EnableMarketing from dbo.Company_General");
                    if (enableMarketing != null && Convert.ToBoolean(enableMarketing) == true)
                    {
                        logErr = true;
                        err = "Failed to get User Integration ID from Users";
                        resp.hdr.Successful = false;
                        resp.hdr.StatusInfo = err;
                        return resp;
                    }
                    else
                    {
                        logErr = true;
                        err = "The Marketing feature is not enabled.";
                        resp.hdr.Successful = false;
                        resp.hdr.StatusInfo = err;
                        return resp;
                    }

                }
                else
                {
                    User_Integration_ID = (string)obj;
                }

                using (LeadStarServiceClient LS = new LeadStarServiceClient())
                {

                string postURL = string.Empty;
                if (!string.IsNullOrEmpty(req.Card_ID))    //gdc 20110827 根据Card_ID 区别 插入 和 更新
                {
                    postURL = LS.GetCreditCardPostStringURL("MY", false, LeadStar_APIKey);
                }
                else
                {
                    postURL = LS.GetCreditCardPostStringURL("MY", true, LeadStar_APIKey);
                }
                    
                CreditCard ccCard = new CreditCard();
                ccCard.Card_ID = req.Card_ID;
                ccCard.Card_Exp_Month = req.Card_Exp_Month;
                ccCard.Card_Exp_Year = req.Card_Exp_Year;
                ccCard.Card_First_Name = req.Card_First_Name;
                ccCard.Card_IsDefault = req.Card_IsDefault;
                ccCard.Card_Last_Name = req.Card_Last_Name;
                ccCard.Card_Number = req.Card_Number;
                ccCard.Card_SIC = req.Card_SIC;
                
                if ( req.Card_Type == CreditCardType.VISA)
                    {
                    ccCard.Card_Type = PurchCreditCardType.VISA;
                    }
                if ( req.Card_Type == CreditCardType.MasterCard)
                    {
                    ccCard.Card_Type = PurchCreditCardType.MasterCard;
                    }
                if ( req.Card_Type == CreditCardType.Amex)
                    {
                    ccCard.Card_Type = PurchCreditCardType.Amex;
                    }
                if ( req.Card_Type == CreditCardType.Discover)
                    {
                    ccCard.Card_Type = PurchCreditCardType.Discover;
                    }

                ccCard.User_Integration_ID = User_Integration_ID;
               
                NameValueCollection nvcElements = new NameValueCollection();

                nvcElements.Add("API_Key", LeadStar_APIKey);
                nvcElements.Add("Instance_ID", LeadStar_InstanceId);
                nvcElements.Add("User_Integration_ID", ccCard.User_Integration_ID);
                nvcElements.Add("Company_Key", LeadStar_CompanyKey);
                nvcElements.Add("Card_Type", ccCard.Card_Type.ToString());
                nvcElements.Add("Card_First_Name", ccCard.Card_First_Name);

                if (!string.IsNullOrEmpty(ccCard.Card_ID))  //gdc 20110827 区别 插入 和 更新
                {
                    nvcElements.Add("Card_ID", ccCard.Card_ID.ToString());
                }

                nvcElements.Add("Card_Last_Name", ccCard.Card_Last_Name);
                nvcElements.Add("Card_Number", ccCard.Card_Number);
                nvcElements.Add("Card_Exp_Month", ccCard.Card_Exp_Month);
                nvcElements.Add("Card_Exp_Year", ccCard.Card_Exp_Year);
                nvcElements.Add("Card_SIC", ccCard.Card_SIC);
                nvcElements.Add("Card_IsDefault", ccCard.Card_IsDefault);

                List<String> items = new List<String>();

                foreach (String name in nvcElements)
                    {
                    items.Add(String.Concat(name, "=", System.Web.HttpUtility.UrlEncode(nvcElements[name])));
                    }

                string strPoststring = String.Join("&", items.ToArray());
                HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(postURL);
                loHttp.ContentType = "application/x-www-form-urlencoded";

                string lcPostData = HttpUtility.UrlEncode(strPoststring);
                loHttp.Method = "POST";
                byte[] lbPostBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(strPoststring);
                loHttp.ContentLength = lbPostBuffer.Length;

                Stream loPostData = loHttp.GetRequestStream();
                loPostData.Write(lbPostBuffer, 0, lbPostBuffer.Length);
                loPostData.Close();

                HttpWebResponse response = (HttpWebResponse)loHttp.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string strResult = readStream.ReadToEnd();

                if ( strResult == null )
                    strResult = "null";

                err1 = string.Format("  [Message detail]: " +
                                     " ccCard.API_Key = '{0}'," +
                                     " ccCard.Card_Exp_Month = '{1}'," +
                                     " ccCard.Card_Exp_Year = '{2}'," +
                                     " ccCard.Card_First_Name = '{3}'," +
                                     " ccCard.Card_ID = '{4}'," +
                                     " ccCard.Card_IsDefault = '{5}'," +
                                     " ccCard.Card_Last_Name = '{6}'," +
                                     " ccCard.Card_Number = '{7}'," +
                                     " ccCard.Card_SIC = '{8}'," +
                                     " ccCard.Card_Type = '{9}'," +
                                     " ccCard.Company_Key = '{10}'," +
                                     " ccCard.Instance_ID = '{11}'," +
                                     " ccCard.User_Integration_ID = '{12}'," +
                                     " strResult = '{13}'",
                                     ccCard.API_Key,
                                     ccCard.Card_Exp_Month,
                                     ccCard.Card_Exp_Year,
                                     ccCard.Card_First_Name,
                                     ccCard.Card_ID,
                                     ccCard.Card_IsDefault,
                                     ccCard.Card_Last_Name,
                                     ccCard.Card_Number,
                                     ccCard.Card_SIC,
                                     ccCard.Card_Type,
                                     ccCard.Company_Key,
                                     ccCard.Instance_ID,
                                     ccCard.User_Integration_ID,
                                     strResult);
  
                resp.hdr.Successful = true;
                resp.hdr.StatusInfo = "";
                return resp;

                //MessageBox.Show(strResult);

                //CreditCard[] arCreditCards = new CreditCard[1];
                //CreditCard ccSelectedCard = new CreditCard();

                //arCreditCards = LS.GetUserCreditCards(LeadStar_InstanceId, ccCard.User_Integration_ID, LeadStar_APIKey, LeadStar_CompanyKey);
                //foreach (CreditCard cCard in arCreditCards)
                //{
                //    ccCard.API_Key = LeadStar_APIKey;
                //    ccCard.Company_Key = LeadStar_CompanyKey;
                //}

                //string strResult1 = LS.AddToAccountBalance(LeadStar_InstanceId, ccCard.User_Integration_ID, 25, LeadStar_APIKey, LeadStar_CompanyKey);
               
                //switch (strResult1.ToLower())
                //{
                //    case "1000":                        
                //        err = "";
                //        break;
                         
                //    case "1202":
                //        logErr = true;
                //        success = false;
                //        err = "Add Fails: Bad Instance ID";
                //        break;

                //    case "1203":
                //        logErr = true;
                //        success = false;
                //        err = "Add Fails: Bad API_Key";
                //        break;

                //    case "1204":
                //        logErr = true;
                //        success = false;
                //        err = "Add Fails: User Not Found";
                //        break;

                //    case "1212":
                //        logErr = true;
                //        success = false;
                //        err = "Add fails - Leadstar Internal Error";
                //        break;

                //    case "1219":
                //        logErr = true;
                //        success = false;
                //        err = "1219	Add Fails: Minimum $25.00 Amount";
                //        break;

                //    case "1220":
                //        logErr = true;
                //        success = false;
                //        err = "Add Fails: Card Declined";
                //        break;

                //    case "1221":
                //        logErr = true;
                //        success = false;
                //        err = "Add Fails: Invalid Card Type";
                //        break;

                //    case "1222":
                //        logErr = true;
                //        success = false;
                //        err = "Add Fails: No Default Card Available";
                //        break;

                //    default:              
                //        logErr = true;
                //        success = false;
                //        err = "Add Fails: " + err;
                //        break;
                //}
                
                }
            }
            catch (Exception ex)
            {
                err = string.Format("Failed to update credit card, UserId={0}. Exception:{1}", req.hdr.UserId, ex.Message);
                logErr = true;
                resp.hdr.Successful = false;
                resp.hdr.StatusInfo = err;
                return resp;               
            }
            finally
            {
                if (logErr)
                {
                    err = err + err1;
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id);
                }
                else
                {
                    err = err1;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id);
                }
            }
        }

     #endregion Update Credit Card

     #region Get User Account Balance

     public GetUserAccountBalanceResponse GetUserAccountBalance(GetUserAccountBalanceRequest req, ref string err)
     {
            err = string.Empty;
            string err1 = string.Empty;
            int Event_id = 6801;
            bool logErr = false;
            bool status = true;
            bool StartMarketingSync = false;
            string user_Integration_ID = string.Empty;

            GetUserAccountBalanceResponse resp = new GetUserAccountBalanceResponse();
            resp.hdr = new RespHdr();
            
         try
            {

                if ((LeadStar_CompanyKey == "") || (LeadStar_CompanyKey == string.Empty))
                {
                    string SqlCmd1 = string.Format("Select top 1 GlobalId from Company_General");
                    object obj1 = DbHelperSQL.GetSingle(SqlCmd1);
                    if (obj1 == null)
                    {
                        logErr = true;
                        err = "Failed to get Company Key from Company_General";
                        resp.hdr.Successful = false;
                        resp.hdr.StatusInfo = err;
                        return resp;
                    }
                    else
                    {
                        LeadStar_CompanyKey = (string)obj1;
                    }
                }

                string SqlCmd = string.Format("Select top 1 GlobalId from Users where UserId={0}", req.hdr.UserId);
                object obj = DbHelperSQL.GetSingle(SqlCmd);
                if (obj == null)
                {
                    object enableMarketing = DbHelperSQL.GetSingle("Select EnableMarketing from dbo.Company_General");
                    if (enableMarketing != null && Convert.ToBoolean(enableMarketing) == true)
                    {
                        logErr = true;
                        err = "Failed to get User Integration ID from Users";
                        resp.hdr.Successful = false;
                        resp.hdr.StatusInfo = err;
                        return resp;
                    }
                    else
                    {
                        logErr = true;
                        err = "The Marketing feature is not enabled.";
                        resp.hdr.Successful = false;
                        resp.hdr.StatusInfo = err;
                        return resp;
                    }
                }
                else
                {
                    user_Integration_ID = (string)obj;
                }

                using (LeadStarServiceClient LS = new LeadStarServiceClient())
                {
                    decimal Balance = 0;
                    Balance = LS.GetUserAccountBalance(LeadStar_InstanceId, user_Integration_ID, LeadStar_APIKey, LeadStar_CompanyKey);

                    err1 = string.Format("LS.GetUserAccountBalance  [Message detail]: " +
                                    " LeadStar_InstanceId = '{0}'," +
                                    " user_Integration_ID = '{1}'," +
                                    " LeadStar_APIKey = '{2}'," +
                                    " LeadStar_CompanyKey = '{3}'," +
                                    " Balance = '{4}'",
                                    LeadStar_InstanceId,
                                    user_Integration_ID,
                                    LeadStar_APIKey.ToString(),
                                    LeadStar_CompanyKey,
                                    Balance.ToString());

                    resp.Balance = Balance;
                    resp.hdr.Successful = true;
                    resp.hdr.StatusInfo = "";
                    return resp;
                }
            }
            catch (Exception ex)
            {
                err = "Leadstar.GetUserAccountBalance, Exception:" + ex.Message;
                Event_id = 6899;
                logErr = true;
                resp.hdr.Successful = false;
                resp.hdr.StatusInfo = err;
                return resp;
            }
            finally
            {
                if (logErr)
                {
                    err = err1;
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id);
                }
                else
                {
                    err = err + err1;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id);
                }
            }
        }

    #endregion Get User Account Balance

     public GetCreditCardResponse GetCreditCard(GetCreditCardRequest req, ref string err)
     {
         err = string.Empty;
         string err1 = " ";
         string err2 = " ";
         int Event_id = 6601;
         string User_Integration_ID = string.Empty;
         bool logErr = false;
         bool success = true;
         GetCreditCardResponse resp = new GetCreditCardResponse();
         resp.hdr = new RespHdr();

         try
         {
             if ((LeadStar_CompanyKey == "") || (LeadStar_CompanyKey == string.Empty))
             {
                 string SqlCmd1 = string.Format("Select top 1 GlobalId from Company_General");
                 object obj1 = DbHelperSQL.GetSingle(SqlCmd1);
                 if (obj1 == null)
                 {
                     logErr = true;
                     err = "Failed to get Company Key from Company_General";
                     resp.hdr.Successful = false;
                     resp.hdr.StatusInfo = err;
                     return resp;
                 }
                 else
                 {
                     LeadStar_CompanyKey = (string)obj1;
                 }
             } 
            
             string SqlCmd = string.Format("Select top 1 GlobalId from Users where UserId={0}", req.hdr.UserId);
             object obj = DbHelperSQL.GetSingle(SqlCmd);
             if (obj == null)
             {
                 object enableMarketing = DbHelperSQL.GetSingle("Select EnableMarketing from dbo.Company_General");
                 if (enableMarketing != null && Convert.ToBoolean(enableMarketing) == true)
                 {
                     logErr = true;
                     err = "Failed to get User Integration ID from Users";
                     resp.hdr.Successful = false;
                     resp.hdr.StatusInfo = err;
                     return resp;
                 }
                 else
                 {
                     logErr = true;
                     err = "The Marketing feature is not enabled.";
                     resp.hdr.Successful = false;
                     resp.hdr.StatusInfo = err;
                     return resp;
                 }     
             }
             else
             {
                 User_Integration_ID = (string)obj;               
             }

             using (LeadStarServiceClient LS = new LeadStarServiceClient())
             {
                 CreditCard[] arCreditCards = new CreditCard[10];
                 CreditCard ccSelectedCard = new CreditCard();

                 arCreditCards = LS.GetUserCreditCards(LeadStar_InstanceId, User_Integration_ID, LeadStar_APIKey, LeadStar_CompanyKey);

                 err1 = string.Format("LS.GetUserCreditCards  [Message detail]: " +
                                    " LeadStar_InstanceId = '{0}'," +
                                    " User_Integration_ID = '{1}'," +
                                    " LeadStar_APIKey = '{2}'," +
                                    " LeadStar_CompanyKey = '{3}'",
                                    LeadStar_InstanceId,
                                    User_Integration_ID,
                                    LeadStar_APIKey.ToString(),
                                    LeadStar_CompanyKey);
                                   
                 //以下为新修改 gdc 20110827
 
                 if (arCreditCards == null)
                 {                     
                     resp.hdr.Successful = false;
                     resp.hdr.StatusInfo = err;
                     return resp;                    
                 }

                 var defaultCard = arCreditCards.FirstOrDefault(c => c.Card_IsDefault.ToLower() == "true");

                 if (defaultCard == null)
                 {
                     resp.hdr.Successful = false;
                     resp.hdr.StatusInfo = err;
                     return resp; 
                 }

                 err2 = string.Format("arCreditCards.FirstOrDefault  [Message detail]: " +
                                     " defaultCard.API_Key = '{0}'," +
                                     " defaultCard.Card_Exp_Month = '{1}'," +
                                     " defaultCard.Card_Exp_Year = '{2}'," +
                                     " defaultCard.Card_First_Name = '{3}'," +
                                     " defaultCard.Card_ID = '{4}'," +
                                     " defaultCard.Card_IsDefault = '{5}'," +
                                     " defaultCard.Card_Last_Name = '{6}'," +
                                     " defaultCard.Card_Number = '{7}'," +
                                     " defaultCard.Card_SIC = '{8}'," +
                                     " defaultCard.Card_Type = '{9}'," +
                                     " defaultCard.Company_Key = '{10}'," +
                                     " defaultCard.Instance_ID = '{11}'," +
                                     " defaultCard.User_Integration_ID = '{12}'",
                                     defaultCard.API_Key,
                                     defaultCard.Card_Exp_Month,
                                     defaultCard.Card_Exp_Year,
                                     defaultCard.Card_First_Name,
                                     defaultCard.Card_ID,
                                     defaultCard.Card_IsDefault,
                                     defaultCard.Card_Last_Name,
                                     defaultCard.Card_Number,
                                     defaultCard.Card_SIC,
                                     defaultCard.Card_Type,
                                     defaultCard.Company_Key,
                                     defaultCard.Instance_ID,
                                     defaultCard.User_Integration_ID);

                 req.Card_ID = defaultCard.Card_ID;
                 req.Card_Exp_Month = defaultCard.Card_Exp_Month;
                 req.Card_Exp_Year = defaultCard.Card_Exp_Year;
                 req.Card_First_Name = defaultCard.Card_First_Name;
                 req.Card_IsDefault = defaultCard.Card_IsDefault;
                 req.Card_Last_Name = defaultCard.Card_Last_Name;
                 req.Card_Number = defaultCard.Card_Number;
                 req.Card_SIC = defaultCard.Card_SIC;

                 switch (defaultCard.Card_Type)
                 {
                     case PurchCreditCardType.Amex:
                         req.Card_Type = CreditCardType.Amex;
                         break;
                     case PurchCreditCardType.Discover:
                         req.Card_Type = CreditCardType.Discover;
                         break;
                     case PurchCreditCardType.MasterCard:
                         req.Card_Type = CreditCardType.MasterCard;
                         break;
                     case PurchCreditCardType.VISA:
                         req.Card_Type = CreditCardType.VISA;
                         break;
                     default: break;
                         
                 }

                 resp.hdr.Successful = true;
                 resp.hdr.StatusInfo = "";
                 return resp; 
             }
         }
         catch (Exception ex)
         {
             err = string.Format("Leadstar.GetUserCreditCards, UserId={0}. Exception:{1}", req.hdr.UserId, ex.Message);
             logErr = true;
             Event_id = 6699;
             resp.hdr.Successful = false;
             resp.hdr.StatusInfo = err;
             return resp;  
         }
         finally
         {
             if (logErr)
             {
                 err = err + err1 + err2;
                 Trace.TraceError(err);
                 EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id);
             }
             else
             {
                 err = err1 + err2;
                 EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, Event_id);
             }
         }
     }

     public AddToAccountResponse AddToAccount(AddToAccountRequest req, ref string err)
     {
         err = string.Empty;
         string err1 = string.Empty;
         int Event_id = 6901;
         AddToAccountResponse resp = new AddToAccountResponse();
         resp.hdr = new RespHdr();
         string User_Integration_ID = string.Empty;
         bool logErr = false;
         bool success = true;

         try
         {
             if ((LeadStar_CompanyKey == "") || (LeadStar_CompanyKey == string.Empty))
             {
                 string SqlCmd1 = string.Format("Select top 1 GlobalId from Company_General");
                 object obj1 = DbHelperSQL.GetSingle(SqlCmd1);
                 if (obj1 == null)
                 {
                     logErr = true;
                     err = "Failed to get Company Key from Company_General";
                     resp.hdr.Successful = false;
                     resp.hdr.StatusInfo = err;
                     return resp;
                 }
                 else
                 {
                     LeadStar_CompanyKey = (string)obj1;
                 }
             }

             string SqlCmd = string.Format("Select top 1 GlobalId from Users where UserId={0}", req.hdr.UserId);
             object obj = DbHelperSQL.GetSingle(SqlCmd);
             if (obj == null)
             {
                 object enableMarketing = DbHelperSQL.GetSingle("Select EnableMarketing from dbo.Company_General");
                 if (enableMarketing != null && Convert.ToBoolean(enableMarketing) == true)
                 {
                     logErr = true;
                     err = "Failed to get User Integration ID from Users";
                     resp.hdr.Successful = false;
                     resp.hdr.StatusInfo = err;
                     return resp;
                 }
                 else
                 {
                     logErr = true;
                     err = "The Marketing feature is not enabled.";
                     resp.hdr.Successful = false;
                     resp.hdr.StatusInfo = err;
                     return resp;
                 }
             }
             else
             {
                 User_Integration_ID = (string)obj;
             }

             using (LeadStarServiceClient LS = new LeadStarServiceClient())
             {

                 CreditCard[] arCreditCards = new CreditCard[1];
                 CreditCard ccSelectedCard = new CreditCard();

                 decimal iAmountToAdd = req.Amount;

                 string strResult1 = LS.AddToAccountBalance(LeadStar_InstanceId, User_Integration_ID, iAmountToAdd, LeadStar_APIKey, LeadStar_CompanyKey);

                 err1 = string.Format("LS.AddToAccountBalance  [Message detail]: " +
                                    " LeadStar_InstanceId = '{0}'," +
                                    " User_Integration_ID = '{1}'," +
                                    " iAmountToAdd = '{2}'," +
                                    " LeadStar_APIKey = '{3}'," +
                                    " LeadStar_CompanyKey = '{4}'," +
                                    " strResult1 = '{5}'", 
                                    LeadStar_InstanceId,
                                    User_Integration_ID,
                                    iAmountToAdd.ToString(),
                                    LeadStar_APIKey,
                                    LeadStar_CompanyKey,                                
                                    strResult1);

                 switch (strResult1.ToLower())
                 {
                     case "1000":
                         success = true;
                         err = "";
                         break;

                     case "1202":
                         logErr = true;
                         success = false;
                         Event_id = 6902;
                         err = "Leadstar.AddToAccountBalance Fails(1202): Bad Instance ID";
                         break;

                     case "1203":
                         logErr = true;
                         success = false;
                         Event_id = 6903;
                         err = "Leadstar.AddToAccountBalance Fails(1203): Bad API_Key";
                         break;

                     case "1204":
                         logErr = true;
                         success = false;
                         Event_id = 6904;
                         err = "Leadstar.AddToAccountBalance Fails(1204): User Not Found";
                         break;

                     case "1212":
                         logErr = true;
                         success = false;
                         Event_id = 6912;
                         err = "Leadstar.AddToAccountBalance Fails(1212): - Leadstar Internal Error";
                         break;

                     case "1219":
                         logErr = true;
                         success = false;
                         Event_id = 6919;
                         err = "Leadstar.AddToAccountBalance Fails(1219): Minimum $25.00 Amount";
                         break;

                     case "1220":
                         logErr = true;
                         success = false;
                         Event_id = 6920;
                         err = "Leadstar.AddToAccountBalance Fails(1220): Card Declined";
                         break;

                     case "1221":
                         logErr = true;
                         success = false;
                         Event_id = 6921;
                         err = "Leadstar.AddToAccountBalance Fails(1221): Invalid Card Type";
                         break;

                     case "1222":
                         logErr = true;
                         success = false;
                         Event_id = 6922;
                         err = "Leadstar.AddToAccountBalance Fails(1222): No Default Card Available";
                         break;

                     default:
                         logErr = true;
                         success = false;
                         Event_id = 6923;
                         err = "Leadstar.AddToAccountBalance Fails: " + err;
                         break;
                 }

                 resp.hdr.Successful = success;
                 resp.hdr.StatusInfo = err;
               
                 return resp;  
               
             }
         }
         catch (Exception ex)
         {
             err = string.Format("Leadstar.AddToAccountBalance Fails: UserId={0}. Exception:{1}", req.hdr.UserId, ex.Message);
             logErr = true;
             Event_id = 6999;
             resp.hdr.Successful = false;
             resp.hdr.StatusInfo = err;
             return resp;            
         }
         finally
         {
             var err_Eventlog = "";
             if (logErr)
             {
                 err_Eventlog = err + err1;
                 Trace.TraceError(err);
                 EventLog.WriteEntry(InfoHubEventLog.LogSource, err_Eventlog, EventLogEntryType.Warning, Event_id);
             }
             else
             {
                 err_Eventlog = err1;
                 EventLog.WriteEntry(InfoHubEventLog.LogSource, err_Eventlog, EventLogEntryType.Information, Event_id);
             }
         }
     }

     public bool ReassignProspect(ReassignProspectRequest req, ref string err)
     {
         err = string.Empty;
         string User_Integration_ID = string.Empty;
         bool logErr = false;
         bool success = true;

         try
         {
             MarketingMgr.LeadStar.Company lsCompany = null;

             lsCompany = SetupCompanyInfo(ref err);
             if ((lsCompany == null) || ( err != "" ))
             {
                 logErr = false;
                 return true;
             }          
      
             using (LeadStarServiceClient LS = new LeadStarServiceClient())
             {
                 int iToUserID = req.ToUser;
                 int iFromUserID = req.FromUser;

                 string sToUserID = string.Empty;
                 string sFromUserID = string.Empty;
              
                 string[] sFileIDs = new string[req.FileId.Length];
                 
                 int j=0;

                 string uSql1 = "Select top 1 GlobalId from Users where UserId=" + req.ToUser;
                 object obj1 = DbHelperSQL.GetSingle(uSql1);

                 if (obj1 != null)
                 {
                     sToUserID = (string)obj1;
                 }
                 else
                 {
                     sToUserID = lsCompany.Integration_ID + "_u" + req.ToUser.ToString(); 
                 }

                 string uSql2 = "Select top 1 GlobalId from Users where UserId=" + req.ToUser;
                 object obj2 = DbHelperSQL.GetSingle(uSql2);

                 if (obj2 != null)
                 {
                     sFromUserID = (string)obj2;
                 }
                 else
                 {
                     sFromUserID = lsCompany.Integration_ID + "_u" + req.FromUser.ToString();
                 }

                 foreach (int i in req.FileId)
                 {
          //      sFileIDs.SetValue(i.ToString(), j);
          //      j++;

                  string lSql = "Select top 1 GlobalId from Loans where FileId=" + i;
                  object obj3= DbHelperSQL.GetSingle(lSql);

                  if (obj3 != null)
                  {
                      sFileIDs[j] = (string)obj3;
                  }
                  else
                  {
                      sFileIDs[j] = lsCompany.Integration_ID + "_l" + i.ToString();
                  }

                  j++;

                 }

                 string strResult1 = LS.ReassignProspects(LeadStar_InstanceId, sFileIDs, sFromUserID, sToUserID, LeadStar_APIKey, LeadStar_CompanyKey);                 
                
                 switch (strResult1.ToLower())
                 {
                     case "1000":
                         err = "";
                         break;

                     case "1202":
                         logErr = true;
                         success = false;
                         err = "Add Fails: Bad Instance ID";
                         break;

                     case "1203":
                         logErr = true;
                         success = false;
                         err = "Add Fails: Bad API_Key";
                         break;

                     case "1204":
                         logErr = true;
                         success = false;
                         err = "Add Fails: User Not Found";
                         break;

                     case "1212":
                         logErr = true;
                         success = false;
                         err = "Add fails - Leadstar Internal Error";
                         break;

                     case "1219":
                         logErr = true;
                         success = false;
                         err = "1219	Add Fails: Minimum $25.00 Amount";
                         break;

                     case "1220":
                         logErr = true;
                         success = false;
                         err = "Add Fails: Card Declined";
                         break;

                     case "1221":
                         logErr = true;
                         success = false;
                         err = "Add Fails: Invalid Card Type";
                         break;

                     case "1222":
                         logErr = true;
                         success = false;
                         err = "Add Fails: No Default Card Available";
                         break;

                     default:
                         logErr = true;
                         success = false;
                         err = "Add Fails: " + err;
                         break;
                 }
                 return true;
             }
         }
         catch (Exception ex)
         {
             err = string.Format("Failed to add to account, UserId={0}. Exception:{1}", req.hdr.UserId, ex.Message);
             logErr = true;
             return false;
         }
         finally
         {
             if (logErr)
             {
                 Trace.TraceError(err);
                 EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
             }
         }
     }

    }   
}


