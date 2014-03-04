using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ServiceModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using focusIT;

using Framework;
using DataAccess;
using Common;
using LP2.Service.Common;

namespace LP2Service
{
    public class PointData
    {
        #region PointData Properties
        short Category = 90;
        PNTLib PNT = null;
        DataAccess.DataAccess da = null;

        List<Table.PointFieldDesc> PointFieldList = null;
        DataAccess.PointConfig pointConfig = null;
        public PointData(PNTLib m_pnt, DataAccess.DataAccess m_da)
        {
            if (m_pnt == null)
                PNT = new PNTLib();
            else
                PNT = m_pnt;
            if (m_da == null)
                da = new DataAccess.DataAccess();
            else
                da = m_da;

            if (PointFieldList == null)
                PointFieldList = PointMgr.Instance.GetPointFieldDescList();
            string err = string.Empty;
            pointConfig = da.GetPointConfigData(ref err);
        }

        public void Dispose()
        {

            if (da != null)
                da = null;
            if (PNT != null)
                PNT = null;
        }
        #endregion
        #region common Point field utilities

        bool IsChecked(short fieldID, List<string> FieldArray, ArrayList FieldSeq)
        {
            string val = "";

            val = PNT.getPointField(FieldArray, FieldSeq, fieldID);
            if (val == null)
                return false;
            val = val.ToUpper();
            if (val == "X")
                return true;
            return false;
        }

        string GetLoanType(List<string> FieldArray, ArrayList FieldSeq)
        {
            string val = "";
            if (IsChecked(26, FieldArray, FieldSeq) == true)             // Loan Type -- conventional
            {
                val = "Conventional";
                return val;
            }
            if (IsChecked(27, FieldArray, FieldSeq) == true)
            {                                                     // Loan Type -- VA
                val = "VA";
                return val;
            }

            if (IsChecked(28, FieldArray, FieldSeq) == true)
            {                                                        // Loan Type -- FHA
                val = "FHA";
                return val;
            }

            if (IsChecked(29, FieldArray, FieldSeq) == true)
            {                                                       // Loan Type - USDA/RH
                val = "USDA/RH";
                return val;
            }

            if (IsChecked(1196, FieldArray, FieldSeq) == true)
            {                                                      //  Loan Type - Other
                val = "Other";
            }

            if (val == null)
                val = "";
            return val;

        }

        string GetAmortizationype(List<string> FieldArray, ArrayList FieldSeq)
        {
            string val = "";

            if (IsChecked(550, FieldArray, FieldSeq) == true)             // Amortization Type -- Fixed
            {
                val = "Fixed Rate";
                return val;
            }

            if (IsChecked(552, FieldArray, FieldSeq) == true)
            {                                                           // Amortization Type -- GPM
                val = "GPM Rate";
                return val;
            }
            if (IsChecked(560, FieldArray, FieldSeq) == true)
            {                                                          // Amortization Type -- ARM
                val = "ARM";
                return val;
            }

            if (IsChecked(562, FieldArray, FieldSeq) == true)
            {                                                          // Amortization Type -- Other
                val = "Other";
            }
            return val;
        }

        string GetYesOrNo(short fId, List<string> FieldArray, ArrayList FieldSeq)
        {
            string temp = "";
            temp = PNT.getPointField(FieldArray, FieldSeq, fId);
            if ((temp == null) || (temp == ""))
            {
                temp = "";
                return temp;
            }
            if (temp.Trim().ToUpper() == "Y")
                temp = "Yes";
            else
                temp = "No";
            return temp;
        }

        string GetLienPos(List<string> FieldArray, ArrayList FieldSeq)
        {
            string val = "";

            if (IsChecked(915, FieldArray, FieldSeq) == true)                           // First Lien Position
            {
                val = "First";
                return val;
            }

            if (IsChecked(916, FieldArray, FieldSeq) == true)                           // Second Lien Position
            {
                val = "Second";
                return val;
            }

            if (IsChecked(917, FieldArray, FieldSeq) == true)                           // Other Lien Position
            {
                val = "Other";
            }
            return val;
        }

        string GetOccupancy(List<string> FieldArray, ArrayList FieldSeq)
        {
            string val = "";

            if (IsChecked(921, FieldArray, FieldSeq) == true)                           // Primary Residence
            {
                val = "Primary";
                return val;
            }

            if (IsChecked(923, FieldArray, FieldSeq) == true)                           // Secondary Residence
            {
                val = "Secondary";
                return val;
            }

            if (IsChecked(924, FieldArray, FieldSeq) == true)                           // Investment
            {
                val = "Investment";
            }
            return val;
        }

        string GetLoanPurpose(List<string> FieldArray, ArrayList FieldSeq)
        {
            string val = "";

            if (IsChecked(1190, FieldArray, FieldSeq) == true)                                                 // Purchase
            {
                val = "Purchase";
                return val;
            }


            if (IsChecked(1191, FieldArray, FieldSeq) == true)                           // Construction-Permanent
            {
                val = "Construction-Permanent";
                return val;
            }

            if (IsChecked(1192, FieldArray, FieldSeq) == true)                           // Construction
            {
                val = "Construction";
                return val;
            }

            if (IsChecked(1193, FieldArray, FieldSeq) == true)                           // Cash-Out Refinance
            {
                val = "Cash-Out Refinance";
                return val;
            }

            if (IsChecked(1194, FieldArray, FieldSeq) == true)                       // Other
            {
                val = "Other";
                return val;
            }

            if (IsChecked(1198, FieldArray, FieldSeq) == true)                           //  No Cash-Out Refinance
                val = "No Cash-Out Refinance";

            return val;

        }

        void getCityStateZip(string val, ref string city, ref string state, ref string zip)
        {
            city = "";
            state = "";
            zip = "";

            if (val == null)
                return;

            if (val == "")
                return;

            int index;
            index = val.IndexOf(",");
            if (index < 0)
                return;

            city = val.Substring(0, index);
            val = val.Substring(index + 1);
            val = val.Substring(0).TrimStart();
            index = val.IndexOf(" ");
            if (index < 0)
                return;

            state = val.Substring(0, index);
            zip = val.Substring(index + 1).Trim();
        }

        void ParseName(string Name, ref string first, ref string last, ref string middle)
        {
            int index = 0;

            first = "";
            last = "";
            middle = "";
            Name = Name.Trim();
            if (Name == String.Empty)
                return;
            index = Name.IndexOf(" ");
            if (index < 0)
            {
                first = Name;
                middle = "";
                last = " ";
                return;
            }

            string NameTemp = Name.Substring(index - 1, 1);
            int len = 0;
            if (NameTemp == ",")
            {
                last = Name.Substring(0, index - 1);
                len = Name.Length - index;
                Name = Name.Substring(index, len);
                Name = Name.Trim();
                index = Name.IndexOf(" ");
                if (index < 0)
                {
                    first = Name;
                }
                else
                {
                    middle = Name.Substring(0, index);
                    int len_first = Name.Length - index;
                    first = Name.Substring(index, len_first);
                    first = first.Trim();
                }
                return;
            }

            first = Name.Substring(0, index);
            len = Name.Length - index;
            Name = Name.Substring(index, len);
            Name = Name.Trim();
            index = Name.IndexOf(" ");
            if (index < 0)
            {
                last = Name.Trim();
                return;
            }
            middle = Name.Substring(0, index);
            len = Name.Length - index;
            last = Name.Substring(index, len);
            last = last.Trim();
        }

        private bool IsDateValid(ref string sTempDate)
        {
            if (string.IsNullOrEmpty(sTempDate))
                return true;

            DateTime tempDate = DateTime.MinValue;
            if (DateTime.TryParse(sTempDate, out tempDate))
            {
                if (tempDate.Year <= 1900)
                {
                    sTempDate = string.Empty;
                    return false;
                }
                return true;
            }
            sTempDate = string.Empty;
            return false;
        }
        #endregion
        #region Loan Data Info & Borrower/Coborrower Contacts

        void AssignLoanStageDatesFromPoint(List<string> FieldArray, ArrayList FieldSeq, ref List<Table.LoanStages> loanStages, List<Table.DefaultStage> defStages)
        {
            string err = string.Empty;
            Dictionary<string, Table.LoanStages> stages = new Dictionary<string, Table.LoanStages>();
            if (loanStages == null)
                loanStages = new List<Table.LoanStages>();
            #region setup Dictionary to compare Stages using Stage Names
            foreach (Table.LoanStages ls in loanStages)
            {
                if (ls == null || string.IsNullOrEmpty(ls.StageName))
                    continue;
                stages.Add(ls.StageName.Trim().ToUpper(), ls);
            }
            #endregion
            #region Compare Default Stage Names against loan stage names
            foreach (Table.DefaultStage dStage in defStages)
            {
                if (dStage == null || string.IsNullOrEmpty(dStage.Name))
                    continue;
                string key = dStage.Name.Trim().ToUpper();
                Table.LoanStages ls = null;
                if (stages.ContainsKey(key))
                {
                    ls = stages[key];
                    continue;
                }
                ls = new Table.LoanStages();
                ls.StageName = dStage.Name.Trim();
                ls.PointDateField = dStage.StageDateFld;
                ls.PointNameField = dStage.StageNameFld;
                ls.LoanStageId = -1;
                ls.SequenceNumber = dStage.SequenceNumber;
                ls.TaskCount = 0;
                ls.WflStageId = dStage.WflStageId;
                ls.WflTemplId = dStage.WflTemplId;
                ls.DaysFromCreation = dStage.DaysAfterCreation;
                ls.DaysFromEstClose = dStage.DaysFromEstClose;
                ls.CalculationMethod = dStage.CalculateMethod;
                loanStages.Add(ls);
            }
            #endregion
            #region Get Stage completion date from Point
            string pointDate = string.Empty;
            short pointDateField = 0;
            short pointStageNameField = 0;
            short defaultPointDateField = 0;
            List<FieldMap> fieldMap = new List<FieldMap>();

            foreach (Table.LoanStages s in loanStages)
            {
                #region reset the values
                pointDateField = 0;
                defaultPointDateField = 0;
                pointStageNameField = 0;
                pointDate = string.Empty;
                if (s == null || string.IsNullOrEmpty(s.StageName))
                    continue;
                #endregion
                if (s.PointDateField > 0)
                    pointDateField = s.PointDateField;
                if (s.PointNameField > 0)
                    pointStageNameField = s.PointNameField;
                DateTime ComplDate = DateTime.MinValue;
                #region assign default stage date
                if (s.PointDateField <= 0)
                {
                    switch (s.StageName)
                    {
                        case "Open":
                        case PointStage.Application:
                            defaultPointDateField = (short)PointStageDateField.Application;
                            break;
                        case PointStage.SentToProcessing:
                        case "Processing":
                        case "Sent To Processing":
                            defaultPointDateField = (short)PointStageDateField.SentToProcessing;
                            break;
                        case PointStage.HMDACompleted:
                        case PointStage.HMDA:
                        case PointStage.HMDAComplete:
                            defaultPointDateField = (short)PointStageDateField.HDMA;
                            break;
                        case PointStage.Submit:
                        case PointStage.Submitted:
                            defaultPointDateField = (short)PointStageDateField.Submitted;
                            break;
                        case PointStage.Approve:
                        case PointStage.Approved:
                            pointDateField = (short)PointStageDateField.Approved;
                            break;
                        case PointStage.Resubmit:
                        case PointStage.Re_submit:
                        case PointStage.Resubmitted:
                            defaultPointDateField = (short)PointStageDateField.Resubmitted;
                            break;
                        case PointStage.CleartoClose:
                        case PointStage.ClearToClose:
                            defaultPointDateField = (short)PointStageDateField.ClearedToClose;
                            break;
                        case PointStage.DocsDrawn:
                            defaultPointDateField = (short)PointStageDateField.DocsDrawn;
                            break;
                        case PointStage.DocsOut:
                        case "Docs":
                            defaultPointDateField = (short)PointStageDateField.DocsOut;
                            break;
                        case PointStage.DocsReceived:
                            defaultPointDateField = (short)PointStageDateField.DocsReceived;
                            break;
                        case PointStage.Fund:
                        case PointStage.Funded:
                            defaultPointDateField = (short)PointStageDateField.Funded;
                            break;
                        case PointStage.Record:
                        case PointStage.Recorded:
                            defaultPointDateField = (short)PointStageDateField.Recorded;
                            break;
                        case PointStage.Suspend:
                        case PointStage.Suspended:
                            defaultPointDateField = (short)PointStageDateField.Suspended;
                            break;
                        case PointStage.Close:
                        case PointStage.Closed:
                            defaultPointDateField = (short)PointStageDateField.Closed;
                            break;
                        default:
                            defaultPointDateField = s.PointDateField;
                            break;
                    }
                }
                #endregion
                if (pointDateField > 0)
                    pointDate = PNT.getPointField(FieldArray, FieldSeq, pointDateField);
                else
                    if (defaultPointDateField > 0)
                        pointDate = PNT.getPointField(FieldArray, FieldSeq, defaultPointDateField);
                if (string.IsNullOrEmpty(pointDate) || pointDate.Contains("1/1/0001"))
                    ComplDate = DateTime.MinValue;
                else
                    DateTime.TryParse(pointDate, out ComplDate);
                s.Completed = ComplDate;
            }
            #endregion
        }

        void SetupDefaultLoanStagesDates(List<string> FieldArray, ArrayList FieldSeq, ref List<Table.LoanStages> loanStages)
        {
            string err = "";

            bool noDefStages = true;
            List<Table.DefaultStage> defStages = null;
            da.GetDefaultStageName(ref defStages, ref err);
            if (defStages != null && defStages.Count > 0)
            {
                noDefStages = false;
            }
            if (loanStages == null)
                loanStages = new List<Table.LoanStages>();
            if (noDefStages)
                return;

            AssignLoanStageDatesFromPoint(FieldArray, FieldSeq, ref loanStages, defStages);
        }

        void AddLoan(ref Record.Loans loans, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (loans == null)
                loans = new Record.Loans();
            bool supportMultipleBranches_Folder = false;
            string err = string.Empty;
            string tempStr = string.Empty;
            if (pointConfig == null)
                pointConfig = da.GetPointConfigData(ref err);
            if (pointConfig != null)
                supportMultipleBranches_Folder = pointConfig.Enable_MultiBranchFolders;
            loans.AppraisedValue = PNT.getPointField(FieldArray, FieldSeq, 801);              //  1
            loans.CCScenario = PNT.getPointField(FieldArray, FieldSeq, 7404);                 //   2
            loans.CLTV = PNT.getPointField(FieldArray, FieldSeq, 541);                        //   3
            loans.County = PNT.getPointField(FieldArray, FieldSeq, 35);                       //   4
            loans.DownPay = PNT.getPointField(FieldArray, FieldSeq, 524);                        //    15
            loans.EstCloseDate = PNT.getPointField(FieldArray, FieldSeq, 6075);              //     16
            loans.BranchName = PNT.getPointField(FieldArray, FieldSeq, 20);                 // get the branch name
            //Stage and Status dates
            loans.DateOpen = PNT.getPointField(FieldArray, FieldSeq, (short)PointStageDateField.Open);                    //   5
            loans.DateSubmit = PNT.getPointField(FieldArray, FieldSeq, (short)PointStageDateField.Submitted);                  //   6
            loans.DateApprove = PNT.getPointField(FieldArray, FieldSeq, (short)PointStageDateField.Approved);               //    7
            loans.DateClearToClose = PNT.getPointField(FieldArray, FieldSeq, (short)PointStageDateField.ClearedToClose);      //    8
            loans.DateDocs = PNT.getPointField(FieldArray, FieldSeq, (short)PointStageDateField.DocsDrawn);                     //    9
            loans.DateFund = PNT.getPointField(FieldArray, FieldSeq, (short)PointStageDateField.Funded);                     //   10
            loans.DateRecord = PNT.getPointField(FieldArray, FieldSeq, (short)PointStageDateField.Recorded);                 //    11
            loans.DateClose = PNT.getPointField(FieldArray, FieldSeq, (short)PointStageDateField.Closed);                    //    12
            loans.DateDenied = PNT.getPointField(FieldArray, FieldSeq, (short)PointStageDateField.Denied);                  //    13
            loans.DateCanceled = PNT.getPointField(FieldArray, FieldSeq, (short)PointStageDateField.Canceled);              //    14
            loans.DateSuspended = PNT.getPointField(FieldArray, FieldSeq, (short)PointStageDateField.Suspended);
            loans.DateHMDA = PNT.getPointField(FieldArray, FieldSeq, (short)PointStageDateField.HDMA);
            loans.DateProcessing = PNT.getPointField(FieldArray, FieldSeq, (short)PointStageDateField.SentToProcessing);
            loans.DateReSubmit = PNT.getPointField(FieldArray, FieldSeq, (short)PointStageDateField.Resubmitted);
            loans.DateDocsOut = PNT.getPointField(FieldArray, FieldSeq, (short)PointStageDateField.DocsOut);
            loans.DateDocsReceived = PNT.getPointField(FieldArray, FieldSeq, (short)PointStageDateField.DocsReceived);

            //       misc fields                                                                                  17
            loans.LienPosition = GetLienPos(FieldArray, FieldSeq);                                     //     18
            loans.LoanAmount = PNT.getPointField(FieldArray, FieldSeq, 11);                     //     19
            tempStr = PNT.getPointField(FieldArray, FieldSeq, 1);
            tempStr = string.IsNullOrEmpty(tempStr) ? string.Empty : tempStr.Trim();
            loans.LoanNumber = tempStr;                                                                             //     20
            loans.LoanType = GetLoanType(FieldArray, FieldSeq);                                      //     21
            loans.LTV = PNT.getPointField(FieldArray, FieldSeq, 540);                                 //     22
            loans.MonthlyPayment = PNT.getPointField(FieldArray, FieldSeq, 527);             //     23
            loans.LenderNotes = PNT.getPointField(FieldArray, FieldSeq, 6380);                //     24
            loans.Occupancy = GetOccupancy(FieldArray, FieldSeq);                                  //     25
            loans.Program = PNT.getPointField(FieldArray, FieldSeq, 7403);                        //     26
            loans.PropertyAddr = PNT.getPointField(FieldArray, FieldSeq, 31);                    //     27
            loans.PropertyCity = PNT.getPointField(FieldArray, FieldSeq, 32);                      //     28            
            loans.PropertyState = PNT.getPointField(FieldArray, FieldSeq, 33);                    //     29                     
            loans.PropertyZip = PNT.getPointField(FieldArray, FieldSeq, 34);                       //      30
            loans.Purpose = GetLoanPurpose(FieldArray, FieldSeq);                                   //      31
            loans.Rate = PNT.getPointField(FieldArray, FieldSeq, 12);                                   //      32
            loans.RateLockExpiration = PNT.getPointField(FieldArray, FieldSeq, 6063);      //      33
            loans.SalesPrice = PNT.getPointField(FieldArray, FieldSeq, 800);                      //       34
            loans.Term = PNT.getPointField(FieldArray, FieldSeq, 13);                                  //       35
            loans.Due = PNT.getPointField(FieldArray, FieldSeq, 3190);                                //       36
            loans.LeadRanking = PNT.getPointField(FieldArray, FieldSeq, 6209);
            loans.PurchasedDate = PNT.getPointField(FieldArray, FieldSeq, 61); //61 PurchasedDate 
            loans.PropertyType = PNT.getPointField(FieldArray, FieldSeq, 2729);                    // Property Type
        }
        void AddLoanARMCaps(ref Record.LoanArmCaps armCaps, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (armCaps == null)
                armCaps = new Record.LoanArmCaps();

            string err = string.Empty;
            string temp = string.Empty;
            string pointFileName = string.Empty;
            //ARM fields
            decimal tempDecimal = decimal.Zero;
            try
            {
                pointFileName = PNT.getPointField(FieldArray, FieldSeq, 7);
                if (string.IsNullOrEmpty(pointFileName))
                    pointFileName = string.Empty;

                temp = PNT.getPointField(FieldArray, FieldSeq, 2322);
                if (!string.IsNullOrEmpty(temp) && decimal.TryParse(temp, out tempDecimal))
                    armCaps.Margin = temp.Trim();
                else if (!string.IsNullOrEmpty(temp))
                    err += string.Format("\r\n ARM Margin (2322) {0} is invalid.", temp);

                temp = PNT.getPointField(FieldArray, FieldSeq, 2324);
                if (!string.IsNullOrEmpty(temp) && decimal.TryParse(temp, out tempDecimal))
                    armCaps.AdjCap = temp.Trim();
                else if (!string.IsNullOrEmpty(temp))
                    err += string.Format("\r\nARM Adj Caps (2324) {0} is invalid.", temp);

                temp = PNT.getPointField(FieldArray, FieldSeq, 2325);
                if (!string.IsNullOrEmpty(temp) && decimal.TryParse(temp, out tempDecimal))
                    armCaps.LifeCap = temp.Trim();
                else if (!string.IsNullOrEmpty(temp))
                    err += string.Format("\r\n ARM Life Cap (2325) {0} is invalid.", temp);
                temp = PNT.getPointField(FieldArray, FieldSeq, 2329);
                if (!string.IsNullOrEmpty(temp))
                    armCaps.Index = temp.Trim();

                temp = PNT.getPointField(FieldArray, FieldSeq, 2338);
                if (!string.IsNullOrEmpty(temp) && decimal.TryParse(temp, out tempDecimal))
                    armCaps.InitAdjCap = temp.Trim();
                else if (!string.IsNullOrEmpty(temp))
                    err += string.Format("\r\n ARM InitAdjCap (2338) {0} is invalid.", temp);

                temp = PNT.getPointField(FieldArray, FieldSeq, 3896);
                armCaps.Index = temp;
            }
            catch (Exception ex)
            {
                err += "\r\n" + ex.ToString();
            }
            finally
            {
                if (!string.IsNullOrEmpty(err) && err.Length > 5)
                {
                    err = string.Format("AddLoanARMCaps, PointFileName: {0}", pointFileName) + err;
                    int Event_id = 9310;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }
        void AddLoanProfit(ref Record.LoanProfit loanProfit, List<string> FieldArray, ArrayList FieldSeq)
        {
            string err = string.Empty;
            string temp = string.Empty;
            decimal tempDecimal = 0;
            loanProfit = da.GetLoanProfit(loanProfit.FileId, ref err);

            if (loanProfit == null)
                loanProfit = new Record.LoanProfit();
            // LW 08/30/2013 Lender Credit 2284-->812
            temp = PNT.getPointField(FieldArray, FieldSeq, 812);                      // Lender Credit
            if (!string.IsNullOrEmpty(temp) && decimal.TryParse(temp, out tempDecimal))
            {
                loanProfit.LenderCredit = temp.Trim();
            }
            else
                err = string.Format("\r\nLender Credit (812) {0} is either empty or invalid.", temp);

        }

        void AddLoanLock(ref Record.LoanLocks loanLocks, ref Record.LoanLocksPage loanLocksPage, List<string> FieldArray, ArrayList FieldSeq, int fileId)
        {
            //var fieldIds = new short[] { 2322, 2324, 2325, 2329, 2338, 3896, 4003, 4004, 12973 }; //ARM Caps Margin, ARM Adj Caps, ARM Life Cap, ARM Init Adj Cap, Escrow Taxes, Escrow Insurance, LPMI
            //var profitIds = new short[] { 2284, 4018, 11241, 11243, 11604, 12492, 12495, 12973, 12974, 12975, 12976, 12977, 14153 }; // Mandatory Final Price, Best Effort Price, Comp Total, Best Effort to LO, Hedge Cost, Cost on Sale,Orig Points, Discount Point, Ext Cost 1, Ext Cost 2
            //var lockIds = new short[] { 12, 13, 1190, 1191, 1192, 1193, 1194, 1198, 921, 923, 924, 2729, 2836, 7505, 2284, 11438, 6100, 6101, 12545, 4018, 6061, 6062, 6063, 7403 };
            string err = string.Empty;
            DataAccess.DataAccess da = new DataAccess.DataAccess();

            if (loanLocks == null)
            {
                loanLocks = da.GetLoanLock(fileId, ref err);
            }
            if (loanLocks == null)
            {
                return;
            }
            if (loanLocksPage == null)
                loanLocksPage = new Record.LoanLocksPage();

            string temp = string.Empty;
            DateTime dt = DateTime.MinValue;
            int tempInt = 0;
            string pointFileName = string.Empty;
            try
            {
                pointFileName = PNT.getPointField(FieldArray, FieldSeq, 7);
                if (string.IsNullOrEmpty(pointFileName))
                    pointFileName = string.Empty;

                temp = PNT.getPointField(FieldArray, FieldSeq, 1143);                      // Lock Extension Term
                if (!string.IsNullOrEmpty(temp) && int.TryParse(temp, out tempInt))
                {
                    loanLocks.Ext1Term = temp.Trim();
                    loanLocks.Ext2Term = temp.Trim();
                    loanLocks.Ext3Term = temp.Trim();
                }
                else
                    err = string.Format("\r\nLock Ext1/2 Term (1143) {0} is either empty or invalid.", temp);

                temp = PNT.getPointField(FieldArray, FieldSeq, 6061);                      // Lock Date
                if (!string.IsNullOrEmpty(temp) && !temp.Contains("01/01/1900") && DateTime.TryParse(temp, out dt))
                    loanLocks.LockTime = temp; // Lock Date
                else if (string.IsNullOrEmpty(temp))
                    err = string.Empty;
                else
                    err += string.Format("\r\n Lock Date (6061) {0} is invalid.", temp);

                temp = PNT.getPointField(FieldArray, FieldSeq, 6062); // Lock Term
                if (!string.IsNullOrEmpty(temp) && int.TryParse(temp, out tempInt))
                    loanLocks.LockTerm = temp.Trim();   // Lock Term
                else
                    err = string.Format("\r\nLock Term (6062) {0} is either empty or invalid.", temp);

                ///////////////////   Lock Rate Page - Begin

                temp = PNT.getPointField(FieldArray, FieldSeq, 2836);   // FICO 
                if (!string.IsNullOrEmpty(temp))
                    loanLocksPage.FICO = temp.Trim();   // FICO
                else
                    err = string.Format("\r\nFICO (2836) {0} is either empty or invalid.", temp);

                temp = PNT.getPointField(FieldArray, FieldSeq, 4018);   // MIOption 
                if (!string.IsNullOrEmpty(temp))
                    loanLocksPage.MIOption = temp.Trim();   // MIOption
                else
                    err = string.Format("\r\nMIOption (4018) {0} is either empty or invalid.", temp);

                temp = PNT.getPointField(FieldArray, FieldSeq, 12492);   // Price 
                if (!string.IsNullOrEmpty(temp))
                    loanLocksPage.Price = temp.Trim();   // Price
                else
                    err = string.Format("\r\nPrice (12492) {0} is either empty or invalid.", temp);

                temp = PNT.getPointField(FieldArray, FieldSeq, 2729);   // Property Type 
                if (!string.IsNullOrEmpty(temp))
                    loanLocksPage.PropertyType = temp.Trim();   // Property Type
                else
                    err = string.Format("\r\nProperty Type (2729) {0} is either empty or invalid.", temp);

                temp = PNT.getPointField(FieldArray, FieldSeq, 6100);
                if (!string.IsNullOrEmpty(temp))
                    loanLocksPage.Float = temp.Trim();
                else
                    err = string.Format("\r\nFloat (6100) {0} is either empty or invalid.", temp);

                temp = PNT.getPointField(FieldArray, FieldSeq, 6101);
                if (!string.IsNullOrEmpty(temp))
                    loanLocksPage.Lock = temp.Trim();
                else
                    err = string.Format("\r\nLock (6101) {0} is either empty or invalid.", temp);

                temp = PNT.getPointField(FieldArray, FieldSeq, 6061);
                if (!string.IsNullOrEmpty(temp))
                    loanLocksPage.LockDate = temp.Trim();
                else
                    err = string.Format("\r\nLock Date (6061) {0} is either empty or invalid.", temp);

                temp = PNT.getPointField(FieldArray, FieldSeq, 6062);
                if (!string.IsNullOrEmpty(temp))
                    loanLocksPage.LockTerm = temp.Trim();
                else
                    err = string.Format("\r\nLock Term (6062) {0} is either empty or invalid.", temp);

                temp = PNT.getPointField(FieldArray, FieldSeq, 6063);
                if (!string.IsNullOrEmpty(temp))
                    loanLocksPage.LockExpDate = temp.Trim();
                else
                    err = string.Format("\r\nLock Exp Date (6063) {0} is either empty or invalid.", temp);

                temp = PNT.getPointField(FieldArray, FieldSeq, 12976);
                if (!string.IsNullOrEmpty(temp))
                    loanLocksPage.Extension12976 = temp.Trim();
                else
                    err = string.Format("\r\nExtension (12976) {0} is either empty or invalid.", temp);

                temp = PNT.getPointField(FieldArray, FieldSeq, 12977);
                if (!string.IsNullOrEmpty(temp))
                    loanLocksPage.Extension12977 = temp.Trim();
                else
                    err = string.Format("\r\nExtension (12977) {0} is either empty or invalid.", temp);

                ///////////////////   Lock Rate Page - End

                // Lock & Extension 1/2 Lock Expiration Date
                temp = PNT.getPointField(FieldArray, FieldSeq, 6063);
                if (!string.IsNullOrEmpty(temp) && !temp.Contains("01/01/1900") && DateTime.TryParse(temp, out dt))
                {
                    loanLocks.LockExpirationDate = temp; // Expiration Date
                    loanLocks.Ext1LockExpDate = temp;
                    loanLocks.Ext2LockExpDate = temp;
                    loanLocks.Ext3LockExpDate = temp;
                }
                temp = PNT.getPointField(FieldArray, FieldSeq, 6100);        // Lock Type = Float
                if (!string.IsNullOrEmpty(temp) && temp.Trim().ToUpper() == "X")
                    loanLocks.LockOption = "Float";
                temp = PNT.getPointField(FieldArray, FieldSeq, 6101);        // Lock Type = Lock
                if (!string.IsNullOrEmpty(temp) && temp.Trim().ToUpper() == "X")
                    loanLocks.LockOption = "Lock";
                temp = PNT.getPointField(FieldArray, FieldSeq, 11438);        // Lock Type = Register
                if (!string.IsNullOrEmpty(temp) && temp.Trim().ToUpper() == "X")
                    loanLocks.LockOption = "Register";

                // Extension 1 Lock Date
                temp = PNT.getPointField(FieldArray, FieldSeq, 7021);
                if (!string.IsNullOrEmpty(temp) && !temp.Contains("01/01/1900") && DateTime.TryParse(temp, out dt))
                {
                    loanLocks.Ext1LockTime = temp;
                }
                else if (!string.IsNullOrEmpty(temp))
                    err += string.Format("\r\n Extension 1/2 Lock Date (7021) {0} is invalid.", temp);
                // Extension 2 Lock Date
                temp = PNT.getPointField(FieldArray, FieldSeq, 7025);
                if (!string.IsNullOrEmpty(temp) && !temp.Contains("01/01/1900") && DateTime.TryParse(temp, out dt))
                    loanLocks.Ext2LockTime = temp;
                else if (!string.IsNullOrEmpty(temp))
                    err += string.Format("\r\n Extension 2 Lock Date (7025) {0} is invalid.", temp);

                loanLocks.LockedBy = PNT.getPointField(FieldArray, FieldSeq, 7338);//7338	Locked By
            }
            catch (Exception ex)
            {
                err += "\r\n" + ex.ToString();
                int Event_id = 9311;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                //if (!string.IsNullOrEmpty(err) && err.Length > 5)
                //{
                //    err = string.Format("AddLoanLock, PointFileName: {0}", pointFileName) + err;
                //    int Event_id = 9312;
                //    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                //}
            }
        }
        void Add_Borrower_Employment(ref List<Record.Employment> employment, List<string> FieldArray, ArrayList FieldSeq)
        {
            string temp = "";
            string temp_c = "";
            string temp_s = "";
            int idx_s = 0;
            bool status_b = true;
            decimal temp_d = 0.0m;

            List<Record.Employment> employmentList = employment;

            if (employmentList == null)
                employmentList = new List<Record.Employment>();

            Record.Employment employment_temp = new Record.Employment();

            employment_temp.EmplId = 0;
            employment_temp.ContactId = 0;

            employment_temp.SelfEmployed = true;
            employment_temp.Position = "";
            employment_temp.StartYear = 0;
            employment_temp.StartMonth = 0;
            employment_temp.EndYear = 0;
            employment_temp.EndMonth = 0;

            employment_temp.YearsOnWork = 0;
            employment_temp.Phone = "";
            employment_temp.ContactBranchId = 0;

            employment_temp.CompanyName = "";
            employment_temp.Address = "";
            employment_temp.City = "";
            employment_temp.State = "";
            employment_temp.Zip = "";

            temp = PNT.getPointField(FieldArray, FieldSeq, 148);
            if ((temp == null) || (temp == ""))
            {
                employment_temp.CompanyName = "";
            }
            else
            {
                employment_temp.CompanyName = temp.Trim();
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 149);
            if ((temp == null) || (temp == ""))
            {
                employment_temp.Address = "";
            }
            else
            {
                employment_temp.Address = temp.Trim();
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 140);
            if ((temp == null) || (temp == ""))
            {
                employment_temp.City = "";
            }
            else
            {
                employment_temp.City = temp.Trim();
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 142);
            if ((temp == null) || (temp == ""))
            {
                employment_temp.State = "";
            }
            else
            {
                employment_temp.State = temp.Trim();
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 143);
            if ((temp == null) || (temp == ""))
            {
                employment_temp.Zip = "";
            }
            else
            {
                employment_temp.Zip = temp.Trim();
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 141);
            if ((temp == null) || (temp == ""))
            {
                employment_temp.SelfEmployed = false;
            }
            else
            {
                if (temp.ToUpper() == "X")
                    employment_temp.SelfEmployed = true;
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 135);
            if ((temp == null) || (temp == ""))
            {
                employment_temp.Position = "";
            }
            else
            {
                employment_temp.Position = temp.Trim();
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 136);
            if ((temp == null) || (temp == ""))
            {
                employment_temp.Phone = "";
            }
            else
            {
                employment_temp.Phone = temp.Trim();
            }

            string temp_StartDate = "";
            string temp_EndDate = "";
            string temp_YearsOnWork = "";

            temp = PNT.getPointField(FieldArray, FieldSeq, 138);
            if ((temp == null) || (temp == ""))
            {
                temp_YearsOnWork = "";
                employment_temp.YearsOnWork = 0;
            }
            else
            {
                temp_YearsOnWork = temp;
                idx_s = temp_YearsOnWork.Length;
                if (idx_s > 0)
                {
                    status_b = decimal.TryParse(temp_YearsOnWork, out temp_d);

                    if (status_b == true)
                    {
                        employment_temp.YearsOnWork = temp_d;
                    }
                }
            }

            employmentList.Add(employment_temp);

            int i = 0;
            short fld = 0;
            for (i = 0; i < 8; i++)
            {

                employment_temp = null;
                employment_temp = new Record.Employment();

                fld = (short)(5220 + (i * 5));
                temp = PNT.getPointField(FieldArray, FieldSeq, fld);
                if ((temp == null) || (temp == ""))
                {
                    continue;
                }
                else
                {
                    employment_temp.CompanyName = temp.Trim();
                }

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(fld + 1));
                if ((temp == null) || (temp == ""))
                {
                    employment_temp.Address = "";
                }
                else
                {
                    employment_temp.Address = temp.Trim();
                }

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(fld + 2));
                if ((temp == null) || (temp == ""))
                {
                    employment_temp.City = "";
                }
                else
                {
                    employment_temp.City = temp.Trim();
                }

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(fld + 3));
                if ((temp == null) || (temp == ""))
                {
                    employment_temp.State = "";
                }
                else
                {
                    employment_temp.State = temp.Trim();
                }

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(fld + 4));
                if ((temp == null) || (temp == ""))
                {
                    employment_temp.Zip = "";
                }
                else
                {
                    employment_temp.Zip = temp.Trim();
                }

