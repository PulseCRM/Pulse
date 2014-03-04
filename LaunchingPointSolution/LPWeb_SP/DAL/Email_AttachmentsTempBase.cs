﻿using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:Email_AttachmentsTempBase
    /// </summary>
    public class Email_AttachmentsTempBase
    {
        public Email_AttachmentsTempBase()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Email_AttachmentsTemp");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = Id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Email_AttachmentsTemp model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Email_AttachmentsTemp(");
            strSql.Append("Token,TemplAttachId,Name,FileType,FileImage,CreateDateTime)");
            strSql.Append(" values (");
            strSql.Append("@Token,@TemplAttachId,@Name,@FileType,@FileImage,@CreateDateTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Token", SqlDbType.NVarChar,255),
					new SqlParameter("@TemplAttachId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@FileType", SqlDbType.NVarChar,255),
					new SqlParameter("@FileImage", SqlDbType.VarBinary),
					new SqlParameter("@CreateDateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Token;
            parameters[1].Value = model.TemplAttachId;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.FileType;
            parameters[4].Value = model.FileImage;
            parameters[5].Value = model.CreateDateTime;

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
        public bool Update(LPWeb.Model.Email_AttachmentsTemp model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Email_AttachmentsTemp set ");
            strSql.Append("Token=@Token,");
            strSql.Append("TemplAttachId=@TemplAttachId,");
            strSql.Append("Name=@Name,");
            strSql.Append("FileType=@FileType,");
            strSql.Append("FileImage=@FileImage,");
            strSql.Append("CreateDateTime=@CreateDateTime");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4),
					new SqlParameter("@Token", SqlDbType.NVarChar,255),
					new SqlParameter("@TemplAttachId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@FileType", SqlDbType.NVarChar,255),
					new SqlParameter("@FileImage", SqlDbType.VarBinary),
					new SqlParameter("@CreateDateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Id;
            parameters[1].Value = model.Token;
            parameters[2].Value = model.TemplAttachId;
            parameters[3].Value = model.Name;
            parameters[4].Value = model.FileType;
            parameters[5].Value = model.FileImage;
            parameters[6].Value = model.CreateDateTime;

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
        public bool Delete(int Id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Email_AttachmentsTemp ");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
};
            parameters[0].Value = Id;

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
        public bool DeleteList(string Idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Email_AttachmentsTemp ");
            strSql.Append(" where Id in (" + Idlist + ")  ");
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
        public LPWeb.Model.Email_AttachmentsTemp GetModel(int Id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Id,Token,TemplAttachId,Name,FileType,FileImage,CreateDateTime from Email_AttachmentsTemp ");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
};
            parameters[0].Value = Id;

            LPWeb.Model.Email_AttachmentsTemp model = new LPWeb.Model.Email_AttachmentsTemp();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Id"].ToString() != "")
                {
                    model.Id = int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
                }
                model.Token = ds.Tables[0].Rows[0]["Token"].ToString();
                if (ds.Tables[0].Rows[0]["TemplAttachId"].ToString() != "")
                {
                    model.TemplAttachId = int.Parse(ds.Tables[0].Rows[0]["TemplAttachId"].ToString());
                }
                model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                model.FileType = ds.Tables[0].Rows[0]["FileType"].ToString();
                if (ds.Tables[0].Rows[0]["FileImage"].ToString() != "")
                {
                    model.FileImage = (byte[])ds.Tables[0].Rows[0]["FileImage"];
                }
                if (ds.Tables[0].Rows[0]["CreateDateTime"].ToString() != "")
                {
                    model.CreateDateTime = DateTime.Parse(ds.Tables[0].Rows[0]["CreateDateTime"].ToString());
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
            strSql.Append("select Id,Token,TemplAttachId,Name,FileType,FileImage,CreateDateTime ");
            strSql.Append(" FROM Email_AttachmentsTemp ");
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
            strSql.Append(" Id,Token,TemplAttachId,Name,FileType,FileImage,CreateDateTime ");
            strSql.Append(" FROM Email_AttachmentsTemp ");
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
            parameters[0].Value = "Email_AttachmentsTemp";
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

