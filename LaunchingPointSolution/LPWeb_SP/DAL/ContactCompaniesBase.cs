using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类ContactCompanies。
	/// </summary>
	public class ContactCompaniesBase
    {
        public ContactCompaniesBase()
		{}
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.ContactCompanies model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ContactCompanies(");
            strSql.Append("Name,Address,City,State,Zip,ServiceTypes,ServiceTypeId,Enabled)");
            strSql.Append(" values (");
            strSql.Append("@Name,@Address,@City,@State,@Zip,@ServiceTypes,@ServiceTypeId,@Enabled)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@Address", SqlDbType.NVarChar,255),
					new SqlParameter("@City", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.NVarChar,2),
					new SqlParameter("@Zip", SqlDbType.NVarChar,12),
					new SqlParameter("@ServiceTypes", SqlDbType.NVarChar,255),
                    new SqlParameter("@ServiceTypeId",SqlDbType.Int),
                    new SqlParameter("@Enabled",SqlDbType.Bit)
                                        };
            parameters[0].Value = model.Name;
            parameters[1].Value = model.Address;
            parameters[2].Value = model.City;
            parameters[3].Value = model.State;
            parameters[4].Value = model.Zip;
            parameters[5].Value = model.ServiceTypes;
            parameters[6].Value = model.ServiceTypeId;
            parameters[7].Value = model.Enabled;

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
        public void Update(LPWeb.Model.ContactCompanies model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ContactCompanies set ");
            strSql.Append("ContactCompanyId=@ContactCompanyId,");
            strSql.Append("Name=@Name,");
            strSql.Append("Address=@Address,");
            strSql.Append("City=@City,");
            strSql.Append("State=@State,");
            strSql.Append("Zip=@Zip,");
            strSql.Append("ServiceTypes=@ServiceTypes");
            strSql.Append(" where ContactCompanyId=@ContactCompanyId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactCompanyId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@Address", SqlDbType.NVarChar,255),
					new SqlParameter("@City", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.NVarChar,2),
					new SqlParameter("@Zip", SqlDbType.NVarChar,12),
					new SqlParameter("@ServiceTypes", SqlDbType.NVarChar,255)};
            parameters[0].Value = model.ContactCompanyId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Address;
            parameters[3].Value = model.City;
            parameters[4].Value = model.State;
            parameters[5].Value = model.Zip;
            parameters[6].Value = model.ServiceTypes;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ContactCompanyId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ContactCompanies ");
            strSql.Append(" where ContactCompanyId=@ContactCompanyId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactCompanyId", SqlDbType.Int,4)};
            parameters[0].Value = ContactCompanyId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.ContactCompanies GetModel(int ContactCompanyId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ContactCompanyId,Name,Address,City,State,Zip,ServiceTypeId from ContactCompanies ");
            strSql.Append(" where ContactCompanyId=@ContactCompanyId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactCompanyId", SqlDbType.Int,4)};
            parameters[0].Value = ContactCompanyId;

            LPWeb.Model.ContactCompanies model = new LPWeb.Model.ContactCompanies();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ContactCompanyId"].ToString() != "")
                {
                    model.ContactCompanyId = int.Parse(ds.Tables[0].Rows[0]["ContactCompanyId"].ToString());
                }
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                model.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                model.City = ds.Tables[0].Rows[0]["City"].ToString();
                model.State = ds.Tables[0].Rows[0]["State"].ToString();
                model.Zip = ds.Tables[0].Rows[0]["Zip"].ToString();
                model.ServiceTypeId = ds.Tables[0].Rows[0]["ServiceTypeId"] == DBNull.Value ? 0 : (int)ds.Tables[0].Rows[0]["ServiceTypeId"];
                if (model.ServiceTypeId > 0)
                {
                    string sqlCmd = "select [Name] as ServiceType from ServiceTypes where ServiceTypes.ServiceTypeId=" + model.ServiceTypeId.ToString();
                    object o = DbHelperSQL.GetSingle(sqlCmd);
                    model.ServiceTypes = o == null ? string.Empty : (string)o;
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        public LPWeb.Model.ContactCompanies GetModelbyName(string Name)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ContactCompanyId,Name,Address,City,State,Zip,ServiceTypeId from ContactCompanies ");
            strSql.Append(" where Name=@Name ");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar, 255)};
            parameters[0].Value = Name;

            LPWeb.Model.ContactCompanies model = new LPWeb.Model.ContactCompanies();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ContactCompanyId"].ToString() != "")
                {
                    model.ContactCompanyId = int.Parse(ds.Tables[0].Rows[0]["ContactCompanyId"].ToString());
                }
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                model.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                model.City = ds.Tables[0].Rows[0]["City"].ToString();
                model.State = ds.Tables[0].Rows[0]["State"].ToString();
                model.Zip = ds.Tables[0].Rows[0]["Zip"].ToString();
                model.ServiceTypeId = ds.Tables[0].Rows[0]["ServiceTypeId"] == DBNull.Value ? 0 : (int)ds.Tables[0].Rows[0]["ServiceTypeId"];
                if (model.ServiceTypeId > 0)
                {
                    string sqlCmd = "select [Name] as ServiceType from ServiceTypes where ServiceTypes.ServiceTypeId=" + model.ServiceTypeId.ToString();
                    object o = DbHelperSQL.GetSingle(sqlCmd);
                    model.ServiceTypes = o == null ? string.Empty : (string)o;
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
            strSql.Append("select ContactCompanyId,Name,Address,City,State,Zip,ServiceTypes,ServiceTypeId,Enabled ");
            strSql.Append(" FROM ContactCompanies ");
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
            strSql.Append(" ContactCompanyId,Name,Address,City,State,Zip,ServiceTypes ");
            strSql.Append(" FROM ContactCompanies ");
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
            parameters[0].Value = "ContactCompanies";
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

