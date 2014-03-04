using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.BLL;
using Utilities;
using System.Linq;


public partial class PointInfoTab_BG : LayoutsPageBase
{
    public int iFileID = 0;
    LoginUser loginUser = new LoginUser();

    /// <summary>
    /// FileName and ADD PROSPECT or  BORROWER folder
    /// </summary>
    public string FileName
    {
        get
        {
            var filename = Request.QueryString["filename"] != null ? Request.QueryString["filename"].ToString() : "";

            if (filename.StartsWith("\\PROSPECT\\") || filename.StartsWith("\\BORROWER\\"))
            {
                return filename;
            }

            if (filename.ToUpper().EndsWith(".PRS"))
            {
                filename = "\\PROSPECT\\" + filename;
            }
            else if (filename.ToUpper().EndsWith(".BRW"))
            {
                filename = "\\BORROWER\\" + filename;
            }
            else
            {
                return filename;
            }
            return filename;
        }
    }
    public int BranchId
    {
        get
        {
            try
            {
                if (Request.QueryString["BranchId"] != null)
                {
                    return Convert.ToInt32(Request.QueryString["BranchId"]);
                }
                return 0;
            }
            catch { return 0; }
        }


    }
    public int LoanOfficerId
    {
        get
        {
            try
            {
                if (Request.QueryString["LoanOfficerId"] != null)
                {
                    return Convert.ToInt32(Request.QueryString["LoanOfficerId"]);
                }
                return 0;
            }
            catch { return 0; }
        }


    }
    public int FolderId
    {
        get
        {
            try
            {
                if (Request.QueryString["FolderId"] != null)
                {
                    return Convert.ToInt32(Request.QueryString["FolderId"]);
                }
                return 0;
            }
            catch { return 0; }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        iFileID = Request.QueryString["LoanID"] == null ? 0 : Convert.ToInt32(Request.QueryString["LoanID"]);
        var op = Request.QueryString["op"] == null ? "" : Request.QueryString["op"].ToString();

        switch (op.ToLower())
        {
            case "createpointfile":
                CreatePointFile();
                return;

            case "updatepoint":
                UpdatePoint();
                return;
            case "syncnow":
                SyncNow();
                return;

            default: break;

        }

        Write(false, "Parameter is missing.");
    }

    public void CreatePointFile()
    {
        if (!BasicChecks())
        {
            return;
        }

        if (!string.IsNullOrEmpty(FileName) && FileName.Length > 0)
        {
            var pointFileinfo = GetPointFileInfo();

            if (pointFileinfo != null && pointFileinfo.FileExists)
            {
                Write(false, "The Point file already exists.");
                return;
            }
        }

        #region checker file exists 
        //PointFiles bllPointFile = new PointFiles();

        //var ds = bllPointFile.GetList(" FolderId =" + FolderId + " and Name ='" + FileName + "'");

        //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{
        //    Write(false, "The New Point file already exists.");
        //    return;
        //}

        #endregion

        #region Save Local

        string errMsg = "";
        if (!SaveLocal(ref errMsg))
        {
            Write(false, errMsg);
            return;
        }

        #endregion

        #region Save wcf
        string errmsg = "";
        if (UpdateLoanInfo(ref errmsg))
        {
            Write(true, "");
            //ImportLoans(ref errmsg); //Sync one times
            return;
        }
        else
        {
            Write(false, errmsg);
            return;
        }

        #endregion

    }


    public void UpdatePoint()
    {
        if (!BasicChecks())
        {
            return;
        }

        var pointFileinfo = GetPointFileInfo();

        if (pointFileinfo == null || !pointFileinfo.FileExists)
        {
            Write(false, "The Point file doesn’t exist.");
            return;
        }

        #region Save Local

        string errMsg = "";
        if (!SaveLocal(ref errMsg))
        {
            Write(false, errMsg);
            return;
        }

        #endregion

        #region Save wcf
        string errmsg = "";
        if (UpdateLoanInfo(ref errmsg))
        {
            Write(true, "");
            return;
        }
        else
        {
            Write(false, errmsg);
            return;
        }

        #endregion
    }

    public void SyncNow()
    {
        if (!BasicChecks())
        {
            return;
        }

        var pointFileinfo = GetPointFileInfo();

        if (pointFileinfo == null || !pointFileinfo.FileExists)
        {
            Write(false, "The Point file doesn’t exist.");
            return;
        }

        #region Save Local

        string errMsg = "";
        if (!SaveLocal(ref errMsg))
        {
            Write(false, errMsg);
            return;
        }

        #endregion

        #region Save wcf
        string errmsg = "";
        if (ImportLoans(ref errmsg))
        {
            Write(true, "");
            return;
        }
        else
        {
            Write(false, errmsg);
            return;
        }

        #endregion
    }



    private GetPointFileInfoResp GetPointFileInfo()
    {
        #region GetPointFileInfo

        GetPointFileInfoReq req = new GetPointFileInfoReq();
        GetPointFileInfoResp resp = new GetPointFileInfoResp();

        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                

                req.FileId = iFileID;
                req.hdr = new ReqHdr() { UserId = loginUser.iUserID };

                resp = service.GetPointFileInfo(req);

                return resp;
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            string sExMsg = string.Format("Failed to get Point file info. Reason: Point Manager is not running.");
            LPLog.LogMessage(LogType.Logerror, sExMsg);

            return null;
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("error: {0}", ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            return null;
        }

        #endregion
    }

    private bool UpdateLoanInfo(ref string errMsg)
    {
        UpdateLoanInfoRequest req = new UpdateLoanInfoRequest();
        UpdateLoanInfoResponse resp = new UpdateLoanInfoResponse();

        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {


                req.FileId = iFileID;
                req.hdr = new ReqHdr() { UserId = loginUser.iUserID };
                req.CreateFile = true;
                
                resp = service.UpdateLoanInfo(req);

                if (resp == null || resp.hdr == null || !resp.hdr.Successful)
                {
                    errMsg = resp.hdr.StatusInfo;
                    return false;
                }

                //sleep 2s  for wait wcf 
                System.Threading.Thread.Sleep(2000);

                return true;
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            string sExMsg = string.Format("Failed to update Point file. Reason: Point Manager is not running.");
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            errMsg = sExMsg;
            return false;
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("error: {0}", ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            errMsg = sExMsg;
            return false;
        }

    }

    private bool ImportLoans(ref string errMsg)
    {
        ImportLoansRequest req = new ImportLoansRequest();
        ImportLoansResponse resp = new ImportLoansResponse();

        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {


                req.FileIds = new int[1] { iFileID };
                req.hdr = new ReqHdr() { UserId = loginUser.iUserID };

                resp = service.ImportLoans(req);

                if (resp == null || resp.hdr == null || !resp.hdr.Successful)
                {
                    errMsg = resp.hdr.StatusInfo;
                    return false;
                }
                return true;
            }
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            string sExMsg = string.Format("Failed to sync data from Point. Reason: Point Manager is not running.");
            LPLog.LogMessage(LogType.Logerror, sExMsg);
            errMsg = sExMsg;
            return false;
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("error: {0}", ex.Message);
            LPLog.LogMessage(LogType.Logerror, sExMsg);

            errMsg = sExMsg;
            return false;
        }
    }

    private bool SaveLocal(ref string errMsg)
    {
        if (!string.IsNullOrEmpty(FileName)  && FileName.Length > 0 && !FileName.ToUpper().EndsWith(".PRS") && !FileName.ToUpper().EndsWith(".BRW"))
        {
            errMsg = "The filename must end with “.PRS” or “.BRW”. ";
            return false;
        }

        try
        {
            #region Save BranchId
            Loans bllLoans = new Loans();
            LPWeb.Model.Loans loanInfo = new LPWeb.Model.Loans();
            loanInfo = bllLoans.GetModel(iFileID);
            if (loanInfo != null)
            {
                loanInfo.BranchID = BranchId;
                bllLoans.Update(loanInfo);
            }
            #endregion

            //Save Loan Officer

            #region Loan Officer

            LoanTeam bllLoanTeam = new LoanTeam();
            Users bllUsers = new Users();
            var loanOfficer = bllLoanTeam.GetLoanOfficer(iFileID);
            var loanOfficerId = bllLoanTeam.GetLoanOfficerID(iFileID);

            #region Loan Officer RolesID   =loanOfficerRolesId
            Roles bllRoles = new Roles();
            int loanOfficerRolesId = 3;//default;
            try
            {
                loanOfficerRolesId = bllRoles.GetModelList(" Name = 'Loan Officer' ").FirstOrDefault().RoleId;
            }
            catch { }
            #endregion

            if (LoanOfficerId != loanOfficerId)
            {
                var loanTeamInfo = bllLoanTeam.GetModel(iFileID, loanOfficerRolesId, loanOfficerId);

                if (loanTeamInfo == null)
                {
                    loanTeamInfo = new LPWeb.Model.LoanTeam();
                }
                else
                {
                    bllLoanTeam.Delete(iFileID, loanOfficerRolesId, loanOfficerId);
                }
                loanTeamInfo.FileId = iFileID;
                loanTeamInfo.RoleId = loanOfficerRolesId;
                loanTeamInfo.UserId = LoanOfficerId;


                bllLoanTeam.Add(loanTeamInfo);
            }

            #endregion

            #region Local PointFile  ----pointFileInfo and  pointFolderInfo

            PointFiles bllPointFile = new PointFiles();
            PointFolders bllPointFolders = new PointFolders();


            LPWeb.Model.PointFiles pointFileInfo = bllPointFile.GetModel(iFileID);
            var IsAddPointFile = false;
            if (pointFileInfo == null)
            {
                IsAddPointFile = true;
                pointFileInfo = new LPWeb.Model.PointFiles();
            }
            else
            {
                IsAddPointFile = false;
            }

            pointFileInfo.FileId = iFileID;
            if (FolderId > 0)
                pointFileInfo.FolderId = FolderId;
            if (FileName.Length > 0)
                pointFileInfo.Name = FileName;


            if (IsAddPointFile)
            {
                bllPointFile.Add(pointFileInfo);
            }
            else
            {
                bllPointFile.UpdateBase(pointFileInfo);
            }

            #endregion

        }
        catch (Exception ex)
        {
            errMsg = ex.Message;
            return false;
        }
        return true;
    }


    private bool BasicChecks()
    {
        if (iFileID <= 0)
        {
            Write(false, "Parameter is missing.");
            return false;
        }

        if (BranchId == 0 || LoanOfficerId == 0 || FolderId == 0)
        {
            Write(false, "Parameter is missing.(BranchId/LoanOfficerId/FolderId)");
            return false;
        }

        if (!string.IsNullOrEmpty(FileName) && FileName.Length > 0 && !FileName.ToUpper().EndsWith(".PRS") && !FileName.ToUpper().EndsWith(".BRW"))
        {
            Write(false, "The filename must end with “.PRS” or “.BRW”. ");
            return false;
        }
        return true;
    }



    string OutJsonTmp = "{\"ExecResult\":\"{0}\",\"ErrorMsg\":\"{1}\"}";
    private string GetOutJson(string State, string msg)
    {
        return OutJsonTmp.Replace("{0}", State).Replace("{1}", msg);

    }

    private void Write(bool isSuccess, string msg)
    {
        msg = msg.Replace("\"", "").Replace("'", "").Replace("\\", "");
        if (isSuccess)
        {
            Response.Write(GetOutJson("Success", msg));
            return;
        }
        else
        {
            Response.Write(GetOutJson("Failed", msg));
            return;
        }
    }

}
