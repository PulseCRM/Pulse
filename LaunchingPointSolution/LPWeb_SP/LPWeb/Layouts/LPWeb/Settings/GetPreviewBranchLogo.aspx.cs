using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public partial class GetPreviewBranchLogo : BasePage
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
                //Response.ContentType = "image/jpeg";
                //Response.OutputStream.Write(model.WebsiteLogo, 0, model.WebsiteLogo.Length);
                //("width:330px; height:64px");

                if (model.WebsiteLogo == null)
                {
                    return;
                }

                int iWidth = 330;
                int iHeight = 90;

                ImageConverter imc = new ImageConverter();
                System.Drawing.Image _img = imc.ConvertFrom(null, null, model.WebsiteLogo) as System.Drawing.Image;

                int iOrgWidth = Convert.ToInt32(_img.Width);
                int iOrgHeight = Convert.ToInt32(_img.Height);

                ResizeImg(iOrgWidth, iOrgHeight, ref iWidth, ref iHeight);

                Bitmap _Bitmap = new Bitmap(iWidth, iHeight);
                Graphics _Graphcis = Graphics.FromImage(_Bitmap);
                _Graphcis.DrawImage(System.Drawing.Image.FromStream(new MemoryStream(model.WebsiteLogo)), 0, 0, iWidth, iHeight);
                _Graphcis.Dispose();

                MemoryStream ms = new MemoryStream();
                _Bitmap.Save(ms, ImageFormat.Png);
                ms.Flush();
                byte[] bData = ms.GetBuffer();
                ms.Close();

                Response.BinaryWrite(bData);
                Response.End();

            }
            catch
            { }
        }
    }

  public void ResizeImg(int iOrgWidth, int iOrgHeight, ref int iWidth, ref int iHeight)
        {
            int NewWidth = 0;
            int NewHeight = 0;

            if (iOrgWidth > iWidth && iOrgHeight < iHeight)
            {

                NewWidth = iWidth;
                string sHegiht = Convert.ToDecimal(iOrgHeight * iWidth / iOrgWidth).ToString();
                NewHeight = Convert.ToInt32(sHegiht);
            }
            else if (iOrgWidth < iWidth && iOrgHeight > iHeight)
            {
                string sWidth = Convert.ToDecimal(iOrgWidth * iHeight / iOrgHeight).ToString();
                NewWidth = Convert.ToInt32(sWidth);

                NewHeight = iHeight;
            }
            else if (iOrgWidth > iWidth && iOrgHeight > iHeight)
            {

                if (iWidth > iHeight)
                {

                    NewWidth = iWidth;
                    string sHegiht = Convert.ToDecimal(iOrgHeight * iWidth / iOrgWidth).ToString();
                    NewHeight = Convert.ToInt32(sHegiht);
                }
                else
                {
                    string sWidth = Convert.ToDecimal(iOrgWidth * iHeight / iOrgHeight).ToString();
                    NewWidth = Convert.ToInt32(sWidth);

                    NewHeight = iHeight;
                }
            }
            else
            {

                // if less than or equal to iWidth & iHeight, do not scale
                NewWidth = iOrgWidth;
                //alert("NewWidth5: " + NewWidth);

                NewHeight = iOrgHeight;
                //alert("NewHeight5: " + NewHeight);
            }

            iWidth = NewWidth;
            iHeight = NewHeight;
        }
  
}
