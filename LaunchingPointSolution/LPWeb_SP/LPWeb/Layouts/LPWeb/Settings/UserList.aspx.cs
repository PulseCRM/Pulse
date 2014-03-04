using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.DAL;
using LPWeb.Common;
using Utilities;
using System.Text;
using LPWeb.LP_Service;
using System.Collections;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Settings
{
    /// <summary>
    /// User List View page
    /// Author: Peter
    /// Date: 2010-09-01
    /// </summary>
    public partial class UserList : BasePage
    {
        BLL.Users UsersManager = new BLL.Users();
        BLL.Regions regions = new BLL.Regions();
        BLL.Divisions division = new BLL.Divisions();
        BLL.Branches branchs = new BLL.Branches();
        BLL.LoanTeam LoanTeamManager = new BLL.LoanTeam();
        DataTable dtUserBranch = new DataTable();
        const int ROLEID_EXECUTIVE = 1;     // Executive RoleId
        const int ROLEID_BRANCHMANAGER = 2; // Branch manager RoleId
        const int ROLEID_LO = 3;            // Loan Officer RoleId
        private bool isReset = false;
        static int originalUserId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CurrUser.userRole.CompanySetup)
                {
                    Response.Redirect("../Unauthorize.aspx");
                    Response.End();
                }
                else
                {
                    this.DoInitData();
                }

                if (!CurrUser.userRole.SetUserGoals)
                {
                    this.lbtnSetGoals.Enabled = false;
                }

                BLL.Company_General comGeneral = new BLL.Company_General();
                Model.Company_General company = comGeneral.GetModel();
                if (null != company)
                    this.hiPrefix.Value = company.AD_OU_Filter;

                BindGrid();
            }
        }

        /// <summary>
        /// load dropdown list data
        /// </summary>
        private void DoInitData()
        {
            try
            {
                DataSet dsFilter = PageCommon.GetOrgStructureDataSourceByLoginUser(null, null, true);
                this.ddlRegion.DataSource = dsFilter.Tables[0];
                this.ddlRegion.DataValueField = "RegionId";
                this.ddlRegion.DataTextField = "Name";
                this.ddlRegion.DataBind();

                this.ddlDivision.DataSource = dsFilter.Tables[1];
                this.ddlDivision.DataValueField = "DivisionId";
                this.ddlDivision.DataTextField = "Name";
                this.ddlDivision.DataBind();

                this.ddlBranch.DataSource = dsFilter.Tables[2];
                this.ddlBranch.DataValueField = "BranchId";
                this.ddlBranch.DataTextField = "Name";
                this.ddlBranch.DataBind();

                this.ddlRegion.SelectedIndex = 0;
                this.ddlDivision.SelectedIndex = 0;
                this.ddlBranch.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get division data
        /// </summary>
        /// <returns></returns>
        private DataTable GetDivisionData(string sRegionID)
        {
            try
            {
                int? nRegion = null;
                int nTemp = -1;
                if (!int.TryParse(sRegionID, out nTemp))
                    nTemp = -1;
                if (nTemp != -1)
                    nRegion = nTemp;
                DataTable dtDivision = PageCommon.GetOrgStructureDataSourceByLoginUser(nRegion, null, true).Tables[1];
                return dtDivision;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Branch data
        /// </summary>
        /// <returns></returns>
        private DataTable GetBranchData(string sRegionID, string sDivisionID)
        {
            try
            {
                int? nDivision = null;
                int nTemp = -1;
                if (!int.TryParse(sDivisionID, out nTemp))
                    nTemp = -1;
                if (nTemp != -1)
                    nDivision = nTemp;

                int? nRegionID = null;
                int nTemp2 = -1;
                if (!int.TryParse(sRegionID, out nTemp2))
                    nTemp2 = -1;
                if (nTemp2 != -1)
                    nRegionID = nTemp2;
                DataTable dtBranches = PageCommon.GetOrgStructureDataSourceByLoginUser(nRegionID, nDivision, true).Tables[2];
                return dtBranches;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Bind user gridview
        /// </summary>
        private void BindGrid()
        {
            // Get user branch info
            dtUserBranch = UsersManager.GetUserBranchInfo();

            int pageSize = AspNetPager1.PageSize;
            int pageIndex = 1;

            if (isReset == true)
                pageIndex = AspNetPager1.CurrentPageIndex = 1;
            else
                pageIndex = AspNetPager1.CurrentPageIndex;

            string strWhare = GetSqlWhereClause();
            int recordCount = 0;

            DataSet userList = null;
            try
            {
                //userList = UsersManager.GetListForGridView(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
                userList = UsersManager.GetUserList(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            gridUserList.DataSource = userList;
            gridUserList.DataBind();

            //ClientFun(this.updatePanel, "registerClearIds", "arrSelectedLOID = new Array(); arrSelectedUId = new Array();");
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
            BindGrid();
        }

        /// <summary>
        /// Get filter
        /// </summary>
        /// <returns></returns>
        private string GetSqlWhereClause()
        {
            string strWhere = "";
            if (ddlBranch.SelectedValue != "0" && ddlBranch.SelectedValue != "-1")
            {
                strWhere = string.Format(@" AND (EXISTS (SELECT 1 FROM 
(SELECT UserId FROM GroupUsers gu WHERE EXISTS (SELECT 1 FROM Groups WHERE BranchID={0} AND GroupId=gu.GroupID)) t1
WHERE t.UserId=t1.UserID)", ddlBranch.SelectedValue);
                //if (ROLEID_BRANCHMANAGER == CurrUser.iRoleID)   // current user is a branchmanager
                //{
                //    strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId "
                //        + "IN(SELECT BranchId FROM BranchManagers WHERE BranchMgrId='{0}') AND BranchId)", CurrUser.iUserID);
                //}

                strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId={0})", ddlBranch.SelectedValue);
                strWhere += ")";
            }
            else if (ddlDivision.SelectedValue != "0" && ddlDivision.SelectedValue != "-1")
            {
                strWhere = string.Format(@"AND (EXISTS (SELECT 1 FROM 
(SELECT UserId FROM GroupUsers gu WHERE EXISTS (SELECT 1 FROM Groups WHERE DivisionID={0} AND GroupId=gu.GroupID)) t1
WHERE t.UserId=t1.UserID)", ddlDivision.SelectedValue);

                // 如果当前用户是所选Region的Excutive，则要显示Region的所有Executive
                //if (ROLEID_EXECUTIVE == CurrUser.iRoleID)   // current user is a executive
                //{
                //    strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM DivisionExecutives WHERE DivisionId "
                //        + "IN(SELECT DivisionId FROM DivisionExecutives WHERE ExecutiveId='{0}'))", CurrUser.iUserID);
                //}
                strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM DivisionExecutives WHERE DivisionId = {0})", ddlDivision.SelectedValue);
                strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId IN(SELECT BranchId FROM Branches WHERE DivisionID = {0}))", ddlDivision.SelectedValue);
                strWhere += ")";
            }
            else if (ddlRegion.SelectedValue != "0" && ddlRegion.SelectedValue != "-1")
            {
                strWhere = string.Format(@"AND (EXISTS (SELECT 1 FROM 
(SELECT UserId FROM GroupUsers gu WHERE EXISTS (SELECT 1 FROM Groups WHERE RegionID={0} AND GroupId=gu.GroupID)) t1
WHERE t.UserId=t1.UserID)", ddlRegion.SelectedValue);

                // 如果当前用户是所选Region的Excutive，则要显示Region的所有Executive
                //if (ROLEID_EXECUTIVE == CurrUser.iRoleID)   // current user is a executive
                //{
                //    strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM RegionExecutives WHERE RegionId " 
                //        + "IN(SELECT RegionId FROM RegionExecutives WHERE ExecutiveId='{0}'))", CurrUser.iUserID);
                //}
                strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM RegionExecutives WHERE RegionId = {0})", ddlRegion.SelectedValue);
                strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM DivisionExecutives WHERE DivisionId IN(SELECT DivisionId FROM Divisions WHERE RegionID = {0}))", ddlRegion.SelectedValue);
                strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId IN(SELECT BranchId FROM Branches WHERE RegionID = {0}))", ddlRegion.SelectedValue);
                strWhere += ")";
            }
            //else
            //{
            #region old code
            //                StringBuilder sbRegionIDs = new StringBuilder();
            //                ListItemCollection listItemCol = null;
            //                string strId = "";
            //                if (this.ddlRegion.Items.Count > 1)
            //                {
            //                    strId = "RegionID";
            //                    listItemCol = this.ddlRegion.Items;
            //                }
            //                else if (this.ddlDivision.Items.Count > 1)
            //                {
            //                    strId = "DivisionID";
            //                    listItemCol = this.ddlDivision.Items;
            //                }
            //                else
            //                {
            //                    strId = "BranchID";
            //                    listItemCol = this.ddlBranch.Items;
            //                }

            //                foreach (ListItem item in listItemCol)
            //                {
            //                    if (item.Value != "0" && item.Value != "-1" && item.Value != "")
            //                    {
            //                        if (sbRegionIDs.Length > 0)
            //                            sbRegionIDs.Append(",");
            //                        sbRegionIDs.Append(item.Value);
            //                    }
            //                }

            //                if (ROLEID_EXECUTIVE == CurrUser.iRoleID)
            //                {
            //                    strWhere = string.Format(@"AND EXISTS (SELECT 1 FROM 
            //(SELECT UserId FROM GroupUsers gu WHERE EXISTS (SELECT 1 FROM Groups WHERE GroupId=gu.GroupID)) t1
            //WHERE t.UserId=t1.UserID)" );
            //                }
            //                else
            //                {
            //                    if (sbRegionIDs.Length > 0)
            //                        strWhere = string.Format(@"AND EXISTS (SELECT 1 FROM 
            //(SELECT UserId FROM GroupUsers gu WHERE EXISTS (SELECT 1 FROM Groups WHERE {0} IN ({1}) AND GroupId=gu.GroupID)) t1
            //WHERE t.UserId=t1.UserID)", strId, sbRegionIDs);
            //                    else
            //                        strWhere = "AND 1=2";
            //                }
            #endregion
            //}
            if (CurrUser.bIsCompanyExecutive)
            {
                strWhere += "";
            }
            else if (CurrUser.bIsRegionExecutive)
            {
                strWhere += " AND (t.UserId IN(SELECT UserId FROM GroupUsers WHERE GroupID IN(SELECT GroupId FROM Groups WHERE RegionID IN(SELECT RegionId FROM RegionExecutives WHERE ExecutiveId = " + CurrUser.iUserID.ToString() + ")))";
                strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM RegionExecutives WHERE RegionId IN (SELECT RegionId FROM RegionExecutives WHERE ExecutiveId = {0}))", CurrUser.iUserID.ToString());
                strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM DivisionExecutives WHERE DivisionId IN(SELECT DivisionId FROM Divisions WHERE RegionID IN (SELECT RegionId FROM RegionExecutives WHERE ExecutiveId = {0})))", CurrUser.iUserID.ToString());
                strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId IN(SELECT BranchId FROM Branches WHERE RegionID IN (SELECT RegionId FROM RegionExecutives WHERE ExecutiveId = {0})))", CurrUser.iUserID.ToString());
                strWhere += ")";
            }
            else if (CurrUser.bIsDivisionExecutive)
            {
                strWhere += " AND (t.UserId IN(SELECT UserId FROM GroupUsers WHERE GroupID IN(SELECT GroupId FROM Groups WHERE DivisionID IN(SELECT DivisionId FROM DivisionExecutives WHERE ExecutiveId = " + CurrUser.iUserID.ToString() + ")))";
                strWhere += string.Format(" OR t.UserId IN (SELECT ExecutiveId FROM DivisionExecutives WHERE DivisionId IN (SELECT DivisionId FROM DivisionExecutives WHERE ExecutiveId ={0}))", CurrUser.iUserID.ToString());
                strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId IN(SELECT BranchId FROM Branches WHERE DivisionID IN(SELECT DivisionId FROM DivisionExecutives WHERE ExecutiveId ={0})))", CurrUser.iUserID.ToString());

                strWhere += ")";
            }
            else if (CurrUser.bIsBranchManager)
            {
                strWhere += " AND (t.UserId IN(SELECT UserId FROM GroupUsers WHERE GroupID IN(SELECT GroupId FROM Groups WHERE BranchID IN(SELECT BranchId FROM BranchManagers WHERE BranchMgrId = " + CurrUser.iUserID.ToString() + ")))";
                strWhere += string.Format(" OR t.UserId IN (SELECT BranchMgrId FROM BranchManagers WHERE BranchId IN (SELECT BranchId FROM BranchManagers WHERE BranchMgrId ={0}))", CurrUser.iUserID.ToString());
                strWhere += ")";
            }
            else
            {
                strWhere += " AND (t.UserId =" + CurrUser.iUserID.ToString() + ")";
            }
            if (this.ddlAlphabet.SelectedIndex > 0)
            {
                if (strWhere.Length > 0)
                {
                    strWhere = string.Format("{0} AND LastName LIKE '{1}%'", strWhere, this.ddlAlphabet.SelectedValue);
                }
                else
                {
                    strWhere = string.Format("AND LastName LIKE '{0}%'", ddlAlphabet.SelectedValue);
                }
            }

            return strWhere;
        }


        private bool AllowReassignProspect(int UserID)
        {
            bool EnableMarketing = false;

            try
            {
                LPWeb.BLL.Roles cg = new LPWeb.BLL.Roles();
                DataTable dt = cg.GetRoleByUserID(UserID.ToString());
                if (dt != null && dt.Rows.Count == 1)
                {
                    if (dt.Rows[0]["Name"].ToString() == "Loan Officer")
                    {
                        EnableMarketing = true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            { }

            try
            {
                LPWeb.BLL.Company_General cg = new LPWeb.BLL.Company_General();
                EnableMarketing = cg.CheckMarketingEnabled();
            }
            catch
            { }

            return EnableMarketing;
        }

        private void ReassignProspect(int UserID)
        {
            if (!AllowReassignProspect(UserID))
            {
                return;
            }
            ServiceManager sm = new ServiceManager();

            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                ReassignProspectRequest rpq = new ReassignProspectRequest();
                rpq.hdr = new ReqHdr();

                rpq.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                rpq.hdr.UserId = CurrUser.iUserID;
                rpq.FromUser = UserID;
                rpq.ToUser = 0;
                rpq.FileId = null;
                rpq.ContactId = null;
                rpq.UserId = new int[] { UserID };

                ReassignProspectResponse rpp = null;
                rpp = service.ReassignProspect(rpq);

                if (!rpp.hdr.Successful)
                {
                    PageCommon.AlertMsg(this, rpp.hdr.StatusInfo);
                }
            }
        }

        protected void lbtnDisable_Click(object sender, EventArgs e)
        {
            Dictionary<int, int> dicIDs = new Dictionary<int, int>();
            // Get userid of current selected row
            foreach (GridViewRow row in gridUserList.Rows)
            {
                if (DataControlRowType.DataRow == row.RowType)
                {
                    CheckBox ckbChecked = row.FindControl("ckbSelected") as CheckBox;
                    if (null != ckbChecked && ckbChecked.Checked)
                        dicIDs.Add(row.RowIndex, (int)gridUserList.DataKeys[row.RowIndex].Value);
                }
            }
            if (dicIDs.Count > 0)
            {
                try
                {
                    UsersManager.SetUsersDisable(dicIDs.Select(i => i.Value).ToList());
                }
                catch (Exception ex)
                {
                    ClientFun(this.updatePanel, "failedtodisableuser", "alert('Failed to disable the selected user account(s), please try it again.');");
                    //PageCommon.AlertMsg(this, "Failed to disable the selected users, please try it again.");
                    LPLog.LogMessage(LogType.Logerror, "Failed to disable the selected user account(s): " + ex.Message);
                    return;
                }
                BindGrid();
                try
                {
                    UpdateAD(dicIDs, UserMgrCommandType.DisableUser);
                }
                catch (Exception ex)
                {
                    ClientFun(this.updatePanel, "failedtodisableuserinad", string.Format("alert('Failed to disable the selected user account(s) in AD, Exception: {0}');", ex.Message.Replace("'", "\"")));
                    //PageCommon.AlertMsg(this, "Failed to disable the user account in AD.");
                    LPLog.LogMessage(LogType.Logerror, "Failed to disable the selected user account(s), exception: " + ex.Message);
                    return;
                }
            }
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            Dictionary<int, int> dicIDs = new Dictionary<int, int>();
            // Get userid of current selected row
            foreach (GridViewRow row in gridUserList.Rows)
            {
                if (DataControlRowType.DataRow == row.RowType)
                {
                    CheckBox ckbChecked = row.FindControl("ckbSelected") as CheckBox;
                    if (null != ckbChecked && ckbChecked.Checked)
                    {
                        dicIDs.Add(row.RowIndex, (int)gridUserList.DataKeys[row.RowIndex].Value);
                        originalUserId = (int)gridUserList.DataKeys[row.RowIndex].Value;
                    }
                }
            }
            if (dicIDs.Count > 0)
            {
                ReassignProspect(originalUserId);
                try
                {
                    // get all loans assigned to this user, dicIDs must have only one item in this case
                    DataSet dsUserLoan = LoanTeamManager.GetUserLoan(dicIDs.FirstOrDefault().Value);
                    int nReassignUserId = 0;
                    if (!int.TryParse(this.hiReassignUserId.Value, out nReassignUserId))
                        nReassignUserId = 0;
                    if (null != dsUserLoan && dsUserLoan.Tables.Count > 0 && dsUserLoan.Tables[0].Rows.Count > 0)
                    {
                        ServiceManager sm1 = new ServiceManager();
                        using (LP2ServiceClient service = sm1.StartServiceClient())
                        {
                            ReassignLoanRequest req = new ReassignLoanRequest();
                            req.hdr = new ReqHdr();
                            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                            req.hdr.UserId = CurrUser.iUserID;
                            List<ReassignUserInfo> uList = new List<ReassignUserInfo>();

                            Model.Users userToDelete = UsersManager.GetModel(dicIDs.FirstOrDefault().Value);
                            ReassignUserInfo uInfo = null;
                            foreach (DataRow drUserLoan in dsUserLoan.Tables[0].Rows)
                            {
                                uInfo = new ReassignUserInfo();
                                uInfo.FileId = int.Parse(string.Format("{0}", drUserLoan["FileId"]));
                                uInfo.RoleId = int.Parse(string.Format("{0}", drUserLoan["RoleId"]));
                                uInfo.NewUserId = nReassignUserId;
                                uList.Add(uInfo);
                            }

                            req.reassignUsers = uList.ToArray();

                            ReassignLoanResponse respone = null;
                            try
                            {
                                respone = service.ReassignLoan(req);

                                if (respone.hdr.Successful)
                                {
                                    bool st = true;

                                    int fileId = 0;
                                    int newUserId = 0;
                                    int roleId = 0;
                                    int requester = req.hdr.UserId;

                                    foreach (ReassignUserInfo u in req.reassignUsers)
                                    {
                                        fileId = u.FileId;
                                        newUserId = u.NewUserId;
                                        roleId = u.RoleId;

                                        st = LPWeb.BLL.WorkflowManager.ReassignLoan(fileId, newUserId, originalUserId, roleId, requester);
                                    }

                                    UsersManager.DeleteUsers(dicIDs.Select(i => i.Value).ToList(), CurrUser.iUserID, nReassignUserId);
                                }
                                else
                                {
                                    //   PageCommon.WriteJsEnd(this, string.Format("Failed to reassign loan, reason:{0}.", respone.hdr.StatusInfo), PageCommon.Js_RefreshSelf);
                                    //   return;
                                }
                            }
                            catch (System.ServiceModel.EndpointNotFoundException ex)
                            {
                                LPLog.LogMessage(ex.Message);
                                //    PageCommon.WriteJsEnd(this, "Failed to reassign loan, reason: Point Manager is not running.", PageCommon.Js_RefreshSelf);
                            }
                            catch (Exception exception)
                            {
                                LPLog.LogMessage(exception.Message);
                                //    PageCommon.WriteJsEnd(this, string.Format("Failed to reassign loan, reason:{0}.", exception.Message), PageCommon.Js_RefreshSelf);
                            }
                        }
                    }
                    else
                    {
                        UsersManager.DeleteUsers(dicIDs.Select(i => i.Value).ToList(), CurrUser.iUserID, nReassignUserId);
                    }

                    BindGrid();
                }
                catch (Exception ex)
                {
                    ClientFun(this.updatePanel, "failedtodeleteuserinad", "alert('Failed to delete the selected user account(s), please try it again.');");
                    //PageCommon.AlertMsg(this, "Failed to delete the selected users, please try it again.");                
                    LPLog.LogMessage(LogType.Logerror, "Failed to delete the selected user account(s): " + ex.Message);
                    return;
                }

                try
                {
                    UpdateAD(dicIDs, UserMgrCommandType.DeleteUser);
                }
                catch (Exception ex)
                {
                    ClientFun(this.updatePanel, "failed to delete user in ad", string.Format("alert('Failed to delete the selected user account(s) in AD, Exception: {0}');", ex.Message.Replace("'", "\"")));
                    //PageCommon.AlertMsg(this, "Failed to delete the selected user account in AD.");
                    LPLog.LogMessage(LogType.Logerror, "Failed to delete the selected user account(s), exception: " + ex.Message);
                    return;
                }

            }
        }

        /// <summary>
        /// update AD 
        /// </summary>
        /// <param name="dicIDs"></param>
        private void UpdateAD(Dictionary<int, int> dicIDs, UserMgrCommandType uType)
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                UpdateADUserRequest uReq;
                UpdateADUserResponse uResponse;
                ReqHdr hdr;
                Dictionary<string, string> dicFailed = new Dictionary<string, string>();
                foreach (KeyValuePair<int, int> kvp in dicIDs)
                {
                    hdr = new ReqHdr();
                    hdr.UserId = kvp.Value;
                    uReq = new UpdateADUserRequest();
                    uReq.hdr = hdr;
                    uReq.Command = uType;
                    uReq.AD_OU_Filter = this.hiPrefix.Value;
                    uReq.AD_User = new LP_Service.User();
                    uReq.AD_User.Username = gridUserList.DataKeys[kvp.Key]["Username"].ToString();
                    uReq.AD_User.Firstname = gridUserList.DataKeys[kvp.Key]["FirstName"].ToString();
                    uReq.AD_User.Lastname = gridUserList.DataKeys[kvp.Key]["LastName"].ToString();
                    uReq.AD_User.Email = gridUserList.DataKeys[kvp.Key]["EmailAddress"].ToString();

                    try
                    {
                        uResponse = service.UpdateADUser(uReq);
                        if (!uResponse.hdr.Successful)
                        {
                            dicFailed.Add(gridUserList.DataKeys[kvp.Key]["Username"].ToString(), uResponse.hdr.StatusInfo);
                        }
                    }
                    catch (System.ServiceModel.EndpointNotFoundException ee)
                    {
                        dicFailed.Add(gridUserList.DataKeys[kvp.Key]["Username"].ToString(), ee.Message);
                    }
                    catch (Exception ex)
                    {
                        dicFailed.Add(gridUserList.DataKeys[kvp.Key]["Username"].ToString(), ex.Message);
                    }
                }
                if (dicFailed.Count > 0)
                {
                    StringBuilder sbErrorMsg = new StringBuilder();
                    StringBuilder sbErrorLog = new StringBuilder();
                    foreach (KeyValuePair<string, string> kvp in dicFailed)
                    {
                        if (sbErrorMsg.Length > 0)
                            sbErrorMsg.Append(",");
                        sbErrorMsg.Append(kvp.Key);
                        if (sbErrorLog.Length > 0)
                            sbErrorLog.Append(",");
                        sbErrorLog.Append(string.Format("{0}:{1}", kvp.Key, kvp.Value));
                    }

                    // get UserMgrCommandType string
                    string strType = "";
                    switch (uType)
                    {
                        case UserMgrCommandType.DisableUser:
                            strType = "disable";
                            break;
                        case UserMgrCommandType.DeleteUser:
                            strType = "delete";
                            break;
                        default:
                            strType = "";
                            break;
                    }
                    ClientFun(this.updatePanel, "failedtodeleteuserinad",
                        string.Format("alert('Failed to {0} the selected user account(s), username = {1}, please try it again.');", strType, sbErrorMsg.ToString()));
                    //PageCommon.AlertMsg(this, string.Format("Failed to {0} the selected user account(s), username = {1}", strType, sbErrorMsg.ToString()));
                    LPLog.LogMessage(LogType.Logerror, string.Format("Failed to {0} the selected user account(s): {1}", strType, sbErrorLog.ToString()));
                }
            }
        }

        StringBuilder sbAllIds = new StringBuilder();
        StringBuilder sbAllLOIds = new StringBuilder();
        int nNonLOUserCount = 0;
        StringBuilder sbUserInfo = new StringBuilder();
        /// <summary>
        /// Set selected row when click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridUserList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (DataControlRowType.DataRow == e.Row.RowType)
            {
                Label lblBranch = e.Row.FindControl("lblBranch") as Label;
                CheckBox ckbChecked = e.Row.FindControl("ckbSelected") as CheckBox;
                int nUserID = 0;
                if (null != gridUserList.DataKeys[e.Row.RowIndex])
                {
                    if (!int.TryParse(gridUserList.DataKeys[e.Row.RowIndex].Value.ToString(), out nUserID))
                        nUserID = 0;

                    if (0 != nUserID)
                    {
                        if (sbAllIds.Length > 0)
                            sbAllIds.Append(",");
                        sbAllIds.AppendFormat("{0}", nUserID);
                        string strRoleID = string.Format("{0}", gridUserList.DataKeys[e.Row.RowIndex]["RoleId"]);
                        if (strRoleID == ROLEID_LO.ToString())
                        {
                            if (sbAllLOIds.Length > 0)
                                sbAllLOIds.Append(",");
                            sbAllLOIds.AppendFormat("{0}", nUserID);
                        }
                        else
                            nNonLOUserCount++;

                        if (null != ckbChecked)
                        {
                            ckbChecked.Attributes.Add("onclick", string.Format("onUserSelected({0}, this.checked, '{1}');",
                                (strRoleID == ROLEID_LO.ToString() ? "true" : "false"), nUserID));
                        }

                        if (null != lblBranch && null != dtUserBranch)
                        {
                            // concatenates all user branch names, using "," between each name
                            StringBuilder sbBName = new StringBuilder();
                            DataRow[] drs = dtUserBranch.Select(string.Format("UserId={0}", nUserID));
                            if (null != drs)
                            {
                                foreach (DataRow row in drs)
                                {
                                    if (sbBName.Length > 0)
                                        sbBName.Append(", ");
                                    sbBName.Append(row["Name"]);
                                }
                            }
                            int nDisLen = 30;
                            if (sbBName.Length > nDisLen)
                            {
                                lblBranch.ToolTip = sbBName.ToString();
                                lblBranch.Text = sbBName.ToString().Substring(0, nDisLen) + "...";
                            }
                            else
                                lblBranch.Text = sbBName.ToString();
                        }

                        // set loan status and branch info
                        if (sbUserInfo.Length > 0)
                            sbUserInfo.Append(";");
                        sbUserInfo.AppendFormat("{0}:{1}:{2}", nUserID, gridUserList.DataKeys[e.Row.RowIndex]["UserLoanCount"], gridUserList.DataKeys[e.Row.RowIndex]["UserContactCount"]);
                    }
                }
            }
        }

        protected void gridUserList_PreRender(object sender, EventArgs e)
        {
            this.hiAllIds.Value = sbAllIds.ToString();
            this.hiSelectedIds.Value = "";
            this.hiAllLOIds.Value = sbAllLOIds.ToString();
            this.hiSelectedLOIds.Value = "";
            this.hiNonLOUserIds.Value = nNonLOUserCount.ToString();
            this.hiUserInfo.Value = sbUserInfo.ToString();
            ClientFun(this.updatePanel, "registerNonLOSign", "nSign = 0;");
        }

        /// <summary>
        /// Handles the Sorting event of the gridUserList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void gridUserList_Sorting(object sender, GridViewSortEventArgs e)
        {
            OrderName = e.SortExpression;
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)                      //设置排序方向
            {
                GridViewSortDirection = SortDirection.Descending;
                OrderType = 0;
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                OrderType = 1;
            }
            BindGrid();
        }

        /// <summary>
        /// Gets or sets the grid view sort direction.
        /// </summary>
        /// <value>The grid view sort direction.</value>
        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;
                return (SortDirection)ViewState["sortDirection"];
            }
            set
            {
                ViewState["sortDirection"] = value;
            }

        }

        /// <summary>
        /// Gets or sets the name of the order.
        /// </summary>
        /// <value>The name of the order.</value>
        public string OrderName
        {
            get
            {
                if (ViewState["orderName"] == null)
                    ViewState["orderName"] = "LastName";
                return Convert.ToString(ViewState["orderName"]);
            }
            set
            {
                ViewState["orderName"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the order.
        /// </summary>
        /// <value>The type of the order.</value>
        public int OrderType
        {
            get
            {
                if (ViewState["orderType"] == null)
                    ViewState["orderType"] = 0;
                return Convert.ToInt32(ViewState["orderType"]);
            }
            set
            {
                ViewState["orderType"] = value;
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            isReset = true;
            BindGrid();
        }

        protected void lbtnRefreshList_Click(object sender, EventArgs e)
        {
            isReset = false;
            BindGrid();
        }

        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRegion.SelectedIndex < 0)
            {
                return;
            }
            try
            {
                this.ddlDivision.AutoPostBack = false;
                if (ddlRegion.SelectedValue == "0" || ddlRegion.SelectedValue == "-1")
                {
                    //string sDivisionID = this.ddlDivision.SelectedValue;
                    //string sBranchID = this.ddlBranch.SelectedValue;
                    this.ddlDivision.DataSource = this.GetDivisionData("-1");
                    this.ddlBranch.DataSource = this.GetBranchData("-1", "-1");
                    this.ddlDivision.DataBind();
                    this.ddlBranch.DataBind();

                    //this.ddlDivision.SelectedValue = sDivisionID;
                    //this.ddlBranch.SelectedValue = sBranchID;
                }
                else
                {
                    this.ddlDivision.DataSource = this.GetDivisionData(this.ddlRegion.SelectedValue);
                    this.ddlBranch.DataSource = this.GetBranchData(this.ddlRegion.SelectedValue, "-1");
                    this.ddlDivision.DataBind();
                    this.ddlBranch.DataBind();
                }
                this.ddlDivision.AutoPostBack = true;
            }
            catch (Exception ex)
            {
                LPLog.LogMessage(LogType.Logdebug, ex.ToString());
            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Bind Branch list when Division select change
            if (ddlDivision.SelectedIndex < 0)
            {
                return;
            }
            try
            {
                if (ddlDivision.SelectedValue == "0" || ddlDivision.SelectedValue == "-1")
                {
                    //string sBranchID = this.ddlBranch.SelectedValue;
                    this.ddlBranch.DataSource = this.GetBranchData(this.ddlRegion.SelectedValue, "-1");
                    this.ddlBranch.DataBind();
                    //this.ddlBranch.SelectedValue = sBranchID;
                }
                else
                {
                    this.ddlBranch.DataSource = this.GetBranchData(this.ddlRegion.SelectedValue, this.ddlDivision.SelectedValue);
                    this.ddlBranch.DataBind();
                }
            }
            catch (Exception ex)
            {
                LPLog.LogMessage(LogType.Logdebug, ex.ToString());
            }
        }

        /// <summary>
        /// bind division of region
        /// </summary>
        /// <param name="strRegionID"></param>
        private void BindDivisionOfRegion(string strRegionID)
        {
            this.ddlDivision.DataSource = division.GetList(string.Format("RegionID={0}", strRegionID));
            this.ddlDivision.DataValueField = "DivisionId";
            this.ddlDivision.DataTextField = "Name";
            this.ddlDivision.DataBind();
            this.ddlDivision.Items.Insert(0, new ListItem("All Divisions", "0"));
        }

        /// <summary>
        /// bind branch of division
        /// </summary>
        /// <param name="strDivisionID"></param>
        private void BindBranchOfDivision(string strDivisionID)
        {
            this.ddlBranch.DataSource = branchs.GetList(string.Format("DivisionID={0}", strDivisionID));
            this.ddlBranch.DataValueField = "BranchId";
            this.ddlBranch.DataTextField = "Name";
            this.ddlBranch.DataBind();
            this.ddlBranch.Items.Insert(0, new ListItem("All Branches", "0"));
        }

        protected void ddlAlphabet_SelectedIndexChanged(object sender, EventArgs e)
        {
            isReset = true;
            BindGrid();
        }

        /// <summary>
        /// call client function
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strScript"></param>
        private void ClientFun(string strKey, string strScript)
        {
            ClientScriptManager csm = this.Page.ClientScript;
            if (!csm.IsStartupScriptRegistered(this.GetType(), strKey))
            {
                csm.RegisterStartupScript(this.GetType(), strKey, strScript, true);
            }
        }

        /// <summary>
        /// call client function
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="strKey"></param>
        /// <param name="strScript"></param>
        private void ClientFun(Control ctl, string strKey, string strScript)
        {
            ScriptManager.RegisterStartupScript(ctl, this.GetType(), strKey, strScript, true);
        }
    }
}