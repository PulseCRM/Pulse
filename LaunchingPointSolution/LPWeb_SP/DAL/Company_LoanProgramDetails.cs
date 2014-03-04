using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace LPWeb.DAL
{
    public class Company_LoanProgramDetails : Company_LoanProgramDetailsBase
    {
        public Company_LoanProgramDetails()
        { }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Company_LoanProgramDetails GetModel(int LoanProgramID, int InvestorID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from Company_LoanProgramDetails ");
            strSql.Append(" where LoanProgramID=@LoanProgramID and InvestorID=@InvestorID ");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanProgramID", SqlDbType.Int,4),
					new SqlParameter("@InvestorID", SqlDbType.Int,4)};

            parameters[0].Value = LoanProgramID;
            parameters[1].Value = InvestorID;

            LPWeb.Model.Company_LoanProgramDetails model = new LPWeb.Model.Company_LoanProgramDetails();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["LoanProgramID"] != DBNull.Value)
                {
                    model.LoanProgramID = int.Parse(ds.Tables[0].Rows[0]["LoanProgramID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IndexType"] != DBNull.Value)
                    model.IndexType = ds.Tables[0].Rows[0]["IndexType"].ToString();
                if (ds.Tables[0].Rows[0]["Margin"] != DBNull.Value)
                {
                    model.Margin = decimal.Parse(ds.Tables[0].Rows[0]["Margin"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FirstAdj"] != DBNull.Value)
                {
                    model.FirstAdj = decimal.Parse(ds.Tables[0].Rows[0]["FirstAdj"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SubAdj"] != DBNull.Value)
                {
                    model.SubAdj = decimal.Parse(ds.Tables[0].Rows[0]["SubAdj"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LifetimeCap"] != DBNull.Value)
                {
                    model.LifetimeCap = decimal.Parse(ds.Tables[0].Rows[0]["LifetimeCap"].ToString());
                }
                if (ds.Tables[0].Rows[0]["InvestorID"] != DBNull.Value)
                {
                    model.InvestorID = int.Parse(ds.Tables[0].Rows[0]["InvestorID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Enabled"] != DBNull.Value)
                {
                    model.Enabled = bool.Parse(ds.Tables[0].Rows[0]["Enabled"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Term"] != DBNull.Value)
                {
                    model.Term = int.Parse(ds.Tables[0].Rows[0]["Term"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Due"] != DBNull.Value)
                {
                    model.Due = int.Parse(ds.Tables[0].Rows[0]["Due"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 更新多条记录Enabled数据
        /// </summary>
        public bool UpdateEnabled(string IdList, bool enabled)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Company_LoanProgramDetails set Enabled = ");
            strSql.Append(enabled ? "1" : "0");
            strSql.Append(" where LoanProgramID in (  ");
            strSql.Append(IdList);
            strSql.Append(" ) ");

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
        /// 更新记录Enabled数据
        /// </summary>
        public bool UpdateEnabled(int LoanProgramID, int InvestorID, bool enabled)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Company_LoanProgramDetails set Enabled = ");
            strSql.Append(enabled ? "1" : "0");
            strSql.Append(" where LoanProgramID =").Append(LoanProgramID);
            strSql.Append(" and InvestorID =").Append(InvestorID);

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

        public bool Delete(int LoanProgramID, int InvestorID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" delete from Company_LoanProgramDetails ");
            strSql.Append(" where LoanProgramID =").Append(LoanProgramID);
            strSql.Append(" and InvestorID =").Append(InvestorID);

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
