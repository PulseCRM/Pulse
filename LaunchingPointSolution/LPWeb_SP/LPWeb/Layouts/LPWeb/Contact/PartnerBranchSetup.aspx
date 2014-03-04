<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartnerBranchSetup.aspx.cs" Inherits="Contact_PartnerBranchSetup" MasterPageFile="~/_layouts/LPWeb/MasterPage/Contact.master"%>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/jqcontextmenu.css" />
    <script type="text/javascript" src="../js/jqcontextmenu.js"></script>

            <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery.formatCurrency.js" type="text/javascript"></script>

    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.alphanumeric.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        // <!CDATA[

        var sHasCreate = "<%=sHasCreate %>";
        var sHasModify = "<%=sHasModify %>";
        var sHasDelete = "<%=sHasDelete %>";
        var sHasAssign = "<%=sHasAssign %>";
        var sHasMerge = "<%=sHasMerge %>";

        jQuery(document).ready(function ($) {

            $('#aMailChimp').addcontextmenu('divMailChimpMenu') //apply context menu to all images on the page 
        })

        $(document).ready(function () {

            AddValidators();

            if (sHasCreate == "0") {
                DisableLink("aCreate");
            }
            if (sHasModify == "0") {
                DisableLink("aRemove");
            }
            if (sHasDelete == "0") {
                DisableLink("aEnable");
            }
            if (sHasAssign == "0") {
                DisableLink("aAssign");
            }
            if (sHasMerge == "0") {
                DisableLink("aMerge");
            }

            //txtBizPhone
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_tbPhone").mask("(999) 999-999?9");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_tbFax").mask("(999) 999-999?9");
        });

        // add jQuery Validators
        function AddValidators() {

            $("#aspnetForm").validate({

                rules: {

                    ctl00$ctl00$PlaceHolderMain$MainArea$tbBranch: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbAddress: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbCity: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$ddlStates: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbZip: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbPhone: {
                        required: true
                    }
                },
                messages: {

                    ctl00$ctl00$PlaceHolderMain$MainArea$tbBranch: {
                        required: "*"
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbAddress: {
                        required: "*"
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbCity: {
                        required: "*"
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$ddlStates: {
                        required: "*"
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbZip: {
                        required: "*"
                    },
                     ctl00$ctl00$PlaceHolderMain$MainArea$tbPhone: {
                         required: "*"
                    }
                }
            });
        }
       

        function btnFilter_onclick() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            // 参数字符串
            var sQueryStrings = "";

            // Alphabet
            var Alphabet = $("#ddlAlphabet").val();
            if (Alphabet != "") {

                sQueryStrings += "&Alphabet=" + encodeURIComponent(Alphabet);
            }


            // PageIndex
            var PageIndex = GetQueryString1("PageIndex");
            if (PageIndex != "" && PageIndex != "1") {

                sQueryStrings += "&PageIndex=" + PageIndex;
            }

            if (sQueryStrings == "") {

                window.location.href = window.location.pathname;
            }
            else {

                window.location.href = window.location.pathname + "?sid=" + RadomStr + sQueryStrings;
            }
        }

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox").attr("checked", "");
            }
        }

        function aCreate_onclick() {

            window.location.href = "PartnerBranchSetup.aspx";
        }

        function aUpdate_onclick() {

            var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("No partner branch was selected.");
                return;
            }
            else if (SelCount > 1) {

                alert("Only one partner branch can be selected.");
                return;
            }

            var BranchID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox:checked").attr("BranchID");

            GoToUpdateContactBranch(BranchID);
        }

        function GoToUpdateContactBranch(BranchId) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            window.location.href = "PartnerBranchEdit.aspx?sid=" + RadomStr + "&ContactBranchId=" + BranchId;
        }

        //#region before save
        function DoValidate() {
//            //  CheckDuplication();
//            var BranchName = $.trim($("#ctl00_ctl00_PlaceHolderMain_MainArea_tbBranch").val());
//            var Company = $.trim($("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlCompany").val());
//            if (BranchName == "") {
//                alert("Plesae enter branch name.");
//                return false;
//            }

//            if (Company == "0") {
//                alert("Plesae select a company.");
//                return false;
//            }

//            // Ajax
//            $.getJSON("CheckBranchNameDuplicate.ashx?sid=" + Radom + "&branchID=0&name=" + encodeURIComponent(BranchName), AfterCheckDuplication);

//            return true;
        }

        function CheckDuplication() {

            //#region check duplication of workflow template name

            var BranchName = $.trim($("#ctl00_ctl00_PlaceHolderMain_MainArea_tbBranch").val());
//            if (BranchName.trim() == "") {
//                alert("Plesae enter branch name.");
//                return;
//            }
            
            // show waiting dialog
            ShowWaitingDialog("Checking duplicates...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // Ajax
            $.getJSON("CheckBranchNameDuplicate.ashx?sid=" + Radom + "&branchID=0&name=" + encodeURIComponent(BranchName), AfterCheckDuplication);

            //#endregion
        }

        function AfterCheckDuplication(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    $("#divContainer").unblock();
                    return false;
                }
            }, 2000);
        }
        //#endregion

        function ShowWaitingDialog(WaitingMsg) {

            $("#WaitingMsg").text(WaitingMsg);
            $("#divContainer").block({ message: $('#divWaiting'), css: { width: '450px'} });
        }

        function aAdd_onclick() {

            alert("Please save branch info at first.");
            return;
        }

        function AlertNoContactSelected() {

            alert("No contact was selected.");
            return;
        }

        function BeforeSave() {

            var IsValid = $("#aspnetForm").valid();

            return IsValid

        }

        function AlertNoContactSelected2() {

            alert("Please select a contact from the list.");
            return;
        }

        // ]]>
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divContainer">
        <div id="divModuleName" class="Heading">Partner Branch Setup</div>
        <div class="SplitLine"></div>
        <div id="divFilters" style="margin-top: 20px;">
            <table >
                <tr>
                    <td style="width: 120px; height:23px" align="left">
                       &nbsp;&nbsp;&nbsp;&nbsp;Company:
                    </td>
                    <td style="width: 420px;" colspan="4">
                        <asp:DropDownList ID="ddlCompany" runat="server" DataValueField="ContactCompanyId" DataTextField="Name" Width="400px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 110px;">
                       &nbsp;
                    </td>
                </tr>
                 <tr>
                    <td style="width: 120px; height:23px" align="left">
                       &nbsp;&nbsp;&nbsp;&nbsp;Branch Name:
                    </td>
                    <td style="width: 420px;" colspan="4">
                        <asp:TextBox ID="tbBranch" runat="server" style=" width:400px" ></asp:TextBox>
                    </td>
                    <td style="width: 110px;">
                        <asp:CheckBox ID="chkEnable" runat="server"  />&nbsp;&nbsp;Enabled
                    </td>
                </tr>
                 <tr>
                    <td style="width: 120px; height:23px" align="left">
                      &nbsp;&nbsp;&nbsp;&nbsp;Address:
                    </td>
                    <td style="width: 420px;" colspan="4">
                        <asp:TextBox ID="tbAddress" runat="server" style=" width:400px" ></asp:TextBox>
                    </td>
                    <td style="width: 100px;">
                        &nbsp;
                    </td>
                </tr>
                 <tr>
                    <td style="width: 120px; height:23px" align="left">
                      &nbsp;&nbsp;&nbsp;&nbsp;City:
                    </td>
                    <td style="width: 160px;">
                        <asp:TextBox ID="tbCity" runat="server" style=" width:140px" ></asp:TextBox>
                    </td>
                     <td style="width: 60px;" align="left">
                      &nbsp;&nbsp;State:
                    </td>
                    <td style="width: 110px;">
                        <asp:DropDownList ID="ddlStates" runat="server"  Width="80px">
                        </asp:DropDownList>
                    </td>
                     <td style="width: 60px;" align="left">
                      &nbsp;&nbsp;Zip:
                    </td>
                    <td style="width: 110px;">
                       <asp:TextBox ID="tbZip" runat="server" style=" width:80px" ></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td style="width: 120px; height:23px" align="left">
                      &nbsp;&nbsp;&nbsp;&nbsp;Phone:
                    </td>
                    <td style="width: 160px;">
                        <asp:TextBox ID="tbPhone" runat="server" style=" width:140px" ></asp:TextBox>
                    </td>
                     <td style="width: 60px;" align="left">
                      &nbsp;&nbsp;Fax:
                    </td>
                    <td style="width: 160px;" colspan="2">
                        <asp:TextBox ID="tbFax" runat="server" style=" width:150px" ></asp:TextBox>
                    </td>
                    <td style="width: 100px;">
                       &nbsp;
                    </td>
                </tr>
                 <tr>
                    <td style="width: 120px; height:23px" align="left">
                      &nbsp;&nbsp;&nbsp;&nbsp;Primary Contact:
                    </td>
                    <td style="width: 240px;" colspan="2">
                         <asp:DropDownList ID="ddlContact" runat="server" DataValueField="ContactId" DataTextField="Contact"  Width="230px">
                        </asp:DropDownList>
                    </td>
                     <td style="width: 80px;" align="left" colspan="3">
                      &nbsp;&nbsp;
                    </td>
                </tr>
                 <tr>
                    <td style="width: 120px; height:25px" align="right">
                      <asp:Button ID="btnSave" runat="server" Text ="Save" CssClass="Btn-66" OnClientClick="return BeforeSave()"  OnClick="btnSave_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td  colspan="5" align="left" >
                       <%--<asp:Button ID="btnDelete" runat="server" Text ="Delete" CssClass="Btn-66" OnClick="btnDelete_Click" />--%>
                    </td>
                </tr>

            </table>
        </div>
         <div id="divToolBar" style="width: 900px; margin-top: 20px;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 50px;">
                        <select id="ddlAlphabet" style="width: 40px;" onchange="btnFilter_onclick()">
                            <option value=''></option>
                            <option value="A">A</option>
                            <option value="B">B</option>
                            <option value="C">C</option>
                            <option value="D">D</option>
                            <option value="E">E</option>
                            <option value="F">F</option>
                            <option value="G">G</option>
                            <option value="H">H</option>
                            <option value="I">I</option>
                            <option value="J">J</option>
                            <option value="K">K</option>
                            <option value="L">L</option>
                            <option value="M">M</option>
                            <option value="N">N</option>
                            <option value="O">O</option>
                            <option value="P">P</option>
                            <option value="Q">Q</option>
                            <option value="R">R</option>
                            <option value="S">S</option>
                            <option value="T">T</option>
                            <option value="U">U</option>
                            <option value="V">V</option>
                            <option value="W">W</option>
                            <option value="X">X</option>
                            <option value="Y">Y</option>
                            <option value="Z">Z</option>
                        </select>
                    </td>
                    <td style="width: 650px;">
                        <ul class="ToolStrip" style="margin-left: 0px;">
                            <li><a id="aCreate" href="javascript:aCreate_onclick()">Create</a><span>|</span></li>
                            <li><a id="aRemove" href="javascript:AlertNoContactSelected()">Update</a><span>|</span></li>
                            <li><a id="aDisable" href="javascript:AlertNoContactSelected()">Disable</a><span>|</span></li>
                            <li><a id="aEnable" href="javascript:AlertNoContactSelected()">Delete</a><span>|</span></li>
                            <li><a id="a1" href="javascript:aAdd_onclick()">Add</a><span>|</span></li>
                            <li><a id="a2" href="javascript:AlertNoContactSelected()">Remove</a><span>|</span></li>
                            <li><a id="aAssign" href="javascript:AlertNoContactSelected()">Assign</a><span>|</span></li>
                            <li><a id="aMerge" href="javascript:AlertNoContactSelected()">Merge</a></li>
                            <li><span>|</span><a id="aMailChimp" href="#">Mail Chimp</a></li>
                        </ul>
                    </td>
                    <td style="text-align: right;">
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="15" CssClass="AspNetPager"
                            UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                            NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                            CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                        </webdiyer:AspNetPager>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divCompanyList" class="ColorGrid" style="margin-top: 5px; width: 700px;">
            <asp:GridView ID="gridContactList" runat="server" DataSourceID="BranchSqlDataSource" AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ckbSelected" runat="server" EnableViewState="true" BranchID='<%# Eval("ContactBranchId") %>'  />
                            </ItemTemplate>
                            <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                            <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contact" SortExpression="Contact">
                            <ItemTemplate>
                                <a href="javascript:LinkContact('<%# Eval("ContactId") %>')"><%# Eval("Contact")%></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phone" SortExpression="CellPhone">
                            <ItemTemplate>
                                <%# Eval("CellPhone")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Email" SortExpression="Email">
                            <ItemTemplate>
                                <%# Eval("Email")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Enabled" SortExpression="Enabled">
                            <ItemTemplate>
                                <%# Eval("Enabled")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User Access" SortExpression="UserAccess">
                            <ItemTemplate>
                                <a href="javascript:LinkUserAccess('<%# Eval("UserAccess") %>')"><%# Eval("UserAccess")%></a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                    </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
        <asp:SqlDataSource ID="BranchSqlDataSource" runat="server" SelectCommand="lpsp_ExecSqlByPager" SelectCommandType="StoredProcedure" DataSourceMode="DataReader">
            <SelectParameters>
                <asp:Parameter Name="OrderByField" Type="String" DefaultValue="Contact" />
                <asp:Parameter Name="AscOrDesc" Type="String" DefaultValue="asc" />
                <asp:Parameter Name="Fields" Type="String" DefaultValue="*" />
                <asp:Parameter Name="DbTable" Type="String" DefaultValue="(select ContactId,ISNULL(Lastname,'')+ (CASE ISNULL(Lastname,'') when '' then ' ' else ', ' end) +ISNULL(Firstname,'') as Contact,
