using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类EmailLogBase。
	/// </summary>
	public class EmailLogBase
	{
		public EmailLogBase()
		{}
		#region  成员方法

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LPWeb.Model.EmailLog model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into EmailLog(");
			strSql.Append("ToUser,ToContact,EmailTmplId,Success,Error,LastSent,LoanAlertId,Retries,FileId,FromEmail,FromUser,Created,AlertEmailType,EmailBody)");
			strSql.Append(" values (");
			strSql.Append("@ToUser,@ToContact,@EmailTmplId,@Success,@Error,@LastSent,@LoanAlertId,@Retries,@FileId,@FromEmail,@FromUser,@Created,@AlertEmailType,@EmailBody)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@ToUser", SqlDbType.NVarChar,255),
					new SqlParameter("@ToContact", SqlDbType.NVarChar,255),
					new SqlParameter("@EmailTmplId", SqlDbType.Int,4),
					new SqlParameter("@Success", SqlDbType.Bit,1),
					new SqlParameter("@Error", SqlDbType.NVarChar,500),
					new SqlParameter("@LastSent", SqlDbType.DateTime),
					new SqlParameter("@LoanAlertId", SqlDbType.Int,4),
					new SqlParameter("@Retries", SqlDbType.SmallInt,2),
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@FromEmail", SqlDbType.NVarChar,255),
					new SqlParameter("@FromUser", SqlDbType.Int,4),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@AlertEmailType", SqlDbType.SmallInt,2),
					new SqlParameter("@EmailBody", SqlDbType.VarBinary)};
			parameters[0].Value = model.ToUser;
			parameters[1].Value = model.ToContact;
			parameters[2].Value = model.EmailTmplId;
			parameters[3].Value = model.Success;
			parameters[4].Value = model.Error;
			parameters[5].Value = model.LastSent;
			parameters[6].Value = model.LoanAlertId;
			parameters[7].Value = model.Retries;
			parameters[8].Value = model.FileId;
			parameters[9].Value = model.FromEmail;
			parameters[10].Value = model.FromUser;
			parameters[11].Value = model.Created;
			parameters[12].Value = model.AlertEmailType;
			parameters[13].Value = model.EmailBody;

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
		public void Update(LPWeb.Model.EmailLog model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update EmailLog set ");
			strSql.Append("EmailLogId=@EmailLogId,");
			strSql.Append("ToUser=@ToUser,");
			strSql.Append("ToContact=@ToContact,");
			strSql.Append("EmailTmplId=@EmailTmplId,");
			strSql.Append("Success=@Success,");
			strSql.Append("Error=@Error,");
			strSql.Append("LastSent=@LastSent,");
			strSql.Append("LoanAlertId=@LoanAlertId,");
			strSql.Append("Retries=@Retries,");
			strSql.Append("FileId=@FileId,");
			strSql.Append("FromEmail=@FromEmail,");
			strSql.Append("FromUser=@FromUser,");
			strSql.Append("Created=@Created,");
			strSql.Append("AlertEmailType=@AlertEmailType,");
			strSql.Append("EmailBody=@EmailBody");
			strSql.Append(" where EmailLogId=@EmailLogId ");
			SqlParameter[] parameters = {
					new SqlParameter("@EmailLogId", SqlDbType.Int,4),
					new SqlParameter("@ToUser", SqlDbType.NVarChar,255),
					new SqlParameter("@ToContact", SqlDbType.NVarChar,255),
					new SqlParameter("@EmailTmplId", SqlDbType.Int,4),
					new SqlParameter("@Success", SqlDbType.Bit,1),
					new SqlParameter("@Error", SqlDbType.NVarChar,500),
					new SqlParameter("@LastSent", SqlDbType.DateTime),
					new SqlParameter("@LoanAlertId", SqlDbType.Int,4),
					new SqlParameter("@Retries", SqlDbType.SmallInt,2),
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@FromEmail", SqlDbType.NVarChar,255),
					new SqlParameter("@FromUser", SqlDbType.Int,4),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@AlertEmailType", SqlDbType.SmallInt,2),
					new SqlParameter("@EmailBody", SqlDbType.VarBinary)};
			parameters[0].Value = model.EmailLogId;
			parameters[1].Value = model.ToUser;
			parameters[2].Value = model.ToContact;
			parameters[3].Value = model.EmailTmplId;
			parameters[4].Value = model.Success;
			parameters[5].Value = model.Error;
			parameters[6].Value = model.LastSent;
			parameters[7].Value = model.LoanAlertId;
			parameters[8].Value = model.Retries;
			parameters[9].Value = model.FileId;
			parameters[10].Value = model.FromEmail;
			parameters[11].Value = model.FromUser;
			parameters[12].Value = model.Created;
			parameters[13].Value = model.AlertEmailType;
			parameters[14].Value = model.EmailBody;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int EmailLogId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from EmailLog ");
			strSql.Append(" where EmailLogId=@EmailLogId ");
			SqlParameter[] parameters = {
					new SqlParameter("@EmailLogId", SqlDbType.Int,4)};
			parameters[0].Value = EmailLogId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.EmailLog GetModel(int EmailLogId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 EmailLogId,ToUser,ToContact,EmailTmplId,Success,Error,LastSent,LoanAlertId,Retries,FileId,FromEmail,FromUser,Created,AlertEmailType,EmailBody from EmailLog ");
			strSql.Append(" where EmailLogId=@EmailLogId ");
			SqlParameter[] parameters = {
					new SqlParameter("@EmailLogId", SqlDbType.Int,4)};
			parameters[0].Value = EmailLogId;

			LPWeb.Model.EmailLog model=new LPWeb.Model.EmailLog();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["EmailLogId"].ToString()!="")
				{
					model.EmailLogId=int.Parse(ds.Tables[0].Rows[0]["EmailLogId"].ToString());
				}
				model.ToUser=ds.Tables[0].Rows[0]["ToUser"].ToString();
				model.ToContact=ds.Tables[0].Rows[0]["ToContact"].ToString();
				if(ds.Tables[0].Rows[0]["EmailTmplId"].ToString()!="")
				{
					model.EmailTmplId=int.Parse(ds.Tables[0].Rows[0]["EmailTmplId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Success"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["Success"].ToString()=="1")||(ds.Tables[0].Rows[0]["Success"].ToString().ToLower()=="true"))
					{
						model.Success=true;
					}
					else
					{
						model.Success=false;
					}
				}
				model.Error=ds.Tables[0].Rows[0]["Error"].ToString();
				if(ds.Tables[0].Rows[0]["LastSent"].ToString()!="")
				{
					model.LastSent=DateTime.Parse(ds.Tables[0].Rows[0]["LastSent"].ToString());
				}
				if(ds.Tables[0].Rows[0]["LoanAlertId"].ToString()!="")
				{
					model.LoanAlertId=int.Parse(ds.Tables[0].Rows[0]["LoanAlertId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Retries"].ToString()!="")
				{
					model.Retries=int.Parse(ds.Tables[0].Rows[0]["Retries"].ToString());
				}
				if(ds.Tables[0].Rows[0]["FileId"].ToString()!="")
				{
					model.FileId=int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
				}
				model.FromEmail=ds.Tables[0].Rows[0]["FromEmail"].ToString();
				if(ds.Tables[0].Rows[0]["FromUser"].ToString()!="")
				{
					model.FromUser=int.Parse(ds.Tables[0].Rows[0]["FromUser"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Created"].ToString()!="")
				{
					model.Created=DateTime.Parse(ds.Tables[0].Rows[0]["Created"].ToString());
				}
				if(ds.Tables[0].Rows[0]["AlertEmailType"].ToString()!="")
				{
					model.AlertEmailType=int.Parse(ds.Tables[0].Rows[0]["AlertEmailType"].ToString());
				}
				if(ds.Tables[0].Rows[0]["EmailBody"].ToString()!="")
				{
					model.EmailBody=(byte[])ds.Tables[0].Rows[0]["EmailBody"];
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
			strSql.Append("select EmailLogId,ToUser,ToContact,EmailTmplId,Success,Error,LastSent,LoanAlertId,Retries,FileId,FromEmail,FromUser,Created,AlertEmailType,EmailBody ");
			strSql.Append(" FROM EmailLog ");
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
			strSql.Append(" EmailLogId,ToUser,ToContact,EmailTmplId,Success,Error,LastSent,LoanAlertId,Retries,FileId,FromEmail,FromUser,Created,AlertEmailType,EmailBody ");
			strSql.Append(" FROM EmailLog ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		
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
			parameters[0].Value = "EmailLog";
			parameters[1].Value = "ID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}

		#endregion  成员方法
	}
}

