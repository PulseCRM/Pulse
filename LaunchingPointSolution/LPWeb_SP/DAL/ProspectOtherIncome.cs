using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类:ProspectOtherIncome
	/// </summary>
    public partial class ProspectOtherIncome : ProspectOtherIncomeBase
	{
		public ProspectOtherIncome()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("ProspectOtherIncomeId", "ProspectOtherIncome"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ProspectOtherIncomeId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from ProspectOtherIncome");
			strSql.Append(" where ProspectOtherIncomeId=@ProspectOtherIncomeId");
			SqlParameter[] parameters = {
					new SqlParameter("@ProspectOtherIncomeId", SqlDbType.Int,4)
};
			parameters[0].Value = ProspectOtherIncomeId;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LPWeb.Model.ProspectOtherIncome model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ProspectOtherIncome(");
			strSql.Append("ContactId,Type,MonthlyIncome)");
			strSql.Append(" values (");
			strSql.Append("@ContactId,@Type,@MonthlyIncome)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.NVarChar,100),
					new SqlParameter("@MonthlyIncome", SqlDbType.Decimal,5)};
			parameters[0].Value = model.ContactId;
			parameters[1].Value = model.Type;
			parameters[2].Value = model.MonthlyIncome;

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
		public bool Update(LPWeb.Model.ProspectOtherIncome model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ProspectOtherIncome set ");
			strSql.Append("ContactId=@ContactId,");
			strSql.Append("Type=@Type,");
			strSql.Append("MonthlyIncome=@MonthlyIncome");
			strSql.Append(" where ProspectOtherIncomeId=@ProspectOtherIncomeId");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.NVarChar,100),
					new SqlParameter("@MonthlyIncome", SqlDbType.Decimal,5),
					new SqlParameter("@ProspectOtherIncomeId", SqlDbType.Int,4)};
			parameters[0].Value = model.ContactId;
			parameters[1].Value = model.Type;
			parameters[2].Value = model.MonthlyIncome;
			parameters[3].Value = model.ProspectOtherIncomeId;

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
		public bool Delete(int ProspectOtherIncomeId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ProspectOtherIncome ");
			strSql.Append(" where ProspectOtherIncomeId=@ProspectOtherIncomeId");
			SqlParameter[] parameters = {
					new SqlParameter("@ProspectOtherIncomeId", SqlDbType.Int,4)
};
			parameters[0].Value = ProspectOtherIncomeId;

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
		public bool DeleteList(string ProspectOtherIncomeIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ProspectOtherIncome ");
			strSql.Append(" where ProspectOtherIncomeId in ("+ProspectOtherIncomeIdlist + ")  ");
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
		public LPWeb.Model.ProspectOtherIncome GetModel(int ProspectOtherIncomeId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ProspectOtherIncomeId,ContactId,Type,MonthlyIncome from ProspectOtherIncome ");
			strSql.Append(" where ProspectOtherIncomeId=@ProspectOtherIncomeId");
			SqlParameter[] parameters = {
					new SqlParameter("@ProspectOtherIncomeId", SqlDbType.Int,4)
};
			parameters[0].Value = ProspectOtherIncomeId;

			LPWeb.Model.ProspectOtherIncome model=new LPWeb.Model.ProspectOtherIncome();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["ProspectOtherIncomeId"]!=null && ds.Tables[0].Rows[0]["ProspectOtherIncomeId"].ToString()!="")
				{
					model.ProspectOtherIncomeId=int.Parse(ds.Tables[0].Rows[0]["ProspectOtherIncomeId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ContactId"]!=null && ds.Tables[0].Rows[0]["ContactId"].ToString()!="")
				{
					model.ContactId=int.Parse(ds.Tables[0].Rows[0]["ContactId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Type"]!=null && ds.Tables[0].Rows[0]["Type"].ToString()!="")
				{
					model.Type=ds.Tables[0].Rows[0]["Type"].ToString();
				}
				if(ds.Tables[0].Rows[0]["MonthlyIncome"]!=null && ds.Tables[0].Rows[0]["MonthlyIncome"].ToString()!="")
				{
					model.MonthlyIncome=decimal.Parse(ds.Tables[0].Rows[0]["MonthlyIncome"].ToString());
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
			strSql.Append("select ProspectOtherIncomeId,ContactId,Type,MonthlyIncome ");
			strSql.Append(" FROM ProspectOtherIncome ");
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
			strSql.Append(" ProspectOtherIncomeId,ContactId,Type,MonthlyIncome ");
			strSql.Append(" FROM ProspectOtherIncome ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/*
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
			parameters[0].Value = "ProspectOtherIncome";
			parameters[1].Value = "ProspectOtherIncomeId";
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

