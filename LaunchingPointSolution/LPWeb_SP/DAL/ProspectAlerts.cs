using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LPWeb.DAL
{
    public class ProspectAlerts : ProspectAlertsBase
    {
        /// <summary>
        /// 得到Prospect 第一个Task Alert 的ID
        /// Alex 2011-01-24
        /// </summary>
        /// <param name="iProspectID"></param>
        /// <returns></returns>
        public int GetProspectTaskAlertID(int iContactID)
        {
            int iProspectAlertId = 0;
            string sSql = "select top 1 ProspectAlertId from ProspectAlerts where ContactId=" + iContactID + " and AlertType='Task Alert' order by DueDate";
            DataTable dt = DbHelperSQL.ExecuteDataTable(sSql);
            if (dt.Rows.Count > 0)
            {
                iProspectAlertId = Convert.ToInt32(dt.Rows[0][0]);
            }
            return iProspectAlertId;
        }

        /// <summary>
        /// get prospect alert info
        /// neo 2011-03-17
        /// </summary>
        /// <param name="iProspectAlertID"></param>
        /// <returns></returns>
        public DataTable GetProspectAlertInfoBase(int iProspectAlertID) 
        {
            string sSql = "select * from ProspectAlerts where ProspectAlertId=" + iProspectAlertID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }
    }
}
