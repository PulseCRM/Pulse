using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using System.Text;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;

public partial class LoanConditions : BasePage
{
    private int FileID = 0;
    private bool IsExternalViewing = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["FileID"]) && PageCommon.IsID(Request.QueryString["FileID"]))
        {
            FileID = Convert.ToInt32(Request.QueryString["FileID"]);
        }
        else
        {
            PageCommon.AlertMsg(this.Page, "Missing required query string.");
            Response.End();
        }

        try
        {
            IsExternalViewing = CurrUser.userRole.ConditionRights != null ? CurrUser.userRole.ConditionRights.Contains("6") : false;

            //CR65 Mark as Received
            //this.linkMarkReceived.Enabled = CurrUser.UpdateCondition != null ? CurrUser.UpdateCondition : false;
            if (CurrUser.UpdateCondition != true)
            {
                this.btnMarkReceived.Attributes.Remove("href");
            }

            if (this.IsExternalViewing == false)
            {
                this.linkEnableExV.Enabled = false;
                this.linkDisableExV.Enabled = false;
            }

        }
        catch { }
        if (!IsPostBack)
        {
            BindGvList();

            //gdc CR40
            #region ddlType

            string sSql = " select distinct CondType FROM [LoanConditions] where CondType is not null and CondType <>'' order by CondType ";

            DataTable ListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

            ddlType.DataTextField = "CondType";
            ddlType.DataValueField = "CondType";

            ddlType.DataSource = ListData;
            ddlType.DataBind();
            
            ddlType.Items.Insert(0, new ListItem() { Text = "All Types", Value = "" });

            #endregion
        }
    }

    /// <summary>
    /// list bind
    /// </summary>
    public void BindGvList()
    {
        string strWhere = " FileId = " + FileID;

        #region 条件
        //status
        switch (ddlAllStatuses.SelectedValue.Trim())
        {
            case "":
                break;
            case "Pending":
                strWhere += " and (Status is null or Status ='') ";
                break;
            case "Received":
                strWhere += " and Status ='Received' ";
                break;
            case "Submitted":
                strWhere += " and Status ='Submitted' ";
                break;
            case "Cleared":
                strWhere += " and Status ='Cleared' ";
                break;
        } 

        //due dates
        switch (ddlAllDueDates.SelectedValue)
        {
            case "0":
                break;
            case "1":
                strWhere += "  and datediff(d,getdate() ,due) < 0";
                break;
            case "2":
                strWhere += "  and datediff(d,getdate() ,due)<= 0";
                break;
            case "3":
                strWhere += " and datediff(d,getdate() ,due) = 0 ";
                break;
            case "4":
                strWhere += " and datediff(d,getdate() ,due)= 1 ";
                break;
            case "5":
                strWhere += " and datediff(d,getdate() ,due) <= 7 and datediff(d,getdate() ,due) >= 0 ";
                break;
        }

        //gdc CR40
        if (!string.IsNullOrEmpty(ddlType.SelectedValue.Trim()))
        {
            strWhere += " and CondType= '" + ddlType.SelectedValue.Trim() + "'";
        }

        #endregion


        LPWeb.BLL.LoanConditions lcon = new LPWeb.BLL.LoanConditions();
        
        int count = 0;
        gridList.DataSource = lcon.GetList(AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, strWhere, out count);
        gridList.DataBind();

        AspNetPager1.RecordCount = count;


    }

    protected void btnFilter_OnClick(object sender, EventArgs e)
    {
        AspNetPager1.CurrentPageIndex = 1;
        BindGvList();
    }

    protected void linkEnableExV_OnClick(object sender, EventArgs e)
    {
        LPWeb.BLL.LoanConditions lcon = new LPWeb.BLL.LoanConditions();
        foreach (GridViewRow row in gridList.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("ckbSelect");

                if (chkSelect != null && chkSelect.Checked && !string.IsNullOrEmpty(chkSelect.ToolTip))
                {
                    var ID = Convert.ToInt32(chkSelect.ToolTip);


                    lcon.UpdateExternalViewing(ID, true);
                }

            }

        }

        BindGvList();
    }

    protected void linkDisableExV_OnClick(object sender, EventArgs e)
    {
        LPWeb.BLL.LoanConditions lcon = new LPWeb.BLL.LoanConditions();
        foreach (GridViewRow row in gridList.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("ckbSelect");

                if (chkSelect != null && chkSelect.Checked && !string.IsNullOrEmpty(chkSelect.ToolTip))
                {
                    var ID = Convert.ToInt32(chkSelect.ToolTip);


                    lcon.UpdateExternalViewing(ID, false);
                }

            }

        }

        BindGvList();
    }


    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        BindGvList();
    }

    protected void gridList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var dr = (DataRowView)e.Row.DataItem;
            //CheckBox chk = (CheckBox)e.Row.FindControl("cbExternalViewing");
            //chk.Checked = dr["ExternalViewing"] != DBNull.Value ? Convert.ToBoolean(dr["ExternalViewing"]) : false;
            //chk.Enabled = IsExternalViewing;
            //chk.ToolTip = dr["LoanCondId"].ToString();

            CheckBox chkSelect = (CheckBox)e.Row.FindControl("ckbSelect");
            chkSelect.ToolTip = dr["LoanCondId"].ToString();
            //chkSelect.Enabled = IsExternalViewing;


            Literal condName = (Literal)e.Row.FindControl("litcondName");
            var name = dr["CondName"].ToString();
            //condName.Text = name.Length > 80 ? name.Substring(0, 80) : name;
            condName.Text = name;

            Image img = (Image)e.Row.FindControl("imgicon");
            string imgUrl = "../images/condition/condIon1.gif";

            string status = dr["Status"] != DBNull.Value ? dr["Status"].ToString() : "";
            DateTime due = dr["Due"] != DBNull.Value ? Convert.ToDateTime(dr["Due"]) : DateTime.MaxValue;

            if (status == "Cleared")
            {
                imgUrl = "../images/condition/condIon6.gif";
            }
            else if (status == "Submitted")
            {
                imgUrl = "../images/condition/condIon5.gif";
            }
            else if (status == "Received")
            {
                imgUrl = "../images/condition/condIon4.gif";
            }
            else if (status == "" && (due.Date - DateTime.Now.Date).Days > TaskYellowDays)
            {
                imgUrl = "../images/condition/condIon3.gif";
            }
            else if (status == "" && (due.Date - DateTime.Now.Date).Days <= TaskYellowDays && (due.Date - DateTime.Now.Date).Days > TaskRedDays)
            {
                imgUrl = "../images/condition/condIon2.gif";
            }
            else if (status == "" && (due.Date - DateTime.Now.Date).Days <= TaskRedDays)
            {
                imgUrl = "../images/condition/condIon1.gif";
            }

            img.ImageUrl = imgUrl;
            img.ToolTip = name;

        }
    }


    protected void cbExternalViewing_OnCheckedChanged(object sender, EventArgs e)
    {
        var cb = (CheckBox)sender;

        if (cb != null && !string.IsNullOrEmpty(cb.ToolTip))
        {
            var ID = Convert.ToInt32(cb.ToolTip);

            LPWeb.BLL.LoanConditions lcon = new LPWeb.BLL.LoanConditions();
            lcon.UpdateExternalViewing(ID, cb.Checked);

            BindGvList();
        }
    }

    private int taskRedDays = -1;
    private int TaskRedDays
    {
        get
        {
            if (taskRedDays != -1)
            {
                return taskRedDays;
            }
            else
            {
                GetTaskDays();
                return taskRedDays;
            }

        }
    }

    private int taskYellowDays = -1;
    private int TaskYellowDays
    {
        get
        {
            if (taskYellowDays != -1)
            {
                return taskYellowDays;
            }
            else
            {
                GetTaskDays();
                return taskYellowDays;
            }

        }
    }

    private void GetTaskDays()
    {
        LPWeb.BLL.Company_Alerts bll = new Company_Alerts();
        LPWeb.Model.Company_Alerts model = bll.GetModel();

        taskRedDays = model.TaskRedDays.Value;
        taskYellowDays = model.TaskYellowDays.Value;

    }

}

