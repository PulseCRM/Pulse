using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LPWeb.Common
{
    public class SqlTextBuilder
    {
        #region 处理字符串参数中的特殊字符
        /// <summary>
        /// 替换更新语句中，参数值中的特殊符号
        /// 刘洋(2007-07-25)
        /// </summary>
        /// <param name="sParamValue"></param>
        /// <returns></returns>
        public static string ConvertUpdateValue(string sParamValue)
        {
            if (string.IsNullOrEmpty(sParamValue))
            {
                return string.Empty;
            }
            return sParamValue.Replace("'", "''");
        }

        /// <summary>
        /// 替换查询语句中，参数值中的特殊符号
        /// 刘洋(2007-07-25)
        /// </summary>
        /// <param name="sParamValue"></param>
        /// <returns></returns>
        public static string ConvertQueryValue(string sParamValue)
        {
            if (string.IsNullOrEmpty(sParamValue))
            {
                return string.Empty;
            }

            sParamValue = sParamValue.Replace("[", "[[]");
            //sParamValue = sParamValue.Replace("]", "[]]");
            sParamValue = sParamValue.Replace("%", "[%]");
            sParamValue = sParamValue.Replace("_", "[_]");
            sParamValue = sParamValue.Replace("^", "[^]");
            sParamValue = sParamValue.Replace("'", "''");

            return sParamValue;
        }
        #endregion

        #region 生成查询条件字符串

        /// <summary>
        /// 生成查询条件语句（文本类）
        /// 刘洋(2008-03-05)
        /// </summary>
        /// <param name="sFieldName">查询字段名</param>
        /// <param name="sFieldValue">
        /// 用户输入文本。
        /// 空字符串表示没有查询条件。
        /// ＜NULL＞表示该字段为NULL。
        /// </param>
        /// <param name="sCompareAction">比较操作符：LIKE或=</param>
        /// <returns></returns>
        public static string BuildTextSearchBlock(string sFieldName, string sFieldValue, string sCompareAction)
        {
            #region 校验参数
            if (sFieldName == null)
            {
                throw new ArgumentNullException("字段名不能为空。");
            }

            if (sFieldName.Trim() == string.Empty)
            {
                throw new ArgumentNullException("字段名不能为空。");
            }

            if (sFieldValue == null)
            {
                throw new ArgumentNullException("用户输入文本不能为空。");
            }

            string sFieldText = sFieldValue.Trim();
            if (sFieldText == string.Empty)
            {
                return string.Empty;
            }
            #endregion

            if (sFieldText == "<NULL>")
            {
                return " AND (" + sFieldName + " IS NULL)";
            }

            string sSqlBlock = string.Empty;

            if (sCompareAction.Trim() == "=")
            {
                sSqlBlock = " AND (" + sFieldName + " = '" + SqlTextBuilder.ConvertQueryValue(sFieldText) + "')";
            }
            else  // LIKE
            {
                sSqlBlock = " AND (" + sFieldName + " LIKE '%" + SqlTextBuilder.ConvertQueryValue(sFieldText) + "%')";
            }

            return sSqlBlock;
        }

        /// <summary>
        /// 生成日期查询条件
        /// 刘洋 2008-05-09
        /// </summary>
        /// <param name="sFiedName"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public static string BuildDateSearchCondition(string sFieldName, DateTime? FromDate, DateTime? ToDate)
        {
            #region 校验参数
            if (sFieldName == null)
            {
                //throw new ArgumentNullException("字段名不能为空。");
                return string.Empty;
            }

            if (sFieldName.Trim() == string.Empty)
            {
                //throw new ArgumentNullException("字段名不能为空。");
                return string.Empty;
            }

            if (FromDate == null && ToDate == null)
            {
                return string.Empty;
            }
            #endregion

            string sSqlBlock = string.Empty;
            if (FromDate != null && ToDate != null)    // 大于等于FromDate，小于等于ToDate
            {
                sSqlBlock = " AND (DATEDIFF(DAY, " + sFieldName + ", '" + Convert.ToDateTime(FromDate).ToShortDateString() + "')<=0 AND DATEDIFF(DAY, " + sFieldName + ", '" + Convert.ToDateTime(ToDate).ToShortDateString() + "')>=0)";
            }
            else if (FromDate != null && ToDate == null)    // 大于等于FromDate
            {
                sSqlBlock = " AND (DATEDIFF(DAY, " + sFieldName + ", '" + Convert.ToDateTime(FromDate).ToShortDateString() + "')<=0)";
            }
            else if (FromDate == null && ToDate != null)    // 小于等于ToDate
            {
                sSqlBlock = " AND (DATEDIFF(DAY, " + sFieldName + ", '" + Convert.ToDateTime(ToDate).ToShortDateString() + "')>=0)";
            }

            return sSqlBlock;
        }

        /// <summary>
        /// 生成日期查询条件
        /// 刘洋 2008-10-04
        /// </summary>
        /// <param name="sFieldName"></param>
        /// <param name="sFromDate"></param>
        /// <param name="sToDate"></param>
        /// <returns></returns>
        public static string BuildDateSearchCondition(string sFieldName, string sFromDate, string sToDate)
        {
            #region 校验参数
            if (sFieldName == null)
            {
                throw new ArgumentNullException("字段名不能为空。");
            }

            if (sFieldName.Trim() == string.Empty)
            {
                throw new ArgumentNullException("字段名不能为空。");
            }

            if (sFromDate == string.Empty && sToDate == string.Empty)
            {
                return string.Empty;
            }
            #endregion

            string sSqlBlock = string.Empty;
            if (sFromDate != null && sToDate != null)    // 大于等于FromDate，小于等于ToDate
            {
                sSqlBlock = " AND (DATEDIFF(DAY, " + sFieldName + ", '" + sFromDate + "')<=0 AND DATEDIFF(DAY, " + sFieldName + ", '" + sToDate + "')>=0)";
            }
            else if (sFromDate != null && sToDate == null)    // 大于等于FromDate
            {
                sSqlBlock = " AND (DATEDIFF(DAY, " + sFieldName + ", '" + sFromDate + "')<=0)";
            }
            else if (sFromDate == null && sToDate != null)    // 小于等于ToDate
            {
                sSqlBlock = " AND (DATEDIFF(DAY, " + sFieldName + ", '" + sToDate + "')>=0)";
            }

            return sSqlBlock;
        }

        /// <summary>
        /// 生成数字查询条件
        /// 刘洋 2008-05-09
        /// </summary>
        /// <param name="sFiedName"></param>
        /// <param name="dMinValue"></param>
        /// <param name="dMaxValue"></param>
        /// <returns></returns>
        public static string BuildNumericSearchCondition(string sFieldName, decimal? dMinValue, decimal? dMaxValue, bool bAllowZero)
        {
            #region 校验参数
            if (sFieldName == null)
            {
                throw new ArgumentNullException("字段名不能为空。");
            }

            if (sFieldName.Trim() == string.Empty)
            {
                throw new ArgumentNullException("字段名不能为空。");
            }

            if (bAllowZero == true)
            {
                if (dMinValue == null && dMaxValue == null)
                {
                    return string.Empty;
                }
            }
            else
            {
                if (dMinValue == 0 && dMaxValue == 0)
                {
                    return string.Empty;
                }
            }
            #endregion

            string sSqlBlock = string.Empty;
            if (bAllowZero == true)
            {
                if (dMinValue != null && dMaxValue != null)    // 大于等于MinValue，小于等于MaxValue
                {
                    sSqlBlock = " AND (" + sFieldName + " >= " + dMinValue + " AND " + sFieldName + " <= " + dMaxValue + ")";
                }
                else if (dMinValue != null && dMaxValue == null)    // 大于等于MinValue
                {
                    sSqlBlock = " AND (" + sFieldName + " >= " + dMinValue + ")";
                }
                else if (dMinValue == null && dMaxValue != null)    // 小于等于MaxValue
                {
                    sSqlBlock = " AND (" + sFieldName + " <= " + dMaxValue + ")";
                }
            }
            else
            {
                if (dMinValue != 0 && dMaxValue != 0)    // 大于等于MinValue，小于等于MaxValue
                {
                    sSqlBlock = " AND (" + sFieldName + " >= " + dMinValue + " AND " + sFieldName + " <= " + dMaxValue + ")";
                }
                else if (dMinValue != 0 && dMaxValue == 0)    // 大于等于MinValue
                {
                    sSqlBlock = " AND (" + sFieldName + " >= " + dMinValue + ")";
                }
                else if (dMinValue == 0 && dMaxValue != 0)    // 小于等于MaxValue
                {
                    sSqlBlock = " AND (" + sFieldName + " <= " + dMaxValue + ")";
                }
            }

            return sSqlBlock;
        }

        #endregion
    }
}