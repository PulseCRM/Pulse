using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����ContactActivities��
	/// </summary>
	public class ContactActivitiesBase
	{
        public ContactActivitiesBase()
		{}
		#region  ��Ա����

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(LPWeb.Model.ContactActivities model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ContactActivities(");
			strSql.Append("ContactId,UserId,ActivityName,ActivityTime)");
			strSql.Append(" values (");
			strSql.Append("@ContactId,@UserId,@ActivityName,@ActivityTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@ActivityName", SqlDbType.NVarChar,500),
					new SqlParameter("@ActivityTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ContactId;
			parameters[1].Value = model.UserId;
			parameters[2].Value = model.ActivityName;
			parameters[3].Value = model.ActivityTime;

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
		public void Update(LPWeb.Model.ContactActivities model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ContactActivities set ");
			strSql.Append("ContactActivityId=@ContactActivityId,");
			strSql.Append("ContactId=@ContactId,");
			strSql.Append("UserId=@UserId,");
			strSql.Append("ActivityName=@ActivityName,");
			strSql.Append("ActivityTime=@ActivityTime");
			strSql.Append(" where ContactActivityId=@ContactActivityId ");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactActivityId", SqlDbType.Int,4),
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@ActivityName", SqlDbType.NVarChar,500),
					new SqlParameter("@ActivityTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ContactActivityId;
			parameters[1].Value = model.ContactId;
			parameters[2].Value = model.UserId;
			parameters[3].Value = model.ActivityName;
			parameters[4].Value = model.ActivityTime;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ContactActivityId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ContactActivities ");
			strSql.Append(" where ContactActivityId=@ContactActivityId ");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactActivityId", SqlDbType.Int,4)};
			parameters[0].Value = ContactActivityId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LPWeb.Model.ContactActivities GetModel(int ContactActivityId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ContactActivityId,ContactId,UserId,ActivityName,ActivityTime from ContactActivities ");
			strSql.Append(" where ContactActivityId=@ContactActivityId ");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactActivityId", SqlDbType.Int,4)};
			parameters[0].Value = ContactActivityId;

			LPWeb.Model.ContactActivities model=new LPWeb.Model.ContactActivities();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["ContactActivityId"].ToString()!="")
				{
					model.ContactActivityId=int.Parse(ds.Tables[0].Rows[0]["ContactActivityId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ContactId"].ToString()!="")
				{
					model.ContactId=int.Parse(ds.Tables[0].Rows[0]["ContactId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["UserId"].ToString()!="")
				{
					model.UserId=int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
				}
				model.ActivityName=ds.Tables[0].Rows[0]["ActivityName"].ToString();
				if(ds.Tables[0].Rows[0]["ActivityTime"].ToString()!="")
				{
					model.ActivityTime=DateTime.Parse(ds.Tables[0].Rows[0]["ActivityTime"].ToString());
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
			strSql.Append("select ContactActivityId,ContactId,UserId,ActivityName,ActivityTime ");
			strSql.Append(" FROM ContactActivities ");
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
			strSql.Append(" ContactActivityId,ContactId,UserId,ActivityName,ActivityTime ");
			strSql.Append(" FROM ContactActivities ");
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
			parameters[0].Value = "ContactActivities";
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

