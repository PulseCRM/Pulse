using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

namespace LPWeb.Settings
{
    /// <summary>
    /// User Loan Rep selection page
    /// Author：Peter
    /// Date：2010-09-04
    /// </summary>
    public partial class UserLoanRepSelection : System.Web.UI.Page
    {
        string strCurrRecord = "";
        List<string> listCurrRow = new List<string>();
        int nUserID = 0;
        string lastNamefilter = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // get current selected rows
            strCurrRecord = Request.QueryString["currIds"];
            if (!string.IsNullOrEmpty(strCurrRecord))
                listCurrRow = strCurrRecord.Split(',').ToList();

            if (!int.TryParse(Request.QueryString["uid"], out nUserID))
                nUserID = 0;

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            BLL.UserLoanRep UserLoanRepManager = new BLL.UserLoanRep();
            string strWhere = "";
            if (0 == nUserID)
                strWhere = "ISNULL(UserId, 0)=0";
            else
                strWhere = string.Format("(ISNULL(UserId, 0)=0 OR UserId={0})", nUserID);

            if (!string.IsNullOrEmpty(lastNamefilter.Trim()))
            {
                strWhere += string.Format(" And  Name LIKE '%{0}%' ", lastNamefilter.Trim());
            }

            strWhere += " Order by Name ";

            DataSet dsLoanRep = UserLoanRepManager.GetList(strWhere);
            this.gridLoanRep.DataSource = dsLoanRep;
            this.gridLoanRep.DataBind();
        }

        protected void btnFilterLastName_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txbLastName.Text.Trim()))
            {
                lastNamefilter = txbLastName.Text.Trim();
            }
            BindData();
        }


        protected void gridLoanRep_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (DataControlRowType.DataRow == e.Row.RowType)
            {
                // set current selected row selected and disabled
                CheckBox ckbSelected = e.Row.FindControl("ckbSelected") as CheckBox;
                if (null != ckbSelected)
                {
                    if (listCurrRow.Contains(gridLoanRep.DataKeys[e.Row.RowIndex].Value.ToString()))
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
            foreach (GridViewRow row in gridLoanRep.Rows)
            {
                CheckBox ckbSelected = row.FindControl("ckbSelected") as CheckBox;
                if (ckbSelected.Checked)
                {
                    var id = gridLoanRep.DataKeys[row.RowIndex].Value.ToString();
                    if (!listCurrRow.Contains(id)) //曾经选过 就不添加
                    {
                        childElement = xmlDoc.CreateElement("LoanRep");
                        element.AppendChild(childElement);

                        attri = xmlDoc.CreateAttribute("NameId");
                        attri.Value = id;
                        childElement.Attributes.Append(attri);

                        attri = xmlDoc.CreateAttribute("Name");
                        attri.Value = gridLoanRep.DataKeys[row.RowIndex][1].ToString();
                        childElement.Attributes.Append(attri);
                    }
                }
            }
            ///补充 原有选过的
            ///
            if (strCurrRecord.Count() > 0)
            {
                BLL.UserLoanRep UserLoanRepManager = new BLL.UserLoanRep();
                string strWhere = "";
                if (0 == nUserID)
                    strWhere = "ISNULL(UserId, 0)=0";
                else
                    strWhere = string.Format("(ISNULL(UserId, 0)=0 OR UserId={0})", nUserID);

                strWhere += string.Format(" And NameId in({0})  ", strCurrRecord);

                DataSet dsLoanRep = UserLoanRepManager.GetList(strWhere);

                if (dsLoanRep != null && dsLoanRep.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsLoanRep.Tables[0].Rows)
                    {
                        childElement = xmlDoc.CreateElement("LoanRep");
                        element.AppendChild(childElement);

                        attri = xmlDoc.CreateAttribute("NameId");
                        attri.Value = dr["NameId"].ToString();
                        childElement.Attributes.Append(attri);

                        attri = xmlDoc.CreateAttribute("Name");
                        attri.Value = dr["Name"].ToString();
                        childElement.Attributes.Append(attri);
                    }
                }
            }
            

            ClientFun("callback", string.Format("callBack('{0}');", xmlDoc.OuterXml.Replace('<', '\u0001')));
        }
    }
}