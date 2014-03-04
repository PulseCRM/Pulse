using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;
using System.Collections.Generic;
using Microsoft.SharePoint;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问抽象基础类
    /// </summary>
    public abstract class DbHelperSQL
    {
        //数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库.		
        //public static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LPCon"].ConnectionString;
        public static string connectionString 
        {
            get
            {
                string strCon = "";
                SPWeb currWeb = SPContext.Current.Web;
                // SPProperty Key name is case sensitive
                if (currWeb.AllProperties["lpcon"] != null && !string.IsNullOrEmpty(currWeb.AllProperties["lpcon"].ToString()))
                    strCon = currWeb.AllProperties["lpcon"].ToString();
                if (string.IsNullOrEmpty(strCon))
                {
                        throw new NullReferenceException("Cannot find database connection string, lpcon.");
                }
                else
                    return strCon;
            }
        }

        public DbHelperSQL()
        {            
        }

        #region 公用方法
        /// <summary>
        /// 判断是否存在某表的某个字段
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列名称</param>
        /// <returns>是否存在</returns>
        public static bool ColumnExists(string tableName, string columnName)
        {
            string sql = "select count(1) from syscolumns where [id]=object_id('" + tableName + "') and [name]='" + columnName + "'";
            object res = GetSingle(sql);
            if (res == null)
            {
                return false;
            }
            return Convert.ToInt32(res) > 0;
        }
        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = DbHelperSQL.GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        public static bool Exists(string strSql)
        {
            object obj = DbHelperSQL.GetSingle(strSql);
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
        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static bool TabExists(string TableName)
        {
            string strsql = "select count(*) from sysobjects where id = object_id(N'[" + TableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
            //string strsql = "SELECT count(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + TableName + "]') AND type in (N'U')";
            object obj = DbHelperSQL.GetSingle(strsql);
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
        public static bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            object obj = DbHelperSQL.GetSingle(strSql, cmdParms);
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

        #region  执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
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
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        public static int ExecuteSqlByTime(string SQLString, int Times)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
      
        ///// <summary>
        ///// 执行Sql和Oracle滴混合事务
        ///// </summary>
        ///// <param name="list">SQL命令行列表</param>
        ///// <param name="oracleCmdSqlList">Oracle命令行列表</param>
        ///// <returns>执行结果 0-由于SQL造成事务失败 -1 由于Oracle造成事务失败 1-整体事务执行成功</returns>
        //public static int ExecuteSqlTran(List<CommandInfo> list, List<CommandInfo> oracleCmdSqlList)
        //{
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = conn;
        //        SqlTransaction tx = conn.BeginTransaction();
        //        cmd.Transaction = tx;
        //        try
        //        {
        //            foreach (CommandInfo myDE in list)
        //            {
        //                string cmdText = myDE.CommandText;
        //                SqlParameter[] cmdParms = (SqlParameter[])myDE.Parameters;
        //                PrepareCommand(cmd, conn, tx, cmdText, cmdParms);
        //                if (myDE.EffentNextType == EffentNextType.SolicitationEvent)
        //                {
        //                    if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
        //                    {
        //                        tx.Rollback();
        //                        throw new Exception("违背要求"+myDE.CommandText+"必须符合select count(..的格式");
        //                        //return 0;
        //                    }

        //                    object obj = cmd.ExecuteScalar();
        //                    bool isHave = false;
        //                    if (obj == null && obj == DBNull.Value)
        //                    {
        //                        isHave = false;
        //                    }
        //                    isHave = Convert.ToInt32(obj) > 0;
        //                    if (isHave)
        //                    {
        //                        //引发事件
        //                        myDE.OnSolicitationEvent();
        //                    }
        //                }
        //                if (myDE.EffentNextType == EffentNextType.WhenHaveContine || myDE.EffentNextType == EffentNextType.WhenNoHaveContine)
        //                {
        //                    if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
        //                    {
        //                        tx.Rollback();
        //                        throw new Exception("SQL:违背要求" + myDE.CommandText + "必须符合select count(..的格式");
        //                        //return 0;
        //                    }

        //                    object obj = cmd.ExecuteScalar();
        //                    bool isHave = false;
        //                    if (obj == null && obj == DBNull.Value)
        //                    {
        //                        isHave = false;
        //                    }
        //                    isHave = Convert.ToInt32(obj) > 0;

        //                    if (myDE.EffentNextType == EffentNextType.WhenHaveContine && !isHave)
        //                    {
        //                        tx.Rollback();
        //                        throw new Exception("SQL:违背要求" + myDE.CommandText + "返回值必须大于0");
        //                        //return 0;
        //                    }
        //                    if (myDE.EffentNextType == EffentNextType.WhenNoHaveContine && isHave)
        //                    {
        //                        tx.Rollback();
        //                        throw new Exception("SQL:违背要求" + myDE.CommandText + "返回值必须等于0");
        //                        //return 0;
        //                    }
        //                    continue;
        //                }
        //                int val = cmd.ExecuteNonQuery();
        //                if (myDE.EffentNextType == EffentNextType.ExcuteEffectRows && val == 0)
        //                {
        //                    tx.Rollback();
        //                    throw new Exception("SQL:违背要求" + myDE.CommandText + "必须有影响行");
        //                    //return 0;
        //                }
        //                cmd.Parameters.Clear();
        //            }
        //            string oraConnectionString = PubConstant.GetConnectionString("ConnectionStringPPC");
        //            bool res = OracleHelper.ExecuteSqlTran(oraConnectionString, oracleCmdSqlList);
        //            if (!res)
        //            {
        //                tx.Rollback();
        //                throw new Exception("Oracle执行失败");
        //                // return -1;
        //            }
        //            tx.Commit();
        //            return 1;
        //        }
        //        catch (System.Data.SqlClient.SqlException e)
        //        {
        //            tx.Rollback();
        //            throw e;
        //        }
        //        catch (Exception e)
        //        {
        //            tx.Rollback();
        //            throw e;
        //        }
        //    }
        //}        
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static int ExecuteSqlTran(List<String> SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch
                {
                    tx.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
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
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static object ExecuteSqlGet(string SQLString, string content)
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
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
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
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
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
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        public static object GetSingle(string SQLString, int Times)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
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
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
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
                throw e;
            }   

        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }
        public static DataSet Query(string SQLString, int Times)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.SelectCommand.CommandTimeout = Times;
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }



        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
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
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
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
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        ///// <summary>
        ///// 执行多条SQL语句，实现数据库事务。
        ///// </summary>
        ///// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        //public static int ExecuteSqlTran(System.Collections.Generic.List<CommandInfo> cmdList)
        //{
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        using (SqlTransaction trans = conn.BeginTransaction())
        //        {
        //            SqlCommand cmd = new SqlCommand();
        //            try
        //            { int count = 0;
        //                //循环
        //                foreach (CommandInfo myDE in cmdList)
        //                {
        //                    string cmdText = myDE.CommandText;
        //                    SqlParameter[] cmdParms = (SqlParameter[])myDE.Parameters;
        //                    PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                           
        //                    if (myDE.EffentNextType == EffentNextType.WhenHaveContine || myDE.EffentNextType == EffentNextType.WhenNoHaveContine)
        //                    {
        //                        if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
        //                        {
        //                            trans.Rollback();
        //                            return 0;
        //                        }

        //                        object obj = cmd.ExecuteScalar();
        //                        bool isHave = false;
        //                        if (obj == null && obj == DBNull.Value)
        //                        {
        //                            isHave = false;
        //                        }
        //                        isHave = Convert.ToInt32(obj) > 0;

        //                        if (myDE.EffentNextType == EffentNextType.WhenHaveContine && !isHave)
        //                        {
        //                            trans.Rollback();
        //                            return 0;
        //                        }
        //                        if (myDE.EffentNextType == EffentNextType.WhenNoHaveContine && isHave)
        //                        {
        //                            trans.Rollback();
        //                            return 0;
        //                        }
        //                        continue;
        //                    }
        //                    int val = cmd.ExecuteNonQuery();
        //                    count += val;
        //                    if (myDE.EffentNextType == EffentNextType.ExcuteEffectRows && val == 0)
        //                    {
        //                        trans.Rollback();
        //                        return 0;
        //                    }
        //                    cmd.Parameters.Clear();
        //                }
        //                trans.Commit();
        //                return count;
        //            }
        //            catch
        //            {
        //                trans.Rollback();
        //                throw;
        //            }
        //        }
        //    }
        //}
        ///// <summary>
        ///// 执行多条SQL语句，实现数据库事务。
        ///// </summary>
        ///// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        //public static void ExecuteSqlTranWithIndentity(System.Collections.Generic.List<CommandInfo> SQLStringList)
        //{
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        using (SqlTransaction trans = conn.BeginTransaction())
        //        {
        //            SqlCommand cmd = new SqlCommand();
        //            try
        //            {
        //                int indentity = 0;
        //                //循环
        //                foreach (CommandInfo myDE in SQLStringList)
        //                {
        //                    string cmdText = myDE.CommandText;
        //                    SqlParameter[] cmdParms = (SqlParameter[])myDE.Parameters;
        //                    foreach (SqlParameter q in cmdParms)
        //                    {
        //                        if (q.Direction == ParameterDirection.InputOutput)
        //                        {
        //                            q.Value = indentity;
        //                        }
        //                    }
        //                    PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
        //                    int val = cmd.ExecuteNonQuery();
        //                    foreach (SqlParameter q in cmdParms)
        //                    {
        //                        if (q.Direction == ParameterDirection.Output)
        //                        {
        //                            indentity = Convert.ToInt32(q.Value);
        //                        }
        //                    }
        //                    cmd.Parameters.Clear();
        //                }
        //                trans.Commit();
        //            }
        //            catch
        //            {
        //                trans.Rollback();
        //                throw;
        //            }
        //        }
        //    }
        //}
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSqlTranWithIndentity(Hashtable SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
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
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw e;
            }
            //			finally
            //			{
            //				cmd.Dispose();
            //				connection.Close();
            //			}	

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
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


                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion

        #region 存储过程操作

        /// <summary>
        /// 执行存储过程，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataReader returnReader;
            connection.Open();
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return returnReader;
            
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }
        public static DataSet RunProcedure_AlertList(string storedProcName, IDataParameter[] parameters, string tableName, out int dcnt)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int ndx = 0;
                dcnt = 0;
                int icnt = 0;
                int scnt = 0;
                int acnt = 0;
                int bcnt = 0;
                int ecnt = 0;                
                bool found = false;
                int rowid = 0;
                List<int> rowids = new List<int>();
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                scnt = (int)parameters[2].Value;
                parameters[2].Value = 2000000;
                icnt = (int)parameters[3].Value;
                parameters[3].Value = 1;
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                DataTable dt = dataSet.Tables[tableName];

                bcnt = (icnt - 1) * scnt;
                ecnt = bcnt + scnt;
               
                foreach (DataRow row in dt.Rows)
                {
                    found = false;
                    rowid = (int)row[1];

                    foreach (int rowint in rowids)
                    {
                        if (rowint == rowid)
                        {
                            found = true;
                        }
                    }

                    if (found == true)
                    {
                        dcnt = dcnt + 1;
                        dataSet.Tables[tableName].Rows[ndx].Delete();
                    }
                    else
                    {
                        acnt = acnt + 1;
                        rowids.Add(rowid);
                    }

                    if (acnt <= bcnt)
                    {                    
                        dataSet.Tables[tableName].Rows[ndx].Delete();
                    }

                    if (acnt > ecnt)
                    {                    
                        dataSet.Tables[tableName].Rows[ndx].Delete();
                    }

                ndx = ndx + 1;
                }

                connection.Close();
                return dataSet;
            }
        }
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int Times)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.SelectCommand.CommandTimeout = Times;
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }


        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        public static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int result;
                connection.Open();
                SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                rowsAffected = command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                //Connection.Close();
                return result;
            }
        }

        /// <summary>
        /// 创建 SqlCommand 对象实例(用来返回一个整数值)	
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand 对象实例</returns>
        public static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }
        #endregion

        /// <summary>
        /// 获取数据数量
        /// 刘洋 2010-02-06
        /// </summary>
        /// <param name="sTable"></param>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public static int Count(string sTable, string sWhere)
        {
            string sSql = "select count(1) from " + sTable + " where (1 = 1)" + sWhere;

            SqlCommand SqlCmd = new SqlCommand(sSql);

            object oCount = ExecuteScalar(SqlCmd);

            return Convert.ToInt32(oCount);
        }

        #region ExecuteDataTable
        /// <summary>
        /// 执行Sql语句，返回DataTable
        /// 刘洋 2009-02-20
        /// </summary>
        /// <param name="sSqlText"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sSqlText)
        {
            SqlCommand SqlCmd = new SqlCommand(sSqlText);

            return ExecuteDataTable(SqlCmd);
        }
        /// <summary>
        /// 执行Sql语句，返回DataTable
        /// 刘洋 2009-02-20
        /// </summary>
        /// <param name="SqlCmd"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 执行Sql语句，返回DataTable
        /// 刘洋 2009-02-20
        /// </summary>
        /// <param name="SqlCmd"></param>
        /// <param name="connectionStringName"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(SqlCommand SqlCmd, string connectionStringName)
        {
            DataTable NewDataTable = new DataTable();

            using (SqlConnection SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
            {
                SqlCmd.Connection = SqlConn;

                SqlDataAdapter SqlAdapter = new SqlDataAdapter(SqlCmd);

                SqlAdapter.Fill(NewDataTable);
            }

            return NewDataTable;
        }
        /// <summary>
        /// 执行Sql语句，返回DataTable，支持事务
        /// 刘洋 2009-07-18
        /// </summary>
        /// <param name="SqlCmd"></param>
        /// <param name="SqlTrans"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(SqlCommand SqlCmd, SqlTransaction SqlTrans)
        {
            SqlCmd.Connection = SqlTrans.Connection;
            SqlCmd.Transaction = SqlTrans;

            DataTable NewDataTable = new DataTable();

            SqlDataAdapter SqlAdapter = new SqlDataAdapter(SqlCmd);

            SqlAdapter.Fill(NewDataTable);

            return NewDataTable;
        }
        /// <summary>
        /// exec sql by pager
        /// neo 2011-04-29
        /// </summary>
        /// <param name="sDbTable"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="sOrderBy"></param>
        /// <param name="iAscOrDesc"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sDbTable, int iStartIndex, int iEndIndex, string strWhere, string sOrderBy, int iAscOrDesc)
        {
            string sAscOrDesc = string.Empty;
            if (iAscOrDesc == 0)
            {
                sAscOrDesc = "asc";
            }
            else
            {
                sAscOrDesc = "desc";
            }

            SqlCommand SqlCmd = new SqlCommand("lpsp_ExecSqlByPager");
            SqlCmd.CommandType = CommandType.StoredProcedure;
            DbHelperSQL.AddSqlParameter(SqlCmd, "@OrderByField", SqlDbType.NVarChar, sOrderBy);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@AscOrDesc", SqlDbType.VarChar, sAscOrDesc);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Fields", SqlDbType.NVarChar, "*");
            DbHelperSQL.AddSqlParameter(SqlCmd, "@DbTable", SqlDbType.NVarChar, sDbTable);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Where", SqlDbType.NVarChar, strWhere);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@StartIndex", SqlDbType.Int, iStartIndex);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@EndIndex", SqlDbType.Int, iEndIndex);

            return DbHelperSQL.ExecuteDataTable(SqlCmd);
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
