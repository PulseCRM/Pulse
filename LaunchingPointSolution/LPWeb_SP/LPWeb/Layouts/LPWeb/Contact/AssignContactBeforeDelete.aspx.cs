using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using System.Data;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using LPWeb.LP_Service;
using Utilities;

public partial class Contact_AssignContactBeforeDelete : BasePage
{
    int iDelContactID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 校验页面参数

        bool bIsValid = PageCommon.ValidateQueryString(this, "DelContactID", QueryStringType.ID);
        if (bIsValid == false)
        {
            PageCommon.WriteJsEnd(this, "Messing required query string.", PageCommon.Js_RefreshParent);
        }

        this.iDelContactID = Convert.ToInt32(this.Request.QueryString["DelContactID"]);

        // 检查Contact是否存在
        string sSql = "select * from Contacts where ContactID=" + iDelContactID;
        DataTable DelContactInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        if (DelContactInfo.Rows.Count == 0)
        {
            PageCommon.WriteJsEnd(this, "Invalid contact id.", PageCommon.Js_RefreshParent);
        }

        #endregion

        if (this.IsPostBack == false)
        {
            #region 加载Service Type Filter

            ServiceTypes ServiceTypeManager = new ServiceTypes();
            DataTable ServiceTypeList = ServiceTypeManager.GetServiceTypeList(" and (Enabled=1)");
            DataRow NewServiceTypeRow = ServiceTypeList.NewRow();
            NewServiceTypeRow["ServiceTypeId"] = DBNull.Value;
            NewServiceTypeRow["Name"] = "All";
            ServiceTypeList.Rows.InsertAt(NewServiceTypeRow, 0);
            ServiceTypeList.AcceptChanges();

            this.ddlServiceType.DataSource = ServiceTypeList;
            this.ddlServiceType.DataBind();

            #endregion

            if (this.Request.QueryString["DoSearch"] == null)
            {
                //this.divSearchCriteria.Visible = true;
                this.divSearchResult.Visible = false;
            }
            else
            {
                //this.divSearchCriteria.Visible = false;
                this.divSearchResult.Visible = true;

                #region 加载Contact List

                string sDbTable = "(select a.*, a.LastName +', '+ a.FirstName + case when ISNULL(a.MiddleName, '') != '' then ' '+ a.MiddleName else '' end as FullName, "
                                + "'Partner' as ContactType, c.ServiceTypeID, e.Name as ServiceType, d.Name as BranchName, c.Name as CompanyName "
                                + "from Contacts as a left outer join LoanContacts as b on a.ContactId = b.ContactId "
                                + "left outer join ContactCompanies as c on a.ContactCompanyId = c.ContactCompanyId "
                                + "left outer join ContactBranches as d on a.ContactBranchId=d.ContactBranchId "
                                + "left outer join ServiceTypes as e on c.ServiceTypeId=e.ServiceTypeId "
                                + "where b.ContactRoleId!=1 and b.ContactRoleId!=2 and a.ContactCompanyId is not null "
                                + "union "
                                + "select a.*, a.LastName +', '+ a.FirstName + case when ISNULL(a.MiddleName, '') != '' then ' '+ a.MiddleName else '' end as FullName, "
                                + "'Client' as ContactType, null as ServiceTypeID, null as ServiceType, null as BranchName, null as CompanyName "
                                + "from Contacts as a left outer join LoanContacts as b on a.ContactId = b.ContactId "
                                + "where (b.ContactRoleId=1 or b.ContactRoleId=2)) as t";

                #region Build Where

                string sWhere = "";

                // ContactType
                if (this.Request.QueryString["ContactType"] != null)
                {
                    string sContactType = this.Request.QueryString["ContactType"].ToString();
                    if (sContactType == "Partner" || sContactType == "Client")
                    {
                        sWhere += " and ContactType='" + sContactType + "'";
                    }
                }

                // ServiceType
                if (this.Request.QueryString["ServiceType"] != null)
                {
                    string sServiceType = this.Request.QueryString["ServiceType"].ToString();
                    sWhere += " and ServiceTypeID=" + sServiceType;
                }

                // Company
                if (this.Request.QueryString["Company"] != null)
                {
                    string sCompany = this.Request.QueryString["Company"].ToString();
                    sWhere += " and CompanyName like '%" + sCompany + "%'";
                }

                // Branch
                if (this.Request.QueryString["Branch"] != null)
                {
                    string sBranch = this.Request.QueryString["Branch"].ToString();
                    sWhere += " and BranchName like '%" + sBranch + "%'";
                }

                // LastName
                if (this.Request.QueryString["LastName"] != null)
                {
                    string sLastName = this.Request.QueryString["LastName"].ToString();
                    sWhere += " and LastName like '" + sLastName + "%'";
                }

                // Address
                if (this.Request.QueryString["Address"] != null)
                {
                    string sAddress = this.Request.QueryString["Address"].ToString();
                    sWhere += " and MailingAddr like '" + sAddress + "%'";
                }

                // City
                if (this.Request.QueryString["City"] != null)
                {
                    string sCity = this.Request.QueryString["City"].ToString();
                    sWhere += " and MailingCity like '" + sCity + "%'";
                }

                // State
                if (this.Request.QueryString["State"] != null)
                {
                    string sState = this.Request.QueryString["State"].ToString();
                    sWhere += " and MailingState='" + sState + "'";
                }

                #endregion

                this.ContactSqlDataSource.ConnectionString = LPWeb.DAL.DbHelperSQL.connectionString;

                this.ContactSqlDataSource.SelectParameters["DbTable"].DefaultValue = sDbTable;

                int iRowCount = LPWeb.DAL.DbHelperSQL.Count(this.ContactSqlDataSource.SelectParameters["DbTable"].DefaultValue, sWhere);

                this.AspNetPager1.RecordCount = iRowCount;

                this.ContactSqlDataSource.SelectParameters["Where"].DefaultValue = sWhere;
                this.gridContactList.DataBind();

                #endregion
            }
        }
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        int iNewContactID = Convert.ToInt32(this.hdnSelContactID.Value);

