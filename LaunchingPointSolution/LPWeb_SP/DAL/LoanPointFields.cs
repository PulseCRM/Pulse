using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类LoanPointFields。
    /// </summary>
    public class LoanPointFields : LoanPointFieldsBase
    {
        public LoanPointFields()
        { }


        public DataSet GetProcessingList(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType, int userID, bool accessOtherLoans)
        {
            #region not use
            //string tempTable =
            //    string.Format(
            //            "  ( SELECT "
            //           + " max(Branch) as 'Branch' "
            //           + " ,max(case PointFieldId when 101 then [CurrentValue] else '' end ) 'LastName' "
            //           + "  ,max(case PointFieldId when 100 then [CurrentValue] else '' end ) 'FirstName' "
            //           + " ,max(case PointFieldId when 6001 then [CurrentValue] else '' end ) 'Lender' "
            //           + " ,max(case PointFieldId when 19 then [CurrentValue] else '' end ) 'LoanOriginator' "
            //           + " ,max(case PointFieldId when 11 then cast(replace([CurrentValue],',','') as numeric(9,3)) else 0.00 end ) 'LoanAmount' "
            //           + " ,max(case PointFieldId when 12 then cast(replace([CurrentValue],',','') as decimal) else 0.00 end ) 'NoteRate' "
            //           + " ,max(Status) as 'Status' "
            //           + " ,'' as 'StatusDate' "
            //           + " ,max(case PointFieldId when 18 then [CurrentValue] else '' end ) 'LoanProcessor' "
            //           + " ,max(case PointFieldId when 7403 then [CurrentValue] else '' end ) 'LoanProgram' "
            //           + " ,max(case PointFieldId when 1023 then [CurrentValue] else '' end ) 'LoanOriginationFee' "
            //           + " ,max(case PointFieldId when 2181 then [CurrentValue] else '' end ) 'GFEDate' "
            //           + " ,max(case PointFieldId when 540 then cast(replace([CurrentValue],',','') as decimal(6,2)) else 0.00 end ) 'LTVRatio' "
            //           + " ,max(case PointFieldId when 11839 then cast(replace([CurrentValue],',','') as decimal(6,2)) else 0.00 end ) 'NetAdjustedPrice' "
            //           + " ,max(case PointFieldId when 6061 then [CurrentValue] else '' end ) 'LockDate' "

            //           + " FROM  "
            //           + " ( "
            //           + "     SELECT f.*,v.Branch,v.[Status] FROM [LoanPointFields] f "

            //           + "     RIGHT JOIN  V_ProcessingPipelineInfo v "
            //           + "     ON v.FileID =f.FileID AND v.Status='Processing' and v.FileId IN (select LoanID from lpfn_GetUserLoans2({1},{2})) "

            //           + "     WHERE f.PointFieldId in (101,100,6001,19,11,12,18,7403,1023,2181,540,11839,6061) AND {0} "
            //           + " ) t GROUP BY  FileID ) p "
            //        , strWhere, userID, accessOtherLoans ? 1 : 0).Replace("'", "''"); 
            #endregion

            string tempTable = string.Format("lpfn_GetProcessingList({0},{1})", userID, accessOtherLoans ? "1" : "0");

            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        /// <summary>
        /// get LoanPointFields.CurrentValue
        /// neo 2012-12-17
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="iPointFieldId"></param>
        /// <returns></returns>
        public DataTable GetPointFieldInfo(int iFileId, int iPointFieldId)
        {
            string sSql = "select * from LoanPointFields where FileId=" + iFileId + " and PointFieldId=" + iPointFieldId;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        //public Model.LoanPointFields GetModel(int iFileId, int iPointFieldId)
        //{
        //    Model.LoanPointFields pointField = null;
        //    string sSql = "select * from LoanPointFields where FileId=" + iFileId + " and PointFieldId=" + iPointFieldId;
        //    DataTable dt = DbHelperSQL.ExecuteDataTable(sSql);
        //    if (dt == null || dt.Rows.Count <= 0)
        //        return pointField;
        //    DataRow dr = dt.Rows[0];
        //    if (dr == null)
        //        return pointField;
        //    pointField = new Model.LoanPointFields() {
        //        FileId = dr["FileId"] == DBNull.Value ? 0 : (int)dr["FileId"],
        //        PointFieldId = dr["PointFieldId"] == DBNull.Value ? 0 : (int)dr["PointFieldId"],
        //        CurrentValue = dr["CurrentValue"] == DBNull.Value ? string.Empty : dr["CurrentValue"].ToString(),
        //        PrevValue = dr["PrevValue"] == DBNull.Value ? string.Empty : dr["PrevValue"].ToString(),
        //    };
        //    if (pointField.FileId == 0 || pointField.PointFieldId == 0)
        //        return null;
        //    return pointField;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iFileId"></param>
        /// <param name="iPointFieldId"></param>
        /// <param name="sCurrentValue"></param>
        public void UpdatePointFieldValue(int iFileId, int iPointFieldId, string sCurrentValue)
        {
            string sSql = "update LoanPointFields set CurrentValue=@CurrentValue where FileId=" + iFileId + " and PointFieldId=" + iPointFieldId;
            SqlCommand SqlCmd = new SqlCommand(sSql);
            if (sCurrentValue == null)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@CurrentValue", SqlDbType.NVarChar, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@CurrentValue", SqlDbType.NVarChar, sCurrentValue);
            }
            DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }
        public void DeletePointFields(int iFileid, string sWhere)
        {
            if (iFileid <= 0 || string.IsNullOrEmpty(sWhere))
                return;
            string sSql = string.Format("Delete LoanPointFields where Fileid={0} AND {1}", iFileid, sWhere);
            DbHelperSQL.ExecuteNonQuery(sSql);
        }
    }
}

