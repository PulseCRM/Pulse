using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.LP_Service;
using System.Text;
using Utilities;
using System.Web;

public partial class LoanDetails_LSRPreview : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string sError_Lost = "Lost required query string.";
        //if (this.Request.QueryString["TextBody"] == null)
        //{
        //    this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"" + sError_Lost + "\"}");
        //    this.Response.End();
        //}
        //string sHtmlBody = this.Request.QueryString["TextBody"];
        //this.ltLSRBody.Text = sHtmlBody;

        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"Failed to do sth."}

        #region 校验页面参数

        string sErrorJs = "alert('Missing required query string.');window.close();";

        if (this.Request.QueryString["FileId"] == null)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing3", sErrorJs, true);
            return;
        }
        string sFileId = this.Request.QueryString["FileId"];

        if (PageCommon.IsID(sFileId) == false)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing1", sErrorJs, true);
            return;
        }
        int iFileId = Convert.ToInt32(sFileId);
        string sContactId = this.Request.QueryString["ContactId"];
        string sUserId = this.Request.QueryString["UserId"];

        if (PageCommon.IsID(sContactId) == false || PageCommon.IsID(sUserId) == false)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Missing1", sErrorJs, true);
            return;
        }
        int iContactId = 0;
        int iUserId = 0;
        string sLSRBody = "LSR content";

        #endregion

        #region Invoke WCF

        string sErrorMsg = string.Empty;

        PreviewLSRRequest req = new PreviewLSRRequest();
        PreviewLSRResponse resp = new PreviewLSRResponse();

        //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Waiting", "ShowWaitingDialog3(\"Preview report...\");", true);
        try
        {
            ServiceManager sm = new ServiceManager();
            using (LP2ServiceClient service = sm.StartServiceClient())
            {
                req.FileId = iFileId;
                req.hdr = new ReqHdr();
                req.ContactId = (Int32.TryParse(sContactId, out iContactId) == true ? iContactId : 0);
                req.UserId = (Int32.TryParse(sUserId, out iUserId) == true ? iUserId : 0);

                resp = service.PreviewLSR(req);

                if (resp == null || resp.hdr == null)
                {
                    PageCommon.WriteJsEnd(this, "", "CloseWaitingDialog3()");
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_FailedGetHtml", "alert('Failed to preview report!');window.close();", true);
                    return;
                }
                int iBranchID = 0;
                int first_check = 0;
                int icon_check = 1;

                string sSql = string.Format("select BranchID from Loans where FileId={0}", iFileId);
                iBranchID = (int)LPWeb.DAL.DbHelperSQL.GetSingle(sSql);                    

                sErrorMsg = resp.hdr.StatusInfo;

                if (resp.hdr.Successful == true)
                {
                    sLSRBody = Encoding.UTF8.GetString(resp.ReportContent);
                    String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                    String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

                    int first = 0;
                    string b_str = "";
                    string m_str = "";
                    int end = 0;
                    string e_str = "";
                    string left_str = "";

                    first_check = sLSRBody.IndexOf("width:330px; height:64px");
                    if (first_check > 0)
                    {
                        first = sLSRBody.IndexOf("cid:");
                        b_str = sLSRBody.Substring(0, first - 12);
                        m_str = "<img src=\"" + strUrl + "_layouts/LPWeb/Settings/GetPreviewBranchLogo.aspx?BranchID=" + iBranchID.ToString() + "&ss=" + DateTime.Now.Millisecond.ToString();
                        end = sLSRBody.IndexOf("width:330px; height:64px");
                        e_str = sLSRBody.Substring(end - 8, sLSRBody.Length - end + 8);
                        sLSRBody = b_str + m_str + e_str;
                    }

                    first_check = sLSRBody.IndexOf("width:96px; height:13px");
                    if (first_check > 0)
                    {
                        first = sLSRBody.IndexOf("cid:");
                        b_str = sLSRBody.Substring(0, first - 12);
                        end = sLSRBody.IndexOf("width:96px; height:13px");
                        e_str = sLSRBody.Substring(end - 8, sLSRBody.Length - end + 8);
                        string p_str_buf = sLSRBody.Substring(first, sLSRBody.Length - first);
                        int p_first = p_str_buf.IndexOf("data=");
                        p_str_buf = p_str_buf.Substring(p_first + 6, p_str_buf.Length - p_first - 6);
                        int p_end = p_str_buf.IndexOf("\"");
                        string p_str = p_str_buf.Substring(0, p_end);
                        int len8 = p_str.Length;
                        m_str = "<img src=\"" + strUrl + string.Format("_layouts/LPWeb/Settings/GetLoanProgressPicture.aspx?Image={0}", p_str);
                        sLSRBody = b_str + m_str + e_str;
                    }

                    first_check = sLSRBody.IndexOf("height:77px");
                    if (first_check > 0)
                    {
                        first = sLSRBody.IndexOf("cid:");
                        b_str = sLSRBody.Substring(0, first - 12);
                        m_str = "<img src=\"" + strUrl + string.Format("_layouts/LPWeb/Settings/GetPreviewUserPicture.aspx?uid={0}&t={1}", this.CurrUser.iUserID.ToString(), DateTime.Now.Ticks);
                        end = sLSRBody.IndexOf("height:77px");
                        e_str = sLSRBody.Substring(end - 20, sLSRBody.Length - end + 20);
                        sLSRBody = b_str + m_str + e_str;
                    }

                    icon_check = sLSRBody.IndexOf("width:16px; height:16px");

                    while (icon_check > 0)
                    { 
                            first = sLSRBody.IndexOf("cid:");
                            if (first > 0)
                            {                              
                                b_str = sLSRBody.Substring(0, first - 10);
                                left_str = sLSRBody.Substring(first - 9, sLSRBody.Length - (first - 9));
                                end = left_str.IndexOf("width:16px; height:16px");
                                if (end > 0)
                                {
                                    e_str = left_str.Substring(end - 8, left_str.Length - end + 8);
                                    string p_str_buf = sLSRBody.Substring(first, sLSRBody.Length - first);
                                    int p_first = p_str_buf.IndexOf("data=");
                                    p_str_buf = p_str_buf.Substring(p_first + 6, p_str_buf.Length - p_first - 6);
                                    int p_end = p_str_buf.IndexOf("\"");
                                    string p_str = p_str_buf.Substring(0, p_end);
                                    int len8 = p_str.Length;
                                    m_str = "<img src=\"" + strUrl + string.Format("_layouts/LPWeb/Settings/GetLoanProgressPicture.aspx?Image={0}", p_str);
                                    sLSRBody = b_str + m_str + e_str;
                                }
                                else
                                {
                                    icon_check = -1;
                                }
                            }
                            else
                            {
                                icon_check = -1;
                            }
                    }

                }
                else
                {
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_FailedGetHtml", "alert('Failed to preview report: " + resp.hdr.StatusInfo + "');window.close();", true);
                    return;
                }

            }
        }
        catch (System.ServiceModel.EndpointNotFoundException ee)
        {
            PageCommon.AlertMsg(this, "Failed to preview report,  Report Manager is not running, error: " + ee.ToString());
            return;
        }
        catch (Exception ex)
        {
            //PageCommon.AlertMsg(this, "Failed to preview report,  Report Manager is not running, error: " + ex.ToString());
            return;
        }
        finally
        {
            this.ltLSRBody.Text = sLSRBody;
        }

        //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "_Waiting", "CloseWaitingDialog3();",true);
        #endregion
    }
}
