using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类LoanProfit。
    /// </summary>
    public class LoanProfitBase
    {
        public LoanProfitBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.LoanProfit model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LoanProfit(");
            strSql.Append("FileId,CompensationPlan,NetSell,SRP,LLPA,LenderCredit,Price)");
            strSql.Append(" values (");
            strSql.Append("@FileId,@CompensationPlan,@NetSell,@SRP,@LLPA,@LenderCredit,@Price)");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@CompensationPlan", SqlDbType.NVarChar,50),
					new SqlParameter("@NetSell", SqlDbType.Decimal,9),
					new SqlParameter("@SRP", SqlDbType.Decimal,9),
					new SqlParameter("@LLPA", SqlDbType.Decimal,9),
					new SqlParameter("@LenderCredit", SqlDbType.Decimal,9),
					new SqlParameter("@Price", SqlDbType.Money,8)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.CompensationPlan;
            parameters[2].Value = model.NetSell;
            parameters[3].Value = model.SRP;
            parameters[4].Value = model.LLPA;
            parameters[5].Value = model.LenderCredit;
            parameters[6].Value = model.Price;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.LoanProfit model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update LoanProfit set ");
                strSql.Append("CompensationPlan=@CompensationPlan,");
                strSql.Append("NetSell=@NetSell,");
                strSql.Append("SRP=@SRP,");
                strSql.Append("LLPA=@LLPA,");
                strSql.Append("LenderCredit=@LenderCredit,");
                strSql.Append("Price=@Price");
                strSql.Append(" where FileId=@FileId ");
                SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int),
					new SqlParameter("@CompensationPlan", SqlDbType.NVarChar,50),
					new SqlParameter("@NetSell", SqlDbType.Decimal,9),
					new SqlParameter("@SRP", SqlDbType.Decimal,9),
					new SqlParameter("@LLPA", SqlDbType.Decimal,9),
					new SqlParameter("@LenderCredit", SqlDbType.Decimal,9),
					new SqlParameter("@Price", SqlDbType.Money,8)};
                parameters[0].Value = model.FileId;
                parameters[1].Value = model.CompensationPlan;
                parameters[2].Value = model.NetSell;
                parameters[3].Value = model.SRP;
                parameters[4].Value = model.LLPA;
                parameters[5].Value = model.LenderCredit;
                parameters[6].Value = model.Price;

                DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            }
            catch (Exception Exc)
            {
                string msg = Exc.Message;
                //Return any Exceptions when handling the worksheet.
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int FileId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LoanProfit ");
            strSql.Append(" where FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4)};
            parameters[0].Value = FileId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.LoanProfit GetModel(int FileId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 FileId,CompensationPlan,NetSell,SRP,LLPA,LenderCredit,Price from LoanProfit ");
            strSql.Append(" where FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4)};
            parameters[0].Value = FileId;

            LPWeb.Model.LoanProfit model = new LPWeb.Model.LoanProfit();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["FileId"].ToString() != "")
                {
                    model.FileId = int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
                }
                model.CompensationPlan = ds.Tables[0].Rows[0]["CompensationPlan"].ToString();
                if (ds.Tables[0].Rows[0]["NetSell"].ToString() != "")
                {
                    model.NetSell = decimal.Parse(ds.Tables[0].Rows[0]["NetSell"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SRP"].ToString() != "")
                {
                    model.SRP = decimal.Parse(ds.Tables[0].Rows[0]["SRP"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LLPA"].ToString() != "")
                {
                    model.LLPA = decimal.Parse(ds.Tables[0].Rows[0]["LLPA"].ToString());
                }
                //model.Investor = ds.Tables[0].Rows[0]["Investor"].ToString();
                if (ds.Tables[0].Rows[0]["LenderCredit"].ToString() != "")
                {
                    model.LenderCredit = decimal.Parse(ds.Tables[0].Rows[0]["LenderCredit"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Price"].ToString() != "")
                {
                    model.Price = decimal.Parse(ds.Tables[0].Rows[0]["Price"].ToString());
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
            strSql.Append("select FileId,CompensationPlan,LenderCredit,Price ");
            strSql.Append(" FROM LoanProfit ");
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
            strSql.Append(" FileId,CompensationPlan,LenderCredit,NetSell,SRP,LLPA,Price ");
            strSql.Append(" FROM LoanProfit ");
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
            parameters[0].Value = "LoanProfit";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  成员方法
    }
}