                switch (i)
                {
                    case 0: fld = 5546; break;
                    case 1: fld = 5556; break;
                    case 2: fld = 5586; break;
                    case 3: fld = 5606; break;
                    case 4: fld = 5616; break;
                    case 5: fld = 5626; break;
                    case 6: fld = 5636; break;
                    case 7: fld = 5646; break;
                    default: break;
                }

                temp_StartDate = "";
                temp = PNT.getPointField(FieldArray, FieldSeq, fld);
                if ((temp == null) || (temp == ""))
                {
                    temp_StartDate = "";
                    employment_temp.StartYear = 0;
                    employment_temp.StartMonth = 0;
                }
                else
                {
                    temp_StartDate = temp;
                    idx_s = temp_StartDate.Length;
                    if (idx_s > 6)
                    {
                        temp_c = temp_StartDate.Substring(1, 1);

                        if (temp_c == "\\")
                        {
                            temp_s = temp_StartDate.Substring(0, 1);
                        }
                        else
                        {
                            temp_s = temp_StartDate.Substring(0, 2);
                        }

                        status_b = decimal.TryParse(temp_s, out temp_d);

                        if (status_b == true)
                        {
                            employment_temp.StartMonth = temp_d;
                        }

                        temp_s = temp_StartDate.Substring(idx_s - 2, 2);
                        status_b = decimal.TryParse(temp_s, out temp_d);

                        if (status_b == true)
                        {
                            employment_temp.StartYear = temp_d;
                        }
                    }
                }

