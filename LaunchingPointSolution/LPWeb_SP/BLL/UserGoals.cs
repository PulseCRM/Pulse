using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ҵ���߼���UserGoals ��ժҪ˵����
	/// </summary>
	public class UserGoals
	{
		private readonly LPWeb.DAL.UserGoals dal=new LPWeb.DAL.UserGoals();
		public UserGoals()
		{}
		#region  ��Ա����
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(LPWeb.Model.UserGoals model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.UserGoals model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int GoalId)
		{
			
			dal.Delete(GoalId);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.UserGoals GetModel(int GoalId)
		{
			
			return dal.GetModel(GoalId);
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
		public List<LPWeb.Model.UserGoals> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<LPWeb.Model.UserGoals> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.UserGoals> modelList = new List<LPWeb.Model.UserGoals>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.UserGoals model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.UserGoals();
					if(dt.Rows[n]["GoalId"].ToString()!="")
					{
						model.GoalId=int.Parse(dt.Rows[n]["GoalId"].ToString());
					}
					if(dt.Rows[n]["UserId"].ToString()!="")
					{
						model.UserId=int.Parse(dt.Rows[n]["UserId"].ToString());
					}
					if(dt.Rows[n]["LowRange"].ToString()!="")
					{
						model.LowRange=decimal.Parse(dt.Rows[n]["LowRange"].ToString());
					}
					if(dt.Rows[n]["MediumRange"].ToString()!="")
					{
						model.MediumRange=decimal.Parse(dt.Rows[n]["MediumRange"].ToString());
					}
					if(dt.Rows[n]["HighRange"].ToString()!="")
					{
						model.HighRange=decimal.Parse(dt.Rows[n]["HighRange"].ToString());
					}
					if(dt.Rows[n]["GoalType"].ToString()!="")
					{
                        model.Month = int.Parse(dt.Rows[n]["Month"].ToString());
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
        /// get user goals of 12 months
        /// neo 2010-09-15
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public DataTable GetUserGoals(int iUserID)
        {
            return dal.GetUserGoalsBase(iUserID);
        }

        /// <summary>
        /// get user goals
        /// neo 2010-09-15
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="iMonth"></param>
        /// <returns></returns>
        public DataTable GetUserGoals(int iUserID, int iMonth)
        {
            return dal.GetUserGoalsBase(iUserID, iMonth);
        }

        /// <summary>
        /// get user goals for multi-months
        /// neo 2010-09-15
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="sMonths"></param>
        /// <returns></returns>
        public DataTable GetUserGoals(int iUserID, string sMonths)
        {
            return dal.GetUserGoalsBase(iUserID, sMonths);
        }

        /// <summary>
        /// get loan amount of this month/quarter/year
        /// neo 2010-09-15
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public decimal GetUserLoanAmount_This(int iUserID, DateTime StartDate, DateTime EndDate)
        {
            return dal.GetUserLoanAmountBase_This(iUserID, StartDate, EndDate);
        }

        /// <summary>
        /// get loan amount of next month/quarter/year
        /// neo 2010-09-15
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public decimal GetUserLoanAmount_Next(int iUserID, DateTime StartDate, DateTime EndDate)
        {
            return dal.GetUserLoanAmountBase_Next(iUserID, StartDate, EndDate);
        }
        
        /// <summary>
        /// get user and his name
        /// </summary>
        /// <param name="strUserIDs"></param>
        /// <returns></returns>
        public DataSet GetUserForGoalsGrid(string strUserIDs) 
        {
            return dal.GetUserForGoalsGrid(strUserIDs);
        }

        /// <summary>
        /// get user goals, multi-user and multi months
        /// </summary>
        /// <param name="strUserID"></param>
        /// <param name="strMonth"></param>
        /// <returns></returns>
        public DataSet GetUserGoals(string strUserID, string strMonth)
        {
            if (string.IsNullOrEmpty(strUserID))
                strUserID = "-1";
            if (string.IsNullOrEmpty(strMonth))
                strMonth = "-1";
            return dal.GetUserGoals(strUserID, strMonth);
        }

        /// <summary>
        /// save user goals info
        /// </summary>
        /// <param name="strUserIDs"></param>
        /// <param name="strMonths"></param>
        /// <param name="dsUserGoals"></param>
        public void SaveUserGoals(string strUserIDs, string strMonths, DataSet dsUserGoals)
        {
            dal.SaveUserGoals(strUserIDs, strMonths, dsUserGoals);
        }
	}
}

