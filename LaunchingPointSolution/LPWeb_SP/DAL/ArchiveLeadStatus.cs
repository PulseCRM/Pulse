using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类:ArchiveLeadStatus
	/// </summary>
	public class ArchiveLeadStatus
	{
		public ArchiveLeadStatus()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("LeadStatusId", "ArchiveLeadStatus"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int LeadStatusId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from ArchiveLeadStatus");
			strSql.Append(" where LeadStatusId=@LeadStatusId ");
			SqlParameter[] parameters = {
					new SqlParameter("@LeadStatusId", SqlDbType.Int,4)};
			parameters[0].Value = LeadStatusId;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LPWeb.Model.ArchiveLeadStatus model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ArchiveLeadStatus(");
			strSql.Append("LeadStatusName,Enabled)");
			strSql.Append(" values (");
			strSql.Append("@LeadStatusName,@Enabled)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@LeadStatusName", SqlDbType.NVarChar,255),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
			parameters[0].Value = model.LeadStatusName;
			parameters[1].Value = model.Enabled;

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
		public bool Update(LPWeb.Model.ArchiveLeadStatus model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ArchiveLeadStatus set ");
			strSql.Append("LeadStatusName=@LeadStatusName,");
			strSql.Append("Enabled=@Enabled");
			strSql.Append(" where LeadStatusId=@LeadStatusId");
			SqlParameter[] parameters = {
					new SqlParameter("@LeadStatusId", SqlDbType.Int,4),
					new SqlParameter("@LeadStatusName", SqlDbType.NVarChar,255),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
			parameters[0].Value = model.LeadStatusId;
			parameters[1].Value = model.LeadStatusName;
			parameters[2].Value = model.Enabled;

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
		public bool Delete(int LeadStatusId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ArchiveLeadStatus ");
			strSql.Append(" where LeadStatusId=@LeadStatusId");
			SqlParameter[] parameters = {
					new SqlParameter("@LeadStatusId", SqlDbType.Int,4)
};
			parameters[0].Value = LeadStatusId;

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
		public bool DeleteList(string LeadStatusIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ArchiveLeadStatus ");
			strSql.Append(" where LeadStatusId in ("+LeadStatusIdlist + ")  ");
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
		public LPWeb.Model.ArchiveLeadStatus GetModel(int LeadStatusId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 LeadStatusId,LeadStatusName,Enabled from ArchiveLeadStatus ");
			strSql.Append(" where LeadStatusId=@LeadStatusId");
			SqlParameter[] parameters = {
					new SqlParameter("@LeadStatusId", SqlDbType.Int,4)
};
			parameters[0].Value = LeadStatusId;

			LPWeb.Model.ArchiveLeadStatus model=new LPWeb.Model.ArchiveLeadStatus();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["LeadStatusId"].ToString()!="")
				{
					model.LeadStatusId=int.Parse(ds.Tables[0].Rows[0]["LeadStatusId"].ToString());
				}
				model.LeadStatusName=ds.Tables[0].Rows[0]["LeadStatusName"].ToString();
				if(ds.Tables[0].Rows[0]["Enabled"].ToString()!="")
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
			strSql.Append("select LeadStatusId,LeadStatusName,Enabled ");
			strSql.Append(" FROM ArchiveLeadStatus ");
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
			strSql.Append(" LeadStatusId,LeadStatusName,Enabled ");
			strSql.Append(" FROM ArchiveLeadStatus ");
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
			parameters[0].Value = "ArchiveLeadStatus";
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

