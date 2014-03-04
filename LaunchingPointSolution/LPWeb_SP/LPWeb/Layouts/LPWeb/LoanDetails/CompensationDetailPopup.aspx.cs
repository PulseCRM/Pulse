using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Common;
using LPWeb.BLL;
using System.Data;
using System.Collections.Generic;
using System.Linq;

public partial class CompensationDetailPopup : LayoutsPageBase
{
    int LoanID = 0;
    LoginUser CurrentUser = new LoginUser();
    protected void Page_Load(object sender, EventArgs e)
    {

        string strLoanID = Request.QueryString["LoanID"];  // file id
        if (string.IsNullOrEmpty(strLoanID))
        {
            PageCommon.WriteJsEnd(this, "Invalid LoanID=" + strLoanID, PageCommon.Js_RefreshSelf);
            return;
        }
        LoanID = Convert.ToInt32(strLoanID);
        if (LoanID <= 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid LoanID.", PageCommon.Js_RefreshSelf);
            return;
        }


        if (!IsPostBack)
        {
            BindData();
        }


    }

    public void BindData()
    {
        LPWeb.BLL.Loans bllLoans = new LPWeb.BLL.Loans();
        LPWeb.BLL.LoanTeam bllLoanTeam = new LPWeb.BLL.LoanTeam();
        LPWeb.BLL.Users bllUsers = new LPWeb.BLL.Users();
        LPWeb.Model.Loans loanInfo = bllLoans.GetModel(LoanID);

        DataTable LoanInfo_datatable = bllLoans.GetLoanInfo(LoanID);
        decimal loanAmount = 0;
        
        LoanPointFields LoanPointFieldsMgr = new LoanPointFields();
        //LPWeb.Model.LoanPointFields PointFieldInfo = LoanPointFieldsMgr.GetPointFieldInfo(LoanID, 21017);
        LPWeb.Model.LoanPointFields PointFieldInfo = LoanPointFieldsMgr.GetModel(LoanID, 21017);
        if (PointFieldInfo != null && !string.IsNullOrEmpty(PointFieldInfo.CurrentValue))
        {
            decimal.TryParse(PointFieldInfo.CurrentValue, out loanAmount);
        }
        if (loanAmount <= 0 && LoanInfo_datatable != null && LoanInfo_datatable.Rows.Count > 0)
        {
            loanAmount = LoanInfo_datatable.Rows[0]["LoanAmount"] == DBNull.Value ? 0 : (decimal)LoanInfo_datatable.Rows[0]["LoanAmount"];
        }
        this.labLoanamount.Text = loanAmount.ToString("n2");

        var list = new List<ViewCompDetail>();
        
        var Lo = new ViewCompDetail();
        Lo.Type = "Loan Officer";
        var LOUserId = bllLoanTeam.GetLoanOfficerID(LoanID);
        Lo.Name = bllLoanTeam.GetLoanOfficer(LoanID);
        Lo.Rate = GetUserCompRate(LOUserId);
        Lo.Amount = Lo.Rate * loanAmount / 100.00M;
        list.Add(Lo);


        loanInfo.BranchID = loanInfo.BranchID != null ? loanInfo.BranchID : 0;
        var branchM = new ViewCompDetail();
        branchM.Type = "Branch Manager";
        LPWeb.BLL.BranchManagers bllbranM = new LPWeb.BLL.BranchManagers();
        var branchUserId = 0;
        var branchObj = bllbranM.GetModelList("BranchId =" + loanInfo.BranchID).FirstOrDefault();

        if (branchObj != null)
        {
            branchUserId = branchObj.BranchMgrId;
        }

        LPWeb.Model.Users userBrM = bllUsers.GetModel(branchUserId);
        if (userBrM != null && userBrM.UserId == branchUserId)
        {
            branchM.Name = userBrM.LastName + "," + userBrM.FirstName;
            branchM.Rate = GetBranchMgrCompRate(LOUserId);
            branchM.Amount = branchM.Rate * loanAmount / 100.00M;
        }
        list.Add(branchM);


        loanInfo.DivisionID = loanInfo.DivisionID != null ? loanInfo.DivisionID : 0;
        var divisionM = new ViewCompDetail();
        divisionM.Type = "Division Manager";
        LPWeb.BLL.DivisionExecutives bllDivM = new LPWeb.BLL.DivisionExecutives();
        var DivMUserId = 0;
        var divobj = bllDivM.GetModelList("DivisionId =" + loanInfo.DivisionID).FirstOrDefault();
        if (divobj != null)
        {
            DivMUserId = divobj.ExecutiveId;
        }

        LPWeb.Model.Users userDivM = bllUsers.GetModel(DivMUserId);
        if (userDivM != null && userDivM.UserId == DivMUserId)
        {
            divisionM.Name = userDivM.LastName + "," + userDivM.FirstName;
            divisionM.Rate = GetDivisionMgrCompRate(LOUserId);
            divisionM.Amount = divisionM.Rate * loanAmount / 100.00M;
        }
        list.Add(divisionM);

        loanInfo.RegionID = loanInfo.RegionID != null ? loanInfo.RegionID : 0;
        var RegionM = new ViewCompDetail();
        RegionM.Type = "Region Manager";
        LPWeb.BLL.RegionExecutives bllRegionM = new LPWeb.BLL.RegionExecutives();
        var RegionMUserId = 0;
        var regionObj = bllRegionM.GetModelList("RegionId =" + loanInfo.RegionID).FirstOrDefault();

        if (regionObj != null)
        {
            RegionMUserId = regionObj.ExecutiveId;
        }

        LPWeb.Model.Users userRegionM = bllUsers.GetModel(RegionMUserId);
        if (userRegionM != null && userRegionM.UserId == RegionMUserId)
        {
            RegionM.Name = userRegionM.LastName + "," + userRegionM.FirstName;
            RegionM.Rate = GetRegionMgrCompRate(LOUserId);
            RegionM.Amount = RegionM.Rate * loanAmount / 100.00M;
        }
        list.Add(RegionM);


        gvCompDetail.DataSource = list;
        gvCompDetail.DataBind();

    }

