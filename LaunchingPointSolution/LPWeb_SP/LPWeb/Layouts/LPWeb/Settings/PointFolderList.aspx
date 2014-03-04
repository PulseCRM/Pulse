<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" AutoEventWireup="true"
    Inherits="LPWeb.Settings.Settings_PointFolderList" CodeBehind="PointFolderList.aspx.cs" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            InitSearchInput();
            InitToolBox();
            InitGrid();
        });
        // check/decheck all
        function CheckAll(CheckBox) {
            if (CheckBox.checked) {
                $("#<%= this.gridFolderList.ClientID %> tr td input[type=checkbox]:not(:disabled=disabled)").attr("checked", "true");
            }
            else {
                $("#<%= this.gridFolderList.ClientID %> tr td input[type=checkbox]").attr("checked", "");
            }
        }
        // init toolbox
        function InitToolBox() {

            $("#aView").attr("href", "javascript:ViewFolder()");

            $("#aSuspendSync").attr("href", "javascript:ViewFolder()");
        }

        function InitGrid() {

            $("#<%= this.gridFolderList.ClientID %> tr:not(:first)").mousedown(function () {

                $("#<%= this.gridFolderList.ClientID %> tr").attr("class", "NormalRow");

                $(this).attr("class", "FocusedRow");
            });
        }

        function InitSearchInput() {

            // Region
            var Region = GetQueryString1("Region");
            if (Region != "") {
                $("#<%= this.ddlRegion.ClientID %>").val(Region);
            }

            // Division
            var Division = GetQueryString1("Division");
            if (Division != "") {
                $("#<%= this.ddlDivision.ClientID %>").val(Division);
            }

            // Branch
            var Branch = GetQueryString1("Branch");
            if (Branch != "") {
                $("#<%= this.ddlBranch.ClientID %>").val(Branch);
            }

            // Alphabet
            var Alphabet = GetQueryString1("Alphabet");
            if (Alphabet != "") {
                $("#ddlAlphabet").val(Alphabet);
            }
        }

        function btnFilter_onclick() {

            var sQueryStrings = BuildQueryString1();
            if (sQueryStrings != "") {

                var sPageIndex = GetQueryString1("PageIndex");

                if (sPageIndex == "") {
                    sQueryStrings += "&PageIndex=1";
                }
                else {
                    sQueryStrings += "&PageIndex=" + sPageIndex;
                }

                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString().substr(2);
                window.location.href = window.location.pathname + "?sid=" + RadomStr + sQueryStrings;
            }
            else {

                window.location.href = window.location.pathname
            }
        }

        function ddlAlphabet_onchange() {

            var sQueryStrings = BuildQueryString1();

            // Alphabet
            var Alphabet = $.trim($("#ddlAlphabet").val());
            if (Alphabet != "") {
                sQueryStrings += "&Alphabet=" + encodeURIComponent(Alphabet);
            }

            if (sQueryStrings != "") {

                var sPageIndex = GetQueryString1("PageIndex");

                if (sPageIndex == "") {
                    sQueryStrings += "&PageIndex=1";
                }
                else {
                    sQueryStrings += "&PageIndex=" + sPageIndex;
                }

                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString().substr(2);
                window.location.href = window.location.pathname + "?sid=" + RadomStr + sQueryStrings;
            }
            else {

                window.location.href = window.location.pathname
            }
        }

        function BuildQueryString1() {

            var sQueryStrings = "";

            // Region
            var Region = $.trim($("#<%= this.ddlRegion.ClientID %>").val());
            if (Region != "All Regions") {
                sQueryStrings += "&Region=" + encodeURIComponent(Region);
            }

            // Division
            var Division = $.trim($("#<%= this.ddlDivision.ClientID %>").val());
            if (Division != "All Divisions") {
                sQueryStrings += "&Division=" + encodeURIComponent(Division);
            }

            // Branch
            var Branch = $.trim($("#<%= this.ddlBranch.ClientID %>").val());
            if (Branch != "All Branches") {
                sQueryStrings += "&Branch=" + encodeURIComponent(Branch);
            }

            return sQueryStrings;
        }

        // View the selected folder info
        function ViewFolder() {

            var gridFolderList;
            gridFolderList = $("table[id=<%= this.gridFolderList.ClientID %>]");
            var FocusedRowCell = gridFolderList.find("tr[class='FocusedRow']").children()[0];


            if (typeof (FocusedRowCell) == "undefined" || FocusedRowCell == null) {
                alert("You have to select a point folder first.");
                return;
            }
            var ActivateCode = gridFolderList.find("tr[class='FocusedRow'] td:first input[type='hidden'][id='hdFolderID']").val();

            // redirection
            window.location.href = "PointFolderDetails.aspx?FromPage=" + encodeURIComponent(window.location.href) + "&PointFolderID=" + encodeURIComponent(ActivateCode);
        }

        function send_request(sType) {
            //XmlHttpRequest
            //New XmlHttpRequest object

            http_request = false;
//            http_request = new XmlHttpRequest();
            if (window.XmlHttpRequest) {
                //not IE
                http_request = new XMLHttpRequest();
                if (http_request.overrideMimeType) {
                    http_request.overrideMimeType('text/xml');
                }

            }
            else if (window.ActiveXObject) {
                //IE
                try {
                    //IE7 or IE8
                    http_request = new ActiveXObject("Msxml2.XMLHTTP");
                }
                catch (e) {
                    try {
                        http_request = new ActiveXObject("Mircosoft.XMLHTTP");
                    }
                    catch (e)
                { }
                }
            }
            else {
//                window.alert("Internet Explorer error, please upgrade！");
                //                return false;
                http_request = new XMLHttpRequest();
                if (http_request.overrideMimeType) {
                    http_request.overrideMimeType('text/xml');
                }
            }
            //readyState: 4: complete
            //Create http request
            //open URL需加上：

             
            if (!http_request) {
                alert('Giving up :( Cannot create an XMLHTTP instance. Your browser may be too old for this application.');
                return false;
            }


            if (sType == "Region") {
                //onreadystatechange      
                http_request.onreadystatechange = GetDivisionBranch;

                var RegionID = $.trim($("#<%= this.ddlRegion.ClientID %>").val());
                http_request.open("get", "PointFolderList_GetDivision.ashx?RegionID=" + RegionID + "&Type=" + sType);
            }
            else if (sType == "Division") {
                //onreadystatechange      
                http_request.onreadystatechange = GetBranch;

                var DivisionID = $.trim($("#<%= this.ddlDivision.ClientID %>").val());
                http_request.open("get", "PointFolderList_GetDivision.ashx?DivisionID=" + DivisionID + "&Type=" + sType);
            }
            //send http request
            http_request.send(null);
        }

        //Region select change get division and branch
        function GetDivisionBranch() {

            document.getElementById("<%= this.ddlDivision.ClientID %>").length = 0;
            document.getElementById("<%= this.ddlBranch.ClientID %>").length = 0;
            if (http_request.readyState == 4)//send succ
            {
                if (http_request.status == 200) {
                    if (http_request.responseText != "") {

                        var ResponseText = http_request.responseText;

                        //Split division and branch
                        var arrDivisionBranch = ResponseText.split("@");
                        var sDivision = arrDivisionBranch[0];
                        var sBranch = arrDivisionBranch[1];
                        if (sDivision != "") {
                            var pos = sDivision.indexOf(";");
                            var i = 0;
                            while (pos != -1) {
                                var myText = sDivision.substring(0, pos);
                                if (myText != "") {
                                    //Split ID and Name 
                                    var myArr = myText.split("|");
                                    document.getElementById("<%= this.ddlDivision.ClientID %>").options[i] = new Option(myArr[1], myArr[0]);
                                    i++;
                                }
                                sDivision = sDivision.substr(pos + 1);
                                pos = sDivision.indexOf(";");
                                continue;
                            }
                        }
                        else {
                            document.getElementById("<%= this.ddlDivision.ClientID %>").options[0] = new Option("All Divisions", "0");
                            document.getElementById("<%= this.ddlDivision.ClientID %>").length = 1;
                        }

                        if (sBranch != "") {
                            var pos = sBranch.indexOf(";");
                            var i = 0;
                            while (pos != -1) {
                                var myText = sBranch.substring(0, pos);
                                if (myText != "") {
                                    //Split ID and Name 
                                    var myArr = myText.split("|");
                                    document.getElementById("<%= this.ddlBranch.ClientID %>").options[i] = new Option(myArr[1], myArr[0]);
                                    i++;
                                }
                                sBranch = sBranch.substr(pos + 1);
                                pos = sBranch.indexOf(";");
                                continue;
                            }
                        }
                        else {
                            document.getElementById("<%= this.ddlBranch.ClientID %>").options[0] = new Option("All Branches", "0");
                            document.getElementById("<%= this.ddlBranch.ClientID %>").length = 1;
                        }
                    }
                    else {
                        document.getElementById("<%= this.ddlDivision.ClientID %>").options[0] = new Option("All Divisions", "0");
                        document.getElementById("<%= this.ddlDivision.ClientID %>").length = 1;
                        document.getElementById("<%= this.ddlBranch.ClientID %>").options[0] = new Option("All Branches", "0");
                        document.getElementById("<%= this.ddlBranch.ClientID %>").length = 1;
                    }
                }
            }
        }

        function BeforeSync() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridFolderList tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return false;
            }
            var FolderIDs = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridFolderList tr td :checkbox[checked=true]").each(function (i) {
                var FolderID = $(this).attr("title");
                if (i == 0) {
                    FolderIDs = FolderID;
                }
                else {
                    FolderIDs += "," + FolderID;
                }
            });

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnFolderPaths").val(FolderIDs);
            return true;
        }
        //Division select changed, get branch
        function GetBranch() {

            document.getElementById("<%= this.ddlBranch.ClientID %>").length = 0;
            if (http_request.readyState == 4)//send succ
            {
                if (http_request.status == 200) {
                    if (http_request.responseText != "") {

                        var ResponseText = http_request.responseText;

                        //Split branch
                        var pos = ResponseText.indexOf(";");
                        var i = 0;
                        while (pos != -1) {
                            var myText = ResponseText.substring(0, pos);
                            if (myText != "") {
                                //Split ID and Name 
                                var myArr = myText.split("|");
                                document.getElementById("<%= this.ddlBranch.ClientID %>").options[i] = new Option(myArr[1], myArr[0]);
                                i++;
                            }
                            ResponseText = ResponseText.substr(pos + 1);
                            pos = ResponseText.indexOf(";");
                            continue;
                        }
                    }
                    else {
                        document.getElementById("<%= this.ddlBranch.ClientID %>").options[0] = new Option("All Branches", "0");
                        document.getElementById("<%= this.ddlBranch.ClientID %>").length = 1;
                    }
                }
            }
        }
// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="ModuleTitle" style="padding-left: 10px">
        Point Folder List View</div>
    <div class="SplitLine">
    </div>
    <div style="padding-left: 10px; padding-right: 10px;">
        <div style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlRegion" runat="server" DataTextField="Name" DataValueField="RegionId"
                            Width="128px">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlDivision" runat="server" DataTextField="Name" DataValueField="DivisionId"
                            Width="128px">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlBranch" runat="server" DataTextField="Name" DataValueField="BranchId"
                            Width="128px">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <input id="btnFilter" type="button" value="Filter" class="Btn-66" onclick="return btnFilter_onclick()" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divToolBar" style="margin-top: 13px;">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 770px;">
                <tr>
                    <td style="width: 40px;">
                        <select id="ddlAlphabet" onchange="return ddlAlphabet_onchange()">
                            <option></option>
                            <option>A</option>
                            <option>B</option>
                            <option>C</option>
                            <option>D</option>
                            <option>E</option>
                            <option>F</option>
                            <option>G</option>
                            <option>H</option>
                            <option>I</option>
                            <option>J</option>
                            <option>K</option>
                            <option>L</option>
                            <option>M</option>
                            <option>N</option>
                            <option>O</option>
                            <option>P</option>
                            <option>Q</option>
                            <option>R</option>
                            <option>S</option>
                            <option>T</option>
                            <option>U</option>
                            <option>V</option>
                            <option>W</option>
                            <option>X</option>
                            <option>Y</option>
                            <option>Z</option>
                        </select>
                    </td>
                    <td style="width: 350px;">
                        <ul class="ToolStrip">
                            <li><a id="aView">View Details </a><span>|</span></li>
                            <li>
                                <asp:LinkButton ID="lbtnSync" Text="Sync Now" runat="server" OnClientClick="return BeforeSync() ;"
                                    OnClick="lbtnSync_Click">Sync Now</asp:LinkButton><span>|</span></li>
                            <li><asp:LinkButton ID="lbtnSuspend" Text="Suspend Sync" runat="server" 
                                    OnClick="lbtnSuspend_Click">Suspend Sync</asp:LinkButton><span></li>
                        </ul>
                    </td>
                    <td style="text-align: right;">
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20" CssClass="AspNetPager"
                            UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                            NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                            CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                        </webdiyer:AspNetPager>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divFolderList" class="ColorGrid" style="width: 780px; margin-top: 5px;">
            <asp:GridView ID="gridFolderList" runat="server" DataSourceID="FolderSqlDataSource" OnSorting="gridFolderList_Sorting"
                DataKeyNames="FolderId" AllowSorting="true" EmptyDataText="There is no point folder in database." 
                AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input id="Checkbox2" type="checkbox" title="<%# Eval("Path") %>" <%# Eval("EnabledCheckBox").ToString()=="False"?"disabled='disabled'":"" %> />
                            <asp:LinkButton ID="SelectButton" runat="server" CommandName="Select"></asp:LinkButton>
                            <input id="hdFolderID" type="hidden" value="<%# Eval("FolderId")%>" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                        <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Folder Name" HeaderStyle-Width="205" SortExpression="Name">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnFolderName" runat="server" Text='<%# Eval("Name") %>' OnClientClick="ViewFolder();return false;"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Path" HeaderStyle-Width="250" SortExpression="Path">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnPath" runat="server" Text='<%# Eval("Path") %>' OnClientClick="ViewFolder();return false;"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Branch Name" HeaderStyle-Width="110" SortExpression="BranchName">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnBranch" runat="server" Text='<%# Eval("BranchName") %>' OnClientClick="ViewFolder();return false;"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Enabled" HeaderStyle-Width="40" SortExpression="Enabled">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnEnabled" runat="server" Text='<%# Eval("Enabled") %>' OnClientClick="ViewFolder();return false;"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Last imported" HeaderStyle-CssClass="TextAlignRight"
                        ItemStyle-CssClass="TextAlignRight" HeaderStyle-Width="120" SortExpression="LastImport">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnLastImport" runat="server" Text='<%# Eval("LastImport") %>'
                                OnClientClick="ViewFolder();return false;" HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy HH:mm:ss}"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Count" HeaderStyle-CssClass="TextAlignRight"
                        ItemStyle-CssClass="TextAlignRight" HeaderStyle-Width="40" SortExpression="ImportCount">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnImportCount" runat="server" Text='<%# Eval("ImportCount") %>'
                                OnClientClick="ViewFolder();return false;" HtmlEncode="false" DataFormatString="{0:d}"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="FolderSqlDataSource" runat="server"
                SelectCommand="lpsp_ExecSqlByPager" SelectCommandType="StoredProcedure" DataSourceMode="DataSet">
                <SelectParameters>
                    <asp:Parameter Name="OrderByField" Type="String" DefaultValue="FolderId" />
                    <asp:Parameter Name="AscOrDesc" Type="String" DefaultValue="asc" />
                    <asp:Parameter Name="Fields" Type="String" DefaultValue="*" />
                    <asp:Parameter Name="DbTable" Type="String" DefaultValue="(SELECT PointFolders.*,Branches.Name AS BranchName,Branches.RegionID,Branches.DivisionID, CASE WHEN (ISNULL(PointFolders.BranchId,0)=0 AND PointFolders.Enabled='0') THEN 'False' ELSE 'True' END AS EnabledCheckBox FROM PointFolders LEFT OUTER JOIN Branches ON PointFolders.BranchId=Branches.BranchId) t" />
                    <asp:Parameter Name="Where" Type="String" DefaultValue="" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="1" Name="StartIndex"
                        PropertyName="StartRecordIndex" Type="Int32" />
                    <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="10" Name="EndIndex"
                        PropertyName="EndRecordIndex" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:HiddenField ID="hdnFolderPaths" runat="server" />
        </div>
    </div>
</asp:Content>
