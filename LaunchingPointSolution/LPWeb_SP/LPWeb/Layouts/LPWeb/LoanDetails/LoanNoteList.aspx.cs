using System;
using System.Data;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.LP_Service;
using Microsoft.SharePoint.WebControls;
using Utilities;
using LoanActivities = LPWeb.Model.LoanActivities;

namespace LPWeb.Layouts.LPWeb.LoanDetails
{
    public partial class LoanNoteList : LayoutsPageBase
    {
        private readonly LoanNotes _bllLoanNotes = new LoanNotes();
        private string sErrorMsg = "Failed to load current page: invalid FileID.";
        private string sReturnPage = "LoanNoteList.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            var loginUser = new LoginUser();
            //权限验证
            //hdnCreateNotes.Value = loginUser.userRole.Loans.Contains("I") == true ? "1" : "0";

            if (loginUser.userRole.CreateNotes == true)
            {
                hdnCreateNotes.Value = "1";
            }
            else
            {
                hdnCreateNotes.Value = "0";
            }           

            if (Request.QueryString["FileID"] != null) // 如果有GroupID
            {
                string sFileID = Request.QueryString["FileID"];

                if (PageCommon.IsID(sFileID) == false)
                {
                    PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                }

                CurrentFileId = Convert.ToInt32(sFileID);
            }
            if (CurrentFileId == 0)
            {
                return;
            }

            if (!IsPostBack)
            {
                BindPage(1);
            }
        }
        private void CheckLoanStatus()
        {
            Loans loanMgr = new Loans();

            this.hdnActiveLoan.Value = loanMgr.IsActiveLoan(CurrentFileId) == true ? "True" : "False";
        }
        private void BindPage(int pageIndex)
        {
            if (CurrentFileId < 1)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.location.href='" + sReturnPage + "'");
                return;
            }
            CheckLoanStatus();
            ValidateButtonPermission();

            int pageSize = 20;
            int recordCount = 0;
            DataSet ds = _bllLoanNotes.GetLoanNotes(pageSize, pageIndex,
                                                         "FileId=" + CurrentFileId,
                                                         out recordCount, OrderName, OrderType);
            AspNetPager1.RecordCount = recordCount;
            AspNetPager1.CurrentPageIndex = pageIndex;
            AspNetPager1.PageSize = pageSize;

            gvNoteList.DataSource = ds;
            gvNoteList.DataBind();
        }

        protected void gvNoteList_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox cb = (CheckBox)e.Row.FindControl("cbExternalViewing");
                cb.Checked = false;
                DataRowView dr = (DataRowView)e.Row.DataItem;

                if (dr["ExternalViewing"] != DBNull.Value)
                {
                    cb.Checked = Convert.ToBoolean(dr["ExternalViewing"]);
                    
                }
                cb.ToolTip = dr["NoteId"].ToString();
            }

        }

        protected void cbExternalViewing_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            int Id = Convert.ToInt32(cb.ToolTip);

            BLL.LoanNotes bll = new BLL.LoanNotes();

            Model.LoanNotes model = bll.GetModel(Id);

            model.ExternalViewing = cb.Checked;

            bll.Update(model);


        }


        private void ValidateButtonPermission()
        {
            var loginUser = new LoginUser();
            btnNew.Disabled = !loginUser.userRole.CreateNotes;
            if (hdnActiveLoan.Value.ToUpper() == "FALSE")
            {
                btnNew.Disabled = true;
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindPage(AspNetPager1.CurrentPageIndex);
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
        /// Gets or sets the current file id.
        /// </summary>
        /// <value>The current file id.</value>
        protected int CurrentFileId
        {
            set
            {
                hfdFileId.Value = value.ToString();
                ViewState["fileId"] = value;
            }
            get
            {
                if (ViewState["fileId"] == null)
                    return 0;
                int fileId = 0;
                int.TryParse(ViewState["fileId"].ToString(), out fileId);

                return fileId;
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
                return (SortDirection) ViewState["sortDirection"];
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