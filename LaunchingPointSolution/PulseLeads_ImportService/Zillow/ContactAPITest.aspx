<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactAPITest.aspx.cs" Inherits="PulseLeads.Zillow.ContactAPITest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

<style type="text/css">
body{margin:10px 10px 10px 10px;padding:0;font-family:Arial,Verdana,sans-serif;font-size:12px;color:#613d2d;}    
form{padding:0;margin:0;}
textarea{width:99%;margin:0;padding:0;font-size:11px;height: 250px;}
form h2{margin:0 0 0 0;}
.frmbtn{margin: 0 0 10px 0;padding:2px 3px;color:#000000;font-size:12px;cursor:pointer;}
button{cursor:pointer;}

table.imagetable {
	font-family: verdana,arial,sans-serif;
	font-size:11px;
	color:#333333;
	border-width: 1px;
	border-color: #999999;
	border-collapse: collapse;
}
table.imagetable th {
	background:#b5cfd2 url('cell-blue.jpg');
	border-width: 1px;
	padding: 8px;
	border-style: solid;
	border-color: #999999;
}
table.imagetable td {
	background:#ffffff url('cell-white.jpg');
	border-width: 1px;
	padding: 8px;
	border-style: solid;
	border-color: #999999;
}
div.col1 {
	padding-top: 10px;
	color:#000000;font-size:14px;
}
div.col2 {
	padding-top: 10px;
	color:#000000;font-size:14px;
}
</style>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>Test for Zillow Contact API HTTP POST</h2>
        <table width="800px">
            <tr>
                <td>
                    <p>
                        Leads from Zillow's Contact API will be posted to Pulse when the contacts are made on Zillow
                        Mortgage Marketplace. In order to receive these HTTP POSTs, the Pulse client must update their
                        lender account on Zillow and indicate to post the contact information to http://[api-ulr]/zillow/contactapi.aspx. 
                        Once the account is configured, all new contacts from borrowers will be posted to Pulse.
                        This page tests the integration with Zillow Contact API to import leads into Pulse.
                    </p>
                    <p>
                        Supports Zillow Contact API XML version 5
                    </p>
                    <p>
                        Source Code version 1.0
                    </p>
                </td>
            </tr>
            <tr>
                <td>
                    Zillow XML v5 (<asp:LinkButton ID="LinkButtonLoadSample" runat="server" OnClick="LinkButtonLoadSample_Click">Open Sample1_V50.xml</asp:LinkButton>)<br/>
                    <asp:TextBox ID="TextBoxZillowXml" runat="server" TextMode="MultiLine" Rows="10" Width="100%"></asp:TextBox>
                    <br/>
                    <asp:Button ID="ButtonParse" runat="server" Text="Parse Xml" OnClick="ButtonParse_Click" CssClass="frmbtn" />
                    &nbsp;&nbsp;
                    <asp:Button ID="ButtonSubmit" runat="server" Text="Post Lead" OnClick="ButtonSubmit_Click" CssClass="frmbtn"  />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="LiteralResults" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
