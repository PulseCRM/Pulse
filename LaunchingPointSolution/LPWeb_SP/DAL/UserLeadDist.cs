using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类:UserLeadDist
	/// </summary>
	public class UserLeadDist
	{
		public UserLeadDist()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("UserID", "UserLeadDist"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int UserID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from UserLeadDist");
			strSql.Append(" where UserID=@UserID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
			parameters[0].Value = UserID;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.UserLeadDist model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into UserLeadDist(");
			strSql.Append("UserID,EnableLeadRouting,MaxDailyLeads,LeadsAssignedToday,LastLeadAssigned,LastAssigned)");
			strSql.Append(" values (");
			strSql.Append("@UserID,@EnableLeadRouting,@MaxDailyLeads,@LeadsAssignedToday,@LastLeadAssigned,@LastAssigned)");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@EnableLeadRouting", SqlDbType.Bit,1),
					new SqlParameter("@MaxDailyLeads", SqlDbType.Int,4),
					new SqlParameter("@LeadsAssignedToday", SqlDbType.Int,4),
					new SqlParameter("@LastLeadAssigned", SqlDbType.Int,4),
					new SqlParameter("@LastAssigned", SqlDbType.DateTime)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.EnableLeadRouting;
			parameters[2].Value = model.MaxDailyLeads;
			parameters[3].Value = model.LeadsAssignedToday;
			parameters[4].Value = model.LastLeadAssigned;
			parameters[5].Value = model.LastAssigned;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.UserLeadDist model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update UserLeadDist set ");
			strSql.Append("EnableLeadRouting=@EnableLeadRouting,");
			strSql.Append("MaxDailyLeads=@MaxDailyLeads,");
			strSql.Append("LeadsAssignedToday=@LeadsAssignedToday,");
			strSql.Append("LastLeadAssigned=@LastLeadAssigned,");
			strSql.Append("LastAssigned=@LastAssigned");
			strSql.Append(" where UserID=@UserID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@EnableLeadRouting", SqlDbType.Bit,1),
					new SqlParameter("@MaxDailyLeads", SqlDbType.Int,4),
					new SqlParameter("@LeadsAssignedToday", SqlDbType.Int,4),
					new SqlParameter("@LastLeadAssigned", SqlDbType.Int,4),
					new SqlParameter("@LastAssigned", SqlDbType.DateTime)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.EnableLeadRouting;
			parameters[2].Value = model.MaxDailyLeads;
			parameters[3].Value = model.LeadsAssignedToday;
			parameters[4].Value = model.LastLeadAssigned;
			parameters[5].Value = model.LastAssigned;

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
		public bool Delete(int UserID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from UserLeadDist ");
			strSql.Append(" where UserID=@UserID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
			parameters[0].Value = UserID;

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
		public bool DeleteList(string UserIDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from UserLeadDist ");
			strSql.Append(" where UserID in ("+UserIDlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
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
		public LPWeb.Model.UserLeadDist GetModel(int UserID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 UserID,EnableLeadRouting,MaxDailyLeads,LeadsAssignedToday,LastLeadAssigned,LastAssigned from UserLeadDist ");
			strSql.Append(" where UserID=@UserID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
			parameters[0].Value = UserID;

			LPWeb.Model.UserLeadDist model=new LPWeb.Model.UserLeadDist();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["UserID"].ToString()!="")
				{
					model.UserID=int.Parse(ds.Tables[0].Rows[0]["UserID"].ToString());
				}
				if(ds.Tables[0].Rows[0]["EnableLeadRouting"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["EnableLeadRouting"].ToString()=="1")||(ds.Tables[0].Rows[0]["EnableLeadRouting"].ToString().ToLower()=="true"))
					{
						model.EnableLeadRouting=true;
					}
					else
					{
						model.EnableLeadRouting=false;
					}
				}
				if(ds.Tables[0].Rows[0]["MaxDailyLeads"].ToString()!="")
				{
					model.MaxDailyLeads=int.Parse(ds.Tables[0].Rows[0]["MaxDailyLeads"].ToString());
				}
				if(ds.Tables[0].Rows[0]["LeadsAssignedToday"].ToString()!="")
				{
					model.LeadsAssignedToday=int.Parse(ds.Tables[0].Rows[0]["LeadsAssignedToday"].ToString());
				}
				if(ds.Tables[0].Rows[0]["LastLeadAssigned"].ToString()!="")
				{
					model.LastLeadAssigned=int.Parse(ds.Tables[0].Rows[0]["LastLeadAssigned"].ToString());
				}
				if(ds.Tables[0].Rows[0]["LastAssigned"].ToString()!="")
				{
					model.LastAssigned=DateTime.Parse(ds.Tables[0].Rows[0]["LastAssigned"].ToString());
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
			strSql.Append("select UserID,EnableLeadRouting,MaxDailyLeads,LeadsAssignedToday,LastLeadAssigned,LastAssigned ");
			strSql.Append(" FROM UserLeadDist ");
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
			strSql.Append(" UserID,EnableLeadRouting,MaxDailyLeads,LeadsAssignedToday,LastLeadAssigned,LastAssigned ");
			strSql.Append(" FROM UserLeadDist ");
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
			parameters[0].Value = "UserLeadDist";
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

