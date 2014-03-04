using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPWeb.BLL
{
    public class Contact_Relationship
    {
        private readonly LPWeb.DAL.Contact_Relationship dal = new LPWeb.DAL.Contact_Relationship();

        #region neo

        /// <summary>
        /// delete contact relationship
        /// neo 2011-03-15
        /// </summary>
        /// <param name="iContactID"></param>
        /// <param name="DelContactIDArray"></param>
        /// <param name="DirectionArray"></param>
        public void DeleteContactRelationship(int iContactID, string[] DelContactIDArray, string[] DirectionArray)
        {
            dal.DeleteContactRelationshipBase(iContactID, DelContactIDArray, DirectionArray);
        }

        /// <summary>
        /// insert contact relationship
        /// neo 2011-03-15
        /// </summary>
        /// <param name="iFromContactID"></param>
        /// <param name="ToContactIDArray"></param>
        /// <param name="RelationshipTypeArray"></param>
        public void InsertContactRelationship(int iFromContactID, string[] ToContactIDArray, string[] RelationshipTypeArray)
        {
            dal.InsertContactRelationshipBase(iFromContactID, ToContactIDArray, RelationshipTypeArray);
        }

        #endregion
    }
}
