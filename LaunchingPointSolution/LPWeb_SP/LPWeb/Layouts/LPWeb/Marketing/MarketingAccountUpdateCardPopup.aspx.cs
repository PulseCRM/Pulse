using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Workflow;
using System.Linq;
using LPWeb.Common;
using LPWeb.LP_Service;
using LPWeb.Layouts.LPWeb.Common;
using LPWeb.LP_Service;

namespace LPWeb.Layouts.LPWeb.Marketing
{
    public partial class UpdateCardPopup : LayoutsPageBase
    {
        public LoginUser loginUser = new LoginUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadInfo();
            }
        }
        /// <summary>
        /// 加载card信息
        /// </summary>
        public void LoadInfo()
        {
            try
            {
                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient client = sm.StartServiceClient())
                {


                    var creditCardInfo = new LP_Service.GetCreditCardRequest();
                    creditCardInfo.hdr = new LP_Service.ReqHdr();
                    creditCardInfo.hdr.UserId = loginUser.iUserID;

                    var rsp = client.GetCreditCard(ref creditCardInfo);


                    if (rsp.hdr.Successful)
                    {
                        hfCardID.Value = creditCardInfo.Card_ID;

                        txbCardNumber.Text = creditCardInfo.Card_Number;
                        txbNameonCard.Text = creditCardInfo.Card_First_Name + " " + creditCardInfo.Card_Last_Name;
                        txbCardExpiration.Text = creditCardInfo.Card_Exp_Month.ToString() + "/" + creditCardInfo.Card_Exp_Year;

                        if (creditCardInfo.Card_SIC != "***")
                        {
                            txbCardCSCcode.Text = creditCardInfo.Card_SIC;
                        }
                        else
                        {
                            txbCardCSCcode.Text = "";
                        }
                        //ddlCardType.SelectedValue = creditCardInfo.Card_Type.ToString();

                        switch (creditCardInfo.Card_Type)
                        {
                            case CreditCardType.VISA:
                                ddlCardType.SelectedValue = "0";
                                break;
                            case CreditCardType.MasterCard:
                                ddlCardType.SelectedValue = "1";
                                break;
                            case CreditCardType.Amex:
                                ddlCardType.SelectedValue = "2";
                                break;
                            case CreditCardType.Discover:
                                ddlCardType.SelectedValue = "3";
                                break;
                            default: ddlCardType.SelectedValue = "";
                                break;
                        }

                        btnAddBalance.Enabled = true;

                        var reqBalance = new LP_Service.GetUserAccountBalanceRequest();
                        reqBalance.hdr = new LP_Service.ReqHdr();
                        reqBalance.hdr.UserId = loginUser.iUserID;
                        var rspBalance = client.GetUserAccountBalance(reqBalance);

                        if (rspBalance.hdr.Successful)
                        {
                            txbBalance.Text = rspBalance.Balance.ToString("N2");
                        }

                    }
                    else
                    {
                        btnAddBalance.Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Load error:" + ex.Message);
            }


        }
        /// <summary>
        /// 更新 card信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdateCard_Click(object sender, EventArgs e)
        {

            #region 检查
            if (ddlCardType.SelectedValue == "")
            {
                PageCommon.RegisterJsMsg(this, "please select Card Type", "");
                return;
            }
            if (string.IsNullOrEmpty(txbCardNumber.Text))
            {
                PageCommon.RegisterJsMsg(this, "please input Card Number", "");
                return;
            }

            if (string.IsNullOrEmpty(txbCardExpiration.Text))
            {
                PageCommon.RegisterJsMsg(this, "please input Card Expiration", "");
                return;
            }

            if (txbCardExpiration.Text.Trim().IndexOf('/') == -1)
            {
                PageCommon.RegisterJsMsg(this, "the expiration in the format of mm/yy", "");
                return;
            }

            if (string.IsNullOrEmpty(txbCardCSCcode.Text))
            {
                PageCommon.RegisterJsMsg(this, "please input Card CSC code", "");
                return;
            }

            if (string.IsNullOrEmpty(txbNameonCard.Text))
            {
                PageCommon.RegisterJsMsg(this, "please input Name on Card", "");
                return;
            }
            #endregion

            try
            {

                var creditCard = new LP_Service.UpdateCreditCardRequest();

                var exp = txbCardExpiration.Text.Trim().Split('/');

                int mm = Convert.ToInt32(exp[0]);
                if (mm >= 1 && mm <= 12)
                {
                    creditCard.Card_Exp_Year = exp[0];
                }
                else
                {
                    PageCommon.RegisterJsMsg(this, "Card Expiration (mm/yy): mm should be a number between 1 and 12", "");
                    return;
                }
                if (Convert.ToInt32(DateTime.Now.Year.ToString().Substring(0, 2) + exp[1]) >= DateTime.Now.Year)
                {
                    creditCard.Card_Exp_Year = exp[1];
                }
                else
                {
                    PageCommon.RegisterJsMsg(this, "Card Expiration (mm/yy): yy Should be more than equal to the current year", "");
                    return;
                }

                var name = txbNameonCard.Text.Split(' ').ToList();
                if (name.Count < 2)
                {

                }
                creditCard.Card_First_Name = name.FirstOrDefault();

                creditCard.Card_Last_Name = name.LastOrDefault();

                //creditCard.Card_IsDefault

                creditCard.Card_Number = txbCardNumber.Text.Trim();
                creditCard.Card_SIC = txbCardCSCcode.Text.Trim();
                creditCard.Card_Type = (LP_Service.CreditCardType)Convert.ToInt32(ddlCardType.SelectedValue.Trim());

                //creditCard.ExtensionData =

                creditCard.Card_ID = hfCardID.Value.Trim();

                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient client = sm.StartServiceClient())
                {
                    creditCard.hdr = new LP_Service.ReqHdr();
                    creditCard.hdr.UserId = loginUser.iUserID;

                    var rq = client.UpdateCreditCard(creditCard);
                    if (rq.hdr.Successful)
                    {
                        PageCommon.RegisterJsMsg(this, "", PageCommon.Js_RefreshSelf);
                    }
                    else
                    {
                        PageCommon.RegisterJsMsg(this, rq.hdr.StatusInfo, "");
                    }
                }
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Error:" + ex.Message);
            }

        }

        protected void btnAddBalance_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(txbAddBalance.Text.Trim()))
                {
                    PageCommon.RegisterJsMsg(this, "please input Card Number", "");
                    return;
                }

                ServiceManager sm = new ServiceManager();
                using (LP2ServiceClient client = sm.StartServiceClient())
                {
                    var req = new LP_Service.AddToAccountRequest();
                    req.Amount = Convert.ToDecimal(txbAddBalance.Text.Trim());
                    req.hdr = new LP_Service.ReqHdr();
                    req.hdr.UserId = loginUser.iUserID;
                    var rsp = client.AddToAccount(req);
                }
            }
            catch (Exception ex)
            {
                PageCommon.AlertMsg(this, "Error:" + ex.Message);
            }

        }
    }
}
