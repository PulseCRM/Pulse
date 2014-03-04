using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Template_Wfl_Task_Alerts。
	/// </summary>
	public class Template_Wfl_Task_AlertsBase
    {
        public Template_Wfl_Task_AlertsBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Template_Wfl_Task_Alerts model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Template_Wfl_Task_Alerts(");
            strSql.Append("TemplTaskId,SuccessTemplateId,WarningTemplateId,OverdueTemplateId,ToContactType)");
            strSql.Append(" values (");
            strSql.Append("@TemplTaskId,@SuccessTemplateId,@WarningTemplateId,@OverdueTemplateId,@ToContactType)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplTaskId", SqlDbType.Int,4),
					new SqlParameter("@SuccessTemplateId", SqlDbType.Int,4),
					new SqlParameter("@WarningTemplateId", SqlDbType.Int,4),
					new SqlParameter("@OverdueTemplateId", SqlDbType.Int,4),
					new SqlParameter("@ToContactType", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.TemplTaskId;
            parameters[1].Value = model.SuccessTemplateId;
            parameters[2].Value = model.WarningTemplateId;
            parameters[3].Value = model.OverdueTemplateId;
            parameters[4].Value = model.ToContactType;

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
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Template_Wfl_Task_Alerts model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_Wfl_Task_Alerts set ");
            strSql.Append("TemplAlertId=@TemplAlertId,");
            strSql.Append("TemplTaskId=@TemplTaskId,");
            strSql.Append("SuccessTemplateId=@SuccessTemplateId,");
            strSql.Append("WarningTemplateId=@WarningTemplateId,");
            strSql.Append("OverdueTemplateId=@OverdueTemplateId,");
            strSql.Append("ToContactType=@ToContactType");
            strSql.Append(" where TemplAlertId=@TemplAlertId ");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplAlertId", SqlDbType.Int,4),
					new SqlParameter("@TemplTaskId", SqlDbType.Int,4),
					new SqlParameter("@SuccessTemplateId", SqlDbType.Int,4),
					new SqlParameter("@WarningTemplateId", SqlDbType.Int,4),
					new SqlParameter("@OverdueTemplateId", SqlDbType.Int,4),
					new SqlParameter("@ToContactType", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.TemplAlertId;
            parameters[1].Value = model.TemplTaskId;
            parameters[2].Value = model.SuccessTemplateId;
            parameters[3].Value = model.WarningTemplateId;
            parameters[4].Value = model.OverdueTemplateId;
            parameters[5].Value = model.ToContactType;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int TemplAlertId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Template_Wfl_Task_Alerts ");
            strSql.Append(" where TemplAlertId=@TemplAlertId ");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplAlertId", SqlDbType.Int,4)};
            parameters[0].Value = TemplAlertId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Template_Wfl_Task_Alerts GetModel(int TemplAlertId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 TemplAlertId,TemplTaskId,SuccessTemplateId,WarningTemplateId,OverdueTemplateId,ToContactType from Template_Wfl_Task_Alerts ");
            strSql.Append(" where TemplAlertId=@TemplAlertId ");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplAlertId", SqlDbType.Int,4)};
            parameters[0].Value = TemplAlertId;

            LPWeb.Model.Template_Wfl_Task_Alerts model = new LPWeb.Model.Template_Wfl_Task_Alerts();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["TemplAlertId"].ToString() != "")
                {
                    model.TemplAlertId = int.Parse(ds.Tables[0].Rows[0]["TemplAlertId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TemplTaskId"].ToString() != "")
                {
                    model.TemplTaskId = int.Parse(ds.Tables[0].Rows[0]["TemplTaskId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SuccessTemplateId"].ToString() != "")
                {
                    model.SuccessTemplateId = int.Parse(ds.Tables[0].Rows[0]["SuccessTemplateId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["WarningTemplateId"].ToString() != "")
                {
                    model.WarningTemplateId = int.Parse(ds.Tables[0].Rows[0]["WarningTemplateId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["OverdueTemplateId"].ToString() != "")
                {
                    model.OverdueTemplateId = int.Parse(ds.Tables[0].Rows[0]["OverdueTemplateId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ToContactType"].ToString() != "")
                {
                    model.ToContactType = int.Parse(ds.Tables[0].Rows[0]["ToContactType"].ToString());
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TemplAlertId,TemplTaskId,SuccessTemplateId,WarningTemplateId,OverdueTemplateId,ToContactType ");
            strSql.Append(" FROM Template_Wfl_Task_Alerts ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" TemplAlertId,TemplTaskId,SuccessTemplateId,WarningTemplateId,OverdueTemplateId,ToContactType ");
            strSql.Append(" FROM Template_Wfl_Task_Alerts ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        /// 分页获取数据列表
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
            parameters[0].Value = "Template_Wfl_Task_Alerts";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage", parameters, "ds");
        }

        #endregion  成员方法
	}
}

