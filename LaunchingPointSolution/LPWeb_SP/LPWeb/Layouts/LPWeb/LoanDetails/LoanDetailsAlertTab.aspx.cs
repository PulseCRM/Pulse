using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.Common;

public partial class LoanDetails_LoanDetailsAlertTab : BasePage
{
    int iLoanID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 检查必要参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
        if (bIsValid == false)
        {
            try
            {
                this.Response.End();
            }
            catch
            {

            }
        }

        this.iLoanID = Convert.ToInt32(this.Request.QueryString["LoanID"]);

        #endregion

        LoginUser CurrentUser = new LoginUser();
        this.hdnLoginUserID.Value = CurrentUser.iUserID.ToString();

        string sRef = this.Request.QueryString["ref"] == null ? "" : this.Request.QueryString["ref"].ToString();
        this.hdnParentForm.Value = sRef;

        #region 初始化Alert列表

        this.AlertSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;

        string sWhere = " and FileId=" + this.iLoanID;

        #region Filter

        bool bSetFilter = false;
        if (this.Request.QueryString["Filter"] != null)
        {
            string sFilter = this.Request.QueryString["Filter"].ToString();
            if (sFilter != "All" && sFilter != "Pending" && sFilter != "Acknowledged" && sFilter != "Dismissed" && sFilter != "Accepted" && sFilter != "Declined")
            {
                sFilter = "All";
            }

            if (sFilter == "All")
            {

            }
            else if (sFilter == "Pending")
            {
                sWhere += " and (a.Status is null or a.Status='Acknowledged')";
            }
            if (sFilter == "Acknowledged")
            {
                sWhere += " and a.Status='Acknowledged'";
            }
            if (sFilter == "Dismissed")
            {
                sWhere += " and a.Status='Dismissed'";
            }
            if (sFilter == "Accepted")
            {
                sWhere += " and a.Status='Accepted'";
            }
            if (sFilter == "Declined")
            {
                sWhere += " and a.Status='Declined'";
            }

            bSetFilter = true;
        }

        if (bSetFilter == true)
        {
            int iRowCount = LPWeb.DAL.DbHelperSQL.Count("LoanAlerts as a", sWhere);
            if (iRowCount == 0)
            {
                this.gridAlertList.EmptyDataText = "There is no alert.";
            }
        }

        #endregion

        string sSql = "select dbo.lpfn_GetAlertIconFileName(a.LoanAlertId) as AlertIconFileName, a.*, dbo.lpfn_GetUserName(a.AcknowledgedBy) as AcknowledgedByName, dbo.lpfn_GetUserName(a.AcceptedBy) as AcceptedByName, dbo.lpfn_GetUserName(a.DeclinedBy) as DeclinedByName, dbo.lpfn_GetUserName(a.DismissedBy) as DismissedByName "
                    + "from LoanAlerts as a  "
            //+ "(select UserId, LastName +', '+ FirstName as FullName from Users) as b on a.AcknowledgedBy = b.UserId "
            //+ "left outer join "
            //+ "(select UserId, LastName +', '+ FirstName as FullName from Users) as c on a.AcceptedBy = c.UserId "
            //+ "left outer join "
            //+ "(select UserId, LastName +', '+ FirstName as FullName from Users) as d on a.DeclinedBy = d.UserId "
            //+ "left outer join "
            //+ "(select UserId, LastName +', '+ FirstName as FullName from Users) as e on a.DismissedBy = e.UserId "
                    + "where 1=1 and a.AlertType='Rule Alert' " + sWhere;

        this.AlertSqlDataSource.SelectCommand = sSql;
        this.gridAlertList.DataBind();

        #endregion
    }

    public string EncodeText(string sText)
    {
        if (sText == string.Empty)
        {
            return string.Empty;
        }
        return Encrypter.Base64Encode(sText);
    }

    public string FormatDateTime(string sDate)
    {
        if (sDate == string.Empty)
        {
            return string.Empty;
        }

        return Convert.ToDateTime(sDate).ToString("MM/dd/yyyy");
    }

    public string PrintImage(string sAlertIconFileName)
    {
        if (sAlertIconFileName == string.Empty)
        {
            return string.Empty;
        }
        else
        {
            return "<img alt='' src='../images/alert/" + sAlertIconFileName + "' />";
        }
    }
}
