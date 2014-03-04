using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    public class Email_AttachmentsTemp : Email_AttachmentsTempBase
    {
        public Email_AttachmentsTemp()
        { }


        public DataSet GetList(int TemplEmailId,string Token )
        {
            string strSql = string.Format(@"select ('s'+cast(TemplAttachId as nvarchar(10))) AS TemplAttachId,TemplEmailId,[Enabled],[Name],FileType 
	                                        FROM Template_Email_Attachments 
	                                        where TemplEmailId ={0}  AND [Enabled] = 1
	                                        and TemplAttachId not in ( 
		                                        select TemplAttachId from Email_AttachmentsTemp 
		                                        where (TemplAttachId is not null and TemplAttachId<>'' ) and  Token='{1}')
                                        union 
                                        select ('c'+cast(id as nvarchar(10))) TemplAttachId,{0} AS TemplEmailId ,1 AS [Enabled],[Name],FileType 
	                                        FROM Email_AttachmentsTemp where Token='{1}' AND (TemplAttachId is null OR TemplAttachId='' )"
                , TemplEmailId, Token);


            return DbHelperSQL.Query(strSql);
        }


        public DataSet GetListWithFileImage(int TemplEmailId, string Token)
        {
            string strSql = string.Format(@"select ('s'+cast(TemplAttachId as nvarchar(10))) AS TemplAttachId,TemplEmailId,[Enabled],[Name],FileType,FileImage 
	                                        FROM Template_Email_Attachments 
	                                        where TemplEmailId ={0}  AND [Enabled] = 1
	                                        and TemplAttachId not in ( 
		                                        select TemplAttachId from Email_AttachmentsTemp 
		                                        where (TemplAttachId is not null and TemplAttachId<>'' ) and  Token='{1}')
                                        union 
                                        select ('c'+cast(id as nvarchar(10))) TemplAttachId,{0} AS TemplEmailId ,1 AS [Enabled],[Name],FileType,FileImage
	                                        FROM Email_AttachmentsTemp where Token='{1}' AND (TemplAttachId is null OR TemplAttachId='' )"
                , TemplEmailId, Token);


            return DbHelperSQL.Query(strSql);
        }

        public bool DeleteByToken(string Token)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Email_AttachmentsTemp ");
            strSql.Append(" where Token=@Token");
            SqlParameter[] parameters = {
					new SqlParameter("@Token", SqlDbType.NVarChar,255)
};
            parameters[0].Value = Token;

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

    }
}
