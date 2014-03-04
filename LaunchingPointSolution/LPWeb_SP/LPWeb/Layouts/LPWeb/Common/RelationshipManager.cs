using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LPWeb.BLL;
using System.Data;

namespace LPWeb.Common
{
    public class RelatedContact
    {
        public RelatedContact()
        {
            ContactId = 0;
            Relationship = string.Empty;
            ContactName = string.Empty;
            Direction = string.Empty;
        }
        public int ContactId;
        public string Relationship;
        public string ContactName;
        public string Direction;    // From or To
    }

    public class RelationshipManager
    {
        Contacts contact = new Contacts();
        public string GetRelationship(int ContactId1, int ContactId2)
        {
            string RelName = string.Empty;
            try
            {
                DataSet ds = contact.GetRelationship(ContactId1, ContactId2);
                if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                {
                    RelName = string.Empty;
                }
                else
                {
                    RelName = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch
            { }
            return RelName;
        }

        /// <summary>
        /// get related contact list
        /// </summary>
        /// <param name="ContactId"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="iEndIndex"></param>
        /// <returns></returns>
        public List<RelatedContact> GetRelatedContacts(int ContactId, int iStartIndex, int iEndIndex)
        {
            List<RelatedContact> list = new List<RelatedContact>();
            DataTable RelatedContactList = contact.GetRelatedContacts(ContactId, iStartIndex, iEndIndex);
            
            foreach (DataRow row in RelatedContactList.Rows)
            {
                RelatedContact rc = new RelatedContact();
                rc.ContactId = int.Parse(row["RelContactID"].ToString());
                rc.ContactName = row["ContactName"].ToString();
                rc.Relationship = row["Relationship"].ToString();
                rc.Direction = row["Direction"].ToString();
                list.Add(rc);
            }

            return list;
        }
    }
}
