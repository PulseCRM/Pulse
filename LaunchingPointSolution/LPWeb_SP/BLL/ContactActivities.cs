using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// 业务逻辑类ContactActivities 的摘要说明。
	/// </summary>
	public class ContactActivities
	{
		private readonly LPWeb.DAL.ContactActivities dal=new LPWeb.DAL.ContactActivities();
		public ContactActivities()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.ContactActivities model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.ContactActivities model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ContactActivityId)
		{
			
			dal.Delete(ContactActivityId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.ContactActivities GetModel(int ContactActivityId)
		{
			
			return dal.GetModel(ContactActivityId);
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
		public List<LPWeb.Model.ContactActivities> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.ContactActivities> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.ContactActivities> modelList = new List<LPWeb.Model.ContactActivities>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.ContactActivities model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.ContactActivities();
					if(dt.Rows[n]["ContactActivityId"].ToString()!="")
					{
						model.ContactActivityId=int.Parse(dt.Rows[n]["ContactActivityId"].ToString());
					}
					if(dt.Rows[n]["ContactId"].ToString()!="")
					{
						model.ContactId=int.Parse(dt.Rows[n]["ContactId"].ToString());
					}
					if(dt.Rows[n]["UserId"].ToString()!="")
					{
						model.UserId=int.Parse(dt.Rows[n]["UserId"].ToString());
					}
					model.ActivityName=dt.Rows[n]["ActivityName"].ToString();
					if(dt.Rows[n]["ActivityTime"].ToString()!="")
					{
						model.ActivityTime=DateTime.Parse(dt.Rows[n]["ActivityTime"].ToString());
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


        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetListForGridView(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }
        public DataTable GetProformedBy(string strWhere)
        {
            return dal.GetProformedBy(strWhere);
        }
	}
}

