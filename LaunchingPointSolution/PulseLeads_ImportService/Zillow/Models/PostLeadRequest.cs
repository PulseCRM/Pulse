namespace PulseLeads.Zillow.Models
{
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

    public class PostLeadRequest
    {
        public RequestHeader RequestHeader;
        public string LeadSource;
        public string BorrowerFirstName;
        public string BorrowerMiddleName;
        public string BorrowerLastName;
        public string HomePhone;
        public string BusinessPhone;
        public string CellPhone;
        public string Email;
        public bool HaveDependents;
        public string DOB;
        public string SSN;
        public string CoBorrowerFirstName;
        public string CoBorrowerMiddleName;
        public string CoBorrowerLastName;
        public string CoBorrowerType;
        public string CoBorrowerPhone;
        public string CoBorrowerEmail;
        public string CoBorrowerBusinessPhone;
        public string CoBorrowerCellPhone;
        public Employment[] CoBorrowerEmployers;
        public OtherIncome[] CoBorrowerOtherIncome;
        public string Notes;
        public string PropertyType;
        public Housing_Status HousingStatus;
        public int RentAmount;
        public PreferredContactMethod PreferredContactMethod;
        public Address MailingAddress;
        public CreditRanking CreditRanking;
        public int CreditScore;
        public int LoanAmount;
        public PurposeOfLoan PurposeOfLoan;
        public OccupancyType OccupancyType;
        public string LoanProgram;
        public int PropertyValue;
        public string Property_Street;
        public string Property_City;
        public string Property_State;
        public string Property_Zip;
        public string LoanOfficerFirstName;
        public string LoanOfficerLastName;
        public string BranchName;
        public Employment[] Employment;
        public OtherIncome[] OtherIncome;
        public LiquidAssets[] LiquidAssets;
        public bool InterestOnly;
        public bool IncludeEscrows;
        public string LeadId;
        public bool CheckDuplicate;
        public decimal MonthlyPayment;
        public string County;
        public string LoanType;
        public int Term;
        public decimal Rate;
    }
}