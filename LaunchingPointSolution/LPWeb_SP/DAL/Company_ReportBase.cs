using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace LPWeb.DAL
{
    public class Company_ReportBase
    {
        public Company_ReportBase()
        { }
        #region  Method

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.Company_Report model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Company_Report(");
            strSql.Append("DOW,TOD,SenderRoleId,SenderEmail,SenderName)");
            strSql.Append(" values (");
            strSql.Append("@DOW,@TOD,@SenderRoleId,@SenderEmail,@SenderName)");
            SqlParameter[] parameters = {
					new SqlParameter("@DOW", SqlDbType.SmallInt,2),
					new SqlParameter("@TOD", SqlDbType.SmallInt,2),
					new SqlParameter("@SenderRoleId", SqlDbType.Int,4),
					new SqlParameter("@SenderEmail", SqlDbType.NVarChar,255),
					new SqlParameter("@SenderName", SqlDbType.NVarChar,255)};
            parameters[0].Value = model.DOW;
            parameters[1].Value = model.TOD;
            parameters[2].Value = model.SenderRoleId;
            parameters[3].Value = model.SenderEmail;
            parameters[4].Value = model.SenderName;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.Company_Report model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Company_Report set ");
            strSql.Append("DOW=@DOW,");
            strSql.Append("TOD=@TOD,");
            strSql.Append("SenderRoleId=@SenderRoleId,");
            strSql.Append("SenderEmail=@SenderEmail,");
            strSql.Append("SenderName=@SenderName");
            //strSql.Append(" where ");
            SqlParameter[] parameters = {
					new SqlParameter("@DOW", SqlDbType.SmallInt,2),
					new SqlParameter("@TOD", SqlDbType.SmallInt,2),
					new SqlParameter("@SenderRoleId", SqlDbType.Int,4),
					new SqlParameter("@SenderEmail", SqlDbType.NVarChar,255),
					new SqlParameter("@SenderName", SqlDbType.NVarChar,255)};
            parameters[0].Value = model.DOW;
            parameters[1].Value = model.TOD;
            parameters[2].Value = model.SenderRoleId;
            parameters[3].Value = model.SenderEmail;
            parameters[4].Value = model.SenderName;

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
        public bool Delete()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Company_Report ");
            strSql.Append(" where ");
            SqlParameter[] parameters = {
};

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
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Company_Report GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 DOW,TOD,SenderRoleId,SenderEmail,SenderName from Company_Report ");

            LPWeb.Model.Company_Report model = new LPWeb.Model.Company_Report();
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["DOW"].ToString() != "")
                {
                    model.DOW = int.Parse(ds.Tables[0].Rows[0]["DOW"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TOD"].ToString() != "")
                {
                    model.TOD = int.Parse(ds.Tables[0].Rows[0]["TOD"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SenderRoleId"].ToString() != "")
                {
                    model.SenderRoleId = int.Parse(ds.Tables[0].Rows[0]["SenderRoleId"].ToString());
                }
                model.SenderEmail = ds.Tables[0].Rows[0]["SenderEmail"].ToString();
                model.SenderName = ds.Tables[0].Rows[0]["SenderName"].ToString();
                return model;
            }
            else
            {
                return null;
            }
        }
        #endregion  Method
    }
}
