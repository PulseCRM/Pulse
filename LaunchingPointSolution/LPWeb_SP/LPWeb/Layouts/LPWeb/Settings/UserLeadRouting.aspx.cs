using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using LPWeb.Common;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.Model;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Utilities;
using System.Data;
using System.Collections.Generic;
using Company_Lead_Sources = LPWeb.BLL.Company_Lead_Sources;
using User2LeadSource = LPWeb.BLL.User2LeadSource;
using User2LoanType = LPWeb.BLL.User2LoanType;
using User2Purpose = LPWeb.BLL.User2Purpose;
using User2State = LPWeb.BLL.User2State;

namespace LPWeb.Layouts.LPWeb.Settings
{
    public partial class UserLeadRouting : BasePage
    {
        private BLL.Users UsersManager = new BLL.Users();
        private BLL.UserLeadDist userLeadDist = new BLL.UserLeadDist();
        private BLL.Company_Lead_Sources leadSources = new Company_Lead_Sources();
        private BLL.User2LeadSource user2LeadSource = new User2LeadSource();
        private BLL.User2State user2State = new User2State();
        private BLL.User2Purpose user2Purpose = new User2Purpose();
        private BLL.User2LoanType user2LoanType = new User2LoanType();

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
            set { ViewState["uid"] = value; }
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

                // get UserId from query string
                int nUserId = -1;
                if (!int.TryParse(Request.QueryString["uid"], out nUserId))
                    nUserId = -1;
                if (-1 == nUserId)
                    UserId = null;
                else
                    UserId = nUserId;

                BindInfo();
            }
        }

        #region <Bind the database data to screen>

        private void BindInfo()
        {
            try
            {
                if (UserId.HasValue)
                {
                    //绑定用户信息
                    BindUserInfo();

                    BindUserLeadDist();

                    BindLeadSource();

                    BindState();

                    BindPurpose();

                    BindLoanType();
                }
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }
        }

        private void BindUserInfo()
        {
            if (UserId.HasValue)
            {
                Model.Users user = UsersManager.GetModel(UserId.Value);
                if (null == user)
                {
                    LPLog.LogMessage(LogType.Logerror,
                                     string.Format("Cannot find the user with UserId = {0}", UserId.Value));
                }
                else
                {
                    tbUserName.Text = user.Username;
                    tbFirstName.Text = user.FirstName;
                    tbLastName.Text = user.LastName;
                }
            }
        }

        private void BindUserLeadDist()
        {
            if (UserId.HasValue)
            {
                Model.UserLeadDist modelUserLeadDist = userLeadDist.GetModel(UserId.Value);
                if (modelUserLeadDist != null)
                {
                    bool enableLeadRouting = modelUserLeadDist.EnableLeadRouting;
                    cbxAcceptRouteLeads.Checked = enableLeadRouting;
                    tbMaxDay.Text = modelUserLeadDist.MaxDailyLeads.ToString();
                    if (enableLeadRouting)
                    {
                        EnableLeadRouting(true);
                    }
                    else
                    {
                        EnableLeadRouting(false);
                        tbMaxDay.Text = "0";
                    }
                }
                else
                {
                    cbxAcceptRouteLeads.Checked = false;
                    tbMaxDay.Text = "0";
                    EnableLeadRouting(false);
                }
            }
        }

        private void EnableLeadRouting(bool enable)
        {
            tbMaxDay.Enabled = enable;
            DisableCheckBox(cblLeadSources,enable);
            DisableCheckBox(cblLicensedStates, enable);
            DisableCheckBox(cblPurposes, enable);
            DisableCheckBox(cblTypes, enable);
        }
    

        private void DisableCheckBox(CheckBoxList control, bool enable)
        {
            foreach (ListItem item in cblLeadSources.Items)
            {
                item.Enabled = enable;
            }
        }

