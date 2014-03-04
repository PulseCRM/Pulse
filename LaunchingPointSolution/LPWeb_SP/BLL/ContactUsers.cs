using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// 业务逻辑类ContactUsers 的摘要说明。
	/// </summary>
	public class ContactUsers
	{
		private readonly LPWeb.DAL.ContactUsers dal=new LPWeb.DAL.ContactUsers();
		public ContactUsers()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.ContactUsers model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.ContactUsers model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int UserId,int ContactId)
		{
			
			dal.Delete(UserId,ContactId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.ContactUsers GetModel(int UserId,int ContactId)
		{
			
			return dal.GetModel(UserId,ContactId);
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
		public List<LPWeb.Model.ContactUsers> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.ContactUsers> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.ContactUsers> modelList = new List<LPWeb.Model.ContactUsers>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.ContactUsers model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.ContactUsers();
					if(dt.Rows[n]["UserId"].ToString()!="")
					{
						model.UserId=int.Parse(dt.Rows[n]["UserId"].ToString());
					}
					if(dt.Rows[n]["ContactId"].ToString()!="")
					{
						model.ContactId=int.Parse(dt.Rows[n]["ContactId"].ToString());
					}
					if(dt.Rows[n]["Enabled"].ToString()!="")
					{
						if((dt.Rows[n]["Enabled"].ToString()=="1")||(dt.Rows[n]["Enabled"].ToString().ToLower()=="true"))
						{
						model.Enabled=true;
						}
						else
						{
							model.Enabled=false;
						}
					}
					if(dt.Rows[n]["Created"].ToString()!="")
					{
						model.Created=DateTime.Parse(dt.Rows[n]["Created"].ToString());
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

        public int GetUserContactCount(int nUserId)
        {
            return dal.GetUserContactCount(nUserId);
        }
	}
}

