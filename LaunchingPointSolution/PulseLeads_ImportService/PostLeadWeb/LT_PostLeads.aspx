<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LT_PostLeads.aspx.cs" Inherits="PulseLeads.LT_PostLeads" validateRequest=false  %>
<%
    if (Request.QueryString["test"] != null && Request.QueryString["test"] == "1")
    {
     %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <textarea id="xmldata" runat="server" rows="30" cols="80">
<?xml version="1.0" ?>
<LeadInformation>
	<LeadApplication>
		<Address1>1800 E Buena Vista Drive</Address1>
		<Address2/>
		<City>ORLANDO</City>
		<State>FL</State>
		<Zip>32830</Zip>
		<EmailAddress>otto386186@lendingtree.com</EmailAddress>
		<HomePhone>4079393463</HomePhone>
		<WorkPhone></WorkPhone>
		<FirstName>Otto</FirstName>
		<LastName>Tester</LastName>
		<SSN/>
		<MonthlyIncome/>
		<AnnualIncome/>
		<CreditRating>EXCELLENT</CreditRating>
		<Bankruptcy>Never declared bankruptcy</Bankruptcy>
		<IsMilitary>N</IsMilitary>
		<TimeToContact/>
		<TimeLine>No time restraint</TimeLine>
		<MonthlyObligations/>
		<WorkingWithRealtor/>
		<PropertyType>Single-family</PropertyType>
		<PropertyUse>Primary residence</PropertyUse>
		<PropertyCounty>ORANGE</PropertyCounty>
		<PropertyZip>32830</PropertyZip>
		<PropertyMSA>Orlando, FL</PropertyMSA>
		<PropertyState>FL</PropertyState>
		<LoanType>MORTGAGE</LoanType>
		<MortgageType>OTHER</MortgageType>
		<LoansToBeFinanced>1st  2nd Mortgage</LoansToBeFinanced>
		<DownPayment/>
		<LoanAmount>305002</LoanAmount>
		<PropertyPrice>350000</PropertyPrice>
		<AddlCashOut/>
		<FirstMortgageBalance>275001</FirstMortgageBalance>
		<FirstMortgageMonthlyPayment>1750</FirstMortgageMonthlyPayment>
		<SecondMortgageBalance>30001</SecondMortgageBalance>
		<AddlMortgagePayment/>
		<MortgageTerm>30</MortgageTerm>
		<CurrentLoanRate>7.2500</CurrentLoanRate>
		<PartnerUID>105961</PartnerUID>
		<NameOfPartner>LenderName</NameOfPartner>
		<TrackingNumber>89962573</TrackingNumber>
		<ApplicantID>41216451</ApplicantID>
		<DTI/>
		<LTV>87.1434</LTV>
		<LoanRequested>Refinance</LoanRequested>
		<ApplicationDate>10/10/2012</ApplicationDate>
		<SourceID/>
		<FilterClass>FIXEDFILTERSUB_SF</FilterClass>
		<FilterName>Quick Match FHA Refinance (Excellent, Good)</FilterName>
		<IsVerified>Y</IsVerified>
		<VerifiedHomePhone/>
		<PromotionalProgram>SFTOLFCONVERTEDQF</PromotionalProgram>
		<OtherLoanProgram/>
		<MatchFee>25</MatchFee>
		<FilterRoutingID>837883</FilterRoutingID>
	</LeadApplication>
</LeadInformation>
        </textarea>
        <br />
        <textarea id="txtareResult" runat="server" rows="20" cols="50"></textarea>
        <br />
        <asp:Button runat="server" Text="btnTest" ID="btnTest" onclick="btnTest_Click" />
    </div>
    </form>
</body>
</html>


<% } %>