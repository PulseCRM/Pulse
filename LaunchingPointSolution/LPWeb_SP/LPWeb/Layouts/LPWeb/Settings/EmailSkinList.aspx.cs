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

namespace LPWeb.Layouts.LPWeb.Settings
{
    public partial class EmailSkinList : BasePage
    {
        bool isReset = false;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        public void BindGrid()
        {
            BLL.Template_EmailSkins tmpEmailSkins = new BLL.Template_EmailSkins();

            int pageSize = AspNetPager1.PageSize;
            int pageIndex = 1;

            if (isReset == true)
                pageIndex = AspNetPager1.CurrentPageIndex = 1;
            else
                pageIndex = AspNetPager1.CurrentPageIndex;

            string strWhare = "";

            if (!string.IsNullOrEmpty(ddlAlphabet.SelectedValue.Trim()))
            {
                strWhare += " And Name like '" + ddlAlphabet.SelectedValue.Trim() + "%'";
            }

            int recordCount = 0;

            DataSet listData = null;
            try
            {
                listData = tmpEmailSkins.GetListForGridView(pageSize, pageIndex, strWhare, out recordCount, OrderName, OrderType);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = recordCount;

            gridList.DataSource = listData;
            gridList.DataBind();

        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
            BindGrid();
        }


        protected void lbtnDisable_Click(object sender, EventArgs e)
        {
            BLL.Template_EmailSkins tmpEmailSkinsbll = new BLL.Template_EmailSkins();

            string sIds = GetSelectedIDs();
            

            if (!string.IsNullOrEmpty(sIds))
            {
                try
                {
                    tmpEmailSkinsbll.SetDisable(sIds);
                }
                catch (Exception ex)
                {
                    ClientFun(this.updatePanel, "failedtodisableet", "alert('Failed to disable the selected email template skin(s), please try it again.');");
                    LPLog.LogMessage(LogType.Logerror, "Failed to disable the selected email template skin(s): " + ex.Message);
                    return;
                }
                BindGrid();
            }
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            var Ids = GetSelectedIDs();

            BLL.Template_EmailSkins emailSkinsBll = new BLL.Template_EmailSkins();

            emailSkinsBll.DeleteList(Ids);

            emailSkinsBll.SetTmpEmail_SkinIdNull(Ids);

            BindGrid();

        }

        protected void lbtnClone_Click(object sender, EventArgs e)
        {
            var emailSkinId = GetSelectedIDList().FirstOrDefault();
            if (emailSkinId == 0)
            {
                return;
            }

            BLL.Template_EmailSkins emailSkinsBll = new BLL.Template_EmailSkins();

            var cloneModel = emailSkinsBll.GetModel(emailSkinId);

            if (cloneModel != null && cloneModel.EmailSkinId == emailSkinId)
            {
                cloneModel.EmailSkinId = 0;
                cloneModel.Name += " Copy";

                cloneModel.Default = false;

                emailSkinsBll.Add(cloneModel);
            }
            BindGrid();
        }

        protected void lbtnEmpty_Click(object sender, EventArgs e)
        {
            //isReset = true;
            //BindGrid();
        }

        protected void lbtnEmpty2_Click(object sender, EventArgs e)
        {
            //isReset = false;
            //BindGrid();
        }



        protected void gridList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        
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


        public List<int> GetSelectedIDList()
        {
            List<int> listIDs = new List<int>();
           
            foreach (GridViewRow row in gridList.Rows)
            {
                if (DataControlRowType.DataRow == row.RowType)
                {
                    CheckBox ckbChecked = row.FindControl("ckbSelected") as CheckBox;
                    if (null != ckbChecked && ckbChecked.Checked)
                    {
                        //sIds += "," + gridList.DataKeys[row.RowIndex].Value;
                        listIDs.Add((int)gridList.DataKeys[row.RowIndex].Value);
                    }
                }
            }

            return listIDs;
        }

        public string GetSelectedIDs()
        {
            string Ids = "";
            var idList = GetSelectedIDList();
            foreach (int Id in idList)
            {
                Ids += "," + Id;
            }
            if (!string.IsNullOrEmpty(Ids))
            {
                Ids = Ids.Remove(0, 1);
            }

            return Ids;
        }
    }
}
