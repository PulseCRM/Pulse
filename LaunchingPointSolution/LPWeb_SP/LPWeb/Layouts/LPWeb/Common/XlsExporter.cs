using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using org.in2bits.MyXls;
using System.IO;
using System.Collections.Specialized;
using System.Web.UI;

namespace LPWeb.Layouts.LPWeb.Common
{
    public class XlsExporter
    {
        public XlsExporter()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 导出Excel文件 输出浏览器下载
        /// neo 2010-12-06
        /// </summary>
        /// <param name="CurrentPage"></param>
        /// <param name="ExcelData"></param>
        /// <param name="GridView1"></param>
        /// <param name="sClientXlsFileName"></param>
        /// <param name="sSheetName"></param>
        public static void DownloadXls(Page CurrentPage, DataTable ExcelData, GridView GridView1, string sClientXlsFileName, string sSheetName) 
        {
            string sCurrentPagePath = CurrentPage.Server.MapPath("~/");
            string sExcelDir = Path.GetDirectoryName(sCurrentPagePath) + "\\TempXls\\";

            // 如果TempXls文件夹不存在，创建
            if (Directory.Exists(sExcelDir) == false) 
            {
                Directory.CreateDirectory(sExcelDir);
            }

            DownloadXls(CurrentPage, ExcelData, GridView1, sClientXlsFileName, sExcelDir, sSheetName);
        }

        /// <summary>
        /// 导出Excel文件 输出浏览器下载
        /// neo 2010-12-06
        /// </summary>
        /// <param name="CurrentPage"></param>
        /// <param name="ExcelData"></param>
        /// <param name="ExcelColumns"></param>
        /// <param name="sClientXlsFileName"></param>
        /// <param name="sSheetName"></param>
        public static void DownloadXls(Page CurrentPage, DataTable ExcelData, NameValueCollection ExcelColumns, string sClientXlsFileName, string sSheetName) 
        {
            string sCurrentPagePath = CurrentPage.Server.MapPath("~/");
            string sExcelDir = Path.GetDirectoryName(sCurrentPagePath) + "\\TempXls\\";

            // 如果TempXls文件夹不存在，创建
            if (Directory.Exists(sExcelDir) == false)
            {
                Directory.CreateDirectory(sExcelDir);
            }

            DownloadXls(CurrentPage, ExcelData, ExcelColumns, sClientXlsFileName, sExcelDir, sSheetName);
        }

        /// <summary>
        /// 导出Excel文件 输出浏览器下载
        /// neo 2010-12-06
        /// </summary>
        /// <param name="CurrentPage"></param>
        /// <param name="ExcelData"></param>
        /// <param name="sClientXlsFileName"></param>
        /// <param name="sSheetName"></param>
        public static void DownloadXls(Page CurrentPage, DataTable ExcelData, string sClientXlsFileName, string sSheetName) 
        {
            string sCurrentPagePath = CurrentPage.Server.MapPath("~/");
            string sExcelDir = Path.GetDirectoryName(sCurrentPagePath) + "\\TempXls\\";
          
            // 如果TempXls文件夹不存在，创建
            if (Directory.Exists(sExcelDir) == false)
            {
                Directory.CreateDirectory(sExcelDir);
            }

            DownloadXls(CurrentPage, ExcelData, sClientXlsFileName, sExcelDir, sSheetName);
        }

