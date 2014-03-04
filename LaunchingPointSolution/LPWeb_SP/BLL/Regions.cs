using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
	/// <summary>
	/// Regions 的摘要说明。
	/// </summary>
	public class Regions
	{
		private readonly LPWeb.DAL.Regions dal=new LPWeb.DAL.Regions();
		public Regions()
		{}
		#region  成员方法
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LPWeb.Model.Regions model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.Regions model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int RegionId)
		{
			
			dal.Delete(RegionId);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.Regions GetModel(int RegionId)
		{
			
			return dal.GetModel(RegionId);
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
		public List<LPWeb.Model.Regions> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LPWeb.Model.Regions> DataTableToList(DataTable dt)
		{
			List<LPWeb.Model.Regions> modelList = new List<LPWeb.Model.Regions>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				LPWeb.Model.Regions model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new LPWeb.Model.Regions();
					if(dt.Rows[n]["RegionId"].ToString()!="")
					{
						model.RegionId=int.Parse(dt.Rows[n]["RegionId"].ToString());
					}
					model.Name=dt.Rows[n]["Name"].ToString();
					model.Desc=dt.Rows[n]["Desc"].ToString();
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

        public bool ExistDivision(int iRegionId)
        {
            bool isExist = false;
            string strWhere = " RegionID = " + iRegionId.ToString();
            DataSet ds = dal.GetList(strWhere);
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                isExist = false;
            }
            else
            {
                isExist = true;
            }

            return isExist;
        }


        public void SaveRegionAndMembersBase(int iRegionID, bool bEnabled, string sDesc,int groupId, string sDivisionMemberIDs, string sExectives)
        {
            dal.SaveRegionAndMembersBase(iRegionID, bEnabled, sDesc,groupId, sDivisionMemberIDs, sExectives);
        }


        public int CreateRegion(string sDivName)
        {
            return dal.CreateRegion(sDivName);
        }


        public bool IsExist_CreateBase(string sDivName)
        {
            return dal.IsExist_CreateBase(sDivName);
        }

        /// <summary>
        /// 获取Region列表
        /// neo 2010-09-24
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetRegionList(string sWhere)
        {
            return dal.GetRegionListBase(sWhere);
        }

        /// <summary>
        /// get items of region filter for "All Loans" in dashboard home
        /// neo 2010-10-14
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetRegionList_AllLoans(int iExecutiveID)
        {
            return dal.GetRegionList_AllLoansBase(iExecutiveID);
        }

        /// <summary>
        /// get items of region filter for "Assigned Loans" in dashboard home
        /// neo 2010-10-24
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetRegionList_AssingedLoans(int iUserID)
        {
            return dal.GetRegionList_AssingedLoansBase(iUserID);
        }

        /// <summary>
        /// get items of region filter
        /// neo 2010-11-10
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetRegionFilter(int iUserID)
        {
            return dal.GetRegionFilterBase(iUserID);
        }

        public DataTable GetRegionFilter_Branch_Manager(int iUserID)
        {
            return dal.GetRegionFilterBase_Branch_Manager(iUserID);
        }

        public DataTable GetRegionFilter_Executive(int iUserID)
        {
            return dal.GetRegionFilterBase_Executive(iUserID);
        }

        /// <summary>
        /// get items of region filter for user list view
        /// neo 2010-11-10
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetRegionFilter_UserList(int iUserID)
        {
            return dal.GetRegionFilterBase_UserList(iUserID);
        }

        
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetRegionGoalsReport(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetRegionGoalsReport(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        
        }


        public DataTable GetUserRegions(int iUserID)
        {
            return dal.GetUserRegions(iUserID);
        }
	}
}

