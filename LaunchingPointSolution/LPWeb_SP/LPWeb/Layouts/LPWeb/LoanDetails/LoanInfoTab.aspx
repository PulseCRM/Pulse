<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="LoanInfoTab" Language="C#" AutoEventWireup="true" CodeBehind="LoanInfoTab.aspx.cs" Inherits="LoanInfoTab" %>

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

             $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtMonthlySalary").numeric({ allow: "," });
             $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtMonthlySalary").blur(function () { FormatMoney(this); });
          
         });


         function BeforeSave() {
            

         }

         
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
                        <td>Employer:</td>
                        <td>
                            <asp:TextBox ID="txtEmployer" runat="server" MaxLength="15" class="dollar-text"></asp:TextBox>
                        </td>
                        <td>Start Date：</td>
                        <td> <asp:DropDownList ID="ddlStartMonth" runat="server" Width="74px">
                                <asp:ListItem Value="">month</asp:ListItem>
                                <asp:ListItem Value="1">01</asp:ListItem>
                                <asp:ListItem Value="2">02</asp:ListItem>
                                <asp:ListItem Value="3">03</asp:ListItem>
                                <asp:ListItem Value="4">04</asp:ListItem>
                                <asp:ListItem Value="5">05</asp:ListItem>
                                <asp:ListItem Value="6">06</asp:ListItem>
                                <asp:ListItem Value="7">07</asp:ListItem>
                                <asp:ListItem Value="8">08</asp:ListItem>
                                <asp:ListItem Value="9">09</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>

                     <tr>
                        <td>Self Employed:</td>
                        <td>
                            <asp:DropDownList ID="ddlDependants" runat="server" Width="100px">
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem Selected>No</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td></td>
                        <td> <asp:DropDownList ID="ddlStartYear" runat="server" Width="74px">
                                
                            </asp:DropDownList></td>
                    </tr>

                      <tr>
                        <td>Title/Position:</td>
                        <td>
                            <asp:TextBox ID="txtTP" runat="server"></asp:TextBox>
                        </td>
                       
                        <td>End Date:</td>
                        <td> <asp:DropDownList ID="ddlEndDate" runat="server" Width="74px">
                                <asp:ListItem Value="">month</asp:ListItem>
                                <asp:ListItem Value="1">01</asp:ListItem>
                                <asp:ListItem Value="2">02</asp:ListItem>
                                <asp:ListItem Value="3">03</asp:ListItem>
                                <asp:ListItem Value="4">04</asp:ListItem>
                                <asp:ListItem Value="5">05</asp:ListItem>
                                <asp:ListItem Value="6">06</asp:ListItem>
                                <asp:ListItem Value="7">07</asp:ListItem>
                                <asp:ListItem Value="8">08</asp:ListItem>
                                <asp:ListItem Value="9">09</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>

                     <tr>
                        <td>Monthly Salary($):</td>
                        <td>                           
                            <asp:TextBox ID="txtMonthlySalary" runat="server" ></asp:TextBox>
                        </td>
                       
                        <td></td>
                        <td> <asp:DropDownList ID="ddlEndYear" runat="server" Width="74px">
                                
                            </asp:DropDownList></td>
                    </tr>

                     <tr>
                        <td>Profession:</td>
                       
                           <td>
                            <asp:TextBox ID="txtProfession" runat="server" MaxLength="15" class="dollar-text"></asp:TextBox>
                        </td>
                        
                        <td></td>
                        <td></td>
                    </tr>

                     <tr>
                        <td>Years in Field:</td>
                        
                           <td>
                            <asp:TextBox ID="txtYearsInField" runat="server" MaxLength="15" class="dollar-text"></asp:TextBox>
                        
                        </td>
                        <td></td>
                        <td></td>
                    </tr>

                  
                </table>


                </div>
            </div>

           
   
    </form>
</body>
</html>