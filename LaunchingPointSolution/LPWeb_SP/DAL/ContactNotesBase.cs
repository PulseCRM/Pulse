using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类ContactNotes。
	/// </summary>
	public class ContactNotesBase
	{
        public ContactNotesBase()
		{}
		#region  成员方法

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LPWeb.Model.ContactNotes model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ContactNotes(");
			strSql.Append("ContactId,Created,CreatedBy,Note)");
			strSql.Append(" values (");
			strSql.Append("@ContactId,@Created,@CreatedBy,@Note)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@CreatedBy", SqlDbType.Int,4),
					new SqlParameter("@Note", SqlDbType.NVarChar,500)};
			parameters[0].Value = model.ContactId;
			parameters[1].Value = model.Created;
			parameters[2].Value = model.CreatedBy;
			parameters[3].Value = model.Note;

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
		public void Update(LPWeb.Model.ContactNotes model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ContactNotes set ");
			strSql.Append("ContactNoteId=@ContactNoteId,");
			strSql.Append("ContactId=@ContactId,");
			strSql.Append("Created=@Created,");
			strSql.Append("CreatedBy=@CreatedBy,");
			strSql.Append("Note=@Note");
			strSql.Append(" where ContactNoteId=@ContactNoteId ");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactNoteId", SqlDbType.Int,4),
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@CreatedBy", SqlDbType.Int,4),
					new SqlParameter("@Note", SqlDbType.NVarChar,500)};
			parameters[0].Value = model.ContactNoteId;
			parameters[1].Value = model.ContactId;
			parameters[2].Value = model.Created;
			parameters[3].Value = model.CreatedBy;
			parameters[4].Value = model.Note;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ContactNoteId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ContactNotes ");
			strSql.Append(" where ContactNoteId=@ContactNoteId ");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactNoteId", SqlDbType.Int,4)};
			parameters[0].Value = ContactNoteId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.ContactNotes GetModel(int ContactNoteId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ContactNoteId,ContactId,Created,CreatedBy,Note from ContactNotes ");
			strSql.Append(" where ContactNoteId=@ContactNoteId ");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactNoteId", SqlDbType.Int,4)};
			parameters[0].Value = ContactNoteId;

			LPWeb.Model.ContactNotes model=new LPWeb.Model.ContactNotes();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["ContactNoteId"].ToString()!="")
				{
					model.ContactNoteId=int.Parse(ds.Tables[0].Rows[0]["ContactNoteId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ContactId"].ToString()!="")
				{
					model.ContactId=int.Parse(ds.Tables[0].Rows[0]["ContactId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Created"].ToString()!="")
				{
					model.Created=DateTime.Parse(ds.Tables[0].Rows[0]["Created"].ToString());
				}
				if(ds.Tables[0].Rows[0]["CreatedBy"].ToString()!="")
				{
					model.CreatedBy=int.Parse(ds.Tables[0].Rows[0]["CreatedBy"].ToString());
				}
				model.Note=ds.Tables[0].Rows[0]["Note"].ToString();
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
			strSql.Append("select ContactNoteId,ContactId,Created,CreatedBy,Note ");
			strSql.Append(" FROM ContactNotes ");
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
			strSql.Append(" ContactNoteId,ContactId,Created,CreatedBy,Note ");
			strSql.Append(" FROM ContactNotes ");
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
			parameters[0].Value = "ContactNotes";
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

