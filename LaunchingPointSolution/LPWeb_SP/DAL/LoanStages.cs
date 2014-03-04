using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LPWeb.DAL
{
    public class LoanStages : LoanStagesBase
    {
        /// <summary>
        /// Gets the loan stage setup info.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public DataSet GetLoanStageSetupInfo(int fileId)
        {
            if (fileId <1)
            {
                return null;
            }

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            strSql.Append(" * ");
            strSql.Append(" FROM lpvw_GetStageInfos ");
            strSql.Append(" where FileId=" + fileId);

            strSql.Append(" order by SequenceNumber ASC ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// Get the loan Stage Alias
        /// </summary>
        /// <param name="WorkflowType"></param>
        /// <returns></returns>
        public DataTable GetLoanStageAlias(string WorkflowType)
        {
            if (string.IsNullOrEmpty(WorkflowType))
            {
                return null;
            }

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select distinct ts.Alias as Stage,ts.TemplStageId,ts.SequenceNumber from LoanStages ls ");
            strSql.Append(" inner join Template_Wfl_Stages tw on ls.WflStageId=tw.WflStageId  ");
            strSql.Append(" inner join Template_Stages ts on tw.TemplStageId=ts.TemplStageId ");
            strSql.Append(" where WorkflowType='" + WorkflowType + "'");
            strSql.Append(" order by ts.SequenceNumber ASC ");

            return DbHelperSQL.ExecuteDataTable(strSql.ToString());
        }
    }
}
