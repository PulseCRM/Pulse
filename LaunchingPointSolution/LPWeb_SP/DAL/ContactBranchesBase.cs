using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类ContactBranches。
	/// </summary>
	public class ContactBranchesBase
	{
        public ContactBranchesBase()
		{}
		#region  成员方法

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LPWeb.Model.ContactBranches model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ContactBranches(");
			strSql.Append("ContactCompanyId,Name,Enabled,Address,City,State,Zip,Phone,Fax,PrimaryContact,Modified,ModifiedBy)");
			strSql.Append(" values (");
			strSql.Append("@ContactCompanyId,@Name,@Enabled,@Address,@City,@State,@Zip,@Phone,@Fax,@PrimaryContact,@Modified,@ModifiedBy)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactCompanyId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Address", SqlDbType.NVarChar,255),
					new SqlParameter("@City", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.NVarChar,2),
					new SqlParameter("@Zip", SqlDbType.NVarChar,12),
					new SqlParameter("@Phone", SqlDbType.NVarChar,20),
					new SqlParameter("@Fax", SqlDbType.NVarChar,20),
					new SqlParameter("@PrimaryContact", SqlDbType.Int,4),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@ModifiedBy", SqlDbType.Int,4)};
			parameters[0].Value = model.ContactCompanyId;
			parameters[1].Value = model.Name;
			parameters[2].Value = model.Enabled;
			parameters[3].Value = model.Address;
			parameters[4].Value = model.City;
			parameters[5].Value = model.State;
			parameters[6].Value = model.Zip;
			parameters[7].Value = model.Phone;
			parameters[8].Value = model.Fax;
			parameters[9].Value = model.PrimaryContact;
			parameters[10].Value = model.Modified;
			parameters[11].Value = model.ModifiedBy;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
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
		public void Update(LPWeb.Model.ContactBranches model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ContactBranches set ");
            //strSql.Append("ContactBranchId=@ContactBranchId,");
			strSql.Append("ContactCompanyId=@ContactCompanyId,");
			strSql.Append("Name=@Name,");
			strSql.Append("Enabled=@Enabled,");
			strSql.Append("Address=@Address,");
			strSql.Append("City=@City,");
			strSql.Append("State=@State,");
			strSql.Append("Zip=@Zip,");
			strSql.Append("Phone=@Phone,");
			strSql.Append("Fax=@Fax,");
			strSql.Append("PrimaryContact=@PrimaryContact,");
			strSql.Append("Modified=@Modified,");
			strSql.Append("ModifiedBy=@ModifiedBy");
			strSql.Append(" where ContactBranchId=@ContactBranchId ");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactBranchId", SqlDbType.Int,4),
					new SqlParameter("@ContactCompanyId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Address", SqlDbType.NVarChar,255),
					new SqlParameter("@City", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.NVarChar,2),
					new SqlParameter("@Zip", SqlDbType.NVarChar,12),
					new SqlParameter("@Phone", SqlDbType.NVarChar,20),
					new SqlParameter("@Fax", SqlDbType.NVarChar,20),
					new SqlParameter("@PrimaryContact", SqlDbType.Int,4),
					new SqlParameter("@Modified", SqlDbType.DateTime),
					new SqlParameter("@ModifiedBy", SqlDbType.Int,4)};
			parameters[0].Value = model.ContactBranchId;
			parameters[1].Value = model.ContactCompanyId;
			parameters[2].Value = model.Name;
			parameters[3].Value = model.Enabled;
			parameters[4].Value = model.Address;
			parameters[5].Value = model.City;
			parameters[6].Value = model.State;
			parameters[7].Value = model.Zip;
			parameters[8].Value = model.Phone;
			parameters[9].Value = model.Fax;
			parameters[10].Value = model.PrimaryContact;
			parameters[11].Value = model.Modified;
			parameters[12].Value = model.ModifiedBy;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ContactBranchId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ContactBranches ");
			strSql.Append(" where ContactBranchId=@ContactBranchId ");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactBranchId", SqlDbType.Int,4)};
			parameters[0].Value = ContactBranchId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.ContactBranches GetModel(int ContactBranchId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ContactBranchId,ContactCompanyId,Name,Enabled,Address,City,State,Zip,Phone,Fax,PrimaryContact,Modified,ModifiedBy from ContactBranches ");
			strSql.Append(" where ContactBranchId=@ContactBranchId ");
			SqlParameter[] parameters = {
					new SqlParameter("@ContactBranchId", SqlDbType.Int,4)};
			parameters[0].Value = ContactBranchId;

			LPWeb.Model.ContactBranches model=new LPWeb.Model.ContactBranches();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["ContactBranchId"].ToString()!="")
				{
					model.ContactBranchId=int.Parse(ds.Tables[0].Rows[0]["ContactBranchId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ContactCompanyId"].ToString()!="")
				{
					model.ContactCompanyId=int.Parse(ds.Tables[0].Rows[0]["ContactCompanyId"].ToString());
				}
				model.Name=ds.Tables[0].Rows[0]["Name"].ToString();
				if(ds.Tables[0].Rows[0]["Enabled"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["Enabled"].ToString()=="1")||(ds.Tables[0].Rows[0]["Enabled"].ToString().ToLower()=="true"))
					{
						model.Enabled=true;
					}
					else
					{
						model.Enabled=false;
					}
				}
				model.Address=ds.Tables[0].Rows[0]["Address"].ToString();
				model.City=ds.Tables[0].Rows[0]["City"].ToString();
				model.State=ds.Tables[0].Rows[0]["State"].ToString();
				model.Zip=ds.Tables[0].Rows[0]["Zip"].ToString();
				model.Phone=ds.Tables[0].Rows[0]["Phone"].ToString();
				model.Fax=ds.Tables[0].Rows[0]["Fax"].ToString();
				if(ds.Tables[0].Rows[0]["PrimaryContact"].ToString()!="")
				{
					model.PrimaryContact=int.Parse(ds.Tables[0].Rows[0]["PrimaryContact"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Modified"].ToString()!="")
				{
					model.Modified=DateTime.Parse(ds.Tables[0].Rows[0]["Modified"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ModifiedBy"].ToString()!="")
				{
					model.ModifiedBy=int.Parse(ds.Tables[0].Rows[0]["ModifiedBy"].ToString());
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ContactBranchId,ContactCompanyId,Name,Enabled,Address,City,State,Zip,Phone,Fax,PrimaryContact,Modified,ModifiedBy ");
			strSql.Append(" FROM ContactBranches ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" ContactBranchId,ContactCompanyId,Name,Enabled,Address,City,State,Zip,Phone,Fax,PrimaryContact,Modified,ModifiedBy ");
			strSql.Append(" FROM ContactBranches ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
			parameters[0].Value = "ContactBranches";
			parameters[1].Value = "ID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}

		#endregion  成员方法
	}
}

