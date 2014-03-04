using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LPWeb.DAL
{
    public class Template_Email_Attachments : Template_Email_AttachmentsBase
    {
        public DataSet GetListWithOutFileImage(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TemplAttachId,TemplEmailId,Enabled,Name,FileType ");
            strSql.Append(" FROM Template_Email_Attachments ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            strSql.Append(" ORDER BY [Name] asc ");
            return DbHelperSQL.Query(strSql.ToString());
        }


        /// <summary>
        /// 删除多条数据
        /// </summary>
        public void DeleteIDList(string IDList)
        {
            if (string.IsNullOrEmpty(IDList))
            {
                return;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Template_Email_Attachments ");
            strSql.Append(" where TemplAttachId IN ( ");

            strSql.Append(IDList + " ) ");
            DbHelperSQL.ExecuteSql(strSql.ToString());
        }

    }
}
