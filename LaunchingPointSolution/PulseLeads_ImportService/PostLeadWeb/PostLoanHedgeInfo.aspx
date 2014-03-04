<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PostLoanHedgeInfo.aspx.cs" Inherits="PulseLeads.PostLoanHedgeInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
  <form id="form1" runat="server">
    <div>
        <textarea id="xmldata" runat="server" rows="30" cols="80">

        </textarea>
        <br />
        <textarea id="txtareResult" runat="server" rows="20" cols="50"></textarea>
        <br />
        <asp:Button runat="server" Text="btnTest" ID="btnTest" onclick="btnTest_Click" />
    </div>
    </form>
</body>
</html>
