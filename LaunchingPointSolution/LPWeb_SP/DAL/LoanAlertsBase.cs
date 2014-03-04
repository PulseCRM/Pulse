using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类LoanAlerts。
    /// </summary>
    public class LoanAlertsBase
    {
        public LoanAlertsBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.LoanAlerts model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LoanAlerts(");
            strSql.Append("FileId,Desc,DueDate,ClearedBy,Cleared,AcknowlegeReq,AcknowledgedBy,Acknowledged,LoanRuleId,OwnerId,LoanTaskId,AlertType,DateCreated,Status,Accepted,Declined,Dismissed,AcceptedBy,DeclinedBy,DismissedBy,AlertEmail,RecomEmail)");
            strSql.Append(" values (");
            strSql.Append("@FileId,@Desc,@DueDate,@ClearedBy,@Cleared,@AcknowlegeReq,@AcknowledgedBy,@Acknowledged,@LoanRuleId,@OwnerId,@LoanTaskId,@AlertType,@DateCreated,@Status,@Accepted,@Declined,@Dismissed,@AcceptedBy,@DeclinedBy,@DismissedBy,@AlertEmail,@RecomEmail)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@Desc", SqlDbType.NVarChar,255),
					new SqlParameter("@DueDate", SqlDbType.DateTime),
					new SqlParameter("@ClearedBy", SqlDbType.Int,4),
					new SqlParameter("@Cleared", SqlDbType.DateTime),
					new SqlParameter("@AcknowlegeReq", SqlDbType.Bit,1),
					new SqlParameter("@AcknowledgedBy", SqlDbType.NVarChar,200),
					new SqlParameter("@Acknowledged", SqlDbType.DateTime),
					new SqlParameter("@LoanRuleId", SqlDbType.Int,4),
					new SqlParameter("@OwnerId", SqlDbType.Int,4),
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4),
					new SqlParameter("@AlertType", SqlDbType.NVarChar,50),
					new SqlParameter("@DateCreated", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.NVarChar,50),
					new SqlParameter("@Accepted", SqlDbType.DateTime),
					new SqlParameter("@Declined", SqlDbType.DateTime),
					new SqlParameter("@Dismissed", SqlDbType.DateTime),
					new SqlParameter("@AcceptedBy", SqlDbType.NVarChar,200),
					new SqlParameter("@DeclinedBy", SqlDbType.NVarChar,200),
					new SqlParameter("@DismissedBy", SqlDbType.NVarChar,200),
					new SqlParameter("@AlertEmail", SqlDbType.VarBinary),
					new SqlParameter("@RecomEmail", SqlDbType.VarBinary)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.Desc;
            parameters[2].Value = model.DueDate;
            parameters[3].Value = model.ClearedBy;
            parameters[4].Value = model.Cleared;
            parameters[5].Value = model.AcknowlegeReq;
            parameters[6].Value = model.AcknowledgedBy;
            parameters[7].Value = model.Acknowledged;
            parameters[8].Value = model.LoanRuleId;
            parameters[9].Value = model.OwnerId;
            parameters[10].Value = model.LoanTaskId;
            parameters[11].Value = model.AlertType;
            parameters[12].Value = model.DateCreated;
            parameters[13].Value = model.Status;
            parameters[14].Value = model.Accepted;
            parameters[15].Value = model.Declined;
            parameters[16].Value = model.Dismissed;
            parameters[17].Value = model.AcceptedBy;
            parameters[18].Value = model.DeclinedBy;
            parameters[19].Value = model.DismissedBy;
            parameters[20].Value = model.AlertEmail;
            parameters[21].Value = model.RecomEmail;

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
        public void Update(LPWeb.Model.LoanAlerts model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update LoanAlerts set ");
            strSql.Append("LoanAlertId=@LoanAlertId,");
            strSql.Append("FileId=@FileId,");
            strSql.Append("Desc=@Desc,");
            strSql.Append("DueDate=@DueDate,");
            strSql.Append("ClearedBy=@ClearedBy,");
            strSql.Append("Cleared=@Cleared,");
            strSql.Append("AcknowlegeReq=@AcknowlegeReq,");
            strSql.Append("AcknowledgedBy=@AcknowledgedBy,");
            strSql.Append("Acknowledged=@Acknowledged,");
            strSql.Append("LoanRuleId=@LoanRuleId,");
            strSql.Append("OwnerId=@OwnerId,");
            strSql.Append("LoanTaskId=@LoanTaskId,");
            strSql.Append("AlertType=@AlertType,");
            strSql.Append("DateCreated=@DateCreated,");
            strSql.Append("Status=@Status,");
            strSql.Append("Accepted=@Accepted,");
            strSql.Append("Declined=@Declined,");
            strSql.Append("Dismissed=@Dismissed,");
            strSql.Append("AcceptedBy=@AcceptedBy,");
            strSql.Append("DeclinedBy=@DeclinedBy,");
            strSql.Append("DismissedBy=@DismissedBy,");
            strSql.Append("AlertEmail=@AlertEmail,");
            strSql.Append("RecomEmail=@RecomEmail");
            strSql.Append(" where LoanAlertId=@LoanAlertId ");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanAlertId", SqlDbType.Int,4),
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@Desc", SqlDbType.NVarChar,255),
					new SqlParameter("@DueDate", SqlDbType.DateTime),
					new SqlParameter("@ClearedBy", SqlDbType.Int,4),
					new SqlParameter("@Cleared", SqlDbType.DateTime),
					new SqlParameter("@AcknowlegeReq", SqlDbType.Bit,1),
					new SqlParameter("@AcknowledgedBy", SqlDbType.NVarChar,200),
					new SqlParameter("@Acknowledged", SqlDbType.DateTime),
					new SqlParameter("@LoanRuleId", SqlDbType.Int,4),
					new SqlParameter("@OwnerId", SqlDbType.Int,4),
					new SqlParameter("@LoanTaskId", SqlDbType.Int,4),
					new SqlParameter("@AlertType", SqlDbType.NVarChar,50),
					new SqlParameter("@DateCreated", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.NVarChar,50),
					new SqlParameter("@Accepted", SqlDbType.DateTime),
					new SqlParameter("@Declined", SqlDbType.DateTime),
					new SqlParameter("@Dismissed", SqlDbType.DateTime),
					new SqlParameter("@AcceptedBy", SqlDbType.NVarChar,200),
					new SqlParameter("@DeclinedBy", SqlDbType.NVarChar,200),
					new SqlParameter("@DismissedBy", SqlDbType.NVarChar,200),
					new SqlParameter("@AlertEmail", SqlDbType.VarBinary),
					new SqlParameter("@RecomEmail", SqlDbType.VarBinary)};
            parameters[0].Value = model.LoanAlertId;
            parameters[1].Value = model.FileId;
            parameters[2].Value = model.Desc;
            parameters[3].Value = model.DueDate;
            parameters[4].Value = model.ClearedBy;
            parameters[5].Value = model.Cleared;
            parameters[6].Value = model.AcknowlegeReq;
            parameters[7].Value = model.AcknowledgedBy;
            parameters[8].Value = model.Acknowledged;
            parameters[9].Value = model.LoanRuleId;
            parameters[10].Value = model.OwnerId;
            parameters[11].Value = model.LoanTaskId;
            parameters[12].Value = model.AlertType;
            parameters[13].Value = model.DateCreated;
            parameters[14].Value = model.Status;
            parameters[15].Value = model.Accepted;
            parameters[16].Value = model.Declined;
            parameters[17].Value = model.Dismissed;
            parameters[18].Value = model.AcceptedBy;
            parameters[19].Value = model.DeclinedBy;
            parameters[20].Value = model.DismissedBy;
            parameters[21].Value = model.AlertEmail;
            parameters[22].Value = model.RecomEmail;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int LoanAlertId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LoanAlerts ");
            strSql.Append(" where LoanAlertId=@LoanAlertId ");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanAlertId", SqlDbType.Int,4)};
            parameters[0].Value = LoanAlertId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.LoanAlerts GetModel(int LoanAlertId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 LoanAlertId,FileId,Desc,DueDate,ClearedBy,Cleared,AcknowlegeReq,AcknowledgedBy,Acknowledged,LoanRuleId,OwnerId,LoanTaskId,AlertType,DateCreated,Status,Accepted,Declined,Dismissed,AcceptedBy,DeclinedBy,DismissedBy,AlertEmail,RecomEmail from LoanAlerts ");
            strSql.Append(" where LoanAlertId=@LoanAlertId ");
            SqlParameter[] parameters = {
					new SqlParameter("@LoanAlertId", SqlDbType.Int,4)};
            parameters[0].Value = LoanAlertId;

            LPWeb.Model.LoanAlerts model = new LPWeb.Model.LoanAlerts();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["LoanAlertId"].ToString() != "")
                {
                    model.LoanAlertId = int.Parse(ds.Tables[0].Rows[0]["LoanAlertId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["FileId"].ToString() != "")
                {
                    model.FileId = int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
                }
                model.Desc = ds.Tables[0].Rows[0]["Desc"].ToString();
                if (ds.Tables[0].Rows[0]["DueDate"].ToString() != "")
                {
                    model.DueDate = DateTime.Parse(ds.Tables[0].Rows[0]["DueDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ClearedBy"].ToString() != "")
                {
                    model.ClearedBy = int.Parse(ds.Tables[0].Rows[0]["ClearedBy"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Cleared"].ToString() != "")
                {
                    model.Cleared = DateTime.Parse(ds.Tables[0].Rows[0]["Cleared"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AcknowlegeReq"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["AcknowlegeReq"].ToString() == "1") || (ds.Tables[0].Rows[0]["AcknowlegeReq"].ToString().ToLower() == "true"))
                    {
                        model.AcknowlegeReq = true;
                    }
                    else
                    {
                        model.AcknowlegeReq = false;
                    }
                }
                model.AcknowledgedBy = ds.Tables[0].Rows[0]["AcknowledgedBy"].ToString();
                if (ds.Tables[0].Rows[0]["Acknowledged"].ToString() != "")
                {
                    model.Acknowledged = DateTime.Parse(ds.Tables[0].Rows[0]["Acknowledged"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LoanRuleId"].ToString() != "")
                {
                    model.LoanRuleId = int.Parse(ds.Tables[0].Rows[0]["LoanRuleId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["OwnerId"].ToString() != "")
                {
                    model.OwnerId = int.Parse(ds.Tables[0].Rows[0]["OwnerId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LoanTaskId"].ToString() != "")
                {
                    model.LoanTaskId = int.Parse(ds.Tables[0].Rows[0]["LoanTaskId"].ToString());
                }
                model.AlertType = ds.Tables[0].Rows[0]["AlertType"].ToString();
                if (ds.Tables[0].Rows[0]["DateCreated"].ToString() != "")
                {
                    model.DateCreated = DateTime.Parse(ds.Tables[0].Rows[0]["DateCreated"].ToString());
                }
                model.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                if (ds.Tables[0].Rows[0]["Accepted"].ToString() != "")
                {
                    model.Accepted = DateTime.Parse(ds.Tables[0].Rows[0]["Accepted"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Declined"].ToString() != "")
                {
                    model.Declined = DateTime.Parse(ds.Tables[0].Rows[0]["Declined"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Dismissed"].ToString() != "")
                {
                    model.Dismissed = DateTime.Parse(ds.Tables[0].Rows[0]["Dismissed"].ToString());
                }
                model.AcceptedBy = ds.Tables[0].Rows[0]["AcceptedBy"].ToString();
                model.DeclinedBy = ds.Tables[0].Rows[0]["DeclinedBy"].ToString();
                model.DismissedBy = ds.Tables[0].Rows[0]["DismissedBy"].ToString();
                if (ds.Tables[0].Rows[0]["AlertEmail"].ToString() != "")
                {
                    model.AlertEmail = (byte[])ds.Tables[0].Rows[0]["AlertEmail"];
                }
                if (ds.Tables[0].Rows[0]["RecomEmail"].ToString() != "")
                {
                    model.RecomEmail = (byte[])ds.Tables[0].Rows[0]["RecomEmail"];
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
            strSql.Append("select LoanAlertId,FileId,[Desc],DueDate,ClearedBy,Cleared,AcknowlegeReq,AcknowledgedBy,Acknowledged,LoanRuleId,OwnerId,LoanTaskId,AlertType,DateCreated,[Status],Accepted,Declined,Dismissed,AcceptedBy,DeclinedBy,DismissedBy,AlertEmail,RecomEmail ");
            strSql.Append(" FROM LoanAlerts ");
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
            strSql.Append(" LoanAlertId,FileId,[Desc],DueDate,ClearedBy,Cleared,AcknowlegeReq,AcknowledgedBy,Acknowledged,LoanRuleId,OwnerId,LoanTaskId,AlertType,DateCreated,[Status],Accepted,Declined,Dismissed,AcceptedBy,DeclinedBy,DismissedBy,AlertEmail,RecomEmail ");
            strSql.Append(" FROM LoanAlerts ");
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
            parameters[0].Value = "LoanAlerts";
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

