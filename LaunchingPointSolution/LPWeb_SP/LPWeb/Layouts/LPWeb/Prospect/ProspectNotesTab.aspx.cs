using System;
using System.Data;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.LP_Service;
using Microsoft.SharePoint.WebControls;
using Utilities;
using LoanActivities = LPWeb.Model.LoanActivities;

namespace LPWeb.Prospect
{
    public partial class ProspectNotesTab : LayoutsPageBase
    {
        private readonly ProspectNotes _bllProspectNotes = new ProspectNotes();
        private string sErrorMsg = "Failed to load current page: invalid ContactID.";
        private string sReturnPage = "ProspectDetailView.aspx";

        protected string sNoteType = "";
        protected string sNowContactID = "0";
        protected int iBorrowerId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            var loginUser = new LoginUser();

            #region CR48
            if (Request.QueryString["ContactID"] == null && Request.QueryString["FileID"] != null)
            {
                var sFileId = Request.QueryString["FileID"].ToString();

                LPWeb.BLL.Loans bllLoans = new Loans();
                iBorrowerId = bllLoans.GetBorrowerID(Convert.ToInt32(sFileId)).Value;

            }
            else
            {
                iBorrowerId = Request.QueryString["ContactID"] != null ? Convert.ToInt32(Request.QueryString["ContactID"]) : 0;
            }

            #endregion

            //权限验证
            hdnCreateNotes.Value = loginUser.userRole.Prospect.Contains("M") == true ? "1" : "0";

            if (iBorrowerId != 0)  //if (Request.QueryString["ContactID"] != null) // 如果有ContactID
            {
                string sContactID = iBorrowerId.ToString(); //Request.QueryString["ContactID"]; //cr48 gdc 20121020
                sNowContactID = sContactID;

                if (PageCommon.IsID(sContactID) == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                }

                CurrentContactId = Convert.ToInt32(sContactID);
            }
            if (CurrentContactId == 0)
            {
                return;
            }

            if (!IsPostBack)
            {
                BindNoteType();
                BindPage(1);
            }
        }

        private void BindNoteType()
        {
            DataTable dtSource = _bllProspectNotes.GetLoanNoteTypeInfo(iBorrowerId); //gdc 20121020 cr48//(Convert.ToInt32(Request.QueryString["ContactID"]));
            DataRow dr = dtSource.NewRow();
            dr["NoteTypeName"] = "Client Notes";
            dr["ContactId"] = 0;
            dtSource.Rows.InsertAt(dr, 0);
            dr = dtSource.NewRow();
            dr["NoteTypeName"] = "All Notes";
            dr["ContactId"] = -1;
            dtSource.Rows.InsertAt(dr, 0);

            ddlNoteType.DataSource = dtSource;
            ddlNoteType.DataBind();

            DataTable dtAddSource = _bllProspectNotes.GetLoanNoteTypeInfoForAdd(iBorrowerId);
            foreach (DataRow drAdd in dtAddSource.Rows)
            {
                sNoteType += "<li id='" + drAdd["ContactId"].ToString() + "'><a href=javascript:PopupAddLoanNoteWindow('" + drAdd["StatusName"].ToString().Replace(" ","") + "'," + drAdd["ContactId"].ToString() + ");><span>" + drAdd["NoteTypeName"].ToString() + "</span></a></li>";
            }
        }

        private void BindPage(int pageIndex)
        {
            if (CurrentContactId < 1)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                return;
            }

            ValidateButtonPermission();

            string sWhereString = " 1>0 ";
            if (ddlNoteType.SelectedValue.ToString() == "-1")
            { 
            }
            else if (ddlNoteType.SelectedValue.ToString() == "0")
            {
                sWhereString += " and form='1' "; 
            }
            else if (Convert.ToInt32(ddlNoteType.SelectedValue.ToString()) > 0)
            {
                sWhereString += " and FileId=" + ddlNoteType.SelectedValue.ToString(); 
            }

            if (!string.IsNullOrEmpty(this.tbSentStart.Text))
            {
                DateTime dt = new DateTime();
                DateTime dtNew = new DateTime();
                try
                {
                    if (DateTime.TryParse(this.tbSentStart.Text, out dt))
                    {
                        dtNew = new DateTime(dt.Year, dt.Month, dt.Day);
                        sWhereString += string.Format(" AND [Created] >= '{0}' ", dtNew.ToString());
                    }
                }
                catch
                { }
            }

            if (!string.IsNullOrEmpty(this.tbSentEnd.Text))
            {
                DateTime dt = new DateTime();
                DateTime dtNew = new DateTime();
                try
                {
                    if (DateTime.TryParse(this.tbSentEnd.Text, out dt))
                    {
                        dtNew = dt.AddDays(1).AddMilliseconds(-1);
                        sWhereString += string.Format(" AND [Created] <= '{0}' ", dtNew.ToString());
                    }
                }
                catch
                { }
            }

            int pageSize = 20;
            int recordCount = 0;
            DataSet ds = _bllProspectNotes.GetProspectNotes(pageSize, pageIndex,
                                                        sWhereString,
                                                         out recordCount, OrderName, OrderType, iBorrowerId);
            AspNetPager1.RecordCount = recordCount;
            AspNetPager1.CurrentPageIndex = pageIndex;
            AspNetPager1.PageSize = pageSize;

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["LoanType"].ToString() == "Client Note")
                    {
                        dr["LoanFile"] = "";
                    }
                }
                ds.Tables[0].AcceptChanges();
            }
            ds.AcceptChanges();

            gvNoteList.DataSource = ds;
            gvNoteList.DataBind();
        }

        private void ValidateButtonPermission()
        {
            var loginUser = new LoginUser();
            btnNew.Disabled = !loginUser.userRole.CreateNotes;
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindPage(AspNetPager1.CurrentPageIndex);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindPage(1);
        }

        protected void gvNoteList_Sorting(object sender, GridViewSortEventArgs e)
        {
            OrderName = e.SortExpression;
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending) //设置排序方向
            {
                GridViewSortDirection = SortDirection.Descending;
                OrderType = 0;
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                OrderType = 1;
            }
            BindPage(AspNetPager1.CurrentPageIndex);
        }
        #region Properties

        /// <summary>
        /// Gets or sets the current contact id.
        /// </summary>
        /// <value>The current contact id.</value>
        protected int CurrentContactId
        {
            set
            {
                hfdContactId.Value = value.ToString();
                ViewState["contactId"] = value;
            }
            get
            {
                if (ViewState["contactId"] == null)
                    return 0;
                int contactId = 0;
                int.TryParse(ViewState["contactId"].ToString(), out contactId);

                return contactId;
            }
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
            set { ViewState["sortDirection"] = value; }
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
                    ViewState["orderName"] = "Created";
                return Convert.ToString(ViewState["orderName"]);
            }
            set { ViewState["orderName"] = value; }
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
                    ViewState["orderType"] = 1;
                return Convert.ToInt32(ViewState["orderType"]);
            }
            set { ViewState["orderType"] = value; }
        }

        #endregion
    }
}
