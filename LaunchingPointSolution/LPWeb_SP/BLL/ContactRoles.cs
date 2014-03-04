using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// ContactRoles 的摘要说明。
	/// </summary>
	public class ContactRoles
	{
		private readonly LPWeb.DAL.ContactRoles dal=new LPWeb.DAL.ContactRoles();
		public ContactRoles()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.ContactRoles model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.ContactRoles model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ContactRoleId)
		{
			
			dal.Delete(ContactRoleId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.ContactRoles GetModel(int ContactRoleId)
		{
			
			return dal.GetModel(ContactRoleId);
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
		public List<LPWeb.Model.ContactRoles> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.ContactRoles> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.ContactRoles> modelList = new List<LPWeb.Model.ContactRoles>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.ContactRoles model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.ContactRoles();
					if(dt.Rows[n]["ContactRoleId"].ToString()!="")
					{
						model.ContactRoleId=int.Parse(dt.Rows[n]["ContactRoleId"].ToString());
					}
					model.Name=dt.Rows[n]["Name"].ToString();
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

        /// <summary>
        /// get contact role list
        /// neo 2010-12-09
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetContactRoleList(string sWhere)
        {
            return dal.GetContactRoleListBase(sWhere);
        }

        /// <summary>
        /// get data for email template list
        /// </summary>
        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetListForGridView(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        public bool IsContactRoleNameExsits(string strName)
        {
            return dal.IsContactRoleNameExsits(strName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strIds"></param>
        public void DeleteContactRole(string strIds)
        {
            List<int> listAllIds = GetAllIDs(strIds);
            if (listAllIds.Count > 0)
                dal.DeleteContactRole(listAllIds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strIds"></param>
        public void EnableContactRole(string strIds)
        {
            List<int> listAllIds = GetAllIDs(strIds);
            if (listAllIds.Count > 0)
                dal.EnableContactRole(listAllIds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strIds"></param>
        public void DisableContactRole(string strIds)
        {
            List<int> listAllIds = GetAllIDs(strIds);
            if (listAllIds.Count > 0)
                dal.DisableContactRole(listAllIds);
        }

        private List<int> GetAllIDs(string strAllIds)
        {
            List<int> listIds = new List<int>();
            foreach (string str in strAllIds.Split(new char[] { ',' }))
            {
                int nTemp = -1;
                if (!int.TryParse(str, out nTemp))
                    nTemp = -1;
                if (nTemp != -1)
                    listIds.Add(nTemp);
            }
            return listIds;
        }
	}
}

