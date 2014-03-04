using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Text;
using System.Text.RegularExpressions;

public partial class Pipeline_ProcessingPipelineSummary : BasePage
{
    private readonly Loans _bllLoans = new Loans();
    private LoginUser _curLoginUser = new LoginUser();
    private LoanAlerts _loanAlerts = new LoanAlerts();
    private LoanNotes _loanNotes = new LoanNotes();
    private UserPipelineColumns _bllUPC = new UserPipelineColumns();
    public string FromURL = string.Empty;
    private bool isReset = false;
    private string fromHomeFilter = string.Empty;
    string sLoanStatus = string.Empty;
    private int CurrentPage = 1;
    private string sUserLoanList = string.Empty;    
    private bool FirstTimeFlag = false;
    int viewstateIdx = 0;
    int currentpageIdx = 0;
    private DataTable dtUserLoansViewPoint = new DataTable();
    private UserLoansViewPointFields bllUserLoansViewPointFields = new UserLoansViewPointFields();
    private UserRecentItems _bllUserRecentItems = new UserRecentItems();

    string AdvacedLoanFilters = string.Empty;    
    string AdvacedSearch = string.Empty;

    string AdvancedLoanFilters = string.Empty;
    public bool isView = false;
    public bool isView_To_Select = false;
    public bool IsDefaultView = false;
    public bool IsGrid = false;
    public bool ViewIndexChanged = false;
    public bool Change_To_Select_After_Filter = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Url != null)
        {
            FromURL = Request.Url.ToString();
        }

        FirstTimeFlag = false;      
        IsDefaultView = false;
        IsGrid = false;
        ViewIndexChanged = false;
        isView_To_Select = false;
        this.AdvancedLoanFilters = "";
        Change_To_Select_After_Filter = false;

        if (!IsPostBack)
        {
            FirstTimeFlag = true;
            viewstateIdx = 0;
            currentpageIdx = 0;

            _curLoginUser = new LoginUser();
            
            if (!_curLoginUser.userRole.ImportLoan)
            {
                btnSync.Enabled = false;
            }
            if (!_curLoginUser.userRole.RemoveLoan)
            {
                btnRemove.Enabled = false;
            }

            if (_curLoginUser.bIsCompanyExecutive ||
                (_curLoginUser.bAccessOtherLoans && _curLoginUser.bIsCompanyUser))
            {
                sUserLoanList = string.Empty;
            }
            else
            {
                //sUserLoanList = _curLoginUser.GetUserLoanList();
                //sUserLoanList = sUserLoanList.Trim();
                sUserLoanList = "0000";
            }

            //gdc CR45
            if (_curLoginUser.userRole.ExportPipelines)
            {
                aExport.Enabled = true;
            }
            else
            {
                aExport.Enabled = false;
            }

            DataTable dtNewColumnHeader = bllUserLoansViewPointFields.GetUserLoansViewPointFieldsHeadingInfo(_curLoginUser.iUserID);
            foreach (DataRow drNewColumnHeader in dtNewColumnHeader.Rows)
            {
                BoundField bf = new BoundField();
                bf.HeaderText = drNewColumnHeader["Heading"].ToString();
                bf.SortExpression = "Borrower"; //it's not has the column when Bind Grid Data;  "CurrentValue" + drNewColumnHeader["PointFieldId"].ToString();
                bf.DataField = "CurrentValue" + drNewColumnHeader["PointFieldId"].ToString();
                if (!gvPipelineView.Columns.Contains(bf))
                {
                    gvPipelineView.Columns.Add(bf);
                }
              
            }
        

            BindPageDefaultValues();
            this.fromHomeFilter = FilterFromHomePiplineSummary();
            this.AdvacedSearch = this.BuildWhere_AdvancedSearch();
            BindLoanGrid();
          
        }

        //if (FirstTimeFlag == false)
        //{
        //    BindPageDefaultValues();
        //}

        if (_curLoginUser.bIsCompanyExecutive ||
                       (_curLoginUser.bAccessOtherLoans && _curLoginUser.bIsCompanyUser))
        {
            sUserLoanList = string.Empty;
        }
        else
        {
            //sUserLoanList = _curLoginUser.GetUserLoanList();
            //sUserLoanList = sUserLoanList.Trim();
            sUserLoanList = "0000";
        }

    }

    /// <summary>
    /// 
    /// </summary>
    private void BindUserPiplineView()
    {
        LPWeb.BLL.UserPipelineViews bll = new UserPipelineViews();

        ddlUserPipelineView.DataSource = bll.GetList_ViewName("PipelineType='Loans' AND Enabled = 1 AND UserID = " + CurrUser.iUserID.ToString(), "ViewName ASC");
        ddlUserPipelineView.DataBind();

        ddlUserPipelineView.Items.Insert(0, new ListItem() { Selected = true, Text = "--select--", Value = "0" });

        if (!IsPostBack && string.IsNullOrEmpty(Request.QueryString["q"]))
        {
            LPWeb.BLL.UserHomePref bllUHP = new UserHomePref();

            var model = bllUHP.GetModel(CurrUser.iUserID);
            if (model != null && model.UserId == CurrUser.iUserID)
            {
                if (model.DefaultLoansPipelineViewId > 0)
                {
                    IsDefaultView = true;
                    ddlUserPipelineView.SelectedValue = model.DefaultLoansPipelineViewId.ToString();
                    ddlUserPipelineView_SelectedIndexChanged(ddlUserPipelineView, new EventArgs());
                }
                else
                {
                    IsDefaultView = false;
                }
            }
        }

    }

    /// <summary>
    /// Filters from home pipline summary.
    /// </summary>
    /// <returns></returns>
    private string FilterFromHomePiplineSummary()
    {
        string searchCondition = string.Empty;
        
        if (!string.IsNullOrEmpty(Request.QueryString["q"]))
        {
            string qString = Encrypter.Base64Decode(Request.QueryString["q"]);
            //string qString = Request.QueryString["q"];
            //string qString = "RegionID=-1&DivisionID=8&BranchID=-1&DateType=CloseDate&FromDate=&ToDate=2010-11-08&Status=Processing&CurrentStage=Open";
            string[] args = qString.Split("&".ToCharArray());

            Dictionary<string, string> KVPs = new Dictionary<string, string>();
            try
            {
                KVPs = (from arg in args.Where(s => !string.IsNullOrEmpty(s)).ToList()
                        select new { key = arg.Split("=".ToCharArray())[0], value = arg.Split("=".ToCharArray())[1] }).
                    ToDictionary(p => p.key, p => p.value);
            }
            catch (Exception e)
            {
                LPLog.LogMessage(e.Message);
                return string.Empty;
            }

            if (KVPs["BranchID"] != "-1")
            {
                searchCondition += " and BranchID=" + KVPs["BranchID"];
            }
            else
            {
                if (KVPs["DivisionID"] != "-1")
                {
                    searchCondition += " and DivisionID=" + KVPs["DivisionID"];
                }
                else if (KVPs["RegionID"] != "-1")
                {
                    searchCondition += " and RegionID=" + KVPs["RegionID"];
                }
            }

            if (KVPs.ContainsKey("DateType") && !string.IsNullOrEmpty(KVPs["DateType"]))
            {
                #region DateType|FromDate|ToDate

                string sDateType = KVPs["DateType"];
                string sDateFiled = string.Empty;
                if (sDateType == "CloseDate")
                {
                    sDateFiled = "a.LastStageComplDate";
                }
                else
                {
                    sDateFiled = "a.LastStageComplDate";
                }
                
                string sFromDate = KVPs["FromDate"];
                string sToDate = KVPs["ToDate"];

                this.EstStartDate.Text = sFromDate;
                this.EstEndDate.Text = sToDate;

                DateTime? FromDate = null;
                DateTime? ToDate = null;

                if (sFromDate != string.Empty)
                {
                    DateTime FromDate1;
                    bool IsDate1 = DateTime.TryParse(sFromDate, out FromDate1);
                    if (IsDate1 == true)
                    {
                        FromDate = FromDate1;
                    }
                }

                if (sToDate != string.Empty)
                {
                    DateTime ToDate1;
                    bool IsDate2 = DateTime.TryParse(sToDate, out ToDate1);
                    if (IsDate2 == true)
                    {
                        ToDate = ToDate1;
                    }
                }

                searchCondition += SqlTextBuilder.BuildDateSearchCondition(sDateFiled, FromDate, ToDate);

                #endregion
            }

            if (KVPs.ContainsKey("Status") && !string.IsNullOrEmpty(KVPs["Status"]))
            {
                //searchCondition += " and [Status]='"+KVPs["Status"] + "'";
                if (KVPs["Status"] == "Processing")
                    ddlLoanStatus.SelectedValue = "Active Loans";
                else
                {
                    ddlLoanStatus.SelectedValue = "Archived Loans";
                    searchCondition += " and [Status] = '" + KVPs["Status"] + "'";
                }
                BindStages();

                if (ddlLoanStatus.SelectedValue == "Archived Loans") //if (KVPs["Status"].ToLower() == "Uncategorized Archive".ToLower()) //20111121 gdc CR28
                {
                    var selectVal = KVPs["Status"];
                    if (selectVal.ToLower() == "Uncategorized".ToLower())
                    {
                        selectVal = "Uncategorized Archive";
                    }
                    var item = this.ddlStage.Items.FindByText(selectVal);
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }
            }

            if (KVPs.ContainsKey("CurrentStage") && !string.IsNullOrEmpty(KVPs["CurrentStage"]))
            {
                string Stage = KVPs["CurrentStage"];
                string sStageFilter = "CurrentStages";
                if (KVPs.ContainsKey("StageFilter") && !string.IsNullOrEmpty(KVPs["StageFilter"]))
                {
                    sStageFilter = KVPs["StageFilter"];
                }

                if (sStageFilter == "CurrentStages")
                {
                    if (KVPs["Status"].ToLower() != "Uncategorized Archive".ToLower())
                    {
                        searchCondition += " and [Stage]='" + KVPs["CurrentStage"] + "'";
                        var item = this.ddlStage.Items.FindByValue("All Stages-" + Stage);
                        if (item != null)
                        {
                            item.Selected = true;
                        }
                    }
                }
                else
                {
                    if (KVPs["Status"].ToLower() != "Uncategorized Archive".ToLower())
                    {
                        searchCondition += " and [LastCompletedStage]='" + KVPs["CurrentStage"] + "'";
                        var item = this.ddlStage.Items.FindByValue("All Completed Stages-" + Stage);
                        if (item != null)
                        {
                            item.Selected = true;
                        }
                    }
                }
                //this.ddlStage.Items.Clear();
                //this.ddlStage.Items.Add(Stage);
                

            }

            if (KVPs.ContainsKey("LoanOfficerIDs") && !string.IsNullOrEmpty(KVPs["LoanOfficerIDs"]))
            {
                searchCondition += " and dbo.lpfn_GetLoanOfficerID(FileId) in (" + KVPs["LoanOfficerIDs"] + ")";
            }
        }
        return searchCondition;
    }
    /// <summary>
    /// Bind Page default values
    /// </summary>
    private void BindPageDefaultValues()
    {
        //Bind Alphabet
        const string alphabets = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        foreach (string alphabet in alphabets.Split(','))
        {
            ddlAlphabets.Items.Add(new ListItem(alphabet, alphabet));
        }

        // 获取当前用户信息
        //_curLoginUser = new LoginUser();

        //Bind region,division,branch dropdownlist
        BindFilter();
    }

    private void BindFilter()
    {
        //this.BindStages();

        //this.BindDateTypes();

        //this.BindOrganizations();

        //this.BindLeadSource();

        this.BindUserPiplineView();//CR45 gdc
    }

    /// <summary>
    /// 根据用户界面选择生成过滤条件
    /// </summary>
    /// <returns></returns>
    private string GenerateQueryCondition()
    {
        string sWhere = string.Empty;

        #region LoanStatus

        sLoanStatus = this.ddlLoanStatus.SelectedValue;
        if (sLoanStatus == "Active Loans") 
        {
            sWhere += " and [Status]='Processing'";
        }
        else if (sLoanStatus == "All Loans")
        {
            sWhere += " and [Status]!='Prospect'";
        }
        else if (sLoanStatus == "Archived Loans")
        {
            //sWhere += " and [Status]!='Processing' and [Status]!='Prospect' and ([Stage]='Canceled' or [Stage]='Closed' or [Stage]='Denied' or [Stage]='Suspended') ";
            sWhere += " and [Status]<>'Processing' and [Status]<>'Prospect' "; //gdc 20111121  CR28  Pipeline View – Loans
        }

        #endregion

        #region Organization

        string sOrganID = this.ddlOrgan.SelectedValue;
        if(sOrganID.StartsWith("Region") == true)
        {
            sWhere += string.Format(" and a.RegionId={0}", sOrganID.Replace("Region", string.Empty));
        }
        else if (sOrganID.StartsWith("Division") == true)
        {
            sWhere += string.Format(" and a.DivisionID={0}", sOrganID.Replace("Division", string.Empty));
        }
        else if (sOrganID.StartsWith("Branch") == true)
        {
            sWhere += string.Format(" and a.BranchID={0}", sOrganID.Replace("Branch", string.Empty));
        }
        else if (sOrganID.StartsWith("LoanOfficer") == true)
        {
            sWhere += string.Format(" and LoanOfficerID={0}", sOrganID.Replace("LoanOfficer", string.Empty));
        }
        else if (sOrganID.StartsWith("Processor") == true)
        {
            sWhere += string.Format(" and ProcessorID={0}", sOrganID.Replace("Processor", string.Empty));
        }
        else if (sOrganID.StartsWith("Underwriter") == true)
        {
            sWhere += string.Format(" and UnderwriterID={0}", sOrganID.Replace("Underwriter", string.Empty));
        }

         //gdc CR40
        else if (sOrganID.StartsWith("Closer-") == true)
        {
            sWhere += string.Format(" and Closer='{0}'", sOrganID.Replace("Closer-", string.Empty));
        }
        else if (sOrganID.StartsWith("Shipper-") == true)
        {
            sWhere += string.Format(" and Shipper='{0}'", sOrganID.Replace("Shipper-", string.Empty));
        }
        else if (sOrganID.StartsWith("DocPrep-") == true)
        {
            sWhere += string.Format(" and DocPrep='{0}'", sOrganID.Replace("DocPrep-", string.Empty));
        }
        else if (sOrganID.StartsWith("Assistant-") == true)
        {
            sWhere += string.Format(" and Assistant='{0}'", sOrganID.Replace("Assistant-", string.Empty));
        }
        else if (sOrganID.StartsWith("JrProcessor") == true) //gdc CR51
        {
            sWhere += string.Format(" and JrProcessor='{0}'", sOrganID.Replace("JrProcessor-", string.Empty));
        }

        #endregion

        #region Stage

        string sStage = this.ddlStage.SelectedValue.Trim();
        if (sStage == "All Stages")
        {
            sWhere += "";
        }
        else if (sStage == "All Completed Stages")
        {
            DataTable dtCompletedStageData = this.GetCompletedStages();
            if (dtCompletedStageData.Rows.Count > 0)
            {
                string sWhereStage = " AND (";
                foreach (DataRow compStageRow in dtCompletedStageData.Rows)
                {
                    if (compStageRow["LastCompletedStage"].ToString().Trim() == "")
                    {
                        continue;
                    }
                    sWhereStage += (sWhereStage == " AND (") ? ("[LastCompletedStage]='" + compStageRow["LastCompletedStage"].ToString().Trim() + "'") : (" OR [LastCompletedStage]='" + compStageRow["LastCompletedStage"].ToString().Trim() + "'");
                }
                sWhereStage += ")";

                sWhere += sWhereStage;
            }
        }
        else if (sStage.IndexOf("All Stages-") >= 0)
        {
            sStage = sStage.Replace("All Stages-", "");
            //20111121 gdc CR28
            if (sStage.ToLower().Trim() == "Uncategorized Archive".ToLower())
            {
                if (IsPostBack) //处理 点击filer时候的条件 直接连接过来的 会由fromHomeFilter处理， 增加判断以避免 出现二次条件
                {
                    sWhere += " and [Status]='Uncategorized Archive'";
                }
            }
            else
            {
                sWhere += string.Format(" and Stage = '{0}' ", sStage.Trim());
            }

        }
        else if (sStage.IndexOf("All Completed Stages-") >= 0)
        {
            sStage = sStage.Replace("All Completed Stages-", "");
            //20111121 gdc CR28
            if (sStage.ToLower().Trim() == "Uncategorized Archive".ToLower())
            {
                if (IsPostBack) //处理 点击filer时候的条件 直接连接过来的 会由fromHomeFilter处理， 增加判断以避免 出现二次条件
                {
                    sWhere += " and [LastCompletedStage]='Uncategorized Archive'";
                }
            }
            else
            {
                sWhere += string.Format(" and LastCompletedStage = '{0}' ", sStage.Trim());
            }
        }

        #endregion

        #region Lead Source

        string sLeadSourceID = this.ddlLeadSource.SelectedValue;

        if (sLeadSourceID.Contains("LeadSource") == true) 
        {
            sWhere += string.Format(" and LeadSource='{0}'", sLeadSourceID.Replace("LeadSource-", string.Empty));
        }
        else if (sLeadSourceID.Contains("Partner") == true)
        {
            sWhere += string.Format(" and PartnerID={0}", sLeadSourceID.Replace("Partner-", string.Empty));
        }
        else if (sLeadSourceID.Contains("Referral") == true)
        {
            sWhere += string.Format(" and ReferralID={0}", sLeadSourceID.Replace("Referral-", string.Empty));
        }
        else if (sLeadSourceID.Contains("Lender") == true)
        {
            sWhere += string.Format(" and LenderId='{0}'", sLeadSourceID.Replace("Lender-", string.Empty));
        }
        else if (sLeadSourceID.StartsWith("LoanProgram-") == true)
        {
            //sWhere += string.Format(" and LoanProgram='{0}'", sLeadSourceID.Replace("LoanProgram-", string.Empty));

            var loanProgram = sLeadSourceID.Replace("LoanProgram-", string.Empty);

            if (string.IsNullOrEmpty(loanProgram))
            {
                sWhere += string.Format(" AND (LoanProgram='{0}' Or  LoanProgram IS NULL) ", sLeadSourceID.Replace("LoanProgram-", string.Empty));
            }
            else
            {
                sWhere += string.Format(" AND  LoanProgram='{0}' ", sLeadSourceID.Replace("LoanProgram-", string.Empty));
            }
        }
            //gdc CR49
        else if (sLeadSourceID.StartsWith("Purpose-") == true)
        {
            //sWhere += string.Format(" and Purpose='{0}'", sLeadSourceID.Replace("Purpose-", string.Empty));

            var Purpose = sLeadSourceID.Replace("Purpose-", string.Empty);

            if (string.IsNullOrEmpty(Purpose))
            {
                sWhere += string.Format(" AND (Purpose='{0}' Or  Purpose IS NULL) ", sLeadSourceID.Replace("Purpose-", string.Empty));
            }
            else
            {
                sWhere += string.Format(" AND  Purpose='{0}' ", sLeadSourceID.Replace("Purpose-", string.Empty));
            }
        }

        #endregion

        #region Dates

        string sDateType = this.ddlDateType.SelectedValue;
        if(sDateType != "All dates")
        {
            string sDateFiled = string.Empty;
            if (sDateType == "Creation Date")
            {
                sDateFiled = "Created";
            }
            else if (sDateType == "Est Close Date")
            {
                sDateFiled = "EstClose";
            }
            else if (sDateType == "Dispose Date")
            {
                sDateFiled = "Disposed";
            }

            string sFromDate = this.EstStartDate.Text.Trim();
            string sToDate = this.EstEndDate.Text.Trim();

            DateTime? FromDate = null;
            DateTime? ToDate = null;

            if (sFromDate != string.Empty)
            {
                DateTime FromDate1;
                bool IsDate1 = DateTime.TryParse(sFromDate, out FromDate1);
                if (IsDate1 == true)
                {
                    FromDate = FromDate1;
                }
            }

            if (sToDate != string.Empty)
            {
                DateTime ToDate1;
                bool IsDate2 = DateTime.TryParse(sToDate, out ToDate1);
                if (IsDate2 == true)
                {
                    ToDate = ToDate1;
                }
            }

            sWhere += SqlTextBuilder.BuildDateSearchCondition(sDateFiled, FromDate, ToDate);
        }
        

        #endregion

        #region Rate

        string sRateStart = txbRateStart.Text.Replace("%", "").Trim();
        string sRateEnd = txbRateEnd.Text.Replace("%", "").Trim();
        if (!string.IsNullOrEmpty(sRateStart))
        {
            try
            {
                decimal rateStart = Convert.ToDecimal(sRateStart);

                sWhere += string.Format(" and Rate >= {0} ", rateStart);
            }
            catch {
                txbRateStart.Text = "";
            }
        }

        if (!string.IsNullOrEmpty(sRateEnd))
        {
            try
            {
                decimal rateEnd = Convert.ToDecimal(sRateEnd);

                sWhere += string.Format(" and Rate <= {0} ", rateEnd);
            }
            catch { txbRateEnd.Text = ""; }
        }


        #endregion

       
        if (!string.IsNullOrEmpty(fromHomeFilter))
        {
            sWhere += fromHomeFilter;
        }

        #region filter from search loan popup

        if (!string.IsNullOrEmpty(this.hiSearchFilter.Value))
        {
            string strReturn = Server.HtmlDecode(this.hiSearchFilter.Value);

            strReturn = strReturn.Replace("'", ""); //bug 2621

            string[] arrSearchPair = strReturn.Split('\u0001');
            foreach (string str in arrSearchPair)
            {
                string[] arrTemp = str.Split('\u0002');
                if (arrTemp.Length == 2)
                {
                    string strValue = string.Format("{0}", arrTemp[1]);
                    switch (arrTemp[0].ToLower())
                    {
                        case "lname":
                            sWhere += string.Format(" AND Borrower LIKE '{0}%'", strValue);
                            break;
                        case "fname":
                            sWhere += string.Format(" AND BorrowerFirstName LIKE '{0}%'", strValue);
                            break;
                        case "status":
                            sWhere += string.Format(" AND Status='{0}'", strValue);
                            break;
                        case "addr":
                            sWhere += string.Format(" AND PropertyAddr LIKE '%{0}%'", strValue);
                            break;
                        case "city":
                            sWhere += string.Format(" AND PropertyCity LIKE '%{0}%'", strValue);
                            break;
                        case "state":
                            sWhere += string.Format(" AND PropertyState='{0}'", strValue);
                            break;
                        case "zip":
                            sWhere += string.Format(" AND PropertyZip LIKE '{0}%'", strValue);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion

        return sWhere;
    }

    private string BuildWhere_AdvacedLoanFilters()
    {
        string sWhere = string.Empty;
        if (this.Request.QueryString["filter"] == null)
        {
            return string.Empty;
        }
        if (this.Request.QueryString["filter"] != "AdvacedLoanFilters")
        {
            return string.Empty;
        }

        #region RegionIDs

        if (this.Request.QueryString["RegionIDs"] != null)
        {
            string RegionIDs = this.Request.QueryString["RegionIDs"];
            sWhere += " and a.RegionId in (" + RegionIDs + ")";
        }

        #endregion

        #region DivisionIDs

        if (this.Request.QueryString["DivisionIDs"] != null)
        {
            string DivisionIDs = this.Request.QueryString["DivisionIDs"];
            sWhere += " and a.DivisionID in (" + DivisionIDs + ")";
        }

        #endregion

        #region BranchIDs

        if (this.Request.QueryString["BranchIDs"] != null)
        {
            string BranchIDs = this.Request.QueryString["BranchIDs"];
            sWhere += " and a.BranchID in (" + BranchIDs + ")";
        }

        #endregion

        #region FolderIDs

        if (this.Request.QueryString["FolderIDs"] != null)
        {
            string FolderIDs = this.Request.QueryString["FolderIDs"];
            sWhere += " and a.[Point Folder] in (select Name from PointFolders where FolderId in (" + FolderIDs + "))";
        }

        #endregion

        #region LoanOfficerIDs

        if (this.Request.QueryString["LoanOfficerIDs"] != null)
        {
            string LoanOfficerIDs = this.Request.QueryString["LoanOfficerIDs"];
            sWhere += " and LoanOfficerID in (" + LoanOfficerIDs + ")";
        }

        #endregion

        #region LoanOfficerAssistant

        if (this.Request.QueryString["LoanOfficerAssistant"] != null)
        {
            string LoanOfficerAssistant = this.Request.QueryString["LoanOfficerAssistant"];
            LoanOfficerAssistant = this.BuildNameStr(LoanOfficerAssistant);
            sWhere += " and Assistant in (" + LoanOfficerAssistant + ")";
        }

        #endregion

        #region ProcessorIDs

        if (this.Request.QueryString["ProcessorIDs"] != null)
        {
            string ProcessorIDs = this.Request.QueryString["ProcessorIDs"];
            sWhere += " and ProcessorID in (" + ProcessorIDs + ")";
        }

        #endregion

        #region JrProcessor

        if (this.Request.QueryString["JrProcessor"] != null)
        {
            string JrProcessor = this.Request.QueryString["JrProcessor"];
            JrProcessor = this.BuildNameStr(JrProcessor);
            sWhere += " and JrProcessor in (" + JrProcessor + ")";
        }

        #endregion

        #region DocPrep

        if (this.Request.QueryString["DocPrep"] != null)
        {
            string DocPrep = this.Request.QueryString["DocPrep"];
            DocPrep = this.BuildNameStr(DocPrep);
            sWhere += " and DocPrep in (" + DocPrep + ")";
        }

        #endregion

        #region Shipper

        if (this.Request.QueryString["Shipper"] != null)
        {
            string Shipper = this.Request.QueryString["Shipper"];
            Shipper = this.BuildNameStr(Shipper);
            sWhere += " and Shipper in (" + Shipper + ")";
        }

        #endregion

        #region Closer

        if (this.Request.QueryString["Closer"] != null)
        {
            string Closer = this.Request.QueryString["Closer"];
            Closer = this.BuildNameStr(Closer);
            sWhere += " and Closer in (" + Closer + ")";
        }

        #endregion

        #region UnderwriterIDs

        if (this.Request.QueryString["UnderwriterIDs"] != null)
        {
            string UnderwriterIDs = this.Request.QueryString["UnderwriterIDs"];
            sWhere += " and UnderwriterID in (" + UnderwriterIDs + ")";
        }

        #endregion

        #region BorrowerLastName

        if (this.Request.QueryString["BorrowerLastName"] != null)
        {
            string BorrowerLastName = this.Request.QueryString["BorrowerLastName"];
            sWhere += " and Borrower like '" + SqlTextBuilder.ConvertQueryValue(BorrowerLastName) + "%'";
        }

        #endregion

        #region LenderIDs

        if (this.Request.QueryString["LenderIDs"] != null)
        {
            string LenderIDs = this.Request.QueryString["LenderIDs"];
            sWhere += " and LenderId in (" + LenderIDs + ")";
        }

        #endregion

        #region Program

        if (this.Request.QueryString["Program"] != null)
        {
            string Program = this.Request.QueryString["Program"];
            Program = this.BuildNameStr(Program);
            sWhere += " and Program in (" + Program + ")";
        }

        #endregion

        #region PartnerIDs

        if (this.Request.QueryString["PartnerIDs"] != null)
        {
            string PartnerIDs = this.Request.QueryString["PartnerIDs"];
            sWhere += " and Partnerid in (" + PartnerIDs + ")";
        }

        #endregion

        #region LeadSource

        if (this.Request.QueryString["LeadSource"] != null)
        {
            string LeadSource = this.Request.QueryString["LeadSource"];
            LeadSource = this.BuildNameStr(LeadSource);
            sWhere += " and LeadSource in (" + LeadSource + ")";
        }

        #endregion

        #region ReferralIDs

        if (this.Request.QueryString["ReferralIDs"] != null)
        {
            string ReferralIDs = this.Request.QueryString["ReferralIDs"];
            sWhere += " and ReferralId in (" + ReferralIDs + ")";
        }

        #endregion

        #region Purpose

        if (this.Request.QueryString["Purpose"] != null)
        {
            string Purpose = this.Request.QueryString["Purpose"];
            Purpose = this.BuildNameStr(Purpose);
            sWhere += " and Purpose in (" + Purpose + ")";
        }

        #endregion

        #region CurrentStageIDs

        if (this.Request.QueryString["CurrentStageIDs"] != null)
        {
            string CurrentStageIDs = this.Request.QueryString["CurrentStageIDs"];
            sWhere += "and CurrentStage in (select Alias from Template_Stages where TemplStageId in (" + CurrentStageIDs + "))";
        }

        #endregion

        #region LoanStatus

        if (this.Request.QueryString["LoanStatus"] != null)
        {
            string LoanStatus = this.Request.QueryString["LoanStatus"];
            if (LoanStatus.Contains("Active"))
            {
                LoanStatus = LoanStatus.Replace("Active", "Processing");
            }

            LoanStatus = this.BuildNameStr(LoanStatus);
            sWhere += " and [Status] in (" + LoanStatus + ")";
        }

        #endregion

        #region StageCompletionStart and StageCompletionEnd

        DateTime? FromDate = null;
        DateTime? ToDate = null;

        if (this.Request.QueryString["StageCompletionStart"] != null)
        {
            string StageCompletionStart = this.Request.QueryString["StageCompletionStart"];
            DateTime FromDate1;
            bool IsDate1 = DateTime.TryParse(StageCompletionStart, out FromDate1);
            if (IsDate1 == true)
            {
                FromDate = FromDate1;
            }
        }

        if (this.Request.QueryString["StageCompletionEnd"] != null)
        {
            string StageCompletionEnd = this.Request.QueryString["StageCompletionEnd"];
            DateTime ToDate1;
            bool IsDate2 = DateTime.TryParse(StageCompletionEnd, out ToDate1);
            if (IsDate2 == true)
            {
                ToDate = ToDate1;
            }
        }

        string sWhere_StageCompletion = SqlTextBuilder.BuildDateSearchCondition("Completed", FromDate, ToDate);

        if (sWhere_StageCompletion != string.Empty)
        {
            sWhere += "and FileId in (select distinct FileId from LoanStages where 1=1 " + SqlTextBuilder.BuildDateSearchCondition("Completed", FromDate, ToDate) + ")";
        }

        #endregion

        #region EstimatedCloseStart and EstimatedCloseEnd

        FromDate = null;
        ToDate = null;

        if (this.Request.QueryString["EstimatedCloseStart"] != null)
        {
            string EstimatedCloseStart = this.Request.QueryString["EstimatedCloseStart"];
            DateTime FromDate1;
            bool IsDate1 = DateTime.TryParse(EstimatedCloseStart, out FromDate1);
            if (IsDate1 == true)
            {
                FromDate = FromDate1;
            }
        }

        if (this.Request.QueryString["EstimatedCloseEnd"] != null)
        {
            string EstimatedCloseEnd = this.Request.QueryString["EstimatedCloseEnd"];
            DateTime ToDate1;
            bool IsDate2 = DateTime.TryParse(EstimatedCloseEnd, out ToDate1);
            if (IsDate2 == true)
            {
                ToDate = ToDate1;
            }
        }

        string sWhere_EstimatedClose = SqlTextBuilder.BuildDateSearchCondition("EstClose", FromDate, ToDate);

        if (sWhere_EstimatedClose != string.Empty)
        {
            sWhere += sWhere_EstimatedClose;
        }

        #endregion

        #region LockExpirationStart and LockExpirationEnd

        FromDate = null;
        ToDate = null;

        if (this.Request.QueryString["LockExpirationStart"] != null)
        {
            string LockExpirationStart = this.Request.QueryString["LockExpirationStart"];
            DateTime FromDate1;
            bool IsDate1 = DateTime.TryParse(LockExpirationStart, out FromDate1);
            if (IsDate1 == true)
            {
                FromDate = FromDate1;
            }
        }

        if (this.Request.QueryString["LockExpirationEnd"] != null)
        {
            string LockExpirationEnd = this.Request.QueryString["LockExpirationEnd"];
            DateTime ToDate1;
            bool IsDate2 = DateTime.TryParse(LockExpirationEnd, out ToDate1);
            if (IsDate2 == true)
            {
                ToDate = ToDate1;
            }
        }

        string sWhere_LockExpiration = SqlTextBuilder.BuildDateSearchCondition("[Lock Expiration Date]", FromDate, ToDate);

        if (sWhere_LockExpiration != string.Empty)
        {
            sWhere += sWhere_LockExpiration;
        }

        #endregion

        #region CreationDateStart and CreationDateEnd

        FromDate = null;
        ToDate = null;

        if (this.Request.QueryString["CreationDateStart"] != null)
        {
            string CreationDateStart = this.Request.QueryString["CreationDateStart"];
            DateTime FromDate1;
            bool IsDate1 = DateTime.TryParse(CreationDateStart, out FromDate1);
            if (IsDate1 == true)
            {
                FromDate = FromDate1;
            }
        }

        if (this.Request.QueryString["CreationDateEnd"] != null)
        {
            string CreationDateEnd = this.Request.QueryString["CreationDateEnd"];
            DateTime ToDate1;
            bool IsDate2 = DateTime.TryParse(CreationDateEnd, out ToDate1);
            if (IsDate2 == true)
            {
                ToDate = ToDate1;
            }
        }

        string sWhere_CreationDate = SqlTextBuilder.BuildDateSearchCondition("Created", FromDate, ToDate);

        if (sWhere_CreationDate != string.Empty)
        {
            sWhere += sWhere_CreationDate;
        }

        #endregion

        #region PointFieldIDs and PointFieldValues

        string sWhere_PointField = string.Empty;
        if (this.Request.QueryString["PointFieldIDs"] != null && this.Request.QueryString["PointFieldValues"] != null && this.Request.QueryString["DataTypeNames"] != null)
        {
            string PointFieldIDs = this.Request.QueryString["PointFieldIDs"];
            string PointFieldValues = this.Request.QueryString["PointFieldValues"];
            string DataTypeNames = this.Request.QueryString["DataTypeNames"];

            if (PointFieldIDs != string.Empty)
            {
                string[] PointFieldIDArray = PointFieldIDs.Split(',');
                string[] PointFieldValueArray = Regex.Split(PointFieldValues, "\\$\\$");
                string[] DataTypeNameArray = DataTypeNames.Split(',');

                for (int i = 0; i < PointFieldIDArray.Length; i++)
                {
                    string PointFieldID = PointFieldIDArray[i];
                    string PointFieldValue = PointFieldValueArray[i];
                    string DataTypeName = DataTypeNameArray[i];

                    if (DataTypeName == "String")
                    {
                        if (PointFieldValue.Contains(";") == false)
                        {
                            sWhere_PointField += " and FileId in (select FileId from LoanPointFields where 1=1 and PointFieldId=" + PointFieldID + " and (CurrentValue like '" + PointFieldValue + "%')) ";
                        }
                        else
                        {
                            if (PointFieldValue.EndsWith(";") == true)
                            {
                                PointFieldValue = PointFieldValue.Substring(0, PointFieldValue.Length - 1);
                            }
                            string[] ValueItems = PointFieldValue.Split(';');
                            string s = string.Empty;
                            for (int j = 0; j < ValueItems.Length; j++)
                            {
                                string value = ValueItems[j];
                                if (j == 0)
                                {
                                    s = "CurrentValue like '" + value + "%'";
                                }
                                else
                                {
                                    s += " or CurrentValue like '" + value + "%'";
                                }
                            }

                            sWhere_PointField += " and FileId in (select FileId from LoanPointFields where 1=1 and PointFieldId=" + PointFieldID + " and (" + s + ")) ";
                        }
                    }
                    else if (DataTypeName == "Yes/No") 
                    {
                        sWhere_PointField += " and FileId in (select FileId from LoanPointFields where 1=1 and PointFieldId=" + PointFieldID + " and (CurrentValue = '" + PointFieldValue + "')) ";
                    }
                    else if (DataTypeName == "Numeric" || DataTypeName == "Date")
                    {
                        string[] TwoValues = PointFieldValue.Split(';');
                        string StartValue = TwoValues[0];
                        string EndValue = TwoValues[1];

                        if (DataTypeName == "Numeric")
                        {
                            if (StartValue != "null" && EndValue != "null")
                            {
                                if (StartValue == EndValue)
                                {
                                    sWhere_PointField += " and FileId in (select FileId from LoanPointFields where 1=1 and PointFieldId=" + PointFieldID + " and (CONVERT(numeric(18,3), CurrentValue) = '" + StartValue + "')) ";
                                }
                                else
                                {
                                    sWhere_PointField += " and FileId in (select FileId from LoanPointFields where 1=1 and PointFieldId=" + PointFieldID + " and (CONVERT(numeric(18,3), CurrentValue) >= '" + StartValue + "') and (CONVERT(numeric(18,3), CurrentValue) <= '" + EndValue + "')) ";
                                }
                            }
                            else if (StartValue != "null" && EndValue == "null")
                            {
                                sWhere_PointField += " and FileId in (select FileId from LoanPointFields where 1=1 and PointFieldId=" + PointFieldID + " and (CONVERT(numeric(18,3), CurrentValue) >= '" + StartValue + "')) ";
                            }
                            else if (StartValue == "null" && EndValue != "null")
                            {
                                sWhere_PointField += " and FileId in (select FileId from LoanPointFields where 1=1 and PointFieldId=" + PointFieldID + " and (CONVERT(numeric(18,3), CurrentValue) <= '" + EndValue + "')) ";
                            }
                        }
                        else // Date
                        {
                            if (StartValue != "null" && EndValue != "null")
                            {
                                if (StartValue == EndValue)
                                {
                                    sWhere_PointField += " and FileId in (select FileId from LoanPointFields where 1=1 and PointFieldId=" + PointFieldID + " and (CONVERT(datetime, CurrentValue) = '" + StartValue + "')) ";
                                }
                                else
                                {
                                    sWhere_PointField += " and FileId in (select FileId from LoanPointFields where 1=1 and PointFieldId=" + PointFieldID + " and (CONVERT(datetime, CurrentValue) >= '" + StartValue + "') and (CONVERT(datetime, CurrentValue) <= '" + EndValue + "')) ";
                                }
                            }
                            else if (StartValue != "null" && EndValue == "null")
                            {
                                sWhere_PointField += " and FileId in (select FileId from LoanPointFields where 1=1 and PointFieldId=" + PointFieldID + " and (CONVERT(datetime, CurrentValue) >= '" + StartValue + "')) ";
                            }
                            else if (StartValue == "null" && EndValue != "null")
                            {
                                sWhere_PointField += " and FileId in (select FileId from LoanPointFields where 1=1 and PointFieldId=" + PointFieldID + " and (CONVERT(datetime, CurrentValue) <= '" + EndValue + "')) ";
                            }
                        }
                    }
                }
            }
        }

        //  this.Response.Write("<h2>" + sWhere_PointField + "</h2>");

        if (sWhere_PointField != string.Empty)
        {
            sWhere += sWhere_PointField;
        }

        #endregion

        #region Task Name
        if (this.Request.QueryString["TaskNames"] != null)
        {
            string sTaskNames = this.Request.QueryString["TaskNames"];
            sTaskNames = Encrypter.Base64Decode(sTaskNames);
            string sTaskNameWhere = "";
            foreach (string sTaskName in sTaskNames.Split(','))
            {
                if (sTaskName != "")
                {
                    sTaskNameWhere += " and  " + sTaskName;
                }
            }
            if (sTaskNameWhere != "")
            {
                sTaskNameWhere = "1>0 " + sTaskNameWhere;
                sWhere += " and a.FileId in (select FileId from LoanTasks where " + sTaskNameWhere + ") ";
            }
        }

        if (this.Request.QueryString["DueDates"] != null)
        {
            string sDueDates = this.Request.QueryString["DueDates"];
            sDueDates = Encrypter.Base64Decode(sDueDates);
            string sDueDatesWhere = "";
            foreach (string sDueDate in sDueDates.Split(','))
            {
                if (sDueDate != "")
                {
                    string sDueDateStart = sDueDate.Split('|')[0].ToString();
                    string sDueDateEnd = sDueDate.Split('|')[1].ToString();
                    if (sDueDateStart != "")
                    {
                        sDueDatesWhere += " and Due >='" + sDueDateStart + "'";
                    }
                    if (sDueDateEnd != "")
                    {
                        sDueDatesWhere += " and Due <'" + Convert.ToDateTime(sDueDateEnd).AddDays(1) + "'";
                    }
                }
            }
            if (sDueDatesWhere != "")
            {
                sDueDatesWhere = " 1>0 " + sDueDatesWhere;
                sWhere += " and a.FileId in (select FileId from LoanTasks where " + sDueDatesWhere + ") ";
            }
        }

        if (this.Request.QueryString["CompDates"] != null)
        {
            string sCompDates = this.Request.QueryString["CompDates"];
            sCompDates = Encrypter.Base64Decode(sCompDates);
            string sCompDatesWhere = "";
            foreach (string sCompDate in sCompDates.Split(','))
            {
                if (sCompDate != "")
                {
                    string sCompDateStart = sCompDate.Split('|')[0].ToString();
                    string sCompDateEnd = sCompDate.Split('|')[1].ToString();
                    if (sCompDateStart != "")
                    {
                        sCompDatesWhere += " and Completed >='" + sCompDateStart + "'";
                    }
                    if (sCompDateEnd != "")
                    {
                        sCompDatesWhere += " and Completed <'" + Convert.ToDateTime(sCompDateEnd).AddDays(1) + "'";
                    }
                }
            }
            if (sCompDatesWhere != "")
            {
                sCompDatesWhere = " 1>0 " + sCompDatesWhere;
                sWhere += " and a.FileId in (select FileId from LoanTasks where " + sCompDatesWhere + ") ";
            }
        } 
        #endregion

        return sWhere;
    }

    private string BuildWhere_AdvancedSearch() 
    {
        string sWhere = string.Empty;
        if (this.Request.QueryString["Ads"] == null)
        {
            return string.Empty;
        }

        string DecodeQueryString = Encrypter.Base64Decode(Request.QueryString["Ads"]);

        if (string.IsNullOrEmpty(DecodeQueryString) == true)
        {
            return string.Empty;
        }

        if (this.Request.QueryString["LoanStatus"] != null)
        {
            ddlLoanStatus.SelectedValue = "Archived Loans";

            sWhere = " and a.FileId in (" + DecodeQueryString + ")";
        }
        else
        {
            sWhere = " and a.FileId in (" + DecodeQueryString + ")";
        }

        return sWhere;
    }

    private string BuildNameStr(string Names) 
    {
        Names = Names.Replace("'", "''").Replace("$$", "','");
        Names = "'" + Names + "'";
        return Names;
    }

    #region Bind Filter Data

    private void BindStages()
    {
        string sLoanStatus = this.ddlLoanStatus.SelectedValue;

        LPWeb.BLL.Template_Stages StagesTemplateManager = new Template_Stages();
        DataTable StageListData = null;
        if (sLoanStatus == "Active Loans") 
        {
            StageListData = StagesTemplateManager.GetStageTemplateList(" and WorkflowType='Processing' and [Enabled]=1 order by [Name] asc ");
        }
        else if (sLoanStatus == "All Loans")
        {
            StageListData = StagesTemplateManager.GetStageTemplateList(" and WorkflowType<>'Prospect' and [Enabled]=1 order by [Name] asc ");
        }
        DataTable dtCompletedStageData = this.GetCompletedStages();

        this.ddlStage.Items.Clear();

        this.ddlStage.Items.Add(new ListItem("All Stages","All Stages"));
        if (sLoanStatus == "Active Loans")
        {
            foreach (DataRow StageTempRow in StageListData.Rows)
            {
                string sStageName = StageTempRow["Alias"].ToString();
                this.ddlStage.Items.Add(new ListItem(sStageName,"All Stages-"+sStageName));
            }
        }
        else if (sLoanStatus == "All Loans")
        {
            foreach (DataRow StageTempRow in StageListData.Rows)
            {
                string sStageName = StageTempRow["Alias"].ToString();
                this.ddlStage.Items.Add(new ListItem(sStageName, "All Stages-" + sStageName));
            }

            this.ddlStage.Items.Add(new ListItem("Canceled","All Stages-Canceled"));
            this.ddlStage.Items.Add(new ListItem("Closed","All Stages-Closed"));
            this.ddlStage.Items.Add(new ListItem("Denied","All Stages-Denied"));
            //this.ddlStage.Items.Add(new ListItem("Suspended","All Stages-Suspended"));  //CR063
            this.ddlStage.Items.Add(new ListItem("Uncategorized Archive","All Stages-Uncategorized Archive"));
        }
        else if (sLoanStatus == "Archived Loans")
        {
            this.ddlStage.Items.Add(new ListItem("Canceled", "All Stages-Canceled"));
            this.ddlStage.Items.Add(new ListItem("Closed", "All Stages-Closed"));
            this.ddlStage.Items.Add(new ListItem("Denied", "All Stages-Denied"));
            //this.ddlStage.Items.Add(new ListItem("Suspended", "All Stages-Suspended"));  //CR063
            this.ddlStage.Items.Add(new ListItem("Uncategorized Archive", "All Stages-Uncategorized Archive"));
        }
        this.ddlStage.Items.Add(new ListItem("All Completed Stages","All Completed Stages"));
        foreach (DataRow compStageRow in dtCompletedStageData.Rows)
        {
            if (compStageRow["LastCompletedStage"].ToString().Trim() == "")
            {
                continue;
            }
            this.ddlStage.Items.Add(new ListItem(compStageRow["LastCompletedStage"].ToString(), "All Completed Stages-" + compStageRow["LastCompletedStage"].ToString()));
            
        }
    }

    private void BindDateTypes() 
    {
        string sLoanStatus = this.ddlLoanStatus.SelectedValue;
        
        this.ddlDateType.Items.Clear();

        this.ddlDateType.Items.Add("All dates");
        if (sLoanStatus == "Active Loans")
        {
            this.ddlDateType.Items.Add("Creation Date");
            this.ddlDateType.Items.Add("Est Close Date");
        }
        else 
        {
            this.ddlDateType.Items.Add("Creation Date");
            this.ddlDateType.Items.Add("Est Close Date");
            this.ddlDateType.Items.Add("Dispose Date");
        }

    }

    private void BindOrganizations() 
    {
        string sOrganType = this.ddlOrganType.SelectedValue;

        DataTable OrganListData = new DataTable();
        OrganListData.Columns.Add("OrganID", typeof(string));
        OrganListData.Columns.Add("OrganName", typeof(string));

        DataRow NewOrganRow = OrganListData.NewRow();
        NewOrganRow["OrganID"] = DBNull.Value;
        NewOrganRow["OrganName"] = "All organizations";
        OrganListData.Rows.Add(NewOrganRow);

        if (sOrganType == "All organization types")
        {
            #region Region

            DataTable RegionListData = this.GetRegionList();

            foreach (DataRow RegionRow in RegionListData.Rows)
            {
                string sRegionID = RegionRow["RegionID"].ToString();
                string sRegion = RegionRow["Name"].ToString();

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "Region" + sRegionID;
                NewOrganRow1["OrganName"] = sRegion;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion

            #region Division

            DataTable DivisionListData = this.GetDivisionList();

            foreach (DataRow DivisionRow in DivisionListData.Rows)
            {
                string sDivisionID = DivisionRow["DivisionID"].ToString();
                string sDivision = DivisionRow["Name"].ToString();

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "Division" + sDivisionID;
                NewOrganRow1["OrganName"] = sDivision;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion

            #region Branch

            DataTable BranchListData = this.GetBranchList();

            foreach (DataRow BranchRow in BranchListData.Rows)
            {
                string sBranchID = BranchRow["BranchID"].ToString();
                string sBranch = BranchRow["Name"].ToString();

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "Branch" + sBranchID;
                NewOrganRow1["OrganName"] = sBranch;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion

            #region Closer //gdc CR40

            DataTable CloserListData = this.GetOrganOther("Closer");

            foreach (DataRow dRow in CloserListData.Rows)
            {
                //string sCloserID = CloserRow["UserID"].ToString();
                string sFullName = dRow["Closer"].ToString();

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "Closer-" + sFullName;
                NewOrganRow1["OrganName"] = sFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion

            #region Doc Prep //gdc CR40

            DataTable DocPrepListData = this.GetOrganOther("DocPrep");

            foreach (DataRow dRow in DocPrepListData.Rows)
            {
                //string sCloserID = CloserRow["UserID"].ToString();
                string sFullName = dRow["DocPrep"].ToString();

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "DocPrep-" + sFullName;
                NewOrganRow1["OrganName"] = sFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion

            #region Loan Officer

            DataTable LoanOfficerListData = this.GetLoanOfficerList();

            foreach (DataRow LoanOfficerRow in LoanOfficerListData.Rows)
            {
                string sLoanOfficerID = LoanOfficerRow["UserID"].ToString();
                string sFirstName = LoanOfficerRow["FirstName"].ToString();
                string sLastName = LoanOfficerRow["LastName"].ToString();

                string sLoanOfficerFullName = sLastName + ", " + sFirstName;

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "LoanOfficer" + sLoanOfficerID;
                NewOrganRow1["OrganName"] = sLoanOfficerFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion

            #region Loan Officer Assistant //gdc CR40

            DataTable AssistantListData = this.GetOrganOther("Assistant");

            foreach (DataRow dRow in AssistantListData.Rows)
            {
                //string sCloserID = CloserRow["UserID"].ToString();
                string sFullName = dRow["Assistant"].ToString();

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "Assistant-" + sFullName;
                NewOrganRow1["OrganName"] = sFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion

            #region Processor

            DataTable ProcessorListData = this.GetProcessorList();

            foreach (DataRow ProcessorRow in ProcessorListData.Rows)
            {
                string sProcessorID = ProcessorRow["UserID"].ToString();
                string sFirstName = ProcessorRow["FirstName"].ToString();
                string sLastName = ProcessorRow["LastName"].ToString();

                string sProcessorFullName = sLastName + ", " + sFirstName;

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "Processor" + sProcessorID;
                NewOrganRow1["OrganName"] = sProcessorFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion

            #region Shipper //gdc CR40

            DataTable ShipperListData = this.GetOrganOther("Shipper");

            foreach (DataRow dRow in ShipperListData.Rows)
            {
                //string sCloserID = CloserRow["UserID"].ToString();
                string sFullName = dRow["Shipper"].ToString();

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "Shipper-" + sFullName;
                NewOrganRow1["OrganName"] = sFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion

            #region Underwriter

            DataTable UnderwriterListData = this.GetUnderwriterList();

            foreach (DataRow UnderwriterRow in UnderwriterListData.Rows)
            {
                string sUnderwriterID = UnderwriterRow["UserID"].ToString();
                string sFirstName = UnderwriterRow["FirstName"].ToString();
                string sLastName = UnderwriterRow["LastName"].ToString();

                string sUnderwriterFullName = sLastName + ", " + sFirstName;

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "Underwriter" + sUnderwriterID;
                NewOrganRow1["OrganName"] = sUnderwriterFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion

            #region JrProcessor //gdc CR51

            DataTable JrProcessorListData = this.GetOrganOther("JrProcessor");

            foreach (DataRow dRow in JrProcessorListData.Rows)
            {
                //string sCloserID = CloserRow["UserID"].ToString();
                string sFullName = dRow["JrProcessor"].ToString();

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "JrProcessor-" + sFullName;
                NewOrganRow1["OrganName"] = sFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion
        }
        else if (sOrganType == "Region")
        {
            #region Region

            DataTable RegionListData = this.GetRegionList();

            foreach (DataRow RegionRow in RegionListData.Rows)
            {
                string sRegionID = RegionRow["RegionID"].ToString();
                string sRegion = RegionRow["Name"].ToString();

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "Region" + sRegionID;
                NewOrganRow1["OrganName"] = sRegion;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion
        }
        else if (sOrganType == "Division")
        {
            #region Division

            DataTable DivisionListData = this.GetDivisionList();

            foreach (DataRow DivisionRow in DivisionListData.Rows)
            {
                string sDivisionID = DivisionRow["DivisionID"].ToString();
                string sDivision = DivisionRow["Name"].ToString();

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "Division" + sDivisionID;
                NewOrganRow1["OrganName"] = sDivision;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion
        }
        else if (sOrganType == "Branch")
        {
            #region Branch

            DataTable BranchListData = this.GetBranchList();

            foreach (DataRow BranchRow in BranchListData.Rows)
            {
                string sBranchID = BranchRow["BranchID"].ToString();
                string sBranch = BranchRow["Name"].ToString();

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "Branch" + sBranchID;
                NewOrganRow1["OrganName"] = sBranch;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion
        }
        else if (sOrganType == "Loan Officer")
        {
            #region Loan Officer

            DataTable LoanOfficerListData = this.GetLoanOfficerList();

            foreach (DataRow LoanOfficerRow in LoanOfficerListData.Rows)
            {
                string sLoanOfficerID = LoanOfficerRow["UserID"].ToString();
                string sFirstName = LoanOfficerRow["FirstName"].ToString();
                string sLastName = LoanOfficerRow["LastName"].ToString();

                string sLoanOfficerFullName = sLastName + ", " + sFirstName;

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "LoanOfficer" + sLoanOfficerID;
                NewOrganRow1["OrganName"] = sLoanOfficerFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion
        }
        else if (sOrganType == "Processor")
        {
            #region Processor

            DataTable ProcessorListData = this.GetProcessorList();

            foreach (DataRow ProcessorRow in ProcessorListData.Rows)
            {
                string sProcessorID = ProcessorRow["UserID"].ToString();
                string sFirstName = ProcessorRow["FirstName"].ToString();
                string sLastName = ProcessorRow["LastName"].ToString();

                string sProcessorFullName = sLastName + ", " + sFirstName;

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "Processor" + sProcessorID;
                NewOrganRow1["OrganName"] = sProcessorFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion
        }
        else if (sOrganType == "Underwriter")
        {
            #region Underwriter

            DataTable UnderwriterListData = this.GetUnderwriterList();

            foreach (DataRow UnderwriterRow in UnderwriterListData.Rows)
            {
                string sUnderwriterID = UnderwriterRow["UserID"].ToString();
                string sFirstName = UnderwriterRow["FirstName"].ToString();
                string sLastName = UnderwriterRow["LastName"].ToString();

                string sUnderwriterFullName = sLastName + ", " + sFirstName;

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "Underwriter" + sUnderwriterID;
                NewOrganRow1["OrganName"] = sUnderwriterFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion
        }

        //gdc CR40
        else if (sOrganType == "Closer")
        {
            #region Closer

            DataTable CloserListData = this.GetOrganOther("Closer");

            foreach (DataRow dRow in CloserListData.Rows)
            {
                //string sCloserID = CloserRow["UserID"].ToString();
                string sFullName = dRow["Closer"].ToString();

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "Closer-" + sFullName;
                NewOrganRow1["OrganName"] = sFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion
        }

        else if (sOrganType == "Doc Prep")
        {
            #region Doc Prep

            DataTable DocPrepListData = this.GetOrganOther("DocPrep");

            foreach (DataRow dRow in DocPrepListData.Rows)
            {
                //string sCloserID = CloserRow["UserID"].ToString();
                string sFullName = dRow["DocPrep"].ToString();

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "DocPrep-" + sFullName;
                NewOrganRow1["OrganName"] = sFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion
        }
        else if (sOrganType == "Loan Officer Assistant")
        {
            #region Loan Officer Assistant

            DataTable AssistantListData = this.GetOrganOther("Assistant");

            foreach (DataRow dRow in AssistantListData.Rows)
            {
                //string sCloserID = CloserRow["UserID"].ToString();
                string sFullName = dRow["Assistant"].ToString();

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "Assistant-" + sFullName;
                NewOrganRow1["OrganName"] = sFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion
        }
        else if (sOrganType == "Shipper")
        {
            #region Shipper

            DataTable ShipperListData = this.GetOrganOther("Shipper");

            foreach (DataRow dRow in ShipperListData.Rows)
            {
                //string sCloserID = CloserRow["UserID"].ToString();
                string sFullName = dRow["Shipper"].ToString();

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "Shipper-" + sFullName;
                NewOrganRow1["OrganName"] = sFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion
        }
            //gdc CR51
        else if (sOrganType == "Jr Processor")
        {
            #region JrProcessor

            DataTable JrProcessorListData = this.GetOrganOther("JrProcessor");

            foreach (DataRow dRow in JrProcessorListData.Rows)
            {
                //string sCloserID = CloserRow["UserID"].ToString();
                string sFullName = dRow["JrProcessor"].ToString();

                DataRow NewOrganRow1 = OrganListData.NewRow();
                NewOrganRow1["OrganID"] = "JrProcessor-" + sFullName;
                NewOrganRow1["OrganName"] = sFullName;
                OrganListData.Rows.Add(NewOrganRow1);
            }

            #endregion
        }

        this.ddlOrgan.DataSource = OrganListData;
        this.ddlOrgan.DataBind();
    }

    private void BindLeadSource() 
    {
        string sLeadSourceType = this.ddlLeadSourceType.SelectedValue;

        DataTable LeadSourceListData = new DataTable();
        LeadSourceListData.Columns.Add("LeadSourceID", typeof(string));
        LeadSourceListData.Columns.Add("LeadSourceName", typeof(string));

        DataRow NewLeadSourceRow = LeadSourceListData.NewRow();
        NewLeadSourceRow["LeadSourceID"] = DBNull.Value;
        NewLeadSourceRow["LeadSourceName"] = "All";
        LeadSourceListData.Rows.Add(NewLeadSourceRow);

        if (sLeadSourceType == "All") 
        {
            #region Lead Source

            DataTable LeadSourceListData1 = this.GetLeadSourceList();

            foreach (DataRow LeadSourceRow in LeadSourceListData1.Rows)
            {
                string sLeadSource = LeadSourceRow["LeadSource"].ToString();

                DataRow NewLeadSourceRow1 = LeadSourceListData.NewRow();
                NewLeadSourceRow1["LeadSourceID"] = "LeadSource-" + sLeadSource;
                NewLeadSourceRow1["LeadSourceName"] = sLeadSource;
                LeadSourceListData.Rows.Add(NewLeadSourceRow1);
            }

            #endregion

            #region Partner

            DataTable PartnerListData = this.GetPartnerList();

            foreach (DataRow PartnerRow in PartnerListData.Rows)
            {
                string sPartnerCompanyID = PartnerRow["PartnerCompanyID"].ToString();
                string sPartnerCompany = PartnerRow["PartnerCompany"].ToString();

                DataRow NewLeadSourceRow1 = LeadSourceListData.NewRow();
                NewLeadSourceRow1["LeadSourceID"] = "Partner-" + sPartnerCompanyID;
                NewLeadSourceRow1["LeadSourceName"] = sPartnerCompany;
                LeadSourceListData.Rows.Add(NewLeadSourceRow1);
            }

            #endregion

            #region Referral

            DataTable ReferralListData = this.GetReferralList();

            foreach (DataRow ReferralRow in ReferralListData.Rows)
            {
                string sReferralID = ReferralRow["ReferralID"].ToString();
                string sReferralName = ReferralRow["ReferralName"].ToString();

                DataRow NewLeadSourceRow1 = LeadSourceListData.NewRow();
                NewLeadSourceRow1["LeadSourceID"] = "Referral-" + sReferralID;
                NewLeadSourceRow1["LeadSourceName"] = sReferralName;
                LeadSourceListData.Rows.Add(NewLeadSourceRow1);
            }

            #endregion

            #region Lender

            DataTable LenderListData = this.GetLenderList();

            foreach (DataRow LenderRow in LenderListData.Rows)
            {
                string sLenderID = LenderRow["LenderID"].ToString();
                string sLenderName = LenderRow["LenderName"].ToString();

                DataRow NewLeadSourceRow1 = LeadSourceListData.NewRow();
                //NewLeadSourceRow1["LeadSourceID"] = "Lender-" + sLenderID;
                NewLeadSourceRow1["LeadSourceID"] = "Lender-" + sLenderID;
                NewLeadSourceRow1["LeadSourceName"] = sLenderName;
                LeadSourceListData.Rows.Add(NewLeadSourceRow1);
            }

            #endregion

            #region Loan Program

            DataTable LoanProgramListData = this.GetLoanProgram();

            foreach (DataRow LoanProgramRow in LoanProgramListData.Rows)
            {
                string sLoanProgram = LoanProgramRow["LoanProgram"].ToString();

                DataRow NewLeadSourceRow1 = LeadSourceListData.NewRow();

                NewLeadSourceRow1["LeadSourceID"] = "LoanProgram-" + sLoanProgram;
                NewLeadSourceRow1["LeadSourceName"] = sLoanProgram;
                LeadSourceListData.Rows.Add(NewLeadSourceRow1);
            }

            #endregion

           // GetPurpose()

            #region Purpose

            //DataTable PurposeListData = this.GetPurpose();

            //foreach (DataRow LoanProgramRow in PurposeListData.Rows)
            //{
            //    string sPurpose = LoanProgramRow["Purpose"].ToString();

            //    DataRow NewLeadSourceRow1 = LeadSourceListData.NewRow();

            //    NewLeadSourceRow1["LeadSourceID"] = "Purpose-" + sPurpose;
            //    NewLeadSourceRow1["LeadSourceName"] = sPurpose;
            //    LeadSourceListData.Rows.Add(NewLeadSourceRow1);
            //}
            DataRow NewLeadSourceRow0 = LeadSourceListData.NewRow();
            string sPurpose = "Cash-Out Refinance";
            NewLeadSourceRow0["LeadSourceID"] = "Purpose-" + sPurpose;
            NewLeadSourceRow0["LeadSourceName"] = sPurpose;
            LeadSourceListData.Rows.Add(NewLeadSourceRow0);

            DataRow NewLeadSourceRow2 = LeadSourceListData.NewRow();
            sPurpose = "Construction";
            NewLeadSourceRow2["LeadSourceID"] = "Purpose-" + sPurpose;
            NewLeadSourceRow2["LeadSourceName"] = sPurpose;
            LeadSourceListData.Rows.Add(NewLeadSourceRow2);

            DataRow NewLeadSourceRow3 = LeadSourceListData.NewRow();
            sPurpose = "Construction-Permanent";
            NewLeadSourceRow3["LeadSourceID"] = "Purpose-" + sPurpose;
            NewLeadSourceRow3["LeadSourceName"] = sPurpose;
            LeadSourceListData.Rows.Add(NewLeadSourceRow3);

            DataRow NewLeadSourceRow4 = LeadSourceListData.NewRow();
            sPurpose = "No Cash-Out Refinance";
            NewLeadSourceRow4["LeadSourceID"] = "Purpose-" + sPurpose;
            NewLeadSourceRow4["LeadSourceName"] = sPurpose;
            LeadSourceListData.Rows.Add(NewLeadSourceRow4);

            DataRow NewLeadSourceRow5 = LeadSourceListData.NewRow();
            sPurpose = "Purchase";
            NewLeadSourceRow5["LeadSourceID"] = "Purpose-" + sPurpose;
            NewLeadSourceRow5["LeadSourceName"] = sPurpose;
            LeadSourceListData.Rows.Add(NewLeadSourceRow5);
            #endregion


        }
        else if (sLeadSourceType == "Lead Source")
        {
            #region Lead Source

            DataTable LeadSourceListData1 = this.GetLeadSourceList();

            foreach (DataRow LeadSourceRow in LeadSourceListData1.Rows)
            {
                string sLeadSource = LeadSourceRow["LeadSource"].ToString();

                DataRow NewLeadSourceRow1 = LeadSourceListData.NewRow();
                NewLeadSourceRow1["LeadSourceID"] = "LeadSource-" + sLeadSource;
                NewLeadSourceRow1["LeadSourceName"] = sLeadSource;
                LeadSourceListData.Rows.Add(NewLeadSourceRow1);
            }

            #endregion
        }
        else if (sLeadSourceType == "Partner")
        {
            #region Partner

            DataTable PartnerListData = this.GetPartnerList();

            foreach (DataRow PartnerRow in PartnerListData.Rows)
            {
                string sPartnerCompanyID = PartnerRow["PartnerCompanyID"].ToString();
                string sPartnerCompany = PartnerRow["PartnerCompany"].ToString();

                DataRow NewLeadSourceRow1 = LeadSourceListData.NewRow();
                NewLeadSourceRow1["LeadSourceID"] = "Partner-" + sPartnerCompanyID;
                NewLeadSourceRow1["LeadSourceName"] = sPartnerCompany;
                LeadSourceListData.Rows.Add(NewLeadSourceRow1);
            }

            #endregion
        }
        else if (sLeadSourceType == "Referral")
        {
            #region Referral

            DataTable ReferralListData = this.GetReferralList();

            foreach (DataRow ReferralRow in ReferralListData.Rows)
            {
                string sReferralID = ReferralRow["ReferralID"].ToString();
                string sReferralName = ReferralRow["ReferralName"].ToString();

                DataRow NewLeadSourceRow1 = LeadSourceListData.NewRow();
                NewLeadSourceRow1["LeadSourceID"] = "Referral-" + sReferralID;
                NewLeadSourceRow1["LeadSourceName"] = sReferralName;
                LeadSourceListData.Rows.Add(NewLeadSourceRow1);
            }

            #endregion
        }
        else if (sLeadSourceType == "Lender")
        {
            #region Lender

            DataTable LenderListData = this.GetLenderList();

            foreach (DataRow LenderRow in LenderListData.Rows)
            {
                string sLenderID = LenderRow["LenderID"].ToString();
                string sLenderName = LenderRow["LenderName"].ToString();

                DataRow NewLeadSourceRow1 = LeadSourceListData.NewRow();
                NewLeadSourceRow1["LeadSourceID"] = "Lender-" + sLenderID;
                NewLeadSourceRow1["LeadSourceName"] = sLenderName;
                LeadSourceListData.Rows.Add(NewLeadSourceRow1);
            }

            #endregion
        }
        else if (sLeadSourceType == "Loan Program")
        {
            #region Loan Program

            DataTable LoanProgramListData = this.GetLoanProgram();

            foreach (DataRow LoanProgramRow in LoanProgramListData.Rows)
            {
                string sLoanProgram = LoanProgramRow["LoanProgram"].ToString();

                DataRow NewLeadSourceRow1 = LeadSourceListData.NewRow();

                NewLeadSourceRow1["LeadSourceID"] = "LoanProgram-" + sLoanProgram;
                NewLeadSourceRow1["LeadSourceName"] = sLoanProgram;
                LeadSourceListData.Rows.Add(NewLeadSourceRow1);
            }

            #endregion

        }
        else if (sLeadSourceType == "Purpose")
        {
            #region Purpose

            //DataTable PurposeListData = this.GetPurpose();

            //foreach (DataRow LoanProgramRow in PurposeListData.Rows)
            //{
            //    string sPurpose = LoanProgramRow["Purpose"].ToString();

                DataRow NewLeadSourceRow0 = LeadSourceListData.NewRow();
                string sPurpose = "Cash-Out Refinance";
                NewLeadSourceRow0["LeadSourceID"] = "Purpose-" + sPurpose;
                NewLeadSourceRow0["LeadSourceName"] = sPurpose;
                LeadSourceListData.Rows.Add(NewLeadSourceRow0);

                DataRow NewLeadSourceRow2 = LeadSourceListData.NewRow();
                sPurpose = "Construction";
                NewLeadSourceRow2["LeadSourceID"] = "Purpose-" + sPurpose;
                NewLeadSourceRow2["LeadSourceName"] = sPurpose;
                LeadSourceListData.Rows.Add(NewLeadSourceRow2);

                DataRow NewLeadSourceRow3 = LeadSourceListData.NewRow();
                sPurpose = "Construction-Permanent";
                NewLeadSourceRow3["LeadSourceID"] = "Purpose-" + sPurpose;
                NewLeadSourceRow3["LeadSourceName"] = sPurpose;
                LeadSourceListData.Rows.Add(NewLeadSourceRow3);

                DataRow NewLeadSourceRow4 = LeadSourceListData.NewRow();
                sPurpose = "No Cash-Out Refinance";
                NewLeadSourceRow4["LeadSourceID"] = "Purpose-" + sPurpose;
                NewLeadSourceRow4["LeadSourceName"] = sPurpose;
                LeadSourceListData.Rows.Add(NewLeadSourceRow4);

                DataRow NewLeadSourceRow5 = LeadSourceListData.NewRow();
                sPurpose = "Purchase";
                NewLeadSourceRow5["LeadSourceID"] = "Purpose-" + sPurpose;
                NewLeadSourceRow5["LeadSourceName"] = sPurpose;
                LeadSourceListData.Rows.Add(NewLeadSourceRow5);
            //}

            #endregion
        }

        this.ddlLeadSource.DataSource = LeadSourceListData;
        this.ddlLeadSource.DataBind();
    }

    #region Get Data for Organizations Filter

    private DataTable GetRegionList()
    {
        //LPWeb.BLL.Regions RegionManager = new LPWeb.BLL.Regions();

        DataTable RegionListData = null;

        //if (this.CurrUser.userRole.OtherLoanAccess == true)   // All Loans
        //{
        //    RegionListData = RegionManager.GetRegionList_AllLoans(this.CurrUser.iUserID);
        //}
        //else // Assigned Loans
        //{
        //    RegionListData = RegionManager.GetRegionList_AssingedLoans(this.CurrUser.iUserID);
        //}
        //string sqlCmd = string.Format("select distinct a.RegionID,a.[Name] from Regions a inner join V_ProcessingPipelineInfo b on a.RegionId=b.RegionId where b.FileId in (select LoanId from dbo.[lpfn_GetUserLoans2]({0}, {1})) order by a.[Name]", CurrUser.iUserID, (CurrUser.bAccessOtherLoans) ? 1 : 0);
        string sqlCmd = "select distinct a.RegionID,a.[Name] from Regions a where a.RegionID in ";
        if (this.CurrUser.bIsCompanyExecutive || this.CurrUser.bIsRegionExecutive || this.CurrUser.bIsDivisionExecutive)
            sqlCmd += string.Format("(select RegionID from dbo.lpfn_GetUserRegions_Executive({0}))", this.CurrUser.iUserID);
        else
            if (this.CurrUser.bIsBranchManager)
                sqlCmd += string.Format("(select RegionID from dbo.lpfn_GetUserRegions_Branch_Manager({0}))", this.CurrUser.iUserID);
            else
                sqlCmd += string.Format("(select RegionID from dbo.lpfn_GetUserRegions({0}))", this.CurrUser.iUserID);

        DataSet ds = LPWeb.DAL.DbHelperSQL.Query(sqlCmd);
        if (ds != null)
        {
            RegionListData = ds.Tables[0];
        }

        // sort
        DataView RegionDataView = new DataView(RegionListData);
        RegionDataView.Sort = "Name";

        return RegionDataView.ToTable();
    }


    private DataTable GetDivisionList()
    {
        LPWeb.BLL.Divisions DivisionManager = new LPWeb.BLL.Divisions();
        DataTable DivisionListData = null;

        //if (this.CurrUser.userRole.OtherLoanAccess == true)   // All Loans
        //{
        //    DivisionListData = DivisionManager.GetDivision_AllLoans(this.CurrUser.iUserID);
        //}
        //else // Assigned Loans
        //{
        //    DivisionListData = DivisionManager.GetDivisionList_AssingedLoans(this.CurrUser.iUserID);
        //}
        //string sqlCmd = string.Format("select distinct a.DivisionId,a.[Name] from Divisions a inner join V_ProcessingPipelineInfo b on a.DivisionId=b.DivisionId where b.FileId in (select LoanId from dbo.[lpfn_GetUserLoans2]({0}, {1})) order by a.[Name]", CurrUser.iUserID, (CurrUser.bAccessOtherLoans) ? 1 : 0);
        //string sqlCmd = "select distinct DivisionId, [Name] from Divisions where Enabled=1 order by [Name]";
        string sqlCmd = "select distinct a.DivisionId,a.[Name] from Divisions a where a.DivisionId in ";
        if (this.CurrUser.bIsCompanyExecutive || this.CurrUser.bIsRegionExecutive || this.CurrUser.bIsDivisionExecutive)
            sqlCmd += string.Format("(select DivisionId from dbo.lpfn_GetUserDivisions_Executive({0}))", this.CurrUser.iUserID);
        else
            if (this.CurrUser.bIsBranchManager)
                sqlCmd += string.Format("(select DivisionId from dbo.lpfn_GetUserDivisions_Branch_Manager({0}))", this.CurrUser.iUserID);
            else
                sqlCmd += string.Format("(select DivisionId from dbo.lpfn_GetUserDivisions({0}))", this.CurrUser.iUserID);
  
        DataSet ds = LPWeb.DAL.DbHelperSQL.Query(sqlCmd);
        if (ds != null)
        {
            DivisionListData = ds.Tables[0];
        }
        // sort
        DataView DivisionDataView = new DataView(DivisionListData);
        DivisionDataView.Sort = "Name";

        return DivisionDataView.ToTable();
    }

    private DataTable GetBranchList()
    {
        LPWeb.BLL.Branches BrancheManager = new LPWeb.BLL.Branches();
        DataTable BranchListData = null;

        //if (this.CurrUser.userRole.OtherLoanAccess == true)   // All Loans
        //{
        //    BranchListData = BrancheManager.GetBranchList_AllLoans(this.CurrUser.iUserID, 0, 0);
        //}
        //else // Assigned Loans
        //{
        //    BranchListData = BrancheManager.GetBranchList_AssingedLoans(this.CurrUser.iUserID, 0, 0);
        //}
        string sqlCmd = "select distinct a.BranchId,a.[Name] from Branches a where a.BranchId in ";
        if (this.CurrUser.bIsCompanyExecutive || this.CurrUser.bIsRegionExecutive || this.CurrUser.bIsDivisionExecutive)
            sqlCmd += string.Format("(select BranchId from dbo.lpfn_GetUserBranches_Executive({0}))", this.CurrUser.iUserID);
        else
            if (this.CurrUser.bIsBranchManager)
                sqlCmd += string.Format("(select BranchId from dbo.lpfn_GetUserBranches_Branch_Manager({0}))", this.CurrUser.iUserID);
            else
                sqlCmd += string.Format("(select BranchId from dbo.lpfn_GetUserBranches({0}))", this.CurrUser.iUserID);
        DataSet ds = LPWeb.DAL.DbHelperSQL.Query(sqlCmd);
        #region commented out
        //if (CurrUser.iRoleID == 1)
        //{
        //    sqlCmd = "select distinct BranchId, [Name] from Branches where ((GroupID > 0) and (Enabled =1)) order by [Name]";
        //    ds = LPWeb.DAL.DbHelperSQL.Query(sqlCmd);
        //}
        //else
        //{
        //    //sqlCmd = string.Format("select distinct a.BranchId,a.[Name] from Branches a inner join V_ProcessingPipelineInfo b on a.BranchId=b.BranchId where b.FileId in (select LoanId from dbo.[lpfn_GetUserLoans2]({0}, {1})) order by a.[Name]", CurrUser.iUserID, (CurrUser.bAccessOtherLoans) ? 1 : 0);

        //    sqlCmd = string.Format("select distinct a.BranchId,a.[Name] from Branches a inner join GroupUsers b on a.Groupid=b.GroupID where ((b.UserID={0}) and (a.Enabled=1)) order by a.[Name]", CurrUser.iUserID);
        //    ds = LPWeb.DAL.DbHelperSQL.Query(sqlCmd);
        //    if (ds == null)
        //    {
        //        sqlCmd = "select distinct BranchId, [Name] from Branches where ((GroupID > 0) and (Enabled =1)) order by [Name]";
        //        ds = LPWeb.DAL.DbHelperSQL.Query(sqlCmd);
        //    }
        //    else
        //    {
        //        if (ds.Tables[0].Rows.Count == 0)
        //        {
        //            sqlCmd = "select distinct BranchId, [Name] from Branches where ((GroupID > 0) and (Enabled =1)) order by [Name]";
        //            ds = LPWeb.DAL.DbHelperSQL.Query(sqlCmd);
        //        }
        //    }
        //}        
        #endregion
        if (ds != null)
        {
            BranchListData = ds.Tables[0];
        }
        // sort
        DataView BranchDataView = new DataView(BranchListData);
        BranchDataView.Sort = "Name";

        return BranchDataView.ToTable();
    }

    private DataTable GetLoanOfficerList() 
    {
        //string sSql = "select * from dbo.lpfn_GetAllLoanOfficer(" + this.CurrUser.iUserID + ") where [Enabled]=1 order by LastName";
        //string sSql = "select  from dbo.lpfn_GetAllLoanOfficer(" + this.CurrUser.iUserID + ") order by LastName"; //取消 enabled断的 以满足 Bug #1041

        //string sSql = "SELECT * FROM dbo.lpfn_GetAllLoanOfficer(" + this.CurrUser.iUserID + ") "
        //        + " WHERE UserId IN(SELECT userID FROM loanteam WHERE RoleId =(SELECT RoleId FROM roles WHERE Name='Loan Officer')) order by LastName";

        string sSql = " select distinct lt.UserID,LastName,FirstName from dbo.LoanTeam lt "
                    + " inner join Roles r on lt.RoleId =r.RoleId  and r.Name ='Loan Officer' "
                    + " inner join Users u on u.UserId =lt.UserID order by LastName,FirstName ";
        DataTable LoanOfficerListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        return LoanOfficerListData;
    }

    private DataTable GetProcessorList()
    {
        //string sSql = "select * from dbo.lpfn_GetAllProcessor(" + this.CurrUser.iUserID + ") where [Enabled]=1 order by LastName";

        //string sSql = "select * from dbo.lpfn_GetAllProcessor(" + this.CurrUser.iUserID + ")"
        //        + " WHERE UserId IN(SELECT userID FROM loanteam WHERE RoleId =(SELECT RoleId FROM roles WHERE Name='Processor')) order by LastName";

        string sSql = " select distinct lt.UserID,LastName,FirstName from dbo.LoanTeam lt "
                    + " inner join Roles r on lt.RoleId =r.RoleId  and r.Name ='Processor' "
                    + " inner join Users u on u.UserId =lt.UserID order by LastName,FirstName ";
    
        DataTable ProcessorListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        return ProcessorListData;
    }

    private DataTable GetUnderwriterList()
    {
        //string sSql = "select * from dbo.lpfn_GetAllUnderwriter(" + this.CurrUser.iUserID + ") where [Enabled]=1 order by LastName";

        //string sSql = "select * from dbo.lpfn_GetAllUnderwriter(" + this.CurrUser.iUserID + ") "
        //        + " WHERE UserId IN(SELECT userID FROM loanteam WHERE RoleId =(SELECT RoleId FROM roles WHERE Name='Underwriter')) order by LastName";

        string sSql = " select distinct lt.UserID,LastName,FirstName from dbo.LoanTeam lt "
                    + " inner join Roles r on lt.RoleId =r.RoleId  and r.Name ='Underwriter' "
                    + " inner join Users u on u.UserId =lt.UserID order by LastName,FirstName ";
        
        DataTable UnderwriterListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        return UnderwriterListData;
    }
    /// <summary>
    /// Closer/Doc Prep/Assistant/Shipper  //gdc CR40
    /// </summary>
    /// <returns></returns>
    private DataTable GetOrganOther(string typeName)
    {
        string sSql = " Select Distinct " + typeName + " from V_ProcessingPipelineInfo where " + typeName + " is not null and " + typeName + " <> ''";

        DataTable ListData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        DataView dv = new DataView(ListData);
        dv.Sort = typeName;

        return dv.ToTable();
    }


    #endregion

    #region Get Data for Lead Source Filter

    private DataTable GetLeadSourceList() 
    {
        string sWhere = string.Empty;
        
        //string sSql = "select distinct c.LeadSource from Loans as a left outer join LoanContacts as b on a.FileId=b.FileId "
        //            + "left outer join Prospect as c on b.ContactId=c.Contactid inner join dbo.lpfn_GetUserLoans(" + this.CurrUser.iUserID + ") as d on a.FileId=d.LoanID "
        //            + "where c.LeadSource is not null and c.LeadSource!='' and b.ContactRoleId=dbo.lpfn_GetBorrowerRoleId()" + sWhere;
        //string sSql = "select distinct LeadSource from V_ProcessingPipelineInfo where LeadSource IS NOT NULL " + sWhere + " Order by LeadSource ASC"; 
        string sSql = "Select DISTINCT LeadSource from Company_Lead_Sources order by LeadSource asc";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    private DataTable GetPartnerList()
    {
        string sWhere = string.Empty;

        //string sSql = "select distinct dbo.lpfn_GetContactCompanyId(c.Referral) as PartnerCompanyID, dbo.lpfn_GetContactCompanyName(c.Referral) as PartnerCompany from Loans as a left outer join LoanContacts as b on a.FileId=b.FileId "
        //            + "left outer join Prospect as c on b.ContactId=c.Contactid inner join dbo.lpfn_GetUserLoans(" + this.CurrUser.iUserID + ") as d on a.FileId=d.LoanID "
        //            + "where c.Referral is not null and c.Referral!='' and b.ContactRoleId=dbo.lpfn_GetBorrowerRoleId() " + sWhere + " order by PartnerCompany ";
        string sSql = "select distinct PartnerId as PartnerCompanyID, Partner as PartnerCompany from V_ProcessingPipelineInfo where (1=1) and (PartnerID > 0) " + sWhere + " order by PartnerCompany asc ";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    private DataTable GetReferralList()
    {
        string sWhere = string.Empty;

        //string sSql = "select distinct dbo.lpfn_GetContactName(c.Referral) as ReferralName, c.Referral as ReferralID from Loans as a left outer join LoanContacts as b on a.FileId=b.FileId "
        //            + "left outer join Prospect as c on b.ContactId=c.Contactid  inner join dbo.lpfn_GetUserLoans(" + this.CurrUser.iUserID + ") as d on a.FileId=d.LoanID "
        //            + "where c.Referral is not null and c.Referral!='' and b.ContactRoleId=dbo.lpfn_GetBorrowerRoleId() " + sWhere + " order by ReferralName ";
        string sSql = "select distinct ReferralId as ReferralID, Referral as ReferralName from V_ProcessingPipelineInfo where (1=1) and (ReferralID > 0) " + sWhere + " order by ReferralName asc ";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    private DataTable GetLenderList()
    {
        string sWhere = string.Empty;

        //string sSql = "select distinct b.ContactId as LenderID, dbo.lpfn_GetContactName(b.ContactId) as LenderName from Loans as a inner join LoanContacts as b on a.FileId=b.FileId "
        //            + "inner join ContactRoles as c on b.ContactRoleId=c.ContactRoleId  inner join dbo.lpfn_GetUserLoans(" + this.CurrUser.iUserID + ") as d on a.FileId=d.LoanID "
        //            + "where c.Name='Lender' " + sWhere + " order by LenderName ";
        string sSql = "select distinct LenderId as LenderID, Lender as LenderName from V_ProcessingPipelineInfo where (1=1) and LenderID IS NOT NULL " + sWhere + " order by LenderName asc ";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }

    //gdc CR47
    private DataTable GetLoanProgram()
    {
        //string sSql = string.Format("SELECT DISTINCT LoanProgram FROM V_ProcessingPipelineInfo WHERE LoanProgram IS NOT NULL AND LoanProgram<>'' ORDER BY LoanProgram ");
        string sSql = "select DISTINCT LoanProgram from Company_Loan_Programs order by LoanProgram";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }
    //gdc CR49
    private DataTable GetPurpose()
    {
        //string sSql = string.Format("SELECT DISTINCT Purpose FROM V_ProcessingPipelineInfo WHERE Purpose IS NOT NULL AND Purpose<>'' ORDER BY Purpose ");
        //string sSql = 
        //return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        return null;
    }
    //CR54
    private DataTable GetCompletedStages()
    {
        //string sSql = string.Format("SELECT DISTINCT LastCompletedStage FROM V_ProcessingPipelineInfo WHERE ISNULL(LastCompletedStage,'') <> '' ORDER BY LastCompletedStage");
        string sSql = "select DISTINCT [Name] as LastCompletedStage from dbo.Template_Stages where WorkflowType='Processing' AND Enabled=1 order by [Name]";
        return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
    }
    #endregion

    #endregion

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        //CheackIsChangeFiler(); //gdc CR45
        isReset = true;       
        BindLoanGrid();
    }

    protected void ddlAlphabets_SelectedIndexChanged(object sender, EventArgs e)
    {
        isReset = true;
        BindLoanGrid();
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        if (FirstTimeFlag == true)
        {
            ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
            return;
        }
        if (ViewState["pageIndex"] != null)
        {
            viewstateIdx = (int)ViewState["pageIndex"];
            currentpageIdx = AspNetPager1.CurrentPageIndex;
            if (viewstateIdx != currentpageIdx)
            {
                ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
                if (FirstTimeFlag == false)
                {
                    BindLoanGrid();
                }
            }
        }
    }

    /// <summary>
    /// Bind Grid
    /// </summary>
    private void BindLoanGrid()
    {
        //if (IsGrid == true)
        //    return;

        IsGrid = true;
        int pageSize = AspNetPager1.PageSize;
        try
        {
            if (_curLoginUser != null)
            {
                Users users = new Users();
                int iLoansPerPage = 0;
                //LPWeb.Model.Users u = users.GetModel(_curLoginUser.iUserID);
                iLoansPerPage = users.GetLoansPerPage(_curLoginUser.iUserID);
                if (iLoansPerPage != 0)
                {
                    pageSize = iLoansPerPage;
                    AspNetPager1.PageSize = iLoansPerPage;
                }
                else
                {
                    pageSize = 20;
                    AspNetPager1.PageSize = 20;
                }
                //_curLoginUser.
            }
        }
        catch (Exception exception)
        {
            pageSize = 20;
            AspNetPager1.PageSize = 20;
            LPLog.LogMessage(exception.Message);
        }

        int pageIndex = 1;

        if (FirstTimeFlag == true)
            pageIndex = AspNetPager1.CurrentPageIndex = 1;
        else
            pageIndex = AspNetPager1.CurrentPageIndex;

        // Advanced Loan Filter
        this.AdvacedLoanFilters = this.BuildWhere_AdvacedLoanFilters();               

        string queryCondition = string.Empty;
        if (string.IsNullOrEmpty(this.AdvacedLoanFilters) == false)
        {
            // from Advanced Loan Filter
            if (ViewIndexChanged == true)
            {
                queryCondition = this.AdvancedLoanFilters;
            }
            else
            {
                if (isView_To_Select == false)
                {
                    queryCondition = this.AdvacedLoanFilters;
                    Change_To_Select_After_Filter = true;
                    ddlUserPipelineView.SelectedValue = "0";
                }
            }
        }
        else
        {
            if (ddlUserPipelineView.SelectedValue != "0")
            {
                ddlUserPipelineView_SelectedIndexChanged(ddlUserPipelineView, new EventArgs());
            }

            if ((isView == true) && (string.IsNullOrEmpty(this.AdvancedLoanFilters) == false))
            {
                queryCondition = this.AdvancedLoanFilters;
            }
            else
            {
                queryCondition = GenerateQueryCondition();
            }
        }
        
        if (!string.IsNullOrEmpty(sUserLoanList))
        {
            //queryCondition = string.Format(" AND a.FileId in ({0}) ", sUserLoanList) + queryCondition;
            if (_curLoginUser.sRoleName == "Executive")
            {
                queryCondition = string.Format(" AND a.FileId in (SELECT LoanID FROM dbo.[lpfn_GetUserLoans_Executive] ({0})) ", CurrUser.iUserID ) + queryCondition;
            }
            else
            {
                if (_curLoginUser.sRoleName == "Branch Manager")
                {
                    queryCondition = string.Format(" AND a.FileId in (SELECT LoanID FROM dbo.[lpfn_GetUserLoans_Branch_Manager] ({0})) ", CurrUser.iUserID ) + queryCondition;
                }
                else
                {
                    queryCondition = string.Format(" AND a.FileId in (SELECT LoanID FROM dbo.[lpfn_GetUserLoans2] ('{0}', '{1}')) ", CurrUser.iUserID, CurrUser.bAccessOtherLoans) + queryCondition;
                }
            }
        }
        if (!string.IsNullOrEmpty(fromHomeFilter))
        {
            queryCondition += fromHomeFilter;
        }
        if (!string.IsNullOrEmpty(this.AdvacedSearch))
        {
            queryCondition += this.AdvacedSearch;
        }

        //alphabets 
        if (!string.IsNullOrEmpty(ddlAlphabets.SelectedValue))
        {
            queryCondition += string.Format(" and [Borrower] Like '{0}%'", ddlAlphabets.SelectedValue);
        }

        int recordCount = 0;        

        DataSet loanLists = loanLists = _bllLoans.GetLoanPipelineList(this.CurrUser.iUserID, pageSize, pageIndex, queryCondition, out recordCount, OrderName, OrderType);

        #region Base64 QueryCondition SQL To HiddenField For Export

        try
        {
            hidFilterQueryCondition.Value = Encrypter.Base64Encode(queryCondition).Replace("+", "_99_");
            hidrecordTotal.Value = recordCount.ToString();
        }
        catch { }
        #endregion
       
        AspNetPager1.PageSize = pageSize;
        AspNetPager1.RecordCount = recordCount;
        AspNetPager1.CurrentPageIndex = pageIndex;

        GetBorrowerContactIdLists(ref loanLists);
        //得到RuleAlertID
        GetRuleAlertloanLists(ref loanLists);
        GetStageLists(ref loanLists);
        //得到LastNote
        GetLastNoteLists(ref loanLists);
        //Points Fields
        GetPointFieldsLists(ref loanLists);

        gvPipelineView.DataSource = loanLists;
        gvPipelineView.DataBind();

        UserColumn(gvPipelineView);

        DataTable table = _bllLoans.GetTotalInfo(CurrUser.iUserID, queryCondition);

        labelTotalLoans.Text = table.Rows[0]["TotalFileId"].ToString();
        labelTotalVolume.Text = table.Rows[0].IsNull("TotalAmount") || string.IsNullOrEmpty(table.Rows[0]["TotalAmount"].ToString()) ? "$0" : Convert.ToDecimal(table.Rows[0]["TotalAmount"].ToString()).ToString("C0");
        CurrentPage = pageIndex;
    }

    private void UserColumn(GridView gridView)
    {
        bool defaultValue = false;
        LPWeb.Model.UserPipelineColumns modUPC = null;
        try
        {
            if (_curLoginUser != null)
            {
                modUPC = _bllUPC.GetModel(_curLoginUser.iUserID);
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }
       
        gridView.Columns[2].Visible = modUPC == null ? defaultValue : modUPC.Stage;
        gridView.Columns[3].Visible = modUPC == null ? defaultValue : modUPC.Purpose; //gdc CR49
        gridView.Columns[4].Visible = modUPC == null ? defaultValue : modUPC.LastCompletedStage;
        gridView.Columns[5].Visible = modUPC == null ? defaultValue : modUPC.LastStageComplDate;
        gridView.Columns[6].Visible = modUPC == null ? defaultValue : modUPC.Alerts;
        gridView.Columns[7].Visible = modUPC == null ? defaultValue : modUPC.PercentCompl;
        gridView.Columns[8].Visible = modUPC == null ? defaultValue : modUPC.EstimatedClose;
        gridView.Columns[9].Visible = modUPC == null ? defaultValue : modUPC.LoanOfficer;

        gridView.Columns[10].Visible = modUPC == null ? defaultValue : modUPC.Amount;
        gridView.Columns[11].Visible = modUPC == null ? defaultValue : modUPC.Lien;
        gridView.Columns[12].Visible = modUPC == null ? defaultValue : modUPC.Rate;
        gridView.Columns[13].Visible = modUPC == null ? defaultValue : modUPC.Lender;
        gridView.Columns[14].Visible = modUPC == null ? defaultValue : modUPC.LockExp;
        gridView.Columns[15].Visible = modUPC == null ? defaultValue : modUPC.Branch;
        gridView.Columns[16].Visible = modUPC == null ? defaultValue : modUPC.Processor;
        gridView.Columns[17].Visible = modUPC == null ? defaultValue : modUPC.TaskCount;

        gridView.Columns[18].Visible = modUPC == null ? defaultValue : modUPC.PointFolder;
        gridView.Columns[19].Visible = modUPC == null ? defaultValue : modUPC.PointFileName;
        gridView.Columns[20].Visible = modUPC == null ? defaultValue : modUPC.Closer;
        gridView.Columns[21].Visible = modUPC == null ? defaultValue : modUPC.DocPrep;
        gridView.Columns[22].Visible = modUPC == null ? defaultValue : modUPC.Assistant;
        gridView.Columns[23].Visible = modUPC == null ? defaultValue : modUPC.Shipper;

        gridView.Columns[24].Visible = modUPC == null ? defaultValue : modUPC.LoanProgram;//gdc CR47

        gridView.Columns[25].Visible = modUPC == null ? defaultValue : modUPC.JrProcessor;//gdc CR51

        gridView.Columns[26].Visible = modUPC == null ? defaultValue : modUPC.LastLoanNote; //CR56 应修改为  modUPC.LastLoanNote 

       
      
    }

    private void GetRuleAlertloanLists(ref DataSet LoadLists)
    {
        if (LoadLists == null)
            return;
        DataTable dt = LoadLists.Tables[0];
        if (!dt.Columns.Contains("AlertID"))
        {
            dt.Columns.Add("AlertID");

            foreach (DataRow dr in dt.Rows)
            {
                dr["AlertID"] = _loanAlerts.GetRuleAlertID(Convert.ToInt32(dr["FileId"]));
            }
        }
        dt.AcceptChanges();
    }

    private void GetStageLists(ref DataSet LoadLists)
    {
        if (LoadLists == null)
            return;
        DataTable dt = LoadLists.Tables[0];
        string status = "";
        if (dt.Columns.Contains("Stage"))
        {            
            foreach (DataRow dr in dt.Rows)
            {
                status = (string)dr["Status"];
                if ((status != null) && (status != "Processing") && (status != "Prospect"))
                {
                    dr["Stage"] = status;
                }                
            }
        }
        dt.AcceptChanges();
    }

    private void GetBorrowerContactIdLists(ref DataSet LoadLists)
    {
        if (LoadLists == null)
            return;
        DataTable dt = LoadLists.Tables[0];
        int? value = 0;

        foreach (DataRow dr in dt.Rows)
            {
                value = _bllLoans.GetBorrowerID(Convert.ToInt32(dr["FileId"]));
                if (value == null)
                {
                    dr["ContactId"] = DBNull.Value;
                }
                else
                {
                    dr["ContactId"] = value;
                }
            }
      
        dt.AcceptChanges();
    }
    
    private void GetPointFieldsLists(ref DataSet LoadLists)
    {
        if (LoadLists == null)
            return;
        int iExistUserLoansViewPointField = bllUserLoansViewPointFields.GetUserLoansViewPointFieldsCount(_curLoginUser.iUserID);
        if (iExistUserLoansViewPointField == 0)
        {
            return;
        }
        DataTable dt = LoadLists.Tables[0];
        DataTable dtNewColumnHeader = bllUserLoansViewPointFields.GetUserLoansViewPointFieldsHeadingInfo(_curLoginUser.iUserID);
      
        
        int i = 0;
        int n = 27;

        foreach (DataRow drNewColumnHeader in dtNewColumnHeader.Rows)
        {
            BoundField bf = new BoundField();
            bf.HeaderText = drNewColumnHeader["Heading"].ToString();
            bf.SortExpression = "FileId"; //it's not has the column when Bind Grid Data;  "CurrentValue" + drNewColumnHeader["PointFieldId"].ToString();
            bf.DataField = "CurrentValue" + drNewColumnHeader["PointFieldId"].ToString();
           
            if (!dt.Columns.Contains("CurrentValue" + drNewColumnHeader["PointFieldId"].ToString()))
            {
                dt.Columns.Add("CurrentValue" + drNewColumnHeader["PointFieldId"].ToString());
            }
            n++;
            i++;
        }

        if (i == 0)
        {
            return;
        }
        foreach (DataRow dr in dt.Rows)
        {
            foreach (DataRow drNewColumnHeader in dtNewColumnHeader.Rows)
            {
                dr["CurrentValue" + drNewColumnHeader["PointFieldId"].ToString()] = bllUserLoansViewPointFields.GetUserLoansViewPointFieldsCurrentValue(_curLoginUser.iUserID, Convert.ToInt32(dr["FileId"]), Convert.ToInt32(drNewColumnHeader["PointFieldId"]));
            }
        }
        dt.AcceptChanges();
      
    }

    private void GetLastNoteLists(ref DataSet LoadLists)
    {
        string stg = string.Empty;

        if (LoadLists == null)
            return;
        LPWeb.Model.UserPipelineColumns modUPC = null;
        try
        {
            if (_curLoginUser != null)
            {
                modUPC = _bllUPC.GetModel(_curLoginUser.iUserID);
            }
        }
        catch (Exception exception)
        {
            LPLog.LogMessage(exception.Message);
        }
        if (modUPC != null && modUPC.LastLoanNote)
        {
            DataTable dt = LoadLists.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                stg = string.Empty;
                stg = _loanNotes.GetLastNoteByFileID(Convert.ToInt32(dr["FileId"]));
                stg = stg.Replace("<", "&lt;");
                dr["LastNote"] = stg.Replace(">", "&gt;");
            }
            dt.AcceptChanges();
        }
    }

    /// <summary>
    /// Handles the Sorting event of the gvPipelineView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
    protected void gvPipelineView_Sorting(object sender, GridViewSortEventArgs e)
    {
        OrderName = e.SortExpression;
        OrderName = "[" + OrderName.Trim() + "]";
        string sortExpression = e.SortExpression;
        if (GridViewSortDirection == SortDirection.Ascending)                      //设置排序方向
        {
            GridViewSortDirection = SortDirection.Descending;
            OrderType = 0;
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            OrderType = 1;
        }
        BindLoanGrid();
    }

    StringBuilder sbAllLIdStatus = new StringBuilder();
    /// <summary>
    /// Set selected row when click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvPipelineView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (DataControlRowType.DataRow == e.Row.RowType)
        {
            int nFileId = 0;
            string strStatus = string.Format("{0}", gvPipelineView.DataKeys[e.Row.RowIndex]["Status"]);
            string strBranchId = string.Format("{0}", gvPipelineView.DataKeys[e.Row.RowIndex]["BranchID"]);
            if (null != gvPipelineView.DataKeys[e.Row.RowIndex])
            {
                if (!int.TryParse(gvPipelineView.DataKeys[e.Row.RowIndex].Value.ToString(), out nFileId))
                    nFileId = 0;

                if (0 != nFileId)
                {
                    sbAllLIdStatus.AppendFormat("allLoan.push(new SelectedLoan('{0}', '{1}', '{2}'));", nFileId, strStatus, strBranchId);
                }
            }
        }
    }

    protected void gvPipelineView_PreRender(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "registerLOIds", sbAllLIdStatus.ToString(), true);
    }

    /// <summary>
    /// Gets or sets the grid view sort direction.
    /// </summary>
    /// <value>The grid view sort direction.</value>
    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;
            return (SortDirection)ViewState["sortDirection"];
        }
        set
        {
            ViewState["sortDirection"] = value;
        }

    }
    /// <summary>
    /// Gets or sets the name of the order.
    /// </summary>
    /// <value>The name of the order.</value>
    public string OrderName
    {
        get
        {
            if (ViewState["orderName"] == null)
                ViewState["orderName"] = "Borrower";
            return Convert.ToString(ViewState["orderName"]);
        }
        set
        {
            ViewState["orderName"] = value;
        }
    }

    /// <summary>
    /// Gets or sets the type of the order.
    /// </summary>
    /// <value>The type of the order.</value>
    public int OrderType
    {
        get
        {
            if (ViewState["orderType"] == null)
                ViewState["orderType"] = 0;
            return Convert.ToInt32(ViewState["orderType"]);
        }
        set
        {
            ViewState["orderType"] = value;
        }
    }

    /// <summary>
    /// Handles the Click event of the btnSync control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnSync_Click(object sender, EventArgs e)
    {
        var selctedStr = this.hfDeleteItems.Value;

        //cheack

        #region cheack MasterSource and Status 
        LPWeb.BLL.Company_Point bllCompanyPoint = new Company_Point();
        var modCompayPoint =bllCompanyPoint.GetModel();
        string MasterSource = modCompayPoint != null ? modCompayPoint.MasterSource : "Point";

        if (MasterSource.ToUpper() == "DataTrac".ToUpper() )
        {

            string sSql = "select count(FileId) from Loans where FileId in (" + selctedStr + ") and Status ='Processing' ";
            object Rval = LPWeb.DAL.DbHelperSQL.ExecuteScalar(sSql);

            if (Rval != null && Convert.ToInt32(Rval) > 0)
            {
                PageCommon.WriteJsEnd(this, "Cannot sync an active Loan with Point while the master data source is DataTrac.", PageCommon.Js_RefreshSelf);
                return;
            }
        }
        #endregion


        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            ImportLoansResponse respone;
            try
            {
                //var selctedStr = this.hfDeleteItems.Value;

                string[] selectedItems = selctedStr.Split(',');
                ImportLoansRequest req = new ImportLoansRequest();
                //req.PointFiles = selectedItems;//todo:check DataContract change
                req.FileIds = Array.ConvertAll(selectedItems, item => int.Parse(item));
                req.hdr = new ReqHdr();
                req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                req.hdr.UserId = 5;//todo:check dummy data

                respone = service.ImportLoans(req);

                if (respone.hdr.Successful)
                {
                    PageCommon.WriteJsEnd(this, "Sync loan(s) Successfully", PageCommon.Js_RefreshSelf);
                }
                else
                {
                    PageCommon.WriteJsEnd(this, "Failed to sync loan(s).", PageCommon.Js_RefreshSelf);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ee)
            {
                LPLog.LogMessage(ee.Message);
                PageCommon.AlertMsg(this, "Failed to sync loan(s), reason: Point Manager is not running.");
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
                PageCommon.WriteJsEnd(this, "Failed to sync loan(s).", PageCommon.Js_RefreshSelf);
            }
        }
    }

    /// <summary>
    /// Handles the Click event of the btnRemove control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        var selctedStr = this.hfDeleteItems.Value;
        if (!string.IsNullOrEmpty(selctedStr))
        {
            string[] selectedItems = selctedStr.Split(',');
            //delete the selected items
            DeleteLoans(selectedItems);
            //reload the grid data
            PageCommon.WriteJsEnd(this, "Loan has been removed successfully.", PageCommon.Js_RefreshSelf);
        }
        this.hfDeleteItems.Value = "";
    }

    /// <summary>
    /// Deletes the loan programs.
    /// </summary>
    /// <param name="items">The items.</param>
    private void DeleteLoans(string[] items)
    {
        int iItem = 0;
        foreach (var item in items)
        {
            if (int.TryParse(item, out iItem))
            {
                try
                {
                    _bllLoans.Delete(iItem);
                    _bllUserRecentItems.DeleteItemsByFileID(iItem); // delete UserRecentItems records
                }
                catch (Exception exception)
                {
                    LPLog.LogMessage(exception.Message);
                }
            }
        }
    }
    private void ConvertToLead(int nFileId, int nFolderId)
    {
        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient client = sm.StartServiceClient())
            {
                ConvertToLeadRequest req = new ConvertToLeadRequest();
                req.FileId = nFileId;
                req.NewFolderId = nFolderId;
                req.hdr = new ReqHdr();
                req.hdr.UserId = CurrUser.iUserID;
 
                ConvertToLeadResponse response = client.ConvertToLead(req);
                if (!response.hdr.Successful)
                {
                    LPLog.LogMessage(LogType.Logerror, string.Format("Failed to convert file:{0} to lead", response.hdr.StatusInfo));
                    PageCommon.AlertMsg(this, response.hdr.StatusInfo);
                    return;
                }
                    
                if (WorkflowManager.UpdateLoanStatus(nFileId, "Prospect", CurrUser.iUserID) == false)
                {
                    PageCommon.AlertMsg(this, "Failed to update loan status.");
                    LPLog.LogMessage(LogType.Logerror, string.Format("Failed to update loan status, LoanId:{0}, to Status:{1}.",
                        nFileId, "Prospect"));
                    return;
                }
 
                BindLoanGrid();

                LPLog.LogMessage(LogType.Loginfo, "Successfully convert the load to lead. ");

                PageCommon.AlertMsg(this, "Successfully convert the load to lead.");
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException ee)
        {
            LPLog.LogMessage(LogType.Logerror, string.Format("Faield to move file:{0}", ee.Message));
            PageCommon.AlertMsg(this, "Failed to move the Point file, reason: Point Manager is not running.");
        }
        catch (Exception ex)
        {
            LPLog.LogMessage(LogType.Logerror, string.Format("Faield to move file:{0}", ex.Message));
            PageCommon.AlertMsg(this, ex.Message);
        }
 
    }

    private void DisposeLoan(string loanStatus, int nFileId, int nFolderId)
    {
        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient client = sm.StartServiceClient())
            {
                DisposeLoanRequest req = new DisposeLoanRequest();
                req.FileId = nFileId;
                req.LoanStatus = this.hiSelectedDisposal.Value;
                req.NewFolderId = nFolderId;
                req.hdr = new ReqHdr();
                req.hdr.UserId = CurrUser.iUserID;
                req.StatusDate = DateTime.Now;

                DisposeLoanResponse response = client.DisposeLoan(req);
                if (!response.hdr.Successful)
                {
                    LPLog.LogMessage(LogType.Logerror, string.Format("Failed to dispose of file:{0}", response.hdr.StatusInfo));
                    PageCommon.AlertMsg(this, response.hdr.StatusInfo);
                    return;
                }

                if (WorkflowManager.UpdateLoanStatus(nFileId, this.hiSelectedDisposal.Value, CurrUser.iUserID) == false)
                {
                    PageCommon.AlertMsg(this, "Failed to update loan status.");
                    LPLog.LogMessage(LogType.Logerror, string.Format("Failed to update loan status, LoanId:{0}, to Status:{1}.",
                        nFileId, this.hiSelectedDisposal.Value));
                    return;
                }
                BindLoanGrid();
                LPLog.LogMessage(LogType.Loginfo, string.Format("Successfully update loan status, LoanId:{0}, to Status:{1}. ",
                    nFileId, this.hiSelectedDisposal.Value));
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException ee)
        {
            LPLog.LogMessage(LogType.Logerror, string.Format("Faield to dispose of file:{0}", ee.Message));
            PageCommon.AlertMsg(this, "Failed to dispose of the loan, reason: Point Manager is not running.");
        }
        catch (Exception ex)
        {
            LPLog.LogMessage(LogType.Logerror, string.Format("Faield to dispose of file:{0}", ex.Message));
            PageCommon.AlertMsg(this, ex.Message);
        }
    }

    private void MoveFile(int FileId, int NewFolderId)
    {
        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient client = sm.StartServiceClient())
            {

                MoveFileRequest req = new MoveFileRequest();
                req.FileId = FileId;
                req.NewFolderId = NewFolderId;
                req.hdr = new ReqHdr();
                req.hdr.UserId = CurrUser.iUserID;

                MoveFileResponse response = client.MoveFile(req);
                if (!response.hdr.Successful)
                {
                    LPLog.LogMessage(LogType.Logerror, string.Format("Failed to move file:{0}", response.hdr.StatusInfo));
                    PageCommon.AlertMsg(this, response.hdr.StatusInfo);
                }
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException ee)
        {
            LPLog.LogMessage(LogType.Logerror, string.Format("Faield to move file:{0}", ee.Message));
            PageCommon.AlertMsg(this, "Failed to move the Point file, reason: Point Manager is not running.");
        }
        catch (Exception ex)
        {
            LPLog.LogMessage(LogType.Logerror, string.Format("Faield to move file:{0}", ex.Message));
            PageCommon.AlertMsg(this, ex.Message);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDispose_Click(object sender, EventArgs e)
    {
        int nFileId = -1;
        if (!int.TryParse(this.hiSelectedLoan.Value, out nFileId))
            nFileId = -1;
        if (nFileId == -1)
        {
            LPLog.LogMessage(LogType.Logerror, "Invalid file Id: " + this.hiSelectedLoan.Value);
            return;
        }

        int nFolderId = -1;
        if (!int.TryParse(this.hiSelectedFolderId.Value, out nFolderId))
            nFolderId = -1;
        if (nFolderId == -1)
        {
            LPLog.LogMessage(LogType.Logerror, "Invalid folder Id: " + this.hiSelectedFolderId.Value);
            return;
        }
        switch (this.hiSelectedDisposal.Value.Trim().ToLower())
        {
            case "dispose":
                DisposeLoan(this.hiSelectedDisposal.Value, nFileId, nFolderId);
                break;
            case "converttolead":
                ConvertToLead(nFileId, nFolderId);
                break;
            case "move":
                MoveFile(nFileId, nFolderId);
                break;
            default:
                DisposeLoan(this.hiSelectedDisposal.Value, nFileId, nFolderId);
                //LPLog.LogMessage(LogType.Logerror, string.Format("Invalid hiSelectedDisposal {0}, FileId {1}, FolderId {2}. ", this.hiSelectedDisposal.Value, this.hiSelectedLoan.Value, this.hiSelectedFolderId.Value));
                break;
        }

    }

    protected void ddlLoanStatus_SelectedIndexChanged(object sender, EventArgs e) 
    {
        isReset = true;
        this.BindStages();
        this.BindDateTypes();
        BindLoanGrid();
    }

    protected void ddlOrganType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.BindOrganizations();
    }

    protected void ddlLeadSourceType_SelectedIndexChanged(object sender, EventArgs e) 
    {
        this.BindLeadSource();
    }


    /// <summary>
    /// UserPipelineView  Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlUserPipelineView_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Load View Filer

        if (Change_To_Select_After_Filter == true)
        {
            Change_To_Select_After_Filter = false;
            return;
        }

        if (!string.IsNullOrEmpty(ddlUserPipelineView.SelectedValue))
        {
            if (FirstTimeFlag == true)
            {
                if (IsDefaultView == true)
                {
                    SetUserPipelineView(Convert.ToInt32(ddlUserPipelineView.SelectedValue));
                }
                else
                {
                    BindDefaultFiler();
                }
            }
            else
            {
                if (ddlUserPipelineView.SelectedValue == "0")
                {
                    IsDefaultView = false;
                    isView = false;
                    BindDefaultFiler();
                    AspNetPager1.CurrentPageIndex = 1;
                    isView_To_Select = true;
                    if (IsGrid == false)
                    {
                        BindLoanGrid();
                    }
                }
                else
                {
                    SetUserPipelineView(Convert.ToInt32(ddlUserPipelineView.SelectedValue));
                    ViewIndexChanged = true;
                    AspNetPager1.CurrentPageIndex = 1;
                    if (IsGrid == false)
                    {
                        BindLoanGrid();
                    }
                }
            }
        }
        else
        {
            IsDefaultView = false;
            BindDefaultFiler();
        }
    }

    /// <summary>
    /// Bind Default Value
    /// </summary>
    /// <param name="userPipelineViewID"></param>
    private void SetUserPipelineView(int userPipelineViewID)
    {
        //this.BindStages();
        //this.BindDateTypes();
        //this.BindOrganizations();
        //this.BindLeadSource();

        if (userPipelineViewID <= 0)
        {
            BindDefaultFiler();

        }
        else
        {
            #region LoadUPV
            LPWeb.BLL.UserPipelineViews bll = new UserPipelineViews();

            var model = bll.GetModel(userPipelineViewID);

            if (model != null && model.UserPipelineViewID != null && model.UserId == CurrUser.iUserID)
            {
                try
                {
                    isView = true;
                    this.AdvancedLoanFilters = model.AdvancedLoanFilters;

            //ddlLoanStatus.SelectedValue = model.ViewFilterDisplay;
            //ddlLoanStatus.Attributes["UPV"] = string.IsNullOrEmpty(model.ViewFilterDisplay) ? "" : model.ViewFilterDisplay;

            //ddlOrganType.SelectedValue = model.OrgTypeFilterDisplay;
            //ddlOrganType.Attributes["UPV"] = string.IsNullOrEmpty(model.OrgTypeFilterDisplay) ? "" : model.OrgTypeFilterDisplay;

            //ddlOrgan.SelectedValue = model.OrgFilter;
            //ddlOrgan.Attributes["UPV"] = string.IsNullOrEmpty(model.OrgFilter) ? "" : model.OrgFilter;

            //ddlStage.SelectedValue = model.StageFilter;
            //ddlStage.Attributes["UPV"] = string.IsNullOrEmpty(model.StageFilter) ? "" : model.StageFilter;

            //ddlLeadSourceType.SelectedValue = model.ContactTypeFilter;
            //ddlLeadSourceType.Attributes["UPV"] = string.IsNullOrEmpty(model.ContactTypeFilter) ? "" : model.ContactTypeFilter;

            //ddlLeadSource.SelectedValue = model.ContactFilter;
            //ddlLeadSource.Attributes["UPV"] = string.IsNullOrEmpty(model.ContactFilter) ? "" : model.ContactFilter;

            //ddlDateType.SelectedValue = model.DateTypeFilterDisplay;
            //ddlDateType.Attributes["UPV"] = string.IsNullOrEmpty(model.DateTypeFilterDisplay) ? "" : model.DateTypeFilterDisplay;

            //#region DateFilter
            //if (!string.IsNullOrEmpty(model.DateFilter))
            //{
            //    var item = model.DateFilter.Split(',');
            //    EstStartDate.Attributes["UPV"] = "";
            //    EstStartDate.Text = "";
            //    if (item.Count() >= 1 && !string.IsNullOrEmpty(item.FirstOrDefault()))
            //    {
            //        EstStartDate.Text = item.FirstOrDefault();
            //        EstStartDate.Attributes["UPV"] = item.FirstOrDefault();
            //    }

            //    EstEndDate.Attributes["UPV"] = "";
            //    EstEndDate.Text = "";
            //    if (item.Count() == 2 && !string.IsNullOrEmpty(item.LastOrDefault()))
            //    {
            //        EstEndDate.Text = item.LastOrDefault();
            //        EstEndDate.Attributes["UPV"] = item.LastOrDefault();
            //    }

            //}
            //#endregion
                }
                catch { }
            }
            #endregion
        }

        //this.btnFilter_Click(this.btnFilter, new EventArgs());

    }

    private void BindDefaultFiler()
    {
        ddlLoanStatus.SelectedIndex = 0;


        ddlOrganType.SelectedIndex = 0;
        //ddlOrganType_SelectedIndexChanged(ddlOrganType, new EventArgs());

        ddlOrgan.SelectedIndex = 0;

        ddlStage.SelectedIndex = 0;

        ddlLeadSourceType.SelectedIndex = 0;

        ddlLeadSource.SelectedIndex = 0;

        ddlDateType.SelectedIndex = 0;

        EstEndDate.Text = "";
        EstStartDate.Text = "";
    }


    private void CheackIsChangeFiler()
    {
        if (ddlLoanStatus.Attributes["UPV"] != ddlLoanStatus.SelectedValue
            || ddlOrganType.Attributes["UPV"] != ddlOrganType.SelectedValue
            || ddlOrgan.Attributes["UPV"] != ddlOrgan.SelectedValue
            || ddlStage.Attributes["UPV"] != ddlStage.SelectedValue
            || ddlLeadSourceType.Attributes["UPV"] != ddlLeadSourceType.SelectedValue
            || ddlLeadSource.Attributes["UPV"] != ddlLeadSource.SelectedValue
            || ddlDateType.Attributes["UPV"] != ddlDateType.SelectedValue
            || EstStartDate.Attributes["UPV"] != EstStartDate.Text
            || EstEndDate.Attributes["UPV"] != EstEndDate.Text
            )
        {
            ddlUserPipelineView.SelectedValue = "0";
        }

    }


    protected void btnSaveView_OnClick(object sender, EventArgs e)
    {
        var viewName = txtSaveViewName.Text.Trim().Replace("'", "");

        LPWeb.BLL.UserPipelineViews bll = new UserPipelineViews();
        LPWeb.Model.UserPipelineViews model = new LPWeb.Model.UserPipelineViews();
        int ID = 0;
        var ds = bll.GetList_ViewName("ViewName ='" + viewName + "'" + " AND PipelineType='Loans'  AND UserId =" + CurrUser.iUserID, "");

        if (ds.Tables[0].Rows.Count > 0)
        {
            ID = ds.Tables[0].Rows[0]["UserPipelineViewID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["UserPipelineViewID"]) : 0;
        }

        model.ViewName = viewName;
        model.ContactFilter = ddlLeadSource.SelectedValue.Trim();
        model.ContactTypeFilter = ddlLeadSourceType.SelectedValue.Trim();
        model.DateFilter = EstStartDate.Text.Trim() + "," + EstEndDate.Text.Trim();
        model.DateTypeFilterDisplay = ddlDateType.SelectedValue.Trim();

        model.Enabled = true;
        model.OrgFilter = ddlOrgan.SelectedValue.Trim();
        model.OrgTypeFilterDisplay = ddlOrganType.SelectedValue.Trim();
        model.PipelineType = "Loans";
        model.StageFilter = ddlStage.SelectedValue.Trim();
        model.UserId = CurrUser.iUserID;
        model.ViewFilterDisplay = ddlLoanStatus.SelectedValue.Trim();

        model.UserPipelineViewID = ID;
        this.AdvancedLoanFilters = this.BuildWhere_AdvacedLoanFilters();               
        model.AdvancedLoanFilters = this.AdvancedLoanFilters;


        if (ID != 0)
        {
            bll.Update(model);
        }
        else
        {
            bll.Add(model);

            ds = bll.GetList_ViewName("ViewName ='" + viewName + "'" + " AND PipelineType='Loans'  AND UserId =" + CurrUser.iUserID, "");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ID = ds.Tables[0].Rows[0]["UserPipelineViewID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["UserPipelineViewID"]) : 0;
            }

            BindUserPiplineView();

            ddlUserPipelineView.SelectedValue = ID.ToString();

            SetUserPipelineView(ID);
        }




    }

    public string GetCurrentStageTooltip(int iFileID) 
    {
        string sTooltipText = string.Empty;

        string sTaskName = string.Empty;
        string sDueDate = string.Empty;
        string sOwnerName = string.Empty;
        string sCompletedDate = string.Empty;
        string sComplatedByName = string.Empty;

        // get current stage id
        string sSql = "select top 1 LoanStageId,StageName from dbo.LoanStages where (Completed IS NULL) AND (FileId=" + iFileID + ") order by SequenceNumber";
        DataTable CurrentStageInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        if (CurrentStageInfo.Rows.Count > 0)
        {
            int iLoanStageId = Convert.ToInt32(CurrentStageInfo.Rows[0]["LoanStageId"]);

            #region 检查是否有uncompleted task

            string sSql4 = "select LoanTaskId from LoanTasks where LoanStageId=" + iLoanStageId + " and (Completed is not null)";
            DataTable LoanTaskList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql4);

            string sSql5 = string.Empty;
            if (LoanTaskList.Rows.Count > 0)    // 有completed task
            {
                sSql5 = "select top 1 Name,Due,Owner,Completed,CompletedBy from LoanTasks where LoanStageId=" + iLoanStageId + " and (Completed is not null) order by Completed desc";
            }
            else
            {
                sSql5 = "select top 1 Name,Due,Owner,Completed,CompletedBy from LoanTasks where LoanStageId=" + iLoanStageId + " order by LoanTaskId";
            }

            #endregion

            DataTable TaskInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql5);
            if (TaskInfo.Rows.Count > 0)
            {
                sTaskName = TaskInfo.Rows[0]["Name"].ToString();

                #region Due

                sDueDate = TaskInfo.Rows[0]["Due"].ToString();
                if (sDueDate != string.Empty)
                {
                    sDueDate = Convert.ToDateTime(TaskInfo.Rows[0]["Due"]).ToShortDateString();
                }

                #endregion

                #region Owner

                string sOwnerID = TaskInfo.Rows[0]["Owner"].ToString();
                if (sOwnerID != string.Empty)
                {
                    int iOwnerId = Convert.ToInt32(sOwnerID);
                    Users UserMrg = new Users();
                    DataTable OwnerInfo = UserMrg.GetUserInfo(iOwnerId);

                    if ((OwnerInfo != null) && (OwnerInfo.Rows.Count > 0))
                    {
                        string sFirstName = OwnerInfo.Rows[0]["FirstName"].ToString();
                        string sLastName = OwnerInfo.Rows[0]["LastName"].ToString();

                        sOwnerName = sLastName + ", " + sFirstName;
                    }
                }

                #endregion

                #region Completed

                sCompletedDate = TaskInfo.Rows[0]["Completed"].ToString();
                if (sCompletedDate != string.Empty)
                {
                    sCompletedDate = Convert.ToDateTime(TaskInfo.Rows[0]["Completed"]).ToShortDateString();
                }

                #endregion

                #region CompletedBy

                string sCompletedByID = TaskInfo.Rows[0]["CompletedBy"].ToString();
                if (sCompletedByID != string.Empty)
                {
                    int iCompletedById = Convert.ToInt32(sCompletedByID);
                    Users UserMrg = new Users();
                    DataTable CompletedByInfo = UserMrg.GetUserInfo(iCompletedById);
                    if (CompletedByInfo.Rows.Count > 0)
                    {
                        string sFirstName = CompletedByInfo.Rows[0]["FirstName"].ToString();
                        string sLastName = CompletedByInfo.Rows[0]["LastName"].ToString();

                        sComplatedByName = sLastName + ", " + sFirstName;
                    }
                    else
                    {
                        sComplatedByName = " ";
                    }
                }

                #endregion

                sTooltipText = "title=\"" + sTaskName + "&#10;    due: " + sDueDate + "&#10;    assigned: " + sOwnerName + "&#10;    completed: " + sCompletedDate + "&#10;    by: " + sComplatedByName + "\"";
            }
        }

        return sTooltipText;
    }

    public string GetLastNoteTooltip(string sLastNote)
    {
        string sNewLastNoteString = "";
        int j = 1;
        if (sLastNote.Length >= 50)
        {
            for (int i = 0; i < sLastNote.Length; i++)
            {
                if (i == 50 * j)
                {
                    sNewLastNoteString += sLastNote.Substring((50*(j - 1)), 50) + "&#10;";
                    j++;
                }
            }
            if (sLastNote.Length < (50 * j))
            {
                sNewLastNoteString += sLastNote.Substring((50 * (j - 1)), (sLastNote.Length - (50 * (j - 1))));
            }
        }
        else
        {
            sNewLastNoteString = sLastNote;
        }
        
        return sNewLastNoteString;
    }
}


