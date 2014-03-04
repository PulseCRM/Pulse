using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Net;
using System.IO;

namespace LPWeb.Layouts.LPWeb.Marketing
{
    public partial class DownloadResource : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strResUrl = Request.QueryString["url"];
            if (!string.IsNullOrEmpty(strResUrl))
            {
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(strResUrl);
                try
                {
                    HttpWebResponse httpResp = (HttpWebResponse)httpReq.GetResponse();
                    if ((httpResp.StatusCode == HttpStatusCode.OK))
                    {
                        Stream respStream = httpResp.GetResponseStream();
                        BinaryReader bReader = new BinaryReader(respStream);
                        byte[] arrContent = new byte[httpResp.ContentLength];
                        arrContent = bReader.ReadBytes(arrContent.Length);
                        Response.BinaryWrite(arrContent);
                    }
                }
                catch (WebException we)
                {
                    if (null != we.Response && ((HttpWebResponse)we.Response).StatusCode == HttpStatusCode.NotFound)
                    {
                        Response.Write(string.Format("File not found: {0}", strResUrl));
                    }
                    else
                    {
                        Response.Write(string.Format("Error when load this resource: {0}", strResUrl));
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(string.Format("Error when load this resource: {0}", strResUrl));
                }
            }
            else
            {
                Response.Write(string.Format("No resource specified."));
            }
            Response.Flush();
            Response.End();
        }
    }
}
