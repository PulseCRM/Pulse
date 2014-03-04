using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类UserPipelineColumns。
	/// </summary>
	public class UserPipelineColumnsBase
    {
        public UserPipelineColumnsBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.UserPipelineColumns model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UserPipelineColumns(");
            strSql.Append("UserId,PointFolder,Stage,Branch,EstimatedClose,Alerts,LoanOfficer,Amount,Lien,Rate,Lender,LockExp,PercentCompl,Processor,TaskCount,PointFileName,LastCompletedStage,LastStageComplDate,[Closer],[Shipper],[DocPrep],[Assistant],LoanProgram,Purpose,[JrProcessor],[LastLoanNote])");
            strSql.Append(" values (");
            strSql.Append("@UserId,@PointFolder,@Stage,@Branch,@EstimatedClose,@Alerts,@LoanOfficer,@Amount,@Lien,@Rate,@Lender,@LockExp,@PercentCompl,@Processor,@TaskCount,@PointFileName,@LastCompletedStage,@LastStageComplDate,@Closer,@Shipper,@DocPrep,@Assistant,@LoanProgram,@Purpose,@JrProcessor,@LastLoanNote)");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@PointFolder", SqlDbType.Bit,1),
					new SqlParameter("@Stage", SqlDbType.Bit,1),
					new SqlParameter("@Branch", SqlDbType.Bit,1),
					new SqlParameter("@EstimatedClose", SqlDbType.Bit,1),
					new SqlParameter("@Alerts", SqlDbType.Bit,1),
					new SqlParameter("@LoanOfficer", SqlDbType.Bit,1),
					new SqlParameter("@Amount", SqlDbType.Bit,1),
					new SqlParameter("@Lien", SqlDbType.Bit,1),
					new SqlParameter("@Rate", SqlDbType.Bit,1),
					new SqlParameter("@Lender", SqlDbType.Bit,1),
					new SqlParameter("@LockExp", SqlDbType.Bit,1),
					new SqlParameter("@PercentCompl", SqlDbType.Bit,1),
					new SqlParameter("@Processor", SqlDbType.Bit,1),
					new SqlParameter("@TaskCount", SqlDbType.Bit,1),
					new SqlParameter("@PointFileName", SqlDbType.Bit,1),
					new SqlParameter("@LastCompletedStage", SqlDbType.Bit,1),
					new SqlParameter("@LastStageComplDate", SqlDbType.Bit,1),
                    new SqlParameter("@Closer", SqlDbType.Bit,1),
                    new SqlParameter("@Shipper", SqlDbType.Bit,1),
                    new SqlParameter("@DocPrep", SqlDbType.Bit,1),
                    new SqlParameter("@Assistant", SqlDbType.Bit,1),
                    new SqlParameter("@LoanProgram", SqlDbType.Bit,1),
                    new SqlParameter("@Purpose", SqlDbType.Bit,1),
                    new SqlParameter("@JrProcessor",SqlDbType.Bit,1),
                    new SqlParameter("@LastLoanNote",SqlDbType.Bit,1)
                                        };
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.PointFolder;
            parameters[2].Value = model.Stage;
            parameters[3].Value = model.Branch;
            parameters[4].Value = model.EstimatedClose;
            parameters[5].Value = model.Alerts;
            parameters[6].Value = model.LoanOfficer;
            parameters[7].Value = model.Amount;
            parameters[8].Value = model.Lien;
            parameters[9].Value = model.Rate;
            parameters[10].Value = model.Lender;
            parameters[11].Value = model.LockExp;
            parameters[12].Value = model.PercentCompl;
            parameters[13].Value = model.Processor;
            parameters[14].Value = model.TaskCount;
            parameters[15].Value = model.PointFileName;

            parameters[16].Value = model.LastCompletedStage;
            parameters[17].Value = model.LastStageComplDate;

            parameters[18].Value = model.Closer;
            parameters[19].Value = model.Shipper;
            parameters[20].Value = model.DocPrep;
            parameters[21].Value = model.Assistant;
            parameters[22].Value =model.LoanProgram;
            parameters[23].Value = model.Purpose;
            parameters[24].Value = model.JrProcessor;
            parameters[25].Value = model.LastLoanNote;


            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.UserPipelineColumns model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UserPipelineColumns set ");
            strSql.Append("UserId=@UserId,");
            strSql.Append("PointFolder=@PointFolder,");
            strSql.Append("Stage=@Stage,");
            strSql.Append("Branch=@Branch,");
            strSql.Append("EstimatedClose=@EstimatedClose,");
            strSql.Append("Alerts=@Alerts,");
            strSql.Append("LoanOfficer=@LoanOfficer,");
            strSql.Append("Amount=@Amount,");
            strSql.Append("Lien=@Lien,");
            strSql.Append("Rate=@Rate,");
            strSql.Append("Lender=@Lender,");
            strSql.Append("LockExp=@LockExp,");
            strSql.Append("PercentCompl=@PercentCompl,");
            strSql.Append("Processor=@Processor,");
            strSql.Append("TaskCount=@TaskCount,");
            strSql.Append("PointFileName=@PointFileName,");
            strSql.Append("LastCompletedStage=@LastCompletedStage,");
            strSql.Append("LastStageComplDate=@LastStageComplDate,");//gdc CR40 ,[Closer],[Shipper],[DocPrep],[Assistant]
            strSql.Append("Closer=@Closer,");
            strSql.Append("Shipper=@Shipper,");
            strSql.Append("DocPrep=@DocPrep,");
            strSql.Append("Assistant=@Assistant,");
            strSql.Append("LoanProgram=@LoanProgram,");
            strSql.Append("Purpose=@Purpose,");
            strSql.Append("JrProcessor=@JrProcessor,");
            strSql.Append("LastLoanNote=@LastLoanNote");
            strSql.Append(" where UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@PointFolder", SqlDbType.Bit,1),
					new SqlParameter("@Stage", SqlDbType.Bit,1),
					new SqlParameter("@Branch", SqlDbType.Bit,1),
					new SqlParameter("@EstimatedClose", SqlDbType.Bit,1),
					new SqlParameter("@Alerts", SqlDbType.Bit,1),
					new SqlParameter("@LoanOfficer", SqlDbType.Bit,1),
					new SqlParameter("@Amount", SqlDbType.Bit,1),
					new SqlParameter("@Lien", SqlDbType.Bit,1),
					new SqlParameter("@Rate", SqlDbType.Bit,1),
					new SqlParameter("@Lender", SqlDbType.Bit,1),
					new SqlParameter("@LockExp", SqlDbType.Bit,1),
					new SqlParameter("@PercentCompl", SqlDbType.Bit,1),
					new SqlParameter("@Processor", SqlDbType.Bit,1),
					new SqlParameter("@TaskCount", SqlDbType.Bit,1),
					new SqlParameter("@PointFileName", SqlDbType.Bit,1),
					new SqlParameter("@LastCompletedStage", SqlDbType.Bit,1),
					new SqlParameter("@LastStageComplDate", SqlDbType.Bit,1),
                    new SqlParameter("@Closer", SqlDbType.Bit,1),
                    new SqlParameter("@Shipper", SqlDbType.Bit,1),
                    new SqlParameter("@DocPrep", SqlDbType.Bit,1),
                    new SqlParameter("@Assistant", SqlDbType.Bit,1),
                    new SqlParameter("@LoanProgram", SqlDbType.Bit,1),   
                    new SqlParameter("@Purpose", SqlDbType.Bit,1),
                    new SqlParameter("@JrProcessor",SqlDbType.Bit,1),
                    new SqlParameter("@LastLoanNote",SqlDbType.Bit,1)
                                        };
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.PointFolder;
            parameters[2].Value = model.Stage;
            parameters[3].Value = model.Branch;
            parameters[4].Value = model.EstimatedClose;
            parameters[5].Value = model.Alerts;
            parameters[6].Value = model.LoanOfficer;
            parameters[7].Value = model.Amount;
            parameters[8].Value = model.Lien;
            parameters[9].Value = model.Rate;
            parameters[10].Value = model.Lender;
            parameters[11].Value = model.LockExp;
            parameters[12].Value = model.PercentCompl;
            parameters[13].Value = model.Processor;
            parameters[14].Value = model.TaskCount;
            parameters[15].Value = model.PointFileName;

            parameters[16].Value = model.LastCompletedStage;
            parameters[17].Value = model.LastStageComplDate;

            parameters[18].Value = model.Closer;
            parameters[19].Value = model.Shipper;
            parameters[20].Value = model.DocPrep;
            parameters[21].Value = model.Assistant;

            parameters[22].Value = model.LoanProgram;
            parameters[23].Value = model.Purpose;
            parameters[24].Value = model.JrProcessor;
            parameters[25].Value = model.LastLoanNote;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int UserId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from UserPipelineColumns ");
            strSql.Append(" where UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = UserId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.UserPipelineColumns GetModel(int UserId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserId,PointFolder,Stage,Branch,EstimatedClose,Alerts,LoanOfficer,Amount,Lien,Rate,Lender,LockExp,PercentCompl,Processor,TaskCount,PointFileName,LastCompletedStage,LastStageComplDate,[Closer],[Shipper],[DocPrep],[Assistant],LoanProgram,Purpose,[JrProcessor],[LastLoanNote] from UserPipelineColumns ");
            strSql.Append(" where UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = UserId;

            LPWeb.Model.UserPipelineColumns model = new LPWeb.Model.UserPipelineColumns();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PointFolder"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["PointFolder"].ToString() == "1") || (ds.Tables[0].Rows[0]["PointFolder"].ToString().ToLower() == "true"))
                    {
                        model.PointFolder = true;
                    }
                    else
                    {
                        model.PointFolder = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Stage"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Stage"].ToString() == "1") || (ds.Tables[0].Rows[0]["Stage"].ToString().ToLower() == "true"))
                    {
                        model.Stage = true;
                    }
                    else
                    {
                        model.Stage = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Branch"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Branch"].ToString() == "1") || (ds.Tables[0].Rows[0]["Branch"].ToString().ToLower() == "true"))
                    {
                        model.Branch = true;
                    }
                    else
                    {
                        model.Branch = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["EstimatedClose"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["EstimatedClose"].ToString() == "1") || (ds.Tables[0].Rows[0]["EstimatedClose"].ToString().ToLower() == "true"))
                    {
                        model.EstimatedClose = true;
                    }
                    else
                    {
                        model.EstimatedClose = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Alerts"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Alerts"].ToString() == "1") || (ds.Tables[0].Rows[0]["Alerts"].ToString().ToLower() == "true"))
                    {
                        model.Alerts = true;
                    }
                    else
                    {
                        model.Alerts = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["LoanOfficer"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["LoanOfficer"].ToString() == "1") || (ds.Tables[0].Rows[0]["LoanOfficer"].ToString().ToLower() == "true"))
                    {
                        model.LoanOfficer = true;
                    }
                    else
                    {
                        model.LoanOfficer = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Amount"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Amount"].ToString() == "1") || (ds.Tables[0].Rows[0]["Amount"].ToString().ToLower() == "true"))
                    {
                        model.Amount = true;
                    }
                    else
                    {
                        model.Amount = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Lien"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Lien"].ToString() == "1") || (ds.Tables[0].Rows[0]["Lien"].ToString().ToLower() == "true"))
                    {
                        model.Lien = true;
                    }
                    else
                    {
                        model.Lien = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Rate"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Rate"].ToString() == "1") || (ds.Tables[0].Rows[0]["Rate"].ToString().ToLower() == "true"))
                    {
                        model.Rate = true;
                    }
                    else
                    {
                        model.Rate = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Lender"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Lender"].ToString() == "1") || (ds.Tables[0].Rows[0]["Lender"].ToString().ToLower() == "true"))
                    {
                        model.Lender = true;
                    }
                    else
                    {
                        model.Lender = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["LockExp"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["LockExp"].ToString() == "1") || (ds.Tables[0].Rows[0]["LockExp"].ToString().ToLower() == "true"))
                    {
                        model.LockExp = true;
                    }
                    else
                    {
                        model.LockExp = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["PercentCompl"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["PercentCompl"].ToString() == "1") || (ds.Tables[0].Rows[0]["PercentCompl"].ToString().ToLower() == "true"))
                    {
                        model.PercentCompl = true;
                    }
                    else
                    {
                        model.PercentCompl = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Processor"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Processor"].ToString() == "1") || (ds.Tables[0].Rows[0]["Processor"].ToString().ToLower() == "true"))
                    {
                        model.Processor = true;
                    }
                    else
                    {
                        model.Processor = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["TaskCount"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["TaskCount"].ToString() == "1") || (ds.Tables[0].Rows[0]["TaskCount"].ToString().ToLower() == "true"))
                    {
                        model.TaskCount = true;
                    }
                    else
                    {
                        model.TaskCount = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["PointFileName"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["PointFileName"].ToString() == "1") || (ds.Tables[0].Rows[0]["PointFileName"].ToString().ToLower() == "true"))
                    {
                        model.PointFileName = true;
                    }
                    else
                    {
                        model.PointFileName = false;
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

                //gdc CR40   ,[Closer],[Shipper],[DocPrep],[Assistant]
                if (ds.Tables[0].Rows[0]["Closer"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Closer"].ToString() == "1") || (ds.Tables[0].Rows[0]["Closer"].ToString().ToLower() == "true"))
                    {
                        model.Closer = true;
                    }
                    else
                    {
                        model.Closer = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Shipper"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Shipper"].ToString() == "1") || (ds.Tables[0].Rows[0]["Shipper"].ToString().ToLower() == "true"))
                    {
                        model.Shipper = true;
                    }
                    else
                    {
                        model.Shipper = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["DocPrep"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["DocPrep"].ToString() == "1") || (ds.Tables[0].Rows[0]["DocPrep"].ToString().ToLower() == "true"))
                    {
                        model.DocPrep = true;
                    }
                    else
                    {
                        model.DocPrep = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Assistant"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Assistant"].ToString() == "1") || (ds.Tables[0].Rows[0]["Assistant"].ToString().ToLower() == "true"))
                    {
                        model.Assistant = true;
                    }
                    else
                    {
                        model.Assistant = false;
                    }
                }

                //ShowTasksInLSR
                model.LoanProgram = false;
                if (ds.Tables[0].Rows[0]["LoanProgram"] != DBNull.Value && ds.Tables[0].Rows[0]["LoanProgram"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["LoanProgram"].ToString() == "1") || (ds.Tables[0].Rows[0]["LoanProgram"].ToString().ToLower() == "true"))
                    {
                        model.LoanProgram = true;
                    }
                    else
                    {
                        model.LoanProgram = false;
                    }
                }

                model.Purpose = false;
                if (ds.Tables[0].Rows[0]["Purpose"] != DBNull.Value && ds.Tables[0].Rows[0]["Purpose"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Purpose"].ToString() == "1") || (ds.Tables[0].Rows[0]["Purpose"].ToString().ToLower() == "true"))
                    {
                        model.Purpose = true;
                    }
                    else
                    {
                        model.Purpose = false;
                    }
                }

                model.JrProcessor = false;
                if (ds.Tables[0].Rows[0]["JrProcessor"] != DBNull.Value && ds.Tables[0].Rows[0]["JrProcessor"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["JrProcessor"].ToString() == "1") || (ds.Tables[0].Rows[0]["JrProcessor"].ToString().ToLower() == "true"))
                    {
                        model.JrProcessor = true;
                    }
                    else
                    {
                        model.JrProcessor = false;
                    }
                }

                model.LastLoanNote = false;
                if (ds.Tables[0].Rows[0]["LastLoanNote"] != DBNull.Value && ds.Tables[0].Rows[0]["LastLoanNote"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["LastLoanNote"].ToString() == "1") || (ds.Tables[0].Rows[0]["LastLoanNote"].ToString().ToLower() == "true"))
                    {
                        model.LastLoanNote = true;
                    }
                    else
                    {
                        model.LastLoanNote = false;
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
            strSql.Append("select UserId,PointFolder,Stage,Branch,EstimatedClose,Alerts,LoanOfficer,Amount,Lien,Rate,Lender,LockExp,PercentCompl,Processor,TaskCount,PointFileName,LastCompletedStage,LastStageComplDate ");
            strSql.Append(" FROM UserPipelineColumns ");
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
            strSql.Append(" UserId,PointFolder,Stage,Branch,EstimatedClose,Alerts,LoanOfficer,Amount,Lien,Rate,Lender,LockExp,PercentCompl,Processor,TaskCount,PointFileName,LastCompletedStage,LastStageComplDate ");
            strSql.Append(" FROM UserPipelineColumns ");
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
            parameters[0].Value = "UserPipelineColumns";
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

