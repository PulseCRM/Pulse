using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LPWeb.BLL;
using System.Data;
using LPWeb.LP_Service;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Layouts.LPWeb
{
    public class LoanTaskCommon
    {
        /// <summary>
        /// 根据LoanStage.Completed更新point file stage
        /// neo 2011-01-17
        /// </summary>
        /// <param name="iLoanID"></param>
        /// <param name="iLoginUserID"></param>
        /// <param name="iLoanStageID"></param>
        /// <returns></returns>
        public static string UpdatePointFileStage(int iLoanID, int iLoginUserID, int iLoanStageID)
        {
            #region get loan stage info

            Loans LoanManager1 = new Loans();
            DataTable LoanStageInfo = LoanManager1.GetLoanStage(" and LoanStageId=" + iLoanStageID);
            string sStageName = LoanStageInfo.Rows[0]["StageName"].ToString();
            string sCompletedDate = LoanStageInfo.Rows[0]["Completed"].ToString();

            #region 获取Template_Wfl_Stages.Name

            string sLoanStage = sStageName;
            if (LoanStageInfo.Rows[0]["WflStageId"] != DBNull.Value)
            {
                string sWflStageId = LoanStageInfo.Rows[0]["WflStageId"].ToString();
                Template_Wfl_Stages Template_Wfl_Stages1 = new Template_Wfl_Stages();
                DataSet Template_Wfl_Stages_Info = Template_Wfl_Stages1.GetList(" WflStageId=" + sWflStageId);
                if (Template_Wfl_Stages_Info.Tables[0].Rows.Count > 0)
                {
                    string sName = Template_Wfl_Stages_Info.Tables[0].Rows[0]["Name"].ToString();
                    if (sName != string.Empty)
                    {
                        sLoanStage = sName;
                    }
                }
            }

            #endregion

            #endregion

            #region invoke PointManager.UpdateStage()

            string sError = string.Empty;
            bool bCompleted = false;
            if (!string.IsNullOrEmpty(sCompletedDate))
            {
                bCompleted = true;
                sCompletedDate = DateTime.Now.ToShortDateString();
            }

            sError = UpdatePointFileStage(bCompleted, sCompletedDate, iLoanID, iLoginUserID, sLoanStage, iLoanStageID);
            #endregion

            return sError;
        }

        /// <summary>
        /// update stage of point file
        /// neo 2011-01-17
        /// </summary>
        /// <param name="bComplete">true or false</param>
        /// <param name="sCompletedDate">if bComplete=true, sCompletedDate=DateTime.ToString() else string.Empty</param>
        /// <param name="iLoanID"></param>
        /// <param name="iLoginUserID"></param>
        /// <param name="sLoanStage"></param>
        /// <returns>error, if success, error=string.Empty</returns>
        private static string UpdatePointFileStage(bool bComplete, string sCompletedDate, int iLoanID, int iLoginUserID, string sLoanStage, int iLoanStageId)
        {
            string sError = string.Empty;
            try
            {
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient service = sm.StartServiceClient())
                {
                    UpdateStageRequest req = new UpdateStageRequest();
                    req.hdr = new ReqHdr();
                    req.FileId = iLoanID;
                    req.hdr.UserId = iLoginUserID;
                    StageInfo StageInfo1 = new StageInfo();
                    StageInfo1.StageName = sLoanStage;
                    StageInfo1.LoanStageId = iLoanStageId;
                    DateTime completionDate = DateTime.MinValue;
                    if (bComplete == true)
                    {
                        DateTime.TryParse(sCompletedDate, out completionDate);
                    }
                    StageInfo1.Completed = completionDate;

                    req.StageList = new StageInfo[1] { StageInfo1 };
                    UpdateStageResponse resp = service.UpdateStage(req);
                    bool bIsSuccess = resp.hdr.Successful;
                    if (bIsSuccess == false)
                    {
                        sError = resp.hdr.StatusInfo;
                    }
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ee)
            {
                sError = " failed to update stage date in Point, reason: Point Manager is not running.";
            }
            catch (Exception ex)
            {
                sError = " failed to update stage date in Point, reason:" + ex.Message;
            }

            return sError;
        }
        /// <summary>
        /// update Point file and move it to the closed loan/archived loan folder
        /// 
        /// </summary>
        /// <param name="bComplete">true or false</param>
        /// <param name="sCompletedDate">if bComplete=true, sCompletedDate=DateTime.ToString() else string.Empty</param>
        /// <param name="iLoanID"></param>
        /// <param name="iLoginUserID"></param>
        /// <param name="sLoanStage"></param>
        /// <returns>error, if success, error=string.Empty</returns>
        public static string CloseLoan(int iLoanID, int iLoginUserID, int iFolderId)
        {
            string sError = string.Empty;
            try
            {
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient client = sm.StartServiceClient())
                {
                    DisposeLoanRequest req = new DisposeLoanRequest();
                    req.FileId = iLoanID;
                    req.LoanStatus = "Closed";
                    req.NewFolderId = iFolderId;
                    req.hdr = new ReqHdr();
                    req.hdr.UserId = iLoginUserID;
                    req.StatusDate = DateTime.Now;

                    DisposeLoanResponse response = client.DisposeLoan(req);
                    if (response.hdr.Successful)
                    {
                        return "Moved the Point file to the selected folder successfully.";

                    }
                    else
                    {
                        return "Failed to move the Point file to the selected folder, reason:" + response.hdr.StatusInfo;
                    }
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ee)
            {
                return "Failed to move the Point file, reason: Point Manager is not running.";
            }
            catch (Exception ex)
            {
                return "Failed to move the Point file, reason:" + ex.Message;

            }

        }
    }
}
