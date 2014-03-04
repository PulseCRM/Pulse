using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类PointFiles。
	/// </summary>
    public class PointFilesBase
    {
        public PointFilesBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.PointFiles model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PointFiles(");
            strSql.Append("FolderId,Name,FirstImported,LastImported,Success)");
            strSql.Append(" values (");
            strSql.Append("@FolderId,@Name,@FirstImported,@LastImported,@Success)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@FolderId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@FirstImported", SqlDbType.DateTime),
					new SqlParameter("@LastImported", SqlDbType.DateTime),
					new SqlParameter("@Success", SqlDbType.Bit,1)};
                    //new SqlParameter("@CurrentImage", SqlDbType.VarBinary),
                    //new SqlParameter("@PreviousImage", SqlDbType.VarBinary)};
            if (model.FolderId <= 0)
                parameters[0].Value = DBNull.Value;
            else
                parameters[0].Value = model.FolderId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.FirstImported;
            parameters[3].Value = model.LastImported;
            parameters[4].Value = model.Success;
            //parameters[5].Value = model.CurrentImage == "" ? null : Convert.ToByte(model.CurrentImage);
            //parameters[6].Value = model.PreviousImage == "" ? null : Convert.ToByte(model.PreviousImage);

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
        public void Update(LPWeb.Model.PointFiles model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update PointFiles set ");
            //strSql.Append("FileId=@FileId,");
            strSql.Append("FolderId=@FolderId,");
            strSql.Append("Name=@Name,");
            strSql.Append("FirstImported=@FirstImported,");
            strSql.Append("LastImported=@LastImported,");
            strSql.Append("Success=@Success,");
            strSql.Append("CurrentImage=@CurrentImage,");
            strSql.Append("PreviousImage=@PreviousImage");
            strSql.Append(" where FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@FolderId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@FirstImported", SqlDbType.DateTime),
					new SqlParameter("@LastImported", SqlDbType.DateTime),
					new SqlParameter("@Success", SqlDbType.Bit,1),
					new SqlParameter("@CurrentImage", SqlDbType.NVarChar),
					new SqlParameter("@PreviousImage", SqlDbType.NVarChar)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.FolderId;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.FirstImported;
            parameters[4].Value = model.LastImported;
            parameters[5].Value = model.Success;
            parameters[6].Value = model.CurrentImage;
            parameters[7].Value = model.PreviousImage;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据 基本数据 
        /// </summary>
        public void UpdateBase(LPWeb.Model.PointFiles model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update PointFiles set ");
            strSql.Append("FolderId=@FolderId,");
            strSql.Append("Name=@Name");
            strSql.Append(" where FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@FolderId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,255)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.FolderId;
            parameters[2].Value = model.Name;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int FileId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from PointFiles ");
            strSql.Append(" where FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4)};
            parameters[0].Value = FileId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.PointFiles GetModel(int FileId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 FileId,FolderId,Name,FirstImported,LastImported,Success,CurrentImage,PreviousImage from PointFiles ");
            strSql.Append(" where FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4)};
            parameters[0].Value = FileId;

            LPWeb.Model.PointFiles model = new LPWeb.Model.PointFiles();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["FileId"].ToString() != "")
                {
                    model.FileId = int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FolderId"].ToString() != "")
                {
                    model.FolderId = int.Parse(ds.Tables[0].Rows[0]["FolderId"].ToString());
                }
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                if (ds.Tables[0].Rows[0]["FirstImported"].ToString() != "")
                {
                    model.FirstImported = DateTime.Parse(ds.Tables[0].Rows[0]["FirstImported"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LastImported"].ToString() != "")
                {
                    model.LastImported = DateTime.Parse(ds.Tables[0].Rows[0]["LastImported"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Success"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Success"].ToString() == "1") || (ds.Tables[0].Rows[0]["Success"].ToString().ToLower() == "true"))
                    {
                        model.Success = true;
                    }
                    else
                    {
                        model.Success = false;
                    }
                }
                model.CurrentImage = ds.Tables[0].Rows[0]["CurrentImage"].ToString();
                model.PreviousImage = ds.Tables[0].Rows[0]["PreviousImage"].ToString();
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
            strSql.Append("select FileId,FolderId,Name,FirstImported,LastImported,Success,CurrentImage,PreviousImage ");
            strSql.Append(" FROM PointFiles ");
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
            strSql.Append(" FileId,FolderId,Name,FirstImported,LastImported,Success,CurrentImage,PreviousImage ");
            strSql.Append(" FROM PointFiles ");
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
            parameters[0].Value = "PointFiles";
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

