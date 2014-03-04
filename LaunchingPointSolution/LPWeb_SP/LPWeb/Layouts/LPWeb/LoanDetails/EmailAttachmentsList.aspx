<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailAttachmentsList.aspx.cs" Inherits="EmailAttachmentsList" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<body>
    <form id="form1" runat="server">
    <div id="AttachmentsTable">

<table  style="margin-top: 5px;" >
    <tr>
        <td style="width: 80px;">Attachments:</td>
        <td>
            <a id="AddAttachments" href="javascript:void(0);">Add Attachments</a> | <a id="RemoveAttachments" href="javascript:;" >Remove Attachments </a>
        </td>
    </tr>
    <tr>
        <td colspan="2" style=" text-align:center; vertical-align:top;">
        <div id="divAttachmentsList" style=" height:70px; overflow:auto;" >
        <table>
            <asp:repeater ID="rpAttachmentsList"  runat="server">  
            <ItemTemplate>
                <tr>
                    <td style="padding-top: 3px; text-align:right;">
                        <input cid="attachments" type="checkbox" attachId='<%# Eval("TemplAttachId")%>'  /></td>
                    <td style="padding-top: 3px; text-align:left; width:300px">
                        <%# Eval("Name")%>.<%# Eval("FileType")%>
                    </td>
                </tr>
            </ItemTemplate> 
            </asp:repeater>
       
        </table>
        </div>
        </td>
    </tr>
</table>

</div>

</form>
</body>