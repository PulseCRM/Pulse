using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LPWeb.BLL;
using System.Text;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;


public partial class EmailAttachmentsList : BasePage
{
    int TemplEmailId = 36;
    string op = "list";
    string AttachId = "";
    string Token = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["TemplEmailId"] == null)
        {
            return;
        }
        TemplEmailId = Convert.ToInt32(Request.QueryString["TemplEmailId"]);

        op = Request.QueryString["op"] == null ? "list" : Request.QueryString["op"].ToString();
        AttachId = Request.QueryString["AttachId"] == null ? "" : Request.QueryString["AttachId"].ToString();
        Token = Request.QueryString["Token"] == null ? "" : Request.QueryString["Token"].ToString();


        
        LPWeb.BLL.Email_AttachmentsTemp bllEmailAttachTemp = new Email_AttachmentsTemp();
        switch (op)
        {
            case "list":

                    rpAttachmentsList.DataSource =bllEmailAttachTemp.GetList(TemplEmailId, Token);//blltmpemalAttach.GetListWithOutFileImage("TemplEmailId = " + TemplEmailId + " and Enabled =1");
                    rpAttachmentsList.DataBind();

                break;
            case "Remove":
                if (string.IsNullOrEmpty(AttachId))
                {
                    Response.Clear();
                    Response.Write("1");
                    Response.End();
                    return;
                }

                var idlist = AttachId.Split(',').ToList();
                foreach (string idstr in idlist)
                {
                    try
                    {
                        if (idstr.ToLower().StartsWith("s"))
                        {
                            int id = Convert.ToInt32(idstr.Replace("s", ""));
                            bllEmailAttachTemp.Add(
                                new LPWeb.Model.Email_AttachmentsTemp { Token = Token, TemplAttachId = id }
                                );
                        }
                        if (idstr.ToLower().StartsWith("c"))
                        {
                            int id = Convert.ToInt32(idstr.Replace("c", ""));
                            bllEmailAttachTemp.Delete(id);
                        }

                    }
                    catch { }
                }



                Response.Clear();
                Response.Write("1");
                Response.End();
                break;
        }

        
    }
}

