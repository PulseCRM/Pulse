using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类:User2LoanType
	/// </summary>
	public class User2LoanType
	{
		public User2LoanType()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("UserID", "User2LoanType"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int UserID,string LoanType)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from User2LoanType");
			strSql.Append(" where UserID=@UserID and LoanType=@LoanType ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@LoanType", SqlDbType.NVarChar,50)};
			parameters[0].Value = UserID;
			parameters[1].Value = LoanType;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.User2LoanType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into User2LoanType(");
			strSql.Append("UserID,LoanType)");
			strSql.Append(" values (");
			strSql.Append("@UserID,@LoanType)");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@LoanType", SqlDbType.NVarChar,255)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.LoanType;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.User2LoanType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update User2LoanType set ");
#warning 系统发现缺少更新的字段，请手工确认如此更新是否正确！ 
			strSql.Append("UserID=@UserID,");
			strSql.Append("LoanType=@LoanType");
			strSql.Append(" where UserID=@UserID and LoanType=@LoanType ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@LoanType", SqlDbType.NVarChar,255)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.LoanType;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
		public bool Delete(int UserID,string LoanType)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from User2LoanType ");
			strSql.Append(" where UserID=@UserID and LoanType=@LoanType ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@LoanType", SqlDbType.NVarChar,50)};
			parameters[0].Value = UserID;
			parameters[1].Value = LoanType;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
        public bool DeleteByUser(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from User2LoanType ");
            strSql.Append(" where UserID=@UserID  ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = UserID;

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
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.User2LoanType GetModel(int UserID,string LoanType)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 UserID,LoanType from User2LoanType ");
			strSql.Append(" where UserID=@UserID and LoanType=@LoanType ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@LoanType", SqlDbType.NVarChar,50)};
			parameters[0].Value = UserID;
			parameters[1].Value = LoanType;

			LPWeb.Model.User2LoanType model=new LPWeb.Model.User2LoanType();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["UserID"].ToString()!="")
				{
					model.UserID=int.Parse(ds.Tables[0].Rows[0]["UserID"].ToString());
				}
				model.LoanType=ds.Tables[0].Rows[0]["LoanType"].ToString();
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select UserID,LoanType ");
			strSql.Append(" FROM User2LoanType ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" UserID,LoanType ");
			strSql.Append(" FROM User2LoanType ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
			parameters[0].Value = "User2LoanType";
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

