using System;
using System.Data;
using System.Collections.Generic;
 
using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ҵ���߼���LoanAutoEmails ��ժҪ˵����
	/// </summary>
	public class LoanAutoEmails
	{
		private readonly LPWeb.DAL.LoanAutoEmails dal=new LPWeb.DAL.LoanAutoEmails();
		public LoanAutoEmails()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(LPWeb.Model.LoanAutoEmails model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.LoanAutoEmails model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int LoanAutoEmailid)
		{
			
			dal.Delete(LoanAutoEmailid);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.LoanAutoEmails GetModel(int LoanAutoEmailid)
		{
			
			return dal.GetModel(LoanAutoEmailid);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// ���ǰ��������
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.LoanAutoEmails> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
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
		/// ��������б�
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			return dal.GetList(PageSize,PageIndex,strWhere);
		}

		#endregion  ��Ա����


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

