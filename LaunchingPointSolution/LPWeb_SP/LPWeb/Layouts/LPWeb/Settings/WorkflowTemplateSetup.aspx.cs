using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;
using LPWeb.Common;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;

namespace LPWeb.Settings
{
    public partial class WorkflowTemplateSetup : BasePage
    {
        #region params
        private bool isReset = false;
        const string alphabets = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        public string FromURL = string.Empty;
        Template_Workflow template = new Template_Workflow();
        string Alphabet = string.Empty;
        string StageID = string.Empty;
        int PageIndex = 1;
        int WflTemplId = 0;
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //权限验证
                var loginUser = new LoginUser();
                if (loginUser.userRole.WorkflowTempl.ToString() == "")
                {
                    Response.Redirect("../Unauthorize.aspx");
                    return;
                }
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }
            PageLoad();
        }
        #endregion

        #region Function
        private void PageLoad()
        {
            if (Request.Url != null)
            {
                FromURL = Request.Url.ToString();
            }

            if (this.Request.QueryString["WflTemplId"] != null) // PageIndex
            {
                try
                {
                    WflTemplId = int.Parse(this.Request.QueryString["WflTemplId"].ToString());
                }
                catch
                {
                    WflTemplId = 0;
                }
            }
            else
            {
                try
                {
                    if (hdnTmpID.Value.Length > 0)
                    {
                        WflTemplId = int.Parse(hdnTmpID.Value);
                    }
                }
                catch
                {
                    WflTemplId = 0;
                }
            }

            if (!IsPostBack)
            {
                BindPageDefaultValues();

                BindStage();
            }

            hdnTmpID.Value = WflTemplId.ToString();
            if (this.Request.QueryString["StageID"] != null) // 如果有StageID
            {
                try
                {
                    int.Parse(this.Request.QueryString["StageID"].ToString());
                    StageID = this.Request.QueryString["StageID"].ToString();
                    ddlStage.SelectedValue = StageID;
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    //int.Parse(this.Request.QueryString["StageID"].ToString());

                    string StageName = ddlStage.SelectedItem.Text;

                    BindStage();
                    foreach (ListItem li in ddlStage.Items)
                    {
                        if(li.Text==StageName)
                        {
                            li.Selected = true;
                            break;
                        }
                    } 
                    StageID = ddlStage.SelectedValue;

                }
                catch
                {
                }
            }

            if (this.Request.QueryString["Alphabet"] != null) // 如果有Alphabet
            {
                Alphabet = this.Request.QueryString["Alphabet"].ToString();
            }
            else
            {
                if (!string.IsNullOrEmpty(ddlAlphabets.SelectedValue))
                    Alphabet = ddlAlphabets.SelectedValue.Trim();
            }
            ddlAlphabets.SelectedValue = Alphabet;
            if (this.Request.QueryString["PageIndex"] != null) // PageIndex
            {
                try
                {
                    PageIndex = int.Parse(this.Request.QueryString["PageIndex"].ToString());
                }
                catch
                {
                    PageIndex = 1;
                }
            }
            else
            {
                PageIndex = AspNetPager1.CurrentPageIndex;
            }

            if (this.Request.QueryString["FromPage"] != null) // FromPage
            {
                hdnPageFrom.Value = this.Request.QueryString["FromPage"];
            }
            else
            {
                if (ViewState["FromPage"] != null)
                {
                    hdnPageFrom.Value = ViewState["FromPage"].ToString();
                }
            }
            ViewState["FromPage"] = hdnPageFrom.Value;
            //if (hdnTmpID.Value.Length > 0 && hdnTmpID.Value != "0")
            //{
            //    WflTemplId = int.Parse(hdnTmpID.Value);
            //}
            //hdnTmpID.Value = WflTemplId.ToString();
            //ViewState["hdnTmpID"] = WflTemplId.ToString();
            if (!IsPostBack)
            {
                BindTemplateTasksGrid();
                BindControls();
                if (WflTemplId == 0)
                {
                    EnableBtns(false);
                }
                else
                {
                    EnableBtns(true);
                }
            }
            //BindStage();
        }

        private void EnableBtns(bool bEnabled)
        {
            btnDelete.Enabled = bEnabled;
            btnUpdateTask.Enabled = bEnabled;
            btnCreateTask.Enabled = bEnabled;
            btnDisable.Enabled = bEnabled;
        }

        /// <summary>
        /// Bind Page default values
        /// </summary>
        private void BindPageDefaultValues()
        {
            //Bind Alphabet
            foreach (string alphabet in alphabets.Split(','))
            {
                ddlAlphabets.Items.Add(new ListItem(alphabet, alphabet));
            }
        }

        private void BindControls()
        {
            if (WflTemplId == 0)
            {
                return;
            }
            try
            {
                LPWeb.Model.Template_Workflow model = new Model.Template_Workflow();
                model = template.GetModel(WflTemplId);
                if (model == null)
                {
                    return;
                }
                txbTemplateName.Text = model.Name;
                txbEescription.Text = model.Desc;
                cbEnabled.Checked = model.Enabled;
            }
            catch
            { }
            try
            {
                string strWhere = string.Format(" WflTemplId= {0} AND WflStageId = {1}",WflTemplId.ToString(),StageID);
                Template_Wfl_Stages stage = new Template_Wfl_Stages();
                DataSet ds = stage.GetList(strWhere);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    txbEstClose.Text = string.Empty;
                }
                else
                {
                    txbEstClose.Text = ds.Tables[0].Rows[0]["DaysFromEstClose"].ToString();
                }

            }
            catch
            { }
        }

        private void BindStage()
        {
            DataSet ds = new DataSet();
            try
            {
                if (WflTemplId == 0)
                {
                    Template_Stages stage = new Template_Stages();
                    ds = stage.GetList("");
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    {
                        return;
                    }
                    ddlStage.DataValueField = "TemplStageId";
                    ddlStage.DataTextField = "Name";
                    ddlStage.DataSource = ds;
                    ddlStage.DataBind();
                    ddlStage.Items.Insert(0, new ListItem("All", "0"));
                }
                else
                {
                    Template_Wfl_Stages stage = new Template_Wfl_Stages();
                    ds = stage.GetList(0, "WflTemplId = " + WflTemplId.ToString(), "SequenceNumber");

                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    {
                        return;
                    }
                    ddlStage.DataValueField = "WflStageId";
                    ddlStage.DataTextField = "Name";
                    ddlStage.DataSource = ds;
                    ddlStage.DataBind();
                    ddlStage.Items.Insert(0, new ListItem("All", "0"));

                    txbEstClose.Text = ds.Tables[0].Rows[0]["DaysFromEstClose"].ToString();
                }

            }
            catch
            { }
        }

        /// <summary>
        /// 根据用户界面选择生成过滤条件
        /// </summary>
        /// <returns></returns>
        private string GenerateQueryCondition()
        {
            //string queryCon = " Cleared IS NULL ";
            string queryCon = " 1=1 ";

            //if (!string.IsNullOrEmpty(ddlAlphabets.SelectedValue))
            //    //alphabets 
            if (!string.IsNullOrEmpty(Alphabet))
                queryCon += string.Format(" AND [Name] Like '{0}%' ", Alphabet);

            if (!string.IsNullOrEmpty(StageID) && StageID != "0" && WflTemplId > 0)
                queryCon += string.Format(" AND [WflStageId] = '{0}' ", StageID);

            queryCon += string.Format(" AND [WflTemplId] = {0} ", WflTemplId.ToString());
            return queryCon;
        }

        /// <summary>
        /// Bind Grid
        /// </summary>
        private void BindTemplateTasksGrid()
        {
            int pageSize = AspNetPager1.PageSize;
            int pageIndex = PageIndex;
            string queryCondition = GenerateQueryCondition();
            if (WflTemplId == 0)
            { return; }
            int recordCount = 0;

            DataSet tpLists = null;
            try
            {
                tpLists = template.GetTemplateWorkflowTasks(pageSize, pageIndex, queryCondition, out recordCount, OrderName, OrderType);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            gvTaskList.DataSource = null;
            gvTaskList.DataBind();
            gvTaskList.DataSource = tpLists;
            gvTaskList.DataBind();

        }

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
        /// Gets or sets the name of the order.
        /// </summary>
        /// <value>The name of the order.</value>
        public string OrderName
        {
            get
            {
                if (ViewState["orderName"] == null)
                {
                    if (OrderType == 0)
                        ViewState["orderName"] = "StageSequenceNumber],[SequenceNumber";
                    else
                        ViewState["orderName"] = "StageSequenceNumber] desc,[SequenceNumber";
                }
                return Convert.ToString(ViewState["orderName"]);
            }
            set
            {
                ViewState["orderName"] = value;
            }
        }

        public LPWeb.Model.Template_Workflow FillModel()
        {
            LPWeb.Model.Template_Workflow model = new LPWeb.Model.Template_Workflow();
            model.Name = txbTemplateName.Text.Trim();
            model.Enabled = cbEnabled.Checked;
            model.Desc = txbEescription.Text.Trim();
            model.DaysFromEstClose = int.Parse(txbEstClose.Text.Trim());
            model.WflTemplId = int.Parse(hdnTmpID.Value);
            try
            {
                model.StageId = int.Parse(StageID);
            }
            catch
            {

                model.StageId = 0;
            }
            return model;
        }

        public bool CheckInput()
        {
            if (txbTemplateName.Text.Trim().Length < 1)
            {
                PageCommon.AlertMsg(this, "Please enter the workflow template name.");
                return false;
            }
            else
            {
                try
                {
                    DataSet ds = template.GetList(" [name] = '" + txbTemplateName.Text.Trim() + "' and WflTemplId <> " + WflTemplId.ToString());
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    { }
                    else
                    {
                        PageCommon.AlertMsg(this, "The workflow template name already exists.");
                        return false;
                    }
                }
                catch { }
            }


            try
            {
                int.Parse(txbEstClose.Text.Trim());
            }
            catch
            {
                PageCommon.AlertMsg(this, "Please enter the stage target completion days from Est Close.");
                return false;
            }
            return true;
        }
        #endregion

        #region Event
        /// <summary>
        /// Handles the Sorting event of the gvTaskList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void gvTaskList_Sorting(object sender, GridViewSortEventArgs e)
        {
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
            OrderName = e.SortExpression;
            BindTemplateTasksGrid();
        }
        protected void ddlAlphabets_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindTemplatesGrid();
        }
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            //BindTemplatesGrid();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckInput())
            {
                return;
            }
            LPWeb.Model.Template_Workflow model = FillModel();
            try
            {
                hdnTmpID.Value = template.WorkflowTemplateAdd(model).ToString();
            }
            catch
            {

            }
        }

        protected void btnDisable_Click(object sender, EventArgs e)
        {
            string TWFIDs = hdnTmpIDs.Value;
            if (TWFIDs.Length > 0)
            {

                try
                {
                    Template_Wfl_Tasks task = new Template_Wfl_Tasks();
                    task.EnabledTemplTasks(TWFIDs, false);
                    //disable
                    BindTemplateTasksGrid();
                }
                catch
                { }
            }
            hdnTmpIDs.Value = string.Empty;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string TWFIDs = hdnTmpIDs.Value;
            if (TWFIDs.Length > 0)
            {
                try
                {
                    //TWFIDs = "'" + TWFIDs + "'";
                    Template_Wfl_Tasks task = new Template_Wfl_Tasks();
                    task.DeleteTasks(TWFIDs);
                    BindTemplateTasksGrid();
                }
                catch
                { }
            }
            hdnTmpIDs.Value = string.Empty;
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                int OldWfltID = int.Parse(hdnTmpID.Value);
                string TemplateName = txbTemplateName1.Text.Trim();
                try
                {
                    DataSet ds = template.GetList(" [name] = '" + txbTemplateName.Text.Trim() + "'");
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    { }
                    else
                    {
                        PageCommon.AlertMsg(this, "The workflow template name already exists.");
                        return;
                    }
                }
                catch { }
                hdnTmpID.Value = template.WorkflowTemplateClone(OldWfltID, TemplateName).ToString();
                PageLoad();
            }
            catch
            {
            }
        }

        protected void btnDeleteTemplate_Click(object sender, EventArgs e)
        {
            string TmpID = hdnTmpID.Value;
            if (TmpID.Length > 0 && TmpID != "0")
            {
                try
                {
                    int wflTemplId = 0;
                    int.TryParse(TmpID, out wflTemplId);
                    if (wflTemplId > 0)
                        template.WorkflowTemplateDelete(wflTemplId);
                }
                catch
                { }
            }
            hdnTmpIDs.Value = string.Empty;
        }
        #endregion
    }
}