using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LPWeb.DAL
{
    /// <summary>
    /// 数据访问类Loans。
    /// </summary>
    public class LoansBase
    {
        public LoansBase()
        { }
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LPWeb.Model.Loans model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Loans(");
            strSql.Append("FileId,AppraisedValue,CCScenario,CLTV,County,DateOpen,DateSubmit,DateApprove,DateClearToClose,DateDocs,DateFund,DateRecord,DateClose,DateDenied,DateCanceled,DownPay,EstCloseDate,Lender,LienPosition,LoanAmount,LoanNumber,LoanType,LTV,MonthlyPayment,LenderNotes,Occupancy,Program,PropertyAddr,PropertyCity,PropertyState,PropertyZip,Purpose,Rate,RateLockExpiration,SalesPrice,Term,Due,DateSuspended,RegionID,DivisionID,BranchID,GroupID,UserID,Status,LastCompletedStage,CurrentStage,ProspectLoanStatus,Disposed,Ranking,Created,CreatedBy,Modifed,ModifiedBy,GlobalId,DateHMDA,DateProcessing,DateReSubmit,DateDocsOut,DateDocsReceived,LOS_LoanOfficer,DateNote,LeadStar_username,LeadStar_userid,Joint,CoBrwType,PropertyType,HousingStatus,RentAmount,InterestOnly,IncludeEscrows,DT_FileID,TD_2,TD_2Amount,Subordinate,MonthlyPMI,MonthlyPMITax,PurchasedDate,FirstTimeBuyer,MIOption,LoanChanged)");
            strSql.Append(" values (");
            strSql.Append("@FileId,@AppraisedValue,@CCScenario,@CLTV,@County,@DateOpen,@DateSubmit,@DateApprove,@DateClearToClose,@DateDocs,@DateFund,@DateRecord,@DateClose,@DateDenied,@DateCanceled,@DownPay,@EstCloseDate,@Lender,@LienPosition,@LoanAmount,@LoanNumber,@LoanType,@LTV,@MonthlyPayment,@LenderNotes,@Occupancy,@Program,@PropertyAddr,@PropertyCity,@PropertyState,@PropertyZip,@Purpose,@Rate,@RateLockExpiration,@SalesPrice,@Term,@Due,@DateSuspended,@RegionID,@DivisionID,@BranchID,@GroupID,@UserID,@Status,@LastCompletedStage,@CurrentStage,@ProspectLoanStatus,@Disposed,@Ranking,@Created,@CreatedBy,@Modifed,@ModifiedBy,@GlobalId,@DateHMDA,@DateProcessing,@DateReSubmit,@DateDocsOut,@DateDocsReceived,@LOS_LoanOfficer,@DateNote,@LeadStar_username,@LeadStar_userid,@Joint,@CoBrwType,@PropertyType,@HousingStatus,@RentAmount,@InterestOnly,@IncludeEscrows,@DT_FileID,@TD_2,@TD_2Amount,@Subordinate,@MonthlyPMI,@MonthlyPMITax,@PurchasedDate, @FirstTimeHomeBuyer,@MIOption,@LoanChanged);");
            strSql.Append("select SCOPE_IDENTITY();");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@AppraisedValue", SqlDbType.Money,8),
					new SqlParameter("@CCScenario", SqlDbType.NVarChar,50),
					new SqlParameter("@CLTV", SqlDbType.SmallMoney,4),
					new SqlParameter("@County", SqlDbType.NVarChar,50),
					new SqlParameter("@DateOpen", SqlDbType.DateTime),
					new SqlParameter("@DateSubmit", SqlDbType.DateTime),
					new SqlParameter("@DateApprove", SqlDbType.DateTime),
					new SqlParameter("@DateClearToClose", SqlDbType.DateTime),
					new SqlParameter("@DateDocs", SqlDbType.DateTime),
					new SqlParameter("@DateFund", SqlDbType.DateTime),
					new SqlParameter("@DateRecord", SqlDbType.DateTime),
					new SqlParameter("@DateClose", SqlDbType.DateTime),
					new SqlParameter("@DateDenied", SqlDbType.DateTime),
					new SqlParameter("@DateCanceled", SqlDbType.DateTime),
					new SqlParameter("@DownPay", SqlDbType.Money,8),
					new SqlParameter("@EstCloseDate", SqlDbType.DateTime),
					new SqlParameter("@Lender", SqlDbType.Int,4),
					new SqlParameter("@LienPosition", SqlDbType.NVarChar,50),
					new SqlParameter("@LoanAmount", SqlDbType.Money,8),
					new SqlParameter("@LoanNumber", SqlDbType.NVarChar,50),
					new SqlParameter("@LoanType", SqlDbType.NVarChar,50),
					new SqlParameter("@LTV", SqlDbType.SmallMoney,4),
					new SqlParameter("@MonthlyPayment", SqlDbType.Money,8),
					new SqlParameter("@LenderNotes", SqlDbType.NVarChar,255),
					new SqlParameter("@Occupancy", SqlDbType.NVarChar,50),
					new SqlParameter("@Program", SqlDbType.NVarChar,255),
					new SqlParameter("@PropertyAddr", SqlDbType.NVarChar,50),
					new SqlParameter("@PropertyCity", SqlDbType.NVarChar,50),
					new SqlParameter("@PropertyState", SqlDbType.Char,2),
					new SqlParameter("@PropertyZip", SqlDbType.NVarChar,10),
					new SqlParameter("@Purpose", SqlDbType.NVarChar,50),
					new SqlParameter("@Rate", SqlDbType.SmallMoney,4),
					new SqlParameter("@RateLockExpiration", SqlDbType.DateTime),
					new SqlParameter("@SalesPrice", SqlDbType.Money,8),
					new SqlParameter("@Term", SqlDbType.SmallInt,2),
					new SqlParameter("@Due", SqlDbType.SmallInt,2),
					new SqlParameter("@DateSuspended", SqlDbType.DateTime),
					new SqlParameter("@RegionID", SqlDbType.Int,4),
					new SqlParameter("@DivisionID", SqlDbType.Int,4),
					new SqlParameter("@BranchID", SqlDbType.Int,4),
					new SqlParameter("@GroupID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.NVarChar,50),
					new SqlParameter("@LastCompletedStage", SqlDbType.NVarChar,50),
					new SqlParameter("@CurrentStage", SqlDbType.NVarChar,50),
					new SqlParameter("@ProspectLoanStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@Disposed", SqlDbType.DateTime),
					new SqlParameter("@Ranking", SqlDbType.NVarChar,20),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@CreatedBy", SqlDbType.Int,4),
					new SqlParameter("@Modifed", SqlDbType.DateTime),
					new SqlParameter("@ModifiedBy", SqlDbType.Int,4),
					new SqlParameter("@GlobalId", SqlDbType.NVarChar,255),
					new SqlParameter("@DateHMDA", SqlDbType.DateTime),
					new SqlParameter("@DateProcessing", SqlDbType.DateTime),
					new SqlParameter("@DateReSubmit", SqlDbType.DateTime),
					new SqlParameter("@DateDocsOut", SqlDbType.DateTime),
					new SqlParameter("@DateDocsReceived", SqlDbType.DateTime),
					new SqlParameter("@LOS_LoanOfficer", SqlDbType.NVarChar,255),
					new SqlParameter("@DateNote", SqlDbType.DateTime),
					new SqlParameter("@LeadStar_username", SqlDbType.NVarChar,255),
					new SqlParameter("@LeadStar_userid", SqlDbType.NVarChar,255),
					new SqlParameter("@Joint", SqlDbType.Bit,1),
					new SqlParameter("@CoBrwType", SqlDbType.NVarChar,255),
					new SqlParameter("@PropertyType", SqlDbType.NVarChar,255),
					new SqlParameter("@HousingStatus", SqlDbType.NVarChar,255),
					new SqlParameter("@RentAmount", SqlDbType.Decimal,5),
					new SqlParameter("@InterestOnly", SqlDbType.Bit,1),
					new SqlParameter("@IncludeEscrows", SqlDbType.Bit,1),
					new SqlParameter("@DT_FileID", SqlDbType.NVarChar,50),
					new SqlParameter("@TD_2", SqlDbType.Bit,1),
					new SqlParameter("@TD_2Amount", SqlDbType.Decimal,9),
					new SqlParameter("@Subordinate", SqlDbType.Bit,1),
					new SqlParameter("@MonthlyPMI", SqlDbType.Decimal,9),
					new SqlParameter("@MonthlyPMITax", SqlDbType.Decimal,9),
					new SqlParameter("@PurchasedDate", SqlDbType.DateTime),
                    new SqlParameter("@FirstTimeHomeBuyer", SqlDbType.Bit),
                    new SqlParameter("@MIOption", SqlDbType.NVarChar, 50), 
                    new SqlParameter("@LoanChanged", SqlDbType.Bit)      
                 };
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.AppraisedValue;
            parameters[2].Value = model.CCScenario;
            parameters[3].Value = model.CLTV;
            parameters[4].Value = model.County;
            parameters[5].Value = model.DateOpen;
            parameters[6].Value = model.DateSubmit;
            parameters[7].Value = model.DateApprove;
            parameters[8].Value = model.DateClearToClose;
            parameters[9].Value = model.DateDocs;
            parameters[10].Value = model.DateFund;
            parameters[11].Value = model.DateRecord;
            parameters[12].Value = model.DateClose;
            parameters[13].Value = model.DateDenied;
            parameters[14].Value = model.DateCanceled;
            parameters[15].Value = model.DownPay;
            parameters[16].Value = model.EstCloseDate;
            parameters[17].Value = model.Lender;
            parameters[18].Value = model.LienPosition;
            parameters[19].Value = model.LoanAmount;
            parameters[20].Value = model.LoanNumber;
            parameters[21].Value = model.LoanType;
            parameters[22].Value = model.LTV;
            parameters[23].Value = model.MonthlyPayment;
            parameters[24].Value = model.LenderNotes;
            parameters[25].Value = model.Occupancy;
            parameters[26].Value = model.Program;
            parameters[27].Value = model.PropertyAddr;
            parameters[28].Value = model.PropertyCity;
            parameters[29].Value = model.PropertyState;
            parameters[30].Value = model.PropertyZip;
            parameters[31].Value = model.Purpose;
            parameters[32].Value = model.Rate;
            parameters[33].Value = model.RateLockExpiration;
            parameters[34].Value = model.SalesPrice;
            parameters[35].Value = model.Term;
            parameters[36].Value = model.Due;
            parameters[37].Value = model.DateSuspended;
            parameters[38].Value = model.RegionID;
            parameters[39].Value = model.DivisionID;
            parameters[40].Value = model.BranchID;
            parameters[41].Value = model.GroupID;
            parameters[42].Value = model.UserID;
            parameters[43].Value = model.Status;
            parameters[44].Value = model.LastCompletedStage;
            parameters[45].Value = model.CurrentStage;
            parameters[46].Value = model.ProspectLoanStatus;
            parameters[47].Value = model.Disposed;
            parameters[48].Value = model.Ranking;
            parameters[49].Value = model.Created;
            parameters[50].Value = model.CreatedBy;
            parameters[51].Value = model.Modifed;
            parameters[52].Value = model.ModifiedBy;
            parameters[53].Value = model.GlobalId;
            parameters[54].Value = model.DateHMDA;
            parameters[55].Value = model.DateProcessing;
            parameters[56].Value = model.DateReSubmit;
            parameters[57].Value = model.DateDocsOut;
            parameters[58].Value = model.DateDocsReceived;
            parameters[59].Value = model.LOS_LoanOfficer;
            parameters[60].Value = model.DateNote;
            parameters[61].Value = model.LeadStar_username;
            parameters[62].Value = model.LeadStar_userid;
            parameters[63].Value = model.Joint;
            parameters[64].Value = model.CoBrwType;
            parameters[65].Value = model.PropertyType;
            parameters[66].Value = model.HousingStatus;
            parameters[67].Value = model.RentAmount;
            parameters[68].Value = model.InterestOnly;
            parameters[69].Value = model.IncludeEscrows;
            parameters[70].Value = model.DT_FileID;
            parameters[71].Value = model.TD_2;
            parameters[72].Value = model.TD_2Amount;
            parameters[73].Value = model.Subordinate;
            parameters[74].Value = model.MonthlyPMI;
            parameters[75].Value = model.MonthlyPMITax;
            parameters[76].Value = model.PurchasedDate;
            parameters[77].Value = model.FirstTimeHomeBuyer;
            parameters[78].Value = model.MIOption;
            parameters[79].Value = model.LoanChanged;
            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LPWeb.Model.Loans model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Loans set ");
            strSql.Append("AppraisedValue=@AppraisedValue,");
            strSql.Append("CCScenario=@CCScenario,");
            strSql.Append("CLTV=@CLTV,");
            strSql.Append("County=@County,");
            strSql.Append("DateOpen=@DateOpen,");
            strSql.Append("DateSubmit=@DateSubmit,");
            strSql.Append("DateApprove=@DateApprove,");
            strSql.Append("DateClearToClose=@DateClearToClose,");
            strSql.Append("DateDocs=@DateDocs,");
            strSql.Append("DateFund=@DateFund,");
            strSql.Append("DateRecord=@DateRecord,");
            strSql.Append("DateClose=@DateClose,");
            strSql.Append("DateDenied=@DateDenied,");
            strSql.Append("DateCanceled=@DateCanceled,");
            strSql.Append("DownPay=@DownPay,");
            strSql.Append("EstCloseDate=@EstCloseDate,");
            strSql.Append("Lender=@Lender,");
            strSql.Append("LienPosition=@LienPosition,");
            strSql.Append("LoanAmount=@LoanAmount,");
            strSql.Append("LoanNumber=@LoanNumber,");
            strSql.Append("LoanType=@LoanType,");
            strSql.Append("LTV=@LTV,");
            strSql.Append("MonthlyPayment=@MonthlyPayment,");
            strSql.Append("LenderNotes=@LenderNotes,");
            strSql.Append("Occupancy=@Occupancy,");
            strSql.Append("Program=@Program,");
            strSql.Append("PropertyAddr=@PropertyAddr,");
            strSql.Append("PropertyCity=@PropertyCity,");
            strSql.Append("PropertyState=@PropertyState,");
            strSql.Append("PropertyZip=@PropertyZip,");
            strSql.Append("Purpose=@Purpose,");
            strSql.Append("Rate=@Rate,");
            strSql.Append("RateLockExpiration=@RateLockExpiration,");
            strSql.Append("SalesPrice=@SalesPrice,");
            strSql.Append("Term=@Term,");
            strSql.Append("Due=@Due,");
            strSql.Append("DateSuspended=@DateSuspended,");
            strSql.Append("RegionID=@RegionID,");
            strSql.Append("DivisionID=@DivisionID,");
            strSql.Append("BranchID=@BranchID,");
            strSql.Append("GroupID=@GroupID,");
            strSql.Append("UserID=@UserID,");
            strSql.Append("Status=@Status,");
            strSql.Append("LastCompletedStage=@LastCompletedStage,");
            strSql.Append("CurrentStage=@CurrentStage,");
            strSql.Append("ProspectLoanStatus=@ProspectLoanStatus,");
            strSql.Append("Disposed=@Disposed,");
            strSql.Append("Ranking=@Ranking,");
            strSql.Append("Created=@Created,");
            strSql.Append("CreatedBy=@CreatedBy,");
            strSql.Append("Modifed=@Modifed,");
            strSql.Append("ModifiedBy=@ModifiedBy,");
            strSql.Append("GlobalId=@GlobalId,");
            strSql.Append("DateHMDA=@DateHMDA,");
            strSql.Append("DateProcessing=@DateProcessing,");
            strSql.Append("DateReSubmit=@DateReSubmit,");
            strSql.Append("DateDocsOut=@DateDocsOut,");
            strSql.Append("DateDocsReceived=@DateDocsReceived,");
            strSql.Append("LOS_LoanOfficer=@LOS_LoanOfficer,");
            strSql.Append("DateNote=@DateNote,");
            strSql.Append("LeadStar_username=@LeadStar_username,");
            strSql.Append("LeadStar_userid=@LeadStar_userid,");
            strSql.Append("Joint=@Joint,");
            strSql.Append("CoBrwType=@CoBrwType,");
            strSql.Append("PropertyType=@PropertyType,");
            strSql.Append("HousingStatus=@HousingStatus,");
            strSql.Append("RentAmount=@RentAmount,");
            strSql.Append("InterestOnly=@InterestOnly,");
            strSql.Append("IncludeEscrows=@IncludeEscrows,");
            strSql.Append("DT_FileID=@DT_FileID,");
            strSql.Append("TD_2=@TD_2,");
            strSql.Append("TD_2Amount=@TD_2Amount,");
            strSql.Append("Subordinate=@Subordinate,");
            strSql.Append("MonthlyPMI=@MonthlyPMI,");
            strSql.Append("MonthlyPMITax=@MonthlyPMITax,");
            strSql.Append("PurchasedDate=@PurchasedDate,");
            strSql.Append("FirstTimeBuyer=@FirstTimeHomeBuyer,");
            strSql.Append("MIOption=@MIOption,");
            strSql.Append("LoanChanged=@LoanChanged ");
            strSql.Append(" where FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4),
					new SqlParameter("@AppraisedValue", SqlDbType.Money,8),
					new SqlParameter("@CCScenario", SqlDbType.NVarChar,50),
					new SqlParameter("@CLTV", SqlDbType.SmallMoney,4),
					new SqlParameter("@County", SqlDbType.NVarChar,50),
					new SqlParameter("@DateOpen", SqlDbType.DateTime),
					new SqlParameter("@DateSubmit", SqlDbType.DateTime),
					new SqlParameter("@DateApprove", SqlDbType.DateTime),
					new SqlParameter("@DateClearToClose", SqlDbType.DateTime),
					new SqlParameter("@DateDocs", SqlDbType.DateTime),
					new SqlParameter("@DateFund", SqlDbType.DateTime),
					new SqlParameter("@DateRecord", SqlDbType.DateTime),
					new SqlParameter("@DateClose", SqlDbType.DateTime),
					new SqlParameter("@DateDenied", SqlDbType.DateTime),
					new SqlParameter("@DateCanceled", SqlDbType.DateTime),
					new SqlParameter("@DownPay", SqlDbType.Money,8),
					new SqlParameter("@EstCloseDate", SqlDbType.DateTime),
					new SqlParameter("@Lender", SqlDbType.Int,4),
					new SqlParameter("@LienPosition", SqlDbType.NVarChar,50),
					new SqlParameter("@LoanAmount", SqlDbType.Money,8),
					new SqlParameter("@LoanNumber", SqlDbType.NVarChar,50),
					new SqlParameter("@LoanType", SqlDbType.NVarChar,50),
					new SqlParameter("@LTV", SqlDbType.SmallMoney,4),
					new SqlParameter("@MonthlyPayment", SqlDbType.Money,8),
					new SqlParameter("@LenderNotes", SqlDbType.NVarChar,255),
					new SqlParameter("@Occupancy", SqlDbType.NVarChar,50),
					new SqlParameter("@Program", SqlDbType.NVarChar,255),
					new SqlParameter("@PropertyAddr", SqlDbType.NVarChar,50),
					new SqlParameter("@PropertyCity", SqlDbType.NVarChar,50),
					new SqlParameter("@PropertyState", SqlDbType.Char,2),
					new SqlParameter("@PropertyZip", SqlDbType.NVarChar,10),
					new SqlParameter("@Purpose", SqlDbType.NVarChar,50),
					new SqlParameter("@Rate", SqlDbType.SmallMoney,4),
					new SqlParameter("@RateLockExpiration", SqlDbType.DateTime),
					new SqlParameter("@SalesPrice", SqlDbType.Money,8),
					new SqlParameter("@Term", SqlDbType.SmallInt,2),
					new SqlParameter("@Due", SqlDbType.SmallInt,2),
					new SqlParameter("@DateSuspended", SqlDbType.DateTime),
					new SqlParameter("@RegionID", SqlDbType.Int,4),
					new SqlParameter("@DivisionID", SqlDbType.Int,4),
					new SqlParameter("@BranchID", SqlDbType.Int,4),
					new SqlParameter("@GroupID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.NVarChar,50),
					new SqlParameter("@LastCompletedStage", SqlDbType.NVarChar,50),
					new SqlParameter("@CurrentStage", SqlDbType.NVarChar,50),
					new SqlParameter("@ProspectLoanStatus", SqlDbType.NVarChar,50),
					new SqlParameter("@Disposed", SqlDbType.DateTime),
					new SqlParameter("@Ranking", SqlDbType.NVarChar,20),
					new SqlParameter("@Created", SqlDbType.DateTime),
					new SqlParameter("@CreatedBy", SqlDbType.Int,4),
					new SqlParameter("@Modifed", SqlDbType.DateTime),
					new SqlParameter("@ModifiedBy", SqlDbType.Int,4),
					new SqlParameter("@GlobalId", SqlDbType.NVarChar,255),
					new SqlParameter("@DateHMDA", SqlDbType.DateTime),
					new SqlParameter("@DateProcessing", SqlDbType.DateTime),
					new SqlParameter("@DateReSubmit", SqlDbType.DateTime),
					new SqlParameter("@DateDocsOut", SqlDbType.DateTime),
					new SqlParameter("@DateDocsReceived", SqlDbType.DateTime),
					new SqlParameter("@LOS_LoanOfficer", SqlDbType.NVarChar,255),
					new SqlParameter("@DateNote", SqlDbType.DateTime),
					new SqlParameter("@LeadStar_username", SqlDbType.NVarChar,255),
					new SqlParameter("@LeadStar_userid", SqlDbType.NVarChar,255),
					new SqlParameter("@Joint", SqlDbType.Bit,1),
					new SqlParameter("@CoBrwType", SqlDbType.NVarChar,255),
					new SqlParameter("@PropertyType", SqlDbType.NVarChar,255),
					new SqlParameter("@HousingStatus", SqlDbType.NVarChar,255),
					new SqlParameter("@RentAmount", SqlDbType.Decimal,5),
					new SqlParameter("@InterestOnly", SqlDbType.Bit,1),
					new SqlParameter("@IncludeEscrows", SqlDbType.Bit,1),
					new SqlParameter("@DT_FileID", SqlDbType.NVarChar,50),
					new SqlParameter("@TD_2", SqlDbType.Bit,1),
					new SqlParameter("@TD_2Amount", SqlDbType.Decimal,9),
					new SqlParameter("@Subordinate", SqlDbType.Bit,1),
					new SqlParameter("@MonthlyPMI", SqlDbType.Decimal,9),
					new SqlParameter("@MonthlyPMITax", SqlDbType.Decimal,9),
					new SqlParameter("@PurchasedDate", SqlDbType.DateTime),
					new SqlParameter("@FirstTimeHomeBuyer", SqlDbType.Bit),
					new SqlParameter("@MIOption", SqlDbType.NVarChar, 50),
   					new SqlParameter("@LoanChanged", SqlDbType.Bit)
            };
            parameters[0].Value = model.FileId;
            parameters[1].Value = model.AppraisedValue;
            parameters[2].Value = model.CCScenario;
            parameters[3].Value = model.CLTV;
            parameters[4].Value = model.County;
            parameters[5].Value = model.DateOpen;
            parameters[6].Value = model.DateSubmit;
            parameters[7].Value = model.DateApprove;
            parameters[8].Value = model.DateClearToClose;
            parameters[9].Value = model.DateDocs;
            parameters[10].Value = model.DateFund;
            parameters[11].Value = model.DateRecord;
            parameters[12].Value = model.DateClose;
            parameters[13].Value = model.DateDenied;
            parameters[14].Value = model.DateCanceled;
            parameters[15].Value = model.DownPay;
            parameters[16].Value = model.EstCloseDate;
            parameters[17].Value = model.Lender;
            parameters[18].Value = model.LienPosition;
            parameters[19].Value = model.LoanAmount;
            parameters[20].Value = model.LoanNumber;
            parameters[21].Value = model.LoanType;
            parameters[22].Value = model.LTV;
            parameters[23].Value = model.MonthlyPayment;
            parameters[24].Value = model.LenderNotes;
            parameters[25].Value = model.Occupancy;
            parameters[26].Value = model.Program;
            parameters[27].Value = model.PropertyAddr;
            parameters[28].Value = model.PropertyCity;
            parameters[29].Value = model.PropertyState;
            parameters[30].Value = model.PropertyZip;
            parameters[31].Value = model.Purpose;
            parameters[32].Value = model.Rate;
            parameters[33].Value = model.RateLockExpiration;
            parameters[34].Value = model.SalesPrice;
            parameters[35].Value = model.Term;
            parameters[36].Value = model.Due;
            parameters[37].Value = model.DateSuspended;
            parameters[38].Value = model.RegionID;
            parameters[39].Value = model.DivisionID;
            parameters[40].Value = model.BranchID;
            parameters[41].Value = model.GroupID;
            parameters[42].Value = model.UserID;
            parameters[43].Value = model.Status;
            parameters[44].Value = model.LastCompletedStage;
            parameters[45].Value = model.CurrentStage;
            parameters[46].Value = model.ProspectLoanStatus;
            parameters[47].Value = model.Disposed;
            parameters[48].Value = model.Ranking;
            parameters[49].Value = model.Created;
            parameters[50].Value = model.CreatedBy;
            parameters[51].Value = model.Modifed;
            parameters[52].Value = model.ModifiedBy;
            parameters[53].Value = model.GlobalId;
            parameters[54].Value = model.DateHMDA;
            parameters[55].Value = model.DateProcessing;
            parameters[56].Value = model.DateReSubmit;
            parameters[57].Value = model.DateDocsOut;
            parameters[58].Value = model.DateDocsReceived;
            parameters[59].Value = model.LOS_LoanOfficer;
            parameters[60].Value = model.DateNote;
            parameters[61].Value = model.LeadStar_username;
            parameters[62].Value = model.LeadStar_userid;
            parameters[63].Value = model.Joint;
            parameters[64].Value = model.CoBrwType;
            parameters[65].Value = model.PropertyType;
            parameters[66].Value = model.HousingStatus;
            parameters[67].Value = model.RentAmount;
            parameters[68].Value = model.InterestOnly;
            parameters[69].Value = model.IncludeEscrows;
            parameters[70].Value = model.DT_FileID;
            parameters[71].Value = model.TD_2;
            parameters[72].Value = model.TD_2Amount;
            parameters[73].Value = model.Subordinate;
            parameters[74].Value = model.MonthlyPMI;
            parameters[75].Value = model.MonthlyPMITax;
            parameters[76].Value = model.PurchasedDate;
            parameters[77].Value = model.FirstTimeHomeBuyer;
            parameters[78].Value = model.MIOption;
            parameters[79].Value = model.LoanChanged;
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int FileId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Loans ");
            strSql.Append(" where FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4)};
            parameters[0].Value = FileId;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LPWeb.Model.Loans GetModel(int FileId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 FileId,AppraisedValue,CCScenario,CLTV,County,DateOpen,DateSubmit,DateApprove,DateClearToClose,DateDocs,DateFund,DateRecord,DateClose,DateDenied,DateCanceled,DownPay,EstCloseDate,Lender,LienPosition,LoanAmount,LoanNumber,LoanType,LTV,MonthlyPayment,LenderNotes,Occupancy,Program,PropertyAddr,PropertyCity,PropertyState,PropertyZip,Purpose,Rate,RateLockExpiration,SalesPrice,Term,Due,DateSuspended,RegionID,DivisionID,BranchID,GroupID,UserID,Status,LastCompletedStage,CurrentStage,ProspectLoanStatus,Disposed,Ranking,Created,CreatedBy,Modifed,ModifiedBy,GlobalId,DateHMDA,DateProcessing,DateReSubmit,DateDocsOut,DateDocsReceived,LOS_LoanOfficer,DateNote,LeadStar_username,LeadStar_userid,Joint,CoBrwType,PropertyType,HousingStatus,RentAmount,InterestOnly,IncludeEscrows,DT_FileID,TD_2,TD_2Amount,Subordinate,MonthlyPMI,MonthlyPMITax,PurchasedDate from Loans ");
            strSql.Append(" where FileId=@FileId ");
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int,4)};
            parameters[0].Value = FileId;

            LPWeb.Model.Loans model = new LPWeb.Model.Loans();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["FileId"].ToString() != "")
                {
                    model.FileId = int.Parse(ds.Tables[0].Rows[0]["FileId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AppraisedValue"].ToString() != "")
                {
                    model.AppraisedValue = decimal.Parse(ds.Tables[0].Rows[0]["AppraisedValue"].ToString());
                }
                model.CCScenario = ds.Tables[0].Rows[0]["CCScenario"].ToString();
                if (ds.Tables[0].Rows[0]["CLTV"].ToString() != "")
                {
                    model.CLTV = decimal.Parse(ds.Tables[0].Rows[0]["CLTV"].ToString());
                }
                model.County = ds.Tables[0].Rows[0]["County"].ToString();
                if (ds.Tables[0].Rows[0]["DateOpen"].ToString() != "")
                {
                    model.DateOpen = DateTime.Parse(ds.Tables[0].Rows[0]["DateOpen"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DateSubmit"].ToString() != "")
                {
                    model.DateSubmit = DateTime.Parse(ds.Tables[0].Rows[0]["DateSubmit"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DateApprove"].ToString() != "")
                {
                    model.DateApprove = DateTime.Parse(ds.Tables[0].Rows[0]["DateApprove"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DateClearToClose"].ToString() != "")
                {
                    model.DateClearToClose = DateTime.Parse(ds.Tables[0].Rows[0]["DateClearToClose"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DateDocs"].ToString() != "")
                {
                    model.DateDocs = DateTime.Parse(ds.Tables[0].Rows[0]["DateDocs"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DateFund"].ToString() != "")
                {
                    model.DateFund = DateTime.Parse(ds.Tables[0].Rows[0]["DateFund"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DateRecord"].ToString() != "")
                {
                    model.DateRecord = DateTime.Parse(ds.Tables[0].Rows[0]["DateRecord"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DateClose"].ToString() != "")
                {
                    model.DateClose = DateTime.Parse(ds.Tables[0].Rows[0]["DateClose"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DateDenied"].ToString() != "")
                {
                    model.DateDenied = DateTime.Parse(ds.Tables[0].Rows[0]["DateDenied"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DateCanceled"].ToString() != "")
                {
                    model.DateCanceled = DateTime.Parse(ds.Tables[0].Rows[0]["DateCanceled"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DownPay"].ToString() != "")
                {
                    model.DownPay = decimal.Parse(ds.Tables[0].Rows[0]["DownPay"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EstCloseDate"].ToString() != "")
                {
                    model.EstCloseDate = DateTime.Parse(ds.Tables[0].Rows[0]["EstCloseDate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Lender"].ToString() != "")
                {
                    model.Lender = int.Parse(ds.Tables[0].Rows[0]["Lender"].ToString());
                }
                model.LienPosition = ds.Tables[0].Rows[0]["LienPosition"].ToString();
                if (ds.Tables[0].Rows[0]["LoanAmount"].ToString() != "")
                {
                    model.LoanAmount = decimal.Parse(ds.Tables[0].Rows[0]["LoanAmount"].ToString());
                }
                model.LoanNumber = ds.Tables[0].Rows[0]["LoanNumber"].ToString();
                model.LoanType = ds.Tables[0].Rows[0]["LoanType"].ToString();
                if (ds.Tables[0].Rows[0]["LTV"].ToString() != "")
                {
                    model.LTV = decimal.Parse(ds.Tables[0].Rows[0]["LTV"].ToString());
                }
                if (ds.Tables[0].Rows[0]["MonthlyPayment"].ToString() != "")
                {
                    model.MonthlyPayment = decimal.Parse(ds.Tables[0].Rows[0]["MonthlyPayment"].ToString());
                }
                model.LenderNotes = ds.Tables[0].Rows[0]["LenderNotes"].ToString();
                model.Occupancy = ds.Tables[0].Rows[0]["Occupancy"].ToString();
                model.Program = ds.Tables[0].Rows[0]["Program"].ToString();
                model.PropertyAddr = ds.Tables[0].Rows[0]["PropertyAddr"].ToString();
                model.PropertyCity = ds.Tables[0].Rows[0]["PropertyCity"].ToString();
                model.PropertyState = ds.Tables[0].Rows[0]["PropertyState"].ToString();
                model.PropertyZip = ds.Tables[0].Rows[0]["PropertyZip"].ToString();
                model.Purpose = ds.Tables[0].Rows[0]["Purpose"].ToString();
                if (ds.Tables[0].Rows[0]["Rate"].ToString() != "")
                {
                    model.Rate = decimal.Parse(ds.Tables[0].Rows[0]["Rate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["RateLockExpiration"].ToString() != "")
                {
                    model.RateLockExpiration = DateTime.Parse(ds.Tables[0].Rows[0]["RateLockExpiration"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SalesPrice"].ToString() != "")
                {
                    model.SalesPrice = decimal.Parse(ds.Tables[0].Rows[0]["SalesPrice"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Term"].ToString() != "")
                {
                    model.Term = int.Parse(ds.Tables[0].Rows[0]["Term"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Due"].ToString() != "")
                {
                    model.Due = int.Parse(ds.Tables[0].Rows[0]["Due"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DateSuspended"].ToString() != "")
                {
                    model.DateSuspended = DateTime.Parse(ds.Tables[0].Rows[0]["DateSuspended"].ToString());
                }
                if (ds.Tables[0].Rows[0]["RegionID"].ToString() != "")
                {
                    model.RegionID = int.Parse(ds.Tables[0].Rows[0]["RegionID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DivisionID"].ToString() != "")
                {
                    model.DivisionID = int.Parse(ds.Tables[0].Rows[0]["DivisionID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["BranchID"].ToString() != "")
                {
                    model.BranchID = int.Parse(ds.Tables[0].Rows[0]["BranchID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["GroupID"].ToString() != "")
                {
                    model.GroupID = int.Parse(ds.Tables[0].Rows[0]["GroupID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UserID"].ToString() != "")
                {
                    model.UserID = int.Parse(ds.Tables[0].Rows[0]["UserID"].ToString());
                }
                model.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                model.LastCompletedStage = ds.Tables[0].Rows[0]["LastCompletedStage"].ToString();
                model.CurrentStage = ds.Tables[0].Rows[0]["CurrentStage"].ToString();
                model.ProspectLoanStatus = ds.Tables[0].Rows[0]["ProspectLoanStatus"].ToString();
                if (ds.Tables[0].Rows[0]["Disposed"].ToString() != "")
                {
                    model.Disposed = DateTime.Parse(ds.Tables[0].Rows[0]["Disposed"].ToString());
                }
                model.Ranking = ds.Tables[0].Rows[0]["Ranking"].ToString();
                if (ds.Tables[0].Rows[0]["Created"].ToString() != "")
                {
                    model.Created = DateTime.Parse(ds.Tables[0].Rows[0]["Created"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CreatedBy"].ToString() != "")
                {
                    model.CreatedBy = int.Parse(ds.Tables[0].Rows[0]["CreatedBy"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Modifed"].ToString() != "")
                {
                    model.Modifed = DateTime.Parse(ds.Tables[0].Rows[0]["Modifed"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ModifiedBy"].ToString() != "")
                {
                    model.ModifiedBy = int.Parse(ds.Tables[0].Rows[0]["ModifiedBy"].ToString());
                }
                model.GlobalId = ds.Tables[0].Rows[0]["GlobalId"].ToString();
                if (ds.Tables[0].Rows[0]["DateHMDA"].ToString() != "")
                {
                    model.DateHMDA = DateTime.Parse(ds.Tables[0].Rows[0]["DateHMDA"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DateProcessing"].ToString() != "")
                {
                    model.DateProcessing = DateTime.Parse(ds.Tables[0].Rows[0]["DateProcessing"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DateReSubmit"].ToString() != "")
                {
                    model.DateReSubmit = DateTime.Parse(ds.Tables[0].Rows[0]["DateReSubmit"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DateDocsOut"].ToString() != "")
                {
                    model.DateDocsOut = DateTime.Parse(ds.Tables[0].Rows[0]["DateDocsOut"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DateDocsReceived"].ToString() != "")
                {
                    model.DateDocsReceived = DateTime.Parse(ds.Tables[0].Rows[0]["DateDocsReceived"].ToString());
                }
                model.LOS_LoanOfficer = ds.Tables[0].Rows[0]["LOS_LoanOfficer"].ToString();
                if (ds.Tables[0].Rows[0]["DateNote"].ToString() != "")
                {
                    model.DateNote = DateTime.Parse(ds.Tables[0].Rows[0]["DateNote"].ToString());
                }
                model.LeadStar_username = ds.Tables[0].Rows[0]["LeadStar_username"].ToString();
                model.LeadStar_userid = ds.Tables[0].Rows[0]["LeadStar_userid"].ToString();
                if (ds.Tables[0].Rows[0]["Joint"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Joint"].ToString() == "1") || (ds.Tables[0].Rows[0]["Joint"].ToString().ToLower() == "true"))
                    {
                        model.Joint = true;
                    }
                    else
                    {
                        model.Joint = false;
                    }
                }
                model.CoBrwType = ds.Tables[0].Rows[0]["CoBrwType"].ToString();
                model.PropertyType = ds.Tables[0].Rows[0]["PropertyType"].ToString();
                model.HousingStatus = ds.Tables[0].Rows[0]["HousingStatus"].ToString();
                if (ds.Tables[0].Rows[0]["RentAmount"].ToString() != "")
                {
                    model.RentAmount = decimal.Parse(ds.Tables[0].Rows[0]["RentAmount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["InterestOnly"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["InterestOnly"].ToString() == "1") || (ds.Tables[0].Rows[0]["InterestOnly"].ToString().ToLower() == "true"))
                    {
                        model.InterestOnly = true;
                    }
                    else
                    {
                        model.InterestOnly = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["IncludeEscrows"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IncludeEscrows"].ToString() == "1") || (ds.Tables[0].Rows[0]["IncludeEscrows"].ToString().ToLower() == "true"))
                    {
                        model.IncludeEscrows = true;
                    }
                    else
                    {
                        model.IncludeEscrows = false;
                    }
                }
                model.DT_FileID = ds.Tables[0].Rows[0]["DT_FileID"].ToString();
                if (ds.Tables[0].Rows[0]["TD_2"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["TD_2"].ToString() == "1") || (ds.Tables[0].Rows[0]["TD_2"].ToString().ToLower() == "true"))
                    {
                        model.TD_2 = true;
                    }
                    else
                    {
                        model.TD_2 = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["TD_2Amount"].ToString() != "")
                {
                    model.TD_2Amount = decimal.Parse(ds.Tables[0].Rows[0]["TD_2Amount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Subordinate"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Subordinate"].ToString() == "1") || (ds.Tables[0].Rows[0]["Subordinate"].ToString().ToLower() == "true"))
                    {
                        model.Subordinate = true;
                    }
                    else
                    {
                        model.Subordinate = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["MonthlyPMI"].ToString() != "")
                {
                    model.MonthlyPMI = decimal.Parse(ds.Tables[0].Rows[0]["MonthlyPMI"].ToString());
                }
                if (ds.Tables[0].Rows[0]["MonthlyPMITax"].ToString() != "")
                {
                    model.MonthlyPMITax = decimal.Parse(ds.Tables[0].Rows[0]["MonthlyPMITax"].ToString());
                }

                if (ds.Tables[0].Rows[0]["PurchasedDate"].ToString() != "")
                {
                    model.PurchasedDate = DateTime.Parse(ds.Tables[0].Rows[0]["PurchasedDate"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select FileId,AppraisedValue,CCScenario,CLTV,County,DateOpen,DateSubmit,DateApprove,DateClearToClose,DateDocs,DateFund,DateRecord,DateClose,DateDenied,DateCanceled,DownPay,EstCloseDate,Lender,LienPosition,LoanAmount,LoanNumber,LoanType,LTV,MonthlyPayment,LenderNotes,Occupancy,Program,PropertyAddr,PropertyCity,PropertyState,PropertyZip,Purpose,Rate,RateLockExpiration,SalesPrice,Term,Due,DateSuspended,RegionID,DivisionID,BranchID,GroupID,UserID,Status,LastCompletedStage,CurrentStage,ProspectLoanStatus,Disposed,Ranking,Created,CreatedBy,Modifed,ModifiedBy,GlobalId,DateHMDA,DateProcessing,DateReSubmit,DateDocsOut,DateDocsReceived,LOS_LoanOfficer,DateNote,LeadStar_username,LeadStar_userid,Joint,CoBrwType,PropertyType,HousingStatus,RentAmount,InterestOnly,IncludeEscrows,DT_FileID,TD_2,TD_2Amount,Subordinate,MonthlyPMI,MonthlyPMITax,PurchasedDate ");
            strSql.Append(" FROM Loans ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" FileId,AppraisedValue,CCScenario,CLTV,County,DateOpen,DateSubmit,DateApprove,DateClearToClose,DateDocs,DateFund,DateRecord,DateClose,DateDenied,DateCanceled,DownPay,EstCloseDate,Lender,LienPosition,LoanAmount,LoanNumber,LoanType,LTV,MonthlyPayment,LenderNotes,Occupancy,Program,PropertyAddr,PropertyCity,PropertyState,PropertyZip,Purpose,Rate,RateLockExpiration,SalesPrice,Term,Due,DateSuspended,RegionID,DivisionID,BranchID,GroupID,UserID,Status,LastCompletedStage,CurrentStage,ProspectLoanStatus,Disposed,Ranking,Created,CreatedBy,Modifed,ModifiedBy,GlobalId,DateHMDA,DateProcessing,DateReSubmit,DateDocsOut,DateDocsReceived,LOS_LoanOfficer,DateNote,LeadStar_username,LeadStar_userid,Joint,CoBrwType,PropertyType,HousingStatus,RentAmount,InterestOnly,IncludeEscrows,DT_FileID,TD_2,TD_2Amount,Subordinate,MonthlyPMI,MonthlyPMITax,PurchasedDate ");
            strSql.Append(" FROM Loans ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@tblName", SqlDbType.VarChar, 255),
                    new SqlParameter("@fldName", SqlDbType.VarChar, 255),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@IsReCount", SqlDbType.Bit),
                    new SqlParameter("@OrderType", SqlDbType.Bit),
                    new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
                    };
            parameters[0].Value = "Loans";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }

        #endregion  成员方法
    }
}

