using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// 业务逻辑类UserLoanRep 的摘要说明。
	/// </summary>
	public class UserLoanRep
	{
		private readonly LPWeb.DAL.UserLoanRep dal=new LPWeb.DAL.UserLoanRep();
		public UserLoanRep()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.UserLoanRep model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.UserLoanRep model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int NameId)
		{
			
			dal.Delete(NameId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.UserLoanRep GetModel(int NameId)
		{
			
			return dal.GetModel(NameId);
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
		public List<LPWeb.Model.UserLoanRep> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.UserLoanRep> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.UserLoanRep> modelList = new List<LPWeb.Model.UserLoanRep>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.UserLoanRep model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.UserLoanRep();
					if(dt.Rows[n]["NameId"].ToString()!="")
					{
						model.NameId=int.Parse(dt.Rows[n]["NameId"].ToString());
					}
					if(dt.Rows[n]["BranchId"].ToString()!="")
					{
						model.BranchId=int.Parse(dt.Rows[n]["BranchId"].ToString());
					}
					model.Name=dt.Rows[n]["Name"].ToString();
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

        /// <summary>
        /// 保存UserLoanRep关系
        /// </summary>
        /// <param name="nUserID"></param>
        /// <param name="strLoanRepIds"></param>
        public void SaveUserLoanRep(int nUserID, string strLoanRepIds)
        {
            if (!string.IsNullOrEmpty(strLoanRepIds))
                dal.SaveUserLoanRep(nUserID, strLoanRepIds);
            else
                dal.DeleteLoanRepMapping(null, nUserID);
        }

        /// <summary>
        /// 删除用户Loan Rep Mapping
        /// </summary>
        public void DeleteLoanRepMapping(int nLoanRepID, int nUserID)
        {
            dal.DeleteLoanRepMapping(nLoanRepID, nUserID);
        }

		#endregion  成员方法
	}
}

