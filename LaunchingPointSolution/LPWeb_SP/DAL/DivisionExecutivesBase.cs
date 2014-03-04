using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����DivisionExecutives��
	/// </summary>
	public class DivisionExecutivesBase
    {
        public DivisionExecutivesBase()
		{}
        #region  ��Ա����

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(LPWeb.Model.DivisionExecutives model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into DivisionExecutives(");
            strSql.Append("DivisionId,ExecutiveId)");
            strSql.Append(" values (");
            strSql.Append("@DivisionId,@ExecutiveId)");
            SqlParameter[] parameters = {
					new SqlParameter("@DivisionId", SqlDbType.Int,4),
					new SqlParameter("@ExecutiveId", SqlDbType.Int,4)};
            parameters[0].Value = model.DivisionId;
            parameters[1].Value = model.ExecutiveId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(LPWeb.Model.DivisionExecutives model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update DivisionExecutives set ");
            strSql.Append("DivisionId=@DivisionId,");
            strSql.Append("ExecutiveId=@ExecutiveId");
            strSql.Append(" where DivisionId=@DivisionId and ExecutiveId=@ExecutiveId ");
            SqlParameter[] parameters = {
					new SqlParameter("@DivisionId", SqlDbType.Int,4),
					new SqlParameter("@ExecutiveId", SqlDbType.Int,4)};
            parameters[0].Value = model.DivisionId;
            parameters[1].Value = model.ExecutiveId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int DivisionId, int ExecutiveId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from DivisionExecutives ");
            strSql.Append(" where DivisionId=@DivisionId and ExecutiveId=@ExecutiveId ");
            SqlParameter[] parameters = {
					new SqlParameter("@DivisionId", SqlDbType.Int,4),
					new SqlParameter("@ExecutiveId", SqlDbType.Int,4)};
            parameters[0].Value = DivisionId;
            parameters[1].Value = ExecutiveId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.DivisionExecutives GetModel(int DivisionId, int ExecutiveId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 DivisionId,ExecutiveId from DivisionExecutives ");
            strSql.Append(" where DivisionId=@DivisionId and ExecutiveId=@ExecutiveId ");
            SqlParameter[] parameters = {
					new SqlParameter("@DivisionId", SqlDbType.Int,4),
					new SqlParameter("@ExecutiveId", SqlDbType.Int,4)};
            parameters[0].Value = DivisionId;
            parameters[1].Value = ExecutiveId;

            LPWeb.Model.DivisionExecutives model = new LPWeb.Model.DivisionExecutives();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["DivisionId"].ToString() != "")
                {
                    model.DivisionId = int.Parse(ds.Tables[0].Rows[0]["DivisionId"].ToString());
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
            strSql.Append("select DivisionId,ExecutiveId ");
            strSql.Append(" FROM DivisionExecutives ");
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
            strSql.Append(" DivisionId,ExecutiveId ");
            strSql.Append(" FROM DivisionExecutives ");
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
            parameters[0].Value = "DivisionExecutives";
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

