using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;
using org.in2bits.MyXls;
using System.Data;
using System.IO;

public partial class Settings_CompanyLoanPrograms : BasePage
{
    Company_Loan_Programs bllLoanProgram = new Company_Loan_Programs();
    Company_Point bllCompanyPoint = new Company_Point();
    private bool isReset = false;
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

        if (!Page.IsPostBack)
        {
            //if (Request.UrlReferrer != null)
            //{
            //    this.btnCancel.PostBackUrl = Request.UrlReferrer.ToString();
            //}

            txtLoanProgram.Attributes.Add("cid", "txtLoanProgram");

            LPWeb.Model.Company_Point modelPoint = bllCompanyPoint.GetModel();

            if (modelPoint.MasterSource.ToLower() == "DataTrac".ToLower())
            {
                btImport.Enabled = true;
            }
            else
            {
                btImport.Enabled = false;
            }

            BindInvestors();
            BindPrograms();
            BindIndexesList();

            FillDataGrid(string.Empty);
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if(!Page.IsValid)
        {
            return;
        }

        var modLoanProgram = new LPWeb.Model.Company_Loan_Programs();
        string loanProgram = this.txtLoanProgram.Text.Trim();
        bool IsARM = this.cbARM.Checked;
        var status = false;

        if (!string.IsNullOrEmpty(loanProgram))
        {
            modLoanProgram.LoanProgram = loanProgram;
            modLoanProgram.IsARM = IsARM;
            try
            {
                bllLoanProgram.Add(modLoanProgram);
                status = true;
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            if (status == true)
            {
                //reload the grid data
                FillDataGrid(string.Empty);
                //todo:display successfuly message
                PageCommon.WriteJsEnd(this, "Loan Program added successfully.", PageCommon.Js_RefreshSelf);
            }
            else
            {
                //todo:display faild message
                PageCommon.WriteJsEnd(this, "Failed to add the Loan  Program.", PageCommon.Js_RefreshSelf);
            }
        }
        else
        {
            //todo:display the field can not be empty
            PageCommon.WriteJsEnd(this, "Loan Program cannot be empty.", PageCommon.Js_RefreshSelf);
        }

    }

    protected void btImportARM_Click(object sender, EventArgs e)
    {

        try
        {
            if (Request.QueryString["upfile"] == null)
            {
                Write(0, "");
                return;
            }

            if (!fileUpARM.HasFile)
            {
                Write(0, "no file");
                return;
            }

            DataTable dtExcelData = new DataTable();

            #region 上传文件数据 读取
            string FilePath = this.fileUpARM.FileName;
            //string strUploadUrl = Server.MapPath("./");//此"."可以换成项目文件里的其它文件夹名称
            string strUploadUrl = Server.MapPath("~/");//此"."可以换成项目文件里的其它文件夹名称
            strUploadUrl = strUploadUrl.Replace("Settings\\", "");

            #region 上传文件到临时文件夹
            string strUploadFolder = @"\UploadFiles\\Temp\\";
            // 临时文件夹
            //string sTempUploadFold = strUploadUrl + strUploadFolder;  // this.MapPath("~/UploadFiles/Temp/");
            string sTempUploadFold = strUploadUrl;  
            string sFileExt1 = Path.GetExtension(this.fileUpARM.FileName).ToLower();
            string sTempFileName1 = Guid.NewGuid().ToString() + sFileExt1;

            // 临时文件路径
            string sTempFilePath1 = Path.Combine(sTempUploadFold, sTempFileName1); // "D:\\Test\\" + FilePath;// 

            try
            {
                // 文件上传到临时文件夹
                this.fileUpARM.SaveAs(sTempFilePath1);
            }
            catch (Exception ex)
            {
                string exmsg = string.Format("Failed to upload the file due to insufficient privilege: {0}", ex.Message);
                PageCommon.WriteJsEnd(this, exmsg, PageCommon.Js_RefreshSelf);
            }

            #endregion
            try
            {

                PageCommon commonMgr = new PageCommon();
                if (sTempFilePath1.Substring(sTempFilePath1.LastIndexOf(".")).ToLower() == ".xlsx")
                {
                    dtExcelData = commonMgr.GetTableFromXlsx(sTempFilePath1);
                }
                else if (sTempFilePath1.Substring(sTempFilePath1.LastIndexOf(".")).ToLower() == ".xls")
                {
                    dtExcelData = commonMgr.GetTableFromXls(sTempFilePath1);
                }
                else if (sTempFilePath1.Substring(sTempFilePath1.LastIndexOf(".")).ToLower() == ".csv")
                {
                    dtExcelData = commonMgr.GetTableFromCsv(sTempFilePath1);
                }

            }
            catch (Exception ex)
            {
                Write(0, "Import Error. Please make sure row 1 of the file has data.");
                return;
            }
            finally
            {
                if (File.Exists(sTempFilePath1))
                {
                    File.Delete(sTempFilePath1);
                }
            }
            #endregion

            if (dtExcelData == null || dtExcelData.Rows.Count == 0)
            {
                Write(0, "No Data");
                return;
            }

            List<LPWeb.Model.Company_LoanProgramDetails> detailList = new List<LPWeb.Model.Company_LoanProgramDetails>();
            #region Check Data

            if (dtExcelData.Columns.Count < 7)
            {
                Write(0, "Columns err");
                return;
            }
            int rowNum = 2;
            bool isErr = false;
            foreach (DataRow dr in dtExcelData.Rows)
            {
                var lpDetail = new LPWeb.Model.Company_LoanProgramDetails();

                lpDetail.Enabled = true;

                if (dr["Investor"] != DBNull.Value)
                {
                    lpDetail.Investor = dr["Investor"].ToString();
                }

                if (dr["Program"] != DBNull.Value)
                {
                    lpDetail.Program = dr["Program"].ToString();
                }

                if (dr["Index"] != DBNull.Value)
                {
                    lpDetail.IndexType = dr["Index"].ToString();
                }

                if (dr["Margin"] != DBNull.Value)
                {
                    try
                    {
                        lpDetail.Margin = Convert.ToDecimal(ReplaceEToD(dr["Margin"]));
                    }
                    catch
                    {
                        Write(0, "The following rows/Column contain invalid data:Row <" + rowNum + "> Column <Margin> must be decimal.");
                        isErr = true;
                        break;
                    }
                }

                if (dr["1st Adj"] != DBNull.Value)
                {
                    try
                    {
                        lpDetail.FirstAdj = Convert.ToDecimal(ReplaceEToD(dr["1st Adj"]));
                    }
                    catch
                    {
                        Write(0, "The following rows/Column contain invalid data:Row <" + rowNum + "> Column <FirstAdj> must be decimal.");
                        isErr = true;
                        break;
                    }
                }

                if (dr["Sub Adj"] != DBNull.Value)
                {
                    try
                    {
                        lpDetail.SubAdj = Convert.ToDecimal(ReplaceEToD(dr["Sub Adj"]));
                    }
                    catch
                    {
                        Write(0, "The following rows/Column contain invalid data:Row <" + rowNum + "> Column <Sub Adj> must be decimal.");
                        isErr = true;
                        break;
                    }
                }

                if (dr["Lifetime"] != DBNull.Value)
                {
                    try
                    {
                        lpDetail.LifetimeCap = Convert.ToDecimal(ReplaceEToD(dr["Lifetime"]));
                    }
                    catch
                    {
                        Write(0, "The following rows/Column contain invalid data:Row <" + rowNum + "> Column <Lifetime> must be decimal.");
                        isErr = true;
                        break;
                    }
                }


                rowNum++;

                detailList.Add(lpDetail);
            }

            if (isErr)
            {
                return;
            }

            #endregion


            #region Import

            ServiceTypes bllServiceTypes = new ServiceTypes();
            LPWeb.BLL.ContactCompanies bllContactCompanies = new ContactCompanies();
            Company_LoanProgramDetails bllLoanProgramDetails = new Company_LoanProgramDetails();
            foreach (LPWeb.Model.Company_LoanProgramDetails detail in detailList)
            {

                #region LoanProgramID
                LPWeb.Model.Company_Loan_Programs loanpro = bllLoanProgram.GetModelList(" LoanProgram ='" + detail.Program.Trim() + "'").FirstOrDefault();

                if (loanpro != null && loanpro.LoanProgram.Trim() == detail.Program.Trim())
                {

                    detail.LoanProgramID = loanpro.LoanProgramID;

                    if (!loanpro.IsARM)
                    {
                        loanpro.IsARM = true;
                        bllLoanProgram.Update(loanpro);
                    }

                }
                else
                {
                    loanpro = new LPWeb.Model.Company_Loan_Programs();
                    loanpro.IsARM = true;
                    loanpro.LoanProgram = detail.Program;

                    int loanprogramsId = bllLoanProgram.Add(loanpro);

                    detail.LoanProgramID = loanprogramsId;

                }
                #endregion


                #region InvestorID

                //detail.LenderCompanyId; 已取出  在CR67
                var ServiceType = bllServiceTypes.GetModelList(" Name ='Investor'").FirstOrDefault(); //cr67
                int InvestorServiceTypeId = ServiceType == null ? 0 : ServiceType.ServiceTypeId;
                string sWhere = string.Format("[Name]='{0}'", detail.Investor.Trim());
                if (InvestorServiceTypeId > 0)
                    sWhere += " AND ServiceTypeId=" + InvestorServiceTypeId;
                
                LPWeb.Model.ContactCompanies contactComp = bllContactCompanies.GetModelList(sWhere).FirstOrDefault();

                if (contactComp != null && contactComp.Name.Trim() == detail.Investor.Trim())
                {
                    detail.InvestorID = contactComp.ContactCompanyId;

                }
                else
                {
                    contactComp = new LPWeb.Model.ContactCompanies();

                    contactComp.Name = detail.Investor.Trim();
                    //var type = bllServiceTypes.GetModelList(" Name ='Lender' or Name = 'Lending '").FirstOrDefault();
                    if (ServiceType != null)
                    {
                        contactComp.ServiceTypeId = ServiceType.ServiceTypeId;
                        contactComp.ServiceTypes = ServiceType.Name;
                    }
                    contactComp.Enabled = true;

                    var contactCompanyId = bllContactCompanies.Add(contactComp);

                    detail.InvestorID = contactCompanyId;
                }




                #endregion

                if (bllLoanProgramDetails.Exists(detail.LoanProgramID, detail.InvestorID))
                {
                    bllLoanProgramDetails.Update(detail);
                }
                else
                {
                    bllLoanProgramDetails.Add(detail);
                }

            }


            #endregion


            Write(1, "");
        }
        catch (Exception ex)
        {
            Write(0, ex.Message.Replace("'", "").Replace("\r\n", ""));
        }

    }

    /// <summary>
    /// 去掉数字里的 E-2字符避免转换decimal时出错
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string ReplaceE(object str)
    {
        return str.ToString().Replace("E-2", "");
    }

    /// <summary>
    /// 去掉数字里的 E-2字符避免转换decimal时出错
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private decimal ReplaceEToD(object obj)
    {
        try
        {
            string str = obj.ToString();
            if (str.IndexOf("E-2") != -1)
            {
                return Convert.ToDecimal(str.Replace("E-2", ""));
            }
            else
            {
                return Convert.ToDecimal(str) * 100;
            }
        }
        catch { return 0M; }
    }



    private void Write(int state, string msg)
    {
        Response.Write("<script>window.parent.fileUPARMOK(" + state.ToString() + ",'" + msg + "');</script>");
        //Response.Write("<script>alert(111);</script>");
        Response.End();
    }

    protected void btImport_Click(object sender, EventArgs e)
    {
        //it will send anImportDT_LoanPrograms request to the DataTrac Manager. If the DataTrac Manager returns an error, display the error; otherwise, refresh the page.

        //LPWeb.LP_Service.DT_ImportLoansRequest im = new LPWeb.LP_Service.DT_ImportLoansRequest();

        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Refresh", "<script>"+PageCommon.Js_RefreshSelf+"</script>");

    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        var selctedStr = this.hfDeleteItems.Value;
        if (!string.IsNullOrEmpty(selctedStr))
        {
            string[] selectedItems = selctedStr.Split(',');
            //delete the selected items
            DeleteLoanPrograms(selectedItems);
            //reload the grid data
            PageCommon.WriteJsEnd(this, "The Loan Program removed successfully.", PageCommon.Js_RefreshSelf);
        } 
        FillDataGrid(string.Empty);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }


    protected void btnDelete_Click(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty(hfDeleteItems.Value.Trim()))
        {
            return;
        }

        string IdList = hfDeleteItems.Value.Trim();

        LPWeb.BLL.Company_LoanProgramDetails blllpd = new Company_LoanProgramDetails();

        var arrIDsList = IdList.Split(',');

        foreach (var item in arrIDsList)
        {
            try
            {
                var ids = item.Split(':');
                if (ids.Length != 2)
                {
                    continue;
                }
                blllpd.Delete(Convert.ToInt32(ids[0]), Convert.ToInt32(ids[1]));
            }
            catch { }
        }

        BindgvList();
    }

