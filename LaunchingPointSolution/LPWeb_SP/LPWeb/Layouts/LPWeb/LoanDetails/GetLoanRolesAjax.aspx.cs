using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Data;
using LPWeb.BLL;
using System.Text;

public partial class LoanDetails_GetLoanRolesAjax : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // json示例
        // [{"FolderID":"1","Name":"FolderA"},{"FolderID":"2","Name":"FolderB"}]

        #region 校验页面参数

        // RegionID
        string sRegionIDs = this.Request.QueryString["RegionIDs"].ToString();

        // DivisionID
        string sDivisionIDs = this.Request.QueryString["DivisionIDs"].ToString();
        
        // BranchIDs
        string sBranchIDs = this.Request.QueryString["BranchIDs"].ToString();

        // Role
        string sRole = this.Request.QueryString["Role"].ToString();
        
        #endregion

        string sWhere = string.Empty;
        if (sRegionIDs != "0")
        {
            sWhere += " and RegionID in (" + sRegionIDs + ")";
        }

        if (sDivisionIDs != "0")
        {
            sWhere += " and DivisionID in (" + sDivisionIDs + ")";
        }

        if (sBranchIDs != "0")
        {
            sWhere += " and BranchId in (" + sBranchIDs + ") ";
        }

        string sSql = string.Empty;
        DataTable NameList = null;

        LoanDetails_AdvancedLoanFilters Mgr = new LoanDetails_AdvancedLoanFilters();

        if (sRole == "LoanOfficer")
        {
            if (sWhere != string.Empty)
            {
                sSql = "select distinct LoanOfficerId as ID, [Loan Officer] as Name from V_ProcessingPipelineInfo where isnull(LoanOfficerId,'')<>'' " + sWhere + " order by [Loan Officer]";
                NameList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
            }
            else
            {
                NameList = Mgr.GetLoanOfficerList();
            }
        }
        else if (sRole == "LoanOfficerAssistant")
        {
            if (sWhere != string.Empty)
            {
                sSql = "select distinct Assistant as Name from V_ProcessingPipelineInfo where isnull(Assistant,'')<>'' " + sWhere + " order by Assistant";
                NameList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
            }
            else
            {
                NameList = Mgr.GetLoanRolesList("Assistant");
            }
        }
        else if (sRole == "Processor")
        {
            if (sWhere != string.Empty)
            {
                sSql = "select distinct  ProcessorId as ID, Processor as Name from V_ProcessingPipelineInfo where isnull(ProcessorId,'')<>'' " + sWhere + " order by Processor";
                NameList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
            }
            else
            {
                NameList = Mgr.GetProcessorList();
            }
        }
        else if (sRole == "JrProcessor")
        {
            if (sWhere != string.Empty)
            {
                sSql = "select distinct JrProcessor as Name from V_ProcessingPipelineInfo where isnull(JrProcessor,'')<>'' " + sWhere + " order by JrProcessor";
                NameList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
            }
            else
            {
                NameList = Mgr.GetLoanRolesList("JrProcessor");
            }
            
        }
        else if (sRole == "DocPre")
        {
            if (sWhere != string.Empty)
            {
                sSql = "select distinct DocPrep as Name from V_ProcessingPipelineInfo where isnull(DocPrep,'')<>'' " + sWhere + " order by DocPrep";
                NameList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
            }
            else
            {
                NameList = Mgr.GetLoanRolesList("DocPrep");
            }
            
        }
        else if (sRole == "Shipper")
        {
            if (sWhere != string.Empty)
            {
                sSql = "select distinct Shipper as Name from V_ProcessingPipelineInfo where isnull(Shipper,'')<>'' " + sWhere + " order by Shipper";
                NameList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
            }
            else
            {
                NameList = Mgr.GetLoanRolesList("Shipper");
            }
            
        }
        else if (sRole == "Closer")
        {
            if (sWhere != string.Empty)
            {
                sSql = "select distinct Closer as Name from V_ProcessingPipelineInfo where isnull(Closer,'')<>'' " + sWhere + " order by Closer";
                NameList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
            }
            else
            {
                NameList = Mgr.GetLoanRolesList("Closer");
            }
            
        }
        else if (sRole == "Underwriter")
        {
            if (sWhere != string.Empty)
            {
                sSql = "select UserId as ID, LastName + ', ' + FirstName as Name from Users where UserId in (select distinct UnderwriterId as Name from V_ProcessingPipelineInfo where UnderwriterId is not null " + sWhere + ") order by LastName + ', ' + FirstName";
                NameList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
            }
            else
            {
                NameList = Mgr.GetUnderwriterList();
            }
        }
        
        StringBuilder sbJson = new StringBuilder();

        int i = 0;
        if (sRole == "LoanOfficer" || sRole == "Processor" || sRole == "Underwriter")
        {
            foreach (DataRow NameRow in NameList.Rows)
            {
                string sID = NameRow["ID"].ToString();
                string sName = NameRow["Name"].ToString();

                if (i == 0)
                {
                    sbJson.Append("{\"ID\":\"" + sID + "\",\"Name\":\"" + sName + "\"}");
                }
                else
                {
                    sbJson.Append(",{\"ID\":\"" + sID + "\",\"Name\":\"" + sName + "\"}");
                }

                i++;
            }
        }
        else
        {
            foreach (DataRow NameRow in NameList.Rows)
            {
                string sName = NameRow["Name"].ToString();

                if (i == 0)
                {
                    sbJson.Append("{\"Name\":\"" + sName + "\"}");
                }
                else
                {
                    sbJson.Append(",{\"Name\":\"" + sName + "\"}");
                }

                i++;
            }
        }

        this.Response.Write("[" + sbJson.ToString() + "]");
    }
}
