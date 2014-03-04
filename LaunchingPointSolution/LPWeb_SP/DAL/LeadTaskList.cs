using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LPWeb.DAL
{
    public class LeadTaskList : LeadTaskListBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrderBy"></param>
        /// <returns></returns>
        public DataTable GetLeadTaskList(string sWhere, string sOrderBy)
        {
            string sSql0 = "select * from LeadTaskList where 1=1 " + sWhere + " order by " + sOrderBy;
            DataTable WorkflowList = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql0);
            return WorkflowList;
        }
    }
}