        #region 加载LoanContacts

        string sSql = "select * from LoanContacts where ContactID=" + this.iDelContactID;
        DataTable RefLoanIDs = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);

        #endregion

        #region 调用PointManager.ReassignContactRequest

        ServiceManager sm = new ServiceManager();
        using (LP2ServiceClient service = sm.StartServiceClient())
        {
            #region Build ReassignContactRequest

            ReassignContactRequest req = new ReassignContactRequest();
            req.hdr = new ReqHdr();
            req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
            req.hdr.UserId = this.CurrUser.iUserID;

            List<ReassignContactInfo> ContactList = new List<ReassignContactInfo>();
            foreach (DataRow RefLoanRow in RefLoanIDs.Rows)
            {
                int iFileID = Convert.ToInt32(RefLoanRow["FileId"]);
                int iContactRoleID = Convert.ToInt32(RefLoanRow["ContactRoleId"]);

                ReassignContactInfo ContactInfo = new ReassignContactInfo();
                ContactInfo.FileId = iFileID;
                ContactInfo.ContactRoleId = iContactRoleID;
                ContactInfo.NewContactId = iNewContactID;
                ContactList.Add(ContactInfo);
            }
            req.reassignContacts = ContactList.ToArray();

            #endregion

            ReassignContactResponse respone = null;
            try
            {
                respone = service.ReassignContact(req);

                if (respone.hdr.Successful)
                {
                    foreach (DataRow RefLoanRow in RefLoanIDs.Rows)
                    {
                        int iFileID = Convert.ToInt32(RefLoanRow["FileId"]);
                        int iContactRoleID = Convert.ToInt32(RefLoanRow["ContactRoleId"]);

                        #region Reassign Loan Contact

                        LPWeb.Model.LoanContacts lcModel = new LPWeb.Model.LoanContacts();
                        lcModel.FileId = iFileID;
                        lcModel.ContactRoleId = iContactRoleID;
                        lcModel.ContactId = iNewContactID;

                        LPWeb.Model.LoanContacts oldlcModel = new LPWeb.Model.LoanContacts();
                        oldlcModel.FileId = iFileID;
                        oldlcModel.ContactRoleId = iContactRoleID;
                        oldlcModel.ContactId = this.iDelContactID;

                        LPWeb.BLL.LoanContacts lc = new LoanContacts();
                        lc.Reassign(oldlcModel, lcModel, req.hdr.UserId);

                        #endregion

                        #region delete contact

                        Contacts bllContact = new Contacts();
                        int iUserType = 0;
                        if (this.CurrUser.bIsCompanyExecutive)
                        {
                            iUserType = 0;
                        }
                        else if (this.CurrUser.bIsBranchManager)
                        {
                            iUserType = 1;
                        }
                        else
                        {
                            iUserType = 2;
                        }
                        bllContact.PartnerContactsDelete(this.iDelContactID, CurrUser.iUserID, iUserType);

                        #endregion
                    }

                    PageCommon.WriteJsEnd(this, "Deleted contact successfully", PageCommon.Js_RefreshParent);
                }
                else
                {
                    PageCommon.WriteJsEnd(this, String.Format("Failed to reassign contact: reason: {0}.", respone.hdr.StatusInfo), PageCommon.Js_RefreshSelf);
                }

            }
            catch (System.ServiceModel.EndpointNotFoundException ex)
            {
                LPLog.LogMessage(ex.Message);
                PageCommon.WriteJsEnd(this, "Failed to reassign contact: reason, Point Manager is not running. ", PageCommon.Js_RefreshSelf);
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
                PageCommon.WriteJsEnd(this, String.Format("Failed to reassign contact, reason: {0}", exception.Message), PageCommon.Js_RefreshSelf);
            }
        }

        #endregion
    }

}
