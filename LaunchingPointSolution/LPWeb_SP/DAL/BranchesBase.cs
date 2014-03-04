using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
    /// <summary>
    /// ݷBranches
    /// </summary>
    public class BranchesBase
    {
        public BranchesBase()
        { }

        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Branches model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Branches(");
            strSql.Append("Name,[Desc],Enabled,RegionID,DivisionID,GroupID,BranchAddress,City,BranchState,Zip,WebsiteLogo,Phone,Fax,Email,WebURL)");
            strSql.Append(" values (");
            strSql.Append("@Name,@Desc,@Enabled,@RegionID,@DivisionID,@GroupID,@BranchAddress,@City,@BranchState,@Zip,@WebsiteLogo,@Phone,@Fax,@Email,@WebURL)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@RegionID", SqlDbType.Int,4),
					new SqlParameter("@DivisionID", SqlDbType.Int,4),
					new SqlParameter("@GroupID", SqlDbType.Int,4),
					new SqlParameter("@BranchAddress", SqlDbType.NVarChar,255),
					new SqlParameter("@City", SqlDbType.NVarChar,50),
					new SqlParameter("@BranchState", SqlDbType.NVarChar,2),
					new SqlParameter("@Zip", SqlDbType.NVarChar,5),
					new SqlParameter("@WebsiteLogo", SqlDbType.VarBinary),
                    new SqlParameter("@Phone", SqlDbType.NVarChar,20),
                    new SqlParameter("@Fax", SqlDbType.NVarChar,20),
                    new SqlParameter("@Email", SqlDbType.NVarChar,255),
                    new SqlParameter("@WebURL", SqlDbType.NVarChar,255)
                                        };

            parameters[0].Value = model.Name;
            parameters[1].Value = model.Desc;
            parameters[2].Value = model.Enabled;
            parameters[3].Value = model.RegionID;
            parameters[4].Value = model.DivisionID;
            parameters[5].Value = model.GroupID;
            parameters[6].Value = model.BranchAddress;
            parameters[7].Value = model.City;
            parameters[8].Value = model.BranchState;
            parameters[9].Value = model.Zip;
            parameters[10].Value = model.WebsiteLogo;
            parameters[11].Value = model.Phone;
            parameters[12].Value = model.Fax;
            parameters[13].Value = model.Email;
            parameters[14].Value = model.WebURL;


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
        public void Update(LPWeb.Model.Branches model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Branches set ");
            strSql.Append("BranchId=@BranchId,");
            strSql.Append("Name=@Name,");
            strSql.Append("[Desc]=@Desc,");
            strSql.Append("Enabled=@Enabled,");
            strSql.Append("RegionID=@RegionID,");
            strSql.Append("DivisionID=@DivisionID,");
            strSql.Append("GroupID=@GroupID,");
            strSql.Append("BranchAddress=@BranchAddress,");
            strSql.Append("City=@City,");
            strSql.Append("BranchState=@BranchState,");
            strSql.Append("Zip=@Zip,");
            strSql.Append("WebsiteLogo=@WebsiteLogo,");
            strSql.Append("Phone=@Phone,");
            strSql.Append("Fax=@Fax,");
            strSql.Append("Email=@Email,");
            strSql.Append("WebURL=@WebURL,");
            strSql.Append(" where BranchId=@BranchId ");
            SqlParameter[] parameters = {
					new SqlParameter("@BranchId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Desc", SqlDbType.NVarChar,500),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@RegionID", SqlDbType.Int,4),
					new SqlParameter("@DivisionID", SqlDbType.Int,4),
					new SqlParameter("@GroupID", SqlDbType.Int,4),
					new SqlParameter("@BranchAddress", SqlDbType.NVarChar,255),
					new SqlParameter("@City", SqlDbType.NVarChar,50),
					new SqlParameter("@BranchState", SqlDbType.NVarChar,2),
					new SqlParameter("@Zip", SqlDbType.NVarChar,5),
					new SqlParameter("@WebsiteLogo", SqlDbType.VarBinary),
                    new SqlParameter("@Phone", SqlDbType.NVarChar,20),
                    new SqlParameter("@Fax", SqlDbType.NVarChar,20),
                    new SqlParameter("@Email", SqlDbType.NVarChar,255),
                    new SqlParameter("@WebURL", SqlDbType.NVarChar,255)  };

            parameters[0].Value = model.BranchId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Desc;
            parameters[3].Value = model.Enabled;
            parameters[4].Value = model.RegionID;
            parameters[5].Value = model.DivisionID;
            parameters[6].Value = model.GroupID;
            parameters[7].Value = model.BranchAddress;
            parameters[8].Value = model.City;
            parameters[9].Value = model.BranchState;
            parameters[10].Value = model.Zip;
            parameters[11].Value = model.WebsiteLogo;
            parameters[12].Value = model.Phone;
            parameters[13].Value = model.Fax;
            parameters[14].Value = model.Email;
            parameters[15].Value = model.WebURL;



            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int BranchId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Branches ");
            strSql.Append(" where BranchId=@BranchId ");
            SqlParameter[] parameters = {
					new SqlParameter("@BranchId", SqlDbType.Int,4)};
            parameters[0].Value = BranchId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Branches GetModel(int BranchId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 BranchId,[Name],[Desc],Enabled,RegionID,DivisionID,GroupID,BranchAddress,City,BranchState,Zip,WebsiteLogo,GlobalId,License1,License2,License3,License4,Disclaimer,Phone,Fax,WebURL,Email,License5,Leadstar_Username,Leadstar_ID,Leadstar_Userid,HomeBranch,EnableMailChimp,MailChimpAPIKey from Branches ");
            strSql.Append(" where BranchId=@BranchId");
            SqlParameter[] parameters = {
					new SqlParameter("@BranchId", SqlDbType.Int,4)
};
            parameters[0].Value = BranchId;

            LPWeb.Model.Branches model = new LPWeb.Model.Branches();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["BranchId"] != null && ds.Tables[0].Rows[0]["BranchId"].ToString() != "")
                {
                    model.BranchId = int.Parse(ds.Tables[0].Rows[0]["BranchId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Name"] != null && ds.Tables[0].Rows[0]["Name"].ToString() != "")
                {
                    model.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Desc"] != null && ds.Tables[0].Rows[0]["Desc"].ToString() != "")
                {
                    model.Desc = ds.Tables[0].Rows[0]["Desc"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Enabled"] != null && ds.Tables[0].Rows[0]["Enabled"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Enabled"].ToString() == "1") || (ds.Tables[0].Rows[0]["Enabled"].ToString().ToLower() == "true"))
                    {
                        model.Enabled = true;
                    }
                    else
                    {
                        model.Enabled = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["RegionID"] != null && ds.Tables[0].Rows[0]["RegionID"].ToString() != "")
                {
                    model.RegionID = int.Parse(ds.Tables[0].Rows[0]["RegionID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DivisionID"] != null && ds.Tables[0].Rows[0]["DivisionID"].ToString() != "")
                {
                    model.DivisionID = int.Parse(ds.Tables[0].Rows[0]["DivisionID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["GroupID"] != null && ds.Tables[0].Rows[0]["GroupID"].ToString() != "")
                {
                    model.GroupID = int.Parse(ds.Tables[0].Rows[0]["GroupID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["BranchAddress"] != null && ds.Tables[0].Rows[0]["BranchAddress"].ToString() != "")
                {
                    model.BranchAddress = ds.Tables[0].Rows[0]["BranchAddress"].ToString();
                }
                if (ds.Tables[0].Rows[0]["City"] != null && ds.Tables[0].Rows[0]["City"].ToString() != "")
                {
                    model.City = ds.Tables[0].Rows[0]["City"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BranchState"] != null && ds.Tables[0].Rows[0]["BranchState"].ToString() != "")
                {
                    model.BranchState = ds.Tables[0].Rows[0]["BranchState"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Zip"] != null && ds.Tables[0].Rows[0]["Zip"].ToString() != "")
                {
                    model.Zip = ds.Tables[0].Rows[0]["Zip"].ToString();
                }
                if (ds.Tables[0].Rows[0]["WebsiteLogo"] != null && ds.Tables[0].Rows[0]["WebsiteLogo"].ToString() != "")
                {
                    model.WebsiteLogo = (byte[])ds.Tables[0].Rows[0]["WebsiteLogo"];
                }
                if (ds.Tables[0].Rows[0]["GlobalId"] != null && ds.Tables[0].Rows[0]["GlobalId"].ToString() != "")
                {
                    model.GlobalId = ds.Tables[0].Rows[0]["GlobalId"].ToString();
                }
                if (ds.Tables[0].Rows[0]["License1"] != null && ds.Tables[0].Rows[0]["License1"].ToString() != "")
                {
                    model.License1 = ds.Tables[0].Rows[0]["License1"].ToString();
                }
                if (ds.Tables[0].Rows[0]["License2"] != null && ds.Tables[0].Rows[0]["License2"].ToString() != "")
                {
                    model.License2 = ds.Tables[0].Rows[0]["License2"].ToString();
                }
                if (ds.Tables[0].Rows[0]["License3"] != null && ds.Tables[0].Rows[0]["License3"].ToString() != "")
                {
                    model.License3 = ds.Tables[0].Rows[0]["License3"].ToString();
                }
                if (ds.Tables[0].Rows[0]["License4"] != null && ds.Tables[0].Rows[0]["License4"].ToString() != "")
                {
                    model.License4 = ds.Tables[0].Rows[0]["License4"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Disclaimer"] != null && ds.Tables[0].Rows[0]["Disclaimer"].ToString() != "")
                {
                    model.Disclaimer = ds.Tables[0].Rows[0]["Disclaimer"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Phone"] != null && ds.Tables[0].Rows[0]["Phone"].ToString() != "")
                {
                    model.Phone = ds.Tables[0].Rows[0]["Phone"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Fax"] != null && ds.Tables[0].Rows[0]["Fax"].ToString() != "")
                {
                    model.Fax = ds.Tables[0].Rows[0]["Fax"].ToString();
                }
                if (ds.Tables[0].Rows[0]["WebURL"] != null && ds.Tables[0].Rows[0]["WebURL"].ToString() != "")
                {
                    model.WebURL = ds.Tables[0].Rows[0]["WebURL"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Email"] != null && ds.Tables[0].Rows[0]["Email"].ToString() != "")
                {
                    model.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                }
                if (ds.Tables[0].Rows[0]["License5"] != null && ds.Tables[0].Rows[0]["License5"].ToString() != "")
                {
                    model.License5 = ds.Tables[0].Rows[0]["License5"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Leadstar_Username"] != null && ds.Tables[0].Rows[0]["Leadstar_Username"].ToString() != "")
                {
                    model.Leadstar_Username = ds.Tables[0].Rows[0]["Leadstar_Username"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Leadstar_ID"] != null && ds.Tables[0].Rows[0]["Leadstar_ID"].ToString() != "")
                {
                    model.Leadstar_ID = ds.Tables[0].Rows[0]["Leadstar_ID"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Leadstar_Userid"] != null && ds.Tables[0].Rows[0]["Leadstar_Userid"].ToString() != "")
                {
                    model.Leadstar_Userid = ds.Tables[0].Rows[0]["Leadstar_Userid"].ToString();
                }
                if (ds.Tables[0].Rows[0]["HomeBranch"] != null && ds.Tables[0].Rows[0]["HomeBranch"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["HomeBranch"].ToString() == "1") || (ds.Tables[0].Rows[0]["HomeBranch"].ToString().ToLower() == "true"))
                    {
                        model.HomeBranch = true;
                    }
                    else
                    {
                        model.HomeBranch = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["EnableMailChimp"] != null && ds.Tables[0].Rows[0]["EnableMailChimp"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["EnableMailChimp"].ToString() == "1") || (ds.Tables[0].Rows[0]["EnableMailChimp"].ToString().ToLower() == "true"))
                    {
                        model.EnableMailChimp = true;
                    }
                    else
                    {
                        model.EnableMailChimp = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["MailChimpAPIKey"] != null && ds.Tables[0].Rows[0]["MailChimpAPIKey"].ToString() != "")
                {
                    model.MailChimpAPIKey = ds.Tables[0].Rows[0]["MailChimpAPIKey"].ToString();
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
            strSql.Append("select BranchId,Name,[Desc],Enabled,RegionID,DivisionID,GroupID,BranchAddress,City,BranchState,Zip,WebsiteLogo ");
            strSql.Append(" FROM Branches ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" ORDER BY [Name]  ");
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
            strSql.Append(" BranchId,Name,[Desc],Enabled,RegionID,DivisionID,GroupID,BranchAddress,City,BranchState,Zip,WebsiteLogo ");
            strSql.Append(" FROM Branches ");
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
            parameters[0].Value = "Branches";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage", parameters, "ds");
        }

        /// <summary>
        /// 设置其他Branch的HomeBranch 为false
        /// </summary>
        /// <param name="BranchId">不包含的ID</param>
        public void SetOtherHomeBranchFalse(int BranchId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" update Branches set ");
            strSql.Append(" HomeBranch=0 ");
            strSql.Append(" where BranchId<>@BranchId ");
            SqlParameter[] parameters = {
					new SqlParameter("@BranchId", SqlDbType.Int,4),
					  };

            parameters[0].Value = BranchId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        #endregion  成员方法
    }
}

