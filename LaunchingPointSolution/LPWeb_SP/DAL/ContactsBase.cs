using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Contacts。
	/// </summary>
    public class ContactsBase
    {
        public ContactsBase()
		{}
        #region  成员方法


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Contacts model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Contacts(");
            strSql.Append("FirstName,MiddleName,LastName,NickName,Title,GenerationCode,SSN,HomePhone,CellPhone,BusinessPhone,Fax,Email,DOB,Experian,TransUnion,Equifax,MailingAddr,MailingCity,MailingState,MailingZip,ContactCompanyId,WebAccountId,ContactEnable,CreatedBy,Created,ContactBranchId,Enabled,Signature,Picture)");
            strSql.Append(" values (");
            strSql.Append("@FirstName,@MiddleName,@LastName,@NickName,@Title,@GenerationCode,@SSN,@HomePhone,@CellPhone,@BusinessPhone,@Fax,@Email,@DOB,@Experian,@TransUnion,@Equifax,@MailingAddr,@MailingCity,@MailingState,@MailingZip,@ContactCompanyId,@WebAccountId,@ContactEnable,@CreatedBy,@Created,@ContactBranchId,@Enabled,@Signature,@Picture)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@FirstName", SqlDbType.NVarChar,50),
					new SqlParameter("@MiddleName", SqlDbType.NVarChar,50),
					new SqlParameter("@LastName", SqlDbType.NVarChar,50),
					new SqlParameter("@NickName", SqlDbType.NVarChar,50),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@GenerationCode", SqlDbType.NVarChar,10),
					new SqlParameter("@SSN", SqlDbType.NVarChar,20),
					new SqlParameter("@HomePhone", SqlDbType.NVarChar,20),
					new SqlParameter("@CellPhone", SqlDbType.NVarChar,20),
					new SqlParameter("@BusinessPhone", SqlDbType.NVarChar,20),
					new SqlParameter("@Fax", SqlDbType.NVarChar,20),
					new SqlParameter("@Email", SqlDbType.NVarChar,255),
					new SqlParameter("@DOB", SqlDbType.DateTime),
					new SqlParameter("@Experian", SqlDbType.SmallInt,2),
					new SqlParameter("@TransUnion", SqlDbType.SmallInt,2),
					new SqlParameter("@Equifax", SqlDbType.SmallInt,2),
					new SqlParameter("@MailingAddr", SqlDbType.NVarChar,50),
					new SqlParameter("@MailingCity", SqlDbType.NVarChar,50),
					new SqlParameter("@MailingState", SqlDbType.NChar,2),
					new SqlParameter("@MailingZip", SqlDbType.NVarChar,12),
					new SqlParameter("@ContactCompanyId", SqlDbType.Int,4),
					new SqlParameter("@WebAccountId", SqlDbType.Int,4),
					new SqlParameter("@ContactEnable", SqlDbType.Bit,1),
					new SqlParameter("@CreatedBy", SqlDbType.Int,4),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@ContactBranchId", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Signature", SqlDbType.NVarChar,500),
					new SqlParameter("@Picture", SqlDbType.VarBinary, -1)};
            parameters[0].Value = model.FirstName;
            parameters[1].Value = model.MiddleName;
            parameters[2].Value = model.LastName;
            parameters[3].Value = model.NickName;
            parameters[4].Value = model.Title;
            parameters[5].Value = model.GenerationCode;
            parameters[6].Value = model.SSN;
            parameters[7].Value = model.HomePhone;
            parameters[8].Value = model.CellPhone;
            parameters[9].Value = model.BusinessPhone;
            parameters[10].Value = model.Fax;
            parameters[11].Value = model.Email;
            parameters[12].Value = model.DOB;
            parameters[13].Value = model.Experian;
            parameters[14].Value = model.TransUnion;
            parameters[15].Value = model.Equifax;
            parameters[16].Value = model.MailingAddr;
            parameters[17].Value = model.MailingCity;
            parameters[18].Value = model.MailingState;
            parameters[19].Value = model.MailingZip;
            parameters[20].Value = model.ContactCompanyId;
            parameters[21].Value = model.WebAccountId;
            parameters[22].Value = model.ContactEnable;
            parameters[23].Value = model.CreatedBy;
            parameters[24].Value = model.Created;
            parameters[25].Value = model.ContactBranchId;
            parameters[26].Value = model.Enabled;
            parameters[27].Value = model.Signature;
            parameters[28].Value = model.Picture;

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
        public void Update(LPWeb.Model.Contacts model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Contacts set ");
            strSql.Append("FirstName=@FirstName,");
            strSql.Append("MiddleName=@MiddleName,");
            strSql.Append("LastName=@LastName,");
            strSql.Append("NickName=@NickName,");
            strSql.Append("Title=@Title,");
            strSql.Append("GenerationCode=@GenerationCode,");
            strSql.Append("SSN=@SSN,");
            strSql.Append("HomePhone=@HomePhone,");
            strSql.Append("CellPhone=@CellPhone,");
            strSql.Append("BusinessPhone=@BusinessPhone,");
            strSql.Append("Fax=@Fax,");
            strSql.Append("Email=@Email,");
            strSql.Append("DOB=@DOB,");
            strSql.Append("Experian=@Experian,");
            strSql.Append("TransUnion=@TransUnion,");
            strSql.Append("Equifax=@Equifax,");
            strSql.Append("MailingAddr=@MailingAddr,");
            strSql.Append("MailingCity=@MailingCity,");
            strSql.Append("MailingState=@MailingState,");
            strSql.Append("MailingZip=@MailingZip,");
            strSql.Append("ContactCompanyId=@ContactCompanyId,");
            strSql.Append("WebAccountId=@WebAccountId,");
            strSql.Append("ContactEnable=@ContactEnable,");
            strSql.Append("CreatedBy=@CreatedBy,");
            strSql.Append("Created=@Created,");
            strSql.Append("ContactBranchId=@ContactBranchId,");
            strSql.Append("Enabled=@Enabled,");
            strSql.Append("Signature=@Signature,");
            strSql.Append("Picture=@Picture");
            strSql.Append(" where ContactId=@ContactId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4),
					new SqlParameter("@FirstName", SqlDbType.NVarChar,50),
					new SqlParameter("@MiddleName", SqlDbType.NVarChar,50),
					new SqlParameter("@LastName", SqlDbType.NVarChar,50),
					new SqlParameter("@NickName", SqlDbType.NVarChar,50),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@GenerationCode", SqlDbType.NVarChar,10),
					new SqlParameter("@SSN", SqlDbType.NVarChar,20),
					new SqlParameter("@HomePhone", SqlDbType.NVarChar,20),
					new SqlParameter("@CellPhone", SqlDbType.NVarChar,20),
					new SqlParameter("@BusinessPhone", SqlDbType.NVarChar,20),
					new SqlParameter("@Fax", SqlDbType.NVarChar,20),
					new SqlParameter("@Email", SqlDbType.NVarChar,255),
					new SqlParameter("@DOB", SqlDbType.DateTime),
					new SqlParameter("@Experian", SqlDbType.SmallInt,2),
					new SqlParameter("@TransUnion", SqlDbType.SmallInt,2),
					new SqlParameter("@Equifax", SqlDbType.SmallInt,2),
					new SqlParameter("@MailingAddr", SqlDbType.NVarChar,50),
					new SqlParameter("@MailingCity", SqlDbType.NVarChar,50),
					new SqlParameter("@MailingState", SqlDbType.NChar,2),
					new SqlParameter("@MailingZip", SqlDbType.NVarChar,12),
					new SqlParameter("@ContactCompanyId", SqlDbType.Int,4),
					new SqlParameter("@WebAccountId", SqlDbType.Int,4),
					new SqlParameter("@ContactEnable", SqlDbType.Bit,1),
					new SqlParameter("@CreatedBy", SqlDbType.Int,4),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@ContactBranchId", SqlDbType.Int,4),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Signature", SqlDbType.NVarChar,500),
					new SqlParameter("@Picture", SqlDbType.VarBinary, -1)};
            parameters[0].Value = model.ContactId;
            parameters[1].Value = model.FirstName;
            parameters[2].Value = model.MiddleName;
            parameters[3].Value = model.LastName;
            parameters[4].Value = model.NickName;
            parameters[5].Value = model.Title;
            parameters[6].Value = model.GenerationCode;
            parameters[7].Value = model.SSN;
            parameters[8].Value = model.HomePhone;
            parameters[9].Value = model.CellPhone;
            parameters[10].Value = model.BusinessPhone;
            parameters[11].Value = model.Fax;
            parameters[12].Value = model.Email;
            parameters[13].Value = model.DOB;
            parameters[14].Value = model.Experian;
            parameters[15].Value = model.TransUnion;
            parameters[16].Value = model.Equifax;
            parameters[17].Value = model.MailingAddr;
            parameters[18].Value = model.MailingCity;
            parameters[19].Value = model.MailingState;
            parameters[20].Value = model.MailingZip;
            parameters[21].Value = model.ContactCompanyId;
            parameters[22].Value = model.WebAccountId;
            parameters[23].Value = model.ContactEnable;
            parameters[24].Value = model.CreatedBy;
            parameters[25].Value = model.Created;
            parameters[26].Value = model.ContactBranchId;
            parameters[27].Value = model.Enabled;
            parameters[28].Value = model.Signature;
            parameters[29].Value = model.Picture;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ContactId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Contacts ");
            strSql.Append(" where ContactId=@ContactId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4)};
            parameters[0].Value = ContactId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Contacts GetModel(int ContactId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ContactId,FirstName,MiddleName,LastName,NickName,Title,GenerationCode,SSN,HomePhone,CellPhone,BusinessPhone,Fax,Email,DOB,Experian,TransUnion,Equifax,MailingAddr,MailingCity,MailingState,MailingZip,ContactCompanyId,WebAccountId,ContactEnable,UpdatePoint,CreatedBy,Created,ContactBranchId,Enabled,Signature,Picture from Contacts ");
            strSql.Append(" where ContactId=@ContactId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactId", SqlDbType.Int,4)};
            parameters[0].Value = ContactId;

            LPWeb.Model.Contacts model = new LPWeb.Model.Contacts();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ContactId"].ToString() != "")
                {
                    model.ContactId = int.Parse(ds.Tables[0].Rows[0]["ContactId"].ToString());
                }
                model.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                model.MiddleName = ds.Tables[0].Rows[0]["MiddleName"].ToString();
                model.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                model.NickName = ds.Tables[0].Rows[0]["NickName"].ToString();
                model.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                model.GenerationCode = ds.Tables[0].Rows[0]["GenerationCode"].ToString();
                model.SSN = ds.Tables[0].Rows[0]["SSN"].ToString();
                model.HomePhone = ds.Tables[0].Rows[0]["HomePhone"].ToString();
                model.CellPhone = ds.Tables[0].Rows[0]["CellPhone"].ToString();
                model.BusinessPhone = ds.Tables[0].Rows[0]["BusinessPhone"].ToString();
                model.Fax = ds.Tables[0].Rows[0]["Fax"].ToString();
                model.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                if (ds.Tables[0].Rows[0]["DOB"].ToString() != "")
                {
                    model.DOB = DateTime.Parse(ds.Tables[0].Rows[0]["DOB"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Experian"].ToString() != "")
                {
                    model.Experian = int.Parse(ds.Tables[0].Rows[0]["Experian"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TransUnion"].ToString() != "")
                {
                    model.TransUnion = int.Parse(ds.Tables[0].Rows[0]["TransUnion"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Equifax"].ToString() != "")
                {
                    model.Equifax = int.Parse(ds.Tables[0].Rows[0]["Equifax"].ToString());
                }
                model.MailingAddr = ds.Tables[0].Rows[0]["MailingAddr"].ToString();
                model.MailingCity = ds.Tables[0].Rows[0]["MailingCity"].ToString();
                model.MailingState = ds.Tables[0].Rows[0]["MailingState"].ToString();
                model.MailingZip = ds.Tables[0].Rows[0]["MailingZip"].ToString();
                if (ds.Tables[0].Rows[0]["ContactCompanyId"].ToString() != "")
                {
                    model.ContactCompanyId = int.Parse(ds.Tables[0].Rows[0]["ContactCompanyId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["WebAccountId"].ToString() != "")
                {
                    model.WebAccountId = int.Parse(ds.Tables[0].Rows[0]["WebAccountId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ContactEnable"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ContactEnable"].ToString() == "1") || (ds.Tables[0].Rows[0]["ContactEnable"].ToString().ToLower() == "true"))
                    {
                        model.ContactEnable = true;
                    }
                    else
                    {
                        model.ContactEnable = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["UpdatePoint"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["UpdatePoint"].ToString() == "1") || (ds.Tables[0].Rows[0]["UpdatePoint"].ToString().ToLower() == "true"))
                    {
                        model.UpdatePoint = true;
                    }
                    else
                    {
                        model.UpdatePoint = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["CreatedBy"].ToString() != "")
                {
                    model.CreatedBy = int.Parse(ds.Tables[0].Rows[0]["CreatedBy"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Created"].ToString() != "")
                {
                    model.Created = DateTime.Parse(ds.Tables[0].Rows[0]["Created"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ContactBranchId"].ToString() != "")
                {
                    model.ContactBranchId = int.Parse(ds.Tables[0].Rows[0]["ContactBranchId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Enabled"].ToString() != "")
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
                model.Signature = ds.Tables[0].Rows[0]["Signature"].ToString();
                model.Picture = DBNull.Value == ds.Tables[0].Rows[0]["Picture"] ? null : (byte[])ds.Tables[0].Rows[0]["Picture"];
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
            strSql.Append("select ContactId,FirstName,MiddleName,LastName,NickName,Title,GenerationCode,SSN,HomePhone,CellPhone,BusinessPhone,Fax,Email,DOB,Experian,TransUnion,Equifax,MailingAddr,MailingCity,MailingState,MailingZip,ContactCompanyId,WebAccountId,ContactEnable ");
            strSql.Append(" FROM Contacts ");
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
            strSql.Append(" ContactId,FirstName,MiddleName,LastName,NickName,Title,GenerationCode,SSN,HomePhone,CellPhone,BusinessPhone,Fax,Email,DOB,Experian,TransUnion,Equifax,MailingAddr,MailingCity,MailingState,MailingZip,ContactCompanyId,WebAccountId,ContactEnable ");
            strSql.Append(" FROM Contacts ");
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
            parameters[0].Value = "Contacts";
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

