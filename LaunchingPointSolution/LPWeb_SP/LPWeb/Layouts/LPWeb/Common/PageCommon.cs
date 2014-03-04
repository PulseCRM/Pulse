using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using OfficeOpenXml;
using ExcelLibrary.SpreadSheet;
using System.Collections;

namespace LPWeb.Common
{
    public class PageCommon
    {
        /// <summary>
        /// Js脚步 Reload Opener
        /// </summary>
        public static readonly string Js_ReloadOpener = "try{window.opener.location.reload();}catch(e){}";

        /// <summary>
        /// Js脚步 Refresh Opener
        /// </summary>
        public static readonly string Js_RefreshOpener =
            "try{window.opener.location.href = window.opener.location.href;}catch(e){}";

        /// <summary>
        /// Js脚步 Refresh 自身窗体
        /// </summary>
        public static readonly string Js_RefreshSelf = "window.location.href = window.location.href;";
        public static readonly string Js_RefreshParent = "window.parent.location.href=window.parent.location.href";
        /// <summary>
        /// 关闭窗口
        /// </summary>
        public static readonly string Js_CloseWindow = "try{window.opener = null;}catch(e){}window.close();";

        /// <summary>
        /// Reload Opener and Refresh self
        /// </summary>
        public static readonly string Js_ReLoadOpener_RefreshSelf =
            "try{window.opener.location.reload();}catch(e){}window.location.href=window.location.href";

        /// <summary>
        /// 返回DataRow中的日期字符串
        /// 刘洋 2008-01-08
        /// </summary>
        /// <param name="DataRowObj"></param>
        /// <param name="sDateField"></param>
        /// <returns></returns>
        public static string GetShortDateString(DataRow DataRowObj, string sDateField)
        {
            return DataRowObj[sDateField] == DBNull.Value
                       ? string.Empty
                       : Convert.ToDateTime(DataRowObj[sDateField]).ToShortDateString();
        }

        /// <summary>
        /// 数字左侧补零
        /// </summary>
        /// <param name="iNum"></param>
        /// <param name="iMaxLength"></param>
        /// <returns></returns>
        public static string LeftPadZero(int iNum, int iMaxLength)
        {
            string sNum = iNum.ToString();

            if (sNum.Length > iMaxLength)
            {
                throw new Exception("积分卡卡号越界");
            }

            if (sNum.Length < iMaxLength) // 如果不够iMaxLength位，则补零
            {
                //int iZeroCount = iMaxLength - sNum.Length;
                sNum = sNum.PadLeft(iMaxLength, '0');
            }

            return sNum;
        }

        /// <summary>
        /// 生成数字密码
        /// </summary>
        /// <param name="Generater"></param>
        /// <param name="iPwdLength"></param>
        /// <returns></returns>
        public static string GenerateNumPassword(RNGCryptoServiceProvider PwdGenerater, int iPwdLength)
        {
            #region 生成密码

            var RandomNumbers = new byte[iPwdLength];
            PwdGenerater.GetBytes(RandomNumbers);

            var sbPassword = new StringBuilder(iPwdLength);
            foreach (byte RandomNum in RandomNumbers)
            {
                sbPassword.Append(Convert.ToInt32(RandomNum));
            }

            string sPassword = sbPassword.ToString();

            #endregion

            string sPassword1 = sPassword.Substring(0, iPwdLength);

            return sPassword1;
        }

        /// <summary>
        /// 生成数字密码
        /// </summary>
        /// <param name="iPwdLength"></param>
        /// <returns></returns>
        public static string GenerateNumPassword(int iPwdLength)
        {
            var PwdGenerater = new RNGCryptoServiceProvider();

            #region 生成密码

            var RandomNumbers = new byte[iPwdLength];
            PwdGenerater.GetBytes(RandomNumbers);

            var sbPassword = new StringBuilder(iPwdLength);
            foreach (byte RandomNum in RandomNumbers)
            {
                sbPassword.Append(Convert.ToInt32(RandomNum));
            }

            string sPassword = sbPassword.ToString();

            #endregion

            string sPassword1 = sPassword.Substring(0, iPwdLength);

            return sPassword1;
        }

        /// <summary>
        /// 生成数字字符混合密码
        /// </summary>
        /// <param name="iPwdLength">最大32位</param>
        /// <returns></returns>
        public static string GenerateCharPassword(int iPwdLength)
        {
            Guid uNewPwd = Guid.NewGuid();
            var sbPwd = new StringBuilder(uNewPwd.ToString());
            sbPwd.Replace("-", string.Empty);
            string sPwd = sbPwd.ToString().Substring(0, iPwdLength);
            return sPwd;
        }

        /// <summary>
        /// format javascript alert message
        /// neo liu 2010-11-24
        /// </summary>
        /// <param name="sMsg"></param>
        /// <returns></returns>
        public static string FormatAlertMsg(string sMsg)
        {
            StringBuilder sbTemp = new StringBuilder(sMsg);
            sbTemp.Replace("\"", "\\\"");
            sbTemp.Replace("'", "\'");
            sbTemp.Replace(Environment.NewLine, "\\r\\n");
            sbTemp.Replace("\r", "\\r");
            sbTemp.Replace("\n", "\\n");
            return sbTemp.ToString();
        }

        /// <summary>
        /// 将Excel导入到DataTable
        /// 刘洋 2010-02-09
        /// </summary>
        /// <param name="sExcelPath"></param>
        /// <param name="sSheetName"></param>
        /// <returns></returns>
        public static DataTable ImportExcelToDataTable(string sExcelPath, string sSheetName)
        {
            var DataTable1 = new DataTable();


            string sConnStr = "provider=microsoft.jet.oledb.4.0;data source=" + sExcelPath +
                              ";extended properties=\"excel 8.0;HDR=NO;IMEX=1;\"";

            if (sExcelPath.Substring(sExcelPath.LastIndexOf(".")).ToLower() == ".xlsx")
            {
                sConnStr = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + sExcelPath +
                           ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'";
            }
            using (var ExcelConn = new OleDbConnection(sConnStr))
            {
                if (sSheetName == "")
                {
                    //No sheet name
                    ExcelConn.Open();
                    DataTable dtSheetName = ExcelConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                                                                          new object[] { null, null, null, "Table" });
                    foreach (DataRow drSheet in dtSheetName.Rows)
                    {
                        sSheetName = drSheet["TABLE_NAME"].ToString().Replace("$", "");
                        DataTable1 = new DataTable();
                        string sSql = "select * from [" + sSheetName + "$]";
                        var OleCmd = new OleDbCommand(sSql, ExcelConn);
                        var OleAdapter = new OleDbDataAdapter(OleCmd);
                        OleAdapter.Fill(DataTable1);
                        if (DataTable1.Rows.Count > 1 && DataTable1.Columns.Count > 1)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    string sSql = "select * from [" + sSheetName + "$]";
                    var OleCmd = new OleDbCommand(sSql, ExcelConn);
                    var OleAdapter = new OleDbDataAdapter(OleCmd);
                    OleAdapter.Fill(DataTable1);
                }
            }

            return DataTable1;
        }

