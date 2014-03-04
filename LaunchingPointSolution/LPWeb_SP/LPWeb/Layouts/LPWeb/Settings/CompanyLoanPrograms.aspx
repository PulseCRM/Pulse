<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master"
    AutoEventWireup="true" Inherits="Settings_CompanyLoanPrograms" CodeBehind="CompanyLoanPrograms.aspx.cs" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
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
        #divUserList td
        {
            margin-left: 0px;
            margin-right: 0px;
            padding: 0;
        }
        
        .Fileup
        {
            cursor: hand;
            position: relative;
            left: -390px;
            width: 90px;
			top:0px;
            background-color: blue;
            opacity: 0;
            filter: alpha(opacity=0);
        }
    </style>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var gridId = "#<%=gvLoanProgramses.ClientID %>";
        $(document).ready(function () {
            DrawTab();


            //var checkItems=
            var checkAll = $(gridId + " :checkbox:eq(0)");
            checkAll.click(function () {
                var allStatus = checkAll.attr("checked");
                $(gridId + " :checkbox:gt(0)").each(function () {
                    $(this).attr("checked", allStatus);
                });
                getSelectedItems();
            });
            //
            $(gridId + " :checkbox:gt(0)").each(function () {
                $(this).unbind("click").click(function () {
                    if ($(this).attr("checked") == false) {
                        checkAll.attr("checked", false);
                    }
                    getSelectedItems();
                });
            });

            $("#<%= btnRemove.ClientID %>").click(function () {
                if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
                    alert("Please select item to delete.");
                    return false;
                }
                else {
                    return confirm("Delete selected item(s)?");
                }
            });

            $("[cid='btImportARM']").click(function () {
                return false;

            });




        });

        var action = "";
        function fileUpARMChange() {

            var file = $("[cid='fileUpARM']");
            //alert(file.val());
            if (file.val().indexOf(".xls") == -1 && file.val().indexOf(".xlsx") && file.val().indexOf(".cvs")) {

                alert("The file type must be xls/xlsx/cvs.");
                return false;
            }
            var thisform = $("form:first");
            action = thisform.attr("action");
            thisform.attr("action", action + "?upfile=1");
            thisform.attr("target", "upfileframe");

            $.blockUI({ message: "Please wait..." });

            $('<iframe  id="upfileframe" name="upfileframe" src="about:blank" height="0" width="0" ></iframe>').appendTo("body");

            $("[cid='btImportARMFile']").click();
        }

        function fileUPARMOK(state, msg) {
            var thisform = $("form:first");
            thisform.attr("action", action);
            thisform.attr("target", "");

            $.unblockUI();
            $("#upfileframe").remove();

            if (state == 1) {
                window.location.reload();
                //window.location.href = action;
            }
            else {
                alert(msg);
                //window.location.href = action;
                window.location.reload()
            }
        }



        function getSelectedItems() {
            var selctedItems = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });

            $("#<%=hfDeleteItems.ClientID %>").val(selctedItems.join(","));
        }

        function IsEmptyLoanProgram() {
            if ($("input[cid='txtLoanProgram']").val() == "") {
                $("#emptyErr").show();
                return false;
            }
        }
    </script>
    <script language="javascript" type="text/javascript">
        
        function aCreate_onclick() {

            var sid = Math.random().toString().substr(2);

            var iFrameSrc = "LoanProgramSetupAdd.aspx?sid=" + sid + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 600
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 250;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Loan Program Setup", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function aUpdate_onclick() {

            var sSelIDs = $("#<%=hfDeleteItems.ClientID %>").val();
            var arrIds = new Array();
            if (sSelIDs.length > 0)
                arrIds = sSelIDs.split(',');

            if (arrIds.length == 0) {
                alert("No record has been selected.");
                return;
            } else if (arrIds.length != 1) {
                alert("Only one record can be selected for this operation.");
                return;
            }
            var Ids = arrIds[0].split(':');

            if (Ids.length != 2) {
                alert("error");
                return;
            }

            var InvestorID = Ids[1];
            var LoanProgramID = Ids[0];

            popLoanProgramSetup(LoanProgramID, InvestorID)
        }

        function aDelete_onclick() {
            
            var selectedList =$("#<%=hfDeleteItems.ClientID %>").val();
            //alert(selectedList);

            if (selectedList.length == 0) {
                alert("No record has been selected.");
                return;
            }

            var r = confirm("Are you sure you want to delete the selected records?");

            if (r) {
                $("[cid='btnDelete']").click();
            }

            //return false;
        }

        function aEnable_onclick() {

            var selectedList = $("#<%=hfDeleteItems.ClientID %>").val();

            if (selectedList.length == 0) {
                alert("No record has been selected.");
                return;
            }

            $("[cid='btnEnable']").click();

            //return false;
        }

        function aDisable_onclick() {

            var selectedList = $("#<%=hfDeleteItems.ClientID %>").val();

            if (selectedList.length == 0) {
                alert("No record has been selected.");
                return;
            }

            $("[cid='btnDisable']").click();

            //return false;
        }

        function popLoanProgramSetup(LoanProgramID, InvestorID) {

            var sid = Math.random().toString().substr(2);

            var iFrameSrc = "LoanProgramSetupEdit.aspx?sid=" + sid + "&LoanProgramID=" + LoanProgramID + "&InvestorID=" + InvestorID;

            var BaseWidth = 600
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 250;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Loan Program Setup", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="Heading">
        Company Setup</div>
    <div id="divCompanyTabs" style="margin-top: 15px;">
        <div class="JTab">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
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
                                <li id="current"><a href="CompanyLoanPrograms.aspx"><span>Loan Programs</span></a></li>
                                 <li><a href="CompanyTaskPickList.aspx"><span>Leads</span></a></li>
                               <%-- <li><a href="CompanyLeadSources.aspx"><span>Lead Sources</span></a></li>--%>
                                <li><a href="CompanyGlobalRules.aspx"><span>Global Rules</span></a></li>
                                <%--<li><a href="CompanyMarketing.aspx"><span>Marketing</span></a></li>--%>
                               <%-- <li><a href="CompanyLeadStatus.aspx"><span>Lead Status</span></a></li>--%>
                                <li><a href="CompanyReport.aspx"><span>Report</span></a></li>
                                <li><a href="CompanyMCT.aspx"><span>MCT</span></a></li>
                                <li><a href="CompanyPipelineViewLoansView.aspx"><span>Pipeline View</span></a></li>
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
                    <table>
                        <tr>
                            <!--
                            <td width="70px">
                                Loan Program
                            </td>
                            <td width="*">
                                <asp:TextBox ID="txtLoanProgram" runat="server" Text="" Width="320" MaxLength="150"></asp:TextBox>
                                <span id="emptyErr" style=" color:Red; display:none;" >*</span>
                                <%--<asp:RequiredFieldValidator ID="rfvLoanProgram" runat="server" ErrorMessage="*" ControlToValidate="txtLoanProgram" ></asp:RequiredFieldValidator>--%>

                                <span><asp:CheckBox ID="cbARM" runat="server" Text=" ARM" /></span>
                            </td>
                            -->
                            <td colspan ="2">
                                
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlInvestors" runat="server" DataTextField="Name" DataValueField ="InvestorID" OnSelectedIndexChanged="ddlInvestors_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="">All Investors</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                         <td>
                                            <asp:DropDownList ID="ddlTypes" runat="server" OnSelectedIndexChanged="ddlTypes_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="">All Types</asp:ListItem>
                                                <asp:ListItem Value="1">ARM Programs</asp:ListItem>
                                                <asp:ListItem Value="0">Fixed Programs</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPrograms" runat="server" DataTextField="LoanProgram" DataValueField="LoanProgramID"  OnSelectedIndexChanged="ddlPrograms_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="">All Programs</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlIndexes" runat="server" DataTextField="IndexType">
                                                <asp:ListItem Value="">All Indexes</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                       
                                        <td>
                                            <asp:Button ID="btnFilter" runat="server" Class ="Btn-66" Text="Filter" OnClick="btnFilter_Click" />
                                        </td>
                                    </tr>
                                </table>

                            </td>

                        </tr>
                        <tr>
                            <td height="20" colspan="2" style="padding-top: 9px; padding-bottom: 9px;">
                                <!-- 
                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="Btn-66" OnClientClick="return IsEmptyLoanProgram();" OnClick="btnAdd_Click" />
                                -->
                                <asp:Button ID="Button1"  cid="btImportARM"  runat="server" Text="Import ARM Programs" Class ="Btn-140" />
                                
                                
                               <div style=" display:none;"> <asp:Button ID="btImportARM" cid="btImportARMFile" runat="server" Text="Import ARM Programs" CssClass ="Btn-250" OnClick="btImportARM_Click" /></div>
                                
                                <asp:Button ID="btImport" runat="server" Text ="Import DataTrac Loan Programs" CssClass ="Btn-250"  OnClick="btImport_Click" />

                                <asp:FileUpload ID="fileUpARM" runat="server" cid="fileUpARM"  CssClass="Fileup" onchange="return fileUpARMChange();"  />
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div id="divToolBar">
                                    <ul class="ToolStrip" style="margin-left: 0px;">
                                        <li><a id="aCreate" href="javascript:aCreate_onclick()">Create</a><span>|</span></li>
                                        <li><a id="aDelete" href="javascript:aDelete_onclick()">Delete</a><span>|</span></li>
                                        <li><a id="aUpdate" href="javascript:aUpdate_onclick()">Update</a><span>|</span></li>
                                        <li><a id="aEnable" href="javascript:aEnable_onclick()">Enable</a><span>|</span></li>
                                        <li><a id="aDisable" href="javascript:aDisable_onclick()">Disable</a><span>|</span></li>
                                    </ul>
                                    <div style=" display:none;">
                                        <asp:Button ID="btnDelete" cid="btnDelete" runat="server" Text="aDelete" OnClick="btnDelete_Click" />
                                        <asp:Button ID="btnEnable" cid="btnEnable" runat="server" Text="aEnable" OnClick="btnEnable_Click" />
                                        <asp:Button ID="btnDisable" cid="btnDisable" runat="server" Text="aDisable" OnClick="btnDisable_Click" />
                                    </div>
                                </div>
                            </td>
                            <td style="text-align: right;">
                                <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20" CssClass="AspNetPager"
                                    OnPageChanged="AspNetPager1_PageChanged" UrlPageIndexName="PageIndex" UrlPaging="false" FirstPageText="<<" LastPageText=">>"
                                    NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                                    CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                                </webdiyer:AspNetPager>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                
                                
                                <div id="divDivision" class="ColorGrid" style="width: 760px;">
                                    <asp:GridView ID="gvLoanProgramses" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
                                        EmptyDataText="There is no data in database." CellPadding="3" GridLines="None">
                                        <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                        <AlternatingRowStyle CssClass="EvenRow" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn"  HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                <HeaderTemplate>
                                                    <input type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="checkbox" tag='<%# Eval("LoanProgramID") %>:<%# Eval("InvestorID") %>' />
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <input type="checkbox" tag='<%# Eval("LoanProgramID") %>:<%# Eval("InvestorID") %>' />
                                                </AlternatingItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Investor" HeaderStyle-CssClass="CheckBoxHeader" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                   <a href="../Contact/PartnerCompanyEdit.aspx?CompanyID=<%# Eval("InvestorID") %>" target="_blank" > <%# Eval("Name")%> </a>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <a href="../Contact/PartnerCompanyEdit.aspx?CompanyID=<%# Eval("InvestorID") %>" target="_blank" > <%# Eval("Name")%> </a>
                                                </AlternatingItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Loan Program" HeaderStyle-CssClass="CheckBoxHeader" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                   <a href="javascript:popLoanProgramSetup(<%# Eval("LoanProgramID") %>,<%# Eval("InvestorID") %>)" > <%# Eval("LoanProgram") %> </a>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <a href="javascript:popLoanProgramSetup(<%# Eval("LoanProgramID") %>,<%# Eval("InvestorID") %>)" > <%# Eval("LoanProgram") %> </a>
                                                </AlternatingItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Index" HeaderStyle-CssClass="CheckBoxHeader" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%# Eval("IndexType")%>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                   <%# Eval("IndexType")%>
                                                </AlternatingItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Margin" HeaderStyle-CssClass="CheckBoxHeader" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# Eval("Margin") != DBNull.Value ? Convert.ToDouble(Eval("Margin")).ToString("0.000") + "%" : ""%>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <%# Eval("Margin") != DBNull.Value ? Convert.ToDouble(Eval("Margin")).ToString("0.000") + "%" : ""%>
                                                </AlternatingItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="1stAdj" HeaderStyle-CssClass="CheckBoxHeader" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# Eval("FirstAdj") != DBNull.Value ? Convert.ToDouble(Eval("FirstAdj")).ToString("0.000") + "%" : ""%>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <%# Eval("FirstAdj") != DBNull.Value ? Convert.ToDouble(Eval("FirstAdj")).ToString("0.000") + "%" : ""%>
                                                </AlternatingItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Sub Adj" HeaderStyle-CssClass="CheckBoxHeader" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# Eval("SubAdj") != DBNull.Value ? Convert.ToDouble(Eval("SubAdj")).ToString("0.000") + "%" : ""%>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <%# Eval("SubAdj") != DBNull.Value ? Convert.ToDouble(Eval("SubAdj")).ToString("0.000") + "%" : ""%>
                                                </AlternatingItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Lifetime" HeaderStyle-CssClass="CheckBoxHeader" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                   <%# Eval("LifetimeCap") != DBNull.Value ? Convert.ToDouble(Eval("LifetimeCap")).ToString("0.000") + "%" : ""%>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                   <%# Eval("LifetimeCap") != DBNull.Value ? Convert.ToDouble(Eval("LifetimeCap")).ToString("0.000")+"%": ""%>
                                                </AlternatingItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Enabled" HeaderStyle-CssClass="CheckBoxHeader" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# (Eval("Enabled") != DBNull.Value && Convert.ToBoolean(Eval("Enabled"))) ? "Yes" : "No"%>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                     <%# (Eval("Enabled") != DBNull.Value && Convert.ToBoolean(Eval("Enabled"))) ? "Yes" : "No"%>
                                                </AlternatingItemTemplate>
                                            </asp:TemplateField>


                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td height="20px" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <!--<tr>
                            <td height="20" colspan="2">
                                <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="Btn-66" OnClick="btnRemove_Click" CausesValidation="false" />
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClick="btnSave_Click"  CausesValidation="false"  Visible="false"/><asp:Button
                                    ID="btnCancel" runat="server" Text="Cancel" CssClass="Btn-66" OnClick="btnCancel_Click" CausesValidation="false" />
                            </td>
                        </tr>-->
                    </table>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfDeleteItems" runat="server" />
    <iframe  id="upfileframe" name="upfileframe" src="about:blank" height="0" width="0" ></iframe>
    
</asp:Content>
