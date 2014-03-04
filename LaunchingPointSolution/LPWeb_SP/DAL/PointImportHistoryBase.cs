using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类PointImportHistory。
	/// </summary>
    public class PointImportHistoryBase
    {
        public PointImportHistoryBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.PointImportHistory model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PointImportHistory(");
            strSql.Append("FileId,ImportTime,Success,Error)");
            strSql.Append(" values (");
            strSql.Append("@FileId,@ImportTime,@Success,@Error)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@ImportTime", SqlDbType.DateTime),
					new SqlParameter("@Success", SqlDbType.Bit,1),
					new SqlParameter("@Error", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.ImportTime;
            parameters[2].Value = model.Success;
            parameters[3].Value = model.Error;

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
        public void Update(LPWeb.Model.PointImportHistory model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update PointImportHistory set ");
            strSql.Append("HistoryId=@HistoryId,");
            strSql.Append("FileId=@FileId,");
            strSql.Append("ImportTime=@ImportTime,");
            strSql.Append("Success=@Success,");
            strSql.Append("Error=@Error");
            strSql.Append(" where HistoryId=@HistoryId ");
            SqlParameter[] parameters = {
					new SqlParameter("@HistoryId", SqlDbType.Int,4),
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@ImportTime", SqlDbType.DateTime),
					new SqlParameter("@Success", SqlDbType.Bit,1),
					new SqlParameter("@Error", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.HistoryId;
            parameters[1].Value = model.FileId;
            parameters[2].Value = model.ImportTime;
            parameters[3].Value = model.Success;
            parameters[4].Value = model.Error;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int HistoryId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from PointImportHistory ");
            strSql.Append(" where HistoryId=@HistoryId ");
            SqlParameter[] parameters = {
					new SqlParameter("@HistoryId", SqlDbType.Int,4)};
            parameters[0].Value = HistoryId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.PointImportHistory GetModel(int HistoryId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 HistoryId,FileId,ImportTime,Success,Error from PointImportHistory ");
            strSql.Append(" where HistoryId=@HistoryId ");
            SqlParameter[] parameters = {
					new SqlParameter("@HistoryId", SqlDbType.Int,4)};
            parameters[0].Value = HistoryId;

            LPWeb.Model.PointImportHistory model = new LPWeb.Model.PointImportHistory();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["HistoryId"].ToString() != "")
                {
                    model.HistoryId = int.Parse(ds.Tables[0].Rows[0]["HistoryId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FileId"].ToString() != "")
                {
                    model.FileId = int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ImportTime"].ToString() != "")
                {
                    model.ImportTime = DateTime.Parse(ds.Tables[0].Rows[0]["ImportTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Success"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Success"].ToString() == "1") || (ds.Tables[0].Rows[0]["Success"].ToString().ToLower() == "true"))
                    {
                        model.Success = true;
                    }
                    else
                    {
                        model.Success = false;
                    }
                }
                model.Error = ds.Tables[0].Rows[0]["Error"].ToString();
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
            strSql.Append("select TOP 500 HistoryId,FileId,ImportTime,Success,Error,CASE Success WHEN 0 THEN 'failure' ELSE 'success' END AS StatusName ");
            strSql.Append(" FROM PointImportHistory ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" ORDER BY ImportTime DESC");
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
            strSql.Append(" HistoryId,FileId,ImportTime,Success,Error ");
            strSql.Append(" FROM PointImportHistory ");
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
            parameters[0].Value = "PointImportHistory";
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

