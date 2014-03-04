<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" AutoEventWireup="true"
    Inherits="LPWeb.Settings.Settings_PointFolderDetails" CodeBehind="PointFolderDetails.aspx.cs" %>

   
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        // <!CDATA[ 
        function Cancel()
        {
            window.location.href = $("#<%= this.hfPageFrom.ClientID %>").val();


            return false;
        }

       





        // ]]>  
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" Runat="Server">
  <script src="../js/common.js" type="text/javascript"></script>
 <script type="text/javascript" language="javascript">
     function BeforeSave() {

         var tt = document.getElementById("<%=cbdigitsName.ClientID%>").checked;
         
         //alert(tt);
         if (tt) {
             //选中之后 判断是否为空



             var t = document.getElementById("<%=txtdigits.ClientID%>").value;

            
             if (t != null && typeof (t) != "undefined" && t != "") {

                 if (/^[-]?\d+$/.test(t)) {//这个正则表达式为整数

                     if (t < 8 || t > 20) {
                         alert("Enter 8 to 20 between!");

                         return false;
                     }

                 }
                 else {
                     alert("Not a number!");

                     return false;
                 }
             }
             else { 
              alert("Digits is null");
                 return false;
             }

             
         }
        
        

         return true;
         
     }




     function CheckChkBox(obj1) {

         if (!obj1.checked) {

             var t = document.getElementById("<%=txtdigits.ClientID%>");
             t.value = "";
             return false;
         }
         else {
             var objcbx = document.getElementById("<%=cbEnableAutoFileNaming.ClientID%>");
             if (!objcbx.checked) {
                 alert('Select Enable Auto File-naming!');
                 obj1.checked = false;
                 
             }
         }

     }

         function CheckboxCheckChange() {

             //alert('11');

             var objcbx = document.getElementById("<%=cbEnableAutoFileNaming.ClientID%>");
             var tt = document.getElementById("<%=cbdigitsName.ClientID%>");
             var t = document.getElementById("<%=txtdigits.ClientID%>");
           

             //alert(objcbx);
             if (!objcbx.checked) {
                 //alert('1');
                 tt.checked = false;
                
                 tt.disabled = true;
                 t.disabled = true;
                 t.value = "";
                 //tt.disabled = false;

             }
             else {
                 //alert('2');
                 //tt.checked = true;
                 tt.disabled = false;
                 t.disabled = false;
             }

        

             return false; 
         }
        


       
     </script>
    <div id="divModuleName" class="Heading">Folder Details</div>
    <div class="SplitLine"></div>
    <div id="divContent" class="DetailsContainer">
        <div>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>Point Folder Name</td>
                    <td style="padding-left: 15px;">
                        <asp:TextBox ID="tbxFolderName" runat="server" Width="450px" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 9px;">Path</td>
                    <td style="padding-left: 15px; padding-top: 9px;">
                        <asp:TextBox ID="tbxPath" runat="server" Width="450px" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 9px;">Branch Name</td>
                    <td style="padding-left: 15px; padding-top: 9px;">
                        <asp:TextBox ID="tbxBranch" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>

                 <tr>
                    <td style="padding-top: 9px;"></td>
                    <td style="padding-left: 15px; padding-top: 9px;">
                       <asp:CheckBox ID="cbEnableAutoFileNaming" runat="server" Text=" Enable Auto File-naming" OnClick="CheckboxCheckChange()"/>
                        <span style=" width:30px;">  &nbsp&nbsp &nbsp&nbsp &nbsp&nbsp &nbsp</span>
                        Prefix &nbsp <asp:TextBox ID="txbPrefix" runat="server"></asp:TextBox>
                    </td>
                </tr>

                 <tr>
                    <td style="padding-top: 9px;"></td>
                    <td style="padding-left: 15px; padding-top: 9px;">
                       <asp:CheckBox ID="cbdigitsName" runat="server" Text=" Use Random Numbers with"/>
                      
                       <asp:TextBox ID="txtdigits" Width="50px" runat="server" MaxLength="2"></asp:TextBox>digits 
                    </td>
                </tr>

                 <tr>
                    <td style="padding-top: 9px;" colspan="2">
                        <asp:Button ID="btnSave" runat="server" class="Btn-66" Text="Save" OnClientClick="return BeforeSave();"  OnClick="btnSave_Click"  />
                        <asp:Button ID="btnCancel" runat="server" class="Btn-66" Text="Cancel" OnClick="btnCancel_Click"/>
                    </td>
                </tr>

                <tr>
                    <td style="padding-top: 9px;">Last import date</td>
                    <td style="padding-left: 15px; padding-top: 9px;">
                        <asp:TextBox ID="tbxLastImportDate" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 9px;">Import Count</td>
                    <td style="padding-left: 15px; padding-top: 9px;">
                        <asp:TextBox ID="tbxImportCount" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 9px;">Enabled</td>
                    <td style="padding-left: 15px; padding-top: 9px;">
                        <asp:TextBox ID="tbxEnabled" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divLogList" class="ColorGrid" style="width: 220px; margin-top: 9px; padding-left:116px;">
        <asp:GridView ID="gridSyncLogList" runat="server" DataKeyNames="HistoryId" EmptyDataText="There is no members in this group." AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="ImportTime" HeaderText="Sync Date" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="120"/>
                <asp:BoundField DataField="StatusName" HeaderText="Status" HeaderStyle-Width="100"/>
            </Columns>
        </asp:GridView>
        <div class="GridPaddingBottom">&nbsp;</div>
    </div>
    <asp:HiddenField ID="hdnFolderID" runat="server" />
    <asp:HiddenField ID="hfPageFrom" runat="server" />

<%--    <div style="margin-top: 20px;">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td style="padding-left: 8px;">
                    <button type="button" class="Btn-66" onclick="Cancel()">Cancel</button>
                 </td>
            </tr>
        </table>
    </div>--%>
</asp:Content>