        /// <summary>
        /// 导出Excel文件 输出浏览器下载
        /// neo 2010-12-02
        /// </summary>
        /// <param name="ExcelData"></param>
        /// <param name="GridView1"></param>
        /// <param name="sClientXlsFileName"></param>
        /// <param name="sExcelDir"></param>
        /// <param name="sSheetName"></param>
        public static void DownloadXls(Page CurrentPage, DataTable ExcelData, GridView GridView1, string sClientXlsFileName, string sExcelDir, string sSheetName)
        {
            string sCurrentPagePath = CurrentPage.Server.MapPath("~/");
            string sXlsFilePath = SaveAsExcel(ExcelData, GridView1, sExcelDir, sSheetName);

            FileInfo FileInfo1 = new FileInfo(sXlsFilePath);
            CurrentPage.Response.Clear();
            CurrentPage.Response.ClearHeaders();
            CurrentPage.Response.Buffer = false;
            CurrentPage.Response.ContentType = "application/octet-stream";
            CurrentPage.Response.AppendHeader("Content-Disposition", "attachment;filename=" + sClientXlsFileName);
            CurrentPage.Response.AppendHeader("Content-Length", FileInfo1.Length.ToString());
            CurrentPage.Response.WriteFile(sXlsFilePath);
            CurrentPage.Response.Flush();

            // 删除临时文件
            File.Delete(sXlsFilePath);

            CurrentPage.Response.End();
        }

        /// <summary>
        /// 导出Excel文件 输出浏览器下载
        /// neo 2010-12-02
        /// </summary>
        /// <param name="ExcelData"></param>
        /// <param name="ExcelColumns"></param>
        /// <param name="sClientXlsFileName"></param>
        /// <param name="sExcelDir"></param>
        /// <param name="sSheetName"></param>
        public static void DownloadXls(Page CurrentPage, DataTable ExcelData, NameValueCollection ExcelColumns, string sClientXlsFileName, string sExcelDir, string sSheetName)
        {
            string sCurrentPagePath = CurrentPage.Server.MapPath("~/");
            string sXlsFilePath = SaveAsExcel(ExcelData, ExcelColumns, sExcelDir, sSheetName);

            FileInfo FileInfo1 = new FileInfo(sXlsFilePath);
            CurrentPage.Response.Clear();
            CurrentPage.Response.ClearHeaders();
            CurrentPage.Response.Buffer = false;
            CurrentPage.Response.ContentType = "application/octet-stream";
            CurrentPage.Response.AppendHeader("Content-Disposition", "attachment;filename=" + sClientXlsFileName);
            CurrentPage.Response.AppendHeader("Content-Length", FileInfo1.Length.ToString());
            CurrentPage.Response.WriteFile(sXlsFilePath);
            CurrentPage.Response.Flush();

            // 删除临时文件
            File.Delete(sXlsFilePath);

            CurrentPage.Response.End();
        }

        /// <summary>
        /// 导出Excel文件 输出浏览器下载
        /// neo 2010-12-02
        /// </summary>
        /// <param name="ExcelData"></param>
        /// <param name="sClientXlsFileName"></param>
        /// <param name="sExcelDir"></param>
        /// <param name="sSheetName"></param>
        public static void DownloadXls(Page CurrentPage, DataTable ExcelData, string sClientXlsFileName, string sExcelDir, string sSheetName)
        {
            string sCurrentPagePath = CurrentPage.Server.MapPath("~/");
            string sXlsFilePath = SaveAsExcel(ExcelData, sExcelDir, sSheetName);

            FileInfo FileInfo1 = new FileInfo(sXlsFilePath);
            CurrentPage.Response.Clear();
            CurrentPage.Response.ClearHeaders();
            CurrentPage.Response.Buffer = false;
            CurrentPage.Response.ContentType = "application/octet-stream";
            CurrentPage.Response.AppendHeader("Content-Disposition", "attachment;filename=" + sClientXlsFileName);
            CurrentPage.Response.AppendHeader("Content-Length", FileInfo1.Length.ToString());
            CurrentPage.Response.WriteFile(sXlsFilePath);
            CurrentPage.Response.Flush();

            // 删除临时文件
            File.Delete(sXlsFilePath);

            CurrentPage.Response.End();
        }

