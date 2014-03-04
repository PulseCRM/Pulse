using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����RegionExecutives��
	/// </summary>
	public class RegionExecutivesBase
    {
        public RegionExecutivesBase()
        { }
        #region  ��Ա����

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(LPWeb.Model.RegionExecutives model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into RegionExecutives(");
            strSql.Append("RegionId,ExecutiveId)");
            strSql.Append(" values (");
            strSql.Append("@RegionId,@ExecutiveId)");
            SqlParameter[] parameters = {
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@ExecutiveId", SqlDbType.Int,4)};
            parameters[0].Value = model.RegionId;
            parameters[1].Value = model.ExecutiveId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(LPWeb.Model.RegionExecutives model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update RegionExecutives set ");
            strSql.Append("RegionId=@RegionId,");
            strSql.Append("ExecutiveId=@ExecutiveId");
            strSql.Append(" where RegionId=@RegionId and ExecutiveId=@ExecutiveId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@ExecutiveId", SqlDbType.Int,4)};
            parameters[0].Value = model.RegionId;
            parameters[1].Value = model.ExecutiveId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int RegionId, int ExecutiveId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from RegionExecutives ");
            strSql.Append(" where RegionId=@RegionId and ExecutiveId=@ExecutiveId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@ExecutiveId", SqlDbType.Int,4)};
            parameters[0].Value = RegionId;
            parameters[1].Value = ExecutiveId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.RegionExecutives GetModel(int RegionId, int ExecutiveId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 RegionId,ExecutiveId from RegionExecutives ");
            strSql.Append(" where RegionId=@RegionId and ExecutiveId=@ExecutiveId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@ExecutiveId", SqlDbType.Int,4)};
            parameters[0].Value = RegionId;
            parameters[1].Value = ExecutiveId;

            LPWeb.Model.RegionExecutives model = new LPWeb.Model.RegionExecutives();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["RegionId"].ToString() != "")
                {
                    model.RegionId = int.Parse(ds.Tables[0].Rows[0]["RegionId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ExecutiveId"].ToString() != "")
                {
                    model.ExecutiveId = int.Parse(ds.Tables[0].Rows[0]["ExecutiveId"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select RegionId,ExecutiveId ");
            //strSql.Append(" FROM RegionExecutives ");
            string sql = "Select * FROM (" +
                "SELECT" +
" RegionExecutives.RegionId," +
" RegionExecutives.ExecutiveId," +
" Users.UserId, " +
" Users.Username," +
" Users.EmailAddress," +
" Users.UserEnabled, " +
" Users.FirstName," +
" Users.LastName," +
" Users.RoleId," +
" Users.UserPictureFile" +
" FROM " +
" Users " +
" INNER JOIN" +
" RegionExecutives " +
" ON " +
" Users.UserId = RegionExecutives.ExecutiveId ) AS tmp";
            strSql.Append(sql);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// ���ǰ��������
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" RegionId,ExecutiveId ");
            strSql.Append(" FROM RegionExecutives ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        /// ��ҳ��ȡ�����б�
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
            parameters[0].Value = "RegionExecutives";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage", parameters, "ds");
        }

        #endregion  ��Ա����
	}
}

