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
    /// ���ݷ��ʳ��������
    /// </summary>
    public abstract class DbHelperSQL
    {
        //���ݿ������ַ���(web.config������)�����Զ�̬����connectionString֧�ֶ����ݿ�.		
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

        #region ���÷���
        /// <summary>
        /// �ж��Ƿ����ĳ���ĳ���ֶ�
        /// </summary>
        /// <param name="tableName">������</param>
        /// <param name="columnName">������</param>
        /// <returns>�Ƿ����</returns>
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
        /// ���Ƿ����
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

        #region  ִ�м�SQL���

        /// <summary>
        /// ִ��SQL��䣬����Ӱ��ļ�¼��
        /// </summary>
        /// <param name="SQLString">SQL���</param>
        /// <returns>Ӱ��ļ�¼��</returns>
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
        ///// ִ��Sql��Oracle�λ������
        ///// </summary>
        ///// <param name="list">SQL�������б�</param>
        ///// <param name="oracleCmdSqlList">Oracle�������б�</param>
        ///// <returns>ִ�н�� 0-����SQL�������ʧ�� -1 ����Oracle�������ʧ�� 1-��������ִ�гɹ�</returns>
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
        //                        throw new Exception("Υ��Ҫ��"+myDE.CommandText+"�������select count(..�ĸ�ʽ");
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
        //                        //�����¼�
        //                        myDE.OnSolicitationEvent();
        //                    }
        //                }
        //                if (myDE.EffentNextType == EffentNextType.WhenHaveContine || myDE.EffentNextType == EffentNextType.WhenNoHaveContine)
        //                {
        //                    if (myDE.CommandText.ToLower().IndexOf("count(") == -1)
        //                    {
        //                        tx.Rollback();
        //                        throw new Exception("SQL:Υ��Ҫ��" + myDE.CommandText + "�������select count(..�ĸ�ʽ");
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
        //                        throw new Exception("SQL:Υ��Ҫ��" + myDE.CommandText + "����ֵ�������0");
        //                        //return 0;
        //                    }
        //                    if (myDE.EffentNextType == EffentNextType.WhenNoHaveContine && isHave)
        //                    {
        //                        tx.Rollback();
        //                        throw new Exception("SQL:Υ��Ҫ��" + myDE.CommandText + "����ֵ�������0");
        //                        //return 0;
        //                    }
        //                    continue;
        //                }
        //                int val = cmd.ExecuteNonQuery();
        //                if (myDE.EffentNextType == EffentNextType.ExcuteEffectRows && val == 0)
        //                {
        //                    tx.Rollback();
        //                    throw new Exception("SQL:Υ��Ҫ��" + myDE.CommandText + "������Ӱ����");
        //                    //return 0;
        //                }
        //                cmd.Parameters.Clear();
        //            }
        //            string oraConnectionString = PubConstant.GetConnectionString("ConnectionStringPPC");
        //            bool res = OracleHelper.ExecuteSqlTran(oraConnectionString, oracleCmdSqlList);
        //            if (!res)
        //            {
        //                tx.Rollback();
        //                throw new Exception("Oracleִ��ʧ��");
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
        /// ִ�ж���SQL��䣬ʵ�����ݿ�����
        /// </summary>
        /// <param name="SQLStringList">����SQL���</param>		
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
        /// ִ�д�һ���洢���̲����ĵ�SQL��䡣
        /// </summary>
        /// <param name="SQLString">SQL���</param>
        /// <param name="content">��������,����һ���ֶ��Ǹ�ʽ���ӵ����£���������ţ�����ͨ�������ʽ���</param>
        /// <returns>Ӱ��ļ�¼��</returns>
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
        /// ִ�д�һ���洢���̲����ĵ�SQL��䡣
        /// </summary>
        /// <param name="SQLString">SQL���</param>
        /// <param name="content">��������,����һ���ֶ��Ǹ�ʽ���ӵ����£���������ţ�����ͨ�������ʽ���</param>
        /// <returns>Ӱ��ļ�¼��</returns>
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
        /// �����ݿ������ͼ���ʽ���ֶ�(������������Ƶ���һ��ʵ��)
        /// </summary>
        /// <param name="strSQL">SQL���</param>
        /// <param name="fs">ͼ���ֽ�,���ݿ���ֶ�����Ϊimage�����</param>
        /// <returns>Ӱ��ļ�¼��</returns>
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
        /// ִ��һ�������ѯ�����䣬���ز�ѯ�����object����
        /// </summary>
        /// <param name="SQLString">�����ѯ������</param>
        /// <returns>��ѯ�����object��</returns>
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
        /// ִ�в�ѯ��䣬����SqlDataReader ( ע�⣺���ø÷�����һ��Ҫ��SqlDataReader����Close )
        /// </summary>
        /// <param name="strSQL">��ѯ���</param>
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
        /// ִ�в�ѯ��䣬����DataSet
        /// </summary>
        /// <param name="SQLString">��ѯ���</param>
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

        #region ִ�д�������SQL���

        /// <summary>
        /// ִ��SQL��䣬����Ӱ��ļ�¼��
        /// </summary>
        /// <param name="SQLString">SQL���</param>
        /// <returns>Ӱ��ļ�¼��</returns>
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
        /// ִ�ж���SQL��䣬ʵ�����ݿ�����
        /// </summary>
        /// <param name="SQLStringList">SQL���Ĺ�ϣ��keyΪsql��䣬value�Ǹ�����SqlParameter[]��</param>
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
                        //ѭ��
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
        ///// ִ�ж���SQL��䣬ʵ�����ݿ�����
        ///// </summary>
        ///// <param name="SQLStringList">SQL���Ĺ�ϣ��keyΪsql��䣬value�Ǹ�����SqlParameter[]��</param>
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
        //                //ѭ��
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
        ///// ִ�ж���SQL��䣬ʵ�����ݿ�����
        ///// </summary>
        ///// <param name="SQLStringList">SQL���Ĺ�ϣ��keyΪsql��䣬value�Ǹ�����SqlParameter[]��</param>
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
        //                //ѭ��
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
        /// ִ�ж���SQL��䣬ʵ�����ݿ�����
        /// </summary>
        /// <param name="SQLStringList">SQL���Ĺ�ϣ��keyΪsql��䣬value�Ǹ�����SqlParameter[]��</param>
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
                        //ѭ��
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
        /// ִ��һ�������ѯ�����䣬���ز�ѯ�����object����
        /// </summary>
        /// <param name="SQLString">�����ѯ������</param>
        /// <returns>��ѯ�����object��</returns>
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
        /// ִ�в�ѯ��䣬����SqlDataReader ( ע�⣺���ø÷�����һ��Ҫ��SqlDataReader����Close )
        /// </summary>
        /// <param name="strSQL">��ѯ���</param>
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
        /// ִ�в�ѯ��䣬����DataSet
        /// </summary>
        /// <param name="SQLString">��ѯ���</param>
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

        #region �洢���̲���

        /// <summary>
        /// ִ�д洢���̣�����SqlDataReader ( ע�⣺���ø÷�����һ��Ҫ��SqlDataReader����Close )
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
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
        /// ִ�д洢����
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <param name="tableName">DataSet����еı���</param>
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
        /// ���� SqlCommand ����(��������һ���������������һ������ֵ)
        /// </summary>
        /// <param name="connection">���ݿ�����</param>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <returns>SqlCommand</returns>
        public static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // ���δ����ֵ���������,���������DBNull.Value.
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
        /// ִ�д洢���̣�����Ӱ�������		
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <param name="rowsAffected">Ӱ�������</param>
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
        /// ���� SqlCommand ����ʵ��(��������һ������ֵ)	
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <returns>SqlCommand ����ʵ��</returns>
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
        /// ��ȡ��������
        /// ���� 2010-02-06
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
        /// ִ��Sql��䣬����DataTable
        /// ���� 2009-02-20
        /// </summary>
        /// <param name="sSqlText"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sSqlText)
        {
            SqlCommand SqlCmd = new SqlCommand(sSqlText);

            return ExecuteDataTable(SqlCmd);
        }
        /// <summary>
        /// ִ��Sql��䣬����DataTable
        /// ���� 2009-02-20
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
        /// ִ��Sql��䣬����DataTable
        /// ���� 2009-02-20
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
        /// ִ��Sql��䣬����DataTable��֧������
        /// ���� 2009-07-18
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
        /// ִ��Sql��䣬����Scalar
        /// ���� 2009-02-20
        /// </summary>
        /// <param name="sSqlText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sSqlText)
        {
            SqlCommand SqlCmd = new SqlCommand(sSqlText);

            return ExecuteScalar(SqlCmd);
        }
        /// <summary>
        /// ִ��Sql��䣬����Scalar
        /// ���� 2009-02-20
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
        /// ִ��Sql��䣬����Scalar
        /// ���� 2009-04-30
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
        /// ִ��Sql��䣬����Scalar
        /// ���� 2009-04-30
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
        /// ִ��Sql��䣬����Ӱ���¼��
        /// ���� 2009-02-20
        /// </summary>
        /// <param name="sSqlText"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sSqlText)
        {
            SqlCommand SqlCmd = new SqlCommand(sSqlText);

            return ExecuteNonQuery(SqlCmd);
        }
        /// <summary>
        /// ִ��Sql��䣬����Ӱ���¼��
        /// ���� 2009-02-20
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
        /// ִ��Sql��䣬����Ӱ���¼��(֧������)
        /// ���� 2009-02-20
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
        /// ִ��Sql��䣬����Ӱ���¼��
        /// ���� 2009-02-20
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
        /// ִ��Sql��䣬����SqlDataReader
        /// ���� 2009-02-20
        /// </summary>
        /// <param name="sSqlText"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteDataReader(string sSqlText)
        {
            SqlCommand SqlCmd = new SqlCommand(sSqlText);

            return ExecuteDataReader(SqlCmd);
        }
        /// <summary>
        /// ִ��Sql��䣬����SqlDataReader
        /// ���� 2009-02-20
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
        /// �ر�SqlDataReader��SqlConnection
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
        /// ��������DataTable
        /// ���� 2009-02-20
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
        /// ��������DataTable(֧������)
        /// ���� 2009-02-20
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
        /// ���SqlParameter
        /// ���� 2009-02-20
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
        /// ���SqlParameter
        /// ���� 2009-02-20
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
        /// ���Sql Parameter
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
        /// ���Sql Parameter
        /// ���� 2009-04-14
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
        /// ��ȡOpen��SqlConnection
        /// ���� 2009-02-20
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetOpenConnection()
        {
            SqlConnection SqlConn = new SqlConnection(connectionString);
            SqlConn.Open();
            return SqlConn;
        }
        /// <summary>
        /// ��ȡOpen��SqlConnection
        /// ���� 2009-02-20
        /// </summary>
        /// <param name="sConnStringName">�����ַ�����</param>
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
