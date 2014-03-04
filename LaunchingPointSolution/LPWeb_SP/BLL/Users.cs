using System;
using System.Data;
using System.Collections.Generic;

using LPWeb.Model;
using System.Text;
using LPWeb.DAL;
namespace LPWeb.BLL
{
    /// <summary>
    /// ҵ���߼���Users ��ժҪ˵����
    /// </summary>
    public class Users
    {
        private readonly LPWeb.DAL.Users dal = new LPWeb.DAL.Users();
        private readonly LPWeb.DAL.UserHomePref userHomePref = new LPWeb.DAL.UserHomePref();
        private readonly LPWeb.DAL.UserPipelineColumns userPipelineCols = new LPWeb.DAL.UserPipelineColumns();
        private readonly LPWeb.DAL.UserLoanRep userLoanRep = new LPWeb.DAL.UserLoanRep();
        private readonly LPWeb.DAL.GroupUsers groupUsers = new LPWeb.DAL.GroupUsers();
        private readonly LPWeb.DAL.Groups groups = new LPWeb.DAL.Groups();
        private const int ROLEID_EXECUTIVE = 1; // role id of Executive

        public Users()
        { }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetListForGridView(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        public DataSet GetListForUserReassign(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetListForUserReassign(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        public DataSet GetUserList(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetUserList(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }
        #region  ��Ա����
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(LPWeb.Model.Users model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(LPWeb.Model.Users model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int UserId)
        {
            dal.Delete(UserId);

            //// delete all personalization info, Loan Rep Mapping and Group info
            //userHomePref.Delete(UserId);
            //userPipelineCols.Delete(UserId);
            //userLoanRep.DeleteLoanRepMapping(null, UserId);
            //groupUsers.Delete(null, UserId);

        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.Users GetModel(int UserId)
        {
            return dal.GetModel(UserId);
        }

        public LPWeb.Model.Users GetModel_WithoutPicture(int UserId)
        {
            return dal.GetModel_WithoutPicture(UserId);
        }

        public int GetLoansPerPage(int UserId)
        {

            return dal.GetLoansPerPage(UserId);
        }

        /// <summary>
        /// �û����Ƿ��Ѿ�����
        /// </summary>
        /// <param name="nID"></param>
        /// <param name="strUserName"></param>
        /// <returns></returns>
        public bool IsUserNameExists(int nID, string strUserName)
        {
            return dal.IsUserNameExists(nID, strUserName);
        }

        /// <summary>
        /// �û��ʼ���ַ�Ƿ��Ѿ�����
        /// </summary>
        /// <param name="nID"></param>
        /// <param name="strUserName"></param>
        /// <returns></returns>
        public bool IsUserEmailExists(int nID, string strUserEmail)
        {
            return dal.IsUserEmailExists(nID, strUserEmail);
        }

        /// <summary>
        /// ����ָ����UserΪDisable
        /// </summary>
        /// <param name="userIDs"></param>
        public void SetUsersDisable(List<int> userIDs)
        {
            StringBuilder sbUserIds = new StringBuilder();

            foreach (int n in userIDs)
            {
                if (sbUserIds.Length > 0)
                    sbUserIds.Append(",");
                sbUserIds.Append(string.Format("'{0}'", n));
            }
            if (sbUserIds.Length > 0)
                dal.SetUsersDisable(string.Format("UserId IN ({0})", sbUserIds.ToString()));
        }

        /// <summary>
        /// ɾ��ָ��ID���û�
        /// </summary>
        /// <param name="userIDs"></param>
        public void DeleteUsers(List<int> userIDs, int nCurrUserId, int nReassignUserId)
        {
            StringBuilder sbUserIds = new StringBuilder();

            foreach (int n in userIDs)
            {
                try
                {
                    dal.DeleteUserInfo(n, null, null, nCurrUserId, nReassignUserId);
                }
                catch
                {
                    continue;
                }
                //if (sbUserIds.Length > 0)
                //    sbUserIds.Append(",");
                //sbUserIds.Append(string.Format("'{0}'", n));
            }
            //if (sbUserIds.Length > 0)
            //    dal.DeleteUsers(string.Format("UserId IN ({0})", sbUserIds.ToString()));
        }


        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// ���ǰ��������
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<LPWeb.Model.Users> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<LPWeb.Model.Users> DataTableToList(DataTable dt)
        {
            List<LPWeb.Model.Users> modelList = new List<LPWeb.Model.Users>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LPWeb.Model.Users model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LPWeb.Model.Users();
                    if (dt.Rows[n]["UserId"].ToString() != "")
                    {
                        model.UserId = int.Parse(dt.Rows[n]["UserId"].ToString());
                    }
                    if (dt.Rows[n]["UserEnabled"].ToString() != "")
                    {
                        if ((dt.Rows[n]["UserEnabled"].ToString() == "1") || (dt.Rows[n]["UserEnabled"].ToString().ToLower() == "true"))
                        {
                            model.UserEnabled = true;
                        }
                        else
                        {
                            model.UserEnabled = false;
                        }
                    }
                    //model.Prefix=dt.Rows[n]["Prefix"].ToString();
                    model.Username = dt.Rows[n]["Username"].ToString();
                    model.EmailAddress = dt.Rows[n]["EmailAddress"].ToString();
                    //model.UserPictureFile = (byte[])dt.Rows[n]["UserPictureFile"];
                    model.UserPictureFile = DBNull.Value == dt.Rows[n]["UserPictureFile"] ? null : (byte[])dt.Rows[n]["UserPictureFile"];
                    model.FirstName = dt.Rows[n]["FirstName"].ToString();
                    model.LastName = dt.Rows[n]["LastName"].ToString();
                    if (dt.Rows[n]["RoleId"].ToString() != "")
                    {
                        model.RoleId = int.Parse(dt.Rows[n]["RoleId"].ToString());
                    }
                    model.Password = dt.Rows[n]["Password"].ToString();
                    if (dt.Rows[n]["LoansPerPage"].ToString() != "")
                    {
                        model.LoansPerPage = int.Parse(dt.Rows[n]["LoansPerPage"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere)
        {
            return dal.GetList(PageSize, PageIndex, strWhere);
        }

        #endregion  ��Ա����

        /// <summary>
        /// delete all user related info
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="nLoanRepID"></param>
        /// <param name="GroupId"></param>
        /// <param name="nCurrUserId"></param>
        /// <param name="nReassignUserId"></param>
        public void DeleteUserInfo(int UserId, int? nLoanRepID, int? GroupId, int nCurrUserId, int nReassignUserId)
        {
            dal.DeleteUserInfo(UserId, nLoanRepID, GroupId, nCurrUserId, nReassignUserId);
        }

        /// <summary>
        /// Add User Info, clone a user when the parameter nSourceID is not null
        /// by Peter
        /// </summary>
        /// <param name="user"></param>
        /// <param name="strLoanRepIds"></param>
        /// <param name="strSelectedIds"></param>
        /// <param name="nSourceID"></param>
        public int AddUserInfo(Model.Users user, string strLoanRepIds, string strGroupIds, int? nSourceID)
        {
            int? nComGroupID = null;
            Model.Groups groupCompany = groups.GetCompanyGroup();
            if (null != groupCompany)
                nComGroupID = groupCompany.GroupId;

            return dal.AddUserInfo(user, strLoanRepIds, strGroupIds, nSourceID, nComGroupID, ROLEID_EXECUTIVE);
        }

        /// <summary>
        /// Save user info from UserSetup page
        /// </summary>
        /// <param name="user"></param>
        /// <param name="strLoanRepIds"></param>
        /// <param name="strGroupIds"></param>
        public void UpdateUserInfo(Model.Users user, string strLoanRepIds, string strGroupIds)
        {
            int? nComGroupID = null;
            Model.Groups groupCompany = groups.GetCompanyGroup();
            if (null != groupCompany)
                nComGroupID = groupCompany.GroupId;

            dal.UpdateUserInfo(user, strLoanRepIds, strGroupIds, nComGroupID, ROLEID_EXECUTIVE);
        }

        /// <summary>
        /// whether user have privilege to set own goals
        /// </summary>
        /// <param name="nUserId"></param>
        /// <returns></returns>
        public bool IsHaveSetOwnGoalsPrivilege(int nUserId)
        {
            return dal.IsHaveSetOwnGoalsPrivilege(nUserId);
        }

        /// <summary>
        /// Get user and his branch group info
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserBranchInfo()
        {
            return dal.GetUserBranchInfo();
        }

        /// <summary>
        /// Get user and his branch group info
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserBranchInfo(string sUserID)
        {
            return dal.GetUserBranchInfo(sUserID);
        }

        #region ������ӵķ���

        /// <summary>
        /// ��ȡUser�б�
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetUserListBase(string sWhere)
        {
            return this.dal.GetUserListBase(sWhere);
        }
        public DataTable GetUserList(string sWhere)
        {
            return this.dal.GetUserList(sWhere);
        }
        #endregion

        /// <summary>
        /// ��ȡGroup Member�б�
        /// </summary>
        /// <param name="iGroupID"></param>
        /// <returns></returns>
        public DataTable GetGroupMemberList(int iGroupID)
        {
            return this.dal.GetGroupMemberListBase(iGroupID);
        }

        /// <summary>
        /// ��ȡRegionѡ��Executive���б�
        /// </summary>
        /// <param name="regionId">region id</param>
        /// <param name="groupId">group id</param>
        /// <returns></returns>
        public DataTable GetRegionExecutivesSelectionList(int regionId, int groupId)
        {
            return dal.GetRegionExecutivesSelectionList(regionId, groupId, 1);
        }


        public DataTable GetBranchManager(string BranchID)
        {
            return dal.GetBranchManager(BranchID);
        }


        public DataTable GetDivisionExecutive(string DivisionId)
        {
            return dal.GetDivisionExecutive(DivisionId);
        }


        /// <summary>
        /// ��ȡ����division executive Ȩ�޵� User�б�
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetDivisionExecutiveSeletion()
        {
            return dal.GetDivisionExecutiveSeletion();
        }


        public string GetLoanOfficer(int FileID)
        {
            string LoanOfficer = string.Empty; ;

            try
            {
                DataSet ds = dal.GetLoanOfficer(FileID, "Loan Officer");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    LoanOfficer = row["LastName"].ToString() + ", " + row["FirstName"].ToString();
                }
            }
            catch
            {
            }

            return LoanOfficer;
        }


        /// <summary>
        /// adjust whether or not the user is company executive
        /// neo 2010-11-14
        /// </summary>
        /// <param name="iUsreID"></param>
        /// <returns></returns>
        public bool IsCompanyExecutive(int iUserID)
        {
            return dal.IsCompanyExecutiveBase(iUserID);
        }

        /// <summary>
        /// adjust whether or not the user is region executive
        /// neo 2010-11-14
        /// </summary>
        /// <param name="iUsreID"></param>
        /// <returns></returns>
        public bool IsRegionExecutive(int iUserID)
        {
            return dal.IsRegionExecutiveBase(iUserID);
        }

        /// <summary>
        /// adjust whether or not the user is division executive
        /// neo 2010-11-14
        /// </summary>
        /// <param name="iUsreID"></param>
        /// <returns></returns>
        public bool IsDivisionExecutive(int iUserID)
        {
            return dal.IsDivisionExecutiveBase(iUserID);
        }

        public DataSet GetAllBranchUser(int FileId, int RoleId)
        {
            return dal.GetAllBranchUser(FileId, RoleId);

        }
        public DataSet GetAllCompanyUserByRoleId(int RoleId)
        {
            return dal.GetAllCompanyUserByRoleId(RoleId);

        }
        /// <summary>
        /// adjust whether or not the user is Branch Manager
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public bool IsBranchManager(int iUserID)
        {
            return dal.IsBranchManagerBase(iUserID);
        }
        /// <summary>
        /// Gets the user name from user role.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public string GetUserNameFromUserRole(int roleId, int fileId)
        {
            return dal.GetUserNameFromUserRole(roleId, fileId);
        }

        public DataSet GetUserBranchOthersLoanOfficerUserInfo(int UserID)
        {
            return dal.GetUserBranchOthersLoanOfficerUserInfo(UserID);

        }

        /// <summary>
        /// adjust whether or not the user is Company user
        /// Alex 2011-02-21
        /// </summary>
        /// <param name="iUsreID"></param>
        /// <returns></returns>
        public bool IsCompanyUser(int iUserID)
        {
            return dal.IsCompanyUserBase(iUserID);
        }

        /// <summary>
        /// adjust whether or not the user is Region user
        /// Alex 2011-02-21
        /// </summary>
        /// <param name="iUsreID"></param>
        /// <returns></returns>
        public bool IsRegionUser(int iUserID)
        {
            return dal.IsRegionUserBase(iUserID);
        }

        /// <summary>
        /// adjust whether or not the user is Division user
        /// Alex 2011-02-21
        /// </summary>
        /// <param name="iUsreID"></param>
        /// <returns></returns>
        public bool IsDivisionUser(int iUserID)
        {
            return dal.IsDivisionUserBase(iUserID);
        }

        /// <summary>
        /// adjust whether or not the user is Branch user
        /// Alex 2011-02-21
        /// </summary>
        /// <param name="iUsreID"></param>
        /// <returns></returns>
        public bool IsBranchUser(int iUserID)
        {
            return dal.IsBranchUserBase(iUserID);
        }


        /// <summary>
        /// all the enable lo in the prospect lo's branch
        /// </summary>
        /// <param name="ContactId"></param>
        /// <returns></returns>
        public DataSet GetProspectLoanOfficers(int ContactId)
        {
            return dal.GetProspectLoanOfficers(ContactId);
        }

        #region neo

        /// <summary>
        /// get user info
        /// neo 2011-03-13
        /// </summary>
        /// <param name="iUserID"></param>
        /// <returns></returns>
        public DataTable GetUserInfo(int iUserID)
        {
            return dal.GetUserInfoBase(iUserID);
        }

        #endregion

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetContactUserByContactID(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            return dal.GetContactUserByContactID(PageSize, PageIndex, strWhere, out recordCount, orderName, orderType);
        }

        /// <summary>
        /// �����������õ�LoanOfficer��Ϣ
        /// </summary>
        /// <param name="ContactId"></param>
        /// <returns></returns>
        public DataSet GetConditionLoanOfficers(string sConditionString)
        {
            return dal.GetConditionLoanOfficers(sConditionString);
        }

        public DataTable GetAllUsers(int branchId)
        {
            return dal.GetAllUsers(branchId);
        }

        public DataTable GetUserListByBranches_Executive(int iUserID)
        {
            return dal.GetUserListByBranches_Executive(iUserID);
        }

        public DataTable GetUserListByUserBranches(int iUserID)
        {
            return dal.GetUserListByUserBranches(iUserID);
        }

        public DataTable GetUserListBuRoles(string roles)
        {
            return dal.GetUserListByRole(roles);
        }
    }
}

