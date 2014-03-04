using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类:LoanMarketing
	/// </summary>
	public class LoanMarketingBase
	{
        public LoanMarketingBase()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("LoanMarketingId", "LoanMarketing"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int LoanMarketingId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from LoanMarketing");
			strSql.Append(" where LoanMarketingId=@LoanMarketingId ");
			SqlParameter[] parameters = {
					new SqlParameter("@LoanMarketingId", SqlDbType.Int,4)};
			parameters[0].Value = LoanMarketingId;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LPWeb.Model.LoanMarketing model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into LoanMarketing(");
			strSql.Append("Selected,Type,Started,StartedBy,CampaignId,Status,FileId,SelectedBy)");
			strSql.Append(" values (");
			strSql.Append("@Selected,@Type,@Started,@StartedBy,@CampaignId,@Status,@FileId,@SelectedBy)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Selected", SqlDbType.DateTime),
					new SqlParameter("@Type", SqlDbType.NVarChar,50),
					new SqlParameter("@Started", SqlDbType.DateTime),
					new SqlParameter("@StartedBy", SqlDbType.Int,4),
					new SqlParameter("@CampaignId", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.NVarChar,50),
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@SelectedBy", SqlDbType.Int,4)};
			parameters[0].Value = model.Selected;
			parameters[1].Value = model.Type;
			parameters[2].Value = model.Started;
			parameters[3].Value = model.StartedBy;
			parameters[4].Value = model.CampaignId;
			parameters[5].Value = model.Status;
			parameters[6].Value = model.FileId;
			parameters[7].Value = model.SelectedBy;

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
		public bool Update(LPWeb.Model.LoanMarketing model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update LoanMarketing set ");
			strSql.Append("Selected=@Selected,");
			strSql.Append("Type=@Type,");
			strSql.Append("Started=@Started,");
			strSql.Append("StartedBy=@StartedBy,");
			strSql.Append("CampaignId=@CampaignId,");
			strSql.Append("Status=@Status,");
			strSql.Append("FileId=@FileId,");
			strSql.Append("SelectedBy=@SelectedBy");
			strSql.Append(" where LoanMarketingId=@LoanMarketingId");
			SqlParameter[] parameters = {
					new SqlParameter("@LoanMarketingId", SqlDbType.Int,4),
					new SqlParameter("@Selected", SqlDbType.DateTime),
					new SqlParameter("@Type", SqlDbType.NVarChar,50),
					new SqlParameter("@Started", SqlDbType.DateTime),
					new SqlParameter("@StartedBy", SqlDbType.Int,4),
					new SqlParameter("@CampaignId", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.NVarChar,50),
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@SelectedBy", SqlDbType.Int,4)};
			parameters[0].Value = model.LoanMarketingId;
			parameters[1].Value = model.Selected;
			parameters[2].Value = model.Type;
			parameters[3].Value = model.Started;
			parameters[4].Value = model.StartedBy;
			parameters[5].Value = model.CampaignId;
			parameters[6].Value = model.Status;
			parameters[7].Value = model.FileId;
			parameters[8].Value = model.SelectedBy;

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
		public bool Delete(int LoanMarketingId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from LoanMarketing ");
			strSql.Append(" where LoanMarketingId=@LoanMarketingId");
			SqlParameter[] parameters = {
					new SqlParameter("@LoanMarketingId", SqlDbType.Int,4)
};
			parameters[0].Value = LoanMarketingId;

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
		public bool DeleteList(string LoanMarketingIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from LoanMarketing ");
			strSql.Append(" where LoanMarketingId in ("+LoanMarketingIdlist + ")  ");
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
		public LPWeb.Model.LoanMarketing GetModel(int LoanMarketingId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 LoanMarketingId,Selected,Type,Started,StartedBy,CampaignId,Status,FileId,SelectedBy from LoanMarketing ");
			strSql.Append(" where LoanMarketingId=@LoanMarketingId");
			SqlParameter[] parameters = {
					new SqlParameter("@LoanMarketingId", SqlDbType.Int,4)
};
			parameters[0].Value = LoanMarketingId;

			LPWeb.Model.LoanMarketing model=new LPWeb.Model.LoanMarketing();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["LoanMarketingId"].ToString()!="")
				{
					model.LoanMarketingId=int.Parse(ds.Tables[0].Rows[0]["LoanMarketingId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Selected"].ToString()!="")
				{
					model.Selected=DateTime.Parse(ds.Tables[0].Rows[0]["Selected"].ToString());
				}
				model.Type=ds.Tables[0].Rows[0]["Type"].ToString();
				if(ds.Tables[0].Rows[0]["Started"].ToString()!="")
				{
					model.Started=DateTime.Parse(ds.Tables[0].Rows[0]["Started"].ToString());
				}
				if(ds.Tables[0].Rows[0]["StartedBy"].ToString()!="")
				{
					model.StartedBy=int.Parse(ds.Tables[0].Rows[0]["StartedBy"].ToString());
				}
				if(ds.Tables[0].Rows[0]["CampaignId"].ToString()!="")
				{
					model.CampaignId=int.Parse(ds.Tables[0].Rows[0]["CampaignId"].ToString());
				}
				model.Status=ds.Tables[0].Rows[0]["Status"].ToString();
				if(ds.Tables[0].Rows[0]["FileId"].ToString()!="")
				{
					model.FileId=int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["SelectedBy"].ToString()!="")
				{
					model.SelectedBy=int.Parse(ds.Tables[0].Rows[0]["SelectedBy"].ToString());
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
			strSql.Append("select LoanMarketingId,Selected,Type,Started,StartedBy,CampaignId,Status,FileId,SelectedBy ");
			strSql.Append(" FROM LoanMarketing ");
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
			strSql.Append(" LoanMarketingId,Selected,Type,Started,StartedBy,CampaignId,Status,FileId,SelectedBy ");
			strSql.Append(" FROM LoanMarketing ");
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
			parameters[0].Value = "LoanMarketing";
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

