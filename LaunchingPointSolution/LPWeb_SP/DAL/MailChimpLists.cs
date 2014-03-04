using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类:MailChimpLists
	/// </summary>
	public partial class MailChimpLists 
	{
		public MailChimpLists()
		{}
		#region  Method

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string LID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from MailChimpLists");
			strSql.Append(" where LID=@LID ");
			SqlParameter[] parameters = {
					new SqlParameter("@LID", SqlDbType.NVarChar,255)};
			parameters[0].Value = LID;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(LPWeb.Model.MailChimpLists model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into MailChimpLists(");
			strSql.Append("LID,Name,BranchId)");
			strSql.Append(" values (");
			strSql.Append("@LID,@Name,@BranchId)");
			SqlParameter[] parameters = {
					new SqlParameter("@LID", SqlDbType.NVarChar,255),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@BranchId", SqlDbType.NVarChar,255)};
			parameters[0].Value = model.LID;
			parameters[1].Value = model.Name;
			parameters[2].Value = model.BranchId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LPWeb.Model.MailChimpLists model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update MailChimpLists set ");
			strSql.Append("Name=@Name,");
			strSql.Append("BranchId=@BranchId");
			strSql.Append(" where LID=@LID ");
			SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@BranchId", SqlDbType.NVarChar,255),
					new SqlParameter("@LID", SqlDbType.NVarChar,255)};
			parameters[0].Value = model.Name;
			parameters[1].Value = model.BranchId;
			parameters[2].Value = model.LID;

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
		public bool Delete(string LID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from MailChimpLists ");
			strSql.Append(" where LID=@LID ");
			SqlParameter[] parameters = {
					new SqlParameter("@LID", SqlDbType.NVarChar,255)};
			parameters[0].Value = LID;

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
		public bool DeleteList(string LIDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from MailChimpLists ");
			strSql.Append(" where LID in ("+LIDlist + ")  ");
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
		public LPWeb.Model.MailChimpLists GetModel(string LID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 LID,Name,BranchId from MailChimpLists ");
			strSql.Append(" where LID=@LID ");
			SqlParameter[] parameters = {
					new SqlParameter("@LID", SqlDbType.NVarChar,255)};
			parameters[0].Value = LID;

			LPWeb.Model.MailChimpLists model=new LPWeb.Model.MailChimpLists();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["LID"]!=null && ds.Tables[0].Rows[0]["LID"].ToString()!="")
				{
					model.LID=ds.Tables[0].Rows[0]["LID"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Name"]!=null && ds.Tables[0].Rows[0]["Name"].ToString()!="")
				{
					model.Name=ds.Tables[0].Rows[0]["Name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["BranchId"]!=null && ds.Tables[0].Rows[0]["BranchId"].ToString()!="")
				{
					model.BranchId=ds.Tables[0].Rows[0]["BranchId"].ToString();
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
			strSql.Append("select LID,Name,BranchId ");
			strSql.Append(" FROM MailChimpLists ");
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
			strSql.Append(" LID,Name,BranchId ");
			strSql.Append(" FROM MailChimpLists ");
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
			parameters[0].Value = "MailChimpLists";
			parameters[1].Value = "LID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		} 

		#endregion  Method

        public DataSet GetMailChimpLists(int PageSize, int PageIndex, int UserID, string strWhere, out int count, string orderName, int orderType)
        {
            //string tempTable = string.Format("(SELECT ML.[LID],ML.[Name] As List,ML.[BranchId],B.Name AS Branch FROM [MailChimpLists] ML INNER JOIN [Branches] B ON ML.BranchId = B.BranchId INNER JOIN [dbo].[lpfn_GetUserBranches]({0}) UB ON ML.BranchId = UB.BranchID) as mcl", UserID.ToString());
            string tempTable = string.Format("(SELECT ML.[LID],ML.[Name] As List,ML.[BranchId],B.Name AS Branch,dbo.lpfn_GetUserName(ML.UserId) AS UserName,ML.UserId FROM [MailChimpLists] ML INNER JOIN [Branches] B ON ML.BranchId = B.BranchId INNER JOIN [dbo].[lpfn_GetUserBranches]({0}) UB ON ML.BranchId = UB.BranchID) as mcl", UserID.ToString());

            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 8000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = tempTable;
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrderBy"></param>
        /// <returns></returns>
        public DataTable GetMailChimpList(string sWhere, string sOrderBy)
        {
            string sSql0 = "select * from MailChimpLists where 1=1 " + sWhere + " order by " + sOrderBy;
            DataTable WorkflowList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
            return WorkflowList;
        }

        public DataTable GetMailChimpList_BranchManager(int userid)
        {
            int branchid = 0;
            string sSql0 = "";
            string sSql = "select BranchId from BranchManagers where BranchMgrId=" + userid;
            object obj = DAL.DbHelperSQL.GetSingle(sSql);

            if (obj != null)
            {
                branchid = (int)obj;
            }

            if (branchid > 0)
            {
                sSql0 = "select * from MailChimpLists where (UserId=" + userid + " or BranchId=" + branchid.ToString() + ") order by Name";
            }
            else
            {
                sSql0 = "select * from MailChimpLists where UserId=" + userid + " order by Name";
            }
            DataTable WorkflowList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
            return WorkflowList;
        }
    }
}


