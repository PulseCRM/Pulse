using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;

public partial class GetPointfilesbg : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Request.QueryString["term"] != null && this.Request.QueryString["term"] != "")
        {
            this.Response.Clear();
            this.Response.Charset = "utf-8";
            this.Response.Buffer = true;
            this.Response.ContentEncoding = System.Text.Encoding.UTF8;
            this.Response.ContentType = "text/plain";
            this.Response.Write(GetLikeUserName1(this.Request.QueryString["term"]));
            this.Response.Flush();
            this.Response.Close();
            this.Response.End();
        }
    }

    private String GetLikeUserName(string key)
    {
        PointFiles pf = new PointFiles();
        DataSet ds = pf.GetList(10, "[name] like '%" + key + "%'", " [name] ");
        if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
        {
            return string.Empty;
        }
        DataTable dt = ds.Tables[0];
        System.Text.StringBuilder sbstr = new System.Text.StringBuilder("[");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            sbstr.Append("\"" + dt.Rows[i]["Name"].ToString().Replace(@"\", @"\\") + "\"");
            if (i == dt.Rows.Count - 1)
                sbstr.Append("]");
            else
                sbstr.Append(",");
        }

        return sbstr.ToString();
    }

    private String GetLikeUserName1(string key)
    {
        PointFiles pf = new PointFiles();
        DataSet ds = pf.GetList(10, "[name] like '%" + key + "%'", " [name] ");
        if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
        {
            return string.Empty;
        }
        DataTable dt = ds.Tables[0];
        System.Text.StringBuilder sbstr = new System.Text.StringBuilder("[");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            //{ name: "Peter Pan", to: "peter@pan.de" },
            // json: [{"label":"Name1","value":"2"},{"label":"Name2","value":"2"},{"label":"Name3","value":"3"}]
            sbstr.Append("{\"label\":\"");
            sbstr.Append(dt.Rows[i]["Name"].ToString().Replace(@"\", @"\\") + "   " + GetBorrower(int.Parse(dt.Rows[i]["FileID"].ToString())));
            sbstr.Append("\", \"value\":\"");
            sbstr.Append(dt.Rows[i]["Name"].ToString().Replace(@"\", @"\\"));
            sbstr.Append("\"}");
            if (i == dt.Rows.Count - 1)
                sbstr.Append("]");
            else
                sbstr.Append(",");
        }

        return sbstr.ToString();
    }

    private string GetBorrower(int iFileID)
    {
        string Borrower = string.Empty;
        try
        {
            Contacts contact = new Contacts();
            Borrower = contact.GetBorrowerLastName(iFileID);
        }
        catch
        { }
        return Borrower;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
