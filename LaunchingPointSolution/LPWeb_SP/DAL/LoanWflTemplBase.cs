using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类LoanWflTempl。
	/// </summary>
    public class LoanWflTemplBase
    {
        public LoanWflTemplBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.LoanWflTempl model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LoanWflTempl(");
            strSql.Append("FileId,WflTemplId,ApplyDate,ApplyBy)");
            strSql.Append(" values (");
            strSql.Append("@FileId,@WflTemplId,@ApplyDate,@ApplyBy)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@WflTemplId", SqlDbType.Int,4),
					new SqlParameter("@ApplyDate", SqlDbType.DateTime),
					new SqlParameter("@ApplyBy", SqlDbType.Int,4)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.WflTemplId;
            parameters[2].Value = model.ApplyDate;
            parameters[3].Value = model.ApplyBy;

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
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.LoanWflTempl model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update LoanWflTempl set ");
            strSql.Append("FileId=@FileId,");
            strSql.Append("WflTemplId=@WflTemplId,");
            strSql.Append("ApplyDate=@ApplyDate,");
            strSql.Append("ApplyBy=@ApplyBy");
            strSql.Append(" where WflTemplId=@WflTemplId and FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@WflTemplId", SqlDbType.Int,4),
					new SqlParameter("@ApplyDate", SqlDbType.DateTime),
					new SqlParameter("@ApplyBy", SqlDbType.Int,4)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.WflTemplId;
            parameters[2].Value = model.ApplyDate;
            parameters[3].Value = model.ApplyBy;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int WflTemplId, int FileId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LoanWflTempl ");
            strSql.Append(" where WflTemplId=@WflTemplId and FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@WflTemplId", SqlDbType.Int,4),
					new SqlParameter("@FileId", SqlDbType.Int,4)};
            parameters[0].Value = WflTemplId;
            parameters[1].Value = FileId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.LoanWflTempl GetModel(int WflTemplId, int FileId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 FileId,WflTemplId,ApplyDate,ApplyBy from LoanWflTempl ");
            strSql.Append(" where WflTemplId=@WflTemplId and FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@WflTemplId", SqlDbType.Int,4),
					new SqlParameter("@FileId", SqlDbType.Int,4)};
            parameters[0].Value = WflTemplId;
            parameters[1].Value = FileId;

            LPWeb.Model.LoanWflTempl model = new LPWeb.Model.LoanWflTempl();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["FileId"].ToString() != "")
                {
                    model.FileId = int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["WflTemplId"].ToString() != "")
                {
                    model.WflTemplId = int.Parse(ds.Tables[0].Rows[0]["WflTemplId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ApplyDate"].ToString() != "")
                {
                    model.ApplyDate = DateTime.Parse(ds.Tables[0].Rows[0]["ApplyDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ApplyBy"].ToString() != "")
                {
                    model.ApplyBy = int.Parse(ds.Tables[0].Rows[0]["ApplyBy"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select FileId,WflTemplId,ApplyDate,ApplyBy ");
            strSql.Append(" FROM LoanWflTempl ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" FileId,WflTemplId,ApplyDate,ApplyBy ");
            strSql.Append(" FROM LoanWflTempl ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        /// 分页获取数据列表
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
            parameters[0].Value = "LoanWflTempl";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage", parameters, "ds");
        }

        #endregion  成员方法
	}
}

