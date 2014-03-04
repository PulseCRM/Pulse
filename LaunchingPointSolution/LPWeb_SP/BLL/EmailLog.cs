using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ҵ���߼���EmailLog ��ժҪ˵����
	/// </summary>
	public class EmailLog
	{
		private readonly LPWeb.DAL.EmailLog dal=new LPWeb.DAL.EmailLog();
		public EmailLog()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(LPWeb.Model.EmailLog model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.EmailLog model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int EmailLogId)
		{
			
			dal.Delete(EmailLogId);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.EmailLog GetModel(int EmailLogId)
		{
			
			return dal.GetModel(EmailLogId);
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
		public List<LPWeb.Model.EmailLog> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.EmailLog> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.EmailLog> modelList = new List<LPWeb.Model.EmailLog>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.EmailLog model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.EmailLog();
					if(dt.Rows[n]["EmailLogId"].ToString()!="")
					{
						model.EmailLogId=int.Parse(dt.Rows[n]["EmailLogId"].ToString());
					}
					model.ToUser=dt.Rows[n]["ToUser"].ToString();
					model.ToContact=dt.Rows[n]["ToContact"].ToString();
					if(dt.Rows[n]["EmailTmplId"].ToString()!="")
					{
						model.EmailTmplId=int.Parse(dt.Rows[n]["EmailTmplId"].ToString());
					}
					if(dt.Rows[n]["Success"].ToString()!="")
					{
						if((dt.Rows[n]["Success"].ToString()=="1")||(dt.Rows[n]["Success"].ToString().ToLower()=="true"))
						{
						model.Success=true;
						}
						else
						{
							model.Success=false;
						}
					}
					model.Error=dt.Rows[n]["Error"].ToString();
					if(dt.Rows[n]["LastSent"].ToString()!="")
					{
						model.LastSent=DateTime.Parse(dt.Rows[n]["LastSent"].ToString());
					}
					if(dt.Rows[n]["LoanAlertId"].ToString()!="")
					{
						model.LoanAlertId=int.Parse(dt.Rows[n]["LoanAlertId"].ToString());
					}
					if(dt.Rows[n]["Retries"].ToString()!="")
					{
						model.Retries=int.Parse(dt.Rows[n]["Retries"].ToString());
					}
					if(dt.Rows[n]["FileId"].ToString()!="")
					{
						model.FileId=int.Parse(dt.Rows[n]["FileId"].ToString());
					}
					model.FromEmail=dt.Rows[n]["FromEmail"].ToString();
					if(dt.Rows[n]["FromUser"].ToString()!="")
					{
						model.FromUser=int.Parse(dt.Rows[n]["FromUser"].ToString());
					}
					if(dt.Rows[n]["Created"].ToString()!="")
					{
						model.Created=DateTime.Parse(dt.Rows[n]["Created"].ToString());
					}
					if(dt.Rows[n]["AlertEmailType"].ToString()!="")
					{
						model.AlertEmailType=int.Parse(dt.Rows[n]["AlertEmailType"].ToString());
					}
					if(dt.Rows[n]["EmailBody"].ToString()!="")
					{
						model.EmailBody=(byte[])dt.Rows[n]["EmailBody"];
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

        /// <summary>
        /// 
        /// </summary>
        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetListForGridView(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        public DataSet GetListForGridView_Client(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetListForGridView_Client(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        /// <summary>
        /// get email log list for prospect
        /// neo 2011-04-28
        /// </summary>
        /// <param name="sDbTable"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="orderName"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public DataTable GetProspectEmailLogList(string sDbTable, int iStartIndex, int iEndIndex, string strWhere, string orderName, int orderType)
        {
            return dal.GetProspectEmailLogListBase(sDbTable, iStartIndex, iEndIndex, strWhere, orderName, orderType);
        }

        public DataTable GetEmailLogAttachments(int iEmailLogID)
        {
            return dal.GetEmailLogAttachments(iEmailLogID);
        }
	}
}

