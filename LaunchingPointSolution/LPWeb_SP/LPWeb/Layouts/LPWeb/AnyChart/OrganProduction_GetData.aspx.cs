using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using System.Data;
using LPWeb.DAL;
using System.Text;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.AnyChart
{
    public partial class OrganProduction_GetData : BasePage
    {
        public string sOrganPoints = string.Empty;
        public int iOverlay = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 校验页面参数

            string sWhere = string.Empty;
            if (this.Request.QueryString["w"] != null) 
            {
                string sWhereEncode = this.Request.QueryString["w"].ToString();
                if(sWhereEncode != string.Empty)
                {
                    string sWhereDecode = Encrypter.Base64Decode(sWhereEncode);
                    if (sWhereEncode == sWhereDecode)
                    {
                        sWhere = "and (1=0)";
                    }
                    else
                    {
                        sWhere = sWhereDecode;
                    }
                }
            }

            string sOrgan = "Branch";
            if (this.Request.QueryString["Organ"] != null)
            {
                sOrgan = this.Request.QueryString["Organ"];
            }

            #endregion

            string sWorkflowType = "Processing";
            if (this.Request.QueryString["wt"] != null)
            {
                sWorkflowType = this.Request.QueryString["wt"];
            }

            #region get user loan list

            string sFromTable = string.Empty;

            if (sWorkflowType == "Prospect")
            {
                #region Prospect

                if (this.CurrUser.sRoleName == "Branch Manager")
                {
                    sFromTable = "(select b.* from lpfn_GetUserLoans_Branch_Manager(" + this.CurrUser.iUserID + ") as a inner join lpvw_PipelineInfo as b on a.LoanID = b.FileId "
                               + "where (1=1) " + sWhere + ") as t";
                }
                else
                {
                    if (this.CurrUser.sRoleName == "Executive")
                    {
                        sFromTable = "(select b.* from lpfn_GetUserLoans_Executive(" + this.CurrUser.iUserID + ") as a inner join lpvw_PipelineInfo as b on a.LoanID = b.FileId "
                                   + "where (1=1) " + sWhere + ") as t";
                    }
                    else
                    {
                        sFromTable = "(select b.* from lpfn_GetUserLoans(" + this.CurrUser.iUserID + ") as a inner join lpvw_PipelineInfo as b on a.LoanID = b.FileId "
                                   + "where (1=1) " + sWhere + ") as t";
                    }
                }

                #endregion
            }
            else
            {
                if (this.CurrUser.sRoleName == "Branch Manager")
                {
                    sFromTable = "(select b.* from lpfn_GetUserLoans_Branch_Manager(" + this.CurrUser.iUserID + ") as a inner join V_ProcessingPipelineInfo as b on a.LoanID = b.FileId "
                               + "where (1=1) " + sWhere + ") as t";
                }
                else
                {
                    if (this.CurrUser.sRoleName == "Executive")
                    {
                        sFromTable = "(select b.* from lpfn_GetUserLoans_Executive(" + this.CurrUser.iUserID + ") as a inner join V_ProcessingPipelineInfo as b on a.LoanID = b.FileId "
                                   + "where (1=1) " + sWhere + ") as t";
                    }
                    else
                    {
                        sFromTable = "select b.* from lpfn_GetUserLoans(" + this.CurrUser.iUserID + ") as a inner join V_ProcessingPipelineInfo as b on a.LoanID = b.FileId "
                                   + "where (1=1) " + sWhere + ") as t";
                    }
                }
            }

            #endregion

            #region 生成Xml for Organ. Production

            LPWeb.BLL.Loans LoanManager = new LPWeb.BLL.Loans();

            

            string sSql = string.Empty;

            if (sOrgan == "Regional")
            {
                sSql = "select t.RegionID, b.Name, SUM(t.Amount) as Amount from " + sFromTable + " inner join Regions as b on t.RegionID=b.RegionId group by t.RegionID, b.Name";
            }

            if (sOrgan == "Division")
            {
                sSql = "select t.DivisionID, b.Name, SUM(t.Amount) as Amount from " + sFromTable + " inner join Divisions as b on t.DivisionID=b.DivisionID group by t.DivisionID, b.Name";
            }

            if (sOrgan == "Branch")
            {
                sSql = "select t.BranchID, b.Name, SUM(t.Amount) as Amount from " + sFromTable + " inner join Branches as b on t.BranchID=b.BranchID group by t.BranchID, b.Name";
            }

            DataTable OrganProductionData = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

            StringBuilder sbOrganPoints = new StringBuilder();
            foreach (DataRow OrganRow in OrganProductionData.Rows) 
            {
                decimal dAmount = Convert.ToDecimal(OrganRow["Amount"]) / 1000;
                string sOrganName = OrganRow["Name"].ToString();

                sOrganName = this.WordWrap(sOrganName, 10);

                sbOrganPoints.AppendLine("<point name=\"" + sOrganName + "\" y=\"" + dAmount + "\"/>");
            }

            if(sbOrganPoints.Length > 0)
            {
                this.sOrganPoints = "<series>" + sbOrganPoints.ToString() + "</series>";
            }

            #endregion

            if(OrganProductionData.Rows.Count > 0)
            {
                DataView OrganView = new DataView(OrganProductionData);
                OrganView.Sort = "Amount desc";
                int iMax = Convert.ToInt32(Convert.ToDecimal(OrganView[0]["Amount"]) / 1000);
                string sMax = iMax.ToString();
                string sFirstNum = sMax.Substring(0, 1);
                int iCeilNum = Convert.ToInt32(sFirstNum) + 1;
                this.iOverlay = iCeilNum * Convert.ToInt32(Math.Pow(Convert.ToDouble(10), Convert.ToDouble(sMax.Length - 1)));
            }
        }

        /// <summary>
        /// Word wraps the given text to fit within the specified width.
        /// </summary>
        /// <param name="text">Text to be word wrapped</param>
        /// <param name="width">Width, in characters, to which the text
        /// should be word wrapped</param>
        /// <returns>The modified text</returns>
        public string WordWrap(string text, int width)
        {
            int pos, next;
            StringBuilder sb = new StringBuilder();

            // Lucidity check
            if (width < 1)
                return text;

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                int eol = text.IndexOf(Environment.NewLine, pos);
                if (eol == -1)
                    next = eol = text.Length;
                else
                    next = eol + Environment.NewLine.Length;

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;
                        if (len > width)
                            len = BreakLine(text, pos, width);
                        sb.Append(text, pos, len);
                        sb.Append(Environment.NewLine);

                        // Trim whitespace following break
                        pos += len;
                        while (pos < eol && Char.IsWhiteSpace(text[pos]))
                            pos++;
                    } while (eol > pos);
                }
                else sb.Append(Environment.NewLine); // Empty line
            }
            return sb.ToString();
        }

        /// <summary>
        /// Locates position to break the given line so as to avoid
        /// breaking words.
        /// </summary>
        /// <param name="text">String that contains line of text</param>
        /// <param name="pos">Index where line of text starts</param>
        /// <param name="max">Maximum line length</param>
        /// <returns>The modified line length</returns>
        private int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;

            // If no whitespace found, break at maximum length
            if (i < 0)
                return max;

            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;

            // Return length of text before whitespace
            return i + 1;
        }
    }
}