using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Template_Wfl_Stages。
    /// </summary>
    public class Template_Wfl_StagesBase
    {
        public Template_Wfl_StagesBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Template_Wfl_Stages model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Template_Wfl_Stages(");
            strSql.Append("WflTemplId,SequenceNumber,Enabled,DaysFromEstClose,Name)");
            strSql.Append(" values (");
            strSql.Append("@WflTemplId,@SequenceNumber,@Enabled,@DaysFromEstClose,@Name)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@WflTemplId", SqlDbType.Int,4),
					new SqlParameter("@SequenceNumber", SqlDbType.SmallInt,2),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@DaysFromEstClose", SqlDbType.SmallInt,2),
					new SqlParameter("@Name", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.WflTemplId;
            parameters[1].Value = model.SequenceNumber;
            parameters[2].Value = model.Enabled;
            parameters[3].Value = model.DaysFromEstClose;
            parameters[4].Value = model.Name;

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
        public void Update(LPWeb.Model.Template_Wfl_Stages model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_Wfl_Stages set ");
            strSql.Append("WflStageId=@WflStageId,");
            strSql.Append("WflTemplId=@WflTemplId,");
            strSql.Append("SequenceNumber=@SequenceNumber,");
            strSql.Append("Enabled=@Enabled,");
            strSql.Append("DaysFromEstClose=@DaysFromEstClose,");
            strSql.Append("Name=@Name");
            strSql.Append(" where WflStageId=@WflStageId ");
            SqlParameter[] parameters = {
					new SqlParameter("@WflStageId", SqlDbType.Int,4),
					new SqlParameter("@WflTemplId", SqlDbType.Int,4),
					new SqlParameter("@SequenceNumber", SqlDbType.SmallInt,2),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@DaysFromEstClose", SqlDbType.SmallInt,2),
					new SqlParameter("@Name", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.WflStageId;
            parameters[1].Value = model.WflTemplId;
            parameters[2].Value = model.SequenceNumber;
            parameters[3].Value = model.Enabled;
            parameters[4].Value = model.DaysFromEstClose;
            parameters[5].Value = model.Name;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int WflStageId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Template_Wfl_Stages ");
            strSql.Append(" where WflStageId=@WflStageId ");
            SqlParameter[] parameters = {
					new SqlParameter("@WflStageId", SqlDbType.Int,4)};
            parameters[0].Value = WflStageId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Template_Wfl_Stages GetModel(int WflStageId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WflStageId,WflTemplId,SequenceNumber,Enabled,DaysFromEstClose,Name,DaysFromCreation,TemplStageId,CalculationMethod from Template_Wfl_Stages ");
            strSql.Append(" where WflStageId=@WflStageId ");
            SqlParameter[] parameters = {
					new SqlParameter("@WflStageId", SqlDbType.Int,4)};
            parameters[0].Value = WflStageId;

            LPWeb.Model.Template_Wfl_Stages model = new LPWeb.Model.Template_Wfl_Stages();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["WflStageId"].ToString() != "")
                {
                    model.WflStageId = int.Parse(ds.Tables[0].Rows[0]["WflStageId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["WflTemplId"].ToString() != "")
                {
                    model.WflTemplId = int.Parse(ds.Tables[0].Rows[0]["WflTemplId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SequenceNumber"].ToString() != "")
                {
                    model.SequenceNumber = int.Parse(ds.Tables[0].Rows[0]["SequenceNumber"].ToString());
                }
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
                if (ds.Tables[0].Rows[0]["DaysFromEstClose"].ToString() != "")
                {
                    model.DaysFromEstClose = int.Parse(ds.Tables[0].Rows[0]["DaysFromEstClose"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DaysFromCreation"].ToString() != "")
                {
                    model.DaysFromCreation = int.Parse(ds.Tables[0].Rows[0]["DaysFromCreation"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TemplStageId"].ToString() != "")
                {
                    model.TemplStageId = int.Parse(ds.Tables[0].Rows[0]["TemplStageId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CalculationMethod"].ToString() != "")
                {
                    model.CalculationMethod = int.Parse(ds.Tables[0].Rows[0]["CalculationMethod"].ToString());
                }
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
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
            strSql.Append("select WflStageId,WflTemplId,SequenceNumber,Enabled,DaysFromEstClose,Name,DaysFromCreation,TemplStageId,CalculationMethod ");
            strSql.Append(" FROM Template_Wfl_Stages ");
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
            strSql.Append(" WflStageId,WflTemplId,SequenceNumber,Enabled,DaysFromEstClose,Name,DaysFromCreation,TemplStageId,CalculationMethod ");
            strSql.Append(" FROM Template_Wfl_Stages ");
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
            parameters[0].Value = "Template_Wfl_Stages";
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

