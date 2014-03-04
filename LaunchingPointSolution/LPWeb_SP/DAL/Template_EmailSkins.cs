using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    public class Template_EmailSkins : Template_EmailSkinsBase
    {
        public DataTable GetEmailSkinList(string sWhere, string sOrderby)
        {
            string sSql = "select * from Template_EmailSkins where 1=1 " + sWhere + " order by " + sOrderby;
            return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        }


        public DataSet GetListForGridView(int PageSize, int PageIndex, string strWhere, out int count, string orderName, int orderType)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 1000),
					new SqlParameter("@QueryName", SqlDbType.VarChar, 255),
					new SqlParameter("@OrderName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					new SqlParameter("@totalCount", SqlDbType.Int),
					};
            parameters[0].Value = "(SELECT [EmailSkinId],[Name],[Desc],[HTMLBody],[Enabled],[Default],(SELECT COUNT(1) FROM Template_Email te WHERE te.EmailSkinId = tes.EmailSkinId) as EmailTemplTotal  FROM [dbo].[Template_EmailSkins] tes  ) as t";
            parameters[1].Value = "*";
            parameters[2].Value = orderName;
            parameters[3].Value = PageSize;
            parameters[4].Value = PageIndex;
            parameters[5].Value = orderType;
            parameters[6].Value = "1=1 " + strWhere;
            parameters[7].Direction = ParameterDirection.Output;

            var ds = DbHelperSQL.RunProcedure("lpsp_GetRecordByPageOrder", parameters, "ds");

            count = int.Parse(parameters[7].Value.ToString());
            return ds;
        }

        public void SetDisable(string Ids)
        {
            string sSql = "UPDATE [Template_EmailSkins] Set [Enabled] = 0,[Default] = 0 where EmailSkinId in (" + Ids + ")";
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
        }


        public void SetTmpEmail_SkinIdNull(string Ids)
        {

            string sSql = "UPDATE Template_Email SET EmailSkinId  = NULL  WHERE EmailSkinId IN (" + Ids + ")";

            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);

        }

        #region neo

        public bool IsDulplicated_Add(string sEmailSkinName)
        {
            string sSql = "select count(1) from Template_EmailSkins where Name=@Name";
            SqlCommand SqlCmd = new SqlCommand(sSql);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sEmailSkinName);

            int iCount = Convert.ToInt32(LPWeb.DAL.DbHelperSQL.ExecuteScalar(SqlCmd));
            if (iCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void InsertEmailSkin(string sEmailSkinName, string sDesc, string sHtmlBody, bool bEnabled, bool bDefault)
        {
            string sSql = "insert into dbo.Template_EmailSkins (Name,[Desc],HTMLBody,Enabled,[Default]) values (@Name,@Desc,@HTMLBody,@Enabled,@Default);";
            if (bDefault == true)
            {
                sSql += "update Template_EmailSkins set [Default]=0;update Template_EmailSkins set [Default]=1 where EmailSkinId=SCOPE_IDENTITY();";
            }

            SqlCommand SqlCmd = new SqlCommand(sSql);

            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sEmailSkinName);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Desc", SqlDbType.NVarChar, sDesc);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@HTMLBody", SqlDbType.NVarChar, sHtmlBody);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, bEnabled);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Default", SqlDbType.Bit, bDefault);

            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        public bool IsDulplicated_Edit(int iEmailSkinID, string sEmailSkinName)
        {
            string sSql = "select count(1) from Template_EmailSkins where Name=@Name and EmailSkinID <> " + iEmailSkinID;
            SqlCommand SqlCmd = new SqlCommand(sSql);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sEmailSkinName);

            int iCount = Convert.ToInt32(LPWeb.DAL.DbHelperSQL.ExecuteScalar(SqlCmd));
            if (iCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateEmailSkin(int iEmailSkinID, string sEmailSkinName, string sDesc, string sHtmlBody, bool bEnabled, bool bDefault)
        {
            string sSql = "update Template_EmailSkins set Name=@Name,[Desc]=@Desc,HTMLBody=@HTMLBody,Enabled=@Enabled,[Default]=@Default where EmailSkinID=" + iEmailSkinID + ";";
            if (bDefault == true)
            {
                sSql += "update Template_EmailSkins set [Default]=0;update Template_EmailSkins set [Default]=1 where EmailSkinId=" + iEmailSkinID + ";";
            }

            SqlCommand SqlCmd = new SqlCommand(sSql);

            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Name", SqlDbType.NVarChar, sEmailSkinName);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Desc", SqlDbType.NVarChar, sDesc);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@HTMLBody", SqlDbType.NVarChar, sHtmlBody);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Enabled", SqlDbType.Bit, bEnabled);
            LPWeb.DAL.DbHelperSQL.AddSqlParameter(SqlCmd, "@Default", SqlDbType.Bit, bDefault);

            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(SqlCmd);
        }

        public DataTable GetEmailSkinInfo(int iEmailSkinID)
        {
            string sSql = "select * from Template_EmailSkins where EmailSkinID=" + iEmailSkinID;
            return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        }

        public void CloneEmailSkin(int iEmailSkinID)
        {
            string sSql = "insert into Template_EmailSkins (Name,[Desc],HTMLBody,Enabled,[Default]) "
                        + "select Name+ ' Copy' as Name,[Desc],HTMLBody,Enabled,0 as [Default] from Template_EmailSkins where EmailSkinID=" + iEmailSkinID;
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
        }

        public void DeleteEmailSkin(int iEmailSkinID)
        {
            string sSql = "update Template_Email set EmailSkinId  = null  where EmailSkinId=" + iEmailSkinID + ";delete from Template_EmailSkins where EmailSkinID=" + iEmailSkinID;
            LPWeb.DAL.DbHelperSQL.ExecuteNonQuery(sSql);
        }

        #endregion
    }
}
