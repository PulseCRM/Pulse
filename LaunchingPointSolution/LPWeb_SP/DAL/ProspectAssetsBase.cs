using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:ProspectAssets
    /// </summary>
    public partial class ProspectAssetsBase
    {
        public ProspectAssetsBase()
        { }
        #region  Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("ProspectAssetId", "ProspectAssets");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ProspectAssetId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from ProspectAssets");
            strSql.Append(" where ProspectAssetId=@ProspectAssetId");
            SqlParameter[] parameters = {
					new SqlParameter("@ProspectAssetId", SqlDbType.Int,4)
};
            parameters[0].Value = ProspectAssetId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.ProspectAssets model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ProspectAssets(");
            strSql.Append("ContactId,Name,Account,Amount,Type)");
            strSql.Append(" values (");
            strSql.Append("@ContactId,@Name,@Account,@Amount,@Type)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Account", SqlDbType.NVarChar,50),
					new SqlParameter("@Amount", SqlDbType.Decimal,9),
					new SqlParameter("@Type", SqlDbType.NVarChar,100)};
            parameters[0].Value = model.ContactId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Account;
            parameters[3].Value = model.Amount;
            parameters[4].Value = model.Type;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.ProspectAssets model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ProspectAssets set ");
            strSql.Append("ContactId=@ContactId,");
            strSql.Append("Name=@Name,");
            strSql.Append("Account=@Account,");
            strSql.Append("Amount=@Amount,");
            strSql.Append("Type=@Type");
            strSql.Append(" where ProspectAssetId=@ProspectAssetId");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Account", SqlDbType.NVarChar,50),
					new SqlParameter("@Amount", SqlDbType.Decimal,9),
					new SqlParameter("@Type", SqlDbType.NVarChar,100),
					new SqlParameter("@ProspectAssetId", SqlDbType.Int,4)};
            parameters[0].Value = model.ContactId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Account;
            parameters[3].Value = model.Amount;
            parameters[4].Value = model.Type;
            parameters[5].Value = model.ProspectAssetId;

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
        public bool Delete(int ProspectAssetId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProspectAssets ");
            strSql.Append(" where ProspectAssetId=@ProspectAssetId");
            SqlParameter[] parameters = {
					new SqlParameter("@ProspectAssetId", SqlDbType.Int,4)
};
            parameters[0].Value = ProspectAssetId;

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
        public bool DeleteList(string ProspectAssetIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProspectAssets ");
            strSql.Append(" where ProspectAssetId in (" + ProspectAssetIdlist + ")  ");
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
        public LPWeb.Model.ProspectAssets GetModel(int ProspectAssetId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ProspectAssetId,ContactId,Name,Account,Amount,Type from ProspectAssets ");
            strSql.Append(" where ProspectAssetId=@ProspectAssetId");
            SqlParameter[] parameters = {
					new SqlParameter("@ProspectAssetId", SqlDbType.Int,4)
};
            parameters[0].Value = ProspectAssetId;

            LPWeb.Model.ProspectAssets model = new LPWeb.Model.ProspectAssets();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ProspectAssetId"] != null && ds.Tables[0].Rows[0]["ProspectAssetId"].ToString() != "")
                {
                    model.ProspectAssetId = int.Parse(ds.Tables[0].Rows[0]["ProspectAssetId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ContactId"] != null && ds.Tables[0].Rows[0]["ContactId"].ToString() != "")
                {
                    model.ContactId = int.Parse(ds.Tables[0].Rows[0]["ContactId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Name"] != null && ds.Tables[0].Rows[0]["Name"].ToString() != "")
                {
                    model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Account"] != null && ds.Tables[0].Rows[0]["Account"].ToString() != "")
                {
                    model.Account = ds.Tables[0].Rows[0]["Account"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Amount"] != null && ds.Tables[0].Rows[0]["Amount"].ToString() != "")
                {
                    model.Amount = decimal.Parse(ds.Tables[0].Rows[0]["Amount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Type"] != null && ds.Tables[0].Rows[0]["Type"].ToString() != "")
                {
                    model.Type = ds.Tables[0].Rows[0]["Type"].ToString();
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
            strSql.Append("select ProspectAssetId,ContactId,Name,Account,Amount,Type ");
            strSql.Append(" FROM ProspectAssets ");
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
            strSql.Append(" ProspectAssetId,ContactId,Name,Account,Amount,Type ");
            strSql.Append(" FROM ProspectAssets ");
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
            parameters[0].Value = "ProspectAssets";
            parameters[1].Value = "ProspectAssetId";
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

