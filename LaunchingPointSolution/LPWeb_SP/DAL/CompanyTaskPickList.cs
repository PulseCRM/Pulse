using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类:ArchiveLeadStatus
	/// </summary>
	public class CompanyTaskPickList
	{
        public CompanyTaskPickList()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
        //public int GetMaxId()
        //{
        //    return DbHelperSQL.GetMaxID("TaskNameID", "LeadTaskList"); 
        //}

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxSequenceNumber()
        {
            return GetSequenceNumber("SequenceNumber", "LeadTaskList");
        }

        public static int GetSequenceNumber(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ") from " + TableName;
            object obj = DbHelperSQL.GetSingle(strsql);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string name)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select count(1) from LeadTaskList");
            strSql.Append(" where TaskName=@name ");
			SqlParameter[] parameters = {
					new SqlParameter("@name", SqlDbType.NVarChar,255)};
            parameters[0].Value = name;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsSequenceNumber(int SeqNumber)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from LeadTaskList");
            strSql.Append(" where SequenceNumber=@SeqNumber");
            SqlParameter[] parameters = {
					new SqlParameter("@SeqNumber", SqlDbType.Int)};
            parameters[0].Value = SeqNumber;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LPWeb.Model.CompanyTaskPick model)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("insert into LeadTaskList(");
            strSql.Append("TaskName,Enabled,SequenceNumber)");
			strSql.Append(" values (");
            strSql.Append("@TaskName,@Enabled,@SequenceNumber)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@TaskName", SqlDbType.NVarChar,255),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
                    new SqlParameter("@SequenceNumber", SqlDbType.Int)};
			parameters[0].Value = model.TaskName;
			parameters[1].Value = model.Enabled;
            parameters[2].Value = model.SequenceNumber;

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
        public bool Update(LPWeb.Model.CompanyTaskPick model)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update LeadTaskList set ");           
            strSql.Append("Enabled=@Enabled,");
            strSql.Append("SequenceNumber=@SequenceNumber");
            strSql.Append(" where TaskName=@TaskName");
            SqlParameter[] parameters = {					
					new SqlParameter("@TaskName", SqlDbType.NVarChar,255),
                    new SqlParameter("@SequenceNumber", SqlDbType.NVarChar,255),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
          
            parameters[0].Value = model.TaskName;
            parameters[1].Value = model.SequenceNumber;
            parameters[2].Value = model.Enabled;

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
		/// 删除一条数据
		/// </summary>
		public bool Delete(string name)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("delete from LeadTaskList");
            strSql.Append(" where TaskName=@name");
			SqlParameter[] parameters = {
					new SqlParameter("@name", SqlDbType.NVarChar,255)
};
            parameters[0].Value = name;

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
		public bool DeleteList(string LeadStatusIdlist )
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("delete from LeadTaskList ");
            strSql.Append(" where TaskName in (" + LeadStatusIdlist + ")  ");
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
        public LPWeb.Model.CompanyTaskPick GetModel(string name)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 TaskName,Enabled,SequenceNumber from LeadTaskList ");
            strSql.Append(" where TaskName=@name");
			SqlParameter[] parameters = {
					new SqlParameter("@name", SqlDbType.NVarChar,255)
};
            parameters[0].Value = name;

            LPWeb.Model.CompanyTaskPick model = new LPWeb.Model.CompanyTaskPick();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
               
                model.TaskName = ds.Tables[0].Rows[0]["TaskName"].ToString();
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

                model.SequenceNumber = int.Parse(ds.Tables[0].Rows[0]["SequenceNumber"].ToString());
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
            strSql.Append("select TaskName,Enabled,SequenceNumber ");
            strSql.Append(" FROM LeadTaskList ");
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
            strSql.Append(" TaskName,Enabled,SequenceNumber ");
            strSql.Append(" FROM LeadTaskList ");
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
			parameters[0].Value = "ArchiveLeadStatus";
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