        /// <summary>
        /// Generate json for flexigrid
        /// 刘洋 2010-02-15
        /// </summary>
        /// <param name="iPageIndex"></param>
        /// <param name="iRowCount"></param>
        /// <param name="dt"></param>
        /// <param name="sIDField"></param>
        /// <returns></returns>
        public static string GenerateJson_Flexigrid(int iPageIndex, int iRowCount, DataTable DataList, string sIDField)
        {
            // json格式：{ page:1, total:38, rows:[{id:'200', cell:['1', '200', '1', 'a61a4c2d-8a12-4ae8-a31f-6ecf1d0471e6', 'fn29', 'ln29\'s \\you', 'fn1.ln15@gmail.com', '2010-2-12 10:46:01']},{id:'199', cell:['2', '199', '1', '7208c14c-ff90-4bb8-aa51-d7dbdca8b690', 'fn28', 'ln28', 'fn2.ln15@gmail.com', '2010-2-12 10:46:01']},{id:'198', cell:['3', '198', '1', 'b7d6a629-9430-4b95-bf73-86c8c1255e10', 'fn27', 'ln27', 'fn1.ln14@gmail.com', '2010-2-12 10:46:01']}]}

            var sbJson = new StringBuilder("{ page:" + iPageIndex + ", total:" + iRowCount + ", rows:[<%=Rows%>]}");

            var sbRows = new StringBuilder();
            for (int i = 0; i < DataList.Rows.Count; i++)
            {
                DataRow DataRow1 = DataList.Rows[i];

                StringBuilder sbRow;
                if (i == 0)
                {
                    sbRow = new StringBuilder("{id:'" + DataRow1[sIDField] + "', cell:[<%=Cells%>]}");
                }
                else
                {
                    sbRow = new StringBuilder(",{id:'" + DataRow1[sIDField] + "', cell:[<%=Cells%>]}");
                }

                var sbCells = new StringBuilder();
                for (int j = 0; j < DataList.Columns.Count; j++)
                {
                    string sRowData = DataRow1[j].ToString();

                    // 替换特殊字符
                    sRowData = sRowData.Replace("\\", "\\\\");
                    sRowData = sRowData.Replace("'", "\\'");

                    if (j == 0)
                    {
                        sbCells.Append("'" + sRowData + "'");
                    }
                    else
                    {
                        sbCells.Append(", '" + sRowData + "'");
                    }
                }

                sbRow.Replace("<%=Cells%>", sbCells.ToString());

                sbRows.Append(sbRow.ToString());
            }


            sbJson.Replace("<%=Rows%>", sbRows.ToString());

            return sbJson.ToString();
        }

