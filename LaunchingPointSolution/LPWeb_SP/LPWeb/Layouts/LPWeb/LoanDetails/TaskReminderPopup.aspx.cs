using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using Utilities;
using LPWeb.Common;
using LPWeb.BLL;
using System.Globalization;




    public partial class TaskReminderPopup : BasePage
    {
        LoanTasks loanTasks = new LoanTasks();
        private bool isReset = false;
        LoginUser loginUser = new LoginUser();
        Users users = new Users();
        int iLoanID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
          
                if (!IsPostBack)
                {
                    //权限验证

                    if (this.loginUser.userRole.CustomTask.ToString() == "")
                    {
                        Response.Redirect("../Unauthorize1.aspx");
                        return;
                    }
                    else
                    {
                        this.hdnCustomTask.Value = this.loginUser.userRole.CustomTask.ToString();
                        this.hdnAssignTask.Value = this.loginUser.userRole.AssignTask.ToString();
                        this.hdnCompleteOtherTask.Value = this.loginUser.userRole.MarkOtherTaskCompl == true ? "True" : "False";
                    }

                    #region 检查必要参数

                    //bool bIsValid = PageCommon.ValidateQueryString(this, "LoanID", QueryStringType.ID);
                    //if (bIsValid == false)
                    //{
                    //    //this.Response.Write("Missing required query string.");
                    //    try
                    //    {
                    //        this.Response.End();
                    //    }
                    //    catch
                    //    {

                    //    }
                    //}

                    //this.iLoanID = Convert.ToInt32(this.Request.QueryString["LoanID"]);

                    #endregion

                    // login user id
                    this.hdnLoginUserID.Value = this.loginUser.iUserID.ToString();

                    #region 加载Loan

                    //Loans LoanManager = new Loans();
                    //DataTable LoanInfo = LoanManager.GetLoanInfo(this.iLoanID);
                    //if (LoanInfo.Rows.Count == 0)
                    //{
                    //    this.Response.Write("Invalid query string.");
                    //    try
                    //    {
                    //        this.Response.End();
                    //    }
                    //    catch
                    //    {

                    //    }
                    //}

                    #endregion

                    #region Active Loan or Not

                    //string sLoanStatus = LoanInfo.Rows[0]["Status"].ToString();
                    //this.hdnLoanStatus.Value = sLoanStatus;

                    #endregion

                    #region Is Post-Close Complated

                    //string sSqlx = "select * from LoanStages where FileId=" + this.iLoanID + " and StageName='Post-Close'";
                    //DataTable PostCloseStageInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSqlx);
                    //if (PostCloseStageInfo.Rows.Count == 0)
                    //{
                    //    this.hdnIsPostCloseUncompleted.Value = "false";
                    //}
                    //else
                    //{
                    //    string CompletedDate = PostCloseStageInfo.Rows[0]["Completed"].ToString();
                    //    if (CompletedDate == string.Empty)
                    //    {
                    //        this.hdnIsPostCloseUncompleted.Value = "true";
                    //    }
                    //    else
                    //    {
                    //        this.hdnIsPostCloseUncompleted.Value = "false";
                    //    }
                    //}

                    #endregion

                    #region hidden fields for Regenerate

                    #region 获取Loan Workflow Template ID

                    //LoanWflTempl LoanWflTemplManager = new LoanWflTempl();
                    //DataTable LoanWflTempInfo = LoanWflTemplManager.GetLoanWorkflowTemplateInfo(this.iLoanID);
                    //if (LoanWflTempInfo.Rows.Count > 0)
                    //{
                    //    this.hdnLoanWflTempID.Value = LoanWflTempInfo.Rows[0]["WflTemplId"].ToString();
                    //}

                    //#endregion

                    //#region 获取Default Workflow Template ID and Name

                    //Template_Workflow WorkflowTempManager = new Template_Workflow();
                    //DataTable DefaultWorkflowTempInfo = WorkflowTempManager.GetDefaultWorkflowTemplateInfo(sLoanStatus);
                    //if (DefaultWorkflowTempInfo.Rows.Count > 0)
                    //{
                    //    this.hdnDefaultWflTempID.Value = DefaultWorkflowTempInfo.Rows[0]["WflTemplId"].ToString();
                    //    this.hdnDefaultWflTempName.Value = DefaultWorkflowTempInfo.Rows[0]["Name"].ToString();
                    //}

                    #endregion

                    #endregion

                    BindGrid();
                }
           
        }

        /// <summary>
        /// Bind email template gridview
        /// </summary>
        private void BindGrid()
        {
            //int pageSize = AspNetPager1.PageSize;
            //int pageIndex = 1;


            int pageSize = 10000;
            int pageIndex = 1;

            //if (isReset == true)
            //    pageIndex = AspNetPager1.CurrentPageIndex = 1;
            //else
            //    pageIndex = AspNetPager1.CurrentPageIndex;

            string strWhare = GetSqlWhereClause();
            int recordCount = 0;

            DataSet listData = null;
            try
            {
                listData = loanTasks.GetListForGridView(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            //AspNetPager1.PageSize = pageSize;
            //AspNetPager1.RecordCount = recordCount;

            if (listData.Tables[0].Rows.Count > 0)
            {
                hdnTaskReminder.Value = listData.Tables[0].Rows.Count.ToString();
            }

            gridList.DataSource = listData;
            gridList.DataBind();
        }

        /// <summary>
        /// Get filter
        /// </summary>
        /// <returns></returns>
        private string GetSqlWhereClause()
        {
            string strWhere = "";


           
                string sqlwhere = "convert(datetime,(convert(varchar(11),Due,120)+' '+convert(varchar(10),DueTime,120)))";

                //strWhere = " and FileId=" + this.iLoanID + " and DueTime is NOT NULL and datediff(day," + sqlwhere + ",getdate())<=" + loginUser.TaskReminder;

                strWhere = " and DueTime is NOT NULL and datediff(day," + sqlwhere + ",getdate())<=" + loginUser.TaskReminder;


          
            
            return strWhere;
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            //ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
            BindGrid();
        }

        protected void lbtnDisable_Click(object sender, EventArgs e)
        {
           
        }

     

        StringBuilder sbAllIds = new StringBuilder();
        StringBuilder sbReferenced = new StringBuilder();
       
        /// <summary>
        /// Set selected row when click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (DataControlRowType.DataRow == e.Row.RowType)
            {


                Label lblDueTime = e.Row.FindControl("lblDueTime") as Label;
                // reset description text
                Label lblDue = e.Row.FindControl("lblDue") as Label;
                if (null != lblDue)
                {


                    lblDue.Text = GetDeu(lblDue.Text.Trim(), lblDueTime.Text.Trim());
                   
                }

                // reset description text
                Label lblOwner = e.Row.FindControl("lblOwner") as Label;
                if (null != lblOwner)
                {
                   
                        
                        lblOwner.Text = GetOwner(lblOwner.Text);
                  
                }

                // reset description text
                Label lblBorrower = e.Row.FindControl("lblBorrower") as Label;
                if (null != lblBorrower)
                {


                    lblBorrower.Text = GetBorrower(lblBorrower.Text);
                   
                }


            }
        }


        private string GetBorrower(string Borrower)
        {
            string getBorrower = "";

            string sSql = "select Borrower from V_ProcessingPipelineInfo where 1=1 and FileId='" + Borrower + "'";
            getBorrower = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql).Rows[0]["Borrower"].ToString(); 
            return getBorrower;
        }


        private string GetDeu(string Deu,string time)
        {
            string getDeu = "";

         
            //getDeu = DateTime.Parse(Deu).ToString("yyyy-MM-dd")+" " + time.ToString("hh:mm:ss tt");            

            getDeu = DateTime.Parse(Deu).ToString("MM/dd/yyyy")+" "+time;

            getDeu = DateTime.Parse(getDeu).ToString("MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture); 

            return getDeu;
        }

        private string GetOwner(string Owner)
        {
           
            string owner = "";

            if (!string.IsNullOrEmpty(Owner))
            {
                owner = users.GetUserInfo(int.Parse(Owner)).Rows[0]["Username"].ToString();
            }

            return owner;
        }

        protected void gridList_PreRender(object sender, EventArgs e)
        {
            //this.hiAllIds.Value = sbAllIds.ToString();
            //this.hiCheckedIds.Value = "";
            //this.hiReferenced.Value = sbReferenced.ToString();
        }

        /// <summary>
        /// Handles the Sorting event of the gridUserList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void gridList_Sorting(object sender, GridViewSortEventArgs e)
        {
            OrderName = e.SortExpression;
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                OrderType = 0;
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                OrderType = 1;
            }
            BindGrid();
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
                    ViewState["orderName"] = "LoanTaskId";
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

        protected void ddlAlphabet_SelectedIndexChanged(object sender, EventArgs e)
        {
            isReset = true;
            BindGrid();
        }

        /// <summary>
        /// call client function
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="strKey"></param>
        /// <param name="strScript"></param>
        private void ClientFun(Control ctl, string strKey, string strScript)
        {
            ScriptManager.RegisterStartupScript(ctl, this.GetType(), strKey, strScript, true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="iLoanTaskID"></param>
        /// <param name="sCompletedDate"></param>
        /// <returns></returns>
        public string GetLoanTaskIconFileName(int iLoanTaskID)
        {
            return LPWeb.DAL.WorkflowManager.GetTaskIcon(iLoanTaskID);
        }
    }

