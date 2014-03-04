using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类ServiceTypes。
	/// </summary>
	public class ServiceTypes
	{
		public ServiceTypes()
		{}

        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.ServiceTypes model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ServiceTypes(");
            strSql.Append("Name,Enabled)");
            strSql.Append(" values (");
            strSql.Append("@Name,@Enabled)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.Enabled;

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
        public void Update(LPWeb.Model.ServiceTypes model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ServiceTypes set ");
            strSql.Append("ServiceTypeId=@ServiceTypeId,");
            strSql.Append("Name=@Name,");
            strSql.Append("Enabled=@Enabled");
            strSql.Append(" where ServiceTypeId=@ServiceTypeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ServiceTypeId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
            parameters[0].Value = model.ServiceTypeId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Enabled;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ServiceTypeId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ServiceTypes ");
            strSql.Append(" where ServiceTypeId=@ServiceTypeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ServiceTypeId", SqlDbType.Int,4)};
            parameters[0].Value = ServiceTypeId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.ServiceTypes GetModel(int ServiceTypeId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ServiceTypeId,Name,Enabled from ServiceTypes ");
            strSql.Append(" where ServiceTypeId=@ServiceTypeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ServiceTypeId", SqlDbType.Int,4)};
            parameters[0].Value = ServiceTypeId;

            LPWeb.Model.ServiceTypes model = new LPWeb.Model.ServiceTypes();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ServiceTypeId"].ToString() != "")
                {
                    model.ServiceTypeId = int.Parse(ds.Tables[0].Rows[0]["ServiceTypeId"].ToString());
                }
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                if (ds.Tables[0].Rows[0]["Enabled"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Enabled"].ToString() == "1") || (ds.Tables[0].Rows[0]["Enabled"].ToString().ToLower() == "true"))
                    {
                        model.Enabled = true;
                    }
                    else
                    {
                        model.Enabled = false;
                    }
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
            strSql.Append("select [Name] as ServiceType,Name, Enabled, ServiceTypeId");
            strSql.Append(" FROM ServiceTypes ");
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
            strSql.Append(" ServiceTypeId,Name,Enabled ");
            strSql.Append(" FROM ServiceTypes ");
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
            parameters[0].Value = "ServiceTypes";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage", parameters, "ds");
        }

        #endregion  成员方法
        #region neo

        /// <summary>
        /// get service type list
        /// neo 2011-04-06
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetServiceTypeListBase(string sWhere)
        {
            string sSql = "select * from ServiceTypes where 1=1 " + sWhere + " order by Name";
            return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        }

        #endregion
        #region changke
        public DataSet GetServiceTypes(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 1000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = "ServiceTypes";
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "1=1 " + strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        /// <summary>
        /// get contact role list
        /// neo 2010-12-09
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public DataTable GetContactRoleListBase(string sWhere)
        {
            string sSql = "select * from ContactRoles where 1=1 " + sWhere;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public bool IsSerivceTypeExsits(string strName)
        {
            string strSql = string.Format("SELECT COUNT(1) FROM ServiceTypes WHERE Name='{0}'", strName);
            object obj = DbHelperSQL.ExecuteScalar(strSql);
            int n = 0;
            if (!int.TryParse(obj.ToString(), out n))
                return false;
            if (n > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listIds"></param>
        public void DeleteServiceType(List<int> listIds)
        {
            string strSql = "";
            string strWhere = GetIDsWhereClauseFromList(listIds);
            if (strWhere.Length > 0)
            {
                strSql = string.Format(@"UPDATE dbo.ContactCompanies SET ServiceTypes=NULL,ServiceTypeId=NULL WHERE ServiceTypeId IN ({0});
                                         DELETE dbo.ServiceTypes WHERE ServiceTypeId IN ({0});",
                    strWhere);
            }

            if (strSql.Length > 0)
            {
                DbHelperSQL.ExecuteNonQuery(strSql);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listIds"></param>
        public void EnableServiceType(List<int> listIds)
        {
            string strSql = "";
            string strWhere = GetIDsWhereClauseFromList(listIds);
            if (strWhere.Length > 0)
            {
                strSql = string.Format("UPDATE ServiceTypes SET [Enabled]=1 WHERE ServiceTypeId IN ({0});",
                    strWhere);
            }

            if (strSql.Length > 0)
            {
                DbHelperSQL.ExecuteNonQuery(strSql);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listIds"></param>
        public void DisableServiceType(List<int> listIds)
        {
            string strSql = "";
            string strWhere = GetIDsWhereClauseFromList(listIds);
            if (strWhere.Length > 0)
            {
                strSql = string.Format("UPDATE ServiceTypes SET [Enabled]=0 WHERE ServiceTypeId IN ({0});",
                    strWhere);
            }

            if (strSql.Length > 0)
            {
                DbHelperSQL.ExecuteNonQuery(strSql);
            }
        }

        private string GetIDsWhereClauseFromList(List<int> list)
        {
            StringBuilder sbIds = new StringBuilder();
            foreach (int n in list)
            {
                if (sbIds.Length > 0)
                    sbIds.Append(",");
                sbIds.AppendFormat("'{0}'", n);
            }
            return sbIds.ToString();
        }
        #endregion
    }
}

