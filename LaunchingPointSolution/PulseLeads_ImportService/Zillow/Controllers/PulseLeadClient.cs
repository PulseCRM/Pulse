using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PulseLeads.Zillow.Models;
using PulseLeads.Zillow._Code;

namespace PulseLeads.Zillow.Controllers
{
    internal sealed class PulseLeadClient
    {
        private const string CONST_LEADSOURCE = "ZillowMortgageContactList";

        /// <summary>
        /// Returns the PostLeadRequest object populated with Zillow XML data
        /// </summary>
        /// <param name="zcol"></param>
        /// <returns></returns>
        internal static PostLeadRequest GetPulseLeadRequest(ZillowAttributeCollection zcol)
        {
            try
            {
                PostLeadRequest req = MapZillowToPulseLeadRequest(zcol);
                NLogger.Info(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, "ZillowImport (" + zcol.ImportTransId.ToString() + ") completed.");
                return req;
            }
            catch (Exception ex)
            {
                NLogger.Error(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Posts the PostLeadRequest object to the PulseLead service
        /// </summary>
        /// <param name="req"></param>
        /// <param name="import_trans_id"></param>
        /// <returns></returns>
        internal static ResponseHdr PostPulseLeadRequest(PostLeadRequest req, string import_trans_id)
        {
            try
            {
                // Post
                var svc = new PostLead();
                ResponseHdr response_hdr = svc.Post(req);

                // save the credit score
                if (response_hdr.Successful && req.CreditScore > 0)
                    svc.SaveCreditScore(response_hdr._postArgs.ContactId, req.CreditScore);

                // log the event
                if (response_hdr.Successful)
                    NLogger.Info(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, "Lead posted to Pulse. Zillow.ImporTransId = " + import_trans_id + ", ContactId = " + response_hdr._postArgs.ContactId.ToString());
                else
                    NLogger.Info(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, "Could not post lead to Pulse.");

                return response_hdr;
            }
            catch (Exception ex)
            {
                NLogger.Error(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Populates PostLeadRequest with Zillow XML data
        /// </summary>
        /// <param name="zcol"></param>
        /// <returns></returns>
        private static PostLeadRequest MapZillowToPulseLeadRequest(ZillowAttributeCollection zcol)
        {
            // initialize request object
            PostLeadRequest req = new PostLeadRequest();
            req.RequestHeader = new RequestHeader();
            req.RequestHeader.SecurityToken = zcol.PulseSecurityToken;

            IEnumerable<ZillowAttribute> query = from p in zcol.ZillowAttributes
                                                 where p.GroupName.Equals("ZillowMortgageContactList", StringComparison.OrdinalIgnoreCase)
                                                     && p.AttributeName.Equals("version", StringComparison.OrdinalIgnoreCase)
                                                 select p;
            req.LeadSource = CONST_LEADSOURCE + " v" + query.First().AttributeValue;

            //------------------------------------------------------------------
            // CreditScore
            //    Excellent = 1,    750-850
            //    VeryGood = 2,     700-749
            //    Good = 3,         650-699
            //    Fair = 4,         600-649
            //    Poor = 5,         300-599
            query = from p in zcol.ZillowAttributes
                    where p.GroupName.Equals("CreditScore", StringComparison.OrdinalIgnoreCase)
                        && p.AttributeName.Equals("CreditScore_Text", StringComparison.OrdinalIgnoreCase)
                    select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    int int_value;
                    if (int.TryParse(query.First().AttributeValue, out int_value))
                    {
                        req.CreditScore = int_value;
                        if (int_value <= 599)
                            req.CreditRanking = CreditRanking.Poor;
                        else if (int_value <= 649)
                            req.CreditRanking = CreditRanking.Fair;
                        else if (int_value <= 699)
                            req.CreditRanking = CreditRanking.Good;
                        else if (int_value <= 749)
                            req.CreditRanking = CreditRanking.VeryGood;
                        else
                            req.CreditRanking = CreditRanking.Excellent;
                    }
                }
            }

            //------------------------------------------------------------------
            //BaseMonthlyIncome
            query = from p in zcol.ZillowAttributes
                        where p.GroupName.Equals("PrimaryBorrower", StringComparison.OrdinalIgnoreCase)
                            && p.AttributeName.Equals("BaseMonthlyIncome", StringComparison.OrdinalIgnoreCase)
                        select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    decimal d_value;
                    if (decimal.TryParse(query.First().AttributeValue, out d_value))
                    {
                        req.Employment = new Employment[]
                        {
                            new Employment()
                                {
                                    MonthlySalary = d_value
                                }
                        };
                    }
                }
            }

            //------------------------------------------------------------------
            //AdditionalMonthlyIncome
            query = from p in zcol.ZillowAttributes
                        where p.GroupName.Equals("PrimaryBorrower", StringComparison.OrdinalIgnoreCase)
                            && p.AttributeName.Equals("AdditionalMonthlyIncome", StringComparison.OrdinalIgnoreCase)
                        select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    decimal d_value;
                    if (decimal.TryParse(query.First().AttributeValue, out d_value))
                    {
                        req.OtherIncome = new OtherIncome[]
                        {
                            new OtherIncome()
                                {
                                    Amount = d_value
                                }
                        };
                    }
                }
            }

            //------------------------------------------------------------------
            //PropertyUsage
            query = from p in zcol.ZillowAttributes
                        where p.GroupName.Equals("PropertyDetails", StringComparison.OrdinalIgnoreCase)
                            && p.AttributeName.Equals("PropertyUsage", StringComparison.OrdinalIgnoreCase)
                        select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    if(query.First().AttributeValue.Equals("primaryResidence", StringComparison.OrdinalIgnoreCase))
                        req.OccupancyType = OccupancyType.PrimaryResidence;
                    else if (query.First().AttributeValue.Equals("secondaryOrVacation", StringComparison.OrdinalIgnoreCase))
                        req.OccupancyType = OccupancyType.SecondHome;
                    else if (query.First().AttributeValue.Equals("investmentOrRental", StringComparison.OrdinalIgnoreCase))
                        req.OccupancyType = OccupancyType.InvestmentProperty;
                }
            }

