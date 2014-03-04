using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.Pipeline
{
    /// <summary>
    /// Point Workflow template selection page
    /// Author：Peter
    /// Date：2011-02-27
    /// </summary>
    public partial class WfTpltSelection : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strWfType = Request.QueryString["wt"];
                BLL.Template_Workflow WFTplt = new BLL.Template_Workflow();
                string sqlCondStr = string.Format(" Enabled=1 AND WorkflowType='{0}'", strWfType);
                DataSet dsWFTplt = WFTplt.GetList(sqlCondStr);
                this.gvList.DataSource = dsWFTplt;
                this.gvList.DataBind();
            }
        }

        protected void lbtnSelect_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvList.Rows)
            {
                CheckBox ckbSelected = row.FindControl("ckbSelected") as CheckBox;
                if (ckbSelected.Checked)
                {
                    ClientFun("callback", string.Format("callBack('{0}');", gvList.DataKeys[row.RowIndex].Value.ToString()));
                    return;
                }
            }
        }

        /// <summary>
        /// Call client function
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strScript"></param>
        private void ClientFun(string strKey, string strScript)
        {
            ClientScriptManager csm = this.Page.ClientScript;
            if (!csm.IsStartupScriptRegistered(this.GetType(), strKey))
            {
                csm.RegisterStartupScript(this.GetType(), strKey, strScript, true);
            }
        }
    }
}
