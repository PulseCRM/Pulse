using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using System.Collections.Generic;
using System.Data;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using LPWeb.BLL;
using Utilities;

public partial class Prospect_ProspectDetailRelationshipTab : BasePage
{
    int iContactID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "ContactID", QueryStringType.ID);
        if (bIsValid == false)
        {
            try
            {
                this.Response.End();
            }
            catch
            {

            }
        }

        this.iContactID = Convert.ToInt32(this.Request.QueryString["ContactID"]);

        #endregion

        #region 加载Relationship数据

        #region get row count

        Contacts ContactManager = new Contacts();
        int iRowCount = ContactManager.GetRelatedContactCount(this.iContactID);
        this.AspNetPager1.RecordCount = iRowCount;

        #endregion

        #region Calc. StartIndex and EndIndex

        int iPageSize = this.AspNetPager1.PageSize;
        int iPageIndex = 1;
        if(this.Request.QueryString["PageIndex"] != null)
        {
            iPageIndex = Convert.ToInt32(this.Request.QueryString["PageIndex"]);
        }
        int iStartIndex = PageCommon.CalcStartIndex(iPageIndex, iPageSize);
        int iEndIndex = PageCommon.CalcEndIndex(iStartIndex, iPageSize, iRowCount);
        
        #endregion

        #region 获取Related Contact List

        RelationshipManager RelationshipManager1 = new RelationshipManager();
        List<RelatedContact> RelationshipList = RelationshipManager1.GetRelatedContacts(this.iContactID, iStartIndex, iEndIndex);

        #endregion

        #region Build DataSource

        DataTable RelationshipListData = new DataTable();
        RelationshipListData.Columns.Add("RelContactID", typeof(int));
        RelationshipListData.Columns.Add("ContactName", typeof(string));
        RelationshipListData.Columns.Add("Relationship", typeof(string));
        RelationshipListData.Columns.Add("Direction", typeof(string));

        foreach (RelatedContact RelatedContact1 in RelationshipList)
        {
            DataRow NewRelRow = RelationshipListData.NewRow();
            NewRelRow["RelContactID"] = RelatedContact1.ContactId;
            NewRelRow["ContactName"] = RelatedContact1.ContactName;
            NewRelRow["Relationship"] = RelatedContact1.Relationship;
            NewRelRow["Direction"] = RelatedContact1.Direction;

            RelationshipListData.Rows.Add(NewRelRow);
        }

        RelationshipListData.AcceptChanges();

        #endregion

        this.gridRelationshipList.DataSource = RelationshipListData;
        this.gridRelationshipList.DataBind();

        #endregion
    }

    protected void lnkRemove_Click(object sender, EventArgs e)
    {
        string sDelContactIDs = this.hdnDelContactIDs.Value;
        string sDirections = this.hdnDirections.Value;

        string[] DelContactIDArray = sDelContactIDs.Split(',');
        string[] DirectionArray = sDirections.Split(',');

        // delete
        Contact_Relationship RelationshipManager = new Contact_Relationship();

        try
        {
            // delete
            RelationshipManager.DeleteContactRelationship(this.iContactID, DelContactIDArray, DirectionArray);
        }
        catch (Exception ex)
        {
            string sExMsg = string.Format("Exception occurred while trying to delete the relationship (ContactID={0}): {1}", this.iContactID, ex.Message);
            LPLog.LogMessage(LogType.Logerror, ex.Message);

            PageCommon.WriteJsEnd(this, "Exception occurred while trying to delete the relationship.", "window.location.href=window.location.href;");
        }
        

        // success
        PageCommon.WriteJsEnd(this, "Deleted the relationship successfully.", "window.location.href=window.location.href;");
    }
}