        /// <summary>
        /// Generate json for flexigrid
        /// 刘洋 2010-02-15
        /// </summary>
        /// <param name="iPageIndex"></param>
        /// <param name="iRowCount"></param>
        /// <param name="DataList"></param>
        /// <param name="sIDField"></param>
        /// <returns></returns>
        public static string GenerateJson_Flexigrid(int iPageIndex, int iRowCount, DataView DataList, string sIDField)
        {
            // json格式：{ page:1, total:38, rows:[{id:'200', cell:['1', '200', '1', 'a61a4c2d-8a12-4ae8-a31f-6ecf1d0471e6', 'fn29', 'ln29\'s \\you', 'fn1.ln15@gmail.com', '2010-2-12 10:46:01']},{id:'199', cell:['2', '199', '1', '7208c14c-ff90-4bb8-aa51-d7dbdca8b690', 'fn28', 'ln28', 'fn2.ln15@gmail.com', '2010-2-12 10:46:01']},{id:'198', cell:['3', '198', '1', 'b7d6a629-9430-4b95-bf73-86c8c1255e10', 'fn27', 'ln27', 'fn1.ln14@gmail.com', '2010-2-12 10:46:01']}]}

            var sbJson = new StringBuilder("{ page:" + iPageIndex + ", total:" + iRowCount + ", rows:[<%=Rows%>]}");

            var sbRows = new StringBuilder();
            for (int i = 0; i < DataList.Count; i++)
            {
                DataRowView DataRow1 = DataList[i];

                StringBuilder sbRow;
                if (i == 0)
                {
                    sbRow = new StringBuilder("{id:'" + DataRow1[sIDField] + "', cell:[<%=Cells%>]}");
                }
                else
                {
                    sbRow = new StringBuilder(",{id:'" + DataRow1[sIDField] + "', cell:[<%=Cells%>]}");
                }

                var sbCells = new StringBuilder();
                for (int j = 0; j < DataList.Table.Columns.Count; j++)
                {
                    string sRowData = DataRow1[j].ToString();

                    // 替换特殊字符
                    sRowData = sRowData.Replace("\\", "\\\\");
                    sRowData = sRowData.Replace("'", "\\'");

                    if (j == 0)
                    {
                        sbCells.Append("'" + sRowData + "'");
                    }
                    else
                    {
                        sbCells.Append(", '" + sRowData + "'");
                    }
                }

                sbRow.Replace("<%=Cells%>", sbCells.ToString());

                sbRows.Append(sbRow.ToString());
            }


            sbJson.Replace("<%=Rows%>", sbRows.ToString());

            return sbJson.ToString();
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="sSmtpHost">The s SMTP host.</param>
        /// <param name="iSmtpPort">The i SMTP port.</param>
        /// <param name="sSmtpUsername">The s SMTP username.</param>
        /// <param name="sSmtpPwd">The s SMTP PWD.</param>
        /// <param name="sFrom">The s from.</param>
        /// <param name="sToName">Name of the s to.</param>
        /// <param name="sToEmail">The s to email.</param>
        /// <param name="sSubject">The s subject.</param>
        /// <param name="sBody">The s body.</param>
        /// <returns>错误信息：如果为空字符串，则为成功，否则为错误信息</returns>
        public static string SendEmail(string sSmtpHost, int iSmtpPort, string sSmtpUsername, string sSmtpPwd,
                                       string sFrom, string sToName, string sToEmail, string sSubject, string sBody)
        {
            string sError = string.Empty;

            ////  The mailman object is used for sending and receiving email.
            //Chilkat.MailMan MailSender = new Chilkat.MailMan();

            ////  Unlock
            //MailSender.UnlockComponent("SCloudbreakMAILQ_oUfVBduJkwkG");

            ////  Set the SMTP server.
            //MailSender.SmtpHost = sSmtpHost;

            ////  Set the SMTP login/password (if required)

            //if (!string.IsNullOrEmpty(sSmtpUsername))
            //{
            //    MailSender.SmtpUsername = sSmtpUsername;
            //}

            //if (!string.IsNullOrEmpty(sSmtpPwd))
            //{
            //    MailSender.SmtpPassword = sSmtpPwd;
            //}

            //if(iSmtpPort>0)
            //{
            //    MailSender.SmtpPort = iSmtpPort;
            //}

            ////  Create a new email object
            //Chilkat.Email email = new Chilkat.Email();

            //email.Subject = sSubject;
            //email.SetHtmlBody(sBody);
            ////email.Body = sBody;
            //email.From = sFrom;
            //email.AddTo(sToName, sToEmail);


            ////  Call SendEmail to connect to the SMTP server and send.
            ////  The connection (i.e. session) to the SMTP server remains
            ////  open so that subsequent SendEmail calls may use the
            ////  same connection.
            //bool bIsSuccess = MailSender.SendEmail(email);
            //if (bIsSuccess != true)
            //{
            //    sError = MailSender.LastErrorText;
            //}

            return sError;
        }

        /// Add thead and tbody to GridView
        /// 刘洋 2010-03-11
        /// </summary>
        /// <param name="grid"></param>
        public static void MakeGridViewAccessible(GridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                //This replaces <td> with <th> and adds the scope attribute 
                grid.UseAccessibleHeader = true;

                //This will add the <thead> and <tbody> elements
                grid.HeaderRow.TableSection = TableRowSection.TableHeader;

                //This adds the <tfoot> element. Remove if you don't have a footer row 
                //grid.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        /// <summary>
        /// 获取根Url
        /// 刘洋 2010-04-06
        /// </summary>
        /// <param name="PageObj"></param>
        /// <returns></returns>
        public static string GetRootUrl(Page PageObj)
        {
            string sDomain = PageObj.Request.ServerVariables["HTTP_HOST"];
            string sRoot = "http://" + sDomain + PageObj.ResolveUrl("~/");
            return sRoot;
        }

        /// <summary>
        /// 根据登录用户获取组织结构筛选数据
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="orgLevel">组织结构level 1:region 的exective;2:division的executives;3:branch的managers;4:普通用户</param>
        /// <param name="firstLevelQuerCon">得到最高级的ids连接起来的查询条件</param>
        /// <returns></returns>
        public static DataSet GetOrgStructureDataSourceByLoginUser(HttpContext context, out int orgLevel,
                                                                   out string firstLevelQuerCon)
        {
            var curUser = new LoginUser();
            int userId = curUser.iUserID;

            var dsOrgStructure = new DataSet();
            var bllRegionExecutives = new RegionExecutives();
            var bllDivisonExecutives = new DivisionExecutives();
            var bllBranchManagers = new BranchManagers();

            DataTable dtRegion = bllRegionExecutives.GetRegionsByExecutiveId(userId);

            //当前用户是某些Region的Executives
            if (dtRegion != null && dtRegion.Rows.Count > 0)
            {
                //1.Add Region Table
                DataTable dtRegionAdd = dtRegion.Copy();
                dtRegionAdd.TableName = "Region";
                dsOrgStructure.Tables.Add(dtRegionAdd);


                firstLevelQuerCon = string.Format(" RegionID IN ({0})",
                                                  dtRegionAdd.AsEnumerable().Select(
                                                      item => item.Field<int>("RegionId").ToString()).Aggregate(
                                                          (ids, next) => ids + "," + next));
                orgLevel = 1;
            }
            else //不是Region的Executive的情况(可能是divison的executive，或者是某些Branches的Manager,或者是普通用户)
            {
                DataTable dtDivision = bllDivisonExecutives.GetDivisonsByExecutiveId(userId);

                //当前用户是某些Divison的Executives
                if (dtDivision != null && dtDivision.Rows.Count > 0)
                {
                    //1.add Division table
                    DataTable dtDivisionAdd = dtDivision.Copy();
                    dtDivisionAdd.TableName = "Divison";
                    dsOrgStructure.Tables.Add(dtDivisionAdd);

                    firstLevelQuerCon = string.Format(" DivisionID IN ({0})",
                                                      dtDivisionAdd.AsEnumerable().Select(
                                                          item => item.Field<int>("DivisionId").ToString()).Aggregate(
                                                              (ids, next) => ids + "," + next));
                    orgLevel = 2;
                }
                else //当前用户是某些Branch的BranchManager
                {
                    DataTable dtBranch = bllBranchManagers.GetBranchesByBranchMgrId(userId);
                    if (dtBranch != null && dtBranch.Rows.Count > 0)
                    {
                        //1.add Branch Table
                        DataTable dtBranchAdd = dtBranch.Copy();
                        dtBranchAdd.TableName = "Branches";
                        dsOrgStructure.Tables.Add(dtBranchAdd);

                        dsOrgStructure.Tables.Add(new DataTable("Region"));
                        dsOrgStructure.Tables.Add(new DataTable("Division"));

                        firstLevelQuerCon = string.Format(" BranchID IN ({0})",
                                                          dtBranch.AsEnumerable().Select(
                                                              item => item.Field<int>("BranchId").ToString()).Aggregate(
                                                                  (ids, next) => ids + "," + next));
                        orgLevel = 3;
                    }
                    else //普通用户
                    {
                        dsOrgStructure.Tables.Add(new DataTable("Region"));
                        dsOrgStructure.Tables.Add(new DataTable("Division"));
                        dsOrgStructure.Tables.Add(new DataTable("Branches"));

                        firstLevelQuerCon = "";
                        orgLevel = 4;
                    }
                }
            }

            return dsOrgStructure;
        }
        /// <summary>
        /// 根据登录用户获取组织结构筛选数据
        /// </summary>
        /// <param name="regionId">Region ID</param>
        /// <param name="divisionId">Division ID</param>
        /// <returns></returns>
        public static DataSet GetOrgStructureDataSourceByLoginUser(int? regionId, int? divisionId, bool bAllLoans)
        {
            var bllRegion = new Regions();
            var bllDivision = new Divisions();
            var bllBranches = new Branches();

            int curUserId = new LoginUser().iUserID;

            //Regions
            DataTable dtRegion = null;

            if (bAllLoans == true)   // All Loans
            {
                dtRegion = bllRegion.GetRegionList_AllLoans(curUserId);
            }
            else // Assigned Loans
            {
                dtRegion = bllRegion.GetRegionList_AssingedLoans(curUserId);
            }
            dtRegion.TableName = "region";

            DataRow drRegion = dtRegion.NewRow();
            drRegion["RegionId"] = "-1";
            drRegion["Name"] = "All Regions";
            dtRegion.Rows.InsertAt(drRegion, 0);

            //Divisions
            DataTable dtDivision = null;
            if (bAllLoans == true)   // All Loans
            {
                if (!regionId.HasValue)
                {
                    dtDivision = bllDivision.GetDivision_AllLoans(curUserId);
                }
                else
                {
                    dtDivision = bllDivision.GetDivision_AllLoans(curUserId, regionId.Value);
                }
            }
            else // Assigned Loans
            {
                if (!regionId.HasValue)
                {
                    dtDivision = bllDivision.GetDivisionList_AssingedLoans(curUserId);
                }
                else
                {
                    dtDivision = bllDivision.GetDivisionList_AssingedLoans(curUserId, regionId.Value);
                }
            }
            dtDivision.TableName = "division";

            DataRow drDivision = dtDivision.NewRow();
            drDivision["DivisionId"] = "-1";
            drDivision["Name"] = "All Divisions";
            dtDivision.Rows.InsertAt(drDivision, 0);

            //Branches
            DataTable dtBranches = null;

            if (bAllLoans == true)   // All Loans
            {
                dtBranches = bllBranches.GetBranchList_AllLoans(curUserId, regionId ?? 0, divisionId ?? 0);
            }
            else // Assigned Loans
            {
                dtBranches = bllBranches.GetBranchList_AssingedLoans(curUserId, regionId ?? 0, divisionId ?? 0);
            }
            dtBranches.TableName = "branches";

            DataRow drBranch = dtBranches.NewRow();
            drBranch["BranchId"] = "-1";
            drBranch["Name"] = "All Branches";
            dtBranches.Rows.InsertAt(drBranch, 0);

            var ds = new DataSet();
            ds.Tables.Add(dtRegion);
            ds.Tables.Add(dtDivision);
            ds.Tables.Add(dtBranches);

            return ds;
        }

        /// <summary>
        /// get organization filter(region/division/branch) collections
        /// neo 2010-11-10
        /// </summary>
        /// <param name="iRegionID"></param>
        /// <param name="iDivisionID"></param>
        /// <returns></returns>
        public static DataSet GetOrganFilter(int iRegionID, int iDivisionID)
        {
            // 获取当前用户信息
            LoginUser CurrentUser = new LoginUser();
            int iCurrentUserID = CurrentUser.iUserID;

            #region 加载Regions

            LPWeb.BLL.Regions RegionManager = new LPWeb.BLL.Regions();

            DataTable RegionListData = null;

            if (CurrentUser.sRoleName == "Branch Manager")
            {
                RegionListData = RegionManager.GetRegionFilter_Branch_Manager(iCurrentUserID);
            }
            else
            {
                if (CurrentUser.sRoleName == "Executive")
                {
                    RegionListData = RegionManager.GetRegionFilter_Executive(iCurrentUserID);
                }
                else
                {
                    RegionListData = RegionManager.GetRegionFilter(iCurrentUserID);
                }
            }

            RegionListData.TableName = "Regions";

            // add "All Regions" row
            DataRow NewRegionRow = RegionListData.NewRow();
            NewRegionRow["RegionID"] = "-1";
            NewRegionRow["Name"] = "All Regions";
            RegionListData.Rows.InsertAt(NewRegionRow, 0);

            #endregion

            #region 加载Divisions

            LPWeb.BLL.Divisions DivisionManager = new LPWeb.BLL.Divisions();
            DataTable DivisionListData = null;

            if (CurrentUser.sRoleName == "Branch Manager")
            {
                DivisionListData = DivisionManager.GetDivisionFilter_Branch_Manager(iCurrentUserID, iRegionID);
            }
            else
            {
                if (CurrentUser.sRoleName == "Executive")
                {
                    DivisionListData = DivisionManager.GetDivisionFilter_Executive(iCurrentUserID, iRegionID);
                }
                else
                {
                    DivisionListData = DivisionManager.GetDivisionFilter(iCurrentUserID, iRegionID);
                }
            }

            DivisionListData.TableName = "Divisions";

            DataRow NewDivisionRow = DivisionListData.NewRow();
            NewDivisionRow["DivisionID"] = "-1";
            NewDivisionRow["Name"] = "All Divisions";
            DivisionListData.Rows.InsertAt(NewDivisionRow, 0);

            #endregion

            #region 加载Branches

            LPWeb.BLL.Branches BrancheManager = new LPWeb.BLL.Branches();
            DataTable BranchListData = null;

            if (CurrentUser.sRoleName == "Branch Manager")
            {
                BranchListData = BrancheManager.GetBranchFilter_Branch_Manager(iCurrentUserID, iRegionID, iDivisionID);
            }
            else
            {
                if (CurrentUser.sRoleName == "Executive")
                {
                    BranchListData = BrancheManager.GetBranchFilter_Executive(iCurrentUserID, iRegionID, iDivisionID);
                }
                else
                {
                    BranchListData = BrancheManager.GetBranchFilter(iCurrentUserID, iRegionID, iDivisionID);
                }
            }

            BranchListData.TableName = "Branches";

            DataRow NewBranchRow = BranchListData.NewRow();
            NewBranchRow["BranchID"] = "-1";
            NewBranchRow["Name"] = "All Branches";
            BranchListData.Rows.InsertAt(NewBranchRow, 0);

            #endregion

            DataSet OrganFilters = new DataSet();
            OrganFilters.Tables.Add(RegionListData);
            OrganFilters.Tables.Add(DivisionListData);
            OrganFilters.Tables.Add(BranchListData);

            return OrganFilters;
        }

        /// <summary>
        /// get organization filter(region/division/branch) collections
        /// neo 2010-11-10
        /// </summary>
        /// <param name="iRegionID"></param>
        /// <param name="iDivisionID"></param>
        /// <returns></returns>
        public static DataSet GetOrganFilter_UserList(int iRegionID, int iDivisionID)
        {
            // 获取当前用户信息
            LoginUser CurrentUser = new LoginUser();
            int iCurrentUserID = CurrentUser.iUserID;

            #region 加载Regions

            LPWeb.BLL.Regions RegionManager = new LPWeb.BLL.Regions();
            DataTable RegionListData = RegionManager.GetRegionFilter_UserList(iCurrentUserID);
            RegionListData.TableName = "Regions";

            // add "All Regions" row
            DataRow NewRegionRow = RegionListData.NewRow();
            NewRegionRow["RegionID"] = "-1";
            NewRegionRow["Name"] = "All Regions";
            RegionListData.Rows.InsertAt(NewRegionRow, 0);

            #endregion

            #region 加载Divisions

            LPWeb.BLL.Divisions DivisionManager = new LPWeb.BLL.Divisions();
            DataTable DivisionListData = DivisionManager.GetDivisionFilter_UserList(iCurrentUserID, iRegionID);
            DivisionListData.TableName = "Divisions";

            DataRow NewDivisionRow = DivisionListData.NewRow();
            NewDivisionRow["DivisionID"] = "-1";
            NewDivisionRow["Name"] = "All Divisions";
            DivisionListData.Rows.InsertAt(NewDivisionRow, 0);

            #endregion

            #region 加载Branches

            LPWeb.BLL.Branches BrancheManager = new LPWeb.BLL.Branches();
            DataTable BranchListData = BrancheManager.GetBranchFilter_UserList(iCurrentUserID, iRegionID, iDivisionID);
            BranchListData.TableName = "Branches";

            DataRow NewBranchRow = BranchListData.NewRow();
            NewBranchRow["BranchID"] = "-1";
            NewBranchRow["Name"] = "All Branches";
            BranchListData.Rows.InsertAt(NewBranchRow, 0);

            #endregion

            DataSet OrganFilters = new DataSet();
            OrganFilters.Tables.Add(RegionListData);
            OrganFilters.Tables.Add(DivisionListData);
            OrganFilters.Tables.Add(BranchListData);

            return OrganFilters;
        }

        #region 分页计算函数

        /// <summary>
        /// 计算分页数
        /// </summary>
        /// <param name="iRowCount"></param>
        /// <param name="iPageSize"></param>
        /// <returns></returns>
        public static int CalcPageCount(int iRowCount, int iPageSize)
        {
            if (iRowCount == 0)
            {
                return 0;
            }

            int iPageCount = 0;
            int iYuShu = iRowCount % iPageSize;
            if (iYuShu == 0)
            {
                iPageCount = iRowCount / iPageSize;
            }
            else
            {
                iPageCount = (iRowCount - iYuShu) / iPageSize + 1;
            }

            return iPageCount;
        }

        /// <summary>
        /// 计算分页起始索引位置
        /// </summary>
        /// <param name="iCurrentPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <returns></returns>
        public static int CalcStartIndex(int iPageIndex, int iPageSize)
        {
            // 1-10, 11-20, 21-30, 31-40
            int iStartIndex = (iPageIndex - 1) * iPageSize + 1;
            return iStartIndex;
        }

        /// <summary>
        /// 计算分页结束索引位置
        /// </summary>
        /// <param name="iCurrentPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <param name="iRowCount"></param>
        /// <returns></returns>
        public static int CalcEndIndex(int iStartInex, int iPageSize, int iRowCount)
        {
            int iEndIndex = iStartInex + iPageSize - 1;
            if (iEndIndex > iRowCount)
            {
                iEndIndex = iRowCount;
            }

            return iEndIndex;
        }

        #endregion

        #region 页面信息函数

        /// <summary>
        /// Response.Write 提示信息和Js代码，不终止输出
        /// 刘洋 2008-01-09
        /// </summary>
        /// <param name="PageObj"></param>
        /// <param name="sMsg"></param>
        /// <param name="JsCode"></param>
        public static void WriteJs(Page PageObj, string sMsg, string JsCode)
        {
            if (sMsg.Trim() == string.Empty)
            {
                PageObj.Response.Write("<script type='text/javascript' language='javascript'>" + JsCode + "</script>");
            }
            else
            {
                sMsg = sMsg.Replace("'", "\\'");
                PageObj.Response.Write("<script type='text/javascript' language='javascript'>alert(\"" + sMsg + "\");" +
                                       JsCode + "</script>");
            }
        }

        /// <summary>
        /// Alert Msg，注册Js脚本
        /// </summary>
        /// <param name="PageObj"></param>
        /// <param name="sMsg"></param>
        /// <param name="JsCode"></param>
        public static void RegisterJsMsg(Page PageObj, string sMsg, string JsCode)
        {
            sMsg = sMsg.Trim();
            JsCode = JsCode.Trim();

            if (sMsg != string.Empty)
            {
                PageObj.ClientScript.RegisterStartupScript(PageObj.GetType(), "AlertMsg",
                                                           "alert(\"" + FormatAlertMsg(sMsg) + "\");", true);
            }

            if (JsCode != string.Empty)
            {
                PageObj.ClientScript.RegisterStartupScript(PageObj.GetType(), "AddJs", JsCode, true);
            }
        }

        /// <summary>
        /// Response.Write 提示信息和Js代码，然后终止输出
        /// 刘洋 2008-01-09
        /// </summary>
        /// <param name="PageObj"></param>
        /// <param name="sMsg"></param>
        /// <param name="JsCode"></param>
        public static void WriteJsEnd(Page PageObj, string sMsg, string JsCode)
        {
            if (sMsg.Trim() == string.Empty)
            {
                PageObj.Response.Write("<script type='text/javascript' language='javascript'>" + JsCode + "</script>");
            }
            else
            {
                sMsg = sMsg.Replace("'", "\\'");
                PageObj.Response.Write("<script type='text/javascript' language='javascript'>alert(\"" + sMsg + "\");" + JsCode + "</script>");
            }

            try
            {
                PageObj.Response.End();
            }
            catch
            {
                
            }
        }

        public static void WriteJsNoEnd(Page PageObj, string sMsg, string JsCode)
        {
            if (sMsg.Trim() == string.Empty)
            {
                PageObj.Response.Write("<script type='text/javascript' language='javascript'>" + JsCode + "</script>");
            }
            else
            {
                sMsg = sMsg.Replace("'", "\\'");
                PageObj.Response.Write("<script type='text/javascript' language='javascript'>alert(\"" + sMsg + "\");" + JsCode + "</script>");
            }
            try
            {
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch
            {

            }

        }

        /// <summary>
        /// Alert javascript message
        /// </summary>
        /// <param name="PageObj"></param>
        /// <param name="sMsg"></param>
        public static void AlertMsg(Page PageObj, string sMsg)
        {
            HiddenField hiMsg = new HiddenField();
            hiMsg.ID = "hiMsg";
            hiMsg.Value = sMsg;
            PageObj.Form.Controls.Add(hiMsg);
            PageObj.ClientScript.RegisterStartupScript(PageObj.GetType(), "_AlertMsg", 
                string.Format("alert(document.getElementById('{0}').value);", hiMsg.ClientID), true);
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="PageObj"></param>
        /// <param name="sErrorMsg"></param>
        /// <param name="sReturnUrl"></param>
        public static void ShowError(Page PageObj, string sErrorMsg, string sBtnText, string sReturnUrl)
        {
            string sFrom =
                PageObj.Server.UrlEncode(PageObj.Request.AppRelativeCurrentExecutionFilePath.Replace("~/",
                                                                                                     GetRootUrl(PageObj)));

            PageObj.Response.Redirect(
                "~/Common/Error.aspx?Error=" + PageObj.Server.UrlEncode(sErrorMsg) + "&From=" + sFrom + "&BtnText=" +
                PageObj.Server.UrlEncode(sBtnText) + "&To=" + PageObj.Server.UrlEncode(sReturnUrl), true);
        }

        #endregion

        #region 校验函数

        /// <summary>
        /// 校验上传文件
        /// 刘洋 2010-01-19
        /// </summary>
        /// <param name="PageObj"></param>
        /// <param name="FileUploadObj"></param>
        /// <param name="iMaxFileSize"></param>
        /// <param name="sMsg"></param>
        /// <param name="AllowFileExts"></param>
        /// <returns></returns>
        public static bool ValidateUpload(Page PageObj, FileUpload FileUploadObj, int iMaxFileSize, out string sMsg,
                                          params string[] AllowFileExts)
        {
            sMsg = string.Empty;

            // 校验是否符合上传文件的前提条件
            if (FileUploadObj.PostedFile == null)
            {
                //sMsg = "提示信息：没有指定上传文件或上传文件不存在，请重新选择上传文件";
                sMsg = "The file does not exist.";
                return false;
            }

            // 校验是否选择文件
            if (FileUploadObj.PostedFile.FileName.Trim() == string.Empty)
            {
                //sMsg = "提示信息：请选择文件";
                sMsg = "Please select a file.";
                return false;
            }

            // 校验文件是否存在
            if (FileUploadObj.PostedFile.ContentLength == 0)
            {
                //sMsg = "提示信息：所选文件不存在或大小为0";
                sMsg = "The file is empty.";
                return false;
            }

            // 校验文件后缀
            bool bAllow = false;
            string sUploadFileExt = Path.GetExtension(FileUploadObj.PostedFile.FileName);
            foreach (string sFileExt in AllowFileExts)
            {
                if (sUploadFileExt.ToLower() == sFileExt.ToLower())
                {
                    bAllow = true;
                    break;
                }
            }

            if (bAllow == false)
            {
                string sExtString = string.Empty;
                int i = 0;
                foreach (string sFileExt in AllowFileExts)
                {
                    if (i == 0)
                    {
                        sExtString = "*" + sFileExt;
                    }
                    else
                    {
                        sExtString += "|*" + sFileExt;
                    }

                    i++;
                }
                sMsg = "Please select a " + sExtString + " file.";
                //sMsg = "提示信息：只允许上传后缀为[" + sExtString + "]的文件";
                return false;
            }


            // 校验文件大小
            if (FileUploadObj.PostedFile.ContentLength > iMaxFileSize)
            {
                string strSize = "";
                decimal mMaxSize = iMaxFileSize / (1024 * 1024);
                //sMsg = "提示信息：上传文件最大不能超过[" + mMaxSize + "M]";
                if (mMaxSize <= 0)
                    strSize = string.Format("{0}KB", iMaxFileSize / 1024);
                else
                    strSize = string.Format("{0}MB", iMaxFileSize / (1024 * 1024));
                sMsg = string.Format("The file you tried to upload has exceeded the allowed limit, {0}.", strSize);

                return false;
            }

            return true;
        }

        /// <summary>
        /// 校验必需参数
        /// 刘洋 2009-03-04
        /// </summary>
        /// <param name="PageObj"></param>
        /// <param name="sQueryString"></param>
        /// <param name="TypeObj"></param>
        /// <returns></returns>
        public static bool ValidateQueryString(Page PageObj, string sQueryStringName, QueryStringType TypeObj)
        {
            object oQueryString = PageObj.Request.QueryString[sQueryStringName];

            // 参数是否存在
            if (oQueryString == null)
            {
                return false;
            }

            string sQueryString = oQueryString.ToString();
            if (TypeObj == QueryStringType.ID)
            {
                // 参数类型是否正确
                int iTempID;
                bool bIsInteger = int.TryParse(sQueryString, out iTempID);
                if (bIsInteger == false)
                {
                    return false;
                }

                // 是否小于或等于零
                if (iTempID < -1)
                {
                    return false;
                }
            }
            else if (TypeObj == QueryStringType.Date)
            {
                return IsDate(sQueryString);
            }
            else if (TypeObj == QueryStringType.Guid) // Guid
            {
                return IsGuid(sQueryString);
            }
            else if(TypeObj == QueryStringType.IDs) // IDs: 1001,1002,1003
            {
                return IsIDString(sQueryString);
            }
            else // 字符串
            {
            }

            return true;
        }

        /// <summary>
        /// 校验是否是邮件地址
        /// </summary>
        /// <param name="sEmail"></param>
        /// <returns></returns>
        public static bool IsEmail(string sEmail)
        {
            if (sEmail == string.Empty)
            {
                return false;
            }

            return Regex.IsMatch(sEmail, @"^[\w-]+(?:\.[\w-]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7}$");
        }

        /// <summary>
        /// 校验Guid
        /// </summary>
        /// <param name="sGuild"></param>
        /// <returns></returns>
        public static bool IsGuid(string sGuild)
        {
            if (sGuild == string.Empty)
            {
                return false;
            }

            return Regex.IsMatch(sGuild,
                                 @"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$");
        }

        /// <summary>
        /// 检查字符串是否是有效的日期
        /// </summary>
        /// <param name="sDate"></param>
        /// <returns></returns>
        public static bool IsDate(string sDate)
        {
            DateTime TempDate;
            return DateTime.TryParse(sDate, out TempDate);
        }

        /// <summary>
        /// 校验正整数
        /// </summary>
        /// <param name="sInteger"></param>
        /// <returns></returns>
        public static bool IsPositiveInteger(string sInteger)
        {
            int iInt;
            bool bIsInt = int.TryParse(sInteger, out iInt);
            if (bIsInt == false)
            {
                return false;
            }

            if (iInt > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否是ID
        /// 刘洋 2010-02-16
        /// </summary>
        /// <param name="sID"></param>
        /// <returns></returns>
        public static bool IsID(string sID)
        {
            int iInt;
            bool bIsInt = int.TryParse(sID, out iInt);
            if (bIsInt == false)
            {
                return false;
            }

            if (iInt >= -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Is ID string: 1001,1002,1003
        /// neo 2011-03-22
        /// </summary>
        /// <param name="sIDs"></param>
        /// <returns></returns>
        public static bool IsIDString(string sIDs) 
        {
            return Regex.IsMatch(sIDs, @"^([1-9]\d*)(,[1-9]\d*)*$");
        }

        #endregion

        /// <summary>
        /// 读取xls到 DataTable
        /// Coder:Alex 
        /// </summary>
        /// <param name="sFilePath">xls 完整路径</param>
        /// <returns></returns>
        public DataTable GetTableFromXls(string sFilePath)
        {
            DataTable ExcelData = new DataTable();

            Workbook book = Workbook.Open(sFilePath); //Open the Excel Document
            Worksheet sheet = book.Worksheets[0]; //Open the Worksheet.

            int LastInt = sheet.Cells.LastRowIndex;
            int LastColInt = sheet.Cells.LastColIndex;


            DataRow dr = null;
            for (int rCnt = 0; rCnt <= LastInt; rCnt++)
            {
                if (rCnt == 0)
                {
                    //遍历第一行，得到表的 Columns
                    for (int j = 0; j <= LastColInt; j++)
                    {
                        if (!ExcelData.Columns.Contains(sheet.Cells[0, j].Value.ToString()))
                        {
                            if (sheet.Cells[0, j].Value.ToString().Contains("DOB"))  //DateTime Columns
                            {
                                ExcelData.Columns.Add(sheet.Cells[0, j].Value.ToString().Trim(), typeof(DateTime));

                            }
                            else
                            {
                                ExcelData.Columns.Add(sheet.Cells[0, j].Value.ToString().Trim());
                            }
                        }
                    }
                }
                else
                {
                    dr = ExcelData.NewRow();
                    for (int j = 0; j <= LastColInt; j++)
                    {
                        if (sheet.Cells[rCnt, j].Value != null)
                        {
                            if (ExcelData.Columns[j].Caption.Contains("DOB"))  //DateTime Columns
                            {
                                dr[j] = GetDateTime(sheet.Cells[rCnt, j].Value.ToString());
                            }
                            else
                            {
                                dr[j] = sheet.Cells[rCnt, j].Value.ToString();
                            }
                        }
                        else
                        {
                            dr[j] = DBNull.Value;
                        }
                    }
                    ExcelData.Rows.Add(dr);
                }
            }
            ExcelData.AcceptChanges();
            sheet = null;
            book = null;
            return ExcelData;
        }

        /// <summary>
        /// 读取xls到 DataTable
        /// Coder:Alex
        /// </summary>
        /// <param name="sFilePath">xlsx完整路径</param>
        /// <returns></returns>
        public DataTable GetTableFromXlsx(string sFilePath)
        {
            DataTable ExcelData = new DataTable();

            using (ExcelPackage xlPackage = new ExcelPackage(new System.IO.FileInfo(sFilePath)))
            {
                try
                {
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];


                    //get the last row of the Excel Worksheet
                    int LastInt = GetLastRow(worksheet);
                    int LastColInt = GetLastCol(worksheet);


                    DataRow dr = null;
                    for (int rCnt = 1; rCnt <= LastInt; rCnt++)
                    {

                        if (rCnt == 1)
                        {
                            //遍历第一行，得到表的 Columns
                            for (int j = 1; j <= LastColInt; j++)
                            {
                                ExcelCell Cell = worksheet.Cell(rCnt, j);
                                if (!ExcelData.Columns.Contains(Cell.Value.ToString()))
                                {
                                    if (Cell.Value.ToString().Contains("DOB"))  //DateTime Columns
                                    {
                                        ExcelData.Columns.Add(Cell.Value.ToString().Trim(), typeof(DateTime));
                                    }
                                    else
                                    {
                                        ExcelData.Columns.Add(Cell.Value.ToString().Trim());
                                    }
                                }
                            }
                        }
                        else
                        {
                            dr = ExcelData.NewRow();
                            for (int j = 1; j <= LastColInt; j++)
                            {
                                ExcelCell Cell = worksheet.Cell(rCnt, j);
                                if (Cell.Value != null)
                                {
                                    if (ExcelData.Columns[j-1].Caption.Contains("DOB"))  //DateTime Columns
                                    {
                                        dr[j - 1] = Cell.Value.ToString();
                                    }
                                    else
                                    {
                                        dr[j - 1] = Cell.Value.ToString();
                                    }
                                }
                                else
                                {
                                    dr[j - 1] = DBNull.Value;
                                }
                            }
                            ExcelData.Rows.Add(dr);
                        }
                    }
                    ExcelData.AcceptChanges();
                    worksheet = null;

                }
                catch (Exception Exc)
                {
                    string msg = Exc.Message;
                    //Return any Exceptions when handling the worksheet.
                }
            }

            return ExcelData;
        }

        /// <summary>
        /// 读取csv到DataTable
        /// Coder: Alex
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        public DataTable GetTableFromCsv(string sFilePath)
        {
            DataTable dt = new DataTable();

            ArrayList rowAL = new ArrayList();        //行链表

            System.IO.StreamReader mysr = new System.IO.StreamReader(sFilePath, GetEncoding(sFilePath));
            try
            {
                int intColCount = 0;
                bool blnFlag = true;
                DataColumn dc;
                DataRow dr;
                string strline;

                string csvDataLine;
                csvDataLine = "";
                while ((strline = mysr.ReadLine()) != null)
                {

                    string fileDataLine;
                    fileDataLine = strline;

                    if (csvDataLine == "")
                    {
                        csvDataLine = fileDataLine;
                    }
                    else
                    {
                        csvDataLine += "\r\n" + fileDataLine;
                    }
                    //如果包含偶数个引号，说明该行数据中出现回车符或包含逗号
                    if (!IfOddQuota(csvDataLine))
                    {
                        AddNewDataLine(csvDataLine, rowAL);
                        csvDataLine = "";
                    }
                }

                for (int j = 0; j < rowAL.Count; j++)
                {
                    ArrayList aryline1 = (ArrayList)rowAL[j];
                    intColCount = aryline1.Count;
                    if (blnFlag)
                    {
                        blnFlag = false;
                        for (int i = 0; i < intColCount; i++)
                        {
                            dc = new DataColumn(aryline1[i].ToString().Trim());
                            dc.ColumnName = aryline1[i].ToString().Trim();
                            dt.Columns.Add(dc);
                        }
                    }
                    else
                    {
                        dr = dt.NewRow();
                        for (int i = 0; i < intColCount; i++)
                        {
                            dr[i] = aryline1[i];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                mysr.Close();
                mysr = null;
            }
            catch (Exception ex)
            {
                mysr.Close();
                mysr = null;
                return dt;
            }
            return dt;
        }
        /// <summary>
        /// 得到的xlsx的行数
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        private int GetLastRow(ExcelWorksheet worksheet)
        {
            System.Xml.XmlDocument Doc = worksheet.WorksheetXml;

            System.Xml.XmlNodeList xmlnode = Doc.GetElementsByTagName("row");
            int LastRow = 0;
            foreach (System.Xml.XmlNode node in xmlnode)
            {
                System.Xml.XmlAttributeCollection Atts = node.Attributes;
                foreach (System.Xml.XmlAttribute att in Atts)
                {
                    if (att.Name == "r")
                        LastRow = Int32.Parse(att.InnerText) > LastRow ? Int32.Parse(att.InnerText) : LastRow;
                }
            }
            return LastRow;
        }

        /// <summary>
        /// 得到的xlsx的列数
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        private int GetLastCol(ExcelWorksheet worksheet)
        {
            System.Xml.XmlDocument Doc = worksheet.WorksheetXml;

            System.Xml.XmlNodeList xmlnode = Doc.GetElementsByTagName("c");
            int LastRow = 0;
            foreach (System.Xml.XmlNode node in xmlnode)
            {
                System.Xml.XmlAttributeCollection Atts = node.Attributes;
                foreach (System.Xml.XmlAttribute att in Atts)
                {
                    if (att.Name == "colNumber")
                        LastRow = Int32.Parse(att.InnerText) > LastRow ? Int32.Parse(att.InnerText) : LastRow;
                }
            }
            return LastRow;
        }

        private DateTime GetDateTime(string sValue)
        {
            DateTime dtDate = new DateTime();

            try
            {
                double iValue = Convert.ToDouble(sValue);
                dtDate = Convert.ToDateTime("1900-1-1").AddDays(iValue-2);
            }
            catch (Exception ex )
            {
                
                return Convert.ToDateTime("1900-1-1");
            }

            return dtDate;
        }

        public static Encoding GetEncoding(string filename)
        {
            Encoding result = System.Text.Encoding.Default;
            FileStream file = null;
            try
            {
                file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);

                if (file.CanSeek)
                {
                    //   get   the   bom,   if   there   is   one   
                    byte[] bom = new byte[4];
                    file.Read(bom, 0, 4);

                    //   utf-8   
                    if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
                        result = System.Text.Encoding.UTF8;
                    //   ucs-2le,   ucs-4le,   ucs-16le,   utf-16,   ucs-2,   ucs-4   
                    else if ((bom[0] == 0xff && bom[1] == 0xfe) ||
                    (bom[0] == 0xfe && bom[1] == 0xff) ||
                    (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff))
                        result = System.Text.Encoding.Unicode;
                    //   else   ascii   
                    else
                        result = System.Text.Encoding.Default;
                }
                else
                {
                    //   can't   detect,   set   to   default   
                    result = System.Text.Encoding.Default;
                }

                return result;
            }
            finally
            {
                if (null != file) file.Close();
            }
        }

        /// <summary>
        /// 判断字符串是否包含奇数个引号
        /// </summary>
        /// <param name="dataLine">数据行</param>
        /// <returns>为奇数时，返回为真；否则返回为假</returns>
        private bool IfOddQuota(string dataLine)
        {
            int quotaCount;
            bool oddQuota;

            quotaCount = 0;
            for (int i = 0; i < dataLine.Length; i++)
            {
                if (dataLine[i] == '\"')
                {
                    quotaCount++;
                }
            }

            oddQuota = false;
            if (quotaCount % 2 == 1)
            {
                oddQuota = true;
            }

            return oddQuota;
        }

        /// <summary>
        /// 判断是否以奇数个引号开始
        /// </summary>
        /// <param name="dataCell"></param>
        /// <returns></returns>
        private bool IfOddStartQuota(string dataCell)
        {
            int quotaCount;
            bool oddQuota;

            quotaCount = 0;
            for (int i = 0; i < dataCell.Length; i++)
            {
                if (dataCell[i] == '\"')
                {
                    quotaCount++;
                }
                else
                {
                    break;
                }
            }

            oddQuota = false;
            if (quotaCount % 2 == 1)
            {
                oddQuota = true;
            }

            return oddQuota;
        }

        /// <summary>
        /// 判断是否以奇数个引号结尾
        /// </summary>
        /// <param name="dataCell"></param>
        /// <returns></returns>
        private bool IfOddEndQuota(string dataCell)
        {
            int quotaCount;
            bool oddQuota;

            quotaCount = 0;
            for (int i = dataCell.Length - 1; i >= 0; i--)
            {
                if (dataCell[i] == '\"')
                {
                    quotaCount++;
                }
                else
                {
                    break;
                }
            }

            oddQuota = false;
            if (quotaCount % 2 == 1)
            {
                oddQuota = true;
            }

            return oddQuota;
        }

        /// <summary>
        /// 加入新的数据行
        /// </summary>
        /// <param name="newDataLine">新的数据行</param>
        private void AddNewDataLine(string newDataLine, ArrayList rowAL)
        {
            ArrayList colAL = new ArrayList();
            string[] dataArray = newDataLine.Split(',');
            bool oddStartQuota;       //是否以奇数个引号开始

            string cellData;

            oddStartQuota = false;
            cellData = "";
            for (int i = 0; i < dataArray.Length; i++)
            {
                if (oddStartQuota)
                {
                    //因为前面用逗号分割,所以要加上逗号
                    cellData += "," + dataArray[i];
                    //是否以奇数个引号结尾
                    if (IfOddEndQuota(dataArray[i]))
                    {
                        colAL.Add(GetHandleData(cellData));
                        oddStartQuota = false;
                        continue;
                    }
                }
                else
                {
                    //是否以奇数个引号开始

                    if (IfOddStartQuota(dataArray[i]))
                    {
                        //是否以奇数个引号结尾,不能是一个双引号,并且不是奇数个引号
                        if (IfOddEndQuota(dataArray[i]) && dataArray[i].Length > 2 && !IfOddQuota(dataArray[i]))
                        {
                            colAL.Add(GetHandleData(dataArray[i]));
                            oddStartQuota = false;
                            continue;
                        }
                        else
                        {

                            oddStartQuota = true;
                            cellData = dataArray[i];
                            continue;
                        }
                    }
                    else
                    {
                        colAL.Add(GetHandleData(dataArray[i]));
                    }
                }
            }
            if (oddStartQuota)
            {
                throw new Exception("Data Error!");
            }
            rowAL.Add(colAL);
        }

        /// <summary>
        /// 去掉格子的首尾引号，把双引号变成单引号
        /// </summary>
        /// <param name="fileCellData"></param>
        /// <returns></returns>
        private string GetHandleData(string fileCellData)
        {
            if (fileCellData == "")
            {
                return "";
            }
            if (IfOddStartQuota(fileCellData))
            {
                if (IfOddEndQuota(fileCellData))
                {
                    return fileCellData.Substring(1, fileCellData.Length - 2).Replace("\"\"", "\""); //去掉首尾引号，然后把双引号变成单引号
                }
                else
                {
                    throw new Exception("Index of Data Error" + fileCellData);
                }
            }
            else
            {
                //考虑形如""    """"      """"""    
                if (fileCellData.Length > 2 && fileCellData[0] == '\"')
                {
                    fileCellData = fileCellData.Substring(1, fileCellData.Length - 2).Replace("\"\"", "\""); //去掉首尾引号，然后把双引号变成单引号
                }
            }

            return fileCellData;
        }

    }

    /// <summary>
    ///QueryStringType 的摘要说明
    /// </summary>
    public enum QueryStringType
    {
        ID,
        String,
        Date,
        Guid,
        IDs
    }
}