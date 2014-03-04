<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartnerCompanyEdit.aspx.cs" Inherits="Contact_PartnerCompanyEdit" MasterPageFile="~/_layouts/LPWeb/MasterPage/Contact.master" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <!CDATA[
    
        var CompanyNameClientId = "#<%= txtCompanyName.ClientID %>";
        var sHasCreate = "<%=sHasCreate %>";
        var sHasModify = "<%=sHasModify %>";
        var sHasAddBranch = "<%=sHasAddBranch %>";
        var sHasRemoveBranch = "<%=sHasRemoveBranch %>"; 

        $.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            var check = false;
            var re = new RegExp(regexp);
            return this.optional(element) || re.test(value);
            },
            "Please enter valid url."
        );

        $(document).ready(function () {

            AddValidators();

            InitSearchInput();

            if (sHasCreate == "0") {
                DisableLink("aCreateBranch");
            }
            if (sHasModify == "0") {
                DisableLink("aUpdateBranch");
            }
            if (sHasAddBranch == "0") {
                DisableLink("aAdd");
            }
            if (sHasRemoveBranch == "0") {
                DisableLink("aRemove");
            }
        });

        // add jQuery Validators
        function AddValidators() {

            $("#aspnetForm").validate({

                rules: {

                    ctl00$ctl00$PlaceHolderMain$MainArea$txtCompanyName: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$ddlServiceType: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$txtAddress: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$txtCity: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$ddlState: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$txtZip: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbWebsite: {
                        regex: /^(https?|ftp):\/\/(((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/
                    }
                },
                messages: {

                    ctl00$ctl00$PlaceHolderMain$MainArea$txtCompanyName: {
                        required: "Please enter Company Name."
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$ddlServiceType: {
                        required: "*"
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$txtAddress: {
                        required: "Please enter Address."
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$txtCity: {
                        required: "*"
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$ddlState: {
                        required: "*"
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$txtZip: {
                        required: "*"
                    }
                }
            });
        }

        function InitSearchInput() {

            // Alphabet
            var Alphabet = GetQueryString1("Alphabet");
            if (Alphabet != "") {

                $("#ddlAlphabet").val(Alphabet);
            }

        }

        function ddlAlphabet_onchange() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            // 参数字符串
            var sQueryStrings = "?sid=" + RadomStr + "&CompanyID=" + GetQueryString1("CompanyID");

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

            window.location.href = window.location.pathname + sQueryStrings;
        }

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox").attr("checked", "");
            }
        }

        function ShowDialog_AddBranch() {

            var CompanyID = GetQueryString1("CompanyID");
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "SelectPartnerBranch.aspx?sid=" + RadomStr + "&Action=Add&CompanyID=" + CompanyID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 400
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 500;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Select Partner Branch", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function ShowDialog_RemoveBranch() {

            var CompanyID = GetQueryString1("CompanyID");
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "SelectPartnerBranch.aspx?sid=" + RadomStr + "&Action=Remove&CompanyID=" + CompanyID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 400
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 500;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Select Partner Branch", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function BeforeDelete() {

            var result = confirm("Deleting a partner company will also delete its branches and contacts. \r\n\r\nAre you sure you want to continue?");
            if (result == false) {

                return false;
            }

            return true;
        }

        function aCreateBranch_onclick() {
            
            var CompanyName = $(CompanyNameClientId).val();          
            window.location.href = "PartnerBranchSetup.aspx?CompanyName=" + CompanyName;
        }

        function aUpdateBranch_onclick() {

            var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr:not(:first) td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("No partner branch was selected.");
                return;
            }
            else if (SelCount > 1) {

                alert("Only one partner branch can be selected.");
                return;
            }

            var BranchID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr:not(:first) td :checkbox:checked").attr("BranchID");

            GoToUpdateContactBranch(BranchID);
        }

        function GoToUpdateContactBranch(BranchId) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            window.location.href = "PartnerBranchEdit.aspx?sid=" + RadomStr + "&ContactBranchId=" + BranchId;
        }

        function BeforeDisableBranch() {

            var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr:not(:first) td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("No partner branch was selected.");
                return false;
            }
            else if (SelCount > 1) {

                alert("Only one partner branch can be selected.");
                return false;
            }

            var result = confirm("Disabling a partner branch will also disable its contacts. \r\n\r\nAre you sure you want to continue?");
            if (result == false) {

                return false;
            }

            var SelBranchID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr:not(:first) td :checkbox:checked").attr("BranchID");

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnSelectedBranchID").val(SelBranchID);

            return true;
        }

        function BeforeDeleteBranch() {

            var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr:not(:first) td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("No partner branch was selected.");
                return false;
            }
            else if (SelCount > 1) {

                alert("Only one partner branch can be selected.");
                return false;
            }

            var result = confirm("Deleting a partner branch will also delete its contacts. \r\n\r\nAre you sure you want to continue?");
            if (result == false) {

                return false;
            }

            var SelBranchID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr:not(:first) td :checkbox:checked").attr("BranchID");

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnSelectedBranchID").val(SelBranchID);

            return true;
        }

        function BeforeSave() {

            var IsValid = $("#aspnetForm").valid();

            return IsValid

        }

        function LinkContacts(ContactBranchId) {
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            window.location.href = "PartnerContactsPage.aspx?sid=" + RadomStr + "&BranchID=" + ContactBranchId;
        }

// ]]>
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divContainer" style="border: solid 0px red;">
        <div id="divModuleName" class="Heading">Partner Company Setup</div>
        <div class="SplitLine"></div>
        <div id="divCompanyDetails" style="margin-top: 10px;">
            <table>
                <tr>
                    <td style="width: 90px;">Company Name:</td>
                    <td>
                        <asp:TextBox ID="txtCompanyName" runat="server" Width="300px" MaxLength="255"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 90px;">Service Type:</td>
                    <td style="width: 240px;">
                        <asp:DropDownList ID="ddlServiceType" runat="server" Width="200px" DataValueField="ServiceTypeId" DataTextField="Name">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkEnabled" runat="server" />
                                </td>
                                <td style="padding-left:5px;"><label for="chkEnabled">Enabled</label></td>
                            </tr>
                        </table>
                    
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 90px;">Address:</td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" Width="300px" MaxLength="255"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 90px;">City:</td>
                    <td style="width: 160px;">
                        <asp:TextBox ID="txtCity" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="width: 40px;">State:</td>
                    <td style="width: 90px;">
                        <asp:DropDownList ID="ddlState" runat="server" Width="50">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="AL" Value="AL"></asp:ListItem>
                            <asp:ListItem Text="AK" Value="AK"></asp:ListItem>
                            <asp:ListItem Text="AZ" Value="AZ"></asp:ListItem>
                            <asp:ListItem Text="AR" Value="AR"></asp:ListItem>
                            <asp:ListItem Text="CA" Value="CA"></asp:ListItem>
                            <asp:ListItem Text="CO" Value="CO"></asp:ListItem>
                            <asp:ListItem Text="CT" Value="CT"></asp:ListItem>
                            <asp:ListItem Text="DC" Value="DC"></asp:ListItem>
                            <asp:ListItem Text="DE" Value="DE"></asp:ListItem>
                            <asp:ListItem Text="FL" Value="FL"></asp:ListItem>
                            <asp:ListItem Text="GA" Value="GA"></asp:ListItem>
                            <asp:ListItem Text="HI" Value="HI"></asp:ListItem>
                            <asp:ListItem Text="ID" Value="ID"></asp:ListItem>
                            <asp:ListItem Text="IL" Value="IL"></asp:ListItem>
                            <asp:ListItem Text="IN" Value="IN"></asp:ListItem>
                            <asp:ListItem Text="IA" Value="IA"></asp:ListItem>
                            <asp:ListItem Text="KS" Value="KS"></asp:ListItem>
                            <asp:ListItem Text="KY" Value="KY"></asp:ListItem>
                            <asp:ListItem Text="LA" Value="LA"></asp:ListItem>
                            <asp:ListItem Text="ME" Value="ME"></asp:ListItem>
                            <asp:ListItem Text="MD" Value="MD"></asp:ListItem>
                            <asp:ListItem Text="MA" Value="MA"></asp:ListItem>
                            <asp:ListItem Text="MI" Value="MI"></asp:ListItem>
                            <asp:ListItem Text="MN" Value="MN"></asp:ListItem>
                            <asp:ListItem Text="MS" Value="MS"></asp:ListItem>
                            <asp:ListItem Text="MO" Value="MO"></asp:ListItem>
                            <asp:ListItem Text="MT" Value="MT"></asp:ListItem>
                            <asp:ListItem Text="NE" Value="NE"></asp:ListItem>
                            <asp:ListItem Text="NV" Value="NV"></asp:ListItem>
                            <asp:ListItem Text="NH" Value="NH"></asp:ListItem>
                            <asp:ListItem Text="NJ" Value="NJ"></asp:ListItem>
                            <asp:ListItem Text="NM" Value="NM"></asp:ListItem>
                            <asp:ListItem Text="NY" Value="NY"></asp:ListItem>
                            <asp:ListItem Text="NC" Value="NC"></asp:ListItem>
                            <asp:ListItem Text="ND" Value="ND"></asp:ListItem>
                            <asp:ListItem Text="OH" Value="OH"></asp:ListItem>
                            <asp:ListItem Text="OK" Value="OK"></asp:ListItem>
                            <asp:ListItem Text="OR" Value="OR"></asp:ListItem>
                            <asp:ListItem Text="PA" Value="PA"></asp:ListItem>
                            <asp:ListItem Text="PR" Value="PR"></asp:ListItem>
                            <asp:ListItem Text="RI" Value="RI"></asp:ListItem>
                            <asp:ListItem Text="SC" Value="SC"></asp:ListItem>
                            <asp:ListItem Text="SD" Value="SD"></asp:ListItem>
                            <asp:ListItem Text="TN" Value="TN"></asp:ListItem>
                            <asp:ListItem Text="TX" Value="TX"></asp:ListItem>
                            <asp:ListItem Text="UT" Value="UT"></asp:ListItem>
                            <asp:ListItem Text="VT" Value="VT"></asp:ListItem>
                            <asp:ListItem Text="VA" Value="VA"></asp:ListItem>
                            <asp:ListItem Text="WA" Value="WA"></asp:ListItem>
                            <asp:ListItem Text="WV" Value="WV"></asp:ListItem>
                            <asp:ListItem Text="WI" Value="WI"></asp:ListItem>
                            <asp:ListItem Text="WY" Value="WY"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>Zip:</td>
                    <td>
                        <asp:TextBox ID="txtZip" runat="server" Width="80px" MaxLength="12"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 90px;">
                        Website:
                    </td>
                    <td>
                        <asp:TextBox ID="tbWebsite" runat="server" Width="300px" MaxLength="255"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="margin-top: 10px;">
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave()" onclick="btnSave_Click" />
                    </td>
                   <td>
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="Btn-66" OnClientClick="return BeforeDelete();" onclick="btnDelete_Click" />
                   </td>
                </tr>
            </table>

        </div>

        <div id="divToolBar" style="width: 700px; margin-top: 10px;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 50px;">
                        <select id="ddlAlphabet" style="width: 40px;" onchange="ddlAlphabet_onchange()">
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
                    <td>
                        <ul class="ToolStrip" style="margin-left: 0px;">
                            <li><a id="aCreateBranch" href="javascript:aCreateBranch_onclick()">Create</a><span>|</span></li>
                            <li><a id="aUpdateBranch" href="javascript:aUpdateBranch_onclick()">Update</a><span>|</span></li>
                            <li><asp:LinkButton ID="lnkDisableBranch" runat="server" OnClientClick="return BeforeDisableBranch()" OnClick="lnkDisableBranch_Click">Disable</asp:LinkButton><span>|</span></li>
                            <li><asp:LinkButton ID="lnkDeleteBranch" runat="server" OnClientClick="return BeforeDeleteBranch()" OnClick="lnkDeleteBranch_Click">Delete</asp:LinkButton><span>|</span></li>
                            <li><a id="aAdd" href="javascript:ShowDialog_AddBranch()">Add</a><span>|</span></li>
                            <li><a id="aRemove" href="javascript:ShowDialog_RemoveBranch()">Remove</a></li>
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

        <div id="divBranchList" class="ColorGrid" style="margin-top: 5px; width: 700px;">
            <asp:GridView ID="gridBranchList" runat="server" DataSourceID="BranchSqlDataSource" AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input id="chkChecked" type="checkbox" BranchID="<%# Eval("ContactBranchId") %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Branch">
                        <ItemTemplate>
                            <a href="javascript:GoToUpdateContactBranch('<%# Eval("ContactBranchId")%>')"><%# Eval("Name")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="State" HeaderText="State" ItemStyle-Width="80px" />
                    <asp:BoundField DataField="City" HeaderText="City" ItemStyle-Width="150px" />
                    <asp:BoundField DataField="Enabled" HeaderText="Enabled" ItemStyle-Width="80px" />
                    <asp:TemplateField HeaderText="Contacts" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <a href="javascript:LinkContacts('<%# Eval("ContactBranchId") %>')"><%# Eval("Contacts")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
        <asp:SqlDataSource ID="BranchSqlDataSource" runat="server" SelectCommand="lpsp_ExecSqlByPager" SelectCommandType="StoredProcedure" DataSourceMode="DataReader">
            <SelectParameters>
                <asp:Parameter Name="OrderByField" Type="String" DefaultValue="Name" />
                <asp:Parameter Name="AscOrDesc" Type="String" DefaultValue="asc" />
                <asp:Parameter Name="Fields" Type="String" DefaultValue="*" />
                <asp:Parameter Name="DbTable" Type="String" DefaultValue="(select *, case when ISNULL(Enabled,1)=1 then 'Yes' else 'No' end as BranchEnabled, (select count(1) from Contacts as a where a.ContactBranchId=b.ContactBranchId) as Contacts from ContactBranches as b) t" />
                <asp:Parameter Name="Where" Type="String" DefaultValue="" ConvertEmptyStringToNull="False" />
                <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="1" Name="StartIndex" PropertyName="StartRecordIndex" Type="Int32" />
                <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="15" Name="EndIndex" PropertyName="EndRecordIndex" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <asp:HiddenField ID="hdnSelectedBranchID" runat="server" />
</asp:Content>
