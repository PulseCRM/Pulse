using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using LPWeb.Model;
using Utilities;

namespace LPWeb.Settings
{
    /// <summary>
    /// Summary description for Image
    /// </summary>
    public class FileTree : IHttpHandler
    {
        private Company_Web modCompanyWeb = new Company_Web();
        LPWeb.BLL.Company_Web bllCompanyWeb = new LPWeb.BLL.Company_Web();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            StringBuilder stringBuilder = new StringBuilder();

            string dir;
            if (context.Request.Form["dir"] == null || context.Request.Form["dir"].Length <= 0)
                dir = "/";
            else
                dir = context.Server.UrlDecode(context.Request.Form["dir"]);

            string filter;
            if (context.Request.Form["filter"] == null || context.Request.Form["filter"].Length <= 0)
                filter = "*.*";
            else
                filter = context.Server.UrlDecode(context.Request.Form["filter"]);


            var di = new System.IO.DirectoryInfo(dir);
            stringBuilder.Append("<ul class=\"jqueryFileTree\" style=\"display: none;\">\n");
            try
            {
                foreach (DirectoryInfo di_child in di.GetDirectories())
                    stringBuilder.Append("\t<li class=\"directory collapsed\"><a href=\"#\" rel=\"" + dir + di_child.Name + "/\">" + di_child.Name + "</a></li>\n");
                foreach (FileInfo fi in di.GetFiles(filter))
                {
                    string ext = "";
                    if (fi.Extension.Length > 1)
                        ext = fi.Extension.Substring(1).ToLower();

                    stringBuilder.Append("\t<li class=\"file ext_" + ext + "\"><a href=\"#\" rel=\"" + fi.FullName + "\">" + fi.Name + "</a></li>\n");
                }
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }
            stringBuilder.Append("</ul>");
            context.Response.Write(stringBuilder.ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}