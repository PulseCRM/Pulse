using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;

//using CommonClasses;

namespace focusIT
{
    public abstract class DbHelperSQL
    {
        #region Connection String

        private static string _connectionString = string.Empty;

        public static string connectionString
        {
            get
            {
                if ((_connectionString == null) || (_connectionString == string.Empty))
                    _connectionString = strDBConn;
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        public static readonly string strDBConn = ConfigurationManager.ConnectionStrings["focusITConnectionString"].ConnectionString;
        
        #endregion 

        #region Common Function

        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        public static bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region  SQL

        public static int ExecuteSql(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))            
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        throw E;
                    }
                    finally
                    {
                        if (cmd != null)
                            cmd.Dispose();
                        if ((connection != null) && (connection.State != ConnectionState.Closed))
                            connection.Close();
                    }
                }
            }
        }

        public static void ExecuteSqlTran(ArrayList SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    SqlTransaction tx = conn.BeginTransaction();
                    cmd.Transaction = tx;
                    try
                    {
                        for (int n = 0; n < SQLStringList.Count; n++)
                        {
                            string strsql = SQLStringList[n].ToString();
                            if (strsql.Trim().Length > 1)
                            {
                                cmd.CommandText = strsql;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        tx.Commit();
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        tx.Rollback();

                        throw new Exception(E.Message);
                    }
                    finally
                    {
                        if (cmd != null)
                            cmd.Dispose();
                        if ((conn != null) && (conn.State != ConnectionState.Closed))
                            conn.Close();
                    }
                }
            }
        }

        public static int ExecuteSql(string SQLString, string content)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))          
            {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@content", SqlDbType.NText);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    if (cmd != null)
                        cmd.Dispose();
                    if ((connection != null) && (connection.State != ConnectionState.Closed))
                        connection.Close();
                }
            }
        }

        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))         
            {
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@fs", SqlDbType.Image);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    if (cmd != null)
                        cmd.Dispose();
                    if ((connection != null) && (connection.State != ConnectionState.Closed))
                        connection.Close();
                }
            }
        }

        public static object GetSingle(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))         
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        //cmd.CommandTimeout = 1000;
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw new Exception(e.Message);
                    }
                    finally 
                    {
                        if (cmd != null)
                            cmd.Dispose();
                        if ((connection != null) && (connection.State != ConnectionState.Closed))
                            connection.Close();
                    }
                }
            }
        }

        public static SqlDataReader ExecuteReader(string strSQL)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(strSQL, connection);
            try
            {
                connection.Open();
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                //if ((connection != null) && (connection.State != ConnectionState.Closed))
                //    connection.Close();
            }
        }

        public static DataSet Query(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))        
            {
                DataSet ds = new DataSet();
               
                    connection.Open();
                    using (SqlDataAdapter command = new SqlDataAdapter(SQLString, connection))
                    {
                        try
                        {
                            command.Fill(ds, "ds");
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            throw new Exception(ex.Message);
                        }
                        finally
                        {
                            if (command != null)
                                command.Dispose();
                            if ((connection != null) && (connection.State != ConnectionState.Closed))
                                connection.Close();
                        }
                        return ds;
                    }
            }
        }

        #endregion

        #region Prams SQL

        public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))        
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        throw new Exception(E.Message);
                    }
                    finally
                    {
                        if (cmd != null)
                            cmd.Dispose();
                        if ((connection != null) && (connection.State != ConnectionState.Closed))
                            connection.Close();
                    }
                }
            }
        }

        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            trans.Commit();
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                    finally
                    {
                        if (trans != null)
                            trans.Dispose();
                        if (cmd != null)
                            cmd.Dispose();
                        if ((conn != null) && (conn.State != ConnectionState.Closed))
                            conn.Close();
                    }
                }
            }
        }

        public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))        
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw new Exception(e.Message);
                    }
                    finally {
                        if (cmd != null)
                            cmd.Dispose();
                        if ((connection != null) && (connection.State != ConnectionState.Closed))
                            connection.Close();
                    }
                }
            }
        }

        public static SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                SqlDataReader myReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if ((connection != null) && (connection.State != ConnectionState.Closed))
                    connection.Close();
            }

        }

        public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))        
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (cmd != null)
                            cmd.Dispose();
                        if ((connection != null)  && (connection.State != ConnectionState.Closed))
                            connection.Close();
                    }
                    return ds;
                }
            }
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion

        #region Store Procedure

        public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataReader returnReader;
                connection.Open();

                using (SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        returnReader = command.ExecuteReader();
                        return returnReader;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (command != null)
                            command.Dispose();
                        if ((connection != null) && (connection.State != ConnectionState.Closed))
                            connection.Close();
                    }
                }
            }
        }

        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))        
             {
                
                     DataSet dataSet = new DataSet();
                     connection.Open();
                     SqlDataAdapter sqlDA = new SqlDataAdapter();
                     using (sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters))
                     {
                         try
                         {
                             sqlDA.Fill(dataSet, tableName);
                             connection.Close();
                             return dataSet;
                         }
                         catch (Exception ex)
                         {
                             throw ex;
                         }
                         finally
                         {
                             if ((connection != null) && (connection.State != ConnectionState.Closed))
                                 connection.Close();
                         }
                     }
            }
        }

        public static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
            return command;
        }

        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                    int result;                  
                    connection.Open();
                    using (SqlCommand command = BuildIntCommand(connection, storedProcName, parameters))
                    {
                        try
                        {
                            rowsAffected = command.ExecuteNonQuery();
                            result = (int)command.Parameters["@ReturnValue"].Value;
                            //Connection.Close();                  
                            return result;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            if (command != null)
                                command.Dispose();
                            if ((connection != null) && (connection.State != ConnectionState.Closed))
                                connection.Close();
                        }
                    }
            }
        }

        /// <summary>
        /// Runs the procedure.
        /// </summary>
        /// <param name="storedProcName">Name of the stored proc.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="rowsAffected">The rows affected.</param>
        /// <param name="outputValue">The output value.</param>
        /// <returns>rowsAffected</returns>
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected,out string outputValue)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = BuildIntCommand(connection, storedProcName, parameters))
                {
                    try
                    {
                        int result;

                        rowsAffected = command.ExecuteNonQuery();
                        result = (int)command.Parameters["@ReturnValue"].Value;
                        outputValue = command.Parameters["@lid"].Value.ToString();
                        //Connection.Close();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (command != null)
                            command.Dispose();
                        if ((connection != null) && (connection.State != ConnectionState.Closed))
                            connection.Close();
                    }
                }
            }
        }

        private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new SqlParameter("@ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }

        public static DataTable ExecuteDataTable(string sSqlText)
        {
            SqlCommand SqlCmd = new SqlCommand(sSqlText);

            return ExecuteDataTable(SqlCmd);
        }
      
        public static DataTable ExecuteDataTable(SqlCommand SqlCmd)
        {
            DataTable NewDataTable = new DataTable();

            using (SqlConnection SqlConn = new SqlConnection(connectionString))
            {
                SqlCmd.Connection = SqlConn;

                SqlDataAdapter SqlAdapter = new SqlDataAdapter(SqlCmd);

                SqlAdapter.Fill(NewDataTable);
            }

            return NewDataTable;
        }

        #endregion

        

        #region ExecuteScalar
        /// <summary>
        /// 执行Sql语句，返回Scalar
        /// 刘洋 2009-02-20
        /// </summary>
        /// <param name="sSqlText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sSqlText)
        {
            SqlCommand SqlCmd = new SqlCommand(sSqlText);

            return ExecuteScalar(SqlCmd);
        }
        /// <summary>
        /// 执行Sql语句，返回Scalar
        /// 刘洋 2009-02-20
        /// </summary>
        /// <param name="SqlCmd"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlCommand SqlCmd)
        {
            object oScalar;

            using (SqlConnection SqlConn = new SqlConnection(connectionString))
            {
                SqlConn.Open();

                SqlCmd.Connection = SqlConn;

                oScalar = SqlCmd.ExecuteScalar();
            }

            return oScalar;
        }
        /// <summary>
        /// 执行Sql语句，返回Scalar
        /// 刘洋 2009-04-30
        /// </summary>
        /// <param name="SqlCmd"></param>
        /// <param name="SqlConn"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlCommand SqlCmd, SqlTransaction SqlTrans)
        {
            SqlCmd.Connection = SqlTrans.Connection;
            SqlCmd.Transaction = SqlTrans;

            object oScalar = SqlCmd.ExecuteScalar();

            return oScalar;
        }
        /// <summary>
        /// 执行Sql语句，返回Scalar
        /// 刘洋 2009-04-30
        /// </summary>
        /// <param name="SqlCmd"></param>
        /// <param name="SqlConn"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlCommand SqlCmd, SqlConnection SqlConn, SqlTransaction SqlTrans)
        {
            SqlCmd.Connection = SqlConn;
            SqlCmd.Transaction = SqlTrans;

            object oScalar = SqlCmd.ExecuteScalar();

            return oScalar;
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// 执行Sql语句，返回影响记录数
        /// 刘洋 2009-02-20
        /// </summary>
        /// <param name="sSqlText"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sSqlText)
        {
            SqlCommand SqlCmd = new SqlCommand(sSqlText);

            return ExecuteNonQuery(SqlCmd);
        }
        /// <summary>
        /// 执行Sql语句，返回影响记录数
        /// 刘洋 2009-02-20
        /// </summary>
        /// <param name="SqlCmd"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlCommand SqlCmd)
        {
            int iRowCount;

            using (SqlConnection SqlConn = new SqlConnection(connectionString))
            {
                SqlConn.Open();

                SqlCmd.Connection = SqlConn;

                iRowCount = SqlCmd.ExecuteNonQuery();
            }

            return iRowCount;
        }
        /// <summary>
        /// 执行Sql语句，返回影响记录数(支持事务)
        /// 刘洋 2009-02-20
        /// </summary>
        /// <param name="SqlCmd"></param>
        /// <param name="SqlTrans"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlCommand SqlCmd, SqlTransaction SqlTrans)
        {
            int iRowCount;

            SqlCmd.Connection = SqlTrans.Connection;
            SqlCmd.Transaction = SqlTrans;

            iRowCount = SqlCmd.ExecuteNonQuery();

            return iRowCount;
        }
        /// <summary>
        /// 执行Sql语句，返回影响记录数
        /// 刘洋 2009-02-20
        /// </summary>
        /// <param name="SqlCmd"></param>
        /// <param name="connectionStringName"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlCommand SqlCmd, string connectionStringName)
        {
            int iRowCount;

            using (SqlConnection SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
            {
                SqlConn.Open();

                SqlCmd.Connection = SqlConn;

                iRowCount = SqlCmd.ExecuteNonQuery();
            }

            return iRowCount;
        }
        #endregion

        #region ExecuteDataReader
        /// <summary>
        /// 执行Sql语句，返回SqlDataReader
        /// 刘洋 2009-02-20
        /// </summary>
        /// <param name="sSqlText"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteDataReader(string sSqlText)
        {
            SqlCommand SqlCmd = new SqlCommand(sSqlText);

            return ExecuteDataReader(SqlCmd);
        }
        /// <summary>
        /// 执行Sql语句，返回SqlDataReader
        /// 刘洋 2009-02-20
        /// </summary>
        /// <param name="SqlCmd"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteDataReader(SqlCommand SqlCmd)
        {
            SqlDataReader SqlReader;

            using (SqlConnection SqlConn = new SqlConnection(connectionString))
            {
                SqlConn.Open();

                SqlCmd.Connection = SqlConn;

                SqlReader = SqlCmd.ExecuteReader();
            }

            return SqlReader;
        }

        /// <summary>
        /// 关闭SqlDataReader和SqlConnection
        /// </summary>
        /// <param name="SqlReader"></param>
        /// <param name="SqlConn"></param>
        public static void CloseReaderAndConn(SqlDataReader SqlReader, SqlConnection SqlConn)
        {
            if (SqlReader != null)
            {
                SqlReader.Close();
            }

            if (SqlConn != null)
            {
                SqlConn.Close();
            }
        }
        #endregion

        #region UpdateDataTable
        /// <summary>
        /// 批量更新DataTable
        /// 刘洋 2009-02-20
        /// </summary>
        /// <param name="DataTableObj"></param>
        /// <param name="insertCmd"></param>
        /// <param name="updateCmd"></param>
        /// <param name="deleteCmd"></param>
        /// <returns></returns>
        public static void UpdateDataTable(DataTable DataTableObj, SqlCommand InsertCmd, SqlCommand UpdateCmd, SqlCommand DeleteCmd)
        {
            using (SqlConnection SqlConn = new SqlConnection(connectionString))
            {
                SqlConn.Open();
                SqlTransaction SqlTrans = SqlConn.BeginTransaction();

                SqlDataAdapter SqlAdapter = new SqlDataAdapter();

                if (InsertCmd != null)
                {
                    InsertCmd.Connection = SqlConn;
                    InsertCmd.Transaction = SqlTrans;

                    SqlAdapter.InsertCommand = InsertCmd;
                }

                if (UpdateCmd != null)
                {
                    UpdateCmd.Connection = SqlConn;
                    UpdateCmd.Transaction = SqlTrans;

                    SqlAdapter.UpdateCommand = UpdateCmd;
                }

                if (DeleteCmd != null)
                {
                    DeleteCmd.Connection = SqlConn;
                    DeleteCmd.Transaction = SqlTrans;

                    SqlAdapter.DeleteCommand = DeleteCmd;
                }

                try
                {
                    SqlAdapter.Update(DataTableObj);

                    SqlTrans.Commit();
                }
                catch (Exception)
                {
                    SqlTrans.Rollback();
                    throw;
                }
            }
        }
        /// <summary>
        /// 批量更新DataTable(支持事务)
        /// 刘洋 2009-02-20
        /// </summary>
        /// <param name="DataTableObj"></param>
        /// <param name="insertCmd"></param>
        /// <param name="updateCmd"></param>
        /// <param name="deleteCmd"></param>
        /// <param name="SqlTrans"></param>
        /// <returns></returns>
        public static void UpdateDataTable(DataTable DataTableObj, SqlCommand InsertCmd, SqlCommand UpdateCmd, SqlCommand DeleteCmd, SqlTransaction SqlTrans)
        {
            SqlConnection SqlConn = SqlTrans.Connection;

            SqlDataAdapter SqlAdapter = new SqlDataAdapter();

            if (InsertCmd != null)
            {
                InsertCmd.Connection = SqlConn;
                InsertCmd.Transaction = SqlTrans;

                SqlAdapter.InsertCommand = InsertCmd;
            }

            if (UpdateCmd != null)
            {
                UpdateCmd.Connection = SqlConn;
                UpdateCmd.Transaction = SqlTrans;

                SqlAdapter.UpdateCommand = UpdateCmd;
            }

            if (DeleteCmd != null)
            {
                DeleteCmd.Connection = SqlConn;
                DeleteCmd.Transaction = SqlTrans;

                SqlAdapter.DeleteCommand = DeleteCmd;
            }

            SqlAdapter.Update(DataTableObj);
        }
        #endregion

        #region AddSqlParameter
        /// <summary>
        /// 添加SqlParameter
        /// 刘洋 2009-02-20
        /// </summary>
        /// <param name="SqlCmd"></param>
        /// <param name="sParameterName"></param>
        /// <param name="SqlDbTypeEnum"></param>
        /// <param name="Value"></param>
        public static void AddSqlParameter(SqlCommand SqlCmd, string sParameterName, SqlDbType SqlDbTypeEnum, object oValue)
        {
            SqlParameter SqlParam = new SqlParameter(sParameterName, SqlDbTypeEnum);
            SqlParam.Value = oValue;

            SqlCmd.Parameters.Add(SqlParam);
        }
        /// <summary>
        /// 添加SqlParameter
        /// 刘洋 2009-02-20
        /// </summary>
        /// <param name="SqlCmd"></param>
        /// <param name="sParameterName"></param>
        /// <param name="Value"></param>
        public static void AddSqlParameter(SqlCommand SqlCmd, string sParameterName, object oValue)
        {
            SqlParameter SqlParam = new SqlParameter(sParameterName, oValue);

            SqlCmd.Parameters.Add(SqlParam);
        }
        /// <summary>
        /// 添加Sql Parameter
        /// </summary>
        /// <param name="SqlCmd"></param>
        /// <param name="sParameterName"></param>
        /// <param name="SqlDbTypeEnum"></param>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public static SqlParameter AddSqlParameter1(SqlCommand SqlCmd, string sParameterName, SqlDbType SqlDbTypeEnum, ParameterDirection Direction)
        {
            SqlParameter SqlParam = new SqlParameter(sParameterName, SqlDbTypeEnum);
            SqlParam.Direction = ParameterDirection.Output;

            SqlCmd.Parameters.Add(SqlParam);

            return SqlParam;
        }

        /// <summary>
        /// 添加Sql Parameter
        /// 刘洋 2009-04-14
        /// </summary>
        /// <param name="SqlCmd"></param>
        /// <param name="sParameterName"></param>
        /// <param name="SqlDbTypeEnum"></param>
        /// <param name="oValue"></param>
        /// <param name="sSourceColumn"></param>
        public static void AddSqlParameter1(SqlCommand SqlCmd, string sParameterName, SqlDbType SqlDbTypeEnum, string sSourceColumn)
        {
            SqlParameter SqlParameter1 = new SqlParameter(sParameterName, SqlDbTypeEnum);
            SqlParameter1.SourceColumn = sSourceColumn;

            SqlCmd.Parameters.Add(SqlParameter1);
        }
        #endregion

        #region GetOpenConnection
        /// <summary>
        /// 获取Open的SqlConnection
        /// 刘洋 2009-02-20
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetOpenConnection()
        {
            SqlConnection SqlConn = new SqlConnection(connectionString);
            SqlConn.Open();
            return SqlConn;
        }
        /// <summary>
        /// 获取Open的SqlConnection
        /// 刘洋 2009-02-20
        /// </summary>
        /// <param name="sConnStringName">连接字符串名</param>
        /// <returns></returns>
        public static SqlConnection GetOpenConnection(string sConnStringName)
        {
            SqlConnection SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings[sConnStringName].ConnectionString);
            SqlConn.Open();
            return SqlConn;
        }
        #endregion
    }
}
