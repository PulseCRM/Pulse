using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using LPWeb.Common;
using LPWeb.BLL;


public partial class CompanyOverview : BasePage
{
    Company_General company = new Company_General();
    int UserID = 0;
    int GroupID = 0;
    int FolderID = 0;
    int FileID = 0;

    string type = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //权限验证
        var loginUser = new LoginUser();
        if (!loginUser.userRole.CompanySetup)
        {
            Response.Redirect("../Unauthorize.aspx");
            return;
        }
    }

    private string GetCompanyName()
    {
        string CompanyName = string.Empty;
        Company_General cg = new Company_General();
        DataSet ds = new DataSet();
        try
        {
            ds = cg.GetList(1, "", "1");
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            { }
            else
            {
                CompanyName = ds.Tables[0].Rows[0]["Name"].ToString();
            }
        }
        catch
        { }

        return CompanyName;
    }

    private int GetUserID(string UserName)
    {
        int UserID = 0;
        try
        {
            Users user = new Users();
            DataSet ds = user.GetList(" [Username] = '" + UserName + "' AND [UserEnabled] = 1");
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                UserID = 0;
            }
            else
            {
                UserID = int.Parse(ds.Tables[0].Rows[0]["UserID"].ToString());
            }

        }
        catch
        { }
        return UserID;
    }

    private int GetGroupID(string GroupName)
    {
        int GroupID = 0;
        try
        {
            Groups group = new Groups();
            DataSet ds = group.GetList(" [GroupName] = '" + GroupName + "' AND [Enabled] = 1");
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                GroupID = 0;
            }
            else
            {
                GroupID = int.Parse(ds.Tables[0].Rows[0]["GroupID"].ToString());
            }

        }
        catch
        { }
        return GroupID;
    }

    private int GetFolderID(string FolderName)
    {
        int FolderID = 0;
        try
        {
            PointFolders folder = new PointFolders();
            DataSet ds = folder.GetList(" [Name] = '" + FolderName + "' AND [Enabled] = 1");
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                FolderID = 0;
            }
            else
            {
                FolderID = int.Parse(ds.Tables[0].Rows[0]["FolderID"].ToString());
            }

        }
        catch
        { }
        return FolderID;
    }

    private int GetFileID(string FileName)
    {
        int FileID = 0;
        try
        {
            PointFiles files = new PointFiles();
            DataSet ds = files.GetList(" [Name] = '" + FileName + "' ");
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                FileID = 0;
            }
            else
            {
                FileID = int.Parse(ds.Tables[0].Rows[0]["FileID"].ToString());
            }

        }
        catch
        { }
        return FileID;
    }

    private void BindCompanyOverview(int typeID, string type)
    {
        DataSet ds = new DataSet();
        DataTable dtCompany = new DataTable();
        dtCompany = company.GetCompanyOverviewByUser(0, typeID, type, "");
        if (dtCompany == null || dtCompany.Rows.Count < 1)
        {
            lbCompanyName.Text = string.Empty;
            Image1.Visible = false;
            Image2.Visible = false;
            rptCompanyOverview.DataSource = null;
            rptCompanyOverview.DataBind();
            return;
        }
        else
        {
            lbCompanyName.Text = dtCompany.Rows[0]["CompanyName"].ToString();
        }
        DataTable dtRegions = new DataTable();
        dtRegions = company.GetCompanyOverviewByUser(1, typeID, type, "");
        dtRegions.TableName = "Region";

        if (dtRegions == null || dtRegions.Rows.Count < 1)
        {
            rptCompanyOverview.DataSource = null;
            rptCompanyOverview.DataBind();
            return;
        }
        else
        {
            ds.Tables.Add(dtRegions);
        }

        Image1.Visible = true;
        Image2.Visible = true;
        rptCompanyOverview.DataSource = null;
        rptCompanyOverview.DataSource = ds.Tables["Region"];
        rptCompanyOverview.DataBind();
        //Page.DataBind(); 
    }

    protected void btnUserSeach_Click(object sender, ImageClickEventArgs e)
    {
        UserID = 0;
        try
        {
            string UserName = txbUserName.Text.Trim();//SP_Carl
            if (UserName.Length < 1)
            {
                lbCompanyName.Text = string.Empty;
                Image1.Visible = false;
                Image2.Visible = false;
                rptCompanyOverview.DataSource = null;
                rptCompanyOverview.DataBind();
                return;
            }
            UserID = GetUserID(UserName);
            if (UserID == 0)
            {
                lbCompanyName.Text = string.Empty;
                Image1.Visible = false;
                Image2.Visible = false;
                rptCompanyOverview.DataSource = null;
                rptCompanyOverview.DataBind();
                PageCommon.AlertMsg(this, "The user does not exist in the database.");
                return;
            }
            type = "User";
            BindCompanyOverview(UserID, "User");
        }
        catch
        { }
    }

    protected void btnGroupSarch_Click(object sender, ImageClickEventArgs e)
    {
        GroupID = 0;
        try
        {
            string GroupName = txbGroupName.Text.Trim();
            if (GroupName.Length < 1)
            {
                lbCompanyName.Text = string.Empty;
                Image1.Visible = false;
                Image2.Visible = false;
                rptCompanyOverview.DataSource = null;
                rptCompanyOverview.DataBind();
                return;
            }
            GroupID = GetGroupID(GroupName);
            if (GroupID == 0)
            {
                lbCompanyName.Text = string.Empty;
                Image1.Visible = false;
                Image2.Visible = false;
                rptCompanyOverview.DataSource = null;
                rptCompanyOverview.DataBind();
                PageCommon.AlertMsg(this, "The group does not exist in the database.");
                return;
            }
            type = "Group";
            BindCompanyOverview(GroupID, "Group");
        }
        catch
        { }
    }

    protected void btnFolderSearch_Click(object sender, ImageClickEventArgs e)
    {
        FolderID = 0;
        try
        {
            string FolderName = txbPointFolder.Text.Trim();
            if (FolderName.Length < 1)
            {
                lbCompanyName.Text = string.Empty;
                Image1.Visible = false;
                Image2.Visible = false;
                rptCompanyOverview.DataSource = null;
                rptCompanyOverview.DataBind();
                return;
            }
            FolderID = GetFolderID(FolderName);
            if (FolderID == 0)
            {
                lbCompanyName.Text = string.Empty;
                Image1.Visible = false;
                Image2.Visible = false;
                rptCompanyOverview.DataSource = null;
                rptCompanyOverview.DataBind();
                PageCommon.AlertMsg(this, "The Point Folder does not exist in the database.");
                return;
            }
            type = "Folder";
            BindCompanyOverview(FolderID, "Folder");
        }
        catch
        { }
    }

    protected void btnFileSearch_Click(object sender, ImageClickEventArgs e)
    {
        FileID = 0;
        try
        {
            string PointFile = txbPointFileName.Text.Trim();
            if (PointFile.Length < 1)
            {
                lbCompanyName.Text = string.Empty;
                Image1.Visible = false;
                Image2.Visible = false;
                rptCompanyOverview.DataSource = null;
                rptCompanyOverview.DataBind();
                return;
            }
            FileID = GetFileID(PointFile);
            if (FileID == 0)
            {
                lbCompanyName.Text = string.Empty;
                Image1.Visible = false;
                Image2.Visible = false;
                rptCompanyOverview.DataSource = null;
                rptCompanyOverview.DataBind();
                PageCommon.AlertMsg(this, "The Point File does not exist in the database.");
                return;
            }
            type = "File";
            BindCompanyOverview(FileID, "File");
        }
        catch
        { }
    }

    protected void rptCompanyOverview_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string RegionName = ((DataRowView)e.Item.DataItem).Row["RegionName"].ToString();//获得对应ID

            Repeater rptDivions = (Repeater)e.Item.FindControl("rptDivions");//找到要绑定数据的Repeater
            if (rptDivions != null && RegionName.Length > 0)
            {
                int typeid = 0;
                if (type == "User")
                {
                    typeid = UserID;
                }

                if (type == "Group")
                {
                    typeid = GroupID;
                }

                if (type == "Folder")
                {
                    typeid = FolderID;
                }
                if (type == "File")
                {
                    typeid = FileID;
                }
                if (typeid == 0)
                {
                    return;
                }

                DataTable dtDivisions = new DataTable();
                dtDivisions = company.GetCompanyOverviewByUser(2, typeid, type, "RegionName='" + RegionName + "'");
                dtDivisions.TableName = "Division";

                if (dtDivisions == null || dtDivisions.Rows.Count < 1)
                {
                    rptDivions.DataSource = null;
                    rptDivions.DataBind();
                    return;
                }
                if (dtDivisions.Rows.Count == 1 && dtDivisions.Rows[0]["DivisionName"].ToString() == string.Empty)
                {
                    rptDivions.DataSource = null;
                    rptDivions.DataBind();
                    return;
                }
                rptDivions.DataSource = dtDivisions;
                rptDivions.DataBind();
            }
        }


    }

    protected void rptDivions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string DivisionName = ((DataRowView)e.Item.DataItem).Row["DivisionName"].ToString();//获得对应ID
            Repeater rptBranchs = (Repeater)e.Item.FindControl("rptBranchs");//找到要绑定数据的Repeater
            if (rptBranchs != null && DivisionName.Length > 0)
            {

                int typeid = 0;
                if (type == "User")
                {
                    typeid = UserID;
                }

                if (type == "Group")
                {
                    typeid = GroupID;
                }

                if (type == "Folder")
                {
                    typeid = FolderID;
                }

                if (type == "File")
                {
                    typeid = FileID;
                }
                if (typeid == 0)
                {
                    return;
                }

                DataTable dtBranchs = new DataTable();
                dtBranchs = company.GetCompanyOverviewByUser(3, typeid, type, "DivisionName='" + DivisionName + "'");
                dtBranchs.TableName = "Branch";

                if (dtBranchs == null || dtBranchs.Rows.Count < 1)
                {
                    rptBranchs.DataSource = null;
                    rptBranchs.DataBind();
                    return;
                }
                if (dtBranchs.Rows.Count == 1 && dtBranchs.Rows[0]["BranchName"].ToString() == string.Empty)
                {
                    rptBranchs.DataSource = null;
                    rptBranchs.DataBind();
                    return;
                }
                rptBranchs.DataSource = dtBranchs;
                rptBranchs.DataBind();
            }
        }
    }
}
