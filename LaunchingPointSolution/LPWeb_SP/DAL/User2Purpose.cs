using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类:User2Purpose
	/// </summary>
	public class User2Purpose
	{
		public User2Purpose()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("UserID", "User2Purpose"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int UserID,string Purpose)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from User2Purpose");
			strSql.Append(" where UserID=@UserID and Purpose=@Purpose ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Purpose", SqlDbType.NVarChar,50)};
			parameters[0].Value = UserID;
			parameters[1].Value = Purpose;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.User2Purpose model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into User2Purpose(");
			strSql.Append("UserID,Purpose)");
			strSql.Append(" values (");
			strSql.Append("@UserID,@Purpose)");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Purpose", SqlDbType.NVarChar,255)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.Purpose;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.User2Purpose model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update User2Purpose set ");
#warning 系统发现缺少更新的字段，请手工确认如此更新是否正确！ 
			strSql.Append("UserID=@UserID,");
			strSql.Append("Purpose=@Purpose");
			strSql.Append(" where UserID=@UserID and Purpose=@Purpose ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Purpose", SqlDbType.NVarChar,255)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.Purpose;

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
		public bool Delete(int UserID,string Purpose)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from User2Purpose ");
			strSql.Append(" where UserID=@UserID and Purpose=@Purpose ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Purpose", SqlDbType.NVarChar,50)};
			parameters[0].Value = UserID;
			parameters[1].Value = Purpose;

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
        /// 根据UserID删除一条数据
        /// </summary>
        public bool DeleteByUserID(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from User2Purpose ");
            strSql.Append(" where UserID=@UserID");
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
		public LPWeb.Model.User2Purpose GetModel(int UserID,string Purpose)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 UserID,Purpose from User2Purpose ");
			strSql.Append(" where UserID=@UserID and Purpose=@Purpose ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Purpose", SqlDbType.NVarChar,50)};
			parameters[0].Value = UserID;
			parameters[1].Value = Purpose;

			LPWeb.Model.User2Purpose model=new LPWeb.Model.User2Purpose();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["UserID"].ToString()!="")
				{
					model.UserID=int.Parse(ds.Tables[0].Rows[0]["UserID"].ToString());
				}
				model.Purpose=ds.Tables[0].Rows[0]["Purpose"].ToString();
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
			strSql.Append("select UserID,Purpose ");
			strSql.Append(" FROM User2Purpose ");
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
			strSql.Append(" UserID,Purpose ");
			strSql.Append(" FROM User2Purpose ");
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
			parameters[0].Value = "User2Purpose";
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

