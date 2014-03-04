using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LPWeb.BLL;

namespace LPWeb.Settings
{
    /// <summary>
    /// Summary description for SettingsHandler
    /// </summary>
    public class SettingsHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string SettingName = context.Request.QueryString["SettingName"];
            string SettingType = context.Request.QueryString["SettingType"];
            context.Response.ContentType = "text/plain";
            context.Response.Write(GetReturnValue(SettingType, SettingName));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private string GetReturnValue(string SettingType, string SettingName)
        {
            string ReturnValue = string.Empty;
            switch (SettingType)
            {
                case "Branch":
                    ReturnValue = CreateBranch(SettingName);
                    break;
                case "Division":
                    ReturnValue = CreateDivision(SettingName);
                    break;
                case "Region":
                    ReturnValue = CreateRegion(SettingName);
                    break;
                default:
                    ReturnValue = string.Empty;
                    break;
            }

            return ReturnValue;
        }

        private string CreateRegion(string sRegionName)
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

        private string CreateBranch(string sBranchName)
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
            catch
            {
                sError = "Fail to Create Branch.";
            }

            return sError;
        }


        private string CreateDivision(string sDivisionName)
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
            catch
            {
                sError = "Fail to Create Division.";
            }

            return sError;
        }
    }
}