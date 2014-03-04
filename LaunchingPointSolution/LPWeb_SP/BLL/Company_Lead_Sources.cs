using System;
using System.Data;
using System.Collections.Generic;
using LPWeb.Model;

namespace LPWeb.BLL
{
    /// <summary>
	/// Company_Lead_Sources 的摘要说明。
	/// </summary>
	public class Company_Lead_Sources
    {
        private readonly LPWeb.DAL.Company_Lead_Sources dal=new LPWeb.DAL.Company_Lead_Sources();
		public Company_Lead_Sources()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.Company_Lead_Sources model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.Company_Lead_Sources model)
		{
			dal.Update(model);
		}

        public void UpdateDefault(bool isDefault)
        {
            if (isDefault)
            {
                dal.UpdateDefault();
            }
        }

        /// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int LeadSourceID)
		{
			
			dal.Delete(LeadSourceID);
		}

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="leadSourceID"></param>
        public void Delete(string leadSourceID)
        {
            string[] leadSources = leadSourceID.Split(',');
            foreach (var leadSource in leadSources)
            {
                dal.Delete(int.Parse(leadSource));
            }
        }

        /// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.Company_Lead_Sources GetModel(int LeadSourceID)
		{
			
			return dal.GetModel(LeadSourceID);
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
		public List<LPWeb.Model.Company_Lead_Sources> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.Company_Lead_Sources> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.Company_Lead_Sources> modelList = new List<LPWeb.Model.Company_Lead_Sources>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.Company_Lead_Sources model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.Company_Lead_Sources();
					if(dt.Rows[n]["LeadSourceID"].ToString().Length > 0)
					{
						model.LeadSourceID=int.Parse(dt.Rows[n]["LeadSourceID"].ToString());
					}
					model.LeadSource=dt.Rows[n]["LeadSource"].ToString();

				    if (dt.Rows[n]["DefaultUser"] != DBNull.Value)
				    {
                        model.DefaultUser = dt.Rows[n]["DefaultUser"].ToString();
				    }
				    else
				    {
                        model.DefaultUser = "";
                    }
				   
				    if (dt.Rows[n]["DefaultUserId"] != DBNull.Value || dt.Rows[n]["DefaultUserId"].ToString().Length > 0)
				    {
                        model.DefaultUserId = int.Parse(dt.Rows[n]["DefaultUserId"].ToString()); 
				    }
                    //else
                    //{
                    //    model.DefaultUserId = -1;
                    //}

                    model.DefaultString = dt.Rows[n]["DefaultString"].ToString();

                    if (dt.Rows[n]["Default"] != DBNull.Value && dt.Rows[n]["Default"].ToString().Length > 0)
                    {
                        model.Default = bool.Parse(dt.Rows[n]["Default"].ToString());
                    }
                    //else
                    //{
                    //    model.Default = false;
                    //}
				    
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
    }
}
