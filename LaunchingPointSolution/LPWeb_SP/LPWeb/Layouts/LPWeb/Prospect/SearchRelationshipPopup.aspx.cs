using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using LPWeb.BLL;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;

public partial class Prospect_SearchRelationshipPopup : BasePage
{
    int iContactID = 0;
    string sCloseDialogCodes = string.Empty;
    DataTable RelationshipRoles = null;
    DataTable ToRelationshipList = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        this.sCloseDialogCodes = this.Request.QueryString["CloseDialogCodes"].ToString() + ";";

        bool bIsValid = PageCommon.ValidateQueryString(this, "ContactID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Missing required query string.", sCloseDialogCodes);
        }
        this.iContactID = Convert.ToInt32(this.Request.QueryString["ContactID"]);

        #endregion

        #region 加载Contact信息

        Contacts ContactManager = new Contacts();
        DataTable ContactInfo = null;
        try
        {
            ContactInfo = ContactManager.GetContactInfo(this.iContactID);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Exception happened when get contact info (ContactID={0}): {1}", this.iContactID, ex.Message);
            //LPLog.LogMessage(LogType.Logerror, ex.Message);
            PageCommon.WriteJsEnd(this, "Exception happened when get contact info.", sCloseDialogCodes);
        }

        if (ContactInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid contact id.", sCloseDialogCodes);
        }

        #endregion

        #region 绑定Contact数据

        string sLastName1 = ContactInfo.Rows[0]["LastName"].ToString();
        string sFirstName1 = ContactInfo.Rows[0]["FirstName"].ToString();
        string sMiddleName1 = ContactInfo.Rows[0]["MiddleName"].ToString();

        string sContactName = sLastName1 + ", " + sFirstName1;
        if (sMiddleName1 != string.Empty)
        {
            sContactName += " " + sMiddleName1;
        }

        this.lbClient.Text = sContactName;
        if(ContactInfo.Rows[0]["DOB"].ToString() != "")
        { 
            this.lbDOB.Text = Convert.ToDateTime(ContactInfo.Rows[0]["DOB"]).ToShortDateString();
        }

        // property
        string sAddress = ContactInfo.Rows[0]["MailingAddr"].ToString();
        string sCity = ContactInfo.Rows[0]["MailingCity"].ToString();
        string sState = ContactInfo.Rows[0]["MailingState"].ToString();
        string sZip = ContactInfo.Rows[0]["MailingZip"].ToString();
        this.lbAddress.Text = sAddress + ", " + sCity + ", " + sState + " " + sZip;

        #endregion

        #region 加载RelationshipRoles

        RelationshipRoles RelationshipRoles1 = new RelationshipRoles();
        this.RelationshipRoles = RelationshipRoles1.GetRelationshipTypeList();

        #endregion

        #region 加载ToRelationshipList

        this.ToRelationshipList = RelationshipRoles1.GetToRelationshipList();

        #endregion

        #region 加载Contact列表

        this.ContactSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;

        string sWhere = " and (ContactID not in (select ToContactId from Contact_Relationship where FromContactId=" + this.iContactID + ")"
                      + " AND ContactID NOT IN (SELECT FromContactId FROM Contact_Relationship WHERE ToContactId =" + this.iContactID + "))";

        bool bSetCondition = false;

        // LastName
        if (this.Request.QueryString["LastName"] != null)
        {
            string sLastName = this.Request.QueryString["LastName"].ToString();
            sWhere += " and (LastName like '%" + sLastName + "%')";

            bSetCondition = true;
        }

        // FirstName
        if (this.Request.QueryString["FirstName"] != null)
        {
            string sFirstName = this.Request.QueryString["FirstName"].ToString();
            sWhere += " and (FirstName like '%" + sFirstName + "%')";

            bSetCondition = true;
        }

        int iRowCount = 0;
        if (bSetCondition == false) // 未设置查询条件
        {
            sWhere = " and 1=1";
            this.gridContactList.EmptyDataText = "Please enter search condition to search contact.";
        }
        else
        {
            iRowCount = LPWeb.DAL.DbHelperSQL.Count(this.ContactSqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);

            if (iRowCount == 0)
            {
                this.gridContactList.EmptyDataText = "There is no branch by search criteria，please search again.";
            }

        }
        this.AspNetPager1.RecordCount = iRowCount;

        this.ContactSqlDataSource.SelectParameters["Where"].DefaultValue = sWhere;
        this.gridContactList.DataBind();

        #endregion
    }

    protected void btnAddRelationship_Click(object sender, EventArgs e)
    {
        string sToContactIDs = this.hdnToContactIDs.Value;
        string sToRelationships = this.hdnToRelationships.Value;
        string sFromRelationships = this.hdnFromRelationships.Value;

        string[] ToContactIDArray = sToContactIDs.Split(',');
        string[] ToRelationshipArray = sToRelationships.Split(',');
        string[] FromRelationshipArray = sFromRelationships.Split(',');

        string[] sRelationshipTypeArray = new string[ToContactIDArray.Length];
        for (int i = 0; i < ToRelationshipArray.Length; i++)
        {
            string sToRelationship = ToRelationshipArray[i];
            string sFromRelationship = FromRelationshipArray[i];

            DataRow[] RelationTypeRows = null;
            if (sFromRelationship != string.Empty) 
            {
                RelationTypeRows = this.RelationshipRoles.Select("RelFromName='" + sFromRelationship + "' and RelToName='" + sToRelationship + "'");
            }
            else
            {
                RelationTypeRows = this.RelationshipRoles.Select("RelToName='" + sToRelationship + "'");
            }
            string sRelationTypeID = RelationTypeRows[0]["RelTypeId"].ToString();

            sRelationshipTypeArray.SetValue(sRelationTypeID, i);
        }

        // insert
        Contact_Relationship RelationshipManager = new Contact_Relationship();
        RelationshipManager.InsertContactRelationship(this.iContactID, ToContactIDArray, sRelationshipTypeArray);

        // success
        PageCommon.WriteJsEnd(this, "Add relationship successfully.", "window.parent.location.href=window.parent.location.href;");
    }

    public string GetOptions_ToRelationship()
    {
        StringBuilder sbOptions = new StringBuilder();
        foreach (DataRow RelRow in this.ToRelationshipList.Rows)
        {
            string sToRelationship = RelRow["RelToName"].ToString();

            if (sbOptions.Length == 0)
            {
                sbOptions.AppendLine("<option value=''>-- select --</option>");
            }
            else
            {
                sbOptions.AppendLine("<option value='" + sToRelationship + "'>" + sToRelationship + "</option>");
            }
        }

        return sbOptions.ToString();
    }

}
