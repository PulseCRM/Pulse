using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.BLL;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.IO;
using System.Web.UI;

namespace LPWeb.Layouts.LPWeb.Pipeline
{
    public partial class ImportLeads : BasePage
    {
        #region Filed
        private DataTable dtErrorInfo = new DataTable();
        private DataTable dtExcelData = new DataTable();

        private string BranchId = "0";

        private string iLoanOfficer = "0";

        private bool bRequired = true;

        private string sLeadSource = "";

        private string sReferenceCode = "";

        private int iBrrowerRoleID = 1;
        private int iCoBrrowerRoleID = 2;
        private int iLoanOfficerRoleID = 3;
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControlDataSource();
            }
            divError.Visible = false;
        }

        private void BindControlDataSource()
        {
            Company_Lead_Sources bCompany_Lead_Sources = new Company_Lead_Sources();
            DataSet dsLeadSources = bCompany_Lead_Sources.GetAllList();
            DataTable dtLS = new DataTable();
            if (dsLeadSources != null && dsLeadSources.Tables[0].Rows.Count > 0)
            {
                dtLS = dsLeadSources.Tables[0];
            }
            if (!dtLS.Columns.Contains("LeadSourceID"))
            {
                dtLS.Columns.Add("LeadSourceID");
            }
            if (!dtLS.Columns.Contains("LeadSource"))
            {
                dtLS.Columns.Add("LeadSource");
            }
            DataRow drls = dtLS.NewRow();
            drls["LeadSourceID"] = "-1";
            drls["LeadSource"] = "- select a Lead Source -";
            dtLS.Rows.InsertAt(drls, 0);
            ddlLeadSource.DataSource = dtLS;
            ddlLeadSource.DataBind();

            Branches bBranches = new Branches();
            DataTable dtBH = null;
            if (CurrUser.bIsCompanyExecutive || CurrUser.bIsDivisionExecutive || CurrUser.bIsRegionExecutive)
                dtBH = bBranches.GetBranchFilter_Executive(CurrUser.iUserID, 0, 0);
            else
                if (CurrUser.bIsBranchManager)
                    dtBH = bBranches.GetBranchFilter_Branch_Manager(CurrUser.iUserID, 0, 0);
                else
                    dtBH = bBranches.GetBranchFilter(CurrUser.iUserID, 0, 0);

            if (dtBH != null)
            {
                DataRow drbh = dtBH.NewRow();
                drbh["BranchId"] = "-1";
                drbh["Name"] = "- select a Branch -";
                dtBH.Rows.InsertAt(drbh, 0);
                ddlBranch.DataSource = dtBH;
                ddlBranch.DataBind();
            }

            Users bUsers = new Users();
            if (CurrUser.bIsBranchUser)
            {
                DataTable dt = bUsers.GetUserBranchInfo(CurrUser.iUserID.ToString());
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.SelectedValue = Convert.ToInt32(dt.Rows[0]["BranchID"]).ToString();
                    ddlBranch.Enabled = false;
                }
            }

            DataTable dtLO = bUsers.GetUserList(string.Format("And RoleName='{0}' and UserId={1}", "Loan Officer", CurrUser.iUserID));
            if (dtLO.Rows.Count > 0)
            {
                //CurrentUser is Loan Officer
                if (!dtLO.Columns.Contains("LoanOfficer"))
                {
                    dtLO.Columns.Add("LoanOfficer");
                }
                ddlLoanOfficer.DataSource = dtLO;
                ddlLoanOfficer.DataTextField = "LoanOfficer";
                ddlLoanOfficer.DataValueField = "UserID";
                ddlLoanOfficer.SelectedValue = CurrUser.iUserID.ToString();
                ddlLoanOfficer.DataBind();
                ddlLoanOfficer.Enabled = false;

            }
        }

        private void BindLoanOfficer(string sBranchID)
        {
            if (ddlLoanOfficer.Enabled == false)
            {
                return;
            }

            //string sqlCmd = "select v.* from dbo.lpvw_GetuserGroups_ByRoleName v where (RoleName='Loan Officer' or RoleName='Branch Manager' or RoleName='Executive') ";
            string sqlCmd = "select distinct v.name, v.UserId from dbo.lpvw_GetuserGroups_ByRoleName v where (RoleName='Loan Officer' or RoleName='Branch Manager' or RoleName='Executive') order by name ";

            DataTable dtLoadOfficer = null;
            if (CurrUser.bIsBranchUser)
            {
                if (CurrUser.userRole.Name.ToUpper() == "LOAN OFFICER")
                    sqlCmd = sqlCmd + string.Format(" and  v.UserId={0} ", CurrUser.iUserID);
                else
                    sqlCmd = sqlCmd + string.Format(" and v.BranchId={0}", sBranchID);
            }

            dtLoadOfficer = DAL.DbHelperSQL.ExecuteDataTable(sqlCmd);

            if ( (dtLoadOfficer != null) && (dtLoadOfficer.Rows.Count > 0))
            {

                if (!dtLoadOfficer.Columns.Contains("LoanOfficer"))
                {
                    dtLoadOfficer.Columns.Add("LoanOfficer");
                }
                foreach (DataRow dr in dtLoadOfficer.Rows)
                {
                    dr["LoanOfficer"] = dr["Name"].ToString();
                }
                DataRow drNew = dtLoadOfficer.NewRow();
                drNew["UserID"] = -1;
                drNew["LoanOfficer"] = "- select a Loan Officer -";
                dtLoadOfficer.Rows.InsertAt(drNew, 0);

                ddlLoanOfficer.ClearSelection();
                ddlLoanOfficer.DataSource = dtLoadOfficer;
                ddlLoanOfficer.DataTextField = "LoanOfficer";
                ddlLoanOfficer.DataValueField = "UserID";
                ddlLoanOfficer.SelectedValue = "-1";
                ddlLoanOfficer.DataBind();
            }
        }

        protected void ddlBranch_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindLoanOfficer(ddlBranch.SelectedValue.ToString());
        }

        protected void btnImport_OnClick(object sender, EventArgs e)
        {
            dtErrorInfo.Columns.Add("RowNo");
            dtErrorInfo.Columns.Add("BorrowerName");
            dtErrorInfo.Columns.Add("ColumnsInfo");

            this.BranchId = ddlBranch.SelectedValue.ToString();
            this.iLoanOfficer = ddlLoanOfficer.SelectedValue.ToString();

            if (ddlLeadSource.SelectedValue != "-1")
            {
                this.sLeadSource = ddlLeadSource.SelectedItem.Text;
            }
            this.sReferenceCode = tbRefCode.Text;


            ContactRoles bContactRoles = new ContactRoles();
            Roles bRoles = new Roles();
            DataSet ds = bContactRoles.GetList("Name = 'Borrower'");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                iBrrowerRoleID = Convert.ToInt32(ds.Tables[0].Rows[0]["ContactRoleId"]);
            }
            ds = bContactRoles.GetList("Name = 'CoBorrower'");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                iCoBrrowerRoleID = Convert.ToInt32(ds.Tables[0].Rows[0]["ContactRoleId"]);
            }

            string FilePath = this.FileUpload.FileName;
            string strUploadUrl = Server.MapPath("./");//此"."可以换成项目文件里的其它文件夹名称
            strUploadUrl = strUploadUrl.Replace("Pipeline\\", "");

            #region 上传文件到临时文件夹
            string strUploadFolder = @"\UploadFiles\\Temp\\";
            // 临时文件夹
            string sTempUploadFold = strUploadUrl + strUploadFolder;  // this.MapPath("~/UploadFiles/Temp/");
            string sFileExt1 = Path.GetExtension(this.FileUpload.FileName).ToLower();
            string sTempFileName1 = Guid.NewGuid().ToString() + sFileExt1;

            // 临时文件路径
            string sTempFilePath1 = Path.Combine(sTempUploadFold, sTempFileName1); // "D:\\Test\\" + FilePath;// 

            try
            {
                // 文件上传到临时文件夹
                this.FileUpload.SaveAs(sTempFilePath1);
            }
            catch (Exception ex)
            {
                ClientScriptManager clientScriptManager = Page.ClientScript;
                clientScriptManager.RegisterStartupScript(this.GetType(), "1", "<script language='javascript'>");
                clientScriptManager.RegisterStartupScript(this.GetType(), "2", "alert('Failed to upload the file due to insufficient privilege. Please contact your system administrator.');");
                clientScriptManager.RegisterStartupScript(this.GetType(), "3", "</script>");
                return;
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

                int iRow = 2;
                foreach (DataRow dr in dtExcelData.Rows)
                {
                    ImportOneRecord(dr, iRow);
                    iRow++;
                }
            }
            catch (Exception ex)
            {
                ClientScriptManager clientScriptManager = Page.ClientScript;
                clientScriptManager.RegisterStartupScript(this.GetType(), "1", "<script language='javascript'>");
                clientScriptManager.RegisterStartupScript(this.GetType(), "2", "alert('Import Error. Please make sure row 1 of the file has data.');");
                clientScriptManager.RegisterStartupScript(this.GetType(), "3", "</script>");
            }
            finally
            {
                if (File.Exists(sTempFilePath1))
                {
                    File.Delete(sTempFilePath1);
                }
            }

            if (dtErrorInfo.Rows.Count > 0)
            {
                //show Error Info
                this.aspnetForm.Visible = false;
                this.divError.Visible = true;
                gvErrorView.DataSource = dtErrorInfo;
                gvErrorView.DataBind();
            }
            else
            {
                ClientScriptManager clientScriptManager = Page.ClientScript;
                clientScriptManager.RegisterStartupScript(this.GetType(), "1", "<script language='javascript'>");
                clientScriptManager.RegisterStartupScript(this.GetType(), "2", "alert('Import success！');");
                clientScriptManager.RegisterStartupScript(this.GetType(), "3", "</script>");
            }
        }
        #endregion

        #region function
        /// <summary>
        /// Import One Record
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="RowNum"></param>
        private void ImportOneRecord(DataRow dr, int RowNum)
        {
            if (!CheckData(dr, RowNum))
            {
                return;
            }
            if (!bRequired)  // not has required columns
            {
                return;
            }

            int iContactID = ImportContact(dr);
            int iContactIDCo = ImportCoBorrower(dr);
            int iFileID = ImportPointFiles();

            //Import Loan
            int iLoanID = ImportLoan(dr, iFileID);

            ImportProspect(iContactID);

            ImportLoanTeam(iFileID);



            ImportLoanContacts(iFileID, iContactID, iBrrowerRoleID);
            if (iContactIDCo != 0)
            {
                ImportLoanContacts(iFileID, iContactIDCo, iCoBrrowerRoleID);
            }

        }

        /// <summary>
        /// 检查Row 信息 是否满足导入要求
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool CheckData(DataRow dr, int RowNum)
        {
            bool bRst = true;
            DataRow drError;
            string sColumnsInfo = "";

            if (iLoanOfficer == "-1")
            {
                Users bUsers = new Users();
                if (dtExcelData.Columns.Contains("Loan Officer"))
                {
                    if (dr["Loan Officer"] != DBNull.Value && dr["Loan Officer"].ToString() != "")
                    {
                        DataSet ds = bUsers.GetList("UserId=" + dr["Loan Officer"].ToString());
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            iLoanOfficer = ds.Tables[0].Rows[0]["UserId"].ToString();
                        }
                        else
                        {
                            sColumnsInfo = "'Loan Officer' not exists,";
                            bRst = false;
                        }
                    }
                    else
                    {
                        DataTable dt = bUsers.GetBranchManager(BranchId);
                        if (dt.Rows.Count > 0)
                        {   //IF Not Select LoanOfficer ,then Select the Branch's Manager
                            iLoanOfficer = dt.Rows[0]["UserId"].ToString();
                        }
                    }
                }
                else
                {
                    DataTable dt = bUsers.GetBranchManager(BranchId);
                    if (dt.Rows.Count > 0)
                    {   //IF Not Select LoanOfficer ,then Select the Branch's Manager
                        iLoanOfficer = dt.Rows[0]["UserId"].ToString();
                    }
                }
            }

            if (!dtExcelData.Columns.Contains("Borrower First Name"))
            {
                sColumnsInfo += "'Borrower First Name' column not exists,";
                bRequired = false;
                bRst = false;
            }
            if (!dtExcelData.Columns.Contains("Borrower Last Name"))
            {
                sColumnsInfo += "'Borrower Last Name' column not exists,";
                bRequired = false;
                bRst = false;
            }
            if (!dtExcelData.Columns.Contains("Borrower Address"))
            {
                sColumnsInfo += "'Borrower Address' column not exists,";
                bRequired = false;
                bRst = false;
            }
            if (!dtExcelData.Columns.Contains("Borrower City"))
            {
                sColumnsInfo += "'Borrower City' column not exists,";
                bRequired = false;
                bRst = false;
            }
            if (!dtExcelData.Columns.Contains("Borrower State"))
            {
                sColumnsInfo += "'Borrower State' column not exists,";
                bRequired = false;
                bRst = false;
            }
            if (!dtExcelData.Columns.Contains("Borrower Zip"))
            {
                sColumnsInfo += "'Borrower Zip' column not exists,";
                bRequired = false;
                bRst = false;
            }
            if (!dtExcelData.Columns.Contains("Borrower Home Phone"))
            {
                sColumnsInfo += "'Borrower Home Phone' column not exists,";
                bRequired = false;
                bRst = false;
            }
            if (bRequired == false)
            {
                drError = dtErrorInfo.NewRow();
                drError["RowNo"] = RowNum;
                string sBorrowerName = "";
                if (dtExcelData.Columns.Contains("Borrower First Name"))
                {
                    sBorrowerName = dr["Borrower First Name"] != DBNull.Value ? dr["Borrower First Name"].ToString() : "";
                }
                drError["BorrowerName"] = sBorrowerName;
                drError["ColumnsInfo"] = sColumnsInfo.TrimEnd(',');
                dtErrorInfo.Rows.Add(drError);

                return bRst;
            }

            if (dr["Borrower First Name"] == DBNull.Value || dr["Borrower First Name"].ToString() == "")
            {
                sColumnsInfo += "'Borrower First Name' is blank,";
                bRst = false;
            }
            if (dr["Borrower Last Name"] == DBNull.Value || dr["Borrower Last Name"].ToString() == "")
            {

                sColumnsInfo += "'Borrower Last Name' is blank,";
                bRst = false;
            }
            if (dr["Borrower Address"] == DBNull.Value || dr["Borrower Address"].ToString() == "")
            {
                sColumnsInfo += "'Borrower Address' is blank,";
                bRst = false;
            }
            if (dr["Borrower City"] == DBNull.Value || dr["Borrower City"].ToString() == "")
            {
                sColumnsInfo += "'Borrower City' is blank,";
                bRst = false;
            }
            if (dr["Borrower State"] == DBNull.Value || dr["Borrower State"].ToString() == "")
            {
                sColumnsInfo += "'Borrower State' is blank,";
                bRst = false;
            }
            if (dr["Borrower Zip"] == DBNull.Value || dr["Borrower Zip"].ToString() == "")
            {
                sColumnsInfo += "'Borrower Zip' is blank,";
                bRst = false;
            }
            if (dr["Borrower Home Phone"] == DBNull.Value || dr["Borrower Home Phone"].ToString() == "")
            {
                sColumnsInfo += "'Borrower Home Phone' is blank,";
                bRst = false;
            }

            // Add to DtErrorInfo;
            if (bRst == false)
            {
                drError = dtErrorInfo.NewRow();
                drError["RowNo"] = RowNum;
                string sBorrowerName = "";
                if (dtExcelData.Columns.Contains("Borrower First Name"))
                {
                    sBorrowerName = dr["Borrower First Name"] != DBNull.Value ? dr["Borrower First Name"].ToString() : "";
                }
                drError["BorrowerName"] = sBorrowerName;
                drError["ColumnsInfo"] = sColumnsInfo.TrimEnd(',');
                dtErrorInfo.Rows.Add(drError);
            }

            return bRst;
        }

        /// <summary>
        /// Import Loans 
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private int ImportLoan(DataRow dr, int iFileID)
        {
            Loans bLoans = new Loans();
            Model.Loans mLoans = new Model.Loans();
            if (dtExcelData.Columns.Contains("Lien"))
            {
                mLoans.LienPosition = dr["Lien"] == DBNull.Value ? "" : dr["Lien"].ToString();
            }
            if (dtExcelData.Columns.Contains("Purpose of Loan"))
            {
                mLoans.Purpose = dr["Purpose of Loan"] == DBNull.Value ? "" : dr["Purpose of Loan"].ToString();
            }
            if (dtExcelData.Columns.Contains("Loan Amount"))
            {
                mLoans.LoanAmount = (dr["Loan Amount"] == DBNull.Value || dr["Loan Amount"].ToString() == "") ? 0 : Convert.ToDecimal(dr["Loan Amount"]);
            }
            if (dtExcelData.Columns.Contains("Appraised Value"))
            {
                mLoans.AppraisedValue = (dr["Appraised Value"] == DBNull.Value || dr["Appraised Value"].ToString() == "") ? 0 : Convert.ToDecimal(dr["Appraised Value"]);
            }
            if (dtExcelData.Columns.Contains("Sales Price"))
            {
                mLoans.SalesPrice = (dr["Sales Price"] == DBNull.Value || dr["Sales Price"].ToString() == "") ? 0 : Convert.ToDecimal(dr["Sales Price"]);
            }
            if (dtExcelData.Columns.Contains("Property Address"))
            {
                mLoans.PropertyAddr = dr["Property Address"] == DBNull.Value ? "" : dr["Property Address"].ToString();
            }
            if (dtExcelData.Columns.Contains("Property City"))
            {
                mLoans.PropertyCity = dr["Property City"] == DBNull.Value ? "" : dr["Property City"].ToString();
            }
            if (dtExcelData.Columns.Contains("Property State"))
            {
                mLoans.PropertyState = dr["Property State"] == DBNull.Value ? "" : dr["Property State"].ToString();
            }
            if (dtExcelData.Columns.Contains("Property Zip"))
            {
                mLoans.PropertyZip = dr["Property Zip"] == DBNull.Value ? "" : dr["Property Zip"].ToString();
            }
            mLoans.Status = "Prospect";
            mLoans.ProspectLoanStatus = "Active";
            mLoans.Ranking = "Hot";
            mLoans.Created = DateTime.Now;
            mLoans.CreatedBy = CurrUser.iUserID;
            mLoans.BranchID = Convert.ToInt32(BranchId);
            mLoans.FileId = iFileID;

            int iLoanID = bLoans.Add(mLoans);
            return iLoanID;
        }

        /// <summary>
        /// Import Contact 
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private int ImportContact(DataRow dr)
        {
            Contacts bContacts = new Contacts();
            Model.Contacts mContacts = new Model.Contacts();
            mContacts.FirstName = dr["Borrower First Name"] == DBNull.Value ? "" : dr["Borrower First Name"].ToString();
            if (dtExcelData.Columns.Contains("Borrower Middle Name"))
            {
                mContacts.MiddleName = dr["Borrower Middle Name"] == DBNull.Value ? "" : dr["Borrower Middle Name"].ToString();
            }
            mContacts.LastName = dr["Borrower Last Name"] == DBNull.Value ? "" : dr["Borrower Last Name"].ToString();
            mContacts.MailingAddr = dr["Borrower Address"] == DBNull.Value ? "" : dr["Borrower Address"].ToString();
            mContacts.MailingCity = dr["Borrower City"] == DBNull.Value ? "" : dr["Borrower City"].ToString();
            mContacts.MailingState = dr["Borrower State"] == DBNull.Value ? "" : dr["Borrower State"].ToString();
            mContacts.MailingZip = dr["Borrower Zip"] == DBNull.Value ? "" : dr["Borrower Zip"].ToString();
            if (dtExcelData.Columns.Contains("Borrower DOB"))
            {
                if (dr["Borrower DOB"] != DBNull.Value && dr["Borrower DOB"].ToString() != "")
                {
                    mContacts.DOB = Convert.ToDateTime(dr["Borrower DOB"]);
                }
            }
            if (dtExcelData.Columns.Contains("Borrower SSN"))
            {
                mContacts.SSN = dr["Borrower SSN"] == DBNull.Value ? "" : dr["Borrower SSN"].ToString();
            }
            mContacts.HomePhone = dr["Borrower Home Phone"] == DBNull.Value ? "" : dr["Borrower Home Phone"].ToString();
            if (dtExcelData.Columns.Contains("Borrower Work Phone"))
            {
                mContacts.BusinessPhone = dr["Borrower Work Phone"] == DBNull.Value ? "" : dr["Borrower Work Phone"].ToString();
            }
            if (dtExcelData.Columns.Contains("Borrower Email"))
            {
                mContacts.Email = dr["Borrower Email"] == DBNull.Value ? "" : dr["Borrower Email"].ToString();
            }

            mContacts.ContactEnable = true;
            mContacts.NickName = dr["Borrower First Name"] == DBNull.Value ? "" : dr["Borrower First Name"].ToString();
            int iContactID = bContacts.Add(mContacts);
            return iContactID;
        }

        /// <summary>
        /// Import Co-borrower to Contact 
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private int ImportCoBorrower(DataRow dr)
        {
            if (dtExcelData.Columns.Contains("Co-Borrower First Name") && dtExcelData.Columns.Contains("Co-Borrower Last Name"))
            {
                if ((dr["Co-Borrower First Name"] == DBNull.Value || dr["Co-Borrower First Name"].ToString() == "") && (dr["Co-Borrower Last Name"] == DBNull.Value || dr["Co-Borrower Last Name"].ToString() == ""))
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }


            Contacts bContacts = new Contacts();
            Model.Contacts mContacts = new Model.Contacts();
            mContacts.FirstName = dr["Co-Borrower First Name"] == DBNull.Value ? "" : dr["Co-Borrower First Name"].ToString();
            if (dtExcelData.Columns.Contains("Co-Borrower Middle Name"))
            {
                mContacts.MiddleName = dr["Co-Borrower Middle Name"] == DBNull.Value ? "" : dr["Co-Borrower Middle Name"].ToString();
            }
            mContacts.LastName = dr["Co-Borrower Last Name"] == DBNull.Value ? "" : dr["Co-Borrower Last Name"].ToString();
            if (dtExcelData.Columns.Contains("Co-Borrower DOB"))
            {
                if (dr["Co-Borrower DOB"] != DBNull.Value && dr["Co-Borrower DOB"].ToString() != "")
                {
                    mContacts.DOB = Convert.ToDateTime(dr["Co-Borrower DOB"]);
                }
            }
            if (dtExcelData.Columns.Contains("Co-Borrower SSN"))
            {
                mContacts.SSN = dr["Co-Borrower SSN"] == DBNull.Value ? "" : dr["Co-Borrower SSN"].ToString();
            }
            if (dtExcelData.Columns.Contains("Co-Borrower Email"))
            {
                mContacts.Email = dr["Co-Borrower Email"] == DBNull.Value ? "" : dr["Co-Borrower Email"].ToString();
            }

            mContacts.ContactEnable = true;
            mContacts.NickName = dr["Co-Borrower First Name"] == DBNull.Value ? "" : dr["Co-Borrower First Name"].ToString();
            int iContactID = bContacts.Add(mContacts);
            return iContactID;
        }

        /// <summary>
        /// Import PointFiles
        /// </summary>
        /// <returns></returns>
        private int ImportPointFiles()
        {
            PointFiles bPointFiles = new PointFiles();
            Model.PointFiles mPointFiles = new Model.PointFiles();

            //PointFolders bPointFolders = new PointFolders();
            //DataSet dsFolder = bPointFolders.GetList("BranchID=" + BranchId + " and Enabled='true' order by FolderId");
            //if (dsFolder != null && dsFolder.Tables[0].Rows.Count > 0)
            //{
            //    mPointFiles.FolderId = Convert.ToInt32(dsFolder.Tables[0].Rows[0]["FolderId"]);
            //}
            mPointFiles.FolderId = 0;
            mPointFiles.FirstImported = DateTime.Now;
            mPointFiles.Success = true;
            //mPointFiles.CurrentImage = Convert.ToByte("");
            // mPointFiles.PreviousImage = Convert.ToByte("");
            int iFileID = bPointFiles.Add(mPointFiles);
            return iFileID;

        }

        /// <summary>
        /// Import Prospect
        /// </summary>
        /// <returns></returns>
        private int ImportProspect(int iContactID)
        {
            BLL.Prospect bProspect = new BLL.Prospect();
            Model.Prospect mProspect = new Model.Prospect();

            mProspect.LeadSource = this.sLeadSource;
            mProspect.ReferenceCode = this.sReferenceCode;
            mProspect.Contactid = iContactID;
            mProspect.Status = "Active";
            mProspect.Created = DateTime.Now;
            mProspect.CreatedBy = CurrUser.iUserID;
            mProspect.Loanofficer = Convert.ToInt32(iLoanOfficer);

            return bProspect.Add(mProspect);
        }

        /// <summary>
        /// Import LoanTeam
        /// </summary>
        /// <param name="iFileId"></param>
        private void ImportLoanTeam(int iFileId)
        {
            LoanTeam bLoanTeam = new LoanTeam();
            Model.LoanTeam mLoanTeam = new Model.LoanTeam();
            mLoanTeam.FileId = iFileId;
            mLoanTeam.UserId = Convert.ToInt32(iLoanOfficer);
            mLoanTeam.RoleId = iLoanOfficerRoleID; //Loan Officer RoleID

            bLoanTeam.Add(mLoanTeam);
        }

        /// <summary>
        ///  Import LoanContacts
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="iContactID"></param>
        /// <param name="RoleID"></param>
        private void ImportLoanContacts(int iFileId, int iContactID, int RoleID)
        {
            LoanContacts bLoanContacts = new LoanContacts();
            Model.LoanContacts mLoanContacts = new Model.LoanContacts();
            mLoanContacts.FileId = iFileId;
            mLoanContacts.ContactId = iContactID;
            mLoanContacts.ContactRoleId = RoleID;
            bLoanContacts.Add(mLoanContacts);

        }
        #endregion
    }
}
