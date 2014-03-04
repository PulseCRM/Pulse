using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// Branches 的摘要说明。
    /// </summary>
    public class Branches
    {
        private readonly LPWeb.DAL.Branches dal = new LPWeb.DAL.Branches();
        public Branches()
        { }
        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Branches model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Branches model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int BranchId)
        {

            dal.Delete(BranchId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Branches GetModel(int BranchId)
        {

            return dal.GetModel(BranchId);
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
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Branches> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Branches> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Branches> modelList = new List<LPWeb.Model.Branches>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Branches model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Branches();
                    if (dt.Rows[n]["BranchId"].ToString() != "")
                    {
                        model.BranchId = int.Parse(dt.Rows[n]["BranchId"].ToString());
                    }
                    model.Name = dt.Rows[n]["Name"].ToString();
                    model.Desc = dt.Rows[n]["Desc"].ToString();
                    if (dt.Rows[n]["Enabled"].ToString() != "")
                    {
                        if ((dt.Rows[n]["Enabled"].ToString() == "1") || (dt.Rows[n]["Enabled"].ToString().ToLower() == "true"))
                        {
                            model.Enabled = true;
                        }
                        else
                        {
                            model.Enabled = false;
                        }
                    }
                    if (dt.Rows[n]["RegionID"].ToString() != "")
                    {
                        model.RegionID = int.Parse(dt.Rows[n]["RegionID"].ToString());
                    }
                    if (dt.Rows[n]["DivisionID"].ToString() != "")
                    {
                        model.DivisionID = int.Parse(dt.Rows[n]["DivisionID"].ToString());
                    }
                    if (dt.Rows[n]["GroupID"].ToString() != "")
                    {
                        model.GroupID = int.Parse(dt.Rows[n]["GroupID"].ToString());
                    }
                    model.BranchAddress = dt.Rows[n]["BranchAddress"].ToString();
                    model.City = dt.Rows[n]["City"].ToString();
                    model.BranchState = dt.Rows[n]["BranchState"].ToString();
                    model.Zip = dt.Rows[n]["Zip"].ToString();
                    if (dt.Rows[n]["WebsiteLogo"].ToString() != "")
                    {
                        model.WebsiteLogo = (byte[])dt.Rows[n]["WebsiteLogo"];
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
        public DataSet GetList(int PageSize, int PageIndex, string strWhere)
        {
            return dal.GetList(PageSize, PageIndex, strWhere);
        }

        #endregion  成员方法

        public bool ExistBranch(int iBranchID)
        {
            bool isExist = false;
            string strWhere = " BranchId = " + iBranchID.ToString();
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

        public bool IsExist_CreateBase(string sBranchName)
        {
            return dal.IsExist_CreateBase(sBranchName);
        }
                
        /// <summary>
        /// Create Division
        /// </summary>
        /// <param name="sGroupName"></param>
        /// <returns></returns>
        public int CreateBranch(string sBranchName)
        {
            return dal.CreateBranch(sBranchName);
        }
        /// <summary>
        /// 获取具有branc manager 权限的 User列表
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetBranchManagerSeletion()
        {
            return dal.GetBranchManagerSeletion();
        }

        /// <summary>
        /// 保存Branch和Members信息
        /// </summary>
        /// <param name="iGroupID"></param>
        /// <param name="bEnabled"></param>
        /// <param name="sGroupDesc"></param>
        /// <param name="sOldGroupMemberIDs"></param>
        /// <param name="sGroupMemberIDs"></param>
        public void SaveBranchAndMembersBase(LPWeb.Model.Branches model, string sFolderIDs, string sManagers)
        {
            dal.SaveBranchAndMembersBase(model, sFolderIDs, sManagers);
        }

        /// <summary>
        /// 获取Branch列表
        /// neo 2010-09-24
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetBranchList(string sWhere)
        {
            return dal.GetBranchListBase(sWhere);
        }

        /// <summary>
        /// get items of branch filter for "All Loans" in dashboard home
        /// neo 2010-10-24
        /// </summary>
        /// <param name="iManagerID"></param>
        /// <param name="iRegionID"></param>
        /// <param name="iDivisionID"></param>
        /// <returns></returns>
        public DataTable GetBranchList_AllLoans(int iManagerID, int iRegionID, int iDivisionID)
        {
            return dal.GetBranchList_AllLoansBase(iManagerID, iRegionID, iDivisionID);
        }

        //public DataTable GetBranchList_AllLoans_Branch_Manager(int iManagerID, int iRegionID, int iDivisionID)
        //{
        //    return dal.GetBranchList_AllLoansBase_Branch_Manager(iManagerID, iRegionID, iDivisionID);
        //}

        /// <summary>
        /// get items of branch filter for "Assigned Loans" in dashboard home
        /// neo 2010-10-24
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="iRegionID"></param>
        /// <param name="iDivisionID"></param>
        /// <returns></returns>
        public DataTable GetBranchList_AssingedLoans(int iUserID, int iRegionID, int iDivisionID)
        {
            return dal.GetBranchList_AssingedLoansBase(iUserID, iRegionID, iDivisionID);
        }

        /// <summary>
        /// get items of branch filter
        /// neo 2010-11-10
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="iRegionID"></param>
        /// <param name="iDivisionID"></param>
        /// <returns></returns>
        public DataTable GetBranchFilter(int iUserID, int iRegionID, int iDivisionID)
        {
            return dal.GetBranchFilterBase(iUserID, iRegionID, iDivisionID);
        }

        public DataTable GetBranchFilter_Branch_Manager(int iUserID, int iRegionID, int iDivisionID)
        {
            return dal.GetBranchFilterBase_Branch_Manager(iUserID, iRegionID, iDivisionID);
        }

        public DataTable GetBranchFilter_Executive(int iUserID, int iRegionID, int iDivisionID)
        {
            return dal.GetBranchFilterBase_Executive(iUserID, iRegionID, iDivisionID);
        }

        public DataTable GetBranchFilter(int iUserID, string sRegionIDs, string sDivisionIDs)
        {
            return dal.GetBranchFilterBase(iUserID, sRegionIDs, sDivisionIDs);
        }

        public DataTable GetBranchFilter_Branch_Manager(int iUserID, string sRegionIDs, string sDivisionIDs)
        {
            return dal.GetBranchFilterBase_Branch_Manager(iUserID, sRegionIDs, sDivisionIDs);
        }

        public DataTable GetBranchFilter_Executive(int iUserID, string sRegionIDs, string sDivisionIDs)
        {
            return dal.GetBranchFilterBase_Executive(iUserID, sRegionIDs, sDivisionIDs);
        }

        /// <summary>
        /// get items of branch filter for user list view
        /// neo 2010-11-10
        /// </summary>
        /// <param name="iUserID"></param>
        /// <param name="iRegionID"></param>
        /// <param name="iDivisionID"></param>
        /// <returns></returns>
        public DataTable GetBranchFilter_UserList(int iUserID, int iRegionID, int iDivisionID)
        {
            return dal.GetBranchFilterBase_UserList(iUserID, iRegionID, iDivisionID);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetBranchGoalsReport(int PageSize, int PageIndex, string strRegion, string strDivision, string strBranch, 
            out int recordCount, string orderName, int orderType)
        {
            return dal.GetBranchGoalsReport(PageSize, PageIndex, strRegion, strDivision, strBranch, out recordCount, orderName, orderType);
        }


        /// <summary>
        /// 设置其他Branch的HomeBranch 为false
        /// </summary>
        /// <param name="BranchId">不包含的ID</param>
        public void SetOtherHomeBranchFalse(int BranchId)
        {
            dal.SetOtherHomeBranchFalse(BranchId);
        }


        /// <summary>
        /// UpdateChimpAPIKey
        /// </summary>
        public void UpdateChimpAPIKey(LPWeb.Model.Branches model)
        {
            dal.UpdateChimpAPIKey(model);
        }

        public DataTable GetUserBranches(int iUserID)
        {
            return dal.GetUserBranches(iUserID);
        }
    }
}

