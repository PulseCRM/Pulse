<%@ Page Title="主页" Language="C#"  AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ShortLeadForm._Default" %>

<html>
<head>
<title>Short Lead</title>
    <style>
        .inputWith
        {
            width: 260px;
        }
        
        *
        {
            text-decoration: none;
            font-size: 1em;
            outline: none;
            padding: 0;
            margin: 0;
        }
        code, kbd, samp, pre, tt, var, textarea, input, select, isindex, listing, xmp, plaintext
        {
            white-space: normal;
            font-size: 1em;
            font: inherit;
        }
        img
        {
            border: 0;
        }
        
        body
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 13px;
            color: #282828;
            background-color: #fff;
            line-height: inherit;
            height: 100%;
            margin: 0px;
        }
        #btnGetAQuote
        {
           background:url(btn.png) 0px 0px  no-repeat;
           width:180px;
           height:32px;
           display:block;
           text-indent:-9999em;
           overflow:hidden;
           margin:0px;
           
        }
        #b
        {
             width:297px;
             border:2px solid #000;
             margin:10px;
        }
        table tr
        {
             height:20px;
        }
    </style>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script src="Scripts/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <script>
        $(function () {

            $("#btnsub").click(function () {

                if ($("#txtFirstName").val() == "" || $("#txtLastName").val() == ""
                || $("#txtEmail").val() == "" || $("#txtPhone").val() == "") {

                    alert("Please enter the FirstName/LastName/Email/Phone.");
                    return;
                }

                $.blockUI({ message: "Please wait..." });

                var data = $("#leadForm").serialize();

                var url = $("#leadForm").attr("action");
                //alert(url);
                $.post(url, data, function (result) {
                    //alert(result);
                    if (result.type == "success") {
                        alert("Thank you for your loan inquiry. Your loan officer will contact you shortly.");
                    } else {
                        alert(result.data);
                    }
                    $.unblockUI();
                }, "json");

            });

            $("#txtPhone").mask("(999) 999-999?9");

        });

    </script>
</head>
<body>
<div id="b">
<form id="leadForm" action="Process.ashx" method="post" >
<table style=" text-align:center; width:290px;">
    <tr>
        <td><img src="titileBG.png" /></td>
    </tr>
    <tr>
        <td align="center">First Name</td>
    </tr>
    <tr>
        <td><input id="txtFirstName" name="txtFirstName" type="text" class="inputWith"  /></td>
    </tr>
    <tr>
        <td>Last Name</td>
    </tr>
    <tr>
        <td><input id="txtLastName" name="txtLastName" type="text" class="inputWith" /></td>
    </tr>
    <tr>
        <td>Email</td>
    </tr>
    <tr>
        <td><input id="txtEmail" name="txtEmail" type="text" class="inputWith" /></td>
    </tr>
    <tr>
        <td>Phone</td>
    </tr>
    <tr>
       <td><input id="txtPhone" name="txtPhone" type="text" class="inputWith" /></td>
    </tr>

     <tr>
        <td>Area of Interest</td>
    </tr>
    <tr>
       <td>
           <select id="selArea" name="selArea" class="inputWith">
               <option>--Select an area--</option>
               <option>Austin</option>
               <option>Dallas</option>
               <option>El Paso</option>
               <option>Fort Worth</option>
               <option>Houston</option>
               <option>San Antonio</option>
               <option>Other</option>
           </select>
       </td>
    </tr>

    <tr>
        <td>Message / Comments</td>
    </tr>
    <tr>
       <td>
           <textarea id="txtComments" name="txtComments" cols="20" rows="5" class="inputWith"></textarea>
       </td>
    </tr>
    <tr>
        <td align="center">
           <a id="btnsub" href="javascript:;" ><img src="btn.png" /></a>
        </td>
    </tr>
    <tr>
        <td style=" height:30px;" > 
            <input id="LeadSource" name="LeadSource" type="hidden" value="<%=LeadSource %>" />
            <input id="LoanOfficerFirstName" name="LoanOfficerFirstName" type="hidden" value="<%=LoanOfficerFirstName %>" />
            <input id="LoanOfficerLastName" name="LoanOfficerLastName" type="hidden" value="<%=LoanOfficerLastName %>" />
        </td>
    </tr>
</table>
</form>
</div>

</body>
</html>