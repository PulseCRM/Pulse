using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����Template_RuleConditions��
	/// </summary>
	public class Template_RuleConditionsBase
    {
        public Template_RuleConditionsBase()
        { }

        #region  ��Ա����

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(LPWeb.Model.Template_RuleConditions model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Template_RuleConditions(");
            strSql.Append("RuleId,PointFieldId,Condition,Tolerance,ToleranceType)");
            strSql.Append(" values (");
            strSql.Append("@RuleId,@PointFieldId,@Condition,@Tolerance,@ToleranceType)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@PointFieldId", SqlDbType.Decimal,5),
					new SqlParameter("@Condition", SqlDbType.SmallInt,2),
					new SqlParameter("@Tolerance", SqlDbType.NVarChar,250),
					new SqlParameter("@ToleranceType", SqlDbType.NChar,10)};
            parameters[0].Value = model.RuleId;
            parameters[1].Value = model.PointFieldId;
            parameters[2].Value = model.Condition;
            parameters[3].Value = model.Tolerance;
            parameters[4].Value = model.ToleranceType;

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
        public void Update(LPWeb.Model.Template_RuleConditions model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_RuleConditions set ");
            strSql.Append("RuleCondId=@RuleCondId,");
            strSql.Append("RuleId=@RuleId,");
            strSql.Append("PointFieldId=@PointFieldId,");
            strSql.Append("Condition=@Condition,");
            strSql.Append("Tolerance=@Tolerance,");
            strSql.Append("ToleranceType=@ToleranceType");
            strSql.Append(" where RuleCondId=@RuleCondId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleCondId", SqlDbType.Int,4),
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@PointFieldId", SqlDbType.Decimal,5),
					new SqlParameter("@Condition", SqlDbType.SmallInt,2),
					new SqlParameter("@Tolerance", SqlDbType.NVarChar,250),
					new SqlParameter("@ToleranceType", SqlDbType.NChar,10)};
            parameters[0].Value = model.RuleCondId;
            parameters[1].Value = model.RuleId;
            parameters[2].Value = model.PointFieldId;
            parameters[3].Value = model.Condition;
            parameters[4].Value = model.Tolerance;
            parameters[5].Value = model.ToleranceType;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int RuleCondId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Template_RuleConditions ");
            strSql.Append(" where RuleCondId=@RuleCondId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleCondId", SqlDbType.Int,4)};
            parameters[0].Value = RuleCondId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.Template_RuleConditions GetModel(int RuleCondId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 RuleCondId,RuleId,PointFieldId,Condition,Tolerance,ToleranceType from Template_RuleConditions ");
            strSql.Append(" where RuleCondId=@RuleCondId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleCondId", SqlDbType.Int,4)};
            parameters[0].Value = RuleCondId;

            LPWeb.Model.Template_RuleConditions model = new LPWeb.Model.Template_RuleConditions();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["RuleCondId"].ToString() != "")
                {
                    model.RuleCondId = int.Parse(ds.Tables[0].Rows[0]["RuleCondId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["RuleId"].ToString() != "")
                {
                    model.RuleId = int.Parse(ds.Tables[0].Rows[0]["RuleId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PointFieldId"].ToString() != "")
                {
                    model.PointFieldId = decimal.Parse(ds.Tables[0].Rows[0]["PointFieldId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Condition"].ToString() != "")
                {
                    model.Condition = int.Parse(ds.Tables[0].Rows[0]["Condition"].ToString());
                }
                model.Tolerance = ds.Tables[0].Rows[0]["Tolerance"].ToString();
                model.ToleranceType = ds.Tables[0].Rows[0]["ToleranceType"].ToString();
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
            strSql.Append("select RuleCondId,RuleId,PointFieldId,Condition,Tolerance,ToleranceType ");
            strSql.Append(" FROM Template_RuleConditions ");
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
            strSql.Append(" RuleCondId,RuleId,PointFieldId,Condition,Tolerance,ToleranceType ");
            strSql.Append(" FROM Template_RuleConditions ");
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
            parameters[0].Value = "Template_RuleConditions";
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

