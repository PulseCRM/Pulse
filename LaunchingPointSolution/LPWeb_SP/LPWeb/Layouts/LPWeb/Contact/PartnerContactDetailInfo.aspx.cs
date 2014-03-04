using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.BLL;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Common;
using Utilities;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;


public partial class PartnerContactDetailInfo : BasePage
{
    Contacts bllContact = new Contacts(); 
    int iContactID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ContactID"] != null) // 如果有FileID
        {
            string sContactID = Request.QueryString["ContactID"];

            if (PageCommon.IsID(sContactID) == false)
            {
                return;
            }
            try
            {
                iContactID = Convert.ToInt32(sContactID);
            }
            catch
            {
                iContactID = 0;
            }
        } 
        hdnContactID.Value = iContactID.ToString();
        if (iContactID == 0)
        {
            return;
        }

        if (this.CurrUser.userRole.ExportClients == true)
        {
            this.btnSaveAsVCard.Enabled = true;
        }
        else
        {
            this.btnSaveAsVCard.Enabled = false;
        }

        if (!IsPostBack)
        {
            BindLables();
        }
    }

    protected void btnSaveAsVCard_Click(object sender, EventArgs e)
    {
        string sCurrentPagePath = this.Server.MapPath("~/");
        string sFileName = Guid.NewGuid().ToString();
        string sFilePath = Path.Combine(Path.GetDirectoryName(sCurrentPagePath), sFileName);

        #region Call vCardToString(ContactId, true) API

        LPWeb.BLL.Contacts x = new LPWeb.BLL.Contacts();
        string sContactStr = x.vCardToString(this.iContactID, false);

        #endregion

        // save file
        //if (File.Exists(sFilePath) == false)
        //{
        //    // Create a file to write to.
        //    using (StreamWriter sw = File.CreateText(sFilePath))
        //    {
        //        sw.Write(sContactStr);
        //    }
        //}

        //FileInfo FileInfo1 = new FileInfo(sFilePath);
        this.Response.Clear();
        this.Response.ClearHeaders();
        this.Response.Buffer = false;
        this.Response.ContentType = "application/octet-stream";
        this.Response.AppendHeader("Content-Disposition", "attachment;filename=vcard.vcf");
        //this.Response.AppendHeader("Content-Length", FileInfo1.Length.ToString());
        //this.Response.WriteFile(sFilePath);

        this.Response.AppendHeader("Content-Length", sContactStr.Length.ToString());
        this.Response.Write(sContactStr);

        this.Response.Flush();

        // 删除临时文件
        File.Delete(sFilePath);

        this.Response.End();
    }

    private void BindLables()
    {
        if (iContactID == 0)
        {
            return;
        }
        int iContactBranchID = 0;
        int iCompanyID = 0;
        string raw_data = "";
        string processed_data = " "; 

        try
        {
            Contacts bllContact = new Contacts();
            LPWeb.Model.Contacts mContact = new LPWeb.Model.Contacts();
            mContact = bllContact.GetModel(iContactID);

            lbName.Text = mContact.LastName + ", " + mContact.FirstName;
            lbEnabled.Text = mContact.Enabled?"Yes":"No";

            if (mContact.CellPhone != null)
            {
                raw_data = mContact.CellPhone.Trim();

                raw_data = System.Text.RegularExpressions.Regex.Replace(raw_data,  @"[-() ]",  String.Empty);

                if (raw_data.Length > 10)
                {
                    raw_data = raw_data.Substring(0, 10);
                }

                if (raw_data.Length == 10)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 4);
                }
                else if (raw_data.Length == 9)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 3);
                }
                else if (raw_data.Length == 8)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 2);
                }
                else if (raw_data.Length == 7)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 1);
                }
                else if (raw_data.Length == 6)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-";
                }
                else if (raw_data.Length == 5)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 2) + " -";
                }
                else if (raw_data.Length == 4)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 1) + "  -";
                }
                else if (raw_data.Length == 3)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + "   -";
                }
                else if (raw_data.Length == 2)
                {
                    processed_data = "(" + raw_data.Substring(0, 2) + " ) " + "   -";
                }
                else if (raw_data.Length == 1)
                {
                    processed_data = "(" + raw_data.Substring(0, 1) + "  ) " + "   -";
                }
                else
                {
                    processed_data = "(   )    -    ";
                }
            }
            else
            {
                processed_data = "(   )    -    ";
            }
            lbCellPhone.Text = processed_data;
                       
            if (mContact.BusinessPhone != null)
            {
                raw_data = mContact.BusinessPhone.Trim();

                raw_data = System.Text.RegularExpressions.Regex.Replace(raw_data, @"[-() ]", String.Empty);
                if (raw_data.Length > 10)
                {
                    raw_data = raw_data.Substring(0, 10);
                }

                if (raw_data.Length == 10)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 4);
                }
                else if (raw_data.Length == 9)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 3);
                }
                else if (raw_data.Length == 8)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 2);
                }
                else if (raw_data.Length == 7)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 1);
                }
                else if (raw_data.Length == 6)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-";
                }
                else if (raw_data.Length == 5)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 2) + " -";
                }
                else if (raw_data.Length == 4)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 1) + "  -";
                }
                else if (raw_data.Length == 3)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + "   -";
                }
                else if (raw_data.Length == 2)
                {
                    processed_data = "(" + raw_data.Substring(0, 2) + " ) " + "   -";
                }
                else if (raw_data.Length == 1)
                {
                    processed_data = "(" + raw_data.Substring(0, 1) + "  ) " + "   -";
                }
                else
                {
                    processed_data = "(   )    -    ";
                }
            }
            else
            {
                processed_data = "(   )    -    ";
            }
            lbBussinessPhone.Text = processed_data;
                    
            raw_data = mContact.Fax;
            if (mContact.Fax != null)
            {
                raw_data = mContact.Fax.Trim();

                raw_data = System.Text.RegularExpressions.Regex.Replace(raw_data, @"[-() ]", String.Empty);
                if (raw_data.Length > 10)
                {
                    raw_data = raw_data.Substring(0, 10);
                }

                if (raw_data.Length == 10)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 4);
                }
                else if (raw_data.Length == 9)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 3);
                }
                else if (raw_data.Length == 8)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 2);
                }
                else if (raw_data.Length == 7)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 1);
                }
                else if (raw_data.Length == 6)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-";
                }
                else if (raw_data.Length == 5)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 2) + " -";
                }
                else if (raw_data.Length == 4)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 1) + "  -";
                }
                else if (raw_data.Length == 3)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + "   -";
                }
                else if (raw_data.Length == 2)
                {
                    processed_data = "(" + raw_data.Substring(0, 2) + " ) " + "   -";
                }
                else if (raw_data.Length == 1)
                {
                    processed_data = "(" + raw_data.Substring(0, 1) + "  ) " + "   -";
                }
                else
                {
                    processed_data = "(   )    -    ";
                }
            }
            else
            {
                processed_data = "(   )    -    ";
            }
            lbFax.Text = processed_data;

            lbEmail.Text = mContact.Email;
            if (mContact.ContactCompanyId.HasValue)
                iCompanyID = mContact.ContactCompanyId.Value;
            if (mContact.ContactBranchId.HasValue)
                iContactBranchID = mContact.ContactBranchId.Value;

            
        }
        catch (Exception ex)
        {
        
        }

        try
        {
            ContactBranches bllCB = new ContactBranches();
            LPWeb.Model.ContactBranches mCB = new LPWeb.Model.ContactBranches();
            mCB = bllCB.GetModel(iContactBranchID);
            lbBranch.Text = mCB.Name;
          
            if (mCB.Phone != null)
            {
                raw_data = mCB.Phone.Trim();

                raw_data = System.Text.RegularExpressions.Regex.Replace(raw_data, @"[-() ]", String.Empty);
                if (raw_data.Length > 10)
                {
                    raw_data = raw_data.Substring(0, 10);
                }

                if (raw_data.Length == 10)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 4);
                }
                else if (raw_data.Length == 9)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 3);
                }
                else if (raw_data.Length == 8)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 2);
                }
                else if (raw_data.Length == 7)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 1);
                }
                else if (raw_data.Length == 6)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-";
                }
                else if (raw_data.Length == 5)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 2) + " -";
                }
                else if (raw_data.Length == 4)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 1) + "  -";
                }
                else if (raw_data.Length == 3)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + "   -";
                }
                else if (raw_data.Length == 2)
                {
                    processed_data = "(" + raw_data.Substring(0, 2) + " ) " + "   -";
                }
                else if (raw_data.Length == 1)
                {
                    processed_data = "(" + raw_data.Substring(0, 1) + "  ) " + "   -";
                }
                else
                {
                    processed_data = "(   )    -    ";
                }
            }
            else
            {
                processed_data = "(   )    -    ";
            }
            lbPhone.Text = processed_data;

            if (mCB.Fax != null)
            {
                raw_data = mCB.Fax.Trim();

                raw_data = System.Text.RegularExpressions.Regex.Replace(raw_data, @"[-() ]", String.Empty);
                if (raw_data.Length > 10)
                {
                    raw_data = raw_data.Substring(0, 10);
                }

                if (raw_data.Length == 10)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 4);
                }
                else if (raw_data.Length == 9)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 3);
                }
                else if (raw_data.Length == 8)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 2);
                }
                else if (raw_data.Length == 7)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-" + raw_data.Substring(6, 1);
                }
                else if (raw_data.Length == 6)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 3) + "-";
                }
                else if (raw_data.Length == 5)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 2) + " -";
                }
                else if (raw_data.Length == 4)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + raw_data.Substring(3, 1) + "  -";
                }
                else if (raw_data.Length == 3)
                {
                    processed_data = "(" + raw_data.Substring(0, 3) + ") " + "   -";
                }
                else if (raw_data.Length == 2)
                {
                    processed_data = "(" + raw_data.Substring(0, 2) + " ) " + "   -";
                }
                else if (raw_data.Length == 1)
                {
                    processed_data = "(" + raw_data.Substring(0, 1) + "  ) " + "   -";
                }
                else
                {
                    processed_data = "(   )    -    ";
                }
            }
            else
            {
                processed_data = "(   )    -    ";
            }
            lbFax1.Text = processed_data;
            lbAddress.Text = mCB.Address;
            lbCity.Text = mCB.City + " " + mCB.State + " " + mCB.Zip;
            if (mCB.ContactCompanyId.HasValue&&iCompanyID==0)
                iCompanyID = mCB.ContactCompanyId.Value;
        }
        catch (Exception ex)
        { }

        int iServiceTypeID = 0;
        try
        {
            ContactCompanies bllCC = new ContactCompanies();
            LPWeb.Model.ContactCompanies mCC = new LPWeb.Model.ContactCompanies();
            mCC = bllCC.GetModel(iCompanyID);
            //lbServiceType.Text = mCC.ServiceTypes;
            lbCompany.Text = mCC.Name;
            iServiceTypeID = mCC.ServiceTypeId ;
        }
        catch
        { }


        try
        { 
            ServiceTypes blST = new ServiceTypes();
            LPWeb.Model.ServiceTypes mST = new LPWeb.Model.ServiceTypes();
            mST = blST.GetModel(iServiceTypeID);
            lbServiceType.Text = mST.Name; 
        }
        catch
        { }

        #region Referral Amount/Referral Funded/Wind Ratio

        string sSql = "select dbo.lpfn_GetTotalReferral(" + this.iContactID + ", " + this.CurrUser.iUserID + ") as TotalReferral, "
                    + "dbo.lpfn_GetTotalReferralFunded(" + this.iContactID + ", " + this.CurrUser.iUserID + ") as TotalReferralFunded, "
                    + "dbo.lpfn_GetTotalReferral_FileIDs(" + this.iContactID + ", " + this.CurrUser.iUserID + ") as TotalReferralFileIDs, "
                    + "isnull(dbo.lpfn_GetTotalReferralFunded(" + this.iContactID + ", " + this.CurrUser.iUserID + "),0)/dbo.lpfn_GetTotalReferral(" + this.iContactID + ", " + this.CurrUser.iUserID + ") as WinRatio";
        DataTable ReferralAmountInfo = LPWeb.DAL.DbHelperSQL.ExecuteDataTable(sSql);
        if(ReferralAmountInfo.Rows.Count > 0)
        {
            if (ReferralAmountInfo.Rows[0]["TotalReferral"] == DBNull.Value) 
            {
                this.aTotalReferral.InnerText = string.Empty;
                this.aTotalReferral.HRef = string.Empty;
            }
            else
            {
                this.aTotalReferral.InnerText = Convert.ToDecimal(ReferralAmountInfo.Rows[0]["TotalReferral"]).ToString("c0");
                this.aTotalReferral.HRef = "javascript:window.parent.SetTab('PartnerContactDetailReferralstab.aspx',3);";
            }

            if (ReferralAmountInfo.Rows[0]["TotalReferralFunded"] == DBNull.Value)
            {
                this.lbTotalReferralFunded.Text = string.Empty;
            }
            else
            {
                this.lbTotalReferralFunded.Text = Convert.ToDecimal(ReferralAmountInfo.Rows[0]["TotalReferralFunded"]).ToString("c0");
            }

            if (ReferralAmountInfo.Rows[0]["WinRatio"] == DBNull.Value)
            {
                this.lbWinRatio.Text = string.Empty;
            }
            else
            {
                this.lbWinRatio.Text = Convert.ToDecimal(ReferralAmountInfo.Rows[0]["WinRatio"]).ToString("p0");
            }
        }

        #endregion
    }
}
