using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class LoanDetails_AssignLoanTeamPopup : BasePage
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
        string sLastName1 = BorrowerInfo.Rows[0]["LastName"].ToString();

        string sBorrower = sLastName1 + ",  " + sFirstName;
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
            sWhere = " and (1=0)";
        }
        else
        {
            sWhere = "";

            // Branch
            if (this.Request.QueryString["Branch"] != null)
            {
                string sBranch = this.Request.QueryString["Branch"];

                //gdc 20120621 #1794
                sWhere += " And UserId IN ( SELECT u.UserId from Users as u ";
                sWhere += "     left outer join GroupUsers as c on u.UserId=c.UserID ";
                sWhere += "     left outer join Groups as d on c.GroupID=d.GroupId ";
                sWhere += "     left outer join Branches as e on d.BranchID=e.BranchId ";
                sWhere += "     where e.BranchId = " + sBranch;
                sWhere += "     ) ";
            }

            // Company
            if (this.Request.QueryString["UserRole"] != null)
            {
                string sUserRole = this.Request.QueryString["UserRole"];

                sWhere += " and RoleId=" + sUserRole;
            }

            // Last Name
            if (this.Request.QueryString["LastName"] != null)
            {
                string sLastName = this.Request.QueryString["LastName"];

                sWhere += " and LastName like '" + sLastName + "%'";
            }
        }

        #endregion

        #region 排序

        string sOrderByField = "FullName";
        if (this.Request.QueryString["OrderByField"] != null)
        {
            sOrderByField = this.Request.QueryString["OrderByField"];
        }

        string sOrderByType = "asc";  // default asc
        if (this.Request.QueryString["OrderByType"] != null)
        {
            sOrderByType = this.Request.QueryString["OrderByType"];
            if (sOrderByType != "asc" && sOrderByType != "desc")
            {
                sOrderByType = "asc";
            }
        }

        #endregion

        if (this.IsPostBack == false)
        {
            #region 加载Branch Filter
            string sSql2 = string.Empty;
            if (CurrUser.bIsCompanyExecutive)
                sSql2 = "Select BranchId, [Name] from Branches where [Enabled]=1 order by [Name]";
            else if (CurrUser.bIsRegionExecutive || CurrUser.bIsDivisionExecutive)
                sSql2 = string.Format("select a.BranchID, b.Name from dbo.lpfn_GetUserBranches_Executive({0}) as a inner join Branches as b on a.BranchID=b.BranchId where b.Enabled=1 order by b.Name", CurrUser.iUserID);
            else if (CurrUser.bIsBranchManager)
                sSql2 = string.Format("select a.BranchID, b.Name from dbo.lpfn_GetUserBranches_Branch_Manager({0}) as a inner join Branches as b on a.BranchID=b.BranchId where b.Enabled=1 order by b.Name", CurrUser.iUserID);
            else
                sSql2 = string.Format("select a.BranchID, b.Name from dbo.lpfn_GetUserBranches({0}) as a inner join Branches as b on a.BranchID=b.BranchId where b.Enabled=1 order by b.Name",CurrUser.iUserID);
            DataTable BranchList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql2);

            DataRow AllRow = BranchList.NewRow();
            AllRow["BranchId"] = 0;
            AllRow["Name"] = "All";

            BranchList.Rows.InsertAt(AllRow, 0);

            this.ddlBranch.DataSource = BranchList;
            this.ddlBranch.DataBind();

            #endregion

            #region 加载User Role Filter

            string sSql3 = "select RoleId, Name from Roles where Name in ('Branch Manager','Closer','Doc Prep', 'Jr Processor','Loan Officer','Loan Officer Assistant','Processor','Shipper','Underwriter') order by Name";
            DataTable UserRoleList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql3);

            DataTable LoanRoleList = UserRoleList.Copy();

            DataRow AllUserRoleRow = UserRoleList.NewRow();
            AllUserRoleRow["RoleId"] = 0;
            AllUserRoleRow["Name"] = "All";

            UserRoleList.Rows.InsertAt(AllUserRoleRow, 0);

            this.ddlUserRole.DataSource = UserRoleList;
            this.ddlUserRole.DataBind();

            #endregion

            #region 加载Loan Role Filter

            DataRow AllLoanRoleRow = LoanRoleList.NewRow();
            AllLoanRoleRow["RoleId"] = 0;
            AllLoanRoleRow["Name"] = "-- select --";

            LoanRoleList.Rows.InsertAt(AllLoanRoleRow, 0);

            this.ddlLoanRole.DataSource = LoanRoleList;
            this.ddlLoanRole.DataBind();

            #endregion

            #region 加载Contact List

            string sSql = "select * from ( "
                        + "select a.UserId,a.LastName,dbo.lpfn_GetUserName(a.UserId) as FullName,' ' as BranchId, ' ' as BranchName,a.RoleId,b.Name as UserRole " //gdc 20120621 #1794
                        + "from Users as a "
                        + "left outer join Roles as b on a.RoleId=b.RoleId "
                        //+ "left outer join GroupUsers as c on a.UserId=c.UserID "  //gdc 20120621 #1794
                        //+ "left outer join Groups as d on c.GroupID=d.GroupId "       //gdc 20120621 #1794
                        //+ "left outer join Branches as e on d.BranchID=e.BranchId "   //gdc 20120621 #1794
                        + " where a.UserEnabled=1 and a.UserId not in (select UserID from LoanTeam where FileId=" + iFileID + ")) as m "
                        + "where 1=1 " + sWhere;

            #region get row count

            int iRowCount = this.GetLoanTeamListRowCount(sSql, sWhere);
            this.AspNetPager1.RecordCount = iRowCount;

            #endregion

            int iPageSize = this.AspNetPager1.PageSize;
            int iPageIndex = 1;
            if (this.Request.QueryString["PageIndex"] != null)
            {
                iPageIndex = Convert.ToInt32(this.Request.QueryString["PageIndex"]);
            }

            DataTable LoanTeamList = this.GetLoanTeamList(sSql, iRowCount, iPageSize, iPageIndex, sOrderByField, sOrderByType);

            #region UserBranchInfo gdc 20120621 #1794
            // Get user branch info
            LPWeb.BLL.Users UsersManager = new LPWeb.BLL.Users();
            DataTable dtUserBranch = new DataTable();
            dtUserBranch = UsersManager.GetUserBranchInfo();

            if (null != dtUserBranch)
            {
                foreach (DataRow item in LoanTeamList.Rows)
                {
                    var nUserID = item["UserId"].ToString();
                    var sUserBranch = item["BranchName"].ToString();

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
                    //int nDisLen = 30;
                    //if (sbBName.Length > nDisLen)
                    //{
                    //    lblBranch.ToolTip = sbBName.ToString();
                    //    lblBranch.Text = sbBName.ToString().Substring(0, nDisLen) + "...";
                    //}
                    //else
                    //    lblBranch.Text = sbBName.ToString();

                    item["BranchName"] = sbBName.ToString();
                }
            } 
            #endregion

            #region set EmptyDataText

            if (this.Request.QueryString["search"] == null)
            {
                this.gridLoanTeamList.EmptyDataText = "Please enter conditions and click Display.";
            }
            else
            {
                if (iRowCount == 0)
                {
                    this.gridLoanTeamList.EmptyDataText = "There is no loan team by the conditions.";
                }
            }

            #endregion

            this.gridLoanTeamList.DataSource = LoanTeamList;
            this.gridLoanTeamList.DataBind();

            #endregion
        }
    }

    private int GetLoanTeamListRowCount(string sSql, string sWhere) 
    {
        // row count
        int iRowCount = LPWeb.DAL.DbHelperSQL.Count("(" + sSql + ") as u", "");

        return iRowCount;
    }

    private DataTable GetLoanTeamList(string sSql, int iRowCount, int iPageSize, int iPageIndex, string sOrderByField, string sAscOrDesc)
    {
        #region Calc. StartIndex and EndIndex

        int iStartIndex = PageCommon.CalcStartIndex(iPageIndex, iPageSize);
        int iEndIndex = PageCommon.CalcEndIndex(iStartIndex, iPageSize, iRowCount);

        #endregion

        SqlCommand SqlCmd = new SqlCommand("lpsp_ExecSqlByPager");
        SqlCmd.CommandType = CommandType.StoredProcedure;

        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@OrderByField", SqlDbType.NVarChar, sOrderByField);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@AscOrDesc", SqlDbType.NVarChar, sAscOrDesc);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Fields", SqlDbType.NVarChar, "*");
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@DbTable", SqlDbType.NVarChar, "(" + sSql + ") as t");
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Where", SqlDbType.NVarChar, "");
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@StartIndex", SqlDbType.Int, iStartIndex);
        LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@EndIndex", SqlDbType.Int, iEndIndex);

        DataTable x = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(SqlCmd);

        return x;
    }
}
