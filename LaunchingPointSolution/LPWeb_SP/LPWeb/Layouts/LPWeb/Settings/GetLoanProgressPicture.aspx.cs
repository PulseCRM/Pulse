using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Model;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Imaging;

namespace LPWeb.Settings
{
    /// <summary>
    /// Render User Picture
    /// Author: Peter
    /// Date: 2010-09-01
    /// </summary>
    public partial class GetLoanProgressPicture : BasePage
    {
        /// <summary>
        /// UserID
        /// </summary>
        /// 

        public int FileID = 0;       

        protected void Page_Load(object sender, EventArgs e)
        {
            string sImage = "";
            string sImage1 = ""; 
            
            sImage = Request.QueryString["Image"].ToString();
            int len = sImage.Length;
            sImage1 = sImage.Substring(0, len - 7);            
        
            string s = sImage1.Trim().Replace(" ", "+");
            if (s.Length % 4 > 0)
            {
                s = s.PadRight(s.Length + 4 - s.Length % 4, '=');
            }

            int len1 = s.Length;

            try
            {
                byte[] data = Convert.FromBase64String(s);

                if (null != data)
                {
                    Response.BinaryWrite(data);
                }
                else
                {
                    ResponseDefaultPicture();
                }
            }
            catch
            {
                ResponseDefaultPicture();
            }          

            Response.End(); 
        } 

        /// <summary>
        /// Render default user picture
        /// </summary>
        private void ResponseDefaultPicture()
        {
            Bitmap defaultPic = new Bitmap(Page.MapPath("~/_layouts/LPWeb/images/Progress.gif"));

            Response.ContentType = "image/jpeg";
            defaultPic.Save(Response.OutputStream, ImageFormat.Jpeg);

            defaultPic.Dispose();
        }
    }
}