/// <summary>
        /// Bind Lead Source 
        /// </summary>
        private void BindLeadSource()
        {
            cblLeadSources.Items.Clear();

            DataSet dsLeadSources = leadSources.GetList(string.Empty);
            cblLeadSources.DataTextField = "LeadSource";
            cblLeadSources.DataValueField = "LeadSourceID";
            cblLeadSources.DataSource = dsLeadSources;
            cblLeadSources.DataBind();

            cblLeadSources.Items.Insert(0,AddListItem("All", "-1"));
           
            if (UserId.HasValue)
            {
                List<Model.User2LeadSource> userLeadSourceList = user2LeadSource.GetModelList(" UserID = " + UserId.Value);
                int itemCount = cblLeadSources.Items.Count;
                for (var i = 0; i < itemCount; i++)
                {
                    if (userLeadSourceList.Count > 0)
                    {
                        if (userLeadSourceList[0].LeadSourceID == -1)
                        {
                            cblLeadSources.Items[i].Selected = true;
                        }
                        else
                        {
                            foreach (var leadSource in userLeadSourceList)
                            {
                                if (int.Parse(cblLeadSources.Items[i].Value) == leadSource.LeadSourceID)
                                {
                                    cblLeadSources.Items[i].Selected = true;
                                }
                            }
                        }
                    }  
                }   
            }

        }

        private void BindState()
        {
            cblLicensedStates.Items.Clear();

            List<ListItem> listItems = USStates.GetStates();
            cblLicensedStates.Items.AddRange(listItems.ToArray());

            if (UserId.HasValue)
            {
                List<Model.User2State> user2StateList = user2State.GetModelList(" UserID = " + UserId.Value);
                int itemCount = cblLicensedStates.Items.Count;
                for (var i = 0; i < itemCount; i++)
                {
                    if (user2StateList.Count > 0)
                    {
                        if (user2StateList[0].State == "-1")
                        {
                            cblLicensedStates.Items[i].Selected = true;
                        }
                        else
                        {
                            foreach (var state in user2StateList)
                            {
                                if (cblLicensedStates.Items[i].Value == state.State)
                                {
                                    cblLicensedStates.Items[i].Selected = true;
                                }
                            }
                        }
                    }
                }  
            }
        }

        private void BindPurpose()
        {
            cblPurposes.Items.Clear();
            cblPurposes.Items.Add(AddListItem("All", "-1"));
            cblPurposes.Items.Add(AddListItem("Cash-Out Refinance", "Cash-Out Refinance"));
            cblPurposes.Items.Add(AddListItem("Construction", "Construction"));
            cblPurposes.Items.Add(AddListItem("Construction-Perm", "Construction-Perm"));
            cblPurposes.Items.Add(AddListItem("Other", "Other"));
            cblPurposes.Items.Add(AddListItem("Purchase", "Purchase"));
            cblPurposes.Items.Add(AddListItem("No Cash-Out Refinance", "No Cash-Out Refinance"));
                  
            if (UserId.HasValue)
            {
                List<Model.User2Purpose> user2PurposeList = user2Purpose.GetModelList(" UserID = " + UserId.Value);
                int itemCount = cblPurposes.Items.Count;
                for (var i = 0; i < itemCount; i++)
                {
                    if (user2PurposeList.Count > 0)
                    {
                        if (user2PurposeList[0].Purpose == "-1")
                        {
                            cblPurposes.Items[i].Selected = true;
                        }
                        else
                        {
                            foreach (var purpose in user2PurposeList)
                            {
                                if (cblPurposes.Items[i].Value == purpose.Purpose)
                                {
                                    cblPurposes.Items[i].Selected = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BindLoanType()
        {
            cblTypes.Items.Clear();
            cblTypes.Items.Add(AddListItem("All", "-1"));
            cblTypes.Items.Add(AddListItem("Conv", "Conv"));
            cblTypes.Items.Add(AddListItem("FHA", "FHA"));
            cblTypes.Items.Add(AddListItem("Other", "Other"));
            cblTypes.Items.Add(AddListItem("USDA/RH", "USDA/RH"));
            cblTypes.Items.Add(AddListItem("VA", "VA"));
           
            if (UserId.HasValue)
            {
                List<Model.User2LoanType> user2LoanTypeList = user2LoanType.GetModelList(" UserID = " + UserId.Value);
                int itemCount = cblTypes.Items.Count;
                for (var i = 0; i < itemCount; i++)
                {
                    if (user2LoanTypeList.Count > 0)
                    {
                        if (user2LoanTypeList[0].LoanType == "-1")
                        {
                            cblTypes.Items[i].Selected = true;
                        }
                        else
                        {
                            foreach (var type in user2LoanTypeList)
                            {
                                if (cblTypes.Items[i].Value == type.LoanType)
                                {
                                    cblTypes.Items[i].Selected = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private ListItem AddListItem(string text, string value)
        {
            ListItem item = new ListItem();
            item.Text = text;
            item.Value = value;
            return item;
        }
        #endregion


        #region <Save Function>
        private void SaveLeadDist()
        {
            if (UserId.HasValue)
            {
                Model.UserLeadDist userLeadDistModel = userLeadDist.GetModel(UserId.Value);
                int maxDays = 0;
                int.TryParse(tbMaxDay.Text, out maxDays);
                if (userLeadDistModel == null)
                {
                    userLeadDistModel = new UserLeadDist();
                    userLeadDistModel.UserID = UserId.Value;
                    userLeadDistModel.EnableLeadRouting = cbxAcceptRouteLeads.Checked;
                    userLeadDistModel.MaxDailyLeads = maxDays;
                    userLeadDist.Add(userLeadDistModel);
                }
                else
                {
                    userLeadDistModel.EnableLeadRouting = cbxAcceptRouteLeads.Checked;
                    userLeadDistModel.MaxDailyLeads = maxDays;
                    userLeadDist.Update(userLeadDistModel);
                }
            }
        }

        private void SaveLeadSource()
        {
             if (UserId.HasValue)
             {
                user2LeadSource.DeleteByUserID(UserId.Value);
                Model.User2LeadSource user2LeadSourceModel = null;
                if (IsSelectAll(cblLeadSources))
                {
                        user2LeadSourceModel = new Model.User2LeadSource();
                        user2LeadSourceModel.UserID = UserId.Value;
                        user2LeadSourceModel.LeadSourceID = -1;
                        user2LeadSource.Add(user2LeadSourceModel);
                }
                else
                {
                    foreach (ListItem item in cblLeadSources.Items)
                    {
                        if (item.Selected)
                        {
                            user2LeadSourceModel = new Model.User2LeadSource();
                            user2LeadSourceModel.UserID = UserId.Value;
                            user2LeadSourceModel.LeadSourceID = int.Parse(item.Value);
                            user2LeadSource.Add(user2LeadSourceModel);
                        }
                    }
                }
            }
        }

        private void SaveState()
        {
            if (UserId.HasValue)
            {
                user2State.DeleteByUserID(UserId.Value);
                Model.User2State user2StateModel = null;
                if (IsSelectAll(cblLicensedStates))
                {
                    user2StateModel = new Model.User2State();
                    user2StateModel.UserID = UserId.Value;
                    user2StateModel.State = "-1";
                    user2State.Add(user2StateModel);
                }
                else
                {
                    foreach (ListItem item in cblLicensedStates.Items)
                    {
                        if (item.Selected)
                        {
                            user2StateModel = new Model.User2State();
                            user2StateModel.UserID = UserId.Value;
                            user2StateModel.State = item.Value;
                            user2State.Add(user2StateModel);
                        }
                    }
                }
            }
        }

        private void SavePurpose()
        {
            if (UserId.HasValue)
            {
                user2Purpose.DeleteByUserID(UserId.Value);
                Model.User2Purpose user2PurposeModel = null;
                if (IsSelectAll(cblPurposes))
                {
                    user2PurposeModel = new Model.User2Purpose();
                    user2PurposeModel.UserID = UserId.Value;
                    user2PurposeModel.Purpose = "-1";
                    user2Purpose.Add(user2PurposeModel);
                }
                else
                {
                    foreach (ListItem item in cblPurposes.Items)
                    {
                        if (item.Selected)
                        {
                            user2PurposeModel = new Model.User2Purpose();
                            user2PurposeModel.UserID = UserId.Value;
                            user2PurposeModel.Purpose = item.Value;
                            user2Purpose.Add(user2PurposeModel);
                        }
                    }
                }
            }
        }

        private void SaveLoanType()
        {
            if (UserId.HasValue)
            {
                user2LoanType.DeleteByUser(UserId.Value);
                Model.User2LoanType user2LoanTypeModel = null;
                if (IsSelectAll(cblTypes))
                {
                    user2LoanTypeModel = new Model.User2LoanType();
                    user2LoanTypeModel.UserID = UserId.Value;
                    user2LoanTypeModel.LoanType = "-1";
                    user2LoanType.Add(user2LoanTypeModel);
                }
                else
                {
                    foreach (ListItem item in cblTypes.Items)
                    {
                        if (item.Selected)
                        {
                            user2LoanTypeModel = new Model.User2LoanType();
                            user2LoanTypeModel.UserID = UserId.Value;
                            user2LoanTypeModel.LoanType = item.Value;
                            user2LoanType.Add(user2LoanTypeModel);
                        }
                    }
                }
            }
        }

        private bool IsSelectAll(CheckBoxList cblControl)
        {
            bool isSelected = cblControl.Items[0].Selected;
            return isSelected;
        }
        #endregion

        #region <Delete Function>

        public void Delete()
        {
            if (UserId.HasValue)
            {
                try
                {
                    userLeadDist.Delete(UserId.Value);
                    user2LeadSource.DeleteByUserID(UserId.Value);
                    user2State.DeleteByUserID(UserId.Value);
                    user2Purpose.DeleteByUserID(UserId.Value);
                    user2LoanType.DeleteByUser(UserId.Value);
                }
                catch (Exception exception)
                {
                    LPLog.LogMessage(exception.Message);
                }
            }
        }

        #endregion

        #region <Event>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveLeadDist();
                SaveLeadSource();
                SaveState();
                SavePurpose();
                SaveLoanType();
                string sMsg = "Save successfully.";
                this.Response.Write("<script type='text/javascript' language='javascript'>alert(\"" + sMsg + "\");</script>");
                BindInfo();
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Delete();
                string sMsg = "Delete successfully.";
                this.Response.Write("<script type='text/javascript' language='javascript'>alert(\"" + sMsg + "\");</script>");
                BindInfo();
                //// success
                //PageCommon.WriteJsEnd(this, "Delete successfully.", "window.location.href='UserLeadRouting.aspx?uid=" + UserId.Value + "&t=" + Random + "'");
            }
            catch (Exception exception)
            {
                LPLog.LogMessage(exception.Message);
            }
        }
        #endregion

    }
}