            //------------------------------------------------------------------
            //LoanAmount
            query = from p in zcol.ZillowAttributes
                        where p.GroupName.Equals("LoanDetails", StringComparison.OrdinalIgnoreCase)
                            && p.AttributeName.Equals("LoanAmount", StringComparison.OrdinalIgnoreCase)
                        select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    int int_value;
                    if (int.TryParse(query.First().AttributeValue, out int_value))
                    {
                        req.LoanAmount = int_value;
                    }
                }
            }

            //------------------------------------------------------------------
            //EstimatedPropertyValue
            query = from p in zcol.ZillowAttributes
                        where p.GroupName.Equals("EstimatedPropertyValue", StringComparison.OrdinalIgnoreCase)
                            && p.AttributeName.Equals("EstimatedPropertyValue_Text", StringComparison.OrdinalIgnoreCase)
                        select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    int int_value;
                    if (int.TryParse(query.First().AttributeValue, out int_value))
                    {
                        req.PropertyValue = int_value;
                    }
                }
            }

            //------------------------------------------------------------------
            //Assets
            query = from p in zcol.ZillowAttributes
                    where p.GroupName.Equals("Asset", StringComparison.OrdinalIgnoreCase)
                    select p;

            if (query.Any())
            {
                List<LiquidAssets> lassets = new List<LiquidAssets>();
                string name_of_account = "Not provided";
                foreach (ZillowAttribute item in query)
                {
                    if (item.AttributeName.Equals("type", StringComparison.OrdinalIgnoreCase))
                    {
                        name_of_account = item.AttributeValue;
                    }
                    else if (item.AttributeName.Equals("asset_text", StringComparison.OrdinalIgnoreCase))
                    {
                        decimal d_value;
                        if (decimal.TryParse(item.AttributeValue, out d_value))
                        {
                            lassets.Add(new LiquidAssets
                                {
                                    NameOfAccount = name_of_account,
                                    Amount = d_value,
                                    AccountNo = ""
                                });
                            name_of_account = "Not provided";
                        }
                    }
                }

                if (lassets.Any())
                {
                    req.LiquidAssets = lassets.ToArray();
                }
            }

            //------------------------------------------------------------------
            //LoanPurpose
            query = from p in zcol.ZillowAttributes
                    where p.GroupName.Equals("LoanRequestCreated", StringComparison.OrdinalIgnoreCase)
                        && p.AttributeName.Equals("LoanPurpose", StringComparison.OrdinalIgnoreCase)
                    select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    if (query.First().AttributeValue.Equals("purchase", StringComparison.OrdinalIgnoreCase))
                        req.PurposeOfLoan = PurposeOfLoan.Purchase;
                    else if (query.First().AttributeValue.Equals("refinance", StringComparison.OrdinalIgnoreCase))
                        req.PurposeOfLoan = PurposeOfLoan.Refinance_No_Cashout;
                    else if (query.First().AttributeValue.Equals("homeEquity", StringComparison.OrdinalIgnoreCase))
                        req.PurposeOfLoan = PurposeOfLoan.Refinance_Cashout;
                }

                //<xsd:enumeration value="purchase"/>
                //<xsd:enumeration value="refinance"/>
                //<xsd:enumeration value="homeEquity"/>
                //    req.PurposeOfLoan = PurposeOfLoan.Construction;
                //    req.PurposeOfLoan = PurposeOfLoan.Other;
                //    req.PurposeOfLoan = PurposeOfLoan.Purchase;
                //    req.PurposeOfLoan = PurposeOfLoan.Refinance_Cashout;
                //    req.PurposeOfLoan = PurposeOfLoan.Refinance_No_Cashout;
            }

            //------------------------------------------------------------------
            //FirstName
            query = from p in zcol.ZillowAttributes
                    where p.GroupName.Equals("WebContact", StringComparison.OrdinalIgnoreCase)
                        && p.AttributeName.Equals("FirstName", StringComparison.OrdinalIgnoreCase)
                    select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    req.BorrowerFirstName = Helpers.FormatSqlStringValue(query.First().AttributeValue,50);
                }
            }

            //------------------------------------------------------------------
            //LastName
            query = from p in zcol.ZillowAttributes
                    where p.GroupName.Equals("WebContact", StringComparison.OrdinalIgnoreCase)
                        && p.AttributeName.Equals("LastName", StringComparison.OrdinalIgnoreCase)
                    select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    req.BorrowerLastName = Helpers.FormatSqlStringValue(query.First().AttributeValue, 50);
                }
            }

            //------------------------------------------------------------------
            //EmailAddress
            query = from p in zcol.ZillowAttributes
                    where p.GroupName.Equals("WebContact", StringComparison.OrdinalIgnoreCase)
                        && p.AttributeName.Equals("EmailAddress", StringComparison.OrdinalIgnoreCase)
                    select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    req.Email = Helpers.FormatSqlStringValue(query.First().AttributeValue, 255);
                }
            }

            //------------------------------------------------------------------
            //PhoneNumber
            query = from p in zcol.ZillowAttributes
                    where p.GroupName.Equals("WebContact", StringComparison.OrdinalIgnoreCase)
                        && p.AttributeName.Equals("PhoneNumber", StringComparison.OrdinalIgnoreCase)
                    select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    req.HomePhone = Helpers.FormatSqlStringValue(query.First().AttributeValue, 20);
                }
            }

            //------------------------------------------------------------------
            //Message
            query = from p in zcol.ZillowAttributes
                    where p.GroupName.Equals("WebContact", StringComparison.OrdinalIgnoreCase)
                        && p.AttributeName.Equals("Message", StringComparison.OrdinalIgnoreCase)
                    select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    req.Notes = Helpers.FormatSqlStringValue(query.First().AttributeValue, 4000);
                }
            }

            //------------------------------------------------------------------
            //City
            query = from p in zcol.ZillowAttributes
                    where p.GroupName.Equals("PropertyDetails", StringComparison.OrdinalIgnoreCase)
                        && p.AttributeName.Equals("City", StringComparison.OrdinalIgnoreCase)
                    select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    req.Property_City = Helpers.FormatSqlStringValue(query.First().AttributeValue, 50);
                }
            }

            //------------------------------------------------------------------
            //State
            query = from p in zcol.ZillowAttributes
                    where p.GroupName.Equals("PropertyDetails", StringComparison.OrdinalIgnoreCase)
                        && p.AttributeName.Equals("State", StringComparison.OrdinalIgnoreCase)
                    select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    req.Property_State = Helpers.ConvertStateNameToAbbrev(query.First().AttributeValue);
                }
            }

            //------------------------------------------------------------------
            //StreetAddress
            query = from p in zcol.ZillowAttributes
                    where p.GroupName.Equals("PropertyInfoType", StringComparison.OrdinalIgnoreCase)
                        && p.AttributeName.Equals("StreetAddress", StringComparison.OrdinalIgnoreCase)
                    select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    req.Property_Street = Helpers.FormatSqlStringValue(query.First().AttributeValue, 50);
                }
            }

            //------------------------------------------------------------------
            //Zip
            query = from p in zcol.ZillowAttributes
                    where p.GroupName.Equals("PropertyDetails", StringComparison.OrdinalIgnoreCase)
                        && p.AttributeName.Equals("Zip", StringComparison.OrdinalIgnoreCase)
                    select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    req.Property_Zip = Helpers.FormatSqlStringValue(query.First().AttributeValue, 10);
                }
            }

            //------------------------------------------------------------------
            //PropertyType
            query = from p in zcol.ZillowAttributes
                    where p.GroupName.Equals("PropertyDetails", StringComparison.OrdinalIgnoreCase)
                        && p.AttributeName.Equals("PropertyType", StringComparison.OrdinalIgnoreCase)
                    select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    req.PropertyType = Helpers.FormatSqlStringValue(query.First().AttributeValue, 255);
                }
            }

            //------------------------------------------------------------------
            //CurrentLoanProgram
            query = from p in zcol.ZillowAttributes
                    where p.GroupName.Equals("LoanDetails", StringComparison.OrdinalIgnoreCase)
                        && p.AttributeName.Equals("CurrentLoanProgram", StringComparison.OrdinalIgnoreCase)
                    select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    req.LoanProgram = Helpers.FormatSqlStringValue(query.First().AttributeValue, 255);
                }
            }

            return req;
        }

        /// <summary>
        /// Returns the credit score read from the Zillow Xml
        /// </summary>
        /// <param name="zcol"></param>
        /// <returns></returns>
        private static int? GetCreditScore(ZillowAttributeCollection zcol)
        {
            IEnumerable<ZillowAttribute> query = from p in zcol.ZillowAttributes
                    where p.GroupName.Equals("CreditScore", StringComparison.OrdinalIgnoreCase)
                        && p.AttributeName.Equals("CreditScore_Text", StringComparison.OrdinalIgnoreCase)
                    select p;

            if (query.Any())
            {
                if (!string.IsNullOrEmpty(query.First().AttributeValue))
                {
                    int int_value;
                    if (int.TryParse(query.First().AttributeValue, out int_value))
                    {
                        return int_value > 0 ? (int?) int_value : null;
                    }
                }
            }

            return null;
        }

        #region Helper Functions
        #endregion
    }
}