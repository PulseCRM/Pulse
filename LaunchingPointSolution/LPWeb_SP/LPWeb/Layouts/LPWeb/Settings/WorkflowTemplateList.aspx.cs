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
    public partial class WorkflowTemplateList : BasePage
    {
        #region Parameters

        private bool isReset = false;
        const string alphabets = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        public string FromURL = string.Empty;
        Template_Workflow template = new Template_Workflow();
        Company_Point comPointMngr = new Company_Point();
        string Alphabet = string.Empty;
        int PageIndex = 1;
        private string SortType = "ASC";

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
                    ViewState["orderName"] = "Name";
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
                    hdnWorkflowTempl.Value = loginUser.userRole.WorkflowTempl.ToString();
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
            if (!IsPostBack)
            {
                BindTemplatesGrid();

                // load workflow configuration info
                Model.Company_Point comPoint = comPointMngr.GetModel();
                if (null != comPoint)
                {
                    this.ckbAutoProcessing.Checked = comPoint.AutoApplyProcessingWorkflow.GetValueOrDefault(false);
                    this.ckbAutoProspecting.Checked = comPoint.AutoApplyProspectWorkflow.GetValueOrDefault(false);
                }
            }
        }
        #endregion

        #region Function
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

            int recordCount = 0;

            DataSet tpLists = null;
            DataTable dtList = null;
            try
            {
                tpLists = template.GetTemplateWorkflows(pageSize, pageIndex, queryCondition, out recordCount, OrderName, OrderType);
                if (!tpLists.Tables[0].Columns.Contains("Stages"))
                {
                    tpLists.Tables[0].Columns.Add("Stages");
                }
                if (!tpLists.Tables[0].Columns.Contains("Custom_Cov"))
                {
                    tpLists.Tables[0].Columns.Add("Custom_Cov");
                }
                if (!tpLists.Tables[0].Columns.Contains("Default_Cov"))
                {
                    tpLists.Tables[0].Columns.Add("Default_Cov");
                }
                if (!tpLists.Tables[0].Columns.Contains("Enabled_Cov"))
                {
                    tpLists.Tables[0].Columns.Add("Enabled_Cov");
                }

                Template_Wfl_Stages stage;
                DataSet ds;
                string sName = "";
                string Default_Type = "";
                int search_idx = -1;
                int iWflTempId = 0;
                
                Template_Workflow WorkflowTemplateManager = new Template_Workflow();               
           
                foreach (DataRow dr in tpLists.Tables[0].Rows)
                {
                    if (dr["WflTemplId"] != DBNull.Value)
                    {
                        iWflTempId = (int)dr["WflTemplId"];
                    }

                     if ((dr["Name"] != DBNull.Value) && (dr["Name"].ToString() != ""))
                    {
                    sName = (string)dr["Name"];
                    if (sName != null)
                        {
                            sName = sName.ToLower();
                            search_idx = sName.IndexOf("prospect");
                            if (search_idx >= 0)
                            {
                                Default_Type = "Prospect";
                            }
                            else
                            {
                                Default_Type = "Processing";
                            }                           
                        }
                    }

                    if (dr["WorkflowType"] == DBNull.Value || dr["WorkflowType"].ToString() == "")
                    {
                        //Default Processing
                        dr["WorkflowType"] = Default_Type;
                        WorkflowTemplateManager.UpdateWorkflowType(iWflTempId, Default_Type);
                    }
                    if (dr["Default"] == DBNull.Value || dr["Default"].ToString() == "" || dr["Default"].ToString() == "0" || dr["Default"].ToString() == "False")
                    {
                        //Default No
                        dr["Default_Cov"] = "No";
                    }
                    else
                    {
                        dr["Default_Cov"] = "Yes";
                    }
                    if (dr["Custom"] == DBNull.Value || dr["Custom"].ToString() == "" || dr["Custom"].ToString() == "0" || dr["Custom"].ToString() == "False")
                    {
                        //Default Standard
                        dr["Custom_Cov"] = "Standard";
                    }
                    else
                    {
                        dr["Custom_Cov"] = "Custom";
                    }

                    if (dr["Enabled"] == DBNull.Value || dr["Enabled"].ToString() == "" || dr["Enabled"].ToString() == "0" || dr["Enabled"].ToString() == "False")
                    {
                        dr["Enabled_Cov"] = "No";
                    }
                    else
                    {
                        dr["Enabled_Cov"] = "Yes";
                    }

                    stage = new Template_Wfl_Stages();
                    ds = stage.GetList(0, "WflTemplId = " + dr["WflTemplId"].ToString(), "SequenceNumber");
                    dr["Stages"] = ds.Tables[0].Rows.Count;
                }
                if (OrderName == "Stages")
                {
                    DataView dv = tpLists.Tables[0].DefaultView;
                    dv.Sort = "Stages " + SortType;
                    dtList = dv.ToTable();
                }
                else
                {
                    tpLists.AcceptChanges();
                    dtList = tpLists.Tables[0];
                }

            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            gvWorkFolwList.DataSource = dtList;
            gvWorkFolwList.DataBind();

        }
        #endregion

        #region Event
        /// <summary>
        /// Handles the Sorting event of the gvWorkFolwList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void gvWorkFolwList_Sorting(object sender, GridViewSortEventArgs e)
        {
            OrderName = e.SortExpression;
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)                      //设置排序方向
            {
                GridViewSortDirection = SortDirection.Descending;
                OrderType = 0;
                SortType = "Desc";
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                OrderType = 1;
                SortType = "Asc";
            }
            BindTemplatesGrid();
        }

        protected void ddlAlphabets_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindTemplatesGrid();
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            //BindTemplatesGrid();
        }

        protected void btnDisable_Click(object sender, EventArgs e)
        {
            string TWFIDs = hdnTmpIDs.Value;
            if (TWFIDs.Length > 0)
            {

                try
                {
                    template.DisableWorkflowTemplates(TWFIDs);
                    BindTemplatesGrid();
                }
                catch
                { }
            }
            hdnTmpIDs.Value = string.Empty;
        }

        protected void btnEnable_Click(object sender, EventArgs e)
        {
            string TWFIDs = hdnTmpIDs.Value;
            if (TWFIDs.Length > 0)
            {

                try
                {
                    template.EnableWorkflowTemplates(TWFIDs);
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
            if (TWFIDs.Length <= 0)
                return;
            string[] templIdList;
            try
            {
                templIdList = TWFIDs.Split(',');
                if (templIdList.Length <= 0)
                    return;
                int wflTemplId = 0;
                BLL.Template_Workflow tw = new Template_Workflow();
                foreach (string tempId in templIdList)
                {
                    try
                    {
                        wflTemplId = 0;
                        int.TryParse(tempId, out wflTemplId);
                        if (wflTemplId > 0)
                            tw.WorkflowTemplateDelete(wflTemplId);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                BindTemplatesGrid();

            }
            catch
            { }
            finally
            {
                hdnTmpIDs.Value = string.Empty;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Model.Company_Point comPoint = comPointMngr.GetModel();
                comPoint.AutoApplyProcessingWorkflow = this.ckbAutoProcessing.Checked;
                comPoint.AutoApplyProspectWorkflow = this.ckbAutoProspecting.Checked;
                comPointMngr.Update(comPoint);
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to save workflow configuration information.");
                LPLog.LogMessage(LogType.Logerror, ex.Message);
            }
        }
        #endregion

    }
}
