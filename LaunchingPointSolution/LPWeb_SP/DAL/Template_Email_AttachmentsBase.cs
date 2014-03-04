using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Template_Email_Attachments。
    /// </summary>
    public class Template_Email_AttachmentsBase
    {
        public Template_Email_AttachmentsBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Template_Email_Attachments model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Template_Email_Attachments(");
            strSql.Append("TemplEmailId,Enabled,Name,FileType,FileImage)");
            strSql.Append(" values (");
            strSql.Append("@TemplEmailId,@Enabled,@Name,@FileType,@FileImage)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplEmailId", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@FileType", SqlDbType.NVarChar,255),
					new SqlParameter("@FileImage", SqlDbType.VarBinary)};
            parameters[0].Value = model.TemplEmailId;
            parameters[1].Value = model.Enabled;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.FileType;
            parameters[4].Value = model.FileImage;

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
        public void Update(LPWeb.Model.Template_Email_Attachments model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_Email_Attachments set ");
            strSql.Append("TemplEmailId=@TemplEmailId,");
            strSql.Append("Enabled=@Enabled,");
            strSql.Append("Name=@Name,");
            strSql.Append("FileType=@FileType,");
            strSql.Append("FileImage=@FileImage");
            strSql.Append(" where TemplAttachId=@TemplAttachId ");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplAttachId", SqlDbType.Int,4),
					new SqlParameter("@TemplEmailId", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@FileType", SqlDbType.NVarChar,255),
					new SqlParameter("@FileImage", SqlDbType.VarBinary)};
            parameters[0].Value = model.TemplAttachId;
            parameters[1].Value = model.TemplEmailId;
            parameters[2].Value = model.Enabled;
            parameters[3].Value = model.Name;
            parameters[4].Value = model.FileType;
            parameters[5].Value = model.FileImage;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int TemplAttachId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Template_Email_Attachments ");
            strSql.Append(" where TemplAttachId=@TemplAttachId ");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplAttachId", SqlDbType.Int,4)};
            parameters[0].Value = TemplAttachId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Template_Email_Attachments GetModel(int TemplAttachId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 TemplAttachId,TemplEmailId,Enabled,Name,FileType,FileImage from Template_Email_Attachments ");
            strSql.Append(" where TemplAttachId=@TemplAttachId ");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplAttachId", SqlDbType.Int,4)};
            parameters[0].Value = TemplAttachId;

            LPWeb.Model.Template_Email_Attachments model = new LPWeb.Model.Template_Email_Attachments();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["TemplAttachId"].ToString() != "")
                {
                    model.TemplAttachId = int.Parse(ds.Tables[0].Rows[0]["TemplAttachId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TemplEmailId"].ToString() != "")
                {
                    model.TemplEmailId = int.Parse(ds.Tables[0].Rows[0]["TemplEmailId"].ToString());
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
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                model.FileType = ds.Tables[0].Rows[0]["FileType"].ToString();
                if (ds.Tables[0].Rows[0]["FileImage"].ToString() != "")
                {
                    model.FileImage = (byte[])ds.Tables[0].Rows[0]["FileImage"];
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
            strSql.Append("select TemplAttachId,TemplEmailId,Enabled,Name,FileType,FileImage ");
            strSql.Append(" FROM Template_Email_Attachments ");
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
            strSql.Append(" TemplAttachId,TemplEmailId,Enabled,Name,FileType,FileImage ");
            strSql.Append(" FROM Template_Email_Attachments ");
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

