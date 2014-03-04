using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using LPWeb.BLL;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;

public partial class Pipeline_PopupPointImportAlertDelete_Background : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 接收参数

        if (this.Request.QueryString["fileId"] == null)
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Missing required query string.\"}");
            this.Response.End();
        }

        int fileId = 0;
        if (!int.TryParse(this.Request.QueryString["fileId"], out fileId))
        {
            this.Response.Write("{\"ExecResult\":\"Failed\",\"ErrorMsg\":\"Query string is invalid.\"}");
            this.Response.End();
        }
        #endregion

        // json示例
        // {"ExecResult":"Success","ErrorMsg":""}
        // {"ExecResult":"Failed","ErrorMsg":"执行数据库脚本时发生错误。"}

        string sExecResult = string.Empty;
        string sErrorMsg = string.Empty;

        try
        {
            LPWeb.BLL.PointImportHistory bllHis = new PointImportHistory();
            var histories = bllHis.GetModelList("FileId=" + fileId);
            var hisIds = histories.Select(his => his.HistoryId).ToList();
            bllHis.DeleteImportErrors(hisIds);
            sExecResult = "Success";
            sErrorMsg = "";
        }
        catch (Exception ex)
        {
            sExecResult = "Failed";
            sErrorMsg = "Failed to delete selected record.";
        }

        System.Threading.Thread.Sleep(1000);

        this.Response.Write("{\"ExecResult\":\"" + sExecResult + "\",\"ErrorMsg\":\"" + sErrorMsg + "\"}");
        this.Response.End();
    }
}

