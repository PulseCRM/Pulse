using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类:ContactMailCampaign
	/// </summary>
	public partial class ContactMailCampaign 
	{
		public ContactMailCampaign()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("ContactMailCampaignId", "ContactMailCampaign"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ContactMailCampaignId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from ContactMailCampaign");
			strSql.Append(" where ContactMailCampaignId=@ContactMailCampaignId");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactMailCampaignId", SqlDbType.Int,4)
};
			parameters[0].Value = ContactMailCampaignId;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LPWeb.Model.ContactMailCampaign model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ContactMailCampaign(");
			strSql.Append("ContactId,CID,LID,BranchId,Added,AddedBy,Result)");
			strSql.Append(" values (");
			strSql.Append("@ContactId,@CID,@LID,@BranchId,@Added,@AddedBy,@Result)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@CID", SqlDbType.NVarChar,255),
					new SqlParameter("@LID", SqlDbType.NVarChar,255),
					new SqlParameter("@BranchId", SqlDbType.Int,4),
					new SqlParameter("@Added", SqlDbType.DateTime),
					new SqlParameter("@AddedBy", SqlDbType.Int,4),
					new SqlParameter("@Result", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.ContactId;
			parameters[1].Value = model.CID;
			parameters[2].Value = model.LID;
			parameters[3].Value = model.BranchId;
			parameters[4].Value = model.Added;
			parameters[5].Value = model.AddedBy;
			parameters[6].Value = model.Result;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.ContactMailCampaign model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ContactMailCampaign set ");
			strSql.Append("ContactId=@ContactId,");
			strSql.Append("CID=@CID,");
			strSql.Append("LID=@LID,");
			strSql.Append("BranchId=@BranchId,");
			strSql.Append("Added=@Added,");
			strSql.Append("AddedBy=@AddedBy,");
			strSql.Append("Result=@Result");
			strSql.Append(" where ContactMailCampaignId=@ContactMailCampaignId");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@CID", SqlDbType.NVarChar,255),
					new SqlParameter("@LID", SqlDbType.NVarChar,255),
					new SqlParameter("@BranchId", SqlDbType.Int,4),
					new SqlParameter("@Added", SqlDbType.DateTime),
					new SqlParameter("@AddedBy", SqlDbType.Int,4),
					new SqlParameter("@Result", SqlDbType.NVarChar,50),
					new SqlParameter("@ContactMailCampaignId", SqlDbType.Int,4)};
			parameters[0].Value = model.ContactId;
			parameters[1].Value = model.CID;
			parameters[2].Value = model.LID;
			parameters[3].Value = model.BranchId;
			parameters[4].Value = model.Added;
			parameters[5].Value = model.AddedBy;
			parameters[6].Value = model.Result;
			parameters[7].Value = model.ContactMailCampaignId;

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
		public bool Delete(int ContactMailCampaignId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ContactMailCampaign ");
			strSql.Append(" where ContactMailCampaignId=@ContactMailCampaignId");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactMailCampaignId", SqlDbType.Int,4)
};
			parameters[0].Value = ContactMailCampaignId;

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
		public bool DeleteList(string ContactMailCampaignIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ContactMailCampaign ");
			strSql.Append(" where ContactMailCampaignId in ("+ContactMailCampaignIdlist + ")  ");
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
		public LPWeb.Model.ContactMailCampaign GetModel(int ContactMailCampaignId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ContactMailCampaignId,ContactId,CID,LID,BranchId,Added,AddedBy,Result from ContactMailCampaign ");
			strSql.Append(" where ContactMailCampaignId=@ContactMailCampaignId");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactMailCampaignId", SqlDbType.Int,4)
};
			parameters[0].Value = ContactMailCampaignId;

			LPWeb.Model.ContactMailCampaign model=new LPWeb.Model.ContactMailCampaign();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["ContactMailCampaignId"]!=null && ds.Tables[0].Rows[0]["ContactMailCampaignId"].ToString()!="")
				{
					model.ContactMailCampaignId=int.Parse(ds.Tables[0].Rows[0]["ContactMailCampaignId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ContactId"]!=null && ds.Tables[0].Rows[0]["ContactId"].ToString()!="")
				{
					model.ContactId=int.Parse(ds.Tables[0].Rows[0]["ContactId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["CID"]!=null && ds.Tables[0].Rows[0]["CID"].ToString()!="")
				{
					model.CID=ds.Tables[0].Rows[0]["CID"].ToString();
				}
				if(ds.Tables[0].Rows[0]["LID"]!=null && ds.Tables[0].Rows[0]["LID"].ToString()!="")
				{
					model.LID=ds.Tables[0].Rows[0]["LID"].ToString();
				}
				if(ds.Tables[0].Rows[0]["BranchId"]!=null && ds.Tables[0].Rows[0]["BranchId"].ToString()!="")
				{
					model.BranchId=int.Parse(ds.Tables[0].Rows[0]["BranchId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Added"]!=null && ds.Tables[0].Rows[0]["Added"].ToString()!="")
				{
					model.Added=DateTime.Parse(ds.Tables[0].Rows[0]["Added"].ToString());
				}
				if(ds.Tables[0].Rows[0]["AddedBy"]!=null && ds.Tables[0].Rows[0]["AddedBy"].ToString()!="")
				{
					model.AddedBy=int.Parse(ds.Tables[0].Rows[0]["AddedBy"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Result"]!=null && ds.Tables[0].Rows[0]["Result"].ToString()!="")
				{
					model.Result=ds.Tables[0].Rows[0]["Result"].ToString();
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
			strSql.Append("select ContactMailCampaignId,ContactId,CID,LID,BranchId,Added,AddedBy,Result ");
			strSql.Append(" FROM ContactMailCampaign ");
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
			strSql.Append(" ContactMailCampaignId,ContactId,CID,LID,BranchId,Added,AddedBy,Result ");
			strSql.Append(" FROM ContactMailCampaign ");
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
			parameters[0].Value = "ContactMailCampaign";
			parameters[1].Value = "ContactMailCampaignId";
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

