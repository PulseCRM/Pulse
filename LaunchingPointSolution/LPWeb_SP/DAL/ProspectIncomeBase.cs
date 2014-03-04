using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:ProspectIncome
    /// </summary>
    public partial class ProspectIncomeBase
    {
        public ProspectIncomeBase()
        { }
        #region  Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("ProspectIncomeId", "ProspectIncome");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ProspectIncomeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from ProspectIncome");
            strSql.Append(" where ProspectIncomeId=@ProspectIncomeId");
            SqlParameter[] parameters = {
					new SqlParameter("@ProspectIncomeId", SqlDbType.Int,4)
};
            parameters[0].Value = ProspectIncomeId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.ProspectIncome model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ProspectIncome(");
            strSql.Append("ContactId,Salary,Overtime,Bonuses,Commission,Div_Int,NetRent,Other,EmplId)");
            strSql.Append(" values (");
            strSql.Append("@ContactId,@Salary,@Overtime,@Bonuses,@Commission,@Div_Int,@NetRent,@Other,@EmplId)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@Salary", SqlDbType.Decimal,9),
					new SqlParameter("@Overtime", SqlDbType.Decimal,5),
					new SqlParameter("@Bonuses", SqlDbType.Decimal,5),
					new SqlParameter("@Commission", SqlDbType.Decimal,5),
					new SqlParameter("@Div_Int", SqlDbType.Decimal,5),
					new SqlParameter("@NetRent", SqlDbType.Decimal,5),
					new SqlParameter("@Other", SqlDbType.Decimal,5),
					new SqlParameter("@EmplId", SqlDbType.Int,4)};
            parameters[0].Value = model.ContactId;
            parameters[1].Value = model.Salary;
            parameters[2].Value = model.Overtime;
            parameters[3].Value = model.Bonuses;
            parameters[4].Value = model.Commission;
            parameters[5].Value = model.Div_Int;
            parameters[6].Value = model.NetRent;
            parameters[7].Value = model.Other;
            parameters[8].Value = model.EmplId;

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
        public bool Update(LPWeb.Model.ProspectIncome model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ProspectIncome set ");
            strSql.Append("ContactId=@ContactId,");
            strSql.Append("Salary=@Salary,");
            strSql.Append("Overtime=@Overtime,");
            strSql.Append("Bonuses=@Bonuses,");
            strSql.Append("Commission=@Commission,");
            strSql.Append("Div_Int=@Div_Int,");
            strSql.Append("NetRent=@NetRent,");
            strSql.Append("Other=@Other,");
            strSql.Append("EmplId=@EmplId");
            strSql.Append(" where ProspectIncomeId=@ProspectIncomeId");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@Salary", SqlDbType.Decimal,9),
					new SqlParameter("@Overtime", SqlDbType.Decimal,5),
					new SqlParameter("@Bonuses", SqlDbType.Decimal,5),
					new SqlParameter("@Commission", SqlDbType.Decimal,5),
					new SqlParameter("@Div_Int", SqlDbType.Decimal,5),
					new SqlParameter("@NetRent", SqlDbType.Decimal,5),
					new SqlParameter("@Other", SqlDbType.Decimal,5),
					new SqlParameter("@EmplId", SqlDbType.Int,4),
					new SqlParameter("@ProspectIncomeId", SqlDbType.Int,4)};
            parameters[0].Value = model.ContactId;
            parameters[1].Value = model.Salary;
            parameters[2].Value = model.Overtime;
            parameters[3].Value = model.Bonuses;
            parameters[4].Value = model.Commission;
            parameters[5].Value = model.Div_Int;
            parameters[6].Value = model.NetRent;
            parameters[7].Value = model.Other;
            parameters[8].Value = model.EmplId;
            parameters[9].Value = model.ProspectIncomeId;

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
        public bool Delete(int ProspectIncomeId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProspectIncome ");
            strSql.Append(" where ProspectIncomeId=@ProspectIncomeId");
            SqlParameter[] parameters = {
					new SqlParameter("@ProspectIncomeId", SqlDbType.Int,4)
};
            parameters[0].Value = ProspectIncomeId;

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
        public bool DeleteList(string ProspectIncomeIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProspectIncome ");
            strSql.Append(" where ProspectIncomeId in (" + ProspectIncomeIdlist + ")  ");
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
        public LPWeb.Model.ProspectIncome GetModel(int ProspectIncomeId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ProspectIncomeId,ContactId,Salary,Overtime,Bonuses,Commission,Div_Int,NetRent,Other,EmplId from ProspectIncome ");
            strSql.Append(" where ProspectIncomeId=@ProspectIncomeId");
            SqlParameter[] parameters = {
					new SqlParameter("@ProspectIncomeId", SqlDbType.Int,4)
};
            parameters[0].Value = ProspectIncomeId;

            LPWeb.Model.ProspectIncome model = new LPWeb.Model.ProspectIncome();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ProspectIncomeId"] != null && ds.Tables[0].Rows[0]["ProspectIncomeId"].ToString() != "")
                {
                    model.ProspectIncomeId = int.Parse(ds.Tables[0].Rows[0]["ProspectIncomeId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ContactId"] != null && ds.Tables[0].Rows[0]["ContactId"].ToString() != "")
                {
                    model.ContactId = int.Parse(ds.Tables[0].Rows[0]["ContactId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Salary"] != null && ds.Tables[0].Rows[0]["Salary"].ToString() != "")
                {
                    model.Salary = decimal.Parse(ds.Tables[0].Rows[0]["Salary"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Overtime"] != null && ds.Tables[0].Rows[0]["Overtime"].ToString() != "")
                {
                    model.Overtime = decimal.Parse(ds.Tables[0].Rows[0]["Overtime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Bonuses"] != null && ds.Tables[0].Rows[0]["Bonuses"].ToString() != "")
                {
                    model.Bonuses = decimal.Parse(ds.Tables[0].Rows[0]["Bonuses"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Commission"] != null && ds.Tables[0].Rows[0]["Commission"].ToString() != "")
                {
                    model.Commission = decimal.Parse(ds.Tables[0].Rows[0]["Commission"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Div_Int"] != null && ds.Tables[0].Rows[0]["Div_Int"].ToString() != "")
                {
                    model.Div_Int = decimal.Parse(ds.Tables[0].Rows[0]["Div_Int"].ToString());
                }
                if (ds.Tables[0].Rows[0]["NetRent"] != null && ds.Tables[0].Rows[0]["NetRent"].ToString() != "")
                {
                    model.NetRent = decimal.Parse(ds.Tables[0].Rows[0]["NetRent"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Other"] != null && ds.Tables[0].Rows[0]["Other"].ToString() != "")
                {
                    model.Other = decimal.Parse(ds.Tables[0].Rows[0]["Other"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmplId"] != null && ds.Tables[0].Rows[0]["EmplId"].ToString() != "")
                {
                    model.EmplId = int.Parse(ds.Tables[0].Rows[0]["EmplId"].ToString());
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
            strSql.Append("select ProspectIncomeId,ContactId,Salary,Overtime,Bonuses,Commission,Div_Int,NetRent,Other,EmplId ");
            strSql.Append(" FROM ProspectIncome ");
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
            strSql.Append(" ProspectIncomeId,ContactId,Salary,Overtime,Bonuses,Commission,Div_Int,NetRent,Other,EmplId ");
            strSql.Append(" FROM ProspectIncome ");
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
            parameters[0].Value = "ProspectIncome";
            parameters[1].Value = "ProspectIncomeId";
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

