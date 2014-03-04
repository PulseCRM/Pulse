using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// LoanTeam 的摘要说明。
	/// </summary>
	public class LoanTeam
	{
		private readonly LPWeb.DAL.LoanTeam dal=new LPWeb.DAL.LoanTeam();
		public LoanTeam()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.LoanTeam model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.LoanTeam model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int FileId,int RoleId,int UserId)
		{
			
			dal.Delete(FileId,RoleId,UserId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.LoanTeam GetModel(int FileId,int RoleId,int UserId)
		{
			
			return dal.GetModel(FileId,RoleId,UserId);
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
		public List<LPWeb.Model.LoanTeam> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.LoanTeam> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.LoanTeam> modelList = new List<LPWeb.Model.LoanTeam>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.LoanTeam model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.LoanTeam();
					if(dt.Rows[n]["FileId"].ToString()!="")
					{
						model.FileId=int.Parse(dt.Rows[n]["FileId"].ToString());
					}
					if(dt.Rows[n]["RoleId"].ToString()!="")
					{
						model.RoleId=int.Parse(dt.Rows[n]["RoleId"].ToString());
					}
					if(dt.Rows[n]["UserId"].ToString()!="")
					{
						model.UserId=int.Parse(dt.Rows[n]["UserId"].ToString());
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

        public void Reassign(LPWeb.Model.LoanTeam oldModel, LPWeb.Model.LoanTeam model, int UserId)
        {
            dal.Reassign(oldModel, model, UserId);
        }
        public string GetLoanOfficer(int FileId)
        {
            return dal.GetLoanOfficer(FileId);
        }
        public int GetLoanOfficerID(int FileId)
        {
            return dal.GetLoanOfficerID(FileId);
        }
        public string GetProcessor(int FileId)
        {
            return dal.GetProcessor(FileId);
        }

        public DataSet GetUserLoan(int nUserId)
        {
            return dal.GetUserLoan(nUserId);
        }

        public int GetUserLoanCount(int nUserId)
        {
            return dal.GetUserLoanCount(nUserId);
        }

        /// <summary>
        /// get loan officer user info
        /// neo 2012-12-29
        /// </summary>
        /// <param name="iFileId"></param>
        /// <returns></returns>
        public DataTable GetLoanOfficerInfo(int iFileId)
        {
            return dal.GetLoanOfficerInfo(iFileId);
        }
	}
}

