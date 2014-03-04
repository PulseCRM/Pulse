using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// EmailQue ��ժҪ˵����
	/// </summary>
	public class EmailQue
	{
		private readonly LPWeb.DAL.EmailQue dal=new LPWeb.DAL.EmailQue();
		public EmailQue()
		{}
		#region  ��Ա����
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(LPWeb.Model.EmailQue model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(LPWeb.Model.EmailQue model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int EmailId)
        {

            dal.Delete(EmailId);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.EmailQue GetModel(int EmailId)
        {

            return dal.GetModel(EmailId);
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
        public List<LPWeb.Model.EmailQue> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<LPWeb.Model.EmailQue> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.EmailQue> modelList = new List<LPWeb.Model.EmailQue>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.EmailQue model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.EmailQue();
                    if (dt.Rows[n]["EmailId"].ToString() != "")
                    {
                        model.EmailId = int.Parse(dt.Rows[n]["EmailId"].ToString());
                    }
                    model.ToUser = dt.Rows[n]["ToUser"].ToString();
                    model.ToContact = dt.Rows[n]["ToContact"].ToString();
                    model.ToBorrower = dt.Rows[n]["ToBorrower"].ToString();
                    if (dt.Rows[n]["EmailTmplId"].ToString() != "")
                    {
                        model.EmailTmplId = int.Parse(dt.Rows[n]["EmailTmplId"].ToString());
                    }
                    if (dt.Rows[n]["LoanAlertId"].ToString() != "")
                    {
                        model.LoanAlertId = int.Parse(dt.Rows[n]["LoanAlertId"].ToString());
                    }
                    if (dt.Rows[n]["FileId"].ToString() != "")
                    {
                        model.FileId = int.Parse(dt.Rows[n]["FileId"].ToString());
                    }
                    if (dt.Rows[n]["AlertEmailType"].ToString() != "")
                    {
                        model.AlertEmailType = int.Parse(dt.Rows[n]["AlertEmailType"].ToString());
                    }
                    if (dt.Rows[n]["EmailBody"].ToString() != "")
                    {
                        model.EmailBody = (byte[])dt.Rows[n]["EmailBody"];
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

		#endregion  ��Ա����
	}
}

