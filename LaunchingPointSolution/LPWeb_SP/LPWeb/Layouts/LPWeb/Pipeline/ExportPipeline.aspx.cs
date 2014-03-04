using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using System.Text;
using LPWeb.LP_Service;
using System.Data;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb;
using System.Linq;


namespace LPWeb.Layouts.LPWeb.Pipeline
{
    public partial class ExportPipeline : BasePage
    {
        private readonly Loans _bllLoans = new Loans();
        private string queryCondition = "";
        private bool isAll = false;
        private int recordTotal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            var IDs = "";

            if (string.IsNullOrEmpty(Request.QueryString["IDs"]))
            {
                Response.Write("<script>alert('Error IDs');</script>");
                Response.End();
                return ;
            }

            IDs = Request.QueryString["IDs"].ToString();

            IDs = IDs.EndsWith(",") ? IDs.Remove(IDs.Length-1, 1) : IDs;

            try
            {
                isAll = (string.IsNullOrEmpty(Request.QueryString["IsAll"]) || Request.QueryString["IsAll"] == "0") ? false : true;

                queryCondition = string.IsNullOrEmpty(Request.QueryString["queryCondition"]) ? "" : Encrypter.Base64Decode(Request.QueryString["queryCondition"].ToString().Replace("_99_", "+"));

                recordTotal = string.IsNullOrEmpty(Request.QueryString["recordTotal"]) ? 0 : Convert.ToInt32(Request.QueryString["recordTotal"]);
            }
            catch {
                Response.Write("<script>alert('Error');</script>");
                Response.End();
                return;
            }

            if (Request.QueryString["action"].ToLower() == "loans")
            {
                //Response.Write("<script>alert('" + IDs + "');</script>");
                ExportLoans(IDs);
            }
            else if(Request.QueryString["action"].ToLower() == "leads")
            {
                ExportLeads(IDs);
            }
            else if (Request.QueryString["action"].ToLower() == "clients")
            {
                ExportClients(IDs);
            }
            
        }

        private void ExportLoans(string IDs)
        {
            try
            {

                DataTable dt = new DataTable();
                int recordCount = 0;

                string strWhere = isAll == true ? queryCondition : " And FileId in(" + IDs + ")";

                DataSet Lists = _bllLoans.GetLoanPipelineList(this.CurrUser.iUserID, recordTotal, 1, strWhere, out recordCount, "FileId", 0);
                if (recordCount > 0)
                {
                    dt = Lists.Tables[0];
                }

                //List<string> ShowColumnsList = ShowColumns();
                List<string> NoShowColumnsList = new List<string>();

                //foreach (DataColumn item in dt.Columns)
                //{
                //    if (ShowColumnsList.Contains(item.ColumnName))
                //    {
                //        item.Caption = item.ColumnName;
                //    }
                //    else
                //    {
                //        NoShowColumnsList.Add(item.ColumnName);
                //    }

                //}

                NoShowColumnsList.Add("RateLockicon");
                NoShowColumnsList.Add("ImportErrorIcon");
                NoShowColumnsList.Add("AlertIcon");
                NoShowColumnsList.Add("RuleAlertIcon");
                NoShowColumnsList.Add("FileId");
                NoShowColumnsList.Add("BranchId");
                NoShowColumnsList.Add("ContactId");

                foreach (string item in NoShowColumnsList)
                {
                    dt.Columns.Remove(dt.Columns[item]);
                }



                Common.XlsExporter.DownloadXls(this.Page, dt, "Loans Pipeline Report.xls", "Loans Pipeline Report");

            }
            catch (Exception ex)
            {

            }
        }

        private List<string> ShowColumns()
        {
            BLL.UserPipelineColumns upcBll = new UserPipelineColumns();

            Model.UserPipelineColumns model = upcBll.GetModel(this.CurrUser.iUserID);

            List<string> ShowColumnsList = new List<string>();

            #region ShowColumns
            ShowColumnsList.Add("Borrower");

            if (model.Alerts)
            {
                ShowColumnsList.Add("Alerts"); //显示什么内容 数据库中仅有图标
            }

            if (model.Amount)
            {
                ShowColumnsList.Add("Amount");
            }

            if (model.Assistant)
            {
                ShowColumnsList.Add("Assistant");
            }
            if (model.Branch)
            {
                ShowColumnsList.Add("Branch");
            }
            if (model.Closer)
            {
                ShowColumnsList.Add("Closer");
            }
            if (model.DocPrep)
            {
                ShowColumnsList.Add("DocPrep");
            }
            if (model.EstimatedClose)
            {
                ShowColumnsList.Add("EstClose");
            }
            if (model.LastCompletedStage)
            {
                ShowColumnsList.Add("LastCompletedStage");
            }
            if (model.LastStageComplDate)
            {
                ShowColumnsList.Add("LastStageComplDate");
            }
            if (model.Lender)
            {
                ShowColumnsList.Add("Lender");
            }
            if (model.Lien)
            {
                ShowColumnsList.Add("Lien");
            }
            if (model.LoanOfficer)
            {
                ShowColumnsList.Add("Loan Officer");
            }
            if (model.LockExp)
            {
                ShowColumnsList.Add("Lock Expiration Date");
            }
            if (model.PercentCompl)
            {
                ShowColumnsList.Add("Progress");
            }
            if (model.PointFileName)
            {
                ShowColumnsList.Add("Filename");
            }
            if (model.PointFolder)
            {
                ShowColumnsList.Add("Point Folder");
            }
            if (model.Processor)
            {
                ShowColumnsList.Add("Processor");
            }
            if (model.Rate)
            {
                ShowColumnsList.Add("Rate");
            }
            if (model.Shipper)
            {
                ShowColumnsList.Add("Shipper");
            }
            if (model.Stage)
            {
                ShowColumnsList.Add("Stage");
            }
            if (model.TaskCount)
            {
                ShowColumnsList.Add("Task Count");
            }

            #endregion

            return ShowColumnsList;
        }



