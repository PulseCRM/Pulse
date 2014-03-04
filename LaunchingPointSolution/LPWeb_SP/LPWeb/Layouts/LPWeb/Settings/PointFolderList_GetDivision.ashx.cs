using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using LPWeb.Common;
using LPWeb.DAL;
using Utilities;

namespace LPWeb.Settings
{
    /// <summary>
    /// Summary description for PointFolderList_GetDivision
    /// </summary>
    public class PointFolderList_GetDivision : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string sDivisionResult = "";
            string sSearchID = "0";
            string sType = context.Request.QueryString["Type"].ToString();
            if (sType == "Region")
            {
                int? intRegionID = null;
                sSearchID = context.Request.QueryString["RegionID"].ToString();
                if (sSearchID != "")
                {
                    intRegionID = Convert.ToInt32(sSearchID);
                }
                //DataTable dtDivision = this.GetDivisionData(sSearchID);
                DataTable dtDivision = PageCommon.GetOrgStructureDataSourceByLoginUser(intRegionID,null,true).Tables[1];
                foreach (DataRow drDivision in dtDivision.Rows)
                {
                    sDivisionResult += drDivision["DivisionId"].ToString() + "|" + drDivision["Name"].ToString() + ";";
                }

                DataTable dtBranch = this.GetBranchData(sSearchID,"0");
                sDivisionResult += "@";
                foreach (DataRow drBranch in dtBranch.Rows)
                {
                    sDivisionResult += drBranch["BranchId"].ToString() + "|" + drBranch["Name"].ToString() + ";";
                }
            }
            else if (sType == "Division")
            {
                int? intDivisionID = null;
                sSearchID = context.Request.QueryString["DivisionID"].ToString();
                if (sSearchID != "")
                {
                    intDivisionID = Convert.ToInt32(sSearchID);
                }
                //DataTable dtBranch = this.GetBranchData("0", sSearchID);
                DataTable dtBranch = PageCommon.GetOrgStructureDataSourceByLoginUser(null, intDivisionID,true).Tables[2];
                foreach (DataRow drBranch in dtBranch.Rows)
                {
                    sDivisionResult += drBranch["BranchId"].ToString() + "|" + drBranch["Name"].ToString() + ";";
                }
            }
            
            context.Response.Write(sDivisionResult);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Get division data
        /// </summary>
        /// <returns></returns>
        private DataTable GetDivisionData(string sRegionID)
        {
            DataTable dtDivision;
            try
            {
                Divisions divisionManager = new Divisions();
                //Binding Division
                string sCondition = " Enabled='true'";
                if (sRegionID != "" && sRegionID != "0")
                {
                    sCondition += " AND RegionID = " + sRegionID;
                }
                dtDivision = divisionManager.GetList(sCondition).Tables[0];

                DataRow drNewDivision = dtDivision.NewRow();
                drNewDivision["DivisionId"] = 0;
                drNewDivision["Name"] = "All Divisions";
                dtDivision.Rows.InsertAt(drNewDivision, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtDivision;
        }
        /// <summary>
        /// Get Branch data
        /// </summary>
        /// <returns></returns>
        private DataTable GetBranchData(string sRegionID, string sDivisionID)
        {
            DataTable dtBranches;
            try
            {
                Branches branchManager = new Branches(); ;
                //Binding Branch
                string sCondition = " Enabled='true'";
                if (sRegionID != "" && sRegionID != "0")
                {
                    sCondition += " AND RegionID = " + sRegionID;
                }
                if (sDivisionID != "" && sDivisionID != "0")
                {
                    sCondition += " AND DivisionID = " + sDivisionID;
                }
                dtBranches = branchManager.GetList(sCondition).Tables[0];

                DataRow drNewBranches = dtBranches.NewRow();
                drNewBranches["BranchId"] = 0;
                drNewBranches["Name"] = "All Branches";
                dtBranches.Rows.InsertAt(drNewBranches, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtBranches;
        }
    }
}