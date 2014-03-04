<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartnerCompanyAdd.aspx.cs" Inherits="Contact_PartnerCompanyAdd" MasterPageFile="~/_layouts/LPWeb/MasterPage/Contact.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        // <!CDATA[

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

        function aAdd_onclick() {

            alert("Please save contact company at first.");
            return;
        }

        function AlertNoBranchSelected() {

            alert("No contact branch was selected.");
            return;
        }

        function aCreateBranch_onclick() {

            window.location.href = "PartnerBranchSetup.aspx";
        }

        function BeforeSave() {

            var IsValid = $("#aspnetForm").valid();

            return IsValid

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
                                    <asp:CheckBox ID="chkEnabled" runat="server" Checked="true" Enabled="false" />
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
                   
                </tr>
            </table>

        </div>

        <div id="divToolBar" style="width: 700px; margin-top: 10px;">
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
                    <td>
                        <ul class="ToolStrip" style="margin-left: 0px;">
                            <li><a id="aCreateBranch" href="javascript:aCreateBranch_onclick()">Create</a><span>|</span></li>
                            <li><a id="aUpdateBranch" href="javascript:AlertNoBranchSelected()">Update</a><span>|</span></li>
                            <li><a id="aDisable" href="javascript:AlertNoBranchSelected()">Disable</a><span>|</span></li>
                            <li><a id="aDelete" href="javascript:AlertNoBranchSelected()">Delete</a><span>|</span></li>
                            <li><a id="aAdd" href="javascript:aAdd_onclick()">Add</a><span>|</span></li>
                            <li><a id="aRemove" href="javascript:AlertNoBranchSelected()">Remove</a></li>
                        </ul>
                    </td>
                    
                </tr>
            </table>
        </div>

        <div id="divBranchList" class="ColorGrid" style="width: 700px; margin-top: 5px;">
            <div>
                <table id="gridBranchList" class="GrayGrid" cellspacing="0" cellpadding="4" style="border-collapse: collapse;">
                    <tr class="EmptyDataRow" align="center">
                        <td colspan="2">
                            There is no contact branch.
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
    </div>
</asp:Content>
