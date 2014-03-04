using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace LPWeb.DAL
{
	/// <summary>
	/// ���ݷ�����Company_Point��
	/// </summary>
    public class Company_PointBase
    {
        public Company_PointBase()
		{}
        #region  ��Ա����

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(LPWeb.Model.Company_Point model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Company_Point(");
            strSql.Append("WinpointIniPath,PointFieldIDMappingFile,CardexFile,PointImportIntervalMinutes)");
            strSql.Append(" values (");
            strSql.Append("@WinpointIniPath,@PointFieldIDMappingFile,@CardexFile,@PointImportIntervalMinutes)");
            SqlParameter[] parameters = {
					new SqlParameter("@WinpointIniPath", SqlDbType.NVarChar,255),
					new SqlParameter("@PointFieldIDMappingFile", SqlDbType.NVarChar,255),
					new SqlParameter("@CardexFile", SqlDbType.NVarChar,255),
					new SqlParameter("@PointImportIntervalMinutes", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.WinpointIniPath;
            parameters[1].Value = model.PointFieldIDMappingFile;
            parameters[2].Value = model.CardexFile;
            parameters[3].Value = model.PointImportIntervalMinutes;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(LPWeb.Model.Company_Point model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Company_Point set ");
            strSql.Append("WinpointIniPath=@WinpointIniPath,");
            strSql.Append("PointFieldIDMappingFile=@PointFieldIDMappingFile,");
            strSql.Append("CardexFile=@CardexFile,");
            strSql.Append("PointImportIntervalMinutes=@PointImportIntervalMinutes");
            //strSql.Append(" where ");
            SqlParameter[] parameters = {
					new SqlParameter("@WinpointIniPath", SqlDbType.NVarChar,255),
					new SqlParameter("@PointFieldIDMappingFile", SqlDbType.NVarChar,255),
					new SqlParameter("@CardexFile", SqlDbType.NVarChar,255),
					new SqlParameter("@PointImportIntervalMinutes", SqlDbType.SmallInt,2)};
            parameters[0].Value = model.WinpointIniPath;
            parameters[1].Value = model.PointFieldIDMappingFile;
            parameters[2].Value = model.CardexFile;
            parameters[3].Value = model.PointImportIntervalMinutes;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete()
        {
            //�ñ���������Ϣ�����Զ�������/�����ֶ�
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Company_Point ");
            strSql.Append(" where ");
            SqlParameter[] parameters = {
};

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LPWeb.Model.Company_Point GetModel()
        {
            //�ñ���������Ϣ�����Զ�������/�����ֶ�
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WinpointIniPath,PointFieldIDMappingFile,CardexFile,PointImportIntervalMinutes from Company_Point ");
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
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select WinpointIniPath,PointFieldIDMappingFile,CardexFile,PointImportIntervalMinutes ");
            strSql.Append(" FROM Company_Point ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// ���ǰ��������
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" WinpointIniPath,PointFieldIDMappingFile,CardexFile,PointImportIntervalMinutes ");
            strSql.Append(" FROM Company_Point ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        /// ��ҳ��ȡ�����б�
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
            parameters[0].Value = "Company_Point";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage", parameters, "ds");
        }

        #endregion  ��Ա����
	}
}