        private void ExportLeads(string IDs)
        {
            try
            {
                BLL.Loans LoansManager = new BLL.Loans();
                DataTable dt = new DataTable();

                int recordCount = 0;

                string strWhere = isAll == true ? queryCondition : " FileId in(" + IDs + ")";

                DataSet Lists = LoansManager.Lead_FirstPage_GetProspectListNew(recordTotal, 1, strWhere, out recordCount, "FileId", 0);
                if (recordCount > 0)
                {
                    dt = Lists.Tables[0];
                }

                List<string> ShowColumnsList = ShowColumns();
                List<string> NoShowColumnsList = new List<string>();

                foreach (DataColumn item in dt.Columns)
                {
                    if (ShowColumnsList.Contains(item.ColumnName))
                    {
                        item.Caption = item.ColumnName;
                    }
                    else
                    {
                        NoShowColumnsList.Add(item.ColumnName);
                    }

                }

                foreach (string item in NoShowColumnsList)
                {
                    dt.Columns.Remove(dt.Columns[item]);
                }



                Common.XlsExporter.DownloadXls(this.Page, dt, "Leads Pipeline Report.xls", "Leads Pipeline Report");

            }
            catch (Exception ex)
            {

            }
        }

        private List<string> ShowColumnsForLead()
        {

            BLL.UserProspectColumns upcBll = new UserProspectColumns();

            Model.UserProspectColumns model = upcBll.GetModel(this.CurrUser.iUserID);

            List<string> ShowColumnsList = new List<string>();

            #region ShowColumns

            ShowColumnsList.Add("Borrower");

            if (model.LastCompletedStage)
            {
                ShowColumnsList.Add("LastCompletedStage");
            }
            if (model.LastStageComplDate)
            {
                ShowColumnsList.Add("LastStageComplDate");
            }


            if (model.Lv_Amount)
            {
                ShowColumnsList.Add("Amount");
            }

            if (model.Lv_Branch)
            {
                ShowColumnsList.Add("Branch");
            }

            if (model.Lv_Estclose)
            {
                ShowColumnsList.Add("Estclose)");
            }
            if (model.Lv_Leadsource)
            {
                ShowColumnsList.Add("Leadsource");
            }
            if (model.Lv_Lien)
            {
                ShowColumnsList.Add("Lien");
            }
            if (model.Lv_Loanofficer)
            {
                ShowColumnsList.Add("Loan officer");
            }
            if (model.Lv_Loanprogram)
            {
                ShowColumnsList.Add("program");
            }
            
            if (model.Lv_Partner)
            {
                ShowColumnsList.Add("Partner");
            }
            if (model.Lv_Pointfilename)
            {
                ShowColumnsList.Add("filename");
            }
            if (model.Lv_Progress)
            {
                ShowColumnsList.Add("Progress");
            }
            if (model.Lv_Ranking)
            {
                ShowColumnsList.Add("Ranking");
            }
            if (model.Lv_Rate)
            {
                ShowColumnsList.Add("Rate");
            }
            if (model.Lv_Refcode)
            {
                ShowColumnsList.Add("Refcode");
            }
            if (model.Lv_Referral)
            {
                ShowColumnsList.Add("Referral");
            }
            if (model.Pv_Branch)
            {
                ShowColumnsList.Add("Branch");
            }
            if (model.Pv_Created)
            {
                ShowColumnsList.Add("Created");
            }
            if (model.Pv_Leadsource)
            {
                ShowColumnsList.Add("Leadsource");
            }
            if (model.Pv_Loanofficer)  // pv 与 lv 区别 有两个 loanofficer
            {
                ShowColumnsList.Add("Loan officer");
            }
            if (model.Pv_Partner)
            {
                ShowColumnsList.Add("Partner");
            }

            if (model.Pv_Progress)
            {
                ShowColumnsList.Add("Progress");
            }

            if (model.Pv_Refcode)
            {
                ShowColumnsList.Add("Refcode");
            }

            if (model.Pv_Referral)
            {
                ShowColumnsList.Add("Referral");
            }


            #endregion

            return ShowColumnsList;

        }


