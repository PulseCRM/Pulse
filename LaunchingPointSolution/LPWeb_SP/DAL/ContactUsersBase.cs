using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����ContactUsers��
	/// </summary>
	public class ContactUsersBase
	{
        public ContactUsersBase()
		{}
		#region  ��Ա����

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.ContactUsers model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ContactUsers(");
			strSql.Append("UserId,ContactId,Enabled,Created)");
			strSql.Append(" values (");
			strSql.Append("@UserId,@ContactId,@Enabled,@Created)");
			SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Created", SqlDbType.DateTime)};
			parameters[0].Value = model.UserId;
			parameters[1].Value = model.ContactId;
			parameters[2].Value = model.Enabled;
			parameters[3].Value = model.Created;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.ContactUsers model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ContactUsers set ");
			strSql.Append("UserId=@UserId,");
			strSql.Append("ContactId=@ContactId,");
			strSql.Append("Enabled=@Enabled,");
			strSql.Append("Created=@Created");
			strSql.Append(" where UserId=@UserId and ContactId=@ContactId ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Created", SqlDbType.DateTime)};
			parameters[0].Value = model.UserId;
			parameters[1].Value = model.ContactId;
			parameters[2].Value = model.Enabled;
			parameters[3].Value = model.Created;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int UserId,int ContactId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ContactUsers ");
			strSql.Append(" where UserId=@UserId and ContactId=@ContactId ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@ContactId", SqlDbType.Int,4)};
			parameters[0].Value = UserId;
			parameters[1].Value = ContactId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.ContactUsers GetModel(int UserId,int ContactId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 UserId,ContactId,Enabled,Created from ContactUsers ");
			strSql.Append(" where UserId=@UserId and ContactId=@ContactId ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@ContactId", SqlDbType.Int,4)};
			parameters[0].Value = UserId;
			parameters[1].Value = ContactId;

			LPWeb.Model.ContactUsers model=new LPWeb.Model.ContactUsers();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["UserId"].ToString()!="")
				{
					model.UserId=int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ContactId"].ToString()!="")
				{
					model.ContactId=int.Parse(ds.Tables[0].Rows[0]["ContactId"].ToString());
				}
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
		/// ��������б�
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select UserId,ContactId,Enabled,Created ");
			strSql.Append(" FROM ContactUsers ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// ���ǰ��������
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" UserId,ContactId,Enabled,Created ");
			strSql.Append(" FROM ContactUsers ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		
		/// <summary>
		/// ��ҳ��ȡ�����б�
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
			parameters[0].Value = "ContactUsers";
			parameters[1].Value = "ID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}

		#endregion  ��Ա����
	}
}

