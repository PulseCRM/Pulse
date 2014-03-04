using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// LoanWflTempl ��ժҪ˵����
	/// </summary>
	public class LoanWflTempl
	{
		private readonly LPWeb.DAL.LoanWflTempl dal=new LPWeb.DAL.LoanWflTempl();
		public LoanWflTempl()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(LPWeb.Model.LoanWflTempl model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.LoanWflTempl model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int WflTemplId,int FileId)
		{
			
			dal.Delete(WflTemplId,FileId);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.LoanWflTempl GetModel(int WflTemplId,int FileId)
		{
			
			return dal.GetModel(WflTemplId,FileId);
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
		public List<LPWeb.Model.LoanWflTempl> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.LoanWflTempl> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.LoanWflTempl> modelList = new List<LPWeb.Model.LoanWflTempl>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.LoanWflTempl model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.LoanWflTempl();
					if(dt.Rows[n]["FileId"].ToString()!="")
					{
						model.FileId=int.Parse(dt.Rows[n]["FileId"].ToString());
					}
					if(dt.Rows[n]["WflTemplId"].ToString()!="")
					{
						model.WflTemplId=int.Parse(dt.Rows[n]["WflTemplId"].ToString());
					}
					if(dt.Rows[n]["ApplyDate"].ToString()!="")
					{
						model.ApplyDate=DateTime.Parse(dt.Rows[n]["ApplyDate"].ToString());
					}
					if(dt.Rows[n]["ApplyBy"].ToString()!="")
					{
						model.ApplyBy=int.Parse(dt.Rows[n]["ApplyBy"].ToString());
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

        public void Apply(LPWeb.Model.LoanWflTempl model)
        {
            dal.Apply(model);
        }

        /// <summary>
        /// Get LoanTask Infos
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="count"></param>
        /// <param name="orderName"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public DataSet GetWorkflowSetupList(int pageSize, int pageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetWorkflowSetupList(pageSize, pageIndex,strWhere, out count,orderName,orderType);
        }

        /// <summary>
        /// get LoanWflTempl info
        /// neo 2011-03-21
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <returns></returns>
        public DataTable GetLoanWorkflowTemplateInfo(int iLoanID)
        {
            return dal.GetLoanWorkflowTemplateInfoBase(iLoanID);
        }
	}
}

