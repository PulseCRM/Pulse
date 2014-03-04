using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����LoanRules��
	/// </summary>
	public class LoanRulesBase
    {
        public LoanRulesBase()
        { }
        #region  ��Ա����

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(LPWeb.Model.LoanRules model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LoanRules(");
            strSql.Append("Fileid,RuleGroupId,RuleId,Applied,AppliedBy)");
            strSql.Append(" values (");
            strSql.Append("@Fileid,@RuleGroupId,@RuleId,@Applied,@AppliedBy)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Fileid", SqlDbType.Int,4),
					new SqlParameter("@RuleGroupId", SqlDbType.Int,4),
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@Applied", SqlDbType.DateTime),
					new SqlParameter("@AppliedBy", SqlDbType.Int,4)};
            parameters[0].Value = model.Fileid;
            parameters[1].Value = model.RuleGroupId;
            parameters[2].Value = model.RuleId;
            parameters[3].Value = model.Applied;
            parameters[4].Value = model.AppliedBy;

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
        public void Update(LPWeb.Model.LoanRules model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update LoanRules set ");
            strSql.Append("LoanRuleId=@LoanRuleId,");
            strSql.Append("Fileid=@Fileid,");
            strSql.Append("RuleGroupId=@RuleGroupId,");
            strSql.Append("RuleId=@RuleId,");
            strSql.Append("Applied=@Applied,");
            strSql.Append("AppliedBy=@AppliedBy");
            strSql.Append(" where LoanRuleId=@LoanRuleId ");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanRuleId", SqlDbType.Int,4),
					new SqlParameter("@Fileid", SqlDbType.Int,4),
					new SqlParameter("@RuleGroupId", SqlDbType.Int,4),
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@Applied", SqlDbType.DateTime),
					new SqlParameter("@AppliedBy", SqlDbType.Int,4)};
            parameters[0].Value = model.LoanRuleId;
            parameters[1].Value = model.Fileid;
            parameters[2].Value = model.RuleGroupId;
            parameters[3].Value = model.RuleId;
            parameters[4].Value = model.Applied;
            parameters[5].Value = model.AppliedBy;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int LoanRuleId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LoanRules ");
            strSql.Append(" where LoanRuleId=@LoanRuleId ");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanRuleId", SqlDbType.Int,4)};
            parameters[0].Value = LoanRuleId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.LoanRules GetModel(int LoanRuleId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 LoanRuleId,Fileid,RuleGroupId,RuleId,Applied,AppliedBy from LoanRules ");
            strSql.Append(" where LoanRuleId=@LoanRuleId ");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanRuleId", SqlDbType.Int,4)};
            parameters[0].Value = LoanRuleId;

            LPWeb.Model.LoanRules model = new LPWeb.Model.LoanRules();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["LoanRuleId"].ToString() != "")
                {
                    model.LoanRuleId = int.Parse(ds.Tables[0].Rows[0]["LoanRuleId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Fileid"].ToString() != "")
                {
                    model.Fileid = int.Parse(ds.Tables[0].Rows[0]["Fileid"].ToString());
                }
                if (ds.Tables[0].Rows[0]["RuleGroupId"].ToString() != "")
                {
                    model.RuleGroupId = int.Parse(ds.Tables[0].Rows[0]["RuleGroupId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["RuleId"].ToString() != "")
                {
                    model.RuleId = int.Parse(ds.Tables[0].Rows[0]["RuleId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Applied"].ToString() != "")
                {
                    model.Applied = DateTime.Parse(ds.Tables[0].Rows[0]["Applied"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AppliedBy"].ToString() != "")
                {
                    model.AppliedBy = int.Parse(ds.Tables[0].Rows[0]["AppliedBy"].ToString());
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
            strSql.Append("select LoanRuleId,Fileid,RuleGroupId,RuleId,Applied,AppliedBy ");
            strSql.Append(" FROM LoanRules ");
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
            strSql.Append(" LoanRuleId,Fileid,RuleGroupId,RuleId,Applied,AppliedBy ");
            strSql.Append(" FROM LoanRules ");
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
            parameters[0].Value = "LoanRules";
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