    protected void btnEnable_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(hfDeleteItems.Value.Trim()))
        {
            return;
        }

        string IdList = hfDeleteItems.Value.Trim();

        LPWeb.BLL.Company_LoanProgramDetails blllpd = new Company_LoanProgramDetails();

        var arrIDsList = IdList.Split(',');

        foreach (var item in arrIDsList)
        {
            try
            {
                var ids = item.Split(':');
                if (ids.Length != 2)
                {
                    continue;
                }
                blllpd.UpdateEnabled(Convert.ToInt32(ids[0]), Convert.ToInt32(ids[1]), true);
            }
            catch { }
        }
        BindgvList();
    }


    protected void btnDisable_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(hfDeleteItems.Value.Trim()))
        {
            return;
        }

        string IdList = hfDeleteItems.Value.Trim();

        LPWeb.BLL.Company_LoanProgramDetails blllpd = new Company_LoanProgramDetails();

        var arrIDsList = IdList.Split(',');

        foreach (var item in arrIDsList)
        {
            try
            {
                var ids = item.Split(':');
                if (ids.Length != 2)
                {
                    continue;
                }
                blllpd.UpdateEnabled(Convert.ToInt32(ids[0]), Convert.ToInt32(ids[1]), false);
            }
            catch { }
        }
        BindgvList();
    }



    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
        BindgvList();
    }


    protected void ddlInvestors_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindPrograms();
        BindIndexesList();
    }

    protected void ddlTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindPrograms();
        BindIndexesList();
    }

    protected void ddlPrograms_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindIndexesList();
    }


    protected void btnFilter_Click(object sender, EventArgs e)
    {
        isReset = true;
        
        BindgvList();
    }


    private void BindgvList()
    {
        string condition = string.Empty;

        if (!string.IsNullOrEmpty(ddlTypes.SelectedValue.Trim()))
        {
            //condition += "IsARM = ";
            condition += " AND IsARM = ";
            condition += ddlTypes.SelectedValue.Trim() == "0" ? "0" : "1";
        }

        string investors = ddlInvestors.SelectedValue.Trim();
        string programID = ddlPrograms.SelectedValue.Trim();

        if (!string.IsNullOrEmpty(programID))
        {
            try
            {
                condition += " AND LoanProgramID = " + Convert.ToInt32(programID).ToString();
            }
            catch { }
        }

        if (!string.IsNullOrEmpty(investors))
        {
            try
            {
                condition += " AND InvestorID = " + Convert.ToInt32(investors).ToString();
            }
            catch { }
        }

        string index = ddlIndexes.SelectedValue.Trim();

        if (!string.IsNullOrEmpty(index))
        {
            index = index.Replace("\'", "").Replace("--", "");
            try
            {
                condition += " AND IndexType = '" + index +"'";
            }
            catch { }

        }

        
        FillDataGrid(condition);

    }

    /// <summary>
    /// Fills the data grid.
    /// </summary>
    /// <param name="condition">The condition.</param>
    private void FillDataGrid(string condition)
    {
        DataSet loanProgramses = null;

        int pageSize = AspNetPager1.PageSize;
        int pageIndex = 1;

        if (isReset == true)
            pageIndex = AspNetPager1.CurrentPageIndex = 1;
        else
            pageIndex = AspNetPager1.CurrentPageIndex;

        int totalcount = 0;
        try
        {
            loanProgramses = bllLoanProgram.GetListInvestorARMprogram(pageSize, pageIndex, condition, out totalcount, "LoanProgramID", 0);
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }


        gvLoanProgramses.DataSource = loanProgramses;
        gvLoanProgramses.DataBind();

        AspNetPager1.RecordCount = totalcount;

    }

    /// <summary>
    /// Deletes the loan programs.
    /// </summary>
    /// <param name="items">The items.</param>
    private void DeleteLoanPrograms(string[] items)
    {
        int iItem = 0;
        foreach (var item in items)
        {
            if (int.TryParse(item, out iItem))
            {
                try
                {
                    bllLoanProgram.Delete(iItem);
                }
                catch (Exception exception)
                {
                    LPLog.LogMessage(exception.Message);
                }
            }
        }
    }

    private void BindInvestors()
    {
        ddlInvestors.DataSource = bllLoanProgram.GetInvestorsList();
        ddlInvestors.DataBind();
        ddlInvestors.Items.Insert(0, new ListItem() { Text = "All Investors", Value = "" });
    }


    private void BindPrograms()
    {
        string investors = ddlInvestors.SelectedValue.Trim();
        string type = ddlTypes.SelectedValue.Trim();

        string wherestr = "";
        if (!string.IsNullOrEmpty(type))
        {
            wherestr += " AND IsARM = ";
            wherestr += type == "0" ? "0" : "1";
        }

        if (!string.IsNullOrEmpty(investors))
        {
            try
            {
                wherestr += " AND InvestorID = " + Convert.ToInt32(investors).ToString();
            }
            catch { }
        }

        ddlPrograms.DataSource = bllLoanProgram.GetProgramsList(wherestr);
        ddlPrograms.DataBind();
        ddlPrograms.Items.Insert(0, new ListItem() { Text = "All Programs", Value = "" });
    }

    private void BindIndexesList()
    {
        string investors = ddlInvestors.SelectedValue.Trim();
        string programID = ddlPrograms.SelectedValue.Trim();

        string wherestr = "";
        if (!string.IsNullOrEmpty(programID))
        {
            try
            {
                wherestr += " AND LoanProgramID = " + Convert.ToInt32(programID).ToString();
            }
            catch { }
        }

        if (!string.IsNullOrEmpty(investors))
        {
            try
            {
                wherestr += " AND InvestorID = " + Convert.ToInt32(investors).ToString();
            }
            catch { }
        }

        ddlIndexes.DataSource = bllLoanProgram.GetIndexesList(wherestr);
        ddlIndexes.DataBind();
        ddlIndexes.Items.Insert(0, new ListItem() { Text = "All Indexes", Value = "" });
    }
}