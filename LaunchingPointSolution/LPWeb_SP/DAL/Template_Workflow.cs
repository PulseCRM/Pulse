using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Template_Workflow。
    /// </summary>
    public class Template_Workflow : Template_WorkflowBase
    {
        public Template_Workflow()
        { }

        public DataSet GetTemplateWorkflows(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
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
            parameters[0].Value = "Template_Workflow";
            parameters[1].Value = "*,0 as Stages";
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

        public void DisableWorkflowTemplates(string TWFIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_Workflow set ");
            strSql.Append("Enabled=0 ");
            strSql.Append(" where WflTemplId in (");
            strSql.Append(TWFIDs);
            strSql.Append(")");
            DbHelperSQL.ExecuteSql(strSql.ToString());
        }

        public void EnableWorkflowTemplates(string TWFIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_Workflow set ");
            strSql.Append("Enabled=1 ");
            strSql.Append(" where WflTemplId in (");
            strSql.Append(TWFIDs);
            strSql.Append(")");
            DbHelperSQL.ExecuteSql(strSql.ToString());
        }

        public DataSet GetTemplateWorkflowTasks(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
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
            parameters[0].Value = "TemplateWorkflowTasks";
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
        ///  增加一条数据
        /// </summary>
        public int WorkflowTemplateAdd(LPWeb.Model.Template_Workflow model)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
					new SqlParameter("@WflTemplId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500),
					new SqlParameter("@DaysFromEstClose", SqlDbType.Int,4),
					new SqlParameter("@StageID", SqlDbType.Int,4)
                                        };
            if (model.WflTemplId == 0)
            {
                parameters[0].Direction = ParameterDirection.Output;
            }
            else
            {
                parameters[0].Value = model.WflTemplId;
            }
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Enabled;
            parameters[3].Value = model.Desc;
            parameters[4].Value = model.DaysFromEstClose;
            parameters[5].Value = model.StageId;

            DbHelperSQL.RunProcedure("lpsp_Template_Workflow_ADD", parameters, out rowsAffected);
            return (int)parameters[0].Value;
        }

        /// <summary>
        ///  CLONE
        /// </summary>
        public int WorkflowTemplateClone(int OldWflTemplId, string TemplateName)
        {
            int rowsAffected;
            int workflowTemplId = 0;
            SqlParameter[] parameters = {
					new SqlParameter("@WflTemplId", SqlDbType.Int,4), 
					new SqlParameter("@OldWflTemplId", SqlDbType.Int,4)
//					new SqlParameter("@name", SqlDbType.NVarChar,50)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = OldWflTemplId;
            //parameters[2].Value = TemplateName;

            DbHelperSQL.RunProcedure("[lpsp_CloneWorkflowTempl]", parameters, out rowsAffected);
            workflowTemplId = (int)parameters[0].Value;
            if (workflowTemplId > 0)
            {
                string SqlCmd = string.Format("Update Template_Workflow set [Name]='{0}' where WflTemplId={1}", TemplateName, workflowTemplId);
                DbHelperSQL.ExecuteSql(SqlCmd);
            }
            return workflowTemplId;
        }

        public void WorkflowTemplateDelete(string WflTemplIds)
        {
            //int rowsAffected;
            //SqlParameter[] parameters = {
            //        new SqlParameter("@WflTemplIds", SqlDbType.NVarChar,2000)
            //                            };
            //parameters[0].Value = WflTemplIds;

            //DbHelperSQL.RunProcedure("lpsp_DeleteWorkflowTemplate", parameters, out rowsAffected);
        }
        public void WorkflowTemplateDelete(int WflTemplId)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
					new SqlParameter("@WflTemplId", SqlDbType.Int)
                                        };
            parameters[0].Value = WflTemplId;

            DbHelperSQL.RunProcedure("lpsp_DeleteWorkflowTemplate", parameters, out rowsAffected);
        }
        #region neo

        /// <summary>
        /// insert workflow template
        /// neo 2011-02-08
        /// </summary>
        /// <param name="sWorkflowTemplateName"></param>
        /// <param name="bEnabled"></param>
        /// <param name="sDesc"></param>
        /// <param name="sWorkflowType"></param>
        /// <param name="bDefault"></param>
        /// <param name="sCalculationMethod"></param>
        /// <param name="StageList"></param>
        public void InsertWorkflowTemplateBase(string sWorkflowTemplateName, bool bEnabled, string sDesc, string sWorkflowType, bool bDefault, string sCalculationMethod, DataTable StageList)
        {
            #region build sql command - insert Template_Workflow

            string sSql = "INSERT INTO Template_Workflow (Name,Enabled,[Desc],WorkflowType,Custom,[Default],CalculationMethod) VALUES (@Name,@Enabled,@Desc,@WorkflowType,@Custom,@Default,@CalculationMethod);"
                        + "select SCOPE_IDENTITY();";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sWorkflowTemplateName);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, bEnabled);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Desc", SqlDbType.NVarChar, sDesc);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@WorkflowType", SqlDbType.NVarChar, sWorkflowType);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Custom", SqlDbType.Bit, true);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Default", SqlDbType.Bit, 0);

            Int16 iCalculationMethod = 0;
            if (sCalculationMethod == "Est Close Date")
            {
                iCalculationMethod = 1;
            }
            else if (sCalculationMethod == "Creation Date")
            {
                iCalculationMethod = 2;
            }
            else
            {
                iCalculationMethod = 3;
            }


            DbHelperSQL.AddSqlParameter(SqlCmd, "@CalculationMethod", SqlDbType.SmallInt, iCalculationMethod);

            #endregion

            #region build sql command - set defaul

            string sSql2 = "update Template_Workflow set [Default]=0 where WorkflowType=@WorkflowType;"
                         + "update Template_Workflow set [Default]=1 where WflTemplId=@WflTemplId";
            SqlCommand SqlCmd2 = new SqlCommand(sSql2);

            DbHelperSQL.AddSqlParameter(SqlCmd2, "@WorkflowType", SqlDbType.NVarChar, sWorkflowType);

            #endregion

            #region build sql command - Template_Wfl_Stages

            string sSql3 = "INSERT INTO Template_Wfl_Stages (WflTemplId,SequenceNumber,[Enabled],DaysFromEstClose,Name,DaysFromCreation,TemplStageId,CalculationMethod) VALUES (@WflTemplId,@SequenceNumber,@Enabled,@DaysFromEstClose,@Name,@DaysFromCreation,@TemplStageId, @CalculationMethod)";
            SqlCommand SqlCmd3 = new SqlCommand(sSql3);

            DbHelperSQL.AddSqlParameter1(SqlCmd3, "@WflTemplId", SqlDbType.Int, "WflTemplId");
            DbHelperSQL.AddSqlParameter1(SqlCmd3, "@SequenceNumber", SqlDbType.SmallInt, "SequenceNumber");
            DbHelperSQL.AddSqlParameter1(SqlCmd3, "@Enabled", SqlDbType.Bit, "Enabled");
            DbHelperSQL.AddSqlParameter1(SqlCmd3, "@DaysFromEstClose", SqlDbType.SmallInt, "DaysFromEstClose");
            DbHelperSQL.AddSqlParameter1(SqlCmd3, "@Name", SqlDbType.NVarChar, "Name");
            DbHelperSQL.AddSqlParameter1(SqlCmd3, "@DaysFromCreation", SqlDbType.SmallInt, "DaysFromCreation");
            DbHelperSQL.AddSqlParameter1(SqlCmd3, "@TemplStageId", SqlDbType.Int, "TemplStageId");
            DbHelperSQL.AddSqlParameter1(SqlCmd3, "@CalculationMethod", SqlDbType.SmallInt, "CalculationMethod");

            #endregion

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                int iNewID = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd, SqlTrans));

                #region update new stage id to Template_Wfl_Stages

                if (StageList.Rows.Count > 0)
                {
                    foreach (DataRow StageRow in StageList.Rows)
                    {
                        StageRow["WflTemplId"] = iNewID;
                    }
                    // update data table
                    DbHelperSQL.UpdateDataTable(StageList, SqlCmd3, null, null, SqlTrans);
                }

                #endregion

                SqlTrans.Commit();

                // is default
                if (bDefault == true)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd2, "@WflTemplId", SqlDbType.Int, iNewID);
                    DbHelperSQL.ExecuteNonQuery(SqlCmd2);
                }
            }
            catch (Exception ex)
            {
                SqlTrans.Rollback();
                throw ex;
            }
            finally
            {
                if (SqlConn != null)
                {
                    SqlConn.Close();
                }
            }

            #endregion

        }

        /// <summary>
        /// update workflow template
        /// neo 2011-02-11
        /// </summary>
        /// <param name="iWorkflowTemplateID"></param>
        /// <param name="sWorkflowTemplateName"></param>
        /// <param name="bEnabled"></param>
        /// <param name="sDesc"></param>
        /// <param name="sWorkflowType"></param>
        /// <param name="bDefault"></param>
        /// <param name="sCalculationMethod"></param>
        /// <param name="StageList"></param>
        /// <param name="sRemovedStageIDs"></param>
        public void UpdateWorkflowTemplateBase(int iWorkflowTemplateID, string sWorkflowTemplateName, bool bEnabled, string sDesc, string sWorkflowType, bool bDefault, string sCalculationMethod, DataTable StageList, string sRemovedStageIDs)
        {
            #region build sql command - insert Template_Workflow

            string sSql = "UPDATE Template_Workflow SET Name = @Name,[Enabled] = @Enabled,[Desc] = @Desc,WorkflowType = @WorkflowType,Custom = @Custom,[Default] = @Default,CalculationMethod = @CalculationMethod WHERE WflTemplId=@WflTemplId";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sWorkflowTemplateName);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, bEnabled);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Desc", SqlDbType.NVarChar, sDesc);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@WorkflowType", SqlDbType.NVarChar, sWorkflowType);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Custom", SqlDbType.Bit, true);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Default", SqlDbType.Bit, 0);

            Int16 iCalculationMethod = 0;
            //if (sCalculationMethod == "Est Close Date")
            //{
            //    iCalculationMethod = 1;
            //}
            //else
            //{
            //    iCalculationMethod = 2;
            //}

            if (sCalculationMethod == "Est Close Date")
            {
                iCalculationMethod = 1;
            }
            else if (sCalculationMethod == "Creation Date")
            {
                iCalculationMethod = 2;
            }
            else
            {
                iCalculationMethod = 3;
            }
            DbHelperSQL.AddSqlParameter(SqlCmd, "@CalculationMethod", SqlDbType.SmallInt, iCalculationMethod);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@WflTemplId", SqlDbType.Int, iWorkflowTemplateID);

            #endregion

            #region build sql command - set defaul

            string sSql2 = "update Template_Workflow set [Default]=0 where WorkflowType=@WorkflowType;"
                         + "update Template_Workflow set [Default]=1 where WflTemplId=@WflTemplId";
            SqlCommand SqlCmd2 = new SqlCommand(sSql2);

            DbHelperSQL.AddSqlParameter(SqlCmd2, "@WorkflowType", SqlDbType.NVarChar, sWorkflowType);
            DbHelperSQL.AddSqlParameter(SqlCmd2, "@WflTemplId", SqlDbType.Int, iWorkflowTemplateID);

            #endregion

            #region build sql command - insert Template_Wfl_Stages

            string sSql3 = "INSERT INTO Template_Wfl_Stages (WflTemplId,SequenceNumber,[Enabled],DaysFromEstClose,Name,DaysFromCreation,TemplStageId, CalculationMethod) VALUES (@WflTemplId,@SequenceNumber,@Enabled,@DaysFromEstClose,@Name,@DaysFromCreation,@TemplStageId, @CalculationMethod)";
            SqlCommand SqlCmd3 = new SqlCommand(sSql3);

            DbHelperSQL.AddSqlParameter1(SqlCmd3, "@WflTemplId", SqlDbType.Int, "WflTemplId");
            DbHelperSQL.AddSqlParameter1(SqlCmd3, "@SequenceNumber", SqlDbType.SmallInt, "SequenceNumber");
            DbHelperSQL.AddSqlParameter1(SqlCmd3, "@Enabled", SqlDbType.Bit, "Enabled");
            DbHelperSQL.AddSqlParameter1(SqlCmd3, "@DaysFromEstClose", SqlDbType.SmallInt, "DaysFromEstClose");
            DbHelperSQL.AddSqlParameter1(SqlCmd3, "@Name", SqlDbType.NVarChar, "Name");
            DbHelperSQL.AddSqlParameter1(SqlCmd3, "@DaysFromCreation", SqlDbType.SmallInt, "DaysFromCreation");
            DbHelperSQL.AddSqlParameter1(SqlCmd3, "@TemplStageId", SqlDbType.Int, "TemplStageId");
            DbHelperSQL.AddSqlParameter1(SqlCmd3, "@CalculationMethod", SqlDbType.SmallInt, "CalculationMethod");

            #endregion

            #region build sql command - update Template_Wfl_Stages

            string sSql4 = "UPDATE Template_Wfl_Stages SET SequenceNumber = @SequenceNumber,Enabled = @Enabled,DaysFromEstClose = @DaysFromEstClose,Name = @Name,DaysFromCreation = @DaysFromCreation,TemplStageId = @TemplStageId WHERE WflStageId=@WflStageId";
            SqlCommand SqlCmd4 = new SqlCommand(sSql4);

            DbHelperSQL.AddSqlParameter1(SqlCmd4, "@SequenceNumber", SqlDbType.SmallInt, "SequenceNumber");
            DbHelperSQL.AddSqlParameter1(SqlCmd4, "@Enabled", SqlDbType.Bit, "Enabled");
            DbHelperSQL.AddSqlParameter1(SqlCmd4, "@DaysFromEstClose", SqlDbType.SmallInt, "DaysFromEstClose");
            DbHelperSQL.AddSqlParameter1(SqlCmd4, "@Name", SqlDbType.NVarChar, "Name");
            DbHelperSQL.AddSqlParameter1(SqlCmd4, "@DaysFromCreation", SqlDbType.SmallInt, "DaysFromCreation");
            DbHelperSQL.AddSqlParameter1(SqlCmd4, "@TemplStageId", SqlDbType.Int, "TemplStageId");
            DbHelperSQL.AddSqlParameter1(SqlCmd4, "@WflStageId", SqlDbType.Int, "WflStageId");

            #endregion

            #region build sql command - delete Template_Wfl_Stages

            Collection<SqlCommand> DeleteSqlCmds = new Collection<SqlCommand>();
            if (sRemovedStageIDs != string.Empty)
            {
                string[] RemovedStageIDArray = sRemovedStageIDs.Split(',');
                for (int i = 0; i < RemovedStageIDArray.Length; i++)
                {
                    string sRemovedStageID = RemovedStageIDArray[i];
                    int iRemovedStageID = Convert.ToInt32(sRemovedStageID);

                    string sSql5 = "update LoanStages set WflStageId=null where WflStageId=@WflStageId;";
                    SqlCommand SqlCmd5 = new SqlCommand(sSql5);
                    DbHelperSQL.AddSqlParameter(SqlCmd5, "@WflStageId", SqlDbType.Int, iRemovedStageID);

                    // delete Template_Wfl_Tasks
                    string sSql51 = "delete from Template_Wfl_CompletionEmails WHERE TemplTaskid IN ( select TemplTaskId from Template_Wfl_Tasks where WflStageId=@WflStageId )";
                    SqlCommand SqlCmd51 = new SqlCommand(sSql51);
                    DbHelperSQL.AddSqlParameter(SqlCmd51, "@WflStageId", SqlDbType.Int, iRemovedStageID);

                    // delete Template_Wfl_Tasks
                    string sSql6 = "delete from Template_Wfl_Tasks where WflStageId=@WflStageId";
                    SqlCommand SqlCmd6 = new SqlCommand(sSql6);
                    DbHelperSQL.AddSqlParameter(SqlCmd6, "@WflStageId", SqlDbType.Int, iRemovedStageID);

                    // delete Template_Wfl_Stages
                    string sSql7 = "delete from Template_Wfl_Stages where WflStageId=@WflStageId";
                    SqlCommand SqlCmd7 = new SqlCommand(sSql7);
                    DbHelperSQL.AddSqlParameter(SqlCmd7, "@WflStageId", SqlDbType.Int, iRemovedStageID);

                    DeleteSqlCmds.Add(SqlCmd5);
                    DeleteSqlCmds.Add(SqlCmd51);
                    DeleteSqlCmds.Add(SqlCmd6);
                    DeleteSqlCmds.Add(SqlCmd7);
                }
            }

            #endregion

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                DbHelperSQL.ExecuteScalar(SqlCmd, SqlTrans);

                // update data table
                DbHelperSQL.UpdateDataTable(StageList, SqlCmd3, SqlCmd4, null, SqlTrans);

                foreach (SqlCommand DeleteSqlCmd in DeleteSqlCmds)
                {
                    DbHelperSQL.ExecuteScalar(DeleteSqlCmd, SqlTrans);
                }

                SqlTrans.Commit();

                // is default
                if (bDefault == true)
                {
                    DbHelperSQL.ExecuteNonQuery(SqlCmd2);
                }
            }
            catch (Exception ex)
            {
                SqlTrans.Rollback();
                throw ex;
            }
            finally
            {
                if (SqlConn != null)
                {
                    SqlConn.Close();
                }
            }

            #endregion

        }

        /// <summary>
        /// 检查workflow template name是否存在
        /// neo 2011-02-08
        /// </summary>
        /// <param name="sRuleName"></param>
        /// <returns></returns>
        public bool IsExist_CreateBase(string sWorkflowTemplateNameName)
        {
            string sSql = "select count(1) from Template_Workflow where Name = @Name";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sWorkflowTemplateNameName);
            int iCount = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd));
            if (iCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查workflow template是否存在
        /// neo 2011-02-08
        /// </summary>
        /// <param name="iRuleID"></param>
        /// <param name="sRuleName"></param>
        /// <returns></returns>
        public bool IsExist_EditBase(int iWorkflowTemplateID, string sWorkflowTemplateNameName)
        {
            string sSql = "select count(1) from Template_Workflow where Name = @Name and WflTemplId != " + iWorkflowTemplateID;
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sWorkflowTemplateNameName);
            int iCount = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd));
            if (iCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// get default workflow template count
        /// neo 2011-02-08
        /// </summary>
        /// <param name="sWorkflowType"></param>
        /// <returns></returns>
        public int GetDefaultWflTemplateCountBase(string sWorkflowType)
        {
            string sSql = "select count(1) from Template_Workflow where WorkflowType=@WorkflowType and [Default]=1";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@WorkflowType", SqlDbType.NVarChar, sWorkflowType);
            int iCount = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd));
            return iCount;
        }

        /// <summary>
        /// get workflow stage list(Template_Wfl_Stages)
        /// neo 2011-02-09
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetWflStageListBase(string sWhere)
        {
            string sSql = "select * from Template_Wfl_Stages where 1=1 " + sWhere;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get workflow template info
        /// neo 2011-02-09
        /// </summary>
        /// <param name="iWorkflowTemplateID"></param>
        /// <returns></returns>
        public DataTable GetWorkflowTemplateInfoBase(int iWorkflowTemplateID)
        {
            string sSql = "select * from Template_Workflow where WflTemplId=" + iWorkflowTemplateID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get stage list of workflow tempalte (Template_Wfl_Stages)
        /// neo 2011-02-09
        /// </summary>
        /// <param name="iWorkflowTemplateID"></param>
        /// <returns></returns>
        public DataTable GetWflStageListBase(int iWorkflowTemplateID)
        {
            string sSql = "select ROW_NUMBER() OVER(ORDER BY a.SequenceNumber) AS RowIndex, *, (select COUNT(1) from Template_Wfl_Tasks where WflStageId=a.WflStageId) as TaskCount "
                        + ", CASE ISNULL(a.CalculationMethod,0) WHEN 0 THEN c.CalculationMethod ELSE a.CalculationMethod END AS CalculationMethodCode"
                        + " from Template_Wfl_Stages as a "
                        + " inner join Template_Stages as b on a.TemplStageId = b.TemplStageId "
                        + " inner join Template_Workflow as c on a.WflTemplId = c.WflTemplId"
                        + " where a.WflTemplId=" + iWorkflowTemplateID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get stage info of workflow tempalte (Template_Wfl_Stages)
        /// neo 2011-02-16
        /// </summary>
        /// <param name="iWflStageID"></param>
        /// <returns></returns>
        public DataTable GetWflStageInfoBase(int iWflStageID)
        {
            string sSql = "select * from Template_Wfl_Stages where WflStageId=" + iWflStageID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// delete workflow template
        /// neo 2011-02-11
        /// </summary>
        /// <param name="iWorkflowTemplateID"></param>
        public void DeleteWorkflowTemplateBase(int iWorkflowTemplateID)
        {
            string sSql = "update LoanTasks set WflTemplId=null, TemplTaskId=null where WflTemplId=@WflTemplId "
                        + "update LoanStages set WflTemplId=null,WflStageId=null where WflTemplId=@WflTemplId "
                        + "delete from LoanWflTempl where WflTemplId=@WflTemplId "
                        + "delete from Template_Wfl_CompletionEmails where TemplTaskId in (select TemplTaskId from Template_Wfl_Tasks tt inner join Template_Wfl_Stages ts on tt.WflStageId=ts.WflStageId where ts.WflTemplId=@WflTemplId) "
                        + "delete from Template_Wfl_Tasks where WflStageId in (select WflStageId from Template_Wfl_Stages where WflTemplId=@WflTemplId) "
                        + "delete from Template_Wfl_Stages where WflTemplId=@WflTemplId "
                        + "delete from Template_Workflow where WflTemplId=@WflTemplId";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@WflTemplId", SqlDbType.Int, iWorkflowTemplateID);
            DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        /// <summary>
        /// update workflow stage
        /// neo 2011-02-16
        /// </summary>
        /// <param name="iWflStageID"></param>
        /// <param name="sWflStageName"></param>
        /// <param name="iSeq"></param>
        /// <param name="bEnabled"></param>
        /// <param name="iDaysFromEstClose"></param>
        /// <param name="iDaysFromCreation"></param>
        public void UpdateWflStageBase(int iWflStageID, Int16 iSeq, bool bEnabled, Int16 iDaysFromEstClose, Int16 iDaysFromCreation, Int16 iCalcMethod)
        {
            string sSql4 = "UPDATE Template_Wfl_Stages SET SequenceNumber = @SequenceNumber,Enabled = @Enabled,DaysFromEstClose = @DaysFromEstClose,DaysFromCreation = @DaysFromCreation, CalculationMethod=@CalculationMethod WHERE WflStageId=@WflStageId";
            SqlCommand SqlCmd4 = new SqlCommand(sSql4);

            DbHelperSQL.AddSqlParameter(SqlCmd4, "@SequenceNumber", SqlDbType.SmallInt, iSeq);
            DbHelperSQL.AddSqlParameter(SqlCmd4, "@Enabled", SqlDbType.Bit, bEnabled);

            if (iDaysFromEstClose == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd4, "@DaysFromEstClose", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd4, "@DaysFromEstClose", SqlDbType.SmallInt, iDaysFromEstClose);
            }

            if (iDaysFromCreation == 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd4, "@DaysFromCreation", SqlDbType.SmallInt, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd4, "@DaysFromCreation", SqlDbType.SmallInt, iDaysFromCreation);
            }
            DbHelperSQL.AddSqlParameter(SqlCmd4, "@CalculationMethod", SqlDbType.SmallInt, iCalcMethod);

            DbHelperSQL.AddSqlParameter(SqlCmd4, "@WflStageId", SqlDbType.Int, iWflStageID);

            DbHelperSQL.ExecuteNonQuery(SqlCmd4);
        }

        public void UpdateWorkflowType(int iWflTemplId, string sWorkflowType)
        {
            string sSql7 = "UPDATE Template_Workflow SET WorkflowType = @WorkflowType WHERE WflTemplId=@WflTemplId";
            SqlCommand SqlCmd7 = new SqlCommand(sSql7);

            DbHelperSQL.AddSqlParameter(SqlCmd7, "@WorkflowType", SqlDbType.NVarChar, sWorkflowType);
            DbHelperSQL.AddSqlParameter(SqlCmd7, "@WflTemplId", SqlDbType.Int, iWflTemplId);

            DbHelperSQL.ExecuteNonQuery(SqlCmd7);
        }

        /// <summary>
        /// 检查sequece of workflow template是否存在
        /// neo 2011-02-16
        /// </summary>
        /// <param name="iWorkflowTemplateID"></param>
        /// <param name="iWflStageID"></param>
        /// <param name="iSeq"></param>
        /// <returns></returns>
        public bool IsWflStageSeqExistBase(int iWorkflowTemplateID, int iWflStageID, short iSeq)
        {
            string sSql = "select count(1) from Template_Wfl_Stages where WflTemplId=" + iWorkflowTemplateID + " and SequenceNumber = " + iSeq + " and WflStageId != " + iWflStageID;
            SqlCommand SqlCmd = new SqlCommand(sSql);
            int iCount = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd));
            if (iCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// get default workflow template
        /// neo 2011-03-21
        /// </summary>
        /// <param name="sStatus"></param>
        /// <returns></returns>
        public DataTable GetDefaultWorkflowTemplateInfoBase(string sStatus)
        {
            string sSql = "select top(1) * from Template_Workflow where WorkflowType='" + sStatus + "' and [Default]=1";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// insert workflow template base on clone
        /// Rocky 2011-09-05
        /// </summary>
        /// <param name="sWorkflowTemplateName"></param>
        /// <param name="bEnabled"></param>
        /// <param name="sDesc"></param>
        /// <param name="sWorkflowType"></param>
        /// <param name="bDefault"></param>
        /// <param name="sCalculationMethod"></param>
        /// <param name="StageList"></param>
        /// <param name="TaskList"></param>
        public void CloneWorkflowTemplateBase(string sWorkflowTemplateName, bool bEnabled, string sDesc, string sWorkflowType, bool bDefault, string sCalculationMethod, DataTable StageList)
        {
            #region build sql command - insert Template_Workflow

            string sSql = "INSERT INTO Template_Workflow (Name,Enabled,[Desc],WorkflowType,Custom,[Default],CalculationMethod) VALUES (@Name,@Enabled,@Desc,@WorkflowType,@Custom,@Default,@CalculationMethod);"
                        + "select SCOPE_IDENTITY();";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sWorkflowTemplateName);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, bEnabled);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Desc", SqlDbType.NVarChar, sDesc);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@WorkflowType", SqlDbType.NVarChar, sWorkflowType);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Custom", SqlDbType.Bit, true);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Default", SqlDbType.Bit, 0);

            Int16 iCalculationMethod = 0;
            //if (sCalculationMethod == "Est Close Date")
            //{
            //    iCalculationMethod = 1;
            //}
            //else
            //{
            //    iCalculationMethod = 2;
            //}

            if (sCalculationMethod == "Est Close Date")
            {
                iCalculationMethod = 1;
            }
            else if (sCalculationMethod == "Creation Date")
            {
                iCalculationMethod = 2;
            }
            else
            {
                iCalculationMethod = 3;
            }
            DbHelperSQL.AddSqlParameter(SqlCmd, "@CalculationMethod", SqlDbType.SmallInt, iCalculationMethod);

            #endregion

            #region build sql command - set defaul

            string sSql2 = "update Template_Workflow set [Default]=0 where WorkflowType=@WorkflowType;"
                         + "update Template_Workflow set [Default]=1 where WflTemplId=@WflTemplId";
            SqlCommand SqlCmd2 = new SqlCommand(sSql2);

            DbHelperSQL.AddSqlParameter(SqlCmd2, "@WorkflowType", SqlDbType.NVarChar, sWorkflowType);

            #endregion


            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                int iNewID = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd, SqlTrans));

                #region update new stage id to Template_Wfl_Stages


                #region build sql command - Template_Wfl_Stages

                if (StageList.Rows.Count > 0)
                {
                    foreach (DataRow StageRow in StageList.Rows)
                    {
                        string sSql3 = "INSERT INTO Template_Wfl_Stages (WflTemplId,SequenceNumber,[Enabled],DaysFromEstClose,Name,DaysFromCreation,TemplStageId,CalculationMethod) VALUES (@WflTemplId,@SequenceNumber,@Enabled,@DaysFromEstClose,@Name,@DaysFromCreation,@TemplStageId, @CalculationMethod);"
                                    + "select SCOPE_IDENTITY();";
                        SqlCommand SqlCmd3 = new SqlCommand(sSql3);

                        DbHelperSQL.AddSqlParameter(SqlCmd3, "@WflTemplId", SqlDbType.Int, iNewID);
                        if (StageRow["SequenceNumber"].ToString() == "")
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd3, "@SequenceNumber", SqlDbType.SmallInt, 0);
                        }
                        else
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd3, "@SequenceNumber", SqlDbType.SmallInt, Convert.ToInt16(StageRow["SequenceNumber"].ToString()));
                        }
                        DbHelperSQL.AddSqlParameter(SqlCmd3, "@Enabled", SqlDbType.Bit, Convert.ToBoolean(StageRow["Enabled"].ToString()));
                        if (StageRow["DaysFromEstClose"].ToString() == "")
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd3, "@DaysFromEstClose", SqlDbType.SmallInt, DBNull.Value);
                        }
                        else
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd3, "@DaysFromEstClose", SqlDbType.SmallInt, Convert.ToInt16(StageRow["DaysFromEstClose"].ToString()));
                        }
                        DbHelperSQL.AddSqlParameter(SqlCmd3, "@Name", SqlDbType.NVarChar, StageRow["Name"].ToString());
                        if (StageRow["DaysFromCreation"].ToString() == "")
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd3, "@DaysFromCreation", SqlDbType.SmallInt, DBNull.Value);
                        }
                        else
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd3, "@DaysFromCreation", SqlDbType.SmallInt, Convert.ToInt16(StageRow["DaysFromCreation"].ToString()));
                        }
                        if (StageRow["TemplStageId"].ToString() == "")
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd3, "@TemplStageId", SqlDbType.Int, DBNull.Value);
                        }
                        else
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd3, "@TemplStageId", SqlDbType.Int, Convert.ToInt32(StageRow["TemplStageId"].ToString()));
                        }
                        if (StageRow["CalculationMethod"].ToString() == "")
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd3, "@CalculationMethod", SqlDbType.SmallInt, DBNull.Value);
                        }
                        else
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd3, "@CalculationMethod", SqlDbType.SmallInt, Convert.ToInt16(StageRow["CalculationMethod"].ToString()));
                        }
                        int iNewStageID = Convert.ToInt32(DbHelperSQL.ExecuteScalar(SqlCmd3, SqlTrans));
                        if (StageRow["OldWflStageId"].ToString() != "")
                        {
                            //string sSql4 = "INSERT INTO [Template_Wfl_Tasks] ([WflStageId],[Name],[Enabled],[Type],[DaysDueFromCoe],[PrerequisiteTaskId],[DaysDueAfterPrerequisite],[OwnerRoleId],[WarningEmailId],[OverdueEmailId],[CompletionEmailId],[SequenceNumber],[Description])"
                            //             + "SELECT " + iNewStageID.ToString() + ",[Name],[Enabled],[Type],DaysDueFromCoe,PrerequisiteTaskId,DaysDueAfterPrerequisite,OwnerRoleId,WarningEmailId,OverdueEmailId,CompletionEmailId,SequenceNumber,[Description]"
                            //             + " FROM Template_Wfl_Tasks WHERE WflStageId = " + StageRow["OldWflStageId"].ToString();
                            //SqlCommand SqlCmd4 = new SqlCommand(sSql4);
                            //DbHelperSQL.ExecuteNonQuery(SqlCmd4, SqlTrans);
                            int oldWflStageId = (int)StageRow["OldWflStageId"];
 
                            string sqlCmd = string.Format("exec [dbo].[lpsp_CloneWorkflowTasks] {0},{1}", oldWflStageId, iNewStageID);
                            SqlCommand SqlCmd4 = new SqlCommand(sqlCmd);
                            DbHelperSQL.ExecuteNonQuery(SqlCmd4, SqlTrans);
                        }
                    }
                }

                #endregion

                #endregion

                SqlTrans.Commit();

                // is default
                if (bDefault == true)
                {
                    DbHelperSQL.AddSqlParameter(SqlCmd2, "@WflTemplId", SqlDbType.Int, iNewID);
                    DbHelperSQL.ExecuteNonQuery(SqlCmd2);
                }
            }
            catch (Exception ex)
            {
                SqlTrans.Rollback();
                throw ex;
            }
            finally
            {
                if (SqlConn != null)
                {
                    SqlConn.Close();
                }
            }

            #endregion

        }

        #endregion        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrderBy"></param>
        /// <returns></returns>
        public DataTable GetWorkflowTemplateList(string sWhere, string sOrderBy)
        {
            string sSql0 = "select * from Template_Workflow where 1=1 " + sWhere + " order by " + sOrderBy;
            DataTable WorkflowList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
            return WorkflowList;
        }
    }
}

