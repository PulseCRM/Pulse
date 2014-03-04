<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="LoanInfoTab" Language="C#" AutoEventWireup="true" CodeBehind="GeneralInfoTab.aspx.cs" Inherits="GeneralInfoTab" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>LoanInfoTab</title>
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

             $(".DateField").datepick();
          
         });


         function BeforeSave() {
             var Zip1 = $("#<%=txtZip1.ClientID %>").val();
             if (Zip1 == null && typeof (Zip1) == "undefined" && Zip1 == "") {

                          }
                          else {
                              var Pattern = /^[0-9]*[1-9][0-9]*$/;
                              var IsValid = Pattern.test(Zip1);
                              if (IsValid == false) {
                                  alert("The data you entered Term invalid!");
                                  return false;
                              }
                          }

             ShowWaitingDialog("please waiting...");

         }

         function ShowWaitingDialog(WaitingMsg) {
             //alert(WaitingMsg);

             $("#WaitingMsg").text(WaitingMsg);
             $.blockUI({ message: $('#divWaiting'), css: { width: '150px'} });
         }
      
     </script>
</head>
<body>
    <form id="form1" runat="server"> 
   
        <div id="TabBody">               
                <div id="divContainer">
                 <table style="margin-top: 10px;">
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
                      <td>Ranking:Hot</td>
                    <td>
                         <asp:TextBox ID="TextBox5" runat="server" Text="" Width="150" MaxLength="20"></asp:TextBox>
                    
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
                      <td>Status:Active</td>
                    <td>
                         
                    
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
                         <asp:DropDownList ID="ddlLoanProgram" runat="server" Width="124px">
                           
                        </asp:DropDownList>
                    </td>
                    <td style="width: 50px;"></td>
                   <td>State：</td>
                    <td>
                         <asp:DropDownList ID="DropDownList1" runat="server" Width="124px">
                           
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
                         <asp:TextBox ID="txtRate" runat="server" Text="" Width="150" MaxLength="20"></asp:TextBox>
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
            </table>


                </div>
            </div>
   
    </form>
</body>
</html>