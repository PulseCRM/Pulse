using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����AutoCampaigns��
	/// </summary>
	public class AutoCampaignsBase
	{
        public AutoCampaignsBase()
		{}
		#region  ��Ա����

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.AutoCampaigns model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into AutoCampaigns(");
			strSql.Append("CampaignId,PaidBy,Enabled,SelectedBy,Started,LoanType,TemplStageId)");
			strSql.Append(" values (");
			strSql.Append("@CampaignId,@PaidBy,@Enabled,@SelectedBy,@Started,@LoanType,@TemplStageId)");
			SqlParameter[] parameters = {
					new SqlParameter("@CampaignId", SqlDbType.Int,4),
					new SqlParameter("@PaidBy", SqlDbType.SmallInt,2),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@SelectedBy", SqlDbType.Int,4),
					new SqlParameter("@Started", SqlDbType.DateTime),
					new SqlParameter("@LoanType", SqlDbType.NVarChar,50),
					new SqlParameter("@TemplStageId", SqlDbType.Int,4)};
			parameters[0].Value = model.CampaignId;
			parameters[1].Value = model.PaidBy;
			parameters[2].Value = model.Enabled;
			parameters[3].Value = model.SelectedBy;
			parameters[4].Value = model.Started;
			parameters[5].Value = model.LoanType;
			parameters[6].Value = model.TemplStageId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LPWeb.Model.AutoCampaigns model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update AutoCampaigns set ");
			strSql.Append("CampaignId=@CampaignId,");
			strSql.Append("PaidBy=@PaidBy,");
			strSql.Append("Enabled=@Enabled,");
			strSql.Append("SelectedBy=@SelectedBy,");
			strSql.Append("Started=@Started,");
			strSql.Append("LoanType=@LoanType,");
			strSql.Append("TemplStageId=@TemplStageId");
			strSql.Append(" where CampaignId=@CampaignId ");
			SqlParameter[] parameters = {
					new SqlParameter("@CampaignId", SqlDbType.Int,4),
					new SqlParameter("@PaidBy", SqlDbType.SmallInt,2),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@SelectedBy", SqlDbType.Int,4),
					new SqlParameter("@Started", SqlDbType.DateTime),
					new SqlParameter("@LoanType", SqlDbType.NVarChar,50),
					new SqlParameter("@TemplStageId", SqlDbType.Int,4)};
			parameters[0].Value = model.CampaignId;
			parameters[1].Value = model.PaidBy;
			parameters[2].Value = model.Enabled;
			parameters[3].Value = model.SelectedBy;
			parameters[4].Value = model.Started;
			parameters[5].Value = model.LoanType;
			parameters[6].Value = model.TemplStageId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int CampaignId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from AutoCampaigns ");
			strSql.Append(" where CampaignId=@CampaignId ");
			SqlParameter[] parameters = {
					new SqlParameter("@CampaignId", SqlDbType.Int,4)};
			parameters[0].Value = CampaignId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.AutoCampaigns GetModel(int CampaignId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 CampaignId,PaidBy,Enabled,SelectedBy,Started,LoanType,TemplStageId from AutoCampaigns ");
			strSql.Append(" where CampaignId=@CampaignId ");
			SqlParameter[] parameters = {
					new SqlParameter("@CampaignId", SqlDbType.Int,4)};
			parameters[0].Value = CampaignId;

			LPWeb.Model.AutoCampaigns model=new LPWeb.Model.AutoCampaigns();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["CampaignId"].ToString()!="")
				{
					model.CampaignId=int.Parse(ds.Tables[0].Rows[0]["CampaignId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["PaidBy"].ToString()!="")
				{
					model.PaidBy=int.Parse(ds.Tables[0].Rows[0]["PaidBy"].ToString());
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
				if(ds.Tables[0].Rows[0]["SelectedBy"].ToString()!="")
				{
					model.SelectedBy=int.Parse(ds.Tables[0].Rows[0]["SelectedBy"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Started"].ToString()!="")
				{
					model.Started=DateTime.Parse(ds.Tables[0].Rows[0]["Started"].ToString());
				}
				model.LoanType=ds.Tables[0].Rows[0]["LoanType"].ToString();
				if(ds.Tables[0].Rows[0]["TemplStageId"].ToString()!="")
				{
					model.TemplStageId=int.Parse(ds.Tables[0].Rows[0]["TemplStageId"].ToString());
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
			strSql.Append("select CampaignId,PaidBy,Enabled,SelectedBy,Started,LoanType,TemplStageId ");
			strSql.Append(" FROM AutoCampaigns ");
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
			strSql.Append(" CampaignId,PaidBy,Enabled,SelectedBy,Started,LoanType,TemplStageId ");
			strSql.Append(" FROM AutoCampaigns ");
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
			parameters[0].Value = "AutoCampaigns";
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

