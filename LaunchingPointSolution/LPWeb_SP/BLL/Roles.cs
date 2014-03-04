using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// Roles 的摘要说明。
	/// </summary>
	public class Roles
	{
		private readonly LPWeb.DAL.Roles dal=new LPWeb.DAL.Roles();
		public Roles()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.Roles model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.Roles model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int RoleId)
		{
			
			dal.Delete(RoleId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.Roles GetModel(int RoleId)
		{
			
			return dal.GetModel(RoleId);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.Roles> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.Roles> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.Roles> modelList = new List<LPWeb.Model.Roles>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.Roles model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.Roles();
					if(dt.Rows[n]["RoleId"].ToString()!="")
					{
						model.RoleId=int.Parse(dt.Rows[n]["RoleId"].ToString());
					}
					model.Name=dt.Rows[n]["Name"].ToString();
					if(dt.Rows[n]["CompanySetup"].ToString()!="")
					{
						if((dt.Rows[n]["CompanySetup"].ToString()=="1")||(dt.Rows[n]["CompanySetup"].ToString().ToLower()=="true"))
						{
						model.CompanySetup=true;
						}
						else
						{
							model.CompanySetup=false;
						}
					}
					if(dt.Rows[n]["LoanSetup"].ToString()!="")
					{
						if((dt.Rows[n]["LoanSetup"].ToString()=="1")||(dt.Rows[n]["LoanSetup"].ToString().ToLower()=="true"))
						{
						model.LoanSetup=true;
						}
						else
						{
							model.LoanSetup=false;
						}
					}
					if(dt.Rows[n]["OtherLoanAccess"].ToString()!="")
					{
						if((dt.Rows[n]["OtherLoanAccess"].ToString()=="1")||(dt.Rows[n]["OtherLoanAccess"].ToString().ToLower()=="true"))
						{
						model.OtherLoanAccess=true;
						}
						else
						{
							model.OtherLoanAccess=false;
						}
					}
					if(dt.Rows[n]["CustomUserHome"].ToString()!="")
					{
						if((dt.Rows[n]["CustomUserHome"].ToString()=="1")||(dt.Rows[n]["CustomUserHome"].ToString().ToLower()=="true"))
						{
						model.CustomUserHome=true;
						}
						else
						{
							model.CustomUserHome=false;
						}
					}
					if(dt.Rows[n]["WorkflowTempl"].ToString()!="")
					{
						model.WorkflowTempl=int.Parse(dt.Rows[n]["WorkflowTempl"].ToString());
					}
					if(dt.Rows[n]["CustomTask"].ToString()!="")
					{
						model.CustomTask=int.Parse(dt.Rows[n]["CustomTask"].ToString());
					}
					if(dt.Rows[n]["AlertRules"].ToString()!="")
					{
						model.AlertRules=int.Parse(dt.Rows[n]["AlertRules"].ToString());
					}
					if(dt.Rows[n]["AlertRuleTempl"].ToString()!="")
					{
						model.AlertRuleTempl=int.Parse(dt.Rows[n]["AlertRuleTempl"].ToString());
					}
					if(dt.Rows[n]["MarkOtherTaskCompl"].ToString()!="")
					{
						if((dt.Rows[n]["MarkOtherTaskCompl"].ToString()=="1")||(dt.Rows[n]["MarkOtherTaskCompl"].ToString().ToLower()=="true"))
						{
						model.MarkOtherTaskCompl=true;
						}
						else
						{
							model.MarkOtherTaskCompl=false;
						}
					}
					if(dt.Rows[n]["AssignTask"].ToString()!="")
					{
						if((dt.Rows[n]["AssignTask"].ToString()=="1")||(dt.Rows[n]["AssignTask"].ToString().ToLower()=="true"))
						{
						model.AssignTask=true;
						}
						else
						{
							model.AssignTask=false;
						}
					}
					if(dt.Rows[n]["ImportLoan"].ToString()!="")
					{
						if((dt.Rows[n]["ImportLoan"].ToString()=="1")||(dt.Rows[n]["ImportLoan"].ToString().ToLower()=="true"))
						{
						model.ImportLoan=true;
						}
						else
						{
							model.ImportLoan=false;
						}
					}
					if(dt.Rows[n]["RemoveLoan"].ToString()!="")
					{
						if((dt.Rows[n]["RemoveLoan"].ToString()=="1")||(dt.Rows[n]["RemoveLoan"].ToString().ToLower()=="true"))
						{
						model.RemoveLoan=true;
						}
						else
						{
							model.RemoveLoan=false;
						}
					}
					if(dt.Rows[n]["AssignLoan"].ToString()!="")
					{
						if((dt.Rows[n]["AssignLoan"].ToString()=="1")||(dt.Rows[n]["AssignLoan"].ToString().ToLower()=="true"))
						{
						model.AssignLoan=true;
						}
						else
						{
							model.AssignLoan=false;
						}
					}
					if(dt.Rows[n]["ApplyWorkflow"].ToString()!="")
					{
						if((dt.Rows[n]["ApplyWorkflow"].ToString()=="1")||(dt.Rows[n]["ApplyWorkflow"].ToString().ToLower()=="true"))
						{
						model.ApplyWorkflow=true;
						}
						else
						{
							model.ApplyWorkflow=false;
						}
					}
					if(dt.Rows[n]["ApplyAlertRule"].ToString()!="")
					{
						if((dt.Rows[n]["ApplyAlertRule"].ToString()=="1")||(dt.Rows[n]["ApplyAlertRule"].ToString().ToLower()=="true"))
						{
						model.ApplyAlertRule=true;
						}
						else
						{
							model.ApplyAlertRule=false;
						}
					}
					if(dt.Rows[n]["SendEmail"].ToString()!="")
					{
						if((dt.Rows[n]["SendEmail"].ToString()=="1")||(dt.Rows[n]["SendEmail"].ToString().ToLower()=="true"))
						{
						model.SendEmail=true;
						}
						else
						{
							model.SendEmail=false;
						}
					}
					if(dt.Rows[n]["CreateNotes"].ToString()!="")
					{
						if((dt.Rows[n]["CreateNotes"].ToString()=="1")||(dt.Rows[n]["CreateNotes"].ToString().ToLower()=="true"))
						{
						model.CreateNotes=true;
						}
						else
						{
							model.CreateNotes=false;
						}
					}
					if(dt.Rows[n]["CompanyCalendar"].ToString()!="")
					{
						if((dt.Rows[n]["CompanyCalendar"].ToString()=="1")||(dt.Rows[n]["CompanyCalendar"].ToString().ToLower()=="true"))
						{
						model.CompanyCalendar=true;
						}
						else
						{
							model.CompanyCalendar=false;
						}
					}
					if(dt.Rows[n]["PipelineChart"].ToString()!="")
					{
						if((dt.Rows[n]["PipelineChart"].ToString()=="1")||(dt.Rows[n]["PipelineChart"].ToString().ToLower()=="true"))
						{
						model.PipelineChart=true;
						}
						else
						{
							model.PipelineChart=false;
						}
					}
					if(dt.Rows[n]["SalesBreakdownChart"].ToString()!="")
					{
						if((dt.Rows[n]["SalesBreakdownChart"].ToString()=="1")||(dt.Rows[n]["SalesBreakdownChart"].ToString().ToLower()=="true"))
						{
						model.SalesBreakdownChart=true;
						}
						else
						{
							model.SalesBreakdownChart=false;
						}
					}
					if(dt.Rows[n]["OrgProductionChart"].ToString()!="")
					{
						if((dt.Rows[n]["OrgProductionChart"].ToString()=="1")||(dt.Rows[n]["OrgProductionChart"].ToString().ToLower()=="true"))
						{
						model.OrgProductionChart=true;
						}
						else
						{
							model.OrgProductionChart=false;
						}
					}
					if(dt.Rows[n]["Org_N_Sales_Charts"].ToString()!="")
					{
						if((dt.Rows[n]["Org_N_Sales_Charts"].ToString()=="1")||(dt.Rows[n]["Org_N_Sales_Charts"].ToString().ToLower()=="true"))
						{
						model.Org_N_Sales_Charts=true;
						}
						else
						{
							model.Org_N_Sales_Charts=false;
						}
					}
					if(dt.Rows[n]["RateSummary"].ToString()!="")
					{
						if((dt.Rows[n]["RateSummary"].ToString()=="1")||(dt.Rows[n]["RateSummary"].ToString().ToLower()=="true"))
						{
						model.RateSummary=true;
						}
						else
						{
							model.RateSummary=false;
						}
					}
					if(dt.Rows[n]["GoalsChart"].ToString()!="")
					{
						if((dt.Rows[n]["GoalsChart"].ToString()=="1")||(dt.Rows[n]["GoalsChart"].ToString().ToLower()=="true"))
						{
						model.GoalsChart=true;
						}
						else
						{
							model.GoalsChart=false;
						}
					}
					if(dt.Rows[n]["OverdueTaskAlerts"].ToString()!="")
					{
						if((dt.Rows[n]["OverdueTaskAlerts"].ToString()=="1")||(dt.Rows[n]["OverdueTaskAlerts"].ToString().ToLower()=="true"))
						{
						model.OverdueTaskAlerts=true;
						}
						else
						{
							model.OverdueTaskAlerts=false;
						}
					}
					if(dt.Rows[n]["Announcements"].ToString()!="")
					{
						if((dt.Rows[n]["Announcements"].ToString()=="1")||(dt.Rows[n]["Announcements"].ToString().ToLower()=="true"))
						{
						model.Announcements=true;
						}
						else
						{
							model.Announcements=false;
						}
					}
					if(dt.Rows[n]["ExchangeInbox"].ToString()!="")
					{
						if((dt.Rows[n]["ExchangeInbox"].ToString()=="1")||(dt.Rows[n]["ExchangeInbox"].ToString().ToLower()=="true"))
						{
						model.ExchangeInbox=true;
						}
						else
						{
							model.ExchangeInbox=false;
						}
					}
					if(dt.Rows[n]["ExchangeCalendar"].ToString()!="")
					{
						if((dt.Rows[n]["ExchangeCalendar"].ToString()=="1")||(dt.Rows[n]["ExchangeCalendar"].ToString().ToLower()=="true"))
						{
						model.ExchangeCalendar=true;
						}
						else
						{
							model.ExchangeCalendar=false;
						}
					}
                    if (dt.Rows[n]["SetOwnGoals"].ToString() != "")
                    {
                        if ((dt.Rows[n]["SetOwnGoals"].ToString() == "1") || (dt.Rows[n]["SetOwnGoals"].ToString().ToLower() == "true"))
                        {
                            model.SetOwnGoals = true;
                        }
                        else
                        {
                            model.SetOwnGoals = false;
                        }
                    }
                    if (dt.Rows[n]["SetUserGoals"].ToString() != "")
                    {
                        if ((dt.Rows[n]["SetUserGoals"].ToString() == "1") || (dt.Rows[n]["SetUserGoals"].ToString().ToLower() == "true"))
                        {
                            model.SetUserGoals = true;
                        }
                        else
                        {
                            model.SetUserGoals = false;
                        }
                    }
                    if (dt.Rows[n]["Reports"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Reports"].ToString() == "1") || (dt.Rows[n]["Reports"].ToString().ToLower() == "true"))
                        {
                            model.Reports = true;
                        }
                        else
                        {
                            model.Reports = false;
                        }
                    }
                    if (dt.Rows[n]["ContactMgmt"].ToString() != "")
                    {
                        model.ContactMgmt = int.Parse(dt.Rows[n]["ContactMgmt"].ToString());
                    }
                    model.Prospect = dt.Rows[n]["Prospect"].ToString();
                    model.Loans = dt.Rows[n]["Loans"].ToString();
                    if (dt.Rows[n]["AccessAllContacts"].ToString() != "")
                    {
                        if ((dt.Rows[n]["AccessAllContacts"].ToString() == "1") || (dt.Rows[n]["AccessAllContacts"].ToString().ToLower() == "true"))
                        {
                            model.AccessAllContacts = true;
                        }
                        else
                        {
                            model.AccessAllContacts = false;
                        }
                    }
                    if (dt.Rows[n]["ContactCompany"].ToString() != "")
                    {
                        model.ContactCompany = int.Parse(dt.Rows[n]["ContactCompany"].ToString());
                    }
                    if (dt.Rows[n]["ContactBranch"].ToString() != "")
                    {
                        model.ContactBranch = int.Parse(dt.Rows[n]["ContactBranch"].ToString());
                    }
                    if (dt.Rows[n]["ServiceType"].ToString() != "")
                    {
                        model.ServiceType = int.Parse(dt.Rows[n]["ServiceType"].ToString());
                    }
                    if (dt.Rows[n]["ContactRole"].ToString() != "")
                    {
                        model.ContactRole = int.Parse(dt.Rows[n]["ContactRole"].ToString());
                    }
					modelList.Add(model);
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			return dal.GetList(PageSize,PageIndex,strWhere);
		}

        #endregion  成员方法

        /// <summary>
        /// 获取Role List
        /// neo 2010-12-07
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetRoleList(string sWhere)
        {
            return dal.GetRoleListBase(sWhere);
        }

        /// <summary>
        /// 获取recipient role list
        /// neo 2010-12-08
        /// </summary>
        /// <returns></returns>
        public DataTable GetRecipientRoleList()
        {
            return dal.GetRecipientRoleListBase();
        }


        public DataTable GetRoleByUserID(string sUserID)
        {
            return dal.GetRoleByUserID(sUserID);
        }

        public DataTable GetRoleByUserID(int iUserId)
        {
            return dal.GetRoleByUserID(iUserId);
        }
    }
}

