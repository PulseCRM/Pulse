using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Template_Wfl_Tasks。
	/// </summary>
    public class Template_Wfl_Tasks : Template_Wfl_TasksBase
	{
		public Template_Wfl_Tasks()
		{}

        public DataSet GetWorkflowStageTasks(int PageSize, int PageIndex, string orderName, string strWhere, out int count)
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
            parameters[0].Value = "lpvw_GetWorkflowTasks";
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = 0;
            parameters[6].Value = " 1=1 "+strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;

        }
        /// <summary>
        /// Get workflow template name by taskid
        /// </summary>
        /// <param name="iTaskID"></param>
        /// <returns></returns>
        public string GetTemplateNameByTaskID(int iTaskID)
        {
            string sRe = "";

            string sSQL = "SELECT Name FROM Template_Workflow WHERE WflTemplId = (SELECT WflTemplId FROM Template_Wfl_Stages WHERE WflStageId=(SELECT WflStageId FROM Template_Wfl_Tasks WHERE TemplTaskId =" + iTaskID.ToString() + "))";

            SqlDataReader drTmp = DbHelperSQL.ExecuteReader(sSQL);
            if (drTmp != null)
            {
                sRe = drTmp[0].ToString();
            }
            drTmp.Close();

            return sRe;
        }
        /// <summary>
        /// 
        /// </summary>
        public void EnabledTemplTasks(string TemplTaskIds, bool bEnabled)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_Wfl_Tasks set "); 
            strSql.Append("Enabled=@Enabled ");
            strSql.Append(" where TemplTaskId in( " + TemplTaskIds + " )");
            SqlParameter[] parameters = {  
					new SqlParameter("@Enabled", SqlDbType.Bit,1 )};
            parameters[0].Value = bEnabled; 

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 
        /// </summary>
        public void DeleteTasks(string TemplTaskIds )
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" update Template_Wfl_Tasks set ");
            strSql.Append("PrerequisiteTaskId=null ");
            strSql.Append(" where PrerequisiteTaskId in (" + TemplTaskIds + " ) ;");

            strSql.Append("delete from Template_Wfl_Tasks ");
            strSql.Append(" where TemplTaskId in ( " + TemplTaskIds + " )");


            SqlParameter[] parameters = { }; 

            //SqlParameter[] parameters = {
            //        new SqlParameter("@TemplTaskIds", SqlDbType.NChar) };
            //parameters[0].Value = TemplTaskIds; 
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
	}
}

