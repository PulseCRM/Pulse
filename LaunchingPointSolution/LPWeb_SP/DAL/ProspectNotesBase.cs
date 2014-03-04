using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类ProspectNotes。
    /// </summary>
    public class ProspectNotesBase
    {
        public ProspectNotesBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.ProspectNotes model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ProspectNotes(");
            strSql.Append("ContactId,Created,Sender,Note,Exported)");
            strSql.Append(" values (");
            strSql.Append("@ContactId,@Created,@Sender,@Note,@Exported)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Sender", SqlDbType.NVarChar,255),
					new SqlParameter("@Note", SqlDbType.NVarChar,500),
					new SqlParameter("@Exported", SqlDbType.Bit,1)};
            parameters[0].Value = model.ContactId;
            parameters[1].Value = model.Created;
            parameters[2].Value = model.Sender;
            parameters[3].Value = model.Note;
            parameters[4].Value = model.Exported;

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
        public void Update(LPWeb.Model.ProspectNotes model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ProspectNotes set ");
            strSql.Append("PropsectNoteId=@PropsectNoteId,");
            strSql.Append("ContactId=@ContactId,");
            strSql.Append("Created=@Created,");
            strSql.Append("Sender=@Sender,");
            strSql.Append("Note=@Note,");
            strSql.Append("Exported=@Exported");
            strSql.Append(" where PropsectNoteId=@PropsectNoteId ");
            SqlParameter[] parameters = {
					new SqlParameter("@PropsectNoteId", SqlDbType.Int,4),
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Sender", SqlDbType.NVarChar,255),
					new SqlParameter("@Note", SqlDbType.NVarChar,500),
					new SqlParameter("@Exported", SqlDbType.Bit,1)};
            parameters[0].Value = model.PropsectNoteId;
            parameters[1].Value = model.ContactId;
            parameters[2].Value = model.Created;
            parameters[3].Value = model.Sender;
            parameters[4].Value = model.Note;
            parameters[5].Value = model.Exported;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int PropsectNoteId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProspectNotes ");
            strSql.Append(" where PropsectNoteId=@PropsectNoteId ");
            SqlParameter[] parameters = {
					new SqlParameter("@PropsectNoteId", SqlDbType.Int,4)};
            parameters[0].Value = PropsectNoteId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.ProspectNotes GetModel(int PropsectNoteId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 PropsectNoteId,ContactId,Created,Sender,Note,Exported from ProspectNotes ");
            strSql.Append(" where PropsectNoteId=@PropsectNoteId ");
            SqlParameter[] parameters = {
					new SqlParameter("@PropsectNoteId", SqlDbType.Int,4)};
            parameters[0].Value = PropsectNoteId;

            LPWeb.Model.ProspectNotes model = new LPWeb.Model.ProspectNotes();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["PropsectNoteId"].ToString() != "")
                {
                    model.PropsectNoteId = int.Parse(ds.Tables[0].Rows[0]["PropsectNoteId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ContactId"].ToString() != "")
                {
                    model.ContactId = int.Parse(ds.Tables[0].Rows[0]["ContactId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Created"].ToString() != "")
                {
                    model.Created = DateTime.Parse(ds.Tables[0].Rows[0]["Created"].ToString());
                }
                model.Sender = ds.Tables[0].Rows[0]["Sender"].ToString();
                model.Note = ds.Tables[0].Rows[0]["Note"].ToString();
                if (ds.Tables[0].Rows[0]["Exported"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Exported"].ToString() == "1") || (ds.Tables[0].Rows[0]["Exported"].ToString().ToLower() == "true"))
                    {
                        model.Exported = true;
                    }
                    else
                    {
                        model.Exported = false;
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
            strSql.Append("select PropsectNoteId,ContactId,Created,Sender,Note,Exported ");
            strSql.Append(" FROM ProspectNotes ");
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
            strSql.Append(" PropsectNoteId,ContactId,Created,Sender,Note,Exported ");
            strSql.Append(" FROM ProspectNotes ");
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
            parameters[0].Value = "ProspectNotes";
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
