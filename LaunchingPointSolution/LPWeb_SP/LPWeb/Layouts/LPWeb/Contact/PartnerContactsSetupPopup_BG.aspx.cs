using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.BLL;
using LPWeb.Common;
using System.Data;

public partial class PartnerContactsSetupPopup_BG : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        // json示例
        // {"ContactBranchId":{0},"Name":"{1}","Address":"{2}","City":"{3}","State":"{4}","Zip":"{5}"}
        #region 接收参数
        string respTempl = "^0^\"ContactBranchId\":{0},\"Name\":\"{1}\",\"Address\":\"{2}\",\"City\":\"{3}\",\"State\":\"{4}\",\"Zip\":\"{5}\"^1^";

        // DelContactID
        bool bIsValid = PageCommon.ValidateQueryString(this, "ContactBranchId", QueryStringType.ID);
        if (bIsValid == false)
        {
            this.Response.Write(string.Format("{\"ContactBranchId\":{0},\"Name\":\"{1}\",\"Address\":\"{2}\",\"City\":\"{3}\",\"State\":\"{4}\",\"Zip\":\"{5}\"}", 0));
            return;
        }

        string sContactBranchId = this.Request.QueryString["ContactBranchId"].ToString();
        int iContactBranchId = Convert.ToInt32(sContactBranchId);

        #endregion

        #region Branch

        ContactBranches contactBranch = new ContactBranches();
        

        try
        {
            DataSet dsCBranch = contactBranch.GetList(" ContactBranchId = " + iContactBranchId.ToString());

            if (dsCBranch != null && dsCBranch.Tables.Count > 0 && dsCBranch.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsCBranch.Tables[0].Rows[0];

                string rspStr = string.Format(respTempl
                    , dr["ContactBranchId"] != DBNull.Value ? dr["ContactBranchId"].ToString() : "0"
                    , dr["Name"] != DBNull.Value ? dr["Name"].ToString() : ""
                    , dr["Address"] != DBNull.Value ? dr["Address"].ToString() : ""
                    , dr["City"] != DBNull.Value ? dr["City"].ToString() : ""
                    , dr["State"] != DBNull.Value ? dr["State"].ToString() : ""
                    , dr["Zip"] != DBNull.Value ? dr["Zip"].ToString() : ""
                        );
                this.Response.Write(respStrReplace(rspStr));
                return;
            }
            this.Response.Write(string.Format(respTempl, 0));
            return;
        }
        catch
        {
            this.Response.Write(string.Format(respTempl, 0));
            return;
        }

        #endregion


    }

    public string respStrReplace(string str)
    {
        return str.Replace("^0^", "{").Replace("^1^", "}");
    }
}

