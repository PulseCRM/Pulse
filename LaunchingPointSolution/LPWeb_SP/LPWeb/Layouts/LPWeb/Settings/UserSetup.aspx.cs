using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Utilities;
using System.Data;
using System.Xml;
using System.Text;
using LPWeb.Common;
using LPWeb.LP_Service;
using Microsoft.SharePoint.WebControls;
using LPWeb.Layouts.LPWeb.Common;

namespace LPWeb.Settings
{
    /// <summary>
    /// User Setup: set user info
    /// Author: Peter
    /// Date: 2010-09-01
    /// </summary>
    public partial class UserSetup : BasePage
    {
        BLL.Users UsersManager = new BLL.Users();
        BLL.UserLoanRep UserLoanRepManager = new BLL.UserLoanRep();
        BLL.Groups GroupManager = new BLL.Groups();
        BLL.GroupUsers GroupUserManager = new BLL.GroupUsers();
        BLL.LoanTeam LoanTeamManager = new BLL.LoanTeam();
        BLL.ContactUsers ContactUsersManager = new BLL.ContactUsers();

        protected int Random
        {
            get
            {
                Random random = new Random(1);
                return random.Next(1, 10000);
            }
        }

        /// <summary>
        /// UserId
        /// </summary>
        protected int? UserId
        {
            get
            {
                if (null == ViewState["uid"])
                    return null;
                int nUID = -1;
                if (!int.TryParse(ViewState["uid"].ToString(), out nUID))
                {
                    nUID = -1;
                }
                if (-1 == nUID)
                    return null;
                else
                    return nUID;
            }
            set
            {
                ViewState["uid"] = value;
            }
        }

