using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Template_Wfl_Tasks。
	/// </summary>
	public class Template_Wfl_TasksBase
    {
        public Template_Wfl_TasksBase()
        { }
        #region
        /// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LPWeb.Model.Template_Wfl_Tasks model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Template_Wfl_Tasks(");
            strSql.Append("WflStageId,Name,Enabled,Type,DaysDueFromCoe,PrerequisiteTaskId,DaysDueAfterPrerequisite,OwnerRoleId,WarningEmailId,OverdueEmailId,CompletionEmailId,SequenceNumber,Description,DaysFromCreation,ExternalViewing)");
			strSql.Append(" values (");
            strSql.Append("@WflStageId,@Name,@Enabled,@Type,@DaysDueFromCoe,@PrerequisiteTaskId,@DaysDueAfterPrerequisite,@OwnerRoleId,@WarningEmailId,@OverdueEmailId,@CompletionEmailId,@SequenceNumber,@Description,@DaysFromCreation,@ExternalViewing)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@WflStageId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Type", SqlDbType.SmallInt,2),
					new SqlParameter("@DaysDueFromCoe", SqlDbType.SmallInt,2),
					new SqlParameter("@PrerequisiteTaskId", SqlDbType.Int,4),
					new SqlParameter("@DaysDueAfterPrerequisite", SqlDbType.SmallInt,2),
					new SqlParameter("@OwnerRoleId", SqlDbType.Int,4),
					new SqlParameter("@WarningEmailId", SqlDbType.Int,4),
					new SqlParameter("@OverdueEmailId", SqlDbType.Int,4),
					new SqlParameter("@CompletionEmailId", SqlDbType.Int,4),
					new SqlParameter("@SequenceNumber", SqlDbType.SmallInt,2),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@DaysFromCreation", SqlDbType.SmallInt,2),
                    new SqlParameter("@ExternalViewing", SqlDbType.Bit,1)};
			parameters[0].Value = model.WflStageId;
			parameters[1].Value = model.Name;
			parameters[2].Value = model.Enabled;
			parameters[3].Value = model.Type;
            if (model.DaysDueFromCoe == null)
            {
                parameters[4].Value = DBNull.Value;
            }
            else
            {
                parameters[4].Value = model.DaysDueFromCoe;
            }
			parameters[5].Value = model.PrerequisiteTaskId;
            if (model.DaysDueAfterPrerequisite == null)
            {
                parameters[6].Value = DBNull.Value;
            }
            else
            {
                parameters[6].Value = model.DaysDueAfterPrerequisite;
            }
			parameters[7].Value = model.OwnerRoleId;
            if (model.WarningEmailId == 0 || model.WarningEmailId == null)
            {
                parameters[8].Value = DBNull.Value;
            }
            else
            {
                parameters[8].Value = model.WarningEmailId;
            }
            if (model.OverdueEmailId == 0 || model.OverdueEmailId == null)
            {
                parameters[9].Value = DBNull.Value;
            }
            else
            {
                parameters[9].Value = model.OverdueEmailId;
            }
            if (model.CompletionEmailId == 0 || model.CompletionEmailId == null)
            {
                parameters[10].Value = DBNull.Value;
            }
            else
            {
                parameters[10].Value = model.CompletionEmailId;
            }
			parameters[11].Value = model.SequenceNumber;
            parameters[12].Value = model.Description;

            if (model.DaysFromCreation == null)
            {
                parameters[13].Value = DBNull.Value;
            }
            else
            {
                parameters[13].Value = model.DaysFromCreation;
            }

            parameters[14].Value = model.ExternalViewing;

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
		public void Update(LPWeb.Model.Template_Wfl_Tasks model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Template_Wfl_Tasks set ");
			strSql.Append("WflStageId=@WflStageId,");
			strSql.Append("Name=@Name,");
			strSql.Append("Enabled=@Enabled,");
			strSql.Append("Type=@Type,");
			strSql.Append("DaysDueFromCoe=@DaysDueFromCoe,");
			strSql.Append("PrerequisiteTaskId=@PrerequisiteTaskId,");
			strSql.Append("DaysDueAfterPrerequisite=@DaysDueAfterPrerequisite,");
			strSql.Append("OwnerRoleId=@OwnerRoleId,");
			strSql.Append("WarningEmailId=@WarningEmailId,");
			strSql.Append("OverdueEmailId=@OverdueEmailId,");
			strSql.Append("CompletionEmailId=@CompletionEmailId,");
			strSql.Append("SequenceNumber=@SequenceNumber,");
            strSql.Append("Description=@Description,");
            strSql.Append("DaysFromCreation=@DaysFromCreation,");
            strSql.Append("ExternalViewing=@ExternalViewing");
			strSql.Append(" where TemplTaskId=@TemplTaskId ");
			SqlParameter[] parameters = {
					new SqlParameter("@TemplTaskId", SqlDbType.Int,4),
					new SqlParameter("@WflStageId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,255),
					new SqlParameter("@Enabled", SqlDbType.Bit,1),
					new SqlParameter("@Type", SqlDbType.SmallInt,2),
					new SqlParameter("@DaysDueFromCoe", SqlDbType.SmallInt,2),
					new SqlParameter("@PrerequisiteTaskId", SqlDbType.Int,4),
					new SqlParameter("@DaysDueAfterPrerequisite", SqlDbType.SmallInt,2),
					new SqlParameter("@OwnerRoleId", SqlDbType.Int,4),
					new SqlParameter("@WarningEmailId", SqlDbType.Int,4),
					new SqlParameter("@OverdueEmailId", SqlDbType.Int,4),
					new SqlParameter("@CompletionEmailId", SqlDbType.Int,4),
					new SqlParameter("@SequenceNumber", SqlDbType.SmallInt,2),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@DaysFromCreation", SqlDbType.SmallInt,2),
                    new SqlParameter("@ExternalViewing", SqlDbType.Bit,1)};
			parameters[0].Value = model.TemplTaskId;
			parameters[1].Value = model.WflStageId;
			parameters[2].Value = model.Name;
			parameters[3].Value = model.Enabled;
			parameters[4].Value = model.Type;
            if (model.DaysDueFromCoe == null)
            {
                parameters[5].Value = DBNull.Value;
            }
            else
            {
                parameters[5].Value = model.DaysDueFromCoe;
            }
			parameters[6].Value = model.PrerequisiteTaskId;
            if (model.DaysDueAfterPrerequisite == null)
            {
                parameters[7].Value = DBNull.Value;
            }
            else
            {
                parameters[7].Value = model.DaysDueAfterPrerequisite;
            }
			parameters[8].Value = model.OwnerRoleId;
            if (model.WarningEmailId == 0 || model.WarningEmailId == null)
            {
                parameters[9].Value = DBNull.Value;
            }
            else
            {
                parameters[9].Value = model.WarningEmailId;
            }
            if (model.OverdueEmailId == 0 || model.OverdueEmailId == null)
            {
                parameters[10].Value = DBNull.Value;
            }
            else
            {
                parameters[10].Value = model.OverdueEmailId;
            }
            if (model.CompletionEmailId == 0 || model.CompletionEmailId == null)
            {
                parameters[11].Value = DBNull.Value;
            }
            else
            {
                parameters[11].Value = model.CompletionEmailId;
            }

			parameters[12].Value = model.SequenceNumber;
            parameters[13].Value = model.Description;
            if (model.DaysFromCreation == null)
            {
                parameters[14].Value = DBNull.Value;
            }
            else
            {
                parameters[14].Value = model.DaysFromCreation;
            }

            parameters[15].Value = model.ExternalViewing;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int TemplTaskId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Template_Wfl_Tasks ");
			strSql.Append(" where TemplTaskId=@TemplTaskId ");
			SqlParameter[] parameters = {
					new SqlParameter("@TemplTaskId", SqlDbType.Int,4)};
			parameters[0].Value = TemplTaskId;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LPWeb.Model.Template_Wfl_Tasks GetModel(int TemplTaskId)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 TemplTaskId,WflStageId,Name,Enabled,Type,DaysDueFromCoe,PrerequisiteTaskId,DaysDueAfterPrerequisite,OwnerRoleId,WarningEmailId,OverdueEmailId,CompletionEmailId,SequenceNumber,Description,DaysFromCreation,ExternalViewing from Template_Wfl_Tasks ");
			strSql.Append(" where TemplTaskId=@TemplTaskId ");
			SqlParameter[] parameters = {
					new SqlParameter("@TemplTaskId", SqlDbType.Int,4)};
			parameters[0].Value = TemplTaskId;

			LPWeb.Model.Template_Wfl_Tasks model=new LPWeb.Model.Template_Wfl_Tasks();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["TemplTaskId"].ToString()!="")
				{
					model.TemplTaskId=int.Parse(ds.Tables[0].Rows[0]["TemplTaskId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["WflStageId"].ToString()!="")
				{
					model.WflStageId=int.Parse(ds.Tables[0].Rows[0]["WflStageId"].ToString());
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
				if(ds.Tables[0].Rows[0]["Type"].ToString()!="")
				{
					model.Type=int.Parse(ds.Tables[0].Rows[0]["Type"].ToString());
				}
				if(ds.Tables[0].Rows[0]["DaysDueFromCoe"].ToString()!="")
				{
					model.DaysDueFromCoe=int.Parse(ds.Tables[0].Rows[0]["DaysDueFromCoe"].ToString());
				}
				if(ds.Tables[0].Rows[0]["PrerequisiteTaskId"].ToString()!="")
				{
					model.PrerequisiteTaskId=int.Parse(ds.Tables[0].Rows[0]["PrerequisiteTaskId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["DaysDueAfterPrerequisite"].ToString()!="")
				{
					model.DaysDueAfterPrerequisite=int.Parse(ds.Tables[0].Rows[0]["DaysDueAfterPrerequisite"].ToString());
				}
				if(ds.Tables[0].Rows[0]["OwnerRoleId"].ToString()!="")
				{
					model.OwnerRoleId=int.Parse(ds.Tables[0].Rows[0]["OwnerRoleId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["WarningEmailId"].ToString()!="")
				{
					model.WarningEmailId=int.Parse(ds.Tables[0].Rows[0]["WarningEmailId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["OverdueEmailId"].ToString()!="")
				{
					model.OverdueEmailId=int.Parse(ds.Tables[0].Rows[0]["OverdueEmailId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["CompletionEmailId"].ToString()!="")
				{
					model.CompletionEmailId=int.Parse(ds.Tables[0].Rows[0]["CompletionEmailId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["SequenceNumber"].ToString()!="")
				{
					model.SequenceNumber=int.Parse(ds.Tables[0].Rows[0]["SequenceNumber"].ToString());
				}
                model.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                if (ds.Tables[0].Rows[0]["DaysFromCreation"].ToString() != "")
                {
                    model.DaysFromCreation = int.Parse(ds.Tables[0].Rows[0]["DaysFromCreation"].ToString());
                }

                if (ds.Tables[0].Rows[0]["ExternalViewing"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["ExternalViewing"].ToString() == "1") || (ds.Tables[0].Rows[0]["ExternalViewing"].ToString().ToLower() == "true"))
                    {
                        model.ExternalViewing = true;
                    }
                    else
                    {
                        model.ExternalViewing = false;
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
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select TemplTaskId,WflStageId,Name,Enabled,Type,DaysDueFromCoe,PrerequisiteTaskId,DaysDueAfterPrerequisite,OwnerRoleId,WarningEmailId,OverdueEmailId,CompletionEmailId,SequenceNumber,Description,DaysFromCreation ");
			strSql.Append(" FROM Template_Wfl_Tasks ");
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
            strSql.Append(" TemplTaskId,WflStageId,Name,Enabled,Type,DaysDueFromCoe,PrerequisiteTaskId,DaysDueAfterPrerequisite,OwnerRoleId,WarningEmailId,OverdueEmailId,CompletionEmailId,SequenceNumber,Description,DaysFromCreation ");
			strSql.Append(" FROM Template_Wfl_Tasks ");
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
			parameters[0].Value = "Template_Wfl_Tasks";
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

