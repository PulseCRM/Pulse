using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using System.Text;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.LoanDetails
{
    public partial class LoanActions : BasePage
    {
        public int iFileID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 校验页面参数

            string sReturnUrl = "../Pipeline/ProcessingPipelineSummary.aspx";
            string sErrorMsg = "Failed to load this page: missing required query string.";

            // FileID
            bool bIsValid = PageCommon.ValidateQueryString(this, "FileID", QueryStringType.ID);
            if (bIsValid == false)
            {
                PageCommon.WriteJsEnd(this, sErrorMsg, "window.parent.location.href='" + sReturnUrl + "'");
            }
            string sFileID = this.Request.QueryString["FileID"];
            this.hfFileID.Value = sFileID;
            this.iFileID = Convert.ToInt32(sFileID);

            #endregion
        }
    }
}