        /// <summary>
        /// 将DataTable导出为Xls文件
        /// neo 2010-12-02
        /// </summary>
        /// <param name="ExcelData"></param>
        /// <param name="GridView1"></param>
        /// <param name="sExcelDir"></param>
        /// <param name="sSheetName"></param>
        /// <returns>返回生成Excel文件本地路径</returns>
        public static string SaveAsExcel(DataTable ExcelData, GridView GridView1, string sExcelDir, string sSheetName)
        {
            if (ExcelData == null)
            {
                return string.Empty;
            }

            NameValueCollection ExcelColumns = new NameValueCollection();
            foreach (DataControlField GridColumn in GridView1.Columns)
            {
                // 隐藏列 不导出
                if (GridColumn.Visible == false)
                {
                    continue;
                }

                // 只导出BoundField列
                if (GridColumn.GetType().FullName != "System.Web.UI.WebControls.BoundField")
                {
                    continue;
                }

                // 如果没有列名 不导出
                if (GridColumn.HeaderText == string.Empty)
                {
                    continue;
                }

                BoundField BoundField1 = GridColumn as BoundField;

                ExcelColumns.Add(BoundField1.HeaderText, BoundField1.DataField);
            }

            // 生成随机唯一文件名
            string sExcelFileName = Guid.NewGuid().ToString() + ".xls";

            // 创建Excel文档
            XlsDocument ExcelDoc = new XlsDocument();
            ExcelDoc.FileName = sExcelFileName;

            // 添加Sheet
            Worksheet Sheet = ExcelDoc.Workbook.Worksheets.Add(sSheetName);
            //Sheet.Name = sSheetName;

            // 添加第一行列名
            ColumnInfo ColumnInfo1 = new ColumnInfo(ExcelDoc, Sheet);
            ColumnInfo1.ColumnIndexStart = 0;
            ColumnInfo1.ColumnIndexEnd = Convert.ToUInt16(ExcelColumns.Count - 1);
            ColumnInfo1.Width = 25 * 150;
            Sheet.AddColumnInfo(ColumnInfo1);

            // 生成第一行Excel列
            int i = 1;
            foreach (string sHeaderText in ExcelColumns.AllKeys)
            {
                Cell CellObj = Sheet.Cells.Add(1, i, sHeaderText);
                CellObj.Font.Weight = FontWeight.Bold;

                CellObj.Pattern = 1;
                CellObj.PatternColor = Colors.Silver;

                CellObj.UseBorder = true;
                CellObj.LeftLineStyle = 1;
                CellObj.LeftLineColor = Colors.Black;
                CellObj.RightLineStyle = 1;
                CellObj.RightLineColor = Colors.Black;
                CellObj.TopLineStyle = 1;
                CellObj.TopLineColor = Colors.Black;
                CellObj.TopLineStyle = 1;
                CellObj.BottomLineColor = Colors.Black;

                i++;
            }

            int j = 2;
            foreach (DataRow RowObj in ExcelData.Rows)
            {
                int m = 1;
                foreach (string sHeaderText in ExcelColumns.AllKeys)
                {
                    // 获取对应的DataField
                    string sDataField = ExcelColumns[sHeaderText];

                    object oValue = RowObj[sDataField];
                    if (oValue.GetType().FullName == "System.Byte")
                    {
                        oValue = Convert.ToInt16(oValue);
                    }
                    else if (oValue.GetType().FullName == "System.DBNull")
                    {
                        oValue = string.Empty;
                    }
                    else if (oValue.GetType().FullName == "System.String")
                    {
                        oValue = oValue.ToString();
                    }
                    else if (oValue.GetType().FullName == "System.DateTime")
                    {
                        oValue = Convert.ToDateTime(oValue).ToString();
                    }

                    Cell CellObj = Sheet.Cells.Add(j, m, oValue);
                    CellObj.UseBorder = true;
                    CellObj.LeftLineStyle = 1;
                    CellObj.LeftLineColor = Colors.Black;
                    CellObj.RightLineStyle = 1;
                    CellObj.RightLineColor = Colors.Black;
                    CellObj.TopLineStyle = 1;
                    CellObj.TopLineColor = Colors.Black;
                    CellObj.TopLineStyle = 1;
                    CellObj.BottomLineColor = Colors.Black;

                    m++;
                }

                j++;
            }

            string sPath = sExcelDir + sExcelFileName;
            if (File.Exists(sPath) == true)
            {
                File.Delete(sPath);
            }

            ExcelDoc.Save(sExcelDir, true);

            return sPath;
        }

