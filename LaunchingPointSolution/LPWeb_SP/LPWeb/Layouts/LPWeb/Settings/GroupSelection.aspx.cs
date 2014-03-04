using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Settings
{
    /// <summary>
    /// Group selection page
    /// Author：Peter
    /// Date：2010-09-04
    /// </summary>
    public partial class GroupSelection : BasePage
    {
        List<string> listCurrRow = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // get current selected rows
                string strCurrRecord = Request.QueryString["currIds"];
                if (!string.IsNullOrEmpty(strCurrRecord))
                    listCurrRow = strCurrRecord.Split(',').ToList();

                BLL.Groups GroupManager = new BLL.Groups();
                DataSet dsGroup = GroupManager.GetList("Enabled=1");
                this.gvGroup.DataSource = dsGroup;
                this.gvGroup.DataBind();
            }
        }

        protected void gvGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (DataControlRowType.DataRow == e.Row.RowType)
            {
                // set current selected row selected and disabled
                CheckBox ckbSelected = e.Row.FindControl("ckbSelected") as CheckBox;
                if (null != ckbSelected)
                {
                    if (listCurrRow.Contains(gvGroup.DataKeys[e.Row.RowIndex].Value.ToString()))
                    {
                        ckbSelected.Checked = true;
                        //ckbSelected.Enabled = false;
                    }
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

        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            // return selected record as XML
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement element = xmlDoc.CreateElement("root");
            xmlDoc.AppendChild(element);
            XmlElement childElement = null;
            XmlAttribute attri = null;
            foreach (GridViewRow row in gvGroup.Rows)
            {
                CheckBox ckbSelected = row.FindControl("ckbSelected") as CheckBox;
                if (ckbSelected.Checked)
                {
                    childElement = xmlDoc.CreateElement("Group");
                    element.AppendChild(childElement);

                    attri = xmlDoc.CreateAttribute("GroupId");
                    attri.Value = gvGroup.DataKeys[row.RowIndex].Value.ToString();
                    childElement.Attributes.Append(attri);

                    attri = xmlDoc.CreateAttribute("GroupName");
                    attri.Value = gvGroup.DataKeys[row.RowIndex][1].ToString();
                    childElement.Attributes.Append(attri);
                }
            }

            ClientFun("callback", string.Format("callBack('{0}');", xmlDoc.OuterXml.Replace('<', '\u0001')));
        }
    }
}