using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类LoanPointFields。
	/// </summary>
	public class LoanPointFieldsBase
    {
        public LoanPointFieldsBase()
		{}
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.LoanPointFields model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LoanPointFields(");
            strSql.Append("FileId,PointFieldId,PrevValue,CurrentValue)");
            strSql.Append(" values (");
            strSql.Append("@FileId,@PointFieldId,@PrevValue,@CurrentValue)");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@PointFieldId", SqlDbType.Int,4),
					new SqlParameter("@PrevValue", SqlDbType.NVarChar,500),
					new SqlParameter("@CurrentValue", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.PointFieldId;
            parameters[2].Value = model.PrevValue;
            parameters[3].Value = model.CurrentValue;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.LoanPointFields model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update LoanPointFields set ");
            strSql.Append("FileId=@FileId,");
            strSql.Append("PointFieldId=@PointFieldId,");
            strSql.Append("PrevValue=@PrevValue,");
            strSql.Append("CurrentValue=@CurrentValue");
            strSql.Append(" where FileId=@FileId and PointFieldId=@PointFieldId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@PointFieldId", SqlDbType.Int,4),
					new SqlParameter("@PrevValue", SqlDbType.NVarChar,500),
					new SqlParameter("@CurrentValue", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.PointFieldId;
            parameters[2].Value = model.PrevValue;
            parameters[3].Value = model.CurrentValue;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int FileId, int PointFieldId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LoanPointFields ");
            strSql.Append(" where FileId=@FileId and PointFieldId=@PointFieldId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@PointFieldId", SqlDbType.Int,4)};
            parameters[0].Value = FileId;
            parameters[1].Value = PointFieldId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.LoanPointFields GetModel(int FileId, int PointFieldId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 FileId,PointFieldId,PrevValue,CurrentValue from LoanPointFields ");
            strSql.Append(" where FileId=@FileId and PointFieldId=@PointFieldId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@PointFieldId", SqlDbType.Int,4)};
            parameters[0].Value = FileId;
            parameters[1].Value = PointFieldId;

            LPWeb.Model.LoanPointFields model = new LPWeb.Model.LoanPointFields();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["FileId"].ToString() != "")
                {
                    model.FileId = int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PointFieldId"].ToString() != "")
                {
                    model.PointFieldId = int.Parse(ds.Tables[0].Rows[0]["PointFieldId"].ToString());
                }
                model.PrevValue = ds.Tables[0].Rows[0]["PrevValue"].ToString();
                model.CurrentValue = ds.Tables[0].Rows[0]["CurrentValue"].ToString();
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
            strSql.Append("select FileId,PointFieldId,PrevValue,CurrentValue ");
            strSql.Append(" FROM LoanPointFields ");
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
            strSql.Append(" FileId,PointFieldId,PrevValue,CurrentValue ");
            strSql.Append(" FROM LoanPointFields ");
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
            parameters[0].Value = "LoanPointFields";
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

