using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����Template_Workflow��
	/// </summary>
	public class Template_WorkflowBase
    {
        public Template_WorkflowBase()
        { }

        #region  ��Ա����

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(LPWeb.Model.Template_Workflow model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Template_Workflow(");
            strSql.Append("Name,Enabled,[Desc])");
            strSql.Append(" values (");
            strSql.Append("@Name,@Enabled,@Desc)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.Enabled;
            parameters[2].Value = model.Desc;

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
        public void Update(LPWeb.Model.Template_Workflow model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_Workflow set ");
            strSql.Append("WflTemplId=@WflTemplId,");
            strSql.Append("Name=@Name,");
            strSql.Append("Enabled=@Enabled,");
            strSql.Append("Desc=@Desc");
            strSql.Append(" where WflTemplId=@WflTemplId ");
            SqlParameter[] parameters = {
					new SqlParameter("@WflTemplId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.WflTemplId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Enabled;
            parameters[3].Value = model.Desc;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int WflTemplId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Template_Workflow ");
            strSql.Append(" where WflTemplId=@WflTemplId ");
            SqlParameter[] parameters = {
					new SqlParameter("@WflTemplId", SqlDbType.Int,4)};
            parameters[0].Value = WflTemplId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.Template_Workflow GetModel(int WflTemplId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WflTemplId,Name,Enabled,[Desc],[Default],WorkflowType,Custom,CalculationMethod from Template_Workflow ");
            strSql.Append(" where WflTemplId=@WflTemplId ");
            SqlParameter[] parameters = {
					new SqlParameter("@WflTemplId", SqlDbType.Int,4)};
            parameters[0].Value = WflTemplId;

            LPWeb.Model.Template_Workflow model = new LPWeb.Model.Template_Workflow();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["WflTemplId"].ToString() != "")
                {
                    model.WflTemplId = int.Parse(ds.Tables[0].Rows[0]["WflTemplId"].ToString());
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
                if (ds.Tables[0].Rows[0]["Default"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Default"].ToString() == "1") || (ds.Tables[0].Rows[0]["Default"].ToString().ToLower() == "true"))
                    {
                        model.Default = true;
                    }
                    else
                    {
                        model.Default = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Custom"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Custom"].ToString() == "1") || (ds.Tables[0].Rows[0]["Custom"].ToString().ToLower() == "true"))
                    {
                        model.Custom = true;
                    }
                    else
                    {
                        model.Custom = false;
                    }
                }
                model.WorkflowType = ds.Tables[0].Rows[0]["WorkflowType"].ToString();
                model.Desc = ds.Tables[0].Rows[0]["Desc"].ToString();
                if (ds.Tables[0].Rows[0]["CalculationMethod"].ToString() != "")
                {
                    model.CalculationMethod = int.Parse(ds.Tables[0].Rows[0]["CalculationMethod"].ToString());
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
            strSql.Append("select WflTemplId,Name,[Enabled],[Desc],[Default],WorkflowType,Custom,CalculationMethod ");
            strSql.Append(" FROM Template_Workflow ");
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
            strSql.Append(" WflTemplId,Name,[Enabled],[Desc],[Default],WorkflowType,Custom,CalculationMethod ");
            strSql.Append(" FROM Template_Workflow ");
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
            parameters[0].Value = "Template_Workflow";
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
