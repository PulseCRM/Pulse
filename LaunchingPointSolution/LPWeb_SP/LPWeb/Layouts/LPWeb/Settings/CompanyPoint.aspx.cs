using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.Model;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;

public partial class Settings_CompanyPoint : BasePage
{
    /// <summary>
    /// 
    /// </summary>
    private Company_Point modCompanyPoint = new Company_Point();
    private Company_General modCompayGeneral = new Company_General();
    LPWeb.BLL.Company_Point bllCompanyPoint = new LPWeb.BLL.Company_Point();
    LPWeb.BLL.Company_General bllCompanyGeneral = new LPWeb.BLL.Company_General();
    private bool isNew = false;
    private bool isGeneralNew = false;
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
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

        try
        {
            if (!Page.IsPostBack)
            {
                if (Request.UrlReferrer != null)
                {
                    this.btnCancel.PostBackUrl = Request.UrlReferrer.ToString();
                }

                GetInitData();
                if (modCompanyPoint != null)
                {
                    SetDataToUI();
                }
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

    }
    /// <summary>
    /// Gets the data from UI.
    /// </summary>
    private void GetDataFromUI()
    {
        //get existing record
        GetInitData();
        if (modCompanyPoint == null)
        {
            modCompanyPoint = new Company_Point();
            isNew = true;
        }

        if (modCompayGeneral == null)
        {
            modCompayGeneral = new Company_General();
            isGeneralNew = true;
        }


        string iniPath = txtWinpointPath.Text;
        if (!string.IsNullOrEmpty(iniPath))
        {
            modCompanyPoint.WinpointIniPath = iniPath.GetFilePath();
        }

        //string fieldIdMappingFile = txtPointFieldIDMappingFile.Text;
        //if (!string.IsNullOrEmpty(fieldIdMappingFile))
        //{
        //    modCompanyPoint.PointFieldIDMappingFile = fieldIdMappingFile.GetFilePath();
        //}

        string cardexFile = txtCardexFile.Text;
        if (!string.IsNullOrEmpty(cardexFile))
        {
            modCompanyPoint.CardexFile = cardexFile.GetFilePath();
        }

        string impInterval = ddlPointImportInterval.SelectedValue;
        if (!string.IsNullOrEmpty(impInterval))
        {
            modCompanyPoint.PointImportIntervalMinutes = impInterval.Parse<int>();
        }

        string masterSource = ddlMasterSource.SelectedValue.Trim();
        if (!string.IsNullOrEmpty(masterSource))
        {
            modCompanyPoint.MasterSource = masterSource;
        }
        else
        {
            modCompanyPoint.MasterSource = "Point";
        }

        modCompayGeneral.ActiveLoanWorkflow = ckActiveLoanWorkflow.Checked;
        modCompanyPoint.Auto_ConvertLead = ckAutoConvertLead.Checked;

        //CR60 ADD
        modCompanyPoint.Enable_MultiBranchFolders = ckEnableMultibranchfolders.Checked;
    }

    /// <summary>
    /// Gets the init data.
    /// </summary>
    private void GetInitData()
    {
        try
        {
            modCompanyPoint = bllCompanyPoint.GetModel();
            modCompayGeneral = bllCompanyGeneral.GetModel();
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }
    }

    /// <summary>
    /// Sets the data to UI.
    /// </summary>
    private void SetDataToUI()
    {
        if (modCompanyPoint == null)
        {
            return;
        }

        txtWinpointPath.Text = modCompanyPoint.WinpointIniPath;
        //txtPointFieldIDMappingFile.Text = modCompanyPoint.PointFieldIDMappingFile;
        txtCardexFile.Text = modCompanyPoint.CardexFile;
        
        try
        {
            //selected value may be not exists
            ddlPointImportInterval.SelectedValue = modCompanyPoint.PointImportIntervalMinutes.ToString();
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }

        ddlMasterSource.SelectedValue = modCompanyPoint.MasterSource == "" ? "Point" : modCompanyPoint.MasterSource;

        ckActiveLoanWorkflow.Checked = modCompayGeneral.ActiveLoanWorkflow;
        ckAutoConvertLead.Checked = modCompanyPoint.Auto_ConvertLead.GetValueOrDefault(false);

        //CR60 ADD
        ckEnableMultibranchfolders.Checked = modCompanyPoint.Enable_MultiBranchFolders; 

    }
    /// <summary>
    /// Saves this instance.
    /// </summary>
    /// <returns></returns>
    private bool Save()
    {
        bool status = false;
        try
        {
            if (isNew)
            {
                bllCompanyPoint.Add(modCompanyPoint);
            }
            else
            {
                bllCompanyPoint.Update(modCompanyPoint);
            }
            //status = true;

            if (isGeneralNew)
            {
                bllCompanyGeneral.Add(modCompayGeneral);
            }
            else
            {
                bllCompanyGeneral.Update(modCompayGeneral);
            }
            status = true;
        }
        catch (Exception exception)
        {
            status = false;
            LPLog.LogMessage(exception.Message);
        }
        return status;
    }

    /// <summary>
    /// Handles the Click event of the btnSave control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        GetDataFromUI();
        if (Save() == true)
        {
            //display successful message
            PageCommon.WriteJsEnd(this, "Saved successfully.", PageCommon.Js_RefreshSelf);

        }
        else
        {
            //display faild message 
            PageCommon.WriteJsEnd(this, "Failed to save the record.", PageCommon.Js_RefreshSelf);
        }
        SetDataToUI();
    }

    /// <summary>
    /// Handles the Click event of the btnCancel control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            ImportCardexRequest req = new ImportCardexRequest();

            req.CardexFile = string.Empty;

            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req.hdr.UserId = 5;//todo:check dummy data

            ImportCardexResponse respone = null;
            try
            {
                respone = service.ImportCardex(req);

                if (respone.hdr.Successful)
                {
                    PageCommon.WriteJsEnd(this, "Imported Cardex successfully.", PageCommon.Js_RefreshSelf);
                }
                else
                {
                    PageCommon.WriteJsEnd(this, "Failed to import Cardex, reason:"+respone.hdr.StatusInfo, PageCommon.Js_RefreshSelf);
                }
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
                PageCommon.WriteJsEnd(this, "Failed to import Cardex, reason:"+exception.Message, PageCommon.Js_RefreshSelf);
            }
        }
    }
}