<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" CodeBehind="CompanyPipelineViewLoansView.aspx.cs" Inherits="CompanyPipelineViewLoansView" %>

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
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            DrawTab();
            DrawSubTab();


            $("[cid = 'PFLable']").each(function () {

                BindAutocomplete($(this));
            });

            var gridListID = $("[cid = 'gridList']:first").attr("id");

            $("#btnAdd").click(function () {

                //tmpTable

                var TrCount = $("#" + gridListID + " tr").length;

                if (TrCount == 21) {

                    alert("You can select up to 20 Point fields.");
                    return;
                }

                // add th
                if (TrCount == 1) {

                    // clear tr
                    $("#" + gridListID).empty();

                    // add th
                    $("#" + gridListID).append($("#tmpTable tr").eq(0).clone());
                }

                $("#" + gridListID).append($("#tmpTable tr").eq(1).clone());

                BindAutocomplete($("#" + gridListID + " [cid = 'PFLable']:last"));
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

    </script>
    <style>
        .TabContent input.Btn-66
        {
            margin-right: 8px;
        }
        .TabContent input[type="text"], select, input[type="file"]
        {
            margin-left: 15px;
            margin-right: 15px;
        }
        .TabContent table td
        {
            padding-top: 9px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="Heading">
        Company Setup</div>
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
                                <li><a href="CompanyGeneral.aspx"><span>General</span></a></li>
                                <li><a href="CompanyWebEmail.aspx"><span>Web/Email</span></a></li>
                                <li><a href="CompanyPoint.aspx"><span>Sync</span></a></li>
                                <li><a href="CompanyAlerts.aspx"><span>Alert</span></a></li>
                                <li><a href="CompanyLoanPrograms.aspx"><span>Loan Programs</span></a></li>
                                <li><a href="CompanyTaskPickList.aspx"><span>Leads</span></a></li>
                               <%-- <li><a href="CompanyLeadSources.aspx"><span>Lead Sources</span></a></li>--%>
                                <li><a href="CompanyGlobalRules.aspx"><span>Global Rules</span></a></li>
<%--                                <li><a href="CompanyMarketing.aspx"><span>Marketing</span></a></li>--%>
                               <%-- <li><a href="CompanyLeadStatus.aspx"><span>Lead Status</span></a></li>--%>
                                <li><a href="CompanyReport.aspx"><span>Report</span></a></li>
                                <li><a href="CompanyMCT.aspx"><span>MCT</span></a></li>
                                <li id="current"><a href="CompanyPipelineViewLoansView.aspx"><span>Pipeline View</span></a></li>
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
                                            <li id="currentsub"><a href="CompanyPipelineViewLoansView.aspx"><span>Loans View</span></a></li>
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
                    
                                    Please select up to 20 Point fields.   &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnSave" cid="btnSave" runat="server" CssClass="Btn-66" Text=" Save " OnClick="btnSave_Click" />
                                <br /><br />
                               
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
                                            <asp:TextBox ID="txbPFLabel" runat="server" pid='<%# Eval("PointFieldId") %>' cid="PFLable" Text='<%# Eval("Label") %>' Width="200"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Column Heading" SortExpression="Heading" ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txbPFHeading" runat="server" cid="heading" Text='<%# Eval("Heading") %>' Width="150" MaxLength ="25"></asp:TextBox>
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
                                    <input name="txbPFLabel" type="text" value=""  cid="PFLable" pid="" style="width:200px;" />
                                    </td><td style="width:150px;">
                                    <input name="txbHeading" type="text" value="" cid="heading" style="width:150px;" maxlength="25" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">There is no data in database.</td>
                                </tr>
                                </table>
                                
                                <asp:HiddenField ID="hidData" runat="server" /> 
                                <asp:HiddenField ID="hidOldPointFieldID" runat="server" /> 
                                
                            </div>
                            <div class="GridPaddingBottom">
                                &nbsp;</div>
                        </div>

                            </div>
                        </div>
                    </div>


                </div>
            </div>
        </div>
    </div>
</asp:Content>