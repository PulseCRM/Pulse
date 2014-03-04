using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Microsoft.SharePoint;
using Microsoft.Office.Server.UserProfiles;

namespace LPWeb.MasterPage
{
    public partial class Super : System.Web.UI.MasterPage
    {
        public string sMyEmailInboxURL = string.Empty;
        public string sMyCalendarURL = string.Empty;
        public string sRatesURL = string.Empty;
        public string sUserRecentItems = string.Empty;
		public string sHomePageUserRecentItems = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            LPWeb.Common.LoginUser login = new Common.LoginUser();
            if (!login.userRole.CompanySetup || !login.userRole.OtherLoanAccess)
            {
                hfdUserPre.Value = "0";
            }
            else
            {
                hfdUserPre.Value = "1";
            }
            //LPWeb.BLL.Users bllUser = new LPWeb.BLL.Users();
            //LPWeb.BLL.Roles bllRole = new LPWeb.BLL.Roles();
            //LPWeb.Model.Roles modelRole = new LPWeb.Model.Roles();

            ////string sLoginUserId = HttpContext.Current.User.Identity.Name;
            //string sLoginUserId = "SPSTESTLO2_2B1";// "SPSTESTReg1Ex";//todo:hard-code
            //if (sLoginUserId.IndexOf("\\") >= 0)
            //{
            //    sLoginUserId = sLoginUserId.Substring(sLoginUserId.LastIndexOf("\\") + 1);
            //}

            //DataTable dtUserInfo = bllUser.GetUserList(" AND Username='" + sLoginUserId + "'");
            //if (dtUserInfo == null || dtUserInfo.Rows.Count == 0)
            //{
            //    HttpContext.Current.Response.Redirect("../NoPermission.htm");
            //    return;
            //}

            //int roleID = Convert.ToInt32(dtUserInfo.Rows[0]["RoleId"].ToString());
            //if (roleID != 0)
            //{
            //    modelRole = bllRole.GetModel(roleID);
            //}

            //if (!modelRole.CompanySetup || !modelRole.OtherLoanAccess)
            //{
            //    hfdUserPre.Value = "0";
            //}
            //else
            //{
            //    hfdUserPre.Value = "1";
            //}

            List<Int32> LUserRecentList = login.RecentItems;
            foreach (var recentItem in LUserRecentList)
            { 
                var _bUserRecentItems=new BLL.UserRecentItems();
                string sBorrowerName = _bUserRecentItems.GetUserRecentItemsBorrowerInfo(Convert.ToInt32(recentItem));
                string sLoanStatus = _bUserRecentItems.GetLoanStatusbyFileID(Convert.ToInt32(recentItem));
                string sCurrentPageURL = Request.Url.AbsoluteUri;
                
                if (sLoanStatus == "Prospect")
                {
                    if (sCurrentPageURL.IndexOf("FileID=") > -1)
                    {
                        sCurrentPageURL = "";
                    }

                    if (sBorrowerName.Trim() != string.Empty)
                    {
                        sUserRecentItems += "<li class='static'><a class='static menu-item' href='../Prospect/ProspectLoanDetails.aspx?FileID=" + recentItem + "&FileIDs=" + recentItem + "&FromPage=" + sCurrentPageURL + "'>";
                        sUserRecentItems += "<span class='additional-background'><span class='menu-item-text'>" + sBorrowerName + "</span></span></a></li>";

                        sHomePageUserRecentItems += "<li class='static'><a class='static menu-item' href='Prospect/ProspectLoanDetails.aspx?FileID=" + recentItem + "&FileIDs=" + recentItem + "&FromPage=" + sCurrentPageURL + "'>";
                        sHomePageUserRecentItems += "<span class='additional-background'><span class='menu-item-text'>" + sBorrowerName + "</span></span></a></li>";
                    }
                }
                else
                {
                    if (sCurrentPageURL.IndexOf("fieldid=") > -1)
                    {
                        sCurrentPageURL = "";
                    }

                    if (sBorrowerName.Trim() != string.Empty)
                    {
                        sUserRecentItems += "<li class='static'><a class='static menu-item' href='../LoanDetails/LoanDetails.aspx?fieldid=" + recentItem + "&fieldids=" + recentItem + "&FromPage=" + sCurrentPageURL + "'>";
                        sUserRecentItems += "<span class='additional-background'><span class='menu-item-text' >" + sBorrowerName + "</span></span></a></li>";

                        sHomePageUserRecentItems += "<li class='static'><a class='static menu-item' href='LoanDetails/LoanDetails.aspx?fieldid=" + recentItem + "&fieldids=" + recentItem + "&FromPage=" + sCurrentPageURL + "'>";
                        sHomePageUserRecentItems += "<span class='additional-background'><span class='menu-item-text' >" + sBorrowerName + "</span></span></a></li>";
                    }
                    
                }
            }


            if (!IsPostBack)
            {
                try
                {
                    // set welcome info
                    BLL.Company_General comGeneral = new BLL.Company_General();
                    Model.Company_General company = comGeneral.GetModel();
                    if (null != company)
                        this.literalComName.Text = string.Format("Welcome to the {0} Portal! ", company.Name);
                }
                catch
                { }
            }

            // 获取Company_General
            LPWeb.BLL.Company_General bllCompanyGeneral = new LPWeb.BLL.Company_General();
            LPWeb.Model.Company_General modCompanyGeneral = bllCompanyGeneral.GetModel();

            this.sMyEmailInboxURL = modCompanyGeneral.MyEmailInboxURL;
            this.sMyCalendarURL = modCompanyGeneral.MyCalendarURL;
            this.sRatesURL = modCompanyGeneral.RatesURL;
        }

        public string GetPersonalSiteUrl()
        {
            //string sAccount = HttpContext.Current.User.Identity.Name;
            //string sPersonalSiteUrl = string.Format("{0}", ConfigurationManager.AppSettings["PersonalSiteUrl"]);

            ////get current service context
            //SPSite site = null;
            //try
            //{
            //    site = new SPSite(sPersonalSiteUrl);

            //    if (null == site)
            //        return null;

            //    SPServiceContext serviceContext = SPServiceContext.GetContext(site);
            //    //initialize user profile config manager object
            //    UserProfileManager upm = new UserProfileManager(serviceContext);

            //    //create user sample
            //    if (!upm.UserExists(sAccount))
            //        upm.CreateUserProfile(sAccount);

            //    UserProfile u = upm.GetUserProfile(sAccount);
            //    //u.CreatePersonalSite();
            //    if (null != u.PersonalUrl)
            //        return u.PersonalUrl.AbsoluteUri;
            //    else
            //        return "";
            //}
            //catch
            //{
            //    return "";
            //}

            return string.Empty;
        }

        protected string HfdClientId
        {
            get { return hfdUserPre.ClientID; }
        }
    }
}