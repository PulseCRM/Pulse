using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Settings
{
    public partial class RegionSetupExecutiveSelection : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["groupid"] != null && Request.QueryString["regionid"] != null)
            {
                BindGrid(int.Parse(Request.QueryString["groupid"].ToString()), int.Parse(Request.QueryString["regionid"].ToString()));
            }
        }

        public void BindGrid(int groupId,int regionId)
        {
            Users bllUser = new Users();
            var executivesSelectionList = bllUser.GetRegionExecutivesSelectionList(regionId, groupId);
            this.gridExecutiveSelectionList.DataSource = executivesSelectionList;
            gridExecutiveSelectionList.DataBind();
        }
    }
}