Fax,Email,CellPhone,HomePhone,ContactBranchId,case when ISNULL([Enabled],1)=1 then 'Yes' else 'No' end as [Enabled],(select COUNT(UserId) from ContactUsers where ContactUsers.ContactId=ContactId) as UserAccess
from Contacts) t" />
                <asp:Parameter Name="Where" Type="String" DefaultValue="" ConvertEmptyStringToNull="False" />
                <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="1" Name="StartIndex" PropertyName="StartRecordIndex" Type="Int32" />
                <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="15" Name="EndIndex" PropertyName="EndRecordIndex" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
         <div id="divWaiting" style="display: none; padding: 5px;">
		    <table style="margin-left: auto; margin-right: auto;">
			    <tr>
				    <td>
					    <img id="imgWaiting" src="../images/waiting.gif" />
				    </td>
				    <td style="padding-left: 5px;">
					    <label id="WaitingMsg" style="color: #818892; font-weight: bold;"></label>
				    </td>
			    </tr>
		    </table>
	    </div>
         <div style="display: none;">
        <div id="dialogSelectContact" title="Select Contacts">
            <iframe id="iframeSelectContact" name="iframeSelectContact" frameborder="0" width="100%"
                height="100%"></iframe>
        </div>
    </div>
    </div>
    <ul id="divMailChimpMenu" class="jqcontextmenu">
        <li>
            <a href="aSubscribe" href="#" onclick="AlertNoContactSelected2(); return false;">Subscribe</a></li>
        <li>
            <a href="aUnsubscribe" onclick="AlertNoContactSelected2(); return false;">Unsubscribe</a></li>
    </ul>
</asp:Content>


