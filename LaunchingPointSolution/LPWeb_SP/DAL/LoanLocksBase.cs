using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:LoanLocks
    /// </summary>
    public class LoanLocksBase
    {
        public LoanLocksBase()
        { }
        #region  Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("FileId", "LoanLocks");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int FileId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from LoanLocks");
            strSql.Append(" where FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4)};
            parameters[0].Value = FileId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.LoanLocks model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into LoanLocks(");
            strSql.Append("FileId,LockOption,LockedBy,LockTime,LockTerm,ConfirmedBy,ConfirmTime,LockExpirationDate,Ext1Term,Ext1LockExpDate,Ext1LockTime,Ext1LockedBy,Ext1ConfirmTime,Ext2Term,Ext2LockExpDate,Ext2LockTime,Ext2LockedBy,Ext2ConfirmTime,Ext3Term,Ext3LockExpDate,Ext3LockTime,Ext3LockedBy,Ext3ConfirmTime,InvestorID,Investor,ProgramID,Program)");
            strSql.Append(" values (");
            strSql.Append("@FileId,@LockOption,@LockedBy,@LockTime,@LockTerm,@ConfirmedBy,@ConfirmTime,@LockExpirationDate,@Ext1Term,@Ext1LockExpDate,@Ext1LockTime,@Ext1LockedBy,@Ext1ConfirmTime,@Ext2Term,@Ext2LockExpDate,@Ext2LockTime,@Ext2LockedBy,@Ext2ConfirmTime,@Ext3Term,@Ext3LockExpDate,@Ext3LockTime,@Ext3LockedBy,@Ext3ConfirmTime,@InvestorID,@Investor,@ProgramID,@Program)");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@LockOption", SqlDbType.NVarChar,50),
					new SqlParameter("@LockedBy", SqlDbType.NVarChar,255),
					new SqlParameter("@LockTime", SqlDbType.DateTime),
					new SqlParameter("@LockTerm", SqlDbType.SmallInt,2),
					new SqlParameter("@ConfirmedBy", SqlDbType.NVarChar,255),
					new SqlParameter("@ConfirmTime", SqlDbType.DateTime),
					new SqlParameter("@LockExpirationDate", SqlDbType.Date,3),
					new SqlParameter("@Ext1Term", SqlDbType.SmallInt,2),
					new SqlParameter("@Ext1LockExpDate", SqlDbType.Date,3),
					new SqlParameter("@Ext1LockTime", SqlDbType.DateTime),
					new SqlParameter("@Ext1LockedBy", SqlDbType.NVarChar,255),
					new SqlParameter("@Ext1ConfirmTime", SqlDbType.DateTime),
					new SqlParameter("@Ext2Term", SqlDbType.SmallInt,2),
					new SqlParameter("@Ext2LockExpDate", SqlDbType.Date,3),
					new SqlParameter("@Ext2LockTime", SqlDbType.DateTime),
					new SqlParameter("@Ext2LockedBy", SqlDbType.NVarChar,255),
					new SqlParameter("@Ext2ConfirmTime", SqlDbType.DateTime),
					new SqlParameter("@Ext3Term", SqlDbType.SmallInt,2),
					new SqlParameter("@Ext3LockExpDate", SqlDbType.Date,3),
					new SqlParameter("@Ext3LockTime", SqlDbType.DateTime),
					new SqlParameter("@Ext3LockedBy", SqlDbType.NVarChar,255),
					new SqlParameter("@Ext3ConfirmTime", SqlDbType.DateTime),
					new SqlParameter("@InvestorID", SqlDbType.Int,4),
					new SqlParameter("@Investor", SqlDbType.NVarChar,255),
					new SqlParameter("@ProgramID", SqlDbType.Int,4),
					new SqlParameter("@Program", SqlDbType.NVarChar,255)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.LockOption;
            parameters[2].Value = model.LockedBy;
            parameters[3].Value = model.LockTime;
            parameters[4].Value = model.LockTerm;
            parameters[5].Value = model.ConfirmedBy;
            parameters[6].Value = model.ConfirmTime;
            parameters[7].Value = model.LockExpirationDate;
            parameters[8].Value = model.Ext1Term;
            parameters[9].Value = model.Ext1LockExpDate;
            parameters[10].Value = model.Ext1LockTime;
            parameters[11].Value = model.Ext1LockedBy;
            parameters[12].Value = model.Ext1ConfirmTime;
            parameters[13].Value = model.Ext2Term;
            parameters[14].Value = model.Ext2LockExpDate;
            parameters[15].Value = model.Ext2LockTime;
            parameters[16].Value = model.Ext2LockedBy;
            parameters[17].Value = model.Ext2ConfirmTime;
            parameters[18].Value = model.Ext3Term;
            parameters[19].Value = model.Ext3LockExpDate;
            parameters[20].Value = model.Ext3LockTime;
            parameters[21].Value = model.Ext3LockedBy;
            parameters[22].Value = model.Ext3ConfirmTime;
            parameters[23].Value = model.InvestorID;
            parameters[24].Value = model.Investor;
            parameters[25].Value = model.ProgramID;
            parameters[26].Value = model.Program;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.LoanLocks model)
        {
            if (model == null || model.FileId <= 0)
                return false;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update LoanLocks set ");
            strSql.Append("LockOption=@LockOption,");
            strSql.Append("LockedBy=@LockedBy,");
            strSql.Append("LockTime=@LockTime,");
            strSql.Append("LockTerm=@LockTerm,");
            strSql.Append("ConfirmedBy=@ConfirmedBy,");
            strSql.Append("ConfirmTime=@ConfirmTime,");
            strSql.Append("LockExpirationDate=@LockExpirationDate,");
            strSql.Append("Ext1Term=@Ext1Term,");
            strSql.Append("Ext1LockExpDate=@Ext1LockExpDate,");
            strSql.Append("Ext1LockTime=@Ext1LockTime,");
            strSql.Append("Ext1LockedBy=@Ext1LockedBy,");
            strSql.Append("Ext1ConfirmTime=@Ext1ConfirmTime,");
            strSql.Append("Ext2Term=@Ext2Term,");
            strSql.Append("Ext2LockExpDate=@Ext2LockExpDate,");
            strSql.Append("Ext2LockTime=@Ext2LockTime,");
            strSql.Append("Ext2LockedBy=@Ext2LockedBy,");
            strSql.Append("Ext2ConfirmTime=@Ext2ConfirmTime,");
            strSql.Append("Ext3Term=@Ext3Term,");
            strSql.Append("Ext3LockExpDate=@Ext3LockExpDate,");
            strSql.Append("Ext3LockTime=@Ext3LockTime,");
            strSql.Append("Ext3LockedBy=@Ext3LockedBy,");
            strSql.Append("Ext3ConfirmTime=@Ext3ConfirmTime,");
            strSql.Append("InvestorID=@InvestorID,");
            strSql.Append("Investor=@Investor,");
            strSql.Append("ProgramID=@ProgramID,");
            strSql.Append("Program=@Program ");
            strSql.Append(" where FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@LockOption", SqlDbType.NVarChar,50),
					new SqlParameter("@LockedBy", SqlDbType.NVarChar,255),
					new SqlParameter("@LockTime", SqlDbType.DateTime),
					new SqlParameter("@LockTerm", SqlDbType.SmallInt,2),
					new SqlParameter("@ConfirmedBy", SqlDbType.NVarChar,255),
					new SqlParameter("@ConfirmTime", SqlDbType.DateTime),
					new SqlParameter("@LockExpirationDate", SqlDbType.Date,3),
					new SqlParameter("@Ext1Term", SqlDbType.SmallInt,2),
					new SqlParameter("@Ext1LockExpDate", SqlDbType.Date,3),
					new SqlParameter("@Ext1LockTime", SqlDbType.DateTime),
					new SqlParameter("@Ext1LockedBy", SqlDbType.NVarChar,255),
					new SqlParameter("@Ext1ConfirmTime", SqlDbType.DateTime),
					new SqlParameter("@Ext2Term", SqlDbType.SmallInt,2),
					new SqlParameter("@Ext2LockExpDate", SqlDbType.Date,3),
					new SqlParameter("@Ext2LockTime", SqlDbType.DateTime),
					new SqlParameter("@Ext2LockedBy", SqlDbType.NVarChar,255),
					new SqlParameter("@Ext2ConfirmTime", SqlDbType.DateTime),
					new SqlParameter("@Ext3Term", SqlDbType.SmallInt,2),
					new SqlParameter("@Ext3LockExpDate", SqlDbType.Date,3),
					new SqlParameter("@Ext3LockTime", SqlDbType.DateTime),
					new SqlParameter("@Ext3LockedBy", SqlDbType.NVarChar,255),
					new SqlParameter("@Ext3ConfirmTime", SqlDbType.DateTime),
					new SqlParameter("@InvestorID", SqlDbType.Int,4),
					new SqlParameter("@Investor", SqlDbType.NVarChar,255),
					new SqlParameter("@ProgramID", SqlDbType.Int,4),
					new SqlParameter("@Program", SqlDbType.NVarChar,255)};
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.LockOption;
            parameters[2].Value = model.LockedBy;
            parameters[3].Value = model.LockTime;
            parameters[4].Value = model.LockTerm;
            parameters[5].Value = model.ConfirmedBy;
            parameters[6].Value = model.ConfirmTime;
            parameters[7].Value = model.LockExpirationDate;
            parameters[8].Value = model.Ext1Term;
            parameters[9].Value = model.Ext1LockExpDate;
            parameters[10].Value = model.Ext1LockTime;
            parameters[11].Value = model.Ext1LockedBy;
            parameters[12].Value = model.Ext1ConfirmTime;
            parameters[13].Value = model.Ext2Term;
            parameters[14].Value = model.Ext2LockExpDate;
            parameters[15].Value = model.Ext2LockTime;
            parameters[16].Value = model.Ext2LockedBy;
            parameters[17].Value = model.Ext2ConfirmTime;
            parameters[18].Value = model.Ext3Term;
            parameters[19].Value = model.Ext3LockExpDate;
            parameters[20].Value = model.Ext3LockTime;
            parameters[21].Value = model.Ext3LockedBy;
            parameters[22].Value = model.Ext3ConfirmTime;
            if (model.InvestorID <= 0)
                parameters[23].Value = DBNull.Value;
            else
                parameters[23].Value = model.InvestorID;
            parameters[24].Value = model.Investor;
            if (model.ProgramID <= 0)
                parameters[25].Value = DBNull.Value;
            else
                parameters[25].Value = model.ProgramID;
            parameters[26].Value = model.Program;

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
        public bool Delete(int FileId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LoanLocks ");
            strSql.Append(" where FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4)};
            parameters[0].Value = FileId;

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
        public bool DeleteList(string FileIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from LoanLocks ");
            strSql.Append(" where FileId in (" + FileIdlist + ")  ");
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
        public LPWeb.Model.LoanLocks GetModel(int FileId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 FileId,LockOption,LockedBy,LockTime,LockTerm,ConfirmedBy,ConfirmTime,LockExpirationDate,Ext1Term,Ext1LockExpDate,Ext1LockTime,Ext1LockedBy,Ext1ConfirmTime,Ext2Term,Ext2LockExpDate,Ext2LockTime,Ext2LockedBy,Ext2ConfirmTime,Ext3Term,Ext3LockExpDate,Ext3LockTime,Ext3LockedBy,Ext3ConfirmTime,InvestorID,Investor,ProgramID,Program from LoanLocks ");
            strSql.Append(" where FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4)};
            parameters[0].Value = FileId;

            LPWeb.Model.LoanLocks model = new LPWeb.Model.LoanLocks();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["FileId"].ToString() != "")
                {
                    model.FileId = int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
                }
                model.LockOption = ds.Tables[0].Rows[0]["LockOption"].ToString();
                model.LockedBy = ds.Tables[0].Rows[0]["LockedBy"].ToString();
                if (ds.Tables[0].Rows[0]["LockTime"].ToString() != "")
                {
                    model.LockTime = DateTime.Parse(ds.Tables[0].Rows[0]["LockTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LockTerm"].ToString() != "")
                {
                    model.LockTerm = int.Parse(ds.Tables[0].Rows[0]["LockTerm"].ToString());
                }
                model.ConfirmedBy = ds.Tables[0].Rows[0]["ConfirmedBy"].ToString();
                if (ds.Tables[0].Rows[0]["ConfirmTime"].ToString() != "")
                {
                    model.ConfirmTime = DateTime.Parse(ds.Tables[0].Rows[0]["ConfirmTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LockExpirationDate"].ToString() != "")
                {
                    model.LockExpirationDate = DateTime.Parse(ds.Tables[0].Rows[0]["LockExpirationDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Ext1Term"].ToString() != "")
                {
                    model.Ext1Term = int.Parse(ds.Tables[0].Rows[0]["Ext1Term"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Ext1LockExpDate"].ToString() != "")
                {
                    model.Ext1LockExpDate = DateTime.Parse(ds.Tables[0].Rows[0]["Ext1LockExpDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Ext1LockTime"].ToString() != "")
                {
                    model.Ext1LockTime = DateTime.Parse(ds.Tables[0].Rows[0]["Ext1LockTime"].ToString());
                }
                model.Ext1LockedBy = ds.Tables[0].Rows[0]["Ext1LockedBy"].ToString();
                if (ds.Tables[0].Rows[0]["Ext1ConfirmTime"].ToString() != "")
                {
                    model.Ext1ConfirmTime = DateTime.Parse(ds.Tables[0].Rows[0]["Ext1ConfirmTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Ext2Term"].ToString() != "")
                {
                    model.Ext2Term = int.Parse(ds.Tables[0].Rows[0]["Ext2Term"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Ext2LockExpDate"].ToString() != "")
                {
                    model.Ext2LockExpDate = DateTime.Parse(ds.Tables[0].Rows[0]["Ext2LockExpDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Ext2LockTime"].ToString() != "")
                {
                    model.Ext2LockTime = DateTime.Parse(ds.Tables[0].Rows[0]["Ext2LockTime"].ToString());
                }
                model.Ext2LockedBy = ds.Tables[0].Rows[0]["Ext2LockedBy"].ToString();
                if (ds.Tables[0].Rows[0]["Ext2ConfirmTime"].ToString() != "")
                {
                    model.Ext2ConfirmTime = DateTime.Parse(ds.Tables[0].Rows[0]["Ext2ConfirmTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Ext3Term"].ToString() != "")
                {
                    model.Ext3Term = int.Parse(ds.Tables[0].Rows[0]["Ext3Term"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Ext3LockExpDate"].ToString() != "")
                {
                    model.Ext3LockExpDate = DateTime.Parse(ds.Tables[0].Rows[0]["Ext3LockExpDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Ext3LockTime"].ToString() != "")
                {
                    model.Ext3LockTime = DateTime.Parse(ds.Tables[0].Rows[0]["Ext3LockTime"].ToString());
                }
                model.Ext3LockedBy = ds.Tables[0].Rows[0]["Ext3LockedBy"].ToString();
                if (ds.Tables[0].Rows[0]["Ext3ConfirmTime"].ToString() != "")
                {
                    model.Ext3ConfirmTime = DateTime.Parse(ds.Tables[0].Rows[0]["Ext3ConfirmTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["InvestorID"].ToString() != "")
                {
                    model.InvestorID = int.Parse(ds.Tables[0].Rows[0]["InvestorID"].ToString());
                }
                model.Investor = ds.Tables[0].Rows[0]["Investor"].ToString();
                if (ds.Tables[0].Rows[0]["ProgramID"].ToString() != "")
                {
                    model.ProgramID = int.Parse(ds.Tables[0].Rows[0]["ProgramID"].ToString());
                }
                model.Program = ds.Tables[0].Rows[0]["Program"].ToString();
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
            strSql.Append("select FileId,LockOption,LockedBy,LockTime,LockTerm,ConfirmedBy,ConfirmTime,LockExpirationDate,Ext1Term,Ext1LockExpDate,Ext1LockTime,Ext1LockedBy,Ext1ConfirmTime,Ext2Term,Ext2LockExpDate,Ext2LockTime,Ext2LockedBy,Ext2ConfirmTime,Ext3Term,Ext3LockExpDate,Ext3LockTime,Ext3LockedBy,Ext3ConfirmTime,InvestorID,Investor,ProgramID,Program ");
            strSql.Append(" FROM LoanLocks ");
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
            strSql.Append(" FileId,LockOption,LockedBy,LockTime,LockTerm,ConfirmedBy,ConfirmTime,LockExpirationDate,Ext1Term,Ext1LockExpDate,Ext1LockTime,Ext1LockedBy,Ext1ConfirmTime,Ext2Term,Ext2LockExpDate,Ext2LockTime,Ext2LockedBy,Ext2ConfirmTime,Ext3Term,Ext3LockExpDate,Ext3LockTime,Ext3LockedBy,Ext3ConfirmTime,InvestorID,Investor,ProgramID,Program ");
            strSql.Append(" FROM LoanLocks ");
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
            parameters[0].Value = "LoanLocks";
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