        private void ExportClients(string IDs)
        {
            try
            {
                BLL.Prospect _bllProspect = new BLL.Prospect();
                DataTable dt = new DataTable();

                int recordCount = 0;

                string strWhere = isAll == true ? queryCondition : " Contactid in(" + IDs + ")";

                DataSet Lists = _bllProspect.GetList(recordTotal, 1, strWhere, out recordCount, "Contactid", 0);
                if (recordCount > 0)
                {
                    dt = Lists.Tables[0];
                }

                List<string> ShowColumnsList = ShowColumnsForClients();
                List<string> NoShowColumnsList = new List<string>();

                foreach (DataColumn item in dt.Columns)
                {
                    if (ShowColumnsList.Contains(item.ColumnName))
                    {
                        item.Caption = item.ColumnName;
                    }
                    else
                    {
                        NoShowColumnsList.Add(item.ColumnName);
                    }

                }

                foreach (string item in NoShowColumnsList)
                {
                    dt.Columns.Remove(dt.Columns[item]);
                }



                Common.XlsExporter.DownloadXls(this.Page, dt, "Clients Pipeline Report.xls", "Clients Pipeline Report");

            }
            catch (Exception ex)
            {

            }
        }

        private List<string> ShowColumnsForClients()
        {

            BLL.UserProspectColumns upcBll = new UserProspectColumns();

            Model.UserProspectColumns model = upcBll.GetModel(this.CurrUser.iUserID);

            List<string> ShowColumnsList = new List<string>();

            #region ShowColumns

            ShowColumnsList.Add("Client");

            if (model.LastCompletedStage)
            {
                ShowColumnsList.Add("LastCompletedStage");
            }
            if (model.LastStageComplDate)
            {
                ShowColumnsList.Add("LastStageComplDate");
            }


            #region MyRegion
            //if (model.Lv_Amount)
            //{
            //    ShowColumnsList.Add("Amount");
            //}

            //if (model.Lv_Branch)
            //{
            //    ShowColumnsList.Add("Branch");
            //}

            //if (model.Lv_Estclose)
            //{
            //    ShowColumnsList.Add("Estclose)");
            //}
            //if (model.Lv_Leadsource)
            //{
            //    ShowColumnsList.Add("Leadsource");
            //}
            //if (model.Lv_Lien)
            //{
            //    ShowColumnsList.Add("Lien");
            //}
            //if (model.Lv_Loanofficer)
            //{
            //    ShowColumnsList.Add("Loan officer");
            //}
            //if (model.Lv_Loanprogram)
            //{
            //    ShowColumnsList.Add("program");
            //}

            //if (model.Lv_Partner)
            //{
            //    ShowColumnsList.Add("Partner");
            //}
            //if (model.Lv_Pointfilename)
            //{
            //    ShowColumnsList.Add("filename");
            //}
            //if (model.Lv_Progress)
            //{
            //    ShowColumnsList.Add("Progress");
            //}
            //if (model.Lv_Ranking)
            //{
            //    ShowColumnsList.Add("Ranking");
            //}
            //if (model.Lv_Rate)
            //{
            //    ShowColumnsList.Add("Rate");
            //}
            //if (model.Lv_Refcode)
            //{
            //    ShowColumnsList.Add("Refcode");
            //}
            //if (model.Lv_Referral)
            //{
            //    ShowColumnsList.Add("Referral");
            //} 
            #endregion
            if (model.Pv_Branch)
            {
                ShowColumnsList.Add("Branch");
            }
            if (model.Pv_Created)
            {
                ShowColumnsList.Add("Created");
            }
            if (model.Pv_Leadsource)
            {
                ShowColumnsList.Add("Leadsource");
            }
            if (model.Pv_Loanofficer)  // pv 与 lv 区别 有两个 loanofficer
            {
                ShowColumnsList.Add("LoanOfficer");
            }
            if (model.Pv_Partner)
            {
                ShowColumnsList.Add("Partner");
            }

            if (model.Pv_Progress)
            {
                ShowColumnsList.Add("Progress");
            }

            if (model.Pv_Refcode)
            {
                ShowColumnsList.Add("Refcode");
            }

            if (model.Pv_Referral)
            {
                ShowColumnsList.Add("Referral");
            }


            #endregion

            return ShowColumnsList;

        }

        private void WriteExcelFile(StringBuilder sb)
        {
            Response.Clear();
            Response.Buffer = false;
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(@"ExportFile.xls"));
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            Response.ContentType = "application/ms-excel";

            Response.Write(sb.ToString());

            Response.End();
        }
    }
}
