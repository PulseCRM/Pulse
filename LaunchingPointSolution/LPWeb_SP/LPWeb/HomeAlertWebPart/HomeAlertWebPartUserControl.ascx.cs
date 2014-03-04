using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using LPWeb.Common;

namespace LPWeb.HomeAlertWebPart
{
    public partial class HomeAlertWebPartUserControl : UserControl
    {
        // 当前用户
        LoginUser CurrentUser;
        int iCurrentUserID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 获取当前用户信息
            this.CurrentUser = new LoginUser();
            this.iCurrentUserID = CurrentUser.iUserID;

            // 默认条件
            string sWhere_DueDate = " and datediff(day, getdate(), a.DueDate) < 0";

            #region 获取 UserHomePref 信息

            BLL.UserHomePref UserHomePrefManager = new BLL.UserHomePref();
            LPWeb.Model.UserHomePref userHomePref = UserHomePrefManager.GetModel(this.iCurrentUserID);
            
            #endregion

            if (userHomePref != null && userHomePref.AlertFilter == 7)      // Overdue + Today
            {
                sWhere_DueDate = " and datediff(day, getdate(), a.DueDate) <= 0";

                this.ddlDueDateFilter.SelectedIndex = 1;
            }

            #region DueDate Filter

            // Due
            if (this.Request.QueryString["DueDate"] != null)
            {
                string sDue = this.Request.QueryString["DueDate"].ToString();

                if (sDue == "In30") // Due in the next month
                {
                    sWhere_DueDate = " and datediff(day, getdate(), a.DueDate) <= 30 and datediff(day, getdate(), a.DueDate) >=0";
                }
                else if (sDue == "In14")  // Due in the next 2 weeks
                {
                    sWhere_DueDate = " and datediff(day, getdate(), a.DueDate) <= 14 and datediff(day, getdate(), a.DueDate) >=0";
                }
                else if (sDue == "In7") // Due in the next 7 days
                {
                    sWhere_DueDate = " and datediff(day, getdate(), a.DueDate) <= 7 and datediff(day, getdate(), a.DueDate) >=0";
                }
                else if (sDue == "In1") // Due tomorrow
                {
                    sWhere_DueDate = " and datediff(day, getdate(), a.DueDate) = 1";
                }
                else if (sDue == "In0") // Due today
                {
                    sWhere_DueDate = " and datediff(day, getdate(), a.DueDate) = 0";
                }
                else if (sDue == "Overdue")
                {
                    sWhere_DueDate = " and datediff(day, getdate(), a.DueDate) < 0";
                }
                else if (sDue == "OverToday")   // Overdue + Today
                {
                    sWhere_DueDate = " and datediff(day, getdate(), a.DueDate) <= 0";
                }
            }

            #endregion

            #region 加载Alerts

            LPWeb.BLL.LoanAlerts AlertManager = new LPWeb.BLL.LoanAlerts();
            DataTable AlertListData = new DataTable();
            AlertListData = AlertManager.Loan_GetSimpleAlertList(this.iCurrentUserID, sWhere_DueDate);
            AlertListData.Columns.Add("HRef", typeof(string));
            foreach (DataRow dr in AlertListData.Rows)
            {
                string Url = string.Empty;
                string CloseDialogCode = "CloseDialogCodes=sharepoint";
                if (dr["AlertType"] != DBNull.Value)
                {
                    string alertType = dr["AlertType"].ToString();
                    switch (alertType.ToLower())
                    {
                        case "rule alert":
                            Url = string.Format("LoanDetails/RuleAlertPopup.aspx?LoanID={0}&AlertID={1}&{2}", dr["FileId"].ToString(), dr["LoanAlertId"].ToString(), CloseDialogCode);
                            break;
                        case "task alert":
                            Url = string.Format("Pipeline/TaskAlertDetail.aspx?fileID={0}&LoanTaskId={1}&{2}", dr["FileId"].ToString(), dr["LoanTaskId"].ToString(), CloseDialogCode);
                            break;
                    }
                }
                dr["HRef"] = Url;
            }
            this.rpAlertList.DataSource = AlertListData;
            this.rpAlertList.DataBind();
            #endregion
        }

        private string BuildWhere_Due()
        {
            string sWhere_Due = string.Empty;

            

            return sWhere_Due;
        }
    }
}
