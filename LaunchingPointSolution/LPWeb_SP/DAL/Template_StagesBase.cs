using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Template_Stages。
    /// </summary>
    public class Template_StagesBase
    {
        public Template_StagesBase()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int TemplStageId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Template_Stages");
            strSql.Append(" where TemplStageId=@TemplStageId ");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplStageId", SqlDbType.Int,4)};
            parameters[0].Value = TemplStageId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Template_Stages model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Template_Stages(");
            strSql.Append("Name,Enabled,SequenceNumber,WorkflowType,Custom,PointStageNameField,PointStageDateField,Alias,DaysFromEstClose,DaysFromCreation)");
            strSql.Append(" values (");
            strSql.Append("@Name,@Enabled,@SequenceNumber,@WorkflowType,@Custom,@PointStageNameField,@PointStageDateField,@Alias,@DaysFromEstClose,@DaysFromCreation)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@SequenceNumber", SqlDbType.SmallInt,2),
					new SqlParameter("@WorkflowType", SqlDbType.NVarChar,50),
					new SqlParameter("@Custom", SqlDbType.Bit,1),
					new SqlParameter("@PointStageNameField", SqlDbType.SmallInt,2),
					new SqlParameter("@PointStageDateField", SqlDbType.SmallInt,2),
					new SqlParameter("@Alias", SqlDbType.NVarChar,50),
					new SqlParameter("@DaysFromEstClose", SqlDbType.SmallInt,2),
					new SqlParameter("@DaysFromCreation", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.Enabled;
            if (model.SequenceNumber == null)
            {
                parameters[2].Value = DBNull.Value;
            }
            else
            {
                parameters[2].Value = model.SequenceNumber;
            }
            parameters[3].Value = model.WorkflowType;
            parameters[4].Value = model.Custom;
            if (model.PointStageNameField == null)
            {
                parameters[5].Value = DBNull.Value;
            }
            else
            {
                parameters[5].Value = model.PointStageNameField;
            }
            if (model.PointStageDateField == null)
            {
                parameters[6].Value = DBNull.Value;
            }
            else
            {
                parameters[6].Value = model.PointStageDateField;
            }
            parameters[7].Value = model.Alias;
            if (model.DaysFromEstClose == null)
            {
                parameters[8].Value = DBNull.Value;
            }
            else
            {
                parameters[8].Value = model.DaysFromEstClose;
            }
            if (model.DaysFromCreation == null)
            {
                parameters[9].Value = DBNull.Value;
            }
            else
            {
                parameters[9].Value = model.DaysFromCreation;
            }

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
        public void Update(LPWeb.Model.Template_Stages model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_Stages set ");
            strSql.Append("Name=@Name,");
            strSql.Append("Enabled=@Enabled,");
            strSql.Append("SequenceNumber=@SequenceNumber,");
            strSql.Append("WorkflowType=@WorkflowType,");
            strSql.Append("Custom=@Custom,");
            strSql.Append("PointStageNameField=@PointStageNameField,");
            strSql.Append("PointStageDateField=@PointStageDateField,");
            strSql.Append("Alias=@Alias,");
            strSql.Append("DaysFromEstClose=@DaysFromEstClose,");
            strSql.Append("DaysFromCreation=@DaysFromCreation");
            strSql.Append(" where TemplStageId=@TemplStageId ");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplStageId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@SequenceNumber", SqlDbType.SmallInt,2),
					new SqlParameter("@WorkflowType", SqlDbType.NVarChar,50),
					new SqlParameter("@Custom", SqlDbType.Bit,1),
					new SqlParameter("@PointStageNameField", SqlDbType.SmallInt,2),
					new SqlParameter("@PointStageDateField", SqlDbType.SmallInt,2),
					new SqlParameter("@Alias", SqlDbType.NVarChar,50),
					new SqlParameter("@DaysFromEstClose", SqlDbType.SmallInt,2),
					new SqlParameter("@DaysFromCreation", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.TemplStageId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Enabled;
            if (model.SequenceNumber == null)
            {
                parameters[3].Value = DBNull.Value;
            }
            else
            {
                parameters[3].Value = model.SequenceNumber;
            }
            parameters[4].Value = model.WorkflowType;
            parameters[5].Value = model.Custom;
            if (model.PointStageNameField == null)
            {
                parameters[6].Value = DBNull.Value;
            }
            else
            {
                parameters[6].Value = model.PointStageNameField;
            }
            if (model.PointStageDateField == null)
            {
                parameters[7].Value = DBNull.Value;
            }
            else
            {
                parameters[7].Value = model.PointStageDateField;
            }
            parameters[8].Value = model.Alias;
            if (model.DaysFromEstClose == null)
            {
                parameters[9].Value = DBNull.Value;
            }
            else
            {
                parameters[9].Value = model.DaysFromEstClose;
            }
            if (model.DaysFromCreation == null)
            {
                parameters[10].Value = DBNull.Value;
            }
            else
            {
                parameters[10].Value = model.DaysFromCreation;
            }

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int TemplStageId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Template_Stages ");
            strSql.Append(" where TemplStageId=@TemplStageId ");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplStageId", SqlDbType.Int,4)};
            parameters[0].Value = TemplStageId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Template_Stages GetModel(int TemplStageId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 TemplStageId,Name,Enabled,SequenceNumber,WorkflowType,Custom,PointStageNameField,PointStageDateField,Alias,DaysFromEstClose,DaysFromCreation from Template_Stages ");
            strSql.Append(" where TemplStageId=@TemplStageId ");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplStageId", SqlDbType.Int,4)};
            parameters[0].Value = TemplStageId;

            LPWeb.Model.Template_Stages model = new LPWeb.Model.Template_Stages();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["TemplStageId"].ToString() != "")
                {
                    model.TemplStageId = int.Parse(ds.Tables[0].Rows[0]["TemplStageId"].ToString());
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
                if (ds.Tables[0].Rows[0]["SequenceNumber"].ToString() != "")
                {
                    model.SequenceNumber = int.Parse(ds.Tables[0].Rows[0]["SequenceNumber"].ToString());
                }
                model.WorkflowType = ds.Tables[0].Rows[0]["WorkflowType"].ToString();
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
                if (ds.Tables[0].Rows[0]["PointStageNameField"].ToString() != "")
                {
                    model.PointStageNameField = int.Parse(ds.Tables[0].Rows[0]["PointStageNameField"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PointStageDateField"].ToString() != "")
                {
                    model.PointStageDateField = int.Parse(ds.Tables[0].Rows[0]["PointStageDateField"].ToString());
                }
                model.Alias = ds.Tables[0].Rows[0]["Alias"].ToString();
                if (ds.Tables[0].Rows[0]["DaysFromEstClose"].ToString() != "")
                {
                    model.DaysFromEstClose = int.Parse(ds.Tables[0].Rows[0]["DaysFromEstClose"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DaysFromCreation"].ToString() != "")
                {
                    model.DaysFromCreation = int.Parse(ds.Tables[0].Rows[0]["DaysFromCreation"].ToString());
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
            strSql.Append("select TemplStageId,Name,Enabled,SequenceNumber,WorkflowType,Custom,PointStageNameField,PointStageDateField,Alias,DaysFromEstClose,DaysFromCreation ");
            strSql.Append(" FROM Template_Stages ");
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
            strSql.Append(" TemplStageId,Name,Enabled,SequenceNumber,WorkflowType,Custom,PointStageNameField,PointStageDateField,Alias,DaysFromEstClose,DaysFromCreation ");
            strSql.Append(" FROM Template_Stages ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }



        #endregion  成员方法
    }
}

