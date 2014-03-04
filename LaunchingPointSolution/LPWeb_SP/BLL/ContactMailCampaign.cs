using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;

namespace LPWeb.BLL
{
	/// <summary>
	/// ContactMailCampaign
	/// </summary>
	public partial class ContactMailCampaign
    {
        private readonly LPWeb.DAL.ContactMailCampaign dal = new LPWeb.DAL.ContactMailCampaign(); 

		public ContactMailCampaign()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ContactMailCampaignId)
		{
			return dal.Exists(ContactMailCampaignId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.ContactMailCampaign model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.ContactMailCampaign model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int ContactMailCampaignId)
		{
			
			return dal.Delete(ContactMailCampaignId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string ContactMailCampaignIdlist )
		{
			return dal.DeleteList(ContactMailCampaignIdlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.ContactMailCampaign GetModel(int ContactMailCampaignId)
		{
			
			return dal.GetModel(ContactMailCampaignId);
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
		public List<LPWeb.Model.ContactMailCampaign> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.ContactMailCampaign> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.ContactMailCampaign> modelList = new List<LPWeb.Model.ContactMailCampaign>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.ContactMailCampaign model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.ContactMailCampaign();
					if(dt.Rows[n]["ContactMailCampaignId"]!=null && dt.Rows[n]["ContactMailCampaignId"].ToString()!="")
					{
						model.ContactMailCampaignId=int.Parse(dt.Rows[n]["ContactMailCampaignId"].ToString());
					}
					if(dt.Rows[n]["ContactId"]!=null && dt.Rows[n]["ContactId"].ToString()!="")
					{
						model.ContactId=int.Parse(dt.Rows[n]["ContactId"].ToString());
					}
					if(dt.Rows[n]["CID"]!=null && dt.Rows[n]["CID"].ToString()!="")
					{
					model.CID=dt.Rows[n]["CID"].ToString();
					}
					if(dt.Rows[n]["LID"]!=null && dt.Rows[n]["LID"].ToString()!="")
					{
					model.LID=dt.Rows[n]["LID"].ToString();
					}
					if(dt.Rows[n]["BranchId"]!=null && dt.Rows[n]["BranchId"].ToString()!="")
					{
						model.BranchId=int.Parse(dt.Rows[n]["BranchId"].ToString());
					}
					if(dt.Rows[n]["Added"]!=null && dt.Rows[n]["Added"].ToString()!="")
					{
						model.Added=DateTime.Parse(dt.Rows[n]["Added"].ToString());
					}
					if(dt.Rows[n]["AddedBy"]!=null && dt.Rows[n]["AddedBy"].ToString()!="")
					{
						model.AddedBy=int.Parse(dt.Rows[n]["AddedBy"].ToString());
					}
					if(dt.Rows[n]["Result"]!=null && dt.Rows[n]["Result"].ToString()!="")
					{
					model.Result=dt.Rows[n]["Result"].ToString();
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

