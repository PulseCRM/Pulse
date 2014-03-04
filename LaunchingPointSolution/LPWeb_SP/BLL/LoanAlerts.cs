using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// LoanAlerts ��ժҪ˵����
	/// </summary>
	public class LoanAlerts
	{
		private readonly LPWeb.DAL.LoanAlerts dal=new LPWeb.DAL.LoanAlerts();
		public LoanAlerts()
		{}
		#region  ��Ա����
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(LPWeb.Model.LoanAlerts model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(LPWeb.Model.LoanAlerts model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int LoanAlertId)
        {

            dal.Delete(LoanAlertId);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.LoanAlerts GetModel(int LoanAlertId)
        {

            return dal.GetModel(LoanAlertId);
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
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<LPWeb.Model.LoanAlerts> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<LPWeb.Model.LoanAlerts> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.LoanAlerts> modelList = new List<LPWeb.Model.LoanAlerts>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.LoanAlerts model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.LoanAlerts();
                    if (dt.Rows[n]["LoanAlertId"].ToString() != "")
                    {
                        model.LoanAlertId = int.Parse(dt.Rows[n]["LoanAlertId"].ToString());
                    }
                    if (dt.Rows[n]["FileId"].ToString() != "")
                    {
                        model.FileId = int.Parse(dt.Rows[n]["FileId"].ToString());
                    }
                    model.Desc = dt.Rows[n]["Desc"].ToString();
                    if (dt.Rows[n]["DueDate"].ToString() != "")
                    {
                        model.DueDate = DateTime.Parse(dt.Rows[n]["DueDate"].ToString());
                    }
                    if (dt.Rows[n]["ClearedBy"].ToString() != "")
                    {
                        model.ClearedBy = int.Parse(dt.Rows[n]["ClearedBy"].ToString());
                    }
                    if (dt.Rows[n]["Cleared"].ToString() != "")
                    {
                        model.Cleared = DateTime.Parse(dt.Rows[n]["Cleared"].ToString());
                    }
                    if (dt.Rows[n]["AcknowlegeReq"].ToString() != "")
                    {
                        if ((dt.Rows[n]["AcknowlegeReq"].ToString() == "1") || (dt.Rows[n]["AcknowlegeReq"].ToString().ToLower() == "true"))
                        {
                            model.AcknowlegeReq = true;
                        }
                        else
                        {
                            model.AcknowlegeReq = false;
                        }
                    }
                    model.AcknowledgedBy = dt.Rows[n]["AcknowledgedBy"].ToString();
                    if (dt.Rows[n]["Acknowledged"].ToString() != "")
                    {
                        model.Acknowledged = DateTime.Parse(dt.Rows[n]["Acknowledged"].ToString());
                    }
                    if (dt.Rows[n]["LoanRuleId"].ToString() != "")
                    {
                        model.LoanRuleId = int.Parse(dt.Rows[n]["LoanRuleId"].ToString());
                    }
                    if (dt.Rows[n]["OwnerId"].ToString() != "")
                    {
                        model.OwnerId = int.Parse(dt.Rows[n]["OwnerId"].ToString());
                    }
                    if (dt.Rows[n]["LoanTaskId"].ToString() != "")
                    {
                        model.LoanTaskId = int.Parse(dt.Rows[n]["LoanTaskId"].ToString());
                    }
                    model.AlertType = dt.Rows[n]["AlertType"].ToString();
                    if (dt.Rows[n]["DateCreated"].ToString() != "")
                    {
                        model.DateCreated = DateTime.Parse(dt.Rows[n]["DateCreated"].ToString());
                    }
                    model.Status = dt.Rows[n]["Status"].ToString();
                    if (dt.Rows[n]["Accepted"].ToString() != "")
                    {
                        model.Accepted = DateTime.Parse(dt.Rows[n]["Accepted"].ToString());
                    }
                    if (dt.Rows[n]["Declined"].ToString() != "")
                    {
                        model.Declined = DateTime.Parse(dt.Rows[n]["Declined"].ToString());
                    }
                    if (dt.Rows[n]["Dismissed"].ToString() != "")
                    {
                        model.Dismissed = DateTime.Parse(dt.Rows[n]["Dismissed"].ToString());
                    }
                    model.AcceptedBy = dt.Rows[n]["AcceptedBy"].ToString();
                    model.DeclinedBy = dt.Rows[n]["DeclinedBy"].ToString();
                    model.DismissedBy = dt.Rows[n]["DismissedBy"].ToString();
                    if (dt.Rows[n]["AlertEmail"].ToString() != "")
                    {
                        model.AlertEmail = (byte[])dt.Rows[n]["AlertEmail"];
                    }
                    if (dt.Rows[n]["RecomEmail"].ToString() != "")
                    {
                        model.RecomEmail = (byte[])dt.Rows[n]["RecomEmail"];
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
        public DataSet GetList(int PageSize, int PageIndex, string strWhere)
        {
            return dal.GetList(PageSize, PageIndex, strWhere);
        }

        /// <summary>
        /// ��ҳ��ȡ�����б�
        /// </summary>
        /// <param name="PageSize">ÿҳ�ļ�¼��</param>
        /// <param name="PageIndex">ҳ��</param>
        /// <param name="orderField">�����ֶ�</param>
        /// <param name="sortDirection">������(false������true�ǽ���)</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="count">���ؼ�¼����</param>
        /// <returns></returns>
        public DataSet GetList(int PageSize, int PageIndex, string orderField, bool sortDirection, string strWhere, out int count)
        {
            return dal.GetList(PageSize, PageIndex, orderField, sortDirection, strWhere, out count);
        }

		#endregion  ��Ա����

	    public DataTable GetBranches(string con)
	    {
	        return dal.GetBranches(con);
	    }

        /// <summary>
        /// ��ȡ��Alert List for Dashboard Home
        /// neo 2010-10-12
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public DataTable GetSimpleAlertList(int iUserID)
        {
            return dal.GetSimpleAlertListBase(iUserID);
        }

        public DataTable GetSimpleAlertList(int iUserID, string sWhere_DueDate)
        {
            return dal.GetSimpleAlertListBase(iUserID, sWhere_DueDate);
        }

        public DataTable Loan_GetSimpleAlertList(int iUserID, string sWhere_DueDate)
        {
            return dal.Loan_GetSimpleAlertListBase(iUserID, sWhere_DueDate);
        }

        /// <summary>
        /// �õ������Rule Alert ID
        /// Alex 2011-01-24
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public int GetRuleAlertID(int iLoanID)
        {
            return dal.GetLoadRuleAlertID(iLoanID);
        }

        /// <summary>
        /// �õ����е�OwnerName
        /// Alex 2011-10-11
        /// </summary>
        /// <returns></returns>
        public DataTable GetAlertOwner()
        {
            return dal.GetAlertOwner();
        }
	}
}