        /// <summary>
        /// Page mode：
        /// 0：new
        /// 1：edit
        /// </summary>
        protected string Mode
        {
            get
            {
                if (null == ViewState["mode"])
                    return null;
                else
                    return ViewState["mode"].ToString();
            }
            set
            {
                ViewState["mode"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                if (!CurrUser.userRole.CompanySetup)
                {
                    Response.Redirect("../Unauthorize.aspx");
                    Response.End();
                }

                this.imgUserPic.Attributes.Add("onload", string.Format("resizeImage('{0}')", this.imgUserPic.ClientID));
                // get UserId from query string
                int nUserId = -1;
                if (!int.TryParse(Request.QueryString["uid"], out nUserId))
                    nUserId = -1;
                if (-1 == nUserId)
                    UserId = null;
                else
                    UserId = nUserId;

                BLL.Company_General comGeneral = new BLL.Company_General();
                Model.Company_General company = comGeneral.GetModel();
                if (null != company)
                    this.lbPrefix.Text = company.AD_OU_Filter;

                // bind User Loan Rep gridview
                BindLoanRep();

                // bind Group gridview
                BindGroup();

                // bind Role dropdown list
                BLL.Roles RolesManager = new BLL.Roles();
                DataSet dsRoles = RolesManager.GetList(string.Empty);
                this.ddlRole.DataSource = dsRoles;
                this.ddlRole.DataValueField = "RoleId";
                this.ddlRole.DataTextField = "Name";
                this.ddlRole.DataBind();
                this.ddlRole.Items.Insert(0, new ListItem("Please Select", ""));
                Mode = Request.QueryString["mode"];
                if ("0" == Mode)
                {
                    this.btnDelete.Enabled = false;
                    this.btnClone.Enabled = false;
                }
                if ("1" == Mode)
                {
                    // Load user info
                    if (!UserId.HasValue)
                    {
                        // if no UserId，thorw exception
                        LPLog.LogMessage(LogType.Logerror, "Invalid UserId");
                        throw new Exception("Invalid UserId");
                    }
                    else
                    {
                        Model.Users user = UsersManager.GetModel(UserId.Value);
                        if (null == user)
                        {
                            LPLog.LogMessage(LogType.Logerror, string.Format("Cannot find the user with UserId = {0}", UserId.Value));
                        }
                        else
                        {
                            this.ckbEnabled.Checked = user.UserEnabled;
                            this.tbUserName.Text = user.Username;
                            this.tbEmail.Text = user.EmailAddress;
                            this.tbFirstName.Text = user.FirstName;
                            this.tbLastName.Text = user.LastName;
                            ListItem item = this.ddlRole.Items.FindByValue(user.RoleId.ToString());
                            if (null != item)
                            {
                                this.ddlRole.ClearSelection();
                                item.Selected = true;
                            }
                            this.hiUserLoanCount.Value = LoanTeamManager.GetUserLoanCount(UserId.Value).ToString();
                            this.hiUserContactCount.Value = ContactUsersManager.GetUserContactCount(UserId.Value).ToString();

                            //gdc 20110606 Add
                            this.txbPhone.Text = user.Phone;
                            this.txbFax.Text = user.Fax;
                            this.txbCell.Text = user.Cell;

                            this.txbBranchManagerCompensation.Text = user.BranchMgrComp.ToString("00.000");
                            this.txbDivisionManagerCompensation.Text = user.DivisionMgrComp.ToString("00.000");
                            this.txbLoanOfficerCompenstation.Text = user.LOComp.ToString("00.000");
                            this.txbRegionalManagerCompensation.Text = user.RegionMgrComp.ToString("00.000");

                            //ExchangePassword
                            this.txbExchangePassword.Text = user.ExchangePassword;
                            this.txbExchangePassword.Attributes.Add("value", user.ExchangePassword);
                        }
                    }
                }

                if (UserId.HasValue && Mode == "1")
                {
                    this.tbUserName.Enabled = false;
                    this.tbUserName.BackColor = System.Drawing.Color.LightGray;
                }
                else
                {
                    this.tbUserName.Enabled = true;
                    this.tbUserName.BackColor = System.Drawing.Color.Transparent;
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if ("0" == Mode)
            {
                this.imgUserPic.ImageUrl = string.Format("~/_layouts/LPWeb/Settings/GetUserPicture.aspx?t={0}", DateTime.Now.Ticks);

                this.trPwd.Attributes.CssStyle.Add("display", "");
                this.lbtnChangePwd.Attributes.CssStyle.Add("display", "none");
                this.lbtnCancelPwd.Attributes.CssStyle.Add("display", "none");
            }
            if ("1" == Mode)
            {
                this.imgUserPic.ImageUrl = string.Format("~/_layouts/LPWeb/Settings/GetUserPicture.aspx?uid={0}&t={1}", UserId, DateTime.Now.Ticks);

                this.trPwd.Attributes.CssStyle.Add("display", "none");
                this.lbtnChangePwd.Attributes.CssStyle.Add("display", "");
                this.lbtnCancelPwd.Attributes.CssStyle.Add("display", "none");
            }
        }

        /// <summary>
        /// Delete all user info when click "Delete" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            bool bRessignSuccess = false;
            ServiceManager sm1 = new ServiceManager();
            using (LP2ServiceClient service = sm1.StartServiceClient())
            {
                ReassignLoanRequest req = new ReassignLoanRequest();
                req.hdr = new ReqHdr();
                req.hdr.SecurityToken = "SecurityToken"; //todo:check dummy data
                req.hdr.UserId = CurrUser.iUserID;

                // get all loans assigned to this user
                DataSet dsUserLoan = LoanTeamManager.GetUserLoan(UserId.Value);
                int nReassignUserId = 0;
                if (!int.TryParse(this.hiReassignUserId.Value, out nReassignUserId))
                    nReassignUserId = 0;
                if (null != dsUserLoan && dsUserLoan.Tables.Count > 0 && dsUserLoan.Tables[0].Rows.Count > 0)
                {
                    List<ReassignUserInfo> uList = new List<ReassignUserInfo>();

                    Model.Users userToDelete = UsersManager.GetModel(UserId.Value);
                    ReassignUserInfo uInfo = null;
                    foreach (DataRow drUserLoan in dsUserLoan.Tables[0].Rows)
                    {
                        uInfo = new ReassignUserInfo();
                        uInfo.FileId = int.Parse(string.Format("{0}", drUserLoan["FileId"]));
                        uInfo.RoleId = int.Parse(string.Format("{0}", drUserLoan["RoleId"]));
                        uInfo.NewUserId = nReassignUserId;
                        uList.Add(uInfo);
                    }

                    req.reassignUsers = uList.ToArray();
                    ReassignLoanResponse respone = null;
                    try
                    {
                        respone = service.ReassignLoan(req);
                        if (respone.hdr.Successful)
                        {
                            bool st = true;

                            int fileId = 0;
                            int newUserId = 0;
                            int roleId = 0;
                            int requester = req.hdr.UserId;

                            foreach (ReassignUserInfo u in req.reassignUsers)
                            {
                                fileId = u.FileId;
                                newUserId = u.NewUserId;
                                roleId = u.RoleId;

                                st = LPWeb.BLL.WorkflowManager.ReassignLoan(fileId, newUserId, UserId.Value, roleId, requester);
                            }
                            bRessignSuccess = true;
                        }
                        else
                        {
                            PageCommon.AlertMsg(this, "Failed to reassign loan, reason:" + respone.hdr.StatusInfo);
                            CloseMe(true, true);
                            return;
                        }
                    }
                    catch (System.ServiceModel.EndpointNotFoundException ex)
                    {
                        LPLog.LogMessage(ex.Message);
                        PageCommon.AlertMsg(this, "Failed to reassign loan, reason: User Manager is not running.");
                        CloseMe(true, true);
                    }
                    catch (Exception exception)
                    {
                        LPLog.LogMessage(exception.Message);
                        PageCommon.AlertMsg(this, string.Format("Failed to reassign loan, reason:{0}.", exception.Message));
                        CloseMe(true, true);
                    }
                }
                else
                {
                    bRessignSuccess = true;
                }

                if (bRessignSuccess)
                {
                    UsersManager.DeleteUserInfo(UserId.Value, null, null, CurrUser.iUserID, nReassignUserId);
                    LPLog.LogMessage(string.Format("User {0} deleted successfully.", this.tbUserName.Text));
                    CloseMe(true, true);

                    try
                    {
                        ServiceManager sm = new ServiceManager();
                        using (LP2ServiceClient service2 = sm.StartServiceClient())
                        {
                            UpdateADUserRequest uReq = new UpdateADUserRequest();
                            ReqHdr hdr = new ReqHdr();
                            hdr.UserId = UserId.Value;
                            uReq.hdr = hdr;
                            uReq.Command = UserMgrCommandType.DeleteUser;
                            uReq.AD_OU_Filter = this.lbPrefix.Text;
                            uReq.AD_User = new LP_Service.User();
                            uReq.AD_User.Username = this.tbUserName.Text;
                            uReq.AD_User.Firstname = this.tbFirstName.Text;
                            uReq.AD_User.Lastname = this.tbLastName.Text;
                            uReq.AD_User.Email = this.tbEmail.Text;
                            uReq.AD_User.Enabled = this.ckbEnabled.Checked;

                            UpdateADUserResponse uResponse;
                            uResponse = service2.UpdateADUser(uReq);
                            if (!uResponse.hdr.Successful)
                            {
                                PageCommon.AlertMsg(this, "Failed to delete user in AD, reason:" + uResponse.hdr.StatusInfo);
                                LPLog.LogMessage(LogType.Logerror, "Failed to delete user in AD. UserID: " + UserId.ToString());
                            }
                        }
                    }
                    catch (System.ServiceModel.EndpointNotFoundException ee)
                    {
                        LPLog.LogMessage(ee.Message);
                        PageCommon.AlertMsg(this, "Failed to delete user in AD, reason: User Manager is not running.");
                    }
                    catch (Exception ex)
                    {
                        PageCommon.AlertMsg(this, "Failed to delete user in AD, reason: " + ex.Message);
                        LPLog.LogMessage(LogType.Logerror, "Error occured while trying to delete user in AD: " + ex.Message);
                    }
                }
            }
        }

        private void CloseMe(bool bRefresh, bool bReset)
        {
            // delete personalization，delete Group info，delete LoanRepMapping, Use tarnsation
            ClientScriptManager csm = this.Page.ClientScript;
            string strKey = "closeme";
            string strScript = string.Format("closeBox({0}, {1});", bRefresh ? "true" : "", bReset ? "true" : "");
            if (!csm.IsStartupScriptRegistered(this.GetType(), strKey))
            {
                csm.RegisterStartupScript(this.GetType(), strKey, strScript, true);
            }
        }

        /// <summary>
        /// Register client javascript function
        /// </summary>
        /// <param name="strScript"></param>
        private void CallClientFn(string strScript)
        {
            ClientScriptManager csm = this.Page.ClientScript;
            string strKey = "clientcallback";
            if (!csm.IsStartupScriptRegistered(this.GetType(), strKey))
            {
                csm.RegisterStartupScript(this.GetType(), strKey, strScript, true);
            }
        }

        /// <summary>
        /// Save user info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Model.Users newUser = null;
            if ("0" == Mode)
            {
                newUser = new Model.Users();
                if (GetValue(ref newUser))
                {
                    //set the new user's default loans per page(50)
                    newUser.LoansPerPage = 50;

                    if (!CheckBeforeSave(newUser))
                        return;

                    int? nSourceUID = null;
                    int nTemp = 0;
                    if (null != ViewState["SourceUID"])
                    {
                        if (int.TryParse(ViewState["SourceUID"].ToString(), out nTemp))
                            nSourceUID = nTemp;
                    }

                    try
                    {
                        // save the new added user info, nSourceUID is null or not indicated to clone or not
                        UserId = UsersManager.AddUserInfo(newUser, this.hiCurrLoanRep.Value, this.hiCurrGroup.Value, nSourceUID);
                        Mode = "1";
                        this.btnDelete.Enabled = true;
                        this.btnClone.Enabled = true;

                        try
                        {
                            ServiceManager sm = new ServiceManager();
                            using (LP2ServiceClient service = sm.StartServiceClient())
                            {
                                UpdateADUserRequest uReq = new UpdateADUserRequest();
                                ReqHdr hdr = new ReqHdr();
                                hdr.UserId = UserId.Value;
                                uReq.hdr = hdr;
                                uReq.AD_OU_Filter = this.lbPrefix.Text;
                                uReq.AD_User = new LP_Service.User();
                                uReq.AD_User.Username = this.tbUserName.Text;
                                uReq.AD_User.Firstname = this.tbFirstName.Text;
                                uReq.AD_User.Lastname = this.tbLastName.Text;
                                uReq.AD_User.Email = this.tbEmail.Text;
                                uReq.AD_User.Enabled = this.ckbEnabled.Checked;
                                if (!string.IsNullOrEmpty(this.tbPWD.Text.Trim()))
                                    uReq.AD_User.Password = this.tbPWD.Text.Trim();
                                else
                                    uReq.AD_User.Password = string.Empty;
                                uReq.Command = UserMgrCommandType.CreateUser;

                                UpdateADUserResponse uResponse;
                                uResponse = service.UpdateADUser(uReq);
                                if (!uResponse.hdr.Successful)
                                {
                                    PageCommon.AlertMsg(this, "Failed to save user info in AD.");
                                    CallClientFn("closeBox(true);");
                                    LPLog.LogMessage(LogType.Logerror, "Failed to save user info in AD. UserID: " + UserId.ToString() + ", Error: " + uResponse.hdr.StatusInfo);
                                    return;
                                }
                            }
                        }
                        catch (System.ServiceModel.EndpointNotFoundException ee)
                        {
                            LPLog.LogMessage(ee.Message);
                            CallClientFn("closeBox(true);");
                            PageCommon.AlertMsg(this, "Failed to save user info in AD, reason: User Manager is not running.");
                            return;
                        }
                        catch (Exception ex)
                        {
                            PageCommon.AlertMsg(this, "Failed to save user info in AD, reason:"+ex.Message);
                            CallClientFn("closeBox(true);");
                            LPLog.LogMessage(LogType.Logerror, "Error occured when save user info in AD: " + ex.Message);
                            return;
                        }

                        CallClientFn("ShowMsg('saveSuccss', '', true, true);");
                    }
                    catch (Exception ex)
                    {
                        PageCommon.AlertMsg(this, "Failed to save user info, reason:"+ex.Message);
                        LPLog.LogMessage(LogType.Logerror, "Error occured when save user info: " + ex.Message);
                        return;
                    }
                }
                else
                {
                    CallClientFn("ShowMsg('invalidInput', '', false);");
                }
            }
            else if ("1" == Mode)
            {
                newUser = UsersManager.GetModel(UserId.Value);
                bool changePwd = false;
                if ((newUser.Password != tbPWD.Text.Trim()) &&
                    !string.IsNullOrEmpty(tbPWD.Text.Trim()))
                    changePwd = true;
                if (GetValue(ref newUser))
                {
                    if (!CheckBeforeSave(newUser))
                        return;
                    try
                    {
                        // update current user info, without personalization info
                        UsersManager.UpdateUserInfo(newUser, this.hiCurrLoanRep.Value, this.hiCurrGroup.Value);

                        try
                        {
                            ServiceManager sm = new ServiceManager();
                            using (LP2ServiceClient service = sm.StartServiceClient())
                            {
                                UpdateADUserRequest uReq = new UpdateADUserRequest();
                                ReqHdr hdr = new ReqHdr();
                                hdr.UserId = UserId.Value;
                                uReq.hdr = hdr;
                                uReq.AD_OU_Filter = this.lbPrefix.Text;
                                uReq.AD_User = new LP_Service.User();
                                uReq.AD_User.Username = this.tbUserName.Text;
                                uReq.AD_User.Firstname = this.tbFirstName.Text;
                                uReq.AD_User.Lastname = this.tbLastName.Text;
                                uReq.AD_User.Email = this.tbEmail.Text;
                                uReq.AD_User.Enabled = this.ckbEnabled.Checked;
                                if (changePwd)
                                    uReq.AD_User.Password = this.tbPWD.Text;
                                else uReq.AD_User.Password = string.Empty;
                                uReq.Command = UserMgrCommandType.UpdateUser;

                                UpdateADUserResponse uResponse;
                                uResponse = service.UpdateADUser(uReq);
                                if (!uResponse.hdr.Successful)
                                {
                                    PageCommon.AlertMsg(this, "Failed to update user info in AD, reason: "+uResponse.hdr.StatusInfo);
                                    CallClientFn("closeBox(true, false);");
                                    LPLog.LogMessage(LogType.Logerror, "Failed to update user info to AD. UserID: " + UserId.ToString() + ", Error: " + uResponse.hdr.StatusInfo);
                                    return;
                                }
                            }
                        }
                        catch (System.ServiceModel.EndpointNotFoundException ee)
                        {
                            LPLog.LogMessage(ee.Message);
                            CallClientFn("closeBox(true, false);");
                            PageCommon.AlertMsg(this, "Failed to update user info in AD, reason: User Manager is not running.");
                            return;
                        }
                        catch (Exception ex)
                        {
                            PageCommon.AlertMsg(this, "Failed to update user info in AD, reason: "+ex.Message);
                            CallClientFn("closeBox(true, false);");
                            LPLog.LogMessage(LogType.Logerror, "Error occured when update user info to AD: " + ex.Message);
                            return;
                        }

                        CallClientFn("ShowMsg('saveSuccss', '', true, true, false);");
                    }
                    catch (Exception ex)
                    {
                        PageCommon.AlertMsg(this, "Failed to save user info, reason: " + ex.Message);
                        LPLog.LogMessage(LogType.Logerror, "Failed to save user info: " + ex.Message);
                        return;
                    }
                }
                else
                {
                    CallClientFn("ShowMsg('invalidInput', '', false);");
                }
            }
        }

        /// <summary>
        /// duplicate check, username and email address
        /// </summary>
        /// <returns></returns>
        private bool CheckBeforeSave(Model.Users user)
        {
            if (UsersManager.IsUserNameExists(user.UserId, user.Username))
            {
                CallClientFn("ShowMsg('userNameExists', '', false);");
                return false;
            }
            //else if (!string.IsNullOrEmpty(user.Username) && UsersManager.IsUserEmailExists(user.UserId, user.EmailAddress))
            //{
            //    CallClientFn("ShowMsg('userEmailExists', '', false);");
            //    return false;
            //}

            if (this.fuPicture.HasFile)
            {
                string strMsg = "";
                bool bIsValid = PageCommon.ValidateUpload(this, this.fuPicture, 1024 * 1024 * 15, out strMsg, ".jpg", ".bmp", ".png", ".gif");
                if (!bIsValid)
                {
                    CallClientFn(string.Format("ShowMsg('userPicInvalid', '{0}', false);", strMsg));
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Load user info from the form
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private bool GetValue(ref Model.Users user)
        {
            if (string.IsNullOrEmpty(this.tbUserName.Text.Trim())
                || string.IsNullOrEmpty(this.tbFirstName.Text.Trim())
                || string.IsNullOrEmpty(this.tbLastName.Text.Trim())
                || string.IsNullOrEmpty(this.ddlRole.SelectedValue))
            {
                return false;
            }
            user.UserEnabled = this.ckbEnabled.Checked;
            user.Username = this.tbUserName.Text.Trim();
            user.EmailAddress = string.IsNullOrEmpty(this.tbEmail.Text.Trim()) ? null : this.tbEmail.Text.Trim();
            user.FirstName = this.tbFirstName.Text.Trim();
            user.LastName = this.tbLastName.Text.Trim();
            if (!string.IsNullOrEmpty(this.tbPWD.Text))
                user.Password = this.tbPWD.Text;
            int nRoleId = -1;
            if (!int.TryParse(this.ddlRole.SelectedValue, out nRoleId))
                nRoleId = -1;
            user.RoleId = nRoleId;

            //gdc 20110606 Add
            user.Cell = this.txbCell.Text.Trim();
            user.Phone = this.txbPhone.Text.Trim();
            user.Fax = this.txbFax.Text.Trim();

            //ExchangePassword
            user.ExchangePassword = this.txbExchangePassword.Text.Trim();
            //Default Exchange password: users.password
            if (user.ExchangePassword == "" && user.Password != "")
            {
                user.ExchangePassword = user.Password;
            }

            if (this.fuPicture.HasFile)
            {
                user.UserPictureFile = this.fuPicture.FileBytes;
            }


            //CR5 add

            user.RegionMgrComp = string.IsNullOrEmpty(txbRegionalManagerCompensation.Text.Trim()) ? 0 : Convert.ToDecimal(txbRegionalManagerCompensation.Text.Trim());
            user.BranchMgrComp = string.IsNullOrEmpty(txbBranchManagerCompensation.Text.Trim()) ? 0 : Convert.ToDecimal(txbBranchManagerCompensation.Text.Trim());
            user.DivisionMgrComp = string.IsNullOrEmpty(txbDivisionManagerCompensation.Text.Trim()) ? 0 : Convert.ToDecimal(txbDivisionManagerCompensation.Text.Trim());
            user.LOComp = string.IsNullOrEmpty(txbLoanOfficerCompenstation.Text.Trim()) ? 0 : Convert.ToDecimal(txbLoanOfficerCompenstation.Text.Trim());


            return true;
        }

        protected void btnClone_Click(object sender, EventArgs e)
        {
            Mode = "0";
            this.btnDelete.Enabled = false;
            this.btnClone.Enabled = false;
            this.tbUserName.Text = "";
            this.tbFirstName.Text = "";
            this.tbLastName.Text = "";
            this.tbEmail.Text = "";
            this.hiLoanRep.Value = "";
            this.tbUserName.Enabled = true;
            this.tbUserName.BackColor = System.Drawing.Color.Transparent;
            //gdc 20110606
            this.txbPhone.Text = "";
            this.txbFax.Text = "";
            this.txbCell.Text = "";

            this.txbExchangePassword.Text = "";

            //CR52 
            txbLoanOfficerCompenstation.Text = "";
            txbRegionalManagerCompensation.Text = "";
            txbDivisionManagerCompensation.Text = "";
            txbBranchManagerCompensation.Text = "";

            BindLoanRepForNew(null);

            // set current userid to ViewState["SourceUID"], then copy Personalization info when save
            ViewState["SourceUID"] = UserId;
        }

        protected void lbtnLoanRepSelected_Click(object sender, EventArgs e)
        {
            BindLoanRepForNew(null);
        }

        protected void lbtnGroupSelected_Click(object sender, EventArgs e)
        {
            BindGroupForNew(null);
        }

        /// <summary>
        /// Bind Loan Rep Mapping gridview
        /// </summary>
        private void BindLoanRep()
        {
            DataSet dsLoanRep = UserLoanRepManager.GetList(string.Format("UserId='{0}'", UserId.GetValueOrDefault(0)));
            if (null != dsLoanRep && dsLoanRep.Tables.Count > 0)
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement element = xmlDoc.CreateElement("root");
                xmlDoc.AppendChild(element);
                XmlElement childElement = null;
                XmlAttribute attri = null;
                foreach (DataRow r in dsLoanRep.Tables[0].Rows)
                {
                    childElement = xmlDoc.CreateElement("LoanRep");
                    element.AppendChild(childElement);

                    attri = xmlDoc.CreateAttribute("NameId");
                    attri.Value = r["NameId"].ToString();
                    childElement.Attributes.Append(attri);

                    attri = xmlDoc.CreateAttribute("Name");
                    attri.Value = r["Name"].ToString();
                    childElement.Attributes.Append(attri);
                }
                this.hiLoanRep.Value = EncodeXml(xmlDoc.OuterXml);
            }
            this.hiCurrLoanRep.Value = string.Join(",", dsLoanRep.Tables[0].Select().Select(i => i[0].ToString()).ToArray());
            this.gridLoanRepMapping.EmptyDataText = "There is no record.";
            this.gridLoanRepMapping.DataSource = dsLoanRep;
            this.gridLoanRepMapping.DataBind();
        }

        /// <summary>
        /// Bind Loan Rep Mapping when new
        /// </summary>
        /// <param name="strIDToDelete"></param>
        private void BindLoanRepForNew(string strIDToDelete)
        {
            // Bind gridview when LoanRep selected
            XmlDocument xmlDoc = new XmlDocument();
            if (string.IsNullOrEmpty(this.hiLoanRep.Value))
            {
                xmlDoc.LoadXml("<root></root>");
            }
            else
                xmlDoc.LoadXml(DecodeXml(this.hiLoanRep.Value));
            this.hiLoanRep.Value = EncodeXml(xmlDoc.OuterXml);
            //this.hiLoanRep.Value = "";
            StringBuilder sbCurrIDs = new StringBuilder();
            foreach (XmlNode node in xmlDoc.SelectNodes("//LoanRep"))
            {
                if (sbCurrIDs.Length > 0)
                    sbCurrIDs.Append(",");
                sbCurrIDs.Append(node.Attributes["NameId"].Value);
            }
            this.hiCurrLoanRep.Value = sbCurrIDs.ToString();

            this.xdsLoanRep.Data = xmlDoc.OuterXml;
            this.gridLoanRepMapping.DataSourceID = this.xdsLoanRep.ID;
            this.gridLoanRepMapping.DataBind();
        }

        /// <summary>
        /// Bind User Group gridview
        /// </summary>
        private void BindGroup()
        {
            DataSet dsGroup = GroupUserManager.GetGroupUsersForUserSetup(string.Format("UserID='{0}'", UserId.GetValueOrDefault(0)));
            if (null != dsGroup && dsGroup.Tables.Count > 0)
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement element = xmlDoc.CreateElement("root");
                xmlDoc.AppendChild(element);
                XmlElement childElement = null;
                XmlAttribute attri = null;
                foreach (DataRow r in dsGroup.Tables[0].Rows)
                {
                    childElement = xmlDoc.CreateElement("Group");
                    element.AppendChild(childElement);

                    attri = xmlDoc.CreateAttribute("GroupId");
                    attri.Value = r["GroupId"].ToString();
                    childElement.Attributes.Append(attri);

                    attri = xmlDoc.CreateAttribute("GroupName");
                    attri.Value = r["GroupName"].ToString();
                    childElement.Attributes.Append(attri);
                }
                this.hiGroup.Value = EncodeXml(xmlDoc.OuterXml);
            }
            this.hiCurrGroup.Value = string.Join(",", dsGroup.Tables[0].Select().Select(i => i[0].ToString()).ToArray());
            this.gvGroup.EmptyDataText = "There is no user record.";
            this.gvGroup.DataSource = dsGroup;
            this.gvGroup.DataBind();
        }

        /// <summary>
        /// Bind User Group gridview when new
        /// </summary>
        /// <param name="strIDToDelete"></param>
        private void BindGroupForNew(string strIDToDelete)
        {
            // Bind gridview when Group selected
            XmlDocument xmlDoc = new XmlDocument();
            if (string.IsNullOrEmpty(this.hiGroup.Value))
            {
                xmlDoc.LoadXml("<root></root>");
            }
            else
                xmlDoc.LoadXml(DecodeXml(this.hiGroup.Value));
            this.hiGroup.Value = EncodeXml(xmlDoc.OuterXml);
            //this.hiGroup.Value = "";
            StringBuilder sbCurrIDs = new StringBuilder();
            foreach (XmlNode node in xmlDoc.SelectNodes("//Group"))
            {
                if (sbCurrIDs.Length > 0)
                    sbCurrIDs.Append(",");
                sbCurrIDs.Append(node.Attributes["GroupId"].Value);
            }
            this.hiCurrGroup.Value = sbCurrIDs.ToString();
            this.xdsGroup.Data = xmlDoc.OuterXml;
            this.gvGroup.DataSourceID = this.xdsGroup.ID;
            this.gvGroup.DataBind();
        }

        protected void lbtnRemoveLoanRep_Click(object sender, EventArgs e)
        {
            // remove select row from hiLoanRep and rebind the grid
            XmlDocument xmlDoc = new XmlDocument();
            if (string.IsNullOrEmpty(this.hiLoanRep.Value))
            {
                xmlDoc.LoadXml("<root></root>");
            }
            else
                xmlDoc.LoadXml(DecodeXml(this.hiLoanRep.Value));

            foreach (GridViewRow row in gridLoanRepMapping.Rows)
            {
                CheckBox ckbItem = row.FindControl("ckbItem") as CheckBox;
                if (ckbItem.Enabled && ckbItem.Checked)
                {
                    string strID = gridLoanRepMapping.DataKeys[row.RowIndex].Value.ToString();
                    XmlNode nodeForDel = xmlDoc.SelectSingleNode(string.Format("//LoanRep[@NameId='{0}']", strID));
                    if (null != nodeForDel)
                        xmlDoc.DocumentElement.RemoveChild(nodeForDel);
                }
            }
            this.hiLoanRep.Value = EncodeXml(xmlDoc.OuterXml);
            BindLoanRepForNew(null);
        }

        protected void lbtnRemoveGroup_Click(object sender, EventArgs e)
        {
            // remove select row from hiGroup and rebind the grid
            XmlDocument xmlDoc = new XmlDocument();
            if (string.IsNullOrEmpty(this.hiGroup.Value))
            {
                xmlDoc.LoadXml("<root></root>");
            }
            else
                xmlDoc.LoadXml(DecodeXml(this.hiGroup.Value));

            foreach (GridViewRow row in gvGroup.Rows)
            {
                CheckBox ckbItem = row.FindControl("ckbItem") as CheckBox;
                if (ckbItem.Enabled && ckbItem.Checked)
                {
                    string strID = gvGroup.DataKeys[row.RowIndex].Value.ToString();
                    XmlNode nodeForDel = xmlDoc.SelectSingleNode(string.Format("//Group[@GroupId='{0}']", strID));
                    if (null != nodeForDel)
                        xmlDoc.DocumentElement.RemoveChild(nodeForDel);
                }
            }
            this.hiGroup.Value = EncodeXml(xmlDoc.OuterXml);
            BindGroupForNew(null);
        }

        /// <summary>
        /// use unicode "0001" to encode xml
        /// </summary>
        /// <param name="strXml"></param>
        /// <returns></returns>
        private string EncodeXml(string strXml)
        {
            return strXml.Replace('<', '\u0001');
        }

        /// <summary>
        /// decode xml which encoded by function EncodeXml
        /// </summary>
        /// <param name="strEncoded"></param>
        /// <returns></returns>
        private string DecodeXml(string strEncoded)
        {
            return strEncoded.Replace('\u0001', '<');
        }
    }
}