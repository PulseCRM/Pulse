using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类:MailChimpCampaigns
	/// </summary>
	public partial class MailChimpCampaigns 
	{
		public MailChimpCampaigns()
		{}
		#region  Method

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string CID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from MailChimpCampaigns");
			strSql.Append(" where CID=@CID ");
			SqlParameter[] parameters = {
					new SqlParameter("@CID", SqlDbType.NVarChar,255)};
			parameters[0].Value = CID;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.MailChimpCampaigns model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into MailChimpCampaigns(");
			strSql.Append("CID,Name,BranchId)");
			strSql.Append(" values (");
			strSql.Append("@CID,@Name,@BranchId)");
			SqlParameter[] parameters = {
					new SqlParameter("@CID", SqlDbType.NVarChar,255),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@BranchId", SqlDbType.Int,4)};
			parameters[0].Value = model.CID;
			parameters[1].Value = model.Name;
			parameters[2].Value = model.BranchId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.MailChimpCampaigns model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update MailChimpCampaigns set ");
			strSql.Append("Name=@Name,");
			strSql.Append("BranchId=@BranchId");
			strSql.Append(" where CID=@CID ");
			SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@BranchId", SqlDbType.Int,4),
					new SqlParameter("@CID", SqlDbType.NVarChar,255)};
			parameters[0].Value = model.Name;
			parameters[1].Value = model.BranchId;
			parameters[2].Value = model.CID;

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
		public bool Delete(string CID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from MailChimpCampaigns ");
			strSql.Append(" where CID=@CID ");
			SqlParameter[] parameters = {
					new SqlParameter("@CID", SqlDbType.NVarChar,255)};
			parameters[0].Value = CID;

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
		public bool DeleteList(string CIDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from MailChimpCampaigns ");
			strSql.Append(" where CID in ("+CIDlist + ")  ");
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
		public LPWeb.Model.MailChimpCampaigns GetModel(string CID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 CID,Name,BranchId from MailChimpCampaigns ");
			strSql.Append(" where CID=@CID ");
			SqlParameter[] parameters = {
					new SqlParameter("@CID", SqlDbType.NVarChar,255)};
			parameters[0].Value = CID;

			LPWeb.Model.MailChimpCampaigns model=new LPWeb.Model.MailChimpCampaigns();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["CID"]!=null && ds.Tables[0].Rows[0]["CID"].ToString()!="")
				{
					model.CID=ds.Tables[0].Rows[0]["CID"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Name"]!=null && ds.Tables[0].Rows[0]["Name"].ToString()!="")
				{
					model.Name=ds.Tables[0].Rows[0]["Name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["BranchId"]!=null && ds.Tables[0].Rows[0]["BranchId"].ToString()!="")
				{
					model.BranchId=int.Parse(ds.Tables[0].Rows[0]["BranchId"].ToString());
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
			strSql.Append("select CID,Name,BranchId ");
			strSql.Append(" FROM MailChimpCampaigns ");
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
			strSql.Append(" CID,Name,BranchId ");
			strSql.Append(" FROM MailChimpCampaigns ");
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
			parameters[0].Value = "MailChimpCampaigns";
			parameters[1].Value = "CID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		} 

		#endregion  Method
	}
}

