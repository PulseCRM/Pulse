using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Settings
{
    public partial class SettingsWebService : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Create Group
        /// 刘洋 2010-09-01
        /// </summary>
        /// <param name="sGroupName"></param>
        /// <returns></returns>
        [WebMethod]
        public static string CreateGroup(string sGroupName)
        {
            string sError = string.Empty;

            #region 检查是否存在
            Groups GroupManager = new Groups();
            bool bIsExist = GroupManager.IsExist_Create(sGroupName);
            if (bIsExist == true)
            {
                sError = "The name is already used by another group. Please use another group name.";
                return sError;
            }

            #endregion

            try
            {
                sError = GroupManager.CreateGroup(sGroupName).ToString();
            }
            catch (Exception ex)
            {
                sError = "Fail to Create Group.";
            }

            return sError;
        }
        

        /// <summary>
        /// Create Group
        /// Shawn 2010-09-02
        /// </summary>
        /// <param name="sGroupName"></param>
        /// <returns></returns>
        [WebMethod]
        public static string CreateDivision(string sDivisionName)
        {
            string sError = string.Empty;

            sDivisionName = sDivisionName.Trim();
            #region 检查是否存在

            Divisions divManager = new Divisions();
            bool bIsExist = divManager.IsExist_CreateBase(sDivisionName);
            if (bIsExist == true)
            {
                sError = "The name is already used by another group. Please use another group name.";
                return sError;
            }

            #endregion

            try
            {
                sError = divManager.CreateDivision(sDivisionName).ToString();
            }
            catch (Exception ex)
            {
                sError = "Fail to Create Division.";
            }

            return sError;
        }
        
        /// <summary>
        /// Create Group
        /// Shawn 2010-09-02
        /// </summary>
        /// <param name="sGroupName"></param>
        /// <returns></returns>
        [WebMethod]
        public static string CreateBranch(string sBranchName)
        {
            string sError = string.Empty;
            sBranchName = sBranchName.Trim();
            #region 检查是否存在 

            Branches branchManager = new Branches();
            bool bIsExist = branchManager.IsExist_CreateBase(sBranchName);
            if (bIsExist == true)
            {
                sError = "The name is already used by another branch. Please use another branch name.";
                return sError;
            }

            #endregion

            try
            {
                sError = branchManager.CreateBranch(sBranchName).ToString();
            }
            catch (Exception ex)
            {
                sError = "Fail to Create Branch.";
            }

            return sError;
        }

        /// <summary>
        /// Create Region
        /// Carl 2010-09-02
        /// </summary>
        /// <param name="sRegionName">region name</param>
        /// <returns></returns>
        [WebMethod]
        public static string CreateRegion(string sRegionName)
        {
            string sError = string.Empty;

            #region 检查是否存在

            var bllRegions = new Regions();
            bool bIsExist = bllRegions.IsExist_CreateBase(sRegionName);
            if (bIsExist == true)
            {
                sError = "The name is already used by another region. Please use another region name.";
                return sError;
            }

            #endregion

            try
            {
                sError = bllRegions.CreateRegion(sRegionName).ToString();
            }
            catch (Exception ex)
            {
                sError = "Fail to Create Region.";
            }

            return sError;
        }
    }
}