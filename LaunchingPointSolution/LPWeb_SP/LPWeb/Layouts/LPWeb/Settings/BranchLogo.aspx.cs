using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;

public partial class BranchLogo : BasePage
{
    int iBranchID = 0;
    Branches branchManager = new Branches();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Request.QueryString["BranchID"] != null) // 如果有GroupID
        {
            string sBranchID = this.Request.QueryString["BranchID"].ToString();
            LPWeb.Model.Branches model = new LPWeb.Model.Branches();
            try
            {

                this.iBranchID = Convert.ToInt32(sBranchID);

                model = branchManager.GetModel(iBranchID);
                //Response.ContentType = getreader["markType"] as string;
                Response.ContentType = "image/jpeg";
                Response.OutputStream.Write(model.WebsiteLogo, 0, model.WebsiteLogo.Length);
                Response.End();
            }
            catch
            { }
        }
    }
}
