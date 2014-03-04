using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Template_Email。
	/// </summary>
	public class Template_EmailBase
    {
        public Template_EmailBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Template_Email model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Template_Email(");
            strSql.Append("Enabled,Name,[Desc],FromUserRoles,FromEmailAddress,Content)");
            strSql.Append(" values (");
            strSql.Append("@Enabled,@Name,@Desc,@FromUserRoles,@FromEmailAddress,@Content)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500),
					new SqlParameter("@FromUserRoles", SqlDbType.Int,4),
					new SqlParameter("@FromEmailAddress", SqlDbType.NVarChar,255),
					new SqlParameter("@Content", SqlDbType.NVarChar)};
            parameters[0].Value = model.Enabled;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Desc;
            parameters[3].Value = model.FromUserRoles;
            parameters[4].Value = model.FromEmailAddress;
            parameters[5].Value = model.Content;

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
        public void Update(LPWeb.Model.Template_Email model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_Email set ");
            strSql.Append("TemplEmailId=@TemplEmailId,");
            strSql.Append("Enabled=@Enabled,");
            strSql.Append("Name=@Name,");
            strSql.Append("Desc=@Desc,");
            strSql.Append("FromUserRoles=@FromUserRoles,");
            strSql.Append("FromEmailAddress=@FromEmailAddress,");
            strSql.Append("Content=@Content");
            strSql.Append(" where TemplEmailId=@TemplEmailId ");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplEmailId", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500),
					new SqlParameter("@FromUserRoles", SqlDbType.Int,4),
					new SqlParameter("@FromEmailAddress", SqlDbType.NVarChar,255),
					new SqlParameter("@Content", SqlDbType.NVarChar)};
            parameters[0].Value = model.TemplEmailId;
            parameters[1].Value = model.Enabled;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.Desc;
            parameters[4].Value = model.FromUserRoles;
            parameters[5].Value = model.FromEmailAddress;
            parameters[6].Value = model.Content;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int TemplEmailId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Template_Email ");
            strSql.Append(" where TemplEmailId=@TemplEmailId ");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplEmailId", SqlDbType.Int,4)};
            parameters[0].Value = TemplEmailId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Template_Email GetModel(int TemplEmailId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 TemplEmailId,Enabled,Name,[Desc],FromUserRoles,FromEmailAddress,Content,EmailSkinId from Template_Email ");
            strSql.Append(" where TemplEmailId=@TemplEmailId ");
            SqlParameter[] parameters = {
					new SqlParameter("@TemplEmailId", SqlDbType.Int,4)};
            parameters[0].Value = TemplEmailId;

            LPWeb.Model.Template_Email model = new LPWeb.Model.Template_Email();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
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
                model.Desc = ds.Tables[0].Rows[0]["Desc"].ToString();
                if (ds.Tables[0].Rows[0]["FromUserRoles"].ToString() != "")
                {
                    model.FromUserRoles = int.Parse(ds.Tables[0].Rows[0]["FromUserRoles"].ToString());
                }
                model.FromEmailAddress = ds.Tables[0].Rows[0]["FromEmailAddress"].ToString();
                model.Content = ds.Tables[0].Rows[0]["Content"].ToString();

                model.EmailSkinId = 0;
                if (ds.Tables[0].Rows[0]["EmailSkinId"] != DBNull.Value && ds.Tables[0].Rows[0]["EmailSkinId"].ToString() != "")
                {
                    model.EmailSkinId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmailSkinId"]);
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
            strSql.Append("select TemplEmailId,Enabled,Name,[Desc],FromUserRoles,FromEmailAddress,Content ");
            strSql.Append(" FROM Template_Email ");
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
            strSql.Append(" TemplEmailId,Enabled,Name,[Desc],FromUserRoles,FromEmailAddress,Content ");
            strSql.Append(" FROM Template_Email ");
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
            parameters[0].Value = "Template_Email";
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

