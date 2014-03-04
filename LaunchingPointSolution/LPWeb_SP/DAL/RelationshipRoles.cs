using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LPWeb.DAL
{
    public class RelationshipRoles
    {
        #region neo

        /// <summary>
        /// get relationship type list
        /// neo 2011-03-15
        /// </summary>
        /// <returns></returns>
        public DataTable GetRelationshipTypeListBase()
        {
            string sSql = "select *, RelToName +'-'+ RelFromName as RelationshipType from RelationshipRoles";
            return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        }

        /// <summary>
        /// get to-relationship list
        /// neo 2011-03-26
        /// </summary>
        /// <returns></returns>
        public DataTable GetToRelationshipListBase()
        {
            string sSql = "select distinct RelToName from RelationshipRoles order by RelToName";
            return LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        }

        #endregion
    }
}
