using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Workflow;
using LPWeb.Layouts.LPWeb.Common; 
using LPWeb.BLL;
using LPWeb.Common;
using System.Data;

public partial class LoanDetaiView : BasePage
{ 
    int iFileID = 0;
    int iContactID = 0;
    Loans loan = new Loans();
    LoginUser loginUser = new LoginUser();

    string sCloseDialogCodes = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        // CloseDialogCodes
        bool bIsValid = PageCommon.ValidateQueryString(this, "CloseDialogCodes", QueryStringType.String);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", "window.parent.location.href=window.parent.location.href");
        }
        this.sCloseDialogCodes = this.Request.QueryString["CloseDialogCodes"].ToString() + ";";
         
        #endregion

        string sFileID = this.Request.QueryString["FileID"];
        string sContactID = this.Request.QueryString["ContactID"];
        if (PageCommon.IsID(sFileID) == false)
        {
            iFileID = 0;
        }
        else
        {
            iFileID = int.Parse(sFileID);
        }

        if (PageCommon.IsID(sContactID) == false)
        {
            iContactID = 0;
            return;
        }
        else
        {
            iContactID = int.Parse(sContactID);
        }
        hfFileID.Value = iFileID.ToString();
        if (!IsPostBack)
        {
            FillLabels();
        }
    }

    private void FillLabels()
    {
        if (iFileID == 0)
        {
            return;
        }
        LPWeb.Model.Loans model = new LPWeb.Model.Loans();
        model = loan.GetModel(iFileID);
        if (model == null)
        {
            return;
        }
        if (model.LoanAmount.HasValue && model.LoanAmount.Value > 0)
        {
            txbAmount.Text = model.LoanAmount.Value.ToString();
        }
        else
        {
            txbAmount.Text = string.Empty;
        }

        if (model.Rate.HasValue && model.Rate.Value > 0)
        {
            txbInterestRate.Text = model.Rate.Value.ToString();
        }
        else
        {
            txbInterestRate.Text = string.Empty;
        }

        if (model.EstCloseDate.HasValue && model.EstCloseDate.Value > DateTime.Parse("1900-1-1"))
        {
            txbEstimatedDate.Text = model.EstCloseDate.Value.ToShortDateString();
        }
        else
        {
            txbEstimatedDate.Text = string.Empty;
        }
        if (model.Program.Length == 0)
        {
            ddlLoanProgram.Text = string.Empty;
        }
        else
        {
            ddlLoanProgram.Text = model.Program;
        }
        if (model.Purpose.Length == 0)
        {
            ddlPurpose.Text = string.Empty;
        }
        else
        {
            ddlPurpose.Text = model.Purpose.Trim();
        }

        if (model.LienPosition.Length == 0)
        {
            ddlLien.Text = string.Empty;
        }
        else
        {
            ddlLien.Text = model.LienPosition.Trim();
        }

        txbPropertyAddress.Text = model.PropertyAddr;
        txbCity.Text = model.PropertyCity;
        txbZip.Text = model.PropertyZip;
        if (model.PropertyState.Length > 0)
        {
            ddlState.Text = string.Empty;
        }
        else
        {
            ddlState.Text = model.PropertyState.Trim();
        }
        try
        {
            ddlRanking.Text = model.Ranking;
        }
        catch
        { }

        Contacts contact = new Contacts();
        try
        {
            ddlBorrower.Text = contact.GetBorrower(iFileID); 
        }
        catch
        { }

        try
        {
            ddlCoBorrower.Text = contact.GetCoBorrower(iFileID);
        }
        catch
        { }
        BindPoint(iFileID);
        try
        {
            Users user = new Users();

            ddlLoanOfficer.Text = user.GetLoanOfficer(iFileID);
        }
        catch
        { }

        if (model.Created.HasValue)
            lbCreatedOn.Text = model.Created.Value.ToShortDateString();
        if (model.CreatedBy.HasValue)
            lbCreatedBy.Text = GetUserName(model.CreatedBy.Value);

        if (model.Modifed.HasValue)
            lbCreatedOn.Text = model.Modifed.Value.ToShortDateString();
        if (model.ModifiedBy.HasValue)
            lbCreatedBy.Text = GetUserName(model.ModifiedBy.Value);

    }

    private void BindPoint(int FileID)
    {
        try
        {
            //btnExport.Enabled = false;
            PointFiles file = new PointFiles();
            LPWeb.Model.PointFiles fileModel = new LPWeb.Model.PointFiles();
            fileModel = file.GetModel(iFileID);

            if (fileModel == null)
            {
                return;
            }
            txbPointFileName.Text = fileModel.Name;

            PointFolders folder = new PointFolders();
            LPWeb.Model.PointFolders folderModel = new LPWeb.Model.PointFolders();
            folderModel = folder.GetModel(fileModel.FolderId);
            if (folderModel != null && folderModel.Name.Length > 0)
            {
                ddlPointFolder.Text = folderModel.Name;
            }
            if (txbPointFileName.Text.Length > 0 && ddlPointFolder.Text.Length > 0)
            {
                //btnExport.Enabled = true;
            }
        }
        catch
        { }
    }

    private string GetUserName(int UserID)
    {
        Users user = new Users();
        try
        {
            LPWeb.Model.Users model = user.GetModel(UserID);
            return model.LastName + ", " + model.FirstName;
        }
        catch
        {
            return string.Empty;
        }
    } 
}
