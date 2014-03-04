using System;
using System.Data;
using LPWeb.BLL;
using LPWeb.Common;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace LPWeb.Layouts.LPWeb.Pipeline
{
    public partial class PopupPointImportAlert : LayoutsPageBase
    {
        string imgSrc = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var loanId = 0;
                var hisId = 0;
                if (Request.QueryString["fileId"] != null)
                    int.TryParse(Request.QueryString["fileId"], out loanId);

                if (Request.QueryString["hisId"] != null)
                    int.TryParse(Request.QueryString["hisId"], out hisId);

                if (Request.QueryString["icon"] != null)
                    imgSrc = Request.QueryString["icon"].ToString();

                if (loanId<1 && hisId<1)
                {
                    PageCommon.AlertMsg(this, "Invalid parameter");
                    return;
                }

                BindPage(loanId,hisId);
                //imgIcon.Src = "../images/loan/" + imgSrc;
            }
        }

        private void BindPage(int fileId, int hisId)
        {
            BLL.Loans bllLoans = new BLL.Loans();
            Model.Loans modelLoan = new Model.Loans();
            BLL.Contacts bllContact = new BLL.Contacts();
            BLL.Users bllUser = new BLL.Users();
            BLL.PointImportHistory bllPointImportHistory = new PointImportHistory();
            BLL.PointFiles bllPointFiles = new PointFiles();
            BLL.PointFolders bllPointFolders = new PointFolders();

            var dsList = new DataSet();
            if (fileId > 0)
            {
                dsList = bllPointImportHistory.GetList(string.Format("FileId={0}", fileId));
            }
            else if (hisId > 0)
            {
                dsList = bllPointImportHistory.GetList(string.Format("HistoryId={0}", hisId));
            }

            if (dsList == null || dsList.Tables.Count == 0 || dsList.Tables[0].Rows.Count == 0)
            {
                PageCommon.AlertMsg(this, "There is no data in database.");
                return;
            }

            fileId = int.Parse(dsList.Tables[0].Rows[0]["FileId"].ToString());
            hfdHisId.Value = fileId.ToString();
            var modelPointFiles = bllPointFiles.GetModel(fileId);
            if (modelPointFiles != null)
            {
                var modelPointFolder = bllPointFolders.GetModel(modelPointFiles.FolderId);
                if (modelPointFolder != null)
                {
                    lblPointFile.Text = modelPointFolder.Name + modelPointFiles.Name;
                }
            }

            lblBorrower.Text = bllContact.GetBorrower(fileId);
            lblLoanOfficer.Text = bllUser.GetLoanOfficer(fileId);




            // Start: get icon name by fileId, 2010-11-15
            if (string.IsNullOrEmpty(imgSrc))
            {
                string strSeverity = dsList.Tables[0].Rows[0]["Severity"].ToString().ToLower();
                switch (strSeverity)
                {
                    case "error":
                        imgIcon.Src = "../images/loan/AlertError.png";
                        break;
                    case "warning":
                        imgIcon.Src = "../images/loan/AlertWarning.png";
                        break;
                    default:
                        imgIcon.Visible = false;
                        break;
                }
            }
            else
                imgIcon.Src = "../images/loan/" + imgSrc;
            // End: get icon name by fileId, 2010-11-15

            DateTime dt = DateTime.MinValue;
            DateTime.TryParse(dsList.Tables[0].Rows[0]["ImportTime"].ToString(), out dt);

            if (dt != DateTime.MinValue)
                lblTime.Text = dt.ToString("MM/dd/yyyy hh:mm:ss");

            if (!string.IsNullOrEmpty(dsList.Tables[0].Rows[0]["Error"].ToString()))
            {
                string s1 = dsList.Tables[0].Rows[0]["Error"].ToString().Trim();
                s1 = s1.Replace("<br/>  ", "\r\n");
                s1 = s1.Replace("<br/> ", "\r\n");
                s1 = s1.Replace("<br/>", "\r\n");
                tbxErrorMessages.Text = s1;
            }
        }
    }
}
