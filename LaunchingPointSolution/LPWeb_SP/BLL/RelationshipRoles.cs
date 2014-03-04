using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LPWeb.BLL
{
    public class RelationshipRoles
    {
        private readonly LPWeb.DAL.RelationshipRoles dal = new LPWeb.DAL.RelationshipRoles();

        #region neo

        /// <summary>
        /// get relationship type list
        /// neo 2011-03-15
        /// </summary>
        /// <returns></returns>
        public DataTable GetRelationshipTypeList()
        {
            return dal.GetRelationshipTypeListBase();
        }

        /// <summary>
        /// get to-relationship list
        /// neo 2011-03-26
        /// </summary>
        /// <returns></returns>
        public DataTable GetToRelationshipList()
        {
            return dal.GetToRelationshipListBase();
        }

        #endregion
    }
}
