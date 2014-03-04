using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Common;
using Utilities;
using LPWeb.Layouts.LPWeb.Common;
using System.Data.SqlClient;
using System.Data;



    /// <summary>
    /// Email template list
    /// author: duanlijun
    /// date: 2012-09-25
    /// </summary>
public partial class GeneralInfoTab : BasePage
    {
        CompanyTaskPickList bllTaskPickList = new CompanyTaskPickList();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //权限验证
                var loginUser = new LoginUser();
                if (!loginUser.userRole.CompanySetup)
                {
                    Response.Redirect("../Unauthorize.aspx");
                    return;
                }

              
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }

            if (!Page.IsPostBack)
            {
                
                if (Request.QueryString["ProspectID"] != null)
                {
                   
                  
                }

              
            }
        }


      

       
       




        protected void btnAdd_Click(object sender, EventArgs e)
        {
           
        }

        protected void btnCA_Click(object sender, EventArgs e)
        {
           
        }

      

       
    }

