using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using System.Data;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;

public partial class LoanConditionMarkAsReceivedAjax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"Failed to do sth."}

        #region 校验页面参数


        string sError_Lost = "Lost required query string.";
        string sError_Invalid = "Invalid query string.";

        if (this.Request.QueryString["FileId"] == null)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sError_Lost + "\"}");
            this.Response.End();
        }
        string sFileId = this.Request.QueryString["FileId"];

        if (PageCommon.IsID(sFileId) == false)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sError_Invalid + "\"}");
            this.Response.End();
        }

        if (this.Request.QueryString["ConditionId"] == null)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sError_Lost + "\"}");
            this.Response.End();
        }
        #endregion

        int iFileId = Convert.ToInt32(sFileId);
        string sConditionIds = this.Request.QueryString["ConditionId"];
        string[] sConditionIDArr = sConditionIds.Split(',');
        string sErrorMsg = string.Empty;
        LPWeb.BLL.LoanConditions lcon = new LPWeb.BLL.LoanConditions();
        LPWeb.BLL.LoanActivities lActive = new LPWeb.BLL.LoanActivities();

        #region Update Condition(Mark as Received)

        UpdateConditionsRequest req = new UpdateConditionsRequest();
        UpdateConditionsResponse resp = new UpdateConditionsResponse();
        try
        {
            Conditions[] myConditions = new Conditions[sConditionIDArr.Length];
            if (sConditionIDArr.Length < 1)
            {
                this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
                return;
            }
            int iConditionIdx = 0;
            foreach (string sConditionID in sConditionIDArr)
            {
                int iConditionId = 0;
                string sConditionName = "";
                if (Int32.TryParse(sConditionID, out iConditionId) == false)
                {
                    continue;
                }
                //Get Condition Status
                DataTable dtCondition = lcon.GetLoanConditionsInfo(iConditionId);
                if (dtCondition != null && dtCondition.Rows.Count > 0)
                {
                    string sStatus = dtCondition.Rows[0]["Status"].ToString().Trim();
                    string Received_Date = dtCondition.Rows[0]["Received"].ToString().Trim();
                    sConditionName = dtCondition.Rows[0]["CondName"].ToString().Trim();
                    if (sStatus == "Cleared" || sStatus == "Received")
                    {
                        if ((Received_Date != null) &&
                             (Received_Date != string.Empty))
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    continue;
                }

                Conditions condition = new Conditions();

                condition.ConditionId = iConditionId;
                condition.Name = sConditionName;
                condition.Status = "Received";
                myConditions.SetValue(condition, iConditionIdx);
                iConditionIdx++;
            }
            req.ConditionList = new Conditions[iConditionIdx];
            int i = 0;
            foreach (Conditions myCon in myConditions)
            {
                if (myCon == null)
                {
                    continue;
                }
                req.ConditionList.SetValue(myCon, i);
                i++;
            }
            if (req.ConditionList.Length < 1)
            {
                this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
                return;
            }


            #region Invoking WCF
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                req.FileId = iFileId;
                req.hdr = new ReqHdr();
                req.hdr.UserId = CurrUser.iUserID;
                resp = service.UpdateConditions(req);

                if (resp == null || resp.hdr == null)
                {
                    this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"UpdateConditions is NULL\"}");
                    return;
                }

                sErrorMsg = resp.hdr.StatusInfo;

                if (resp.hdr.Successful == true)
                {
                    foreach (Conditions conObj in req.ConditionList)
                    {
                        if (conObj == null)
                        {
                            continue;
                        }
                        //Update Loan Condition Status
                        lcon.UpdateConditionStatusToReceived(conObj.ConditionId, CurrUser.sFullName);
                        //Insert Loan Activities
                        LPWeb.Model.LoanActivities lActiveMode = new LPWeb.Model.LoanActivities();
                        lActiveMode.FileId = iFileId;
                        lActiveMode.ActivityName = "Marked Condition – " + conObj.Name + " as Received";
                        lActiveMode.ActivityTime = DateTime.Now;
                        lActiveMode.UserId = CurrUser.iUserID;
                        lActive.Add(lActiveMode);
                    }
                    this.Response.Write("{\"ExecResult\":\"Success\",\"ErrorMsg\":\"\"}");
                    return;
                }
                else
                {
                    sErrorMsg = "Update Condition Exception: " + sErrorMsg;
                    this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
                    return;
                }
            }
            #endregion Invoking WCF


        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            sErrorMsg = string.Format("Exception: Point Manager is not running.");
            LPLog.LogMessage(LogType.Logerror, sErrorMsg);

            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
            return;
        }
        catch (Exception ex)
        {
            sErrorMsg = string.Format("error: {0}", ex.Message);
            LPLog.LogMessage(LogType.Logerror, sErrorMsg);
            sErrorMsg = "Point Manager Exception: Database timeout expired";

            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
            return;
        }

        #endregion
    }
}

