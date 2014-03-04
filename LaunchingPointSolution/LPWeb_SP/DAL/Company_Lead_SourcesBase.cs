using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����:Company_Lead_Sources
	/// </summary>
	public class Company_Lead_SourcesBase
	{
        public Company_Lead_SourcesBase()
		{}
		#region  Method

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("LeadSourceID", "Company_Lead_Sources"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int LeadSourceID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Company_Lead_Sources");
			strSql.Append(" where LeadSourceID=@LeadSourceID ");
			SqlParameter[] parameters = {
					new SqlParameter("@LeadSourceID", SqlDbType.Int,4)};
			parameters[0].Value = LeadSourceID;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(LPWeb.Model.Company_Lead_Sources model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Company_Lead_Sources(");
			strSql.Append("LeadSource,DefaultUserId,Default)");
			strSql.Append(" values (");
			strSql.Append("@LeadSource,@DefaultUserId,@Default)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@LeadSource", SqlDbType.NVarChar,150),
					new SqlParameter("@DefaultUserId", SqlDbType.Int,4),
					new SqlParameter("@Default", SqlDbType.Bit,1)};
			parameters[0].Value = model.LeadSource;
			parameters[1].Value = model.DefaultUserId;
			parameters[2].Value = model.Default;

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
		public bool Update(LPWeb.Model.Company_Lead_Sources model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Company_Lead_Sources set ");
			strSql.Append("LeadSource=@LeadSource,");
			strSql.Append("DefaultUserId=@DefaultUserId,");
			strSql.Append("Default=@Default");
			strSql.Append(" where LeadSourceID=@LeadSourceID");
			SqlParameter[] parameters = {
					new SqlParameter("@LeadSourceID", SqlDbType.Int,4),
					new SqlParameter("@LeadSource", SqlDbType.NVarChar,150),
					new SqlParameter("@DefaultUserId", SqlDbType.Int,4),
					new SqlParameter("@Default", SqlDbType.Bit,1)};
			parameters[0].Value = model.LeadSourceID;
			parameters[1].Value = model.LeadSource;
			parameters[2].Value = model.DefaultUserId;
			parameters[3].Value = model.Default;

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
		/// ɾ��һ������
		/// </summary>
		public bool Delete(int LeadSourceID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Company_Lead_Sources ");
			strSql.Append(" where LeadSourceID=@LeadSourceID");
			SqlParameter[] parameters = {
					new SqlParameter("@LeadSourceID", SqlDbType.Int,4)
};
			parameters[0].Value = LeadSourceID;

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
		/// ɾ��һ������
		/// </summary>
		public bool DeleteList(string LeadSourceIDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Company_Lead_Sources ");
			strSql.Append(" where LeadSourceID in ("+LeadSourceIDlist + ")  ");
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
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.Company_Lead_Sources GetModel(int LeadSourceID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 LeadSourceID,LeadSource,DefaultUserId,Default from Company_Lead_Sources ");
			strSql.Append(" where LeadSourceID=@LeadSourceID");
			SqlParameter[] parameters = {
					new SqlParameter("@LeadSourceID", SqlDbType.Int,4)
};
			parameters[0].Value = LeadSourceID;

			LPWeb.Model.Company_Lead_Sources model=new LPWeb.Model.Company_Lead_Sources();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["LeadSourceID"].ToString()!="")
				{
					model.LeadSourceID=int.Parse(ds.Tables[0].Rows[0]["LeadSourceID"].ToString());
				}
				model.LeadSource=ds.Tables[0].Rows[0]["LeadSource"].ToString();
				if(ds.Tables[0].Rows[0]["DefaultUserId"].ToString()!="")
				{
					model.DefaultUserId=int.Parse(ds.Tables[0].Rows[0]["DefaultUserId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Default"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["Default"].ToString()=="1")||(ds.Tables[0].Rows[0]["Default"].ToString().ToLower()=="true"))
					{
						model.Default=true;
					}
					else
					{
						model.Default=false;
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
			strSql.Append("select LeadSourceID,LeadSource,DefaultUserId,Default ");
			strSql.Append(" FROM Company_Lead_Sources ");
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
			strSql.Append(" LeadSourceID,LeadSource,DefaultUserId,Default ");
			strSql.Append(" FROM Company_Lead_Sources ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		
		#endregion  Method
	}
}

