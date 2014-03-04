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
using System.IO;

namespace LPWeb.Settings
{
    /// <summary>
    /// Render User Picture
    /// Author: Peter
    /// Date: 2010-09-01
    /// </summary>
    public partial class GetPreviewUserPicture : BasePage
    {
        /// <summary>
        /// UserID
        /// </summary>
        private int? UserId
        {
            get
            {
                int nUID = -1;
                if (!int.TryParse(Request.QueryString["uid"], out nUID))
                {
                    nUID = -1;
                }
                if (-1 == nUID)
                    return null;
                else
                    return nUID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserId.HasValue)
            {
                BLL.Users UsersManager = new BLL.Users();
                Model.Users user = UsersManager.GetModel(UserId.Value);

                //style="width:77px; height:77px" />
                int iWidth = 77;
                int iHeight = 77;

                if (null != user && null != user.UserPictureFile)
                {
                    Bitmap _Bitmap = new Bitmap(iWidth, iHeight);
                    Graphics _Graphcis = Graphics.FromImage(_Bitmap);
                    _Graphcis.DrawImage(System.Drawing.Image.FromStream(new MemoryStream(user.UserPictureFile)), 0, 0, iWidth, iHeight);
                    _Graphcis.Dispose();

                    MemoryStream ms = new MemoryStream();
                    _Bitmap.Save(ms, ImageFormat.Png);
                    ms.Flush();
                    byte[] bData = ms.GetBuffer();
                    ms.Close(); 

                    //Response.BinaryWrite(user.UserPictureFile);
                    Response.BinaryWrite(bData);
                }
                else
                {
                    ResponseDefaultPicture();
                }
            }
            else
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
            Bitmap defaultPic = new Bitmap(Page.MapPath("~/_layouts/LPWeb/images/DefaultUserPhoto.jpg"));

            Response.ContentType = "image/jpeg";
            defaultPic.Save(Response.OutputStream, ImageFormat.Jpeg);

            defaultPic.Dispose();
        }
    }
}