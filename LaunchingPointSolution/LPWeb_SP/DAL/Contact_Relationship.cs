using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;

namespace LPWeb.DAL
{
    public class Contact_Relationship
    {
        #region neo

        /// <summary>
        /// delete contact relationship
        /// neo 2011-03-15
        /// </summary>
        /// <param name="iContactID"></param>
        /// <param name="DelContactIDArray"></param>
        /// <param name="DirectionArray"></param>
        public void DeleteContactRelationshipBase(int iContactID, string[] DelContactIDArray, string[] DirectionArray)
        {
            string sSql = "delete from Contact_Relationship where FromContactId=@FromContactId and ToContactId=@ToContactId";
            Collection<SqlCommand> DeleteSqlCmds = new Collection<SqlCommand>();
            for (int i = 0; i < DelContactIDArray.Length; i++)
            {
                int iDelContactID = Convert.ToInt32(DelContactIDArray[i]);
                string sDirection = DirectionArray[i];

                SqlCommand SqlCmd = new SqlCommand(sSql);

                if (sDirection == "To")
                {
                    LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "FromContactId", SqlDbType.Int, iContactID);
                    LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "ToContactId", iDelContactID);
                }
                else if (sDirection == "From")
                {
                    LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "FromContactId", SqlDbType.Int, iDelContactID);
                    LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "ToContactId", iContactID);
                }

                DeleteSqlCmds.Add(SqlCmd);
            }

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = LPWeb.DAL.DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                foreach (SqlCommand InsertSqlCmd in DeleteSqlCmds)
                {
                    LPWeb.DAL.DbHelperSQL.ExecuteScalar(InsertSqlCmd, SqlTrans);
                }

                SqlTrans.Commit();
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
        /// insert contact relationship
        /// neo 2011-03-15
        /// </summary>
        /// <param name="iFromContactID"></param>
        /// <param name="ToContactIDArray"></param>
        /// <param name="RelationshipTypeArray"></param>
        public void InsertContactRelationshipBase(int iFromContactID, string[] ToContactIDArray, string[] RelationshipTypeArray)
        {
            string sSql = "insert into Contact_Relationship (FromContactId, ToContactId, RelTypeId) values (@FromContactId, @ToContactId, @RelTypeId)";
            Collection<SqlCommand> InsertSqlCmds = new Collection<SqlCommand>();
            for (int i = 0; i < ToContactIDArray.Length; i++)
            {
                int iToContactID = Convert.ToInt32(ToContactIDArray[i]);
                int iRelationshipTypeID = Convert.ToInt32(RelationshipTypeArray[i]);

                SqlCommand SqlCmd = new SqlCommand(sSql);
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "FromContactId", SqlDbType.Int, iFromContactID);
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "ToContactId", SqlDbType.Int, iToContactID);
                LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "RelTypeId", SqlDbType.Int, iRelationshipTypeID);

                InsertSqlCmds.Add(SqlCmd);
            }

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = LPWeb.DAL.DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                foreach (SqlCommand InsertSqlCmd in InsertSqlCmds)
                {
                    LPWeb.DAL.DbHelperSQL.ExecuteScalar(InsertSqlCmd, SqlTrans);
                }

                SqlTrans.Commit();
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
    }
}
