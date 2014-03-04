using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类PointFieldDesc。
	/// </summary>
	public class PointFieldDescBase
    {
        public PointFieldDescBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.PointFieldDesc model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PointFieldDesc(");
            strSql.Append("PointFieldId,Label,DataType)");
            strSql.Append(" values (");
            strSql.Append("@PointFieldId,@Label,@DataType)");
            SqlParameter[] parameters = {
					new SqlParameter("@PointFieldId", SqlDbType.Decimal,5),
					new SqlParameter("@Label", SqlDbType.NVarChar,50),
					new SqlParameter("@DataType", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.PointFieldId;
            parameters[1].Value = model.Label;
            parameters[2].Value = model.DataType;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.PointFieldDesc model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update PointFieldDesc set ");
            strSql.Append("PointFieldId=@PointFieldId,");
            strSql.Append("Label=@Label,");
            strSql.Append("DataType=@DataType");
            strSql.Append(" where PointFieldId=@PointFieldId ");
            SqlParameter[] parameters = {
					new SqlParameter("@PointFieldId", SqlDbType.Decimal,5),
					new SqlParameter("@Label", SqlDbType.NVarChar,50),
					new SqlParameter("@DataType", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.PointFieldId;
            parameters[1].Value = model.Label;
            parameters[2].Value = model.DataType;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(decimal PointFieldId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from PointFieldDesc ");
            strSql.Append(" where PointFieldId=@PointFieldId ");
            SqlParameter[] parameters = {
					new SqlParameter("@PointFieldId", SqlDbType.Decimal)};
            parameters[0].Value = PointFieldId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.PointFieldDesc GetModel(decimal PointFieldId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 PointFieldId,Label,DataType from PointFieldDesc ");
            strSql.Append(" where PointFieldId=@PointFieldId ");
            SqlParameter[] parameters = {
					new SqlParameter("@PointFieldId", SqlDbType.Decimal)};
            parameters[0].Value = PointFieldId;

            LPWeb.Model.PointFieldDesc model = new LPWeb.Model.PointFieldDesc();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["PointFieldId"].ToString() != "")
                {
                    model.PointFieldId = decimal.Parse(ds.Tables[0].Rows[0]["PointFieldId"].ToString());
                }
                model.Label = ds.Tables[0].Rows[0]["Label"].ToString();
                if (ds.Tables[0].Rows[0]["DataType"].ToString() != "")
                {
                    model.DataType = int.Parse(ds.Tables[0].Rows[0]["DataType"].ToString());
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
            strSql.Append("select PointFieldId,Label,DataType ");
            strSql.Append(" FROM PointFieldDesc ");
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
            strSql.Append(" PointFieldId,Label,DataType ");
            strSql.Append(" FROM PointFieldDesc ");
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
            parameters[0].Value = "PointFieldDesc";
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

