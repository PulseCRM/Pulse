using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;

namespace LPWeb.BLL
{
	/// <summary>
	/// MailChimpCampaigns
	/// </summary>
	public partial class MailChimpCampaigns
    {
        private readonly LPWeb.DAL.MailChimpCampaigns dal = new LPWeb.DAL.MailChimpCampaigns();

		public MailChimpCampaigns()
		{}
		#region  Method
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string CID)
		{
			return dal.Exists(CID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.MailChimpCampaigns model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.MailChimpCampaigns model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string CID)
		{
			
			return dal.Delete(CID);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string CIDlist )
		{
			return dal.DeleteList(CIDlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.MailChimpCampaigns GetModel(string CID)
		{
			
			return dal.GetModel(CID);
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
		public List<LPWeb.Model.MailChimpCampaigns> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.MailChimpCampaigns> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.MailChimpCampaigns> modelList = new List<LPWeb.Model.MailChimpCampaigns>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.MailChimpCampaigns model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.MailChimpCampaigns();
					if(dt.Rows[n]["CID"]!=null && dt.Rows[n]["CID"].ToString()!="")
					{
					model.CID=dt.Rows[n]["CID"].ToString();
					}
					if(dt.Rows[n]["Name"]!=null && dt.Rows[n]["Name"].ToString()!="")
					{
					model.Name=dt.Rows[n]["Name"].ToString();
					}
					if(dt.Rows[n]["BranchId"]!=null && dt.Rows[n]["BranchId"].ToString()!="")
					{
						model.BranchId=int.Parse(dt.Rows[n]["BranchId"].ToString());
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
	}
}

