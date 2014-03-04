using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����:MarketingSettings
	/// </summary>
	public class MarketingSettings
	{
		public MarketingSettings()
		{}
		#region  Method



		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.MarketingSettings model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into MarketingSettings(");
			strSql.Append("WebServiceURL,CampaignDetailURL,ReconcileInterval)");
			strSql.Append(" values (");
			strSql.Append("@WebServiceURL,@CampaignDetailURL,@ReconcileInterval)");
			SqlParameter[] parameters = {
					new SqlParameter("@WebServiceURL", SqlDbType.NVarChar,255),
					new SqlParameter("@CampaignDetailURL", SqlDbType.NVarChar,500),
					new SqlParameter("@ReconcileInterval", SqlDbType.Int,4)};
			parameters[0].Value = model.WebServiceURL;
			parameters[1].Value = model.CampaignDetailURL;
			parameters[2].Value = model.ReconcileInterval;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(LPWeb.Model.MarketingSettings model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update MarketingSettings set ");
			strSql.Append("WebServiceURL=@WebServiceURL,");
			strSql.Append("CampaignDetailURL=@CampaignDetailURL,");
			strSql.Append("ReconcileInterval=@ReconcileInterval");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
					new SqlParameter("@WebServiceURL", SqlDbType.NVarChar,255),
					new SqlParameter("@CampaignDetailURL", SqlDbType.NVarChar,500),
					new SqlParameter("@ReconcileInterval", SqlDbType.Int,4)};
			parameters[0].Value = model.WebServiceURL;
			parameters[1].Value = model.CampaignDetailURL;
			parameters[2].Value = model.ReconcileInterval;

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
		public bool Delete()
		{
			//�ñ���������Ϣ�����Զ�������/�����ֶ�
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from MarketingSettings ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
};

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
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.MarketingSettings GetModel()
		{
			//�ñ���������Ϣ�����Զ�������/�����ֶ�
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 WebServiceURL,CampaignDetailURL,ReconcileInterval from MarketingSettings ");

			LPWeb.Model.MarketingSettings model=new LPWeb.Model.MarketingSettings();
			DataSet ds=DbHelperSQL.Query(strSql.ToString());
			if(ds.Tables[0].Rows.Count>0)
			{
				model.WebServiceURL=ds.Tables[0].Rows[0]["WebServiceURL"].ToString();
				model.CampaignDetailURL=ds.Tables[0].Rows[0]["CampaignDetailURL"].ToString();
				if(ds.Tables[0].Rows[0]["ReconcileInterval"].ToString()!="")
				{
					model.ReconcileInterval=int.Parse(ds.Tables[0].Rows[0]["ReconcileInterval"].ToString());
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
			strSql.Append("select WebServiceURL,CampaignDetailURL,ReconcileInterval ");
			strSql.Append(" FROM MarketingSettings ");
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
			strSql.Append(" WebServiceURL,CampaignDetailURL,ReconcileInterval ");
			strSql.Append(" FROM MarketingSettings ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/*
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
			parameters[0].Value = "MarketingSettings";
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