        /// <summary>
        /// 将DataTable导出为Xls文件
        /// neo 2010-12-02
        /// </summary>
        /// <param name="ExcelData"></param>
        /// <param name="ExcelColumns">HeadText(key)→DataField(value), e.g. User Name→UserName</param>
        /// <param name="sExcelDir"></param>
        /// <param name="sSheetName"></param>
        /// <returns>返回生成Excel文件本地路径</returns>
        public static string SaveAsExcel(DataTable ExcelData, NameValueCollection ExcelColumns, string sExcelDir, string sSheetName)
        {
            if (ExcelData == null)
            {
                return string.Empty;
            }

            // 生成随机唯一文件名
            string sExcelFileName = Guid.NewGuid().ToString() + ".xls";

            // 创建Excel文档
            XlsDocument ExcelDoc = new XlsDocument();
            ExcelDoc.FileName = sExcelFileName;

            // 添加Sheet
            Worksheet Sheet = ExcelDoc.Workbook.Worksheets.Add(sSheetName);
            //Sheet.Name = sSheetName;

            // 添加第一行列名
            ColumnInfo ColumnInfo1 = new ColumnInfo(ExcelDoc, Sheet);
            ColumnInfo1.ColumnIndexStart = 0;
            ColumnInfo1.ColumnIndexEnd = Convert.ToUInt16(ExcelColumns.Count - 1);
            ColumnInfo1.Width = 25 * 150;
            Sheet.AddColumnInfo(ColumnInfo1);

            // 生成第一行Excel列
            int i = 1;
            foreach (string sHeaderText in ExcelColumns.AllKeys)
            {
                Cell CellObj = Sheet.Cells.Add(1, i, sHeaderText);
                CellObj.Font.Weight = FontWeight.Bold;

                CellObj.Pattern = 1;
                CellObj.PatternColor = Colors.Silver;

                CellObj.UseBorder = true;
                CellObj.LeftLineStyle = 1;
                CellObj.LeftLineColor = Colors.Black;
                CellObj.RightLineStyle = 1;
                CellObj.RightLineColor = Colors.Black;
                CellObj.TopLineStyle = 1;
                CellObj.TopLineColor = Colors.Black;
                CellObj.TopLineStyle = 1;
                CellObj.BottomLineColor = Colors.Black;

                i++;
            }

            int j = 2;
            foreach (DataRow RowObj in ExcelData.Rows)
            {
                int m = 1;
                foreach (string sHeaderText in ExcelColumns.AllKeys)
                {
                    // 获取对应的DataField
                    string sDataField = ExcelColumns[sHeaderText];

                    object oValue = RowObj[sDataField];
                    if (oValue.GetType().FullName == "System.Byte")
                    {
                        oValue = Convert.ToInt16(oValue);
                    }
                    else if (oValue.GetType().FullName == "System.DBNull")
                    {
                        oValue = string.Empty;
                    }
                    else if (oValue.GetType().FullName == "System.String")
                    {
                        oValue = oValue.ToString();
                    }
                    else if (oValue.GetType().FullName == "System.DateTime")
                    {
                        oValue = Convert.ToDateTime(oValue).ToString();
                    }

                    Cell CellObj = Sheet.Cells.Add(j, m, oValue);
                    CellObj.UseBorder = true;
                    CellObj.LeftLineStyle = 1;
                    CellObj.LeftLineColor = Colors.Black;
                    CellObj.RightLineStyle = 1;
                    CellObj.RightLineColor = Colors.Black;
                    CellObj.TopLineStyle = 1;
                    CellObj.TopLineColor = Colors.Black;
                    CellObj.TopLineStyle = 1;
                    CellObj.BottomLineColor = Colors.Black;

                    m++;
                }

                j++;
            }

            string sPath = sExcelDir + sExcelFileName;
            if (File.Exists(sPath) == true)
            {
                File.Delete(sPath);
            }

            ExcelDoc.Save(sExcelDir, true);

            return sPath;
        }

