using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using LPWeb.Model;
using Utilities;

namespace LPWeb.Settings
{
    /// <summary>
    /// Summary description for Image
    /// </summary>
    public class Image : IHttpHandler
    {
        private Company_Web modCompanyWeb = new Company_Web();
        LPWeb.BLL.Company_Web bllCompanyWeb = new LPWeb.BLL.Company_Web();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();
            context.Response.ContentType = "image/jpeg";
            int photoId = -1;
            byte[] imageData = null;

            if (context.Request.QueryString["PhotoID"] != null &&
                context.Request.QueryString["PhotoID"] != "")
            {
                photoId = Convert.ToInt32(context.Request.QueryString["PhotoID"]);
                imageData = GetPhoto(photoId);
                if (imageData == null)
                    return;
                context.Response.BinaryWrite(imageData);
                context.Response.Flush();
                context.Response.End();
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public byte[] GetPhoto(int photoId)
        {
            try
            {
                modCompanyWeb = bllCompanyWeb.GetModel();
                if (modCompanyWeb != null)
                {
                    if (modCompanyWeb.HomePageLogoData.Length > 0 && photoId == 1)
                    {
                        return modCompanyWeb.HomePageLogoData;
                    }
                    if (modCompanyWeb.SubPageLogoData.Length > 0 && photoId == 2)
                    {
                        return modCompanyWeb.SubPageLogoData;
                    }
                }
                return null;
            }
            catch (Exception exception)
            {
                modCompanyWeb = null;
                LPLog.LogMessage(exception.Message);
            }
            return null;
        }
    }
}