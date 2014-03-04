using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����:User2State
	/// </summary>
	public class User2State
	{
		public User2State()
		{}
		#region  Method

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("UserID", "User2State"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int UserID,string State)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from User2State");
			strSql.Append(" where UserID=@UserID and State=@State ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.NVarChar,50)};
			parameters[0].Value = UserID;
			parameters[1].Value = State;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(LPWeb.Model.User2State model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into User2State(");
			strSql.Append("UserID,State)");
			strSql.Append(" values (");
			strSql.Append("@UserID,@State)");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.NVarChar,2)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.State;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(LPWeb.Model.User2State model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update User2State set ");
#warning ϵͳ����ȱ�ٸ��µ��ֶΣ����ֹ�ȷ����˸����Ƿ���ȷ�� 
			strSql.Append("UserID=@UserID,");
			strSql.Append("State=@State");
			strSql.Append(" where UserID=@UserID and State=@State ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.NVarChar,2)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.State;

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
		public bool Delete(int UserID,string State)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from User2State ");
			strSql.Append(" where UserID=@UserID and State=@State ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.NVarChar,50)};
			parameters[0].Value = UserID;
			parameters[1].Value = State;

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
        public bool DeleteByUserID(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from User2State ");
            strSql.Append(" where UserID=@UserID");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = UserID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
		public LPWeb.Model.User2State GetModel(int UserID,string State)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 UserID,State from User2State ");
			strSql.Append(" where UserID=@UserID and State=@State ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.NVarChar,50)};
			parameters[0].Value = UserID;
			parameters[1].Value = State;

			LPWeb.Model.User2State model=new LPWeb.Model.User2State();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["UserID"].ToString()!="")
				{
					model.UserID=int.Parse(ds.Tables[0].Rows[0]["UserID"].ToString());
				}
				model.State=ds.Tables[0].Rows[0]["State"].ToString();
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
			strSql.Append("select UserID,State ");
			strSql.Append(" FROM User2State ");
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
			strSql.Append(" UserID,State ");
			strSql.Append(" FROM User2State ");
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
			parameters[0].Value = "User2State";
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

