using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Company_Loan_Programs。
	/// </summary>
	public class Company_Loan_ProgramsBase
    {
        public Company_Loan_ProgramsBase()
		{}
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Company_Loan_Programs model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Company_Loan_Programs(");
            strSql.Append("LoanProgram)");
            strSql.Append(" values (");
            strSql.Append("@LoanProgram)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanProgram", SqlDbType.NVarChar,150)};
            parameters[0].Value = model.LoanProgram;

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
            strSql.Append("LoanProgramID=@LoanProgramID,");
            strSql.Append("LoanProgram=@LoanProgram");
            strSql.Append(" where LoanProgramID=@LoanProgramID ");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanProgramID", SqlDbType.Int,4),
					new SqlParameter("@LoanProgram", SqlDbType.NVarChar,150)};
            parameters[0].Value = model.LoanProgramID;
            parameters[1].Value = model.LoanProgram;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int LoanProgramID)
        {

            StringBuilder strSql = new StringBuilder();
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
            strSql.Append("select LoanProgramID,LoanProgram ");
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
            strSql.Append(" LoanProgramID,LoanProgram ");
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
	}
}

