using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PulseLeads.ConsoleTest.PulseLeadServiceClient;

namespace PulseLeads.ConsoleTest
{
    public class PulseLeadServiceTest
    {
        static void MyTest(string[] args)
        {
            using (PulseLeadServiceClient.PulseLeadServiceClient client = new PulseLeadServiceClient.PulseLeadServiceClient())
            {
                PostLeadRequest req = new PostLeadRequest();
                req.BorrowerFirstName = "Peter";
                req.BorrowerLastName = "FocusITestH";
                req.HomePhone = "222-222-3333";
                req.BusinessPhone = "233-333-4444";
                req.CellPhone = "244-444-5555";
                req.CreditRanking = CreditRanking.Good;
                req.Email = "peter.horiz010@bademail.com";
                req.LoanProgram = "30-year fixed";
                req.OccupancyType = OccupancyType.PrimaryResidence;
                req.LoanAmount = 200000;
                req.PurposeOfLoan = PurposeOfLoan.Refinance_Cashout;
                req.PreferredContactMethod = PreferredContactMethod.Email;
                //req.BranchName = "Atlanta"; 
                //req.LoanOfficerFirstName = "Betty"; 
                //req.LoanOfficerLastName = "Wilkinson"; 
                req.MailingAddress = new Address() { City = "San Jose", State = "CA", Street = "123 Any Street", Zip = "95110" };
                req.PropertyValue = 400000;
                req.Property_Street = "123 Any Street";
                req.Property_City = "San Jose";
                req.Property_State = "CA";
                req.Property_Zip = "95110";
                req.RequestHeader = new ReqHdr();
                req.RequestHeader.SecurityToken = "90e588a7-df07-4abf-976c-ce655d6effa6";
                RespHdr resp = null;
                resp = client.PostLead(req);
                if (resp.Successful)
                {
                    Console.WriteLine("Successfully posted the lead to Pulse!");
                }
                else
                {
                    Console.WriteLine("Failed to post the lead to Pulse, reason:{0}.", resp.Error);
                }
                Console.ReadKey();
            }

            using (PulseLeadServiceClient.PulseLeadServiceClient client = new PulseLeadServiceClient.PulseLeadServiceClient())
            {
                PostLoanAppRequest req = new PostLoanAppRequest();
                req.BorrowerFirstName = "PeterG";
                req.BorrowerMiddleName = "";
                req.BorrowerLastName = "TerrizziG";
                //req.CoBorrowerFirstName
                //req.CoBorrowerMiddleName
                //req.CoBorrowerLastName
                //req.CoBorrowerType
                //req.CoBorrowerPhone
                //req.CoBorrowerCellPhone
                //req.CoBorrowerBusinessPhone
                //req.CoBorrowerEmail
                req.HomePhone = "203-221-3221";
                req.BusinessPhone = "203-331-4441";
                req.CellPhone = "203-441-5551";
                //req.MailingAddress
                req.CreditRanking = CreditRanking.Good;
                req.Email = "pterrizzi@provenit.com";
                //req.HasDependents
                //req.DOB
                //req.BorrowerSSN
                //req.Employment
                //req.CoBorrowerEmployers
                //req.OtherIncome
                //req.CoBorrowerOtherIncome
                //req.LiquidAssets
                req.LoanProgram = "30-year fixed";
                req.OccupancyType = OccupancyType.PrimaryResidence;
                req.LoanAmount = 300000;
                req.PurposeOfLoan = PurposeOfLoan.Purchase;
                req.PreferredContactMethod = PreferredContactMethod.Email;
                req.BranchName = "Atlanta"; //id=8 // "Branch1A1";
                req.LoanOfficerFirstName = "Betty"; //"Frank"
                req.LoanOfficerLastName = "Wilkinson"; // "Smith";
                req.PropertyValue = 300000;
                //req.PropertyType
                //req.HousingStatus
                //req.RentAmount
                //req.Property
                //req.InterestOnly
                //req.IncludeEscrows
                //req.Notes

                req.RequestHeader = new ReqHdr();
                req.RequestHeader.SecurityToken = "ff5eaf4a-6c97-4a00-8252-9eec8bf2979b"; // "2934230595-324923804-2394923";
                RespHdr resp = client.PostLoanApp(req);

                if (resp.Successful)
                {
                    Console.WriteLine("Successfully posted the lead to Pulse!");
                }
                else
                {
                    Console.WriteLine("Failed to post the lead to Pulse, reason:{0}.", resp.Error);
                }
                Console.ReadKey();
            }

        }

    }
}
