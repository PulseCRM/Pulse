using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.BLL;
using System.Data;
using System.Text;

public partial class Settings_GetPointField_Background : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sTerm = this.Request.QueryString["term"].ToString();

        Template_Email EmailTemplateManager = new Template_Email();
        DataTable PrevPointFieldList = EmailTemplateManager.GetPointFieldList(" and lower([Label]) like lower('%" + sTerm + "%')");

        // json: [{"label":"Name1","value":"2"},{"label":"Name2","value":"2"},{"label":"Name3","value":"3"}]
        StringBuilder sbPointFields = new StringBuilder();
        foreach (DataRow PointFieldRow in PrevPointFieldList.Rows)
        {
            string sPointFieldID = PointFieldRow["PointFieldId"].ToString();
            string sPointField = PointFieldRow["Label"].ToString();
            string sDataType = PointFieldRow["DataType"].ToString();

            if (sbPointFields.Length == 0)
            {
                sbPointFields.Append("{\"label\":\"" + sPointField + "\",\"value\":\"" + sPointField + "\",\"id\":\"" + sPointFieldID + "\",\"datatype\":\"" + sDataType + "\"}");
            }
            else
            {
                sbPointFields.Append(",{\"label\":\"" + sPointField + "\",\"value\":\"" + sPointField + "\",\"id\":\"" + sPointFieldID + "\",\"datatype\":\"" + sDataType + "\"}");
            }
        }

        this.Response.Write("[" + sbPointFields.ToString() + "]");
    }
}
