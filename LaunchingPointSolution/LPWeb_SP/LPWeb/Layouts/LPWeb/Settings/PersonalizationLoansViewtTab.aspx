<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" 
Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalizationLoansViewtTab.aspx.cs" 
Inherits="LPWeb.Layouts.LPWeb.Settings.PersonalizationLoansViewtTab"  MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.validate1.8.1.js" type="text/javascript"></script>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
	.ui-autocomplete-input { width: 220px; }
	.ui-autocomplete-loading { background: white url('../images/loading.gif') right center no-repeat; }
	.ui-autocomplete {
		max-height: 430px;
		overflow-y: auto;
		/* prevent horizontal scrollbar */
		overflow-x: hidden;
		/* add padding to account for vertical scrollbar */
		padding-right: 20px;
	}
	/* IE 6 doesn't support max-height
	 * we use height instead, but this forces the menu to always be this tall
	 */
	* html .ui-autocomplete {
		height: 430px;
	}
	</style>
     <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.progressbar.min.js"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.plugin.contextmenu.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>

 
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.button.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.autocomplete.min.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.bestupper.js" type="text/javascript"></script>
    
    <style type="text/css">
        .TabContent table td
        {
            /* padding-top: 9px; */
        }
    </style>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            DrawTab();


            var gridListID = $("[cid = 'gridList']:first").attr("id");

            $("[cid = 'PFLable']").each(function () {

                BindAutocomplete($(this));
            });

            $("#btnAdd").click(function () {


                if (GetPCAndPFCount() >= 20) {

                    alert("You can select up to 20 columns including the Point fields.");
                    return;
                }

                $("#dialog-modal").dialog({
                   // height: divHeight,
                    width: 440,
                    modal: true,
                    resizable: false,
                });

            });


            $("#btnRemove").click(function () {

                var removeObj = $("#" + gridListID + " tr:not(:first) input:checked").each(function () {

                    $(this).parent().parent().parent().remove();

                });

                if ($("#" + gridListID + " tr:not(:first)").size() == 0) {

                    $("#" + gridListID).empty();
                    $("#" + gridListID).append($("#tmpTable tr").eq(2).clone());
                }


            });

            $('[cid="btnSave"]').click(function () {
                var hiData = '<%=hidData.ClientID %>';
                var data = "";
                var list = $("#" + gridListID + " tr:not(:first)");

                if (list.size() == 0) {

                    $('#' + hiData).val("");
                    return true;
                }

                list.each(function () {
                    //pid='' cid="PFLable"
                    var pid = $(this).find('input[cid="PFLable"]:first').attr('pid');
                    var heading = $(this).find('input[cid="heading"]:first').val();

                    if (pid != "") {

                        if (data != "") {
                            data = data + ";";
                        }
                        data = data + "pid=" + pid + ",heading=" + heading;
                    }

                });
                $('#' + hiData).val(data);


                //alert($('#' + hiData).val());
            });


        });

        function BindAutocomplete(jqobj) {
            jqobj.autocomplete({

                source: "GetPointField_Background.aspx",
                minLength: 2,
                search: function (event, ui) {

                    // clear point field id
                    $(this).attr("pid", "");
                },
                select: function (event, ui) {

                    $(this).attr("pid", ui.item.id);
                    $(this).val(ui.item.label);
                }
            });
        }


        function CheckAll(CheckBox) {
            if (CheckBox.checked) {
                $("#" + gridListID + " tr td input[type=checkbox]").attr("checked", "true");
            }
            else {
                $("#" + gridListID + " tr td input[type=checkbox]").attr("checked", "");
            }
        }


        function GetPCAndPFCount()
        {
          var gridListID = $("[cid = 'gridList']:first").attr("id");
          var total = nPCSChecked+ $("#" + gridListID + " tr input[cid='PFLable']").size();

          
          return total;
        }

        var nPCSChecked = 0;
        function PCSSelected(ckb) {
            //alert(nPCSChecked);

            if (ckb.checked) {
               
                //alert(nPCSChecked);
                if (GetPCAndPFCount() >=20) {
                    alert("You can not pick up more than 20 selections.");
                    ckb.checked = false;
                    
                    return false;
                }
                else {
                    
                    nPCSChecked++;
                    return true;
                }
            }
            else {
                
                nPCSChecked--;
                return true;
            }
        }

        function btnManage_onclick() {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            var iFrameSrc = "../ManagePipelineViewsPopup.aspx?sid=" + sid

            var BaseWidth = 450;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 500;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.ShowGlobalPopup("Manage Pipeline Views", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        

        // add Point FileID
        
        function btnSelectAdd_onclick() {

            var selectCount  = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPipelinePointFieldsList tr td :checkbox[checked=true]").length;
            if (selectCount == 0) {
                alert("Please select one or more point field members.");
                return;
            }

            if(GetPCAndPFCount()+selectCount>20)
            {
                alert("You can select up to 20 columns including the Point fields.("+(GetPCAndPFCount()+selectCount)+" field is currently selected)");
                return;
            }


//            // no data, add columns
//            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserPointFieldsList tr th").length == 0) {
//                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserPointFieldsList tr:first").remove();
//                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPipelinePointFieldsList tr:first").clone().appendTo("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserPointFieldsList");

            //            }

            // add th
            var gridListID = $("[cid = 'gridList']:first").attr("id");
            var TrCount = $("#" + gridListID + " tr").length;
            if (TrCount == 1) {

                // clear tr
                $("#" + gridListID).empty();

                // add th
                $("#" + gridListID).append($("#tmpTable tr").eq(0).clone());
            }


            

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPipelinePointFieldsList tr td :checkbox[checked=true]").each(function(){
                var pointfield = $(this).parent().parent();

                var pid = pointfield.find("input:first").attr("title");
                var label = pointfield.find("td").eq(1).text();
                var heading = pointfield.find("td").eq(2).text();
                //alert( $("#" + gridListID + " [pid = '"+pid+"']").size());
                if($("#" + gridListID + " [pid = '"+pid+"']").size()>0)
                {
                    return;
                }
                    var tmp = $("#tmpTable tr").eq(1).clone()

                    $("#" + gridListID).append(tmp);

                    BindAutocomplete($("#" + gridListID + " [cid = 'PFLable']:last"));

                    var lastAddTr = $("#" + gridListID + " tr:last");

                    lastAddTr.find('[cid="heading"]').val(heading);
                    lastAddTr.find('[cid="PFLable"]').val(label);
                    lastAddTr.find('[cid="PFLable"]').attr('pid', pid);
               
            });
            
            


            // clear user list
//            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserPointFieldsList tr:not(:first)").remove();

//            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPipelinePointFieldsList tr td :checkbox[checked=true]").parent().parent().clone().appendTo("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserPointFieldsList");

            // uncheck
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserPointFieldsList tr td :checkbox").attr("checked", "");

            // close modal
            $("#dialog-modal").dialog("close");
        }

        // popup cancel
        function btnSelectCancel_onclick() {

            // close modal
            $("#dialog-modal").dialog("close");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="Heading">
        User Personalization</div>
    <div id="divCompanyTabs" style="margin-top: 15px;">
        <div class="JTab">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">
                        &nbsp;
                    </td>
                    <td>
                        <div id="tabs10">
                            <ul>
                                <li><a href="PersonalizationSettings.aspx"><span>Settings</span></a></li>
                                <li><a href="PersonalizationPreferences.aspx"><span>Preferences</span></a></li>
                                <%--<li><a href="PersonalizationMarketing.aspx"><span>Marketing</span></a></li>--%>
                                <li id="current"><a href="PersonalizationLoansViewtTab.aspx"><span>Pipeline View</span></a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="TabBody">
                <div id="TabLine1" class="TabLeftLine" style="width: 242px">
                    &nbsp;</div>
                <div id="TabLine2" class="TabRightLine" style="width: 434px">
                    &nbsp;</div>
                <div class="TabContent">
                    

                    <div class="subJTab">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td style="width: 10px;">
                                    &nbsp;
                                </td>
                                <td>
                                    <div id="tabsubs10">
                                        <ul>
                                            <li id="currentsub"><a href="PersonalizationLoansViewtTab.aspx"><span>Loans View</span></a></li>
                                        </ul>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="TabsubBody">
                            <div id="TabsubLine1" class="TabLeftLine" style="width: 242px">
                                &nbsp;</div>
                            <div id="TabsubLine2" class="TabRightLine" style="width: 434px">
                                &nbsp;</div>
                            <div class="TabContent">


                    <br />
                    <asp:Button ID="btnSave" runat="server" cid="btnSave" CssClass="Btn-66" Text="Save" OnClick="btnSave_Click" />

                    <input id="btnManage" type="button" value="Manage My Pipeline Views" class="Btn-250" onclick="btnManage_onclick()" />

                    <div id="divModuleName1" class="ModuleTitle" style="padding-left: 0px; padding-top: 15px;">
                        Loans View Pipeline Column Selections (pick up to 20) 
                        <span style=" margin-left:20px;">Default Loans Pipeline View: <asp:DropDownList ID="ddlDefaultLoansPV" runat="server" DataTextField="ViewName" DataValueField="UserPipelineViewID"><asp:ListItem Selected="True" Text="-- select --" Value="0"></asp:ListItem> </asp:DropDownList></span> 
                    </div>
                    
                    <div class="DashedBorder" style="margin-top: 8px;">
                        &nbsp;</div>
                    <div style="margin-top: 3px;">
                        <table cellpadding="2" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbBranch" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Branch
                                </td>


                                <td>
                                    <asp:CheckBox ID="ckbAmount" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px; width: 210px;">
                                    Amount
                                </td>


                                <td>
                                    <asp:CheckBox ID="ckbPercentComplete" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px; width: 210px;">
                                    Percent complete
                                </td>


                                <td>
                                    <asp:CheckBox ID="ckbLoanOfficer" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Loan Officer
                                </td>


                            </tr>


                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbPointFolder" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px; width: 210px;">
                                    Point folder
                                </td>
                                
                                <td>
                                    <asp:CheckBox ID="ckbLien" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Lien
                                </td>

                                 <td>
                                    <asp:CheckBox ID="ckbTaskCount" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Task count
                                </td>

                                <td>
                                    <asp:CheckBox ID="ckbAssistant" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Loan Officer Assistant
                                </td>
                               
                            </tr>

                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbFilename" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Filename
                                </td>

                                <td>
                                    <asp:CheckBox ID="ckbRate" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Rate
                                </td>

                                <td>
                                    <asp:CheckBox ID="ckbAlerts" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Alerts
                                </td>

                                <td>
                                    <asp:CheckBox ID="ckbCloser" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Closer
                                </td>

                            </tr>
                            <tr>

                                <td></td>
                                <td></td>

                                <td>
                                    <asp:CheckBox ID="ckbLender" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Lender
                                </td>

                                <td>
                                    <asp:CheckBox ID="chkLastComplStage" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Last Completed Stage
                                </td>

                                <td>
                                    <asp:CheckBox ID="ckbDocPrep" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Doc Prep
                                </td>

                            </tr>

                            <tr>
                               
                                 <td></td>
                                <td></td>


                                <td>
                                    <asp:CheckBox ID="ckbLockExpirDate" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Lock expiration date
                                </td>

                                <td>
                                    <asp:CheckBox ID="chkLastStageComlDate" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Last Completed Stage Date
                                </td>

                                <td>
                                    <asp:CheckBox ID="ckbShipper" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Shipper
                                </td>
                                
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>

                                <td>
                                    <asp:CheckBox ID="cbxPurpose" runat="server" onclick="PCSSelected(this)" /> 
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Purpose
                                </td>


                                <td>
                                    <asp:CheckBox ID="ckbStage" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Current Stage
                                </td>

                                <td>
                                    <asp:CheckBox ID="ckbProcessor" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Processor
                                </td>

                            </tr>
                            <tr>
                                
                                 <td></td>
                                <td></td>

                                <td>
                                    <asp:CheckBox ID="cbxLoanProgram" runat="server" onclick="PCSSelected(this)" /> 
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Loan Program
                                </td>


                                 <td></td>
                                <td></td>
                               

                                <td>
                                    <asp:CheckBox ID="cbxJrProcessor" runat="server" onclick="PCSSelected(this)" /> 
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Jr Processor
                                </td>
                                
                            </tr>
                            <tr>
                                
                                <td></td>
                                <td></td>

                                <td>
                                    <asp:CheckBox ID="ckbEstimatedClose" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Estimated Close
                                </td>


                                <td></td>
                                <td></td>
                                
                                
                                <td></td>
                                <td></td>
                                
                            </tr>

                            <tr>
                                
                                <td></td>
                                <td></td>

                                <td>
                                    <asp:CheckBox ID="ckbLastLoanNote" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;height:25px;">
                                    Last Loan Note
                                </td>


                                <td></td>
                                <td></td>
                                
                                
                                <td></td>
                                <td></td>
                                
                            </tr>
                        </table>

                    </div>

                    <div style=" width:500px;">
                    <fieldset>
                        <legend>Point Fields</legend>
                        
                        <div style=" margin-left:60px;"><a id="btnAdd" href="javascript:;">Add</a> &nbsp;&nbsp;&nbsp;&nbsp;| &nbsp;&nbsp;&nbsp;&nbsp;<a id="btnRemove" href="javascript:;">Remove</a></div> 
                                <br />
                                <div id="divpflList" class="ColorGrid" style="margin-top: 5px;">
                            <asp:GridView ID="gridList"  cid="gridList" runat="server" DataKeyNames="PointFieldId" EmptyDataText="There is no data in database."
                                AutoGenerateColumns="False" CellPadding="3"
                                CssClass="GrayGrid" GridLines="None" >
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <AlternatingRowStyle CssClass="EvenRow" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                        <HeaderTemplate>
                                            <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ckbSelected" runat="server" EnableViewState="true" ToolTip='<%# Eval("PointFieldId") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                                        <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Point Field Label" SortExpression="PointFieldId" ItemStyle-Width="250px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txbPFLabel" runat="server" pid='<%# Eval("PointFieldId") %>' cid="PFLable" ReadOnly="true" Text='<%# Eval("Label") %>' Width="200"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Column Heading" SortExpression="Heading" ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txbPFHeading" runat="server" cid="heading" Text='<%# Eval("Heading") %>' ReadOnly="true" Width="150"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div style="display:none">
                                <table id="tmpTable">
                                <tr>
                                    <th class="CheckBoxHeader" scope="col">
                                    <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                                    </th>
                                    <th scope="col">Point Field Label</th><th scope="col">Column Heading</th>
                                </tr>
                                <tr>
                                    <td class="CheckBoxColumn">
                                    <span title=""><input  type="checkbox" name="ckbSelected" /></span>
                                    </td><td style="width:250px;">
                                    <input name="txbPFLabel" type="text" value=""  cid="PFLable" pid="" readonly="readonly" style="width:200px;" />
                                    </td><td style="width:150px;">
                                    <input name="txbHeading" type="text" value="" cid="heading" readonly="readonly" style="width:150px;" />
                                    </td>
                                </tr>
                                <TR class=EmptyDataRow align=middle><TD colSpan=3>There is no data in database.</TD></TR>
                                </table>
                                
                                <asp:HiddenField ID="hidData" runat="server" /> 
                                <asp:HiddenField ID="hidOldPointFieldID" runat="server" /> 
                                
                            </div>
                            <div class="GridPaddingBottom">
                                &nbsp;</div>

                    </fieldset>
                    </div>

                    </div>
                    </div>

                </div>

            </div>
        </div>
         <div id="dialog-modal" title="Select Pipeline Point Fields" style="display: none;">
            <br />
             <div style="margin-top: 20px; margin-left:20px; text-align:left;">
                <input id="btnSelectAdd" type="button" value="Select" class="Btn-66" onclick="return btnSelectAdd_onclick()" />&nbsp;&nbsp;
                <input id="btnSelectCancel" type="button" value="Cancel" class="Btn-66" onclick="return btnSelectCancel_onclick()" />
            </div>
             <div class="SplitLine" style="position: absolute; left: -8px; top: 380px; width: 446px;">
                &nbsp;
            </div>
            <div style="width: 418px; height: 370px; overflow: auto;">
                <div id="divPipelinePointFieldsList" class="ColorGrid" style="width: 400px;">
                    <asp:GridView ID="gridPipelinePointFieldsList" runat="server" DataKeyNames="PointFieldId" EmptyDataText="There is no Point Fields for selection."
                        CellPadding="3" CssClass="GrayGrid" GridLines="None" AutoGenerateColumns="false">
                        <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                        <AlternatingRowStyle CssClass="EvenRow" />
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                <HeaderTemplate>
                                    <input id="Checkbox1" type="checkbox" onclick="CheckAll2(this)" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <input id="Checkbox2" type="checkbox" title="<%# Eval("PointFieldId") %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Label" HeaderText="Point Field Label" />
                            <asp:BoundField DataField="Heading" HeaderText="Pipeline Column Heading" />
                        </Columns>
                    </asp:GridView>
                    <div class="GridPaddingBottom">
                        &nbsp;</div>
                </div>
            </div>
           
           
        </div>
    </div>
</asp:Content>
