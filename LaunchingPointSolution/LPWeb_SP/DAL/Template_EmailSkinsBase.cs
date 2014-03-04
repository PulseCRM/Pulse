using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
//using Maticsoft.DBUtility;//Please add references
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:Template_EmailSkins
    /// </summary>
    public class Template_EmailSkinsBase
    {
        public Template_EmailSkinsBase()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int EmailSkinId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Template_EmailSkins");
            strSql.Append(" where EmailSkinId=@EmailSkinId ");
            SqlParameter[] parameters = {
					new SqlParameter("@EmailSkinId", SqlDbType.Int,4)};
            parameters[0].Value = EmailSkinId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Template_EmailSkins model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Template_EmailSkins(");
            strSql.Append("Name,[Desc],HTMLBody,[Enabled],[Default])");
            strSql.Append(" values (");
            strSql.Append("@Name,@Desc,@HTMLBody,@Enabled,@Default)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500),
					new SqlParameter("@HTMLBody", SqlDbType.NVarChar),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Default", SqlDbType.Bit,1)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.Desc;
            parameters[2].Value = model.HTMLBody;
            parameters[3].Value = model.Enabled;
            parameters[4].Value = model.Default;

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
        public bool Update(LPWeb.Model.Template_EmailSkins model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Template_EmailSkins set ");
            strSql.Append("Name=@Name,");
            strSql.Append("[Desc]=@Desc,");
            strSql.Append("HTMLBody=@HTMLBody,");
            strSql.Append("[Enabled]=@Enabled,");
            strSql.Append("[Default]=@Default");
            strSql.Append(" where EmailSkinId=@EmailSkinId");
            SqlParameter[] parameters = {
					new SqlParameter("@EmailSkinId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500),
					new SqlParameter("@HTMLBody", SqlDbType.NVarChar),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Default", SqlDbType.Bit,1)};
            parameters[0].Value = model.EmailSkinId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Desc;
            parameters[3].Value = model.HTMLBody;
            parameters[4].Value = model.Enabled;
            parameters[5].Value = model.Default;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int EmailSkinId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Template_EmailSkins ");
            strSql.Append(" where EmailSkinId=@EmailSkinId");
            SqlParameter[] parameters = {
					new SqlParameter("@EmailSkinId", SqlDbType.Int,4)
};
            parameters[0].Value = EmailSkinId;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string EmailSkinIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Template_EmailSkins ");
            strSql.Append(" where EmailSkinId in (" + EmailSkinIdlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Template_EmailSkins GetModel(int EmailSkinId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 EmailSkinId,Name,[Desc],HTMLBody,[Enabled],[Default] from Template_EmailSkins ");
            strSql.Append(" where EmailSkinId=@EmailSkinId");
            SqlParameter[] parameters = {
					new SqlParameter("@EmailSkinId", SqlDbType.Int,4)
};
            parameters[0].Value = EmailSkinId;

            LPWeb.Model.Template_EmailSkins model = new LPWeb.Model.Template_EmailSkins();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["EmailSkinId"].ToString() != "")
                {
                    model.EmailSkinId = int.Parse(ds.Tables[0].Rows[0]["EmailSkinId"].ToString());
                }
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                model.Desc = ds.Tables[0].Rows[0]["Desc"] != DBNull.Value ? ds.Tables[0].Rows[0]["Desc"].ToString() : "";
                model.HTMLBody = ds.Tables[0].Rows[0]["HTMLBody"] != DBNull.Value ? ds.Tables[0].Rows[0]["HTMLBody"].ToString() : "";
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
            strSql.Append("select EmailSkinId,Name,[Desc],HTMLBody,[Enabled],[Default] ");
            strSql.Append(" FROM Template_EmailSkins ");
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
            strSql.Append(" EmailSkinId,Name,[Desc],HTMLBody,[Enabled],[Default] ");
            strSql.Append(" FROM Template_EmailSkins ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
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
            parameters[0].Value = "Template_EmailSkins";
            parameters[1].Value = "";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  Method
    }
}

