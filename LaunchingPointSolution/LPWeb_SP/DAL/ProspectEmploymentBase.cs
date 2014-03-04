using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类:ProspectEmployment
    /// </summary>
    public class ProspectEmploymentBase
    {
        public ProspectEmploymentBase()
        { }
        #region  Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("EmplId", "ProspectEmployment");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int EmplId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from ProspectEmployment");
            strSql.Append(" where EmplId=@EmplId");
            SqlParameter[] parameters = {
					new SqlParameter("@EmplId", SqlDbType.Int,4)
};
            parameters[0].Value = EmplId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.ProspectEmployment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ProspectEmployment(");
            strSql.Append("ContactId,SelfEmployed,Position,StartYear,StartMonth,EndYear,EndMonth,YearsOnWork,Phone,ContactBranchId,CompanyName,Address,City,State,Zip,BusinessType,VerifyYourTaxes)");
            strSql.Append(" values (");
            strSql.Append("@ContactId,@SelfEmployed,@Position,@StartYear,@StartMonth,@EndYear,@EndMonth,@YearsOnWork,@Phone,@ContactBranchId,@CompanyName,@Address,@City,@State,@Zip,@BusinessType,@VerifyYourTaxes)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@SelfEmployed", SqlDbType.Bit,1),
					new SqlParameter("@Position", SqlDbType.NVarChar,50),
					new SqlParameter("@StartYear", SqlDbType.Decimal,5),
					new SqlParameter("@StartMonth", SqlDbType.Decimal,5),
					new SqlParameter("@EndYear", SqlDbType.Decimal,5),
					new SqlParameter("@EndMonth", SqlDbType.Decimal,5),
					new SqlParameter("@YearsOnWork", SqlDbType.Decimal,5),
					new SqlParameter("@Phone", SqlDbType.NVarChar,20),
					new SqlParameter("@ContactBranchId", SqlDbType.Int,4),
					new SqlParameter("@CompanyName", SqlDbType.NVarChar,255),
					new SqlParameter("@Address", SqlDbType.NVarChar,255),
					new SqlParameter("@City", SqlDbType.NVarChar,100),
					new SqlParameter("@State", SqlDbType.NVarChar,2),
					new SqlParameter("@Zip", SqlDbType.NVarChar,20),
					new SqlParameter("@BusinessType", SqlDbType.NVarChar,255),
					new SqlParameter("@VerifyYourTaxes",  SqlDbType.Bit,1)};
            parameters[0].Value = model.ContactId;
            parameters[1].Value = model.SelfEmployed;
            parameters[2].Value = model.Position;
            parameters[3].Value = model.StartYear;
            parameters[4].Value = model.StartMonth;
            parameters[5].Value = model.EndYear;
            parameters[6].Value = model.EndMonth;
            parameters[7].Value = model.YearsOnWork;
            parameters[8].Value = model.Phone;
            parameters[9].Value = model.ContactBranchId;
            parameters[10].Value = model.CompanyName;
            parameters[11].Value = model.Address;
            parameters[12].Value = model.City;
            parameters[13].Value = model.State;
            parameters[14].Value = model.Zip;
            parameters[15].Value = model.BusinessType;
            parameters[16].Value = model.VerifyYourTaxes;

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
        /// 更新一条数据
        /// </summary>
        public bool Update(LPWeb.Model.ProspectEmployment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ProspectEmployment set ");
            strSql.Append("ContactId=@ContactId,");
            strSql.Append("SelfEmployed=@SelfEmployed,");
            strSql.Append("Position=@Position,");
            strSql.Append("StartYear=@StartYear,");
            strSql.Append("StartMonth=@StartMonth,");
            strSql.Append("EndYear=@EndYear,");
            strSql.Append("EndMonth=@EndMonth,");
            strSql.Append("YearsOnWork=@YearsOnWork,");
            strSql.Append("Phone=@Phone,");
            strSql.Append("ContactBranchId=@ContactBranchId,");
            strSql.Append("CompanyName=@CompanyName,");
            strSql.Append("Address=@Address,");
            strSql.Append("City=@City,");
            strSql.Append("State=@State,");
            strSql.Append("Zip=@Zip,");
            strSql.Append("BusinessType=@BusinessType,");
            strSql.Append("VerifyYourTaxes=@VerifyYourTaxes");
            strSql.Append(" where EmplId=@EmplId");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@SelfEmployed", SqlDbType.Bit,1),
					new SqlParameter("@Position", SqlDbType.NVarChar,50),
					new SqlParameter("@StartYear", SqlDbType.Decimal,5),
					new SqlParameter("@StartMonth", SqlDbType.Decimal,5),
					new SqlParameter("@EndYear", SqlDbType.Decimal,5),
					new SqlParameter("@EndMonth", SqlDbType.Decimal,5),
					new SqlParameter("@YearsOnWork", SqlDbType.Decimal,5),
					new SqlParameter("@Phone", SqlDbType.NVarChar,20),
					new SqlParameter("@ContactBranchId", SqlDbType.Int,4),
					new SqlParameter("@CompanyName", SqlDbType.NVarChar,255),
					new SqlParameter("@Address", SqlDbType.NVarChar,255),
					new SqlParameter("@City", SqlDbType.NVarChar,100),
					new SqlParameter("@State", SqlDbType.NVarChar,2),
					new SqlParameter("@Zip", SqlDbType.NVarChar,20),
					new SqlParameter("@BusinessType", SqlDbType.NVarChar,255),
					new SqlParameter("@VerifyYourTaxes", SqlDbType.Bit,1),
                    new SqlParameter("@EmplId", SqlDbType.Int,4)};
            parameters[0].Value = model.ContactId;
            parameters[1].Value = model.SelfEmployed;
            parameters[2].Value = model.Position;
            parameters[3].Value = model.StartYear;
            parameters[4].Value = model.StartMonth;
            parameters[5].Value = model.EndYear;
            parameters[6].Value = model.EndMonth;
            parameters[7].Value = model.YearsOnWork;
            parameters[8].Value = model.Phone;
            parameters[9].Value = model.ContactBranchId;
            parameters[10].Value = model.CompanyName;
            parameters[11].Value = model.Address;
            parameters[12].Value = model.City;
            parameters[13].Value = model.State;
            parameters[14].Value = model.Zip;
            parameters[15].Value = model.BusinessType;
            parameters[16].Value = model.VerifyYourTaxes;
            parameters[17].Value = model.EmplId;

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
        public bool Delete(int EmplId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProspectEmployment ");
            strSql.Append(" where EmplId=@EmplId");
            SqlParameter[] parameters = {
					new SqlParameter("@EmplId", SqlDbType.Int,4)
};
            parameters[0].Value = EmplId;

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
        public bool DeleteList(string EmplIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProspectEmployment ");
            strSql.Append(" where EmplId in (" + EmplIdlist + ")  ");
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
        public LPWeb.Model.ProspectEmployment GetModel(int EmplId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 EmplId,ContactId,SelfEmployed,Position,StartYear,StartMonth,EndYear,EndMonth,YearsOnWork,Phone,ContactBranchId,CompanyName,Address,City,State,Zip,BusinessType,VerifyYourTaxes from ProspectEmployment ");
            strSql.Append(" where EmplId=@EmplId ");
            SqlParameter[] parameters = {
					new SqlParameter("@EmplId", SqlDbType.Int,4)};
            parameters[0].Value = EmplId;

            LPWeb.Model.ProspectEmployment model = new LPWeb.Model.ProspectEmployment();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["EmplId"].ToString() != "")
                {
                    model.EmplId = int.Parse(ds.Tables[0].Rows[0]["EmplId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ContactId"].ToString() != "")
                {
                    model.ContactId = int.Parse(ds.Tables[0].Rows[0]["ContactId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SelfEmployed"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["SelfEmployed"].ToString() == "1") || (ds.Tables[0].Rows[0]["SelfEmployed"].ToString().ToLower() == "true"))
                    {
                        model.SelfEmployed = true;
                    }
                    else
                    {
                        model.SelfEmployed = false;
                    }
                }
                model.Position = ds.Tables[0].Rows[0]["Position"].ToString();
                if (ds.Tables[0].Rows[0]["StartYear"].ToString() != "")
                {
                    model.StartYear = decimal.Parse(ds.Tables[0].Rows[0]["StartYear"].ToString());
                }
                if (ds.Tables[0].Rows[0]["StartMonth"].ToString() != "")
                {
                    model.StartMonth = decimal.Parse(ds.Tables[0].Rows[0]["StartMonth"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EndYear"].ToString() != "")
                {
                    model.EndYear = decimal.Parse(ds.Tables[0].Rows[0]["EndYear"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EndMonth"].ToString() != "")
                {
                    model.EndMonth = decimal.Parse(ds.Tables[0].Rows[0]["EndMonth"].ToString());
                }
                if (ds.Tables[0].Rows[0]["YearsOnWork"].ToString() != "")
                {
                    model.YearsOnWork = decimal.Parse(ds.Tables[0].Rows[0]["YearsOnWork"].ToString());
                }
                model.Phone = ds.Tables[0].Rows[0]["Phone"].ToString();
                if (ds.Tables[0].Rows[0]["ContactBranchId"].ToString() != "")
                {
                    model.ContactBranchId = int.Parse(ds.Tables[0].Rows[0]["ContactBranchId"].ToString());
                }
                model.CompanyName = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                model.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                model.City = ds.Tables[0].Rows[0]["City"].ToString();
                model.State = ds.Tables[0].Rows[0]["State"].ToString();
                model.Zip = ds.Tables[0].Rows[0]["Zip"].ToString();
                model.BusinessType = ds.Tables[0].Rows[0]["BusinessType"].ToString();
                if (ds.Tables[0].Rows[0]["VerifyYourTaxes"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["VerifyYourTaxes"].ToString() == "1") || (ds.Tables[0].Rows[0]["VerifyYourTaxes"].ToString().ToLower() == "true"))
                    {
                        model.VerifyYourTaxes = true;
                    }
                    else
                    {
                        model.VerifyYourTaxes = false;
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
            strSql.Append("select EmplId,ContactId,SelfEmployed,Position,StartYear,StartMonth,EndYear,EndMonth,YearsOnWork,Phone,ContactBranchId,CompanyName,Address,City,State,Zip,BusinessType,VerifyYourTaxes ");
            strSql.Append(" FROM ProspectEmployment ");
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
            strSql.Append(" EmplId,ContactId,SelfEmployed,Position,StartYear,StartMonth,EndYear,EndMonth,YearsOnWork,Phone,ContactBranchId,CompanyName,Address,City,State,Zip,BusinessType,VerifyYourTaxes ");
            strSql.Append(" FROM ProspectEmployment ");
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
            parameters[0].Value = "ProspectEmployment";
            parameters[1].Value = "EmplId";
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

