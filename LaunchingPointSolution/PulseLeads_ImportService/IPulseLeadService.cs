using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PulseLeads;

namespace FocusIT.Pulse
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IPulseLeadService
    {
        [OperationContract]
        RespHdr PostLead(PostLeadRequest req);
        [OperationContract]
        RespHdr PostLoanApp(PostLoanAppRequest req);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    public enum OccupancyType
    {
        PrimaryResidence = 1,
        SecondHome = 2,
        InvestmentProperty = 3
    }

    public enum PurposeOfLoan
    {
        Purchase = 1,
        Refinance_No_Cashout = 2,
        Refinance_Cashout = 3,
        Construction = 4,
        Other = 5
    }

    public enum PreferredContactMethod
    {
        HomePhone = 1,
        BusinessPhone = 2,
        CellPhone = 3,
        Email = 4
    }

    public enum CreditRanking
    {
        Excellent = 1,
        VeryGood = 2,
        Good = 3,
        Fair = 4,
        Poor = 5,
    }

    public enum Property_Type
    {
        SFR = 0,
        Condo = 1,
        Townhome = 2,
        TwotoFourUnit = 3,
        Other = 4
    }

    public enum Housing_Status
    {
        Own = 0,
        Rent = 1
    }

    [DataContract]
    public class Address
    {
        [DataMember]
        public string Street;
        [DataMember]
        public string City;
        [DataMember]
        public string State;
        [DataMember]
        public string Zip;
    }

    [DataContract]
    public class Employment
    {
        [DataMember]
        public string Company;
        [DataMember]
        public bool SelfEmployed;
        [DataMember]
        public string Position;
        [DataMember]
        public decimal MonthlySalary;
        [DataMember]
        public int StartMonth;
        [DataMember]
        public int StartYear;
        [DataMember]
        public int EndMonth;
        [DataMember]
        public int EndYear;
        [DataMember]
        public int YearsInProfession;
        [DataMember]
        public int MonthsInProfession;
        [DataMember]
        public string Phone;
        [DataMember]
        public Address Address;
        [DataMember]
        public int YearsOnWork;
        [DataMember]
        public string BusinessType;
        [DataMember]
        public bool VerifyYourTaxes;
    }

    [DataContract]
    public class OtherIncome
    {
        [DataMember]
        public string Type;
        [DataMember]
        public decimal Amount;
    }

    [DataContract]
    public class LiquidAssets
    {
        [DataMember]
        public string NameOfAccount;
        [DataMember]
        public string AccountNo;
        [DataMember]
        public decimal Amount;
    }

    [DataContract]
    public class ReqHdr
    {
        [DataMember(IsRequired = true)]
        public string SecurityToken;
    }

    [DataContract]
    public class RespHdr
    {
        [DataMember(IsRequired = true)]
        public bool Successful;
        [DataMember(IsRequired = false)]
        public string Error;
    }

    [DataContract]
    public class PostLeadRequest
    {
        [DataMember(IsRequired = true)]
        public ReqHdr RequestHeader;
        [DataMember]
        public string LeadSource;
        [DataMember]
        public string BorrowerFirstName;
        [DataMember]
        public string BorrowerMiddleName;
        [DataMember]
        public string BorrowerLastName;
        [DataMember]
        public string HomePhone;
        [DataMember]
        public string BusinessPhone;
        [DataMember]
        public string CellPhone;
        [DataMember]
        public string Email;
        [DataMember]
        public bool HaveDependents;
        [DataMember]
        public string DOB;
        [DataMember]
        public string SSN;
        [DataMember]
        public string CoBorrowerFirstName;
        [DataMember]
        public string CoBorrowerMiddleName;
        [DataMember]
        public string CoBorrowerLastName;
        [DataMember]
        public string CoBorrowerType;
        [DataMember]
        public string CoBorrowerPhone;
        [DataMember]
        public string CoBorrowerEmail;


        //gdc CR42 add fields

        [DataMember]
        public string CoBorrowerBusinessPhone;
        [DataMember]
        public string CoBorrowerCellPhone;
        //[DataMember]
        //public string CoBorrowerEmail;
        [DataMember]
        public Employment[] CoBorrowerEmployers;
        [DataMember]
        public OtherIncome[] CoBorrowerOtherIncome;
        [DataMember]
        public string Notes;
        //gdc CR42 add fields  end


        [DataMember]
        public string PropertyType;
        [DataMember]
        public Housing_Status HousingStatus;
        [DataMember]
        public int RentAmount;
        [DataMember]
        public PreferredContactMethod PreferredContactMethod;
        [DataMember]
        public Address MailingAddress;
        [DataMember]
        public CreditRanking CreditRanking;
        [DataMember]
        public int LoanAmount;
        [DataMember]
        public PurposeOfLoan PurposeOfLoan;
        [DataMember]
        public OccupancyType OccupancyType;
        [DataMember]
        public string LoanProgram;
        [DataMember]
        public int PropertyValue;
        [DataMember]
        public string Property_Street;
        [DataMember]
        public string Property_City;
        [DataMember]
        public string Property_State;
        [DataMember]
        public string Property_Zip;
        [DataMember]
        public string LoanOfficerFirstName;
        [DataMember]
        public string LoanOfficerLastName;
        [DataMember]
        public string BranchName;
        [DataMember]
        public Employment[] Employment;
        [DataMember]
        public OtherIncome[] OtherIncome;
        [DataMember]
        public LiquidAssets[] LiquidAssets;
        [DataMember]
        public bool InterestOnly;
        [DataMember]
        public bool IncludeEscrows;

        //gdc 20130303 CR61
        [DataMember]
        public string LeadId;

        [DataMember]
        public bool CheckDuplicate;

        [DataMember]
        public decimal MonthlyPayment;
        //gdc 20130303 CR61 END


        //gdc 20130315 CR61
        [DataMember]
        public string County;

        [DataMember]
        public string LoanType;

        [DataMember]
        public int Term;

        [DataMember]
        public decimal Rate;

        //gdc 20130315 CR61 END
    }

    [DataContract]
    public class PostLoanAppRequest
    {
        [DataMember(IsRequired = true)]
        public ReqHdr RequestHeader;
        [DataMember]
        public string LeadSource;
        [DataMember]
        public string BorrowerFirstName;
        [DataMember]
        public string BorrowerMiddleName;
        [DataMember]
        public string BorrowerLastName;
        [DataMember]
        public string HomePhone;
        [DataMember]
        public string BusinessPhone;
        [DataMember]
        public string CellPhone;
        [DataMember]
        public string Email;
        [DataMember]
        public bool HaveDependents;
        [DataMember]
        public string DOB;
        [DataMember]
        public string SSN;
        [DataMember]
        public string CoBorrowerFirstName;
        [DataMember]
        public string CoBorrowerMiddleName;
        [DataMember]
        public string CoBorrowerLastName;
        [DataMember]
        public string CoBorrowerType;
        [DataMember]
        public string CoBorrowerPhone;
        [DataMember]
        public string CoBorrowerEmail;

        //gdc CR42 add fields

        [DataMember]
        public string CoBorrowerBusinessPhone;
        [DataMember]
        public string CoBorrowerCellPhone;
        //[DataMember]
        //public string CoBorrowerEmail;
        [DataMember]
        public Employment[] CoBorrowerEmployers;
        [DataMember]
        public OtherIncome[] CoBorrowerOtherIncome;
        [DataMember]
        public string Notes;
        //gdc CR42 add fields  end

        [DataMember]
        public string PropertyType;
        [DataMember]
        public Housing_Status HousingStatus;
        [DataMember]
        public int RentAmount;
        [DataMember]
        public PreferredContactMethod PreferredContactMethod;
        [DataMember]
        public Address MailingAddress;
        [DataMember]
        public CreditRanking CreditRanking;
        [DataMember]
        public int LoanAmount;
        [DataMember]
        public PurposeOfLoan PurposeOfLoan;
        [DataMember]
        public OccupancyType OccupancyType;
        [DataMember]
        public string LoanProgram;
        [DataMember]
        public int PropertyValue;
        [DataMember]
        public Address Property;
        [DataMember]
        public string LoanOfficerFirstName;
        [DataMember]
        public string LoanOfficerLastName;
        [DataMember]
        public string BranchName;
        [DataMember]
        public Employment[] Employment;
        [DataMember]
        public OtherIncome[] OtherIncome;
        [DataMember]
        public LiquidAssets[] LiquidAssets;
        [DataMember]
        public bool InterestOnly;
        [DataMember]
        public bool IncludeEscrows;
        //gdc 20130303 CR61
        [DataMember]
        public string LeadId;

        [DataMember]
        public bool CheckDuplicate;

        [DataMember]
        public decimal MonthlyPayment;
        //gdc 20130303 CR61 END
        //gdc 20130315 CR61
        [DataMember]
        public string County;

        [DataMember]
        public string LoanType;

        [DataMember]
        public int Term;

        [DataMember]
        public decimal Rate;

        //gdc 20130315 CR61 END
    }

}
