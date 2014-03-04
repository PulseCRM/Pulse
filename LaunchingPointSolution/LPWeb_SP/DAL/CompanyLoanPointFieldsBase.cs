using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:CompanyLoanPointFields
    /// </summary>
    public class CompanyLoanPointFieldsBase
    {
        public CompanyLoanPointFieldsBase()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int PointFieldId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from CompanyLoanPointFields");
            strSql.Append(" where PointFieldId=@PointFieldId ");
            SqlParameter[] parameters = {
					new SqlParameter("@PointFieldId", SqlDbType.Int,4)};
            parameters[0].Value = PointFieldId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.CompanyLoanPointFields model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into CompanyLoanPointFields(");
            strSql.Append("PointFieldId,Heading)");
            strSql.Append(" values (");
            strSql.Append("@PointFieldId,@Heading)");
            SqlParameter[] parameters = {
					new SqlParameter("@PointFieldId", SqlDbType.Int,4),
					new SqlParameter("@Heading", SqlDbType.NVarChar,255)};
            parameters[0].Value = model.PointFieldId;
            parameters[1].Value = model.Heading;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.CompanyLoanPointFields model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CompanyLoanPointFields set ");
            strSql.Append("Heading=@Heading");
            strSql.Append(" where PointFieldId=@PointFieldId ");
            SqlParameter[] parameters = {
					new SqlParameter("@PointFieldId", SqlDbType.Int,4),
					new SqlParameter("@Heading", SqlDbType.NVarChar,255)};
            parameters[0].Value = model.PointFieldId;
            parameters[1].Value = model.Heading;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int PointFieldId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from CompanyLoanPointFields ");
            strSql.Append(" where PointFieldId=@PointFieldId ");
            SqlParameter[] parameters = {
					new SqlParameter("@PointFieldId", SqlDbType.Int,4)};
            parameters[0].Value = PointFieldId;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string PointFieldIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from CompanyLoanPointFields ");
            strSql.Append(" where PointFieldId in (" + PointFieldIdlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.CompanyLoanPointFields GetModel(int PointFieldId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 PointFieldId,Heading from CompanyLoanPointFields ");
            strSql.Append(" where PointFieldId=@PointFieldId ");
            SqlParameter[] parameters = {
					new SqlParameter("@PointFieldId", SqlDbType.Int,4)};
            parameters[0].Value = PointFieldId;

            LPWeb.Model.CompanyLoanPointFields model = new LPWeb.Model.CompanyLoanPointFields();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["PointFieldId"].ToString() != "")
                {
                    model.PointFieldId = int.Parse(ds.Tables[0].Rows[0]["PointFieldId"].ToString());
                }
                model.Heading = ds.Tables[0].Rows[0]["Heading"].ToString();
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
            strSql.Append("select PointFieldId,Heading ");
            strSql.Append(" FROM CompanyLoanPointFields ");
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
            strSql.Append(" PointFieldId,Heading ");
            strSql.Append(" FROM CompanyLoanPointFields ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
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
            parameters[0].Value = "CompanyLoanPointFields";
            parameters[1].Value = "";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  Method
    }
}

