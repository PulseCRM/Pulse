using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;
using LPWeb.Layouts.LPWeb.Common;
using Utilities;
using System.Xml;

namespace LPWeb.Layouts.LPWeb.Settings
{
    /// <summary>
    /// rule selection
    /// </summary>
    public partial class RuleSelection : BasePage
    {
        BLL.Template_Rules rulesManager = new BLL.Template_Rules();
        List<string> listCurrRow = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // get current selected rows
                string strCurrRecord = Request.QueryString["currIds"];
                if (!string.IsNullOrEmpty(strCurrRecord))
                    listCurrRow = strCurrRecord.Split(',').ToList();

                // save all current data in hidden field
                DataSet listCurrData = rulesManager.GetListOfCurrSelectedRule(listCurrRow);
                this.hiCheckedData.Value = GetEncodedXmlOfRuleItems(listCurrData);

                BindGrid();
            }
        }

        /// <summary>
        /// get selected data as encoded xml
        /// </summary>
        /// <param name="dsCurr"></param>
        /// <returns></returns>
        private string GetEncodedXmlOfRuleItems(DataSet dsCurr)
        {
            if (null == dsCurr || dsCurr.Tables.Count != 1)
                return "<table />";

            // return selected record as XML
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement element = xmlDoc.CreateElement("table");
            xmlDoc.AppendChild(element);
            XmlElement childElement = null;
            XmlAttribute attri = null;

            foreach (DataRow row in dsCurr.Tables[0].Rows)
            {
                childElement = xmlDoc.CreateElement("tr");
                element.AppendChild(childElement);

                attri = xmlDoc.CreateAttribute("RuleId");
                attri.Value = row["RuleId"].ToString();
                childElement.Attributes.Append(attri);

                attri = xmlDoc.CreateAttribute("Name");
                attri.Value = row["Name"].ToString().Replace("<", "&lt;");
                childElement.Attributes.Append(attri);

                attri = xmlDoc.CreateAttribute("AlertEmailTemplId");
                attri.Value = row["AlertEmailTemplId"].ToString();
                childElement.Attributes.Append(attri);

                attri = xmlDoc.CreateAttribute("AlertEmailTpltName");
                attri.Value = row["AlertEmailTpltName"].ToString().Replace("<", "&lt;");
                childElement.Attributes.Append(attri);
            }
            return xmlDoc.OuterXml.Replace('<', '\u0001').Replace("'", "&#39;");
        }

        /// <summary>
        /// Bind rule gridview
        /// </summary>
        private void BindGrid()
        {
            //int pageSize = AspNetPager1.PageSize;
            //int pageIndex = 1;

            //if (isReset == true)
            //    pageIndex = AspNetPager1.CurrentPageIndex = 1;
            //else
            //    pageIndex = AspNetPager1.CurrentPageIndex;

            string strWhare = GetSqlWhereClause();
            int recordCount = 0;

            DataSet listData = null;
            try
            {
                listData = rulesManager.GetListForRuleSelection(10000, 0, strWhare, out recordCount, OrderName, OrderType);
                this.hiAllData.Value = GetEncodedXmlOfRuleItems(listData);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            //AspNetPager1.PageSize = pageSize;
            //AspNetPager1.RecordCount = recordCount;

            gridList.DataSource = listData;
            gridList.DataBind();
        }

        StringBuilder sbAllIds = new StringBuilder();
        string strCkAllID = "";
        /// <summary>
        /// Set selected row when click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView gv = sender as GridView;
            if (null == gv)
                return;

            if (DataControlRowType.Header == e.Row.RowType)
            {
                CheckBox ckbAll = e.Row.FindControl("ckbAll") as CheckBox;
                if (null != ckbAll)
                {
                    ckbAll.Attributes.Add("onclick", string.Format("CheckAllClicked(this, '{0}', '{1}', '{2}', '{3}');",
                        gv.ClientID, this.hiAllIds.ClientID, this.hiCheckedIds.ClientID, this.hiAllData.ClientID));
                    strCkAllID = ckbAll.ClientID;
                }
            }
            else if (DataControlRowType.DataRow == e.Row.RowType)
            {
                string strID = string.Format("{0}", gv.DataKeys[e.Row.RowIndex].Value);
                string strName = string.Format("{0}", gv.DataKeys[e.Row.RowIndex]["Name"]).Replace("'", "&#39;").Replace("<", "&amp;lt;");
                string strAlertTemplId = string.Format("{0}", gv.DataKeys[e.Row.RowIndex]["AlertEmailTemplId"]);
                string strAlertTpltName = string.Format("{0}", gv.DataKeys[e.Row.RowIndex]["AlertEmailTpltName"]).Replace("'", "&#39;").Replace("<", "&amp;lt;");

                if (sbAllIds.Length > 0)
                    sbAllIds.Append(",");
                sbAllIds.AppendFormat("{0}", strID);

                CheckBox ckb = e.Row.FindControl("ckbSelect") as CheckBox;
                if (null != ckb)
                {
                    if (listCurrRow.Contains(gv.DataKeys[e.Row.RowIndex].Value.ToString()))
                    {
                        ckb.Checked = true;
                    }
                    ckb.Attributes.Add("onclick", string.Format("CheckBoxClicked(this, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');",
                        strCkAllID, this.hiAllIds.ClientID, this.hiCheckedIds.ClientID, strID, strName, strAlertTemplId, strAlertTpltName));
                }
            }
        }

        protected void gridList_PreRender(object sender, EventArgs e)
        {
            this.hiAllIds.Value = sbAllIds.ToString();
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

        /// <summary>
        /// Get filter
        /// </summary>
        /// <returns></returns>
        private string GetSqlWhereClause()
        {
            string strWhere = " AND Enabled=1";
            //if (this.ddlAlphabet.SelectedIndex > 0)
            //{
            //    if (strWhere.Length > 0)
            //    {
            //        strWhere = string.Format("{0} AND Name LIKE '{1}%'", strWhere, this.ddlAlphabet.SelectedValue);
            //    }
            //    else
            //    {
            //        strWhere = string.Format("AND Name LIKE '{0}%'", ddlAlphabet.SelectedValue);
            //    }
            //}
            return strWhere;
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            //ViewState["pageIndex"] = AspNetPager1.CurrentPageIndex;
            BindGrid();
        }

        protected void lbtnEmpty_Click(object sender, EventArgs e)
        {
            //isReset = false;
            BindGrid();
        }

        protected void lbtnEmptyReset_Click(object sender, EventArgs e)
        {
            //isReset = true;
            BindGrid();
        }
    }
}
