using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����Template_Email_RecipientsBase��
	/// </summary>
	public class Template_Email_RecipientsBase
	{
		public Template_Email_RecipientsBase()
		{}
		#region  ��Ա����

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(LPWeb.Model.Template_Email_Recipients model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Template_Email_Recipients(");
			strSql.Append("TemplEmailId,EmailAddr,UserRoles,ContactRoles,RecipientType,TaskOwner)");
			strSql.Append(" values (");
			strSql.Append("@TemplEmailId,@EmailAddr,@UserRoles,@ContactRoles,@RecipientType,@TaskOwner)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@TemplEmailId", SqlDbType.Int,4),
					new SqlParameter("@EmailAddr", SqlDbType.NVarChar,255),
					new SqlParameter("@UserRoles", SqlDbType.NVarChar,255),
					new SqlParameter("@ContactRoles", SqlDbType.NVarChar,255),
					new SqlParameter("@RecipientType", SqlDbType.NVarChar,10),
					new SqlParameter("@TaskOwner", SqlDbType.Bit,1)};
			parameters[0].Value = model.TemplEmailId;
			parameters[1].Value = model.EmailAddr;
			parameters[2].Value = model.UserRoles;
			parameters[3].Value = model.ContactRoles;
			parameters[4].Value = model.RecipientType;
			parameters[5].Value = model.TaskOwner;

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
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.Template_Email_Recipients model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Template_Email_Recipients set ");
			strSql.Append("TemplRecipientId=@TemplRecipientId,");
			strSql.Append("TemplEmailId=@TemplEmailId,");
			strSql.Append("EmailAddr=@EmailAddr,");
			strSql.Append("UserRoles=@UserRoles,");
			strSql.Append("ContactRoles=@ContactRoles,");
			strSql.Append("RecipientType=@RecipientType,");
			strSql.Append("TaskOwner=@TaskOwner");
			strSql.Append(" where TemplRecipientId=@TemplRecipientId ");
			SqlParameter[] parameters = {
					new SqlParameter("@TemplRecipientId", SqlDbType.Int,4),
					new SqlParameter("@TemplEmailId", SqlDbType.Int,4),
					new SqlParameter("@EmailAddr", SqlDbType.NVarChar,255),
					new SqlParameter("@UserRoles", SqlDbType.NVarChar,255),
					new SqlParameter("@ContactRoles", SqlDbType.NVarChar,255),
					new SqlParameter("@RecipientType", SqlDbType.NVarChar,10),
					new SqlParameter("@TaskOwner", SqlDbType.Bit,1)};
			parameters[0].Value = model.TemplRecipientId;
			parameters[1].Value = model.TemplEmailId;
			parameters[2].Value = model.EmailAddr;
			parameters[3].Value = model.UserRoles;
			parameters[4].Value = model.ContactRoles;
			parameters[5].Value = model.RecipientType;
			parameters[6].Value = model.TaskOwner;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int TemplRecipientId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Template_Email_Recipients ");
			strSql.Append(" where TemplRecipientId=@TemplRecipientId ");
			SqlParameter[] parameters = {
					new SqlParameter("@TemplRecipientId", SqlDbType.Int,4)};
			parameters[0].Value = TemplRecipientId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.Template_Email_Recipients GetModel(int TemplRecipientId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 TemplRecipientId,TemplEmailId,EmailAddr,UserRoles,ContactRoles,RecipientType,TaskOwner from Template_Email_Recipients ");
			strSql.Append(" where TemplRecipientId=@TemplRecipientId ");
			SqlParameter[] parameters = {
					new SqlParameter("@TemplRecipientId", SqlDbType.Int,4)};
			parameters[0].Value = TemplRecipientId;

			LPWeb.Model.Template_Email_Recipients model=new LPWeb.Model.Template_Email_Recipients();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["TemplRecipientId"].ToString()!="")
				{
					model.TemplRecipientId=int.Parse(ds.Tables[0].Rows[0]["TemplRecipientId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["TemplEmailId"].ToString()!="")
				{
					model.TemplEmailId=int.Parse(ds.Tables[0].Rows[0]["TemplEmailId"].ToString());
				}
				model.EmailAddr=ds.Tables[0].Rows[0]["EmailAddr"].ToString();
				model.UserRoles=ds.Tables[0].Rows[0]["UserRoles"].ToString();
				model.ContactRoles=ds.Tables[0].Rows[0]["ContactRoles"].ToString();
				model.RecipientType=ds.Tables[0].Rows[0]["RecipientType"].ToString();
				if(ds.Tables[0].Rows[0]["TaskOwner"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["TaskOwner"].ToString()=="1")||(ds.Tables[0].Rows[0]["TaskOwner"].ToString().ToLower()=="true"))
					{
						model.TaskOwner=true;
					}
					else
					{
						model.TaskOwner=false;
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
		/// ��������б�
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select TemplRecipientId,TemplEmailId,EmailAddr,UserRoles,ContactRoles,RecipientType,TaskOwner ");
			strSql.Append(" FROM Template_Email_Recipients ");
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
			strSql.Append(" TemplRecipientId,TemplEmailId,EmailAddr,UserRoles,ContactRoles,RecipientType,TaskOwner ");
			strSql.Append(" FROM Template_Email_Recipients ");
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
			parameters[0].Value = "Template_Email_Recipients";
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

