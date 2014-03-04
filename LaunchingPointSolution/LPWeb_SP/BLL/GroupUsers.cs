using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// GroupUsers 的摘要说明。
	/// </summary>
	public class GroupUsers
	{
		private readonly LPWeb.DAL.GroupUsers dal=new LPWeb.DAL.GroupUsers();
		public GroupUsers()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.GroupUsers model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.GroupUsers model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int GroupId,int UserId)
		{
			
			dal.Delete(GroupId,UserId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.GroupUsers GetModel(int GroupId,int UserId)
		{
			
			return dal.GetModel(GroupId,UserId);
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
		public List<LPWeb.Model.GroupUsers> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.GroupUsers> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.GroupUsers> modelList = new List<LPWeb.Model.GroupUsers>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.GroupUsers model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.GroupUsers();
					if(dt.Rows[n]["GroupId"].ToString()!="")
					{
						model.GroupId=int.Parse(dt.Rows[n]["GroupId"].ToString());
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

        public DataSet GetGroupUsersForUserSetup(string strWhere)
        {
            return dal.GetGroupUsersForUserSetup(strWhere);
        }

        /// <summary>
        /// 保存GroupUser关系
        /// </summary>
        /// <param name="nUserId"></param>
        /// <param name="strSelectedIds"></param>
        public void SaveGroupUser(int nUserId, string strSelectedIds)
        {
            dal.SaveGroupUser(nUserId, strSelectedIds);
        }

		#endregion  成员方法
	}
}

