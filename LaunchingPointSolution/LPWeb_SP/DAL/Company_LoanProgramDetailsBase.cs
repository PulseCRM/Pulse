using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:Company_LoanProgramDetails
    /// </summary>
    public class Company_LoanProgramDetailsBase
    {
        public Company_LoanProgramDetailsBase()
        { }
        #region  Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("LoanProgramID", "Company_LoanProgramDetails");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int LoanProgramID, int InvestorID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Company_LoanProgramDetails");
            //strSql.Append(" where LoanProgramID=@LoanProgramID AND LenderCompanyId=@LenderCompanyId");
            strSql.Append(" where LoanProgramID=@LoanProgramID AND InvestorID=@InvestorID");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanProgramID", SqlDbType.Int,4),
                    new SqlParameter("@InvestorID",SqlDbType.Int,4)                   
                                        };
            parameters[0].Value = LoanProgramID;
            parameters[1].Value = InvestorID;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.Company_LoanProgramDetails model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Company_LoanProgramDetails(");
            strSql.Append("LoanProgramID,InvestorID,IndexType,Margin,FirstAdj,SubAdj,LifetimeCap,Enabled,Term,Due)");
            strSql.Append(" values (");
            strSql.Append("@LoanProgramID,@InvestorID,@IndexType,@Margin,@FirstAdj,@SubAdj,@LifetimeCap,@Enabled,@Term,@Due)");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanProgramID", SqlDbType.Int,4),
					new SqlParameter("@InvestorID", SqlDbType.Int,4),
					new SqlParameter("@IndexType", SqlDbType.NVarChar,255),
					new SqlParameter("@Margin", SqlDbType.Money,8),
					new SqlParameter("@FirstAdj", SqlDbType.Money,8),
					new SqlParameter("@SubAdj", SqlDbType.Money,8),
					new SqlParameter("@LifetimeCap", SqlDbType.Money,8),
					new SqlParameter("@Enabled", SqlDbType.Bit),
           			new SqlParameter("@Term", SqlDbType.Int),
                    new SqlParameter("@Due", SqlDbType.Int)        
             };
            parameters[0].Value = model.LoanProgramID;
            parameters[1].Value = model.InvestorID;
            parameters[2].Value = model.IndexType;
            parameters[3].Value = model.Margin;
            parameters[4].Value = model.FirstAdj;
            parameters[5].Value = model.SubAdj;
            parameters[6].Value = model.LifetimeCap;
            parameters[7].Value = model.Enabled;
            parameters[8].Value = model.Term;
            parameters[9].Value = model.Due;
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.Company_LoanProgramDetails model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Company_LoanProgramDetails set ");
            //strSql.Append("LenderCompanyId=@LenderCompanyId,");
            strSql.Append("IndexType=@IndexType,");
            strSql.Append("Margin=@Margin,");
            strSql.Append("FirstAdj=@FirstAdj,");
            strSql.Append("SubAdj=@SubAdj,");
            strSql.Append("LifetimeCap=@LifetimeCap,");
            strSql.Append("InvestorID =@InvestorID,");
            strSql.Append("Term =@Term,");
            strSql.Append("Due =@Due,");
            strSql.Append("Enabled=@Enabled");
            strSql.Append(" where LoanProgramID=@LoanProgramID AND InvestorID=@InvestorID");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanProgramID", SqlDbType.Int,4),
					new SqlParameter("@IndexType", SqlDbType.NVarChar,255),
					new SqlParameter("@Margin", SqlDbType.Money,8),
					new SqlParameter("@FirstAdj", SqlDbType.Money,8),
					new SqlParameter("@SubAdj", SqlDbType.Money,8),
					new SqlParameter("@LifetimeCap", SqlDbType.Money,8),
					new SqlParameter("@InvestorID", SqlDbType.Int,4),
                    new SqlParameter("@Term", SqlDbType.Int,4),
                    new SqlParameter("@Due", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit)};
            parameters[0].Value = model.LoanProgramID;
            parameters[1].Value = model.IndexType;
            parameters[2].Value = model.Margin;
            parameters[3].Value = model.FirstAdj;
            parameters[4].Value = model.SubAdj;
            parameters[5].Value = model.LifetimeCap;
            parameters[6].Value = model.InvestorID;
            parameters[7].Value = model.Term;
            parameters[8].Value = model.Due;
            parameters[9].Value = model.Enabled;

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
        public bool Delete(int LoanProgramID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Company_LoanProgramDetails ");
            strSql.Append(" where LoanProgramID=@LoanProgramID ");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanProgramID", SqlDbType.Int,4)};
            parameters[0].Value = LoanProgramID;

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
        public bool DeleteList(string LoanProgramIDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Company_LoanProgramDetails ");
            strSql.Append(" where LoanProgramID in (" + LoanProgramIDlist + ")  ");
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
        public LPWeb.Model.Company_LoanProgramDetails GetModel(int LoanProgramID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 LoanProgramID,IndexType,Margin,FirstAdj,SubAdj,LifetimeCap,InvestorID,Enabled from Company_LoanProgramDetails ");
            strSql.Append(" where LoanProgramID=@LoanProgramID ");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanProgramID", SqlDbType.Int,4)};
            parameters[0].Value = LoanProgramID;

            LPWeb.Model.Company_LoanProgramDetails model = new LPWeb.Model.Company_LoanProgramDetails();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["LoanProgramID"].ToString() != "")
                {
                    model.LoanProgramID = int.Parse(ds.Tables[0].Rows[0]["LoanProgramID"].ToString());
                }
                //if (ds.Tables[0].Rows[0]["LenderCompanyId"].ToString() != "")
                //{
                //    model.LenderCompanyId = int.Parse(ds.Tables[0].Rows[0]["LenderCompanyId"].ToString());
                //}
                model.IndexType = ds.Tables[0].Rows[0]["IndexType"].ToString();
                if (ds.Tables[0].Rows[0]["Margin"].ToString() != "")
                {
                    model.Margin = decimal.Parse(ds.Tables[0].Rows[0]["Margin"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FirstAdj"].ToString() != "")
                {
                    model.FirstAdj = decimal.Parse(ds.Tables[0].Rows[0]["FirstAdj"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SubAdj"].ToString() != "")
                {
                    model.SubAdj = decimal.Parse(ds.Tables[0].Rows[0]["SubAdj"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LifetimeCap"].ToString() != "")
                {
                    model.LifetimeCap = decimal.Parse(ds.Tables[0].Rows[0]["LifetimeCap"].ToString());
                }
                if (ds.Tables[0].Rows[0]["InvestorID"].ToString() != "")
                {
                    model.InvestorID = int.Parse(ds.Tables[0].Rows[0]["InvestorID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Enabled"].ToString() != "")
                {
                    model.Enabled = bool.Parse(ds.Tables[0].Rows[0]["Enabled"].ToString());
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
            strSql.Append("select LoanProgramID,IndexType,Margin,FirstAdj,SubAdj,LifetimeCap,InvestorID,Enabled ");
            strSql.Append(" FROM Company_LoanProgramDetails ");
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
            strSql.Append(" LoanProgramID,IndexType,Margin,FirstAdj,SubAdj,LifetimeCap,InvestorID,Enabled ");
            strSql.Append(" FROM Company_LoanProgramDetails ");
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
            parameters[0].Value = "Company_LoanProgramDetails";
            parameters[1].Value = "";
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

