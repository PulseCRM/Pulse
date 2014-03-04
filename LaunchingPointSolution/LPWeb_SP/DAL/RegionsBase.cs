using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Regions。
    /// </summary>
    public class RegionsBase
    {
        public RegionsBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Regions model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Regions(");
            strSql.Append("Name,[Desc],Enabled,GroupID)");
            strSql.Append(" values (");
            strSql.Append("@Name,@Desc,@Enabled,@GroupID)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
                    new SqlParameter("@GroupID", SqlDbType.Int)                  };
            parameters[0].Value = model.Name;
            parameters[1].Value = model.Desc;
            parameters[2].Value = model.Enabled;
            parameters[3].Value = model.GroupID;

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
        public void Update(LPWeb.Model.Regions model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Regions set ");
            strSql.Append("Name=@Name,");
            strSql.Append("Desc=@Desc,");
            strSql.Append("Enabled=@Enabled");
            strSql.Append("GroupID=@GroupID");
            strSql.Append(" where RegionId=@RegionId ");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@GroupID", SqlDbType.Int)
                                        };
            parameters[0].Value = model.Name;
            parameters[1].Value = model.Desc;
            parameters[2].Value = model.Enabled;
            parameters[3].Value = model.GroupID;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int RegionId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Regions ");
            strSql.Append(" where RegionId=@RegionId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RegionId", SqlDbType.Int,4)};
            parameters[0].Value = RegionId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Regions GetModel(int RegionId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 RegionId,Name,[Desc],Enabled,GroupID from Regions ");
            strSql.Append(" where RegionId=@RegionId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RegionId", SqlDbType.Int,4)};
            parameters[0].Value = RegionId;

            LPWeb.Model.Regions model = new LPWeb.Model.Regions();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["RegionId"].ToString() != "")
                {
                    model.RegionId = int.Parse(ds.Tables[0].Rows[0]["RegionId"].ToString());
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
                if (ds.Tables[0].Rows[0]["GroupID"].ToString() != "")
                {
                    model.GroupID = int.Parse(ds.Tables[0].Rows[0]["GroupID"].ToString());
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
            strSql.Append("select RegionId,Name,[Desc],Enabled,GroupID ");
            strSql.Append(" FROM Regions ");
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
            strSql.Append(" RegionId,Name,[Desc],Enabled,GroupID ");
            strSql.Append(" FROM Regions ");
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
            parameters[0].Value = "Regions";
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

