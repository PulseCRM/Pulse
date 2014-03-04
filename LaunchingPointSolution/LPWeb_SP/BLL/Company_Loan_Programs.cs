using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// Company_Loan_Programs ��ժҪ˵����
	/// </summary>
	public class Company_Loan_Programs
	{
		private readonly LPWeb.DAL.Company_Loan_Programs dal=new LPWeb.DAL.Company_Loan_Programs();
		public Company_Loan_Programs()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(LPWeb.Model.Company_Loan_Programs model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.Company_Loan_Programs model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int LoanProgramID)
		{
			
			dal.Delete(LoanProgramID);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.Company_Loan_Programs GetModel(int LoanProgramID)
		{
			
			return dal.GetModel(LoanProgramID);
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
		public List<LPWeb.Model.Company_Loan_Programs> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.Company_Loan_Programs> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.Company_Loan_Programs> modelList = new List<LPWeb.Model.Company_Loan_Programs>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.Company_Loan_Programs model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.Company_Loan_Programs();
					if(dt.Rows[n]["LoanProgramID"].ToString()!="")
					{
						model.LoanProgramID=int.Parse(dt.Rows[n]["LoanProgramID"].ToString());
					}
					model.LoanProgram=dt.Rows[n]["LoanProgram"].ToString();

                    model.IsARM = (dt.Rows[n]["IsARM"] != DBNull.Value && Convert.ToBoolean(dt.Rows[n]["IsARM"]) == true) ? true : false;

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
        /// ��������б�
        /// </summary>
        public DataSet GetListInvestorARMprogram(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetListInvestorARMprogram(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }


        public DataSet GetInvestorsList()
        {
            return dal.GetInvestorsList();
        }

        public DataSet GetProgramsList(string whereStr)
        {
            return dal.GetProgramsList(whereStr);
        }

        public DataSet GetIndexesList(string whereStr)
        {
            return dal.GetIndexesList(whereStr);
        }
	}
}

