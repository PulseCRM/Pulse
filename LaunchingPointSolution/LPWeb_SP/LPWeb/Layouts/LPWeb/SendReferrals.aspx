<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" DynamicMasterPageFile="~masterurl/default.master" AutoEventWireup="true" Inherits="SendReferrals" Codebehind="SendReferrals.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" Runat="Server">
    <div>Please enter the contact of your friends and relatives below:</div>
    <div id="divCalendar" class="Widget" style="width: 920px; margin-top: 10px;">
        <div class="Widget_Grid_Header">
            <div>
                <table cellpadding="0" cellspacing="0" style="width: 100%; color: #656e7b; font-size: 12px;">
                    <tr>
                        <td style="width: 57px;">
                            <input id="Checkbox1" type="checkbox" />
                         </td>
                        <td style="width: 211px;">Name</td>
                        <td style="width: 268px;">Phone</td>
                        <td>Email</td>
                    </tr>
                </table>
            </div>
        </div>

        <div class="Widget_Body">
            
            <div class="GridRow24_Odd" style="">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                    <tr>
                        <td style="width: 57px; border-right: solid 1px #e4e7ef; text-align: center;">
                            <input id="Checkbox2" type="checkbox" />
                         </td>
                        <td style="width: 211px; border-right: solid 1px #e4e7ef;">&nbsp;</td>
                        <td style="width: 268px; border-right: solid 1px #e4e7ef;">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                <div class="DashedBorder">&nbsp;</div>
            </div>             
            <div class="GridRow24_Even" style="">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                    <tr>
                        <td style="width: 57px; border-right: solid 1px #e4e7ef; text-align: center;">
                            <input id="Checkbox3" type="checkbox" />
                         </td>
                        <td style="width: 211px; border-right: solid 1px #e4e7ef;">&nbsp;</td>
                        <td style="width: 268px; border-right: solid 1px #e4e7ef;">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                <div class="DashedBorder">&nbsp;</div>
            </div>
            
            <div class="GridRow24_Odd" style="">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                    <tr>
                        <td style="width: 57px; border-right: solid 1px #e4e7ef; text-align: center;">
                            <input id="Checkbox4" type="checkbox" />
                         </td>
                        <td style="width: 211px; border-right: solid 1px #e4e7ef;">&nbsp;</td>
                        <td style="width: 268px; border-right: solid 1px #e4e7ef;">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                <div class="DashedBorder">&nbsp;</div>
            </div>             
            <div class="GridRow24_Even" style="">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                    <tr>
                        <td style="width: 57px; border-right: solid 1px #e4e7ef; text-align: center;">
                            <input id="Checkbox5" type="checkbox" />
                         </td>
                        <td style="width: 211px; border-right: solid 1px #e4e7ef;">&nbsp;</td>
                        <td style="width: 268px; border-right: solid 1px #e4e7ef;">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                <div class="DashedBorder">&nbsp;</div>
            </div>

            <div class="GridRow24_Odd" style="">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                    <tr>
                        <td style="width: 57px; border-right: solid 1px #e4e7ef; text-align: center;">
                            <input id="Checkbox6" type="checkbox" />
                         </td>
                        <td style="width: 211px; border-right: solid 1px #e4e7ef;">&nbsp;</td>
                        <td style="width: 268px; border-right: solid 1px #e4e7ef;">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                <div class="DashedBorder">&nbsp;</div>
            </div>             
            <div class="GridRow24_Even" style="">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                    <tr>
                        <td style="width: 57px; border-right: solid 1px #e4e7ef; text-align: center;">
                            <input id="Checkbox7" type="checkbox" />
                         </td>
                        <td style="width: 211px; border-right: solid 1px #e4e7ef;">&nbsp;</td>
                        <td style="width: 268px; border-right: solid 1px #e4e7ef;">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                <div class="DashedBorder">&nbsp;</div>
            </div>

            
        </div>
    </div>
</asp:Content>

