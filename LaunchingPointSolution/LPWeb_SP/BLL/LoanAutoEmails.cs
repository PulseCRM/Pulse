using System;
using System.Data;
using System.Collections.Generic;
 
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// 业务逻辑类LoanAutoEmails 的摘要说明。
	/// </summary>
	public class LoanAutoEmails
	{
		private readonly LPWeb.DAL.LoanAutoEmails dal=new LPWeb.DAL.LoanAutoEmails();
		public LoanAutoEmails()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.LoanAutoEmails model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.LoanAutoEmails model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int LoanAutoEmailid)
		{
			
			dal.Delete(LoanAutoEmailid);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.LoanAutoEmails GetModel(int LoanAutoEmailid)
		{
			
			return dal.GetModel(LoanAutoEmailid);
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
		public List<LPWeb.Model.LoanAutoEmails> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.LoanAutoEmails> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.LoanAutoEmails> modelList = new List<LPWeb.Model.LoanAutoEmails>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.LoanAutoEmails model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.LoanAutoEmails();
					if(dt.Rows[n]["LoanAutoEmailid"].ToString()!="")
					{
						model.LoanAutoEmailid=int.Parse(dt.Rows[n]["LoanAutoEmailid"].ToString());
					}
					if(dt.Rows[n]["FileId"].ToString()!="")
					{
						model.FileId=int.Parse(dt.Rows[n]["FileId"].ToString());
					}
					if(dt.Rows[n]["ToContactId"].ToString()!="")
					{
						model.ToContactId=int.Parse(dt.Rows[n]["ToContactId"].ToString());
					}
					if(dt.Rows[n]["ToUserId"].ToString()!="")
					{
						model.ToUserId=int.Parse(dt.Rows[n]["ToUserId"].ToString());
					}
					if(dt.Rows[n]["Enabled"].ToString()!="")
					{
						if((dt.Rows[n]["Enabled"].ToString()=="1")||(dt.Rows[n]["Enabled"].ToString().ToLower()=="true"))
						{
						model.Enabled=true;
						}
						else
						{
							model.Enabled=false;
						}
					}
					if(dt.Rows[n]["External"].ToString()!="")
					{
						if((dt.Rows[n]["External"].ToString()=="1")||(dt.Rows[n]["External"].ToString().ToLower()=="true"))
						{
						model.External=true;
						}
						else
						{
							model.External=false;
						}
					}
					if(dt.Rows[n]["TemplReportId"].ToString()!="")
					{
						model.TemplReportId=int.Parse(dt.Rows[n]["TemplReportId"].ToString());
					}
					if(dt.Rows[n]["Applied"].ToString()!="")
					{
						model.Applied=DateTime.Parse(dt.Rows[n]["Applied"].ToString());
					}
					if(dt.Rows[n]["AppliedBy"].ToString()!="")
					{
						model.AppliedBy=int.Parse(dt.Rows[n]["AppliedBy"].ToString());
					}
					if(dt.Rows[n]["LastRun"].ToString()!="")
					{
						model.LastRun=DateTime.Parse(dt.Rows[n]["LastRun"].ToString());
					}
					if(dt.Rows[n]["ScheduleType"].ToString()!="")
					{
						model.ScheduleType=int.Parse(dt.Rows[n]["ScheduleType"].ToString());
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


        public void UpdateEmailSettings(LPWeb.Model.LoanAutoEmails model)
        {
            dal.UpdateEmailSettings(model);
        }
        public int GetLoanAutoEmailIdByContactUserId(int FileId, int UserId=0, int ContactId=0)
        { 
            return dal.GetLoanAutoEmailIdByContactUserId(FileId, UserId, ContactId);
        }
	}
}

