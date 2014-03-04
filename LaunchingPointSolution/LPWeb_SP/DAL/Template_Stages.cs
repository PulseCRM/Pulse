using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Template_Stages。
	/// </summary>
    public class Template_Stages : Template_StagesBase
	{
		public Template_Stages()
		{}
        public DataSet GetStageList(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
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
            parameters[0].Value = "(select *,CASE Custom WHEN 1 THEN 'Custom' ELSE 'Standard' END as CustomName, WorkflowType AS WorkflowTypeName, CASE Enabled WHEN '0' THEN 'No' ELSE 'Yes' END AS EnabledName from Template_Stages) as t";
            parameters[1].Value = "* ";
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

        #region neo

        /// <summary>
        /// get stage template list by WorkflowType
        /// neo 2011-02-09
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetStageTemplateListBase(string sWhere)
        {
            string sSql = "select * from Template_Stages where 1=1 " + sWhere;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get worflow stage info
        /// neo 2011-02-16
        /// </summary>
        /// <param name="iWflStageID"></param>
        /// <returns></returns>
        public DataTable GetStageTemplateInfoBase(int iWflStageID)
        {
            string sSql = "select * from Template_Stages where WflStageId=" + iWflStageID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// Get max sequence number by workflow type
        /// Rocky 2011-2-18
        /// </summary>
        /// <param name="sType"></param>
        /// <returns></returns>
        public int GetMaxSequenceBase(string sType)
        {
            int iSequenceNumber = 0;

            string sSql = "SELECT MAX(SequenceNumber) FROM Template_Stages WHERE WorkflowType = '" + sType + "'";
            object objRe = DbHelperSQL.ExecuteScalar(sSql);
            if (objRe != null)
            {
                int.TryParse(objRe.ToString(), out iSequenceNumber);
            }
            return iSequenceNumber;
        }
        #endregion


        public void DisableStageTemplates(string StageIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_Stages set ");
            strSql.Append("Enabled=0 ");
            strSql.Append(" where TemplStageId in (");
            strSql.Append(StageIDs);
            strSql.Append(")");
            DbHelperSQL.ExecuteSql(strSql.ToString());
        }

        public void DeleteStageTemplates(string StageIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Template_Wfl_Stages SET TemplStageId = NULL");
            strSql.Append(" where TemplStageId in (");
            strSql.Append(StageIDs);
            strSql.Append(")");
            strSql.Append(";DELETE FROM Template_Stages ");
            strSql.Append(" where TemplStageId in (");
            strSql.Append(StageIDs);
            strSql.Append(")");
            DbHelperSQL.ExecuteSql(strSql.ToString());
        }
	}
}

