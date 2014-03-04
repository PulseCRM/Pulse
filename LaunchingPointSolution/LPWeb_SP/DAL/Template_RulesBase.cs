using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Template_Rules。
	/// </summary>
	public class Template_RulesBase
    {
        public Template_RulesBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Template_Rules model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Template_Rules(");
            strSql.Append("Name,desc,Enabled,AlertEmailTemplId,AckReq,RecomEmailTemplid,AdvFormula)");
            strSql.Append(" values (");
            strSql.Append("@Name,@desc,@Enabled,@AlertEmailTemplId,@AckReq,@RecomEmailTemplid,@AdvFormula)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@desc", SqlDbType.NVarChar,500),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@AlertEmailTemplId", SqlDbType.Int,4),
					new SqlParameter("@AckReq", SqlDbType.Bit,1),
					new SqlParameter("@RecomEmailTemplid", SqlDbType.Int,4),
					new SqlParameter("@AdvFormula", SqlDbType.NVarChar,200)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.desc;
            parameters[2].Value = model.Enabled;
            parameters[3].Value = model.AlertEmailTemplId;
            parameters[4].Value = model.AckReq;
            parameters[5].Value = model.RecomEmailTemplid;
            parameters[6].Value = model.AdvFormula;

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
        public void Update(LPWeb.Model.Template_Rules model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_Rules set ");
            strSql.Append("RuleId=@RuleId,");
            strSql.Append("Name=@Name,");
            strSql.Append("desc=@desc,");
            strSql.Append("Enabled=@Enabled,");
            strSql.Append("AlertEmailTemplId=@AlertEmailTemplId,");
            strSql.Append("AckReq=@AckReq,");
            strSql.Append("RecomEmailTemplid=@RecomEmailTemplid,");
            strSql.Append("AdvFormula=@AdvFormula");
            strSql.Append(" where RuleId=@RuleId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@desc", SqlDbType.NVarChar,500),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@AlertEmailTemplId", SqlDbType.Int,4),
					new SqlParameter("@AckReq", SqlDbType.Bit,1),
					new SqlParameter("@RecomEmailTemplid", SqlDbType.Int,4),
					new SqlParameter("@AdvFormula", SqlDbType.NVarChar,200)};
            parameters[0].Value = model.RuleId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.desc;
            parameters[3].Value = model.Enabled;
            parameters[4].Value = model.AlertEmailTemplId;
            parameters[5].Value = model.AckReq;
            parameters[6].Value = model.RecomEmailTemplid;
            parameters[7].Value = model.AdvFormula;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int RuleId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Template_Rules ");
            strSql.Append(" where RuleId=@RuleId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4)};
            parameters[0].Value = RuleId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Template_Rules GetModel(int RuleId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 RuleId,Name,desc,Enabled,AlertEmailTemplId,AckReq,RecomEmailTemplid,AdvFormula from Template_Rules ");
            strSql.Append(" where RuleId=@RuleId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4)};
            parameters[0].Value = RuleId;

            LPWeb.Model.Template_Rules model = new LPWeb.Model.Template_Rules();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["RuleId"].ToString() != "")
                {
                    model.RuleId = int.Parse(ds.Tables[0].Rows[0]["RuleId"].ToString());
                }
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                model.desc = ds.Tables[0].Rows[0]["desc"].ToString();
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
                if (ds.Tables[0].Rows[0]["AlertEmailTemplId"].ToString() != "")
                {
                    model.AlertEmailTemplId = int.Parse(ds.Tables[0].Rows[0]["AlertEmailTemplId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AckReq"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["AckReq"].ToString() == "1") || (ds.Tables[0].Rows[0]["AckReq"].ToString().ToLower() == "true"))
                    {
                        model.AckReq = true;
                    }
                    else
                    {
                        model.AckReq = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["RecomEmailTemplid"].ToString() != "")
                {
                    model.RecomEmailTemplid = int.Parse(ds.Tables[0].Rows[0]["RecomEmailTemplid"].ToString());
                }
                model.AdvFormula = ds.Tables[0].Rows[0]["AdvFormula"].ToString();
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
            strSql.Append("select RuleId,Name,desc,Enabled,AlertEmailTemplId,AckReq,RecomEmailTemplid,AdvFormula ");
            strSql.Append(" FROM Template_Rules ");
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
            strSql.Append(" RuleId,Name,desc,Enabled,AlertEmailTemplId,AckReq,RecomEmailTemplid,AdvFormula ");
            strSql.Append(" FROM Template_Rules ");
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
            parameters[0].Value = "Template_Rules";
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

