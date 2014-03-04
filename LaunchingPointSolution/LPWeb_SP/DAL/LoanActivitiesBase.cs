using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����LoanActivities��
	/// </summary>
    public class LoanActivitiesBase
    {
        public LoanActivitiesBase()
		{}
        #region  ��Ա����

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(LPWeb.Model.LoanActivities model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LoanActivities(");
            strSql.Append("FileId,UserId,ActivityName,ActivityTime)");
            strSql.Append(" values (");
            strSql.Append("@FileId,@UserId,@ActivityName,@ActivityTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@ActivityName", SqlDbType.NVarChar,255),
					new SqlParameter("@ActivityTime", SqlDbType.DateTime)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.UserId;
            parameters[2].Value = model.ActivityName;
            parameters[3].Value = model.ActivityTime;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(LPWeb.Model.LoanActivities model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update LoanActivities set ");
            strSql.Append("ActivityId=@ActivityId,");
            strSql.Append("FileId=@FileId,");
            strSql.Append("UserId=@UserId,");
            strSql.Append("ActivityName=@ActivityName,");
            strSql.Append("ActivityTime=@ActivityTime");
            strSql.Append(" where ActivityId=@ActivityId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ActivityId", SqlDbType.Int,4),
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@ActivityName", SqlDbType.NVarChar,255),
					new SqlParameter("@ActivityTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ActivityId;
            parameters[1].Value = model.FileId;
            parameters[2].Value = model.UserId;
            parameters[3].Value = model.ActivityName;
            parameters[4].Value = model.ActivityTime;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ActivityId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LoanActivities ");
            strSql.Append(" where ActivityId=@ActivityId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ActivityId", SqlDbType.Int,4)};
            parameters[0].Value = ActivityId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.LoanActivities GetModel(int ActivityId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ActivityId,FileId,UserId,ActivityName,ActivityTime from LoanActivities ");
            strSql.Append(" where ActivityId=@ActivityId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ActivityId", SqlDbType.Int,4)};
            parameters[0].Value = ActivityId;

            LPWeb.Model.LoanActivities model = new LPWeb.Model.LoanActivities();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ActivityId"].ToString() != "")
                {
                    model.ActivityId = int.Parse(ds.Tables[0].Rows[0]["ActivityId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FileId"].ToString() != "")
                {
                    model.FileId = int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
                }
                model.ActivityName = ds.Tables[0].Rows[0]["ActivityName"].ToString();
                if (ds.Tables[0].Rows[0]["ActivityTime"].ToString() != "")
                {
                    model.ActivityTime = DateTime.Parse(ds.Tables[0].Rows[0]["ActivityTime"].ToString());
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
            strSql.Append("select ActivityId,FileId,UserId,ActivityName,ActivityTime ");
            strSql.Append(" FROM LoanActivities ");
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
            strSql.Append(" ActivityId,FileId,UserId,ActivityName,ActivityTime ");
            strSql.Append(" FROM LoanActivities ");
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
            parameters[0].Value = "LoanActivities";
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

