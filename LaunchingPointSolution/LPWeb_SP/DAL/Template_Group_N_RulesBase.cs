using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Template_Group_N_Rules。
	/// </summary>
	public class Template_Group_N_RulesBase
	{
        public Template_Group_N_RulesBase()
		{}
		#region  成员方法

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.Template_Group_N_Rules model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Template_Group_N_Rules(");
			strSql.Append("RuleGroupId,RuleId)");
			strSql.Append(" values (");
			strSql.Append("@RuleGroupId,@RuleId)");
			SqlParameter[] parameters = {
					new SqlParameter("@RuleGroupId", SqlDbType.Int,4),
					new SqlParameter("@RuleId", SqlDbType.Int,4)};
			parameters[0].Value = model.RuleGroupId;
			parameters[1].Value = model.RuleId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.Template_Group_N_Rules model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Template_Group_N_Rules set ");
			strSql.Append("RuleGroupId=@RuleGroupId,");
			strSql.Append("RuleId=@RuleId");
			strSql.Append(" where RuleGroupId=@RuleGroupId and RuleId=@RuleId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RuleGroupId", SqlDbType.Int,4),
					new SqlParameter("@RuleId", SqlDbType.Int,4)};
			parameters[0].Value = model.RuleGroupId;
			parameters[1].Value = model.RuleId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int RuleGroupId,int RuleId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Template_Group_N_Rules ");
			strSql.Append(" where RuleGroupId=@RuleGroupId and RuleId=@RuleId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RuleGroupId", SqlDbType.Int,4),
					new SqlParameter("@RuleId", SqlDbType.Int,4)};
			parameters[0].Value = RuleGroupId;
			parameters[1].Value = RuleId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.Template_Group_N_Rules GetModel(int RuleGroupId,int RuleId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 RuleGroupId,RuleId from Template_Group_N_Rules ");
			strSql.Append(" where RuleGroupId=@RuleGroupId and RuleId=@RuleId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RuleGroupId", SqlDbType.Int,4),
					new SqlParameter("@RuleId", SqlDbType.Int,4)};
			parameters[0].Value = RuleGroupId;
			parameters[1].Value = RuleId;

			LPWeb.Model.Template_Group_N_Rules model=new LPWeb.Model.Template_Group_N_Rules();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["RuleGroupId"].ToString()!="")
				{
					model.RuleGroupId=int.Parse(ds.Tables[0].Rows[0]["RuleGroupId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["RuleId"].ToString()!="")
				{
					model.RuleId=int.Parse(ds.Tables[0].Rows[0]["RuleId"].ToString());
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
			strSql.Append("select RuleGroupId,RuleId ");
			strSql.Append(" FROM Template_Group_N_Rules ");
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
			strSql.Append(" RuleGroupId,RuleId ");
			strSql.Append(" FROM Template_Group_N_Rules ");
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
			parameters[0].Value = "Template_Group_N_Rules";
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