        /// < summary> 
        /// 保存Excel
        /// neo 2010-12-02
        /// < /summary> 
        /// < param name="ExcelData"> < /param> 
        /// < param name="sExcelFileName"> < /param> 
        /// < param name="sExcelDir"> < /param> 
        /// < param name="sSheetName"> < /param> 
        public static string SaveAsExcel(DataTable ExcelData, string sExcelDir, string sSheetName)
        {
            if (ExcelData == null)
            {
                return string.Empty;
            }

            // 生成随机唯一文件名
            string sExcelFileName = Guid.NewGuid().ToString() + ".xls";

            // 创建Excel文档
            XlsDocument ExcelDoc = new XlsDocument();
            ExcelDoc.FileName = sExcelFileName;

            // 添加Sheet
            Worksheet Sheet = ExcelDoc.Workbook.Worksheets.Add(sSheetName);
            //Sheet.Name = sSheetName;

            // 添加第一行列名
            ColumnInfo ColumnInfo1 = new ColumnInfo(ExcelDoc, Sheet);
            ColumnInfo1.ColumnIndexStart = 0;
            ColumnInfo1.ColumnIndexEnd = Convert.ToUInt16(ExcelData.Columns.Count - 1);
            ColumnInfo1.Width = 25 * 200;
            Sheet.AddColumnInfo(ColumnInfo1);

            int i = 1;
            foreach (DataColumn ColumnObj in ExcelData.Columns)
            {
                Cell CellObj = Sheet.Cells.Add(1, i, ColumnObj.ColumnName);
                CellObj.Font.Weight = FontWeight.Bold;

                CellObj.Pattern = 1;
                CellObj.PatternColor = Colors.Silver;

                CellObj.UseBorder = true;
                CellObj.LeftLineStyle = 1;
                CellObj.LeftLineColor = Colors.Black;
                CellObj.RightLineStyle = 1;
                CellObj.RightLineColor = Colors.Black;
                CellObj.TopLineStyle = 1;
                CellObj.TopLineColor = Colors.Black;
                CellObj.TopLineStyle = 1;
                CellObj.BottomLineColor = Colors.Black;

                i++;
            }

            int j = 2;
            foreach (DataRow RowObj in ExcelData.Rows)
            {
                int m = 1;
                foreach (DataColumn ColumnObj in ExcelData.Columns)
                {
                    object oValue = RowObj[ColumnObj.ColumnName];
                    if (oValue.GetType().FullName == "System.Byte")
                    {
                        oValue = Convert.ToInt16(oValue);
                    }
                    else if (oValue.GetType().FullName == "System.DBNull")
                    {
                        oValue = string.Empty;
                    }
                    else if (oValue.GetType().FullName == "System.String")
                    {
                        oValue = oValue.ToString();
                    }
                    else if (oValue.GetType().FullName == "System.DateTime")
                    {
                        oValue = Convert.ToDateTime(oValue).ToString();
                    }

                    Cell CellObj = Sheet.Cells.Add(j, m, oValue);
                    CellObj.UseBorder = true;
                    CellObj.LeftLineStyle = 1;
                    CellObj.LeftLineColor = Colors.Black;
                    CellObj.RightLineStyle = 1;
                    CellObj.RightLineColor = Colors.Black;
                    CellObj.TopLineStyle = 1;
                    CellObj.TopLineColor = Colors.Black;
                    CellObj.TopLineStyle = 1;
                    CellObj.BottomLineColor = Colors.Black;

                    m++;
                }

                j++;
            }

            string sPath = sExcelDir + sExcelFileName;
            if (File.Exists(sPath) == true)
            {
                File.Delete(sPath);
            }

            ExcelDoc.Save(sExcelDir, true);

            return sPath;
        }
    }
}
