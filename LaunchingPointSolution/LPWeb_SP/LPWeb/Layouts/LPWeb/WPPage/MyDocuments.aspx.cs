using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;

namespace LPWeb.Layouts.LPWeb.WPPage
{
    public partial class MyDocuments : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SPWeb web = this.Web;
            //SPList spList = web.Lists[System.Configuration.ConfigurationManager.AppSettings["WPMyDocsName"]];
            //SPView spView = spList.Views[System.Configuration.ConfigurationManager.AppSettings["WPMyDocsViewName"]];
            //this.xlvwpMyDocs.ListId = spList.ID;
            //this.xlvwpMyDocs.ListName = string.Format("{{{0}}}", spList.ID.ToString());
            //this.xlvwpMyDocs.ViewGuid = spView.ID.ToString();

            string strPersonalSiteUrl = System.Configuration.ConfigurationManager.AppSettings["PersonalSiteUrl"];
            string strPersonalDocLib = System.Configuration.ConfigurationManager.AppSettings["WPMyDocsName"];
            if (!string.IsNullOrEmpty(strPersonalSiteUrl))
            {
                // check parameters
                strPersonalSiteUrl = strPersonalSiteUrl.TrimEnd(new char[] { '/' });
                if (string.IsNullOrEmpty(strPersonalDocLib))
                    strPersonalDocLib = "Personal Documents";   // default personal document library name

                SPSite mySite = new SPSite(string.Format("{0}/my/personal/{1}", strPersonalSiteUrl, CurrUser.sUserName));
                SPWeb myWeb = mySite.OpenWeb();
                SPList list = myWeb.Lists[strPersonalDocLib];

                // data source table
                DataTable dtDocs = new DataTable();
                DataColumn col = new DataColumn("Name");
                dtDocs.Columns.Add(col);
                col = new DataColumn("Modified");
                dtDocs.Columns.Add(col);

                // get data from document library list
                if (null != list && list.Items.Count > 0)
                {
                    DataRow dr = null;
                    foreach (SPListItem item in list.Items)
                    {
                        dr = dtDocs.NewRow();
                        dtDocs.Rows.Add(dr);
                        dr["Name"] = item["Name"];
                        dr["Modified"] = item["Modified"];
                    }
                }

                // bind list data to GridView
                this.gridList.DataSource = dtDocs;
                this.gridList.DataBind();
            }
        }
    }
}
