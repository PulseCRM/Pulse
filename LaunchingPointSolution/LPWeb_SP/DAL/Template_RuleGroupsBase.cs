using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����Template_RuleGroups��
	/// </summary>
	public class Template_RuleGroupsBase
	{
        public Template_RuleGroupsBase()
		{}
        #region  ��Ա����

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(LPWeb.Model.Template_RuleGroups model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Template_RuleGroups(");
            strSql.Append("Name,[Desc],[Enabled],RuleScope,LoanTarget)");
            strSql.Append(" values (");
            strSql.Append("@Name,@Desc,@Enabled,@RuleScope,@LoanTarget)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@RuleScope", SqlDbType.SmallInt,2),
					new SqlParameter("@LoanTarget", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.Desc;
            parameters[2].Value = model.Enabled;
            parameters[3].Value = model.RuleScope;
            parameters[4].Value = model.LoanTarget;

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
        public void Update(LPWeb.Model.Template_RuleGroups model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_RuleGroups set ");
            strSql.Append("RuleGroupId=@RuleGroupId,");
            strSql.Append("Name=@Name,");
            strSql.Append("[Desc]=@Desc,");
            strSql.Append("[Enabled]=@Enabled,");
            strSql.Append("RuleScope=@RuleScope,");
            strSql.Append("LoanTarget=@LoanTarget");
            strSql.Append(" where RuleGroupId=@RuleGroupId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleGroupId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@RuleScope", SqlDbType.SmallInt,2),
					new SqlParameter("@LoanTarget", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.RuleGroupId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Desc;
            parameters[3].Value = model.Enabled;
            parameters[4].Value = model.RuleScope;
            parameters[5].Value = model.LoanTarget;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int RuleGroupId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Template_RuleGroups ");
            strSql.Append(" where RuleGroupId=@RuleGroupId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleGroupId", SqlDbType.Int,4)};
            parameters[0].Value = RuleGroupId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.Template_RuleGroups GetModel(int RuleGroupId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 RuleGroupId,Name,[Desc],[Enabled],RuleScope,LoanTarget from Template_RuleGroups ");
            strSql.Append(" where RuleGroupId=@RuleGroupId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleGroupId", SqlDbType.Int,4)};
            parameters[0].Value = RuleGroupId;

            LPWeb.Model.Template_RuleGroups model = new LPWeb.Model.Template_RuleGroups();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["RuleGroupId"].ToString() != "")
                {
                    model.RuleGroupId = int.Parse(ds.Tables[0].Rows[0]["RuleGroupId"].ToString());
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
                if (ds.Tables[0].Rows[0]["RuleScope"].ToString() != "")
                {
                    model.RuleScope = int.Parse(ds.Tables[0].Rows[0]["RuleScope"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LoanTarget"].ToString() != "")
                {
                    model.LoanTarget = int.Parse(ds.Tables[0].Rows[0]["LoanTarget"].ToString());
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
            strSql.Append("select RuleGroupId,Name,[Desc],[Enabled],RuleScope,LoanTarget ");
            strSql.Append(" FROM Template_RuleGroups ");
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
            strSql.Append(" RuleGroupId,Name,[Desc],[Enabled],RuleScope,LoanTarget ");
            strSql.Append(" FROM Template_RuleGroups ");
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
            parameters[0].Value = "Template_RuleGroups";
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

