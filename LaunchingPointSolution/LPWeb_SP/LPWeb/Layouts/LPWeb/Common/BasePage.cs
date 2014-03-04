using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using LPWeb.BLL;
using Microsoft.SharePoint.WebControls;
using LPWeb.Common;

namespace LPWeb.Layouts.LPWeb.Common
{
    public class BasePage : LayoutsPageBase
    {
        private LoginUser currUser = null;
        //private Company_Web _bll = null;
        public LoginUser CurrUser
        {
            get 
            {
                if (null != ViewState["lpuser"])
                    return (LoginUser)ViewState["lpuser"];
                else
                {
                    this.currUser = new LoginUser();
                    ViewState["lpuser"] = this.currUser;
                    return this.currUser;
                }
            }
            set 
            {
                ViewState["lpuser"] = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            bool logErr = false;
            string err = string.Empty;
            try
            {
                if (!IsPostBack)
                {
                    this.currUser = new LoginUser();

                    ViewState["lpuser"] = currUser;
                }
                base.OnLoad(e);
            }
            catch (SqlException exBase)
            {
                if (exBase.Message.Contains("connection to SQL Server"))
                {
                    logErr = true;
                    err = exBase.Message + exBase.StackTrace;
                    return;
                }
                else
                    throw exBase;
            }
            catch (NullReferenceException exNull)
            {
                if (exNull.Message.Contains("database connection string"))
                {
                    logErr = true;
                    err = exNull.Message + exNull.StackTrace;
                    return;
                }
                else
                    throw exNull;
            }
            finally
            {
                if (logErr)
                {
                    err = "<h3>Failed to load the page. Please contact your system administrator. </h3>Error:" + err + "<p>";
                    PageCommon.AlertMsg(this, err);
                    Response.Write(err);
                    Response.Flush();
                    Response.End();
                }
            }
        }


        protected override void OnPreInit(EventArgs e)
        {
            if (null != this.MasterPageFile)
            {
                string strMaster = "";
                if (this.MasterPageFile.LastIndexOf('/') > -1)
                    strMaster = this.MasterPageFile.Substring(this.MasterPageFile.LastIndexOf('/'));
                switch (strMaster.ToLower())
                {
                    case "/super.master":
                        this.MasterPageFile = "/_catalogs/masterpage/LPWeb/Super.master";
                        break;
                    case "/home.master":
                        this.MasterPageFile = "/_catalogs/masterpage/LPWeb/Home.master";
                        break;
                    case "/pipeline.master":
                        this.MasterPageFile = "/_catalogs/masterpage/LPWeb/Pipeline.master";
                        break;
                    case "/settings.master":
                        this.MasterPageFile = "/_catalogs/masterpage/LPWeb/Settings.master";
                        break;
                    case "/contact.master":
                        this.MasterPageFile = "/_catalogs/masterpage/LPWeb/Contact.master";
                        break;
                    case "/marketing.master":
                        this.MasterPageFile = "/_catalogs/masterpage/LPWeb/Marketing.master";
                        break;
                    default:
                        break;
                }
                base.OnPreInit(e);
            }
        }
    }
}
