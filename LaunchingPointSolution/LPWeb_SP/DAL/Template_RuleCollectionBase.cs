using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����Template_RuleCollection��
	/// </summary>
    public class Template_RuleCollectionBase
    {
        public Template_RuleCollectionBase()
        { }
        #region  ��Ա����

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(LPWeb.Model.Template_RuleCollection model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Template_RuleCollection(");
            strSql.Append("Name,Enabled)");
            strSql.Append(" values (");
            strSql.Append("@Name,@Enabled)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.Enabled;

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
        public void Update(LPWeb.Model.Template_RuleCollection model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_RuleCollection set ");
            strSql.Append("RuleCollectionlId=@RuleCollectionlId,");
            strSql.Append("Name=@Name,");
            strSql.Append("Enabled=@Enabled");
            strSql.Append(" where RuleCollectionlId=@RuleCollectionlId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleCollectionlId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Enabled", SqlDbType.Bit,1)};
            parameters[0].Value = model.RuleCollectionlId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Enabled;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int RuleCollectionlId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Template_RuleCollection ");
            strSql.Append(" where RuleCollectionlId=@RuleCollectionlId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleCollectionlId", SqlDbType.Int,4)};
            parameters[0].Value = RuleCollectionlId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.Template_RuleCollection GetModel(int RuleCollectionlId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 RuleCollectionlId,Name,Enabled from Template_RuleCollection ");
            strSql.Append(" where RuleCollectionlId=@RuleCollectionlId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleCollectionlId", SqlDbType.Int,4)};
            parameters[0].Value = RuleCollectionlId;

            LPWeb.Model.Template_RuleCollection model = new LPWeb.Model.Template_RuleCollection();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["RuleCollectionlId"].ToString() != "")
                {
                    model.RuleCollectionlId = int.Parse(ds.Tables[0].Rows[0]["RuleCollectionlId"].ToString());
                }
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
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
            strSql.Append("select RuleCollectionlId,Name,Enabled ");
            strSql.Append(" FROM Template_RuleCollection ");
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
            strSql.Append(" RuleCollectionlId,Name,Enabled ");
            strSql.Append(" FROM Template_RuleCollection ");
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
            parameters[0].Value = "Template_RuleCollection";
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
