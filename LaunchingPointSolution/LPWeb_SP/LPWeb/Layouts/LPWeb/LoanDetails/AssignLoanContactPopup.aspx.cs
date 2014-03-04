using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Data;

public partial class LoanDetails_AssignLoanContactPopup : BasePage
{
    int iFileID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        if (this.Request.QueryString["CloseDialogCodes"] == null)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "try{window.parent.location.href=window.parent.location.href}catch{}");
        }

        string sCloseDialogCodes = this.Request.QueryString["CloseDialogCodes"].ToString() + ";";

        bool bIsValid = PageCommon.ValidateQueryString(this, "FileID", QueryStringType.ID);

        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", sCloseDialogCodes);
        }

        string sFileID = this.Request.QueryString["FileID"].ToString();
        this.iFileID = Convert.ToInt32(sFileID);

        #endregion

        #region 获取Borrower和Property信息

        #region Property

        LPWeb.BLL.Loans LoanManager = new LPWeb.BLL.Loans();
        DataTable LoanInfo = LoanManager.GetLoanInfo(this.iFileID);
        if (LoanInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid query string.", sCloseDialogCodes);
        }
        string sPropertyAddress = LoanInfo.Rows[0]["PropertyAddr"].ToString();
        string sPropertyCity = LoanInfo.Rows[0]["PropertyCity"].ToString();
        string sPropertyState = LoanInfo.Rows[0]["PropertyState"].ToString();
        string sPropertyZip = LoanInfo.Rows[0]["PropertyZip"].ToString();

        string sProperty = sPropertyAddress + ", " + sPropertyCity + ", " + sPropertyState + " " + sPropertyZip;

        #endregion

        #region Borrower

        DataTable BorrowerInfo = LoanManager.GetBorrowerInfo(this.iFileID);
        if (BorrowerInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "There is no Borrower in this loan.", sCloseDialogCodes);
        }
        string sFirstName = BorrowerInfo.Rows[0]["FirstName"].ToString();
        string sMiddleName = BorrowerInfo.Rows[0]["MiddleName"].ToString();
        string sLastName = BorrowerInfo.Rows[0]["LastName"].ToString();

        string sBorrower = sLastName + ",  " + sFirstName;
        if (sMiddleName != string.Empty)
        {
            sBorrower += " " + sMiddleName;
        }

        this.lbBorrower.Text = sBorrower;
        this.lbProperty.Text = sProperty;
        
        #endregion

        #endregion

        #region 查询条件

        string sWhere = string.Empty;

        if (this.Request.QueryString["search"] == null)    // 未设置查询条件
        {
            sWhere = " (1=0)";
        }
        else
        {
            //sWhere = " (FileId <> " + this.iFileID + ")";
            sWhere = " (1=1) ";
            // Service Type
            if (this.Request.QueryString["ServiceType"] != null)
            {
                string sServiceType = this.Request.QueryString["ServiceType"];

                sWhere += "  and ServiceTypes='" + sServiceType + "'";
            }

            // Company
            if (this.Request.QueryString["Company"] != null)
            {
                string sCompany = this.Request.QueryString["Company"];

                sWhere += " and CompanyName like '" + sCompany + "%'";
            }

            // Zip
            if (this.Request.QueryString["Zip"] != null)
            {
                string sZip = this.Request.QueryString["Zip"];

                sWhere += " and MailingZip like '" + sZip + "%'";
            }
        }

        #endregion

        #region 排序

        string sOrderByField = "ContactsName";
        if (this.Request.QueryString["OrderByField"] != null)
        {
            sOrderByField = this.Request.QueryString["OrderByField"];
        }

        int iOrderByType = 0;  // default asc
        if (this.Request.QueryString["OrderByType"] != null)
        {
            string sOrderByType = this.Request.QueryString["OrderByType"];
            if (sOrderByType != "0")
            {
                sOrderByType = "1";
            }

            iOrderByType = Convert.ToInt32(sOrderByType);
        }

        #endregion

        if(this.IsPostBack == false)
        {
            #region 加载Service Type Filter

            string sSql2 = "select * from ServiceTypes where Enabled=1 order by Name";
            DataTable ServiceTypeList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql2);

            DataRow AllRow = ServiceTypeList.NewRow();
            AllRow["ServiceTypeId"] = 0;
            AllRow["Name"] = "All";
            AllRow["Enabled"] = true;

            ServiceTypeList.Rows.InsertAt(AllRow, 0);

            this.ddlServiceType.DataSource = ServiceTypeList;
            this.ddlServiceType.DataBind();

            #endregion

            #region 加载Contact Role Filter

            string sSql3 = "select * from ContactRoles where Enabled=1 and Name <> 'Borrower' and Name <> 'CoBorrower' order by Name";
            DataTable ContactRoleList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql3);

            DataRow EmptyRow = ContactRoleList.NewRow();
            EmptyRow["ContactRoleId"] = 0;
            EmptyRow["Name"] = "-- select --";
            EmptyRow["Enabled"] = true;

            ContactRoleList.Rows.InsertAt(EmptyRow, 0);

            this.ddlContactRole.DataSource = ContactRoleList;
            this.ddlContactRole.DataBind();

            #endregion

            #region 加载Contact List

            int iPageSize = this.AspNetPager1.PageSize;
            int iPageIndex = 1;
            if (this.Request.QueryString["PageIndex"] != null)
            {
                iPageIndex = Convert.ToInt32(this.Request.QueryString["PageIndex"]);
            }

            int iRowCount = 0;

            LPWeb.BLL.LoanContacts contacts = new LPWeb.BLL.LoanContacts();
            DataSet ds = contacts.GetDistinctLoanContactsForReassign(iPageSize, iPageIndex, sWhere, out iRowCount, sOrderByField, iOrderByType);

            this.AspNetPager1.RecordCount = iRowCount;

            #region set EmptyDataText

            if (this.Request.QueryString["search"] == null)
            {
                this.gridLoanContactList.EmptyDataText = "Please enter conditions and click Display.";
            }
            else
            {
                if (iRowCount == 0)
                {
                    this.gridLoanContactList.EmptyDataText = "There is no loan contact by the conditions.";
                }
            }

            #endregion

            this.gridLoanContactList.DataSource = ds;
            this.gridLoanContactList.DataBind();

            

            #endregion
        }
    }
}
