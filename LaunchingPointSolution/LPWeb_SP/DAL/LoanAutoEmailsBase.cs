using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类LoanAutoEmails。
	/// </summary>
	public class LoanAutoEmailsBase
	{
        public LoanAutoEmailsBase()
		{}
		#region  成员方法

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LPWeb.Model.LoanAutoEmails model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into LoanAutoEmails(");
			strSql.Append("FileId,ToContactId,ToUserId,[Enabled],[External],TemplReportId,Applied,AppliedBy,LastRun,ScheduleType)");
			strSql.Append(" values (");
			strSql.Append("@FileId,@ToContactId,@ToUserId,@Enabled,@External,@TemplReportId,@Applied,@AppliedBy,@LastRun,@ScheduleType)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@ToContactId", SqlDbType.Int,4),
					new SqlParameter("@ToUserId", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@External", SqlDbType.Bit,1),
					new SqlParameter("@TemplReportId", SqlDbType.Int,4),
					new SqlParameter("@Applied", SqlDbType.DateTime),
					new SqlParameter("@AppliedBy", SqlDbType.Int,4),
					new SqlParameter("@LastRun", SqlDbType.DateTime),
					new SqlParameter("@ScheduleType", SqlDbType.SmallInt,2)};
			parameters[0].Value = model.FileId;
			parameters[1].Value = model.ToContactId;
			parameters[2].Value = model.ToUserId;
			parameters[3].Value = model.Enabled;
			parameters[4].Value = model.External;
			parameters[5].Value = model.TemplReportId;
			parameters[6].Value = model.Applied;
			parameters[7].Value = model.AppliedBy;
			parameters[8].Value = model.LastRun;
			parameters[9].Value = model.ScheduleType;

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
		public void Update(LPWeb.Model.LoanAutoEmails model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update LoanAutoEmails set ");
			strSql.Append("FileId=@FileId,");
			strSql.Append("ToContactId=@ToContactId,");
			strSql.Append("ToUserId=@ToUserId,");
			strSql.Append("[Enabled]=@Enabled,");
			strSql.Append("[External]=@External,");
			strSql.Append("TemplReportId=@TemplReportId,");
			strSql.Append("Applied=@Applied,");
			strSql.Append("AppliedBy=@AppliedBy,");
            strSql.Append("LastRun=@LastRun,");
			strSql.Append("ScheduleType=@ScheduleType");
			strSql.Append(" where LoanAutoEmailid=@LoanAutoEmailid ");
			SqlParameter[] parameters = {
					new SqlParameter("@LoanAutoEmailid", SqlDbType.Int,4),
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@ToContactId", SqlDbType.Int,4),
					new SqlParameter("@ToUserId", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@External", SqlDbType.Bit,1),
					new SqlParameter("@TemplReportId", SqlDbType.Int,4),
					new SqlParameter("@Applied", SqlDbType.DateTime),
					new SqlParameter("@AppliedBy", SqlDbType.Int,4),
                    new SqlParameter("@LastRun", SqlDbType.DateTime),
					new SqlParameter("@ScheduleType", SqlDbType.SmallInt,2)};
			parameters[0].Value = model.LoanAutoEmailid;
			parameters[1].Value = model.FileId;
			parameters[2].Value = model.ToContactId;
			parameters[3].Value = model.ToUserId;
			parameters[4].Value = model.Enabled;
			parameters[5].Value = model.External;
			parameters[6].Value = model.TemplReportId;
			parameters[7].Value = model.Applied;
			parameters[8].Value = model.AppliedBy;
            parameters[9].Value = model.LastRun;
			parameters[10].Value = model.ScheduleType;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int LoanAutoEmailid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from LoanAutoEmails ");
			strSql.Append(" where LoanAutoEmailid=@LoanAutoEmailid ");
			SqlParameter[] parameters = {
					new SqlParameter("@LoanAutoEmailid", SqlDbType.Int,4)};
			parameters[0].Value = LoanAutoEmailid;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.LoanAutoEmails GetModel(int LoanAutoEmailid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 LoanAutoEmailid,FileId,ToContactId,ToUserId,[Enabled],[External],TemplReportId,Applied,AppliedBy,LastRun,ScheduleType from LoanAutoEmails ");
			strSql.Append(" where LoanAutoEmailid=@LoanAutoEmailid ");
			SqlParameter[] parameters = {
					new SqlParameter("@LoanAutoEmailid", SqlDbType.Int,4)};
			parameters[0].Value = LoanAutoEmailid;

			LPWeb.Model.LoanAutoEmails model=new LPWeb.Model.LoanAutoEmails();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["LoanAutoEmailid"].ToString()!="")
				{
					model.LoanAutoEmailid=int.Parse(ds.Tables[0].Rows[0]["LoanAutoEmailid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["FileId"] != DBNull.Value && ds.Tables[0].Rows[0]["FileId"].ToString()!="")
				{
					model.FileId=int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ToContactId"] != DBNull.Value && ds.Tables[0].Rows[0]["ToContactId"].ToString()!="")
				{
					model.ToContactId=int.Parse(ds.Tables[0].Rows[0]["ToContactId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ToUserId"] != DBNull.Value && ds.Tables[0].Rows[0]["ToUserId"].ToString()!="")
				{
					model.ToUserId=int.Parse(ds.Tables[0].Rows[0]["ToUserId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Enabled"] != DBNull.Value && ds.Tables[0].Rows[0]["Enabled"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["Enabled"].ToString()=="1")||(ds.Tables[0].Rows[0]["Enabled"].ToString().ToLower()=="true"))
					{
						model.Enabled=true;
					}
					else
					{
						model.Enabled=false;
					}
				}
				if(ds.Tables[0].Rows[0]["External"] != DBNull.Value && ds.Tables[0].Rows[0]["External"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["External"].ToString()=="1")||(ds.Tables[0].Rows[0]["External"].ToString().ToLower()=="true"))
					{
						model.External=true;
					}
					else
					{
						model.External=false;
					}
				}
				if(ds.Tables[0].Rows[0]["TemplReportId"] != DBNull.Value && ds.Tables[0].Rows[0]["TemplReportId"].ToString()!="")
				{
					model.TemplReportId=int.Parse(ds.Tables[0].Rows[0]["TemplReportId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Applied"] != DBNull.Value && ds.Tables[0].Rows[0]["Applied"].ToString()!="")
				{
					model.Applied=DateTime.Parse(ds.Tables[0].Rows[0]["Applied"].ToString());
				}
				if(ds.Tables[0].Rows[0]["AppliedBy"] != DBNull.Value && ds.Tables[0].Rows[0]["AppliedBy"].ToString()!="")
				{
					model.AppliedBy=int.Parse(ds.Tables[0].Rows[0]["AppliedBy"].ToString());
				}
				if(ds.Tables[0].Rows[0]["LastRun"] != DBNull.Value && ds.Tables[0].Rows[0]["LastRun"].ToString()!="")
				{
					model.LastRun=DateTime.Parse(ds.Tables[0].Rows[0]["LastRun"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ScheduleType"] != DBNull.Value && ds.Tables[0].Rows[0]["ScheduleType"].ToString()!="")
				{
					model.ScheduleType=int.Parse(ds.Tables[0].Rows[0]["ScheduleType"].ToString());
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
			strSql.Append("select LoanAutoEmailid,FileId,ToContactId,ToUserId,Enabled,External,TemplReportId,Applied,AppliedBy,LastRun,ScheduleType ");
			strSql.Append(" FROM LoanAutoEmails ");
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
			strSql.Append(" LoanAutoEmailid,FileId,ToContactId,ToUserId,Enabled,External,TemplReportId,Applied,AppliedBy,LastRun,ScheduleType ");
			strSql.Append(" FROM LoanAutoEmails ");
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
			parameters[0].Value = "LoanAutoEmails";
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

