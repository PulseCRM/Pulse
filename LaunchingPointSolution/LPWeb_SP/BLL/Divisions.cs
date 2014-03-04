using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
namespace LPWeb.BLL
{
    /// <summary>
    /// Divisions 的摘要说明。
    /// </summary>
    public class Divisions
    {
        private readonly LPWeb.DAL.Divisions dal = new LPWeb.DAL.Divisions();
        public Divisions()
        { }
        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Divisions model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Divisions model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int DivisionId)
        {

            dal.Delete(DivisionId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Divisions GetModel(int DivisionId)
        {

            return dal.GetModel(DivisionId);
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
        public List<LPWeb.Model.Divisions> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Divisions> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Divisions> modelList = new List<LPWeb.Model.Divisions>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Divisions model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Divisions();
                    if (dt.Rows[n]["DivisionId"].ToString() != "")
                    {
                        model.DivisionId = int.Parse(dt.Rows[n]["DivisionId"].ToString());
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
                    if (dt.Rows[n]["GroupID"].ToString() != "")
                    {
                        model.GroupID = int.Parse(dt.Rows[n]["GroupID"].ToString());
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

        public bool ExistDivision(int iDivisionID)
        {
            bool isExist = false;
            string strWhere = " DivisionID = " + iDivisionID.ToString();
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


        public void SaveDivisionAndMembersBase(int iDivisionID, bool bEnabled, string sDesc, int iGroupID, string sBranchMemberIDs, string sExectives)
        {
            dal.SaveDivisionAndMembersBase(iDivisionID, bEnabled, sDesc, iGroupID, sBranchMemberIDs, sExectives);
        }


        public int CreateDivision(string sDivName)
        {
            return dal.CreateDivision(sDivName);
        }


        public bool IsExist_CreateBase(string sDivName)
        {
            return dal.IsExist_CreateBase(sDivName);
        }

        /// <summary>
        /// 获取Division列表
        /// neo 2010-09-24
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetDivisionList(string sWhere)
        {
            return dal.GetDivisionListBase(sWhere);
        }

        /// <summary>
        /// get items of division filter for "All Loans" in dashboard home
        /// neo 2010-10-19
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetDivision_AllLoans(int iExecutiveID)
        {
            return dal.GetDivision_AllLoansBase(iExecutiveID);
        }

        /// <summary>
        /// get items of division filter for "All Loans" in dashboard home
        /// neo 2010-10-19
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetDivision_AllLoans(int iExecutiveID, int iRegionID)
        {
            return dal.GetDivision_AllLoansBase(iExecutiveID, iRegionID);
        }

        /// <summary>
        /// get items of division filter for "Assigned Loans" in dashboard home
        /// neo 2010-10-24
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetDivisionList_AssingedLoans(int iUserID)
        {
            return dal.GetDivisionList_AssingedLoansBase(iUserID);
        }

        /// <summary>
        /// get items of division filter for "Assigned Loans" in dashboard home
        /// neo 2010-10-24
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetDivisionList_AssingedLoans(int iUserID, int iRegionID)
        {
            return dal.GetDivisionList_AssingedLoansBase(iUserID, iRegionID);
        }

        /// <summary>
        /// get items of division filter
        /// neo 2010-11-10
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetDivisionFilter(int iUserID, int iRegionID)
        {
            return dal.GetDivisionFilterBase(iUserID, iRegionID);
        }

        public DataTable GetDivisionFilter_Branch_Manager(int iUserID, int iRegionID)
        {
            return dal.GetDivisionFilterBase_Branch_Manager(iUserID, iRegionID);
        }

        public DataTable GetDivisionFilter_Executive(int iUserID, int iRegionID)
        {
            return dal.GetDivisionFilterBase_Executive(iUserID, iRegionID);
        }

        public DataTable GetDivisionFilter(int iUserID, string sRegionIDs)
        {
            return dal.GetDivisionFilterBase(iUserID, sRegionIDs);
        }

        public DataTable GetDivisionFilter_Branch_Manager(int iUserID, string sRegionIDs)
        {
            return dal.GetDivisionFilterBase_Branch_Manager(iUserID, sRegionIDs);
        }

        public DataTable GetDivisionFilter_Executive(int iUserID, string sRegionIDs)
        {
            return dal.GetDivisionFilterBase_Executive(iUserID, sRegionIDs);
        }

        /// <summary>
        /// get items of division filter for user list view
        /// neo 2010-11-10
        /// </summary>
        /// <param name="iExecutiveID"></param>
        /// <returns></returns>
        public DataTable GetDivisionFilter_UserList(int iUserID, int iRegionID)
        {
            return dal.GetDivisionFilterBase_UserList(iUserID, iRegionID);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetDivisionGoalsReport(int PageSize, int PageIndex, string strRegion, string strDivision, out int recordCount, string orderName, int orderType)
        {
            return dal.GetDivisionGoalsReport(PageSize, PageIndex, strRegion, strDivision, out recordCount, orderName, orderType);
        }


        public DataTable GetUserDivisions(int iUserID)
        {
            return dal.GetUserDivisions(iUserID);
        }

    }
}

