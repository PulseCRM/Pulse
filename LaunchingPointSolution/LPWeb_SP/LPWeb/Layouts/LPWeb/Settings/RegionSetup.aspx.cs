using System;
using System.Data;
using System.Text;
using System.Web.UI;
using LPWeb.BLL;
using LPWeb.Common;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;

public partial class Settings_RegionSetup : BasePage
{
    private readonly Regions _bllRegions = new Regions();
    private readonly Divisions _bllDivisions = new Divisions();
    string sErrorMsg = "Failed to load the current page: invalid RegionID.";
    string sReturnPage = "RegionSetup.aspx";

    private LPWeb.Model.Regions CurrentRegion
    {
        set
        {
            ViewState["curRegion"] = value;
        }
        get
        {
            if (ViewState["curRegion"] == null)
                return null;

            return ViewState["curRegion"] as LPWeb.Model.Regions;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //权限验证
            var loginUser = new LoginUser();
            if (!loginUser.userRole.CompanySetup)
            {
                Response.Redirect("../Unauthorize.aspx");
                return;
            }

            #region Bind the regions list

            // 获取Region列表
            DataSet dsRegions = _bllRegions.GetList(0, string.Empty, "Name ASC");

            // 如果没有Region，加重空页面)
            if (dsRegions == null || dsRegions.Tables.Count == 0 || dsRegions.Tables[0].Rows.Count == 0)
            {
                return;
            }

            ddlRegions.DataSource = dsRegions;
            ddlRegions.DataBind();

            #endregion  Bind the regions list

            int regionId = int.Parse(dsRegions.Tables[0].Rows[0]["RegionID"].ToString());

            if (this.Request.QueryString["RegionID"] != null) // 如果有RegionID
            {
                string parRegionId = this.Request.QueryString["RegionID"].ToString();
                if (PageCommon.IsID(parRegionId) == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                }
                regionId = int.Parse(parRegionId);
            }

            ddlRegions.SelectedValue = regionId.ToString();//设置默认选中

            RefreshBindPages(regionId);
        }
    }

    /// <summary>
    /// 绑定页面
    /// </summary>
    /// <param name="regionId"></param>
    private void RefreshBindPages(int regionId)
    {
        hfdRegionId.Value = regionId.ToString();

        CurrentRegion = _bllRegions.GetModel(regionId);

        cbxEnabled.Checked = CurrentRegion.Enabled;
        tbxDescription.Text = CurrentRegion.Desc;

        BindGroupAccess();

        BindDivisions();

        BindExecutives();

        //绑定division的选择列表
        BindDivisionSelectionList();

        //绑定Excutives选择列表
        //BindExecutivesSelectionList();
    }

    /// <summary>
    /// Bind the group access dropdownlist
    /// </summary>
    private void BindGroupAccess()
    {
        var bllGroups = new Groups();

        //Region、Division、Branch，与Group，都是一一对应的关系。例如：Region通过Group Access选入唯一的Group，而一个Group也只能被引用一次。
        ddlGroupAccess.DataSource = bllGroups.GetGroupsByRegionID(CurrentRegion.RegionId);
        ddlGroupAccess.DataBind();

        var item = new System.Web.UI.WebControls.ListItem();
        item.Text = "----Select a Group ----";
        item.Value = "0";
        ddlGroupAccess.Items.Add(item);
        if (CurrentRegion.GroupID.HasValue && ddlGroupAccess.Items.FindByValue(CurrentRegion.GroupID.Value.ToString()) != null)
            ddlGroupAccess.SelectedValue = CurrentRegion.GroupID.Value.ToString();//设置选中项
        else
            ddlGroupAccess.SelectedValue = "0";
    }

    /// <summary>
    /// Bind the divisions by region
    /// </summary>
    private void BindDivisions()
    {
        //获取当前region的所有division
        var dsDivisions = _bllDivisions.GetList(string.Format(" [RegionID]={0}", CurrentRegion.RegionId));

        gridDivisionList.DataSource = dsDivisions;
        gridDivisionList.DataBind();
    }

    /// <summary>
    /// Bind Executives
    /// </summary>
    private void BindExecutives()
    {
        RegionExecutives bllRegionExe = new RegionExecutives();

        this.gridExecutivesList.DataSource = bllRegionExe.GetList(string.Format("RegionID={0} ", CurrentRegion.RegionId.ToString())); ;
        this.gridExecutivesList.DataBind();
    }

    /// <summary>
    /// 绑定division的选择列表
    /// </summary>
    private void BindDivisionSelectionList()
    {
        DataSet dsDivisionSelection = _bllDivisions.GetList(string.Format(" ([RegionID] IS NULL) OR ([RegionID]={0})", CurrentRegion.RegionId));

        gridDivisionSelectionList.DataSource = dsDivisionSelection;
        gridDivisionSelectionList.DataBind();
    }

    ///// <summary>
    ///// 绑定division的选择列表
    ///// </summary>
    //private void BindExecutivesSelectionList()
    //{
    //    var bllUser = new Users();

    //    if (CurrentRegion.GroupID.HasValue)
    //    {
    //        //region选择Executives的规则：
    //        //1.该用户是当前Region关联的Group下的
    //        //2.该用户是"Executive"角色
    //        //3.该用户当前是不属于任何Region的Executive
    //        DataTable executivesSelectionList = bllUser.GetRegionExecutivesSelectionList(CurrentRegion.RegionId, CurrentRegion.GroupID.Value);
    //        this.gridExecutiveSelectionList.DataSource = executivesSelectionList;
    //    }
    //    this.gridExecutiveSelectionList.DataBind();
    //}

    /// <summary>
    /// Region Dropdownlist改变选择事件处理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlRegions_SelectedIndexChanged(object sender, EventArgs e)
    {
        RefreshBindPages(int.Parse(ddlRegions.SelectedValue));
    }

    /// <summary>
    /// Save changes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool bEnabled = cbxEnabled.Checked;
        string sDesc = tbxDescription.Text;
        string sDivisionMemberIDs = hdnDivisionMemberIDs.Value;
        string sExectives = hdnExecutivesMembers.Value;
        int groupId = int.Parse(ddlGroupAccess.SelectedValue);

        StringBuilder sbOldGroupMemberIDs = new StringBuilder();

        try
        {
            _bllRegions.SaveRegionAndMembersBase(CurrentRegion.RegionId, bEnabled, sDesc, groupId, sDivisionMemberIDs, sExectives);
            //Save group folder info
            if (groupId != 0)
            {
                GroupFolder groupFolder = new GroupFolder();
                groupFolder.DoSaveGroupFolder(groupId, CurrentRegion.RegionId, "region", Convert.ToInt32(CurrentRegion.GroupID));
            }
        }
        catch(Exception ex)
        {
            PageCommon.AlertMsg(this, "Failed to save region info, please try it again.");
            LPLog.LogMessage(LogType.Logerror, "Failed to save region info, reason: " + ex.Message);
            return;
        }

        PageCommon.WriteJsEnd(this, "Region saved successfully.", "window.location.href='" + sReturnPage + "?RegionID=" + CurrentRegion.RegionId + "'");
    }
}