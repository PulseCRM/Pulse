using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    public class Company_MCT : Company_MCTBase
    {
        public Company_MCT()
        { }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Company_MCT GetModel()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ClientID,PostURL,PostDataEnabled,ActiveLoanInterval,ArchivedLoanInterval,ArchivedLoanDisposeMonth,ArchivedLoanStatuses from Company_MCT ");


            LPWeb.Model.Company_MCT model = new LPWeb.Model.Company_MCT();
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ClientID"] != null && ds.Tables[0].Rows[0]["ClientID"].ToString() != "")
                {
                    model.ClientID = ds.Tables[0].Rows[0]["ClientID"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PostURL"] != null && ds.Tables[0].Rows[0]["PostURL"].ToString() != "")
                {
                    model.PostURL = ds.Tables[0].Rows[0]["PostURL"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PostDataEnabled"] != null && ds.Tables[0].Rows[0]["PostDataEnabled"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["PostDataEnabled"].ToString() == "1") || (ds.Tables[0].Rows[0]["PostDataEnabled"].ToString().ToLower() == "true"))
                    {
                        model.PostDataEnabled = true;
                    }
                    else
                    {
                        model.PostDataEnabled = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ActiveLoanInterval"] != null && ds.Tables[0].Rows[0]["ActiveLoanInterval"].ToString() != "")
                {
                    model.ActiveLoanInterval = int.Parse(ds.Tables[0].Rows[0]["ActiveLoanInterval"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ArchivedLoanInterval"] != null && ds.Tables[0].Rows[0]["ArchivedLoanInterval"].ToString() != "")
                {
                    model.ArchivedLoanInterval = int.Parse(ds.Tables[0].Rows[0]["ArchivedLoanInterval"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ArchivedLoanDisposeMonth"] != null && ds.Tables[0].Rows[0]["ArchivedLoanDisposeMonth"].ToString() != "")
                {
                    model.ArchivedLoanDisposeMonth = int.Parse(ds.Tables[0].Rows[0]["ArchivedLoanDisposeMonth"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ArchivedLoanStatuses"] != null && ds.Tables[0].Rows[0]["ArchivedLoanStatuses"].ToString() != "")
                {
                    model.ArchivedLoanStatuses = ds.Tables[0].Rows[0]["ArchivedLoanStatuses"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Save MCT Data
        /// </summary>
        public bool Save(LPWeb.Model.Company_MCT model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Company_MCT(");
            strSql.Append("ClientID,PostURL,PostDataEnabled,ActiveLoanInterval,ArchivedLoanInterval,ArchivedLoanDisposeMonth,ArchivedLoanStatuses)");
            strSql.Append(" values (");
            strSql.Append("@ClientID,@PostURL,@PostDataEnabled,@ActiveLoanInterval,@ArchivedLoanInterval,@ArchivedLoanDisposeMonth,@ArchivedLoanStatuses)");

            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("update Company_MCT set ");
            strSql1.Append("ClientID=@ClientID,");
            strSql1.Append("PostURL=@PostURL,");
            strSql1.Append("PostDataEnabled=@PostDataEnabled,");
            strSql1.Append("ActiveLoanInterval=@ActiveLoanInterval,");
            strSql1.Append("ArchivedLoanInterval=@ArchivedLoanInterval,");
            strSql1.Append("ArchivedLoanDisposeMonth=@ArchivedLoanDisposeMonth,");
            strSql1.Append("ArchivedLoanStatuses=@ArchivedLoanStatuses");

            SqlParameter[] parameters = {
					new SqlParameter("@ClientID", SqlDbType.NVarChar,255),
					new SqlParameter("@PostURL", SqlDbType.NVarChar,255),
					new SqlParameter("@PostDataEnabled", SqlDbType.Bit,1),
					new SqlParameter("@ActiveLoanInterval", SqlDbType.Int,4),
					new SqlParameter("@ArchivedLoanInterval", SqlDbType.Int,4),
					new SqlParameter("@ArchivedLoanDisposeMonth", SqlDbType.Int,4),
					new SqlParameter("@ArchivedLoanStatuses", SqlDbType.NVarChar,255)};
            parameters[0].Value = model.ClientID;
            parameters[1].Value = model.PostURL;
            parameters[2].Value = model.PostDataEnabled;
            parameters[3].Value = model.ActiveLoanInterval;
            parameters[4].Value = model.ArchivedLoanInterval;
            parameters[5].Value = model.ArchivedLoanDisposeMonth;
            parameters[6].Value = model.ArchivedLoanStatuses;

            int rows = 0;
            if (this.GetRecordCount("") < 1)
            {
                rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            }
            else
            {
                rows = DbHelperSQL.ExecuteSql(strSql1.ToString(), parameters);
            }
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

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Company_MCT ");


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
    }
}
