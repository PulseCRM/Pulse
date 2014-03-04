<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskReminderSnoozePopup.aspx.cs" Inherits="TaskReminderSnoozePopup"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Task Reminder Snooze Popup</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
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
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
   

       <script src="../js/jquery.blockUI.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">


        function btnCancel_onclick() {


            //window.parent.location.href = window.parent.location.href;


            //window.parent.location.href = "../LPWeb/LoanDetails/TaskReminderPopup.aspx";
            window.opener = null;
            window.close();
            
          
        }

      function BeforeSave() {

          var txtSnooze = document.getElementById("<%=txtSnooze.ClientID%>").value;

          if (txtSnooze != null && typeof (txtSnooze) != "undefined" && txtSnooze != "") {
              if (/^[0-9]*[1-9][0-9]*$/.test(txtSnooze)) {//这个正则表达式为整数                         
                  if (txtSnooze<1 || txtSnooze >= 32767) {
                      alert("Enter 1 to 32767 between!");

                      return false;
                  }
              }
              else {
                  alert("Snooze Minutes is a positive integer!");

                  return false;
              }
           }
           else {
               alert("Snooze Minutes is Null!");
               return false;

           }

        
       }

       function ShowWaitingDialog(WaitingMsg) {
           //alert(WaitingMsg);

           $("#WaitingMsg").text(WaitingMsg);
           $.blockUI({ message: $('#divWaiting'), css: { width: '100px'} });
       }

       function first() {
           parent.checkLeave();
       }

     </script>    
   
</head>
<%--<body onbeforeunload="first()">--%>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 150px; height: 100px; border: solid 0px red;">
        <div id="tipsDiv">  
        </div> 
        <div id="divDetails" style="margin-top: 10px;">
            <table>
                <tr>
                    <td>
                        Snooze
                    </td>
                    <td>
                        <asp:TextBox ID="txtSnooze" runat="server" MaxLength="5" style="width: 50px"></asp:TextBox> minutes
                    </td>
                       
                   
                </tr>
                 
                
            </table>
          
           
        </div>

        <div id="divBtns">
            <table>
                <tr>
                    <td>
                       
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClick="btnSave_Click"
                         OnClientClick="return BeforeSave();" />
                    </td>
                     <td>

                    

                    <%--<input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()"/>--%>
                </td>
                </tr>
            </table>
        </div>

     </div> 

     <div id="divWaiting" style="display: none; padding: 5px;">
		<table style="margin-left: auto; margin-right: auto;">
			<tr>
				<td>
					<img id="imgWaiting" src="../images/waiting.gif" />
				</td>
				<td style="padding-left: 15px;">
					<label id="WaitingMsg" style="color: #818892; font-weight: bold;"></label>
				</td>
			</tr>
		</table>
	</div>
    </form>

   
   <%-- <div id="divSearchContact" title="Partner Contact Search" style="display: none;"> 
        <iframe id="ifrSearchContact" frameborder="0" style="overflow: SCROLL; overflow-x: HIDDEN">
        </iframe>
    </div>--%>
</body>
</html>