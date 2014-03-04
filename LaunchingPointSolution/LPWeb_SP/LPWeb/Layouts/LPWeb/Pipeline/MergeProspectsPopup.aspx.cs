using System;
using System.Collections.Generic;
using System.Linq;
using LPWeb.BLL;
using LPWeb.Common;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;

namespace LPWeb.Layouts.LPWeb.Pipeline
{

    public partial class MergeProspectsPopup : BasePage
    {
        private void Page_Load(object sender, EventArgs e)
        {
            DataSet dataSet = null;
            try
            {
                string sProspects = Request.QueryString["prospects"];
                if (!string.IsNullOrEmpty(sProspects))
                {
                    Contacts bllCnt = new Contacts();
                    dataSet = bllCnt.GetProspectsByFileIds(sProspects);
                }
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }
            this.gvProspect.DataSource = dataSet;
            this.gvProspect.DataBind();
        }

        protected void btnMerge_Click(object sender, EventArgs e)
        {
            string sFrom = this.hfFroms.Value;
            string sTo = this.htTo.Value;

            int iTo = 0;
            if (!int.TryParse(sTo, out iTo))
            {
                PageCommon.AlertMsg(this, "invalid paramter merge prospect to.");
            }
            List<int> iFroms = new List<int>();
            if (!string.IsNullOrEmpty(sFrom))
            {
                try
                {
                    var query = from it in sFrom.Split(new char[] { ',' }) select int.Parse(it);
                    iFroms = query.ToList();
                }
                catch (Exception exception)
                {
                    LPLog.LogMessage(exception.Message);
                    PageCommon.AlertMsg(this, "invalid paramter merge prospect(s) from.");
                }
            }

            Contacts bllCnt = new Contacts();
            try
            {
                if(bllCnt.MergeProspects(iFroms,iTo,CurrUser.iUserID))
                {
                    //PageCommon.WriteJsEnd(this, "Merge prospect(s) Successfully", PageCommon.Js_RefreshSelf);
                    PageCommon.WriteJsEnd(this, "Merge prospect(s) Successfully", "window.parent.CloseDialog_MergeProspects();");
                }
            }
            catch (Exception exception)
            {
                PageCommon.WriteJsEnd(this, "Merge prospect(s) failed", PageCommon.Js_RefreshSelf);
                LPLog.LogMessage(exception.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
