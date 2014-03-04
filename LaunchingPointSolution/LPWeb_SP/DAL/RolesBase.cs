using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:Roles
    /// </summary>
    public class RolesBase
    {
        public RolesBase()
        { }
        #region  Method

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Roles model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Roles(");
            strSql.Append("Name,CompanySetup,LoanSetup,OtherLoanAccess,CustomUserHome,WorkflowTempl,CustomTask,AlertRules,AlertRuleTempl,MarkOtherTaskCompl,AssignTask,ImportLoan,RemoveLoan,AssignLoan,ApplyWorkflow,ApplyAlertRule,SendEmail,CreateNotes,CompanyCalendar,PipelineChart,SalesBreakdownChart,OrgProductionChart,Org_N_Sales_Charts,RateSummary,GoalsChart,OverdueTaskAlerts,Announcements,ExchangeInbox,ExchangeCalendar,SetOwnGoals,SetUserGoals,Reports,ContactMgmt,Prospect,Loans,AccessAllContacts,ContactCompany,ContactBranch,ServiceType,ContactRole,Marketing,SendLSR,ExtendRateLock,ConditionRights,AccessAllMailChimpList,AccessUnassignedLeads,ViewLockInfo,LockRate,AccessProfitability,ViewCompensation,UpdateCondition)");
            strSql.Append(" values (");
            strSql.Append("@Name,@CompanySetup,@LoanSetup,@OtherLoanAccess,@CustomUserHome,@WorkflowTempl,@CustomTask,@AlertRules,@AlertRuleTempl,@MarkOtherTaskCompl,@AssignTask,@ImportLoan,@RemoveLoan,@AssignLoan,@ApplyWorkflow,@ApplyAlertRule,@SendEmail,@CreateNotes,@CompanyCalendar,@PipelineChart,@SalesBreakdownChart,@OrgProductionChart,@Org_N_Sales_Charts,@RateSummary,@GoalsChart,@OverdueTaskAlerts,@Announcements,@ExchangeInbox,@ExchangeCalendar,@SetOwnGoals,@SetUserGoals,@Reports,@ContactMgmt,@Prospect,@Loans,@AccessAllContacts,@ContactCompany,@ContactBranch,@ServiceType,@ContactRole,@Marketing,@SendLSR,@ExtendRateLock,@ConditionRights,@AccessAllMailChimpList,@AccessUnassignedLeads,@ViewLockInfo,@LockRate,@AccessProfitability,@ViewCompensation,@UpdateCondition)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@CompanySetup", SqlDbType.Bit,1),
					new SqlParameter("@LoanSetup", SqlDbType.Bit,1),
					new SqlParameter("@OtherLoanAccess", SqlDbType.Bit,1),
					new SqlParameter("@CustomUserHome", SqlDbType.Bit,1),
					new SqlParameter("@WorkflowTempl", SqlDbType.SmallInt,2),
					new SqlParameter("@CustomTask", SqlDbType.SmallInt,2),
					new SqlParameter("@AlertRules", SqlDbType.SmallInt,2),
					new SqlParameter("@AlertRuleTempl", SqlDbType.SmallInt,2),
					new SqlParameter("@MarkOtherTaskCompl", SqlDbType.Bit,1),
					new SqlParameter("@AssignTask", SqlDbType.Bit,1),
					new SqlParameter("@ImportLoan", SqlDbType.Bit,1),
					new SqlParameter("@RemoveLoan", SqlDbType.Bit,1),
					new SqlParameter("@AssignLoan", SqlDbType.Bit,1),
					new SqlParameter("@ApplyWorkflow", SqlDbType.Bit,1),
					new SqlParameter("@ApplyAlertRule", SqlDbType.Bit,1),
					new SqlParameter("@SendEmail", SqlDbType.Bit,1),
					new SqlParameter("@CreateNotes", SqlDbType.Bit,1),
					new SqlParameter("@CompanyCalendar", SqlDbType.Bit,1),
					new SqlParameter("@PipelineChart", SqlDbType.Bit,1),
					new SqlParameter("@SalesBreakdownChart", SqlDbType.Bit,1),
					new SqlParameter("@OrgProductionChart", SqlDbType.Bit,1),
					new SqlParameter("@Org_N_Sales_Charts", SqlDbType.Bit,1),
					new SqlParameter("@RateSummary", SqlDbType.Bit,1),
					new SqlParameter("@GoalsChart", SqlDbType.Bit,1),
					new SqlParameter("@OverdueTaskAlerts", SqlDbType.Bit,1),
					new SqlParameter("@Announcements", SqlDbType.Bit,1),
					new SqlParameter("@ExchangeInbox", SqlDbType.Bit,1),
					new SqlParameter("@ExchangeCalendar", SqlDbType.Bit,1),
					new SqlParameter("@SetOwnGoals", SqlDbType.Bit,1),
					new SqlParameter("@SetUserGoals", SqlDbType.Bit,1),
					new SqlParameter("@Reports", SqlDbType.Bit,1),
					new SqlParameter("@ContactMgmt", SqlDbType.Int,4),
					new SqlParameter("@Prospect", SqlDbType.NVarChar,50),
					new SqlParameter("@Loans", SqlDbType.NVarChar,50),
					new SqlParameter("@AccessAllContacts", SqlDbType.Bit,1),
					new SqlParameter("@ContactCompany", SqlDbType.Int,4),
					new SqlParameter("@ContactBranch", SqlDbType.Int,4),
					new SqlParameter("@ServiceType", SqlDbType.SmallInt,2),
					new SqlParameter("@ContactRole", SqlDbType.SmallInt,2),
					new SqlParameter("@Marketing", SqlDbType.SmallInt,2),
                    new SqlParameter("@SendLSR",SqlDbType.Bit,1),
                    new SqlParameter("@ExtendRateLock", SqlDbType.Bit,1),
                    new SqlParameter("@ConditionRights", SqlDbType.NVarChar,50),
                    new SqlParameter("@AccessAllMailChimpList", SqlDbType.Bit),
                    new SqlParameter("@AccessUnassignedLeads", SqlDbType.Bit),
                    new SqlParameter("@ViewLockInfo", SqlDbType.Bit),
                    new SqlParameter("@LockRate", SqlDbType.Bit),
                    new SqlParameter("@AccessProfitability", SqlDbType.Bit),
                    new SqlParameter("@ViewCompensation", SqlDbType.Bit),
                    new SqlParameter("@UpdateCondition", SqlDbType.Bit)
                                        };
            parameters[0].Value = model.Name;
            parameters[1].Value = model.CompanySetup;
            parameters[2].Value = model.LoanSetup;
            parameters[3].Value = model.OtherLoanAccess;
            parameters[4].Value = model.CustomUserHome;
            parameters[5].Value = model.WorkflowTempl;
            parameters[6].Value = model.CustomTask;
            parameters[7].Value = model.AlertRules;
            parameters[8].Value = model.AlertRuleTempl;
            parameters[9].Value = model.MarkOtherTaskCompl;
            parameters[10].Value = model.AssignTask;
            parameters[11].Value = model.ImportLoan;
            parameters[12].Value = model.RemoveLoan;
            parameters[13].Value = model.AssignLoan;
            parameters[14].Value = model.ApplyWorkflow;
            parameters[15].Value = model.ApplyAlertRule;
            parameters[16].Value = model.SendEmail;
            parameters[17].Value = model.CreateNotes;
            parameters[18].Value = model.CompanyCalendar;
            parameters[19].Value = model.PipelineChart;
            parameters[20].Value = model.SalesBreakdownChart;
            parameters[21].Value = model.OrgProductionChart;
            parameters[22].Value = model.Org_N_Sales_Charts;
            parameters[23].Value = model.RateSummary;
            parameters[24].Value = model.GoalsChart;
            parameters[25].Value = model.OverdueTaskAlerts;
            parameters[26].Value = model.Announcements;
            parameters[27].Value = model.ExchangeInbox;
            parameters[28].Value = model.ExchangeCalendar;
            parameters[29].Value = model.SetOwnGoals;
            parameters[30].Value = model.SetUserGoals;
            parameters[31].Value = model.Reports;
            parameters[32].Value = model.ContactMgmt;
            parameters[33].Value = model.Prospect;
            parameters[34].Value = model.Loans;
            parameters[35].Value = model.AccessAllContacts;
            parameters[36].Value = model.ContactCompany;
            parameters[37].Value = model.ContactBranch;
            parameters[38].Value = model.ServiceType;
            parameters[39].Value = model.ContactRole;
            parameters[40].Value = model.Marketing;
            parameters[41].Value = model.SendLSR;
            parameters[42].Value = model.ExtendRateLock;
            parameters[43].Value = model.ConditionRights;
            parameters[44].Value = model.AccessAllMailChimpList;
            parameters[45].Value = model.AccessUnassignedLeads;
            parameters[46].Value = model.ViewLockInfo;
            parameters[47].Value = model.LockRate;
            parameters[48].Value = model.AccessProfitability;
            parameters[49].Value = model.ViewCompensation;
            parameters[50].Value = model.UpdateCondition;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.Roles model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Roles set ");
            strSql.Append("Name=@Name,");
            strSql.Append("CompanySetup=@CompanySetup,");
            strSql.Append("LoanSetup=@LoanSetup,");
            strSql.Append("OtherLoanAccess=@OtherLoanAccess,");
            strSql.Append("CustomUserHome=@CustomUserHome,");
            strSql.Append("WorkflowTempl=@WorkflowTempl,");
            strSql.Append("CustomTask=@CustomTask,");
            strSql.Append("AlertRules=@AlertRules,");
            strSql.Append("AlertRuleTempl=@AlertRuleTempl,");
            strSql.Append("MarkOtherTaskCompl=@MarkOtherTaskCompl,");
            strSql.Append("AssignTask=@AssignTask,");
            strSql.Append("ImportLoan=@ImportLoan,");
            strSql.Append("RemoveLoan=@RemoveLoan,");
            strSql.Append("AssignLoan=@AssignLoan,");
            strSql.Append("ApplyWorkflow=@ApplyWorkflow,");
            strSql.Append("ApplyAlertRule=@ApplyAlertRule,");
            strSql.Append("SendEmail=@SendEmail,");
            strSql.Append("CreateNotes=@CreateNotes,");
            strSql.Append("CompanyCalendar=@CompanyCalendar,");
            strSql.Append("PipelineChart=@PipelineChart,");
            strSql.Append("SalesBreakdownChart=@SalesBreakdownChart,");
            strSql.Append("OrgProductionChart=@OrgProductionChart,");
            strSql.Append("Org_N_Sales_Charts=@Org_N_Sales_Charts,");
            strSql.Append("RateSummary=@RateSummary,");
            strSql.Append("GoalsChart=@GoalsChart,");
            strSql.Append("OverdueTaskAlerts=@OverdueTaskAlerts,");
            strSql.Append("Announcements=@Announcements,");
            strSql.Append("ExchangeInbox=@ExchangeInbox,");
            strSql.Append("ExchangeCalendar=@ExchangeCalendar,");
            strSql.Append("SetOwnGoals=@SetOwnGoals,");
            strSql.Append("SetUserGoals=@SetUserGoals,");
            strSql.Append("Reports=@Reports,");
            strSql.Append("ContactMgmt=@ContactMgmt,");
            strSql.Append("Prospect=@Prospect,");
            strSql.Append("Loans=@Loans,");
            strSql.Append("AccessAllContacts=@AccessAllContacts,");
            strSql.Append("ContactCompany=@ContactCompany,");
            strSql.Append("ContactBranch=@ContactBranch,");
            strSql.Append("ServiceType=@ServiceType,");
            strSql.Append("ContactRole=@ContactRole,");
            strSql.Append("Marketing=@Marketing,");
            strSql.Append("SendLSR=@SendLSR,");
            strSql.Append("ExtendRateLock=@ExtendRateLock,");
            strSql.Append("ConditionRights=@ConditionRights,");
            strSql.Append("AccessAllMailChimpList=@AccessAllMailChimpList,");
            strSql.Append("ExportPipelines=@ExportPipelines,");
            strSql.Append("AccessUnassignedLeads=@AccessUnassignedLeads,");
            strSql.Append("ViewLockInfo=@ViewLockInfo,");
            strSql.Append("LockRate=@LockRate,");
            strSql.Append("AccessProfitability=@AccessProfitability,");
            strSql.Append("ViewCompensation=@ViewCompensation,");
            strSql.Append("UpdateCondition=@UpdateCondition");
            strSql.Append(" where RoleId=@RoleId");
            SqlParameter[] parameters = {
					new SqlParameter("@RoleId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@CompanySetup", SqlDbType.Bit,1),
					new SqlParameter("@LoanSetup", SqlDbType.Bit,1),
					new SqlParameter("@OtherLoanAccess", SqlDbType.Bit,1),
					new SqlParameter("@CustomUserHome", SqlDbType.Bit,1),
					new SqlParameter("@WorkflowTempl", SqlDbType.SmallInt,2),
					new SqlParameter("@CustomTask", SqlDbType.SmallInt,2),
					new SqlParameter("@AlertRules", SqlDbType.SmallInt,2),
					new SqlParameter("@AlertRuleTempl", SqlDbType.SmallInt,2),
					new SqlParameter("@MarkOtherTaskCompl", SqlDbType.Bit,1),
					new SqlParameter("@AssignTask", SqlDbType.Bit,1),
					new SqlParameter("@ImportLoan", SqlDbType.Bit,1),
					new SqlParameter("@RemoveLoan", SqlDbType.Bit,1),
					new SqlParameter("@AssignLoan", SqlDbType.Bit,1),
					new SqlParameter("@ApplyWorkflow", SqlDbType.Bit,1),
					new SqlParameter("@ApplyAlertRule", SqlDbType.Bit,1),
					new SqlParameter("@SendEmail", SqlDbType.Bit,1),
					new SqlParameter("@CreateNotes", SqlDbType.Bit,1),
					new SqlParameter("@CompanyCalendar", SqlDbType.Bit,1),
					new SqlParameter("@PipelineChart", SqlDbType.Bit,1),
					new SqlParameter("@SalesBreakdownChart", SqlDbType.Bit,1),
					new SqlParameter("@OrgProductionChart", SqlDbType.Bit,1),
					new SqlParameter("@Org_N_Sales_Charts", SqlDbType.Bit,1),
					new SqlParameter("@RateSummary", SqlDbType.Bit,1),
					new SqlParameter("@GoalsChart", SqlDbType.Bit,1),
					new SqlParameter("@OverdueTaskAlerts", SqlDbType.Bit,1),
					new SqlParameter("@Announcements", SqlDbType.Bit,1),
					new SqlParameter("@ExchangeInbox", SqlDbType.Bit,1),
					new SqlParameter("@ExchangeCalendar", SqlDbType.Bit,1),
					new SqlParameter("@SetOwnGoals", SqlDbType.Bit,1),
					new SqlParameter("@SetUserGoals", SqlDbType.Bit,1),
					new SqlParameter("@Reports", SqlDbType.Bit,1),
					new SqlParameter("@ContactMgmt", SqlDbType.Int,4),
					new SqlParameter("@Prospect", SqlDbType.NVarChar,50),
					new SqlParameter("@Loans", SqlDbType.NVarChar,50),
					new SqlParameter("@AccessAllContacts", SqlDbType.Bit,1),
					new SqlParameter("@ContactCompany", SqlDbType.Int,4),
					new SqlParameter("@ContactBranch", SqlDbType.Int,4),
					new SqlParameter("@ServiceType", SqlDbType.SmallInt,2),
					new SqlParameter("@ContactRole", SqlDbType.SmallInt,2),
					new SqlParameter("@Marketing", SqlDbType.SmallInt,2),
                    new SqlParameter("@SendLSR", SqlDbType.Bit,1),
                    new SqlParameter("@ExtendRateLock",SqlDbType.Bit,1),
                    new SqlParameter("@ConditionRights",SqlDbType.NVarChar,50),
                    new SqlParameter("@AccessAllMailChimpList",SqlDbType.Bit),
                    new SqlParameter("@ExportPipelines",SqlDbType.Bit),
                    new SqlParameter("@AccessUnassignedLeads",SqlDbType.Bit),
                    new SqlParameter("@ViewLockInfo",SqlDbType.Bit),
                    new SqlParameter("@LockRate",SqlDbType.Bit),
                    new SqlParameter("@AccessProfitability",SqlDbType.Bit),
                    new SqlParameter("@ViewCompensation",SqlDbType.Bit),
                    new SqlParameter("@UpdateCondition",SqlDbType.Bit)
                                        };
            parameters[0].Value = model.RoleId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.CompanySetup;
            parameters[3].Value = model.LoanSetup;
            parameters[4].Value = model.OtherLoanAccess;
            parameters[5].Value = model.CustomUserHome;
            parameters[6].Value = model.WorkflowTempl;
            parameters[7].Value = model.CustomTask;
            parameters[8].Value = model.AlertRules;
            parameters[9].Value = model.AlertRuleTempl;
            parameters[10].Value = model.MarkOtherTaskCompl;
            parameters[11].Value = model.AssignTask;
            parameters[12].Value = model.ImportLoan;
            parameters[13].Value = model.RemoveLoan;
            parameters[14].Value = model.AssignLoan;
            parameters[15].Value = model.ApplyWorkflow;
            parameters[16].Value = model.ApplyAlertRule;
            parameters[17].Value = model.SendEmail;
            parameters[18].Value = model.CreateNotes;
            parameters[19].Value = model.CompanyCalendar;
            parameters[20].Value = model.PipelineChart;
            parameters[21].Value = model.SalesBreakdownChart;
            parameters[22].Value = model.OrgProductionChart;
            parameters[23].Value = model.Org_N_Sales_Charts;
            parameters[24].Value = model.RateSummary;
            parameters[25].Value = model.GoalsChart;
            parameters[26].Value = model.OverdueTaskAlerts;
            parameters[27].Value = model.Announcements;
            parameters[28].Value = model.ExchangeInbox;
            parameters[29].Value = model.ExchangeCalendar;
            parameters[30].Value = model.SetOwnGoals;
            parameters[31].Value = model.SetUserGoals;
            parameters[32].Value = model.Reports;
            parameters[33].Value = model.ContactMgmt;
            parameters[34].Value = model.Prospect;
            parameters[35].Value = model.Loans;
            parameters[36].Value = model.AccessAllContacts;
            parameters[37].Value = model.ContactCompany;
            parameters[38].Value = model.ContactBranch;
            parameters[39].Value = model.ServiceType;
            parameters[40].Value = model.ContactRole;
            parameters[41].Value = model.Marketing;
            parameters[42].Value = model.SendLSR;
            parameters[43].Value = model.ExtendRateLock;
            parameters[44].Value = model.ConditionRights;
            parameters[45].Value = model.AccessAllMailChimpList;
            parameters[46].Value = model.ExportPipelines;
            parameters[47].Value = model.AccessUnassignedLeads;
            parameters[48].Value = model.ViewLockInfo;
            parameters[49].Value = model.LockRate;
            parameters[50].Value = model.AccessProfitability;
            parameters[51].Value = model.ViewCompensation;
            parameters[52].Value = model.UpdateCondition;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int RoleId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Roles ");
            strSql.Append(" where RoleId=@RoleId");
            SqlParameter[] parameters = {
					new SqlParameter("@RoleId", SqlDbType.Int,4)
};
            parameters[0].Value = RoleId;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string RoleIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Roles ");
            strSql.Append(" where RoleId in (" + RoleIdlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Roles GetModel(int RoleId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 RoleId,Name,CompanySetup,LoanSetup,OtherLoanAccess,CustomUserHome,WorkflowTempl,CustomTask,AlertRules,AlertRuleTempl,MarkOtherTaskCompl,AssignTask,ImportLoan,RemoveLoan,AssignLoan,ApplyWorkflow,ApplyAlertRule,SendEmail,CreateNotes,CompanyCalendar,PipelineChart,SalesBreakdownChart,OrgProductionChart,Org_N_Sales_Charts,RateSummary,GoalsChart,OverdueTaskAlerts,Announcements,ExchangeInbox,ExchangeCalendar,SetOwnGoals,SetUserGoals,Reports,ContactMgmt,Prospect,Loans,AccessAllContacts,ContactCompany,ContactBranch,ServiceType,ContactRole,Marketing,SendLSR,ConditionRights,ExtendRateLock,AccessAllMailChimpList,ExportPipelines,AccessUnassignedLeads,ViewLockInfo,LockRate,AccessProfitability,ViewCompensation,UpdateCondition from Roles ");            
            strSql.Append(" where RoleId=@RoleId");
            SqlParameter[] parameters = {
					new SqlParameter("@RoleId", SqlDbType.Int,4)
};
            parameters[0].Value = RoleId;

            LPWeb.Model.Roles model = new LPWeb.Model.Roles();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["RoleId"].ToString() != "")
                {
                    model.RoleId = int.Parse(ds.Tables[0].Rows[0]["RoleId"].ToString());
                }
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                if (ds.Tables[0].Rows[0]["CompanySetup"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["CompanySetup"].ToString() == "1") || (ds.Tables[0].Rows[0]["CompanySetup"].ToString().ToLower() == "true"))
                    {
                        model.CompanySetup = true;
                    }
                    else
                    {
                        model.CompanySetup = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["LoanSetup"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["LoanSetup"].ToString() == "1") || (ds.Tables[0].Rows[0]["LoanSetup"].ToString().ToLower() == "true"))
                    {
                        model.LoanSetup = true;
                    }
                    else
                    {
                        model.LoanSetup = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["OtherLoanAccess"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["OtherLoanAccess"].ToString() == "1") || (ds.Tables[0].Rows[0]["OtherLoanAccess"].ToString().ToLower() == "true"))
                    {
                        model.OtherLoanAccess = true;
                    }
                    else
                    {
                        model.OtherLoanAccess = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["CustomUserHome"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["CustomUserHome"].ToString() == "1") || (ds.Tables[0].Rows[0]["CustomUserHome"].ToString().ToLower() == "true"))
                    {
                        model.CustomUserHome = true;
                    }
                    else
                    {
                        model.CustomUserHome = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["WorkflowTempl"].ToString() != "")
                {
                    model.WorkflowTempl = int.Parse(ds.Tables[0].Rows[0]["WorkflowTempl"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CustomTask"].ToString() != "")
                {
                    model.CustomTask = int.Parse(ds.Tables[0].Rows[0]["CustomTask"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AlertRules"].ToString() != "")
                {
                    model.AlertRules = int.Parse(ds.Tables[0].Rows[0]["AlertRules"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AlertRuleTempl"].ToString() != "")
                {
                    model.AlertRuleTempl = int.Parse(ds.Tables[0].Rows[0]["AlertRuleTempl"].ToString());
                }
                if (ds.Tables[0].Rows[0]["MarkOtherTaskCompl"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["MarkOtherTaskCompl"].ToString() == "1") || (ds.Tables[0].Rows[0]["MarkOtherTaskCompl"].ToString().ToLower() == "true"))
                    {
                        model.MarkOtherTaskCompl = true;
                    }
                    else
                    {
                        model.MarkOtherTaskCompl = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["AssignTask"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["AssignTask"].ToString() == "1") || (ds.Tables[0].Rows[0]["AssignTask"].ToString().ToLower() == "true"))
                    {
                        model.AssignTask = true;
                    }
                    else
                    {
                        model.AssignTask = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ImportLoan"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ImportLoan"].ToString() == "1") || (ds.Tables[0].Rows[0]["ImportLoan"].ToString().ToLower() == "true"))
                    {
                        model.ImportLoan = true;
                    }
                    else
                    {
                        model.ImportLoan = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["RemoveLoan"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["RemoveLoan"].ToString() == "1") || (ds.Tables[0].Rows[0]["RemoveLoan"].ToString().ToLower() == "true"))
                    {
                        model.RemoveLoan = true;
                    }
                    else
                    {
                        model.RemoveLoan = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["AssignLoan"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["AssignLoan"].ToString() == "1") || (ds.Tables[0].Rows[0]["AssignLoan"].ToString().ToLower() == "true"))
                    {
                        model.AssignLoan = true;
                    }
                    else
                    {
                        model.AssignLoan = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ApplyWorkflow"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ApplyWorkflow"].ToString() == "1") || (ds.Tables[0].Rows[0]["ApplyWorkflow"].ToString().ToLower() == "true"))
                    {
                        model.ApplyWorkflow = true;
                    }
                    else
                    {
                        model.ApplyWorkflow = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ApplyAlertRule"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ApplyAlertRule"].ToString() == "1") || (ds.Tables[0].Rows[0]["ApplyAlertRule"].ToString().ToLower() == "true"))
                    {
                        model.ApplyAlertRule = true;
                    }
                    else
                    {
                        model.ApplyAlertRule = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["SendEmail"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["SendEmail"].ToString() == "1") || (ds.Tables[0].Rows[0]["SendEmail"].ToString().ToLower() == "true"))
                    {
                        model.SendEmail = true;
                    }
                    else
                    {
                        model.SendEmail = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["CreateNotes"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["CreateNotes"].ToString() == "1") || (ds.Tables[0].Rows[0]["CreateNotes"].ToString().ToLower() == "true"))
                    {
                        model.CreateNotes = true;
                    }
                    else
                    {
                        model.CreateNotes = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["CompanyCalendar"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["CompanyCalendar"].ToString() == "1") || (ds.Tables[0].Rows[0]["CompanyCalendar"].ToString().ToLower() == "true"))
                    {
                        model.CompanyCalendar = true;
                    }
                    else
                    {
                        model.CompanyCalendar = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["PipelineChart"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["PipelineChart"].ToString() == "1") || (ds.Tables[0].Rows[0]["PipelineChart"].ToString().ToLower() == "true"))
                    {
                        model.PipelineChart = true;
                    }
                    else
                    {
                        model.PipelineChart = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["SalesBreakdownChart"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["SalesBreakdownChart"].ToString() == "1") || (ds.Tables[0].Rows[0]["SalesBreakdownChart"].ToString().ToLower() == "true"))
                    {
                        model.SalesBreakdownChart = true;
                    }
                    else
                    {
                        model.SalesBreakdownChart = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["OrgProductionChart"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["OrgProductionChart"].ToString() == "1") || (ds.Tables[0].Rows[0]["OrgProductionChart"].ToString().ToLower() == "true"))
                    {
                        model.OrgProductionChart = true;
                    }
                    else
                    {
                        model.OrgProductionChart = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Org_N_Sales_Charts"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Org_N_Sales_Charts"].ToString() == "1") || (ds.Tables[0].Rows[0]["Org_N_Sales_Charts"].ToString().ToLower() == "true"))
                    {
                        model.Org_N_Sales_Charts = true;
                    }
                    else
                    {
                        model.Org_N_Sales_Charts = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["RateSummary"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["RateSummary"].ToString() == "1") || (ds.Tables[0].Rows[0]["RateSummary"].ToString().ToLower() == "true"))
                    {
                        model.RateSummary = true;
                    }
                    else
                    {
                        model.RateSummary = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["GoalsChart"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["GoalsChart"].ToString() == "1") || (ds.Tables[0].Rows[0]["GoalsChart"].ToString().ToLower() == "true"))
                    {
                        model.GoalsChart = true;
                    }
                    else
                    {
                        model.GoalsChart = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["OverdueTaskAlerts"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["OverdueTaskAlerts"].ToString() == "1") || (ds.Tables[0].Rows[0]["OverdueTaskAlerts"].ToString().ToLower() == "true"))
                    {
                        model.OverdueTaskAlerts = true;
                    }
                    else
                    {
                        model.OverdueTaskAlerts = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Announcements"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Announcements"].ToString() == "1") || (ds.Tables[0].Rows[0]["Announcements"].ToString().ToLower() == "true"))
                    {
                        model.Announcements = true;
                    }
                    else
                    {
                        model.Announcements = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ExchangeInbox"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ExchangeInbox"].ToString() == "1") || (ds.Tables[0].Rows[0]["ExchangeInbox"].ToString().ToLower() == "true"))
                    {
                        model.ExchangeInbox = true;
                    }
                    else
                    {
                        model.ExchangeInbox = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ExchangeCalendar"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ExchangeCalendar"].ToString() == "1") || (ds.Tables[0].Rows[0]["ExchangeCalendar"].ToString().ToLower() == "true"))
                    {
                        model.ExchangeCalendar = true;
                    }
                    else
                    {
                        model.ExchangeCalendar = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["SetOwnGoals"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["SetOwnGoals"].ToString() == "1") || (ds.Tables[0].Rows[0]["SetOwnGoals"].ToString().ToLower() == "true"))
                    {
                        model.SetOwnGoals = true;
                    }
                    else
                    {
                        model.SetOwnGoals = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["SetUserGoals"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["SetUserGoals"].ToString() == "1") || (ds.Tables[0].Rows[0]["SetUserGoals"].ToString().ToLower() == "true"))
                    {
                        model.SetUserGoals = true;
                    }
                    else
                    {
                        model.SetUserGoals = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Reports"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Reports"].ToString() == "1") || (ds.Tables[0].Rows[0]["Reports"].ToString().ToLower() == "true"))
                    {
                        model.Reports = true;
                    }
                    else
                    {
                        model.Reports = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ContactMgmt"].ToString() != "")
                {
                    model.ContactMgmt = int.Parse(ds.Tables[0].Rows[0]["ContactMgmt"].ToString());
                }
                model.Prospect = ds.Tables[0].Rows[0]["Prospect"].ToString();
                model.Loans = ds.Tables[0].Rows[0]["Loans"].ToString();
                if (ds.Tables[0].Rows[0]["AccessAllContacts"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["AccessAllContacts"].ToString() == "1") || (ds.Tables[0].Rows[0]["AccessAllContacts"].ToString().ToLower() == "true"))
                    {
                        model.AccessAllContacts = true;
                    }
                    else
                    {
                        model.AccessAllContacts = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ContactCompany"].ToString() != "")
                {
                    model.ContactCompany = int.Parse(ds.Tables[0].Rows[0]["ContactCompany"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ContactBranch"].ToString() != "")
                {
                    model.ContactBranch = int.Parse(ds.Tables[0].Rows[0]["ContactBranch"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ServiceType"].ToString() != "")
                {
                    model.ServiceType = int.Parse(ds.Tables[0].Rows[0]["ServiceType"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ContactRole"].ToString() != "")
                {
                    model.ContactRole = int.Parse(ds.Tables[0].Rows[0]["ContactRole"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Marketing"].ToString() != "")
                {
                    model.Marketing = int.Parse(ds.Tables[0].Rows[0]["Marketing"].ToString());
                }

                if (ds.Tables[0].Rows[0]["SendLSR"] == DBNull.Value) 
                {
                    model.SendLSR = false;
                }
                else
                {
                    model.SendLSR = Convert.ToBoolean(ds.Tables[0].Rows[0]["SendLSR"]);
                }

                if (ds.Tables[0].Rows[0]["ExtendRateLock"] != DBNull.Value)
                {
                    if ((ds.Tables[0].Rows[0]["ExtendRateLock"].ToString() == "1") || (ds.Tables[0].Rows[0]["ExtendRateLock"].ToString().ToLower() == "true"))
                    {
                        model.ExtendRateLock = true;
                    }
                    else
                    {
                        model.ExtendRateLock = false;
                    }
                }
                else
                {
                    model.ExtendRateLock = false;
                }

                if (ds.Tables[0].Rows[0]["ConditionRights"] != DBNull.Value)
                {
                    model.ConditionRights = ds.Tables[0].Rows[0]["ConditionRights"].ToString();
                }
                if (ds.Tables[0].Rows[0]["AccessAllMailChimpList"] != DBNull.Value)
                {
                    if ((ds.Tables[0].Rows[0]["AccessAllMailChimpList"].ToString() == "1") || (ds.Tables[0].Rows[0]["AccessAllMailChimpList"].ToString().ToLower() == "true"))
                    {
                        model.AccessAllMailChimpList = true;
                    }
                    else
                    {
                        model.AccessAllMailChimpList = false;
                    }
                }

                model.ExportPipelines = false;
                if (ds.Tables[0].Rows[0]["ExportPipelines"] != DBNull.Value && ds.Tables[0].Rows[0]["ExportPipelines"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ExportPipelines"].ToString() == "1") || (ds.Tables[0].Rows[0]["ExportPipelines"].ToString().ToLower() == "true"))
                    {
                        model.ExportPipelines = true;
                    }
                    else
                    {
                        model.ExportPipelines = false;
                    }
                }

                model.AccessUnassignedLeads = false;
                if (ds.Tables[0].Rows[0]["AccessUnassignedLeads"] != DBNull.Value && ds.Tables[0].Rows[0]["AccessUnassignedLeads"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["AccessUnassignedLeads"].ToString() == "1") || (ds.Tables[0].Rows[0]["AccessUnassignedLeads"].ToString().ToLower() == "true"))
                    {
                        model.AccessUnassignedLeads = true;
                    }
                    else
                    {
                        model.AccessUnassignedLeads = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ViewLockInfo"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ViewLockInfo"].ToString() == "1") || (ds.Tables[0].Rows[0]["ViewLockInfo"].ToString().ToLower() == "true"))
                    {
                        model.ViewLockInfo = true;
                    }
                    else
                    {
                        model.ViewLockInfo = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["LockRate"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["LockRate"].ToString() == "1") || (ds.Tables[0].Rows[0]["LockRate"].ToString().ToLower() == "true"))
                    {
                        model.LockRate = true;
                    }
                    else
                    {
                        model.LockRate = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["AccessProfitability"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["AccessProfitability"].ToString() == "1") || (ds.Tables[0].Rows[0]["AccessProfitability"].ToString().ToLower() == "true"))
                    {
                        model.AccessProfitability = true;
                    }
                    else
                    {
                        model.AccessProfitability = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ViewCompensation"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ViewCompensation"].ToString() == "1") || (ds.Tables[0].Rows[0]["ViewCompensation"].ToString().ToLower() == "true"))
                    {
                        model.ViewCompensation = true;
                    }
                    else
                    {
                        model.ViewCompensation = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["UpdateCondition"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["UpdateCondition"].ToString() == "1") || (ds.Tables[0].Rows[0]["UpdateCondition"].ToString().ToLower() == "true"))
                    {
                        model.UpdateCondition = true;
                    }
                    else
                    {
                        model.UpdateCondition = false;
                    }
                }

                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select RoleId,Name,CompanySetup,LoanSetup,OtherLoanAccess,CustomUserHome,WorkflowTempl,CustomTask,AlertRules,AlertRuleTempl,MarkOtherTaskCompl,AssignTask,ImportLoan,RemoveLoan,AssignLoan,ApplyWorkflow,ApplyAlertRule,SendEmail,CreateNotes,CompanyCalendar,PipelineChart,SalesBreakdownChart,OrgProductionChart,Org_N_Sales_Charts,RateSummary,GoalsChart,OverdueTaskAlerts,Announcements,ExchangeInbox,ExchangeCalendar,SetOwnGoals,SetUserGoals,Reports,ContactMgmt,Prospect,Loans,AccessAllContacts,ContactCompany,ContactBranch,ServiceType,ContactRole,Marketing,SendLSR,ConditionRights,ExtendRateLock,AccessAllMailChimpList,ViewLockInfo,LockRate,AccessProfitability,ViewCompensation,UpdateCondition ");
            strSql.Append(" FROM Roles ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" RoleId,Name,CompanySetup,LoanSetup,OtherLoanAccess,CustomUserHome,WorkflowTempl,CustomTask,AlertRules,AlertRuleTempl,MarkOtherTaskCompl,AssignTask,ImportLoan,RemoveLoan,AssignLoan,ApplyWorkflow,ApplyAlertRule,SendEmail,CreateNotes,CompanyCalendar,PipelineChart,SalesBreakdownChart,OrgProductionChart,Org_N_Sales_Charts,RateSummary,GoalsChart,OverdueTaskAlerts,Announcements,ExchangeInbox,ExchangeCalendar,SetOwnGoals,SetUserGoals,Reports,ContactMgmt,Prospect,Loans,AccessAllContacts,ContactCompany,ContactBranch,ServiceType,ContactRole,Marketing,SendLSR,ConditionRights,ExtendRateLock,AccessAllMailChimpList,ViewLockInfo,LockRate,AccessProfitability,ViewCompensation,UpdateCondition ");
            strSql.Append(" FROM Roles ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@tblName", SqlDbType.VarChar, 255),
                    new SqlParameter("@fldName", SqlDbType.VarChar, 255),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@IsReCount", SqlDbType.Bit),
                    new SqlParameter("@OrderType", SqlDbType.Bit),
                    new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
                    };
            parameters[0].Value = "Roles";
            parameters[1].Value = "";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage", parameters, "ds");
        }

        #endregion  Method
    }
}

