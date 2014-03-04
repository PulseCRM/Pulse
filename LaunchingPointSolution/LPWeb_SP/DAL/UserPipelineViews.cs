using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    public class UserPipelineViews
    {


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.UserPipelineViews model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UserPipelineViews (");
            strSql.Append("UserId,PipelineType,ViewName,Enabled,ViewFilter,OrgTypeFilter,OrgFilter,StageFilter,ContactTypeFilter,ContactFilter,DateTypeFilter,DateFilter,AdvancedLoanFilters)");
            strSql.Append(" values (");
            strSql.Append("@UserId,@PipelineType,@ViewName,@Enabled,@ViewFilter,@OrgTypeFilter,@OrgFilter,@StageFilter,@ContactTypeFilter,@ContactFilter,@DateTypeFilter,@DateFilter,@AdvancedLoanFilters)");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@PipelineType", SqlDbType.NVarChar,50),
					new SqlParameter("@ViewName", SqlDbType.NVarChar,200),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@ViewFilter", SqlDbType.NVarChar,50),
					new SqlParameter("@OrgTypeFilter", SqlDbType.NVarChar,50),
					new SqlParameter("@OrgFilter", SqlDbType.NVarChar,50),
					new SqlParameter("@StageFilter", SqlDbType.NVarChar,50),
					new SqlParameter("@ContactTypeFilter", SqlDbType.NVarChar,250),
					new SqlParameter("@ContactFilter", SqlDbType.NVarChar,50),
					new SqlParameter("@DateTypeFilter", SqlDbType.NVarChar,50),
					new SqlParameter("@DateFilter", SqlDbType.NVarChar,50),
                    new SqlParameter("@AdvancedLoanFilters", SqlDbType.NVarChar, -1)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.PipelineType;
            parameters[2].Value = model.ViewName;
            parameters[3].Value = model.Enabled;
            parameters[4].Value = model.ViewFilter;
            parameters[5].Value = model.OrgTypeFilter;
            parameters[6].Value = model.OrgFilter;
            parameters[7].Value = model.StageFilter;
            parameters[8].Value = model.ContactTypeFilter;
            parameters[9].Value = model.ContactFilter;
            parameters[10].Value = model.DateTypeFilter;
            parameters[11].Value = model.DateFilter;
            parameters[12].Value = model.AdvancedLoanFilters;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.UserPipelineViews model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UserPipelineViews set ");
            strSql.Append("UserId=@UserId,");
            strSql.Append("PipelineType=@PipelineType,");
            strSql.Append("ViewName=@ViewName,");
            strSql.Append("Enabled=@Enabled,");
            strSql.Append("ViewFilter=@ViewFilter,");
            strSql.Append("OrgTypeFilter=@OrgTypeFilter,");
            strSql.Append("OrgFilter=@OrgFilter,");
            strSql.Append("StageFilter=@StageFilter,");
            strSql.Append("ContactTypeFilter=@ContactTypeFilter,");
            strSql.Append("ContactFilter=@ContactFilter,");
            strSql.Append("DateTypeFilter=@DateTypeFilter,");
            strSql.Append("DateFilter=@DateFilter,");
            strSql.Append("AdvancedLoanFilters=@AdvancedLoanFilters");
            strSql.Append(" where UserPipelineViewID=@UserPipelineViewID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserPipelineViewID", SqlDbType.Int,4),
					new SqlParameter("@PipelineType", SqlDbType.NVarChar,50),
					new SqlParameter("@ViewName", SqlDbType.NVarChar,200),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@ViewFilter", SqlDbType.NVarChar,50),
					new SqlParameter("@OrgTypeFilter", SqlDbType.NVarChar,50),
					new SqlParameter("@OrgFilter", SqlDbType.NVarChar,50),
					new SqlParameter("@StageFilter", SqlDbType.NVarChar,50),
					new SqlParameter("@ContactTypeFilter", SqlDbType.NVarChar,250),
					new SqlParameter("@ContactFilter", SqlDbType.NVarChar,50),
					new SqlParameter("@DateTypeFilter", SqlDbType.NVarChar,50),
					new SqlParameter("@DateFilter", SqlDbType.NVarChar,50),
                    new SqlParameter("@AdvancedLoanFilters", SqlDbType.NVarChar, -1),
                    new SqlParameter("@UserID",SqlDbType.Int)    };
            parameters[0].Value = model.UserPipelineViewID;
            parameters[1].Value = model.PipelineType;
            parameters[2].Value = model.ViewName;
            parameters[3].Value = model.Enabled;
            parameters[4].Value = model.ViewFilter;
            parameters[5].Value = model.OrgTypeFilter;
            parameters[6].Value = model.OrgFilter;
            parameters[7].Value = model.StageFilter;
            parameters[8].Value = model.ContactTypeFilter;
            parameters[9].Value = model.ContactFilter;
            parameters[10].Value = model.DateTypeFilter;
            parameters[11].Value = model.DateFilter;
            parameters[12].Value = model.AdvancedLoanFilters;
            parameters[13].Value = model.UserId;

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
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [UserPipelineViewID],[UserId],[PipelineType],[ViewName],[Enabled],[ViewFilter],[OrgTypeFilter],[OrgFilter],[StageFilter],[ContactTypeFilter],[ContactFilter],[DateTypeFilter],[DateFilter],[AdvancedLoanFilters] ");
            strSql.Append(" FROM UserPipelineViews ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (filedOrder.Trim() != "")
            {
                strSql.Append(" order by " + filedOrder);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList_ViewName(string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT [UserPipelineViewID],[ViewName] ");
            strSql.Append(" FROM UserPipelineViews ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            if (filedOrder.Trim() != "")
            {
                strSql.Append(" order by " + filedOrder);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.UserPipelineViews GetModel(int UserPipelineViewID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  top 1 [UserPipelineViewID],[UserId],[PipelineType],[ViewName],[Enabled],[ViewFilter],[OrgTypeFilter],[OrgFilter],[StageFilter],[ContactTypeFilter],[ContactFilter],[DateTypeFilter],[DateFilter],[AdvancedLoanFilters]  FROM UserPipelineViews ");
            strSql.Append(" WHERE UserPipelineViewID=@UserPipelineViewID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserPipelineViewID", SqlDbType.Int,4)};
            parameters[0].Value = UserPipelineViewID;

            LPWeb.Model.UserPipelineViews model = new LPWeb.Model.UserPipelineViews();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                var dr = ds.Tables[0].Rows[0];
                model.UserPipelineViewID = 0;
                if (dr["UserPipelineViewID"].ToString() != "")
                {
                    model.UserPipelineViewID = int.Parse(dr["UserPipelineViewID"].ToString());
                }

                if (dr["UserId"] != DBNull.Value && dr["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(dr["UserId"].ToString());
                }

                if (dr["PipelineType"] != DBNull.Value && dr["PipelineType"] != "")
                {
                    model.PipelineType = dr["PipelineType"].ToString();
                }

                if (dr["ViewName"] != DBNull.Value && dr["ViewName"] != "")
                {
                    model.PipelineType = dr["ViewName"].ToString();
                }

                model.Enabled = false;
                if (dr["Enabled"] != DBNull.Value && dr["Enabled"] != "")
                {
                    model.Enabled = Convert.ToBoolean(dr["Enabled"]);
                }

                if (dr["OrgTypeFilter"] != DBNull.Value && dr["OrgTypeFilter"] != "")
                {
                    model.OrgTypeFilter = dr["OrgTypeFilter"].ToString();
                }

                if (dr["OrgFilter"] != DBNull.Value && dr["OrgFilter"] != "")
                {
                    model.OrgFilter = dr["OrgFilter"].ToString();
                }

                if (dr["ViewFilter"] != DBNull.Value && dr["ViewFilter"] != "")
                {
                    model.ViewFilter = dr["ViewFilter"].ToString();
                }

                if (dr["StageFilter"] != DBNull.Value && dr["StageFilter"] != "")
                {
                    model.StageFilter = dr["StageFilter"].ToString();
                }

                if (dr["ContactTypeFilter"] != DBNull.Value && dr["ContactTypeFilter"] != "")
                {
                    model.ContactTypeFilter = dr["ContactTypeFilter"].ToString();
                }

                if (dr["ContactFilter"] != DBNull.Value && dr["ContactFilter"] != "")
                {
                    model.ContactFilter = dr["ContactFilter"].ToString();
                }

                if (dr["DateTypeFilter"] != DBNull.Value && dr["DateTypeFilter"] != "")
                {
                    model.DateTypeFilter = dr["DateTypeFilter"].ToString();
                }

                if (dr["DateFilter"] != DBNull.Value && dr["DateFilter"] != "")
                {
                    model.DateFilter = dr["DateFilter"].ToString();
                }

                if (dr["AdvancedLoanFilters"] != DBNull.Value && dr["AdvancedLoanFilters"] != "")
                {
                    model.AdvancedLoanFilters = dr["AdvancedLoanFilters"].ToString();
                }


                return model;
            }
            else
            {
                return null;
            }
        }

    }
}
