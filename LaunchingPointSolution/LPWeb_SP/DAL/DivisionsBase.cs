using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����Divisions��
	/// </summary>
	public class DivisionsBase
    {
        public DivisionsBase()
		{}
        #region  ��Ա����

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(LPWeb.Model.Divisions model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Divisions(");
            strSql.Append("Name,[Desc],Enabled,RegionID,GroupID)");
            strSql.Append(" values (");
            strSql.Append("@Name,@Desc,@Enabled,@RegionID,@GroupID)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@RegionID", SqlDbType.Int,4),
					new SqlParameter("@GroupID", SqlDbType.Int,4)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.Desc;
            parameters[2].Value = model.Enabled;
            parameters[3].Value = model.RegionID;
            parameters[4].Value = model.GroupID;

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
        public void Update(LPWeb.Model.Divisions model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Divisions set ");
            strSql.Append("DivisionId=@DivisionId,");
            strSql.Append("Name=@Name,");
            strSql.Append("[Desc]=@Desc,");
            strSql.Append("Enabled=@Enabled,");
            strSql.Append("RegionID=@RegionID,");
            strSql.Append("GroupID=@GroupID");
            strSql.Append(" where DivisionId=@DivisionId ");
            SqlParameter[] parameters = {
					new SqlParameter("@DivisionId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@RegionID", SqlDbType.Int,4),
					new SqlParameter("@GroupID", SqlDbType.Int,4)};
            parameters[0].Value = model.DivisionId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Desc;
            parameters[3].Value = model.Enabled;
            parameters[4].Value = model.RegionID;
            parameters[5].Value = model.GroupID;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int DivisionId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Divisions ");
            strSql.Append(" where DivisionId=@DivisionId ");
            SqlParameter[] parameters = {
					new SqlParameter("@DivisionId", SqlDbType.Int,4)};
            parameters[0].Value = DivisionId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.Divisions GetModel(int DivisionId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 DivisionId,Name,[Desc],Enabled,RegionID,GroupID from Divisions ");
            strSql.Append(" where DivisionId=@DivisionId ");
            SqlParameter[] parameters = {
					new SqlParameter("@DivisionId", SqlDbType.Int,4)};
            parameters[0].Value = DivisionId;

            LPWeb.Model.Divisions model = new LPWeb.Model.Divisions();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["DivisionId"].ToString() != "")
                {
                    model.DivisionId = int.Parse(ds.Tables[0].Rows[0]["DivisionId"].ToString());
                }
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                model.Desc = ds.Tables[0].Rows[0]["Desc"].ToString();
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
                if (ds.Tables[0].Rows[0]["RegionID"].ToString() != "")
                {
                    model.RegionID = int.Parse(ds.Tables[0].Rows[0]["RegionID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["GroupID"].ToString() != "")
                {
                    model.GroupID = int.Parse(ds.Tables[0].Rows[0]["GroupID"].ToString());
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
            strSql.Append("select DivisionId,[Name],[Desc],Enabled,RegionID,GroupID ");
            strSql.Append(" FROM Divisions ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by [Name] ");
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
            strSql.Append(" DivisionId,Name,[Desc],Enabled,RegionID,GroupID ");
            strSql.Append(" FROM Divisions ");
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
            parameters[0].Value = "Divisions";
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

