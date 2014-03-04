using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
using LPWeb.DAL;
using System.Data.SqlClient;
namespace LPWeb.BLL
{
    /// <summary>
    /// Groups 的摘要说明。
    /// </summary>
    public class Groups
    {
        private readonly LPWeb.DAL.Groups dal = new LPWeb.DAL.Groups();
        public Groups()
        { }
        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Groups model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Groups model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int GroupId)
        {

            dal.Delete(GroupId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Groups GetModel(int GroupId)
        {

            return dal.GetModel(GroupId);
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
        public List<LPWeb.Model.Groups> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LPWeb.Model.Groups> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Groups> modelList = new List<LPWeb.Model.Groups>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Groups model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Groups();
                    if (dt.Rows[n]["GroupId"].ToString() != "")
                    {
                        model.GroupId = int.Parse(dt.Rows[n]["GroupId"].ToString());
                    }
                    model.GroupName = dt.Rows[n]["GroupName"].ToString();
                    model.OrganizationType = dt.Rows[n]["OrganizationType"].ToString();
                    if (dt.Rows[n]["OrganizationId"].ToString() != "")
                    {
                        model.OrganizationId = int.Parse(dt.Rows[n]["OrganizationId"].ToString());
                    }
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

        #region 刘洋添加的方法

        /// <summary>
        /// 获取Group列表
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetGroupList(string sWhere)
        {
            return dal.GetGroupListBase(sWhere);
        }

        /// <summary>
        /// 获取Group信息
        /// </summary>
        /// <param name="iGroupID"></param>
        /// <returns></returns>
        public DataTable GetGroupInfo(int iGroupID)
        {
            return dal.GetGroupInfoBase(iGroupID);
        }

        /// <summary>
        /// 保存Group和Members信息
        /// </summary>
        /// <param name="iGroupID"></param>
        /// <param name="bEnabled"></param>
        /// <param name="sGroupDesc"></param>
        /// <param name="sOldGroupMemberIDs"></param>
        /// <param name="sGroupMemberIDs"></param>
        public void SaveGroupAndMembers(int iGroupID, bool bEnabled, string sGroupDesc, string sOldGroupMemberIDs, string sGroupMemberIDs)
        {
            dal.SaveGroupAndMembersBase(iGroupID, bEnabled, sGroupDesc, sOldGroupMemberIDs, sGroupMemberIDs);

        }

        /// <summary>
        /// 检查Group Name是否存在
        /// </summary>
        /// <param name="sGroupName"></param>
        /// <returns></returns>
        public bool IsExist_Create(string sGroupName)
        {
            return dal.IsExist_CreateBase(sGroupName);
        }

        /// <summary>
        /// 创建Group
        /// </summary>
        /// <param name="sGroupName"></param>
        /// <returns></returns>
        public int CreateGroup(string sGroupName)
        {
            return dal.CreateGroupBase(sGroupName);
        }

        /// <summary>
        /// 获取Group Member列表
        /// neo 2010-09-04
        /// </summary>
        /// <param name="iGroupID"></param>
        /// <returns></returns>
        public DataTable GetGroupMemberList(int iGroupID)
        {
            return dal.GetGroupMemberListBase(iGroupID);
        }

        /// <summary>
        /// 获取Group Member选择列表
        /// neo 2010-09-04
        /// </summary>
        /// <param name="iGroupID"></param>
        /// <returns></returns>
        public DataTable GetGroupMemberSelectionList()
        {
            return dal.GetGroupMemberSelectionListBase();
        }

        #endregion


        public DataSet GetGroupsByDivisionID(int Divisionid)
        {
            return dal.GetGroupsByDivisionID(Divisionid);
        }

        public DataSet GetGroupsByBranchID(int BranchID)
        {
            return dal.GetGroupsByBranchID(BranchID);
        }

        /// <summary>
        /// Query Groups by region id
        /// </summary>
        /// <param name="regionId">region id</param>
        /// <returns></returns>
        public DataSet GetGroupsByRegionID(int regionId)
        {
            return dal.GetGroupsByRegionID(regionId);
        }

        /// <summary>
        /// Gets the company rel groups.
        /// </summary>
        /// <returns></returns>
        public DataSet GetCompanyRelGroups()
        {
            return dal.GetCompanyRelGroups();
        }

        /// <summary>
        /// Updates the group access.
        /// </summary>
        /// <param name="prevId">The prev id.</param>
        /// <param name="newId">The new id.</param>
        /// <param name="companyId">The company id.</param>
        /// <param name="orgType">Type of the org.</param>
        public bool UpdateGroupAccess(int prevId, int newId, int companyId, string orgType)
        {
            return dal.UpdateGroupAccess(prevId, newId, companyId, orgType);
        }

        /// <summary>
        /// get group object of Company
        /// </summary>
        /// <returns></returns>
        public Model.Groups GetCompanyGroup()
        {
            return dal.GetCompanyGroup();
        }
    }
}

