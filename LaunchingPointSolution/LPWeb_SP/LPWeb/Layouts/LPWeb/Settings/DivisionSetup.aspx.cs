using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using Utilities;
public partial class Settings_DivisionSetup : BasePage
{
    #region 
    int iDivisionID = 0;
    Divisions divManager = new Divisions();
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //LoginUser loginUser = new LoginUser();
            //loginUser.ValidatePageVisitPermission("DivisionSetup");
            //权限验证
            var loginUser = new LoginUser();
            if (!loginUser.userRole.CompanySetup)
            {
                Response.Redirect("../Unauthorize.aspx");
                return;
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }
        if (!IsPostBack)
        {
            BindDivNames();

            string sErrorMsg = "Failed to load current page: invalid DivisionID.";
            string sReturnPage = "DivisionSetup.aspx";
            if (this.Request.QueryString["DivisionID"] != null) // 如果有GroupID
            {
                string sDivisionID = this.Request.QueryString["DivisionID"].ToString();
                if (PageCommon.IsID(sDivisionID) == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                }

                this.iDivisionID = Convert.ToInt32(sDivisionID);
                // 设置Selected  

            }
            else // 如果没有Division，初始化时
            {
                // 取第一个 Division的ID
                if (ddlDivision.Items.Count > 0)
                {
                    this.iDivisionID = Convert.ToInt32(ddlDivision.Items[0].Value);
                }
            }
            if (!divManager.ExistDivision(iDivisionID))
            {
                return;
            }
            ViewState["divisionid"] = iDivisionID.ToString();
            this.ddlDivision.SelectedValue = iDivisionID.ToString();
            LoadControls();
        }
        iDivisionID = int.Parse(ViewState["divisionid"].ToString());
    }
    #endregion

    #region Function
    private void LoadControls()
    {
        BindGroups(iDivisionID);
        BindBranchs(iDivisionID);
        FillControls();
        BindSelectBranchs();
        BindExecutivesSelection(iDivisionID);
    }

    private void BindDivNames()
    {
        try
        {
            ddlDivision.DataValueField = "DivisionId";
            ddlDivision.DataTextField = "Name";
            ddlDivision.DataSource = divManager.GetAllList();
            ddlDivision.DataBind();
        }
        catch
        {
        }
    }

    private void BindGroups(int DivisionID)
    {
        try
        {
            Groups group = new Groups();
            ddlGroupAccess.DataValueField = "GroupId";
            ddlGroupAccess.DataTextField = "GroupName";
            ddlGroupAccess.DataSource = group.GetGroupsByDivisionID(DivisionID);
            ddlGroupAccess.DataBind();

            var item = new System.Web.UI.WebControls.ListItem();
            item.Text = "----Select a Group ----";
            item.Value = "0";
            ddlGroupAccess.Items.Insert(0, item);
        }
        catch
        {
        }
    }

    private void BindBranchs(int divID)
    {
        Branches branch = new Branches();
        try
        {
            string strWhere = " DivisionID = " + divID;
            DataSet ds = branch.GetList(strWhere);
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                //ds=null;
            }
            gridBranchList.AutoGenerateColumns = false;
            gridBranchList.DataSource = ds.Tables[0];
            gridBranchList.DataBind();
        }
        catch
        { }
    }

    private void BindSelectBranchs()
    {
        Branches branch = new Branches();
        try
        {
            string strWhere = " (DivisionID is null) or (DivisionID = " + this.iDivisionID.ToString() + ")";
            DataSet ds = branch.GetList(strWhere);
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                //ds = null;
            }
            gridBranchSelectionList.AutoGenerateColumns = false;
            gridBranchSelectionList.DataSource = ds.Tables[0];
            gridBranchSelectionList.DataBind();
        }
        catch
        { }
    }

    private void FillControls()
    {
        LPWeb.Model.Divisions model = null;
        try
        {
            model = divManager.GetModel(iDivisionID);
            if (model == null)
            {
                return;
            }
            ckbEnabled.Checked = model.Enabled;
            txbDescription.Text = model.Desc;

            BindExecutives(iDivisionID);

            if (model.GroupID.HasValue && ddlGroupAccess.Items.FindByValue(model.GroupID.Value.ToString()) != null)
            {
                ddlGroupAccess.SelectedValue = model.GroupID.Value.ToString();//设置选中项  
            }
            else
            {
                ddlGroupAccess.SelectedValue = "0";
            }
        }
        catch
        { }
    }

    private void BindExecutives(int DivisionID)
    {
        try
        {
            Users UserManager = new Users();
            DataTable GroupMembers = UserManager.GetDivisionExecutive(DivisionID.ToString());
            this.gridUserList.DataSource = GroupMembers;
            this.gridUserList.DataBind();
        }
        catch
        { }
    }

    private void BindExecutivesSelection(int DivisionID)
    {
        try
        {
            Users UserManager = new Users();
            DataTable GroupMembers = UserManager.GetDivisionExecutiveSeletion();
            this.gridUserSelectionList.DataSource = GroupMembers;
            this.gridUserSelectionList.DataBind();
        }
        catch
        { }
    }
    #endregion

    #region Event
    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool bEnabled = this.ckbEnabled.Checked;
        string sDesc = this.txbDescription.Text.Trim();
        string sBranchMemberIDs = this.hdnBranchMemberIDs.Value;
        string sExectives = this.hdnExecutiveIDs.Value;

        StringBuilder sbOldGroupMemberIDs = new StringBuilder();
        int iGroupID = 0;
        if (this.ddlGroupAccess.SelectedIndex >= 0)
        {
            iGroupID = Convert.ToInt32(this.ddlGroupAccess.SelectedValue);
        }
        LPWeb.Model.Divisions model = this.divManager.GetModel(this.iDivisionID);
        int iOldGroupID = Convert.ToInt32(model.GroupID);

        try
        {
            this.divManager.SaveDivisionAndMembersBase(this.iDivisionID, bEnabled, sDesc, iGroupID, sBranchMemberIDs, sExectives);

            //Save group folder info
            GroupFolder groupFolder = new GroupFolder();
            if (iGroupID != 0)
            {
                groupFolder.DoSaveGroupFolder(Convert.ToInt32(iGroupID), this.iDivisionID, "division", iOldGroupID);
            }
            model = this.divManager.GetModel(this.iDivisionID);
            if (model.RegionID != 0 && model.RegionID != null)
            {
                Regions regMgr = new Regions();
                LPWeb.Model.Regions regionModel = regMgr.GetModel(Convert.ToInt32(model.RegionID));
                if (regionModel.GroupID != null && regionModel.GroupID != 0)
                {
                    groupFolder.DoSaveGroupFolder(Convert.ToInt32(regionModel.GroupID), Convert.ToInt32(model.RegionID), "region",Convert.ToInt32(regionModel.GroupID));
                }
            }
            PageCommon.WriteJsEnd(this, "Division saved successfully.", PageCommon.Js_RefreshSelf);
        }
        catch(Exception ex)
        {
            LPLog.LogMessage(ex.Message);
            PageCommon.WriteJsEnd(this, "Failed to save the record.", PageCommon.Js_RefreshSelf);
        }

    }

    private Dictionary<string, SqlParameter[]> GetDivisionSaveDictionary()
    {
        Dictionary<string, SqlParameter[]> param = new Dictionary<string, SqlParameter[]>();

        LPWeb.Model.Divisions model = new LPWeb.Model.Divisions();
        StringBuilder strSql = new StringBuilder();
        strSql.Append("insert into Divisions(");
        strSql.Append("Name,[Desc],Enabled)");
        strSql.Append(" values (");
        strSql.Append("@Name,@Desc,@Enabled)");
        strSql.Append(";select @@IDENTITY");
        SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
        parameters[0].Value = model;
        parameters[1].Value = model.Desc;
        parameters[2].Value = model.Enabled;

        param.Add(strSql.ToString(), parameters);
        return param;
    }

    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        iDivisionID = int.Parse(ddlDivision.SelectedValue);
        LoadControls();
    }

    #endregion 
}