using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类EmailQue。
	/// </summary>
	public class EmailQueBase
    {
        public EmailQueBase()
		{}
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.EmailQue model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into EmailQue(");
            strSql.Append("ToUser,ToContact,ToBorrower,EmailTmplId,LoanAlertId,FileId,AlertEmailType,EmailBody)");
            strSql.Append(" values (");
            strSql.Append("@ToUser,@ToContact,@ToBorrower,@EmailTmplId,@LoanAlertId,@FileId,@AlertEmailType,@EmailBody)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ToUser", SqlDbType.NVarChar,255),
					new SqlParameter("@ToContact", SqlDbType.NVarChar,255),
					new SqlParameter("@ToBorrower", SqlDbType.NVarChar,255),
					new SqlParameter("@EmailTmplId", SqlDbType.Int,4),
					new SqlParameter("@LoanAlertId", SqlDbType.Int,4),
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@AlertEmailType", SqlDbType.SmallInt,2),
					new SqlParameter("@EmailBody", SqlDbType.VarBinary)};
            parameters[0].Value = model.ToUser;
            parameters[1].Value = model.ToContact;
            parameters[2].Value = model.ToBorrower;
            parameters[3].Value = model.EmailTmplId;
            parameters[4].Value = model.LoanAlertId;
            parameters[5].Value = model.FileId;
            parameters[6].Value = model.AlertEmailType;
            parameters[7].Value = model.EmailBody;

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
        public void Update(LPWeb.Model.EmailQue model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update EmailQue set ");
            strSql.Append("EmailId=@EmailId,");
            strSql.Append("ToUser=@ToUser,");
            strSql.Append("ToContact=@ToContact,");
            strSql.Append("ToBorrower=@ToBorrower,");
            strSql.Append("EmailTmplId=@EmailTmplId,");
            strSql.Append("LoanAlertId=@LoanAlertId,");
            strSql.Append("FileId=@FileId,");
            strSql.Append("AlertEmailType=@AlertEmailType,");
            strSql.Append("EmailBody=@EmailBody");
            strSql.Append(" where EmailId=@EmailId ");
            SqlParameter[] parameters = {
					new SqlParameter("@EmailId", SqlDbType.Int,4),
					new SqlParameter("@ToUser", SqlDbType.NVarChar,255),
					new SqlParameter("@ToContact", SqlDbType.NVarChar,255),
					new SqlParameter("@ToBorrower", SqlDbType.NVarChar,255),
					new SqlParameter("@EmailTmplId", SqlDbType.Int,4),
					new SqlParameter("@LoanAlertId", SqlDbType.Int,4),
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@AlertEmailType", SqlDbType.SmallInt,2),
					new SqlParameter("@EmailBody", SqlDbType.VarBinary)};
            parameters[0].Value = model.EmailId;
            parameters[1].Value = model.ToUser;
            parameters[2].Value = model.ToContact;
            parameters[3].Value = model.ToBorrower;
            parameters[4].Value = model.EmailTmplId;
            parameters[5].Value = model.LoanAlertId;
            parameters[6].Value = model.FileId;
            parameters[7].Value = model.AlertEmailType;
            parameters[8].Value = model.EmailBody;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int EmailId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from EmailQue ");
            strSql.Append(" where EmailId=@EmailId ");
            SqlParameter[] parameters = {
					new SqlParameter("@EmailId", SqlDbType.Int,4)};
            parameters[0].Value = EmailId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.EmailQue GetModel(int EmailId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 EmailId,ToUser,ToContact,ToBorrower,EmailTmplId,LoanAlertId,FileId,AlertEmailType,EmailBody from EmailQue ");
            strSql.Append(" where EmailId=@EmailId ");
            SqlParameter[] parameters = {
					new SqlParameter("@EmailId", SqlDbType.Int,4)};
            parameters[0].Value = EmailId;

            LPWeb.Model.EmailQue model = new LPWeb.Model.EmailQue();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["EmailId"].ToString() != "")
                {
                    model.EmailId = int.Parse(ds.Tables[0].Rows[0]["EmailId"].ToString());
                }
                model.ToUser = ds.Tables[0].Rows[0]["ToUser"].ToString();
                model.ToContact = ds.Tables[0].Rows[0]["ToContact"].ToString();
                model.ToBorrower = ds.Tables[0].Rows[0]["ToBorrower"].ToString();
                if (ds.Tables[0].Rows[0]["EmailTmplId"].ToString() != "")
                {
                    model.EmailTmplId = int.Parse(ds.Tables[0].Rows[0]["EmailTmplId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LoanAlertId"].ToString() != "")
                {
                    model.LoanAlertId = int.Parse(ds.Tables[0].Rows[0]["LoanAlertId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FileId"].ToString() != "")
                {
                    model.FileId = int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AlertEmailType"].ToString() != "")
                {
                    model.AlertEmailType = int.Parse(ds.Tables[0].Rows[0]["AlertEmailType"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EmailBody"].ToString() != "")
                {
                    model.EmailBody = (byte[])ds.Tables[0].Rows[0]["EmailBody"];
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
            strSql.Append("select EmailId,ToUser,ToContact,ToBorrower,EmailTmplId,LoanAlertId,FileId,AlertEmailType,EmailBody ");
            strSql.Append(" FROM EmailQue ");
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
            strSql.Append(" EmailId,ToUser,ToContact,ToBorrower,EmailTmplId,LoanAlertId,FileId,AlertEmailType,EmailBody ");
            strSql.Append(" FROM EmailQue ");
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
            parameters[0].Value = "EmailQue";
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

