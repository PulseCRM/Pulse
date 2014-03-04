using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类:ProspectAlerts
	/// </summary>
	public class ProspectAlertsBase
	{
		public ProspectAlertsBase()
		{}
		#region  Method

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LPWeb.Model.ProspectAlerts model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ProspectAlerts(");
			strSql.Append("ContactId,Desc,DueDate,OwnerId,ProspectTaskId,AlertType,Created)");
			strSql.Append(" values (");
			strSql.Append("@ContactId,@Desc,@DueDate,@OwnerId,@ProspectTaskId,@AlertType,@Created)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@Desc", SqlDbType.NVarChar,255),
					new SqlParameter("@DueDate", SqlDbType.DateTime),
					new SqlParameter("@OwnerId", SqlDbType.Int,4),
					new SqlParameter("@ProspectTaskId", SqlDbType.Int,4),
					new SqlParameter("@AlertType", SqlDbType.NVarChar,50),
					new SqlParameter("@Created", SqlDbType.DateTime)};
			parameters[0].Value = model.ContactId;
			parameters[1].Value = model.Desc;
			parameters[2].Value = model.DueDate;
			parameters[3].Value = model.OwnerId;
			parameters[4].Value = model.ProspectTaskId;
			parameters[5].Value = model.AlertType;
			parameters[6].Value = model.Created;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
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
		public bool Update(LPWeb.Model.ProspectAlerts model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ProspectAlerts set ");
			strSql.Append("ContactId=@ContactId,");
			strSql.Append("Desc=@Desc,");
			strSql.Append("DueDate=@DueDate,");
			strSql.Append("OwnerId=@OwnerId,");
			strSql.Append("ProspectTaskId=@ProspectTaskId,");
			strSql.Append("AlertType=@AlertType,");
			strSql.Append("Created=@Created");
			strSql.Append(" where ProspectAlertId=@ProspectAlertId");
			SqlParameter[] parameters = {
					new SqlParameter("@ProspectAlertId", SqlDbType.Int,4),
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@Desc", SqlDbType.NVarChar,255),
					new SqlParameter("@DueDate", SqlDbType.DateTime),
					new SqlParameter("@OwnerId", SqlDbType.Int,4),
					new SqlParameter("@ProspectTaskId", SqlDbType.Int,4),
					new SqlParameter("@AlertType", SqlDbType.NVarChar,50),
					new SqlParameter("@Created", SqlDbType.DateTime)};
			parameters[0].Value = model.ProspectAlertId;
			parameters[1].Value = model.ContactId;
			parameters[2].Value = model.Desc;
			parameters[3].Value = model.DueDate;
			parameters[4].Value = model.OwnerId;
			parameters[5].Value = model.ProspectTaskId;
			parameters[6].Value = model.AlertType;
			parameters[7].Value = model.Created;

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
		public bool Delete(int ProspectAlertId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ProspectAlerts ");
			strSql.Append(" where ProspectAlertId=@ProspectAlertId");
			SqlParameter[] parameters = {
					new SqlParameter("@ProspectAlertId", SqlDbType.Int,4)
};
			parameters[0].Value = ProspectAlertId;

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
		public bool DeleteList(string ProspectAlertIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ProspectAlerts ");
			strSql.Append(" where ProspectAlertId in ("+ProspectAlertIdlist + ")  ");
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
		public LPWeb.Model.ProspectAlerts GetModel(int ProspectAlertId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ProspectAlertId,ContactId,Desc,DueDate,OwnerId,ProspectTaskId,AlertType,Created from ProspectAlerts ");
			strSql.Append(" where ProspectAlertId=@ProspectAlertId");
			SqlParameter[] parameters = {
					new SqlParameter("@ProspectAlertId", SqlDbType.Int,4)
};
			parameters[0].Value = ProspectAlertId;

			LPWeb.Model.ProspectAlerts model=new LPWeb.Model.ProspectAlerts();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["ProspectAlertId"].ToString()!="")
				{
					model.ProspectAlertId=int.Parse(ds.Tables[0].Rows[0]["ProspectAlertId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ContactId"].ToString()!="")
				{
					model.ContactId=int.Parse(ds.Tables[0].Rows[0]["ContactId"].ToString());
				}
				model.Desc=ds.Tables[0].Rows[0]["Desc"].ToString();
				if(ds.Tables[0].Rows[0]["DueDate"].ToString()!="")
				{
					model.DueDate=DateTime.Parse(ds.Tables[0].Rows[0]["DueDate"].ToString());
				}
				if(ds.Tables[0].Rows[0]["OwnerId"].ToString()!="")
				{
					model.OwnerId=int.Parse(ds.Tables[0].Rows[0]["OwnerId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ProspectTaskId"].ToString()!="")
				{
					model.ProspectTaskId=int.Parse(ds.Tables[0].Rows[0]["ProspectTaskId"].ToString());
				}
				model.AlertType=ds.Tables[0].Rows[0]["AlertType"].ToString();
				if(ds.Tables[0].Rows[0]["Created"].ToString()!="")
				{
					model.Created=DateTime.Parse(ds.Tables[0].Rows[0]["Created"].ToString());
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
			strSql.Append("select ProspectAlertId,ContactId,Desc,DueDate,OwnerId,ProspectTaskId,AlertType,Created ");
			strSql.Append(" FROM ProspectAlerts ");
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
			strSql.Append(" ProspectAlertId,ContactId,Desc,DueDate,OwnerId,ProspectTaskId,AlertType,Created ");
			strSql.Append(" FROM ProspectAlerts ");
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
			parameters[0].Value = "ProspectAlerts";
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

