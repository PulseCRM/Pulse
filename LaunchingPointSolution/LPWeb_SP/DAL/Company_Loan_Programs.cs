using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Company_Loan_Programs。
	/// </summary>
    public class Company_Loan_Programs : Company_Loan_ProgramsBase
	{
		public Company_Loan_Programs()
		{}
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Company_Loan_Programs model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Company_Loan_Programs(");
            strSql.Append("LoanProgram,");
            strSql.Append("IsARM)");
            strSql.Append(" values (");
            strSql.Append("@LoanProgram,");
            strSql.Append("@IsARM)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanProgram", SqlDbType.NVarChar,150),
                    new SqlParameter("@IsARM",SqlDbType.Bit)
                                        
                                        };
            parameters[0].Value = model.LoanProgram;
            parameters[1].Value = model.IsARM;

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
        public void Update(LPWeb.Model.Company_Loan_Programs model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Company_Loan_Programs set ");
            //strSql.Append("LoanProgramID=@LoanProgramID,");
            strSql.Append("LoanProgram=@LoanProgram,");
            strSql.Append("IsARM=@IsARM");
            strSql.Append(" where LoanProgramID=@LoanProgramID ");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanProgramID", SqlDbType.Int,4),
					new SqlParameter("@LoanProgram", SqlDbType.NVarChar,150),
                    new SqlParameter("@IsARM", SqlDbType.Bit)};
            parameters[0].Value = model.LoanProgramID;
            parameters[1].Value = model.LoanProgram;
            parameters[2].Value = model.IsARM;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int LoanProgramID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM [Company_LoanProgramDetails] WHERE LoanProgramID = @LoanProgramID;");
            strSql.Append("delete from Company_Loan_Programs ");
            strSql.Append(" where LoanProgramID=@LoanProgramID ");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanProgramID", SqlDbType.Int,4)};
            parameters[0].Value = LoanProgramID;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Company_Loan_Programs GetModel(int LoanProgramID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 LoanProgramID,LoanProgram from Company_Loan_Programs ");
            strSql.Append(" where LoanProgramID=@LoanProgramID ");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanProgramID", SqlDbType.Int,4)};
            parameters[0].Value = LoanProgramID;

            LPWeb.Model.Company_Loan_Programs model = new LPWeb.Model.Company_Loan_Programs();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["LoanProgramID"].ToString() != "")
                {
                    model.LoanProgramID = int.Parse(ds.Tables[0].Rows[0]["LoanProgramID"].ToString());
                }
                model.LoanProgram = ds.Tables[0].Rows[0]["LoanProgram"].ToString();

                model.IsARM = (ds.Tables[0].Rows[0]["IsARM"] != DBNull.Value && Convert.ToBoolean(ds.Tables[0].Rows[0]["IsARM"]) == true) ? true : false;
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
            strSql.Append("select LoanProgramID,LoanProgram,IsARM ");
            strSql.Append(" FROM Company_Loan_Programs ");
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
            strSql.Append(" LoanProgramID,LoanProgram,IsARM ");
            strSql.Append(" FROM Company_Loan_Programs ");
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
            parameters[0].Value = "Company_Loan_Programs";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage", parameters, "ds");
        }

        #endregion  成员方法


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetListInvestorARMprogram(int PageSize, int PageIndex, string strWhere, out int recordCount, string orderName, int orderType)
        {
            string tempTable = @"(select p.LoanProgramID,p.LoanProgram,p.IsARM,[IndexType],[Margin],[FirstAdj],[SubAdj],[LifetimeCap],[InvestorID],pd.[Enabled],cc.Name from Company_LoanProgramDetails pd 
inner join Company_Loan_Programs p on p.LoanProgramID = pd.LoanProgramID
inner join ContactCompanies cc on pd.InvestorID =cc.ContactCompanyID) t";
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 1000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,2000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "1=1 " + strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            recordCount = int.Parse(parameters[7].Value.ToString());
            return ds;
        }



        public DataSet GetInvestorsList()
        {

            string strSql = @"SELECT DISTINCT cc.Name,pd.InvestorID FROM [Company_LoanProgramDetails] pd
left join ContactCompanies cc on pd.InvestorID = cc.ContactCompanyID
where cc.Name is not null  
order by cc.Name asc";
            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetProgramsList(string whereStr)
        {
            StringBuilder sb = new StringBuilder();
            string strSql = @"SELECT DISTINCT lp.LoanProgram,lp.LoanProgramID FROM [Company_Loan_Programs] lp 
left join [Company_LoanProgramDetails] pd on pd.LoanProgramID =lp.LoanProgramID
where lp.LoanProgram is not null";
            sb.Append(strSql);
            if (!string.IsNullOrEmpty(whereStr))
            {
                sb.Append(whereStr);
            }
            sb.Append(" order by lp.LoanProgram asc ");
            return DbHelperSQL.Query(sb.ToString());
        }

        public DataSet GetIndexesList(string whereStr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT DISTINCT IndexType from [Company_LoanProgramDetails] WHERE 1=1 ");
            if (!string.IsNullOrEmpty(whereStr))
            {
                sb.Append(whereStr);// AND InvestorID = 1 AND LoanProgramID =1
            }
            sb.Append(" ORDER BY IndexType ASC ");
            return DbHelperSQL.Query(sb.ToString());
        }

	}
}

