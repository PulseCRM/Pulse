using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Prospect。
    /// </summary>
    public class ProspectBase
    {
        public ProspectBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.Prospect model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Prospect(");
            strSql.Append("Contactid,LeadSource,ReferenceCode,Referral,Created,CreatedBy,Modifed,ModifiedBy,Loanofficer,Status,CreditRanking,PreferredContact,Dependents)");
            strSql.Append(" values (");
            strSql.Append("@Contactid,@LeadSource,@ReferenceCode,@Referral,@Created,@CreatedBy,@Modifed,@ModifiedBy,@Loanofficer,@Status,@CreditRanking,@PreferredContact,@Dependents)");
            SqlParameter[] parameters = {
					new SqlParameter("@Contactid", SqlDbType.Int,4),
					new SqlParameter("@LeadSource", SqlDbType.NVarChar,255),
					new SqlParameter("@ReferenceCode", SqlDbType.NVarChar,255),
					new SqlParameter("@Referral", SqlDbType.Int,4),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@CreatedBy", SqlDbType.Int,4),
					new SqlParameter("@Modifed", SqlDbType.DateTime),
					new SqlParameter("@ModifiedBy", SqlDbType.Int,4),
					new SqlParameter("@Loanofficer", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.NVarChar,50),
					new SqlParameter("@CreditRanking", SqlDbType.NVarChar,50),
					new SqlParameter("@PreferredContact", SqlDbType.NVarChar,50),
					new SqlParameter("@Dependents", SqlDbType.Bit,1)};
            parameters[0].Value = model.Contactid;
            parameters[1].Value = model.LeadSource;
            parameters[2].Value = model.ReferenceCode;
            parameters[3].Value = model.Referral;
            parameters[4].Value = model.Created;
            parameters[5].Value = model.CreatedBy;
            parameters[6].Value = model.Modifed;
            parameters[7].Value = model.ModifiedBy;
            parameters[8].Value = model.Loanofficer;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.CreditRanking;
            parameters[11].Value = model.PreferredContact;
            parameters[12].Value = model.Dependents;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Prospect model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Prospect set ");
            strSql.Append("LeadSource=@LeadSource,");
            strSql.Append("ReferenceCode=@ReferenceCode,");
            strSql.Append("Referral=@Referral,");
            strSql.Append("Created=@Created,");
            strSql.Append("CreatedBy=@CreatedBy,");
            strSql.Append("Modifed=@Modifed,");
            strSql.Append("ModifiedBy=@ModifiedBy,");
            strSql.Append("Loanofficer=@Loanofficer,");
            strSql.Append("Status=@Status,");
            strSql.Append("CreditRanking=@CreditRanking,");
            strSql.Append("PreferredContact=@PreferredContact,");
            strSql.Append("Dependents=@Dependents");
            strSql.Append(" where Contactid=@Contactid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Contactid", SqlDbType.Int,4),
					new SqlParameter("@LeadSource", SqlDbType.NVarChar,255),
					new SqlParameter("@ReferenceCode", SqlDbType.NVarChar,255),
					new SqlParameter("@Referral", SqlDbType.Int,4),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@CreatedBy", SqlDbType.Int,4),
					new SqlParameter("@Modifed", SqlDbType.DateTime),
					new SqlParameter("@ModifiedBy", SqlDbType.Int,4),
					new SqlParameter("@Loanofficer", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.NVarChar,50),
					new SqlParameter("@CreditRanking", SqlDbType.NVarChar,50),
					new SqlParameter("@PreferredContact", SqlDbType.NVarChar,50),
					new SqlParameter("@Dependents", SqlDbType.Bit,1)};
            parameters[0].Value = model.Contactid;
            parameters[1].Value = model.LeadSource;
            parameters[2].Value = model.ReferenceCode;
            parameters[3].Value = model.Referral;
            parameters[4].Value = model.Created;
            parameters[5].Value = model.CreatedBy;
            parameters[6].Value = model.Modifed;
            parameters[7].Value = model.ModifiedBy;
            parameters[8].Value = model.Loanofficer;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.CreditRanking;
            parameters[11].Value = model.PreferredContact;
            parameters[12].Value = model.Dependents;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int Contactid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Prospect ");
            strSql.Append(" where Contactid=@Contactid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Contactid", SqlDbType.Int,4)};
            parameters[0].Value = Contactid;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Prospect GetModel(int Contactid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Contactid,LeadSource,ReferenceCode,Referral,Created,CreatedBy,Modifed,ModifiedBy,Loanofficer,Status,CreditRanking,PreferredContact,Dependents from Prospect ");
            strSql.Append(" where Contactid=@Contactid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Contactid", SqlDbType.Int,4)};
            parameters[0].Value = Contactid;

            LPWeb.Model.Prospect model = new LPWeb.Model.Prospect();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Contactid"].ToString() != "")
                {
                    model.Contactid = int.Parse(ds.Tables[0].Rows[0]["Contactid"].ToString());
                }
                model.LeadSource = ds.Tables[0].Rows[0]["LeadSource"].ToString();
                model.ReferenceCode = ds.Tables[0].Rows[0]["ReferenceCode"].ToString();
                if (ds.Tables[0].Rows[0]["Referral"].ToString() != "")
                {
                    model.Referral = int.Parse(ds.Tables[0].Rows[0]["Referral"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Created"].ToString() != "")
                {
                    model.Created = DateTime.Parse(ds.Tables[0].Rows[0]["Created"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CreatedBy"].ToString() != "")
                {
                    model.CreatedBy = int.Parse(ds.Tables[0].Rows[0]["CreatedBy"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Modifed"].ToString() != "")
                {
                    model.Modifed = DateTime.Parse(ds.Tables[0].Rows[0]["Modifed"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ModifiedBy"].ToString() != "")
                {
                    model.ModifiedBy = int.Parse(ds.Tables[0].Rows[0]["ModifiedBy"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Loanofficer"].ToString() != "")
                {
                    model.Loanofficer = int.Parse(ds.Tables[0].Rows[0]["Loanofficer"].ToString());
                }
                model.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                model.CreditRanking = ds.Tables[0].Rows[0]["CreditRanking"].ToString();
                model.PreferredContact = ds.Tables[0].Rows[0]["PreferredContact"].ToString();
                if (ds.Tables[0].Rows[0]["Dependents"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Dependents"].ToString() == "1") || (ds.Tables[0].Rows[0]["Dependents"].ToString().ToLower() == "true"))
                    {
                        model.Dependents = true;
                    }
                    else
                    {
                        model.Dependents = false;
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
            strSql.Append("select Contactid,LeadSource,ReferenceCode,Referral,Created,CreatedBy,Modifed,ModifiedBy,Loanofficer,Status,CreditRanking,PreferredContact,Dependents ");
            strSql.Append(" FROM Prospect ");
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
            strSql.Append(" Contactid,LeadSource,ReferenceCode,Referral,Created,CreatedBy,Modifed,ModifiedBy,Loanofficer,Status,CreditRanking,PreferredContact,Dependents ");
            strSql.Append(" FROM Prospect ");
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
            parameters[0].Value = "Prospect";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  成员方法
    }
}