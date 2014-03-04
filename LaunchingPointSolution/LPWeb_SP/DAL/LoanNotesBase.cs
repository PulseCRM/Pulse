using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类LoanNotes。
	/// </summary>
	public class LoanNotesBase
    {
        public LoanNotesBase()
		{}
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.LoanNotes model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LoanNotes(");
            strSql.Append("FileId,Created,Sender,Note,Exported,ExternalViewing,LoanTaskId)");
            strSql.Append(" values (");
            strSql.Append("@FileId,@Created,@Sender,@Note,@Exported,@ExternalViewing,@LoanTaskId)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Sender", SqlDbType.NVarChar,255),
					new SqlParameter("@Note", SqlDbType.NVarChar,500),
					new SqlParameter("@Exported", SqlDbType.Int),
                    new SqlParameter("@ExternalViewing",SqlDbType.Bit),
                    new SqlParameter("@LoanTaskId", SqlDbType.Int,4)  };
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.Created;
            parameters[2].Value = model.Sender;
            parameters[3].Value = model.Note;
            parameters[4].Value = model.Exported;
            parameters[5].Value = model.ExternalViewing;
            parameters[6].Value = DBNull.Value;

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
        public void Update(LPWeb.Model.LoanNotes model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update LoanNotes set ");
            //strSql.Append("NoteId=@NoteId,");
            strSql.Append("FileId=@FileId,");
            strSql.Append("Created=@Created,");
            strSql.Append("Sender=@Sender,");
            strSql.Append("Note=@Note,");
            strSql.Append("Exported=@Exported,");
            strSql.Append("ExternalViewing=@ExternalViewing");
            strSql.Append(" where NoteId=@NoteId ");
            SqlParameter[] parameters = {
					new SqlParameter("@NoteId", SqlDbType.Int,4),
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Sender", SqlDbType.NVarChar,255),
					new SqlParameter("@Note", SqlDbType.NVarChar,500),
                    new SqlParameter("@Exported",SqlDbType.Bit),
                    new SqlParameter("@ExternalViewing",SqlDbType.Bit)};
            parameters[0].Value = model.NoteId;
            parameters[1].Value = model.FileId;
            parameters[2].Value = model.Created;
            parameters[3].Value = model.Sender;
            parameters[4].Value = model.Note;
            parameters[5].Value = model.Exported;
            parameters[6].Value = model.ExternalViewing;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        public void Update_LoanTaskId(LPWeb.Model.LoanNotes model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update LoanNotes set ");
            //strSql.Append("NoteId=@NoteId,");
            strSql.Append("FileId=@FileId,");
            strSql.Append("Created=@Created,");
            strSql.Append("Sender=@Sender,");
            strSql.Append("Note=@Note,");
            strSql.Append("Exported=@Exported,");
            strSql.Append("ExternalViewing=@ExternalViewing");
            strSql.Append(" where (FileId=@FileId) AND (LoanTaskId=@LoanTaskId) ");
            SqlParameter[] parameters = {
					new SqlParameter("@NoteId", SqlDbType.Int,4),
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@Sender", SqlDbType.NVarChar,255),
					new SqlParameter("@Note", SqlDbType.NVarChar,500),
                    new SqlParameter("@Exported",SqlDbType.Bit),
                    new SqlParameter("@ExternalViewing",SqlDbType.Bit),
                    new SqlParameter("@LoanTaskId", SqlDbType.Int,4)};
            parameters[0].Value = model.NoteId;
            parameters[1].Value = model.FileId;
            parameters[2].Value = model.Created;
            parameters[3].Value = model.Sender;
            parameters[4].Value = model.Note;
            parameters[5].Value = model.Exported;
            parameters[6].Value = model.ExternalViewing;
            parameters[7].Value = model.LoanTaskId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int NoteId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LoanNotes ");
            strSql.Append(" where NoteId=@NoteId ");
            SqlParameter[] parameters = {
					new SqlParameter("@NoteId", SqlDbType.Int,4)};
            parameters[0].Value = NoteId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.LoanNotes GetModel(int NoteId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 NoteId,FileId,Created,Sender,Note,Exported,ExternalViewing from LoanNotes ");
            strSql.Append(" where NoteId=@NoteId ");
            SqlParameter[] parameters = {
					new SqlParameter("@NoteId", SqlDbType.Int,4)};
            parameters[0].Value = NoteId;

            LPWeb.Model.LoanNotes model = new LPWeb.Model.LoanNotes();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["NoteId"].ToString() != "")
                {
                    model.NoteId = int.Parse(ds.Tables[0].Rows[0]["NoteId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FileId"].ToString() != "")
                {
                    model.FileId = int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
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

                //ExternalViewing gdc CR43
                if (ds.Tables[0].Rows[0]["ExternalViewing"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ExternalViewing"].ToString() == "1") || (ds.Tables[0].Rows[0]["ExternalViewing"].ToString().ToLower() == "true"))
                    {
                        model.ExternalViewing = true;
                    }
                    else
                    {
                        model.ExternalViewing = false;
                    }
                }

                return model;
            }
            else
            {
                return null;
            }
        }

        public LPWeb.Model.LoanNotes GetModel_LoanTaskId(int FileId, int LoanTaskId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 NoteId,FileId,Created,Sender,Note,Exported,ExternalViewing from LoanNotes ");
            strSql.Append(" where (FileId=@FileId) AND (LoanTaskId=@LoanTaskId) ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
                    new SqlParameter("@LoanTaskId", SqlDbType.Int,4)};

            parameters[0].Value = FileId;
            parameters[1].Value = LoanTaskId;

            LPWeb.Model.LoanNotes model = new LPWeb.Model.LoanNotes();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["NoteId"].ToString() != "")
                {
                    model.NoteId = int.Parse(ds.Tables[0].Rows[0]["NoteId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FileId"].ToString() != "")
                {
                    model.FileId = int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
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

                //ExternalViewing gdc CR43
                if (ds.Tables[0].Rows[0]["ExternalViewing"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ExternalViewing"].ToString() == "1") || (ds.Tables[0].Rows[0]["ExternalViewing"].ToString().ToLower() == "true"))
                    {
                        model.ExternalViewing = true;
                    }
                    else
                    {
                        model.ExternalViewing = false;
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
            strSql.Append("select NoteId,FileId,Created,Sender,Note,Exported ");
            strSql.Append(" FROM LoanNotes ");
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
            strSql.Append(" NoteId,FileId,Created,Sender,Note,Exported ");
            strSql.Append(" FROM LoanNotes ");
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
            parameters[0].Value = "LoanNotes";
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

