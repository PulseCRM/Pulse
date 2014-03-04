<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Email Template List" Language="C#" AutoEventWireup="true" CodeBehind="GeneralInfoTab.aspx.cs" Inherits="GeneralInfoTab" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Create a Lead</title>
   <%--  <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>--%>

        <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script> 
    <script src="../js/jquery.tab.js" type="text/javascript"></script> 
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
     <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
      <script language="javascript" type="text/javascript">


          $(document).ready(function () {

              // init tab
              $("#tabs").tabs();

              AddValidators();

              //#region Borrower

              $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtOtherMonthlyIncome").numeric({ allow: "," });
              $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtOtherMonthlyIncome").blur(function () { FormatMoney(this); });

              $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtLiquidAsset").numeric({ allow: "." });
              $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtLiquidAsset").blur(function () { FormatMoney(this); });
             
              //#endregion

           

           

          //#endregion

        

    </script>
</head>
<body>
    <form id="form1" runat="server"> 
   
       <div id="TabBody">               
                <div id="divContainer">

                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <td colspan="2">
                            
                        </td>
                        <td colspan="2">
                            
                        </td>
                    </tr>
                    <tr>
                        <td>Other Monthly Income($):</td>
                        <td>
                            <asp:TextBox ID="txtOtherMonthlyIncome" runat="server" MaxLength="15" class="dollar-text"></asp:TextBox>
                        </td>
                        <td>Comments：</td>
                         <td>
                            <table cellpadding="0" cellspacing="0">
                           
                                <tr>
                                                                      
                                    <td>
                                        <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Height="90px" Width="450px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                     <tr>
                        <td>Liquid Asset($):</td>
                        <td>
                        <table cellpadding="0" cellspacing="0">
                                <tr>                                   
                                    <td>
                                         <asp:TextBox ID="txtLiquidAsset" runat="server" MaxLength="15" class="dollar-text"></asp:TextBox>                       
                                    </td>
                                </tr>
                            </table>
                         </td>
                        <td></td>
                         <td>
                         </td>
                    </tr>

                   

                  
                </table>



               <%--  <table style="margin-top: 10px;">
                <tr>
                    <td>Mailing Address：</td>
                    <td>
                         <asp:DropDownList ID="ddlMailingAddress" runat="server" Width="124px">                           
                            <asp:ListItem Value="Borrower">Borrower</asp:ListItem>
                            <asp:ListItem Value="Co-Borrower">Co-Borrower</asp:ListItem>
                            <asp:ListItem Value="Both" Selected="True">Both</asp:ListItem>
                           
                           
                        </asp:DropDownList>
                    </td>
                    <td style="width: 50px;"></td>
                    <td>Subject Property Address:</td>
                    <td>
                         </td>
                    
                </tr>

                <tr>
                    <td>Street Address 1：</td>
                    <td>
                         <asp:TextBox ID="txtStreetAddress1" runat="server" Text="" Width="150" MaxLength="20"></asp:TextBox>
                    
                    </td>
                    <td style="width: 50px;"></td>
                   <td>Street Address 1：</td>
                    <td>
                         <asp:TextBox ID="txtStreetAddress2" runat="server" Text="" Width="150" MaxLength="20"></asp:TextBox>
                    
                    </td>

                     <td style="width: 50px;"></td>
                      <td>Ranking:</td>
                    <td>
                        <asp:Label ID="lblRanking" runat="server" Text=""></asp:Label>
                    
                    </td>
                    
                </tr>

                 <tr>
                    <td>Street Address 2：</td>
                    <td>
                         <asp:TextBox ID="TextBox3" runat="server" Text="" Width="150" MaxLength="20"></asp:TextBox>
                    
                    </td>
                    <td style="width: 50px;"></td>
                   <td>Street Address 2：</td>
                    <td>
                         <asp:TextBox ID="TextBox4" runat="server" Text="" Width="150" MaxLength="20"></asp:TextBox>
                    
                    </td>
                      <td style="width: 50px;"></td>
                      <td>Status:</td>
                    <td>
                          <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
                    
                    </td>
                    
                </tr>

                 <tr>
                    <td>City：</td>
                   
                    <td>
                          <asp:TextBox ID="TextBox6" runat="server" Text="" Width="150" MaxLength="20"></asp:TextBox>
                    
                    </td>
                       
                    <td style="width: 50px;"></td>
                   <td>City：</td>
                   
                    <td>
                          <asp:TextBox ID="TextBox7" runat="server" Text="" Width="150" MaxLength="20"></asp:TextBox>
                    
                    </td>

                    <td style="width: 50px;"></td>
                   <td></td>
                   
                    <td>
                          
                    </td>

                    
                </tr>

                 <tr>
                    <td>State：</td>
                    <td>
                         <asp:DropDownList ID="ddlState" runat="server" Width="50">                           
                            <asp:ListItem Text="AL" Value="AL"></asp:ListItem>
                            <asp:ListItem Text="AK" Value="AK"></asp:ListItem>
                            <asp:ListItem Text="AZ" Value="AZ"></asp:ListItem>
                            <asp:ListItem Text="AR" Value="AR"></asp:ListItem>
                            <asp:ListItem Text="CA" Value="CA"></asp:ListItem>
                            <asp:ListItem Text="CO" Value="CO"></asp:ListItem>
                            <asp:ListItem Text="CT" Value="CT"></asp:ListItem>
                            <asp:ListItem Text="DC" Value="DC"></asp:ListItem>
                            <asp:ListItem Text="DE" Value="DE"></asp:ListItem>
                            <asp:ListItem Text="FL" Value="FL"></asp:ListItem>
                            <asp:ListItem Text="GA" Value="GA"></asp:ListItem>
                            <asp:ListItem Text="HI" Value="HI"></asp:ListItem>
                            <asp:ListItem Text="ID" Value="ID"></asp:ListItem>
                            <asp:ListItem Text="IL" Value="IL"></asp:ListItem>
                            <asp:ListItem Text="IN" Value="IN"></asp:ListItem>
                            <asp:ListItem Text="IA" Value="IA"></asp:ListItem>
                            <asp:ListItem Text="KS" Value="KS"></asp:ListItem>
                            <asp:ListItem Text="KY" Value="KY"></asp:ListItem>
                            <asp:ListItem Text="LA" Value="LA"></asp:ListItem>
                            <asp:ListItem Text="ME" Value="ME"></asp:ListItem>
                            <asp:ListItem Text="MD" Value="MD"></asp:ListItem>
                            <asp:ListItem Text="MA" Value="MA"></asp:ListItem>
                            <asp:ListItem Text="MI" Value="MI"></asp:ListItem>
                            <asp:ListItem Text="MN" Value="MN"></asp:ListItem>
                            <asp:ListItem Text="MS" Value="MS"></asp:ListItem>
                            <asp:ListItem Text="MO" Value="MO"></asp:ListItem>
                            <asp:ListItem Text="MT" Value="MT"></asp:ListItem>
                            <asp:ListItem Text="NE" Value="NE"></asp:ListItem>
                            <asp:ListItem Text="NV" Value="NV"></asp:ListItem>
                            <asp:ListItem Text="NH" Value="NH"></asp:ListItem>
                            <asp:ListItem Text="NJ" Value="NJ"></asp:ListItem>
                            <asp:ListItem Text="NM" Value="NM"></asp:ListItem>
                            <asp:ListItem Text="NY" Value="NY"></asp:ListItem>
                            <asp:ListItem Text="NC" Value="NC"></asp:ListItem>
                            <asp:ListItem Text="ND" Value="ND"></asp:ListItem>
                            <asp:ListItem Text="OH" Value="OH"></asp:ListItem>
                            <asp:ListItem Text="OK" Value="OK"></asp:ListItem>
                            <asp:ListItem Text="OR" Value="OR"></asp:ListItem>
                            <asp:ListItem Text="PA" Value="PA"></asp:ListItem>
                            <asp:ListItem Text="PR" Value="PR"></asp:ListItem>
                            <asp:ListItem Text="RI" Value="RI"></asp:ListItem>
                            <asp:ListItem Text="SC" Value="SC"></asp:ListItem>
                            <asp:ListItem Text="SD" Value="SD"></asp:ListItem>
                            <asp:ListItem Text="TN" Value="TN"></asp:ListItem>
                            <asp:ListItem Text="TX" Value="TX"></asp:ListItem>
                            <asp:ListItem Text="UT" Value="UT"></asp:ListItem>
                            <asp:ListItem Text="VT" Value="VT"></asp:ListItem>
                            <asp:ListItem Text="VA" Value="VA"></asp:ListItem>
                            <asp:ListItem Text="WA" Value="WA"></asp:ListItem>
                            <asp:ListItem Text="WV" Value="WV"></asp:ListItem>
                            <asp:ListItem Text="WI" Value="WI"></asp:ListItem>
                            <asp:ListItem Text="WY" Value="WY"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 50px;"></td>
                   <td>State：</td>
                    <td>
                         <asp:DropDownList ID="ddlStateNew" runat="server" Width="124px">
                            <asp:ListItem Text="AL" Value="AL"></asp:ListItem>
                            <asp:ListItem Text="AK" Value="AK"></asp:ListItem>
                            <asp:ListItem Text="AZ" Value="AZ"></asp:ListItem>
                            <asp:ListItem Text="AR" Value="AR"></asp:ListItem>
                            <asp:ListItem Text="CA" Value="CA"></asp:ListItem>
                            <asp:ListItem Text="CO" Value="CO"></asp:ListItem>
                            <asp:ListItem Text="CT" Value="CT"></asp:ListItem>
                            <asp:ListItem Text="DC" Value="DC"></asp:ListItem>
                            <asp:ListItem Text="DE" Value="DE"></asp:ListItem>
                            <asp:ListItem Text="FL" Value="FL"></asp:ListItem>
                            <asp:ListItem Text="GA" Value="GA"></asp:ListItem>
                            <asp:ListItem Text="HI" Value="HI"></asp:ListItem>
                            <asp:ListItem Text="ID" Value="ID"></asp:ListItem>
                            <asp:ListItem Text="IL" Value="IL"></asp:ListItem>
                            <asp:ListItem Text="IN" Value="IN"></asp:ListItem>
                            <asp:ListItem Text="IA" Value="IA"></asp:ListItem>
                            <asp:ListItem Text="KS" Value="KS"></asp:ListItem>
                            <asp:ListItem Text="KY" Value="KY"></asp:ListItem>
                            <asp:ListItem Text="LA" Value="LA"></asp:ListItem>
                            <asp:ListItem Text="ME" Value="ME"></asp:ListItem>
                            <asp:ListItem Text="MD" Value="MD"></asp:ListItem>
                            <asp:ListItem Text="MA" Value="MA"></asp:ListItem>
                            <asp:ListItem Text="MI" Value="MI"></asp:ListItem>
                            <asp:ListItem Text="MN" Value="MN"></asp:ListItem>
                            <asp:ListItem Text="MS" Value="MS"></asp:ListItem>
                            <asp:ListItem Text="MO" Value="MO"></asp:ListItem>
                            <asp:ListItem Text="MT" Value="MT"></asp:ListItem>
                            <asp:ListItem Text="NE" Value="NE"></asp:ListItem>
                            <asp:ListItem Text="NV" Value="NV"></asp:ListItem>
                            <asp:ListItem Text="NH" Value="NH"></asp:ListItem>
                            <asp:ListItem Text="NJ" Value="NJ"></asp:ListItem>
                            <asp:ListItem Text="NM" Value="NM"></asp:ListItem>
                            <asp:ListItem Text="NY" Value="NY"></asp:ListItem>
                            <asp:ListItem Text="NC" Value="NC"></asp:ListItem>
                            <asp:ListItem Text="ND" Value="ND"></asp:ListItem>
                            <asp:ListItem Text="OH" Value="OH"></asp:ListItem>
                            <asp:ListItem Text="OK" Value="OK"></asp:ListItem>
                            <asp:ListItem Text="OR" Value="OR"></asp:ListItem>
                            <asp:ListItem Text="PA" Value="PA"></asp:ListItem>
                            <asp:ListItem Text="PR" Value="PR"></asp:ListItem>
                            <asp:ListItem Text="RI" Value="RI"></asp:ListItem>
                            <asp:ListItem Text="SC" Value="SC"></asp:ListItem>
                            <asp:ListItem Text="SD" Value="SD"></asp:ListItem>
                            <asp:ListItem Text="TN" Value="TN"></asp:ListItem>
                            <asp:ListItem Text="TX" Value="TX"></asp:ListItem>
                            <asp:ListItem Text="UT" Value="UT"></asp:ListItem>
                            <asp:ListItem Text="VT" Value="VT"></asp:ListItem>
                            <asp:ListItem Text="VA" Value="VA"></asp:ListItem>
                            <asp:ListItem Text="WA" Value="WA"></asp:ListItem>
                            <asp:ListItem Text="WV" Value="WV"></asp:ListItem>
                            <asp:ListItem Text="WI" Value="WI"></asp:ListItem>
                            <asp:ListItem Text="WY" Value="WY"></asp:ListItem>
                        </asp:DropDownList>
                    </td>

                      <td style="width: 50px;"></td>
                   <td></td>
                   
                    <td>
                          
                    </td>
                    
                </tr>

                 <tr>
                    <td>Zip：</td>
                    <td>
                         <asp:TextBox ID="txtZip1" runat="server" Text="" Width="150" MaxLength="20"></asp:TextBox>
                    </td>
                    <td style="width: 50px;"></td>
                   
                   <td>Zip：</td>
                    <td>
                         <asp:TextBox ID="txtZip2" runat="server" Text="" Width="150" MaxLength="20"></asp:TextBox>
                    </td>
                       <td style="width: 50px;"></td>
                   <td></td>
                   
                    <td>
                          
                    </td>
                </tr>

                 <tr>
                    <td>Lead Source：</td>
                    <td>
                         <asp:DropDownList ID="ddlLeadSource" runat="server" Width="124px">
                           
                        </asp:DropDownList>
                    </td>
                    <td style="width: 50px;"></td>
                   
                    <td>Property Value：</td>
                    <td>
                         <asp:TextBox ID="txtRateNew" runat="server" Text="" Width="150" MaxLength="20"></asp:TextBox>
                    </td>
                        <td style="width: 50px;"></td>
                   <td></td>
                   
                    <td>
                          
                    </td>
                </tr>

                 <tr>
                    <td>Referral Source：</td>
                    <td>
                         <asp:TextBox ID="txtMonthlyPmi" runat="server" Text="" Width="150" MaxLength="20"></asp:TextBox>
                    </td>
                    <td style="width: 50px;"></td>
                   
                    <td></td>
                    <td>
                        
                           </td>
                             <td style="width: 50px;"></td>
                   <td></td>
                   
                    <td>
                          
                    </td>
                </tr>

                
                 

               


                 <tr>
                            <td height="20" colspan="2" style="padding-top: 9px; padding-bottom: 3px;">
                                <asp:Button ID="btnAdd" runat="server" Text="Save and Go to Lead Detail" CssClass="Btn-140" OnClick="btnAdd_Click"  OnClientClick="return BeforeSave()"/>
                            </td>

                             <td height="20" colspan="2" style="padding-top: 9px; padding-bottom: 3px;">
                                <asp:Button ID="btnCA" runat="server" Text="Save and Create Another" CssClass="Btn-140" OnClick="btnCA_Click"  OnClientClick="return BeforeSave()"/>
                            </td>
                        </tr>
            </table>--%>


                </div>
            </div>
   
    </form>
</body>
</html>