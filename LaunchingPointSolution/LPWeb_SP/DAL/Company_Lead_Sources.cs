using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Company_Lead_Sources。
    /// </summary>
    public class Company_Lead_Sources : Company_Lead_SourcesBase
    {
        public Company_Lead_Sources()
		{}
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Company_Lead_Sources model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Company_Lead_Sources(");
            strSql.Append("LeadSource, DefaultUserId, [Default])");
            strSql.Append(" values (");
            strSql.Append("@LeadSource, @DefaultUserId, @Default)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					                    new SqlParameter("@LeadSource", SqlDbType.NVarChar,150),
                                        new SqlParameter("@DefaultUserId", SqlDbType.Int,4),
                                        new SqlParameter("@Default", SqlDbType.Bit)};
            parameters[0].Value = model.LeadSource;
            parameters[1].Value = model.DefaultUserId;
            parameters[2].Value = model.Default;

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
        public void Update(LPWeb.Model.Company_Lead_Sources model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Company_Lead_Sources set ");
            strSql.Append("LeadSource=@LeadSource,");
            strSql.Append("DefaultUserId=@DefaultUserId,");
            strSql.Append("[Default]=@Default");
            strSql.Append(" where LeadSourceID=@LeadSourceID ");
            SqlParameter[] parameters = {
					new SqlParameter("@LeadSourceID", SqlDbType.Int,4),
					new SqlParameter("@LeadSource", SqlDbType.NVarChar,150),
                    new SqlParameter("@DefaultUserId", SqlDbType.Int,4),
                    new SqlParameter("@Default", SqlDbType.Bit)};
            parameters[0].Value = model.LeadSourceID;
            parameters[1].Value = model.LeadSource;
            parameters[2].Value = model.DefaultUserId;
            parameters[3].Value = model.Default;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 把[Default]设置为False
        /// </summary>
        public void UpdateDefault()
        {
            string strSql = "update Company_Lead_Sources set [default] = 0 ";
             DbHelperSQL.ExecuteSql(strSql);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int LeadSourceID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Company_Lead_Sources ");
            strSql.Append(" where LeadSourceID=@LeadSourceID ");
            SqlParameter[] parameters = {
					new SqlParameter("@LeadSourceID", SqlDbType.Int,4)};
            parameters[0].Value = LeadSourceID;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Company_Lead_Sources GetModel(int LeadSourceID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 LeadSourceID,LeadSource from Company_Lead_Sources ");
            strSql.Append(" where LeadSourceID=@LeadSourceID ");
            SqlParameter[] parameters = {
					new SqlParameter("@LeadSourceID", SqlDbType.Int,4)};
            parameters[0].Value = LeadSourceID;

            LPWeb.Model.Company_Lead_Sources model = new LPWeb.Model.Company_Lead_Sources();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["LeadSourceID"].ToString() != "")
                {
                    model.LeadSourceID = int.Parse(ds.Tables[0].Rows[0]["LeadSourceID"].ToString());
                }
                model.LeadSource = ds.Tables[0].Rows[0]["LeadSource"].ToString();
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
            strSql.Append("select LeadSourceID,LeadSource,'DefaultUser'=dbo.lpfn_GetUserName(DefaultUserId),DefaultUserId,[Default], 'DefaultString' ="
                        + " case [Default] when 1 then 'Yes' when 0 then 'No' else '' end ");
            strSql.Append(" FROM Company_Lead_Sources ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);               
            }
            else
            {
                strSql.Append(" order by leadsource ");
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
            strSql.Append(" LeadSourceID,LeadSource ");
            strSql.Append(" FROM Company_Lead_Sources ");
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
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000)
					};
            parameters[0].Value = "Company_Lead_Sources";
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
