using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;
using Utilities;
using System.Web.UI;
using LPWeb.LP_Service;
using LPWeb.Common;
using System.Collections.Generic;

namespace LPWeb.Layouts.LPWeb.Settings
{
    /// <summary>
    /// Personalization - Settings
    /// Author: Peter
    /// Date: 2011-05-25
    /// </summary>
    public partial class PersonalizationSettings : BasePage
    {
        BLL.Users UsersManager = new BLL.Users();
        BLL.Company_General comGeneral = new BLL.Company_General();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Model.Users userInfo = UsersManager.GetModel(CurrUser.iUserID);
                if (null == userInfo)
                {
                    LPLog.LogMessage(LogType.Logerror, string.Format("User Personalization: User with id {0} does not exist.", CurrUser.iUserID));
                    ClientFun("unknowerrormsg", "alert('User does not exists, unknow error.');");
                }
                if (CurrUser.userRole.SetOwnGoals || CurrUser.userRole.SetUserGoals)
                {
                    this.btnSetGoals.OnClientClick = string.Format("showUserGoalsSetupWin('{0}'); return false;", CurrUser.iUserID);
                }
                else
                {
                    this.btnSetGoals.Enabled = false;
                }
                Model.Company_General company = comGeneral.GetModel();
                if (null != company)
                    this.hiPrefix.Value = company.AD_OU_Filter;
                this.lbUserName.Text = string.Format("{0} {1}", userInfo.FirstName, userInfo.LastName);
                this.lbEmail.Text = userInfo.EmailAddress;
                this.hiUsername.Value = userInfo.Username;
                this.hiFirstName.Value = userInfo.FirstName;
                this.hiLastName.Value = userInfo.LastName;

                this.txbCell.Text = userInfo.Cell;
                this.txbFax.Text = userInfo.Fax;
                this.txbPhone.Text = userInfo.Phone;
                this.txbLicense.Text = userInfo.LicenseNumber;
                this.txbExchangPwd.Text = userInfo.ExchangePassword;
                this.txbExchangPwd.Attributes.Add("value", userInfo.ExchangePassword);

                //gdc CR43
                this.txbNMLS.Text = userInfo.NMLS;

                #region show my picture neo

                this.imgUserPhoto.Attributes.Add("onload", string.Format("resizeImage('{0}')", this.imgUserPhoto.ClientID));

                #endregion

                // My Signature
                this.txtSignature.Text = userInfo.Signature;

                // password area
                if (string.IsNullOrEmpty(userInfo.Password))
                {
                    this.trPwd.Attributes.CssStyle.Add("display", "none");
                    this.lbtnChangePwd.Attributes.CssStyle.Add("display", "");
                    this.lbtnChangePwd.Text = "Store your password";
                    this.lbtnCancelPwd.Attributes.CssStyle.Add("display", "none");
                }
                else
                {
                    this.trPwd.Attributes.CssStyle.Add("display", "none");
                    this.lbtnChangePwd.Attributes.CssStyle.Add("display", "");
                    this.lbtnCancelPwd.Attributes.CssStyle.Add("display", "none");
                }


