using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Web.UI.WebControls;
using System.Data;
using Utilities;
using LPWeb.Common;
using LPWeb.BLL;

namespace LPWeb.Layouts.LPWeb.Settings
{
    public partial class StageTemplateList : BasePage
    {
        #region Parameters

        public string FromURL = string.Empty;
        Template_Stages template = new Template_Stages();
        int PageIndex = 1;
        string sWorkflowType = "";
        string SortType = "Asc";

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
                {
                    ViewState["orderName"] = "SequenceNumber";
                }
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
                else
                {
                    this.hdnStageTemplate.Value = loginUser.userRole.WorkflowTempl.ToString();
                    if (loginUser.userRole.WorkflowTempl.ToString().IndexOf('2') == -1)
                    {
                        btnDisable.Enabled = false;
                    }
                    if (loginUser.userRole.WorkflowTempl.ToString().IndexOf('3') == -1)
                    {
                        btnDelete.Enabled = false;
                    }
                }
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }
            if (Request.Url != null)
            {
                FromURL = Request.Url.ToString();
            }
            if (!IsPostBack)
            {
                BindPageDefaultValues();
            }

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

            if (this.Request.QueryString["WorkflowType"] != null)
            {
                this.sWorkflowType = this.Request.QueryString["WorkflowType"].ToString();
                this.ddlWorkflowType.SelectedValue = this.sWorkflowType;
            }
            if (!IsPostBack)
            {
                BindTemplatesGrid();
            }
        }
        #endregion

        #region Function
        /// <summary>
        /// Bind Page default values
        /// </summary>
        private void BindPageDefaultValues()
        {

            //Binding workflow type
            this.ddlWorkflowType.Items.Add(new ListItem("All Workflow Types", ""));
            this.ddlWorkflowType.Items.Add(new ListItem("Processing", "Processing"));
            this.ddlWorkflowType.Items.Add(new ListItem("Prospect", "Prospect"));
            this.ddlWorkflowType.SelectedIndex = 0;
        }

        /// <summary>
        /// 根据用户界面选择生成过滤条件
        /// </summary>
        /// <returns></returns>
        private string GenerateQueryCondition()
        {
            string queryCon = " 1=1 ";
            
            if (this.Request.QueryString["WorkflowType"] != null)
            {
                string sWorkflowType = this.Request.QueryString["WorkflowType"];
                queryCon += string.Format(" AND ISNULL([WorkflowType],'Processing') Like '%{0}%' ", sWorkflowType);
            }
            
            return queryCon;
        }

        /// <summary>
        /// Bind Grid
        /// </summary>
        private void BindTemplatesGrid()
        {
            int pageSize = AspNetPager1.PageSize;
            int pageIndex = PageIndex;
            string queryCondition = GenerateQueryCondition();

            #region order by
            OrderName = "WorkflowType,SequenceNumber";
            OrderType = 0;
            if (this.Request.QueryString["OrderByField"] != null)
            {
                OrderName = this.Request.QueryString["OrderByField"];
                OrderType = Convert.ToInt32(this.Request.QueryString["OrderByType"]);
            }

            #endregion

            int recordCount = 0;

            DataSet tpLists = null;
            DataTable dtList = null;
            try
            {
                tpLists = template.GetStageList(pageSize, pageIndex, queryCondition, out recordCount, OrderName, OrderType);
                dtList = tpLists.Tables[0];
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            this.gvStageList.DataSource = dtList;
            gvStageList.DataBind();

        }
        #endregion

        #region Event
        
        protected void btnDisable_Click(object sender, EventArgs e)
        {
            string TWFIDs = hdnTmpIDs.Value;
            if (TWFIDs.Length > 0)
            {

                try
                {
                    this.template.DisableStageTemplates(TWFIDs);
                    BindTemplatesGrid();
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
                    TWFIDs = "'" + TWFIDs.Replace(",", "','") + "'";
                    template.DeleteStageTemplates(TWFIDs);
                    BindTemplatesGrid();
                }
                catch
                { }
            }
            hdnTmpIDs.Value = string.Empty;
        }
        #endregion
    }
}