                temp_EndDate = "";
                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(fld + 1));
                if ((temp == null) || (temp == ""))
                {
                    temp_EndDate = "";
                    employment_temp.EndYear = 0;
                    employment_temp.EndMonth = 0;
                }
                else
                {
                    temp_EndDate = temp;
                    idx_s = temp_EndDate.Length;
                    if (idx_s > 6)
                    {
                        temp_c = temp_EndDate.Substring(1, 1);

                        if (temp_c == "\\")
                        {
                            temp_s = temp_EndDate.Substring(0, 1);
                        }
                        else
                        {
                            temp_s = temp_EndDate.Substring(0, 2);
                        }

                        status_b = decimal.TryParse(temp_s, out temp_d);

                        if (status_b == true)
                        {
                            employment_temp.EndMonth = temp_d;
                        }

                        temp_s = temp_EndDate.Substring(idx_s - 2, 2);
                        status_b = decimal.TryParse(temp_s, out temp_d);

                        if (status_b == true)
                        {
                            employment_temp.EndYear = temp_d;
                        }
                    }
                }

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(fld - 2));
                if ((temp == null) || (temp == ""))
                {
                    employment_temp.Position = "";
                }
                else
                {
                    employment_temp.Position = temp.Trim();
                }

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(fld - 1));
                if ((temp == null) || (temp == ""))
                {
                    employment_temp.Phone = "";
                }
                else
                {
                    employment_temp.Phone = temp.Trim();
                }

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(fld - 6));
                if ((temp == null) || (temp == ""))
                {
                    employment_temp.SelfEmployed = false;
                }
                else
                {
                    if (temp.ToUpper() == "X")
                        employment_temp.SelfEmployed = true;
                }

                employmentList.Add(employment_temp);

            }
        }

        void Add_Borrower_Income(ref Record.Income income, List<string> FieldArray, ArrayList FieldSeq)
        {
            string temp = "";
            decimal num = 0.00m;

            if (income == null)
                income = new Record.Income();

            income.ProspectIncomeId = 0;
            income.ContactId = 0;
            income.Salary = 0.0m;
            income.Overtime = 0.0m;
            income.Bonuses = 0.0m;
            income.Commission = 0.0m;
            income.Div_Int = 0.0m;
            income.NetRent = 0.0m;
            income.Other = 0.0m;
            income.EmplId = 0;

            temp = PNT.getPointField(FieldArray, FieldSeq, 600);
            if ((temp == null) || (temp == ""))
            {
                income.Salary = 0.0m;
            }
            else
            {
                if (decimal.TryParse(temp, out num) == false)
                    income.Salary = 0.0m;
                else
                {
                    income.Salary = num;
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 601);
            if ((temp == null) || (temp == ""))
            {
                income.Overtime = 0.0m;
            }
            else
            {
                if (decimal.TryParse(temp, out num) == false)
                    income.Overtime = 0.0m;
                else
                {
                    income.Overtime = num;
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 602);
            if ((temp == null) || (temp == ""))
            {
                income.Bonuses = 0.0m;
            }
            else
            {
                if (decimal.TryParse(temp, out num) == false)
                    income.Bonuses = 0.0m;
                else
                {
                    income.Bonuses = num;
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 603);
            if ((temp == null) || (temp == ""))
            {
                income.Commission = 0.0m;
            }
            else
            {
                if (decimal.TryParse(temp, out num) == false)
                    income.Commission = 0.0m;
                else
                {
                    income.Commission = num;
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 604);
            if ((temp == null) || (temp == ""))
            {
                income.Div_Int = 0.0m;
            }
            else
            {
                if (decimal.TryParse(temp, out num) == false)
                    income.Div_Int = 0.0m;
                else
                {
                    income.Div_Int = num;
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 605);
            if ((temp == null) || (temp == ""))
            {
                income.NetRent = 0.0m;
            }
            else
            {
                if (decimal.TryParse(temp, out num) == false)
                    income.NetRent = 0.0m;
                else
                {
                    income.NetRent = num;
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 606);
            if ((temp == null) || (temp == ""))
            {
                income.Other = 0.0m;
            }
            else
            {
                if (decimal.TryParse(temp, out num) == false)
                    income.Other = 0.0m;
                else
                {
                    income.Other = num;
                }
            }

        }

        void Add_Borrower_OtherIncome(ref List<Record.OtherIncome> otherincome, List<string> FieldArray, ArrayList FieldSeq)
        {
            string temp = "";
            decimal num = 0.0m;
            string temp_s = "";

            List<Record.OtherIncome> otherincomeList = otherincome;

            if (otherincomeList == null)
                otherincomeList = new List<Record.OtherIncome>();

            Record.OtherIncome otherincome_temp = new Record.OtherIncome();

            otherincome_temp.ProspectOtherIncomeId = 0;
            otherincome_temp.ContactId = 0;
            otherincome_temp.Type = "";
            otherincome_temp.MonthlyIncome = 0.0m;

            temp = PNT.getPointField(FieldArray, FieldSeq, 1230);
            if ((temp == null) || (temp == ""))
            {
                return;
            }
            else
            {
                if (temp.Trim().ToUpper() == "B")
                {
                    temp_s = PNT.getPointField(FieldArray, FieldSeq, 1231);
                    otherincome_temp.Type = temp_s;

                    temp_s = PNT.getPointField(FieldArray, FieldSeq, 1232);
                    if (decimal.TryParse(temp_s, out num) == false)
                        otherincome_temp.MonthlyIncome = 0.0m;
                    else
                    {
                        otherincome_temp.MonthlyIncome = num;
                        otherincomeList.Add(otherincome_temp);
                        otherincome_temp = null;
                        otherincome_temp = new Record.OtherIncome();
                    }
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 1233);
            if ((temp == null) || (temp == ""))
            {
                return;
            }
            else
            {
                if (temp.Trim().ToUpper() == "B")
                {
                    temp_s = PNT.getPointField(FieldArray, FieldSeq, 1234);
                    otherincome_temp.Type = temp_s;

                    temp_s = PNT.getPointField(FieldArray, FieldSeq, 1235);
                    if (decimal.TryParse(temp_s, out num) == false)
                        otherincome_temp.MonthlyIncome = 0.0m;
                    else
                    {
                        otherincome_temp.MonthlyIncome = num;
                        otherincomeList.Add(otherincome_temp);
                        otherincome_temp = null;
                        otherincome_temp = new Record.OtherIncome();
                    }
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 1236);
            if ((temp == null) || (temp == ""))
            {
                return;
            }
            else
            {
                if (temp.Trim().ToUpper() == "B")
                {
                    temp_s = PNT.getPointField(FieldArray, FieldSeq, 1237);
                    otherincome_temp.Type = temp_s;

                    temp_s = PNT.getPointField(FieldArray, FieldSeq, 1238);
                    if (decimal.TryParse(temp_s, out num) == false)
                        otherincome_temp.MonthlyIncome = 0.0m;
                    else
                    {
                        otherincome_temp.MonthlyIncome = num;
                        otherincomeList.Add(otherincome_temp);
                        otherincome_temp = null;
                    }
                }
            }
        }

        void Add_Borrower_Assets(ref List<Record.Assets> assets, List<string> FieldArray, ArrayList FieldSeq)
        {
            decimal num = 0.0m;

            List<Record.Assets> assetsList = assets;

            if (assetsList == null)
                assetsList = new List<Record.Assets>();

            Record.Assets assets_temp = new Record.Assets();

            assets_temp.ProspectAssetId = 0;
            assets_temp.ContactId = 0;
            assets_temp.Name = "";
            assets_temp.Account = "";
            assets_temp.Amount = 0.0m;
            assets_temp.Type = "";

            string temp = "";
            int i = 0;
            short nmFld = 1294, noFld = 1298, vFld = 1299, tFld = 8880;
            for (i = 0; i < 20; i++)
            {
                assets_temp = null;
                assets_temp = new Record.Assets();

                if (i >= 11)
                {
                    nmFld = 3854;
                    noFld = 3858;
                    vFld = 3859;
                }

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(nmFld + (i * 6)));
                if ((temp == null) || (temp == ""))
                    continue;
                assets_temp.Name = temp.Trim();

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(noFld + (i * 6)));
                if ((temp == null) || (temp == ""))
                    assets_temp.Account = "";
                else
                    assets_temp.Account = temp.Trim();

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(vFld + (i * 6)));
                if ((temp == null) || (temp == ""))
                    assets_temp.Amount = 0.0m;
                else
                {
                    if (decimal.TryParse(temp, out num) == false)
                        assets_temp.Amount = 0.0m;
                    else
                    {
                        assets_temp.Amount = num;
                    }
                }

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(tFld + i));
                if ((temp == null) || (temp == ""))
                    assets_temp.Type = "";
                else
                    assets_temp.Type = temp.Trim();
                if ((assets_temp != null) && (assets_temp.Name != ""))
                {
                    assetsList.Add(assets_temp);
                }
            }

        }

        void Add_CoBorrower_Employment(ref List<Record.Employment> employment, List<string> FieldArray, ArrayList FieldSeq)
        {
            string temp = "";
            string temp_c = "";
            string temp_s = "";
            int idx_s = 0;
            bool status_b = true;
            decimal temp_d = 0.0m;

            List<Record.Employment> employmentList = employment;

            if (employmentList == null)
                employmentList = new List<Record.Employment>();

            Record.Employment employment_temp = new Record.Employment();

            employment_temp.EmplId = 0;
            employment_temp.ContactId = 0;
            employment_temp.SelfEmployed = true;
            employment_temp.Position = "";
            employment_temp.StartYear = 0;
            employment_temp.StartMonth = 0;
            employment_temp.EndYear = 0;
            employment_temp.EndMonth = 0;
            employment_temp.YearsOnWork = 0;
            employment_temp.Phone = "";
            employment_temp.ContactBranchId = 0;
            employment_temp.CompanyName = "";
            employment_temp.Address = "";
            employment_temp.City = "";
            employment_temp.State = "";
            employment_temp.Zip = "";

            temp = PNT.getPointField(FieldArray, FieldSeq, 198);
            if ((temp == null) || (temp == ""))
            {
                employment_temp.CompanyName = "";
            }
            else
            {
                employment_temp.CompanyName = temp.Trim();
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 199);
            if ((temp == null) || (temp == ""))
            {
                employment_temp.Address = "";
            }
            else
            {
                employment_temp.Address = temp.Trim();
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 190);
            if ((temp == null) || (temp == ""))
            {
                employment_temp.City = "";
            }
            else
            {
                employment_temp.City = temp.Trim();
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 192);
            if ((temp == null) || (temp == ""))
            {
                employment_temp.State = "";
            }
            else
            {
                employment_temp.State = temp.Trim();
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 193);
            if ((temp == null) || (temp == ""))
            {
                employment_temp.Zip = "";
            }
            else
            {
                employment_temp.Zip = temp.Trim();
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 191);
            if ((temp == null) || (temp == ""))
            {
                employment_temp.SelfEmployed = false;
            }
            else
            {
                if (temp.ToUpper() == "X")
                    employment_temp.SelfEmployed = true;
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 185);
            if ((temp == null) || (temp == ""))
            {
                employment_temp.Position = "";
            }
            else
            {
                employment_temp.Position = temp.Trim();
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 186);
            if ((temp == null) || (temp == ""))
            {
                employment_temp.Phone = "";
            }
            else
            {
                employment_temp.Phone = temp.Trim();
            }

            string temp_StartDate = "";
            string temp_EndDate = "";
            string temp_YearsOnWork = "";

            temp = PNT.getPointField(FieldArray, FieldSeq, 188);
            if ((temp == null) || (temp == ""))
            {
                temp_YearsOnWork = "";
                employment_temp.YearsOnWork = 0;
            }
            else
            {
                temp_YearsOnWork = temp;
                idx_s = temp_YearsOnWork.Length;
                if (idx_s > 0)
                {
                    status_b = decimal.TryParse(temp_YearsOnWork, out temp_d);

                    if (status_b == true)
                    {
                        employment_temp.YearsOnWork = temp_d;
                    }
                }
            }

            employmentList.Add(employment_temp);

            int i = 0;
            short fld = 0;
            for (i = 0; i < 8; i++)
            {
                employment_temp = null;
                employment_temp = new Record.Employment();

                fld = (short)(5260 + (i * 5));
                if (i == 6)
                    fld = 5010;
                if (i == 7)
                    fld = 5015;
                temp = PNT.getPointField(FieldArray, FieldSeq, fld);
                if ((temp == null) || (temp == ""))
                {
                    continue;
                }
                else
                {
                    employment_temp.CompanyName = temp.Trim();
                }

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(fld + 1));
                if ((temp == null) || (temp == ""))
                {
                    employment_temp.Address = "";
                }
                else
                {
                    employment_temp.Address = temp.Trim();
                }

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(fld + 2));
                if ((temp == null) || (temp == ""))
                {
                    employment_temp.City = "";
                }
                else
                {
                    employment_temp.City = temp.Trim();
                }

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(fld + 3));
                if ((temp == null) || (temp == ""))
                {
                    employment_temp.State = "";
                }
                else
                {
                    employment_temp.State = temp.Trim();
                }

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(fld + 4));
                if ((temp == null) || (temp == ""))
                {
                    employment_temp.Zip = "";
                }
                else
                {
                    employment_temp.Zip = temp.Trim();
                }

                switch (i)
                {
                    case 0: fld = 5566; break;
                    case 1: fld = 5576; break;
                    case 2: fld = 5596; break;
                    case 3: fld = 5656; break;
                    case 4: fld = 5666; break;
                    case 5: fld = 5676; break;
                    case 6: fld = 5686; break;
                    case 7: fld = 5696; break;
                    default: break;
                }

                temp_StartDate = "";
                temp = PNT.getPointField(FieldArray, FieldSeq, fld);
                if ((temp == null) || (temp == ""))
                {
                    temp_StartDate = "";
                    employment_temp.StartYear = 0;
                    employment_temp.StartMonth = 0;
                }
                else
                {
                    temp_StartDate = temp;
                    idx_s = temp_StartDate.Length;
                    if (idx_s > 6)
                    {
                        temp_c = temp_StartDate.Substring(1, 1);

                        if (temp_c == "\\")
                        {
                            temp_s = temp_StartDate.Substring(0, 1);
                        }
                        else
                        {
                            temp_s = temp_StartDate.Substring(0, 2);
                        }

                        status_b = decimal.TryParse(temp_s, out temp_d);

                        if (status_b == true)
                        {
                            employment_temp.StartMonth = temp_d;
                        }

                        temp_s = temp_StartDate.Substring(idx_s - 2, 2);
                        status_b = decimal.TryParse(temp_s, out temp_d);

                        if (status_b == true)
                        {
                            employment_temp.StartYear = temp_d;
                        }
                    }
                }

                temp_EndDate = "";
                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(fld + 1));
                if ((temp == null) || (temp == ""))
                {
                    temp_EndDate = "";
                    employment_temp.EndMonth = 0.0m;
                    employment_temp.EndYear = 0.0m;
                }
                else
                {
                    temp_EndDate = temp;
                    idx_s = temp_EndDate.Length;
                    if (idx_s > 6)
                    {
                        temp_c = temp_EndDate.Substring(1, 1);

                        if (temp_c == "\\")
                        {
                            temp_s = temp_EndDate.Substring(0, 1);
                        }
                        else
                        {
                            temp_s = temp_EndDate.Substring(0, 2);
                        }

                        status_b = decimal.TryParse(temp_s, out temp_d);

                        if (status_b == true)
                        {
                            employment_temp.EndMonth = temp_d;
                        }

                        temp_s = temp_EndDate.Substring(idx_s - 2, 2);
                        status_b = decimal.TryParse(temp_s, out temp_d);

                        if (status_b == true)
                        {
                            employment_temp.EndYear = temp_d;
                        }
                    }
                }

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(fld - 2));
                if ((temp == null) || (temp == ""))
                {
                    employment_temp.Position = "";
                }
                else
                {
                    employment_temp.Position = temp.Trim();
                }

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(fld - 1));
                if ((temp == null) || (temp == ""))
                {
                    employment_temp.Phone = "";
                }
                else
                {
                    employment_temp.Phone = temp.Trim();
                }

                temp = PNT.getPointField(FieldArray, FieldSeq, (short)(fld - 6));
                if ((temp == null) || (temp == ""))
                {
                    employment_temp.SelfEmployed = false;
                }
                else
                {
                    if (temp.ToUpper() == "X")
                        employment_temp.SelfEmployed = true;
                }

                employmentList.Add(employment_temp);
            }
        }

        void Add_CoBorrower_Income(ref Record.Income income, List<string> FieldArray, ArrayList FieldSeq)
        {
            string temp = "";
            decimal num = 0.0m;

            if (income == null)
                income = new Record.Income();

            income.ProspectIncomeId = 0;
            income.ContactId = 0;
            income.Salary = 0.0m;
            income.Overtime = 0.0m;
            income.Bonuses = 0.0m;
            income.Commission = 0.0m;
            income.Div_Int = 0.0m;
            income.NetRent = 0.0m;
            income.Other = 0.0m;
            income.EmplId = 0;

            temp = PNT.getPointField(FieldArray, FieldSeq, 650);
            if ((temp == null) || (temp == ""))
            {
                income.Salary = 0.0m;
            }
            else
            {
                if (decimal.TryParse(temp, out num) == false)
                    income.Salary = 0.0m;
                else
                {
                    income.Salary = num;
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 651);
            if ((temp == null) || (temp == ""))
            {
                income.Overtime = 0.0m;
            }
            else
            {
                if (decimal.TryParse(temp, out num) == false)
                    income.Overtime = 0.0m;
                else
                {
                    income.Overtime = num;
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 652);
            if ((temp == null) || (temp == ""))
            {
                income.Bonuses = 0.0m;
            }
            else
            {
                if (decimal.TryParse(temp, out num) == false)
                    income.Bonuses = 0.0m;
                else
                {
                    income.Bonuses = num;
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 653);
            if ((temp == null) || (temp == ""))
            {
                income.Commission = 0.0m;
            }
            else
            {
                if (decimal.TryParse(temp, out num) == false)
                    income.Commission = 0.0m;
                else
                {
                    income.Commission = num;
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 654);
            if ((temp == null) || (temp == ""))
            {
                income.Div_Int = 0.0m;
            }
            else
            {
                if (decimal.TryParse(temp, out num) == false)
                    income.Div_Int = 0.0m;
                else
                {
                    income.Div_Int = num;
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 655);
            if ((temp == null) || (temp == ""))
            {
                income.NetRent = 0.0m;
            }
            else
            {
                if (decimal.TryParse(temp, out num) == false)
                    income.NetRent = 0.0m;
                else
                {
                    income.NetRent = num;
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 656);
            if ((temp == null) || (temp == ""))
            {
                income.Other = 0.0m;
            }
            else
            {
                if (decimal.TryParse(temp, out num) == false)
                    income.Other = 0.0m;
                else
                {
                    income.Other = num;
                }
            }
        }

        void Add_CoBorrower_OtherIncome(ref List<Record.OtherIncome> otherincome, List<string> FieldArray, ArrayList FieldSeq)
        {
            string temp = "";
            decimal num = 0.0m;
            string temp_s = "";

            List<Record.OtherIncome> otherincomeList = otherincome;

            if (otherincomeList == null)
                otherincomeList = new List<Record.OtherIncome>();

            Record.OtherIncome otherincome_temp = new Record.OtherIncome();

            otherincome_temp.ProspectOtherIncomeId = 0;
            otherincome_temp.ContactId = 0;
            otherincome_temp.Type = "";
            otherincome_temp.MonthlyIncome = 0.0m;

            temp = PNT.getPointField(FieldArray, FieldSeq, 1230);
            if ((temp == null) || (temp == ""))
            {
                return;
            }
            else
            {
                if (temp.Trim().ToUpper() == "C")
                {
                    temp_s = PNT.getPointField(FieldArray, FieldSeq, 1231);
                    otherincome_temp.Type = temp_s;

                    temp_s = PNT.getPointField(FieldArray, FieldSeq, 1232);
                    if (decimal.TryParse(temp_s, out num) == false)
                        otherincome_temp.MonthlyIncome = 0.0m;
                    else
                    {
                        otherincome_temp.MonthlyIncome = num;
                        otherincomeList.Add(otherincome_temp);
                        otherincome_temp = null;
                        otherincome_temp = new Record.OtherIncome();
                    }
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 1233);
            if ((temp == null) || (temp == ""))
            {
                return;
            }
            else
            {
                if (temp.Trim().ToUpper() == "C")
                {
                    temp_s = PNT.getPointField(FieldArray, FieldSeq, 1234);
                    otherincome_temp.Type = temp_s;

                    temp_s = PNT.getPointField(FieldArray, FieldSeq, 1235);
                    if (decimal.TryParse(temp_s, out num) == false)
                        otherincome_temp.MonthlyIncome = 0.0m;
                    else
                    {
                        otherincome_temp.MonthlyIncome = num;
                        otherincomeList.Add(otherincome_temp);
                        otherincome_temp = null;
                        otherincome_temp = new Record.OtherIncome();
                    }
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 1236);
            if ((temp == null) || (temp == ""))
            {
                return;
            }
            else
            {
                if (temp.Trim().ToUpper() == "C")
                {
                    temp_s = PNT.getPointField(FieldArray, FieldSeq, 1237);
                    otherincome_temp.Type = temp_s;

                    temp_s = PNT.getPointField(FieldArray, FieldSeq, 1238);
                    if (decimal.TryParse(temp_s, out num) == false)
                        otherincome_temp.MonthlyIncome = 0.0m;
                    else
                    {
                        otherincome_temp.MonthlyIncome = num;
                        otherincomeList.Add(otherincome_temp);
                        otherincome_temp = null;
                    }
                }
            }
        }

        void Add_CoBorrower_Assets(ref List<Record.Assets> assets, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (assets == null)
                assets = new List<Record.Assets>();

            Record.Assets assets_temp = new Record.Assets();

            assets_temp.ProspectAssetId = 0;
            assets_temp.ContactId = 0;
            assets_temp.Name = "";
            assets_temp.Account = "";
            assets_temp.Amount = 0.0m;
            assets_temp.Type = "";
        }

        void AddBorrower(ref Record.Contacts contacts, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (contacts == null)
                contacts = new Record.Contacts();

            contacts.FirstName = PNT.getPointField(FieldArray, FieldSeq, 100);               //   1
            if (string.IsNullOrEmpty(contacts.FirstName))
                contacts.FirstName = PNT.getPointField(FieldArray, FieldSeq, 3);
            contacts.MiddleName = PNT.getPointField(FieldArray, FieldSeq, 117);           //   2 
            if (string.IsNullOrEmpty(contacts.MiddleName))
                contacts.MiddleName = PNT.getPointField(FieldArray, FieldSeq, 11191);
            contacts.LastName = PNT.getPointField(FieldArray, FieldSeq, 101);               //   3
            if (string.IsNullOrEmpty(contacts.LastName))
                contacts.LastName = PNT.getPointField(FieldArray, FieldSeq, 2);
            contacts.NickName = PNT.getPointField(FieldArray, FieldSeq, 11234);           //   4 
            //         Title                                                                                5
            contacts.GenerationCode = PNT.getPointField(FieldArray, FieldSeq, 119);     //    6
            contacts.SSN = PNT.getPointField(FieldArray, FieldSeq, 108);                          //   7       
            if (contacts.SSN == null)
                contacts.SSN = "";
            contacts.HomePhone = PNT.getPointField(FieldArray, FieldSeq, 106);             //   8      
            contacts.CellPhone = PNT.getPointField(FieldArray, FieldSeq, 139);                 //   9 
            contacts.BusinessPhone = PNT.getPointField(FieldArray, FieldSeq, 136);        //   10
            contacts.Fax = PNT.getPointField(FieldArray, FieldSeq, 107);                            //    11
            contacts.Email = PNT.getPointField(FieldArray, FieldSeq, 112);                         //    12
            string tempDate = PNT.getPointField(FieldArray, FieldSeq, 118);
            if (IsDateValid(ref tempDate))
                contacts.DOB = tempDate;
            else
                contacts.DOB = string.Empty;                                                                            //    13
            contacts.Experian = PNT.getPointField(FieldArray, FieldSeq, 5032);                 //    14 
            contacts.TransUnion = PNT.getPointField(FieldArray, FieldSeq, 5034);             //    15
            contacts.Equifax = PNT.getPointField(FieldArray, FieldSeq, 5036);                    //    16         
            contacts.MailingAddr = PNT.getPointField(FieldArray, FieldSeq, 102);               //    17         
            contacts.MailingCity = PNT.getPointField(FieldArray, FieldSeq, 103);                 //    18           
            contacts.MailingState = PNT.getPointField(FieldArray, FieldSeq, 104);              //     19             
            contacts.MailingZip = PNT.getPointField(FieldArray, FieldSeq, 105);             //     20 

            contacts.employment = new List<Record.Employment>();
            Add_Borrower_Employment(ref contacts.employment, FieldArray, FieldSeq);

            contacts.income = new Record.Income();
            Add_Borrower_Income(ref contacts.income, FieldArray, FieldSeq);

            contacts.otherincome = new List<Record.OtherIncome>();
            Add_Borrower_OtherIncome(ref contacts.otherincome, FieldArray, FieldSeq);

            contacts.assets = new List<Record.Assets>();
            Add_Borrower_Assets(ref contacts.assets, FieldArray, FieldSeq);
        }

        void AddCoBorrower(ref Record.Contacts contacts, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (contacts == null)
                contacts = new Record.Contacts();

            if ((PNT.getPointField(FieldArray, FieldSeq, 150) == null) || (PNT.getPointField(FieldArray, FieldSeq, 150) == String.Empty))
                return;
            if ((PNT.getPointField(FieldArray, FieldSeq, 151) == null) || (PNT.getPointField(FieldArray, FieldSeq, 151) == String.Empty))
                return;
            contacts.FirstName = PNT.getPointField(FieldArray, FieldSeq, 150);               //   1
            contacts.MiddleName = PNT.getPointField(FieldArray, FieldSeq, 167);           //   2 
            contacts.LastName = PNT.getPointField(FieldArray, FieldSeq, 151);               //   3
            contacts.NickName = PNT.getPointField(FieldArray, FieldSeq, 11236);           //   4 
            //         Title                                                                                5
            contacts.GenerationCode = PNT.getPointField(FieldArray, FieldSeq, 169);      //  6
            contacts.SSN = PNT.getPointField(FieldArray, FieldSeq, 158);                          //   7       
            if (contacts.SSN == null)
                contacts.SSN = "";
            contacts.HomePhone = PNT.getPointField(FieldArray, FieldSeq, 156);             //   8      
            contacts.CellPhone = PNT.getPointField(FieldArray, FieldSeq, 189);                 //   9 
            contacts.BusinessPhone = PNT.getPointField(FieldArray, FieldSeq, 186);        //   10
            contacts.Fax = PNT.getPointField(FieldArray, FieldSeq, 157);                            //    11
            contacts.Email = PNT.getPointField(FieldArray, FieldSeq, 162);                         //    12
            string tempDate = PNT.getPointField(FieldArray, FieldSeq, 168);                      // 13
            if (IsDateValid(ref tempDate))
                contacts.DOB = tempDate;
            else
                contacts.DOB = string.Empty;
            contacts.Experian = PNT.getPointField(FieldArray, FieldSeq, 5033);                 //    14 
            contacts.TransUnion = PNT.getPointField(FieldArray, FieldSeq, 5035);             //    15
            contacts.Equifax = PNT.getPointField(FieldArray, FieldSeq, 5037);                    //    16         
            contacts.MailingAddr = PNT.getPointField(FieldArray, FieldSeq, 152);               //    17         
            contacts.MailingCity = PNT.getPointField(FieldArray, FieldSeq, 153);                 //    18           
            contacts.MailingState = PNT.getPointField(FieldArray, FieldSeq, 154);              //     19             
            contacts.MailingZip = PNT.getPointField(FieldArray, FieldSeq, 155);                  //     20    
            //   ContactCompanyId                                                   //     21
            //   WebAccountId 

            contacts.employment = new List<Record.Employment>();
            Add_CoBorrower_Employment(ref contacts.employment, FieldArray, FieldSeq);

            contacts.income = new Record.Income();
            Add_CoBorrower_Income(ref contacts.income, FieldArray, FieldSeq);

            contacts.otherincome = new List<Record.OtherIncome>();
            Add_CoBorrower_OtherIncome(ref contacts.otherincome, FieldArray, FieldSeq);

            contacts.assets = new List<Record.Assets>();
            Add_CoBorrower_Assets(ref contacts.assets, FieldArray, FieldSeq);
        }
        #endregion
        #region Loan Agents
        void AddAgents(ref Record.LoanAgents agents, List<string> FieldArray, ArrayList FieldSeq)
        {
            AddAppraiser(ref agents, FieldArray, FieldSeq);
            AddBroker(ref agents, FieldArray, FieldSeq);
            AddBuilder(ref agents, FieldArray, FieldSeq);
            AddBuyerAgent(ref agents, FieldArray, FieldSeq);
            AddBuyerAttorney(ref agents, FieldArray, FieldSeq);
            AddClosingAgent(ref agents, FieldArray, FieldSeq);
            AddFloodInsurance(ref agents, FieldArray, FieldSeq);
            AddHazardInsurance(ref agents, FieldArray, FieldSeq);
            AddInvestor(ref agents, FieldArray, FieldSeq);
            AddLender(ref agents, FieldArray, FieldSeq);
            AddListingAgent(ref agents, FieldArray, FieldSeq);
            AddMortgageInsurance(ref agents, FieldArray, FieldSeq);
            AddSellingAgent(ref agents, FieldArray, FieldSeq);
            AddTitle(ref agents, FieldArray, FieldSeq);
        }

        void AddAppraiser(ref Record.LoanAgents agents, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (agents == null)
                agents = new Record.LoanAgents();

            string temp = "";
            temp = PNT.getPointField(FieldArray, FieldSeq, 330);
            if ((temp == null) || (temp.Trim() == string.Empty))
                return;
            if (agents.appraiser == null)
                agents.appraiser = new Record.Agent();
            if (agents.appraiser.Contact == null)
                agents.appraiser.Contact = new Record.Contacts();

            ParseName(temp, ref agents.appraiser.Contact.FirstName, ref agents.appraiser.Contact.LastName, ref agents.appraiser.Contact.MiddleName);
            temp = PNT.getPointField(FieldArray, FieldSeq, 331);
            if ((temp == null) || (temp == string.Empty))
                agents.appraiser.Company = "";
            else
                agents.appraiser.Company = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 332);
            if ((temp == null) || (temp == string.Empty))
                agents.appraiser.Contact.BusinessPhone = "";
            else
                agents.appraiser.Contact.BusinessPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 333);
            if ((temp == null) || (temp == string.Empty))
                agents.appraiser.Contact.MailingAddr = "";
            else
                agents.appraiser.Contact.MailingAddr = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 334);
            if ((temp == null) || (temp == string.Empty))
            {
                agents.appraiser.Contact.MailingCity = "";
                agents.appraiser.Contact.MailingState = "";
                agents.appraiser.Contact.MailingZip = "";
            }
            else
                getCityStateZip(temp, ref agents.appraiser.Contact.MailingCity, ref agents.appraiser.Contact.MailingState, ref agents.appraiser.Contact.MailingZip);

            temp = PNT.getPointField(FieldArray, FieldSeq, 335);
            if ((temp == null) || (temp == string.Empty))
                agents.appraiser.Contact.Fax = "";
            else
                agents.appraiser.Contact.Fax = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12367);
            if ((temp == null) || (temp == string.Empty))
                agents.appraiser.Contact.CellPhone = "";
            else
                agents.appraiser.Contact.CellPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12368);
            if ((temp == null) || (temp == string.Empty))
                agents.appraiser.Contact.Email = "";
            else
                agents.appraiser.Contact.Email = temp.Trim();
        }

        void AddBroker(ref Record.LoanAgents agents, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (agents == null)
                agents = new Record.LoanAgents();

            string temp = "";
            temp = PNT.getPointField(FieldArray, FieldSeq, 6371);
            if ((temp == null) || (temp.Trim() == string.Empty))
                return;
            if (agents.mortgage_broker == null)
                agents.mortgage_broker = new Record.Agent();
            if (agents.mortgage_broker.Contact == null)
                agents.mortgage_broker.Contact = new Record.Contacts();

            ParseName(temp, ref agents.mortgage_broker.Contact.FirstName, ref agents.mortgage_broker.Contact.LastName, ref agents.mortgage_broker.Contact.MiddleName);

            temp = PNT.getPointField(FieldArray, FieldSeq, 6370);
            if ((temp == null) || (temp == string.Empty))
                agents.mortgage_broker.Company = "";
            else
                agents.mortgage_broker.Company = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6374);
            if ((temp == null) || (temp == string.Empty))
                agents.mortgage_broker.Contact.BusinessPhone = "";
            else
                agents.mortgage_broker.Contact.BusinessPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6372);
            if ((temp == null) || (temp == string.Empty))
                agents.mortgage_broker.Contact.MailingAddr = "";
            else
                agents.mortgage_broker.Contact.MailingAddr = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6373);
            if ((temp == null) || (temp == string.Empty))
            {
                agents.mortgage_broker.Contact.MailingCity = "";
                agents.mortgage_broker.Contact.MailingState = "";
                agents.mortgage_broker.Contact.MailingZip = "";
            }
            else
                getCityStateZip(temp, ref agents.mortgage_broker.Contact.MailingCity, ref agents.mortgage_broker.Contact.MailingState, ref agents.mortgage_broker.Contact.MailingZip);

            temp = PNT.getPointField(FieldArray, FieldSeq, 6375);
            if ((temp == null) || (temp == string.Empty))
                agents.mortgage_broker.Contact.Fax = "";
            else
                agents.mortgage_broker.Contact.Fax = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12355);
            if ((temp == null) || (temp == string.Empty))
                agents.mortgage_broker.Contact.CellPhone = "";
            else
                agents.mortgage_broker.Contact.CellPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12356);
            if ((temp == null) || (temp == string.Empty))
                agents.mortgage_broker.Contact.Email = "";
            else
                agents.mortgage_broker.Contact.Email = temp.Trim();
        }

        void AddBuilder(ref Record.LoanAgents agents, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (agents == null)
                agents = new Record.LoanAgents();

            string temp = "";
            temp = PNT.getPointField(FieldArray, FieldSeq, 360);
            if ((temp == null) || (temp.Trim() == string.Empty))
                return;
            if (agents.builder == null)
                agents.builder = new Record.Agent();
            if (agents.builder.Contact == null)
                agents.builder.Contact = new Record.Contacts();

            ParseName(temp, ref agents.builder.Contact.FirstName, ref agents.builder.Contact.LastName, ref agents.builder.Contact.MiddleName);
            temp = PNT.getPointField(FieldArray, FieldSeq, 361);
            if ((temp == null) || (temp == string.Empty))
                agents.builder.Company = "";
            else
                agents.builder.Company = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 362);
            if ((temp == null) || (temp == string.Empty))
                agents.builder.Contact.BusinessPhone = "";
            else
                agents.builder.Contact.BusinessPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 363);
            if ((temp == null) || (temp == string.Empty))
                agents.builder.Contact.MailingAddr = "";
            else
                agents.builder.Contact.MailingAddr = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 364);
            if ((temp == null) || (temp == string.Empty))
            {
                agents.builder.Contact.MailingCity = "";
                agents.builder.Contact.MailingState = "";
                agents.builder.Contact.MailingZip = "";
            }
            else
                getCityStateZip(temp, ref agents.builder.Contact.MailingCity, ref agents.builder.Contact.MailingState, ref agents.builder.Contact.MailingZip);

            temp = PNT.getPointField(FieldArray, FieldSeq, 368);
            if ((temp == null) || (temp == string.Empty))
                agents.builder.Contact.Fax = "";
            else
                agents.builder.Contact.Fax = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12381);
            if ((temp == null) || (temp == string.Empty))
                agents.builder.Contact.CellPhone = "";
            else
                agents.builder.Contact.CellPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12382);
            if ((temp == null) || (temp == string.Empty))
                agents.builder.Contact.Email = "";
            else
                agents.builder.Contact.Email = temp.Trim();
        }

        void AddBuyerAgent(ref Record.LoanAgents agents, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (agents == null)
                agents = new Record.LoanAgents();

            string temp = "";
            temp = PNT.getPointField(FieldArray, FieldSeq, 6191);
            if ((temp == null) || (temp.Trim() == string.Empty))
                return;
            if (agents.buyers_agent == null)
                agents.buyers_agent = new Record.Agent();
            if (agents.buyers_agent.Contact == null)
                agents.buyers_agent.Contact = new Record.Contacts();

            ParseName(temp, ref agents.buyers_agent.Contact.FirstName, ref agents.buyers_agent.Contact.LastName, ref agents.buyers_agent.Contact.MiddleName);
            temp = PNT.getPointField(FieldArray, FieldSeq, 6196);
            if ((temp == null) || (temp == string.Empty))
                agents.buyers_agent.Company = "";
            else
                agents.buyers_agent.Company = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6192);
            if ((temp == null) || (temp == string.Empty))
                agents.buyers_agent.Contact.BusinessPhone = "";
            else
                agents.buyers_agent.Contact.BusinessPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6197);
            if ((temp == null) || (temp == string.Empty))
                agents.buyers_agent.Contact.MailingAddr = "";
            else
                agents.buyers_agent.Contact.MailingAddr = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6198);
            if ((temp == null) || (temp == string.Empty))
            {
                agents.buyers_agent.Contact.MailingCity = "";
                agents.buyers_agent.Contact.MailingState = "";
                agents.buyers_agent.Contact.MailingZip = "";
            }
            else
                getCityStateZip(temp, ref agents.buyers_agent.Contact.MailingCity, ref agents.buyers_agent.Contact.MailingState, ref agents.buyers_agent.Contact.MailingZip);

            temp = PNT.getPointField(FieldArray, FieldSeq, 6193);
            if ((temp == null) || (temp == string.Empty))
                agents.buyers_agent.Contact.Fax = "";
            else
                agents.buyers_agent.Contact.Fax = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6194);
            if ((temp == null) || (temp == string.Empty))
                agents.buyers_agent.Contact.CellPhone = "";
            else
                agents.buyers_agent.Contact.CellPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6195);
            if ((temp == null) || (temp == string.Empty))
                agents.buyers_agent.Contact.Email = "";
            else
                agents.buyers_agent.Contact.Email = temp.Trim();
        }

        void AddBuyerAttorney(ref Record.LoanAgents agents, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (agents == null)
                agents = new Record.LoanAgents();

            string temp = "";
            temp = PNT.getPointField(FieldArray, FieldSeq, 430);
            if ((temp == null) || (temp.Trim() == string.Empty))
                return;

            if (agents.buyers_attorney == null)
                agents.buyers_attorney = new Record.Agent();

            if (agents.buyers_attorney.Contact == null)
                agents.buyers_attorney.Contact = new Record.Contacts();

            ParseName(temp, ref agents.buyers_attorney.Contact.FirstName, ref agents.buyers_attorney.Contact.LastName, ref agents.buyers_attorney.Contact.MiddleName);
            temp = PNT.getPointField(FieldArray, FieldSeq, 431);
            if ((temp == null) || (temp == string.Empty))
                agents.buyers_attorney.Company = "";
            else
                agents.buyers_attorney.Company = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 432);
            if ((temp == null) || (temp == string.Empty))
                agents.buyers_attorney.Contact.BusinessPhone = "";
            else
                agents.buyers_attorney.Contact.BusinessPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 433);
            if ((temp == null) || (temp == string.Empty))
                agents.buyers_attorney.Contact.MailingAddr = "";
            else
                agents.buyers_attorney.Contact.MailingAddr = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 434);
            if ((temp == null) || (temp == string.Empty))
            {
                agents.buyers_attorney.Contact.MailingCity = "";
                agents.buyers_attorney.Contact.MailingState = "";
                agents.buyers_attorney.Contact.MailingZip = "";
            }
            else
                getCityStateZip(temp, ref agents.buyers_attorney.Contact.MailingCity, ref agents.buyers_attorney.Contact.MailingState, ref agents.buyers_attorney.Contact.MailingZip);

            temp = PNT.getPointField(FieldArray, FieldSeq, 435);
            if ((temp == null) || (temp == string.Empty))
                agents.buyers_attorney.Contact.Fax = "";
            else
                agents.buyers_attorney.Contact.Fax = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12373);
            if ((temp == null) || (temp == string.Empty))
                agents.buyers_attorney.Contact.CellPhone = "";
            else
                agents.buyers_attorney.Contact.CellPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12374);
            if ((temp == null) || (temp == string.Empty))
                agents.buyers_attorney.Contact.Email = "";
            else
                agents.buyers_attorney.Contact.Email = temp.Trim();
        }

        void AddClosingAgent(ref Record.LoanAgents agents, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (agents == null)
                agents = new Record.LoanAgents();

            string temp = "";
            temp = PNT.getPointField(FieldArray, FieldSeq, 6110);
            if ((temp == null) || (temp.Trim() == string.Empty))
                return;
            if (agents.closing_agent == null)
                agents.closing_agent = new Record.Agent();
            if (agents.closing_agent.Contact == null)
                agents.closing_agent.Contact = new Record.Contacts();

            ParseName(temp, ref agents.closing_agent.Contact.FirstName, ref agents.closing_agent.Contact.LastName, ref agents.closing_agent.Contact.MiddleName);
            temp = PNT.getPointField(FieldArray, FieldSeq, 6111);
            if ((temp == null) || (temp == string.Empty))
                agents.closing_agent.Company = "";
            else
                agents.closing_agent.Company = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6112);
            if ((temp == null) || (temp == string.Empty))
                agents.closing_agent.Contact.BusinessPhone = "";
            else
                agents.closing_agent.Contact.BusinessPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6113);
            if ((temp == null) || (temp == string.Empty))
                agents.closing_agent.Contact.MailingAddr = "";
            else
                agents.closing_agent.Contact.MailingAddr = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6114);
            if ((temp == null) || (temp == string.Empty))
            {
                agents.closing_agent.Contact.MailingCity = "";
                agents.closing_agent.Contact.MailingState = "";
                agents.closing_agent.Contact.MailingZip = "";
            }
            else
                getCityStateZip(temp, ref agents.closing_agent.Contact.MailingCity, ref agents.closing_agent.Contact.MailingState, ref agents.closing_agent.Contact.MailingZip);

            temp = PNT.getPointField(FieldArray, FieldSeq, 6115);
            if ((temp == null) || (temp == string.Empty))
                agents.closing_agent.Contact.Fax = "";
            else
                agents.closing_agent.Contact.Fax = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12369);
            if ((temp == null) || (temp == string.Empty))
                agents.closing_agent.Contact.CellPhone = "";
            else
                agents.closing_agent.Contact.CellPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12370);
            if ((temp == null) || (temp == string.Empty))
                agents.closing_agent.Contact.Email = "";
            else
                agents.closing_agent.Contact.Email = temp.Trim();
        }

        void AddFloodInsurance(ref Record.LoanAgents agents, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (agents == null)
                agents = new Record.LoanAgents();

            string temp = "";
            temp = PNT.getPointField(FieldArray, FieldSeq, 12786);
            if ((temp == null) || (temp.Trim() == string.Empty))
                return;
            if (agents.flood_insurance == null)
                agents.flood_insurance = new Record.Agent();
            if (agents.flood_insurance.Contact == null)
                agents.flood_insurance.Contact = new Record.Contacts();

            ParseName(temp, ref agents.flood_insurance.Contact.FirstName, ref agents.flood_insurance.Contact.LastName, ref agents.flood_insurance.Contact.MiddleName);
            temp = PNT.getPointField(FieldArray, FieldSeq, 12787);
            if ((temp == null) || (temp == string.Empty))
                agents.flood_insurance.Company = "";
            else
                agents.flood_insurance.Company = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 13000);
            if ((temp == null) || (temp == string.Empty))
                agents.flood_insurance.Contact.BusinessPhone = "";
            else
                agents.flood_insurance.Contact.BusinessPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12762);
            if ((temp == null) || (temp == string.Empty))
                agents.flood_insurance.Contact.MailingAddr = "";
            else
                agents.flood_insurance.Contact.MailingAddr = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12763);
            if ((temp == null) || (temp == string.Empty))
                agents.flood_insurance.Contact.MailingCity = "";
            else
                agents.flood_insurance.Contact.MailingCity = temp.Trim();
            temp = PNT.getPointField(FieldArray, FieldSeq, 12764);
            if ((temp == null) || (temp == string.Empty))
                agents.flood_insurance.Contact.MailingState = "";
            else
                agents.flood_insurance.Contact.MailingState = temp.Trim();
            temp = PNT.getPointField(FieldArray, FieldSeq, 12765);
            if ((temp == null) || (temp == string.Empty))
                agents.flood_insurance.Contact.MailingZip = "";
            else
                agents.flood_insurance.Contact.MailingZip = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 13002);
            if ((temp == null) || (temp == string.Empty))
                agents.flood_insurance.Contact.Fax = "";
            else
                agents.flood_insurance.Contact.Fax = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 13001);
            if ((temp == null) || (temp == string.Empty))
                agents.flood_insurance.Contact.CellPhone = "";
            else
                agents.flood_insurance.Contact.CellPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 13003);
            if ((temp == null) || (temp == string.Empty))
                agents.flood_insurance.Contact.Email = "";
            else
                agents.flood_insurance.Contact.Email = temp.Trim();
        }

        void AddHazardInsurance(ref Record.LoanAgents agents, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (agents == null)
                agents = new Record.LoanAgents();

            string temp = "";
            temp = PNT.getPointField(FieldArray, FieldSeq, 450);
            if ((temp == null) || (temp.Trim() == string.Empty))
                return;
            if (agents.hazard_insurance == null)
                agents.hazard_insurance = new Record.Agent();
            if (agents.hazard_insurance.Contact == null)
                agents.hazard_insurance.Contact = new Record.Contacts();

            ParseName(temp, ref agents.hazard_insurance.Contact.FirstName, ref agents.hazard_insurance.Contact.LastName, ref agents.hazard_insurance.Contact.MiddleName);
            temp = PNT.getPointField(FieldArray, FieldSeq, 453);
            if ((temp == null) || (temp == string.Empty))
                agents.hazard_insurance.Company = "";
            else
                agents.hazard_insurance.Company = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 451);
            if ((temp == null) || (temp == string.Empty))
                agents.hazard_insurance.Contact.BusinessPhone = "";
            else
                agents.hazard_insurance.Contact.BusinessPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 454);
            if ((temp == null) || (temp == string.Empty))
                agents.hazard_insurance.Contact.MailingAddr = "";
            else
                agents.hazard_insurance.Contact.MailingAddr = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 455);
            if ((temp == null) || (temp == string.Empty))
            {
                agents.hazard_insurance.Contact.MailingCity = "";
                agents.hazard_insurance.Contact.MailingState = "";
                agents.hazard_insurance.Contact.MailingZip = "";
            }
            else
                getCityStateZip(temp, ref agents.hazard_insurance.Contact.MailingCity, ref agents.hazard_insurance.Contact.MailingState, ref agents.hazard_insurance.Contact.MailingZip);

            temp = PNT.getPointField(FieldArray, FieldSeq, 452);
            if ((temp == null) || (temp == string.Empty))
                agents.hazard_insurance.Contact.Fax = "";
            else
                agents.hazard_insurance.Contact.Fax = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12385);
            if ((temp == null) || (temp == string.Empty))
                agents.hazard_insurance.Contact.CellPhone = "";
            else
                agents.hazard_insurance.Contact.CellPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12386);
            if ((temp == null) || (temp == string.Empty))
                agents.hazard_insurance.Contact.Email = "";
            else
                agents.hazard_insurance.Contact.Email = temp.Trim();
        }

        void AddInvestor(ref Record.LoanAgents agents, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (agents == null)
                agents = new Record.LoanAgents();

            string temp = "";
            temp = PNT.getPointField(FieldArray, FieldSeq, 7340);
            if ((temp == null) || (temp.Trim() == string.Empty))
                return;
            if (agents.investor == null)
                agents.investor = new Record.Agent();
            if (agents.investor.Contact == null)
                agents.investor.Contact = new Record.Contacts();

            ParseName(temp, ref agents.investor.Contact.FirstName, ref agents.investor.Contact.LastName, ref agents.investor.Contact.MiddleName);
            temp = PNT.getPointField(FieldArray, FieldSeq, 7341);
            if ((temp == null) || (temp == string.Empty))
                agents.investor.Company = "";
            else
                agents.investor.Company = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 7342);
            if ((temp == null) || (temp == string.Empty))
                agents.investor.Contact.BusinessPhone = "";
            else
                agents.investor.Contact.BusinessPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 7343);
            if ((temp == null) || (temp == string.Empty))
                agents.investor.Contact.MailingAddr = "";
            else
                agents.investor.Contact.MailingAddr = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 7344);
            if ((temp == null) || (temp == string.Empty))
                agents.investor.Contact.MailingCity = "";
            else
                agents.investor.Contact.MailingCity = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 7345);
            if ((temp == null) || (temp == string.Empty))
                agents.investor.Contact.MailingState = "";
            else
                agents.investor.Contact.MailingState = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 7346);
            if ((temp == null) || (temp == string.Empty))
                agents.investor.Contact.MailingZip = "";
            else
                agents.investor.Contact.MailingZip = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 7348);
            if ((temp == null) || (temp == string.Empty))
                agents.investor.Contact.Fax = "";
            else
                agents.investor.Contact.Fax = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12363);
            if ((temp == null) || (temp == string.Empty))
                agents.investor.Contact.CellPhone = "";
            else
                agents.investor.Contact.CellPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12364);
            if ((temp == null) || (temp == string.Empty))
                agents.investor.Contact.Email = "";
            else
                agents.investor.Contact.Email = temp.Trim();
        }

        void AddLender(ref Record.LoanAgents agents, List<string> FieldArray, ArrayList FieldSeq)
        {
            string err = "";
            string Name = "";
            string FirstName = "";
            string LastName = "";
            string MiddleName = "";

            if (agents == null)
                agents = new Record.LoanAgents();

            Name = PNT.getPointField(FieldArray, FieldSeq, 6000);

            if (Name == null)
            {
                Name = "";
            }
            else
            {
                Name = Name.Trim();
                ParseName(Name, ref FirstName, ref LastName, ref MiddleName);

                if (FirstName == String.Empty)
                {
                    FirstName = "";
                }

                if (LastName == String.Empty)
                {
                    LastName = "";
                }
            }

            if (agents.lender_contact1 == null)
                agents.lender_contact1 = new Record.Agent();

            Record.Agent rec = agents.lender_contact1;
            // company
            Name = PNT.getPointField(FieldArray, FieldSeq, 6001);
            if ((Name == null) || (Name == String.Empty))
                rec.Company = "";
            else
                rec.Company = Name.Trim();

            if (rec.Contact == null)
                rec.Contact = new Record.Contacts();
            rec.Contact.FirstName = FirstName;               //   1
            rec.Contact.MiddleName = MiddleName;        //      2 
            rec.Contact.LastName = LastName;               //   3

            rec.Contact.CellPhone = PNT.getPointField(FieldArray, FieldSeq, 12357);                 //   9 
            rec.Contact.BusinessPhone = PNT.getPointField(FieldArray, FieldSeq, 6002);        //   10
            rec.Contact.Fax = PNT.getPointField(FieldArray, FieldSeq, 6005);                            //    11
            rec.Contact.Email = PNT.getPointField(FieldArray, FieldSeq, 12358);                         //    12

            rec.Contact.MailingAddr = PNT.getPointField(FieldArray, FieldSeq, 6003);
            string city = "", state = "", zip = "";
            Name = PNT.getPointField(FieldArray, FieldSeq, 6004);
            if ((Name == null) || (Name == String.Empty))
                return;
            getCityStateZip(Name, ref city, ref state, ref zip);
            rec.Contact.MailingCity = city;
            rec.Contact.MailingState = state;
            rec.Contact.MailingZip = zip;
        }

        void AddListingAgent(ref Record.LoanAgents agents, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (agents == null)
                agents = new Record.LoanAgents();

            string temp = "";
            temp = PNT.getPointField(FieldArray, FieldSeq, 6130);
            if ((temp == null) || (temp.Trim() == string.Empty))
                return;
            if (agents.listing_agent == null)
                agents.listing_agent = new Record.Agent();
            if (agents.listing_agent.Contact == null)
                agents.listing_agent.Contact = new Record.Contacts();

            ParseName(temp, ref agents.listing_agent.Contact.FirstName, ref agents.listing_agent.Contact.LastName, ref agents.listing_agent.Contact.MiddleName);
            temp = PNT.getPointField(FieldArray, FieldSeq, 6131);
            if ((temp == null) || (temp == string.Empty))
                agents.listing_agent.Company = "";
            else
                agents.listing_agent.Company = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6132);
            if ((temp == null) || (temp == string.Empty))
                agents.listing_agent.Contact.BusinessPhone = "";
            else
                agents.listing_agent.Contact.BusinessPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6133);
            if ((temp == null) || (temp == string.Empty))
                agents.listing_agent.Contact.MailingAddr = "";
            else
                agents.listing_agent.Contact.MailingAddr = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6134);
            if ((temp == null) || (temp == string.Empty))
            {
                agents.listing_agent.Contact.MailingCity = "";
                agents.listing_agent.Contact.MailingState = "";
                agents.listing_agent.Contact.MailingZip = "";
            }
            else
                getCityStateZip(temp, ref agents.listing_agent.Contact.MailingCity, ref agents.listing_agent.Contact.MailingState, ref agents.listing_agent.Contact.MailingZip);

            temp = PNT.getPointField(FieldArray, FieldSeq, 6135);
            if ((temp == null) || (temp == string.Empty))
                agents.listing_agent.Contact.Fax = "";
            else
                agents.listing_agent.Contact.Fax = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12377);
            if ((temp == null) || (temp == string.Empty))
                agents.listing_agent.Contact.CellPhone = "";
            else
                agents.listing_agent.Contact.CellPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12378);
            if ((temp == null) || (temp == string.Empty))
                agents.listing_agent.Contact.Email = "";
            else
                agents.listing_agent.Contact.Email = temp.Trim();
        }

        void AddMortgageInsurance(ref Record.LoanAgents agents, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (agents == null)
                agents = new Record.LoanAgents();

            string temp = "";
            temp = PNT.getPointField(FieldArray, FieldSeq, 460);
            if ((temp == null) || (temp.Trim() == string.Empty))
                return;
            if (agents.mortgage_insurance == null)
                agents.mortgage_insurance = new Record.Agent();
            if (agents.mortgage_insurance.Contact == null)
                agents.mortgage_insurance.Contact = new Record.Contacts();

            ParseName(temp, ref agents.mortgage_insurance.Contact.FirstName, ref agents.mortgage_insurance.Contact.LastName, ref agents.mortgage_insurance.Contact.MiddleName);
            temp = PNT.getPointField(FieldArray, FieldSeq, 463);
            if ((temp == null) || (temp == string.Empty))
                agents.mortgage_insurance.Company = "";
            else
                agents.mortgage_insurance.Company = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 461);
            if ((temp == null) || (temp == string.Empty))
                agents.mortgage_insurance.Contact.BusinessPhone = "";
            else
                agents.mortgage_insurance.Contact.BusinessPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 464);
            if ((temp == null) || (temp == string.Empty))
                agents.mortgage_insurance.Contact.MailingAddr = "";
            else
                agents.mortgage_insurance.Contact.MailingAddr = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 465);
            if ((temp == null) || (temp == string.Empty))
            {
                agents.mortgage_insurance.Contact.MailingCity = "";
                agents.mortgage_insurance.Contact.MailingState = "";
                agents.mortgage_insurance.Contact.MailingZip = "";
            }
            else
                getCityStateZip(temp, ref agents.mortgage_insurance.Contact.MailingCity, ref agents.mortgage_insurance.Contact.MailingState, ref agents.mortgage_insurance.Contact.MailingZip);

            temp = PNT.getPointField(FieldArray, FieldSeq, 462);
            if ((temp == null) || (temp == string.Empty))
                agents.mortgage_insurance.Contact.Fax = "";
            else
                agents.mortgage_insurance.Contact.Fax = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12387);
            if ((temp == null) || (temp == string.Empty))
                agents.mortgage_insurance.Contact.CellPhone = "";
            else
                agents.mortgage_insurance.Contact.CellPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12388);
            if ((temp == null) || (temp == string.Empty))
                agents.mortgage_insurance.Contact.Email = "";
            else
                agents.mortgage_insurance.Contact.Email = temp.Trim();
        }

        void AddSellingAgent(ref Record.LoanAgents agents, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (agents == null)
                agents = new Record.LoanAgents();

            string temp = "";
            temp = PNT.getPointField(FieldArray, FieldSeq, 6140);
            if ((temp == null) || (temp.Trim() == string.Empty))
                return;
            if (agents.selling_agent == null)
                agents.selling_agent = new Record.Agent();
            if (agents.selling_agent.Contact == null)
                agents.selling_agent.Contact = new Record.Contacts();

            ParseName(temp, ref agents.selling_agent.Contact.FirstName, ref agents.selling_agent.Contact.LastName, ref agents.selling_agent.Contact.MiddleName);
            temp = PNT.getPointField(FieldArray, FieldSeq, 6141);
            if ((temp == null) || (temp == string.Empty))
                agents.selling_agent.Company = "";
            else
                agents.selling_agent.Company = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6142);
            if ((temp == null) || (temp == string.Empty))
                agents.selling_agent.Contact.BusinessPhone = "";
            else
                agents.selling_agent.Contact.BusinessPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6143);
            if ((temp == null) || (temp == string.Empty))
                agents.selling_agent.Contact.MailingAddr = "";
            else
                agents.selling_agent.Contact.MailingAddr = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6144);
            if ((temp == null) || (temp == string.Empty))
            {
                agents.selling_agent.Contact.MailingCity = "";
                agents.selling_agent.Contact.MailingState = "";
                agents.selling_agent.Contact.MailingZip = "";
            }
            else
                getCityStateZip(temp, ref agents.selling_agent.Contact.MailingCity, ref agents.selling_agent.Contact.MailingState, ref agents.selling_agent.Contact.MailingZip);

            temp = PNT.getPointField(FieldArray, FieldSeq, 6145);
            if ((temp == null) || (temp == string.Empty))
                agents.selling_agent.Contact.Fax = "";
            else
                agents.selling_agent.Contact.Fax = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12379);
            if ((temp == null) || (temp == string.Empty))
                agents.selling_agent.Contact.CellPhone = "";
            else
                agents.selling_agent.Contact.CellPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12380);
            if ((temp == null) || (temp == string.Empty))
                agents.selling_agent.Contact.Email = "";
            else
                agents.selling_agent.Contact.Email = temp.Trim();
        }

        void AddTitle(ref Record.LoanAgents agents, List<string> FieldArray, ArrayList FieldSeq)
        {
            if (agents == null)
                agents = new Record.LoanAgents();

            string temp = "";
            temp = PNT.getPointField(FieldArray, FieldSeq, 6120);
            if ((temp == null) || (temp.Trim() == string.Empty))
                return;
            if (agents.title_insurance == null)
                agents.title_insurance = new Record.Agent();
            if (agents.title_insurance.Contact == null)
                agents.title_insurance.Contact = new Record.Contacts();

            ParseName(temp, ref agents.title_insurance.Contact.FirstName, ref agents.title_insurance.Contact.LastName, ref agents.title_insurance.Contact.MiddleName);
            temp = PNT.getPointField(FieldArray, FieldSeq, 6121);
            if ((temp == null) || (temp == string.Empty))
                agents.title_insurance.Company = "";
            else
                agents.title_insurance.Company = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6122);
            if ((temp == null) || (temp == string.Empty))
                agents.title_insurance.Contact.BusinessPhone = "";
            else
                agents.title_insurance.Contact.BusinessPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6123);
            if ((temp == null) || (temp == string.Empty))
                agents.title_insurance.Contact.MailingAddr = "";
            else
                agents.title_insurance.Contact.MailingAddr = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 6124);
            if ((temp == null) || (temp == string.Empty))
            {
                agents.title_insurance.Contact.MailingCity = "";
                agents.title_insurance.Contact.MailingState = "";
                agents.title_insurance.Contact.MailingZip = "";
            }
            else
                getCityStateZip(temp, ref agents.title_insurance.Contact.MailingCity, ref agents.title_insurance.Contact.MailingState, ref agents.title_insurance.Contact.MailingZip);

            temp = PNT.getPointField(FieldArray, FieldSeq, 6125);
            if ((temp == null) || (temp == string.Empty))
                agents.title_insurance.Contact.Fax = "";
            else
                agents.title_insurance.Contact.Fax = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12371);
            if ((temp == null) || (temp == string.Empty))
                agents.title_insurance.Contact.CellPhone = "";
            else
                agents.title_insurance.Contact.CellPhone = temp.Trim();

            temp = PNT.getPointField(FieldArray, FieldSeq, 12372);
            if ((temp == null) || (temp == string.Empty))
                agents.title_insurance.Contact.Email = "";
            else
                agents.title_insurance.Contact.Email = temp.Trim();
        }

        #endregion
        #region Loan Team (Loan Officer, Processor, Underwriter, etc.)
        void AddLoanTeam(ref Record.LoanTeam team, List<string> FieldArray, ArrayList FieldSeq)
        {
            team.lo = AddTeamMember(FieldArray, FieldSeq, 19);
            team.processor = AddTeamMember(FieldArray, FieldSeq, 18);
            team.underwriter = AddTeamMember(FieldArray, FieldSeq, 942);
            team.docPrep = AddTeamMember(FieldArray, FieldSeq, 11550);
            team.closer = AddTeamMember(FieldArray, FieldSeq, 11556);
            team.shipper = AddTeamMember(FieldArray, FieldSeq, 11562);
        }

        string AddTeamMember(List<string> FieldArray, ArrayList FieldSeq, short fieldId)
        {
            string outMember = string.Empty;
            string Name = string.Empty;
            string FirstName = string.Empty;
            string LastName = string.Empty;
            string MiddleName = string.Empty;

            Name = PNT.getPointField(FieldArray, FieldSeq, fieldId);
            if (string.IsNullOrEmpty(Name))
                return outMember;
            ParseName(Name, ref FirstName, ref LastName, ref MiddleName);
            MiddleName = MiddleName.Trim();
            FirstName = FirstName.Trim();
            LastName = LastName.Trim();
            if (string.IsNullOrEmpty(MiddleName))
                outMember = FirstName + " " + LastName;
            else
                outMember = FirstName + " " + MiddleName + " " + LastName;
            return outMember;
        }

        void Process_LoanTeam(int fileId, int branchId, Record.LoanTeam team, bool activeLoan, ref string err)
        {
            err = "";
            string errMsg = "";
            string teamMember = string.Empty;
            if (string.IsNullOrEmpty(team.lo))
                errMsg = @"Missing Loan Rep.\r\n";
            else
            {
                teamMember = da.GetLoanTeamMemberByRoleName(fileId, UserRoles.UserRole_LoanOfficer, ref err);
                if (string.IsNullOrEmpty(teamMember) || teamMember.Trim().ToUpper() != team.lo.Trim().ToUpper())
                {
                    if (da.Save_Team(fileId, branchId, UserRoles.UserRole_LoanOfficer, team.lo, activeLoan, ref err) == false)
                        errMsg += err + ".\r\n";
                }
            }
            if ((team.processor == null) || (team.processor == String.Empty))
                errMsg += @"Missing Processor.\r\n";
            else
            {
                teamMember = da.GetLoanTeamMemberByRoleName(fileId, UserRoles.UserRole_Processor, ref err);
                if (string.IsNullOrEmpty(teamMember) || teamMember.Trim().ToUpper() != team.processor.Trim().ToUpper())
                {
                    if (da.Save_Team(fileId, branchId, UserRoles.UserRole_Processor, team.processor, false, ref err) == false)
                    {
                        errMsg += err + ".\r\n";
                    }
                }
            }
            if ((team.underwriter != null) && (team.underwriter != String.Empty))
            {
                teamMember = da.GetLoanTeamMemberByRoleName(fileId, UserRoles.UserRole_Underwriter, ref err);
                if (string.IsNullOrEmpty(teamMember) || teamMember.Trim().ToUpper() != team.underwriter.Trim().ToUpper())
                {
                    if (da.Save_Team(fileId, branchId, UserRoles.UserRole_Underwriter, team.underwriter, false, ref err) == false)
                        errMsg += err + ".\r\n";
                }
            }

            if ((team.docPrep != null) && (team.docPrep != String.Empty))
            {
                teamMember = da.GetLoanTeamMemberByRoleName(fileId, UserRoles.UserRole_DocPrep, ref err);
                if (string.IsNullOrEmpty(teamMember) || teamMember.Trim().ToUpper() != team.docPrep.Trim().ToUpper())
                {
                    if (da.Save_Team(fileId, branchId, UserRoles.UserRole_DocPrep, team.docPrep, false, ref err) == false)
                        errMsg += err + ".\r\n";
                }
            }

            if ((team.closer != null) && (team.closer != String.Empty))
            {
                teamMember = da.GetLoanTeamMemberByRoleName(fileId, UserRoles.UserRole_Closer, ref err);
                if (string.IsNullOrEmpty(teamMember) || teamMember.Trim().ToUpper() != team.closer.Trim().ToUpper())
                {
                    if (da.Save_Team(fileId, branchId, UserRoles.UserRole_Closer, team.closer, false, ref err) == false)
                        errMsg += err + ".\r\n";
                }
            }

            if ((team.shipper != null) && (team.shipper != String.Empty))
            {
                teamMember = da.GetLoanTeamMemberByRoleName(fileId, UserRoles.UserRole_Shipper, ref err);
                if (string.IsNullOrEmpty(teamMember) || teamMember.Trim().ToUpper() != team.shipper.Trim().ToUpper())
                {
                    if (da.Save_Team(fileId, branchId, UserRoles.UserRole_Shipper, team.shipper, false, ref err) == false)
                        errMsg += err + ".\r\n";
                }
            }
        }
        #endregion
        #region Conditions, Basic Docs
        Record.Docs GetDocFields(List<string> FieldArray, ArrayList FieldSeq, int DocNameField, int OrderedField, int ReceivedField, int DueField)
        {
            Record.Docs doc = null;
            string temp = "";
            string docName = string.Empty;

            if (DocNameField > 0)
            {
                temp = PNT.getPointField(FieldArray, FieldSeq, (short)DocNameField);
                if (string.IsNullOrEmpty(temp))
                    return doc;
                docName = temp.Trim();
            }
            temp = PNT.getPointField(FieldArray, FieldSeq, (short)OrderedField);
            DateTime tempDate = DateTime.MinValue;
            if (DocNameField <= 0 &&
                (string.IsNullOrEmpty(temp) || !DateTime.TryParse(temp, out tempDate) || tempDate == DateTime.MinValue))
                return doc;

            doc = new Record.Docs();
            doc.DocName = docName;
            doc.Ordered = tempDate;
            doc.OrderedBy = string.Empty;
            doc.Status = "Ordered";

            tempDate = DateTime.MinValue;
            temp = PNT.getPointField(FieldArray, FieldSeq, (short)ReceivedField);
            if (!string.IsNullOrEmpty(temp) && DateTime.TryParse(temp, out tempDate) && tempDate != DateTime.MinValue)
            {
                doc.Received = tempDate;
                doc.ReceivedBy = string.Empty;
                doc.Status = "Received";
            }
            tempDate = DateTime.MinValue;
            temp = PNT.getPointField(FieldArray, FieldSeq, (short)DueField);
            if (!string.IsNullOrEmpty(temp) && DateTime.TryParse(temp, out tempDate) && tempDate != DateTime.MinValue)
            {
                doc.Due = tempDate;
            }
            return doc;
        }
        void AddBasicDocs(List<Record.Docs> list, List<string> FieldArray, ArrayList FieldSeq)
        {
            string err = "";
            try
            {
                if (list == null)
                    list = new List<Record.Docs>();

                // Credit Report
                Record.Docs creditReport = GetDocFields(FieldArray, FieldSeq, 0, 6305, 6306, 13107);
                if (creditReport != null)
                {
                    creditReport.DocName = "Credit Report";
                    list.Add(creditReport);
                }
                // Business Credit Report
                Record.Docs bizCreditReport = GetDocFields(FieldArray, FieldSeq, 0, 6310, 6311, 13113);
                if (bizCreditReport != null)
                {
                    bizCreditReport.DocName = "Business Credit Report";
                    list.Add(bizCreditReport);
                }

                // Preliminary Title
                Record.Docs titleReport = GetDocFields(FieldArray, FieldSeq, 0, 6300, 6301, 13101);
                if (titleReport != null)
                {
                    titleReport.DocName = "Preliminary Title";
                    list.Add(titleReport);
                }

                // Initial Disclosures
                Record.Docs InitialDisclosures = GetDocFields(FieldArray, FieldSeq, 0, 6320, 6321, 13131);
                if (InitialDisclosures != null)
                {
                    InitialDisclosures.DocName = "Initial Disclosures";
                    list.Add(InitialDisclosures);
                }

                // Appraisal Report
                Record.Docs appraisalReport = GetDocFields(FieldArray, FieldSeq, 0, 6315, 6316, 13119);
                if (appraisalReport != null)
                {
                    appraisalReport.DocName = "Appraisal Report";
                    list.Add(appraisalReport);
                }

                // AVM Report
                Record.Docs AVMReport = GetDocFields(FieldArray, FieldSeq, 0, 6780, 6781, 13125);
                if (AVMReport != null)
                {
                    AVMReport.DocName = "AVM Report";
                    list.Add(AVMReport);
                }

                // Flood Certification
                Record.Docs floodCert = GetDocFields(FieldArray, FieldSeq, 0, 6369, 6378, 13137);
                if (floodCert != null)
                {
                    floodCert.DocName = "Flood Certification";
                    list.Add(floodCert);
                }

                // Closing Documents
                Record.Docs closingDocs = GetDocFields(FieldArray, FieldSeq, 0, 11463, 11464, 13173);
                if (closingDocs != null)
                {
                    closingDocs.DocName = "Closing Documents";
                    list.Add(closingDocs);
                }
                int docName = 6329;
                int ordered = 6325;
                int received = 6326;
                int due = 13143;
                while (docName <= 6349)
                {
                    Record.Docs doc = GetDocFields(FieldArray, FieldSeq, docName, ordered, received, due);
                    if (doc != null)
                        list.Add(doc);
                    docName += 5;
                    ordered += 5;
                    received += 5;
                    due += 6;
                }
                docName = 13899;
                ordered = 13897;
                received = 13898;
                due = 13896;
                while (docName <= 13919)
                {
                    Record.Docs doc = GetDocFields(FieldArray, FieldSeq, docName, ordered, received, due);
                    if (doc != null)
                        list.Add(doc);
                    docName += 5;
                    ordered += 5;
                    received += 5;
                    due += 5;
                }
            }
            catch (Exception ex)
            {
                err = "AddBasicDocs, Exception: " + ex.Message;
                Trace.TraceError(err);
                int Event_id = 9314;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }
        }
        void AddBankerConditions(List<Record.Conditions> list, List<string> FieldArray, ArrayList FieldSeq)
        {
            string err = "";
            try
            {
                string temp = "";
                Int16 i16;
                temp = PNT.getPointField(FieldArray, FieldSeq, 15251);
                int total = 0;
                if (Int16.TryParse(temp, out i16))
                {
                    total = i16;
                }
                else
                {
                    total = 0;
                }

                if (total <= 0)
                    return;

                if (list == null)
                    list = new List<Record.Conditions>();

                // condition name
                temp = PNT.getPointField(FieldArray, FieldSeq, 15252);
                if ((temp == null) || (temp == string.Empty))
                    return;
                string[] desc = temp.Split('|');
                if (desc.Length < total)
                    return;
                // condition type
                temp = PNT.getPointField(FieldArray, FieldSeq, 15253);
                if ((temp == null) || (temp == string.Empty))
                    temp = "";
                string[] type = temp.Split('|');
                if (type.Length < total)
                    return;
                // condition received date
                temp = PNT.getPointField(FieldArray, FieldSeq, 15254);
                if ((temp == null) || (temp == string.Empty))
                    temp = "";
                string[] rcvDate = temp.Split('|');
                if (rcvDate.Length < total)
                    return;
                // condition cleared date
                temp = PNT.getPointField(FieldArray, FieldSeq, 15255);
                if ((temp == null) || (temp == string.Empty))
                    temp = "";
                string[] clearDate = temp.Split('|');
                if (clearDate.Length < total)
                    return;
                // condition cleared by
                temp = PNT.getPointField(FieldArray, FieldSeq, 15256);
                if ((temp == null) || (temp == string.Empty))
                    temp = "";
                string[] clearBy = temp.Split('|');
                if (clearBy.Length < total)
                    return;
                // condition created by
                temp = PNT.getPointField(FieldArray, FieldSeq, 15257);
                if ((temp == null) || (temp == string.Empty))
                    temp = "";
                string[] createBy = temp.Split('|');
                if (createBy.Length < total)
                    return;
                //condition created date
                temp = PNT.getPointField(FieldArray, FieldSeq, 15258);
                if ((temp == null) || (temp == string.Empty))
                    temp = "";
                string[] createDate = temp.Split('|');
                if (createDate.Length < total)
                    return;
                // condition received by
                temp = PNT.getPointField(FieldArray, FieldSeq, 15259);
                if ((temp == null) || (temp == string.Empty))
                    temp = "";
                string[] receivedBy = temp.Split('|');
                if (receivedBy.Length < total)
                    return;
                // condition collected date
                temp = PNT.getPointField(FieldArray, FieldSeq, 15260);
                if ((temp == null) || (temp == string.Empty))
                    temp = "";
                string[] collectedDate = temp.Split('|');
                if (collectedDate.Length < total)
                    return;
                // condition collected by
                temp = PNT.getPointField(FieldArray, FieldSeq, 15261);
                if ((temp == null) || (temp == string.Empty))
                    temp = "";
                string[] collectedBy = temp.Split('|');
                if (collectedBy.Length < total)
                    return;
                // condition submitted date
                temp = PNT.getPointField(FieldArray, FieldSeq, 15262);
                if ((temp == null) || (temp == string.Empty))
                    temp = "";
                string[] submittedDate = temp.Split('|');
                if (submittedDate.Length < total)
                    return;
                // condition submitted by
                temp = PNT.getPointField(FieldArray, FieldSeq, 15263);
                if ((temp == null) || (temp == string.Empty))
                    temp = "";
                string[] submittedBy = temp.Split('|');
                if (submittedBy.Length < total)
                    return;
                // condition externalViewing by
                //temp = PNT.getPointField(FieldArray, FieldSeq, 15264);
                //if ((temp == null) || (temp == string.Empty))
                //    temp = "";
                //string[] externalViewing = temp.Split('|');
                //if (externalViewing.Length < total)
                //    return;

                int i;
                for (i = 0; i < total; i++)
                {
                    if (string.IsNullOrEmpty(desc[i]))
                        continue;
                    Record.Conditions cond = new Record.Conditions
                    {
                        CondName = string.IsNullOrEmpty(desc[i]) ? string.Empty : desc[i].Trim(),
                        CondType = string.IsNullOrEmpty(type[i]) ? string.Empty : type[i].Trim(),
                        Created = string.IsNullOrEmpty(createDate[i]) ? DateTime.Now : DateTime.Parse(createDate[i]),
                        CreatedBy = string.IsNullOrEmpty(createBy[i]) ? string.Empty : createBy[i].Trim(),
                        Cleared = string.IsNullOrEmpty(clearDate[i]) ? DateTime.MinValue : DateTime.Parse(clearDate[i]),
                        ClearedBy = string.IsNullOrEmpty(clearBy[i]) ? string.Empty : clearBy[i].Trim(),
                        Collected = string.IsNullOrEmpty(collectedDate[i]) ? DateTime.MinValue : DateTime.Parse(collectedDate[i]),
                        CollectedBy = string.IsNullOrEmpty(collectedBy[i]) ? string.Empty : collectedBy[i].Trim(),
                        Received = string.IsNullOrEmpty(rcvDate[i]) ? DateTime.MinValue : DateTime.Parse(rcvDate[i]),
                        ReceivedBy = string.IsNullOrEmpty(receivedBy[i]) ? string.Empty : receivedBy[i].Trim(),
                        Submitted = string.IsNullOrEmpty(submittedDate[i]) ? DateTime.MinValue : DateTime.Parse(submittedDate[i]),
                        SubmittedBy = string.IsNullOrEmpty(submittedBy[i]) ? string.Empty : submittedBy[i].Trim(),
                        //ExternalViewing = string.IsNullOrEmpty(externalViewing[i]) ? false : true
                    };
                    if (string.IsNullOrEmpty(cond.CondName))
                        continue;
                    cond.Status = string.Empty;
                    if (cond.Cleared != DateTime.MinValue)
                        cond.Status = "Cleared";
                    else
                        if (cond.Submitted != DateTime.MinValue)
                            cond.Status = "Submitted";
                        else
                            if (cond.Received != DateTime.MinValue)
                                cond.Status = "Received";
                    list.Add(cond);
                }
            }
            catch (Exception ex)
            {
                err = "AddBankerConditions, Exception: " + ex.Message;
                Trace.TraceError(err);
                int Event_id = 9315;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }
        }

        void AddDocsConditions(ref Record.LoanRecord rec, List<string> FieldArray, ArrayList FieldSeq)
        {
            AddBasicDocs(rec.docs, FieldArray, FieldSeq);
            AddBankerConditions(rec.conditions, FieldArray, FieldSeq);
        }
        #endregion
        #region Loan Notes
        public void AddNotes(ref List<Table.LoanNotes> notes, List<string> FieldArray, ArrayList FieldSeq, string filepath)
        {
            if (notes == null)
                notes = new List<Table.LoanNotes>();
            notes.Clear();
            string temp = PNT.getPointField(FieldArray, FieldSeq, 15);
            if ((temp == null) || (temp == string.Empty))
                return;

            string[] tNotes = temp.Split('^');
            if (tNotes.Length <= 0)
                return;

            string noteStr = "";

            int i;
            bool duplicate = false;
            bool duplicate_once = false;
            Table.LoanNotes note = null;
            DateTime dt = DateTime.MinValue;

            List<string> clean_notes = new List<string>();
            clean_notes.Clear();

            for (i = 0; i < tNotes.Length; i++)
            {
                duplicate = false;
                string clean_note = tNotes[i];
                if (clean_notes.Count > 0)
                {
                    foreach (string exist_clean_note in clean_notes)
                    {
                        string[] clean_note_fields = clean_note.Split('|');
                        if (clean_note_fields.Length < 3)
                            continue;

                        string[] exist_clean_note_fields = exist_clean_note.Split('|');
                        if (exist_clean_note_fields.Length < 3)
                            continue;

                        if ((clean_note_fields[0].Trim() == exist_clean_note_fields[0].Trim()) &&
                            (clean_note_fields[1].Trim() == exist_clean_note_fields[1].Trim()) &&
                            (clean_note_fields[2].Trim() == exist_clean_note_fields[2].Trim()))
                        {
                            duplicate = true;
                            duplicate_once = true;
                            continue;
                        }
                    }
                }
                if (duplicate == false)
                {
                    clean_notes.Add(clean_note);
                }
            }

            if (clean_notes.Count > 0)
            {
                int counter = 0;
                foreach (string cn in clean_notes)
                {
                    if (counter == 0)
                    {
                        noteStr = noteStr + cn;
                    }
                    else
                    {
                        noteStr = noteStr + "^" + cn;
                    }
                    counter = counter + 1;
                }
            }
            else
            {
                return;
            }

            if (duplicate_once == true)
            {
                //write back to point
                string err = "";

                bool logErr = false;
                List<Framework.FieldMap> UpdatedFieldds = new List<Framework.FieldMap>();
                UpdatedFieldds.Clear();
                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                pm.AddUpdatedFields(ref UpdatedFieldds, 15, noteStr);
                if (PNT.WritePointData(UpdatedFieldds, filepath, ref  err) == false)
                {
                    int Event_id = 9317;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }

            temp = PNT.getPointField(FieldArray, FieldSeq, 15);
            if ((temp == null) || (temp == string.Empty))
                return;

            tNotes = noteStr.Split('^');
            if (tNotes.Length <= 0)
                return;

            note = null;
            dt = DateTime.MinValue;

            for (i = 0; i < tNotes.Length; i++)
            {
                string[] fields = tNotes[i].Split('|');
                if (fields.Length < 3)
                    continue;
                note = new Table.LoanNotes();
                if ((fields[0] == null) || (fields[0] == String.Empty))  // Note Datetime
                    continue;

                note.Created = dt;
                if (DateTime.TryParse(fields[0], out dt))
                    note.Created = dt;

                if ((fields[1] == null) || (fields[1] == String.Empty))  // Note content
                    continue;
                note.Note = fields[1].Trim();
                if ((fields[2] == null) || (fields[2] == String.Empty))  // Note creator
                    continue;
                note.Sender = fields[2].Trim();
                note.Exported = true;
                notes.Add(note);
            }

        }
        #endregion
        #region Check Required Fields
        bool CheckLoanFields(ref Record.LoanRecord rec, string filename, ProspectFlagEnum prospectFlag)
        {
            string err = "";
            string errType = "Warning";
            string errMsg = "";
            bool logErr = false;
            bool pointErr = false;
            int fileId = 0;
            try
            {
                if ((rec == null) || (rec.loans == null) || (rec.borrower_contacts == null) || (rec.team == null))
                {
                    err = MethodBase.GetCurrentMethod() + ", missing loan record within Point File " + filename;
                    int Event_id = 9318;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return pointErr;
                }
                fileId = rec.loans.FileId;

                if (CheckContactRecord(rec.borrower_contacts, ContactRoles.ContactRole_Borrower, true, ref err) == false)
                {
                    pointErr = true;
                    errType = "Error";
                    errMsg += err;
                }
                if (prospectFlag == ProspectFlagEnum.RealtimePointFlag || prospectFlag == ProspectFlagEnum.ScheduledPointFlag)
                {
                    //if ((rec.loans.EstCloseDate == null) || (rec.loans.EstCloseDate == string.Empty))
                    //{
                    //    pointErr = true;
                    //    errMsg += " Missing Estimated Close Date. <br/>";
                    //    errType = "Error";
                    //}

                    if (!string.IsNullOrEmpty(rec.loans.PurchasedDate))
                    {
                        DateTime dt;
                        if (!DateTime.TryParse(rec.loans.PurchasedDate, out dt))
                        {
                            pointErr = true;
                            errMsg += string.Format(" PurchasedDate:{0} is not a vialid date format. <br/>", rec.loans.PurchasedDate);
                            errType = "Error";
                        }
                    }

                    if ((rec.loans.PropertyAddr == null) || (rec.loans.PropertyAddr == string.Empty))
                    {
                        pointErr = true;
                        errMsg += " Missing Property Address. <br/>";
                        errType = "Error";
                    }

                    if ((rec.loans.Rate == null) || (rec.loans.Rate == string.Empty))
                    {
                        pointErr = true;
                        errMsg += " Missing Rate. <br/>";
                    }

                    if ((rec.team.processor == null) || (rec.team.processor == string.Empty))
                    {
                        pointErr = true;
                        errMsg += " Missing Processor. <br/>";
                    }
                    else
                    {
                        string ProcInfo = string.Empty;
                        ProcInfo = da.GetLoanTeamMemberByRoleName(rec.loans.FileId, UserRoles.UserRole_Processor, ref err);
                        if (!string.IsNullOrEmpty(err))
                        { Trace.TraceError(err); }
                        if (string.IsNullOrEmpty(ProcInfo) && da.Find_User(rec.loans.BranchId, rec.team.processor, ref err) <= 0)
                        {
                            pointErr = true;
                            errMsg += " Processor " + rec.team.processor + " does not have a user account in the system.<br/>";
                        }
                    }

                    if (!string.IsNullOrEmpty(rec.team.underwriter) &&
                        string.IsNullOrEmpty(da.GetLoanTeamMemberByRoleName(rec.loans.FileId, UserRoles.UserRole_Underwriter, ref err)))
                    {
                        if (da.Find_User(rec.loans.BranchId, rec.team.underwriter, ref err) <= 0)
                        {
                            pointErr = true;
                            errMsg += "Underwriter " + rec.team.underwriter + " does not have a user account in the system.<br/>";
                        }
                    }

                    if (!string.IsNullOrEmpty(rec.team.docPrep) &&
                        string.IsNullOrEmpty(da.GetLoanTeamMemberByRoleName(rec.loans.FileId, UserRoles.UserRole_DocPrep, ref err)))
                    {
                        if (da.Find_User(rec.loans.BranchId, rec.team.docPrep, ref err) <= 0)
                        {
                            pointErr = true;
                            errMsg += " Doc Prep " + rec.team.docPrep + " does not have a user account in the system.<br/>";
                        }
                    }

                    if (!string.IsNullOrEmpty(rec.team.closer) &&
                        string.IsNullOrEmpty(da.GetLoanTeamMemberByRoleName(rec.loans.FileId, UserRoles.UserRole_Closer, ref err)))
                    {
                        if (da.Find_User(rec.loans.BranchId, rec.team.closer, ref err) <= 0)
                        {
                            pointErr = true;
                            errMsg += " Closer/Funder " + rec.team.closer + " does not have a user account in the system.<br/>";
                        }
                    }

                    if (!string.IsNullOrEmpty(rec.team.shipper) &&
                        string.IsNullOrEmpty(da.GetLoanTeamMemberByRoleName(rec.loans.FileId, UserRoles.UserRole_Shipper, ref err)))
                    {
                        if (da.Find_User(rec.loans.BranchId, rec.team.shipper, ref err) <= 0)
                        {
                            pointErr = true;
                            errMsg += " Shipper " + rec.team.shipper + " does not have a user account in the system.<br/>";
                        }
                    }
                }

                if ((rec.team.lo == null) || (rec.team.lo == string.Empty))
                {
                    pointErr = true;
                    errMsg += " Missing Loan Rep. <br/>";
                }
                else
                {
                    string LO_Info = da.GetLoanTeamMemberByRoleName(rec.loans.FileId, UserRoles.UserRole_LoanOfficer, ref err);
                    if (!string.IsNullOrEmpty(err))
                    { Trace.TraceError(err); }
                    if (string.IsNullOrEmpty(LO_Info))
                    {
                        rec.loans.UserID = da.Find_LO(rec.loans.BranchId, rec.team.lo, ref err);
                        if (rec.loans.UserID <= 0)
                        {
                            rec.loans.UserID = da.Find_LOName(rec.loans.BranchId, rec.team.lo, ref err);
                            if (rec.loans.UserID <= 0)
                            {
                                if (da.Find_User(rec.loans.BranchId, rec.team.lo, ref err) <= 0)
                                {
                                    pointErr = true;
                                    errMsg += " Loan Rep " + rec.team.lo + " does not have a user account in the system.<br/>";
                                }
                            }
                        }
                    }
                }

                return pointErr;
            }
            catch (Exception ex)
            {
                err = "CheckPointData, Exception: " + ex.Message;
                int Event_id = 9319;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return pointErr;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9320;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceWarning(err);
                }
                if (pointErr)
                {
                    da.Save_PointImportHistory(fileId, errType, errMsg, ref err);
                }
            }
        }
        #endregion
        #region Database Hooks to process Borrower/Coborrower
        bool CheckContactRecord(Record.Contacts contact, string role, bool checkAddr, ref string err)
        {
            err = "";

            if ((contact == null) || (contact.FirstName == null) || (contact.FirstName.Trim() == String.Empty) ||
                (contact.LastName == null) || (contact.LastName.Trim() == String.Empty))
            {
                err = "Missing " + role + " name.<br/>";
                //int Event_id = 9321;
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                  
                return false;
            }

            if (!checkAddr)
                return true;

            if ((contact.MailingAddr == null) || (contact.MailingAddr == String.Empty) ||
                (contact.MailingCity == null) || (contact.MailingCity == String.Empty) ||
                (contact.MailingState == null) || (contact.MailingState == String.Empty) ||
                (contact.MailingZip == null) || (contact.MailingZip == String.Empty))
            {
                err = "Missing " + role + " mailing address.<br/>";
                //int Event_id = 9322;
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                  
                return false;
            }
            return true;
        }

        bool Check_Contact(Record.Contacts c, string role, List<Table.Contacts> cList, ref int ContactId, ref string errMsg)
        {
            bool checkMailing = false;
            ContactId = 0;
            string borrowerRole = ContactRoles.ContactRole_Borrower.Trim().ToUpper();
            string coBorrowerRole = ContactRoles.ContactRole_Coborrower.Trim().ToUpper();

            role = role.Trim().ToUpper();
            //if ((role == borrowerRole) || (role == coBorrowerRole))
            //    checkMailing = true;

            if ((cList == null) || (cList.Count == 0))
                return true;
            string firstName = "";
            string lastName = "";
            string cFirst = c.FirstName.Trim().ToUpper();
            string cLast = c.LastName.Trim().ToUpper();
            foreach (Table.Contacts cc in cList)
            {
                string contactRole = cc.ContactRoleName.Trim().ToUpper();
                lastName = cc.LastName.Trim().ToUpper();
                firstName = cc.FirstName.Trim().ToUpper();
                if (contactRole == role)
                {
                    if ((cFirst == cc.FirstName.Trim().ToUpper()) && (cLast == cc.LastName.Trim().ToUpper()))
                    {
                        ContactId = cc.ContactId;
                        if (cc.ContactCompanyId <= 0)
                            c.ContactCompanyId = "0";
                        else
                            c.ContactCompanyId = cc.ContactCompanyId.ToString();
                        if (cc.ContactBranchId <= 0)
                            c.ContactBranchId = "0";
                        else
                            c.ContactBranchId = cc.ContactBranchId.ToString();
                        return true;
                    }
                }
            }

            if (ContactId == 0)
            {
                ContactId = da.GetContactIdByName(c.FirstName, c.LastName, ref errMsg);
            }

            if (CheckContactRecord(c, role, checkMailing, ref errMsg) == false)
            {
                return false;
            }
            return true;
        }
        int FindBrwCoBrwContactId(bool BRW, Record.Contacts contact, List<Table.Contacts> loanContactList, out string err)
        {
            int contactId = 0;
            err = string.Empty;
            string ContactRoleName = BRW ? ContactRoles.ContactRole_Borrower : ContactRoles.ContactRole_Coborrower;
            foreach (Table.Contacts c in loanContactList)
            {
                if (c.ContactRoleName != ContactRoles.ContactRole_Borrower && c.ContactRoleName != ContactRoles.ContactRole_Coborrower)
                    continue;
                if (c.ContactRoleName == ContactRoleName)
                {
                    contactId = c.ContactId;
                    return contactId;
                }
            }

            return contactId;
        }

        int Update_BorrowerContact(int fileId, Record.Contacts contact, bool BRW, List<Table.Contacts> cList, bool updateContact, ref  string errMsg)
        {
            string contactRole = BRW ? ContactRoles.ContactRole_Borrower : ContactRoles.ContactRole_Coborrower;
            string err = string.Empty;
            int contactId = 0;
            if (Check_Contact(contact, contactRole, cList, ref contactId, ref errMsg) == false)
            {
                Trace.TraceWarning(errMsg);
                //return false;
            }

            if (contactId > 0)
                contact.ContactId = contactId;
            else
                contact.ContactId = 0;

            if (contactId <= 0)
            {
                contactId = da.Save_Contacts(contact, ContactRoles.ContactRole_Borrower, contactId, ref err);
            }
            else
            {
                if (updateContact)
                {
                    contactId = da.Save_Contacts(contact, contactRole, contactId, ref err);
                }
            }

            if (contactId > 0)
                da.Save_LoanContacts(fileId, contactRole, contactId, ref err);
            return contactId;
        }

        bool ProcessBorrower_Coborrower(Record.LoanRecord rec, List<Table.Contacts> cList, ProspectFlagEnum Prospect_flag, bool ActiveLoan, ref string err)
        {
            err = "";
            int EmplId = 0;
            int current_emplid = 0;
            int contactId = 0;
            int prospectId = 0;
            int ProspectAssetId = 0;
            int ProspectIncomeId = 0;
            int ProspectOtherIncomeId = 0;
            string errMsg = "";
            int fileId = rec.loans.FileId;
            bool logErr = false;
            try
            {
                bool updateContact = (ActiveLoan || Prospect_flag == ProspectFlagEnum.ScheduledProspectFlag || Prospect_flag == ProspectFlagEnum.RealtimeProspectFlag) ? true : false;
                if (rec.borrower_contacts == null || string.IsNullOrEmpty(rec.borrower_contacts.FirstName) || string.IsNullOrEmpty(rec.borrower_contacts.LastName))
                {
                    errMsg = "Missing Borrower First or Last Name.";
                    errMsg = "Point File Path: " + rec.loans.PointFilePath + ", File ID: " + rec.loans.FileId + ", Error: " + errMsg;
                    da.Remove_LoanContacts(fileId, ContactRoles.ContactRole_Borrower, contactId, ref err);
                    int Event_id = 9323;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                contactId = Update_BorrowerContact(fileId, rec.borrower_contacts, true, cList, updateContact, ref err);
                if (contactId <= 0)
                {
                    err = errMsg;
                    logErr = true;
                    return false;
                }

                prospectId = da.Save_Prospect(contactId, rec.loans.UserID, ref err);
                EmplId = da.Save_ProspectEmployment(rec.borrower_contacts, contactId, ref current_emplid, ref err);
                ProspectAssetId = da.Save_ProspectAssets(rec.borrower_contacts, contactId, ref err);
                ProspectIncomeId = da.Save_ProspectIncome(rec.borrower_contacts, contactId, current_emplid, ref err);
                ProspectOtherIncomeId = da.Save_ProspectOtherIncome(rec.borrower_contacts, contactId, ref err);

                // handle Coborrower, if Coborrower is not present, remove the loan contact
                if ((rec.coborrower_contacts == null) || (rec.coborrower_contacts.FirstName == null) || (rec.coborrower_contacts.LastName == null))
                {
                    if (da.Remove_LoanContacts(fileId, ContactRoles.ContactRole_Coborrower, 0, ref err) == false)
                    {
                        int Event_id = 9324;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                    }
                    return true;
                }

                if ((rec.coborrower_contacts.MailingAddr == null) || (rec.coborrower_contacts.MailingAddr == String.Empty) ||
                   (rec.coborrower_contacts.MailingCity == null) || (rec.coborrower_contacts.MailingCity == String.Empty) ||
                   (rec.coborrower_contacts.MailingState == null) || (rec.coborrower_contacts.MailingState == String.Empty) ||
                   (rec.coborrower_contacts.MailingZip == null) || (rec.coborrower_contacts.MailingZip == String.Empty))
                {
                    rec.coborrower_contacts.MailingAddr = rec.borrower_contacts.MailingAddr;
                    rec.coborrower_contacts.MailingCity = rec.borrower_contacts.MailingCity;
                    rec.coborrower_contacts.MailingState = rec.borrower_contacts.MailingState;
                    rec.coborrower_contacts.MailingZip = rec.borrower_contacts.MailingZip;
                }

                contactId = Update_BorrowerContact(fileId, rec.coborrower_contacts, false, cList, updateContact, ref err);
                if (contactId <= 0)
                {
                    errMsg = err;
                    int Event_id = 9325;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                prospectId = da.Save_Prospect(contactId, rec.loans.UserID, ref err);
                EmplId = da.Save_ProspectEmployment(rec.coborrower_contacts, contactId, ref current_emplid, ref err);
                ProspectIncomeId = da.Save_ProspectIncome(rec.coborrower_contacts, contactId, current_emplid, ref err);
                ProspectOtherIncomeId = da.Save_ProspectOtherIncome(rec.coborrower_contacts, contactId, ref err);
                return true;
            }
            catch (Exception ex)
            {
                err = ex.Message + ex.StackTrace;
                int Event_id = 9326;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    err = string.Format("ProcessBorrower_Coborrower, FileId {0}, Error: {1}", fileId, err);
                    int Event_id = 9327;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
                if (!string.IsNullOrEmpty(errMsg))
                {
                    err = errMsg;
                }
            }
        }

        #endregion
        #region Database Hooks to Loan Agents
        bool Process_AgentRecord(int fileId, Record.Agent agent, string ContactRole, string ServiceType, List<Table.Contacts> ContactList, ref string err)
        {
            int contactId = 0;
            string errMsg = "";
            if ((agent == null) || (agent.Contact == null))
            {
                err = "Loan Contact " + ContactRole + " information is empty, FileId=" + fileId;
                int Event_id = 9328;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if (Check_Contact(agent.Contact, ContactRole, ContactList, ref  contactId, ref errMsg) == false)
                return false;

            Int32 i32 = 0;
            //int companyId = Convert.ToInt32(agent.Contact.ContactCompanyId);
            int companyId = 0;
            if (Int32.TryParse(agent.Contact.ContactCompanyId, out i32))
            {
                companyId = i32;
            }
            else
            {
                companyId = 0;
            }
            //int contactBranchId = Convert.ToInt32(agent.Contact.ContactBranchId);
            int contactBranchId = 0;
            if (Int32.TryParse(agent.Contact.ContactBranchId, out i32))
            {
                contactBranchId = i32;
            }
            else
            {
                contactBranchId = 0;
            }
            Address adr = new Address();
            adr.Street = agent.Contact.MailingAddr;
            adr.City = agent.Contact.MailingCity;
            adr.State = agent.Contact.MailingState;
            adr.Zip = agent.Contact.MailingZip;

            if ((agent.Company != null) && (agent.Company != String.Empty))
            {
                if (da.Save_CardexCompany(agent.Company, null, "", ServiceType, ref companyId, ref err) == false)
                    return false;
                if (da.Save_ContactBranch(agent.Company, adr, companyId, ref contactBranchId, ref err) == false)
                    return false;
            }

            if (companyId <= 0)
                agent.Contact.ContactCompanyId = "0";
            else
                agent.Contact.ContactCompanyId = companyId.ToString();

            if (contactBranchId <= 0)
                agent.Contact.ContactBranchId = "0";
            else
                agent.Contact.ContactBranchId = contactBranchId.ToString();

            if ((agent.Company != null) && (agent.Company != String.Empty))
            {
                contactId = da.Save_Contacts(agent.Contact, ContactRole, contactId, ref err);
                if (contactId <= 0)
                    return false;

                da.Save_LoanContacts(fileId, ContactRole, contactId, ref err);
                return da.Save_ContactUsers(fileId, contactId, ref err);
            }
            else
            {
                return false;
            }
        }

        bool Process_Lender(int fileId, Record.Agent agent, List<Table.Contacts> cList, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (fileId <= 0)
                {
                    err = "Process_Lender, invalid FileId=" + fileId;
                    int Event_id = 9329;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if ((agent == null) || (agent == null) || (agent.Contact == null))
                {
                    err = "Process_Lender, missing Loan Contact info for Lender, FileId=" + fileId;
                    Trace.TraceError(err);
                    int Event_id = 9330;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return true;
                }

                if (Process_AgentRecord(fileId, agent, ContactRoles.ContactRole_Lender, ContactServiceTypes.ContactService_Lending, cList, ref err) == false)
                {
                    if (err != String.Empty && err != null)
                    {
                        err = "Process_Lender, " + err;
                        int Event_id = 9331;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Process_Lender, FileiD=" + fileId + " Exception: " + ex.Message;
                int Event_id = 9332;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9333;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }

            }
        }

        bool Process_Appraiser(int fileId, Record.Agent agent, List<Table.Contacts> cList, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (fileId <= 0)
                {
                    err = "Process_Appraiser, invalid FileId=" + fileId;
                    int Event_id = 9334;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if ((agent == null) || (agent.Contact == null))
                {
                    err = "Process_Appraiser, missing Loan Contact info, FileId=" + fileId;
                    int Event_id = 9335;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                    return true;
                }

                if (Process_AgentRecord(fileId, agent, ContactRoles.ContactRole_Appraiser, ContactServiceTypes.ContactService_Appraisal, cList, ref err) == false)
                {
                    if (err != String.Empty && err != null)
                    {
                        err = "Process_Appraiser, " + err;
                        int Event_id = 9336;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Process_Appraiser, FileiD=" + fileId + " Exception: " + ex.Message;
                int Event_id = 9337;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9338;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }

            }
        }

        bool Process_Builder(int fileId, Record.Agent agent, List<Table.Contacts> cList, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {

                if (fileId <= 0)
                {
                    err = "Process_Builder, invalid FileId=" + fileId;
                    int Event_id = 9339;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if ((agent == null) || (agent == null) || (agent.Contact == null))
                {
                    err = "Process_Builder, missing Loan Contact info, FileId=" + fileId;
                    int Event_id = 9340;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                    return true;
                }

                if (Process_AgentRecord(fileId, agent, ContactRoles.ContactRole_Builder, ContactServiceTypes.ContactService_Builder, cList, ref err) == false)
                {
                    if (err != String.Empty && err != null)
                    {
                        err = "Process_Builder, " + err;
                        int Event_id = 9341;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Process_Builder, FileiD=" + fileId + " Exception: " + ex.Message;
                int Event_id = 9342;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9343;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }

            }
        }

        bool Process_Buyers_Agent(int fileId, Record.Agent agent, List<Table.Contacts> cList, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (fileId <= 0)
                {
                    err = "Process_Buyers_Agent, invalid FileId=" + fileId;
                    int Event_id = 9344;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if ((agent == null) || (agent == null) || (agent.Contact == null))
                {
                    err = "Process_Buyers_Agent, missing Loan Contact info, FileId=" + fileId;
                    int Event_id = 9345;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                    return true;
                }

                if (Process_AgentRecord(fileId, agent, ContactRoles.ContactRole_BuyersAgent, ContactServiceTypes.ContactService_RealEstate, cList, ref err) == false)
                {
                    if (err != String.Empty && err != null)
                    {
                        err = "Process_Buyers_Agent, " + err;
                        int Event_id = 9346;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Process_Buyers_Agent, FileiD=" + fileId + " Exception: " + ex.Message;
                int Event_id = 9348;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9349;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }

            }
        }

        bool Process_Buyers_Attorney(int fileId, Record.Agent agent, List<Table.Contacts> cList, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (fileId <= 0)
                {
                    err = "Process_Buyers_Attorney, invalid FileId=" + fileId;
                    int Event_id = 9350;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                if ((agent == null) || (agent == null) || (agent.Contact == null))
                {
                    err = "Process_Buyers_Attorney, missing Loan Contact info, FileId=" + fileId;
                    int Event_id = 9351;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                    return true;
                }

                if (Process_AgentRecord(fileId, agent, ContactRoles.ContactRole_BuyersAttorney, ContactServiceTypes.ContactService_RealEstate, cList, ref err) == false)
                {
                    if (err != String.Empty && err != null)
                    {
                        err = "Process_Buyers_Attorney, " + err;
                        int Event_id = 9352;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Process_Buyers_Attorney, FileiD=" + fileId + " Exception: " + ex.Message;
                int Event_id = 9353;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9354;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }

            }
        }

        bool Process_ClosingAgent(int fileId, Record.Agent agent, List<Table.Contacts> cList, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (fileId <= 0)
                {
                    err = "Process_ClosingAgent, invalid FileId=" + fileId;
                    int Event_id = 9355;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if ((agent == null) || (agent == null) || (agent.Contact == null))
                {
                    err = "Process_ClosingAgent, missing Loan Contact info, FileId=" + fileId;
                    int Event_id = 9356;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                    return true;
                }

                if (Process_AgentRecord(fileId, agent, ContactRoles.ContactRole_Closing, ContactServiceTypes.ContactService_Closing, cList, ref err) == false)
                {
                    if (err != String.Empty && err != null)
                    {
                        err = "Process_ClosingAgent, " + err;
                        int Event_id = 9357;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Process_ClosingAgent, FileiD=" + fileId + " Exception: " + ex.Message;
                int Event_id = 9358;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9359;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }

            }
        }

        bool Process_FloodInsurance(int fileId, Record.Agent agent, List<Table.Contacts> cList, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (fileId <= 0)
                {
                    err = "Process_FloodInsurance, invalid FileId=" + fileId;
                    int Event_id = 9360;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if ((agent == null) || (agent == null) || (agent.Contact == null))
                {
                    err = "Process_FloodInsurance, missing Loan Contact info, FileId=" + fileId;
                    int Event_id = 9361;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                    return true;
                }

                if (Process_AgentRecord(fileId, agent, ContactRoles.ContactRole_FloodInsurance, ContactServiceTypes.ContactService_FloodInsurance, cList, ref err) == false)
                {
                    if (err != String.Empty && err != null)
                    {
                        err = "Process_FloodInsurance, " + err;
                        int Event_id = 9362;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Process_FloodInsurance, FileiD=" + fileId + " Exception: " + ex.Message;
                int Event_id = 9363;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9364;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }

            }
        }

        bool Process_HazardInsurance(int fileId, Record.Agent agent, List<Table.Contacts> cList, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (fileId <= 0)
                {
                    err = "Process_HazardInsurance, invalid FileId=" + fileId;
                    int Event_id = 9365;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if ((agent == null) || (agent == null) || (agent.Contact == null))
                {
                    err = "Process_HazardInsurance, missing Loan Contact info, FileId=" + fileId;
                    int Event_id = 9366;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                    return true;
                }

                if (Process_AgentRecord(fileId, agent, ContactRoles.ContactRole_HazardInsurance, ContactServiceTypes.ContactService_HazardInsurance, cList, ref err) == false)
                {
                    if (err != String.Empty && err != null)
                    {
                        err = "Process_HazardInsurance, " + err;
                        int Event_id = 9367;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Process_HazardInsurance, FileiD=" + fileId + " Exception: " + ex.Message;
                int Event_id = 9368;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9369;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }

            }
        }

        bool Process_Investor(int fileId, Record.Agent agent, List<Table.Contacts> cList, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (fileId <= 0)
                {
                    err = "Process_Investor, invalid FileId=" + fileId;
                    int Event_id = 9370;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if ((agent == null) || (agent == null) || (agent.Contact == null))
                {
                    err = "Process_Investor, missing Loan Contact info, FileId=" + fileId;
                    int Event_id = 9371;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                    return true;
                }

                if (Process_AgentRecord(fileId, agent, ContactRoles.ContactRole_Investor, ContactServiceTypes.ContactService_Investor, cList, ref err) == false)
                {
                    if (err != String.Empty && err != null)
                    {
                        err = "Process_Investor, " + err;
                        int Event_id = 9372;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Process_Investor, FileiD=" + fileId + " Exception: " + ex.Message;
                int Event_id = 9373;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 9374;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }

            }
        }

        bool Process_ListingAgent(int fileId, Record.Agent agent, List<Table.Contacts> cList, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (fileId <= 0)
                {
                    err = "Process_ListingAgent, invalid FileId=" + fileId;
                    int Event_id = 9375;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if ((agent == null) || (agent == null) || (agent.Contact == null))
                {
                    err = "Process_ListingAgent, missing Loan Contact info, FileId=" + fileId;
                    int Event_id = 9376;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return true;
                }

                if (Process_AgentRecord(fileId, agent, ContactRoles.ContactRole_ListingAgent, ContactServiceTypes.ContactService_RealEstate, cList, ref err) == false)
                {
                    if (err != String.Empty && err != null)
                    {
                        err = "Process_ListingAgent, " + err;
                        int Event_id = 9377;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Process_ListingAgent, FileiD=" + fileId + " Exception: " + ex.Message;
                int Event_id = 9378;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9379;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }

            }
        }

        bool Process_MortgageBroker(int fileId, Record.Agent agent, List<Table.Contacts> cList, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (fileId <= 0)
                {
                    err = "Process_MortgageBroker, invalid FileId=" + fileId;
                    int Event_id = 9380;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if ((agent == null) || (agent == null) || (agent.Contact == null))
                {
                    err = "Process_MortgageBroker, missing Loan Contact info, FileId=" + fileId;
                    int Event_id = 9381;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return true;
                }

                if (Process_AgentRecord(fileId, agent, ContactRoles.ContactRole_MortgageBroker, ContactServiceTypes.ContactService_Mortgage, cList, ref err) == false)
                {
                    if (err != String.Empty && err != null)
                    {
                        err = "Process_MortgageBroker, " + err;
                        int Event_id = 9382;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Process_MortgageBroker, FileiD=" + fileId + " Exception: " + ex.Message;
                int Event_id = 9383;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9384;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }

            }
        }

        bool Process_MortgageInsurance(int fileId, Record.Agent agent, List<Table.Contacts> cList, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (fileId <= 0)
                {
                    err = "Process_MortgageInsurance, invalid FileId=" + fileId;
                    int Event_id = 9385;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if ((agent == null) || (agent == null) || (agent.Contact == null))
                {
                    err = "Process_MortgageInsurance, missing Loan Contact info, FileId=" + fileId;
                    int Event_id = 9386;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return true;
                }

                if (Process_AgentRecord(fileId, agent, ContactRoles.ContactRole_MortgageInsurance, ContactServiceTypes.ContactService_MortgageInsurance, cList, ref err) == false)
                {
                    if (err != String.Empty && err != null)
                    {
                        err = "Process_MortgageInsurance, " + err;
                        int Event_id = 9387;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Process_MortgageInsurance, FileiD=" + fileId + " Exception: " + ex.Message;
                int Event_id = 9388;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9389;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        bool Process_SellingAgent(int fileId, Record.Agent agent, List<Table.Contacts> cList, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (fileId <= 0)
                {
                    err = "Process_SellingAgent, invalid FileId=" + fileId;
                    int Event_id = 9390;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if ((agent == null) || (agent == null) || (agent.Contact == null))
                {
                    err = "Process_SellingAgent, missing Loan Contact info, FileId=" + fileId;
                    int Event_id = 9391;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return true;
                }

                if (Process_AgentRecord(fileId, agent, ContactRoles.ContactRole_SellingAgent, ContactServiceTypes.ContactService_RealEstate, cList, ref err) == false)
                {
                    if (err != String.Empty && err != null)
                    {
                        err = "Process_SellingAgent, " + err;
                        int Event_id = 9392;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Process_SellingAgent, FileiD=" + fileId + " Exception: " + ex.Message;
                int Event_id = 9393;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9394;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }

            }
        }

        bool Process_Title(int fileId, Record.Agent agent, List<Table.Contacts> cList, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (fileId <= 0)
                {
                    err = "Process_Title, invalid FileId=" + fileId;
                    int Event_id = 9395;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if ((agent == null) || (agent == null) || (agent.Contact == null))
                {
                    err = "Process_Title, missing Loan Contact info, FileId=" + fileId;
                    int Event_id = 9396;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return true;
                }

                if (Process_AgentRecord(fileId, agent, ContactRoles.ContactRole_Title, ContactServiceTypes.ContactService_Title, cList, ref err) == false)
                {
                    if (err != String.Empty && err != null)
                    {
                        err = "Process_Title, " + err;
                        int Event_id = 9397;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Process_Title, FileiD=" + fileId + " Exception: " + ex.Message;
                int Event_id = 9398;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9399;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        bool Process_Agents(int fileId, Record.LoanAgents agents, List<Table.Contacts> cList, ref string err)
        {
            bool logErr = false;
            if (fileId <= 0)
            {
                err = "Process_Agents,  invalid FileId=" + fileId;
                int Event_id = 9410;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if (agents == null)
            {
                err = "Process_Agents, missing loan agent list and info, fileId = " + fileId;
                int Event_id = 9411;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            try
            {
                if (agents.lender_contact1 != null)
                    Process_Lender(fileId, agents.lender_contact1, cList, ref err);

                if (agents.appraiser != null)
                    Process_Appraiser(fileId, agents.appraiser, cList, ref err);

                if (agents.builder != null)
                    Process_Builder(fileId, agents.builder, cList, ref err);

                if (agents.buyers_agent != null)
                    Process_Buyers_Agent(fileId, agents.buyers_agent, cList, ref err);

                if (agents.buyers_attorney != null)
                    Process_Buyers_Attorney(fileId, agents.buyers_attorney, cList, ref err);

                if (agents.closing_agent != null)
                    Process_ClosingAgent(fileId, agents.closing_agent, cList, ref err);

                if (agents.flood_insurance != null)
                    Process_FloodInsurance(fileId, agents.flood_insurance, cList, ref err);

                if (agents.hazard_insurance != null)
                    Process_HazardInsurance(fileId, agents.hazard_insurance, cList, ref err);

                if (agents.investor != null)
                    Process_Investor(fileId, agents.investor, cList, ref err);

                if (agents.listing_agent != null)
                    Process_ListingAgent(fileId, agents.listing_agent, cList, ref err);

                if (agents.mortgage_broker != null)
                    Process_MortgageBroker(fileId, agents.mortgage_broker, cList, ref err);

                if (agents.mortgage_insurance != null)
                    Process_MortgageInsurance(fileId, agents.mortgage_insurance, cList, ref err);

                if (agents.selling_agent != null)
                    Process_SellingAgent(fileId, agents.selling_agent, cList, ref err);

                if (agents.title_insurance != null)
                    Process_Title(fileId, agents.title_insurance, cList, ref err);
            }
            catch (Exception ex)
            {
                err = "Process_Agents, Exception: " + ex.Message;
                int Event_id = 9412;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9414;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
            return true;
        }
        #endregion
        #region Database Hooks to notes, Conditions, Docs and other misc fields
        bool ProcessNotes(int FileId, List<Table.LoanNotes> noteList, ref string err)
        {
            err = "";
            bool logErr = false;
            List<Table.LoanNotes> dbNoteList = null;
            try
            {
                if (FileId <= 0)
                {
                    err = "ProcessNotes, invalid FileId specified, " + FileId;
                    int Event_id = 9415;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                if ((noteList == null) || (noteList.Count <= 0))
                    return false;

                dbNoteList = da.Get_LoanNotes(FileId, ref err);

                if ((dbNoteList != null) && (dbNoteList.Count > 0))
                {
                    DateTime noteTime = DateTime.MinValue;
                    string sender = "";
                    string note = "";
                    int noteId;
                    foreach (Table.LoanNotes dbNote in dbNoteList)
                    {
                        noteTime = dbNote.Created;
                        noteId = dbNote.NoteId;
                        sender = dbNote.Sender;
                        note = dbNote.Note;
                        foreach (Table.LoanNotes n in noteList)
                        {
                            if (n.NoteId > 0)
                                continue;
                            if ((noteTime.Date != n.Created.Date) || (noteTime.Hour != n.Created.Hour) || (noteTime.Minute != n.Created.Minute))
                                continue;
                            if (sender.Trim() != n.Sender.Trim())
                                continue;
                            if (note.Trim() != n.Note.Trim())
                                continue;
                            n.NoteId = noteId;
                            n.Exported = true;
                            break;
                        }
                    }
                    int[] ix = new int[noteList.Count];
                    int i = 0;
                    foreach (Table.LoanNotes n in noteList)
                    {
                        if (n.NoteId > 0)
                        {
                            if (noteList.IndexOf(n) >= 0)
                            {
                                ix[i] = noteList.IndexOf(n);
                                i++;
                            }
                        }
                    }
                    int j = 0;
                    for (j = i - 1; j >= 0; j--)
                    {
                        int idx = ix[j];
                        noteList.RemoveAt(idx);
                    }
                }

                if (noteList.Count > 0)
                {
                    if (da.Update_LoanNotes(FileId, noteList, ref err) == false)
                    {
                        logErr = true;
                    }
                }

                bool st = true;
                bool duplicate = false;
                DateTime dt = DateTime.MinValue;

                List<Table.LoanNotes> clean_notes = new List<Table.LoanNotes>();
                clean_notes.Clear();

                if ((dbNoteList != null) && (dbNoteList.Count > 0))
                {
                    DateTime noteTime = DateTime.MinValue;
                    foreach (Table.LoanNotes dbNote in dbNoteList)
                    {
                        duplicate = false;
                        Table.LoanNotes clean_note = dbNote;
                        if (clean_notes.Count > 0)
                        {
                            foreach (Table.LoanNotes exist_clean_note in clean_notes)
                            {
                                if ((clean_note.Created.Date == exist_clean_note.Created.Date) &&
                                    (clean_note.Created.Hour == exist_clean_note.Created.Hour) &&
                                    (clean_note.Created.Minute == exist_clean_note.Created.Minute) &&
                                    (clean_note.Note.Trim() == exist_clean_note.Note.Trim()) &&
                                    (clean_note.Sender.Trim() == exist_clean_note.Sender.Trim()))
                                {
                                    duplicate = true;
                                    st = da.DeleteLoanNotes(clean_note.NoteId, ref err);
                                    continue;
                                }
                            }
                        }
                        if (duplicate == false)
                        {
                            clean_notes.Add(clean_note);
                        }
                    }
                }

                st = da.DeleteLoanNotesBlank(FileId, ref err);

                dbNoteList = da.Get_LoanNotes_DESC(FileId, ref err);

                PointMgr pm = PointMgr.Instance;
                AddNoteRequest addnote_req = new AddNoteRequest();
                ReqHdr req_hdr = new ReqHdr();

                req_hdr.UserId = 1;
                addnote_req.hdr = req_hdr;

                addnote_req.FileId = FileId;

                bool sta = true;

                if ((dbNoteList != null) && (dbNoteList.Count > 0))
                {
                    foreach (Table.LoanNotes dbNote in dbNoteList)
                    {
                        addnote_req.Created = dbNote.Created;
                        addnote_req.NoteTime = dbNote.Created;
                        addnote_req.Note = dbNote.Note;
                        addnote_req.Sender = dbNote.Sender;

                        if (dbNote.Note != "")
                        {
                            sta = pm.AddNote(addnote_req, ref err);
                            Thread.Sleep(30);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                err = "ProcessLoanNotes, Exception: " + ex.Message;
                int Event_id = 9416;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (dbNoteList != null)
                {
                    dbNoteList.Clear();
                    dbNoteList = null;
                }
                if (noteList != null)
                {
                    noteList.Clear();
                    noteList = null;
                }
                if (logErr)
                {
                    int Event_id = 9418;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        bool SyncConditions(int FileId, List<Record.Conditions> condList, List<Record.Conditions> dbCondList, ref string err)
        {
            if ((dbCondList == null || dbCondList.Count <= 0) && (condList == null || condList.Count <= 0))
                return true;

            if (dbCondList == null || dbCondList.Count <= 0)
            {
                foreach (Record.Conditions cond in condList)
                {
                    cond.LoanCondId = 0;
                }
                return da.UpdateConditions(FileId, condList, ref err);
            }

            if (condList == null || condList.Count <= 0)
            {
                foreach (Record.Conditions cond in dbCondList)
                {
                    cond.Delete = true;
                }
                return da.UpdateConditions(FileId, dbCondList, ref err);
            }

            bool found = false;
            foreach (Record.Conditions dbCond in dbCondList)
            {
                found = false;
                foreach (Record.Conditions pCond in condList)
                {
                    if (dbCond.CondName.Trim().ToUpper() == pCond.CondName.Trim().ToUpper())
                    {
                        dbCond.CondType = pCond.CondType;
                        dbCond.Cleared = pCond.Cleared;
                        dbCond.ClearedBy = pCond.ClearedBy;
                        dbCond.Collected = pCond.Collected;
                        dbCond.CollectedBy = pCond.CollectedBy;
                        dbCond.Created = pCond.Created;
                        dbCond.CreatedBy = pCond.CreatedBy;
                        dbCond.Due = pCond.Due;
                        dbCond.Received = pCond.Received;
                        dbCond.ReceivedBy = pCond.ReceivedBy;
                        dbCond.Submitted = pCond.Submitted;
                        dbCond.SubmittedBy = pCond.SubmittedBy;
                        dbCond.Status = pCond.Status;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    dbCond.Delete = true;
                }
            }

            foreach (Record.Conditions pCond in condList)
            {
                found = false;
                foreach (Record.Conditions dbCond in dbCondList)
                {
                    if (dbCond.CondName.Trim().ToUpper() == pCond.CondName.Trim().ToUpper())
                    {
                        found = true;
                        dbCond.Delete = false;
                        break;
                    }
                }
                if (!found)
                {
                    dbCondList.Add(pCond);
                }
            }
            return da.UpdateConditions(FileId, dbCondList, ref err);
        }

        bool ProcessConditions(int FileId, List<Record.Conditions> condList, ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            List<Record.Conditions> dbConditionList = null;
            try
            {
                if (FileId <= 0)
                {
                    err = string.Format("ProcessConditions, invalid FileId {0} specified, ", FileId);
                    int Event_id = 9419;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                dbConditionList = da.GetConditionList(FileId, ref err);
                SyncConditions(FileId, condList, dbConditionList, ref err);
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("ProcessConditions FileId {0}, Exception: {1}. ", FileId, ex.Message);
                int Event_id = 9420;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9421;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        bool SyncDocs(int FileId, List<Record.Docs> docList, List<Record.Docs> dbDocList, ref string err)
        {
            if ((dbDocList == null || dbDocList.Count <= 0) && (docList == null || docList.Count <= 0))
                return true;

            if (dbDocList == null || dbDocList.Count <= 0)
            {
                foreach (Record.Docs doc in docList)
                {
                    doc.BasicDocId = 0;
                }
                return da.UpdateDocs(FileId, docList, ref err);
            }

            if (docList == null || docList.Count <= 0)
            {
                foreach (Record.Docs doc in dbDocList)
                {
                    doc.Delete = true;
                }
                return da.UpdateDocs(FileId, dbDocList, ref err);
            }

            bool found = false;
            foreach (Record.Docs dbDoc in dbDocList)
            {
                found = false;
                foreach (Record.Docs pDoc in docList)
                {
                    if (dbDoc.DocName.Trim().ToUpper() == pDoc.DocName.Trim().ToUpper())
                    {
                        dbDoc.Cleared = pDoc.Cleared;
                        dbDoc.ClearedBy = pDoc.ClearedBy;
                        dbDoc.Due = pDoc.Due;
                        dbDoc.Received = pDoc.Received;
                        dbDoc.ReceivedBy = pDoc.ReceivedBy;
                        dbDoc.Submitted = pDoc.Submitted;
                        dbDoc.SubmittedBy = pDoc.SubmittedBy;
                        dbDoc.Status = pDoc.Status;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    dbDoc.Delete = true;
                }
            }

            foreach (Record.Docs pDoc in docList)
            {
                found = false;
                foreach (Record.Docs dbDoc in dbDocList)
                {
                    if (dbDoc.DocName.Trim().ToUpper() == pDoc.DocName.Trim().ToUpper())
                    {
                        found = true;
                        dbDoc.Delete = false;
                        break;
                    }
                }
                if (!found)
                {
                    dbDocList.Add(pDoc);
                    //break;
                }
            }
            return da.UpdateDocs(FileId, dbDocList, ref err);
        }

        bool ProcessDocs(int FileId, List<Record.Docs> docList, ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            List<Record.Docs> dbDocList = null;
            try
            {
                if (FileId <= 0)
                {
                    err = string.Format("ProcessDocs, invalid FileId {0} specified, ", FileId);
                    int Event_id = 9422;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }

                dbDocList = da.GetDocList(FileId, ref err);
                SyncDocs(FileId, docList, dbDocList, ref err);
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("ProcessConditions FileId {0}, Exception: {1}. ", FileId, ex.Message);
                int Event_id = 9423;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9424;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }
        #endregion

        #region Save Loan  & Stage Information
        private bool Process_Loan_Table(Common.Record.Loans loans_record, List<Table.LoanStages> stageList, ProspectFlagEnum Prospect_flag, string LO_Name, out bool activeLoan)
        {
            bool logErr = false;
            activeLoan = false;
            string err = "";
            int fileId = 0;
            if ((loans_record == null) || (loans_record.FileId <= 0))
            {
                err = "Process_Loan_Table Loans Record is null or FieldId is invalid.";
                int Event_id = 9425;
                short Category = 99;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
                return false;
            }
            fileId = loans_record.FileId;
            Table.PointFileInfo pf_Info = da.GetPointFileInfo(loans_record.FileId, ref err);
            if (pf_Info == null)
            {
                err = string.Format("Process_Loan_Table, unable to get PointFiles record for FileiD={0}. ", loans_record.FileId);
                int Event_id = 9426;
                short Category = 99;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
                return false;
            }
            LoanStatusEnum loanStatus = LoanStatusEnum.Prospect;
            if (pf_Info.FolderId > 0)
            {
                PointFolderInfo pf = da.GetPointFolderInfo(pf_Info.FolderId, ref err);
                if (pf != null)
                    loanStatus = (LoanStatusEnum)pf.LoanStatus;
            }
            Table.Loans loans_table = new Table.Loans();
            try
            {
                loans_table.FileId = loans_record.FileId;
                loans_table.CurrentStage = PointStage.Application;
                #region Miscellaneous fields
                double dbe;
                Int32 i32;
                Int16 i16;

                if (Double.TryParse(loans_record.AppraisedValue, out dbe))
                {
                    loans_table.AppraisedValue = dbe;
                }
                else
                {
                    loans_table.AppraisedValue = 0;
                }
                loans_table.CCScenario = loans_record.CCScenario;
                //loans_table.CLTV = Convert.ToDouble(loans_record.CLTV);
                if (Double.TryParse(loans_record.CLTV, out dbe))
                {
                    loans_table.CLTV = dbe;
                }
                else
                {
                    loans_table.CLTV = 0;
                }
                loans_table.County = loans_record.County;
                //loans_table.DownPay = Convert.ToDouble(loans_record.DownPay);
                if (Double.TryParse(loans_record.DownPay, out dbe))
                {
                    loans_table.DownPay = dbe;
                }
                else
                {
                    loans_table.DownPay = 0;
                }
                //loans_table.Lender = Convert.ToInt32(loans_record.Lender);
                if (Int32.TryParse(loans_record.Lender, out i32))
                {
                    loans_table.Lender = i32;
                }
                else
                {
                    loans_table.Lender = 0;
                }
                loans_table.LienPosition = loans_record.LienPosition;
                //loans_table.LoanAmount = Convert.ToDouble(loans_record.LoanAmount);
                if (Double.TryParse(loans_record.LoanAmount, out dbe))
                {
                    loans_table.LoanAmount = dbe;
                }
                else
                {
                    loans_table.LoanAmount = 0;
                }
                loans_table.LoanNumber = loans_record.LoanNumber;
                loans_table.LoanType = loans_record.LoanType;
                //loans_table.LTV = Convert.ToDouble(loans_record.LTV);
                if (Double.TryParse(loans_record.LTV, out dbe))
                {
                    loans_table.LTV = dbe;
                }
                else
                {
                    loans_table.LTV = 0;
                }
                //loans_table.MonthlyPayment = Convert.ToDouble(loans_record.MonthlyPayment);
                if (Double.TryParse(loans_record.MonthlyPayment, out dbe))
                {
                    loans_table.MonthlyPayment = dbe;
                }
                else
                {
                    loans_table.MonthlyPayment = 0;
                }
                loans_table.LenderNotes = loans_record.LenderNotes;
                loans_table.Occupancy = loans_record.Occupancy;
                loans_table.Program = loans_record.Program;
                loans_table.PropertyAddr = loans_record.PropertyAddr;
                loans_table.PropertyCity = loans_record.PropertyCity;
                loans_table.PropertyState = loans_record.PropertyState;
                loans_table.PropertyZip = loans_record.PropertyZip;
                loans_table.Purpose = loans_record.Purpose;
                if (Double.TryParse(loans_record.Rate, out dbe))
                {
                    loans_table.Rate = dbe;
                }
                else
                {
                    loans_table.Rate = 0;
                }
                if (Double.TryParse(loans_record.SalesPrice, out dbe))
                {
                    loans_table.SalesPrice = dbe;
                }
                else
                {
                    loans_table.SalesPrice = 0;
                }
                if (Int16.TryParse(loans_record.Term, out i16))
                {
                    loans_table.Term = i16;
                }
                else
                {
                    loans_table.Term = 0;
                }
                if (Int16.TryParse(loans_record.Due, out i16))
                {
                    loans_table.Due = i16;
                }
                else
                {
                    loans_table.Due = 0;
                }

                DateTime dtPurchasedDate;
                if (DateTime.TryParse(loans_record.PurchasedDate, out dtPurchasedDate))
                {
                    loans_table.PurchasedDate = dtPurchasedDate;
                }
                else
                {
                    loans_table.PurchasedDate = null;
                }
                loans_table.PropertyType = loans_record.PropertyType;

                if (string.IsNullOrEmpty(loans_record.LeadRanking))
                    loans_table.LeadRanking = string.Empty;
                else
                    loans_table.LeadRanking = loans_record.LeadRanking == "Cool" ? "Cold" : loans_record.LeadRanking;  // Hot, Warm, Cool -->Cold
                #endregion
                #region Status Dates
                DateTime dt = DateTime.MinValue;
                if (DateTime.TryParse(loans_record.DateOpen, out dt))
                    loans_table.DateOpen = dt;
                if (DateTime.TryParse(loans_record.DateHMDA, out dt))
                    loans_table.DateHMDA = DateTime.MinValue;
                loans_table.DateHMDA = dt;
                if (DateTime.TryParse(loans_record.DateProcessing, out dt))
                    loans_table.DateProcessing = dt;
                if (DateTime.TryParse(loans_record.DateSubmit, out dt))
                    loans_table.DateSubmit = dt;
                loans_table.DateApprove = DateTime.MinValue;
                if (DateTime.TryParse(loans_record.DateApprove, out dt))
                    loans_table.DateApprove = dt;
                loans_table.DateClearToClose = DateTime.MinValue;
                if (DateTime.TryParse(loans_record.DateClearToClose, out dt))
                    loans_table.DateClearToClose = dt;
                loans_table.DateClose = DateTime.MinValue;
                if (DateTime.TryParse(loans_record.DateClose, out dt))
                    loans_table.DateClose = dt;
                loans_table.DateDocs = DateTime.MinValue;
                if (DateTime.TryParse(loans_record.DateDocs, out dt))
                    loans_table.DateDocs = dt;
                loans_table.DateDocsOut = DateTime.MinValue;
                if (DateTime.TryParse(loans_record.DateDocsOut, out dt))
                    loans_table.DateDocsOut = dt;
                loans_table.DateDocsReceived = DateTime.MinValue;
                if (DateTime.TryParse(loans_record.DateDocsReceived, out dt))
                    loans_table.DateDocsReceived = dt;
                loans_table.DateFund = DateTime.MinValue;
                if (DateTime.TryParse(loans_record.DateFund, out dt))
                    loans_table.DateFund = dt;
                loans_table.DateRecord = DateTime.MinValue;
                if (DateTime.TryParse(loans_record.DateRecord, out dt))
                    loans_table.DateRecord = dt;
                loans_table.DateSuspended = DateTime.MinValue;
                if (DateTime.TryParse(loans_record.DateSuspended, out dt))
                    loans_table.DateSuspended = dt;
                loans_table.DateReSubmit = DateTime.MinValue;
                if (DateTime.TryParse(loans_record.DateReSubmit, out dt))
                    loans_table.DateReSubmit = dt;
                #endregion
                switch (Prospect_flag)
                {
                    case ProspectFlagEnum.ScheduledProspectFlag:
                    case ProspectFlagEnum.RealtimeProspectFlag:
                        loans_table.LoanStatus = LoanStatus.LoanStatus_Prospect;
                        if (loanStatus == LoanStatusEnum.ProspectArchive)
                            loans_table.ProspectLoanStatus = "Uncategorized Archive";
                        else
                            loans_table.ProspectLoanStatus = "Active";
                        loans_table.CurrentStage = string.Empty;
                        break;
                    case ProspectFlagEnum.ScheduledPointFlag:
                    case ProspectFlagEnum.RealtimePointFlag:
                        loans_table.LoanStatus = "Processing";
                        Process_LoanStatus_Stages(loans_record, ref loans_table, stageList);
                        activeLoan = true;
                        break;
                    case ProspectFlagEnum.ArchivedLoansFlag:
                        Process_LoanStatus(loans_record, ref loans_table);
                        if (loans_table.DateCanceled == DateTime.MinValue &&
                            loans_table.DateClose == DateTime.MinValue &&
                            loans_table.DateDenied == DateTime.MinValue &&
                            loans_table.DateSuspended == DateTime.MinValue)
                            loans_table.LoanStatus = "Uncategorized Archive";
                        break;
                }
                if (pointConfig != null && pointConfig.Enable_MultiBranchFolders)
                {
                    if (da.Save_Loans(loans_record.FileId, loans_record.BranchId, loans_record.BranchName, loans_table, LO_Name, ref err) == false)
                    {
                        int Event_id = 9427;
                        short Category = 99;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
                        return false;
                    }
                    return true;
                }

                if (da.Save_Loans(loans_record.FileId, loans_record.BranchId, loans_table, LO_Name, ref err) == false)
                {
                    int Event_id = 9428;
                    short Category = 99;
                    err = "Point File Path: " + loans_record.PointFilePath + ", File ID: " + loans_record.FileId + ",  da.Save_Loans Error: " + err;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                string pointFilename = (pf_Info == null || string.IsNullOrEmpty(pf_Info.Path)) ? string.Empty : pf_Info.Path;
                err = string.Format("Process_Loan_Table, FileId={0}, PointFile={1}, \r\nException: {2}", loans_record.FileId, pointFilename, ex.Message);
                int Event_id = 9429;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9430;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }
        private List<Table.LoanStages> ImportLoanStages(Common.Record.Loans loans_record, int fileId, List<string> FieldArray, ArrayList FieldSeq)
        {
            string err = "";
            List<Table.LoanStages> stageList = da.GetLoanStagesByFileId(fileId, ref err);
            // if the loan already has loan stages and tasks, no need to set up the stages
            if (da.GetNumberLoanTasks(fileId, ref err) > 0 && stageList != null && stageList.Count > 0)
                return stageList;
            if (stageList == null)
                stageList = new List<Table.LoanStages>();
            SetupDefaultLoanStagesDates(FieldArray, FieldSeq, ref stageList);

            // if the loan already has loan stages in Pulse, no need to do the rest
            if (!da.NeedDefaultStages(fileId))
                return stageList;
            CheckGap(ref stageList);
            List<FieldMap> fieldMap = new List<FieldMap>();
            foreach (Table.LoanStages s in stageList)
            {
                if (s == null)
                    continue;
                if (s.PointNameField <= 0 && s.PointDateField <= 0)
                    continue;
                FieldMap fm = null;
                if (s.PointNameField > 0)
                {
                    fm = new FieldMap();
                    fm.FieldId = s.PointNameField;
                    fm.Value = s.StageName.Trim();
                    fieldMap.Add(fm);
                }

                FieldMap fm1 = null;
                if (s.PointDateField > 0 && s.Completed != DateTime.MinValue)
                {
                    fm1 = new FieldMap();
                    fm1.FieldId = s.PointDateField;
                    fm1.Value = s.Completed.Date.ToString();
                    fieldMap.Add(fm1);
                }
            }
            if (fieldMap.Count > 0)
            {
                string filePath = da.GetPointFilePath(fileId, ref err);
                if (string.IsNullOrEmpty(filePath))
                {
                    err = string.Format("ImportLoanStages, FileId={0}, filepath for the loan is empty, cannot update custom Point Stages in the Point file.", fileId);
                    int Event_id = 9431;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return stageList;
                }
                PNT.WritePointData(fieldMap, filePath, ref err);
            }
            return stageList;
        }

        private void FillUpStatusGap(ref List<Table.LoanStages> stageList, DateTime Completed, int SequenceNumber, DateTime LastCompleted)
        {
            foreach (Table.LoanStages ls in stageList)
            {
                if (ls.SequenceNumber <= SequenceNumber && ls.Completed == DateTime.MinValue)
                {
                    if (LastCompleted != DateTime.MinValue)
                        ls.Completed = LastCompleted;
                    else
                        ls.Completed = Completed;
                }
            }
        }

        private void CheckGap(ref List<Table.LoanStages> stageList)
        {
            DateTime Last_CompleteDate = DateTime.MinValue;
            DateTime First_CompleteDate = DateTime.MinValue;
            int SequenceNumber = 0;
            foreach (Table.LoanStages ls in stageList)
            {
                if (ls.Completed != DateTime.MinValue)
                {
                    First_CompleteDate = ls.Completed;
                    SequenceNumber = ls.SequenceNumber;
                    FillUpStatusGap(ref stageList, First_CompleteDate, SequenceNumber, Last_CompleteDate);
                }
                if (First_CompleteDate != DateTime.MinValue)
                    Last_CompleteDate = First_CompleteDate;
            }
        }
        #region obsolete code
        //private void UpdateStageDates(ref List<Table.LoanStages> stageList, Table.Loans loans_table)
        //{
        //    foreach (Table.LoanStages ls in stageList)
        //    {
        //        switch (ls.StageName)
        //        {
        //            case "Open":
        //            case PointStage.Application:
        //                ls.Completed = loans_table.DateOpen;
        //                break;
        //            case "HMDA":
        //            case PointStage.HMDACompleted:
        //            case PointStage.HMDAComplete:
        //                ls.Completed = loans_table.DateHMDA;
        //                break;
        //            case "Processing":
        //            case "Sent To Processing":
        //            case PointStage.SentToProcessing:
        //                ls.Completed = loans_table.DateProcessing;
        //                break;
        //            case PointStage.Submit:
        //            case PointStage.Submitted:
        //                ls.Completed = loans_table.DateSubmit;
        //                break;
        //            case PointStage.Approve:
        //            case PointStage.Approved:
        //                ls.Completed = loans_table.DateApprove;
        //                break;
        //            case PointStage.CleartoClose:
        //            case PointStage.ClearToClose:
        //                ls.Completed = loans_table.DateClearToClose;
        //                break;
        //            case PointStage.Close:
        //            case PointStage.Closed:
        //                ls.Completed = loans_table.DateClose;
        //                break;
        //            case PointStage.DocsDrawn:
        //                ls.Completed = loans_table.DateDocs;
        //                break;
        //            case PointStage.DocsOut:
        //            case "Docs":
        //                ls.Completed = loans_table.DateDocsOut;
        //                break;
        //            case PointStage.DocsReceived:
        //                ls.Completed = loans_table.DateDocsReceived;
        //                break;
        //            case PointStage.Fund:
        //            case PointStage.Funded:
        //                ls.Completed = loans_table.DateFund;
        //                break;
        //            case PointStage.Record:
        //            case PointStage.Recorded:
        //                ls.Completed = loans_table.DateRecord;
        //                break;
        //            case PointStage.Suspend:
        //            case PointStage.Suspended:
        //                ls.Completed = loans_table.DateSuspended;
        //                break;
        //            case PointStage.Resubmit:
        //            case PointStage.Re_submit:
        //            case PointStage.Resubmitted:
        //                ls.Completed = loans_table.DateReSubmit;
        //                break;
        //        }
        //    }

        //    CheckGap(ref stageList);
        //}
        #endregion
        private void Process_LoanStatus(Common.Record.Loans loans_record, ref Common.Table.Loans loans_table)
        {
            DateTime dt = DateTime.MinValue;
            loans_table.DateSuspended = DateTime.MinValue;
            if (DateTime.TryParse(loans_record.DateSuspended, out dt))
            {
                loans_table.DateSuspended = dt;
            }

            loans_table.DateClose = DateTime.MinValue;
            if (DateTime.TryParse(loans_record.DateClose, out dt))
            {
                loans_table.DateClose = dt;
                loans_table.LoanStatus = LoanStatus.LoanStatus_Closed;
            }

            loans_table.DateDenied = DateTime.MinValue;
            if (DateTime.TryParse(loans_record.DateDenied, out dt))
            {
                loans_table.DateDenied = dt;
                loans_table.LoanStatus = LoanStatus.LoanStatus_Denied;
            }

            loans_table.DateCanceled = DateTime.MinValue;
            if (DateTime.TryParse(loans_record.DateCanceled, out dt))
            {
                loans_table.DateCanceled = dt;
                loans_table.LoanStatus = LoanStatus.LoanStatus_Canceled;
            }
        }
        public void Process_LoanStatus_Stages(Common.Record.Loans loans_record, ref Common.Table.Loans loans_table, List<Table.LoanStages> stageList)
        {
            string err = "";
            #region check parameters
            if (loans_record == null)
            {
                err = "Process_LoanStatus_Stages: Loan_Record cannot be null.";
                int Event_id = 9432;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }
            int fileId = loans_record.FileId;
            if (fileId <= 0)
            {
                err = "Process_LoanStatus_Stages: Invalid FileId, " + fileId + " specified.";
                int Event_id = 9433;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }
            #endregion
            #region Est Close Date and CurrentStage

            DateTime dt = new DateTime();
            loans_table.EstCloseDate = DateTime.MinValue;
            if (DateTime.TryParse(loans_record.EstCloseDate, out dt))
            {
                loans_table.EstCloseDate = dt;
            }
            loans_table.DateOpen = DateTime.MinValue;

            foreach (Table.LoanStages ls in stageList)
            {
                if (ls.Completed == DateTime.MinValue)
                {
                    loans_table.CurrentStage = ls.StageName.Trim();
                    break;
                }
                else
                {
                    loans_table.LastCompletedStage = ls.StageName.Trim();
                }
            }
            #region obsolete code
            //List<Table.LoanStages> PointStageList = new List<Table.LoanStages>();
            //int SequenceNumber = 0;

            //foreach (Table.LoanStages ls in stageList)
            //{
            //    switch (ls.StageName)
            //    {
            //        case "Open":
            //        case PointStage.Application:
            //            if (DateTime.TryParse(loans_record.DateOpen, out dt))
            //                loans_table.DateOpen = dt;
            //            SequenceNumber++;
            //            PointStageList.Add(
            //                new Table.LoanStages
            //                {
            //                    StageName = PointStage.Application,
            //                    Completed = loans_table.DateOpen,
            //                    SequenceNumber = SequenceNumber
            //                });
            //            break;

            //        case "HMDA":
            //        case PointStage.HMDACompleted:
            //        case PointStage.HMDAComplete:
            //            loans_table.DateHMDA = DateTime.MinValue;
            //            if (DateTime.TryParse(loans_record.DateHMDA, out dt))
            //                loans_table.DateHMDA = dt;
            //            SequenceNumber++;
            //            PointStageList.Add(
            //                new Table.LoanStages
            //                {
            //                    StageName = PointStage.HMDAComplete,
            //                    Completed = loans_table.DateHMDA,
            //                    SequenceNumber = SequenceNumber
            //                });
            //            break;

            //        case "Processing":
            //        case "Sent To Processing":
            //        case PointStage.SentToProcessing:
            //            loans_table.DateProcessing = DateTime.MinValue;
            //            if (DateTime.TryParse(loans_record.DateProcessing, out dt))
            //                loans_table.DateProcessing = dt;
            //            SequenceNumber++;
            //            PointStageList.Add(
            //            new Table.LoanStages
            //            {
            //                StageName = PointStage.SentToProcessing,
            //                Completed = loans_table.DateProcessing,
            //                SequenceNumber = SequenceNumber
            //            });
            //            break;

            //        case PointStage.Submit:
            //        case PointStage.Submitted:
            //            loans_table.DateSubmit = DateTime.MinValue;
            //            if (DateTime.TryParse(loans_record.DateSubmit, out dt))
            //                loans_table.DateSubmit = dt;
            //            SequenceNumber++;
            //            PointStageList.Add(
            //                new Table.LoanStages
            //                {
            //                    StageName = PointStage.Submitted,
            //                    Completed = loans_table.DateSubmit,
            //                    SequenceNumber = SequenceNumber
            //                });
            //            break;
            //        case PointStage.Approve:
            //        case PointStage.Approved:
            //            loans_table.DateApprove = DateTime.MinValue;
            //            if (DateTime.TryParse(loans_record.DateApprove, out dt))
            //                loans_table.DateApprove = dt;
            //            SequenceNumber++;
            //            PointStageList.Add(
            //            new Table.LoanStages
            //            {
            //                StageName = PointStage.Approved,
            //                Completed = loans_table.DateApprove,
            //                SequenceNumber = SequenceNumber
            //            });
            //            break;
            //        case PointStage.CleartoClose:
            //        case PointStage.ClearToClose:
            //            loans_table.DateClearToClose = DateTime.MinValue;
            //            if (DateTime.TryParse(loans_record.DateClearToClose, out dt))
            //                loans_table.DateClearToClose = dt;
            //            SequenceNumber++;
            //            PointStageList.Add(
            //                new Table.LoanStages
            //                {
            //                    StageName = PointStage.CleartoClose,
            //                    Completed = loans_table.DateClearToClose,
            //                    SequenceNumber = SequenceNumber
            //                });
            //            break;
            //        case PointStage.Close:
            //        case PointStage.Closed:
            //            loans_table.DateClose = DateTime.MinValue;
            //            if (DateTime.TryParse(loans_record.DateClose, out dt))
            //                loans_table.DateClose = dt;
            //            SequenceNumber++;
            //            PointStageList.Add(
            //                new Table.LoanStages
            //                {
            //                    StageName = PointStage.Closed,
            //                    Completed = loans_table.DateClose,
            //                    SequenceNumber = SequenceNumber
            //                });
            //            break;
            //        case PointStage.DocsDrawn:
            //            loans_table.DateDocs = DateTime.MinValue;
            //            if (DateTime.TryParse(loans_record.DateDocs, out dt))
            //                loans_table.DateDocs = dt;
            //            SequenceNumber++;
            //            PointStageList.Add(
            //                new Table.LoanStages
            //                {
            //                    StageName = PointStage.DocsDrawn,
            //                    Completed = loans_table.DateDocs,
            //                    SequenceNumber = SequenceNumber
            //                });
            //            break;
            //        case PointStage.DocsOut:
            //        case "Docs":
            //            loans_table.DateDocsOut = DateTime.MinValue;
            //            if (DateTime.TryParse(loans_record.DateDocsOut, out dt))
            //                loans_table.DateDocsOut = dt;
            //            SequenceNumber++;
            //            PointStageList.Add(
            //                new Table.LoanStages
            //                {
            //                    StageName = PointStage.DocsOut,
            //                    Completed = loans_table.DateDocsOut,
            //                    SequenceNumber = SequenceNumber
            //                });
            //            break;
            //        case PointStage.DocsReceived:
            //            loans_table.DateDocsReceived = DateTime.MinValue;
            //            if (DateTime.TryParse(loans_record.DateDocsReceived, out dt))
            //                loans_table.DateDocsReceived = dt;
            //            SequenceNumber++;
            //            PointStageList.Add(
            //                new Table.LoanStages
            //                {
            //                    StageName = PointStage.DocsReceived,
            //                    Completed = loans_table.DateDocsReceived,
            //                    SequenceNumber = SequenceNumber
            //                });
            //            break;
            //        case PointStage.Fund:
            //        case PointStage.Funded:
            //            loans_table.DateFund = DateTime.MinValue;
            //            if (DateTime.TryParse(loans_record.DateFund, out dt))
            //                loans_table.DateFund = dt;
            //            SequenceNumber++;
            //            PointStageList.Add(
            //            new Table.LoanStages
            //            {
            //                StageName = PointStage.Funded,
            //                Completed = loans_table.DateFund,
            //                SequenceNumber = SequenceNumber
            //            });
            //            break;
            //        case PointStage.Record:
            //        case PointStage.Recorded:
            //            loans_table.DateRecord = DateTime.MinValue;
            //            if (DateTime.TryParse(loans_record.DateRecord, out dt))
            //                loans_table.DateRecord = dt;
            //            SequenceNumber++;
            //            PointStageList.Add(
            //                new Table.LoanStages
            //                {
            //                    StageName = PointStage.Recorded,
            //                    Completed = loans_table.DateRecord,
            //                    SequenceNumber = SequenceNumber
            //                });
            //            break;
            //        case PointStage.Suspend:
            //        case PointStage.Suspended:
            //            loans_table.DateSuspended = DateTime.MinValue;
            //            if (DateTime.TryParse(loans_record.DateSuspended, out dt))
            //                loans_table.DateSuspended = dt;
            //            SequenceNumber++;
            //            PointStageList.Add(
            //                new Table.LoanStages
            //                {
            //                    StageName = PointStage.Suspended,
            //                    Completed = loans_table.DateSuspended,
            //                    SequenceNumber = SequenceNumber
            //                });
            //            break;
            //        case PointStage.Resubmit:
            //        case PointStage.Re_submit:
            //        case PointStage.Resubmitted:
            //            loans_table.DateReSubmit = DateTime.MinValue;
            //            if (DateTime.TryParse(loans_record.DateReSubmit, out dt))
            //                loans_table.DateReSubmit = dt;
            //            SequenceNumber++;
            //            PointStageList.Add(
            //                new Table.LoanStages
            //                {
            //                    StageName = PointStage.Resubmit,
            //                    Completed = loans_table.DateReSubmit,
            //                    SequenceNumber = SequenceNumber
            //                });
            //            break;
            //    }

            //}

            //CheckGap(ref PointStageList);
            //foreach (Table.LoanStages ls in PointStageList)
            //{
            //    switch (ls.StageName)
            //    {
            //        case "Open":
            //        case PointStage.Application:
            //            loans_table.DateOpen = ls.Completed;
            //            break;
            //        case "HMDA":
            //        case PointStage.HMDACompleted:
            //        case PointStage.HMDAComplete:
            //            loans_table.DateHMDA = ls.Completed;
            //            break;
            //        case "Processing":
            //        case "Sent To Processing":
            //        case PointStage.SentToProcessing:
            //            loans_table.DateProcessing = ls.Completed;
            //            break;
            //        case PointStage.Submit:
            //        case PointStage.Submitted:
            //            loans_table.DateSubmit = ls.Completed;
            //            break;
            //        case PointStage.Approve:
            //        case PointStage.Approved:
            //            loans_table.DateApprove = ls.Completed;
            //            break;
            //        case PointStage.CleartoClose:
            //        case PointStage.ClearToClose:
            //            loans_table.DateClearToClose = ls.Completed;
            //            break;
            //        case PointStage.Close:
            //        case PointStage.Closed:
            //            loans_table.DateClose = ls.Completed;
            //            break;
            //        case PointStage.DocsDrawn:
            //            loans_table.DateDocs = ls.Completed;
            //            break;
            //        case PointStage.DocsOut:
            //        case "Docs":
            //            loans_table.DateDocsOut = ls.Completed;
            //            break;
            //        case PointStage.DocsReceived:
            //            loans_table.DateDocsReceived = ls.Completed;
            //            break;
            //        case PointStage.Fund:
            //        case PointStage.Funded:
            //            loans_table.DateFund = ls.Completed;
            //            break;
            //        case PointStage.Record:
            //        case PointStage.Recorded:
            //            loans_table.DateRecord = ls.Completed;
            //            break;
            //        case PointStage.Suspend:
            //        case PointStage.Suspended:
            //            loans_table.DateSuspended = ls.Completed;
            //            break;
            //        case PointStage.Resubmit:
            //        case PointStage.Re_submit:
            //        case PointStage.Resubmitted:
            //            loans_table.DateReSubmit = ls.Completed;
            //            break;
            //    }
            //}

            //if (da.GetNumberLoanTasks(fileId, ref err) == 0)
            //    UpdateStageDates(ref stageList, loans_table);
            #endregion
            #endregion
            // set default loan status = processing
            if (string.IsNullOrEmpty(loans_table.LoanStatus))
                loans_table.LoanStatus = "Processing";
            Process_LoanStatus(loans_record, ref loans_table);
            // set up rate lock
            loans_table.RateLockExpiration = DateTime.MinValue;
            if (DateTime.TryParse(loans_record.RateLockExpiration, out dt))
            {
                loans_table.RateLockExpiration = dt;
            }
        }
        bool ProcessLoan(Record.LoanRecord rec, string filename, byte[] pointbuffer, List<Table.LoanStages> stageList, ProspectFlagEnum Prospect_flag, ref string err)
        {
            bool logErr = false;
            if ((rec == null) || (rec.loans == null) || (rec.loans.FileId <= 0))
            {
                err = "Process_Loan: loan record is empty or loan file id is <= 0.";
                int Event_id = 9434;
                short Category = 99;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
                return false;
            }

            int fileId = rec.loans.FileId;
            bool PointErr = false;
            List<Table.Contacts> cList = null;
            try
            {
                PointErr = CheckLoanFields(ref rec, filename, Prospect_flag);
                bool ActiveLoan = false;
                if (Process_Loan_Table(rec.loans, stageList, Prospect_flag, rec.team.lo, out ActiveLoan) == false)
                {
                    err = "Point File Path: " + rec.loans.PointFilePath + " Error: Process_Loan_Table return false";
                    int Event_id = 9435;
                    short Category = 99;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
                    return false;
                }

                cList = da.Get_LoanContacts(fileId, "", ref err);
                ProcessBorrower_Coborrower(rec, cList, Prospect_flag, ActiveLoan, ref err);
                if (!string.IsNullOrEmpty(err))
                {
                    err = "Point File Path: " + rec.loans.PointFilePath + " ProcessBorrower_Coborrower error: " + err;
                    int Event_id = 9436;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }

                ProcessNotes(fileId, rec.notes, ref err);
                ProcessDocs(fileId, rec.docs, ref err);
                ProcessConditions(fileId, rec.conditions, ref err);
                Process_LoanTeam(fileId, rec.loans.BranchId, rec.team, ActiveLoan, ref err);
                ProcessLoanLocks_Profit(fileId, rec.lockInfos, rec.lockInfosPage, rec.loanProfit, ref err);

                if (ActiveLoan && Process_Agents(fileId, rec.agents, cList, ref err) == false)
                {
                    err = "Point File Path: " + rec.loans.PointFilePath + " Process_Agents error: " + err;
                    int Event_id = 9437;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }

                if (rec.agents.lender_contact1 != null)
                {
                    if ((rec.agents.lender_contact1.Contact != null) &&
                        (rec.agents.lender_contact1.Contact.ContactCompanyId != String.Empty))
                        rec.loans.Lender = rec.agents.lender_contact1.Contact.ContactCompanyId;
                }
                //  set up loan stages and the completion status
                if (stageList != null && stageList.Count > 0)
                {
                    da.Setup_LoanStages(fileId, stageList, ref err);
                }
                return true;
            }
            catch (Exception e)
            {
                string errMsg = "";
                err = "Point File Path: " + rec.loans.PointFilePath + " ProcessLoanRecord, Exception:" + e.Message;
                int Event_id = 9438;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                da.Save_PointImportHistory(fileId, "Error", e.Message, ref errMsg);
                return false;
            }
            finally
            {
                if (cList != null)
                {
                    cList.Clear();
                    cList = null;
                    if (rec.agents != null)
                    {
                        rec.agents.lender_contact1 = null;
                        rec.agents.lender_contact2 = null;
                        rec.agents.lender_contact3 = null;
                        rec.agents.appraiser = null;
                        rec.agents.builder = null;
                        rec.agents.buyers_agent = null;
                        rec.agents.closing_agent = null;
                        rec.agents.flood_insurance = null;
                        rec.agents.hazard_insurance = null;
                        rec.agents.investor = null;
                        rec.agents.listing_agent = null;
                        rec.agents.mortgage_broker = null;
                        rec.agents.mortgage_insurance = null;
                        rec.agents.selling_agent = null;
                        rec.agents.title_insurance = null;
                        rec.agents = null;
                    }
                    if (rec.loans != null)
                    {
                        rec.loans = null;
                    }
                    if (rec.team != null)
                    {
                        rec.team = null;
                    }
                }

                if (!PointErr)
                    da.Save_PointImportHistory(fileId, "", "", ref err);
            }
        }
     
        private void ProcessLoanLocks_Profit(int fileId, Record.LoanLocks lockInfos, Record.LoanLocksPage lockInfosPage, Record.LoanProfit loanProfit, ref string err)
        {
            err = string.Empty;
            bool logErr = false;
            bool status = false;
            short fieldId = 0;
            string fieldValue = string.Empty;
            string prevValue = string.Empty;

            try
            {
                if (fileId <= 0)
                {
                    err = string.Format("ProcessLoanLocks, invalid FileId {0} specified, ", fileId);
                    int Event_id = 9437;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }

                if (lockInfosPage != null)
                {
                    if ((lockInfosPage.MIOption != null) &&
                         (lockInfosPage.MIOption != String.Empty))
                    {
                        fieldId = 4018;
                        fieldValue = lockInfosPage.MIOption.Trim();
                        string sqlCmd5 = "Select CurrentValue FROM LoanPointFields Where FileId=" + fileId + " and PointFieldId=" + fieldId;
                        try
                        {
                            object ds5 = DbHelperSQL.GetSingle(sqlCmd5);
                            if ((ds5 != null) && (ds5 != DBNull.Value))
                            {
                                prevValue = (string)ds5;
                            }
                        }
                        catch (Exception e)
                        {
                        }

                        if (prevValue != fieldValue)
                        {
                            status = da.Save_LoanPointFields(fileId, fieldId, fieldValue, prevValue, ref err);
                        }
                    }

                    if ((lockInfosPage.PropertyType != null) &&
                         (lockInfosPage.PropertyType != String.Empty))
                    {
                        fieldId = 2729;
                        fieldValue = lockInfosPage.PropertyType.Trim();
                        string sqlCmd5 = "Select CurrentValue FROM LoanPointFields Where FileId=" + fileId + " and PointFieldId=" + fieldId;
                        try
                        {
                            object ds5 = DbHelperSQL.GetSingle(sqlCmd5);
                            if ((ds5 != null) && (ds5 != DBNull.Value))
                            {
                                prevValue = (string)ds5;
                            }
                        }
                        catch (Exception e)
                        {
                        }

                        if (prevValue != fieldValue)
                        {
                            status = da.Save_LoanPointFields(fileId, fieldId, fieldValue, prevValue, ref err);
                        }
                    }

                    if ((lockInfosPage.FICO != null) &&
                         (lockInfosPage.FICO != String.Empty))
                    {
                        fieldId = 2836;
                        fieldValue = lockInfosPage.FICO.Trim();
                        string sqlCmd5 = "Select CurrentValue FROM LoanPointFields Where FileId=" + fileId + " and PointFieldId=" + fieldId;
                        try
                        {
                            object ds5 = DbHelperSQL.GetSingle(sqlCmd5);
                            if ((ds5 != null) && (ds5 != DBNull.Value))
                            {
                                prevValue = (string)ds5;
                            }
                        }
                        catch (Exception e)
                        {
                        }

                        if (prevValue != fieldValue)
                        {
                            status = da.Save_LoanPointFields(fileId, fieldId, fieldValue, prevValue, ref err);
                        }
                    }

                    if ((lockInfosPage.Float != null) &&
                         (lockInfosPage.Float != String.Empty))
                    {
                        fieldId = 6100;
                        fieldValue = lockInfosPage.Float.Trim();
                        if (fieldValue.ToUpper() == "X")
                        {
                            fieldValue = "Yes";
                        }
                        string sqlCmd5 = "Select CurrentValue FROM LoanPointFields Where FileId=" + fileId + " and PointFieldId=" + fieldId;
                        try
                        {
                            object ds5 = DbHelperSQL.GetSingle(sqlCmd5);
                            if ((ds5 != null) && (ds5 != DBNull.Value))
                            {
                                prevValue = (string)ds5;
                            }
                        }
                        catch (Exception e)
                        {
                        }

                        if (prevValue != fieldValue)
                        {
                            status = da.Save_LoanPointFields(fileId, fieldId, fieldValue, prevValue, ref err);
                        }
                    }

                    if ((lockInfosPage.Lock != null) &&
                         (lockInfosPage.Lock != String.Empty))
                    {
                        fieldId = 6101;
                        fieldValue = lockInfosPage.Lock.Trim();
                        if (fieldValue.ToUpper() == "X")
                        {
                            fieldValue = "Yes";
                        }
                        string sqlCmd5 = "Select CurrentValue FROM LoanPointFields Where FileId=" + fileId + " and PointFieldId=" + fieldId;
                        try
                        {
                            object ds5 = DbHelperSQL.GetSingle(sqlCmd5);
                            if ((ds5 != null) && (ds5 != DBNull.Value))
                            {
                                prevValue = (string)ds5;
                            }
                        }
                        catch (Exception e)
                        {
                        }

                        if (prevValue != fieldValue)
                        {
                            status = da.Save_LoanPointFields(fileId, fieldId, fieldValue, prevValue, ref err);
                        }
                    }

                    if ((lockInfosPage.LockDate != null) &&
                         (lockInfosPage.LockDate != String.Empty))
                    {
                        fieldId = 6061;
                        fieldValue = lockInfosPage.LockDate.Trim();

                        string sqlCmd5 = "Select CurrentValue FROM LoanPointFields Where FileId=" + fileId + " and PointFieldId=" + fieldId;
                        try
                        {
                            object ds5 = DbHelperSQL.GetSingle(sqlCmd5);
                            if ((ds5 != null) && (ds5 != DBNull.Value))
                            {
                                prevValue = (string)ds5;
                            }
                        }
                        catch (Exception e)
                        {
                        }

                        if (prevValue != fieldValue)
                        {
                            status = da.Save_LoanPointFields(fileId, fieldId, fieldValue, prevValue, ref err);
                        }
                    }

                    if ((lockInfosPage.LockTerm != null) &&
                         (lockInfosPage.LockTerm != String.Empty))
                    {
                        fieldId = 6062;
                        fieldValue = lockInfosPage.LockTerm.Trim();

                        string sqlCmd5 = "Select CurrentValue FROM LoanPointFields Where FileId=" + fileId + " and PointFieldId=" + fieldId;
                        try
                        {
                            object ds5 = DbHelperSQL.GetSingle(sqlCmd5);
                            if ((ds5 != null) && (ds5 != DBNull.Value))
                            {
                                prevValue = (string)ds5;
                            }
                        }
                        catch (Exception e)
                        {
                        }

                        if (prevValue != fieldValue)
                        {
                            status = da.Save_LoanPointFields(fileId, fieldId, fieldValue, prevValue, ref err);
                        }
                    }

                    if ((lockInfosPage.LockExpDate != null) &&
                         (lockInfosPage.LockExpDate != String.Empty))
                    {
                        fieldId = 6063;
                        fieldValue = lockInfosPage.LockExpDate.Trim();

                        string sqlCmd5 = "Select CurrentValue FROM LoanPointFields Where FileId=" + fileId + " and PointFieldId=" + fieldId;
                        try
                        {
                            object ds5 = DbHelperSQL.GetSingle(sqlCmd5);
                            if ((ds5 != null) && (ds5 != DBNull.Value))
                            {
                                prevValue = (string)ds5;
                            }
                        }
                        catch (Exception e)
                        {
                        }

                        if (prevValue != fieldValue)
                        {
                            status = da.Save_LoanPointFields(fileId, fieldId, fieldValue, prevValue, ref err);
                        }
                    }

                    if ((lockInfosPage.Price != null) &&
                         (lockInfosPage.Price != String.Empty))
                    {
                        fieldId = 12492;
                        fieldValue = lockInfosPage.Price.Trim();
                        string sqlCmd5 = "Select CurrentValue FROM LoanPointFields Where FileId=" + fileId + " and PointFieldId=" + fieldId;
                        try
                        {
                            object ds5 = DbHelperSQL.GetSingle(sqlCmd5);
                            if ((ds5 != null) && (ds5 != DBNull.Value))
                            {
                                prevValue = (string)ds5;
                            }
                        }
                        catch (Exception e)
                        {
                        }

                        if (prevValue != fieldValue)
                        {
                            status = da.Save_LoanPointFields(fileId, fieldId, fieldValue, prevValue, ref err);
                        }
                    }

                    if ((lockInfosPage.Extension12976 != null) &&
                         (lockInfosPage.Extension12976 != String.Empty))
                    {
                        fieldId = 12976;
                        fieldValue = lockInfosPage.Extension12976.Trim();
                        string sqlCmd5 = "Select CurrentValue FROM LoanPointFields Where FileId=" + fileId + " and PointFieldId=" + fieldId;
                        try
                        {
                            object ds5 = DbHelperSQL.GetSingle(sqlCmd5);
                            if ((ds5 != null) && (ds5 != DBNull.Value))
                            {
                                prevValue = (string)ds5;
                            }
                        }
                        catch (Exception e)
                        {
                        }

                        if (prevValue != fieldValue)
                        {
                            status = da.Save_LoanPointFields(fileId, fieldId, fieldValue, prevValue, ref err);
                        }
                    }

                    if ((lockInfosPage.Extension12977 != null) &&
                         (lockInfosPage.Extension12977 != String.Empty))
                    {
                        fieldId = 12977;
                        fieldValue = lockInfosPage.Extension12977.Trim();
                        string sqlCmd5 = "Select CurrentValue FROM LoanPointFields Where FileId=" + fileId + " and PointFieldId=" + fieldId;
                        try
                        {
                            object ds5 = DbHelperSQL.GetSingle(sqlCmd5);
                            if ((ds5 != null) && (ds5 != DBNull.Value))
                            {
                                prevValue = (string)ds5;
                            }
                        }
                        catch (Exception e)
                        {
                        }

                        if (prevValue != fieldValue)
                        {
                            status = da.Save_LoanPointFields(fileId, fieldId, fieldValue, prevValue, ref err);
                        }
                    }
                }

                if (loanProfit != null)
                {
                    if ((loanProfit.LenderCredit != null) &&
                         (loanProfit.LenderCredit != String.Empty))
                    {
                        fieldId = 812;
                        fieldValue = loanProfit.LenderCredit.Trim();
                        string sqlCmd6 = "Select CurrentValue FROM LoanPointFields Where FileId=" + fileId + " and PointFieldId=" + fieldId;
                        try
                        {
                            object ds6 = DbHelperSQL.GetSingle(sqlCmd6);
                            if ((ds6 != null) && (ds6 != DBNull.Value))
                            {
                                prevValue = (string)ds6;
                            }
                        }
                        catch (Exception e)
                        {
                        }

                        if (prevValue != fieldValue)
                        {
                            status = da.Save_LoanPointFields(fileId, fieldId, fieldValue, prevValue, ref err);
                        }
                    }
                }

                da.SaveLoanLocks_Profit(fileId, lockInfos, loanProfit, ref err);
            }
            catch (Exception ex)
            {
                err = string.Format("ProcessLoanLocks FileId {0}, Exception: {1}. ", fileId, ex.Message);
                int Event_id = 9438;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 9439;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        private void AutoConvertLead(int fileId, int branchId, List<string> FieldArray, ArrayList FieldSeq, ref string err)
        {
            string sExperian_Borrower = PNT.getPointField(FieldArray, FieldSeq, 5032);
            string sTransUnion_Borrower = PNT.getPointField(FieldArray, FieldSeq, 5034);
            string sEquifax_Borrower = PNT.getPointField(FieldArray, FieldSeq, 5036);

            string sExperian_CoBorrower = PNT.getPointField(FieldArray, FieldSeq, 5033);
            string sTransUnion_CoBorrower = PNT.getPointField(FieldArray, FieldSeq, 5035);
            string sEquifax_CoBorrower = PNT.getPointField(FieldArray, FieldSeq, 5037);

            if (string.IsNullOrEmpty(sExperian_Borrower)
                    && string.IsNullOrEmpty(sTransUnion_Borrower)
                    && string.IsNullOrEmpty(sEquifax_Borrower)
                    && string.IsNullOrEmpty(sExperian_CoBorrower)
                    && string.IsNullOrEmpty(sTransUnion_CoBorrower)
                    && string.IsNullOrEmpty(sEquifax_CoBorrower))
                return;

            // Get the default “Processing” Point FolderId
            try
            {
                PointConfig pointConfig;
                pointConfig = da.GetPointConfigData(ref err);
                if ((pointConfig == null) ||
                    (pointConfig.Auto_ConvertLead == false))
                    return;

                DataTable ProcessingFolderInfo = da.GetDefaultProcessingFolderInfo(branchId);
                if (ProcessingFolderInfo.Rows.Count <= 0)
                    return;

                Int32 i32 = 0;
                String Sg = (string)ProcessingFolderInfo.Rows[0]["FolderId"];
                //int iProcessingFolderID = Convert.ToInt32(ProcessingFolderInfo.Rows[0]["FolderId"]);
                int iProcessingFolderID = 0;
                if (Int32.TryParse(Sg, out i32))
                {
                    iProcessingFolderID = i32;
                }
                else
                {
                    iProcessingFolderID = 0;
                }

                #region Call DisposeLoanRequest

                DisposeLoanRequest req = new DisposeLoanRequest();
                req.hdr = new ReqHdr();
                req.hdr.UserId = 0;
                req.FileId = fileId;
                req.NewFolderId = iProcessingFolderID;
                req.LoanStatus = "Processing";
                req.StatusDate = DateTime.Now;

                LP2Service.PointMgr pm = LP2Service.PointMgr.Instance;
                LP2Service.PointMgrEvent pe = new LP2Service.PointMgrEvent(0, PointMgrCommandType.DisposeLoan, req.FileId, req);
                if (pm.ProcessRequest(pe, ref err) == false)
                {
                    return;
                }

                #endregion

                if (string.IsNullOrEmpty(pointConfig.MasterSource) ||
                    pointConfig.MasterSource.Trim().ToLower() != "datatrac")
                    return;

                #region Call DT_SubmitLoanRequest

                DT_SubmitLoanRequest req2 = new DT_SubmitLoanRequest();
                req2.hdr = new ReqHdr();
                req2.hdr.UserId = 0;
                req2.FileId = fileId;

                string sLoan_Program = PNT.getPointField(FieldArray, FieldSeq, 7403);
                req2.Loan_Program = sLoan_Program;

                req2.Originator_Type = "Branch";

                LP2Service.PointMgrEvent pe2 = new LP2Service.PointMgrEvent(0, PointMgrCommandType.DisposeLoan, req.FileId, req);
                if (pm.ProcessRequest(pe2, ref err) == false)
                {
                    return;
                }

                #endregion

                #region Add LoanActivities

                da.Save_LoanActivities(fileId, 0, "The loan has been submitted to DataTrac during Auto Convert due to the presence of credit score(s).", ref err);

                #endregion
            }
            catch (Exception ex)
            {
                err = string.Format("AutoConvertLead, FileId {0}, BranchId {1} Exception:{2}", fileId, branchId, ex.Message);
                int Event_id = 9440;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return;
            }

        }
        public int ProcessPointData(string filepath, string folder, int folderId, int branchId, bool changed, ProspectFlagEnum Prospect_flag, int PDSFolderId, ref string err)
        {
            err = "";
            bool logErr = false;
            bool fatal = false;

            FileStream fs = null;
            bool status = false;

            byte[] pointbuffer = null;
            int fileId = 0;
            string errMsg = "";
            Record.LoanRecord rec = null;
            List<string> FieldArray = null;
            ArrayList FieldSeq = null;
            try
            {
                fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                if (FileHelper.ReadDataFromFile(fs, ref pointbuffer, ref err) <= 0)
                {
                    errMsg = "Process Point file" + filepath + ", err: " + err;
                    int Event_id = 9441;
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);             
                    return fileId;
                }
                fs.Close();
                fs.Dispose();
                fs = null;

                fileId = da.Save_PointFiles(filepath, folder, ref folderId, ref branchId, pointbuffer, changed, ref err);//todo:check
                if (fileId <= 0)
                {
                    logErr = true;
                    return fileId;
                }
                rec = new Record.LoanRecord();
                rec.loans = new Record.Loans();
                rec.loans.FileId = fileId;
                rec.borrower_contacts = new Record.Contacts();
                rec.coborrower_contacts = new Record.Contacts();
                rec.agents = new Record.LoanAgents();
                rec.team = new Record.LoanTeam();
                rec.conditions = new List<Record.Conditions>();
                rec.notes = new List<Table.LoanNotes>();
                FieldArray = new List<string>();
                FieldSeq = new ArrayList();
                if (PNT.ReadPointData(ref FieldArray, ref FieldSeq, pointbuffer, ref err) == false)
                {
                    fatal = true;
                    logErr = true;
                    return fileId;
                }
                AddBorrower(ref rec.borrower_contacts, FieldArray, FieldSeq);
                AddLoan(ref rec.loans, FieldArray, FieldSeq);
                AddLoanLock(ref rec.lockInfos, ref rec.lockInfosPage, FieldArray, FieldSeq, fileId);
                rec.loanProfit = new Record.LoanProfit();
                rec.loanProfit.FileId = fileId;
                AddLoanProfit(ref rec.loanProfit, FieldArray, FieldSeq);
                AddLoanARMCaps(ref rec.armsCapInfo, FieldArray, FieldSeq);
                AddCoBorrower(ref rec.coborrower_contacts, FieldArray, FieldSeq);
                AddAgents(ref rec.agents, FieldArray, FieldSeq);
                AddLoanTeam(ref rec.team, FieldArray, FieldSeq);
                AddNotes(ref rec.notes, FieldArray, FieldSeq, filepath);
                if (Prospect_flag == ProspectFlagEnum.RealtimePointFlag ||
                    Prospect_flag == ProspectFlagEnum.ScheduledPointFlag)
                    AddDocsConditions(ref rec, FieldArray, FieldSeq);
                rec.loans.FileId = fileId;
                rec.loans.BranchId = branchId;
                List<Table.LoanStages> loanStages = ImportLoanStages(rec.loans, fileId, FieldArray, FieldSeq);
                status = ProcessLoan(rec, filepath, pointbuffer, loanStages, Prospect_flag, ref err);

                if (PointFieldList == null)
                    PointFieldList = PointMgr.Instance.GetPointFieldDescList();
                if (da.IfLoanPointFieldsExist(fileId, ref err) == false)
                    changed = true;
                if (Prospect_flag == ProspectFlagEnum.ScheduledProspectFlag || Prospect_flag == ProspectFlagEnum.RealtimeProspectFlag)
                {
                    AutoConvertLead(fileId, branchId, FieldArray, FieldSeq, ref err);
                }
                else
                {
                    if (da.Check_V_ProcessingPipelineInfo(fileId, ref err) == false)
                    {
                        int Event_id = 9442;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                }

                if ((changed) && (PointFieldList != null))
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ImportPointFields), (object)fileId);

                return fileId;
            }
            catch (Exception e)
            {
                errMsg = "Exception: " + e.Message + " while processing Point file=" + filepath;
                int Event_id = 9443;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, errMsg, EventLogEntryType.Warning, Event_id, Category);
                return fileId;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (logErr)
                {
                    string severity = "Warning";
                    int Event_id = 9444;
                    err = string.Format("File Id= {0}", fileId) + err;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    da.Save_PointImportHistory(fileId, severity, errMsg, ref err);

                    if (pointbuffer != null)
                    {
                        pointbuffer = null;
                    }
                    if (rec != null)
                    {
                        rec.agents = null;
                        rec.borrower_contacts = null;
                        rec.coborrower_contacts = null;
                        rec.notes = null;
                        rec.team = null;
                        rec.loans = null;
                        rec.conditions = null;
                        rec = null;
                    }
                    if (FieldArray != null)
                    {
                        FieldArray.Clear();
                        FieldArray = null;
                    }
                    if (FieldSeq != null)
                    {
                        FieldSeq.Clear();
                        FieldSeq = null;
                    }
                }
            }
        }
            
        #region Import Point Fields
        private int FindLoanPointField(short fieldId, List<Table.LoanPointField> loanPointFields, string fieldValue, ref string prevValue, ref string err)
        {
            err = "";
            if ((fieldId <= 0) || (fieldId > 31000))
            {
                err = "FindPointFieldInLoanFieldList, invalid fieldId = " + fieldId;
                int Event_id = 9445;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return -1;
            }

            foreach (Table.LoanPointField lpf in loanPointFields)
            {
                if (lpf.FieldId > fieldId)
                    return -1;
                if (lpf.FieldId == fieldId)
                {
                    return loanPointFields.IndexOf(lpf);
                }
            }
            return -1;
        }
        private bool ProcessChangedFields(int fileId, List<Table.PointFieldDesc> pointFieldList, byte[] currentImage, byte[] previousImage, ref string err)
        {
            List<string> CurrFieldArray = null;
            ArrayList CurrFieldSeq = null;
            List<string> PrevFieldArray = null;
            ArrayList PrevFieldSeq = null;
            PNTLib PNT = new PNTLib();
            DataAccess.DataAccess da = new DataAccess.DataAccess();
            List<Table.LoanPointField> loanPointFields = null;
            bool logErr = false;
            //int e_id = 9028;      
            //EventLog.WriteEntry(InfoHubEventLog.LogSource, "ProcessChangedFields::Importing Point Fields for FileId " + fileId, EventLogEntryType.Information, e_id, Category);
            string logFieldIds = "";
            int numFields = 0;
            DateTime tempDate = DateTime.MinValue;
            decimal tempNumber = 0;
            string temp = string.Empty;
            try
            {
                if (currentImage == null)
                {
                    err = string.Format("ProcessChangedFields:: FileId {0} current Point file image is null.", fileId);
                    int Event_id = 9446;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                if (PNT.ReadPointData(ref CurrFieldArray, ref CurrFieldSeq, currentImage, ref err) == false)
                {
                    err = "ImportPointFields, err:" + err;
                    int Event_id = 9447;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                PrevFieldArray = new List<string>();
                PrevFieldSeq = new ArrayList();
                if ((previousImage != null) && (previousImage.Length > 4))
                {
                    if (PNT.ReadPointData(ref PrevFieldArray, ref PrevFieldSeq, previousImage, ref err) == false)
                    {
                        err = "Import Point Fields, unable to process previous Point File Image for Point FileId=" + fileId;
                        int Event_id = 9448;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    }
                }
                #region obsolete code
                //if ((previousImage != null) && (previousImage.Length > 4) &&
                //    (currentImage != null) && (currentImage.Length > 4))
                //{
                //    logFieldIds = String.Format("ProcessChangedFields:: trying to process {0} fields for FileId {1}, CurrentImage length={2}, PreviousImage length={3}",
                //                                 pointFieldList.Count, fileId, currentImage.Length, previousImage.Length);
                //    EventLog.WriteEntry(InfoHubEventLog.LogSource, logFieldIds, EventLogEntryType.Information);
                //}
                //logFieldIds = String.Format("ProcessChangedFields:: processed FileId={0}, updated Point Field Ids: ", fileId);
                #endregion
                foreach (Table.PointFieldDesc pf in pointFieldList)
                {
                    short fieldId = (short)pf.FieldId;
                    string fieldValue = PNT.getPointField(CurrFieldArray, CurrFieldSeq, fieldId);
                    string prevValue = "";
                    if (PrevFieldArray.Count > 0)
                    {
                        prevValue = PNT.getPointField(PrevFieldArray, PrevFieldSeq, fieldId);
                    }
                    switch (pf.DataType)
                    {
                        case PointFieldDataType.BooleanType:
                            if (!string.IsNullOrEmpty(fieldValue))
                                fieldValue = fieldValue.ToUpper() == "Y" || fieldValue.ToUpper() == "X" ? "Yes" : "No";
                            if (!string.IsNullOrEmpty(prevValue))
                                prevValue = prevValue.ToUpper() == "Y" || prevValue.ToUpper() == "X" ? "Yes" : "No";
                            break;
                        case PointFieldDataType.DateType:
                            if (!string.IsNullOrEmpty(fieldValue))
                            {
                                if (!DateTime.TryParse(fieldValue, out tempDate))
                                {
                                    err = string.Format("Error processing a date field {0} in Point (FieldID={1}), FileId={2}.", fieldValue, fieldId, fileId);
                                    int Event_id = 9449;
                                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
                                    fieldValue = string.Empty;
                                    continue;
                                }
                            }
                            break;
                        case PointFieldDataType.NumericType:
                            if (!string.IsNullOrEmpty(fieldValue))
                            {
                                temp = fieldValue.Trim();
                                if (temp.StartsWith("(") && temp.EndsWith(")"))
                                {
                                    temp = temp.Replace('(', ' ');
                                    temp = temp.Replace(')', ' ');
                                    temp = temp.Trim();
                                    temp = "-" + temp;
                                }
                                if (temp.Contains("%"))
                                    temp = fieldValue.Replace('%', ' ');
                                if (!temp.Contains("."))
                                    temp += ".00";
                                if (temp.StartsWith("."))
                                    temp = "0." + temp;
                                if (temp.ToUpper() == @"N/A")
                                    fieldValue = string.Empty;
                                else
                                    if (!decimal.TryParse(temp, out tempNumber))
                                    {
                                        err = string.Format("Error processing a numeric field, Value: {0}    in Point (FieldID={1}),    FileId={2}.", fieldValue, fieldId, fileId);
                                        int Event_id = 9450;
                                        //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                                        fieldValue = string.Empty;
                                        continue;
                                    }
                            }
                            break;
                        case PointFieldDataType.StringType:
                            break;
                        default:
                            if (string.IsNullOrEmpty(fieldValue))
                                continue;
                            err = string.Format("Unkown data type {0} in processing a date field {1} in Point (FieldID={2}), FileId={3}.", pf.DataType, fieldValue, fieldId, fileId);
                            int event_id = 9451;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, event_id, Category);
                            fieldValue = string.Empty;
                            continue;
                    }


                    if (fieldValue != null)
                        fieldValue = fieldValue.Trim();
                    if (prevValue != null)
                        prevValue = prevValue.Trim();
                    if (fieldId == 21017)                // Loan Amount with MI & F
                    {
                        if (string.IsNullOrEmpty(fieldValue))
                        {
                            string tempAmount = PNT.getPointField(CurrFieldArray, CurrFieldSeq, 819); // Total Loan Amount
                            if (decimal.TryParse(tempAmount, out tempNumber))
                                fieldValue = tempAmount;
                        }
                    }
                    if ((fieldValue == null) || (fieldValue == String.Empty))
                        if ((prevValue == null) || (prevValue == String.Empty))
                            continue;

                    if ((fieldValue != null) && (prevValue != null) && (fieldValue.ToLower() == prevValue.ToLower()))
                    {
                        string sqlCmd5 = "Select Count(PointFieldId) FROM LoanPointFields Where FileId=" + fileId + " and PointFieldId=" + fieldId;
                        try
                        {
                            object ds5 = DbHelperSQL.GetSingle(sqlCmd5);
                            int count = (ds5 == null || ds5 == DBNull.Value) ? 0 : (int)ds5;
                            if (count > 0)
                            {
                                continue;
                            }
                        }
                        catch (Exception e)
                        {
                        }

                    }

                    if (da.Save_LoanPointFields(fileId, fieldId, fieldValue, prevValue, ref err) == false)
                    {
                        int event_id = 9452;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, event_id, Category);
                        return false;
                    }
                    if (fieldId != 15)
                        logFieldIds = logFieldIds + string.Format("{0}:{1}->{2} ", fieldId, prevValue, fieldValue);

                    numFields++;
                }
                return true;
            }
            catch (Exception ex)
            {
                err = String.Format("ProcessChangedFields FileId={0}, Exception:{1}", fileId, ex.Message, ex.StackTrace);
                int event_id = 9453;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int event_id = 9454;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, event_id, Category);
                }
                //Trace.TraceInformation(logFieldIds);
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, logFieldIds, EventLogEntryType.Information);
                logFieldIds = String.Format("ProcessChangedFields:: updated {0} Point fields for FileId {1}.", numFields, fileId);
                Trace.TraceInformation(logFieldIds);
                //int Event_id = 9028;                
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, logFieldIds, EventLogEntryType.Information, Event_id, Category);
                if (loanPointFields != null)
                {
                    loanPointFields.Clear();
                    loanPointFields = null;
                }
                if (CurrFieldArray != null)
                {
                    CurrFieldArray.Clear();
                    CurrFieldArray = null;
                }
                if (CurrFieldSeq != null)
                {
                    CurrFieldSeq.Clear();
                    CurrFieldSeq = null;
                }
                if (PrevFieldArray != null)
                {
                    PrevFieldArray.Clear();
                    PrevFieldArray = null;
                }
                if (PrevFieldSeq != null)
                {
                    PrevFieldSeq.Clear();
                    PrevFieldSeq = null;
                }
            }
        }
        private void ImportPointFields(object o)
        {
            string err = "";
            bool logErr = false;
            int ofileId = (int)o;
            DataAccess.DataAccess da = new DataAccess.DataAccess();

            if (ofileId <= 0)
            {
                err = "ImportPointFields, invalid FieldId=" + ofileId;
                int event_id = 9455;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, event_id, Category);
                return;
            }

            if (PointFieldList == null || PointFieldList.Count <= 0)
            {
                lock (PointFieldList)
                {
                    PointFieldList = da.GetPointFieldDesc(ref err);
                }
                if ((PointFieldList == null) || (PointFieldList.Count <= 0))
                {
                        err = "Point Manager Database Error: Point Field Desc List is null/empty";
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, 9099, Category);
                }
                return;
            }

            int fileId = ofileId;

            byte[] currentImage = null;
            byte[] previousImage = null;

            try
            {
                if (da.GetPointFileImages(ofileId, ref currentImage, ref previousImage, ref err) == false)
                {
                    err = "File ID: " + ofileId + " da.GetPointFileImages error: " + err;
                    int Event_id = 9456;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }
                if ((currentImage == null) || (currentImage.Length <= 4))
                {
                    err = "Import Point Fields, no current image available for comparison.";
                    int Event_id = 9457;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return;
                }

                if (ProcessChangedFields(ofileId, PointFieldList, currentImage, previousImage, ref err) == false)
                {
                    int Event_id = 9458;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
                return;
            }
            catch (Exception ex)
            {
                err = "ImportPointFields, Exception: " + ex.Message;
                int Event_id = 9458;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                if (currentImage != null)
                    currentImage = null;
                if (previousImage != null)
                    previousImage = null;
                if (logErr)
                {
                    int Event_id = 9459;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }
        #endregion

        public void UpdateConditions(int fileId, int usrId, List<Conditions> ConditionList, ref string err)
        {
            var filePath = string.Empty;
            var usrName = string.Empty;
            err = string.Empty;
            byte[] pointbuffer = null;
            var logErr = false;
            List<string> FieldArray = null;
            ArrayList FieldSeq = null;
            var pd = new PointData(PNT, da);
            try
            {
                var usr = da.Get_UserInfo(usrId, ref err);
                if (usr == null)
                {
                    err = string.Format("UpdateConditions, Can't get user information for UserId:{0}", usrId);
                    logErr = true;
                    return;
                }
                usrName = string.Format("{0},{1}", usr.Lastname, usr.Firstname);
                filePath = da.GetPointFilePath(fileId, ref err);
                if (string.IsNullOrEmpty(filePath))
                {
                    err = string.Format("UpdateConditions, FileId={0}, filepath for the loan is empty, cannot update conditions in the Point file.", fileId);
                    logErr = true;
                    return;
                }

                var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                if (FileHelper.ReadDataFromFile(fs, ref pointbuffer, ref err) <= 0)
                {
                    err = "UpdateConditions Point file" + filePath + ", err: " + err;
                    logErr = true;
                    return;
                }
                fs.Close();
                fs.Dispose();
                fs = null;

                FieldArray = new List<string>();
                FieldSeq = new ArrayList();
                if (PNT.ReadPointData(ref FieldArray, ref FieldSeq, pointbuffer, ref err) == false)
                {
                    logErr = true;
                    return;
                }

                var conditions = new List<Common.Record.Conditions>();
                AddBankerConditions(conditions, FieldArray, FieldSeq);

                if (conditions.Any())
                {
                    #region update conditions

                    foreach (var updateCond in ConditionList)
                    {
                        foreach (var condition in conditions)
                        {
                            if (updateCond.Name.Equals(condition.CondName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                switch (updateCond.Status.ToUpper())
                                {
                                    case "CREATED":
                                        condition.Created = DateTime.Now;
                                        condition.CreatedBy = usrName;
                                        break;
                                    case "CLEARED":
                                        condition.Cleared = DateTime.Now;
                                        condition.ClearedBy = usrName;
                                        break;
                                    case "COLLECTED":
                                        condition.Collected = DateTime.Now;
                                        condition.CollectedBy = usrName;
                                        break;
                                    case "SUBMITTED":
                                        condition.Submitted = DateTime.Now;
                                        condition.SubmittedBy = usrName;
                                        break;
                                    case "RECEIVED":
                                        condition.Received = DateTime.Now;
                                        condition.ReceivedBy = usrName;
                                        break;
                                }
                                break; //todo: check 
                            }
                        }
                    }

                    #endregion

                    List<FieldMap> UpdatedFieldds = new List<FieldMap>();
                    UpdatedFieldds.Clear();
                    PointMgr pm = PointMgr.Instance;
                    //pm.AddUpdatedFields(ref UpdatedFieldds, 15252, string.Join("|", conditions.Select(item => item.CondName).ToArray()));//condition name
                    //pm.AddUpdatedFields(ref UpdatedFieldds, 15253, string.Join("|", conditions.Select(item => item.CondType).ToArray()));//condition type

                    pm.AddUpdatedFields(ref UpdatedFieldds, 15254, string.Join("|", conditions.Select(item => item.Received.ToShortDateString()).ToArray()));//condition received date
                    //pm.AddUpdatedFields(ref UpdatedFieldds, 15255, string.Join("|", conditions.Select(item => item.Cleared.ToShortDateString()).ToArray()));//condition cleared date

                    //pm.AddUpdatedFields(ref UpdatedFieldds, 15256, string.Join("|", conditions.Select(item => item.ClearedBy).ToArray()));//condition cleared by
                    //pm.AddUpdatedFields(ref UpdatedFieldds, 15257, string.Join("|", conditions.Select(item => item.CreatedBy).ToArray()));//condition created by

                    //pm.AddUpdatedFields(ref UpdatedFieldds, 15258, string.Join("|", conditions.Select(item => item.Created.ToShortDateString()).ToArray()));//condition created date
                    pm.AddUpdatedFields(ref UpdatedFieldds, 15259, string.Join("|", conditions.Select(item => item.ReceivedBy).ToArray()));//condition received by

                    //pm.AddUpdatedFields(ref UpdatedFieldds, 15260, string.Join("|", conditions.Select(item => item.Collected.ToShortDateString()).ToArray()));//condition collected date
                    //pm.AddUpdatedFields(ref UpdatedFieldds, 15261, string.Join("|", conditions.Select(item => item.CollectedBy).ToArray()));//condition collected by

                    //pm.AddUpdatedFields(ref UpdatedFieldds, 15262, string.Join("|", conditions.Select(item => item.Submitted.ToShortDateString()).ToArray()));//condition submitted date
                    //pm.AddUpdatedFields(ref UpdatedFieldds, 15263, string.Join("|", conditions.Select(item => item.SubmittedBy).ToArray()));//condition submitted by

                    if (PNT.WritePointData(UpdatedFieldds, filePath, ref  err) == false)
                    {
                        logErr = true;
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (logErr)
                {
                    string severity = "Warning";
                    int Event_id = 10001;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    if (pointbuffer != null)
                    {
                        pointbuffer = null;
                    }

                    if (FieldArray != null)
                    {
                        FieldArray.Clear();
                        FieldArray = null;
                    }
                    if (FieldSeq != null)
                    {
                        FieldSeq.Clear();
                        FieldSeq = null;
                    }
                }
            }
        }
    }
        #endregion
}

