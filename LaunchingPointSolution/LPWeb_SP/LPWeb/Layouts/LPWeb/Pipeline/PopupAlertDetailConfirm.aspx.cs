using System;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.LP_Service;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.Pipeline
{
    public partial class PopupAlertDetailConfirm : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (OrgDate == DateTime.MinValue || FileId < 1)
                {
                    PageCommon.WriteJs(this, "Invalid parameters!", "parent.CloseTheUpdateExtendWindow();");
                    return;
                }
            }
        }

        protected int FileId
        {
            get
            {
                int fileId = 0;
                if (Request.QueryString["fileId"] != null)
                    int.TryParse(Request.QueryString["fileId"], out fileId);
                return fileId;
            }
        }

        protected DateTime OrgDate
        {
            get
            {
                DateTime dtMin = DateTime.MinValue;
                if (Request.QueryString["orgDate"] != null)
                {
                    double oaDate;
                    if(!double.TryParse(Request.QueryString["orgDate"], out oaDate))
                    {
                        return DateTime.MinValue;
                    }

                   return DateTime.FromOADate(oaDate);
                }
                return dtMin;
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (OrgDate == DateTime.MinValue || FileId < 1)
            {
                PageCommon.WriteJs(this, "Invalid parameters!", "parent.CloseTheUpdateExtendWindow();");
                return;
            }

            int extendDays = 0;
            int.TryParse(tbxExtendDays.Text.Trim(), out extendDays);
            if (extendDays < 1)
            {
                PageCommon.WriteJs(this, "Please input the number of days to extend the rate lock!", "parent.CloseTheUpdateExtendWindow();");
                return;
            }
            try
            {
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient client = sm.StartServiceClient())
                {
                    var req = new ExtendRateLockRequest
                                  {
                                      DaysExtended = extendDays,
                                      FileId = FileId,
                                      NewDate = OrgDate.AddDays(extendDays),
                                      hdr = new ReqHdr { UserId = new LoginUser().iUserID }
                                  };

                    var response = client.ExtendRateLock(req);
                    if (!response.hdr.Successful)
                    {
                        PageCommon.AlertMsg(this, "Unable to update Point at the moment, reason: " + response.hdr.StatusInfo);
                        //PageCommon.WriteJs(this, "Please input the number of days to extend the rate lock!", "parent.CloseTheUpdateExtendWindow();");
                        return;
                    }
                    else
                    {
                        UpdateTheRateExtendLock(extendDays);
                        return;
                    }
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ee)
            {                
                PageCommon.AlertMsg(this, "Failed reason: Point Manager is not running.");
            }

        }

        private void UpdateTheRateExtendLock(int extendDays)
        {
            BLL.Company_Alerts bllComAlert = new Company_Alerts();
            BLL.LoanAlerts bllAlert = new LoanAlerts();
            BLL.Loans bllLoans = new BLL.Loans();
            Model.Loans modelLoan = new Model.Loans();

            modelLoan = bllLoans.GetModel(FileId);
            modelLoan.RateLockExpiration = OrgDate.AddDays(extendDays);

            bllLoans.Update(modelLoan);

            var comModel = bllComAlert.GetModel();
            //todo:确认此处是否这样查询LoanAlerts表中对应当前Loan的记录
            //此处应该只能查处一条数据
            var alertModelList = bllAlert.GetModelList(string.Format("FileId={0} AND AlertType={1}", FileId, "'Rate Lock'"));
            if(alertModelList.Count==0)
                return;

            //if (extendDays < comModel.RateLockYellowDays || extendDays < comModel.RateLockRedDays)
            //{
            //    alertModelList[0].DueDate = OrgDate.AddDays(extendDays);
            //    bllAlert.Update(alertModelList[0]);
            //}
            //else if (extendDays > comModel.RateLockYellowDays || extendDays > comModel.RateLockRedDays)
            //{
            //    alertModelList[0].DueDate = OrgDate.AddDays(extendDays);
            //    bllAlert.Delete(alertModelList[0].LoanAlertId);
            //}

            PageCommon.WriteJs(this, "Update succeeded!", "window.parent.parent.location.href=window.parent.parent.location.href;parent.CloseCurrentWindowHandller();");
        }
    }
}

