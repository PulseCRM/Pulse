<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendNowWithAttachments.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.LoanDetails.SendNowWithAttachments" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.thickbox.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>

    <style>
        div.opt
        {
            margin-top: 20px;
        }
        div.opt input
        {
            margin-right: 10px;
        }
        form
        {
            padding-left: 10px;
        }
        .detail
        {
            margin-top: 20px;
        }
        .detail td
        {
            line-height: 20px;
            white-space: nowrap;
        }
        
		.filebtn{cursor:pointer; width:110px; border:none; height:40px; padding:0px; margin:0px;position:absolute; opacity:0;filter:alpha(opacity=0);font-size:20px;}
        textarea {background-color: white;color: black; }

    </style>
    <script type="text/javascript">
		var upfilecount = 0 ;
        $().ready(function () {
            AddFile();
            $("#btnSend").click(function(){
                    
                //cheack input

                if($("#lbxAttachments option").size()<=0 && $("#tbxNote").val()=="")
                {
                    alert("Attachments / Note is empty.");
                    return false;
                }

                // waiting msg
                ShowWaitingDialog("Please wait,Sending...");

                $("#form1").submit();

            });
            
            $("#btnCancel").click(CloseCurrentWindow);

			$("#btnRemoveAttachment").click(function(){
				
			  var id = $("#lbxAttachments").val();

				$("#file"+id).remove();
				$("#lbxAttachments").find("option:selected").remove();
				return false;
			});
		
             var myDate = new Date();
            $("#hfLocalTime").val(myDate.getFullYear() + "-" + (myDate.getMonth() + 1) + "-" + myDate.getDate() + " " + myDate.getHours() + ":" + myDate.getMinutes() + ":" + myDate.getSeconds());

        });

		function AddFile()
		{   
			upfilecount +=1 ;
			$('<input cid="upfile"  type="file" hideFocus ="true"  id="file' + upfilecount + '" Name="file' + upfilecount + '" accept="" css="filebtn" style="cursor:pointer; width:110px; border:none; height:40px; padding:0px; margin:0px;position:absolute; opacity:0;filter:alpha(opacity=0);font-size:20px;" />')
		   .change(function(){

               if($(this).val()!="" )
               {
                    var isSelect = false;
					var SelectFile = $(this).val();
					$("#lbxAttachments option").each(function(){  
						if($(this).text()==SelectFile)
						{
							isSelect = true;
						}
					});

					if(isSelect)
					{
						alert("The file has been selected!");
						$(this).val('');
						return ;
					}

               
                   if(  $(this).val().toLowerCase().indexOf(".pdf") > 0 || $(this).val().toLowerCase().indexOf(".doc") > 0 
                            || $(this).val().toLowerCase().indexOf(".docx") > 0|| $(this).val().toLowerCase().indexOf(".xls") > 0|| $(this).val().toLowerCase().indexOf(".xlsx") > 0 )  //*.PDF, *.Doc, *.DocX, *.XLS, *.XLSX
				    {
                        var fileNameList  = $(this).val().split("\\");
                        
					    $("#lbxAttachments").append('<option value="1" >'+fileNameList[fileNameList.length-1]+'</option>');

					    $(this).hide();

					    AddFile();
				    }
                    else
                    {
                        alert("File type does not match(*.PDF, *.Doc, *.DocX, *.XLS, *.XLSX)!");   
                    }
               }
           })
           .appendTo("#upfilePool");
		}


        function SendOK(State,Msg)
        {
            if(State==1)
            {
                CloseWaitingDialog();
                CloseCurrentWindow();
            }
            else
            {
                alert(Msg);
                CloseWaitingDialog();
            }
        }

        function CloseCurrentWindow() {
            <%=CloseDialogCodes %>;
        }

        function ShowWaitingDialog(WaitingMsg) {
         
             $("#WaitingMsg3").text(WaitingMsg);
             $.blockUI({ message: $('#divWaiting3'), css: { width: '200px', padding: '7px'} });

        }
        function ShowWaitingDialog3(WaitingMsg) {
 
           
        }
 
        function CloseWaitingDialog() {
 
            $.unblockUI();
        }


    </script>
</head>
<body>
    <form id="form1" method="post" target="ifrmSend"  action="<%= Request.Url.ToString() %>" enctype="multipart/form-data">
    <div id="divDetail">
        <div id="divButtons" class="opt">
            <input ID="btnSend"  type="button" value="Send" class="Btn-91" />
            <input id="btnCancel" type="button" value="Cancel" class="Btn-91" />
        </div>
        <table class="detail">
            <tr>
                <td valign="top" align="center" >
                    Attachments:
                </td>
                <td>
                    <select id="lbxAttachments" name="lbxAttachments" multiple="multiple" style="color:black; background-color:white; width:380px; height:150px; border:1px solid #bbb; " >
                    </select>
                </td>
                <td>
                    <div style="background: url(../images/Btn-AddAttachement.gif) 0 0 no-repeat;width:110px; height:40px; margin:6px 4px; cursor:pointer; ">
                    <a id="upfilePool" ></a>
                    </div>
                    <br />
                    <button id="btnRemoveAttachment" style="background: url(../images/Btn-RemoveAttachements.gif) 0 0 no-repeat;width:110px; height:40px; border:none;cursor:pointer;" ></button>
                </td>
            </tr>
            <tr>
                <td>
                    Note:
                    <br /><br />
                    <input id="cbExternalViewing" name="cbExternalViewing" checked="checked" type="checkbox" />External
                </td>
                <td colspan="2">
                <textarea id="tbxNote" name="tbxNote" cols="55" rows="3"  style="color:black; background-color:white; width:500px; height:100px"></textarea>
                </td>
            </tr>
        </table>
       
        <input id="hidIsSend" name="hidIsSend" type="hidden" value="1" />
        <input id="hfLocalTime" name="hfLocalTime" type="hidden" value="" />
    </div>
    
    </form>
    <iframe id="ifrmSend" name="ifrmSend" style="display:none;"></iframe>

    <div id="divWaiting" title="Message" style="display: none;">
        <table style="margin-left: auto; margin-right: auto;">
            <tr>
                <td>
                </td>
                <td style="padding-left: 5px; width: 320px;">
                    <label id="WaitingMsg" style="color: #818892; font-weight: bold;">
                    </label>
                </td>
            </tr>
        </table>
    </div>
    <div id="divWaiting3" style="display: none;">
		<table style="margin-left: auto; margin-right: auto;">
			<tr>
				<td>
					<img id="imgWaiting3" src="../images/waiting.gif" />
				</td>
				<td style="padding-left: 5px;">
					<label id="WaitingMsg3" style="color: #818892; font-weight: bold;"></label>
				</td>
			</tr>
		</table>
	</div>


</body>
</html>