    private decimal GetUserCompRate(int UserID)
    {
        LPWeb.BLL.Users bllUsers = new LPWeb.BLL.Users();

        LPWeb.Model.Users userInfo = bllUsers.GetModel(UserID);

        if (userInfo != null && userInfo.LOComp != null)
        {

            return userInfo.LOComp;
        }
        else
        {
            return 0.000M;
        }
    }

    private decimal GetBranchMgrCompRate(int UserID)
    {
        LPWeb.BLL.Users bllUsers = new LPWeb.BLL.Users();

        LPWeb.Model.Users userInfo = bllUsers.GetModel(UserID);

        if (userInfo != null && userInfo.BranchMgrComp != null)
        {

            return userInfo.BranchMgrComp;
        }
        else
        {
            return 0.000M;
        }
    }

    private decimal GetDivisionMgrCompRate(int UserID)
    {
        LPWeb.BLL.Users bllUsers = new LPWeb.BLL.Users();

        LPWeb.Model.Users userInfo = bllUsers.GetModel(UserID);

        if (userInfo != null && userInfo.DivisionMgrComp != null)
        {

            return userInfo.DivisionMgrComp;
        }
        else
        {
            return 0.000M;
        }
    }

    private decimal GetRegionMgrCompRate(int UserID)
    {
        LPWeb.BLL.Users bllUsers = new LPWeb.BLL.Users();

        LPWeb.Model.Users userInfo = bllUsers.GetModel(UserID);

        if (userInfo != null && userInfo.RegionMgrComp != null)
        {

            return userInfo.RegionMgrComp;
        }
        else
        {
            return 0.000M;
        }
    }

}

public class ViewCompDetail
{
    public string Type { get; set; }

    public string Name { get; set; }

    public decimal Rate { get; set; }

    public decimal Amount { get; set; }
    
}