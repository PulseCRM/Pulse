using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
	/// 数据访问类LoanStages。
	/// </summary>
	public class LoanStagesBase
	{
        public LoanStagesBase()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int LoanStageId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from LoanStages");
			strSql.Append(" where LoanStageId=@LoanStageId ");
			SqlParameter[] parameters = {
					new SqlParameter("@LoanStageId", SqlDbType.Int,4)};
			parameters[0].Value = LoanStageId;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LPWeb.Model.LoanStages model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into LoanStages(");
			strSql.Append("Completed,SequenceNumber,FileId,DaysFromEstClose,StageName,WflTemplId,WflStageId)");
			strSql.Append(" values (");
			strSql.Append("@Completed,@SequenceNumber,@FileId,@DaysFromEstClose,@StageName,@WflTemplId,@WflStageId)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Completed", SqlDbType.DateTime),
					new SqlParameter("@SequenceNumber", SqlDbType.SmallInt,2),
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@DaysFromEstClose", SqlDbType.SmallInt,2),
					new SqlParameter("@StageName", SqlDbType.NVarChar,120),
					new SqlParameter("@WflTemplId", SqlDbType.Int,4),
					new SqlParameter("@WflStageId", SqlDbType.Int,4)};
			parameters[0].Value = model.Completed;
			parameters[1].Value = model.SequenceNumber;
			parameters[2].Value = model.FileId;
			parameters[3].Value = model.DaysFromEstClose;
			parameters[4].Value = model.StageName;
			parameters[5].Value = model.WflTemplId;
			parameters[6].Value = model.WflStageId;

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
		/// 更新一条数据
		/// </summary>
		public void Update(LPWeb.Model.LoanStages model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update LoanStages set ");
			strSql.Append("Completed=@Completed,");
			strSql.Append("SequenceNumber=@SequenceNumber,");
			strSql.Append("FileId=@FileId,");
			strSql.Append("DaysFromEstClose=@DaysFromEstClose,");
			strSql.Append("StageName=@StageName,");
			strSql.Append("WflTemplId=@WflTemplId,");
			strSql.Append("WflStageId=@WflStageId");
			strSql.Append(" where LoanStageId=@LoanStageId ");
			SqlParameter[] parameters = {
					new SqlParameter("@LoanStageId", SqlDbType.Int,4),
					new SqlParameter("@Completed", SqlDbType.DateTime),
					new SqlParameter("@SequenceNumber", SqlDbType.SmallInt,2),
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@DaysFromEstClose", SqlDbType.SmallInt,2),
					new SqlParameter("@StageName", SqlDbType.NVarChar,120),
					new SqlParameter("@WflTemplId", SqlDbType.Int,4),
					new SqlParameter("@WflStageId", SqlDbType.Int,4)};
			parameters[0].Value = model.LoanStageId;
			parameters[1].Value = model.Completed;
			parameters[2].Value = model.SequenceNumber;
			parameters[3].Value = model.FileId;
			parameters[4].Value = model.DaysFromEstClose;
			parameters[5].Value = model.StageName;
			parameters[6].Value = model.WflTemplId;
			parameters[7].Value = model.WflStageId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int LoanStageId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from LoanStages ");
			strSql.Append(" where LoanStageId=@LoanStageId ");
			SqlParameter[] parameters = {
					new SqlParameter("@LoanStageId", SqlDbType.Int,4)};
			parameters[0].Value = LoanStageId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.LoanStages GetModel(int LoanStageId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 LoanStageId,Completed,SequenceNumber,FileId,DaysFromEstClose,StageName,WflTemplId,WflStageId from LoanStages ");
			strSql.Append(" where LoanStageId=@LoanStageId ");
			SqlParameter[] parameters = {
					new SqlParameter("@LoanStageId", SqlDbType.Int,4)};
			parameters[0].Value = LoanStageId;

			LPWeb.Model.LoanStages model=new LPWeb.Model.LoanStages();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["LoanStageId"].ToString()!="")
				{
					model.LoanStageId=int.Parse(ds.Tables[0].Rows[0]["LoanStageId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Completed"].ToString()!="")
				{
					model.Completed=DateTime.Parse(ds.Tables[0].Rows[0]["Completed"].ToString());
				}
				if(ds.Tables[0].Rows[0]["SequenceNumber"].ToString()!="")
				{
					model.SequenceNumber=int.Parse(ds.Tables[0].Rows[0]["SequenceNumber"].ToString());
				}
				if(ds.Tables[0].Rows[0]["FileId"].ToString()!="")
				{
					model.FileId=int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["DaysFromEstClose"].ToString()!="")
				{
					model.DaysFromEstClose=int.Parse(ds.Tables[0].Rows[0]["DaysFromEstClose"].ToString());
				}
				model.StageName=ds.Tables[0].Rows[0]["StageName"].ToString();
				if(ds.Tables[0].Rows[0]["WflTemplId"].ToString()!="")
				{
					model.WflTemplId=int.Parse(ds.Tables[0].Rows[0]["WflTemplId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["WflStageId"].ToString()!="")
				{
					model.WflStageId=int.Parse(ds.Tables[0].Rows[0]["WflStageId"].ToString());
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
			strSql.Append("select LoanStageId,Completed,SequenceNumber,FileId,DaysFromEstClose,StageName,WflTemplId,WflStageId ");
			strSql.Append(" FROM LoanStages ");
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
			strSql.Append(" LoanStageId,Completed,SequenceNumber,FileId,DaysFromEstClose,StageName,WflTemplId,WflStageId ");
			strSql.Append(" FROM LoanStages ");
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
			parameters[0].Value = "LoanStages";
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

