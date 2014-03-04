<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogLeadTaskPopup.aspx.cs" Inherits="LogLeadTaskPopup"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Log Lead Task Popup</title>
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

        $(document).ready(function () {

            $("#txtDate").datepick();
            $("#txtDate").ReadOnly();
            

        });


        function radTaskList_onclick() {

            $("#ddlTaskList").removeAttr("disabled");
            $("#txtTaskName").val("");
            $("#txtTaskName").attr("disabled", "true");
            $("#txtTaskName").rules("remove", "required");
        }

        function radTaskName_onclicik() {

            $("#ddlTaskList").attr("disabled", "true");
            $("#txtTaskName").removeAttr("disabled");
            $("#txtTaskName").rules("add", {
                required: true,
                messages: { required: "*" }
            });
        }


        function btnCancel_onclick() {


            window.parent.location.href = window.parent.location.href;
          
        }

        function BeforeSave() {
           var type = $("#<%=rbtaskNameSelect.ClientID%>:checked");
           var taskname = "";

           if (type.size() > 0) {
               taskname = $("#<%=ddlTaskList.ClientID%>").val();
           }
           else {
               taskname = document.getElementById("<%=txtTaskname.ClientID%>").value;
           }

           if (taskname != null && typeof (taskname) != "undefined" && taskname != "" && taskname != "-- select --") {
               // show waiting dialog
               ShowWaitingDialog("Please wait...");
           }
           else {
               alert("Please enter/select the task name.");
               return false;

           }

          

        
       }

       function ShowWaitingDialog(WaitingMsg) {
           //alert(WaitingMsg);

           $("#WaitingMsg").text(WaitingMsg);
           $.blockUI({ message: $('#divWaiting'), css: { width: '150px'} });
       }

       function CheckboxCheckChange(checkbox) {

           //alert('11');

           if (checkbox.checked == false) {

               $("#txtDate").val("");
               $("#txtTime").val("");

               $("#txtDate").attr("disabled", true);
               $("#txtTime").attr("disabled", true);
           }
           else {

               $("#txtDate").removeAttr("disabled");
               $("#txtTime").removeAttr("disabled");


               $("#txtDate").val($("#hdnNowDate").val());
               //$("#txtTime").val($("#hdnNowTime").val());
           }

           return false;
       }


           
    </script> 
   <script language="javascript" type="text/javascript">
       $(function () {
           $("#txtTaskname").blur(function () {
               var params = '{str:"' + $(this).val() + '"}';  //此处参数名要注意和后台方法参数名要一致               
               $.ajax({
                   type: "POST",                   //提交方式                      
                   url: "LogLeadTaskPopup.aspx/ShowTaskName",   //提交的页面/方法名                      
                   data: params,                   //参数（如果没有参数：null） 
                   //dataType: "text",               //类型 
                   dataType: 'json',
                   contentType: "application/json; charset=utf-8",
                   //                   beforeSend: function (XMLHttpRequest) {
                   //                       //$('#tipsDiv').text("");
                   //                       //$('#tipsDiv').append("<img id='imgWaiting' src='../images/waiting.gif' style='position: relative; top: 2px;' />"); 

                   //                   },

                   success: function (data) {

//                       alert(data.d);
//                       //$('#tipsDiv').text("");
//                       //alert(data.d);
//                       if (data.d == "error") {

//                           //document.getElementById("Label3").innerHTML = "Task Name Repeat!"; 
//                           //alert("Task Name Repeat!");
//                          // history.back(-1); 


//                           return false;
//                          
//                       }
//                       else if (data.d == "null") {
//                           alert("Task Name is Null!");
//                           return false;
//                       }
                   },
                   //                     error: function (xhr, msg, e) {
                   //                         alert("Task Name Repeat!");
                   //                         //$('#tipsDiv').text("");
                   //                     }

                   error: function (XMLResponse) { alert(XMLResponse.responseText) }


               });
           });
       });






           
     </script> 

    

       
   
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 350px; height: 200px; border: solid 0px red;">
        <div id="tipsDiv">  
        </div> 

        

        <div id="divDetails" style="margin-top: 10px;">

            <div id="divBtns">
                <table>
                    <tr>
                        <td>
                       
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClick="btnSave_Click" OnClientClick="return BeforeSave();"/>
                        </td>
                         <td>
                        <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()"/>
                    </td>
                    </tr>
                </table>
            </div>

            <table>
                <tr>
                    <td style="padding-left: 2px; width:80px; vertical-align:top;" rowspan="2">
                        <asp:Label ID="Label1" runat="server" Text="Task Name:"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButton ID="rbtaskNameSelect" GroupName="taskname" Checked="true" runat="server" onclick="radTaskList_onclick()" /> <asp:DropDownList ID="ddlTaskList" runat="server" Width="164px" DataTextField="TaskName" DataValueField="TaskName">
                                        </asp:DropDownList>
                    </td>
                   
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton ID="rbtaskNameInput" GroupName="taskname"  onclick="radTaskName_onclicik()"   runat="server" /> <asp:TextBox ID="txtTaskname" runat="server" MaxLength="20" style="width: 100px" OnMouseOut="CheckUserName()"></asp:TextBox>
                    </td>
                </tr>
                 
                
            </table>
            <br />
            <table>
               
                  <tr>
                    <td style="width: 80px;vertical-align:top;"  rowspan="2">
                       <asp:CheckBox ID="cbInterestOnly" runat="server" Text="Completed"  OnClick="CheckboxCheckChange(this)"/>
                    </td>
                     <td style="width: 60px; padding:10px;">
                        <asp:Label ID="Label2" runat="server" Text="Due Date:"></asp:Label>
                     </td>
                     <td> 
                        <asp:TextBox ID="txtDate" runat="server" style="width: 80px" class="dollar-text"></asp:TextBox>
                     </td>
                   
                </tr>

                <tr>
                    
                    <td style="width: 60px; padding:10px;">
                        <asp:Label ID="Label3" runat="server" Text="Due Time:"></asp:Label>
                     </td>
                     <td>
                        <asp:TextBox ID="txtTime" runat="server" style="width: 40px" class="dollar-text" Visible="false"></asp:TextBox>
                         <asp:DropDownList ID="ddlDueTime_hour" runat="server">
                             <asp:ListItem Value="00">00am</asp:ListItem>
                             <asp:ListItem Value="01">01am</asp:ListItem>
                             <asp:ListItem Value="02">02am</asp:ListItem>
                             <asp:ListItem Value="03">03am</asp:ListItem>
                             <asp:ListItem Value="04">04am</asp:ListItem>
                             <asp:ListItem Value="05">05am</asp:ListItem>
                             <asp:ListItem Value="06">06am</asp:ListItem>
                             <asp:ListItem Value="07">07am</asp:ListItem>
                             <asp:ListItem Value="08">08am</asp:ListItem>
                             <asp:ListItem Value="09">09am</asp:ListItem>
                             <asp:ListItem Value="10">10am</asp:ListItem>
                             <asp:ListItem Value="11">11am</asp:ListItem>
                             <asp:ListItem Value="12">12am</asp:ListItem>
                             <asp:ListItem Value="13">13pm</asp:ListItem>
                             <asp:ListItem Value="14">14pm</asp:ListItem>
                             <asp:ListItem Value="15">15pm</asp:ListItem>
                             <asp:ListItem Value="16">16pm</asp:ListItem>
                             <asp:ListItem Value="17">17pm</asp:ListItem>
                             <asp:ListItem Value="18">18pm</asp:ListItem>
                             <asp:ListItem Value="19">19pm</asp:ListItem>
                             <asp:ListItem Value="20">20pm</asp:ListItem>
                             <asp:ListItem Value="21">21pm</asp:ListItem>
                             <asp:ListItem Value="22">22pm</asp:ListItem>
                             <asp:ListItem Value="23">23pm</asp:ListItem>
                         </asp:DropDownList>
                         <%--<% ddlDueTime_hour.SelectedValue = DateTime.Now.Hour.ToString(); %>--%>
                         <asp:DropDownList ID="ddlDueTime_min" runat="server">
                             <asp:ListItem>00</asp:ListItem>
                             <asp:ListItem>05</asp:ListItem>
                             <asp:ListItem>10</asp:ListItem>
                             <asp:ListItem>15</asp:ListItem>
                             <asp:ListItem>20</asp:ListItem>
                             <asp:ListItem>25</asp:ListItem>
                             <asp:ListItem>30</asp:ListItem>
                             <asp:ListItem>35</asp:ListItem>
                             <asp:ListItem>40</asp:ListItem>
                             <asp:ListItem>45</asp:ListItem>
                             <asp:ListItem>50</asp:ListItem>
                             <asp:ListItem>55</asp:ListItem>
                         </asp:DropDownList>
                    </td>

                </tr>
            </table>

            <table>
                <tr>
                    <td style="vertical-align:top; padding:10px;">
                        Notes
                    </td>
                    
                    <td> 
                        <asp:TextBox ID="txbNotes" runat="server" TextMode="MultiLine" Rows="3" MaxLength="500" Width="240" Height="80"></asp:TextBox>
                    
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
    <asp:HiddenField ID="hdnNowDate" runat="server" />
    <asp:HiddenField ID="hdnNowTime" runat="server" />
    </form>

   
   <%-- <div id="divSearchContact" title="Partner Contact Search" style="display: none;"> 
        <iframe id="ifrSearchContact" frameborder="0" style="overflow: SCROLL; overflow-x: HIDDEN">
        </iframe>
    </div>--%>
</body>
</html>