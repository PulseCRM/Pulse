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

namespace LPWeb.Layouts.LPWeb.Settings
{
    /// <summary>
    /// Render Contact Picture
    /// Author: Peter
    /// Date: 2011-06-06
    /// </summary>
    public partial class GetContactPicture : BasePage
    {
        /// <summary>
        /// contact id
        /// </summary>
        private int? ContactId
        {
            get
            {
                int nCID = -1;
                if (!int.TryParse(Request.QueryString["cid"], out nCID))
                {
                    nCID = -1;
                }
                if (-1 == nCID)
                    return null;
                else
                    return nCID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ContactId.HasValue)
            {
                BLL.Contacts ContactsManager = new BLL.Contacts();
                Model.Contacts contact = ContactsManager.GetModel(ContactId.Value);
                if (null != contact && null != contact.Picture)
                {
                    Response.BinaryWrite(contact.Picture);
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
        /// Render default contact picture
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
