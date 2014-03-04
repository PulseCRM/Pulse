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

    public partial class MergeContactsPopup : BasePage
    {
        string sCloseDialogCodes = string.Empty;

        private void Page_Load(object sender, EventArgs e)
        {
            DataSet dataSet = null;
            try
            {
                string sContacts = Request.QueryString["contacts"];
                if (!string.IsNullOrEmpty(sContacts))
                {
                    Contacts bllCnt = new Contacts();
                    dataSet = bllCnt.GetContactsByFileIds(sContacts);
                }            // CloseDialogCodes
                bool bIsValid = PageCommon.ValidateQueryString(this, "CloseDialogCodes", QueryStringType.String);
                if (bIsValid == false)
                {
                    PageCommon.WriteJsEnd(this, "Missing required query string.", "window.parent.location.href=window.parent.location.href");
                }
                this.sCloseDialogCodes = this.Request.QueryString["CloseDialogCodes"].ToString() + ";";
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }
            this.gvContacts.DataSource = dataSet;
            this.gvContacts.DataBind();
        }

        protected void btnMerge_Click(object sender, EventArgs e)
        {
            string sFrom = this.hfFroms.Value;
            string sTo = this.htTo.Value;

            int iTo = 0;
            if (!int.TryParse(sTo, out iTo))
            {
                PageCommon.AlertMsg(this, "Invalid paramter merge contact to.");
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
                    PageCommon.AlertMsg(this, "Invalid paramter merge contact(s) from.");
                }
            }

            Contacts bllCnt = new Contacts();
            try
            {
                if(bllCnt.MergeContacts(iFroms,iTo,CurrUser.iUserID))
                {
                    //PageCommon.WriteJsEnd(this, "Merge Contact(s) Successfully", "btnCancel_onclick();");
                    PageCommon.WriteJsEnd(this, "Merged contact(s) successfully.", "window.parent.location.href=window.parent.location.href;" + this.sCloseDialogCodes);
                }
            }
            catch (Exception exception)
            {
                PageCommon.WriteJsEnd(this, "Failed to merge contact(s).", PageCommon.Js_RefreshSelf);
                LPLog.LogMessage(exception.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
