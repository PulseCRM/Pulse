using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Users。
    /// </summary>
    public class UsersBase
    {
        public UsersBase()
        { }

        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Users model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Users(");
            strSql.Append("UserEnabled,Username,EmailAddress,FirstName,LastName,RoleId,Password,LoansPerPage,UserPictureFile,Phone,Fax,Cell,Signature,GlobalId,MarketingAcctEnabled,LicenseNumber,Leadstar_ID,NMLS,ShowTasksInLSR,RemindTaskDue,TaskReminder,SortTaskPickList,ExchangePassword)");
            strSql.Append(" values (");
            strSql.Append("@UserEnabled,@Username,@EmailAddress,@FirstName,@LastName,@RoleId,@Password,@LoansPerPage,@UserPictureFile,@Phone,@Fax,@Cell,@Signature,@GlobalId,@MarketingAcctEnabled,@LicenseNumber,@Leadstar_ID,@NMLS,@ShowTasksInLSR,@RemindTaskDue,@TaskReminder,@SortTaskPickList,@ExchangePassword)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UserEnabled", SqlDbType.Bit,1),
					new SqlParameter("@Username", SqlDbType.NVarChar,50),
					new SqlParameter("@EmailAddress", SqlDbType.NVarChar,255),
					new SqlParameter("@FirstName", SqlDbType.NVarChar,50),
					new SqlParameter("@LastName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoleId", SqlDbType.Int,4),
					new SqlParameter("@Password", SqlDbType.NVarChar,50),
					new SqlParameter("@LoansPerPage", SqlDbType.SmallInt,2),
					new SqlParameter("@UserPictureFile", SqlDbType.VarBinary),
					new SqlParameter("@Phone", SqlDbType.NVarChar,20),
					new SqlParameter("@Fax", SqlDbType.NVarChar,20),
					new SqlParameter("@Cell", SqlDbType.NVarChar,20),
					new SqlParameter("@Signature", SqlDbType.NVarChar),
					new SqlParameter("@GlobalId", SqlDbType.NVarChar,255),
					new SqlParameter("@MarketingAcctEnabled", SqlDbType.Bit,1),
					new SqlParameter("@LicenseNumber", SqlDbType.NVarChar,255),
					new SqlParameter("@Leadstar_ID", SqlDbType.NVarChar,255),
					new SqlParameter("@NMLS", SqlDbType.NVarChar,255),
					new SqlParameter("@ShowTasksInLSR", SqlDbType.Bit,1),
					new SqlParameter("@RemindTaskDue", SqlDbType.Bit,1),
					new SqlParameter("@TaskReminder", SqlDbType.Int,4),
					new SqlParameter("@SortTaskPickList", SqlDbType.NVarChar,1),
					new SqlParameter("@ExchangePassword", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.UserEnabled;
            parameters[1].Value = model.Username;
            parameters[2].Value = model.EmailAddress;
            parameters[3].Value = model.FirstName;
            parameters[4].Value = model.LastName;
            parameters[5].Value = model.RoleId;
            parameters[6].Value = model.Password;
            parameters[7].Value = model.LoansPerPage;
            parameters[8].Value = model.UserPictureFile;
            parameters[9].Value = model.Phone;
            parameters[10].Value = model.Fax;
            parameters[11].Value = model.Cell;
            parameters[12].Value = model.Signature;
            parameters[13].Value = model.GlobalId;
            parameters[14].Value = model.MarketingAcctEnabled;
            parameters[15].Value = model.LicenseNumber;
            parameters[16].Value = model.Leadstar_ID;
            parameters[17].Value = model.NMLS;
            parameters[18].Value = model.ShowTasksInLSR;
            parameters[19].Value = model.RemindTaskDue;
            parameters[20].Value = model.TaskReminder;
            parameters[21].Value = model.SortTaskPickList;
            parameters[22].Value = model.ExchangePassword;

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
        public void Update(LPWeb.Model.Users model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Users set ");
            strSql.Append("UserEnabled=@UserEnabled,");
            strSql.Append("Username=@Username,");
            strSql.Append("EmailAddress=@EmailAddress,");
            strSql.Append("FirstName=@FirstName,");
            strSql.Append("LastName=@LastName,");
            strSql.Append("RoleId=@RoleId,");
            strSql.Append("Password=@Password,");
            strSql.Append("LoansPerPage=@LoansPerPage,");
            strSql.Append("UserPictureFile=@UserPictureFile,");
            strSql.Append("Phone=@Phone,");
            strSql.Append("Fax=@Fax,");
            strSql.Append("Cell=@Cell,");
            strSql.Append("Signature=@Signature,");
            strSql.Append("GlobalId=@GlobalId,");
            strSql.Append("MarketingAcctEnabled=@MarketingAcctEnabled,");
            strSql.Append("LicenseNumber=@LicenseNumber,");
            strSql.Append("Leadstar_ID=@Leadstar_ID,");
            strSql.Append("NMLS=@NMLS,");
            strSql.Append("ShowTasksInLSR=@ShowTasksInLSR,");
            strSql.Append("RemindTaskDue=@RemindTaskDue,");
            strSql.Append("TaskReminder=@TaskReminder,");
            strSql.Append("SortTaskPickList=@SortTaskPickList,");
            strSql.Append("ExchangePassword=@ExchangePassword");
            strSql.Append(" where UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserEnabled", SqlDbType.Bit,1),
					new SqlParameter("@Username", SqlDbType.NVarChar,50),
					new SqlParameter("@EmailAddress", SqlDbType.NVarChar,255),
					new SqlParameter("@FirstName", SqlDbType.NVarChar,50),
					new SqlParameter("@LastName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoleId", SqlDbType.Int,4),
					new SqlParameter("@Password", SqlDbType.NVarChar,50),
					new SqlParameter("@LoansPerPage", SqlDbType.SmallInt,2),
					new SqlParameter("@UserPictureFile", SqlDbType.VarBinary),
					new SqlParameter("@Phone", SqlDbType.NVarChar,20),
					new SqlParameter("@Fax", SqlDbType.NVarChar,20),
					new SqlParameter("@Cell", SqlDbType.NVarChar,20),
					new SqlParameter("@Signature", SqlDbType.NVarChar),
					new SqlParameter("@GlobalId", SqlDbType.NVarChar,255),
					new SqlParameter("@MarketingAcctEnabled", SqlDbType.Bit,1),
					new SqlParameter("@LicenseNumber", SqlDbType.NVarChar,255),
					new SqlParameter("@Leadstar_ID", SqlDbType.NVarChar,255),
					new SqlParameter("@NMLS", SqlDbType.NVarChar,255),
					new SqlParameter("@ShowTasksInLSR", SqlDbType.Bit,1),
					new SqlParameter("@RemindTaskDue", SqlDbType.Bit,1),
					new SqlParameter("@TaskReminder", SqlDbType.Int,4),
					new SqlParameter("@SortTaskPickList", SqlDbType.NVarChar,1),
					new SqlParameter("@ExchangePassword", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.UserEnabled;
            parameters[2].Value = model.Username;
            parameters[3].Value = model.EmailAddress;
            parameters[4].Value = model.FirstName;
            parameters[5].Value = model.LastName;
            parameters[6].Value = model.RoleId;
            parameters[7].Value = model.Password;
            parameters[8].Value = model.LoansPerPage;
            parameters[9].Value = model.UserPictureFile;
            parameters[10].Value = model.Phone;
            parameters[11].Value = model.Fax;
            parameters[12].Value = model.Cell;
            parameters[13].Value = model.Signature;
            parameters[14].Value = model.GlobalId;
            parameters[15].Value = model.MarketingAcctEnabled;
            parameters[16].Value = model.LicenseNumber;
            parameters[17].Value = model.Leadstar_ID;
            parameters[18].Value = model.NMLS;
            parameters[19].Value = model.ShowTasksInLSR;
            parameters[20].Value = model.RemindTaskDue;
            parameters[21].Value = model.TaskReminder;
            parameters[22].Value = model.SortTaskPickList;
            parameters[23].Value = model.ExchangePassword;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int UserId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Users ");
            strSql.Append(" where UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = UserId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Users GetModel(int UserId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserId,UserEnabled,Username,EmailAddress,UserPictureFile,FirstName,LastName,RoleId,Password,LoansPerPage,Signature,MarketingAcctEnabled,Phone,Cell,Fax,LicenseNumber,NMLS,ShowTasksInLSR,RemindTaskDue,TaskReminder,SortTaskPickList,[LOComp],[BranchMgrComp],[DivisionMgrComp],[RegionMgrComp],[ExchangePassword] from Users ");
            strSql.Append(" where UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = UserId;

            LPWeb.Model.Users model = new LPWeb.Model.Users();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UserEnabled"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["UserEnabled"].ToString() == "1") || (ds.Tables[0].Rows[0]["UserEnabled"].ToString().ToLower() == "true"))
                    {
                        model.UserEnabled = true;
                    }
                    else
                    {
                        model.UserEnabled = false;
                    }
                }
                model.Username = ds.Tables[0].Rows[0]["Username"].ToString();
                model.EmailAddress = ds.Tables[0].Rows[0]["EmailAddress"].ToString();
                model.UserPictureFile = DBNull.Value == ds.Tables[0].Rows[0]["UserPictureFile"] ? null : (byte[])ds.Tables[0].Rows[0]["UserPictureFile"];
                model.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                model.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                if (ds.Tables[0].Rows[0]["RoleId"].ToString() != "")
                {
                    model.RoleId = int.Parse(ds.Tables[0].Rows[0]["RoleId"].ToString());
                }
                model.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                if (ds.Tables[0].Rows[0]["LoansPerPage"].ToString() != "")
                {
                    model.LoansPerPage = int.Parse(ds.Tables[0].Rows[0]["LoansPerPage"].ToString());
                }
                model.Signature = ds.Tables[0].Rows[0]["Signature"].ToString();
                if (ds.Tables[0].Rows[0]["MarketingAcctEnabled"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["MarketingAcctEnabled"].ToString() == "1") || (ds.Tables[0].Rows[0]["MarketingAcctEnabled"].ToString().ToLower() == "true"))
                    {
                        model.MarketingAcctEnabled = true;
                    }
                    else
                    {
                        model.MarketingAcctEnabled = false;
                    }
                }

                model.Cell = ds.Tables[0].Rows[0]["Cell"].ToString();
                model.Phone = ds.Tables[0].Rows[0]["Phone"].ToString();
                model.Fax = ds.Tables[0].Rows[0]["Fax"].ToString();
                model.LicenseNumber = ds.Tables[0].Rows[0]["LicenseNumber"].ToString();

                model.NMLS = ds.Tables[0].Rows[0]["NMLS"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["NMLS"].ToString();

                //gdc CR47 ShowTasksInLSR
                model.ShowTasksInLSR = (ds.Tables[0].Rows[0]["ShowTasksInLSR"] == DBNull.Value || ds.Tables[0].Rows[0]["ShowTasksInLSR"].ToString() == "") ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowTasksInLSR"].ToString());

                // CR48
                if (ds.Tables[0].Rows[0]["RemindTaskDue"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["RemindTaskDue"].ToString() == "1") || (ds.Tables[0].Rows[0]["RemindTaskDue"].ToString().ToLower() == "true"))
                    {
                        model.RemindTaskDue = true;
                    }
                    else
                    {
                        model.RemindTaskDue = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["TaskReminder"].ToString() != "")
                {
                    model.TaskReminder = int.Parse(ds.Tables[0].Rows[0]["TaskReminder"].ToString());
                }
                model.SortTaskPickList = ds.Tables[0].Rows[0]["SortTaskPickList"].ToString();

                //,[LOComp],[BranchMgrComp],[DivisionMgrComp],[RegionMgrComp]

                model.LOComp = ds.Tables[0].Rows[0]["LOComp"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["LOComp"]);
                model.BranchMgrComp = ds.Tables[0].Rows[0]["BranchMgrComp"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["BranchMgrComp"]);
                model.DivisionMgrComp = ds.Tables[0].Rows[0]["DivisionMgrComp"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["DivisionMgrComp"]);
                model.RegionMgrComp = ds.Tables[0].Rows[0]["RegionMgrComp"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["RegionMgrComp"]);

                model.ExchangePassword = ds.Tables[0].Rows[0]["ExchangePassword"].ToString();

                return model;
            }
            else
            {
                return null;
            }
        }

        public LPWeb.Model.Users GetModel_WithoutPicture(int UserId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserId,UserEnabled,Username,EmailAddress,FirstName,LastName,RoleId,Password,LoansPerPage,MarketingAcctEnabled,Phone,Cell,Fax,LicenseNumber,NMLS,ShowTasksInLSR,RemindTaskDue,TaskReminder,SortTaskPickList,[LOComp],[BranchMgrComp],[DivisionMgrComp],[RegionMgrComp],[ExchangePassword] from Users ");
            strSql.Append(" where UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = UserId;

            LPWeb.Model.Users model = new LPWeb.Model.Users();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UserEnabled"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["UserEnabled"].ToString() == "1") || (ds.Tables[0].Rows[0]["UserEnabled"].ToString().ToLower() == "true"))
                    {
                        model.UserEnabled = true;
                    }
                    else
                    {
                        model.UserEnabled = false;
                    }
                }
                model.Username = ds.Tables[0].Rows[0]["Username"].ToString();
                model.EmailAddress = ds.Tables[0].Rows[0]["EmailAddress"].ToString();
                model.UserPictureFile = null;
                model.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                model.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                if (ds.Tables[0].Rows[0]["RoleId"].ToString() != "")
                {
                    model.RoleId = int.Parse(ds.Tables[0].Rows[0]["RoleId"].ToString());
                }
                model.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                if (ds.Tables[0].Rows[0]["LoansPerPage"].ToString() != "")
                {
                    model.LoansPerPage = int.Parse(ds.Tables[0].Rows[0]["LoansPerPage"].ToString());
                }
                model.Signature = string.Empty;
                if (ds.Tables[0].Rows[0]["MarketingAcctEnabled"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["MarketingAcctEnabled"].ToString() == "1") || (ds.Tables[0].Rows[0]["MarketingAcctEnabled"].ToString().ToLower() == "true"))
                    {
                        model.MarketingAcctEnabled = true;
                    }
                    else
                    {
                        model.MarketingAcctEnabled = false;
                    }
                }

                model.Cell = ds.Tables[0].Rows[0]["Cell"].ToString();
                model.Phone = ds.Tables[0].Rows[0]["Phone"].ToString();
                model.Fax = ds.Tables[0].Rows[0]["Fax"].ToString();
                model.LicenseNumber = ds.Tables[0].Rows[0]["LicenseNumber"].ToString();

                model.NMLS = ds.Tables[0].Rows[0]["NMLS"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["NMLS"].ToString();

                //gdc CR47 ShowTasksInLSR
                model.ShowTasksInLSR = (ds.Tables[0].Rows[0]["ShowTasksInLSR"] == DBNull.Value || ds.Tables[0].Rows[0]["ShowTasksInLSR"].ToString() == "") ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowTasksInLSR"].ToString());

                // CR48
                if (ds.Tables[0].Rows[0]["RemindTaskDue"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["RemindTaskDue"].ToString() == "1") || (ds.Tables[0].Rows[0]["RemindTaskDue"].ToString().ToLower() == "true"))
                    {
                        model.RemindTaskDue = true;
                    }
                    else
                    {
                        model.RemindTaskDue = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["TaskReminder"].ToString() != "")
                {
                    model.TaskReminder = int.Parse(ds.Tables[0].Rows[0]["TaskReminder"].ToString());
                }
                model.SortTaskPickList = ds.Tables[0].Rows[0]["SortTaskPickList"].ToString();

                //,[LOComp],[BranchMgrComp],[DivisionMgrComp],[RegionMgrComp]

                model.LOComp = ds.Tables[0].Rows[0]["LOComp"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["LOComp"]);
                model.BranchMgrComp = ds.Tables[0].Rows[0]["BranchMgrComp"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["BranchMgrComp"]);
                model.DivisionMgrComp = ds.Tables[0].Rows[0]["DivisionMgrComp"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["DivisionMgrComp"]);
                model.RegionMgrComp = ds.Tables[0].Rows[0]["RegionMgrComp"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["RegionMgrComp"]);

                model.ExchangePassword = ds.Tables[0].Rows[0]["ExchangePassword"].ToString();

                return model;
            }
            else
            {
                return null;
            }
        }

        public int GetLoansPerPage(int UserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 LoansPerPage from Users ");
            strSql.Append(" where UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = UserId;
                       
            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UserId,UserEnabled,Username,EmailAddress,FirstName,LastName,RoleId,Password,LoansPerPage,UserPictureFile,Phone,Fax,Cell,Signature,GlobalId,MarketingAcctEnabled,LicenseNumber,Leadstar_ID,NMLS,ShowTasksInLSR,RemindTaskDue,TaskReminder,SortTaskPickList,ExchangePassword ");
            strSql.Append(" FROM Users ");
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
            strSql.Append(" UserId,UserEnabled,Username,EmailAddress,FirstName,LastName,RoleId,Password,LoansPerPage,UserPictureFile,Phone,Fax,Cell,Signature,GlobalId,MarketingAcctEnabled,LicenseNumber,Leadstar_ID,NMLS,ShowTasksInLSR,RemindTaskDue,TaskReminder,SortTaskPickList,ExchangePassword ");
            strSql.Append(" FROM Users ");
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
            parameters[0].Value = "Users";
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