                BindLicensesList();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Model.Users userInfo = UsersManager.GetModel(CurrUser.iUserID);
            if (userInfo.UserPictureFile == null)
            {
                this.imgUserPhoto.ImageUrl = string.Format("~/_layouts/LPWeb/Settings/GetUserPicture.aspx?t={0}", DateTime.Now.Ticks);
            }
            else
            {
                this.imgUserPhoto.ImageUrl = string.Format("~/_layouts/LPWeb/Settings/GetUserPicture.aspx?uid={0}&t={1}", this.CurrUser.iUserID.ToString(), DateTime.Now.Ticks);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Model.Users userInfo = UsersManager.GetModel(CurrUser.iUserID);

            try
            {
                if (null == userInfo)
                {
                    LPLog.LogMessage(LogType.Logerror, string.Format("User Personalization - Settings: User with id {0} does not exist.", CurrUser.iUserID));
                    ClientFun("unknowerrmsg2", "alert('User does not exists, unknow error.');");
                    return;
                }
                if (!GetUserInfo(ref userInfo))
                {
                    ClientFun("invalidinputmsg", "alert('Invalid input!');");
                    return;
                }

                if (this.FileUpload1.HasFile)
                {
                    string strMsg = "";
                    bool bIsValid = PageCommon.ValidateUpload(this, this.FileUpload1, 1024 * 1024 * 15, out strMsg, ".jpg", ".bmp", ".png", ".gif");
                    if (!bIsValid)
                    {
                        ClientFun("userPicInvalid", string.Format("alert('{0}');", strMsg));
                        return;
                    }
                }

                string phone = this.txbPhone.Text.Trim();
                userInfo.Phone = phone;
                userInfo.Fax = this.txbFax.Text.Trim();
                userInfo.Cell = this.txbCell.Text.Trim();
                userInfo.LicenseNumber = this.txbLicense.Text.Trim();
                userInfo.NMLS = this.txbNMLS.Text.Trim();
                userInfo.ExchangePassword = this.txbExchangPwd.Text.Trim();
                if (userInfo.ExchangePassword == "" && !string.IsNullOrEmpty(this.tbPWD.Text))
                {
                    userInfo.ExchangePassword = this.tbPWD.Text.Trim();
                }


                UsersManager.Update(userInfo);


                #region Save as UserLicense    gdc CR43

                if (!string.IsNullOrEmpty(hidLicenseNumberList.Value.Trim()))
                {
                    try
                    {
                        List<Model.UserLicenses> ulList = new List<Model.UserLicenses>();
                        foreach (var item in hidLicenseNumberList.Value.Split(','))
                        {
                            if (string.IsNullOrEmpty(item))
                            {
                                continue;
                            }

                            Model.UserLicenses model = new Model.UserLicenses();

                            model.LicenseNumber = item;
                            model.UserId = CurrUser.iUserID;

                            ulList.Add(model);
                        }

                        BLL.UserLicenses ulBll = new BLL.UserLicenses();

                        ulBll.UpdatebatchByUserID(ulList);

                        BindLicensesList();
                    }
                    catch (Exception ex)
                    {
                        LPLog.LogMessage(ex.Message);
                        PageCommon.AlertMsg(this, "Failed to Save User Licenses:" + ex.Message);
                        return;
                    }
                }

                #endregion

                if (!string.IsNullOrEmpty(this.tbPWD.Text))
                {
                    try
                    {
                        ServiceManager sm = new ServiceManager();
                        using (LP2ServiceClient service = sm.StartServiceClient())
                        {
                            ReqHdr hdr;
                            UpdateADUserRequest uReq = new UpdateADUserRequest();
                            hdr = new ReqHdr();
                            hdr.UserId = CurrUser.iUserID;
                            uReq.hdr = hdr;
                            uReq.Command = UserMgrCommandType.ChangePassword;
                            uReq.AD_OU_Filter = this.hiPrefix.Value;
                            uReq.AD_User = new LP_Service.User();
                            uReq.AD_User.Password = this.tbPWD.Text;
                            uReq.AD_User.Username = this.hiUsername.Value;
                            uReq.AD_User.Firstname = this.hiFirstName.Value;
                            uReq.AD_User.Lastname = this.hiLastName.Value;
                            uReq.AD_User.Email = this.lbEmail.Text;

                            UpdateADUserResponse uResponse;
                            uResponse = service.UpdateADUser(uReq);
                            if (!uResponse.hdr.Successful)
                            {
                                PageCommon.AlertMsg(this, "Failed to update your password. Please make sure your new password meets the password policy requirements.");
                                LPLog.LogMessage(LogType.Logerror, "Failed to change password in AD, username=" + this.hiUsername.Value);
                                return;
                            }
                        }
                    }
                    catch (System.ServiceModel.EndpointNotFoundException ee)
                    {
                        LPLog.LogMessage(ee.Message);
                        PageCommon.AlertMsg(this, "Failed to change password in AD, reason: User Manager is not running.");
                        return;
                    }
                    catch (Exception ex)
                    {
                        PageCommon.AlertMsg(this, "Failed to change password in AD, exception info: " + ex.Message);
                        LPLog.LogMessage(LogType.Logerror, "Failed to change password in AD, username=" + this.hiUsername.Value + ", Exception:" + ex.Message);
                        return;
                    }
                }

                ClientFun("sucsmsg", "alert('Saved!');");
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Failed to save user Personalization - Settings info, reason:" + ex.Message);
                LPLog.LogMessage(LogType.Logerror, "Failed to save user Personalization - Settings info: " + ex.Message);
                return;
            }
        }

        /// <summary>
        /// load Users Personalization Info
        /// </summary>
        /// <param name="user"></param>
        private bool GetUserInfo(ref Model.Users user)
        {
            if (!string.IsNullOrEmpty(this.tbPWD.Text))
            {
                if (this.tbPWD.Text == this.tbPWDCfm.Text)
                    user.Password = this.tbPWD.Text;
                else
                    return false;
            }

            #region my picture neo

            if (this.FileUpload1.PostedFile.ContentLength > 0)
            {
                byte[] ImageData = new byte[this.FileUpload1.PostedFile.ContentLength];
                this.FileUpload1.PostedFile.InputStream.Read(ImageData, 0, this.FileUpload1.PostedFile.ContentLength);
                user.UserPictureFile = ImageData;
            }

            #endregion

            #region my signature neo

            string sMySignature = this.txtSignature.Text.Trim();
            user.Signature = sMySignature;

            #endregion

            return true;
        }

        /// <summary>
        /// call client function
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strScript"></param>
        private void ClientFun(string strKey, string strScript)
        {
            ClientScriptManager csm = this.Page.ClientScript;
            if (!csm.IsStartupScriptRegistered(this.GetType(), strKey))
            {
                csm.RegisterStartupScript(this.GetType(), strKey, strScript, true);
            }
        }


        private void BindLicensesList()
        {
            BLL.UserLicenses bll = new BLL.UserLicenses();

            gridLicensesList.DataSource = bll.GetList("UserID = " + CurrUser.iUserID);
            gridLicensesList.DataBind();

        }
    }
}
