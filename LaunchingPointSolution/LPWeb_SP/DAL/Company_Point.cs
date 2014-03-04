using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// 数据访问类Company_Point。
	/// </summary>
    public class Company_Point : Company_PointBase
	{
		public Company_Point()
		{}
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(LPWeb.Model.Company_Point model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Company_Point(");
            strSql.Append("WinpointIniPath,PointFieldIDMappingFile,CardexFile,PointImportIntervalMinutes,MasterSource,TracToolsLogin,TracToolsPwd,Auto_ConvertLead,AutoApplyProcessingWorkflow,AutoApplyProspectWorkflow,Enable_MultiBranchFolders)");
            strSql.Append(" values (");
            strSql.Append("@WinpointIniPath,@PointFieldIDMappingFile,@CardexFile,@PointImportIntervalMinutes,@MasterSource,@TracToolsLogin,@TracToolsPwd,@Auto_ConvertLead,@AutoApplyProcessingWorkflow,@AutoApplyProspectWorkflow,@Enable_MultiBranchFolders)");
            SqlParameter[] parameters = {
					new SqlParameter("@WinpointIniPath", SqlDbType.NVarChar,255),
					new SqlParameter("@PointFieldIDMappingFile", SqlDbType.NVarChar,255),
					new SqlParameter("@CardexFile", SqlDbType.NVarChar,255),
					new SqlParameter("@PointImportIntervalMinutes", SqlDbType.SmallInt,2),
                    new SqlParameter("@MasterSource",SqlDbType.NVarChar,50),
                    new SqlParameter("@TracToolsLogin",SqlDbType.NVarChar,50),
                    new SqlParameter("@TracToolsPwd",SqlDbType.NVarChar,25),
                    new SqlParameter("@Auto_ConvertLead",SqlDbType.Bit),
                    new SqlParameter("@AutoApplyProcessingWorkflow",SqlDbType.Bit),
                    new SqlParameter("@AutoApplyProspectWorkflow",SqlDbType.Bit),
                    new SqlParameter("@Enable_MultiBranchFolders",SqlDbType.Bit)
                                        };
            parameters[0].Value = model.WinpointIniPath;
            parameters[1].Value = model.PointFieldIDMappingFile;
            parameters[2].Value = model.CardexFile;
            parameters[3].Value = model.PointImportIntervalMinutes;
            parameters[4].Value = model.MasterSource;
            parameters[5].Value = model.TracToolsLogin;
            parameters[6].Value = model.TracToolsPwd;
            parameters[7].Value = model.Auto_ConvertLead;
            parameters[8].Value = model.AutoApplyProcessingWorkflow;
            parameters[9].Value = model.AutoApplyProspectWorkflow;
            parameters[10].Value = model.Enable_MultiBranchFolders;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Company_Point model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Company_Point set ");
            strSql.Append("WinpointIniPath=@WinpointIniPath,");
            strSql.Append("PointFieldIDMappingFile=@PointFieldIDMappingFile,");
            strSql.Append("CardexFile=@CardexFile,");
            strSql.Append("PointImportIntervalMinutes=@PointImportIntervalMinutes,");
            strSql.Append("MasterSource=@MasterSource,");
            strSql.Append("TracToolsLogin=@TracToolsLogin,");
            strSql.Append("TracToolsPwd=@TracToolsPwd,");
            strSql.Append("Auto_ConvertLead=@Auto_ConvertLead,");
            strSql.Append("AutoApplyProcessingWorkflow=@AutoApplyProcessingWorkflow,");
            strSql.Append("AutoApplyProspectWorkflow=@AutoApplyProspectWorkflow,");
            strSql.Append("Enable_MultiBranchFolders=@Enable_MultiBranchFolders");
            //strSql.Append(" where ");
            SqlParameter[] parameters = {
					new SqlParameter("@WinpointIniPath", SqlDbType.NVarChar,255),
					new SqlParameter("@PointFieldIDMappingFile", SqlDbType.NVarChar,255),
					new SqlParameter("@CardexFile", SqlDbType.NVarChar,255),
					new SqlParameter("@PointImportIntervalMinutes", SqlDbType.SmallInt,2),
                    new SqlParameter("@MasterSource", SqlDbType.NVarChar,50),
                    new SqlParameter("@TracToolsLogin", SqlDbType.NVarChar,50),
                    new SqlParameter("@TracToolsPwd", SqlDbType.NVarChar,25),
                    new SqlParameter("@Auto_ConvertLead",SqlDbType.Bit),
                    new SqlParameter("@AutoApplyProcessingWorkflow",SqlDbType.Bit),
                    new SqlParameter("@AutoApplyProspectWorkflow",SqlDbType.Bit),
                    new SqlParameter("@Enable_MultiBranchFolders",SqlDbType.Bit)
                                        };
            parameters[0].Value = model.WinpointIniPath;
            parameters[1].Value = model.PointFieldIDMappingFile;
            parameters[2].Value = model.CardexFile;
            parameters[3].Value = model.PointImportIntervalMinutes;
            parameters[4].Value = model.MasterSource;
            parameters[5].Value = model.TracToolsLogin;
            parameters[6].Value = model.TracToolsPwd;
            parameters[7].Value = model.Auto_ConvertLead;
            parameters[8].Value = model.AutoApplyProcessingWorkflow;
            parameters[9].Value = model.AutoApplyProspectWorkflow;
            parameters[10].Value = model.Enable_MultiBranchFolders;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Company_Point GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WinpointIniPath,PointFieldIDMappingFile,CardexFile,PointImportIntervalMinutes,MasterSource,TracToolsLogin,TracToolsPwd,Auto_ConvertLead,AutoApplyProcessingWorkflow,AutoApplyProspectWorkflow,Enable_MultiBranchFolders from Company_Point ");
            //strSql.Append(" where ");
            SqlParameter[] parameters = {
};

            LPWeb.Model.Company_Point model = new LPWeb.Model.Company_Point();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                model.WinpointIniPath = ds.Tables[0].Rows[0]["WinpointIniPath"].ToString();
                model.PointFieldIDMappingFile = ds.Tables[0].Rows[0]["PointFieldIDMappingFile"].ToString();
                model.CardexFile = ds.Tables[0].Rows[0]["CardexFile"].ToString();
                if (ds.Tables[0].Rows[0]["PointImportIntervalMinutes"].ToString() != "")
                {
                    model.PointImportIntervalMinutes = int.Parse(ds.Tables[0].Rows[0]["PointImportIntervalMinutes"].ToString());
                }
                model.MasterSource = ds.Tables[0].Rows[0]["MasterSource"] != null ? ds.Tables[0].Rows[0]["MasterSource"].ToString() : "";
                model.TracToolsLogin = ds.Tables[0].Rows[0]["TracToolsLogin"] != null ? ds.Tables[0].Rows[0]["TracToolsLogin"].ToString() : "";
                model.TracToolsPwd = ds.Tables[0].Rows[0]["TracToolsPwd"] != null ? ds.Tables[0].Rows[0]["TracToolsPwd"].ToString() : "";
                model.Auto_ConvertLead = ds.Tables[0].Rows[0]["Auto_ConvertLead"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Auto_ConvertLead"]) : false;
                model.AutoApplyProcessingWorkflow = ds.Tables[0].Rows[0]["AutoApplyProcessingWorkflow"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["AutoApplyProcessingWorkflow"]) : false;
                model.AutoApplyProspectWorkflow = ds.Tables[0].Rows[0]["AutoApplyProspectWorkflow"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["AutoApplyProspectWorkflow"]) : false;
                model.Enable_MultiBranchFolders = ds.Tables[0].Rows[0]["Enable_MultiBranchFolders"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["Enable_MultiBranchFolders"]) : false;
                return model;
            }
            else
            {
                return null;
            }
        }
        #endregion  成员方法

        public DataTable GetCompany_PointInfo()
        {
            string sSql = "select * from Company_Point";
            return DbHelperSQL.ExecuteDataTable(sSql);
        }
	}
}

