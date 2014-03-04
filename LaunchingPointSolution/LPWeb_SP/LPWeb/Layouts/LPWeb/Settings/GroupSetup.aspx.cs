using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using LPWeb.Common;
using System.Text;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Settings
{
    public partial class GroupSetup : BasePage
    {
        int iGroupID = 0;
        Groups GroupManager = new Groups();
        DataTable GroupMembers;

        /// <summary>
        /// add FullName column
        /// neo 2010-09-04
        /// </summary>
        /// <param name="MemeberList"></param>
        private void AddFullNameColumn(DataTable MemeberList) 
        {
            // add FullName
            MemeberList.Columns.Add("FullName", typeof(String));

            foreach (DataRow MemberRow in MemeberList.Rows)
            {
                string sLastName = MemberRow["LastName"].ToString();
                string sFirstName = MemberRow["FirstName"].ToString();

                if (sFirstName == string.Empty)
                {
                    MemberRow["FullName"] = sLastName;
                }
                else
                {
                    MemberRow["FullName"] = sLastName + ", " + sFirstName;
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // 获取Group列表
            DataTable GroupList = GroupManager.GetGroupList(string.Empty);
            
            // 如果没有Group，加重空页面
            if(GroupList.Rows.Count == 0)
            {
                return;
            }

            // 绑定User List
            this.ddlGroupList.DataSource = GroupList;
            this.ddlGroupList.DataBind();

            #region 设置GroupID

            string sErrorMsg = "Failed to load current page: invalid GroupID.";
            string sReturnPage = "GroupSetup.aspx";

            if(this.Request.QueryString["GroupID"] != null) // 如果有GroupID
            {
                string sGroupID = this.Request.QueryString["GroupID"].ToString();
                if(PageCommon.IsID(sGroupID) == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                }

                this.iGroupID = Convert.ToInt32(sGroupID);
            }
            else // 如果没有GroupID，初始化时
            {
                // 取第一个Group的ID
                this.iGroupID = Convert.ToInt32(GroupList.Rows[0]["GroupID"]);
            }

            #endregion

            #region 加载选中Group信息

            DataTable CurrentGroupInfo = GroupManager.GetGroupInfo(this.iGroupID);

            if (CurrentGroupInfo.Rows.Count == 0)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
            }

            #endregion

            // 设置Selected Group
            this.ddlGroupList.SelectedValue = this.iGroupID.ToString();

            #region 获取Group Members

            this.GroupMembers = this.GroupManager.GetGroupMemberList(this.iGroupID);

            // add FullName column
            this.AddFullNameColumn(this.GroupMembers);

            this.gridUserList.DataSource = this.GroupMembers;
            this.gridUserList.DataBind();
            
            #endregion

            #region 获取User Selection List

            // 未分配的和已分配到当前Group中的User
            DataTable UserSelectionList = this.GroupManager.GetGroupMemberSelectionList();

            // add FullName column
            this.AddFullNameColumn(UserSelectionList);

            this.gridUserSelectionList.DataSource = UserSelectionList;
            this.gridUserSelectionList.DataBind();

            #endregion

            if (this.IsPostBack == false)
            {
                //权限验证
                var loginUser = new LoginUser();
                if (!loginUser.userRole.CompanySetup)
                {
                    Response.Redirect("../Unauthorize.aspx");
                    return;
                }
                // 设置Enabled
                this.chkEnabled.Checked = Convert.ToBoolean(CurrentGroupInfo.Rows[0]["Enabled"].ToString());

                // 设置Group Description
                this.txtGroupDesc.Text = CurrentGroupInfo.Rows[0]["GroupDesc"].ToString();
            }
        }

        /// <summary>
        /// 保存Group信息
        /// neo 2010-09-04
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            #region 用户输入
            bool bEnabled = this.chkEnabled.Checked;
            string sGroupDesc = this.txtGroupDesc.Text.Trim();
            string sGroupMemberIDs = this.hdnGroupMemberIDs.Value;

            StringBuilder sbOldGroupMemberIDs = new StringBuilder();
            int i = 0;
            foreach(DataRow GroupMemberRow in this.GroupMembers.Rows)
            {
                string sMemberID = GroupMemberRow["UserID"].ToString();
                if(i == 0)
                {
                    sbOldGroupMemberIDs.Append(sMemberID);
                }
                else
                {
                    sbOldGroupMemberIDs.Append("," + sMemberID);
                }

                i++;
            }
            #endregion

            #region 保存Group信息
            try
            {
                this.GroupManager.SaveGroupAndMembers(this.iGroupID, bEnabled, sGroupDesc, sbOldGroupMemberIDs.ToString(), sGroupMemberIDs);
            }
            catch(Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to save group info, please try it again.");
                LPLog.LogMessage(LogType.Logerror, "Failed to save group info: " + ex.Message);
                return;
            }
            #endregion

            // 成功信息
            PageCommon.WriteJsEnd(this, "Group save successfully.", PageCommon.Js_RefreshSelf);
        }
    }
}