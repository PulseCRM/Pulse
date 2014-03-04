using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Template_Reports。
	/// </summary>
	public class Template_Reports
	{
		public Template_Reports()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("TemplReportId", "Template_Reports"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int TemplReportId)
		{
			int rowsAffected;
			SqlParameter[] parameters = {
					new SqlParameter("@TemplReportId", SqlDbType.Int,4)};
			parameters[0].Value = TemplReportId;

			int result= DbHelperSQL.RunProcedure("UP_Template_Reports_Exists",parameters,out rowsAffected);
			if(result==1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		///  增加一条数据
		/// </summary>
		public int Add(LPWeb.Model.Template_Reports model)
		{
			int rowsAffected;
			SqlParameter[] parameters = {
					new SqlParameter("@TemplReportId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@Standard", SqlDbType.Bit,1),
					new SqlParameter("@HtmlTemplContent", SqlDbType.VarChar)};
			parameters[0].Direction = ParameterDirection.Output;
			parameters[1].Value = model.Name;
			parameters[2].Value = model.Standard;
			parameters[3].Value = model.HtmlTemplContent;

			DbHelperSQL.RunProcedure("UP_Template_Reports_ADD",parameters,out rowsAffected);
			return (int)parameters[0].Value;
		}

		/// <summary>
		///  更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.Template_Reports model)
		{
			int rowsAffected;
			SqlParameter[] parameters = {
					new SqlParameter("@TemplReportId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@Standard", SqlDbType.Bit,1),
					new SqlParameter("@HtmlTemplContent", SqlDbType.VarChar)};
			parameters[0].Value = model.TemplReportId;
			parameters[1].Value = model.Name;
			parameters[2].Value = model.Standard;
			parameters[3].Value = model.HtmlTemplContent;

			DbHelperSQL.RunProcedure("UP_Template_Reports_Update",parameters,out rowsAffected);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int TemplReportId)
		{
			int rowsAffected;
			SqlParameter[] parameters = {
					new SqlParameter("@TemplReportId", SqlDbType.Int,4)};
			parameters[0].Value = TemplReportId;

			DbHelperSQL.RunProcedure("UP_Template_Reports_Delete",parameters,out rowsAffected);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.Template_Reports GetModel(int TemplReportId)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@TemplReportId", SqlDbType.Int,4)};
			parameters[0].Value = TemplReportId;

			LPWeb.Model.Template_Reports model=new LPWeb.Model.Template_Reports();
			DataSet ds= DbHelperSQL.RunProcedure("UP_Template_Reports_GetModel",parameters,"ds");
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["TemplReportId"].ToString()!="")
				{
					model.TemplReportId=int.Parse(ds.Tables[0].Rows[0]["TemplReportId"].ToString());
				}
				model.Name=ds.Tables[0].Rows[0]["Name"].ToString();
				if(ds.Tables[0].Rows[0]["Standard"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["Standard"].ToString()=="1")||(ds.Tables[0].Rows[0]["Standard"].ToString().ToLower()=="true"))
					{
						model.Standard=true;
					}
					else
					{
						model.Standard=false;
					}
				}
				model.HtmlTemplContent=ds.Tables[0].Rows[0]["HtmlTemplContent"].ToString();
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
			strSql.Append("select TemplReportId,Name,Standard,HtmlTemplContent ");
			strSql.Append(" FROM Template_Reports ");
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
			strSql.Append(" TemplReportId,Name,Standard,HtmlTemplContent ");
			strSql.Append(" FROM Template_Reports ");
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
			parameters[0].Value = "Template_Reports";
			parameters[1].Value = "ID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  成员方法
	}
}

