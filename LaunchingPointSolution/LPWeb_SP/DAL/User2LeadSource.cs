using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类:User2LeadSource
	/// </summary>
	public class User2LeadSource
	{
		public User2LeadSource()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("UserID", "User2LeadSource"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int UserID,int LeadSourceID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from User2LeadSource");
			strSql.Append(" where UserID=@UserID and LeadSourceID=@LeadSourceID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@LeadSourceID", SqlDbType.Int,4)};
			parameters[0].Value = UserID;
			parameters[1].Value = LeadSourceID;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.User2LeadSource model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into User2LeadSource(");
			strSql.Append("UserID,LeadSourceID)");
			strSql.Append(" values (");
			strSql.Append("@UserID,@LeadSourceID)");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@LeadSourceID", SqlDbType.Int,4)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.LeadSourceID;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.User2LeadSource model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update User2LeadSource set ");
#warning 系统发现缺少更新的字段，请手工确认如此更新是否正确！ 
			strSql.Append("UserID=@UserID,");
			strSql.Append("LeadSourceID=@LeadSourceID");
			strSql.Append(" where UserID=@UserID and LeadSourceID=@LeadSourceID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@LeadSourceID", SqlDbType.Int,4)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.LeadSourceID;

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
		public bool Delete(int UserID,int LeadSourceID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from User2LeadSource ");
			strSql.Append(" where UserID=@UserID and LeadSourceID=@LeadSourceID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@LeadSourceID", SqlDbType.Int,4)};
			parameters[0].Value = UserID;
			parameters[1].Value = LeadSourceID;

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
        /// 根据LeadSourceID删除数据
        /// </summary>
        /// <param name="leadSourceID"></param>
        /// <returns></returns>
	    public bool Delete(int leadSourceID)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from User2LeadSource ");
            strSql.Append(" where LeadSourceID=@LeadSourceID ");
            SqlParameter[] parameters = {
					new SqlParameter("@LeadSourceID", SqlDbType.Int,4)};
            parameters[0].Value = leadSourceID;

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
        /// 根据LeadSourceID删除数据
        /// </summary>
        /// <param name="leadSourceID"></param>
        /// <returns></returns>
        public bool DeleteByUserID(int userID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from User2LeadSource ");
            strSql.Append(" where userid=@userID ");
            SqlParameter[] parameters = {
					new SqlParameter("@userID", SqlDbType.Int,4)};
            parameters[0].Value = userID;

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
		public LPWeb.Model.User2LeadSource GetModel(int UserID,int LeadSourceID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 UserID,LeadSourceID from User2LeadSource ");
			strSql.Append(" where UserID=@UserID and LeadSourceID=@LeadSourceID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@LeadSourceID", SqlDbType.Int,4)};
			parameters[0].Value = UserID;
			parameters[1].Value = LeadSourceID;

			LPWeb.Model.User2LeadSource model=new LPWeb.Model.User2LeadSource();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["UserID"].ToString()!="")
				{
					model.UserID=int.Parse(ds.Tables[0].Rows[0]["UserID"].ToString());
				}
				if(ds.Tables[0].Rows[0]["LeadSourceID"].ToString()!="")
				{
					model.LeadSourceID=int.Parse(ds.Tables[0].Rows[0]["LeadSourceID"].ToString());
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select UserID,LeadSourceID ");
			strSql.Append(" FROM User2LeadSource ");
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
			strSql.Append(" UserID,LeadSourceID ");
			strSql.Append(" FROM User2LeadSource ");
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
			parameters[0].Value = "User2LeadSource";
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

