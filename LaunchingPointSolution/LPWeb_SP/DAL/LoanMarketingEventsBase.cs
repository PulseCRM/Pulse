using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类LoanMarketingEvents。
    /// </summary>
    public class LoanMarketingEventsBase
    {
        public LoanMarketingEventsBase()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("LoanMarketingEventId", "LoanMarketingEvents"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int LoanMarketingEventId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from LoanMarketingEvents");
			strSql.Append(" where LoanMarketingEventId=@LoanMarketingEventId ");
			SqlParameter[] parameters = {
					new SqlParameter("@LoanMarketingEventId", SqlDbType.Int,4)};
			parameters[0].Value = LoanMarketingEventId;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LPWeb.Model.LoanMarketingEvents model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into LoanMarketingEvents(");
			strSql.Append("Action,ExecutionDate,LoanMarketingId,LeadStarEventId,Completed,WeekNo,EventContent,EventURL,FileId)");
			strSql.Append(" values (");
			strSql.Append("@Action,@ExecutionDate,@LoanMarketingId,@LeadStarEventId,@Completed,@WeekNo,@EventContent,@EventURL,@FileId)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Action", SqlDbType.NVarChar,50),
					new SqlParameter("@ExecutionDate", SqlDbType.DateTime),
					new SqlParameter("@LoanMarketingId", SqlDbType.Int,4),
					new SqlParameter("@LeadStarEventId", SqlDbType.NVarChar,255),
					new SqlParameter("@Completed", SqlDbType.Bit,1),
					new SqlParameter("@WeekNo", SqlDbType.Int,4),
					new SqlParameter("@EventContent", SqlDbType.NVarChar),
					new SqlParameter("@EventURL", SqlDbType.NVarChar,255),
					new SqlParameter("@FileId", SqlDbType.Int,4)};
			parameters[0].Value = model.Action;
			parameters[1].Value = model.ExecutionDate;
			parameters[2].Value = model.LoanMarketingId;
			parameters[3].Value = model.LeadStarEventId;
			parameters[4].Value = model.Completed;
			parameters[5].Value = model.WeekNo;
			parameters[6].Value = model.EventContent;
			parameters[7].Value = model.EventURL;
			parameters[8].Value = model.FileId;

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
		public void Update(LPWeb.Model.LoanMarketingEvents model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update LoanMarketingEvents set ");
			strSql.Append("Action=@Action,");
			strSql.Append("ExecutionDate=@ExecutionDate,");
			strSql.Append("LoanMarketingId=@LoanMarketingId,");
			strSql.Append("LeadStarEventId=@LeadStarEventId,");
			strSql.Append("Completed=@Completed,");
			strSql.Append("WeekNo=@WeekNo,");
			strSql.Append("EventContent=@EventContent,");
			strSql.Append("EventURL=@EventURL,");
			strSql.Append("FileId=@FileId");
			strSql.Append(" where LoanMarketingEventId=@LoanMarketingEventId ");
			SqlParameter[] parameters = {
					new SqlParameter("@LoanMarketingEventId", SqlDbType.Int,4),
					new SqlParameter("@Action", SqlDbType.NVarChar,50),
					new SqlParameter("@ExecutionDate", SqlDbType.DateTime),
					new SqlParameter("@LoanMarketingId", SqlDbType.Int,4),
					new SqlParameter("@LeadStarEventId", SqlDbType.NVarChar,255),
					new SqlParameter("@Completed", SqlDbType.Bit,1),
					new SqlParameter("@WeekNo", SqlDbType.Int,4),
					new SqlParameter("@EventContent", SqlDbType.NVarChar),
					new SqlParameter("@EventURL", SqlDbType.NVarChar,255),
					new SqlParameter("@FileId", SqlDbType.Int,4)};
			parameters[0].Value = model.LoanMarketingEventId;
			parameters[1].Value = model.Action;
			parameters[2].Value = model.ExecutionDate;
			parameters[3].Value = model.LoanMarketingId;
			parameters[4].Value = model.LeadStarEventId;
			parameters[5].Value = model.Completed;
			parameters[6].Value = model.WeekNo;
			parameters[7].Value = model.EventContent;
			parameters[8].Value = model.EventURL;
			parameters[9].Value = model.FileId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int LoanMarketingEventId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from LoanMarketingEvents ");
			strSql.Append(" where LoanMarketingEventId=@LoanMarketingEventId ");
			SqlParameter[] parameters = {
					new SqlParameter("@LoanMarketingEventId", SqlDbType.Int,4)};
			parameters[0].Value = LoanMarketingEventId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.LoanMarketingEvents GetModel(int LoanMarketingEventId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 LoanMarketingEventId,Action,ExecutionDate,LoanMarketingId,LeadStarEventId,Completed,WeekNo,EventContent,EventURL,FileId from LoanMarketingEvents ");
			strSql.Append(" where LoanMarketingEventId=@LoanMarketingEventId ");
			SqlParameter[] parameters = {
					new SqlParameter("@LoanMarketingEventId", SqlDbType.Int,4)};
			parameters[0].Value = LoanMarketingEventId;

			LPWeb.Model.LoanMarketingEvents model=new LPWeb.Model.LoanMarketingEvents();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["LoanMarketingEventId"].ToString()!="")
				{
					model.LoanMarketingEventId=int.Parse(ds.Tables[0].Rows[0]["LoanMarketingEventId"].ToString());
				}
				model.Action=ds.Tables[0].Rows[0]["Action"].ToString();
				if(ds.Tables[0].Rows[0]["ExecutionDate"].ToString()!="")
				{
					model.ExecutionDate=DateTime.Parse(ds.Tables[0].Rows[0]["ExecutionDate"].ToString());
				}
				if(ds.Tables[0].Rows[0]["LoanMarketingId"].ToString()!="")
				{
					model.LoanMarketingId=int.Parse(ds.Tables[0].Rows[0]["LoanMarketingId"].ToString());
				}
				model.LeadStarEventId=ds.Tables[0].Rows[0]["LeadStarEventId"].ToString();
				if(ds.Tables[0].Rows[0]["Completed"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["Completed"].ToString()=="1")||(ds.Tables[0].Rows[0]["Completed"].ToString().ToLower()=="true"))
					{
						model.Completed=true;
					}
					else
					{
						model.Completed=false;
					}
				}
				if(ds.Tables[0].Rows[0]["WeekNo"].ToString()!="")
				{
					model.WeekNo=int.Parse(ds.Tables[0].Rows[0]["WeekNo"].ToString());
				}
				model.EventContent=ds.Tables[0].Rows[0]["EventContent"].ToString();
				model.EventURL=ds.Tables[0].Rows[0]["EventURL"].ToString();
				if(ds.Tables[0].Rows[0]["FileId"].ToString()!="")
				{
					model.FileId=int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
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
			strSql.Append("select LoanMarketingEventId,Action,ExecutionDate,LoanMarketingId,LeadStarEventId,Completed,WeekNo,EventContent,EventURL,FileId ");
			strSql.Append(" FROM LoanMarketingEvents ");
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
			strSql.Append(" LoanMarketingEventId,Action,ExecutionDate,LoanMarketingId,LeadStarEventId,Completed,WeekNo,EventContent,EventURL,FileId ");
			strSql.Append(" FROM LoanMarketingEvents ");
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
			parameters[0].Value = "LoanMarketingEvents";
			parameters[1].Value = "ID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  成员方法
    }
}
