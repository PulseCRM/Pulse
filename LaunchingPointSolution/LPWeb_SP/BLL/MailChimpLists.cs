using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;

namespace LPWeb.BLL
{
	/// <summary>
	/// MailChimpLists
	/// </summary>
	public partial class MailChimpLists
    {
        private readonly LPWeb.DAL.MailChimpLists dal = new LPWeb.DAL.MailChimpLists();

		public MailChimpLists()
		{}
		#region  Method
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string LID)
		{
			return dal.Exists(LID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.MailChimpLists model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.MailChimpLists model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string LID)
		{
			
			return dal.Delete(LID);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string LIDlist )
		{
			return dal.DeleteList(LIDlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.MailChimpLists GetModel(string LID)
		{
			
			return dal.GetModel(LID);
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
		public List<LPWeb.Model.MailChimpLists> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.MailChimpLists> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.MailChimpLists> modelList = new List<LPWeb.Model.MailChimpLists>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.MailChimpLists model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.MailChimpLists();
					if(dt.Rows[n]["LID"]!=null && dt.Rows[n]["LID"].ToString()!="")
					{
					model.LID=dt.Rows[n]["LID"].ToString();
					}
					if(dt.Rows[n]["Name"]!=null && dt.Rows[n]["Name"].ToString()!="")
					{
					model.Name=dt.Rows[n]["Name"].ToString();
					}
					if(dt.Rows[n]["BranchId"]!=null && dt.Rows[n]["BranchId"].ToString()!="")
					{
					model.BranchId=dt.Rows[n]["BranchId"].ToString();
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
		/// 分页获取数据列表
		/// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere)
        {
            return dal.GetList(PageSize, PageIndex, strWhere);
        }

		#endregion  Method

        
        public DataSet GetMailChimpLists(int PageSize, int PageIndex, int UserID, string strWhere, out int count, string orderName, int orderType)
        {
            return dal.GetMailChimpLists(PageSize, PageIndex, UserID, strWhere, out count, orderName, orderType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrderBy"></param>
        /// <returns></returns>
        public DataTable GetMailChimpList(string sWhere, string sOrderBy)
        {
            return this.dal.GetMailChimpList(sWhere, sOrderBy);
        }

        public DataTable GetMailChimpList_BranchManager(int userid)
        {
            return this.dal.GetMailChimpList_BranchManager(userid);
        }
	}
}

