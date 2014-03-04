using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����PointFolders��
	/// </summary>
	public class PointFoldersBase
    {
        public PointFoldersBase()
        { }
        #region  ��Ա����

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(LPWeb.Model.PointFolders model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PointFolders(");
            strSql.Append("Name,BranchId,Path,Enabled,ImportCount,LastImport,LoanStatus)");
            strSql.Append(" values (");
            strSql.Append("@Name,@BranchId,@Path,@Enabled,@ImportCount,@LastImport,@LoanStatus)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@BranchId", SqlDbType.Int,4),
					new SqlParameter("@Path", SqlDbType.NVarChar,255),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@ImportCount", SqlDbType.SmallInt,2),
					new SqlParameter("@LastImport", SqlDbType.DateTime),
					new SqlParameter("@LoanStatus", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.BranchId;
            parameters[2].Value = model.Path;
            parameters[3].Value = model.Enabled;
            parameters[4].Value = model.ImportCount;
            parameters[5].Value = model.LastImport;
            parameters[6].Value = model.LoanStatus;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
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
        public void Update(LPWeb.Model.PointFolders model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update PointFolders set ");
            strSql.Append("FolderId=@FolderId,");
            strSql.Append("Name=@Name,");
            strSql.Append("BranchId=@BranchId,");
            strSql.Append("Path=@Path,");
            strSql.Append("Enabled=@Enabled,");
            strSql.Append("ImportCount=@ImportCount,");
            strSql.Append("LastImport=@LastImport,");
            strSql.Append("LoanStatus=@LoanStatus");
            strSql.Append(" where FolderId=@FolderId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FolderId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@BranchId", SqlDbType.Int,4),
					new SqlParameter("@Path", SqlDbType.NVarChar,255),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@ImportCount", SqlDbType.SmallInt,2),
					new SqlParameter("@LastImport", SqlDbType.DateTime),
					new SqlParameter("@LoanStatus", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.FolderId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.BranchId;
            parameters[3].Value = model.Path;
            parameters[4].Value = model.Enabled;
            parameters[5].Value = model.ImportCount;
            parameters[6].Value = model.LastImport;
            parameters[7].Value = model.LoanStatus;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int FolderId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from PointFolders ");
            strSql.Append(" where FolderId=@FolderId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FolderId", SqlDbType.Int,4)};
            parameters[0].Value = FolderId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.PointFolders GetModel(int FolderId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 PointFolders.FolderId,PointFolders.Name,PointFolders.BranchId,PointFolders.Path,PointFolders.Enabled,PointFolders.ImportCount,PointFolders.LastImport,PointFolders.LoanStatus, Branches.Name AS BranchName from PointFolders LEFT OUTER JOIN Branches ON PointFolders.BranchId = Branches.BranchId ");
            strSql.Append(" where PointFolders.FolderId=@FolderId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FolderId", SqlDbType.Int,4)};
            parameters[0].Value = FolderId;

            LPWeb.Model.PointFolders model = new LPWeb.Model.PointFolders();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["FolderId"].ToString() != "")
                {
                    model.FolderId = int.Parse(ds.Tables[0].Rows[0]["FolderId"].ToString());
                }
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                if (ds.Tables[0].Rows[0]["BranchId"].ToString() != "")
                {
                    model.BranchId = int.Parse(ds.Tables[0].Rows[0]["BranchId"].ToString());
                }
                model.BranchName = ds.Tables[0].Rows[0]["BranchName"].ToString();
                model.Path = ds.Tables[0].Rows[0]["Path"].ToString();
                if (ds.Tables[0].Rows[0]["Enabled"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Enabled"].ToString() == "1") || (ds.Tables[0].Rows[0]["Enabled"].ToString().ToLower() == "true"))
                    {
                        model.Enabled = true;
                    }
                    else
                    {
                        model.Enabled = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["ImportCount"].ToString() != "")
                {
                    model.ImportCount = int.Parse(ds.Tables[0].Rows[0]["ImportCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LastImport"].ToString() != "")
                {
                    model.LastImport = DateTime.Parse(ds.Tables[0].Rows[0]["LastImport"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LoanStatus"].ToString() != "")
                {
                    model.LoanStatus = int.Parse(ds.Tables[0].Rows[0]["LoanStatus"].ToString());
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select FolderId,Name,BranchId,Path,Enabled,ImportCount,LastImport,LoanStatus,[Default],AutoNaming,FilenamePrefix, ");
            strSql.Append("CASE LoanStatus WHEN '0' THEN '' WHEN '1' THEN 'Active Loans' WHEN '2' THEN 'Canceled' ");
            strSql.Append("WHEN '3' THEN 'Closed' WHEN '4' THEN 'Denied' WHEN '5' THEN 'Suspended' ");
            strSql.Append("WHEN '6' THEN 'Active Leads' WHEN '7' THEN 'Archive Loans' WHEN '8' THEN 'Archive Leads' END AS StatusName ");
            strSql.Append(" FROM PointFolders ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// ���ǰ��������
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" FolderId,Name,BranchId,Path,Enabled,ImportCount,LastImport,LoanStatus ");
            strSql.Append(" FROM PointFolders ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        /// ��ҳ��ȡ�����б�
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere)
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
            parameters[0].Value = "PointFolders";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage", parameters, "ds");
        }

        #endregion  ��Ա����
	}
}

