using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:UserProspectColumns
    /// </summary>
    public class UserProspectColumnsBase
    {
        public UserProspectColumnsBase()
        { }
        #region  Method

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.UserProspectColumns model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UserProspectColumns(");
            strSql.Append("UserId,Pv_Created,Pv_LeadSource,Pv_RefCode,Pv_LoanOfficer,Pv_Branch,Pv_Progress,Lv_Ranking,Lv_Amount,Lv_Rate,Lv_LoanOfficer,Lv_Lien,Lv_Progress,Lv_Branch,Lv_LoanProgram,Lv_LeadSource,Lv_RefCode,Lv_EstClose,Lv_PointFilename,Pv_Referral,Pv_Partner,Lv_Referral,Lv_Partner,LastCompletedStage,LastStageComplDate)");
            strSql.Append(" values (");
            strSql.Append("@UserId,@Pv_Created,@Pv_LeadSource,@Pv_RefCode,@Pv_LoanOfficer,@Pv_Branch,@Pv_Progress,@Lv_Ranking,@Lv_Amount,@Lv_Rate,@Lv_LoanOfficer,@Lv_Lien,@Lv_Progress,@Lv_Branch,@Lv_LoanProgram,@Lv_LeadSource,@Lv_RefCode,@Lv_EstClose,@Lv_PointFilename,@Pv_Referral,@Pv_Partner,@Lv_Referral,@Lv_Partner,@LastCompletedStage,@LastStageComplDate)");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@Pv_Created", SqlDbType.Bit,1),
					new SqlParameter("@Pv_LeadSource", SqlDbType.Bit,1),
					new SqlParameter("@Pv_RefCode", SqlDbType.Bit,1),
					new SqlParameter("@Pv_LoanOfficer", SqlDbType.Bit,1),
					new SqlParameter("@Pv_Branch", SqlDbType.Bit,1),
					new SqlParameter("@Pv_Progress", SqlDbType.Bit,1),
					new SqlParameter("@Lv_Ranking", SqlDbType.Bit,1),
					new SqlParameter("@Lv_Amount", SqlDbType.Bit,1),
					new SqlParameter("@Lv_Rate", SqlDbType.Bit,1),
					new SqlParameter("@Lv_LoanOfficer", SqlDbType.Bit,1),
					new SqlParameter("@Lv_Lien", SqlDbType.Bit,1),
					new SqlParameter("@Lv_Progress", SqlDbType.Bit,1),
					new SqlParameter("@Lv_Branch", SqlDbType.Bit,1),
					new SqlParameter("@Lv_LoanProgram", SqlDbType.Bit,1),
					new SqlParameter("@Lv_LeadSource", SqlDbType.Bit,1),
					new SqlParameter("@Lv_RefCode", SqlDbType.Bit,1),
					new SqlParameter("@Lv_EstClose", SqlDbType.Bit,1),
					new SqlParameter("@Lv_PointFilename", SqlDbType.Bit,1),
					new SqlParameter("@Pv_Referral", SqlDbType.Bit,1),
					new SqlParameter("@Pv_Partner", SqlDbType.Bit,1),
					new SqlParameter("@Lv_Referral", SqlDbType.Bit,1),
					new SqlParameter("@Lv_Partner", SqlDbType.Bit,1),
                    new SqlParameter("@LastCompletedStage", SqlDbType.Bit,1),
					new SqlParameter("@LastStageComplDate", SqlDbType.Bit,1)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.Pv_Created;
            parameters[2].Value = model.Pv_Leadsource;
            parameters[3].Value = model.Pv_Refcode;
            parameters[4].Value = model.Pv_Loanofficer;
            parameters[5].Value = model.Pv_Branch;
            parameters[6].Value = model.Pv_Progress;
            parameters[7].Value = model.Lv_Ranking;
            parameters[8].Value = model.Lv_Amount;
            parameters[9].Value = model.Lv_Rate;
            parameters[10].Value = model.Lv_Loanofficer;
            parameters[11].Value = model.Lv_Lien;
            parameters[12].Value = model.Lv_Progress;
            parameters[13].Value = model.Lv_Branch;
            parameters[14].Value = model.Lv_Loanprogram;
            parameters[15].Value = model.Lv_Leadsource;
            parameters[16].Value = model.Lv_Refcode;
            parameters[17].Value = model.Lv_Estclose;
            parameters[18].Value = model.Lv_Pointfilename;
            parameters[19].Value = model.Pv_Referral;
            parameters[20].Value = model.Pv_Partner;
            parameters[21].Value = model.Lv_Referral;
            parameters[22].Value = model.Lv_Partner;
            parameters[23].Value = model.LastCompletedStage;
            parameters[24].Value = model.LastStageComplDate;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.UserProspectColumns model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UserProspectColumns set ");
            strSql.Append("Pv_Created=@Pv_Created,");
            strSql.Append("Pv_LeadSource=@Pv_LeadSource,");
            strSql.Append("Pv_RefCode=@Pv_RefCode,");
            strSql.Append("Pv_LoanOfficer=@Pv_LoanOfficer,");
            strSql.Append("Pv_Branch=@Pv_Branch,");
            strSql.Append("Pv_Progress=@Pv_Progress,");
            strSql.Append("Lv_Ranking=@Lv_Ranking,");
            strSql.Append("Lv_Amount=@Lv_Amount,");
            strSql.Append("Lv_Rate=@Lv_Rate,");
            strSql.Append("Lv_LoanOfficer=@Lv_LoanOfficer,");
            strSql.Append("Lv_Lien=@Lv_Lien,");
            strSql.Append("Lv_Progress=@Lv_Progress,");
            strSql.Append("Lv_Branch=@Lv_Branch,");
            strSql.Append("Lv_LoanProgram=@Lv_LoanProgram,");
            strSql.Append("Lv_LeadSource=@Lv_LeadSource,");
            strSql.Append("Lv_RefCode=@Lv_RefCode,");
            strSql.Append("Lv_EstClose=@Lv_EstClose,");
            strSql.Append("Lv_PointFilename=@Lv_PointFilename,");
            strSql.Append("Pv_Referral=@Pv_Referral,");
            strSql.Append("Pv_Partner=@Pv_Partner,");
            strSql.Append("Lv_Referral=@Lv_Referral,");
            strSql.Append("Lv_Partner=@Lv_Partner,");
            strSql.Append("LastCompletedStage=@LastCompletedStage,");
            strSql.Append("LastStageComplDate=@LastStageComplDate");
            strSql.Append(" where UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@Pv_Created", SqlDbType.Bit,1),
					new SqlParameter("@Pv_LeadSource", SqlDbType.Bit,1),
					new SqlParameter("@Pv_RefCode", SqlDbType.Bit,1),
					new SqlParameter("@Pv_LoanOfficer", SqlDbType.Bit,1),
					new SqlParameter("@Pv_Branch", SqlDbType.Bit,1),
					new SqlParameter("@Pv_Progress", SqlDbType.Bit,1),
					new SqlParameter("@Lv_Ranking", SqlDbType.Bit,1),
					new SqlParameter("@Lv_Amount", SqlDbType.Bit,1),
					new SqlParameter("@Lv_Rate", SqlDbType.Bit,1),
					new SqlParameter("@Lv_LoanOfficer", SqlDbType.Bit,1),
					new SqlParameter("@Lv_Lien", SqlDbType.Bit,1),
					new SqlParameter("@Lv_Progress", SqlDbType.Bit,1),
					new SqlParameter("@Lv_Branch", SqlDbType.Bit,1),
					new SqlParameter("@Lv_LoanProgram", SqlDbType.Bit,1),
					new SqlParameter("@Lv_LeadSource", SqlDbType.Bit,1),
					new SqlParameter("@Lv_RefCode", SqlDbType.Bit,1),
					new SqlParameter("@Lv_EstClose", SqlDbType.Bit,1),
					new SqlParameter("@Lv_PointFilename", SqlDbType.Bit,1),
					new SqlParameter("@Pv_Referral", SqlDbType.Bit,1),
					new SqlParameter("@Pv_Partner", SqlDbType.Bit,1),
					new SqlParameter("@Lv_Referral", SqlDbType.Bit,1),
					new SqlParameter("@Lv_Partner", SqlDbType.Bit,1),
                    new SqlParameter("@LastCompletedStage", SqlDbType.Bit,1),
					new SqlParameter("@LastStageComplDate", SqlDbType.Bit,1)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.Pv_Created;
            parameters[2].Value = model.Pv_Leadsource;
            parameters[3].Value = model.Pv_Refcode;
            parameters[4].Value = model.Pv_Loanofficer;
            parameters[5].Value = model.Pv_Branch;
            parameters[6].Value = model.Pv_Progress;
            parameters[7].Value = model.Lv_Ranking;
            parameters[8].Value = model.Lv_Amount;
            parameters[9].Value = model.Lv_Rate;
            parameters[10].Value = model.Lv_Loanofficer;
            parameters[11].Value = model.Lv_Lien;
            parameters[12].Value = model.Lv_Progress;
            parameters[13].Value = model.Lv_Branch;
            parameters[14].Value = model.Lv_Loanprogram;
            parameters[15].Value = model.Lv_Leadsource;
            parameters[16].Value = model.Lv_Refcode;
            parameters[17].Value = model.Lv_Estclose;
            parameters[18].Value = model.Lv_Pointfilename;
            parameters[19].Value = model.Pv_Referral;
            parameters[20].Value = model.Pv_Partner;
            parameters[21].Value = model.Lv_Referral;
            parameters[22].Value = model.Lv_Partner;
            parameters[23].Value = model.LastCompletedStage;
            parameters[24].Value = model.LastStageComplDate;

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
        public bool Delete(int UserId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from UserProspectColumns ");
            strSql.Append(" where UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = UserId;

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
        public bool DeleteList(string UserIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from UserProspectColumns ");
            strSql.Append(" where UserId in (" + UserIdlist + ")  ");
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
        public LPWeb.Model.UserProspectColumns GetModel(int UserId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserId,Pv_Created,Pv_LeadSource,Pv_RefCode,Pv_LoanOfficer,Pv_Branch,Pv_Progress,Lv_Ranking,Lv_Amount,Lv_Rate,Lv_LoanOfficer,Lv_Lien,Lv_Progress,Lv_Branch,Lv_LoanProgram,Lv_LeadSource,Lv_RefCode,Lv_EstClose,Lv_PointFilename,Pv_Referral,Pv_Partner,Lv_Referral,Lv_Partner,LastCompletedStage,LastStageComplDate from UserProspectColumns ");
            strSql.Append(" where UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = UserId;

            LPWeb.Model.UserProspectColumns model = new LPWeb.Model.UserProspectColumns();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Pv_Created"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Pv_Created"].ToString() == "1") || (ds.Tables[0].Rows[0]["Pv_Created"].ToString().ToLower() == "true"))
                    {
                        model.Pv_Created = true;
                    }
                    else
                    {
                        model.Pv_Created = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Pv_LeadSource"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Pv_LeadSource"].ToString() == "1") || (ds.Tables[0].Rows[0]["Pv_LeadSource"].ToString().ToLower() == "true"))
                    {
                        model.Pv_Leadsource = true;
                    }
                    else
                    {
                        model.Pv_Leadsource = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Pv_RefCode"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Pv_RefCode"].ToString() == "1") || (ds.Tables[0].Rows[0]["Pv_RefCode"].ToString().ToLower() == "true"))
                    {
                        model.Pv_Refcode = true;
                    }
                    else
                    {
                        model.Pv_Refcode = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Pv_LoanOfficer"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Pv_LoanOfficer"].ToString() == "1") || (ds.Tables[0].Rows[0]["Pv_LoanOfficer"].ToString().ToLower() == "true"))
                    {
                        model.Pv_Loanofficer = true;
                    }
                    else
                    {
                        model.Pv_Loanofficer = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Pv_Branch"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Pv_Branch"].ToString() == "1") || (ds.Tables[0].Rows[0]["Pv_Branch"].ToString().ToLower() == "true"))
                    {
                        model.Pv_Branch = true;
                    }
                    else
                    {
                        model.Pv_Branch = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Pv_Progress"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Pv_Progress"].ToString() == "1") || (ds.Tables[0].Rows[0]["Pv_Progress"].ToString().ToLower() == "true"))
                    {
                        model.Pv_Progress = true;
                    }
                    else
                    {
                        model.Pv_Progress = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Lv_Ranking"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Lv_Ranking"].ToString() == "1") || (ds.Tables[0].Rows[0]["Lv_Ranking"].ToString().ToLower() == "true"))
                    {
                        model.Lv_Ranking = true;
                    }
                    else
                    {
                        model.Lv_Ranking = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Lv_Amount"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Lv_Amount"].ToString() == "1") || (ds.Tables[0].Rows[0]["Lv_Amount"].ToString().ToLower() == "true"))
                    {
                        model.Lv_Amount = true;
                    }
                    else
                    {
                        model.Lv_Amount = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Lv_Rate"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Lv_Rate"].ToString() == "1") || (ds.Tables[0].Rows[0]["Lv_Rate"].ToString().ToLower() == "true"))
                    {
                        model.Lv_Rate = true;
                    }
                    else
                    {
                        model.Lv_Rate = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Lv_LoanOfficer"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Lv_LoanOfficer"].ToString() == "1") || (ds.Tables[0].Rows[0]["Lv_LoanOfficer"].ToString().ToLower() == "true"))
                    {
                        model.Lv_Loanofficer = true;
                    }
                    else
                    {
                        model.Lv_Loanofficer = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Lv_Lien"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Lv_Lien"].ToString() == "1") || (ds.Tables[0].Rows[0]["Lv_Lien"].ToString().ToLower() == "true"))
                    {
                        model.Lv_Lien = true;
                    }
                    else
                    {
                        model.Lv_Lien = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Lv_Progress"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Lv_Progress"].ToString() == "1") || (ds.Tables[0].Rows[0]["Lv_Progress"].ToString().ToLower() == "true"))
                    {
                        model.Lv_Progress = true;
                    }
                    else
                    {
                        model.Lv_Progress = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Lv_Branch"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Lv_Branch"].ToString() == "1") || (ds.Tables[0].Rows[0]["Lv_Branch"].ToString().ToLower() == "true"))
                    {
                        model.Lv_Branch = true;
                    }
                    else
                    {
                        model.Lv_Branch = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Lv_LoanProgram"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Lv_LoanProgram"].ToString() == "1") || (ds.Tables[0].Rows[0]["Lv_LoanProgram"].ToString().ToLower() == "true"))
                    {
                        model.Lv_Loanprogram = true;
                    }
                    else
                    {
                        model.Lv_Loanprogram = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Lv_LeadSource"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Lv_LeadSource"].ToString() == "1") || (ds.Tables[0].Rows[0]["Lv_LeadSource"].ToString().ToLower() == "true"))
                    {
                        model.Lv_Leadsource = true;
                    }
                    else
                    {
                        model.Lv_Leadsource = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Lv_RefCode"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Lv_RefCode"].ToString() == "1") || (ds.Tables[0].Rows[0]["Lv_RefCode"].ToString().ToLower() == "true"))
                    {
                        model.Lv_Refcode = true;
                    }
                    else
                    {
                        model.Lv_Refcode = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Lv_EstClose"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Lv_EstClose"].ToString() == "1") || (ds.Tables[0].Rows[0]["Lv_EstClose"].ToString().ToLower() == "true"))
                    {
                        model.Lv_Estclose = true;
                    }
                    else
                    {
                        model.Lv_Estclose = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Lv_PointFilename"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Lv_PointFilename"].ToString() == "1") || (ds.Tables[0].Rows[0]["Lv_PointFilename"].ToString().ToLower() == "true"))
                    {
                        model.Lv_Pointfilename = true;
                    }
                    else
                    {
                        model.Lv_Pointfilename = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Pv_Referral"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Pv_Referral"].ToString() == "1") || (ds.Tables[0].Rows[0]["Pv_Referral"].ToString().ToLower() == "true"))
                    {
                        model.Pv_Referral = true;
                    }
                    else
                    {
                        model.Pv_Referral = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Pv_Partner"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Pv_Partner"].ToString() == "1") || (ds.Tables[0].Rows[0]["Pv_Partner"].ToString().ToLower() == "true"))
                    {
                        model.Pv_Partner = true;
                    }
                    else
                    {
                        model.Pv_Partner = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Lv_Referral"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Lv_Referral"].ToString() == "1") || (ds.Tables[0].Rows[0]["Lv_Referral"].ToString().ToLower() == "true"))
                    {
                        model.Lv_Referral = true;
                    }
                    else
                    {
                        model.Lv_Referral = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Lv_Partner"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Lv_Partner"].ToString() == "1") || (ds.Tables[0].Rows[0]["Lv_Partner"].ToString().ToLower() == "true"))
                    {
                        model.Lv_Partner = true;
                    }
                    else
                    {
                        model.Lv_Partner = false;
                    }
                }

                if (ds.Tables[0].Rows[0]["LastCompletedStage"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["LastCompletedStage"].ToString() == "1") || (ds.Tables[0].Rows[0]["LastCompletedStage"].ToString().ToLower() == "true"))
                    {
                        model.LastCompletedStage = true;
                    }
                    else
                    {
                        model.LastCompletedStage = false;
                    }
                }

                if (ds.Tables[0].Rows[0]["LastStageComplDate"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["LastStageComplDate"].ToString() == "1") || (ds.Tables[0].Rows[0]["LastStageComplDate"].ToString().ToLower() == "true"))
                    {
                        model.LastStageComplDate = true;
                    }
                    else
                    {
                        model.LastStageComplDate = false;
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
            strSql.Append("select UserId,Pv_Created,Pv_LeadSource,Pv_RefCode,Pv_LoanOfficer,Pv_Branch,Pv_Progress,Lv_Ranking,Lv_Amount,Lv_Rate,Lv_LoanOfficer,Lv_Lien,Lv_Progress,Lv_Branch,Lv_LoanProgram,Lv_LeadSource,Lv_RefCode,Lv_EstClose,Lv_PointFilename,Pv_Referral,Pv_Partner,Lv_Referral,Lv_Partner,LastCompletedStage,LastStageComplDate ");
            strSql.Append(" FROM UserProspectColumns ");
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
            strSql.Append(" UserId,Pv_Created,Pv_LeadSource,Pv_RefCode,Pv_LoanOfficer,Pv_Branch,Pv_Progress,Lv_Ranking,Lv_Amount,Lv_Rate,Lv_LoanOfficer,Lv_Lien,Lv_Progress,Lv_Branch,Lv_LoanProgram,Lv_LeadSource,Lv_RefCode,Lv_EstClose,Lv_PointFilename,Pv_Referral,Pv_Partner,Lv_Referral,Lv_Partner,LastCompletedStage,LastStageComplDate ");
            strSql.Append(" FROM UserProspectColumns ");
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
            parameters[0].Value = "UserProspectColumns";
            parameters[1].Value = "";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }

        #endregion  Method
    }
}

