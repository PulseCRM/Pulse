using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using Common;
using focusIT;
using LP2.Service.Common;

namespace DataAccess
{
    public class PointFolderInfo
    {
        public int FolderId;
        public string Name;
        public string Path;
        public int BranchId;
        public bool Enabled;
        public int LoanStatus; // 1 = Processing, 2 = Canceled, 3 = Closed, 4 = Denied, 5 = Suspended, 6 = Prospect, 7 = Archive, 8 = Archive Prospect
        public bool AutoNaming;
        public string FilenamePrefix;
        public bool RandomNumbering;
        public int FilenameLength;
    }

    public class AlertDays
    {
        public int AlertYellowDays = 1;
        public int AlertRedDays = 3;
        public int TaskYellowDays = 5;
        public int TaskRedDays = 3;
        public int RateLockYellowDays = 7;
        public int RateLockRedDays = 5;
    }

    public class PointConfig
    {
        public int ImportInterval;
        public string WinPointIni;
        public string PointFldIdMap;
        public string CardexFile;
        public string MasterSource;
        public string TracToolsLogin;
        public string TracToolsPwd;
        public bool Auto_ConvertLead { get; set; }
        public bool AutoApplyProcessingWorkflow { get; set; }
        public bool AutoApplyProspectWorkflow { get; set; }
        public bool Enable_MultiBranchFolders;
    }

    public class EmailQue
    {
        public int EmailId { get; set; }
        public string ToUser { get; set; }
        public string ToContact { get; set; }
        public string ToBorrower { get; set; }
        public int EmailTmplId { get; set; }
        public int LoanAlertId { get; set; }
        public int FileId { get; set; }
    }

    public partial class DataAccess
    {
        static readonly string PointCentralDBConnString = ConfigurationManager.ConnectionStrings["PointCentralDBString"].ConnectionString;
        public static Dictionary<string, string> DbFieldMapping = new Dictionary<string, string>();
        static DataAccess()
        {
            #region Email Body DB fields
            //	Company 
            DbFieldMapping.Add("Company Name", "SELECT TOP 1 [Name] AS [retValue] FROM dbo.Company_General");

            DbFieldMapping.Add("Company Address", "SELECT TOP 1 [Address]  AS [retValue] FROM dbo.Company_General");

            DbFieldMapping.Add("Company City", "SELECT TOP 1 [City]  AS [retValue] FROM dbo.Company_General");

            DbFieldMapping.Add("Company State", "SELECT TOP 1 [State]  AS [retValue] FROM dbo.Company_General");

            DbFieldMapping.Add("Company Zip", "SELECT TOP 1 [Zip]  AS [retValue] FROM dbo.Company_General");
            DbFieldMapping.Add("Company Email", "SELECT TOP 1 [Email]  AS [retValue] FROM dbo.Company_General");
            DbFieldMapping.Add("Company Fax", "SELECT TOP 1 [Fax]  AS [retValue] FROM dbo.Company_General");
            DbFieldMapping.Add("Company Phone", "SELECT TOP 1 [Phone]  AS [retValue] FROM dbo.Company_General");
            DbFieldMapping.Add("Company Website", "SELECT TOP 1 [WebURL]  AS [retValue] FROM dbo.Company_General");

            DbFieldMapping.Add("Company Homepage Logo", "SELECT TOP 1 [HomePageLogoData]  AS [retValue] FROM dbo.Company_Web");

            DbFieldMapping.Add("Company Subpage Logo", "SELECT TOP 1 [SubPageLogoData]   AS [retValue] FROM dbo.Company_Web");

            //BRANCH
            DbFieldMapping.Add("Branch Name", "select TOP 1 [Name]   AS [retValue] from dbo.Branches where BranchId in (select BranchId from dbo.PointFolders pf inner join dbo.PointFiles fi on pf.FolderId=fi.FolderId where fi.FileId={0})");

            DbFieldMapping.Add("Branch Address", "select TOP 1 [BranchAddress]   AS [retValue] from dbo.Branches where BranchId in (select BranchId from dbo.PointFolders pf inner join dbo.PointFiles fi on pf.FolderId=fi.FolderId where fi.FileId={0})");

            DbFieldMapping.Add("Branch City", "select TOP 1 [City]   AS [retValue] from dbo.Branches where BranchId in (select BranchId from dbo.PointFolders pf inner join dbo.PointFiles fi on pf.FolderId=fi.FolderId where fi.FileId={0})");

            DbFieldMapping.Add("Branch State", "select TOP 1 [BranchState]   AS [retValue] from dbo.Branches where BranchId in (select BranchId from dbo.PointFolders pf inner join dbo.PointFiles fi on pf.FolderId=fi.FolderId where fi.FileId={0})");

            DbFieldMapping.Add("Branch Zip", "select TOP 1 [Zip]   AS [retValue] from dbo.Branches where BranchId in (select BranchId from dbo.PointFolders pf inner join dbo.PointFiles fi on pf.FolderId=fi.FolderId where fi.FileId={0})");

            DbFieldMapping.Add("Branch Email", "select TOP 1 [Email]   AS [retValue] from dbo.Branches where BranchId in (select BranchId from dbo.PointFolders pf inner join dbo.PointFiles fi on pf.FolderId=fi.FolderId where fi.FileId={0})");
            DbFieldMapping.Add("Branch Fax", "select TOP 1 [Fax]   AS [retValue] from dbo.Branches where BranchId in (select BranchId from dbo.PointFolders pf inner join dbo.PointFiles fi on pf.FolderId=fi.FolderId where fi.FileId={0})");
            DbFieldMapping.Add("Branch Phone", "select TOP 1 [Phone]   AS [retValue] from dbo.Branches where BranchId in (select BranchId from dbo.PointFolders pf inner join dbo.PointFiles fi on pf.FolderId=fi.FolderId where fi.FileId={0})");
            DbFieldMapping.Add("Branch Website", "select TOP 1 [WebURL]   AS [retValue] from dbo.Branches where BranchId in (select BranchId from dbo.PointFolders pf inner join dbo.PointFiles fi on pf.FolderId=fi.FolderId where fi.FileId={0})");

            //todo:binary check data
            //DbFieldMapping.Add("Branch Logo", "select TOP 1 [WebsiteLogo]   AS [retValue] from dbo.Branches where BranchId in (select BranchId from dbo.PointFolders pf inner join dbo.PointFiles fi on pf.FolderId=fi.FolderId where fi.FileId={0})");
            DbFieldMapping.Add("Branch Logo", "Select top 1 b.[WebsiteLogo]  AS [retValue] from dbo.Branches b inner join Loans l on b.BranchId=l.BranchId where l.FileId={0}");

            DbFieldMapping.Add("Loan Officer Picture", "select u.[UserPictureFile]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=3 and lt.FileId = {0}");
            DbFieldMapping.Add("Loan Officer First Name", "select u.[FirstName]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=3 and lt.FileId = {0}");
            DbFieldMapping.Add("Loan Officer Last Name", "select u.[LastName]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=3 and lt.FileId = {0}");
            DbFieldMapping.Add("Loan Officer Email", "select u.[EmailAddress]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=3 and lt.FileId = {0}");
            DbFieldMapping.Add("Loan Officer Cell", "select u.[Cell]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=3 and lt.FileId = {0}");
            DbFieldMapping.Add("Loan Officer Fax", "select u.[Fax]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=3 and lt.FileId = {0}");
            DbFieldMapping.Add("Loan Officer Phone", "select u.[Phone]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=3 and lt.FileId = {0}");

            DbFieldMapping.Add("Processor First Name", "select u.[FirstName]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=5 and lt.FileId = {0}");
            DbFieldMapping.Add("Processor Last Name", "select u.[LastName]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=5 and lt.FileId = {0}");
            DbFieldMapping.Add("Processor Email", "select u.[EmailAddress]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=5 and lt.FileId = {0}");
            DbFieldMapping.Add("Processor Cell", "select u.[Cell]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=5 and lt.FileId = {0}");
            DbFieldMapping.Add("Processor Fax", "select u.[Fax]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=5 and lt.FileId = {0}");
            DbFieldMapping.Add("Processor Phone", "select u.[Phone]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=5 and lt.FileId = {0}");

            DbFieldMapping.Add("Underwriter First Name", "select u.[FirstName]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=6 and lt.FileId = {0}");
            DbFieldMapping.Add("Underwriter Last Name", "select u.[LastName]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=6 and lt.FileId = {0}");
            DbFieldMapping.Add("Underwriter Email", "select u.[EmailAddress]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=6 and lt.FileId = {0}");
            DbFieldMapping.Add("Underwriter Cell", "select u.[Cell]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=6 and lt.FileId = {0}");
            DbFieldMapping.Add("Underwriter Fax", "select u.[Fax]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=6 and lt.FileId = {0}");
            DbFieldMapping.Add("Underwriter Phone", "select u.[Phone]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=6 and lt.FileId = {0}");


            DbFieldMapping.Add("Loan Officer Assistant First Name", "select u.[FirstName]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=4 and lt.FileId = {0}");
            DbFieldMapping.Add("Loan Officer Assistant Last Name", "select u.[LastName]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=4 and lt.FileId = {0}");
            DbFieldMapping.Add("Loan Officer Assistant Email", "select u.[EmailAddress]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=4 and lt.FileId = {0}");
            DbFieldMapping.Add("Loan Officer Assistant Cell", "select u.[Cell]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=4 and lt.FileId = {0}");
            DbFieldMapping.Add("Loan Officer Assistant Fax", "select u.[Fax]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=4 and lt.FileId = {0}");
            DbFieldMapping.Add("Loan Officer Assistant Phone", "select u.[Phone]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=4 and lt.FileId = {0}");


            DbFieldMapping.Add("Doc Prep First Name", "select u.[FirstName]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=8 and lt.FileId = {0}");
            DbFieldMapping.Add("Doc Prep Last Name", "select u.[LastName]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=8 and lt.FileId = {0}");
            DbFieldMapping.Add("Doc Prep Email", "select u.[EmailAddress]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=8 and lt.FileId = {0}");
            DbFieldMapping.Add("Doc Prep Cell", "select u.[Cell]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=8 and lt.FileId = {0}");
            DbFieldMapping.Add("Doc Prep Fax", "select u.[Fax]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=8 and lt.FileId = {0}");
            DbFieldMapping.Add("Doc Prep Phone", "select u.[Phone]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=8 and lt.FileId = {0}");


            DbFieldMapping.Add("Closer First Name", "select u.[FirstName]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=7 and lt.FileId = {0}");
            DbFieldMapping.Add("Closer Last Name", "select u.[LastName]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=7 and lt.FileId = {0}");
            DbFieldMapping.Add("Closer Email", "select u.[EmailAddress]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=7 and lt.FileId = {0}");
            DbFieldMapping.Add("Closer Cell", "select u.[Cell]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=7 and lt.FileId = {0}");
            DbFieldMapping.Add("Closer Fax", "select u.[Fax]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=7 and lt.FileId = {0}");
            DbFieldMapping.Add("Closer Phone", "select u.[Phone]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=7 and lt.FileId = {0}");

            DbFieldMapping.Add("Shipper First Name", "select u.[FirstName]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=9 and lt.FileId = {0}");
            DbFieldMapping.Add("Shipper Last Name", "select u.[LastName]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=9 and lt.FileId = {0}");
            DbFieldMapping.Add("Shipper Email", "select u.[EmailAddress]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=9 and lt.FileId = {0}");
            DbFieldMapping.Add("Shipper Cell", "select u.[Cell]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=9 and lt.FileId = {0}");
            DbFieldMapping.Add("Shipper Fax", "select u.[Fax]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=9 and lt.FileId = {0}");
            DbFieldMapping.Add("Shipper Phone", "select u.[Phone]   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=9 and lt.FileId = {0}");

            DbFieldMapping.Add("Sender Picture", "select UserPictureFile AS [retValue] from dbo.Users where UserId={0}");
            DbFieldMapping.Add("Sender Signature", "select [Signature] AS [retValue] from dbo.Users where UserId={0}");

            //prospect fields for fileid
            DbFieldMapping.Add("Client First Name", "SELECT FirstName AS [retValue] FROM lpvw_LoanProspect WHERE FileId={0}");
            DbFieldMapping.Add("Client Last Name", "SELECT LastName AS [retValue] FROM lpvw_LoanProspect WHERE FileId={0}");
            DbFieldMapping.Add("Client Middle Name", "SELECT MiddleName AS [retValue] FROM lpvw_LoanProspect WHERE FileId={0}");
            DbFieldMapping.Add("Client Nick Name", "SELECT NickName AS [retValue] FROM lpvw_LoanProspect WHERE FileId={0}");
            DbFieldMapping.Add("Client Title", "SELECT Title AS [retValue] FROM lpvw_LoanProspect WHERE FileId={0}");
            DbFieldMapping.Add("Client Home Phone", "SELECT HomePhone AS [retValue] FROM lpvw_LoanProspect WHERE FileId={0}");
            DbFieldMapping.Add("Client Cell Phone", "SELECT CellPhone AS [retValue] FROM lpvw_LoanProspect WHERE FileId={0}");
            DbFieldMapping.Add("Client Business Phone", "SELECT BusinessPhone AS [retValue] FROM lpvw_LoanProspect WHERE FileId={0}");
            DbFieldMapping.Add("Client Fax", "SELECT Fax AS [retValue] FROM lpvw_LoanProspect WHERE FileId={0}");
            DbFieldMapping.Add("Client Email", "SELECT Email AS [retValue] FROM lpvw_LoanProspect WHERE FileId={0}");
            DbFieldMapping.Add("Client DOB", "SELECT DOB AS [retValue] FROM lpvw_LoanProspect WHERE FileId={0}");
            DbFieldMapping.Add("Client Mailing Address", "SELECT MailingAddr AS [retValue] FROM lpvw_LoanProspect WHERE FileId={0}");
            DbFieldMapping.Add("Client Mailing City", "SELECT MailingCity AS [retValue] FROM lpvw_LoanProspect WHERE FileId={0}");
            DbFieldMapping.Add("Client Mailing State", "SELECT MailingState AS [retValue] FROM lpvw_LoanProspect WHERE FileId={0}");
            DbFieldMapping.Add("Client Mailing Zip", "SELECT MailingZip AS [retValue] FROM lpvw_LoanProspect WHERE FileId={0}");
            //prospect field prospectid
            DbFieldMapping.Add("Client First NameP", "SELECT FirstName AS [retValue] FROM Contacts WHERE ContactId={0}");
            DbFieldMapping.Add("Client Last NameP", "SELECT LastName AS [retValue] FROM Contacts WHERE ContactId={0}");
            DbFieldMapping.Add("Client Middle NameP", "SELECT MiddleName AS [retValue] FROM Contacts WHERE ContactId={0}");
            DbFieldMapping.Add("Client Nick NameP", "SELECT NickName AS [retValue] FROM Contacts WHERE ContactId={0}");
            DbFieldMapping.Add("Client TitleP", "SELECT Title AS [retValue] FROM Contacts WHERE ContactId={0}");
            DbFieldMapping.Add("Client Home PhoneP", "SELECT HomePhone AS [retValue] FROM Contacts WHERE ContactId={0}");
            DbFieldMapping.Add("Client Cell PhoneP", "SELECT CellPhone AS [retValue] FROM Contacts WHERE ContactId={0}");
            DbFieldMapping.Add("Client Business PhoneP", "SELECT BusinessPhone AS [retValue] FROM Contacts WHERE ContactId={0}");
            DbFieldMapping.Add("Client FaxP", "SELECT Fax AS [retValue] FROM Contacts WHERE ContactId={0}");
            DbFieldMapping.Add("Client EmailP", "SELECT Email AS [retValue] FROM Contacts WHERE ContactId={0}");
            DbFieldMapping.Add("Client DOBP", "SELECT DOB AS [retValue] FROM Contacts WHERE ContactId={0}");
            DbFieldMapping.Add("Client Mailing AddressP", "SELECT MailingAddr AS [retValue] FROM Contacts WHERE ContactId={0}");
            DbFieldMapping.Add("Client Mailing CityP", "SELECT MailingCity AS [retValue] FROM Contacts WHERE ContactId={0}");
            DbFieldMapping.Add("Client Mailing StateP", "SELECT MailingState AS [retValue] FROM Contacts WHERE ContactId={0}");
            DbFieldMapping.Add("Client Mailing ZipP", "SELECT MailingZip AS [retValue] FROM Contacts WHERE ContactId={0}");
            //prospect field ProspectTask Id
            DbFieldMapping.Add("Client First NameT", "SELECT c.FirstName AS [retValue] FROM dbo.Contacts c INNER JOIN dbo.ProspectTasks pt  ON c.ContactId=pt.ContactId  WHERE pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Client Last NameT", "SELECT c.LastName AS [retValue] FROM dbo.Contacts c INNER JOIN dbo.ProspectTasks pt  ON c.ContactId=pt.ContactId  WHERE pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Client Middle NameT", "SELECT c.MiddleName AS [retValue] FROM dbo.Contacts c INNER JOIN dbo.ProspectTasks pt  ON c.ContactId=pt.ContactId  WHERE pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Client Nick NameT", "SELECT c.NickName AS [retValue] FROM dbo.Contacts c INNER JOIN dbo.ProspectTasks pt  ON c.ContactId=pt.ContactId  WHERE pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Client TitleT", "SELECT c.Title AS [retValue] FROM dbo.Contacts c INNER JOIN dbo.ProspectTasks pt  ON c.ContactId=pt.ContactId  WHERE pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Client Home PhoneT", "SELECT c.HomePhone AS [retValue] FROM dbo.Contacts c INNER JOIN dbo.ProspectTasks pt  ON c.ContactId=pt.ContactId  WHERE pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Client Cell PhoneT", "SELECT c.CellPhone AS [retValue] FROM dbo.Contacts c INNER JOIN dbo.ProspectTasks pt  ON c.ContactId=pt.ContactId  WHERE pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Client Business PhoneT", "SELECT c.BusinessPhone AS [retValue] FROM dbo.Contacts c INNER JOIN dbo.ProspectTasks pt  ON c.ContactId=pt.ContactId  WHERE pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Client FaxT", "SELECT c.Fax AS [retValue] FROM dbo.Contacts c INNER JOIN dbo.ProspectTasks pt  ON c.ContactId=pt.ContactId  WHERE pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Client EmailT", "SELECT c.Email AS [retValue] FROM dbo.Contacts c INNER JOIN dbo.ProspectTasks pt  ON c.ContactId=pt.ContactId  WHERE pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Client DOBT", "SELECT c.DOB AS [retValue] FROM dbo.Contacts c INNER JOIN dbo.ProspectTasks pt  ON c.ContactId=pt.ContactId  WHERE pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Client Mailing AddressT", "SELECT c.MailingAddr AS [retValue] FROM dbo.Contacts c INNER JOIN dbo.ProspectTasks pt  ON c.ContactId=pt.ContactId  WHERE pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Client Mailing CityT", "SELECT c.MailingCity AS [retValue] FROM dbo.Contacts c INNER JOIN dbo.ProspectTasks pt  ON c.ContactId=pt.ContactId  WHERE pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Client Mailing StateT", "SELECT c.MailingState AS [retValue] FROM dbo.Contacts c INNER JOIN dbo.ProspectTasks pt  ON c.ContactId=pt.ContactId  WHERE pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Client Mailing ZipT", "SELECT c.MailingZip AS [retValue] FROM dbo.Contacts c INNER JOIN dbo.ProspectTasks pt  ON c.ContactId=pt.ContactId  WHERE pt.ProspectTaskId={0}");


            //loan alert id
            DbFieldMapping.Add("Task Owner First nameLA", "select u.FirstName AS [retValue] from LoanTasks lt inner join LoanAlerts la on lt.LoanTaskId=la.LoanTaskId inner join Users u on lt.Owner=u.UserId where la.LoanAlertId={0}");
            DbFieldMapping.Add("Task Owner Last nameLA", "select u.LastName AS [retValue] from LoanTasks lt inner join LoanAlerts la on lt.LoanTaskId=la.LoanTaskId inner join Users u on lt.Owner=u.UserId where la.LoanAlertId={0}");
            DbFieldMapping.Add("Task Owner Full nameLA", "select u.Username AS [retValue] from LoanTasks lt inner join LoanAlerts la on lt.LoanTaskId=la.LoanTaskId inner join Users u on lt.Owner=u.UserId where la.LoanAlertId={0}");
            DbFieldMapping.Add("Task Owner EmailLA", "select u.EmailAddress AS [retValue] from LoanTasks lt inner join LoanAlerts la on lt.LoanTaskId=la.LoanTaskId inner join Users u on lt.Owner=u.UserId where la.LoanAlertId={0}");
            DbFieldMapping.Add("Task Owner SignatureLA", "select u.Signature AS [retValue] from LoanTasks lt inner join LoanAlerts la on lt.LoanTaskId=la.LoanTaskId inner join Users u on lt.Owner=u.UserId where la.LoanAlertId={0}");

            //prospect task id
            DbFieldMapping.Add("Task Owner First nameT", "select u.FirstName AS [retValue] from dbo.ProspectTasks pt inner join dbo.ProspectAlerts pa on pt.ProspectTaskId=pa.ProspectTaskId inner join Users u on pt.OwnerId=u.UserId where pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Task Owner Last nameT", "select u.LastName AS [retValue] from dbo.ProspectTasks pt inner join dbo.ProspectAlerts pa on pt.ProspectTaskId=pa.ProspectTaskId inner join Users u on pt.OwnerId=u.UserId where pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Task Owner Full nameT", "select u.Username AS [retValue] from dbo.ProspectTasks pt inner join dbo.ProspectAlerts pa on pt.ProspectTaskId=pa.ProspectTaskId inner join Users u on pt.OwnerId=u.UserId where pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Task Owner EmailT", "select u.EmailAddress AS [retValue] from dbo.ProspectTasks pt inner join dbo.ProspectAlerts pa on pt.ProspectTaskId=pa.ProspectTaskId inner join Users u on pt.OwnerId=u.UserId where pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Task Owner SignatureT", "select u.Signature AS [retValue] from dbo.ProspectTasks pt inner join dbo.ProspectAlerts pa on pt.ProspectTaskId=pa.ProspectTaskId inner join Users u on pt.OwnerId=u.UserId where pt.ProspectTaskId={0}");

            //loan alert id
            DbFieldMapping.Add("Task DescriptionLA", "select lt.Name AS [retValue] from LoanTasks lt inner join LoanAlerts la on lt.LoanTaskId=la.LoanTaskId inner join Users u on lt.Owner=u.UserId where la.LoanAlertId={0}");
            DbFieldMapping.Add("Task Due DateLA", "select lt.Due AS [retValue] from LoanTasks lt inner join LoanAlerts la on lt.LoanTaskId=la.LoanTaskId inner join Users u on lt.Owner=u.UserId where la.LoanAlertId={0}");


            //prospect id
            DbFieldMapping.Add("Task DescriptionP", "select pt.Desc AS [retValue] from dbo.ProspectTasks pt inner join dbo.ProspectAlerts pa on pt.ProspectTaskId=pa.ProspectTaskId inner join Users u on pt.OwnerId=u.UserId where pa.ContactId={0}");
            DbFieldMapping.Add("Task Due DateP", "select pt.Due AS [retValue] from dbo.ProspectTasks pt inner join dbo.ProspectAlerts pa on pt.ProspectTaskId=pa.ProspectTaskId inner join Users u on pt.OwnerId=u.UserId where pa.ContactId={0}");
            DbFieldMapping.Add("Loan Officer SignatureP", "select u.Signature   AS [retValue] from dbo.Prospect p inner join Users u on p.Loanofficer=u.UserId where p.Contactid={0}");

            //prospect task id
            DbFieldMapping.Add("Task DescriptionT", "select pt.Desc AS [retValue] from dbo.ProspectTasks pt inner join dbo.ProspectAlerts pa on pt.ProspectTaskId=pa.ProspectTaskId inner join Users u on pt.OwnerId=u.UserId where pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Task Due DateT", "select pt.Due AS [retValue] from dbo.ProspectTasks pt inner join dbo.ProspectAlerts pa on pt.ProspectTaskId=pa.ProspectTaskId inner join Users u on pt.OwnerId=u.UserId where pt.ProspectTaskId={0}");
            DbFieldMapping.Add("Loan Officer SignatureT", "select u.Signature   AS [retValue] from dbo.Prospect p inner join Users u on p.Loanofficer=u.UserId inner join dbo.ProspectTasks pt on p.Contactid=pt.ContactId   where pt.ProspectTaskId={0}");

            //field id
            //DbFieldMapping.Add("Loan Officer Picture", "select u.UserPictureFile   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=3 and lt.FileId = {0}");
            DbFieldMapping.Add("Loan Officer Signature", "select u.Signature   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=3 and lt.FileId = {0}");
            DbFieldMapping.Add("Loan Officer Assistant Picture", "select u.UserPictureFile   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=4 and lt.FileId = {0}");
            DbFieldMapping.Add("Loan Officer Assistant Signature", "select u.Signature   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=4 and lt.FileId = {0}");
            DbFieldMapping.Add("Processor Picture", "select u.UserPictureFile   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=5 and lt.FileId = {0}");
            DbFieldMapping.Add("Processor Signature", "select u.Signature   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=5 and lt.FileId = {0}");
            DbFieldMapping.Add("Underwriter Picture", "select u.UserPictureFile   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=6 and lt.FileId = {0}");
            DbFieldMapping.Add("Underwriter Signature", "select u.Signature   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=6 and lt.FileId = {0}");
            DbFieldMapping.Add("Closer Picture", "select u.UserPictureFile   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=7 and lt.FileId = {0}");
            DbFieldMapping.Add("Closer Signature", "select u.Signature   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=7 and lt.FileId = {0}");
            DbFieldMapping.Add("Shipper Picture", "select u.UserPictureFile   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=9 and lt.FileId = {0}");
            DbFieldMapping.Add("Shipper Signature", "select u.Signature   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=9 and lt.FileId = {0}");
            DbFieldMapping.Add("Doc Prep Picture", "select u.UserPictureFile   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=8 and lt.FileId = {0}");
            DbFieldMapping.Add("Doc Prep Signature", "select u.Signature   AS [retValue] from LoanTeam lt inner join Users u  on lt.UserId=u.UserId inner join Roles r  on lt.RoleId=r.RoleId and r.RoleId=8 and lt.FileId = {0}");


            //Task Alert Link
            DbFieldMapping.Add("Task Alert LinkLA", "SELECT REPLACE(REPLACE((SELECT TOP 1 replace('<a href=\"@URL@Pipeline/TaskAlertDetail.aspx?fileID=@fileID@&LoanTaskId=@LoanTaskID@\">@URL@Pipeline/TaskAlertDetail.aspx?fileID=@fileID@&LoanTaskId=@LoanTaskID@</a>','@URL@',LPCompanyURL) from dbo.Company_Web),'@fileID@',FileId),'@LoanTaskID@',LoanTaskId)  AS [retValue] from  dbo.LoanAlerts where LoanAlertId={0}");
            //Rule Alert Link
            DbFieldMapping.Add("Rule Alert LinkLA", "SELECT replace((SELECT TOP 1 replace('<a href=\"@URL@LoanDetails/RuleAlertPopup.aspx?LoanID=@fileID@&AlertID={0}\">@URL@LoanDetails/RuleAlertPopup.aspx?LoanID=@fileID@&AlertID={0}</a>','@URL@',LPCompanyURL) from dbo.Company_Web),'@fileID@',FileId) AS [retValue] from  dbo.LoanAlerts where LoanAlertId={0}");

            //prospect only have task alert link
            DbFieldMapping.Add("Task Alert LinkT", "SELECT REPLACE((SELECT TOP 1 replace('<a href=\"@URL@Pipeline/ProspectTaskAlertDetail.aspx?contactID=@prospectID@\">@URL@Pipeline/ProspectTaskAlertDetail.aspx?contactID=@prospectID@</a>','@URL@',LPCompanyURL) from dbo.Company_Web),'@prospectID@',ContactId)  AS [retValue] from  dbo.ProspectTasks where ProspectTaskId={0}");
            DbFieldMapping.Add("Task Alert LinkPA", "SELECT REPLACE((SELECT TOP 1 replace('<a href=\"@URL@Pipeline/ProspectTaskAlertDetail.aspx?contactID=@prospectID@\">@URL@Pipeline/ProspectTaskAlertDetail.aspx?contactID=@prospectID@</a>','@URL@',LPCompanyURL) from dbo.Company_Web),'@prospectID@',ContactId)  AS [retValue] from  dbo.ProspectAlerts where ProspectAlertId={0}");

            DbFieldMapping.Add("Loan Link", "SELECT TOP 1 replace('<a href=\"@URL@LoanDetails/LoanDetails.aspx?FromPage=@URL@Pipeline/ProcessingPipelineSummary.aspx&fieldid={0}&fieldids={0}\">@URL@LoanDetails/LoanDetails.aspx?FromPage=@URL@Pipeline/ProcessingPipelineSummary.aspx&fieldid={0}&fieldids={0}</a>','@URL@',LPCompanyURL) AS [retValue] from dbo.Company_Web");
            #endregion

        }

        #region Point Import Methods
        public List<PointFolderInfo> GetPDSPointFolders(ref string err)
        {
            using (SqlConnection con = new SqlConnection(PointCentralDBConnString))
            {
                con.Open();
                err = "";
                List<PointFolderInfo> folderList = null;
                string sqlCmd = "Select FolderName, FolderPath,iFolderID from Folder WHERE iEnabled=1";
                DataSet ds = new DataSet();
                SqlDataAdapter command = null;
                try
                {
                    using (command = new SqlDataAdapter(sqlCmd, con))
                    {
                        command.Fill(ds, "ds");
                    }

                    if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    {
                        err = "PDS DB String=" + PointCentralDBConnString;
                        err += "GetPDSPointFolders, no active folder record found in the Poitn Central database.";
                        return folderList;
                    }
                    folderList = new List<PointFolderInfo>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        PointFolderInfo f = new PointFolderInfo();
                        if (dr["FolderName"] == DBNull.Value)
                            f.Name = "";
                        else
                            f.Name = dr["FolderName"].ToString().Trim();

                        if (dr["iFolderID"] == DBNull.Value)
                            f.FolderId = 0;
                        else
                            f.FolderId = Convert.ToInt32(dr["iFolderID"]);

                        string path = "";
                        if (dr["FolderPath"] == DBNull.Value)
                            path = "";
                        else
                            path = dr["FolderPath"].ToString().Trim();
                        if (path != String.Empty)
                            path = Path.GetDirectoryName(path);
                        f.Path = path.Trim();
                        if ((f.Name == string.Empty) || (f.Path == string.Empty))
                            continue;
                        folderList.Add(f);
                    }
                    return folderList;
                }
                catch (Exception ex)
                {
                    err = "PDS DB String=" + PointCentralDBConnString;
                    err += " GetPDSPointFolders Exception: " + ex.Message;
                    Trace.TraceError(err);

                    return folderList;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                    if ((con != null) && (con.State != ConnectionState.Closed))
                        con.Close();
                }

            }
        }

        public int GetPDSPointFileId(string pointFileName, int pointFolderId, ref string err)
        {
            using (SqlConnection con = new SqlConnection(PointCentralDBConnString))
            {
                con.Open();
                err = "";
                bool v7 = true;
                //string sqlCmd = "select top 1 iFileID from dbo.[Files]";
                string sqlCmd = "Select OBJECT_ID(N'[dbo].[Files]', 'U')";
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sqlCmd, con))
                    {
                        object obj = cmd.ExecuteScalar();
                        v7 = (obj == DBNull.Value || obj == null) ? false : (bool)obj;
                    }
                }
                catch (Exception ex)
                {
                    v7 = false;
                }

                if (v7)
                    sqlCmd = string.Format("SELECT  [iFileID] FROM    [dbo].[Files] WHERE   [iFolderID] = {0}  AND [FileName] = '{1}'", pointFolderId, pointFileName);
                else
                    sqlCmd = string.Format("SELECT  [iFileID] FROM    [dbo].[Package_Files] WHERE   [iFolderID] = {0}  AND [FileName] = '{1}'", pointFolderId, pointFileName);

                DataSet ds = new DataSet();
                SqlDataAdapter command = null;
                try
                {
                    using (command = new SqlDataAdapter(sqlCmd, con))
                    {
                        command.Fill(ds, "ds");
                    }

                    if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    {
                        err = "PDS DB String=" + PointCentralDBConnString;
                        err += "GetPDSPointFileId, no point file record found in the Poitn Central database.";
                        return 0;
                    }

                    var query = ds.Tables[0].AsEnumerable().FirstOrDefault();
                    if (query != null)
                        return query.Field<int>("iFileID");
                    else
                        return 0;
                }
                catch (Exception ex)
                {
                    err = "PDS DB String=" + PointCentralDBConnString;
                    err += " GetPDSPointFileId Exception: " + ex.Message;
                    Trace.TraceError(err);

                    return 0;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                    if ((con != null) && (con.State != ConnectionState.Closed))
                        con.Close();
                }

            }
        }

        public int GetPDSPointFileUserNameById(int pointFileId, ref string userName, ref string err)
        {
            using (SqlConnection con = new SqlConnection(PointCentralDBConnString))
            {
                con.Open();
                err = "";
                bool v7 = true;
                //string sqlCmd = "select top 1 iFileID from dbo.[Files]";
                string sqlCmd = "Select OBJECT_ID(N'[dbo].[Files]', 'U')";
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sqlCmd, con))
                    {
                        object obj = cmd.ExecuteScalar();
                        v7 = (obj == DBNull.Value || obj == null) ? false : (bool)obj;
                    }
                }
                catch (Exception ex)
                {
                    v7 = false;
                }
                if (v7)
                    sqlCmd = string.Format(@"SELECT f.iFileID,p.vcFullName FROM dbo.Files f LEFT JOIN dbo.Reservation r ON f.iFileID = r.iFileID
                                                                LEFT JOIN dbo.PointUser p ON r.iUserID = p.iUserID
                                                                WHERE f.iFileID = {0}", pointFileId);
                else
                    sqlCmd = string.Format(@"SELECT f.iFileID,p.vcFullName FROM dbo.Package_Files f LEFT JOIN dbo.System_Reservation r ON f.iFileID = r.iFileID
                                                                LEFT JOIN dbo.[User] p ON r.iUserID = p.iUserID
                                                                WHERE f.iFileID = {0}", pointFileId);
                DataSet ds = new DataSet();
                SqlDataAdapter command = null;
                try
                {
                    using (command = new SqlDataAdapter(sqlCmd, con))
                    {
                        command.Fill(ds, "ds");
                    }

                    if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    {
                        err = "PDS DB String=" + PointCentralDBConnString;
                        err += "GetPDSPointFileUserNameById, no point file record found in the Poitn Central database.";
                        return 0;
                    }

                    var query = ds.Tables[0].AsEnumerable().FirstOrDefault();
                    if (query != null)
                    {
                        userName = query.Field<string>("vcFullName");
                        return query.Field<int>("iFileID");
                    }
                    else
                        return 0;
                }
                catch (Exception ex)
                {
                    err = "PDS DB String=" + PointCentralDBConnString;
                    err += " GetPDSPointFileUserNameById Exception: " + ex.Message;
                    Trace.TraceError(err);

                    return 0;
                }
                finally
                {
                    if (command != null)
                        command.Dispose();
                    if ((con != null) && (con.State != ConnectionState.Closed))
                        con.Close();
                }

            }
        }

        public List<Table.LoanPointField> GetLoanPointFieldList(int fileId, ref string err)
        {
            err = "";
            DataSet ds = null;
            List<Table.LoanPointField> loanPointFieldList = null;
            if (fileId <= 0)
            {
                err = "GetLoanPointFieldList, invalid FileId=" + fileId;
                return loanPointFieldList;
            }

            string sqlCmd = "SELECT PointFieldId, CurrentValue, PrevValue FROM LoanPointFields WHERE FileId=" + fileId + " ORDER BY PointFieldId ASC";
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "GetLoanPointFieldList, no record found in the PointFieldDesc table.";
                    return loanPointFieldList;
                }
                loanPointFieldList = new List<Table.LoanPointField>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[0] == DBNull.Value)
                        continue;
                    Table.LoanPointField pf = new Table.LoanPointField();
                    if (dr["PointFieldId"] == DBNull.Value)
                        continue;
                    pf.FieldId = (short)dr["PointFieldId"];
                    if (dr["CurrentValue"] == DBNull.Value)
                        pf.CurrValue = "";
                    else
                        pf.CurrValue = dr["CurrentValue"].ToString().Trim();
                    if (dr["PrevValue"] == DBNull.Value)
                        pf.PrevValue = "";
                    else
                        pf.PrevValue = dr["PrevValue"].ToString().Trim();
                    loanPointFieldList.Add(pf);
                }
                return loanPointFieldList;
            }
            catch (Exception e)
            {
                err = ", Exception:" + e.Message;
                Trace.TraceError(err);
                return loanPointFieldList;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public bool IfLoanPointFieldsExist(int fileId, ref string err)
        {
            err = "";
            DataSet ds = null;
            string sqlCmd = "Select Count(PointFieldId) FROM LoanPointFields Where FileId=" + fileId;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "IfLoanPointFieldsExist, no record found in the PointFiles table for FileId=" + fileId;
                    return false;
                }
                DataRow dr = ds.Tables[0].Rows[0];

                int numRec = 0;
                if (dr[0] == DBNull.Value)
                    return false;
                else
                    numRec = (int)dr[0];
                if (numRec <= 0)
                    return false;

                sqlCmd = "Select Count(CurrentImage) FROM PointFiles Where FileId=" + fileId;
                ds.Clear();

                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "IfLoanPointFieldsExist, no record found in the PointFiles table for FileId=" + fileId;
                    return false;
                }
                dr = ds.Tables[0].Rows[0];
                if (dr[0] == DBNull.Value)
                    return true;

                return false;
            }
            catch (Exception e)
            {
                err = ", Exception:" + e.Message;
                Trace.TraceError(err);
                return true;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }

        }

        public bool GetPointFileImages(int fileId, ref byte[] currentImage, ref byte[] previousImage, ref string err)
        {
            err = "";
            DataSet ds = null;
            string sqlCmd = "Select CurrentImage, PreviousImage From PointFiles Where FileId=" + fileId;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "GetPointFileImages, no record found in the PointFiles table for FileId=" + fileId;
                    return false;
                }
                DataRow dr = ds.Tables[0].Rows[0];

                if (dr["CurrentImage"] != DBNull.Value)
                {
                    currentImage = (byte[])dr["CurrentImage"];
                }
                else
                    return false;
                if (dr["PreviousImage"] != DBNull.Value)
                {
                    previousImage = (byte[])dr["PreviousImage"];
                }
                return true;
            }
            catch (Exception e)
            {
                err = ", Exception:" + e.Message;
                Trace.TraceError(err);
                return false;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public List<Table.PointFieldDesc> GetPointFieldDesc(ref string err)
        {
            err = "";
            List<Table.PointFieldDesc> pointFieldList = null;
            DataSet ds = null;
            try
            {
                string sqlCmd = "Select PointFieldId, DataType From PointFieldDesc Order by PointFieldId asc";
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "No record found in the PointFieldDesc table. The Point field labels are not set up.";
                    return pointFieldList;
                }
                pointFieldList = new List<Table.PointFieldDesc>();
                Table.PointFieldDesc pf = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["PointFieldId"] == DBNull.Value)
                        continue;
                    pf = new Table.PointFieldDesc();
                    pf.FieldId = Convert.ToInt16(dr["PointFieldId"].ToString());
                    if (dr["DataType"] == DBNull.Value)
                        pf.DataType = PointFieldDataType.StringType;
                    else
                        pf.DataType = (PointFieldDataType)((short)dr["DataType"]);
                    pointFieldList.Add(pf);
                }
                return pointFieldList;
            }
            catch (Exception ex)
            {
                err = "GetPointFieldDesc, Exception:" + ex.Message;
                return pointFieldList;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public bool Save_LoanPointFields(int fileId, short fieldId, string curValue, string prevValue, ref string err)
        {
            err = "";
            if (fileId <= 0)
            {
                err = "Save_LoanPointFields, invalid FileId=" + fileId;
                return false;
            }
            if ((fieldId <= 0) || (fieldId > 31000))
            {
                err = "Save_LoanPointFields, invalid Point FieldId=" + fieldId;
                return false;
            }
            if (curValue != null)
                curValue = curValue.Trim();
            if (prevValue != null)
                prevValue = prevValue.Trim();

            if ((curValue == null) || (curValue == String.Empty))
                if ((prevValue == null) || (prevValue == String.Empty))
                    return true;

            //if ((curValue != String.Empty) && (prevValue != String.Empty) &&
            //    (curValue != null) && (prevValue != null) &&
            //    (curValue.ToLower() == prevValue.ToLower()))
            //    return true;

            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@FileId", SqlDbType.Int, 4),                                         //0
                    new SqlParameter("@PointFieldId", SqlDbType.SmallInt),                         //1  
                    new SqlParameter("@CurrentValue", SqlDbType.NVarChar, 500),            //2         
                    new SqlParameter("@PreviousValue", SqlDbType.NVarChar, 500)
               };

                parameters[0].Value = fileId;
                parameters[1].Value = fieldId;
                if (curValue == String.Empty)
                    parameters[2].Value = DBNull.Value;
                else
                    parameters[2].Value = curValue;
                if (prevValue == String.Empty)
                    parameters[3].Value = DBNull.Value;
                else
                    parameters[3].Value = prevValue;
                int rows = 0;
                DbHelperSQL.RunProcedure("Save_LoanPointFields", parameters, out rows);
                return true;
            }
            catch (Exception e)
            {
                err = "Failed to update PointFieldDesc due to database error:" + e.Message;
                Trace.TraceError(err);
                return false;
            }
        }

        public int Save_PointFieldDesc(int PointFieldId, string Label, int DataType, ref string err)
        {
            err = "";

            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@PointFieldId", SqlDbType.Int, 4),                                 //0
                    new SqlParameter("@Label", SqlDbType.NVarChar, 500),                       //1  
                    new SqlParameter("@DataType", SqlDbType.SmallInt, 1 )                                    //2         
                  };

                parameters[0].Value = PointFieldId;
                parameters[1].Value = Label;
                parameters[2].Value = DataType;

                int rows = 0;
                PointFieldId =
                    DbHelperSQL.RunProcedure("PointFieldDesc_Save", parameters, out rows);
                if (PointFieldId > 0)
                {
                    return PointFieldId;
                }

                return -1;
            }
            catch (Exception e)
            {
                err = "Failed to update PointFieldDesc due to database error:" + e.Message;
                Trace.TraceError(err);
                return -1;
            }

        }
        public int Save_ServiceStatus(string ServiceName, bool Running, ref string err)
        {
            int Status = 0;
            DateTime dt = DateTime.Now;

            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@ServiceName", SqlDbType.NVarChar, 50),                  //0                   
                    new SqlParameter("@Running", SqlDbType.Bit , 1 ),                        //1                    
                  };

                parameters[0].Value = ServiceName;
                parameters[1].Value = Running;

                int rows = 0;
                Status =
                    DbHelperSQL.RunProcedure("ServiceStatus_Save", parameters, out rows);
                if (Status >= 0)
                {
                    string outcontactId = Status.ToString();
                    return Status;
                }

                return -1;
            }
            catch (Exception e)
            {
                err = "Failed to save ServiceStatus due to database error:" + e.Message;
                Trace.TraceError(err);
                return -1;
            }
        }

        public bool Save_PointFolderImportStats(int folderId, int processed, ref string err)
        {
            err = "";
            string sqlCmd = "Update dbo.PointFolders Set ImportCount=" + processed + ", LastImport=GetDate() WHERE folderId=" + folderId;
            try
            {
                int rows = 0;
                rows = DbHelperSQL.ExecuteSql(sqlCmd);
                return true;
            }
            catch (Exception ex)
            {
                err = "Failed to update PointFolder stats. Exception:" + ex.Message;
                Trace.TraceError(err);
                return false;
            }
        }

        public int Save_PointImportHistory(int FileId, string severity, string errMsg, ref string err)
        {
            int HistoryId = 0;
            DateTime dt = DateTime.Now;
            err = "";

            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@HistoryId", SqlDbType.Int, 4),                                 //0
                    new SqlParameter("@FileId", SqlDbType.Int, 4 ),                                     //1
                    new SqlParameter("@ImportTime", SqlDbType.DateTime , 1),               //2                 
                    new SqlParameter("@Success", SqlDbType.Bit , 1 ),                               //3
                    new SqlParameter("@Error", SqlDbType.NVarChar, 500),                       //4  
                    new SqlParameter("@Severity", SqlDbType.NVarChar, 50)
                  };

                parameters[0].Value = DBNull.Value;
                parameters[0].Direction = ParameterDirection.Output;
                parameters[1].Value = FileId;
                parameters[2].Value = dt;
                parameters[3].Value = true;
                parameters[4].Value = errMsg;
                parameters[5].Value = severity;

                int rows = 0;
                HistoryId =
                    DbHelperSQL.RunProcedure("PointImportHistory_Save", parameters, out rows);
                if (HistoryId > 0)
                {
                    string outcontactId = HistoryId.ToString();
                    return HistoryId;
                }

                return -1;
            }
            catch (Exception e)
            {
                err = "Failed to update PointImportHistory due to database error:" + e.Message;
                Trace.TraceError(err);
                return -1;
            }

        }

        public bool Save_UserLoanRep(string loanRep, int branchId, ref string err)
        {
            int NameId = 0;

            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@NameId", SqlDbType.Int, 4),                  //0
                    new SqlParameter("@BranchId", SqlDbType.Int, 4 ),                   //1
                    new SqlParameter("@Name", SqlDbType.NVarChar, 255),               //2                 
                    new SqlParameter("@UserId", SqlDbType.Int, 4 )               
                  };

                parameters[0].Value = DBNull.Value;
                parameters[0].Direction = ParameterDirection.Output;
                parameters[1].Value = branchId;
                parameters[2].Value = loanRep;
                parameters[3].Value = DBNull.Value;

                int rows = 0;
                NameId =
                    DbHelperSQL.RunProcedure("UserLoanRep_Save", parameters, out rows);
                if (NameId > 0)
                {
                    string outcontactId = NameId.ToString();
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                err = "Failed to update UserLoanRep due to database error. Exception:" + e.Message;
                Trace.TraceError(err);
                return false;
            }

        }

        public int Save_LoanStages(int fileId, Table.LoanStages stage, ref string err)
        {
            int LoanStageId = 0;
            if (stage == null)
            {
                err = "Save_LoanStages: LoanStages cannot be null.";
                return -1;
            }
            if (fileId <= 0)
            {
                err = "Save_LoanStages: Invalid FileId, " + fileId + " specified.";
                return -1;
            }
            if (stage.StageName == null)
            {
                err = "Save_LoanStages:: Stage name cannot be null.";
                return -1;
            }
            stage.StageName = stage.StageName.Trim();
            if (stage.StageName == string.Empty)
            {
                err = "Save_LoanStages:: Stage name cannot be empty.";
                return -1;
            }
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@LoanStageId", SqlDbType.Int, 4),                                       //0
                    new SqlParameter("@Completed", SqlDbType.DateTime ),                                  //1
                    new SqlParameter("@SequenceNumber", SqlDbType.SmallInt , 1),                   //2
                    new SqlParameter("@FileId", SqlDbType.Int, 4 ),                                                 //3
                    new SqlParameter("@WflTemplId", SqlDbType.Int, 4 ),                                      //4       
                    new SqlParameter("@WflStageId", SqlDbType.Int, 4 ) ,                                      //5
                    new SqlParameter("@DaysFromEstClose", SqlDbType.SmallInt),                     //6
                    new SqlParameter("@DaysFromCreation", SqlDbType.SmallInt),                     //7
                    new SqlParameter("@StageName", SqlDbType.NVarChar, 120),                        //8
                    new SqlParameter("@CalculationMethod", SqlDbType.SmallInt),                     //9
                    new SqlParameter("@DaysDueAfterPrevStage", SqlDbType.SmallInt)               //10
                  };

                if (stage.LoanStageId > 0)
                    parameters[0].Value = stage.LoanStageId;
                else
                    parameters[0].Value = DBNull.Value;
                //parameters[0].Direction = ParameterDirection.Output;
                if ((stage.Completed == null) || (stage.Completed == DateTime.MinValue))
                    parameters[1].Value = DBNull.Value;
                else
                    parameters[1].Value = stage.Completed;

                if (stage.SequenceNumber > 0)
                    parameters[2].Value = stage.SequenceNumber;
                else
                    parameters[2].Value = DBNull.Value;

                parameters[3].Value = fileId;

                if (stage.WflTemplId > 0)
                    parameters[4].Value = stage.WflTemplId;
                else
                    parameters[4].Value = DBNull.Value;

                if (stage.WflStageId > 0)
                    parameters[5].Value = stage.WflStageId;
                else
                    parameters[5].Value = DBNull.Value;

                if (stage.DaysFromEstClose < -360)
                    parameters[6].Value = DBNull.Value;
                else
                    parameters[6].Value = stage.DaysFromEstClose;
                if (stage.DaysFromCreation < -360)
                    parameters[7].Value = DBNull.Value;
                else
                    parameters[7].Value = stage.DaysFromCreation;
                parameters[8].Value = stage.StageName;

                parameters[9].Value = stage.CalculationMethod <= 0 ? 1 : stage.CalculationMethod;
                parameters[10].Value = DBNull.Value;

                int rows = 0;
                LoanStageId =
                    DbHelperSQL.RunProcedure("LoanStages_Save", parameters, out rows);
                if (LoanStageId > 0)
                {
                    string outcontactId = LoanStageId.ToString();
                    return LoanStageId;
                }

                return -1;
            }
            catch (Exception e)
            {
                err = "Failed to update LoanStages due to database error:" + e.Message;
                Trace.TraceError(err);
                return -1;
            }
        }

        public int Find_User(int branchId, string name, ref string err)
        {
            int userId = -1;
            string sqlCmd = "Select TOP 1 UserId from lpvw_GetUserGroups WHERE [Name]=@Fullname";
            if (branchId > 0)
            {
                sqlCmd += " AND BranchId=" + branchId;
            }
            DataSet ds = null;
            try
            {
                SqlCommand cmd = new SqlCommand(sqlCmd);
                cmd.Parameters.Add(new SqlParameter("@Fullname", SqlDbType.NVarChar));
                cmd.Parameters[0].Value = name.Trim();
                object obj = DbHelperSQL.ExecuteScalar(cmd);
                userId = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
                return userId;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return userId;
            }
        }

        public string GetLoanTeamMemberByRoleName(int FileId, string roleName, ref string err)
        {
            err = "";
            string userInfo = string.Empty;
            bool logErr = false;
            string sqlStr =
                "select u.[FirstName]+' '+u.[LastName] as Fullname from LoanTeam l inner join Roles r on l.RoleId=r.RoleId inner join users u on l.UserId=u.UserId " +
                " where l.FileId=@FileId and r.[Name]=@Rolename";
            try
            {
                SqlCommand cmd = new SqlCommand(sqlStr);
                cmd.Parameters.Add(new SqlParameter("@FileId", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@Rolename", SqlDbType.NVarChar));
                cmd.Parameters[0].Value = FileId;
                cmd.Parameters[1].Value = roleName;
                object obj = DbHelperSQL.ExecuteScalar(cmd);
                userInfo = obj == null ? String.Empty : obj.ToString();
                return userInfo;
            }
            catch (Exception ex)
            {
                err = "GetLoanTeamMemberByRoleName, Exception: " + ex.Message;
                logErr = true;
                return userInfo;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }

        public int GetContactIdByName(string firstname, string lastname, ref string err)
        {
            err = "";
            int ContactId = 0;
            bool logErr = false;
            string sqlCmd = "select top 1 ContactId from dbo.Contacts " +
                             string.Format(" where [FirstName]=@Firstname and [LastName]=@Lastname", firstname, lastname);
            try
            {
                SqlCommand cmd = new SqlCommand(sqlCmd);
                cmd.Parameters.Add(new SqlParameter("@Firstname", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@Lastname", SqlDbType.NVarChar));
                cmd.Parameters[0].Value = firstname;
                cmd.Parameters[1].Value = lastname;
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                ContactId = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
                return ContactId;
            }
            catch (Exception ex)
            {
                err = "GetContactIdByName, Exception: " + ex.Message;
                return ContactId;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }

        public bool NeedDefaultStages(int FileId)
        {
            bool need = false;
            if (FileId <= 0)
                return need;
            try
            {
                object ob = DbHelperSQL.GetSingle("Select count(1) from LoanStages where FileId=" + FileId);
                need = (ob == null || ob == DBNull.Value || (int)ob == 0) ? true : false;
                return need;
            }
            catch (Exception ex)
            {
                string err = string.Format("NeedDefaultStages, FileId={0}, Exception: {1}", FileId, ex.Message);
                Trace.TraceError(err);
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                return need;
            }
        }
        public Table.LoanStages GetWflStageNameByLoanStageId(int FileId, int LoanStageId, ref string err)
        {
            bool logErr = false;
            err = string.Empty;
            Table.LoanStages ls = null;
            DataSet ds = null;
            string sqlCmd = string.Format("select ls.StageName as StageName, ws.Name as Wfl_StageName, ts.Name as TemplStageName, ts.PointStageDateField, ts.PointStageNameField from LoanStages ls inner join  Template_Wfl_Stages ws on ls.WflStageId=ws.WflStageId inner join Template_Stages ts on ws.TemplStageId=ts.TemplStageId where ls.LoanStageId={0} and FileId={1}", LoanStageId, FileId);
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    sqlCmd = string.Format("select ls.StageName as StageName, ts.Name as Wfl_StageName, ts.Name as TemplStageName, ts.PointStageDateField, ts.PointStageNameField from LoanStages ls inner join Template_Stages ts on ts.Alias=ls.StageName where ls.LoanStageId={0} and FileId={1}", LoanStageId, FileId);
                    ds = DbHelperSQL.Query(sqlCmd);
                    if (ds == null || ds.Tables[0].Rows.Count <= 0)
                        return ls;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr == null)
                    return ls;
                // if none of the Loan StageName, Workflow Stage Name or Template Alias is available, there is a problem.
                if ((dr["StageName"] == null || dr["StageName"] == DBNull.Value) &&
                     (dr["Wfl_StageName"] == null || dr["Wfl_StageName"] == DBNull.Value) &&
                     (dr["TemplStageName"] == null || dr["TemplStageName"] == DBNull.Value))
                {
                    err = string.Format("Cannot get a valid StageName, Wfl_StageName and TemplStageName using SqlCmd: {0}, Exception:{1}", sqlCmd);
                    return ls;
                }

                string StageName = dr["TemplStageName"] == DBNull.Value ? string.Empty : (string)dr["TemplStageName"];
                if (string.IsNullOrEmpty(StageName))
                {
                    StageName = dr["Wfl_StageName"] == DBNull.Value ? string.Empty : (string)dr["Wfl_StageName"];
                }
                if (string.IsNullOrEmpty(StageName))
                {
                    StageName = dr["StageName"] == DBNull.Value ? string.Empty : (string)dr["StageName"];
                }
                if (string.IsNullOrEmpty(StageName))
                {
                    err = string.Format("Cannot get a valid StageName, Wfl_StageName and TemplStageName using SqlCmd: {0}, Exception:{1}", sqlCmd);
                    return ls;
                }
                ls = new Table.LoanStages();
                ls.StageName = StageName;
                ls.PointDateField = 0;
                ls.PointNameField = 0;
                string temp = dr["PointStageDateField"] == DBNull.Value ? "0" : dr["PointStageDateField"].ToString();
                short.TryParse(temp, out ls.PointDateField);
                temp = dr["PointStageNameField"] == DBNull.Value ? "0" : dr["PointStageNameField"].ToString();
                short.TryParse(temp, out ls.PointNameField);
                ls.LoanStageId = LoanStageId;
                ls.FileId = FileId;
                return ls;
            }
            catch (Exception ex)
            {
                err = string.Format("Failed to fetch record using SqlCmd: {0}, Exception:{1}", sqlCmd, ex.Message);
                logErr = true;
                return ls;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }


        public bool GetDefaultStageName(ref List<Table.DefaultStage> DefaultStage, ref string err)
        {
            bool logErr = false;
            err = "";

            DataSet ds = null;
            if (DefaultStage == null)
                DefaultStage = new List<Table.DefaultStage>();
            DefaultStage.Clear();
            string SQLDefault = "select * from Template_Stages where WorkflowType='Processing' AND Enabled=1 order by SequenceNumber asc";
            string SQLString = "select ws.*, ts.Alias, ts.PointStageDateField, ts.PointStageNameField from Template_Wfl_Stages ws inner join Template_Stages ts on ws.TemplStageId=ts.TemplStageId inner join Template_Workflow tw on ws.WflTemplId=tw.WflTemplId where tw.WorkflowType='Processing' AND tw.[Default]=1 AND ws.[Enabled]=1 order by SequenceNumber asc";

            try
            {
                ds = DbHelperSQL.Query(SQLString);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    ds = DbHelperSQL.Query(SQLDefault);
                    if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    {
                        return false;
                    }
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DefaultStage.Add(new Table.DefaultStage()
                        {
                            WflStageId = -1,
                            WflTemplId = -1,
                            Name = dr["Alias"] == DBNull.Value ? String.Empty : dr["Alias"].ToString(),
                            SequenceNumber = (short)(dr["SequenceNumber"] == DBNull.Value ? -1 : (short)dr["SequenceNumber"]),
                            DaysAfterCreation = (short)(dr["DaysFromCreation"] == DBNull.Value ? -365 : (short)dr["DaysFromCreation"]),
                            DaysFromEstClose = (short)(dr["DaysFromEstClose"] == DBNull.Value ? -365 : (short)dr["DaysFromEstClose"]),
                            StageDateFld = (short)(dr["PointStageDateField"] == DBNull.Value ? 0 : (short)dr["PointStageDateField"]),
                            StageNameFld = (short)(dr["PointStageNameField"] == DBNull.Value ? 0 : (short)dr["PointStageNameField"])
                        });
                    }
                    return true;
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DefaultStage.Add(new Table.DefaultStage()
                    {
                        WflStageId = dr["WflStageId"] == DBNull.Value ? -1 : (int)dr["WflStageId"],
                        WflTemplId = dr["WflTemplId"] == DBNull.Value ? -1 : (int)dr["WflTemplId"],
                        Name = dr["Alias"] == DBNull.Value ? String.Empty : dr["Alias"].ToString(),
                        SequenceNumber = (short)(dr["SequenceNumber"] == DBNull.Value ? -1 : (short)dr["SequenceNumber"]),
                        DaysAfterCreation = (short)(dr["DaysFromCreation"] == DBNull.Value ? -365 : (short)dr["DaysFromCreation"]),
                        DaysFromEstClose = (short)(dr["DaysFromEstClose"] == DBNull.Value ? -365 : (short)dr["DaysFromEstClose"]),
                        StageDateFld = (short)(dr["PointStageDateField"] == DBNull.Value ? 0 : (short)dr["PointStageDateField"]),
                        StageNameFld = (short)(dr["PointStageNameField"] == DBNull.Value ? 0 : (short)dr["PointStageNameField"])
                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in Template_Workflow, Exception:" + ex.Message;
                logErr = true;
                return false;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }

        }
        public Record.Contacts GetLoanContactInfo(int FileId, string role, ref string err)
        {
            Record.Contacts contact = null;
            if (FileId <= 0 || string.IsNullOrEmpty(role))
            {
                err = string.Format("GetLoanContactInfo: Invalid FileId={0} or ContactRole={1}.", FileId, role);
                return contact;
            }
            try
            {
                string sqlCmd = string.Format("select * from lpvw_GetLoanContactInfowRoles where FileId={0} AND RoleName='{1}'", FileId, role);
                DataSet ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                {
                    err = string.Format("Unable to find Loan Contact record for FileId={0} for ContactRole={1}.", FileId, role);
                    return contact;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                contact = new Record.Contacts()
                {
                    FirstName = dr["FirstName"] == DBNull.Value ? string.Empty : dr["FirstName"].ToString(),
                    MiddleName = dr["MiddleName"] == DBNull.Value ? string.Empty : dr["MiddleName"].ToString(),
                    LastName = dr["LastName"] == DBNull.Value ? string.Empty : dr["LastName"].ToString(),
                    NickName = dr["NickName"] == DBNull.Value ? string.Empty : dr["NickName"].ToString(),
                    Title = dr["Title"] == DBNull.Value ? string.Empty : dr["Title"].ToString(),
                    GenerationCode = dr["GenerationCode"] == DBNull.Value ? string.Empty : dr["GenerationCode"].ToString(),
                    SSN = dr["SSN"] == DBNull.Value ? string.Empty : dr["SSN"].ToString(),
                    HomePhone = dr["HomePhone"] == DBNull.Value ? string.Empty : dr["HomePhone"].ToString(),
                    CellPhone = dr["CellPhone"] == DBNull.Value ? string.Empty : dr["CellPhone"].ToString(),
                    BusinessPhone = dr["BusinessPhone"] == DBNull.Value ? string.Empty : dr["BusinessPhone"].ToString(),
                    Fax = dr["Fax"] == DBNull.Value ? string.Empty : dr["Fax"].ToString(),
                    Email = dr["Email"] == DBNull.Value ? string.Empty : dr["Email"].ToString(),
                    DOB = dr["DOB"] == DBNull.Value ? string.Empty : dr["DOB"].ToString(),
                    Experian = dr["Experian"] == DBNull.Value ? string.Empty : dr["Experian"].ToString(),
                    TransUnion = dr["TransUnion"] == DBNull.Value ? string.Empty : dr["TransUnion"].ToString(),
                    Equifax = dr["Equifax"] == DBNull.Value ? string.Empty : dr["Equifax"].ToString(),
                    MailingAddr = dr["MailingAddr"] == DBNull.Value ? string.Empty : dr["MailingAddr"].ToString(),
                    MailingCity = dr["MailingCity"] == DBNull.Value ? string.Empty : dr["MailingCity"].ToString(),
                    MailingState = dr["MailingState"] == DBNull.Value ? string.Empty : dr["MailingState"].ToString(),
                    MailingZip = dr["MailingZip"] == DBNull.Value ? string.Empty : dr["MailingZip"].ToString(),
                    CompanyName = dr["CompanyName"] == DBNull.Value ? string.Empty : dr["CompanyName"].ToString()
                };
            }
            catch (Exception ex)
            {
                err = string.Format("Failed to get Loans record for FileId={0} due to db error:{1}", FileId, ex.Message);
            }
            return contact;
        }
        public Table.LoanTeam GetLoanTeamInfo(int FileId, ref string err)
        {
            Table.LoanTeam team = null;
            if (FileId <= 0)
            {
                err = "GetLoanTeamInfo: Invalid FileId=" + FileId;
                return team;
            }
            try
            {
                string sqlCmd = string.Format("select r.[Name] +';'+ d.[Name] +';' +b.[Name] as Org from lpvw_PipelineInfo v " +
                                               " inner join Regions r on v.RegionID=r.RegionID " +
                                               " inner join Divisions d on v.DivisionID=d.DivisionID " +
                                               " inner join Branches b on v.BranchID=b.BranchId " +
                                                " where FileId={0}", FileId);
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                string tempOrg = obj == null ? ";;" : obj.ToString();
                string[] org = tempOrg.Split(';');
                team = new Table.LoanTeam();
                if (org.Length < 3)
                {
                    team.Region = string.Empty;
                    team.Division = string.Empty;
                    team.Branch = string.Empty;
                }
                else
                {
                    team.Region = org[0];
                    team.Division = org[1];
                    team.Branch = org[2];
                }
                sqlCmd = string.Format("select * from lpvw_GetLoanContacts where FileId={0} and ContactId like 'User%'", FileId);
                DataSet ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                {
                    err = string.Format("Unable to find Loan team record for FileId={0}.", FileId);
                    return team;
                }

                Table.Users user = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["ContactRole"] == DBNull.Value
                        || dr["Firstname"] == DBNull.Value || dr["LastName"] == DBNull.Value)
                        continue;
                    string role = dr["ContactRole"].ToString().Trim().ToUpper();
                    user = new Table.Users()
                    {
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        Phone = dr["BusinessPhone"] == DBNull.Value ? string.Empty : dr["BusinessPhone"].ToString(),
                        Cell = dr["CellPhone"] == DBNull.Value ? string.Empty : dr["CellPhone"].ToString(),
                        Fax = dr["Fax"] == DBNull.Value ? string.Empty : dr["Fax"].ToString(),
                        EmailAddress = dr["Email"] == DBNull.Value ? string.Empty : dr["Email"].ToString()
                    };
                    string name = dr["FirstName"].ToString() + " " + dr["LastName"].ToString();
                    switch (role)
                    {
                        case "LOAN OFFICER": team.LoanOfficer = user;
                            break;
                        case "PROCESSOR": team.Processor = user;
                            break;
                        case "CLOSER": team.Closer = user;
                            break;
                        case "DOC PREP": team.DocPrep = user;
                            break;
                        case "SHIPPER": team.Shipper = user;
                            break;
                        case "UNDERWRITER": team.Underwriter = user;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                err = string.Format("Failed to get Loan Team record for FileId={0} due to db error:{1}", FileId, ex.Message);
            }
            return team;
        }
        public Table.Loans GetTableLoanInfo(int FileId, ref string err)
        {
            Table.Loans loan = null;
            if (FileId <= 0)
            {
                err = "GetLoanInfo: Invalid FileId=" + FileId;
                return loan;
            }
            DataSet ds = null;
            try
            {
                string sqlCmd = "select * from Loans where FileId=" + FileId;
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                {
                    err = string.Format("Unable to find Loans record for FileId={0}.", FileId);
                    return loan;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                loan = new Table.Loans()
                {
                    AppraisedValue = dr["AppraisedValue"] == DBNull.Value ? 0 : Convert.ToInt64(dr["AppraisedValue"].ToString()),
                    CCScenario = dr["CCScenario"] == DBNull.Value ? string.Empty : dr["CCScenario"].ToString(),
                    CLTV = dr["CLTV"] == DBNull.Value ? 0 : Convert.ToInt64(dr["CLTV"].ToString()),
                    County = dr["County"] == DBNull.Value ? string.Empty : dr["County"].ToString(),
                    LoanStatus = dr["Status"] == DBNull.Value ? string.Empty : dr["Status"].ToString(),
                    DateOpen = dr["DateOpen"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DateOpen"],
                    DateSubmit = dr["DateSubmit"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DateSubmit"],
                    DateApprove = dr["DateApprove"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DateApprove"],
                    DateClearToClose = dr["DateClearToClose"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DateClearToClose"],
                    DateDocs = dr["DateDocs"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DateDocs"],
                    DateHMDA = dr["DateHMDA"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DateHMDA"],
                    DateFund = dr["DateFund"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DateFund"],
                    DateRecord = dr["DateRecord"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DateRecord"],
                    DateClose = dr["DateClose"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DateClose"],
                    DateDenied = dr["DateDenied"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DateDenied"],
                    DateCanceled = dr["DateCanceled"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DateCanceled"],
                    DateReSubmit = dr["DateReSubmit"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DateReSubmit"],
                    DateDocsOut = dr["DateDocsOut"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DateDocsOut"],
                    DateProcessing = dr["DateProcessing"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DateProcessing"],
                    DateDocsReceived = dr["DateDocsReceived"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DateDocsReceived"],
                    DateSuspended = dr["DateSuspended"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DateSuspended"],
                    DownPay = dr["DownPay"] == DBNull.Value ? 0 : Convert.ToInt64(dr["DownPay"].ToString()),
                    EstCloseDate = dr["EstCloseDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["EstCloseDate"],
                    LienPosition = dr["LienPosition"] == DBNull.Value ? string.Empty : dr["LienPosition"].ToString(),
                    LoanAmount = dr["LoanAmount"] == DBNull.Value ? 0 : Convert.ToInt64(dr["LoanAmount"].ToString()),
                    LoanNumber = dr["LoanNumber"] == DBNull.Value ? string.Empty : dr["LoanNumber"].ToString(),
                    LoanType = dr["LoanType"] == DBNull.Value ? string.Empty : dr["LoanType"].ToString(),
                    LTV = dr["LTV"] == DBNull.Value ? 0 : Convert.ToInt64(dr["LTV"].ToString()),
                    LenderNotes = dr["LenderNotes"] == DBNull.Value ? string.Empty : dr["LenderNotes"].ToString(),
                    MonthlyPayment = dr["MonthlyPayment"] == DBNull.Value ? 0 : Convert.ToInt64(dr["MonthlyPayment"].ToString()),
                    Occupancy = dr["Occupancy"] == DBNull.Value ? string.Empty : dr["Occupancy"].ToString(),
                    Program = dr["Program"] == DBNull.Value ? string.Empty : dr["Program"].ToString(),
                    PropertyAddr = dr["PropertyAddr"] == DBNull.Value ? string.Empty : dr["PropertyAddr"].ToString(),
                    PropertyCity = dr["PropertyCity"] == DBNull.Value ? string.Empty : dr["PropertyCity"].ToString(),
                    PropertyState = dr["PropertyState"] == DBNull.Value ? string.Empty : dr["PropertyState"].ToString(),
                    PropertyZip = dr["PropertyZip"] == DBNull.Value ? string.Empty : dr["PropertyZip"].ToString(),
                    Purpose = dr["Purpose"] == DBNull.Value ? string.Empty : dr["Purpose"].ToString(),
                    Rate = dr["Rate"] == DBNull.Value ? 0 : Convert.ToInt64(dr["Rate"].ToString()),
                    RateLockExpiration = dr["RateLockExpiration"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["RateLockExpiration"],
                    SalesPrice = dr["SalesPrice"] == DBNull.Value ? 0 : Convert.ToInt64(dr["SalesPrice"].ToString()),
                    Term = dr["Term"] == DBNull.Value ? 0 : Convert.ToInt64(dr["Term"].ToString()),
                    Due = dr["Due"] == DBNull.Value ? 0 : Convert.ToInt64(dr["Due"].ToString()),
                    LeadRanking = (dr["Ranking"] == DBNull.Value) ? string.Empty : (string)dr["Ranking"]
                };
            }
            catch (Exception ex)
            {
                err = string.Format("Failed to get Loans record for FileId={0} due to db error:{1}", FileId, ex.Message);
            }
            return loan;
        }

        public Record.Loans GetLoanInfo(int FileId, ref string err)
        {
            Record.Loans loan = null;
            if (FileId <= 0)
            {
                err = "GetLoanInfo: Invalid FileId=" + FileId;
                return loan;
            }
            DataSet ds = null;
            try
            {
                string sqlCmd = "select * from Loans where FileId=" + FileId;
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                {
                    err = string.Format("Unable to find Loans record for FileId={0}.", FileId);
                    return loan;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                loan = new Record.Loans()
                {
                    AppraisedValue = dr["AppraisedValue"] == DBNull.Value ? string.Empty : dr["AppraisedValue"].ToString(),
                    CCScenario = dr["CCScenario"] == DBNull.Value ? string.Empty : dr["CCScenario"].ToString(),
                    CLTV = dr["CLTV"] == DBNull.Value ? string.Empty : dr["CLTV"].ToString(),
                    County = dr["County"] == DBNull.Value ? string.Empty : dr["County"].ToString(),
                    DateCreated = dr["Created"] == DBNull.Value ? string.Empty : dr["Created"].ToString(),
                    DateOpen = dr["DateOpen"] == DBNull.Value ? string.Empty : dr["DateOpen"].ToString(),
                    DateSubmit = dr["DateSubmit"] == DBNull.Value ? string.Empty : dr["DateSubmit"].ToString(),
                    DateApprove = dr["DateApprove"] == DBNull.Value ? string.Empty : dr["DateApprove"].ToString(),
                    DateClearToClose = dr["DateClearToClose"] == DBNull.Value ? string.Empty : dr["DateClearToClose"].ToString(),
                    DateDocs = dr["DateDocs"] == DBNull.Value ? string.Empty : dr["DateDocs"].ToString(),
                    DateHMDA = dr["DateHMDA"] == DBNull.Value ? string.Empty : dr["DateHMDA"].ToString(),
                    DateFund = dr["DateFund"] == DBNull.Value ? string.Empty : dr["DateFund"].ToString(),
                    DateRecord = dr["DateRecord"] == DBNull.Value ? string.Empty : dr["DateRecord"].ToString(),
                    DateClose = dr["DateClose"] == DBNull.Value ? string.Empty : dr["DateClose"].ToString(),
                    DateDenied = dr["DateDenied"] == DBNull.Value ? string.Empty : dr["DateDenied"].ToString(),
                    DateCanceled = dr["DateCanceled"] == DBNull.Value ? string.Empty : dr["DateCanceled"].ToString(),
                    DateReSubmit = dr["DateReSubmit"] == DBNull.Value ? string.Empty : dr["DateReSubmit"].ToString(),
                    DateDocsOut = dr["DateDocsOut"] == DBNull.Value ? string.Empty : dr["DateDocsOut"].ToString(),
                    DateProcessing = dr["DateProcessing"] == DBNull.Value ? string.Empty : dr["DateProcessing"].ToString(),
                    DateDocsReceived = dr["DateDocsReceived"] == DBNull.Value ? string.Empty : dr["DateDocsReceived"].ToString(),
                    DateSuspended = dr["DateSuspended"] == DBNull.Value ? string.Empty : dr["DateSuspended"].ToString(),
                    DownPay = dr["DownPay"] == DBNull.Value ? string.Empty : dr["DownPay"].ToString(),
                    EstCloseDate = dr["EstCloseDate"] == DBNull.Value ? string.Empty : dr["EstCloseDate"].ToString(),
                    LienPosition = dr["LienPosition"] == DBNull.Value ? string.Empty : dr["LienPosition"].ToString(),
                    LoanAmount = dr["LoanAmount"] == DBNull.Value ? string.Empty : dr["LoanAmount"].ToString(),
                    LoanNumber = dr["LoanNumber"] == DBNull.Value ? string.Empty : dr["LoanNumber"].ToString(),
                    LoanType = dr["LoanType"] == DBNull.Value ? string.Empty : dr["LoanType"].ToString(),
                    LTV = dr["LTV"] == DBNull.Value ? string.Empty : dr["LTV"].ToString(),
                    LenderNotes = dr["LenderNotes"] == DBNull.Value ? string.Empty : dr["LenderNotes"].ToString(),
                    MonthlyPayment = dr["MonthlyPayment"] == DBNull.Value ? string.Empty : dr["MonthlyPayment"].ToString(),
                    Occupancy = dr["Occupancy"] == DBNull.Value ? string.Empty : dr["Occupancy"].ToString(),
                    Program = dr["Program"] == DBNull.Value ? string.Empty : dr["Program"].ToString(),
                    PropertyAddr = dr["PropertyAddr"] == DBNull.Value ? string.Empty : dr["PropertyAddr"].ToString(),
                    PropertyCity = dr["PropertyCity"] == DBNull.Value ? string.Empty : dr["PropertyCity"].ToString(),
                    PropertyState = dr["PropertyState"] == DBNull.Value ? string.Empty : dr["PropertyState"].ToString(),
                    PropertyZip = dr["PropertyZip"] == DBNull.Value ? string.Empty : dr["PropertyZip"].ToString(),
                    Purpose = dr["Purpose"] == DBNull.Value ? string.Empty : dr["Purpose"].ToString(),
                    Rate = dr["Rate"] == DBNull.Value ? string.Empty : dr["Rate"].ToString(),
                    RateLockExpiration = dr["RateLockExpiration"] == DBNull.Value ? string.Empty : dr["RateLockExpiration"].ToString(),
                    SalesPrice = dr["SalesPrice"] == DBNull.Value ? string.Empty : dr["SalesPrice"].ToString(),
                    Term = dr["Term"] == DBNull.Value ? string.Empty : dr["Term"].ToString(),
                    Due = dr["Due"] == DBNull.Value ? string.Empty : dr["Due"].ToString(),
                    InterestOnly = (dr["InterestOnly"] == DBNull.Value || (bool)dr["InterestOnly"] == false) ? "0" : "1",
                    IncludeEscrow = (dr["IncludeEscrows"] == DBNull.Value || (bool)dr["IncludeEscrows"] == false) ? "0" : "1",
                    RentAmount = dr["RentAmount"] == DBNull.Value ? string.Empty : dr["RentAmount"].ToString(),
                    HousingStatus = dr["HousingStatus"] == DBNull.Value ? string.Empty : dr["HousingStatus"].ToString(),
                    Joint = (dr["Joint"] == DBNull.Value || (bool)dr["Joint"] == false) ? "0" : "1",
                    LeadRanking = (dr["Ranking"] == DBNull.Value) ? string.Empty : (string)dr["Ranking"]
                };
            }
            catch (Exception ex)
            {
                err = string.Format("Failed to get Loans record for FileId={0} due to db error:{1}", FileId, ex.Message);
            }
            return loan;
        }

        public int GetLoanStage(int fileId, string stageName, ref Table.LoanStages stage, ref string err)
        {
            err = "";
            DataSet ds = null;
            DataRow dr = null;
            if (fileId <= 0)
            {
                err = "GetLoanStage: Invalid FileId, " + fileId + " specified.";
                return -1;
            }
            if ((stageName == null) || (stageName.Trim() == string.Empty))
            {
                err = "GetLoanStage: Invalid Stage Name, " + stageName + " specified.";
                return -1;
            }
            int loanStageId = 0;
            string sqlCmd = "Select TOP 1 * from LoanStages WHERE FileId=" + fileId + " AND StageName='" + stageName.Trim() + "'";
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    return -1;

                if (stage == null)
                    stage = new Table.LoanStages();
                dr = ds.Tables[0].Rows[0];
                if (dr["LoanStageId"] != DBNull.Value)
                {
                    stage.LoanStageId = (int)dr["LoanStageId"];
                    loanStageId = stage.LoanStageId;
                }
                else
                    return -1;
                if (dr["Completed"] != DBNull.Value)
                    stage.Completed = (DateTime)dr["Completed"];
                else
                    stage.Completed = DateTime.MinValue;

                if (dr["SequenceNumber"] != DBNull.Value)
                    stage.SequenceNumber = (short)dr["SequenceNumber"];
                else
                    stage.SequenceNumber = -1;

                if (dr["FileId"] != DBNull.Value)
                    stage.FileId = (int)dr["FileId"];
                else
                    stage.FileId = -1;

                if (dr["StageName"] != DBNull.Value)
                    stage.StageName = dr["StageName"].ToString().Trim();
                else
                    stage.StageName = stageName.Trim();

                if (dr["WflTemplId"] != DBNull.Value)
                    stage.WflTemplId = (int)dr["WflTemplId"];
                else
                    stage.WflTemplId = -1;

                if (dr["WflStageId"] != DBNull.Value)
                    stage.WflStageId = (int)dr["WflStageId"];
                else
                    stage.WflStageId = -1;

                if (dr["DaysFromEstClose"] != DBNull.Value)
                    stage.DaysFromEstClose = (short)dr["DaysFromEstClose"];
                else
                    stage.DaysFromEstClose = 0;

                return loanStageId;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return -1;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
            }
        }

        public bool GetTemplateStages(int FileId, string stageName, ref int PointStageNameField, ref int PointStageDateField, ref string err)
        {
            err = "";
            DataSet ds = null;
            DataRow dr = null;

            if ((stageName == null) || (stageName.Trim() == string.Empty))
            {
                err = "GetTemplateStages: empty Stage Name, specified.";
                return false;
            }

            string sqlCmd = string.Format("select ts.* from LoanStages ls inner join Template_Wfl_Stages ws on ls.WflStageId=ws.WflStageId inner join Template_Stages ts on ws.TemplStageId=ts.TemplStageId where ls.FileId={0} and ls.StageName='{1}'", FileId, stageName.Trim());
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    return false;

                dr = ds.Tables[0].Rows[0];

                if (dr["PointStageNameField"] != DBNull.Value)
                {
                    PointStageNameField = Convert.ToInt16(dr["PointStageNameField"].ToString());
                }
                else
                    PointStageNameField = -1;

                if (dr["PointStageDateField"] != DBNull.Value)
                {
                    PointStageDateField = Convert.ToInt16(dr["PointStageDateField"].ToString());
                }
                else
                    PointStageDateField = -1;

                return true;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return false;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
            }
        }

        public int GetLoanStageId(int fileId, string stage, ref string err)
        {
            int stageId = -1;
            try
            {
                SqlParameter[] parameters = { 
                    new SqlParameter("@FileId", SqlDbType.Int, 4 ),                              //0                   
                    new SqlParameter("@Stage", SqlDbType.NVarChar, 50)               //1                                               
                  };

                parameters[0].Value = fileId;
                parameters[1].Value = stage;

                int rows = 0;
                stageId =
                    DbHelperSQL.RunProcedure("Get_LoanStageId", parameters, out rows);
                return stageId;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return stageId;
            }
        }

        public int Save_LoanTasks(int fileId, Table.LoanTasks task, ref string err)
        {
            if (fileId <= 0)
            {
                err = "Ssave_LoanTasks: Invalid FileId specified, " + fileId;
                return -1;
            }
            if (task == null)
            {
                err = "Save_LoanTasks: Task info is null.";
                return -1;
            }

            task.LastModified = DateTime.MinValue;
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@LoanTaskId", SqlDbType.Int, 4),                            //0               
                    new SqlParameter("@FileId", SqlDbType.Int, 4 ),                                      //1                   
                    new SqlParameter("@Name", SqlDbType.NVarChar, 255),                      //2
                    new SqlParameter("@PrerequisiteTaskId", SqlDbType.Int, 4),                 //3
                    new SqlParameter("@DaysAfterPrerequisite", SqlDbType.SmallInt),       //4
                    new SqlParameter("@DaysFromEstClose", SqlDbType.SmallInt),           //5
                    new SqlParameter("@DaysFromCreation", SqlDbType.SmallInt),           //6
                    new SqlParameter("@OwnerId", SqlDbType.Int, 4),                                   //7
                    new SqlParameter("@Due", SqlDbType.DateTime ),                                 //8
                    new SqlParameter("@Completed", SqlDbType.DateTime ),                     //9
                    new SqlParameter("@CompletedBy", SqlDbType.Int, 4),                         //10
                    new SqlParameter("@LastModified", SqlDbType.DateTime),                  //11     
                    new SqlParameter("@LoanStageId", SqlDbType.Int, 4),                          //12     
                    new SqlParameter("@Created", SqlDbType.DateTime),                          //13     
                    new SqlParameter("@TemplTaskId", SqlDbType.Int, 4),                          //14     
                    new SqlParameter("@WflTemplId", SqlDbType.Int, 4),                            //15     
                    new SqlParameter("@WarningEmailId", SqlDbType.Int, 4),                     //16
                    new SqlParameter("@OverdueEmailId", SqlDbType.Int, 4),                     //17     
                    new SqlParameter("@CompletionEmailId", SqlDbType.Int, 4),                //18     
                    new SqlParameter("@SequenceNumber", SqlDbType.SmallInt),            //19     
                };
                if (task.LoanTaskId <= 0)
                    parameters[0].Value = DBNull.Value;
                else
                    parameters[0].Value = task.LoanTaskId;
                parameters[0].Direction = ParameterDirection.Output;
                parameters[1].Value = task.FileId;
                parameters[2].Value = task.Name;
                if (task.PrerequisiteTaskId <= 0)
                {
                    parameters[3].Value = DBNull.Value;
                    parameters[4].Value = DBNull.Value;
                }
                else
                {
                    parameters[3].Value = task.PrerequisiteTaskId;
                    parameters[4].Value = task.DaysAfterPrerequisiteTask;
                }

                parameters[5].Value = task.DaysFromEstClose;
                parameters[6].Value = task.DaysFromCreation;

                if (task.Owner <= 0)
                    parameters[7].Value = DBNull.Value;
                else
                    parameters[7].Value = task.Owner;
                if (task.Due == DateTime.MinValue)
                    parameters[8].Value = DBNull.Value;
                else
                    parameters[8].Value = task.Due;
                if (task.Completed == DateTime.MinValue)
                    parameters[9].Value = DBNull.Value;
                else
                    parameters[9].Value = task.Completed;
                if (task.CompletedBy < 0)
                    parameters[10].Value = DBNull.Value;
                else
                    parameters[10].Value = task.CompletedBy;

                if (task.LastModified == DateTime.MinValue)
                    parameters[11].Value = DBNull.Value;
                else
                    parameters[11].Value = task.LastModified;

                if (task.LoanStageId <= 0)
                    parameters[12].Value = DBNull.Value;
                else
                    parameters[12].Value = task.LoanStageId;

                if (task.Created == DateTime.MinValue)
                    parameters[13].Value = DBNull.Value;
                else
                    parameters[13].Value = task.Created;

                if (task.TemplTaskId <= 0)
                    parameters[14].Value = DBNull.Value;
                else
                    parameters[14].Value = task.TemplTaskId;

                if (task.WflTemplId <= 0)
                    parameters[15].Value = DBNull.Value;
                else
                    parameters[15].Value = task.WflTemplId;

                if (task.WarmingEmailId <= 0)
                    parameters[16].Value = DBNull.Value;
                else
                    parameters[16].Value = task.WarmingEmailId;

                if (task.OverdueEmailId <= 0)
                    parameters[17].Value = DBNull.Value;
                else
                    parameters[17].Value = task.OverdueEmailId;

                if (task.CompletionEmailId <= 0)
                    parameters[18].Value = DBNull.Value;
                else
                    parameters[18].Value = task.CompletionEmailId;

                if (task.SequenceNumber <= 0)
                    parameters[19].Value = DBNull.Value;
                else
                    parameters[19].Value = task.SequenceNumber;

                int rows = 0;
                int LoanTaskId = 0;
                LoanTaskId =
                    DbHelperSQL.RunProcedure("LoanTasks_Save", parameters, out rows);
                if (LoanTaskId > 0)
                {
                    string outcontactId = LoanTaskId.ToString();
                    return LoanTaskId;
                }

                return -1;
            }
            catch (Exception e)
            {
                err = "Failed to update LoanTasks due to database error:" + e.Message;
                Trace.TraceError(err);
                return -1;
            }
        }
        public int Find_LOName(int branchId, string name, ref string err)
        {
            err = "";
            string sqlCmd = "Select UserId From UserLoanRep WHERE [Name]=@Fullname";
            if (branchId > 0)
            {
                sqlCmd += " AND BranchId = " + branchId;
            }

            int userId = -1;
            try
            {
                SqlCommand cmd = new SqlCommand(sqlCmd);
                cmd.Parameters.Add(new SqlParameter("@Fullname", SqlDbType.NVarChar));
                cmd.Parameters[0].Value = name.Trim();
                object obj = DbHelperSQL.ExecuteScalar(cmd);
                if (obj != null && obj != DBNull.Value)
                {
                    userId = (int)obj;
                    return userId;
                }

                SqlParameter[] parameters = {
                    new SqlParameter("@NameId", SqlDbType.Int, 4),                    //0
    	            new SqlParameter("@BranchId", SqlDbType.Int, 4),                  //1
                    new SqlParameter("@Name", SqlDbType.NVarChar, 255),       //2
                    new SqlParameter("@UserId", SqlDbType.Int, 4)};                     //3

                parameters[0].Value = DBNull.Value;
                if (branchId <= 0)
                    parameters[1].Value = DBNull.Value;
                else
                    parameters[1].Value = branchId;
                parameters[2].Value = name.Trim();
                parameters[3].Value = DBNull.Value;

                int rowsAffected = 0;
                int outputId = 0;
                outputId =
                DbHelperSQL.RunProcedure("UserLoanRep_Save", parameters, out rowsAffected);
                return userId;
            }
            catch (Exception ex)
            {
                err = "Find_LOName, Exception:" + ex.Message;
                Trace.TraceError(err);
                return -1;
            }
        }

        public int Find_User(int branchId, string name, string role, ref string err)
        {
            int userId = -1;
            string sqlCmd = "select dbo.[lpfn_FindUserByFullName] (@UserFullname, @RoleName, @BranchId)";
            try
            {
                SqlCommand cmd = new SqlCommand(sqlCmd);
                cmd.Parameters.Add("@UserFullName", SqlDbType.NVarChar).Value = name.Trim();
                cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar).Value = role.Trim();
                cmd.Parameters.Add("@BranchId ", SqlDbType.Int).Value = branchId;
                object obj = DbHelperSQL.ExecuteScalar(cmd);
                if (obj == null || obj == DBNull.Value)
                    return userId;
                userId = (int)obj;
                return userId;
            }
            catch (Exception ex)
            {
                err = "Find_User, Exception:" + ex.Message;
                Trace.TraceError(err);
                return userId;
            }

        }

        public string Get_ContactRoleName(int RoleId, ref string err)
        {
            string sqlCmd = "Select TOP 1 [Name] from ContactRoles WHERE ContactRoleId=" + RoleId;
            string roleName = "";
            DataSet ds = null;
            if (RoleId <= 0)
            {
                err = "Get_ContactRoleName, invalid RoleId" + RoleId + " specified.";
                return roleName;
            }

            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "Unable to find Contact Role Name for ContactRoleId " + RoleId;
                    return roleName;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["Name"] == DBNull.Value)
                    return roleName;
                roleName = dr["Name"].ToString().Trim();
                return roleName;
            }
            catch (Exception ex)
            {
                err = "Get_ContactRoleName, Exception:" + ex.Message;
                Trace.TraceError(err);
                return roleName;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public string Get_RoleName(int RoleId, ref string err)
        {
            string sqlCmd = "Select TOP 1 [Name] from Roles WHERE RoleId=" + RoleId;
            string roleName = "";
            DataSet ds = null;
            if (RoleId <= 0)
            {
                err = "Get_RoleName, invalid RoleId" + RoleId + " specified.";
                return roleName;
            }

            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "Unable to find Role Name for RoleId " + RoleId;
                    return roleName;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["Name"] == DBNull.Value)
                    return roleName;
                roleName = dr["Name"].ToString().Trim();
                return roleName;
            }
            catch (Exception ex)
            {
                err = "Get_RoleName, Exception:" + ex.Message;
                Trace.TraceError(err);
                return roleName;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public User Get_UserInfo(int UserId, ref string err)
        {
            string sqlCmd = "Select TOP 1 FirstName, LastName,Username,[Password], EmailAddress, Phone, Fax, Cell,ExchangePassword from Users WHERE UserId=" + UserId;
            User uInfo = null;
            DataSet ds = null;
            if (UserId <= 0)
            {
                err = "Get_UserInfo, invalid UserId " + UserId + " specified.";
                return uInfo;
            }

            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "Unable to find user record for UserId " + UserId;
                    return uInfo;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["FirstName"] == DBNull.Value)
                    return uInfo;
                if (dr["LastName"] == DBNull.Value)
                    return uInfo;
                uInfo = new User();

                uInfo.Firstname = dr["FirstName"].ToString().Trim();
                uInfo.Lastname = dr["LastName"].ToString().Trim();
                uInfo.Password = dr["Password"] == DBNull.Value ? string.Empty : dr["Password"].ToString().Trim();
                uInfo.Username = dr["Username"] == DBNull.Value ? string.Empty : dr["Username"].ToString().Trim();
                uInfo.Email = dr["EmailAddress"] == DBNull.Value ? string.Empty : dr["EmailAddress"].ToString().Trim();
                uInfo.Phone = dr["Phone"] == DBNull.Value ? string.Empty : dr["Phone"].ToString().Trim();
                uInfo.Cell = dr["Cell"] == DBNull.Value ? string.Empty : dr["Cell"].ToString().Trim();
                uInfo.Fax = dr["Fax"] == DBNull.Value ? string.Empty : dr["Fax"].ToString().Trim();
                uInfo.ExchangePassword = dr["ExchangePassword"] == DBNull.Value ? string.Empty : dr["ExchangePassword"].ToString().Trim();

                return uInfo;
            }
            catch (Exception ex)
            {
                err = "Get_UserInfo, Exception:" + ex.Message;
                Trace.TraceError(err);
                return uInfo;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public int Find_LO(int branchId, string name, ref string err)
        {
            int userId = -1;
            err = "";
            userId = Find_User(branchId, name, UserRoles.UserRole_LoanOfficer, ref err);
            if (userId <= 0)
                userId = Find_LOName(branchId, name, ref err);

            return userId;
        }

        public bool Save_Team(int fileId, int branchId, string role, string name, bool createUser, ref string err)
        {
            err = "";
            int userId = -1;

            try
            {
                string sqlCmd = string.Empty;
                if (fileId <= 0 || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(role))
                {
                    err = string.Format("Unable to save the team record, missing required fields, FileId={0}, role={1}, name={2}.", fileId, role, name);
                    return false;
                }

                userId = Find_User(branchId, name, role, ref err);
                if (userId <= 0)
                {
                    err = "Unable to find the user account for " + role + ", " + name;
                    return false;
                }
                SqlParameter[] parameters = {
                        new SqlParameter("@FileId", SqlDbType.Int, 4),
    	                new SqlParameter("@UserId", SqlDbType.Int, 4),
                        new SqlParameter("@RoleName", SqlDbType.NVarChar, 50)};

                parameters[0].Value = fileId;
                parameters[1].Value = userId;
                parameters[2].Value = role;

                int rowsAffected = 0;
                DbHelperSQL.RunProcedure("Save_LoanTeam", parameters, out rowsAffected);

                return true;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return false;
            }
        }

        public bool ReassignLoan(int FileId, int RoleId, int NewUserId, int Requester, ref string err)
        {
            err = "";
            if ((FileId <= 0) || (RoleId <= 0) || (NewUserId <= 0))
            {
                err = "ReassignLoan=, Invalid FileId=" + FileId + ", RoleId=" + RoleId + " or UserId=" + NewUserId + " specified";
                return false;
            }
            if (Requester < 0)
                Requester = 0;
            try
            {

                SqlParameter[] parameters = {
                    new SqlParameter("@FileId", SqlDbType.Int, 4),
                    new SqlParameter("@NewUserId", SqlDbType.Int, 4),
                    new SqlParameter("@OldUserId", SqlDbType.Int, 4),
    	            new SqlParameter("@RoleId", SqlDbType.Int, 4),
                    new SqlParameter("@Requester", SqlDbType.Int, 4)
                };

                parameters[0].Value = FileId;
                parameters[1].Value = NewUserId;
                parameters[2].Value = DBNull.Value;
                parameters[3].Value = RoleId;
                if (Requester <= 0)
                    parameters[4].Value = DBNull.Value;
                else
                    parameters[4].Value = Requester;

                int rowsAffected = 0;
                int outputId = 0;
                outputId =
                DbHelperSQL.RunProcedure("lpsp_ReassignLoan", parameters, out rowsAffected);
                return true;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return false;
            }
        }

        public bool UpdateLoanTeam(int FileId, int RoleId, int UserId)
        {
            bool status = true;
            string err = "";
            if ((FileId <= 0) || (RoleId <= 0) || (UserId <= 0))
            {
                err = "UpdateLoanTeam=, Invalid FileId=" + FileId + ", RoleId=" + RoleId + " or UserId=" + UserId + " specified";
                return false;
            }
            try
            {
                string sqlCmd = "Delete LoanTeam Where fileId=" + FileId + " and RoleId=" + RoleId + " and UserId=" + UserId;
                DbHelperSQL.ExecuteSql(sqlCmd);

                SqlParameter[] parameters = {
            new SqlParameter("@FileId", SqlDbType.Int, 4),
    	    new SqlParameter("@RoleId", SqlDbType.Int, 4),
            new SqlParameter("@UserId", SqlDbType.Int, 4)};

                parameters[0].Value = FileId;
                parameters[1].Value = RoleId;
                parameters[2].Value = UserId;

                int rowsAffected = 0;
                int outputId = 0;
                outputId =
                DbHelperSQL.RunProcedure("LP_AddLoanTeam", parameters, out rowsAffected);
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
            }

            return status;
        }

        public void Create_Users_Table(Common.Record.Users users_record, ref Common.Table.Users users_table)
        {
            users_table.UserEnabled = true;
            users_table.Prefix = users_record.Prefix;
            users_table.Username = users_record.Username;
            users_table.EmailAddress = users_record.EmailAddress;
            users_table.UserPictureFile = users_record.UserPictureFile;
            users_table.FirstName = users_record.FirstName;
            users_table.LastName = users_record.LastName;
            users_table.RoleId = Convert.ToInt32(users_record.RoleId);
            users_table.Password = users_record.Password;
            users_table.LoansPerPage = Convert.ToInt16(users_record.LoansPerPage);
        }

        public int Save_Borrower(int FileId, int ContactId, ref string err)
        {
            err = "";
            try
            {
                SqlParameter[] parameters = {
                new SqlParameter("@FileId", SqlDbType.Int, 4),
                new SqlParameter("@ContactId", SqlDbType.Int, 4)};

                parameters[0].Value = FileId;
                parameters[1].Value = ContactId;
                int rowsAffected = 0;
                ContactId =
                DbHelperSQL.RunProcedure("Borrower_Save", parameters, out rowsAffected);
                return ContactId;
            }
            catch (Exception e)
            {
                err = "Failed to save Borrower due to database error, Exception:" + e.Message;
                Trace.TraceError(err);
                return -1;
            }
        }
        public int Save_CoBorrower(int FileId, int ContactId, ref string err)
        {
            err = "";
            try
            {
                SqlParameter[] parameters = {
                new SqlParameter("@FileId", SqlDbType.Int, 4),
                new SqlParameter("@ContactId", SqlDbType.Int, 4)};

                parameters[0].Value = FileId;
                parameters[1].Value = ContactId;
                int rowsAffected = 0;
                ContactId =
                DbHelperSQL.RunProcedure("CoBorrower_Save", parameters, out rowsAffected);
                return ContactId;
            }
            catch (Exception e)
            {
                err = "Failed to save CoBorrower due to database error, Exception:" + e.Message;
                Trace.TraceError(err);
                return -1;
            }
        }

        public int Save_LoanActivities(int FileId, int UserId, string Activity, ref string err)
        {
            err = "";
            int activityId = 0;
            if ((FileId <= 0) || (UserId < 0))
            {
                err = "Save_LoanActivities, invalid FileId " + FileId + " or UserId " + UserId + ".";
                return activityId;
            }
            try
            {
                string sqlCmd = "INSERT INTO LoanActivities (FileId, UserId, ActivityName, ActivityTime) VALUES (";
                sqlCmd = +FileId + ", " + UserId + ", '" + Activity.Trim() + "', '" + DateTime.Now.ToString() + "'";
                DbHelperSQL.ExecuteSql(sqlCmd);
                activityId = 1;
                return activityId;
            }
            catch (Exception ex)
            {
                err = "Save_LoanActivities, Exception: " + ex.Message;
                return activityId;
            }
        }

        public bool Check_V_ProcessingPipelineInfo(int FileId, ref string err)
        {
            err = "";
            if (FileId <= 0)
            {
                err = string.Format("Check_V_ProcessingPipelineInfo, FileId {0} is invavlid.", FileId);
                return false;
            }
            string sqlCmd = "Select FileId from V_ProcessingPipelineInfo where FileId=" + FileId;
            try
            {
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                int rowsAffected = 0;
                int tempFileId = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
                if (tempFileId > 0)
                {
                    sqlCmd = "Delete V_ProcessingPipelineInfo where Fileid=" + FileId;
                    DbHelperSQL.ExecuteSql(sqlCmd);
                }

                SqlParameter[] parameters = {
                    new SqlParameter("@FileId", SqlDbType.Int, 4)};

                parameters[0].Value = FileId;
                DbHelperSQL.RunProcedure("dbo.lpsp_INSERT_V_ProcessingPipelineInfo", parameters, out rowsAffected);
                return true;
            }
            catch (Exception e)
            {
                err = "Failed to save LoanContacts due to database error, Exception:" + e.Message;
                Trace.TraceError(err);
                return false;
            }
        }
        public bool Remove_LoanContacts(int FileId, string ContactRole, int ContactId, ref string err)
        {
            if (FileId <= 0 || string.IsNullOrEmpty(ContactRole))
            {
                err = string.Format("Remove_LoanContacts, invalid parameter FileId = {0}, contactRole={1}, ContactId={2}.", FileId, ContactRole, ContactId);
                return false;
            }
            try
            {
                string sqlCmd = string.Format("select ContactRoleId from ContactRoles where [Name]='{0}'", ContactRole);
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                int contactRoleId = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
                if (contactRoleId <= 0)
                {
                    err = string.Format("Remove_LoanContacts, unable to find ContactRoleId for ContactRole={0}, FileId={1}.", ContactRole, FileId);
                    return false;
                }
                sqlCmd = string.Format("Delete From LoanContacts where FileId={0} AND ContactRoleId={1}  ", FileId, contactRoleId);
                if (ContactId > 0)
                    sqlCmd += string.Format(" AND ContactId={0}", ContactId);
                DbHelperSQL.ExecuteSql(sqlCmd);
            }
            catch (Exception ex)
            {
                err = string.Format(" Remove_LoanContacts, FileId = {0}, ContactRole={1}, ContactId={2}, Exception: {3}", FileId, ContactRole, ContactId, ex.Message);
                return false;
            }
            return true;
        }
        public int Save_LoanContacts(int FileId, string ContactRole, int ContactId, ref string err)
        {
            err = "";
            if ((ContactRole == null) || (ContactRole == String.Empty))
            {
                err = "Contact Role name is empty";
                return -1;
            }

            try
            {
                SqlParameter[] parameters = {
                new SqlParameter("@FileId", SqlDbType.Int, 4),
    	        new SqlParameter("@ContactRole", SqlDbType.NVarChar, 100),
                new SqlParameter("@ContactId", SqlDbType.Int, 4)};

                parameters[0].Value = FileId;
                parameters[1].Value = ContactRole;
                parameters[2].Value = ContactId;
                int rowsAffected = 0;
                FileId =
                DbHelperSQL.RunProcedure("LoanContacts_Save", parameters, out rowsAffected);
                if (FileId > 0)
                {
                    string outFileId = FileId.ToString();
                    return FileId;
                }

                return -1;
            }
            catch (Exception e)
            {
                err = "Failed to save LoanContacts due to database error, Exception:" + e.Message;
                Trace.TraceError(err);
                return -1;
            }
        }

        public void Create_Contacts_Table(Common.Record.Contacts contacts_record, ref Common.Table.Contacts contacts_table)
        {
            DateTime dt = new DateTime();
            int i1 = 0;

            if (contacts_record.ContactId > 0)
                contacts_table.ContactId = contacts_record.ContactId;
            else
                contacts_table.ContactId = 0;
            contacts_table.FirstName = contacts_record.FirstName;
            contacts_table.MiddleName = contacts_record.MiddleName;
            contacts_table.LastName = contacts_record.LastName;
            contacts_table.NickName = contacts_record.NickName;
            contacts_table.Title = contacts_record.Title;
            contacts_table.GenerationCode = contacts_record.GenerationCode;
            contacts_table.SSN = contacts_record.SSN;
            contacts_table.HomePhone = contacts_record.HomePhone;
            contacts_table.CellPhone = contacts_record.CellPhone;
            contacts_table.BusinessPhone = contacts_record.BusinessPhone;
            contacts_table.Fax = contacts_record.Fax;
            contacts_table.Email = contacts_record.Email;
            contacts_table.DOB = DateTime.MinValue;

            if (DateTime.TryParse(contacts_record.DOB, out dt))
            {
                contacts_table.DOB = dt;
            }

            if (int.TryParse(contacts_record.Experian, out i1))
            {
                contacts_table.Experian = i1;
            }
            else
            {
                contacts_table.Experian = 0;
            }

            if (int.TryParse(contacts_record.TransUnion, out i1))
            {
                contacts_table.TransUnion = i1;
            }
            else
            {
                contacts_table.TransUnion = 0;
            }

            if (int.TryParse(contacts_record.Equifax, out i1))
            {
                contacts_table.Equifax = i1;
            }
            else
            {
                contacts_table.Equifax = 0;
            }

            contacts_table.MailingAddr = contacts_record.MailingAddr;
            contacts_table.MailingCity = contacts_record.MailingCity;
            contacts_table.MailingState = contacts_record.MailingState;
            contacts_table.MailingZip = contacts_record.MailingZip;

            if (int.TryParse(contacts_record.ContactCompanyId, out i1))
            {
                contacts_table.ContactCompanyId = i1;
            }
            else
            {
                contacts_table.ContactCompanyId = 0;
            }

            if (int.TryParse(contacts_record.ContactBranchId, out i1))
            {
                contacts_table.ContactBranchId = i1;
            }
            else
            {
                contacts_table.ContactBranchId = 0;
            }

            if (int.TryParse(contacts_record.WebAccountId, out i1))
            {
                contacts_table.WebAccountId = i1;
            }
            else
            {
                contacts_table.WebAccountId = 0;
            }
        }

        public bool Save_Alert(int FileId, Common.Table.LoanAlert alert, bool delete, ref string err)
        {
            err = "";

            if (FileId <= 0)
            {
                err = "Save_Alert:: Invalid FileId specified, FieldId=" + FileId.ToString();
                return false;
            }

            if (alert == null)
            {
                err = "Alert info is empty.";
                return false;
            }

            if ((!delete) && ((alert.Desc == String.Empty) || (alert.Due == DateTime.MinValue)))
            {
                err = "Missing Desc or Due Date in Alert info.";
                return false;
            }

            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@FileId", SqlDbType.Int, 4),                                          //0               
                    new SqlParameter("@Desc", SqlDbType.NVarChar, 50),                            //1
                    new SqlParameter("@AlertType", SqlDbType.NVarChar, 50),                     //2
                    new SqlParameter("@Due", SqlDbType.DateTime),                                    //3
                    new SqlParameter("@Owner", SqlDbType.NVarChar, 50),                           //4
                    new SqlParameter("@LoanTaskId", SqlDbType.Int, 4),                                //5
                    new SqlParameter("@LoanRuleId", SqlDbType.Int, 4 ),                               //6
                    new SqlParameter("@AcknowledgeReq", SqlDbType.Bit),                          //7
                    new SqlParameter("@Acknowledged", SqlDbType.DateTime),                  //8
                    new SqlParameter("@AcknowledgedBy", SqlDbType.NVarChar, 20),        //9
                    new SqlParameter("@ClearedBy", SqlDbType.Int, 4),                                 //10
                    new SqlParameter("@Cleared", SqlDbType.DateTime),                             //11
                    new SqlParameter("@DateCreated", SqlDbType.DateTime, 255),             //12
                    new SqlParameter("@Delete", SqlDbType.Bit) ,                                            //13
                    new SqlParameter("@EmailId", SqlDbType.Int, 4)                                        //14
                                            };
                parameters[0].Value = FileId;
                parameters[1].Value = alert.Desc;
                parameters[2].Value = alert.AlertType;

                if (alert.Due == DateTime.MinValue)
                    parameters[3].Value = DBNull.Value;
                else
                    parameters[3].Value = alert.Due;

                if (alert.Owner <= 0)
                    parameters[4].Value = DBNull.Value;
                else
                    parameters[4].Value = alert.Owner;

                if (alert.TaskId <= 0)
                    parameters[5].Value = DBNull.Value;
                else
                    parameters[5].Value = alert.TaskId;

                if (alert.RuleId <= 0)
                    parameters[6].Value = DBNull.Value;
                else
                    parameters[6].Value = alert.RuleId;

                parameters[7].Value = DBNull.Value;
                parameters[8].Value = DBNull.Value;
                parameters[9].Value = DBNull.Value;

                if (alert.ClearedBy <= 0)
                    parameters[10].Value = DBNull.Value;
                else
                    parameters[10].Value = alert.ClearedBy;

                if (alert.Cleared == DateTime.MinValue)
                    parameters[11].Value = DBNull.Value;
                else
                    parameters[11].Value = alert.Cleared;

                if (alert.Created == DateTime.MinValue)
                    parameters[12].Value = DBNull.Value;
                else
                    parameters[12].Value = alert.Created;

                if (delete)
                    parameters[13].Value = "true";
                else
                    parameters[13].Value = "false";

                parameters[14].Value = DBNull.Value;                    // Email Id -- warning or overdue email, set to null for now

                int alertId = 0;
                int rows = 0;
                alertId =
                    DbHelperSQL.RunProcedure("Check_Save_LoanAlert", parameters, out rows);

                if (alertId > 0)
                {
                    string outcontactId = alertId.ToString();
                    return true;
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return false;
            }
            return true;
        }

        public bool Save_Note(int fileId, Table.LoanNotes note, ref string err)
        {
            err = "";
            bool logErr = false;
            if ((fileId <= 0) || (note == null))
            {
                err = "Save_Note:: Invalid parameters, FieldId:" + fileId + " or Note is null.";
                return false;
            }
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@NoteId", SqlDbType.Int, 4),                                          //0               
                    new SqlParameter("@FileId", SqlDbType.Int, 4),                                             //1
                    new SqlParameter("@Created", SqlDbType.DateTime),                               //2
                    new SqlParameter("@Sender", SqlDbType.NVarChar, 255),                         //3
                    new SqlParameter("@Note", SqlDbType.NVarChar),                             //4
                    new SqlParameter("@Exported", SqlDbType.Bit)                                            //5
                    };
                if (note.NoteId <= 0)
                    parameters[0].Value = DBNull.Value;
                else
                    parameters[0].Value = note.NoteId;
                parameters[1].Value = fileId;
                if (note.Created == DateTime.MinValue)
                    parameters[2].Value = DBNull.Value;
                else
                    parameters[2].Value = note.Created;

                if (note.Sender == String.Empty)
                    parameters[3].Value = DBNull.Value;
                else
                    parameters[3].Value = note.Sender;

                if (note.Note == String.Empty)
                    parameters[4].Value = DBNull.Value;
                else
                    parameters[4].Value = note.Note;

                if (note.Exported)
                    parameters[5].Value = 1;
                else
                    parameters[5].Value = 0;

                int noteId = 0;
                int rows = 0;
                noteId =
                    DbHelperSQL.RunProcedure("Save_Note", parameters, out rows);

                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("Save_Note, Exception:{0}, FileId {1}.", ex.Message, fileId);
                int Event_id = 3051;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 3052;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        public bool Update_LoanNotes(int fileId, List<Table.LoanNotes> noteList, ref string err)
        {
            bool bl = true;

            if (fileId <= 0)
            {
                err = "Update_LoanNotes, FileId " + fileId + " is not valid.";
                return false;
            }

            if ((noteList == null) || (noteList.Count <= 0))
            {
                err = "Update_LoanNotes, noteList is null or empty, fileId" + fileId;
                return false;
            }

            foreach (Table.LoanNotes n in noteList)
            {
                try
                {
                    bl = Save_Note(fileId, n, ref err);
                }
                catch (Exception ex)
                {
                    err = "Update_LoanNotes, Exception: " + ex.Message;
                }
            }

            return true;
        }

        public List<Table.TaskHistory> Get_TaskHistoryList(int fileId, bool include_exported, ref string err)
        {
            List<Table.TaskHistory> taskHistoryList = null;
            err = "";
            if (fileId <= 0)
            {
                err = "Get_LoanNotes, FileId " + fileId.ToString() + " is not valid.";
                return taskHistoryList;
            }
            string sqlCmd = "Select TaskHistoryId, ActivityTime, Sender, Activity,Exported from lpvw_GetTaskHistory WHERE FileId=" + fileId;
            if (include_exported)
                sqlCmd += " ";
            else
                sqlCmd += " AND (( EXPORTED IS NULL) OR (EXPORTED=0)) ";

            sqlCmd += "  ORDER BY TaskHistoryId ASC";
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    return taskHistoryList;
                taskHistoryList = new List<Table.TaskHistory>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if ((dr["TaskHistoryId"] == DBNull.Value) || (dr["ActivityTime"] == DBNull.Value) || (dr["Activity"] == DBNull.Value) || (dr["Sender"] == DBNull.Value))
                        continue;
                    Table.TaskHistory t = new Table.TaskHistory();
                    if (dr["TaskHistoryId"] != DBNull.Value)
                        t.TaskHistoryId = (int)dr["TaskHistoryId"];

                    if (dr["ActivityTime"] != DBNull.Value)
                        t.ActivityTime = (DateTime)dr["ActivityTime"];

                    if (dr["Sender"] != DBNull.Value)
                        t.User = dr["Sender"].ToString();

                    if (dr["Activity"] != DBNull.Value)
                        t.ActivityName = dr["Activity"].ToString();

                    if (dr["Exported"] != DBNull.Value)
                        t.Exported = (bool)dr["Exported"];
                    else
                        t.Exported = false;
                    taskHistoryList.Add(t);
                }
                return taskHistoryList;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return taskHistoryList;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
            }
        }

        public bool Update_EmailLogExported(string emailLogIds, ref string err)
        {
            bool logErr = false;
            err = "";
            if (emailLogIds.Length <= 0)
            {
                err = "Update_EmailLogExported, invalid EmaiLLogIds=" + emailLogIds;
                int Event_id = 3053;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            try
            {
                string sqlCmd = "Update EmailLog set Exported=1 WHERE EmailLogId in (" + emailLogIds + ")";
                DbHelperSQL.ExecuteSql(sqlCmd);
                return true;
            }
            catch (Exception ex)
            {
                err = "Update_EmailLogExported, Exception:" + ex.Message;
                int Event_id = 3054;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 3055;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }

        }

        public bool Update_PointFiles(int FileId, int NewFolderId, string filename, ref string err)
        {
            bool logErr = false;
            err = "";

            try
            {
                string sqlCmd = "Update PointFiles set FolderId=" + NewFolderId + ", Name='" + filename + "' WHERE FileId=" + FileId;
                DbHelperSQL.ExecuteSql(sqlCmd);
                return true;
            }
            catch (Exception ex)
            {
                err = "Update_PointFiles, Exception:" + ex.Message;
                int Event_id = 3056;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 3057;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }

        }

        public bool Update_TaskHistoryStatus(int fileid, string task_historyId, ref string err)
        {
            err = "";
            if (fileid <= 0)
            {
                err = "Update_TaskHistoryStatus, invalid FileId = " + fileid;
                return false;
            }
            if ((task_historyId == null) || (task_historyId.Length <= 0))
            {
                err = "Update_TaskHistoryStatus, TaskHistoryIds empty, FileId=" + fileid;
                return false;
            }
            string[] temp_Ids = task_historyId.Split(';');
            if (temp_Ids.Length <= 0)
            {
                err = "Update_TaskHistoryStatus, TaskHistoryIds empty, FileId=" + fileid;
                return false;
            }
            string sqlCmd = "Update LoanTaskHistory Set Exported=1 WHERE FileId=" + fileid + " And TaskHistoryId ";
            if (temp_Ids.Length > 1)
                sqlCmd += " in (" + task_historyId.Replace(';', ',') + ") ";
            else
                sqlCmd += "=" + task_historyId;
            try
            {
                DbHelperSQL.ExecuteSql(sqlCmd);
            }
            catch (Exception ex)
            {
                err = "Update_TaskHistoryStatus, Exception: " + ex.Message;
                return false;
            }

            return true;
        }

        public List<Table.LoanNotes> Get_LoanNotes(int fileId, ref string err)
        {
            List<Table.LoanNotes> noteList = null;
            err = "";
            if (fileId <= 0)
            {
                err = "Get_LoanNotes, FileId " + fileId.ToString() + " is not valid.";
                return noteList;
            }
            string sqlCmd = "Select NoteId, Created, Sender, Note, Exported from LoanNotes WHERE FileId=" + fileId + " ORDER BY Created ASC";
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    return noteList;
                noteList = new List<Table.LoanNotes>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Table.LoanNotes n = new Table.LoanNotes();
                    if (dr["NoteId"] != DBNull.Value)
                        n.NoteId = (int)dr["NoteId"];
                    else
                        continue;
                    if (dr["Created"] != DBNull.Value)
                        n.Created = (DateTime)dr["Created"];
                    else
                        continue;

                    if (dr["Sender"] != DBNull.Value)
                        n.Sender = dr["Sender"].ToString();
                    else
                        continue;
                    if (dr["Note"] != DBNull.Value)
                        n.Note = dr["Note"].ToString();
                    else
                        continue;
                    if ((dr["Exported"] != DBNull.Value) && ((bool)dr["Exported"]))
                        n.Exported = true;
                    else
                        n.Exported = false;
                    noteList.Add(n);
                }
                return noteList;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return noteList;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
            }
        }

        public List<Table.LoanNotes> Get_LoanNotes_DESC(int fileId, ref string err)
        {
            List<Table.LoanNotes> noteList = null;
            err = "";
            bool b = false;
            string cString = "";
            string csqlCmd = "";
            int rows = 0;
            if (fileId <= 0)
            {
                err = "Get_LoanNotes, FileId " + fileId.ToString() + " is not valid.";
                return noteList;
            }
            string sqlCmd = "Select NoteId, Created, Sender, Note, Exported from LoanNotes WHERE FileId=" + fileId + " ORDER BY Created DESC";
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    return noteList;
                noteList = new List<Table.LoanNotes>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    b = false;
                    Table.LoanNotes n = new Table.LoanNotes();
                    if (dr["NoteId"] != DBNull.Value)
                        n.NoteId = (int)dr["NoteId"];
                    else
                        continue;
                    if (dr["Created"] != DBNull.Value)
                        n.Created = (DateTime)dr["Created"];
                    else
                        continue;

                    if (dr["Sender"] != DBNull.Value)
                        n.Sender = dr["Sender"].ToString();
                    else
                        continue;
                    if (dr["Note"] != DBNull.Value)
                    {
                        n.Note = dr["Note"].ToString();
                        b = n.Note.Contains("|");
                        if (b == true)
                        {
                            cString = n.Note.Replace("|", " ");
                            n.Note = cString;
                            csqlCmd = string.Format("Update LoanNotes Set [Note]=@Note WHERE FileId={0} and NoteId ={1}", fileId, n.NoteId);
                            SqlParameter[] parameters = {                                         
                                new SqlParameter("@Note", SqlDbType.NVarChar)       
                                                        };
                            parameters[0].Value = n.Note;

                            try
                            {
                                rows = DbHelperSQL.ExecuteSql(csqlCmd, parameters);
                            }
                            catch (Exception ex)
                            {
                                string msg = "";
                                msg = ex.Message;
                            }
                        }
                    }
                    else
                        continue;
                    if ((dr["Exported"] != DBNull.Value) && ((bool)dr["Exported"]))
                        n.Exported = true;
                    else
                        n.Exported = false;
                    noteList.Add(n);
                }
                return noteList;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return noteList;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
            }
        }

        public bool GetPartnerAddress(int fileId, ref Table.Contacts c, ref string err)
        {
            string sqlCmd = string.Empty;
            if (c.ContactBranchId > 0)
            {
                sqlCmd = string.Format("Select Address, City, State, Zip from ContactBranches where ContactBranchId={0}", c.ContactBranchId);
            }
            else if (c.ContactCompanyId > 0)
            {
                sqlCmd = string.Format("Select Address, City, State, Zip from ContactCompanies where ContactCompanyId={0}", c.ContactCompanyId);
            }

            try
            {
                if (string.IsNullOrEmpty(sqlCmd))
                    return true;

                DataSet ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                    return true;

                DataRow dr = ds.Tables[0].Rows[0];
                if (dr == null)
                    return true;

                c.MailingAddr = dr["Address"] == DBNull.Value ? string.Empty : dr["Address"].ToString();
                c.MailingCity = dr["City"] == DBNull.Value ? string.Empty : dr["City"].ToString();
                c.MailingState = dr["State"] == DBNull.Value ? string.Empty : dr["State"].ToString();
                c.MailingZip = dr["Zip"] == DBNull.Value ? string.Empty : dr["Zip"].ToString();
                return true;
            }
            catch (Exception ex)
            {
                err = "GetPartnerAddress, error:" + ex.Message;
                return false;
            }
        }

        public List<Table.Contacts> Get_LoanContacts(int fileId, string role, ref string err)
        {
            List<Table.Contacts> contactList = null;
            err = "";
            if (fileId <= 0)
            {
                err = "Get_LoanContacts, FileId " + fileId.ToString() + " is not valid.";
                return contactList;
            }
            string sqlCmd = "Select * from lpvw_GetLoanContactwRoles where FileId=" + fileId;
            if ((role != null) && (role != String.Empty))
            {
                sqlCmd += " AND RoleName='" + role + "'";
            }
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    return contactList;
                contactList = new List<Table.Contacts>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["ContactRoleId"] == DBNull.Value || dr["ContactId"] == DBNull.Value || dr["FirstName"] == DBNull.Value || dr["LastName"] == DBNull.Value)
                        continue;
                    Table.Contacts c = new Table.Contacts();
                    c.ContactId = (int)dr["ContactId"];
                    c.ContactRoleId = (int)dr["ContactRoleId"];
                    c.CompanyName = dr["CompanyName"] == DBNull.Value ? string.Empty : dr["CompanyName"].ToString();
                    c.FirstName = dr["FirstName"] == DBNull.Value ? string.Empty : dr["FirstName"].ToString();
                    c.LastName = dr["LastName"] == DBNull.Value ? string.Empty : dr["LastName"].ToString();
                    c.MiddleName = dr["MiddleName"] == DBNull.Value ? string.Empty : dr["MiddleName"].ToString();
                    c.SSN = dr["SSN"] == DBNull.Value ? string.Empty : dr["SSN"].ToString();
                    c.MailingAddr = dr["MailingAddr"] == DBNull.Value ? string.Empty : dr["MailingAddr"].ToString();
                    c.MailingCity = dr["MailingCity"] == DBNull.Value ? string.Empty : dr["MailingCity"].ToString();
                    c.MailingState = dr["MailingState"] == DBNull.Value ? string.Empty : dr["MailingState"].ToString();
                    c.MailingZip = dr["MailingZip"] == DBNull.Value ? string.Empty : dr["MailingZip"].ToString();
                    c.Email = dr["Email"] == DBNull.Value ? string.Empty : dr["Email"].ToString();
                    c.ContactRoleName = dr["RoleName"] == DBNull.Value ? string.Empty : dr["RoleName"].ToString();
                    c.ContactBranchId = dr["ContactBranchId"] == DBNull.Value ? 0 : (int)dr["ContactBranchId"];
                    c.ContactCompanyId = dr["ContactCompanyId"] == DBNull.Value ? 0 : (int)dr["ContactCompanyId"];
                    if (!string.IsNullOrEmpty(c.ContactRoleName) && c.ContactRoleName.ToUpper() != "BORROWER" && c.ContactRoleName.ToUpper() != "COBORROWER")
                    {
                        GetPartnerAddress(fileId, ref c, ref err);
                    }
                    contactList.Add(c);
                }
                return contactList;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return contactList;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
            }
        }

        public bool Get_Users(int UserId, ref string FirstName, ref string LastName, ref string err)
        {
            err = "";
            if (UserId <= 0)
            {
                err = "Get_Users, UserId " + UserId.ToString() + " is not valid.";
                return false;
            }
            string sqlCmd = "Select FirstName, LastName from Users where UserId=" + UserId;

            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    return false;
                DataRow dr = ds.Tables[0].Rows[0];
                FirstName = (string)dr["FirstName"];
                LastName = (string)dr["LastName"];
                return true;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return false;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
            }
        }

        public bool Get_Contacts(int ContactId, ref string FirstName, ref string LastName, ref string err)
        {
            err = "";
            if (ContactId <= 0)
            {
                err = "Get_Contacts, ContactId " + ContactId.ToString() + " is not valid.";
                return false;
            }
            string sqlCmd = "Select FirstName, LastName from Contacts where ContactId=" + ContactId;

            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    return false;
                DataRow dr = ds.Tables[0].Rows[0];
                FirstName = (string)dr["FirstName"];
                LastName = (string)dr["LastName"];
                return true;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return false;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
            }
        }

        public int Save_Contacts(Common.Record.Contacts contacts_record, string role, int contactId, ref string err)
        {
            err = "";
            Common.Table.Contacts contacts_table = new Common.Table.Contacts();
            Create_Contacts_Table(contacts_record, ref contacts_table);

            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@ContactId", SqlDbType.Int, 4),                  //0               
                    new SqlParameter("@FirstName", SqlDbType.NVarChar, 50),                   //1
                    new SqlParameter("@MiddleName", SqlDbType.NVarChar, 50),                 //2
                    new SqlParameter("@LastName", SqlDbType.NVarChar, 50),               //3
                    new SqlParameter("@NickName", SqlDbType.NVarChar, 50),           //4
                    new SqlParameter("@Title", SqlDbType.NVarChar, 50),                      //5
                    new SqlParameter("@GenerationCode", SqlDbType.NVarChar, 10 ),         //6
                    new SqlParameter("@SSN", SqlDbType.NVarChar, 20),        //7
                    new SqlParameter("@HomePhone", SqlDbType.NVarChar, 20),          //8
                    new SqlParameter("@CellPhone", SqlDbType.NVarChar, 20),            //9
                    new SqlParameter("@BusinessPhone", SqlDbType.NVarChar, 20),              //10
                    new SqlParameter("@Fax", SqlDbType.NVarChar, 20),                //11
                    new SqlParameter("@Email", SqlDbType.NVarChar, 255),           //12
                    new SqlParameter("@DOB", SqlDbType.DateTime),               //13
                    new SqlParameter("@Experian", SqlDbType.Int, 2),           //14
                    new SqlParameter("@TransUnion", SqlDbType.Int, 2),     //15
                    new SqlParameter("@Equifax", SqlDbType.Int, 2),  //16
                    new SqlParameter("@MailingAddr", SqlDbType.NVarChar, 50),            //17
                    new SqlParameter("@MailingCity", SqlDbType.NVarChar, 50 ),            //18
                    new SqlParameter("@MailingState", SqlDbType.NVarChar, 2 ),     //19
                    new SqlParameter("@MailingZip", SqlDbType.NVarChar, 12),     //20
                    new SqlParameter("@ContactCompanyId", SqlDbType.Int, 4),      //21
                    new SqlParameter("@ContactBranchId", SqlDbType.Int, 4),       //22
                    new SqlParameter("@WebAccountId", SqlDbType.Int, 4),          //23
                    new SqlParameter("@Enabled", SqlDbType.Bit),                  //24
                    new SqlParameter("@Borrower", SqlDbType.Bit)                  //25
                  };

                if (contactId <= 0)
                {
                    parameters[0].Value = DBNull.Value;
                    parameters[0].Direction = ParameterDirection.Output;
                }
                else
                    parameters[0].Value = contactId;

                parameters[1].Value = contacts_table.FirstName;
                parameters[2].Value = contacts_table.MiddleName;
                parameters[3].Value = contacts_table.LastName;
                parameters[4].Value = contacts_table.NickName;
                parameters[5].Value = contacts_table.Title;
                parameters[6].Value = contacts_table.GenerationCode;
                parameters[7].Value = contacts_table.SSN;
                parameters[8].Value = contacts_table.HomePhone;
                parameters[9].Value = contacts_table.CellPhone;
                parameters[10].Value = contacts_table.BusinessPhone;
                parameters[11].Value = contacts_table.Fax;
                parameters[12].Value = contacts_table.Email;

                if (contacts_table.DOB == DateTime.MinValue)
                {
                    parameters[13].Value = DBNull.Value;
                }
                else
                {
                    parameters[13].Value = contacts_table.DOB;
                }

                parameters[14].Value = contacts_table.Experian;
                parameters[15].Value = contacts_table.TransUnion;
                parameters[16].Value = contacts_table.Equifax;
                parameters[17].Value = contacts_table.MailingAddr;
                parameters[18].Value = contacts_table.MailingCity;
                parameters[19].Value = contacts_table.MailingState;
                parameters[20].Value = contacts_table.MailingZip;

                if (contacts_table.ContactCompanyId == 0)
                {
                    parameters[21].Value = DBNull.Value;
                }
                else
                {
                    parameters[21].Value = contacts_table.ContactCompanyId;
                }
                if (contacts_table.ContactBranchId == 0)
                {
                    parameters[22].Value = DBNull.Value;
                }
                else
                {
                    parameters[22].Value = contacts_table.ContactBranchId;
                }
                if (contacts_table.WebAccountId == 0)
                {
                    parameters[23].Value = DBNull.Value;
                }
                else
                {
                    parameters[23].Value = contacts_table.WebAccountId;
                }
                parameters[24].Value = 1;
                if (role != null && ((role.Trim().ToUpper() == "BORROWER") || (role.Trim().ToUpper() == "COBORROWER")))
                    parameters[25].Value = 1;
                else
                    parameters[25].Value = 0;
                int rows = 0;
                int cId = 0;
                cId =
                    DbHelperSQL.RunProcedure("Contacts_Save", parameters, out rows);
                if (contactId > 0)
                {
                    string outcontactId = contactId.ToString();
                    return contactId;
                }
                else
                {
                    if (cId > 0)
                        return cId;
                }

                return -1;
            }
            catch (Exception e)
            {
                err = " Save_Contacts, Failed to save Contacts due to database error, Exception:" + e.Message;
                int Event_id = 3058;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                Trace.TraceError(err);
                return -1;
            }
        }

        public int Save_ProspectEmployment(Common.Record.Contacts contacts_record, int contactId, ref int current_emplid, ref string err)
        {
            err = "";
            int idx = 0;
            int EmplId = 0;

            Record.Employment employment_temp = new Record.Employment();

            idx = contacts_record.employment.Count;

            for (int i = 0; i < idx; i++)
            {

                employment_temp = contacts_record.employment[i];

                try
                {
                    SqlParameter[] parameters = {
                    new SqlParameter("@EmplId", SqlDbType.Int, 4),         //0               
                    new SqlParameter("@ContactId", SqlDbType.Int, 4),      //1
                    new SqlParameter("@SelfEmployed", SqlDbType.Bit),      //2
                    new SqlParameter("@Position", SqlDbType.NVarChar, 50), //3
                    new SqlParameter("@StartYear", SqlDbType.Decimal, 2),     //4 
                    new SqlParameter("@StartMonth", SqlDbType.Decimal, 2),     //5     
                    new SqlParameter("@EndYear", SqlDbType.Decimal, 2),       //6                    
                    new SqlParameter("@EndMonth", SqlDbType.Decimal, 2),      //7                    
                    new SqlParameter("@YearsOnWork", SqlDbType.Decimal, 2),   //8                    
                    new SqlParameter("@Phone", SqlDbType.NVarChar, 20),    //9                    
                    new SqlParameter("@ContactBranchId", SqlDbType.Int, 4),     //10                    
                    new SqlParameter("@CompanyName", SqlDbType.NVarChar, 255),  //11                    
                    new SqlParameter("@Address", SqlDbType.NVarChar, 255),      //12                    
                    new SqlParameter("@City", SqlDbType.NVarChar, 100),         //13
                    new SqlParameter("@State", SqlDbType.NVarChar, 2),          //14                    
                    new SqlParameter("@Zip", SqlDbType.NVarChar, 20)            //15
                };

                    if (contactId <= 0)
                    {
                        return contactId;
                    }
                    else
                    {
                        parameters[0].Value = DBNull.Value;
                        parameters[0].Direction = ParameterDirection.Output;
                        parameters[1].Value = contactId;
                    }

                    if (employment_temp.SelfEmployed == true)
                        parameters[2].Value = 1;
                    else
                        parameters[2].Value = 0;

                    parameters[3].Value = employment_temp.Position;
                    parameters[4].Value = employment_temp.StartYear;
                    parameters[5].Value = employment_temp.StartMonth;
                    parameters[6].Value = employment_temp.EndYear;
                    parameters[7].Value = employment_temp.EndMonth;
                    parameters[8].Value = employment_temp.YearsOnWork;

                    parameters[9].Value = employment_temp.Phone;
                    //parameters[10].Value = employment_temp.ContactBranchId;
                    parameters[10].Value = DBNull.Value;
                    parameters[11].Value = employment_temp.CompanyName;
                    parameters[12].Value = employment_temp.Address;
                    parameters[13].Value = employment_temp.City;
                    parameters[14].Value = employment_temp.State;
                    parameters[15].Value = employment_temp.Zip;

                    int rows = 0;

                    EmplId =
                        DbHelperSQL.RunProcedure("ProspectEmployment_Save", parameters, out rows);

                    if (i == 0)
                    {
                        current_emplid = EmplId;
                    }

                }
                catch (Exception e)
                {
                    err = "Failed to save Prospect Employment due to database error, Exception:" + e.Message;
                    Trace.TraceError(err);
                    return EmplId;
                }
            }
            return EmplId;
        }

        public int Save_ProspectAssets(Common.Record.Contacts contacts_record, int contactId, ref string err)
        {
            err = "";
            int idx = 0;
            int ProspectAssetId = 0;

            Record.Assets assets_temp = new Record.Assets();

            idx = contacts_record.assets.Count;

            for (int i = 0; i < idx; i++)
            {

                assets_temp = contacts_record.assets[i];

                try
                {

                    SqlParameter[] parameters = { 
                    new SqlParameter("@ProspectAssetId", SqlDbType.Int, 4),         //0               
                    new SqlParameter("@ContactId", SqlDbType.Int, 4),      //1
                    new SqlParameter("@Name", SqlDbType.NVarChar, 50),      //2
                    new SqlParameter("@Account", SqlDbType.NVarChar, 50), //3
                    new SqlParameter("@Amount", SqlDbType.Decimal, 11),     //4                    
                    new SqlParameter("@Type", SqlDbType.NVarChar, 100)       //5                   
                 };

                    if (contactId <= 0)
                    {
                        return contactId;
                    }
                    else
                    {
                        parameters[0].Value = DBNull.Value;
                        parameters[0].Direction = ParameterDirection.Output;
                        parameters[1].Value = contactId;
                    }

                    parameters[2].Value = assets_temp.Name;
                    parameters[3].Value = assets_temp.Account;
                    parameters[4].Value = assets_temp.Amount;
                    parameters[5].Value = assets_temp.Type;

                    int rows = 0;

                    ProspectAssetId =
                        DbHelperSQL.RunProcedure("ProspectAssets_Save", parameters, out rows);

                }
                catch (Exception e)
                {
                    err = "Failed to save Prospect Assets due to database error, Exception:" + e.Message;
                    Trace.TraceError(err);
                    return ProspectAssetId;
                }
            }
            return ProspectAssetId;
        }

        public int Save_ProspectIncome(Common.Record.Contacts contacts_record, int contactId, int current_emplid, ref string err)
        {
            err = "";
            int ProspectIncomeId = 0;

            Record.Income income_temp = new Record.Income();

            income_temp = contacts_record.income;

            try
            {
                SqlParameter[] parameters = { 
                new SqlParameter("@ProspectIncomeId", SqlDbType.Int, 4),    //0               
                new SqlParameter("@ContactId", SqlDbType.Int, 4),           //1
                new SqlParameter("@Salary", SqlDbType.Decimal, 11),             //2
                new SqlParameter("@Overtime", SqlDbType.Decimal, 9),           //3
                new SqlParameter("@Bonuses", SqlDbType.Decimal, 9),            //4                    
                new SqlParameter("@Commission", SqlDbType.Decimal, 9),         //5   
                new SqlParameter("@Div_Int", SqlDbType.Decimal, 9),            //6
                new SqlParameter("@NetRent", SqlDbType.Decimal, 9),            //7
                new SqlParameter("@Other", SqlDbType.Decimal, 9),              //8                    
                new SqlParameter("@EmplId", SqlDbType.Int, 4)               //9                   
                 };

                if (contactId <= 0)
                {
                    return contactId;
                }
                else
                {
                    parameters[0].Value = DBNull.Value;
                    parameters[0].Direction = ParameterDirection.Output;
                    parameters[1].Value = contactId;
                }

                parameters[2].Value = income_temp.Salary;
                parameters[3].Value = income_temp.Overtime;
                parameters[4].Value = income_temp.Bonuses;
                parameters[5].Value = income_temp.Commission;
                parameters[6].Value = income_temp.Div_Int;
                parameters[7].Value = income_temp.NetRent;
                parameters[8].Value = income_temp.Other;
                parameters[9].Value = current_emplid;

                int rows = 0;

                ProspectIncomeId =
                    DbHelperSQL.RunProcedure("ProspectIncome_Save", parameters, out rows);

                return ProspectIncomeId;

            }
            catch (Exception e)
            {
                err = "Failed to save Prospect Income due to database error, Exception:" + e.Message;
                Trace.TraceError(err);
                return ProspectIncomeId;
            }

            return ProspectIncomeId;
        }

        public int Save_ProspectOtherIncome(Common.Record.Contacts contacts_record, int contactId, ref string err)
        {
            err = "";
            int idx = 0;
            int ProspectOtherIncomeId = 0;

            Record.OtherIncome otherincome_temp = new Record.OtherIncome();

            idx = contacts_record.otherincome.Count;

            for (int i = 0; i < idx; i++)
            {

                otherincome_temp = contacts_record.otherincome[i];

                try
                {
                    SqlParameter[] parameters = {
                new SqlParameter("@ProspectOtherIncomeId", SqlDbType.Int, 4),    //0               
                new SqlParameter("@ContactId", SqlDbType.Int, 4),                //1
                new SqlParameter("@Type", SqlDbType.NVarChar, 100),              //2
                new SqlParameter("@MonthlyIncome", SqlDbType.Decimal, 9)         //3              
                 };

                    if (contactId <= 0)
                    {
                        return contactId;
                    }
                    else
                    {
                        parameters[0].Value = DBNull.Value;
                        parameters[0].Direction = ParameterDirection.Output;
                        parameters[1].Value = contactId;
                    }

                    parameters[2].Value = otherincome_temp.Type;
                    parameters[3].Value = otherincome_temp.MonthlyIncome;

                    int rows = 0;

                    ProspectOtherIncomeId =
                        DbHelperSQL.RunProcedure("ProspectOtherIncome_Save", parameters, out rows);
                }
                catch (Exception e)
                {
                    err = "Failed to save Prospect Other Income due to database error, Exception:" + e.Message;
                    return ProspectOtherIncomeId;
                }
            }
            return ProspectOtherIncomeId;
        }

        public int Save_Prospect(Common.Table.Prospect prospect, ref string err)
        {
            err = "";

            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@ContactId", SqlDbType.Int, 4),                  //0               
                    new SqlParameter("@LeadSource", SqlDbType.NVarChar, 255),                   //1
                    new SqlParameter("@ReferenceCode", SqlDbType.NVarChar, 255),                 //2
                    new SqlParameter("@Referral", SqlDbType.Int, 4),               //3
                    new SqlParameter("@Created", SqlDbType.DateTime ),           //4
                    new SqlParameter("@CreatedBy", SqlDbType.Int, 4),            //5
                    new SqlParameter("@Modifed", SqlDbType.DateTime ),         //6
                    new SqlParameter("@ModifiedBy", SqlDbType.Int, 4),        //7
                    new SqlParameter("@Loanofficer", SqlDbType.Int, 4 ),          //8
                    new SqlParameter("@Status", SqlDbType.NVarChar, 50),            //9
                    new SqlParameter("@CreditRanking", SqlDbType.NVarChar, 50),     // 10
                    new SqlParameter("@PreferredContact", SqlDbType.NVarChar, 50),   // 11
                    new SqlParameter("@Dependents", SqlDbType.Bit, 1)         // 12
                  };

                if (prospect.ContactId <= 0)
                {
                    parameters[0].Value = DBNull.Value;
                    parameters[0].Direction = ParameterDirection.Output;
                }
                else
                    parameters[0].Value = prospect.ContactId;

                parameters[1].Value = prospect.LeadSource;
                parameters[2].Value = prospect.ReferenceCode;
                parameters[3].Value = prospect.Referral;
                parameters[4].Value = DateTime.Now;
                parameters[5].Value = prospect.CreatedBy;
                parameters[6].Value = DateTime.Now;
                parameters[7].Value = prospect.ModifiedBy;
                parameters[8].Value = prospect.LoanOfficer;
                parameters[9].Value = prospect.Status;
                parameters[10].Value = prospect.CreditRanking;
                parameters[11].Value = prospect.PreferredContact;
                parameters[12].Value = prospect.Dependents;

                int rows = 0;
                int cId = 0;
                cId = DbHelperSQL.RunProcedure("Prospect_Save", parameters, out rows);
                return cId;

            }
            catch (Exception e)
            {
                err = "Failed to save Prospect due to database error, Exception:" + e.Message;
                Trace.TraceError(err);
                return -1;
            }
        }

        public int Save_Prospect(int contactId, int UserID, ref string err)
        {
            err = "";
            string status = "Active";

            try
            {
                string sqlCmd = string.Format("Select top 1 ContactId from Prospect where ContactId={0}", contactId);
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                int cId = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
                if (cId > 0)
                {
                    return contactId;
                }

                SqlParameter[] parameters = {
                    new SqlParameter("@ContactId", SqlDbType.Int, 4),                  //0               
                    new SqlParameter("@LeadSource", SqlDbType.NVarChar, 255),                   //1
                    new SqlParameter("@ReferenceCode", SqlDbType.NVarChar, 255),                 //2
                    new SqlParameter("@Referral", SqlDbType.NVarChar, 255),               //3
                    new SqlParameter("@Created", SqlDbType.DateTime ),           //4
                    new SqlParameter("@CreatedBy", SqlDbType.Int, 4),                      //5
                    new SqlParameter("@Modifed", SqlDbType.DateTime ),         //6
                    new SqlParameter("@ModifiedBy", SqlDbType.Int, 4),        //7
                    new SqlParameter("@Loanofficer", SqlDbType.Int, 4 ),          //8
                    new SqlParameter("@Status", SqlDbType.NVarChar, 50),            //9
             
                  };

                if (contactId <= 0)
                {
                    parameters[0].Value = DBNull.Value;
                    parameters[0].Direction = ParameterDirection.Output;
                }
                else
                    parameters[0].Value = contactId;

                parameters[1].Value = DBNull.Value;
                parameters[2].Value = DBNull.Value;
                parameters[3].Value = DBNull.Value;
                parameters[4].Value = DateTime.Now;
                parameters[5].Value = UserID;
                parameters[6].Value = DBNull.Value;
                parameters[7].Value = DBNull.Value;
                parameters[8].Value = UserID;
                parameters[9].Value = status;

                int rows = 0;
                cId = 0;
                cId = DbHelperSQL.RunProcedure("Prospect_Save", parameters, out rows);
                if (contactId > 0)
                {
                    string outcontactId = contactId.ToString();
                    return contactId;
                }
                else
                {
                    if (cId > 0)
                        return cId;
                }

                return -1;
            }
            catch (Exception e)
            {
                err = "Failed to save Prospect due to database error, Exception:" + e.Message;
                Trace.TraceError(err);
                return -1;
            }
        }
        public bool ReplaceLoanStageTasks(int FileId, Table.LoanStages ls, int WorkflowTemplateId, int WorkflowStageId, out bool StageCompleted, ref string err)
        {
            bool logErr = false;
            StageCompleted = false;
            string sqlCmd = string.Empty;
            try
            {
                int LoanStageId = 0; int Sequence = 0; short CalculationMethod = 0;
                if (FileId <= 0 || ls == null || WorkflowTemplateId <= 0 || WorkflowStageId <= 0 || ls.LoanStageId <= 0)
                {
                    err = string.Format("ReplaceLoanStageTasks, Invalid FileId {0} oR LoanStage IS NULL or WorkflowStageId {1}, or WorkflowTemplateId {2}, LoanStageId {3}", FileId, WorkflowStageId, WorkflowTemplateId, ls.LoanStageId);
                    int Event_id = 3059;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return false;
                }
                string stageName = string.Empty;
                sqlCmd = string.Format("Select ts.Alias, ws.SequenceNumber, ws.CalculationMethod from Template_Stages ts inner join Template_Wfl_Stages ws on ws.TemplStageId=ts.TemplStageId where ws.WflStageId={0}", WorkflowStageId);
                DataSet ds2 = DbHelperSQL.Query(sqlCmd);
                if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                {
                    DataRow dr2 = ds2.Tables[0].Rows[0];
                    stageName = dr2["Alias"] == DBNull.Value ? string.Empty : (string)dr2["Alias"];
                    Sequence = (short)(dr2["SequenceNumber"] == DBNull.Value ? 0 : dr2["SequenceNumber"]);
                    CalculationMethod = (short)(dr2["CalculationMethod"] == DBNull.Value ? 0 : dr2["CalculationMethod"]);
                }
                LoanStageId = ls.LoanStageId;

                if (LoanStageId > 0)
                {
                    sqlCmd = string.Format("Update LoanStages set WflStageId={0}, SequenceNumber={1}, WflTemplId={2}, StageName='{3}', CalculateMethod={6} Where LoanStageId={4} and FileId={5}", WorkflowStageId, Sequence, WorkflowTemplateId, stageName, LoanStageId, FileId, CalculationMethod);
                    DbHelperSQL.ExecuteSql(sqlCmd);
                    sqlCmd = string.Format("Delete LoanTasks where ((LoanStageId={0} AND COMPLETED IS NULL) OR (LoanStageId IS NULL)) AND FileId={1}", LoanStageId, FileId);
                    DbHelperSQL.ExecuteSql(sqlCmd);
                }

                sqlCmd = string.Format("Select LoanTaskId from LoanTasks where LoanStageId={0} and Completed IS NULL and TemplTaskId not in (select TemplTaskId from Template_Wfl_Tasks where WflStageId={1})", LoanStageId, WorkflowStageId);
                DataSet ds1 = DbHelperSQL.Query(sqlCmd);
                if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                {
                    int LoanTaskId = 0;

                    foreach (DataRow dr1 in ds1.Tables[0].Rows)
                    {
                        LoanTaskId = dr1["LoanTaskId"] == DBNull.Value ? 0 : (int)dr1["LoanTaskId"];
                        if (LoanTaskId <= 0)
                            continue;
                        sqlCmd = string.Format("DELETE dbo.LoanTask_CompletionEmails where LoanTaskId={0}; EXEC [dbo].[lpsp_RemoveTaskAlerts] {0}", LoanTaskId);
                        DbHelperSQL.ExecuteNonQuery(sqlCmd);
                    }
                }

                SqlParameter[] parameters = {
                                                new SqlParameter("@FileID", SqlDbType.Int),
                                                new SqlParameter("@WorkflowTemplateId", SqlDbType.Int),
                                                new SqlParameter("@WorkflowStageId", SqlDbType.Int),
                                                new SqlParameter("@LoanStageId", SqlDbType.Int)
                                             };
                parameters[0].Value = FileId;
                parameters[1].Value = WorkflowTemplateId;
                parameters[2].Value = WorkflowStageId;
                parameters[3].Value = LoanStageId;
                DbHelperSQL.RunProcedure("dbo.[ReplaceLoanStageTasks]", parameters);

                SqlParameter[] parameters1 = {
                                                new SqlParameter("@FileID", SqlDbType.Int),
                                                new SqlParameter("@WorkflowTemplateId", SqlDbType.Int),
                                                new SqlParameter("@WorkflowStageId", SqlDbType.Int)
                                             };
                parameters1[0].Value = FileId;
                parameters1[1].Value = WorkflowTemplateId;
                parameters1[2].Value = WorkflowStageId;
                DbHelperSQL.RunProcedure("dbo.[SetUpTaskDueDates]", parameters1);
                sqlCmd = string.Format("select Completed from LoanStages where LoanStageId = {0} and FileId={1}", LoanStageId, FileId);
                object obj1 = DbHelperSQL.GetSingle(sqlCmd);
                DateTime complDate = (obj1 == null || obj1 == DBNull.Value) ? DateTime.MinValue : (DateTime)obj1;
                if (complDate != DateTime.MinValue)
                {
                    StageCompleted = true;
                }

                return true;
            }
            catch (Exception ex)
            {
                err = "ReplaceLoanStageTasks, Exception:" + ex.Message;
                int Event_id = 3060;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 3000;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        public bool SaveLoanWflTempl(int FileId, int WorkflowTemplateId, int Userid, ref string err)
        {
            string sqlCmd = string.Format("Select count(1) from LoanWflTempl where FileId={0}", FileId);
            try
            {
                SqlParameter[] parameters = {                    
                     new SqlParameter("@FileId", SqlDbType.Int),                                //0
                     new SqlParameter("@WfTemplId", SqlDbType.Int),                      //1
                     new SqlParameter("@UserId", SqlDbType.Int)                             //2
                   };

                parameters[0].Value = FileId;
                parameters[1].Value = WorkflowTemplateId;
                parameters[2].Value = Userid;
                int rows = 0;

                DbHelperSQL.RunProcedure("lpsp_ApplyLoanWflTempl", parameters, out rows);
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("SaveLoanWflTempl, FileId={0}, WorkflowTemplId={1}, Exception:{2}", FileId, WorkflowTemplateId, ex.Message);
                Trace.TraceError(err);
                int Event_id = 3062;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
        }

        public bool GenerateLoanTaskByStage(int FileId, int WorkflowTemplateId, int WorkflowStageId, ref string err)
        {
            bool logErr = false;
            try
            {
                string sqlCmd = string.Format("Select top 1 LoanStageId, ws.SequenceNumber as SequenceNumber from LoanStages ls inner join Template_Wfl_Stages ws on ls.StageName=ws.[Name] where ws.WflStageId={0} and FileId={1}", WorkflowStageId, FileId);
                DataSet ds = DbHelperSQL.Query(sqlCmd);
                DataRow dr = null;
                int LoanStageId = 0;
                int Sequence = 0;
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    dr = ds.Tables[0].Rows[0];
                    LoanStageId = dr["LoanStageId"] == null || dr["LoanStageId"] == DBNull.Value ? 0 : (int)dr["LoanStageId"];
                    Sequence = dr["SequenceNumber"] == null || dr["SequenceNumber"] == DBNull.Value ? 0 : (short)dr["SequenceNumber"];
                    if (LoanStageId > 0)
                    {
                        sqlCmd = string.Format("Update LoanStages set WflStageId={0}, SequenceNumber={1}, WflTemplId={2} Where LoanStageId={3} and FileId={4}", WorkflowStageId, Sequence, WorkflowTemplateId, LoanStageId, FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd);
                    }
                }
                SqlParameter[] parameters ={
                            new SqlParameter("@FileId", SqlDbType.Int, 4),
                            new SqlParameter("@WorkflowTemplateId", SqlDbType.Int, 4),
                            new SqlParameter("@WorkflowStageId", SqlDbType.Int, 4)
                         };
                parameters[0].Value = FileId;
                parameters[1].Value = WorkflowTemplateId;
                parameters[2].Value = WorkflowStageId;
                int rowsAffected = 0;
                DbHelperSQL.RunProcedure("GenerateLoanTasksForStage", parameters, out rowsAffected);
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("GenerateLoanTaskByStage, FileId={0}, WorkflowTemplId={1}, WorkflowStageId={2}, Exception:{3}", FileId, WorkflowTemplateId, WorkflowStageId, ex.Message);
                int Event_id = 3063;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 3064;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        public bool GenerateMultipleCompletionEmails(int FileId, ref string err)
        {
            DataSet ds = null;
            DataSet ds1 = null;
            bool enabled = true;
            bool logErr = false;
            string sqlCmd5 = string.Empty;

            string sqlCmd = "Select LoanTaskId, TemplTaskId From LoanTasks WHERE [FileId]=" + FileId;

            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    return true;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int LoanTaskId = 0;
                    int TemplTaskId = 0;

                    if (dr["LoanTaskId"] == DBNull.Value)
                        continue;

                    if (dr["TemplTaskId"] == DBNull.Value)
                        continue;

                    LoanTaskId = (int)dr["LoanTaskId"];
                    TemplTaskId = (int)dr["TemplTaskId"];

                    string sqlCmd1 = "Select TemplEmailId From Template_Wfl_CompletionEmails WHERE (Enabled=1) and [TemplTaskid]=" + TemplTaskId;
                    ds1 = DbHelperSQL.Query(sqlCmd1);
                    sqlCmd5 = string.Format("Delete LoanTask_CompletionEmails where LoanTaskid='{0}'", LoanTaskId);
                    DbHelperSQL.ExecuteSql(sqlCmd5);

                    if ((ds1 == null) || (ds1.Tables[0].Rows.Count <= 0))
                        continue;

                    foreach (DataRow dr1 in ds1.Tables[0].Rows)
                    {

                        int TemplEmailId = 0;

                        if (dr1["TemplEmailId"] == DBNull.Value)
                            continue;

                        TemplEmailId = (int)dr1["TemplEmailId"];

                        sqlCmd = string.Format("Insert into LoanTask_CompletionEmails (LoanTaskid, TemplEmailId, Enabled) values ({0}, {1}, 1)", LoanTaskId, TemplEmailId);
                        DbHelperSQL.ExecuteSql(sqlCmd);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                err = "GenerateMultipleCompletionEmails, Exception:" + ex.Message;
                int Event_id = 3065;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 3066;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        public bool DeleteObsoleteLoanStage(int FileId, Table.WorkflowStage wsStage, ref string err)
        {
            if (FileId <= 0 || wsStage == null)
            {
                err = string.Format("DeleteObsoleteLoanStage, Invalid Fileid {0} or WsStage=null", FileId);
                int Event_id = 3067;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            if (wsStage.WorkflowStageId <= 0)
            {
                err = string.Format("DeleteObsoleteLoanStage, Invalid Workflow StageId {0}, FileId {1} ", wsStage.WorkflowStageId, FileId);
                int Event_id = 3068;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            DataSet ds = null;
            try
            {
                string sqlCmd = string.Format("select * from LoanStages where StageName='{0}' and FileId={1} and WflStageId <> {2}", wsStage.Name, FileId, wsStage.WorkflowStageId);
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DeleteLoanStage(FileId, (int)dr["LoanStageId"], ref err);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("DeleteObsoleteLoanStage, Workflow StageId {0}, FileId {1}, Exception: {2} ", wsStage.WorkflowStageId, FileId, ex.Message);
                int Event_id = 3069;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
        }
        public bool CleanupObsoleteLoanStagesTasks(int FileId, int WflTemplId, ref string err)
        {
            if (FileId <= 0)
            {
                err = string.Format("CleanupObsoleteLoanStagesTasks, Invalid Fileid {0}", FileId);
                int Event_id = 3070;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            DataSet ds = null;
            try
            {
                string sqlCmd = string.Format("select LoanStageId from LoanStages where FileId={0} and SequenceNumber=-1", FileId);
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                    return true;
                int loanStageId = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    loanStageId = dr["LoanStageId"] == DBNull.Value ? 0 : (int)dr["LoanStageId"];
                    if (loanStageId <= 0)
                        continue;
                    DeleteLoanStage(FileId, loanStageId, ref err);
                }

                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("CleanupObsoleteLoanStagesTasks, Invalid Fileid {0}, Exception: {1}", FileId, ex.Message);
                int Event_id = 3071;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
        }

        public bool InitObsoleteLoanStagesTasks(int FileId, int WflTemplId, ref string err)
        {
            if (FileId <= 0)
            {
                err = string.Format("InitLoanStageDuringReGen, Invalid Fileid {0}", FileId);
                int Event_id = 3072;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            try
            {
                string sqlCmd = string.Format("Update LoanStages set SequenceNumber=-1 where FileId={0} AND (WflTemplId IS NULL OR WflStageId IS NULL OR " +
                                              " WflStageId not in (select WflStageId from Template_Wfl_Stages where WflTemplId={1}))", FileId, WflTemplId);
                DbHelperSQL.ExecuteSql(sqlCmd);

                sqlCmd = string.Format("Delete LoanTasks where LoanStageId IS NULL and FileId={0}", FileId);
                DbHelperSQL.ExecuteSql(sqlCmd);
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("InitLoanStageDuringReGen, Invalid Fileid {0}, Exception: {1}", FileId, ex.Message);
                int Event_id = 3073;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
        }

        public bool DeleteLoanStage(int FileId, int LoanStageId, ref string err)
        {
            bool logErr = false;
            err = string.Empty;
            try
            {
                SqlParameter[] parameters ={
                            new SqlParameter("@LoanStageId", SqlDbType.Int)
                         };
                parameters[0].Value = LoanStageId;
                int rowsAffected = 0;
                DbHelperSQL.RunProcedure("[DeleteLoanStage]", parameters, out rowsAffected);
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("DeleteLoanStage, FileId={0}, LoanStageId={1}, Exception: {2}", FileId, LoanStageId, ex.Message);
                int Event_id = 3074;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 3075;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    Trace.TraceError(err);
                }
            }
        }

        public bool GenerateLoanStages(int FileId, int WorkflowTemplateId, ref string err)
        {
            bool logErr = false;
            try
            {
                SqlParameter[] parameters ={
                            new SqlParameter("@FileId", SqlDbType.Int, 4),
                            new SqlParameter("@WorkflowTemplateId", SqlDbType.Int, 4)
                         };
                parameters[0].Value = FileId;
                parameters[1].Value = WorkflowTemplateId;
                int rowsAffected = 0;
                DbHelperSQL.RunProcedure("GenerateLoanStages", parameters, out rowsAffected);
                return true;
            }
            catch (Exception ex)
            {
                err = "File ID: " + FileId + " GenerateLoanStages, Exception:" + ex.Message;
                int Event_id = 3076;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 3077;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }

        }

        public List<Table.WorkflowTask> GetWorkflowTasksByStage(int WorkflowStageId, ref string err)
        {
            bool logErr = false;
            List<Table.WorkflowTask> TaskList = null;
            DataSet ds = null;
            err = "";
            string sqlCmd = "Select TemplTaskId, WflStageId, SequenceNumber, DaysDueFromCoe, Name, PrerequisiteTaskId, DaysDueAfterPrerequisite, OwnerRoleId, WarningEmailId, OverdueEmailId, CompletionEmailId " +
                                        " From Template_Wfl_Tasks WHERE WflStageId=" + WorkflowStageId + " AND Enabled=1";
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    return TaskList;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["TemplTaskId"] == DBNull.Value)
                        continue;
                    if (dr["WflStageId"] == DBNull.Value)
                        continue;
                    if (dr["Name"] == DBNull.Value)
                        continue;
                    Table.WorkflowTask ti = new Table.WorkflowTask();
                    ti.WorkflowStageId = (int)dr["WflStageId"];
                    ti.WorkflowTaskId = (int)dr["TemplTaskId"];
                    ti.Name = dr["Name"].ToString().Trim();

                    if (dr["SequenceNumber"] == DBNull.Value)
                        ti.SequenceNumber = -1;
                    else
                        ti.SequenceNumber = (int)dr["SequenceNumber"];

                    if (dr["DaysDueFromCoe"] == DBNull.Value)
                        ti.DaysFromEstClose = -99999;
                    else
                        ti.DaysFromEstClose = (int)dr["DaysDueFromCoe"];

                    if (dr["PrerequisiteTaskId"] == DBNull.Value)
                        ti.PrerequisiteTaskId = -1;
                    else
                        ti.PrerequisiteTaskId = (int)dr["PrerequisiteTaskId"];

                    if (dr["DaysDueAfterPrerequisite"] == DBNull.Value)
                        ti.DaysAfterPrereq = 0;
                    else
                        ti.DaysAfterPrereq = (int)dr["DaysDueAfterPrerequisite"];

                    if (dr["OwnerRoleId"] == DBNull.Value)
                        ti.OwnerRoleId = (int)dr["OwnerRoleId"];
                    else
                        ti.OwnerRoleId = -1;

                    if (dr["WarningEmailId"] == DBNull.Value)
                        ti.WarningEmailId = -1;
                    else
                        ti.WarningEmailId = (int)dr["WarningEmailId"];

                    if (dr["OverdueEmailId"] == DBNull.Value)
                        ti.OverdueEmailId = -1;
                    else
                        ti.OverdueEmailId = (int)dr["OverdueEmailId"];

                    if (dr["CompletionEmailId"] == DBNull.Value)
                        ti.CompletionEmailId = -1;
                    else
                        ti.CompletionEmailId = (int)dr["CompletionEmailId"];
                    TaskList.Add(ti);
                }
                return TaskList;
            }
            catch (Exception ex)
            {
                err = "GetWorkflowStages, Exception: " + ex.Message;
                int Event_id = 3078;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return TaskList;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
                if (logErr)
                {
                    int Event_id = 3079;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        public List<Table.LoanStages> GetLoanStagesByFileId(int FileId, ref string err)
        {
            bool logErr = false;
            List<Table.LoanStages> StageList = null;
            DataSet ds = null;
            err = "";
            //string sqlCmd = "Select LoanStageId, WflTemplId, WflStageId, SequenceNumber, DaysFromEstClose, StageName, Completed From LoanStages WHERE FileId=" + FileId + " ORDER BY SequenceNumber ASC";
            string sqlCmd = "Select ls.LoanStageId, ls.WflTemplId, ls.WflStageId, ls.SequenceNumber, ls.DaysFromEstClose, ls.StageName, ls.Completed, ts.PointStageNameField, ts.PointStageDateField  From LoanStages ls inner join Template_Wfl_Stages ws on ls.WflStageId=ws.WflStageId join Template_Stages ts on ws.TemplStageId=ts.TemplStageId  WHERE FileId=" + FileId + " ORDER BY SequenceNumber ASC";
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    return StageList;
                StageList = new List<Table.LoanStages>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["LoanStageId"] == DBNull.Value)
                        continue;
                    if (dr["StageName"] == DBNull.Value)
                        continue;
                    Table.LoanStages si = new Table.LoanStages();
                    si.LoanStageId = (int)dr["LoanStageId"];
                    si.StageName = dr["StageName"].ToString().Trim();
                    if (dr["WflTemplId"] == DBNull.Value)
                        si.WflTemplId = -1;
                    else
                        si.WflTemplId = (int)dr["WflTemplId"];
                    if (dr["WflStageId"] == DBNull.Value)
                        si.WflStageId = -1;
                    else
                        si.WflStageId = (int)dr["WflStageId"];
                    if (dr["SequenceNumber"] == DBNull.Value)
                        si.SequenceNumber = -1;
                    else
                        si.SequenceNumber = (short)dr["SequenceNumber"];
                    if (dr["DaysFromEstClose"] == DBNull.Value)
                        si.DaysFromEstClose = -1;
                    else
                        si.DaysFromEstClose = (short)dr["DaysFromEstClose"];
                    si.Completed = (dr["Completed"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["Completed"];
                    if (dr["PointStageDateField"] == DBNull.Value)
                        si.PointDateField = -1;
                    else
                        si.PointDateField = (short)dr["PointStageDateField"];
                    if (dr["PointStageNameField"] == DBNull.Value)
                        si.PointNameField = -1;
                    else
                        si.PointNameField = (short)dr["PointStageNameField"];
                    sqlCmd = string.Format("Select count(1) from LoanTasks where FileId={0} and LoanStageId={1}", FileId, si.LoanStageId);
                    object obj = DbHelperSQL.GetSingle(sqlCmd);
                    si.TaskCount = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
                    StageList.Add(si);
                }
                return StageList;
            }
            catch (Exception ex)
            {
                err = "File ID: " + FileId + " GetLoanStagesByFileId, Exception: " + ex.Message;
                int Event_id = 3080;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return StageList;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 3081;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }

        }

        public List<Table.WorkflowStage> GetWorkflowStagesByWorkflowTemplate(int WorkflowTemplId, ref string err)
        {
            bool logErr = false;
            List<Table.WorkflowStage> StageList = null;
            DataSet ds = null;
            err = "";
            string sqlCmd = "Select ws.WflStageId, ws.SequenceNumber, ws.DaysFromEstClose, ws.Name, ts.Alias, ws.TemplStageId From Template_Wfl_Stages ws inner join Template_Stages ts on ws.TemplStageId=ts.TemplStageId WHERE WflTemplId=" + WorkflowTemplId + " AND ws.Enabled=1 ORDER BY ws.SequenceNumber ASC";
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    return StageList;
                StageList = new List<Table.WorkflowStage>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["WflStageId"] == DBNull.Value)
                        continue;
                    if (dr["Name"] == DBNull.Value)
                        continue;
                    Table.WorkflowStage si = new Table.WorkflowStage();
                    si.WorkflowTemplId = WorkflowTemplId;
                    si.WorkflowStageId = (int)dr["WflStageId"];
                    si.Name = dr["Alias"] == DBNull.Value ? string.Empty : dr["Alias"].ToString().Trim();
                    if (string.IsNullOrEmpty(si.Name))
                        si.Name = dr["Name"].ToString().Trim();
                    if (dr["SequenceNumber"] == DBNull.Value)
                        si.SequenceNumber = -1;
                    else
                        si.SequenceNumber = (short)dr["SequenceNumber"];
                    if (dr["DaysFromEstClose"] == DBNull.Value)
                        si.DaysFromEstClose = -1;
                    else
                        si.DaysFromEstClose = (short)dr["DaysFromEstClose"];
                    si.TemplStageId = dr["TemplStageId"] == DBNull.Value ? 0 : (int)dr["TemplStageId"];
                    StageList.Add(si);
                }
                return StageList;
            }
            catch (Exception ex)
            {
                err = "GetWorkflowStages, Exception: " + ex.Message;
                int Event_id = 3082;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return StageList;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 3083;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }

        }

        string GetLastCompletedStage(int fileId, string currentStage)
        {
            string stage = "";
            DataSet ds = null;
            string sqlCmd = "Select SequenceNumber, StageName from LoanStages Where FileId=" + fileId + " and Completed is NOT NULL ORDER BY SequenceNumber";
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    return stage;
                int seqnum = 0;
                int tempNum = 0;
                string tempStage = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["SequenceNumber"] != DBNull.Value)
                        tempNum = (short)dr["SequenceNumber"];
                    else
                        tempNum = 0;
                    if (dr["StageName"] != DBNull.Value)
                        tempStage = dr["StageName"].ToString().Trim();
                    if (tempNum > seqnum)
                    {
                        seqnum = tempNum;
                        stage = tempStage;
                    }
                }
                return stage;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return stage;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
            }
        }

        public bool Setup_LoanStages(int FileId, List<Table.LoanStages> stageList, ref string err)
        {
            err = "";
            if (FileId <= 0)
            {
                err = string.Format("Setup_LoanStages:, invalid FileId:{0}.", FileId);
                return false;
            }
            if ((stageList == null) || (stageList.Count <= 0))
            {
                err = "Setup_LoanStages: Stage List is empty.";
                return false;
            }
            int loanStageId = 0;
            foreach (Table.LoanStages stage in stageList)
            {
                try
                {
                    if (stage.StageName == String.Empty)
                        continue;
                    //if (stage.FileId <= 0)
                    //    continue;
                    loanStageId = Save_LoanStages(FileId, stage, ref err);
                    if (loanStageId <= 0)
                    {
                        Trace.TraceError(err);
                        int Event_id = 3084;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        continue;
                    }
                    stage.LoanStageId = loanStageId;
                    if ((stage.Completed != null) && (stage.Completed != DateTime.MinValue))
                    {
                        SqlParameter[] parameters ={
                            new SqlParameter("@FileId", SqlDbType.Int, 4),
                            new SqlParameter("@LoanStageId", SqlDbType.Int, 4),
                            new SqlParameter("@Completed", SqlDbType.DateTime)
                         };
                        parameters[0].Value = stage.FileId;
                        parameters[1].Value = stage.LoanStageId;
                        parameters[2].Value = stage.Completed;
                        int rowsAffected = 0;
                        DbHelperSQL.RunProcedure("CompleteLoanTasksForStage", parameters, out rowsAffected);
                    }
                }
                catch (Exception ex)
                {
                    err = "Setup_LoanStages, Exception: " + ex.Message;
                    return false;
                }
            }

            //try
            //{
            //    SqlParameter[] parameters1 ={
            //                new SqlParameter("@FileId", SqlDbType.Int, 4)
            //              };
            //    parameters1[0].Value = FileId;
            //    int rowsAffected = 0;
            //    DbHelperSQL.RunProcedure("lpsp_FixStageGap", parameters1, out rowsAffected);
            //}
            //catch (Exception ee)
            //{
            //    err = "Setup_LoanStages, Exception: " + ee.Message;
            //    return false;
            //}
            return true;
        }

        public bool Save_OrganizationInfo(int fileId, int branchId, ref string err)
        {
            err = "";
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@FileId", SqlDbType.Int, 4),                                        //0               
                    new SqlParameter("@BranchId", SqlDbType.Int, 4)
               };
                parameters[0].Value = fileId;
                parameters[1].Value = branchId;

                int rows = 0;
                DbHelperSQL.RunProcedure("Save_Loan_OrgInfo", parameters, out rows);
                return true;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return false;
            }

        }

        public bool Save_RatelockAlert(int fileId, Common.Table.Loans loans_table, ref string err)
        {
            err = "";
            try
            {
                SqlParameter[] parameters = {
                new SqlParameter("@FileId", SqlDbType.Int, 4)
                                        };
                parameters[0].Value = fileId;
                DbHelperSQL.RunProcedure("dbo.[CheckRateLockAlert]", parameters);
            }
            catch (Exception ex)
            {
                err = "Failed to save Rate Lock Alert, Exception:" + ex.Message;
                return false;
            }
            return true;
        }

        public int GetNumberLoanTasks(int fileId, ref string err)
        {
            err = "";
            int numTasks = -1;
            if (fileId <= 0)
            {
                err = "GetNumberLoanTasks, invalid File Id:" + fileId;
                return numTasks;
            }

            try
            {
                string sqlCmd = "Select Count(1) from LoanTasks where Fileid=" + fileId;
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                numTasks = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
                return numTasks;
            }
            catch (Exception ex)
            {
                err = "GetNumberLoanTasks, Exception: " + ex.Message;
                return numTasks; ;
            }
        }

        public bool Remove_Loan(int fileId, ref string err)
        {
            err = "";
            bool logErr = false;
            if (fileId <= 0)
            {
                err = "Remove_Loan, invalid File Id:" + fileId;
                int Event_id = 3085;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@FileId", SqlDbType.Int, 4)            //0               
                                            };
                parameters[0].Value = fileId;
                int rows = 0;
                DbHelperSQL.RunProcedure("dbo.[lpsp_RemoveLoan]", parameters, out rows);
                return true;
            }
            catch (Exception ex)
            {
                err = "Remove_Loan, Exception: " + ex.Message;
                return false;
            }
        }

        public int GetBranchFromBranchName(int branchId, string branchName, ref string err)
        {
            err = "";
            int homeBranchId = -1;
            int savedBranchId = branchId;
            string sqlCmd = "select BranchID from Branches where HomeBranch=1 AND Enabled=1";
            try
            {
                object obj;
                obj = DbHelperSQL.GetSingle(sqlCmd);
                homeBranchId = (obj == null || obj == DBNull.Value) ? -1 : (int)obj;
                if (!string.IsNullOrEmpty(branchName))
                {
                    string sqlCmd1 = string.Format("select BranchID from Branches where [Name] = '{0}' AND Enabled=1", branchName.Trim());
                    obj = DbHelperSQL.GetSingle(sqlCmd1);
                    branchId = (obj == null || obj == DBNull.Value) ? -1 : (int)obj;
                    if (branchId > 0)
                    {
                        return branchId;
                    }
                }
                return homeBranchId;
            }
            catch (Exception ex)
            {
                err = "GetBranchFromBranchName, Exception:" + ex.Message;
                int Event_id = 3087;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return savedBranchId;
            }
        }

        public bool Save_Loans(int fileId, int branchId, string branchName, Table.Loans loans_table, string LO_Name, ref string err)
        {
            int tempBranchId = GetBranchFromBranchName(branchId, branchName, ref err);
            if (tempBranchId > 0)
                branchId = tempBranchId;
            return Save_Loans(fileId, branchId, loans_table, LO_Name, ref err);
        }

        public bool Save_Loans(int fileId, int branchId, Table.Loans loans_table, string LO_Name, ref string err)
        {
            err = "";
            bool logErr = false;
            if (fileId <= 0)
            {
                err = "Save_Loans, invalid File Id:" + fileId;
                int Event_id = 3088;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            if (loans_table == null)
            {
                err = "Save_Loans, Loans record is null, fileId=" + fileId;
                int Event_id = 3089;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }

            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@FileId", SqlDbType.Int, 4),                  //0               
                    new SqlParameter("@AppraisedValue", SqlDbType.Money),                   //1
                    new SqlParameter("@CCScenario", SqlDbType.NVarChar, 50),                 //2
                    new SqlParameter("@CLTV", SqlDbType.Money),               //3
                    new SqlParameter("@County", SqlDbType.NVarChar, 50),           //4
                    new SqlParameter("@DateOpen", SqlDbType.DateTime),                      //5
                    new SqlParameter("@DateSubmit", SqlDbType.DateTime),         //6
                    new SqlParameter("@DateApprove", SqlDbType.DateTime),        //7
                    new SqlParameter("@DateClearToClose", SqlDbType.DateTime),          //8
                    new SqlParameter("@DateDocs", SqlDbType.DateTime),            //9
                    new SqlParameter("@DateFund", SqlDbType.DateTime),              //10
                    new SqlParameter("@DateRecord", SqlDbType.DateTime),                //11
                    new SqlParameter("@DateClose", SqlDbType.DateTime),           //12
                    new SqlParameter("@DateDenied", SqlDbType.DateTime),               //13
                    new SqlParameter("@DateCanceled", SqlDbType.DateTime),           //14
                    new SqlParameter("@DownPay", SqlDbType.Money),     //15
                    new SqlParameter("@EstCloseDate", SqlDbType.DateTime),  //16
                    new SqlParameter("@Lender", SqlDbType.Int, 4),            //17
                    new SqlParameter("@LienPosition", SqlDbType.NVarChar, 50 ),            //18
                    new SqlParameter("@LoanAmount", SqlDbType.Money ),     //19
                    new SqlParameter("@LoanNumber", SqlDbType.NVarChar, 50),     //20
                    new SqlParameter("@LoanType", SqlDbType.NVarChar, 50),      //21
                    new SqlParameter("@LTV", SqlDbType.Money),       //22
                    new SqlParameter("@MonthlyPayment", SqlDbType.Money),     //23
                    new SqlParameter("@LenderNotes", SqlDbType.NVarChar, 255),             //24
                    new SqlParameter("@Occupancy", SqlDbType.NVarChar, 50),      //25
                    new SqlParameter("@Program", SqlDbType.NVarChar, 255),       //26
                    new SqlParameter("@PropertyAddr", SqlDbType.NVarChar, 50),     //27
                    new SqlParameter("@PropertyCity", SqlDbType.NVarChar, 50),             //28
                    new SqlParameter("@PropertyState", SqlDbType.NVarChar, 2),      //29
                    new SqlParameter("@PropertyZip", SqlDbType.NVarChar, 10),       //30
                    new SqlParameter("@Purpose", SqlDbType.NVarChar, 50),     //31
                    new SqlParameter("@Rate", SqlDbType.Money),             //32
                    new SqlParameter("@RateLockExpiration", SqlDbType.DateTime),      //33
                    new SqlParameter("@SalesPrice", SqlDbType.Money),       //34
                    new SqlParameter("@Term", SqlDbType.Int, 2),     //35
                    new SqlParameter("@Due", SqlDbType.Int, 2),             //36L                   
                    new SqlParameter("@DateSuspended", SqlDbType.DateTime),                     //37
                    new SqlParameter("@CurrentStage", SqlDbType.NVarChar, 50),                   //38
                    new SqlParameter("@LastCompletedStage", SqlDbType.NVarChar, 50),      //39
                    new SqlParameter("@Status", SqlDbType.NVarChar, 50),        //40
                    new SqlParameter("@ProspectLoanStatus", SqlDbType.NVarChar, 50),        //41
                    new SqlParameter("@Ranking", SqlDbType.NVarChar, 50),                    //42
                    new SqlParameter("@PurchasedDate", SqlDbType.DateTime)                    //43
                  };

                parameters[0].Value = fileId;
                parameters[1].Value = loans_table.AppraisedValue;
                parameters[2].Value = loans_table.CCScenario;
                parameters[3].Value = loans_table.CLTV;
                parameters[4].Value = loans_table.County;

                if (loans_table.DateOpen == DateTime.MinValue)
                    parameters[5].Value = DBNull.Value;
                else
                    parameters[5].Value = loans_table.DateOpen;

                if (loans_table.DateSubmit == DateTime.MinValue)
                    parameters[6].Value = DBNull.Value;
                else
                    parameters[6].Value = loans_table.DateSubmit;

                if (loans_table.DateApprove == DateTime.MinValue)
                    parameters[7].Value = DBNull.Value;
                else
                    parameters[7].Value = loans_table.DateApprove;

                if (loans_table.DateClearToClose == DateTime.MinValue)
                    parameters[8].Value = DBNull.Value;
                else
                    parameters[8].Value = loans_table.DateClearToClose;

                if (loans_table.DateDocs == DateTime.MinValue)
                    parameters[9].Value = DBNull.Value;
                else
                    parameters[9].Value = loans_table.DateDocs;

                if (loans_table.DateFund == DateTime.MinValue)
                    parameters[10].Value = DBNull.Value;
                else
                    parameters[10].Value = loans_table.DateFund;

                if (loans_table.DateRecord == DateTime.MinValue)
                    parameters[11].Value = DBNull.Value;
                else
                    parameters[11].Value = loans_table.DateRecord;

                if (loans_table.DateClose == DateTime.MinValue)
                    parameters[12].Value = DBNull.Value;
                else
                    parameters[12].Value = loans_table.DateClose;

                if (loans_table.DateDenied == DateTime.MinValue)
                    parameters[13].Value = DBNull.Value;
                else
                    parameters[13].Value = loans_table.DateDenied;

                if (loans_table.DateCanceled == DateTime.MinValue)
                    parameters[14].Value = DBNull.Value;
                else
                    parameters[14].Value = loans_table.DateCanceled;

                parameters[15].Value = loans_table.DownPay;

                if (loans_table.EstCloseDate == DateTime.MinValue)
                    parameters[16].Value = DBNull.Value;
                else
                    parameters[16].Value = loans_table.EstCloseDate;

                parameters[17].Value = loans_table.Lender;
                parameters[18].Value = loans_table.LienPosition;
                parameters[19].Value = loans_table.LoanAmount;

                #region for DataTrac

                if (loans_table.LoanNumber == string.Empty)
                {
                    parameters[20].Value = DBNull.Value;
                }
                else
                {
                    parameters[20].Value = loans_table.LoanNumber;
                }

                #endregion

                parameters[21].Value = loans_table.LoanType;
                parameters[22].Value = loans_table.LTV;
                parameters[23].Value = loans_table.MonthlyPayment;
                parameters[24].Value = loans_table.LenderNotes;
                parameters[25].Value = loans_table.Occupancy;
                parameters[26].Value = loans_table.Program;
                parameters[27].Value = loans_table.PropertyAddr;
                parameters[28].Value = loans_table.PropertyCity;
                parameters[29].Value = loans_table.PropertyState;
                parameters[30].Value = loans_table.PropertyZip;
                parameters[31].Value = loans_table.Purpose;
                parameters[32].Value = loans_table.Rate;

                if (loans_table.RateLockExpiration == DateTime.MinValue)
                    parameters[33].Value = DBNull.Value;
                else
                    parameters[33].Value = loans_table.RateLockExpiration;

                parameters[34].Value = loans_table.SalesPrice;
                parameters[35].Value = loans_table.Term;
                parameters[36].Value = loans_table.Due;

                if (loans_table.DateSuspended == DateTime.MinValue)
                    parameters[37].Value = DBNull.Value;
                else
                    parameters[37].Value = loans_table.DateSuspended;

                parameters[38].Value = loans_table.CurrentStage;
                parameters[39].Value = loans_table.LastCompletedStage;
                parameters[40].Value = loans_table.LoanStatus;
                parameters[41].Value = loans_table.ProspectLoanStatus;
                if (string.IsNullOrEmpty(loans_table.LeadRanking))
                    parameters[42].Value = DBNull.Value;
                else
                    parameters[42].Value = loans_table.LeadRanking;

                if (loans_table.PurchasedDate.HasValue)
                {
                    parameters[43].Value = loans_table.PurchasedDate.Value;
                }
                else
                {
                    parameters[43].Value = DBNull.Value;
                }


                int rows = 0;
                DbHelperSQL.RunProcedure("Loans_Save", parameters, out rows);
                //Save_OrganizationInfo(fileId, branchId, ref err);
                string sqlCmd = "Update Loans set DateHMDA=@DateHMDA, DateProcessing=@DateProcessing, DateReSubmit=@DateReSubmit, DateDocsOut=@DateDocsOut, DateDocsReceived=@DateDocsReceived, LOS_LoanOfficer=@LO, BranchId=@BranchId, PropertyType=@PropertyType Where FileId=@FileId ";

                SqlParameter[] cmdParams = {
                      new SqlParameter("@DateHMDA", SqlDbType.DateTime),
                      new SqlParameter("@DateProcessing", SqlDbType.DateTime),
                      new SqlParameter("@DateReSubmit", SqlDbType.DateTime),
                      new SqlParameter("@DateDocsOut", SqlDbType.DateTime),
                      new SqlParameter("@DateDocsReceived", SqlDbType.DateTime),
                      new SqlParameter("@LO", SqlDbType.NVarChar, 200),
                      new SqlParameter("@BranchId", SqlDbType.Int),
                      new SqlParameter("@FileId", SqlDbType.Int, 4),
                      new SqlParameter("@PropertyType", SqlDbType.NVarChar, 255),
                                            };

                if (loans_table.DateHMDA == DateTime.MinValue)
                    cmdParams[0].Value = DBNull.Value;
                else
                    cmdParams[0].Value = loans_table.DateHMDA;

                if (loans_table.DateProcessing == DateTime.MinValue)
                    cmdParams[1].Value = DBNull.Value;
                else
                    cmdParams[1].Value = loans_table.DateProcessing;

                if (loans_table.DateReSubmit == DateTime.MinValue)
                    cmdParams[2].Value = DBNull.Value;
                else
                    cmdParams[2].Value = loans_table.DateReSubmit;

                if (loans_table.DateDocsOut == DateTime.MinValue)
                    cmdParams[3].Value = DBNull.Value;
                else
                    cmdParams[3].Value = loans_table.DateDocsOut;

                if (loans_table.DateDocsReceived == DateTime.MinValue)
                    cmdParams[4].Value = DBNull.Value;
                else
                    cmdParams[4].Value = loans_table.DateDocsReceived;
                if (string.IsNullOrEmpty(LO_Name))
                    cmdParams[5].Value = DBNull.Value;
                else
                    cmdParams[5].Value = LO_Name;
                cmdParams[6].Value = branchId;
                cmdParams[7].Value = fileId;
                if (string.IsNullOrEmpty(loans_table.PropertyType))
                    cmdParams[8].Value = "";
                else
                    cmdParams[8].Value = loans_table.PropertyType;
                DbHelperSQL.ExecuteSql(sqlCmd, cmdParams);
                Save_RatelockAlert(fileId, loans_table, ref err);

                return true;
            }
            catch (Exception e)
            {
                err = "File ID: " + fileId + " Failed to save Loans due to database error, Loans_Save Error:" + e.ToString();
                int Event_id = 3090;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Error, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 3091;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        public string Get_PointFiles_Name(int FileId, ref string err)
        {
            string pointfiles_Name = null;

            if (FileId <= 0)
                return null;

            try
            {
                object ob = DbHelperSQL.GetSingle("Select Name from PointFiles where FileId=" + FileId);
                if (ob == null || ob == DBNull.Value)
                {
                    return null;
                }

                pointfiles_Name = (string)ob;

                return pointfiles_Name;
            }
            catch (Exception ex)
            {
                err = "Get_PointFiles_Name, Exception: " + ex.Message;
                return null;
            }
        }

        public bool Update_PointFiles_Name(int FileId, string filePath, int folderId, ref string err)
        {
            err = "";

            if (FileId <= 0)
            {
                err = "Update_PointFiles_Name, invalid FileId " + FileId;
                return false;
            }

            try
            {
                //string sqlCmd = "Update PointFiles set [Name]='" + name + "' where FileId=" + FileId;
                //string fileName = SeparatePointFileNameInPath(filePath, folderId);
                string fileName = Path.GetFileName(filePath);
                fileName = fileName.ToUpper().EndsWith(".BRW") ? @"\BORROWER\" + fileName : @"\PROSPECT\" + fileName;
                string sqlCmd = string.Format("Update PointFiles set FolderId={0}, [Name]='{1}' where FileId={2}", folderId, fileName, FileId);
                DbHelperSQL.ExecuteSql(sqlCmd);
                return true;
            }
            catch (Exception ex)
            {
                err = "Update_PointFiles_Name, Exception: " + ex.Message;
                return false;
            }
        }

        public bool Update_Loans_LOS_LoanOfficer(int FileId, string LOS_LoanOfficer, ref string err)
        {
            err = "";

            if (FileId <= 0)
            {
                err = "Update_Loans_LOS_LoanOfficer, invalid FileId " + FileId;
                return false;
            }

            try
            {
                string sqlCmd = "Update Loans set [LOS_LoanOfficer]='" + LOS_LoanOfficer + "' where FileId=" + FileId;
                DbHelperSQL.ExecuteSql(sqlCmd);
                return true;
            }
            catch (Exception ex)
            {
                err = "Update_Loans_LOS_LoanOfficer, Exception: " + ex.Message;
                return false;
            }
        }

        public Table.PointFileInfo GetPointFileInfo(int FileId, ref string err)
        {
            Table.PointFileInfo pf = null;
            if (FileId <= 0)
            {
                err = "GetPointFileInfo, invalid FileId=" + FileId;
                return pf;
            }
            string sqlCmd = "Select top 1 FileId, FolderId, BranchId, Path, LastImported, CurrentImage, PreviousImage, PDSFileId, PDSFolderId from lpvw_GetPointFileInfo WHERE FileId=" + FileId;
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "GetPointFileInfo, no Point File found in the database with FileId= " + FileId;
                    return pf;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr == null)
                {
                    err = "GetPointFileInfo, no Point File found in the database with FileId= " + FileId;
                    return pf;
                }
                pf = new Table.PointFileInfo();

                pf.FileId = (dr["FileId"] == DBNull.Value) ? 0 : (int)dr["FileId"];
                pf.FolderId = (dr["FolderId"] == DBNull.Value) ? 0 : (int)dr["FolderId"];
                pf.BranchId = (dr["BranchId"] == DBNull.Value) ? 0 : (int)dr["BranchId"];
                pf.Path = (dr["Path"] == DBNull.Value) ? string.Empty : dr["Path"].ToString().Trim();
                pf.LastImported = (dr["LastImported"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["LastImported"];
                pf.CurrentImage = (dr["CurrentImage"] == DBNull.Value) ? null : (byte[])dr["CurrentImage"];
                pf.PreviousImage = (dr["PreviousImage"] == DBNull.Value) ? null : (byte[])dr["PreviousImage"];
                pf.PDSFileId = (dr["PDSFileId"] == DBNull.Value) ? 0 : (int)dr["PDSFileId"];
                pf.PDSFolderId = (dr["PDSFolderId"] == DBNull.Value) ? 0 : (int)dr["PDSFolderId"];

                return pf;
            }
            catch (Exception ex)
            {
                err = "GetPointFileInfo, Exception: " + ex.Message;
                Trace.TraceError(err);
                return pf;
            }
        }

        public int GetPDSFileId(int FileId, int PDSFolderId, ref string err)
        {
            int idx = 0;
            int PDSFileId = 0;
            string Name = string.Empty;

            if (PDSFolderId <= 0)
            {
                err = "GetPDSFileId, invalid FolderId=" + PDSFolderId;
                return PDSFileId;
            }

            string SqlCmd = "Select top 1 Name from dbo.PointFiles WHERE FileId=" + FileId;
            try
            {
                object obj = DbHelperSQL.GetSingle(SqlCmd);
                if (obj == null || obj == DBNull.Value)
                {
                    return PDSFileId;
                }
                else
                {
                    Name = (string)obj;
                }

                if (string.IsNullOrEmpty(Name))
                {
                    return 0;
                }

                idx = Name.LastIndexOf("\\");

                Name = Name.Substring(idx + 1);

                PDSFileId = GetPDSPointFileId(Name, PDSFolderId, ref err);
                return PDSFileId;
            }
            catch (Exception ex)
            {
                err = "GetPDSFileId, Exception: " + ex.Message;
                return 0;
            }
        }

        public List<Table.PointFileInfo> GetPointFileInfoByFolderId(int folderId, ref string err)
        {
            err = "";
            List<Table.PointFileInfo> PInfoList = null;
            if (folderId <= 0)
            {
                err = "GetPointFileInfoByFolderId, invalid FolderId=" + folderId;
                return PInfoList;
            }
            string sqlCmd = "Select FileId, FolderId, BranchId, Path, LastImported, CurrentImage, PreviousImage, PDSFileId, PDSFolderId from lpvw_GetPointFileInfo WHERE FolderId=" + folderId;
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "GetPointFileInfoByFolderId, no Point Files found in the database with FolderId= " + folderId;
                    return PInfoList;
                }
                PInfoList = new List<Table.PointFileInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Table.PointFileInfo pInfo = new Table.PointFileInfo();
                    if (dr["FileId"] != DBNull.Value)
                        pInfo.FileId = (int)dr["FileId"];
                    else
                        continue;
                    if (dr["FolderId"] != DBNull.Value)
                        pInfo.FolderId = (int)dr["FolderId"];
                    else
                        continue;
                    if (dr["BranchId"] != DBNull.Value)
                        pInfo.BranchId = (int)dr["BranchId"];
                    if (dr["Path"] != DBNull.Value)
                        pInfo.Path = dr["Path"].ToString();
                    else
                        continue;
                    if (pInfo.Path.Trim() == string.Empty)
                        continue;
                    if (dr["LastImported"] == DBNull.Value)
                        pInfo.LastImported = DateTime.MinValue;
                    else
                        DateTime.TryParse(dr["LastImported"].ToString(), out pInfo.LastImported);

                    if (dr["CurrentImage"] != DBNull.Value)
                    {
                        pInfo.CurrentImage = (byte[])dr["CurrentImage"];
                    }
                    else
                        pInfo.CurrentImage = null;

                    if (dr["PreviousImage"] != DBNull.Value)
                    {
                        pInfo.PreviousImage = (byte[])dr["PreviousImage"];
                    }
                    else
                        pInfo.PreviousImage = null;

                    pInfo.PDSFileId = (dr["PDSFileId"] == DBNull.Value) ? 0 : (int)dr["PDSFileId"];
                    pInfo.PDSFolderId = (dr["PDSFolderId"] == DBNull.Value) ? 0 : (int)dr["PDSFolderId"];

                    PInfoList.Add(pInfo);
                }
                return PInfoList;
            }
            catch (Exception ex)
            {
                err = "GetPointFileInfoByFolderId, Exception: " + ex.Message;
                Trace.TraceError(err);
                return PInfoList;
            }
        }

        private string SeparatePointFileNameInPath(string filepath, int folderId)
        {
            bool logErr = false;
            string err = "";
            string fileName = string.Empty;
            try
            {
                if (folderId <= 0 || filepath == null || filepath == string.Empty)
                {
                    err = String.Format("SeparatePointFileNameInPath, invalid folderId {0}, or filepath {1}.", folderId, filepath);
                    int Event_id = 3092;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return fileName;
                }
                string folder = GetPointFolderPath(folderId, ref err);
                if (folder == null || folder == String.Empty)
                {
                    logErr = true;
                    return fileName;
                }
                int index = folder.Length;
                if (filepath.Length <= index + 5)
                {
                    err = String.Format("SeparatePointFileNameInPath, invalid filepath, {0}.", filepath);
                    int Event_id = 3093;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                    return fileName;
                }

                fileName = filepath.Substring(index, filepath.Length - index);
                return fileName;
            }
            catch (Exception ex)
            {
                err = String.Format("SeparatePointFileNameInPath, folderId {0}, or filepath {1}, Exception:{2}", folderId, filepath, ex.Message);
                int Event_id = 3094;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return fileName;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    int Event_id = 3095;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
        }

        public string GetPointFolderPath(int folderId, ref string err)
        {
            err = "";
            string path = "";
            if (folderId <= 0)
            {
                err = "GetPointFolderPath, invalid FolderId=" + folderId;
                return path;
            }
            string sqlCmd = "Select TOP 1 Path from PointFolders WHERE FolderId =" + folderId;
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "No Point Folder Path found in the database for file Id: " + folderId;
                    return path;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr[0] == DBNull.Value)
                {
                    err = "No Point Folder Path found in the database for file id: " + folderId;
                    return path;
                }
                else
                {
                    path = dr[0].ToString().Trim();
                }
                return path;
            }
            catch (Exception ex)
            {
                err = "GetPointFolderPath, Exception: " + ex.Message;
                return path;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public string GetPointFolderPathByName(string Name, ref string err)
        {
            err = "";
            string path = "";
            if ((Name == "") ||
                (Name == null) ||
                (Name == string.Empty))
            {
                err = "GetPointFolderPathByName, invalid Name=" + Name;
                return path;
            }
            string sqlCmd = "Select TOP 1 Path from PointFolders WHERE Name =" + Name;
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "No Point Folder Path found in the database for Name: " + Name;
                    return path;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr[0] == DBNull.Value)
                {
                    err = "No Point Folder Path found in the database for Name: " + Name;
                    return path;
                }
                else
                {
                    path = dr[0].ToString().Trim();
                }
                return path;
            }
            catch (Exception ex)
            {
                err = "GetPointFolderPathByName, Exception: " + ex.Message;
                return path;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public bool ConvertToLead(int fileId, int userId, ref string err)
        {
            err = string.Empty;
            try
            {
                SqlParameter[] parameters1 = {                    
                new SqlParameter("@FileId", SqlDbType.Int, 4 ),                   //0
                new SqlParameter("@RequestBy", SqlDbType.Int, 4)                  //2
                };

                parameters1[0].Value = fileId;
                if (userId <= 0)
                    parameters1[1].Value = DBNull.Value;
                else
                    parameters1[1].Value = userId;

                int rows = 0;
                DbHelperSQL.RunProcedure("dbo.[lpsp_ConvertToLead]", parameters1, out rows);

                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("ConvertToLead, FileId={0}, Exception:{1}", fileId, ex.Message + ex.StackTrace);
                Trace.TraceError(err);
                return false;
            }

        }

        public bool UpdateLoanStatus(int fileId, string newLoanStatus, int userId, LoanStatusEnum OldFolderStatus, ref string err)
        {
            err = "";
            if (fileId <= 0)
            {
                err = string.Format("UpdateLoanStatus, invalid FileId={0}.", fileId);
                return false;
            }
            string sqlCmd = string.Empty;
            string newStatus = newLoanStatus;
            try
            {
                SqlParameter[] parameters = {                    
                new SqlParameter("@FileId", SqlDbType.Int, 4 ),                   //0
                new SqlParameter("@Status", SqlDbType.NVarChar, 50),              //1
                new SqlParameter("@RequestBy", SqlDbType.Int, 4)                  //2
                };

                parameters[0].Value = fileId;
                parameters[1].Value = newLoanStatus.Trim();
                if (userId <= 0)
                    parameters[2].Value = DBNull.Value;
                else
                    parameters[2].Value = userId;

                int rows = 0;
                DbHelperSQL.RunProcedure("dbo.lpsp_UpdateLoanStatus", parameters, out rows);

                sqlCmd = "Delete dbo.V_ProcessingPipelineInfo where FileId=" + fileId;
                DbHelperSQL.ExecuteSql(sqlCmd);

                SqlParameter[] parameter1 = {                    
                    new SqlParameter("@FileId", SqlDbType.Int, 4 )                   //0
                   };

                parameter1[0].Value = fileId;

                rows = 0;
                DbHelperSQL.RunProcedure("dbo.lpsp_INSERT_V_ProcessingPipelineInfo", parameter1, out rows);

                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("UpdateLoanStatus, FileId={0}, Exception:{1}", fileId, ex.Message + ex.StackTrace);
                Trace.TraceError(err);
                return false;
            }

        }

        public bool DisposeLead(int fileId, string newLeadStatus, int userId, string filePath, PointFolderInfo pf, ref string err)
        {
            err = "";
            if (fileId <= 0 || pf == null || string.IsNullOrEmpty(newLeadStatus))
            {
                err = string.Format("DisposeLead, invalid parameter FileId={0}, LeadStatus={1}, or PointFolder is NULL.", fileId, newLeadStatus);
                return false;
            }
            string sqlCmd = string.Empty;
            string newStatus = newLeadStatus;
            try
            {
                SqlParameter[] parameters = {                    
                new SqlParameter("@FileId", SqlDbType.Int, 4 ),                   //0
                new SqlParameter("@NewLeadStatus", SqlDbType.NVarChar, 50),              //1
                new SqlParameter("@RequestBy", SqlDbType.Int, 4)                  //2
                };

                parameters[0].Value = fileId;
                parameters[1].Value = newLeadStatus.Trim();
                if (userId <= 0)
                    parameters[2].Value = DBNull.Value;
                else
                    parameters[2].Value = userId;

                int rows = 0;

                DbHelperSQL.RunProcedure("dbo.[lpsp_DisposeLead]", parameters, out rows);
                if (rows <= 0)
                    return false;

                Update_PointFiles_Name(fileId, filePath, pf.FolderId, ref err);
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("DisposeLead, FileId={0}, Exception:{1}", fileId, ex.Message + ex.StackTrace);
                Trace.TraceError(err);
                return false;
            }

        }

        public bool MovePointFile(int fileId, int newFolderId, string newFilename, int userId, ref string err)
        {
            err = "";
            if ((fileId <= 0) || (newFolderId <= 0))
            {
                err = string.Format("MovePointFile, invalid FileId={0} or NewFolderId={1}.", fileId, newFolderId);
                return false;
            }

            try
            {
                string fileName = SeparatePointFileNameInPath(newFilename, newFolderId);
                SqlParameter[] parameters = {                   
                    new SqlParameter("@FileId", SqlDbType.Int, 4 ),                   //0
                    new SqlParameter("@NewFolderId", SqlDbType.Int, 4),               //1
                    new SqlParameter("@UserId", SqlDbType.Int, 4),                    //2
                    new SqlParameter("@NewFilename", SqlDbType.NVarChar, 255)
                                            };

                parameters[0].Value = fileId;
                parameters[1].Value = newFolderId;
                if (userId <= 0)
                    parameters[2].Value = DBNull.Value;
                else
                    parameters[2].Value = userId;
                if (string.IsNullOrEmpty(fileName))
                {
                    parameters[3].Value = DBNull.Value;
                    err = String.Format("MovePointFile, newFilename is empty, fileId={0}, newFolderId={1}.", fileId, newFolderId);
                    Trace.TraceError(err);
                    int Event_id = 3096;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
                else
                    parameters[3].Value = fileName.Trim();
                int rows = 0;
                fileId =
                    DbHelperSQL.RunProcedure("dbo.MovePointFile", parameters, out rows);
                return true;
            }
            catch (Exception ex)
            {
                err = "MovePointFile, Exception: " + ex.Message;
                Trace.TraceError(err);
                return false;
            }
        }

        public string GetPointFilePath(int fileId, ref string FolderStatus, ref string err)
        {
            err = "";
            string path = "";
            if (fileId <= 0)
            {
                err = "GetPointFilePath, invalid FileId=" + fileId;
                return path;
            }
            string sqlCmd = "Select TOP 1 pf.Path, f.[Name] as FileName, pf.LoanStatus from PointFiles f inner join PointFolders pf on f.FolderId=pf.FolderId WHERE FileId =" + fileId;
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "No Point File Path found in the database for file Id: " + fileId;
                    return path;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                string temp = dr[0] == DBNull.Value ? string.Empty : dr[0].ToString().Trim();
                if (string.IsNullOrEmpty(temp))
                {
                    err = "No Point File Path found in the database for file id: " + fileId;
                    return path;
                }
                string temp1 = dr[1] == DBNull.Value ? string.Empty : dr[1].ToString().Trim();
                if (string.IsNullOrEmpty(temp1))
                {
                    err = "No Point Filename found in the database for fileId: " + fileId;
                    return path;
                }
                if (!temp1.ToUpper().StartsWith(@"\BORROWER\") && !temp1.ToUpper().StartsWith(@"\PROSPECT\"))
                {
                    err = string.Format(@"Point Filename {0} does not start with BORROWER\ OR PROSPECT\, FileId={1}", temp1, fileId);
                    return path;
                }
                path = temp + temp1;
                FolderStatus = dr["LoanStatus"] == DBNull.Value ? string.Empty : dr["LoanStatus"].ToString();
                if (string.IsNullOrEmpty(FolderStatus))
                {
                    err = string.Format(@"Point Filename {0} with FileId={1} is in a folder with empty LoanStatus.", temp1, fileId);
                    FolderStatus = LoanStatusEnum.Prospect.ToString();
                    return path;
                }

                return path;
            }
            catch (Exception ex)
            {
                err = "GetPointFilePaths, Exception: " + ex.Message;
                return path;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public string GetPointFileNameByFileId(int fileId, ref string err)
        {
            err = "";
            string filename = "";
            if (fileId <= 0)
            {
                err = "GetPointFileNameByFileId, invalid FileId=" + fileId;
                return filename;
            }
            string sqlCmd = "Select TOP 1 [Name] from PointFiles WHERE FileId =" + fileId;
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "No Point File Name found in the database for file Id: " + fileId;
                    return filename;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                string temp = dr[0] == DBNull.Value ? string.Empty : dr[0].ToString().Trim();
                if (string.IsNullOrEmpty(temp))
                {
                    err = "No Point File Name found in the database for file id: " + fileId;
                    return filename;
                }
                if (!temp.ToUpper().StartsWith(@"\BORROWER\") && !temp.ToUpper().StartsWith(@"\PROSPECT\"))
                {
                    err = string.Format(@"Point Filename {0} does not start with BORROWER\ OR PROSPECT\, FileId={1}", temp, fileId);
                    return filename;
                }
                filename = temp;
                return filename;
            }
            catch (Exception ex)
            {
                err = "GetPointFileNameByFileId, Exception: " + ex.Message;
                return filename;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public string GetPointFilePath(int fileId, ref string err)
        {
            err = "";
            string path = "";
            bool updateFileName = false;
            if (fileId <= 0)
            {
                err = "GetPointFilePath, invalid FileId=" + fileId;
                return path;
            }
            string sqlCmd = "Select TOP 1 pf.Path, f.[Name] as FileName from PointFiles f inner join PointFolders pf on f.FolderId=pf.FolderId WHERE FileId =" + fileId;
            DataSet ds = null;
            string fileName = string.Empty;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "No Point File Path found in the database for file Id: " + fileId;
                    return path;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                string temp = dr[0] == DBNull.Value ? string.Empty : dr[0].ToString().Trim();
                if (string.IsNullOrEmpty(temp))
                {
                    err = "No Point File Path found in the database for file id: " + fileId;
                    return path;
                }
                path = temp.Trim();
                fileName = dr[1] == DBNull.Value ? string.Empty : dr[1].ToString().Trim();
                if (string.IsNullOrEmpty(fileName))
                {
                    err = "No Point Filename found in the database for fileId: " + fileId;
                    return path;
                }
                if (!fileName.ToUpper().StartsWith(@"\"))
                {
                    fileName = @"\" + fileName;
                    updateFileName = true;
                }
                if (!fileName.ToUpper().StartsWith(@"\BORROWER\") && !fileName.ToUpper().StartsWith(@"\PROSPECT\"))
                {
                    err = string.Format(@"Point File {0} must reside in BORROWER\ OR PROSPECT\ folder, FileId={1}", fileName, fileId);
                    return path;
                }
                path = temp.Trim() + fileName;
                return path;
            }
            catch (Exception ex)
            {
                err = "GetPointFilePaths, Exception: " + ex.Message;
                return path;
            }
            finally
            {
                if (updateFileName && !string.IsNullOrEmpty(fileName))
                {
                    sqlCmd = string.Format("Update PointFiles set [Name]='{0}' where FileId={1}", fileName, fileId);
                    DbHelperSQL.ExecuteSql(sqlCmd);
                }
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public List<string> GetPointFilePaths(int[] fileIds, ref string err)
        {
            err = "";
            List<string> PointFilePaths = null;
            if (fileIds.Length <= 0)
            {
                err = "GetPointFilePaths, no file Ids specified.";
                return PointFilePaths;
            }
            string sqlCmd = "Select Path from lpvw_GetPointFileInfo WHERE FileId in (";
            int i, j = 0;
            string fileList = "";
            for (i = 0; i < fileIds.Length; i++)
            {
                j = fileIds[i];
                fileList += j;
                if (i < fileIds.Length - 1)
                    fileList += ",";
            }
            sqlCmd += fileList + ")";
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "No Point File Paths found in the database for file Ids: " + fileList;
                    return PointFilePaths;
                }
                PointFilePaths = new List<string>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string path = "";
                    if (dr["Path"] != DBNull.Value)
                        path = dr["Path"].ToString().Trim();
                    else
                        continue;
                    if (path.ToUpper().Contains(".BRW"))
                    {
                        PointFilePaths.Add(path);
                    }
                }
                return PointFilePaths;
            }
            catch (Exception ex)
            {
                err = "GetPointFilePaths, Exception: " + ex.Message;
                Trace.TraceError(err);
                return PointFilePaths;
            }

        }

        public bool GetPointFolderId(string folder, ref int folderId, ref int branchId, ref short loanStatus, ref DateTime lastImport, ref int PDSFolderId, ref string err)
        {
            err = "";
            loanStatus = 0;
            folderId = branchId = 0;
            if ((folder == null) || (folder.Trim() == string.Empty))
            {
                err = "File path for the Point folder is empty.";
                return false;
            }

            string sqlCmd = "Select top 1 FolderId, BranchId, LastImport, LoanStatus, PDSFolderId from PointFolders where path='" + folder + "'";
            try
            {
                DataSet ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null)
                {
                    err = "No Point Folder record matching folder=" + folder;
                    return false;
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    folderId = dr["FolderId"] == DBNull.Value ? 0 : (int)dr["FolderId"];
                    branchId = dr["BranchId"] == DBNull.Value ? 0 : (int)dr["BranchId"];
                    PDSFolderId = dr["PDSFolderId"] == DBNull.Value ? 0 : (int)dr["PDSFolderId"];

                    if ((folderId <= 0) || (branchId <= 0))
                        return false;
                    lastImport = dr["LastImport"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["LastImport"];

                    loanStatus = dr["LoanStatus"] == DBNull.Value ? (short)0 : (short)dr["LoanStatus"];
                    if (loanStatus <= 0)
                    {
                        err = string.Format("The specified Point Folder, {0} is not configured as an 'Active' loan folder. Pulse will handle only folders configured to be 'Prospect' or 'Processing' folders.", folder);
                        return false;
                    }
                    return true; ;
                }
                return true;
            }
            catch (Exception e)
            {
                err = e.Message;
                Trace.TraceError(err);
                return false;
            }
        }

        public bool GetBranchIdByFolderName(string filepath, ref int branchId, ref string err)
        {
            err = "";
            branchId = 0;
            if (filepath == null)
            {
                err = "File path for the Point folder is empty.";
                return false;
            }

            string path = Path.GetDirectoryName(filepath);
            if (path == String.Empty)
            {
                err = "File path for the Point folder is empty.";
                return false;
            }

            string sqlCmd = "Select top 1 BranchId from PointFolders where path='" + path + "'";
            try
            {
                DataSet ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "No Point Folder record matching folder=" + path;
                    return false;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr[0] != DBNull.Value)
                    branchId = (int)dr[0];
                return true;
            }
            catch (Exception e)
            {
                err = e.Message;
                Trace.TraceError(err);
                return false;
            }
        }

        public int Save_PointFiles(string filename, string folder, ref int folderId, ref int branchId, byte[] currentImage, bool changed, ref string err)
        {
            int fileId = 0;
            string name = "";

            int PDSFileId = 0;
            err = "";
            name = filename;
            DateTime dt = DateTime.Now;
            string folder1 = Path.GetDirectoryName(filename);
            if (folder1 == string.Empty)
            {
                err = "Point Filename has invalid path";
                return -1;
            }

            if (filename.Length <= folder.Length + 5)
            {
                err = string.Format("Invalid Filename specified {0}.", filename);
                return -1;
            }

            name = filename.Substring(folder.Length, filename.Length - folder.Length);
            string sqlCmd = string.Format("Select PDSFileId from PointFiles where [Name]=@Name AND FolderId={0}", folderId);
            SqlCommand cmd = new SqlCommand(sqlCmd);
            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar));
            cmd.Parameters[0].Value = name.Trim();
            object obj = DbHelperSQL.ExecuteScalar(cmd);
            //object obj = DbHelperSQL.GetSingle(sqlCmd);
            PDSFileId = (obj == DBNull.Value || obj == null) ? 0 : (int)obj;
            try
            {
                if (PDSFileId <= 0)
                {
                    sqlCmd = string.Format("Select PDSFolderId from PointFolders where FolderId={0}", folderId);
                    obj = DbHelperSQL.GetSingle(sqlCmd);
                    int PDSFolderId = (obj == DBNull.Value || obj == null) ? 0 : (int)obj;
                    if (PDSFolderId > 0)
                        PDSFileId = GetPDSPointFileId(Path.GetFileName(name), PDSFolderId, ref err);
                }
            }
            catch (Exception ee)
            {
                err = string.Format("Unable to get PDS FileID for Filename {0}, Exception: {1}", filename, ee.ToString());
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, 9103);
            }
            byte[] previousImage = null;
            try
            {
                SqlParameter[] parameters = {                    
                    new SqlParameter("@FileId", SqlDbType.Int, 4 ),                               //0
                    new SqlParameter("@FolderId", SqlDbType.Int, 4),                           //1
                    new SqlParameter("@Name", SqlDbType.NVarChar, 255),               //2
                    new SqlParameter("@FirstImported", SqlDbType.DateTime),           //3
                    new SqlParameter("@LastImported", SqlDbType.DateTime ),          //4
                    new SqlParameter("@success", SqlDbType.Bit, 1 ),                           //5
                    new SqlParameter("@CurrentImage", SqlDbType.VarBinary, -1 ),     //6
                    new SqlParameter("@PreviousImage", SqlDbType.VarBinary, -1 ),    //7
                    new SqlParameter("@PDSFileId", SqlDbType.Int, 4)    //8
                   };

                parameters[0].Value = DBNull.Value;
                parameters[1].Value = folderId;
                parameters[2].Value = name;
                parameters[3].Value = DBNull.Value;

                DateTime date = DateTime.Now;
                TimeSpan time = new TimeSpan(0, 0, 5, 0);
                DateTime combined = date.Add(time);

                parameters[4].Value = combined;
                parameters[5].Value = true;
                if (currentImage == null)
                    parameters[6].Value = DBNull.Value;
                else
                    parameters[6].Value = (object)currentImage;
                if ((previousImage == null) || (previousImage.Length <= 0))
                    parameters[7].Value = DBNull.Value;
                else
                    parameters[7].Value = (object)previousImage;

                if (PDSFileId == 0) parameters[8].Value = DBNull.Value;
                else parameters[8].Value = PDSFileId;

                int rows = 0;
                fileId =
                    DbHelperSQL.RunProcedure("PointFiles_Save", parameters, out rows);
                if (fileId > 0)
                {
                    string outcontactId = fileId.ToString();
                    string SqlCmd = "Update PointImportHistory Set ImportTime ='" + dt.ToString() + "' Where FileId=" + fileId;
                    DbHelperSQL.ExecuteSql(SqlCmd);
                    return fileId;
                }

                return -1;
            }
            catch (Exception e)
            {
                err = "Failed to save PointFiles due to database error, Exception:" + e.Message;
                Trace.TraceError(err);
                return -1;
            }
        }

        private PointFolderInfo GetPointFolderInfo(DataRow dr)
        {
            PointFolderInfo pf = new PointFolderInfo()
            {
                FolderId = dr["FolderId"] == DBNull.Value ? 0 : (int)dr["FolderId"],
                Name = dr["Name"] == DBNull.Value ? string.Empty : dr["Name"].ToString(),
                Path = dr["Path"] == DBNull.Value ? string.Empty : dr["Path"].ToString(),
                BranchId = dr["BranchId"] == DBNull.Value ? 0 : (int)dr["BranchId"],
                LoanStatus = (short)(dr["LoanStatus"] == DBNull.Value ? 0 : dr["LoanStatus"]),
                Enabled = dr["Enabled"] == DBNull.Value ? false : (bool)dr["Enabled"],
                AutoNaming = dr["AutoNaming"] == DBNull.Value ? false : (bool)dr["AutoNaming"],
                FilenamePrefix = dr["FilenamePrefix"] == DBNull.Value ? string.Empty : (string)dr["FilenamePrefix"],
                RandomNumbering = dr["RandomFileNaming"] == DBNull.Value ? false : (bool)dr["RandomFileNaming"],
                FilenameLength = (short)(dr["FilenameLength"] == DBNull.Value ? 0 : (short)dr["FilenameLength"])
            };
            return pf;
        }

        public PointFolderInfo GetPointFolderInfo(int PointFolderId, ref string err)
        {
            err = string.Empty;
            PointFolderInfo pf = null;
            try
            {
                string sqlcmd = string.Format("Select * from PointFolders where FolderId={0} ", PointFolderId);
                DataSet ds = DbHelperSQL.Query(sqlcmd);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                {
                    err = string.Format("Failed to obtain the PointFolder information for FolderId={0}.", PointFolderId);
                    return pf;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr == null)
                {
                    err = string.Format("Failed to obtain the PointFolder information for FolderId={0}.", PointFolderId);
                    return pf;
                }
                if (dr["Name"] == DBNull.Value || dr["BranchId"] == DBNull.Value || dr["Path"] == DBNull.Value || dr["LoanStatus"] == DBNull.Value)
                {
                    err = string.Format("Missign [Name], [BranchId], [Path], or [LoanStatus] in the PointFolders record for FolderId={0}.", PointFolderId);
                    return pf;
                }
                pf = GetPointFolderInfo(dr);
                return pf;
            }
            catch (Exception ex)
            {
                err = string.Format("GetPointFolderInfo FolderId{0} Exception: {1}", PointFolderId, ex.Message);
                return pf;
            }
        }

        public List<PointFolderInfo> GetPointFolders(int loanStatus, bool enabled, ref string err)
        {
            err = "";
            List<PointFolderInfo> FolderList = null;
            int active = enabled ? 1 : 0;
            string sqlCmd = "Select * from PointFolders where Enabled=" + active + " and LoanStatus=" + loanStatus;
            try
            {
                DataSet ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "No record found in PointFolders for LoanStatus=" + loanStatus.ToString();
                    return FolderList;
                }
                FolderList = new List<PointFolderInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["FolderId"] == DBNull.Value || dr["Name"] == DBNull.Value || dr["Path"] == DBNull.Value)
                        continue;
                    PointFolderInfo pi = GetPointFolderInfo(dr);
                    FolderList.Add(pi);
                }
            }
            catch (Exception e)
            {
                err = "Failed to get Point Folders, Err:" + e.Message;
                Trace.TraceError(err);
            }
            return FolderList;
        }

        public DataTable GetDefaultProcessingFolderInfo(int iBranchID)
        {
            string sSql = "Select top 1 FolderId from PointFolders where [Default]=1 AND Enabled=1 AND BranchId=" + iBranchID + " AND LoanStatus=1";
            DataTable FolderInfo = DbHelperSQL.ExecuteDataTable(sSql);
            if (FolderInfo.Rows.Count == 0)
            {
                string sSql1 = "Select top 1 FolderId from PointFolders where Enabled=1 AND BranchId=" + iBranchID + " AND LoanStatus=1";
                FolderInfo = DbHelperSQL.ExecuteDataTable(sSql1);
            }
            return FolderInfo;
        }

        public int Add_PointFolders(string name, string folderpath, int PDSFolderId, ref string err)
        {
            int FolderId = 0;
            DateTime dt = DateTime.Now;

            try
            {
                SqlParameter[] parameters = {                    
                    new SqlParameter("@FolderId", SqlDbType.Int, 4 ),                               //0                   
                    new SqlParameter("@Name", SqlDbType.NVarChar, 255),                   //1
                    new SqlParameter("@BranchId", SqlDbType.Int, 4),                              //2
                    new SqlParameter("@Path", SqlDbType.NVarChar, 255 ),                    //3
                    new SqlParameter("@Enabled", SqlDbType.Bit, 1 ),                              //4
                      new SqlParameter("@LoanStatus", SqlDbType.SmallInt ),                //5
                      new SqlParameter("@PDSFolderId", SqlDbType.Int,4 )                    //6
                   };

                parameters[0].Value = DBNull.Value;
                parameters[0].Direction = ParameterDirection.Output;
                parameters[1].Value = name;
                parameters[2].Value = DBNull.Value;
                parameters[3].Value = folderpath;
                parameters[4].Value = false;
                parameters[5].Value = 0;

                if (PDSFolderId == 0) parameters[6].Value = DBNull.Value;
                else parameters[6].Value = PDSFolderId;

                int rows = 0;
                FolderId =
                    DbHelperSQL.RunProcedure("Add_PointFolder", parameters, out rows);
                if (FolderId > 0)
                {
                    string outcontactId = FolderId.ToString();
                    return FolderId;
                }

                return -1;
            }
            catch (Exception e)
            {
                err = "Failed to save PointFolders due to database error." + e.Message;
                Trace.TraceError(err);
                return -1;
            }

        }

        public int Save_Roles(string name, ref string err)
        {
            int RoleId = 0;
            DateTime dt = DateTime.Now;

            try
            {
                SqlParameter[] parameters = {                    
                    new SqlParameter("@RoleId", SqlDbType.Int, 4 ),                               //0                   
                    new SqlParameter("@Name", SqlDbType.NVarChar, 255),                 //1
                    new SqlParameter("@CompanySetup", SqlDbType.Bit, 1 ),                //2
                    new SqlParameter("@LoanSetup", SqlDbType.Bit, 1 ),                        //3
                    new SqlParameter("@OtherLoanAccess", SqlDbType.Bit, 1 ),            //4
                    new SqlParameter("@CustomUserHome", SqlDbType.Bit, 1 ),           //5
                    new SqlParameter("@WorkflowTempl", SqlDbType.SmallInt, 1 ),       //6
                    new SqlParameter("@CustomTask", SqlDbType.SmallInt, 1 ),            //7
                    new SqlParameter("@AlertRules", SqlDbType.SmallInt, 1 ),                //8
                    new SqlParameter("@AlertRuleTempl", SqlDbType.SmallInt, 1 ),     //9
                    new SqlParameter("@MarkOtherTaskCompl", SqlDbType.Bit, 1 ),      //10
                    new SqlParameter("@AssignTask", SqlDbType.Bit, 1 ),                       //11
                    new SqlParameter("@ImportLoan", SqlDbType.Bit, 1 ),                       //12
                    new SqlParameter("@RemoveLoan", SqlDbType.Bit, 1 ),                   //13
                    new SqlParameter("@AssignLoan", SqlDbType.Bit, 1 ),                      //14
                    new SqlParameter("@ApplyWorkflow", SqlDbType.Bit, 1 ),                 //15
                    new SqlParameter("@ApplyAlertRule", SqlDbType.Bit, 1 ),                 //16
                    new SqlParameter("@SendEmail", SqlDbType.Bit, 1 ),                        //17
                    new SqlParameter("@CreateNotes", SqlDbType.Bit, 1 ),                     //18
                    new SqlParameter("@CompanyCalendar", SqlDbType.Bit, 1 ),           //19  
                    new SqlParameter("@PipelineChart", SqlDbType.Bit, 1 ),                    //20
                    new SqlParameter("@SalesBreakdownChart", SqlDbType.Bit, 1 ),     //21
                    new SqlParameter("@OrgProductionChart", SqlDbType.Bit, 1 ),         //22
                    new SqlParameter("@Org_N_Sales_Charts", SqlDbType.Bit, 1 ),       //23
                    new SqlParameter("@RateSummary", SqlDbType.Bit, 1 ),                   //24
                    new SqlParameter("@GoalsChart", SqlDbType.Bit, 1 ),                        //25
                    new SqlParameter("@OverdueTaskAlerts", SqlDbType.Bit, 1 ),           //26
                     new SqlParameter("@Announcements", SqlDbType.Bit, 1 ),               //27
                    new SqlParameter("@ExchangeInbox", SqlDbType.Bit, 1 ),                 //28
                    new SqlParameter("@ExchangeCalendar", SqlDbType.Bit, 1 )            //29     
             
                   };

                parameters[0].Value = DBNull.Value;
                parameters[0].Direction = ParameterDirection.Output;
                parameters[1].Value = name;
                parameters[2].Value = true;
                parameters[3].Value = true;
                parameters[4].Value = true;
                parameters[5].Value = true;
                parameters[6].Value = 1;
                parameters[7].Value = 1;
                parameters[8].Value = 1;
                parameters[9].Value = 1;
                parameters[10].Value = true;
                parameters[11].Value = true;
                parameters[12].Value = true;
                parameters[13].Value = true;
                parameters[14].Value = true;
                parameters[15].Value = true;
                parameters[16].Value = true;
                parameters[17].Value = true;
                parameters[18].Value = true;
                parameters[19].Value = true;
                parameters[20].Value = true;
                parameters[21].Value = true;
                parameters[22].Value = true;
                parameters[23].Value = true;
                parameters[24].Value = true;
                parameters[25].Value = true;
                parameters[26].Value = true;
                parameters[27].Value = true;
                parameters[28].Value = true;
                parameters[29].Value = true;

                int rows = 0;
                RoleId =
                    DbHelperSQL.RunProcedure("Roles_Save", parameters, out rows);

                if (RoleId > 0)
                {
                    string outcontactId = RoleId.ToString();
                    return RoleId;
                }

                return -1;
            }
            catch (Exception e)
            {
                err = "Failed to save Roles due to database error, Exception:" + e.Message;
                Trace.TraceError(err);
                return -1;
            }

        }

        public bool Save_ContactUsers(int fileId, int contactId, ref string err)
        {
            if (fileId <= 0 || contactId <= 0)
            {
                err = string.Format("Save_ContactUsers, invalid FileId={0} or Contactid={1}.", fileId, contactId);
                return false;
            }

            try
            {
                SqlParameter[] parameters = {                    
                    new SqlParameter("@FileId", SqlDbType.Int, 4 ),                               //0                   
                    new SqlParameter("@ContactId", SqlDbType.Int, 4)                               //1                   
                   };

                parameters[0].Value = fileId;
                parameters[1].Value = contactId;

                int rows = 0;
                DbHelperSQL.RunProcedure("[dbo].[ContactUsers_Save]", parameters, out rows);

                return true;
            }
            catch (Exception e)
            {
                err = "Failed to save ContactUsers due to database error:" + e.Message;
                Trace.TraceError(err);
                return false;
            }
        }

        public int Save_ContactRoles(string name, ref string err)
        {
            int ContactRoleId = 0;
            DateTime dt = DateTime.Now;

            try
            {
                SqlParameter[] parameters = {                    
                    new SqlParameter("@ContactRoleId", SqlDbType.Int, 4 ),                               //0                   
                    new SqlParameter("@Name", SqlDbType.NVarChar, 255)                   //1                   
                   };

                parameters[0].Value = DBNull.Value;
                parameters[0].Direction = ParameterDirection.Output;

                parameters[1].Value = name;

                int rows = 0;
                ContactRoleId =
                    DbHelperSQL.RunProcedure("ContactRoles_Save", parameters, out rows);
                if (ContactRoleId > 0)
                {
                    string outcontactId = ContactRoleId.ToString();
                    return ContactRoleId;
                }

                return -1;
            }
            catch (Exception e)
            {
                err = "Failed to save ContactRoles due to database error:" + e.Message;
                Trace.TraceError(err);
                return -1;
            }

        }

        public int Save_Template_Workflow(string name, ref string err)
        {
            int WflTemplId = 0;
            DateTime dt = DateTime.Now;

            try
            {
                SqlParameter[] parameters = {                    
                    new SqlParameter("@WflTemplId", SqlDbType.Int, 4 ),                         //0                   
                    new SqlParameter("@Name", SqlDbType.NVarChar, 255),                   //1                   
                    new SqlParameter("@Enabled", SqlDbType.Bit, 1 )                               //1                   
                   };

                parameters[0].Value = DBNull.Value;
                parameters[0].Direction = ParameterDirection.Output;

                parameters[1].Value = name;

                parameters[2].Value = true;

                int rows = 0;
                WflTemplId =
                    DbHelperSQL.RunProcedure("Template_Workflow_Save", parameters, out rows);
                if (WflTemplId > 0)
                {
                    string outcontactId = WflTemplId.ToString();
                    return WflTemplId;
                }

                return -1;
            }
            catch (Exception e)
            {
                err = "Failed to save Template_Workflow due to database error:" + e.Message;
                Trace.TraceError(err);
                return -1;
            }

        }

        public int Save_Template_Stages(string name, short sequence, ref string err)
        {
            int TemplStageId = 0;
            DateTime dt = DateTime.Now;

            try
            {
                SqlParameter[] parameters = {                    
                    new SqlParameter("@TemplStageId", SqlDbType.Int, 4 ),                               //0                   
                    new SqlParameter("@Name", SqlDbType.NVarChar, 255),                              //1                   
                    new SqlParameter("@Enabled", SqlDbType.Bit, 1 ),                                        //2 
                    new SqlParameter("@SequenceNumber", SqlDbType.SmallInt)
                   };

                parameters[0].Value = DBNull.Value;
                parameters[0].Direction = ParameterDirection.Output;
                parameters[1].Value = name.Trim();
                parameters[2].Value = true;
                parameters[3].Value = sequence;

                int rows = 0;
                TemplStageId =
                    DbHelperSQL.RunProcedure("Template_Stages_Save", parameters, out rows);
                if (TemplStageId > 0)
                {
                    string outcontactId = TemplStageId.ToString();
                    return TemplStageId;
                }

                return -1;
            }
            catch (Exception e)
            {
                err = "Failed to save Template_Stages due to database error:" + e.Message;
                Trace.TraceError(err);
                return -1;
            }

        }

        public int Save_Template_Wfl_Stages(string name, int SequenceNumber, ref string err)
        {
            int WflStageId = 0;
            DateTime dt = DateTime.Now;

            try
            {
                SqlParameter[] parameters = {                    
                    new SqlParameter("@WflStageId", SqlDbType.Int, 4 ),                               //0                   
                    new SqlParameter("@WflTemplId", SqlDbType.Int, 4 ),                               //1                   
                    new SqlParameter("@SequenceNumber", SqlDbType.SmallInt, 1 ),                   //1 
                    new SqlParameter("@Enabled", SqlDbType.Bit, 1 ),                                             //1        
                    new SqlParameter("@DaysFromEstClose", SqlDbType.SmallInt, 1),                   //1        
                    new SqlParameter("@Name", SqlDbType.NVarChar, 50),                                //1                   
                   };

                parameters[0].Value = DBNull.Value;
                parameters[0].Direction = ParameterDirection.Output;
                parameters[1].Value = 1;
                parameters[2].Value = SequenceNumber;
                parameters[3].Value = true;
                parameters[4].Value = DBNull.Value;
                parameters[5].Value = name;

                int rows = 0;
                WflStageId =
                    DbHelperSQL.RunProcedure("Template_Wfl_Stages_Save", parameters, out rows);

                if (WflStageId > 0)
                {
                    string outcontactId = WflStageId.ToString();
                    return WflStageId;
                }

                return -1;
            }
            catch (Exception e)
            {
                err = "Failed to save Template_Wfl_Stages due to database error:" + e.Message;
                Trace.TraceError(err);
                return -1;
            }

        }

        public void Save_ApplyWorkflowTempl(int FileId, int WflTemplId, int UserId, ref string err)
        {
            err = string.Empty;
            string SqlCmd = string.Format("select WflTemplId from LoanWflTempl where FileId={0}", FileId);
            try
            {
                object obj = DbHelperSQL.GetSingle(SqlCmd);
                if (obj == null || obj == DBNull.Value)
                {
                    SqlCmd = string.Format("Insert into LoanWflTempl (FileId, WflTemplId, ApplyDate, ApplyBy) Values ('{0}', '{1}', '{2}', '{3}')",
                                   FileId, WflTemplId, DateTime.Now.ToString(), UserId);
                }
                else
                {
                    SqlCmd = string.Format("Update LoanWflTempl set FileId={0}, WflTemplId={1}, ApplyDate='{2}', ApplyBy='{3}'",
                             FileId, WflTemplId, DateTime.Now.ToString(), UserId);
                }
                DbHelperSQL.ExecuteSql(SqlCmd);

            }
            catch (Exception ex)
            {
                err = ex.Message + "\n" + ex.StackTrace;
            }
            try
            {
                SqlCmd = string.Format("Select top 1 [Name] from Template_Workflow where WflTemplId={0}", WflTemplId);
                object obj1 = DbHelperSQL.GetSingle(SqlCmd);
                string WflTemplName = (obj1 == null || obj1 == DBNull.Value) ? string.Empty : (string)obj1;
                SqlCmd = string.Format("Insert into LoanActivities (FileId, UserId, ActivityName, ActivityTime) Values ('{0}', '{1}', '{2}', '{3}')",
                        FileId, UserId, "Workflow Template " + WflTemplName + " has been applied.", DateTime.Now.ToString());
                DbHelperSQL.ExecuteSql(SqlCmd);
            }
            catch (Exception ex)
            {
                err = ex.Message + "\n" + ex.StackTrace;
            }
        }
        #endregion
        #region Loan Data Access
        public bool ExtendRateLock(int FileId, short NumDaysExtended, int UserId, ref string err)
        {
            err = "";

            if (FileId <= 0)
            {
                err = "ExtendRateLock:: Invalid FileId=" + FileId;
                return false;
            }
            if (NumDaysExtended <= 0)
            {
                err = "ExtendRateLock:: Invalid NumberDaysExtended=" + NumDaysExtended;
                return false;
            }
            if (UserId <= 0)
            {
                err = "ExtendRateLock:: Invalid UserId=" + UserId;
                return false;
            }
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@FileId", SqlDbType.Int, 4),                 //0 
                    new SqlParameter("@NumberDaysExtended", SqlDbType.SmallInt),   //1
                    new SqlParameter("@UserId", SqlDbType.Int, 4)
                };

                parameters[0].Value = FileId;
                parameters[1].Value = NumDaysExtended;
                parameters[2].Value = UserId;
                // this stored procedure calculate new due dates for tasks
                DbHelperSQL.RunProcedure("[dbo].[ExtendRateLock]", parameters);
                return true;
            }
            catch (Exception ex)
            {
                err = "ExtendRateLock, Exception:" + ex.Message;
                return false;
            }
        }

        public bool CheckLoanStages(int FileId, ref string err)
        {
            err = "";
            bool logErr = false;
            if (FileId <= 0)
            {
                err = "CheckLoanStages:: Invalid FileId=" + FileId + " specified.";
                int Event_id = 3097;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@FileId", SqlDbType.Int, 4)                 //0 
                  };

                parameters[0].Value = FileId;
                // this stored procedure checks and save the rate lock alert
                DbHelperSQL.RunProcedure("CheckLoanStages", parameters);
                return true;
            }
            catch (Exception ex)
            {
                err = "CheckLoanStages, Exception:" + ex.Message;
                //int Event_id = 3098;
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                }
            }

        }

        public bool Check_SaveRateLockAlert(int FileId, ref string err)
        {
            err = "";
            if (FileId <= 0)
            {
                err = "Check_SaveRateLockAlert:: Invalid FileId=" + FileId + " specified.";
                return false;
            }
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@FileId", SqlDbType.Int, 4)                 //0 
                  };

                parameters[0].Value = FileId;
                // this stored procedure checks and save the rate lock alert
                DbHelperSQL.RunProcedure("CheckRateLockAlert", parameters);
                return true;
            }
            catch (Exception ex)
            {
                err = "Check_SaveRateLockAlert, Exception:" + ex.Message;
                return false;
            }

        }
        public bool UpdateEstCloseDate(int fileId, int userId, DateTime newDate, ref string err)
        {
            err = "";
            if (fileId <= 0)
            {
                err = "UpdateEstCloseDate: invalid FileId specified, " + fileId;
                return false;
            }
            if (userId == null)
            {
                err = "UpdateEstCloseDate: invalid userId " + userId;
                return false;
            }
            if ((newDate == null) || (newDate == DateTime.MinValue))
            {
                err = "UpdateEstCloseDate: invalid Est Close Date.";
                return false;
            }

            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@FileId", SqlDbType.Int, 4),              //0 
                    new SqlParameter("@UserId", SqlDbType.Int, 4),
                    new SqlParameter("@NewDate", SqlDbType.DateTime)
                  };

                parameters[0].Value = fileId;
                parameters[1].Value = userId;
                parameters[2].Value = newDate;
                DbHelperSQL.RunProcedure("lpsp_UpdateEstCloseDate", parameters);
                return true;
            }
            catch (Exception ex)
            {
                err = "UpdateEstCloseDate, Exception:" + ex.Message;
                return false;
            }
            finally
            {
                if (err != String.Empty)
                    Trace.TraceError(err);
            }

        }

        public bool UpdateTaskDueDate(int fileId, Table.LoanTasks task, ref string err)
        {
            err = "";
            if (fileId <= 0)
            {
                err = "UpdateTaskDueDate: invalid FileId specified, " + fileId;
                return false;
            }
            if (task == null)
            {
                err = "UpdateTaskDueDate: LoanTask is null.";
                return false;
            }
            string sqlCmd = "";
            DataSet ds = null;
            DataRow dr = null;
            DateTime dt = DateTime.MinValue;
            try
            {
                if (task.PrerequisiteTaskId > 0)
                {
                    sqlCmd = "Select TOP 1 Completed from LoanTasks WHERE LoanTaskId=" + task.PrerequisiteTaskId + " AND FileId=" + fileId;
                    ds = DbHelperSQL.Query(sqlCmd);
                }
                else
                {
                    sqlCmd = "Select TOP 1 EstCloseDate from Loans WHERE FileId=" + fileId;
                    ds = DbHelperSQL.Query(sqlCmd);
                }

                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    return true;
                dr = ds.Tables[0].Rows[0];
                if (dr[0] != DBNull.Value)
                    dt = (DateTime)dr[0];

                DateTime tempDue = DateTime.MinValue;
                if ((task.PrerequisiteTaskId > 0) && (dt != DateTime.MinValue))
                {
                    tempDue = dt.AddDays((double)task.DaysAfterPrerequisiteTask);
                }
                else if ((task.DaysFromEstClose != 0) && (dt != DateTime.MinValue))
                {
                    tempDue = dt.AddDays((double)task.DaysFromEstClose);
                }
                if (tempDue != DateTime.MinValue)
                {
                    task.Due = tempDue;
                    sqlCmd = "Update LoanTasks SET Due='" + task.Due.Date.ToString() + "' WHERE LoanTaskId=" + task.LoanTaskId + " AND FileId=" + fileId + " AND LoanStageId=" + task.LoanStageId;
                    DbHelperSQL.ExecuteSql(sqlCmd);
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "UpdateTaskDueDate, Exception:" + ex.Message;
                return false;
            }
            finally
            {
                if (dr != null)
                    dr = null;
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
            }
        }

        public bool Check_SaveTaskAlert(int FileId, int LoanTaskId, ref string err)
        {
            err = "";
            if (FileId <= 0)
                err = "Check_SaveTaskAlert:: Invalid FileId=" + FileId + " specified.";
            if (LoanTaskId <= 0)
                err += " Invalid LoanTaskId=" + LoanTaskId + " specified.";
            if ((FileId <= 0) || (LoanTaskId <= 0))
                return false;
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@FileId", SqlDbType.Int, 4),                 //0 
                    new SqlParameter("@TaskId", SqlDbType.Int, 4)
                };

                parameters[0].Value = FileId;
                parameters[1].Value = LoanTaskId;
                // this stored procedure checks and save the task alert
                DbHelperSQL.RunProcedure("CheckTaskAlert", parameters);
                return true;
            }
            catch (Exception ex)
            {
                err = "Check_SaveTaskAlert, Exception:" + ex.Message;
                return false;
            }
        }

        public bool CalculateDueDates(int FileId, DateTime EstCloseDate, ref string err)
        {
            err = "";
            if (FileId <= 0)
            {
                err = "CalculateDueDates:: Invalid FileId=" + FileId + " specified.";
                return false;
            }
            if (EstCloseDate == DateTime.MinValue)
            {
                err = "CalculateDueDates:: Invalid Est Close Date specified for FileId=" + FileId;
                return false;
            }

            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@FileId", SqlDbType.Int, 4),                 //0 
                    new SqlParameter("@EstCloseDate", SqlDbType.DateTime)
                };

                parameters[0].Value = FileId;
                parameters[1].Value = EstCloseDate;
                // this stored procedure calculate new due dates for tasks
                DbHelperSQL.RunProcedure("[dbo].[CalculateLoanTaskDueDates]", parameters);
                return true;
            }
            catch (Exception ex)
            {
                err = "CalculateDueDates, Exception:" + ex.Message;
                return false;
            }
        }

        public bool Save_EstCloseDate(int FileId, DateTime newDate, int UserId, ref string err)
        {
            err = "";
            if (FileId <= 0)
            {
                err = "Save_EstCloseDate:: Invalid FileId=" + FileId + " specified.";
                return false;
            }
            if (newDate == DateTime.MinValue)
            {
                err = "Save_EstCloseDate:: Invalid Est Close Date specified for FileId=" + FileId;
                return false;
            }

            try
            {
                string sqlCmd = "Update Loans SET EstCloseDate='" + newDate.ToString("d") + "' WHERE FileId=" + FileId;
                DbHelperSQL.ExecuteSql(sqlCmd);

                sqlCmd = "Insert Into LoanActivities (FileId, ActivityTime, UserId, ActivityName) VALUES (" +
                    FileId + ", '" + DateTime.Now.ToString() + "', " + UserId + ", 'Estimated Close Date has been changed to " + newDate.ToString("d") + ".')";
                DbHelperSQL.ExecuteSql(sqlCmd);
                return true;
            }
            catch (Exception ex)
            {
                err = "SaveEstCloseDate, Exception:" + ex.Message;
                return false;
            }
        }

        public List<Table.LoanTasks> GetLoanTasks(int FileId, bool All, bool Completed, ref string err)
        {
            err = "";
            string sqlCmd = "Select * from LoanTasks Where FileId=" + FileId;
            if (!All)
            {
                sqlCmd += " AND Completed ";
                if (Completed)
                    sqlCmd += " IS NOT NULL";
                else
                    sqlCmd += " IS NULL";
            }
            List<Table.LoanTasks> taskList = null;
            try
            {
                DataSet ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                    return taskList;
                taskList = new List<Table.LoanTasks>();
                taskList.Clear();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["LoanTaskId"] == DBNull.Value)
                        continue;
                    if (dr["FileId"] == DBNull.Value)
                        continue;
                    if (dr["Name"] == DBNull.Value)
                        continue;
                    Table.LoanTasks task = new Table.LoanTasks();
                    task.LoanTaskId = (int)dr["LoanTaskId"];
                    task.FileId = (int)dr["FileId"];
                    task.Name = dr["Name"].ToString().Trim();

                    if (dr["Due"] != DBNull.Value)
                        task.Due = (DateTime)dr["Due"];
                    else
                        task.Due = DateTime.MinValue;
                    if (dr["Completed"] != DBNull.Value)
                        task.Completed = (DateTime)dr["Completed"];
                    else
                        task.Completed = DateTime.MinValue;
                    if (dr["CompletedBy"] != DBNull.Value)
                        task.CompletedBy = (int)dr["CompletedBy"];
                    else
                        task.CompletedBy = 0;
                    if (dr["LastModified"] != DBNull.Value)
                        task.LastModified = (DateTime)dr["LastModified"];
                    else
                        task.LastModified = DateTime.MinValue;

                    if (dr["LoanStageId"] != DBNull.Value)
                        task.LoanStageId = (int)dr["LoanStageId"];
                    else
                        task.LoanStageId = 0;
                    if (dr["Created"] != DBNull.Value)
                        task.Created = (DateTime)dr["Created"];
                    else
                        task.Created = DateTime.MinValue;
                    if (dr["DaysDueFromEstClose"] != DBNull.Value)
                        task.DaysFromEstClose = (short)dr["DaysDueFromEstClose"];
                    else
                        task.DaysFromEstClose = 0;
                    if (dr["PrerequisiteTaskId"] != DBNull.Value)
                        task.PrerequisiteTaskId = (int)dr["PrerequisiteTaskId"];
                    else
                        task.PrerequisiteTaskId = 0;
                    if (dr["DaysDueAfterPrerequisite"] != DBNull.Value)
                        task.DaysAfterPrerequisiteTask = (short)dr["DaysDueAfterPrerequisite"];
                    else
                        task.DaysAfterPrerequisiteTask = 0;
                    if (dr["SequenceNumber"] != DBNull.Value)
                        task.SequenceNumber = (short)dr["SequenceNumber"];
                    else
                        task.SequenceNumber = -1;
                    if (dr["TemplTaskId"] != DBNull.Value)
                        task.TemplTaskId = (int)dr["TemplTaskId"];
                    else
                        task.TemplTaskId = 0;
                    if (dr["WflTemplId"] != DBNull.Value)
                        task.WflTemplId = (int)dr["WflTemplId"];
                    else
                        task.WflTemplId = 0;

                    if (dr["Owner"] != DBNull.Value)
                        task.Owner = (int)dr["Owner"];

                    if (dr["WarningEmailId"] != DBNull.Value)
                        task.WarmingEmailId = (int)dr["WarningEmailId"];

                    if (dr["OverdueEmailId"] != DBNull.Value)
                        task.OverdueEmailId = (int)dr["OverdueEmailId"];

                    taskList.Add(task);
                }
                return taskList;
            }
            catch (Exception ex)
            {
                err = "GetLoanTasks, Exception: " + ex.Message;
                return taskList;
            }
        }

        public bool DeleteObsoleteLoanAlerts(ref string err)
        {
            err = "";

            string sqlCmd = string.Empty;
            try
            {
                sqlCmd = "Delete LoanAlerts where FileId not in (select FileId from Loans) ";
                sqlCmd += "; Delete LoanAlerts where LoanTaskId > 0 and LoanTaskId not in (select LoanTaskId from LoanTasks)";
                sqlCmd += "; Delete EmailQue where LoanAlertId > 0 and LoanAlertId not in (select LoanAlertId from LoanAlerts) ";
                sqlCmd += "; Delete EmailQue where FileId not in (select FileId from Loans)";
                sqlCmd += "; Delete EmailQue where LoanAlertId > 0 and LoanAlertId in (select LoanAlertId from EmailLog where Retries > 0)";
                DbHelperSQL.ExecuteNonQuery(sqlCmd);
                return true;
            }
            catch (Exception ex)
            {
                err = "DeleteObsoleteLoanAlerts, Exception: " + ex.Message;
                return false;
            }
        }

        public bool DeleteLoanAlerts(int fileId, ref string err)
        {
            err = "";
            if (fileId <= 0)
            {
                err = "DeleteLoanAlerts:: Invalid FileId =" + fileId + " specified.";
                return false;
            }

            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@FileId", SqlDbType.Int, 4)                 //0 
                };

                parameters[0].Value = fileId;
                // this stored procedure checks and save the task alert
                DbHelperSQL.RunProcedure("DeleteLoanAlerts", parameters);
                return true;
            }
            catch (Exception ex)
            {
                err = "DeleteLoanAlerts, Exception: " + ex.Message;
                return false;
            }
        }

        public bool DeleteEmailQue(int EmailId, ref string err)
        {
            err = "";
            if (EmailId <= 0)
            {
                err = "Invalid EmailId =" + EmailId + " specified.";
                return false;
            }

            string sqlCmd = "Delete EmailQue Where EmailId=" + EmailId;

            try
            {
                DbHelperSQL.ExecuteSql(sqlCmd);
                return true;
            }
            catch (Exception ex)
            {
                err = "DeleteEmailQue, Exception: " + ex.Message;
                return false;
            }
        }

        public bool DeleteLoanNotes(int NoteId, ref string err)
        {
            err = "";
            if (NoteId <= 0)
            {
                err = "Invalid NoteId =" + NoteId + " specified.";
                return false;
            }

            string sqlCmd = "Delete LoanNotes Where NoteId=" + NoteId;

            try
            {
                DbHelperSQL.ExecuteSql(sqlCmd);
                return true;
            }
            catch (Exception ex)
            {
                err = "DeleteLoanNotes, Exception: " + ex.Message;
                return false;
            }
        }

        public bool DeleteLoanNotesBlank(int FileId, ref string err)
        {
            err = "";
            if (FileId <= 0)
            {
                err = "Invalid FileId =" + FileId + " specified.";
                return false;
            }

            string sqlCmd = string.Format("delete from loannotes where fileid={0} and Note=''", FileId);

            try
            {
                DbHelperSQL.ExecuteSql(sqlCmd);
                return true;
            }
            catch (Exception ex)
            {
                err = "DeleteLoanNotesBlank, Exception: " + ex.Message;
                return false;
            }
        }

        public List<Table.LoanStatus> GetInactiveProspectLoans(ref string err)
        {
            err = "";
            string sqlCmd = "Select FileId, ProspectLoanStatus from Loans WHERE Status ='" + LoanStatus.LoanStatus_Prospect + "' AND ProspectLoanStatus<>'Active'";
            List<Table.LoanStatus> LoanStatusList = null;
            try
            {
                DataSet ds = null;
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return LoanStatusList;
                }
                LoanStatusList = new List<Table.LoanStatus>();
                LoanStatusList.Clear();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[0] == DBNull.Value)
                        continue;
                    LoanStatusList.Add(new Table.LoanStatus()
                    {
                        FileId = dr[0] == DBNull.Value ? 0 : (int)dr[0],
                        Status = dr[1] == DBNull.Value ? "Inactive" : dr[1].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
            }
            return LoanStatusList;
        }

        public List<Table.LoanStatus> GetInactiveLoans(ref string err)
        {
            err = "";
            string sqlCmd = "Select FileId, Status from Loans WHERE Status<>'" + LoanStatus.LoanStatus_Processing + "' AND Status<>'" + LoanStatus.LoanStatus_Prospect + "'";
            List<Table.LoanStatus> LoanStatusList = null;
            try
            {
                DataSet ds = null;
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    err = "No inactive loans available.";
                    return LoanStatusList;
                }
                LoanStatusList = new List<Table.LoanStatus>();
                LoanStatusList.Clear();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[0] == DBNull.Value)
                        continue;
                    Table.LoanStatus ls = new Table.LoanStatus();
                    ls.FileId = (int)dr[0];
                    if (dr[1] != DBNull.Value)
                        ls.Status = dr[1].ToString();
                    LoanStatusList.Add(ls);
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
            }
            return LoanStatusList;

        }

        public bool RemoveProspectAlerts(int ProspectId, ref string err)
        {
            err = "";
            if (ProspectId <= 0)
            {
                err = "RemoveProspectAlerts, invalid ProspectId=" + ProspectId;
                return false;
            }
            try
            {
                SqlParameter[] parameters = {
					new SqlParameter("@ProspectContactId", SqlDbType.Int)
					};
                parameters[0].Value = ProspectId;

                int rowsAffected;
                DbHelperSQL.RunProcedure("[dbo].[RemoveProspectAlerts]", parameters, out rowsAffected);
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return false;
            }
            return true;
        }

        public bool MonitorProspectTasks(int ProspectId, ref string err)
        {
            err = "";
            if (ProspectId <= 0)
            {
                err = "MonitorProspectTasks, invalid ProspectId=" + ProspectId;
                return false;
            }
            try
            {
                SqlParameter[] parameters = {
					new SqlParameter("@ProspectContactId", SqlDbType.Int)
					};
                parameters[0].Value = ProspectId;

                int rowsAffected;
                DbHelperSQL.RunProcedure("[dbo].[MonitorProspectTasks]", parameters, out rowsAffected);
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return false;
            }
            return true;
        }

        public List<int> GetInactiveProspects(ref string err)
        {
            err = "";
            string sqlCmd = "Select ContactId from Prospect WHERE Status<>'" + LoanStatus.LoanStatus_Prospect_Active + "'";
            List<int> ProspectList = null;
            try
            {
                DataSet ds = null;
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    err = "No active Prospect loans available.";
                    return ProspectList;
                }
                ProspectList = new List<int>();
                ProspectList.Clear();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[0] == DBNull.Value)
                        continue;
                    int ProspectId = (int)dr[0];
                    ProspectList.Add(ProspectId);
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
            }
            return ProspectList;
        }

        public List<int> GetActiveProspects(ref string err)
        {
            err = "";
            string sqlCmd = "Select ContactId from Prospect WHERE Status='" + LoanStatus.LoanStatus_Prospect_Active + "'";
            List<int> ProspectList = null;
            try
            {
                DataSet ds = null;
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    err = "No active Prospect loans available.";
                    return ProspectList;
                }
                ProspectList = new List<int>();
                ProspectList.Clear();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[0] == DBNull.Value)
                        continue;
                    int ProspectId = (int)dr[0];
                    ProspectList.Add(ProspectId);
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
            }
            return ProspectList;
        }

        public List<Table.LoanStatus> GetActiveProspectLoans(ref string err)
        {
            err = "";
            string sqlCmd = "Select FileId from Loans WHERE Status='" + LoanStatus.LoanStatus_Prospect + "' AND ProspectLoanStatus='" + LoanStatus.LoanStatus_Prospect_Active + "'";
            List<Table.LoanStatus> LoanStatusList = null;
            try
            {
                DataSet ds = null;
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    err = "No active Prospect loans available.";
                    return LoanStatusList;
                }
                LoanStatusList = new List<Table.LoanStatus>();
                LoanStatusList.Clear();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[0] == DBNull.Value)
                        continue;
                    LoanStatusList.Add(new Table.LoanStatus()
                    {
                        FileId = dr[0] == DBNull.Value ? 0 : (int)dr[0],
                        Status = LoanStatus.LoanStatus_Prospect
                    }
                    );
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
            }
            return LoanStatusList;

        }

        public List<Table.LoanStatus> GetActiveLoans(ref string err)
        {
            err = "";
            string sqlCmd = string.Format("Select FileId from Loans WHERE Status='{0}' union Select ls.FileId from LoanStages ls inner join Loans l on ls.FileId=l.FileId where l.Status='{1}' AND ls.StageName='{2}'",
                LoanStatus.LoanStatus_Processing, LoanStatus.LoanStatus_Closed, LoanStatus.LoanStatus_PostClose);
            List<Table.LoanStatus> LoanStatusList = null;
            try
            {
                DataSet ds = null;
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    err = "No active loans available.";
                    return LoanStatusList;
                }
                LoanStatusList = new List<Table.LoanStatus>();
                LoanStatusList.Clear();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[0] == DBNull.Value)
                        continue;
                    Table.LoanStatus ls = new Table.LoanStatus();
                    ls.FileId = (int)dr[0];
                    ls.Status = LoanStatus.LoanStatus_Processing;
                    LoanStatusList.Add(ls);
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
            }
            return LoanStatusList;
        }

        public bool IsClosedLoan(int FileId)
        {
            bool Active = false;
            if (FileId <= 0)
                return true;
            string sqlCmd = string.Format("select Status from Loans where FileId={0}", FileId);
            object obj = DbHelperSQL.GetSingle(sqlCmd);
            Active = (obj != null && obj.ToString().ToUpper() == "CLOSED") ? true : false;
            return Active;
        }
        #endregion

        #region Conditions & Docs
        public bool UpdateConditions(int FileId, List<Record.Conditions> condList, ref string err)
        {
            err = string.Empty;
            bool logErr = false;

            if (FileId <= 0)
            {
                err = string.Format("UpdateConditions invalid FileId {0} specified.", FileId);
                logErr = true;
                return false;
            }
            string sqlStr = string.Empty;
            try
            {
                if (condList == null || condList.Count <= 0)
                {
                    sqlStr = string.Format("Delete LoanConditions where FileId={0}", FileId);
                    DbHelperSQL.ExecuteSql(sqlStr);
                    return true;
                }
                foreach (Record.Conditions cond in condList)
                {
                    if (cond.Delete && cond.LoanCondId > 0)
                    {
                        sqlStr = string.Format("Delete LoanConditions where LoanCondId={0} and FileId={1}", cond.LoanCondId, FileId);
                        DbHelperSQL.ExecuteSql(sqlStr);
                        continue;
                    }
                    if (string.IsNullOrEmpty(cond.CondName))
                        continue;
                    if (cond.LoanCondId > 0)
                    {
                        sqlStr = @"Update LoanConditions 
                                    SET CondType=@CondType,
                                        Status=@Status,
                                        Created=@Created,
                                        CreatedBy=@CreatedBy,
                                        Received=@Received,
                                        ReceivedBy=@ReceivedBy,
                                        Collected=@Collected,
                                        CollectedBy=@CollectedBy,
                                        Submitted=@Submitted,
                                        SubmittedBy=@SubmittedBy,
                                        Cleared=@Cleared,
                                        ClearedBy=@ClearedBy,
                                        Sequence=@Sequence,
                                        ExternalViewing=@ExternalViewing
                                   where LoanCondId=@LoanCondId and FileId=@FileId";
                        SqlCommand sqlCmd = new SqlCommand(sqlStr);
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.Parameters.AddWithValue("@CondType", cond.CondType);
                        sqlCmd.Parameters.AddWithValue("@Status", cond.Status);
                        sqlCmd.Parameters.AddWithValue("@Created", cond.Created);
                        sqlCmd.Parameters.AddWithValue("@CreatedBy", cond.CreatedBy);
                        sqlCmd.Parameters.AddWithValue("@Received", (cond.Received == DateTime.MinValue) ? DBNull.Value : (object)cond.Received);
                        sqlCmd.Parameters.AddWithValue("@ReceivedBy", cond.ReceivedBy);
                        sqlCmd.Parameters.AddWithValue("@Collected", cond.Collected == DateTime.MinValue ? DBNull.Value : (object)cond.Collected);
                        sqlCmd.Parameters.AddWithValue("@CollectedBy", cond.CollectedBy);
                        sqlCmd.Parameters.AddWithValue("@Submitted", cond.Submitted == DateTime.MinValue ? DBNull.Value : (object)cond.Submitted);
                        sqlCmd.Parameters.AddWithValue("@SubmittedBy", cond.SubmittedBy);
                        sqlCmd.Parameters.AddWithValue("@Cleared", cond.Cleared == DateTime.MinValue ? DBNull.Value : (object)cond.Cleared);
                        sqlCmd.Parameters.AddWithValue("@ClearedBy", cond.ClearedBy);
                        sqlCmd.Parameters.AddWithValue("@Sequence", cond.Sequence);
                        sqlCmd.Parameters.AddWithValue("@ExternalViewing", cond.ExternalViewing);
                        sqlCmd.Parameters.AddWithValue("@LoanCondId", cond.LoanCondId);
                        sqlCmd.Parameters.AddWithValue("@FileId", FileId);
                        DbHelperSQL.ExecuteNonQuery(sqlCmd);
                        continue;
                    }
                    sqlStr = @"Insert into LoanConditions (
                                        CondName,
                                        CondType,
                                        Status,
                                        Created,
                                        CreatedBy,
                                        Received,
                                        ReceivedBy,
                                        Collected,
                                        CollectedBy,
                                        Submitted,
                                        SubmittedBy,
                                        Cleared,
                                        ClearedBy,
                                        Sequence,
                                        ExternalViewing,
                                        FileId) VALUES (
                                        @CondName,
                                        @CondType,
                                        @Status,
                                        @Created,
                                        @CreatedBy,
                                        @Received,
                                        @ReceivedBy,
                                        @Collected,
                                        @CollectedBy,
                                        @Submitted,
                                        @SubmittedBy,
                                        @Cleared,
                                        @ClearedBy,
                                        @Sequence,
                                        @ExternalViewing,
                                        @FileId)";
                    SqlCommand sqlCmd1 = new SqlCommand(sqlStr);
                    sqlCmd1.CommandType = CommandType.Text;
                    sqlCmd1.Parameters.AddWithValue("@CondName", cond.CondName);
                    sqlCmd1.Parameters.AddWithValue("@CondType", cond.CondType);
                    sqlCmd1.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(cond.Status) ? DBNull.Value : (object)cond.Status);
                    sqlCmd1.Parameters.AddWithValue("@Created", cond.Created == DateTime.MinValue ? DBNull.Value : (object)cond.Created);
                    sqlCmd1.Parameters.AddWithValue("@CreatedBy", cond.CreatedBy);
                    sqlCmd1.Parameters.AddWithValue("@Received", cond.Received == DateTime.MinValue ? DBNull.Value : (object)cond.Received);
                    sqlCmd1.Parameters.AddWithValue("@ReceivedBy", cond.ReceivedBy);
                    sqlCmd1.Parameters.AddWithValue("@Collected", cond.Collected == DateTime.MinValue ? DBNull.Value : (object)cond.Collected);
                    sqlCmd1.Parameters.AddWithValue("@CollectedBy", cond.CollectedBy);
                    sqlCmd1.Parameters.AddWithValue("@Submitted", cond.Submitted == DateTime.MinValue ? DBNull.Value : (object)cond.Submitted);
                    sqlCmd1.Parameters.AddWithValue("@SubmittedBy", cond.SubmittedBy);
                    sqlCmd1.Parameters.AddWithValue("@Cleared", cond.Cleared == DateTime.MinValue ? DBNull.Value : (object)cond.Cleared);
                    sqlCmd1.Parameters.AddWithValue("@ClearedBy", cond.ClearedBy);
                    sqlCmd1.Parameters.AddWithValue("@Sequence", cond.Sequence);
                    sqlCmd1.Parameters.AddWithValue("@ExternalViewing", cond.ExternalViewing);
                    sqlCmd1.Parameters.AddWithValue("@FileId", FileId);
                    DbHelperSQL.ExecuteNonQuery(sqlCmd1);
                }
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("UpdateConditions, FileId {0}, Exception: {1}", FileId, ex.Message);
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }
        public bool UpdateDocs(int FileId, List<Record.Docs> docList, ref string err)
        {
            err = string.Empty;
            bool logErr = false;

            if (FileId <= 0)
            {
                err = string.Format("UpdateDocs invalid FileId {0} specified.", FileId);
                logErr = true;
                return false;
            }
            string sqlStr = string.Empty;
            try
            {
                if (docList == null || docList.Count <= 0)
                {
                    sqlStr = string.Format("Delete LoanBasicDocs where FileId={0}", FileId);
                    DbHelperSQL.ExecuteSql(sqlStr);
                    return true;
                }
                foreach (Record.Docs doc in docList)
                {
                    if (doc.Delete && doc.BasicDocId > 0)
                    {
                        sqlStr = string.Format("Delete LoanBasicDocs where BasicDocId={0} and FileId={1}", doc.BasicDocId, FileId);
                        DbHelperSQL.ExecuteSql(sqlStr);
                        continue;
                    }

                    if (doc.BasicDocId > 0)
                    {
                        sqlStr = @"Update LoanBasicDocs 
                                    SET Status=@Status,
                                        Due=@Due,
                                        Ordered=@Ordered,
                                        OrderedBy=@OrderedBy,
                                        Received=@Received,
                                        ReceivedBy=@ReceivedBy,
                                        Submitted=@Submitted,
                                        SubmittedBy=@SubmittedBy,
                                        Cleared=@Cleared,
                                        ClearedBy=@ClearedBy
                                    where BasicDocId=@BasicDocId and FileId=@FileId";
                        SqlCommand sqlCmd = new SqlCommand(sqlStr);
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.Parameters.AddWithValue("@Status", doc.Status);
                        sqlCmd.Parameters.AddWithValue("@Due", doc.Due == DateTime.MinValue ? DBNull.Value : (object)doc.Due);
                        sqlCmd.Parameters.AddWithValue("@Ordered", doc.Ordered == DateTime.MinValue ? DBNull.Value : (object)doc.Ordered);
                        sqlCmd.Parameters.AddWithValue("@OrderedBy", string.IsNullOrEmpty(doc.OrderedBy) ? DBNull.Value : (object)doc.OrderedBy);
                        sqlCmd.Parameters.AddWithValue("@Received", doc.Received == DateTime.MinValue ? DBNull.Value : (object)doc.Received);
                        sqlCmd.Parameters.AddWithValue("@ReceivedBy", string.IsNullOrEmpty(doc.ReceivedBy) ? DBNull.Value : (object)doc.ReceivedBy);
                        sqlCmd.Parameters.AddWithValue("@Submitted", doc.Submitted == DateTime.MinValue ? DBNull.Value : (object)doc.Submitted);
                        sqlCmd.Parameters.AddWithValue("@SubmittedBy", string.IsNullOrEmpty(doc.SubmittedBy) ? DBNull.Value : (object)doc.SubmittedBy);
                        sqlCmd.Parameters.AddWithValue("@Cleared", doc.Cleared == DateTime.MinValue ? DBNull.Value : (object)doc.Cleared);
                        sqlCmd.Parameters.AddWithValue("@ClearedBy", string.IsNullOrEmpty(doc.ClearedBy) ? DBNull.Value : (object)doc.ClearedBy);
                        sqlCmd.Parameters.AddWithValue("@BasicDocId", doc.BasicDocId);
                        sqlCmd.Parameters.AddWithValue("@FileId", FileId);
                        DbHelperSQL.ExecuteNonQuery(sqlCmd);
                        continue;
                    }
                    sqlStr = @"Insert into LoanBasicDocs (DocName, Status, Due, Ordered, OrderedBy,Received,ReceivedBy,Submitted,SubmittedBy,Cleared,ClearedBy,FileId) VALUES (
                                        @DocName,@Status,@Due, @Ordered, @OrderedBy,@Received,@ReceivedBy,@Submitted,@SubmittedBy,@Cleared,@ClearedBy,@FileId)";
                    SqlCommand sqlCmd1 = new SqlCommand(sqlStr);
                    sqlCmd1.CommandType = CommandType.Text;
                    sqlCmd1.Parameters.AddWithValue("@DocName", doc.DocName);
                    if (string.IsNullOrEmpty(doc.Status))
                        sqlCmd1.Parameters.AddWithValue("@Status", DBNull.Value);
                    else
                        sqlCmd1.Parameters.AddWithValue("@Status", doc.Status);
                    if (doc.Due == DateTime.MinValue)
                        sqlCmd1.Parameters.AddWithValue("@Due", DBNull.Value);
                    else
                        sqlCmd1.Parameters.AddWithValue("@Due", doc.Due);
                    if (doc.Ordered == DateTime.MinValue)
                        sqlCmd1.Parameters.AddWithValue("@Ordered", DBNull.Value);
                    else
                        sqlCmd1.Parameters.AddWithValue("@Ordered", doc.Ordered);
                    if (string.IsNullOrEmpty(doc.OrderedBy))
                        sqlCmd1.Parameters.AddWithValue("@OrderedBy", DBNull.Value);
                    else
                        sqlCmd1.Parameters.AddWithValue("@OrderedBy", doc.OrderedBy);
                    if (doc.Received == DateTime.MinValue)
                        sqlCmd1.Parameters.AddWithValue("@Received", DBNull.Value);
                    else
                        sqlCmd1.Parameters.AddWithValue("@Received", doc.Received);
                    if (string.IsNullOrEmpty(doc.ReceivedBy))
                        sqlCmd1.Parameters.AddWithValue("@ReceivedBy", DBNull.Value);
                    else
                        sqlCmd1.Parameters.AddWithValue("@ReceivedBy", doc.ReceivedBy);
                    if (doc.Submitted == DateTime.MinValue)
                        sqlCmd1.Parameters.AddWithValue("@Submitted", DBNull.Value);
                    else
                        sqlCmd1.Parameters.AddWithValue("@Submitted", doc.Submitted);
                    if (string.IsNullOrEmpty(doc.SubmittedBy))
                        sqlCmd1.Parameters.AddWithValue("@SubmittedBy", DBNull.Value);
                    else
                        sqlCmd1.Parameters.AddWithValue("@SubmittedBy", doc.SubmittedBy);
                    if (doc.Cleared == DateTime.MinValue)
                        sqlCmd1.Parameters.AddWithValue("@Cleared", DBNull.Value);
                    else
                        sqlCmd1.Parameters.AddWithValue("@Cleared", doc.Cleared);
                    if (string.IsNullOrEmpty(doc.ClearedBy))
                        sqlCmd1.Parameters.AddWithValue("@ClearedBy", DBNull.Value);
                    else
                        sqlCmd1.Parameters.AddWithValue("@ClearedBy", doc.ClearedBy);
                    sqlCmd1.Parameters.AddWithValue("@FileId", FileId);
                    DbHelperSQL.ExecuteNonQuery(sqlCmd1);
                }
                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("UpdateDocs, FileId {0}, Exception: {1}", FileId, ex.Message);
                logErr = true;
                return false;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }
        public List<Record.Conditions> GetConditionList(int FileId, ref string err)
        {
            List<Record.Conditions> condList = null;
            err = string.Empty;
            bool logErr = false;

            if (FileId <= 0)
            {
                err = string.Format("GetConditionList invalid FileId {0} specified.", FileId);
                logErr = true;
                return condList;
            }
            string sqlCmd = string.Format("Select * from LoanConditions where FileId={0}", FileId);
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                    return condList;
                condList = new List<Record.Conditions>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Record.Conditions cond = new Record.Conditions
                    {
                        LoanCondId = (int)dr["LoanCondId"],
                        FileId = (int)dr["FileId"],
                        CondName = dr["CondName"] == DBNull.Value ? string.Empty : dr["CondName"].ToString(),
                        CondType = dr["CondType"] == DBNull.Value ? string.Empty : dr["CondType"].ToString(),
                        Status = dr["Status"] == DBNull.Value ? string.Empty : dr["Status"].ToString(),
                        Created = dr["Created"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["Created"],
                        CreatedBy = dr["CreatedBy"] == DBNull.Value ? string.Empty : dr["CreatedBy"].ToString(),
                        Received = dr["Received"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["Received"],
                        ReceivedBy = dr["ReceivedBy"] == DBNull.Value ? string.Empty : dr["ReceivedBy"].ToString(),
                        //Collected = dr["Collected"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["Collected"],
                        CollectedBy = dr["CollectedBy"] == DBNull.Value ? string.Empty : dr["CollectedBy"].ToString(),
                        Submitted = dr["Submitted"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["Submitted"],
                        SubmittedBy = dr["SubmittedBy"] == DBNull.Value ? string.Empty : dr["SubmittedBy"].ToString(),
                        Cleared = dr["Cleared"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["Cleared"],
                        ClearedBy = dr["ClearedBy"] == DBNull.Value ? string.Empty : dr["ClearedBy"].ToString(),
                        Sequence = dr["Sequence"] == DBNull.Value ? 0 : (int)dr["Sequence"],
                        Due = dr["Due"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["Due"],
                        ExternalViewing = dr["ExternalViewing"] == DBNull.Value ? false : (bool)dr["ExternalViewing"]
                    };
                    if (string.IsNullOrEmpty(cond.CondName))
                        continue;
                    condList.Add(cond);
                }
            }
            catch (Exception ex)
            {
                err = string.Format("GetConditionList, FileId {0}, Exception: {1}.", FileId, ex.Message);
                logErr = true;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
            return condList;
        }

        public List<Record.Docs> GetDocList(int FileId, ref string err)
        {
            List<Record.Docs> docList = null;
            err = string.Empty;
            bool logErr = false;

            if (FileId <= 0)
            {
                err = string.Format("GetDocList invalid FileId {0} specified.", FileId);
                logErr = true;
                return docList;
            }
            string sqlCmd = string.Format("Select * from LoanBasicDocs where FileId={0}", FileId);
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                    return docList;
                docList = new List<Record.Docs>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Record.Docs doc = new Record.Docs
                    {
                        BasicDocId = (int)dr["BasicDocId"],
                        FileId = (int)dr["FileId"],
                        DocName = dr["DocName"] == DBNull.Value ? string.Empty : dr["DocName"].ToString(),
                        Status = dr["Status"] == DBNull.Value ? string.Empty : dr["Status"].ToString(),
                        Ordered = dr["Ordered"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["Ordered"],
                        OrderedBy = dr["OrderedBy"] == DBNull.Value ? string.Empty : dr["OrderedBy"].ToString(),
                        ReOrdered = dr["ReOrdered"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["ReOrdered"],
                        ReOrderedBy = dr["ReOrderedBy"] == DBNull.Value ? string.Empty : dr["ReOrderedBy"].ToString(),
                        Received = dr["Received"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["Received"],
                        ReceivedBy = dr["ReceivedBy"] == DBNull.Value ? string.Empty : dr["ReceivedBy"].ToString(),
                        Submitted = dr["Submitted"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["Submitted"],
                        SubmittedBy = dr["SubmittedBy"] == DBNull.Value ? string.Empty : dr["SubmittedBy"].ToString(),
                        Cleared = dr["Cleared"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["Cleared"],
                        ClearedBy = dr["ClearedBy"] == DBNull.Value ? string.Empty : dr["ClearedBy"].ToString(),
                        Due = dr["Due"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["Due"],
                    };
                    if (string.IsNullOrEmpty(doc.DocName))
                        continue;
                    docList.Add(doc);
                }
            }
            catch (Exception ex)
            {
                err = string.Format("GetDocList, FileId {0}, Exception: {1}.", FileId, ex.Message);
                logErr = true;
            }
            finally
            {
                if (logErr)
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
            }
            return docList;
        }
        #endregion

        #region RequestQUeue
        public void AddRequestQueue(int userId, string reqType, string msg, ref string err)
        {
            err = string.Empty;
            string sqlCmd = string.Format("Insert into dbo.RequestQueue (StartTime, RequestType, Userid, Error) values ('{0}', '{1}', {2}, '{3}')",
                                        DateTime.Now.ToString(), reqType, userId, msg);
            try
            {
                DbHelperSQL.ExecuteNonQuery(sqlCmd);
            }
            catch (Exception e)
            {
                err = "Failed to add request to the queue, sqlCmd=" + sqlCmd + ", Exception:" + e.Message;
                Trace.TraceError(err);
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
            }
        }

        public bool AddRequestQueue(int userId, string reqType, ref int reqId, ref string err)
        {
            string msgDetail = err;
            err = "";
            if (userId <= 0)
            {
                //err = "User Id is not valid, UserId=" + userId.ToString();
                //return false;
                userId = 0;
            }

            if (reqType == String.Empty)
            {
                err = "Request Type is empty.";
                return false;
            }

            if (reqId <= 0)
                reqId = 0;

            try
            {

                SqlParameter[] parameters = {                    
                     new SqlParameter("@RequestType", SqlDbType.NVarChar, 50),           //0
                     new SqlParameter("@UserId", SqlDbType.Int, 4),                      //1
                     new SqlParameter("@RequestId", SqlDbType.Int, 4 )                   //2
                   };

                parameters[0].Value = reqType;
                parameters[1].Value = userId;
                parameters[2].Value = DBNull.Value;
                parameters[2].Direction = ParameterDirection.Output;
                int rows = 0;

                reqId =
                    DbHelperSQL.RunProcedure("AddRequestToQueue", parameters, out rows);

                if (reqId <= 0)
                {
                    err = "Failed to add request to the queue, ReqType=" + reqType + ", userId=" + userId.ToString();
                    Trace.TraceError(err);
                    return false;
                }
            }
            catch (Exception e)
            {
                err = "Failed to add request to the queue, ReqType=" + reqType + ", userId=" + userId.ToString() + ", Exception:" + e.Message;
                Trace.TraceError(err);
                return false;
            }

            return true;
        }

        public bool UpdateRequestQueue(int userId, string reqType, ref int reqId, bool success, string errMsg, ref string err)
        {
            err = "";

            if (userId <= 0)
            {
                userId = 0;
                //err = "User Id is not valid, UserId=" + userId.ToString();
                //return false;
            }

            if (reqType == String.Empty)
            {
                err = "Request Type is empty.";
                return false;
            }

            if (reqId <= 0)
            {
                err = "Request Id must be greater than zero.";
                return false;
            }

            try
            {
                SqlParameter[] parameters = {                    
                    new SqlParameter("@RequestId", SqlDbType.Int, 4 ),                  //0
                    new SqlParameter("@RequestType", SqlDbType.NVarChar, 50),           //1
                    new SqlParameter("@UserId", SqlDbType.Int, 4),                      //2
                    new SqlParameter("@Success", SqlDbType.Bit),                        //3
                    new SqlParameter("@Error", SqlDbType.NVarChar, 255)                 //4
                   };

                parameters[0].Value = reqId;
                //parameters[0].Direction = ParameterDirection.Output; 
                parameters[1].Value = reqType;
                parameters[2].Value = userId;
                parameters[3].Value = success;
                parameters[4].Value = errMsg;

                int rows = 0;

                reqId =
                    DbHelperSQL.RunProcedure("UpdateRequestStatus", parameters, out rows);

                if (reqId <= 0)
                {
                    err = "Failed to update request queue, ReqType=" + reqType + ", userId=" + userId.ToString();
                    return false;
                }
            }
            catch (Exception e)
            {
                err = "Failed to update request queue, ReqType=" + reqType + ", userId=" + userId.ToString() + ", Exception:" + e.Message;
                Trace.TraceError(err);
                return false;
            }

            return true;
        }

        public bool UpdateReqProgress(int userId, string reqType, ref int reqId, int total, int processed, int failed, ref string err)
        {
            err = "";

            if (userId <= 0)
            {
                userId = 0;
            }

            if (reqType == String.Empty)
            {
                err = "Request Type is empty.";
                return false;
            }

            if (reqId <= 0)
            {
                err = "Request Id must be greater than zero.";
                return false;
            }

            try
            {
                SqlParameter[] parameters = {                    
                    new SqlParameter("@RequestId", SqlDbType.Int, 4 ),                               //0
                    new SqlParameter("@RequestType", SqlDbType.NVarChar, 50),             //1
                    new SqlParameter("@UserId", SqlDbType.Int, 4),                                      //2
                    new SqlParameter("@Total", SqlDbType.Int, 4),                                          //3
                    new SqlParameter("@Processed", SqlDbType.Int, 4),                               //4
                    new SqlParameter("@Failed", SqlDbType.Int, 4)                                        //5
                   };
                parameters[0].Value = reqId;
                //parameters[0].Direction = ParameterDirection.Output;
                parameters[1].Value = reqType;
                parameters[2].Value = userId;
                parameters[3].Value = total;
                parameters[4].Value = processed;
                parameters[5].Value = failed;

                int rows = 0;

                reqId =
                    DbHelperSQL.RunProcedure("UpdateReqProgress", parameters, out rows);

                if (reqId <= 0)
                {
                    err = "Failed to update request progress, ReqType=" + reqType + ", userId=" + userId.ToString();
                    return false;
                }
            }
            catch (Exception e)
            {
                err = "Failed to update request progress, ReqType=" + reqType + ", userId=" + userId.ToString() + ", Exception:" + e.Message;
                Trace.TraceError(err);
                return false;
            }

            return true;
        }
        #endregion
        #region  User Management
        public List<LP2.Service.Common.User> GetUserAccounts(ref string err)
        {
            err = "";
            string sqlCmd = "Select userid, username, firstname, lastname, emailaddress, userenabled from users";
            List<LP2.Service.Common.User> userList = null;
            try
            {
                DataSet ds = null;
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    err = "No user accounts available.";
                    return userList;
                }
                userList = new List<LP2.Service.Common.User>();
                LP2.Service.Common.User u = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    u = new LP2.Service.Common.User();
                    if (dr["userid"] != DBNull.Value)
                        u.UserId = (int)dr["userid"];
                    else
                        continue;
                    if (dr["username"] != DBNull.Value)
                        u.Username = dr["username"].ToString();
                    else
                        continue;
                    if (dr["firstname"] != DBNull.Value)
                        u.Firstname = dr["firstname"].ToString();
                    if (dr["lastname"] != DBNull.Value)
                        u.Lastname = dr["lastname"].ToString();
                    if (dr["emailaddress"] != DBNull.Value)
                        u.Email = dr["emailaddress"].ToString();
                    string enableStr = "false";
                    if (dr["userenabled"] != DBNull.Value)
                        enableStr = dr["userenabled"].ToString().ToLower();
                    if (enableStr == "true")
                        u.Enabled = true;
                    else u.Enabled = false;
                    userList.Add(u);
                }
            }
            catch (Exception ex)
            {
                err = "Failed to get user accounts, Exception:" + ex.Message;
                Trace.TraceError(err);
                return userList;
            }
            return userList;

        }
        public User GetUserAccount(string login, ref string err)
        {
            User u = null;
            err = "";
            login = login.Trim();
            if ((login == null) || (login == String.Empty))
            {
                err = "GetUserAccount " + "user account is empty.";
                return u;
            }
            try
            {
                string sqlCmd = "Select Top 1 userid, username, firstname, lastname, emailaddress, userenabled from users WHERE username='" + login + "'";
                DataSet ds = null;
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    err = "GetUserAccount " + " user account " + login + " not found in the database.";
                    return u;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr == null)
                {
                    err = "GetUserAccount " + " user account " + login + " not found in the database.";
                    return u;
                }
                u = new User();
                if (dr["userid"] != DBNull.Value)
                    u.UserId = (int)dr["userid"];
                else
                    return null;
                if (dr["username"] != DBNull.Value)
                    u.Username = dr["username"].ToString();
                else
                    return null;
                if (dr["firstname"] != DBNull.Value)
                    u.Firstname = dr["firstname"].ToString();
                if (dr["lastname"] != DBNull.Value)
                    u.Lastname = dr["lastname"].ToString();
                if (dr["emailaddress"] != DBNull.Value)
                    u.Email = dr["emailaddress"].ToString();
                string enableStr = "false";
                if (dr["userenabled"] != DBNull.Value)
                    enableStr = dr["userenabled"].ToString().ToLower();
                if (enableStr == "true")
                    u.Enabled = true;
                else u.Enabled = false;
                return u;
            }
            catch (Exception ex)
            {
                err = "GetUserAccount, Exception:" + ex.Message;
                Trace.TraceError(err);
                return u;
            }
        }


        public int UpdateUser(LP2.Service.Common.User user, ref string err)
        {
            err = "";
            int userId = 0;
            if (user == null)
            {
                err = "User info is empty.";
                return userId;
            }

            if (user.Username == String.Empty)
            {
                err = "Username is empty. ";
            }

            if (user.Firstname == String.Empty)
            {
                err += "User Firstname is empty. ";
            }

            if (user.Lastname == String.Empty)
            {
                err += "User Lastname is empty.";
            }

            if (err != String.Empty)
                return userId;

            try
            {
                SqlParameter[] parameters = {                    
                    new SqlParameter("@UserId", SqlDbType.Int, 4 ),                  //0
                    new SqlParameter("@Enabled", SqlDbType.Bit),                     //1
                    new SqlParameter("@UserName", SqlDbType.NVarChar, 50),           //2
                    new SqlParameter("@Email", SqlDbType.NVarChar, 255),             //3
                    new SqlParameter("@First", SqlDbType.NVarChar, 50),              //4
                    new SqlParameter("@Last", SqlDbType.NVarChar, 50)                //5
                   };

                parameters[0].Value = user.UserId;
                parameters[0].Direction = ParameterDirection.Output;
                parameters[1].Value = user.Enabled;
                parameters[2].Value = user.Username;
                if (string.IsNullOrEmpty(user.Email))
                    parameters[3].Value = DBNull.Value;
                else
                    parameters[3].Value = user.Email;
                parameters[4].Value = user.Firstname;
                parameters[5].Value = user.Lastname;
                int rows = 0;

                user.UserId =
                    DbHelperSQL.RunProcedure("UpdateUser", parameters, out rows);

                if (user.UserId <= 0)
                {
                    err = "Failed to update user, username=" + user.Username; ;
                    return userId;
                }
                userId = user.UserId;
            }
            catch (Exception e)
            {
                err = "Failed to update user, username=" + user.Username + ", Exception:" + e.Message;
                Trace.TraceError(err);
                return userId;
            }

            return userId;
        }

        public bool DisableUser(LP2.Service.Common.User u, ref string err)
        {
            err = "";

            if (u == null)
            {
                err = "User is empty.";
                return false;
            }

            string sqlCmd = "Update users set UserEnabled=0 where userid = " + u.UserId;

            try
            {
                DbHelperSQL.ExecuteSql(sqlCmd);
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return false;
            }

            return true;
        }

        #endregion
        #region ContactCompany & Contact Branches
        public bool Save_ContactBranch(string name, Address adr, int companyId, ref int branchId, ref string err)
        {
            err = "";
            if (name == string.Empty)
            {
                err = "Contact Branch name is empty.";
                return false;
            }

            if (adr == null)
            {
                err = "Contact Branch  address is empty. ";
                return false;
            }

            try
            {
                SqlParameter[] parameters = {                    
                new SqlParameter("@BranchId", SqlDbType.Int, 4 ),                           //0
                new SqlParameter("@Name", SqlDbType.NVarChar, 255),                         //1
                new SqlParameter("@Address", SqlDbType.NVarChar, 255),                      //2
                new SqlParameter("@City", SqlDbType.NVarChar, 100),                         //3
                new SqlParameter("@State", SqlDbType.NVarChar, 2),                          //4
                new SqlParameter("@Zip", SqlDbType.NVarChar, 12),                           //5
                new SqlParameter("@CompanyId", SqlDbType.Int, 4 ),                          //6
                new SqlParameter("@Enabled", SqlDbType.Bit)
                                            };
                parameters[0].Value = branchId <= 0 ? 0 : branchId;
                parameters[0].Direction = ParameterDirection.Output;
                parameters[1].Value = name;
                parameters[2].Value = adr == null ? string.Empty : adr.Street;
                parameters[3].Value = adr == null ? string.Empty : adr.City;
                parameters[4].Value = adr == null ? string.Empty : adr.State;
                parameters[5].Value = adr == null ? string.Empty : adr.Zip;
                parameters[6].Value = companyId;
                parameters[7].Value = 1;
                int rows = 0;
                branchId =
                    DbHelperSQL.RunProcedure("[dbo].[Save_Contact_Branch]", parameters, out rows);
                if (branchId <= 0)
                {
                    err = "Failed to update Contact Company, name=" + name;
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                err = "Failed to update Contact Company, name=" + name + ", Exception:" + e.Message;
                Trace.TraceError(err);
                return false;
            }
        }
        public bool Save_CardexCompany(string name, Address adr, string website, string service, ref int companyId, ref string err)
        {
            err = "";

            if (name == string.Empty)
            {
                err = "Company name is empty.";
                return false;
            }

            //if (adr == null)
            //{
            //    err = "Company address is empty. ";
            //    return false;
            //}

            try
            {
                SqlParameter[] parameters = {                    
                new SqlParameter("@CompanyId", SqlDbType.Int, 4 ),                     //0
                new SqlParameter("@Name", SqlDbType.NVarChar, 255),                    //1
                new SqlParameter("@Address", SqlDbType.NVarChar, 255),                 //2
                new SqlParameter("@City", SqlDbType.NVarChar, 100),                    //3
                new SqlParameter("@State", SqlDbType.NVarChar, 2),                     //4
                new SqlParameter("@Zip", SqlDbType.NVarChar, 12),                      //5
                new SqlParameter("@Website", SqlDbType.NVarChar, 255) ,                //6
                new SqlParameter("@ServiceTypes", SqlDbType.NVarChar, 255),            //7
                new SqlParameter("@Enabled", SqlDbType.Bit)
                };
                parameters[0].Value = companyId;
                parameters[0].Direction = ParameterDirection.Output;
                parameters[1].Value = name;
                parameters[2].Value = adr == null ? string.Empty : adr.Street;
                parameters[3].Value = adr == null ? string.Empty : adr.City;
                parameters[4].Value = adr == null ? string.Empty : adr.State;
                parameters[5].Value = adr == null ? string.Empty : adr.Zip;

                if (website.Length > 0)
                    parameters[6].Value = website;
                else
                    parameters[6].Value = DBNull.Value;

                if (service.Length > 0)
                    parameters[7].Value = service;
                else
                    parameters[7].Value = DBNull.Value;
                parameters[8].Value = 1;
                int rows = 0;
                companyId =
                    DbHelperSQL.RunProcedure("Save_Contact_Company", parameters, out rows);
                if (companyId <= 0)
                {
                    err = "Failed to update Contact Company, name=" + name;
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                err = "Failed to update Contact Company, name=" + name + ", Exception:" + e.Message;
                Trace.TraceError(err);
                return false;
            }
        }

        public bool Save_CardexContact(string firstName, string lastName, string title, Address adr, ContactInfo contact, int companyId, ref string err)
        {
            err = "";
            if (firstName == string.Empty)
                err = "Contact Firstname is empty.";

            if (lastName == string.Empty)
                err += " Contact Lastname is empty.";

            if (adr == null)
                err += " Contact Address is empty.";

            if (contact == null)
            {
                err += " Contact Info is empty.";
                return false;
            }

            try
            {
                int contactId = 0;
                SqlParameter[] parameters = {                    
                new SqlParameter("@ContactId", SqlDbType.Int, 4 ),                               //0
                new SqlParameter("@First", SqlDbType.NVarChar, 50),                          //1
                new SqlParameter("@Last", SqlDbType.NVarChar, 50),                          //2
                new SqlParameter("@Title", SqlDbType.NVarChar, 50),                          //3
                new SqlParameter("@Address", SqlDbType.NVarChar, 255),                 //4            
                new SqlParameter("@City", SqlDbType.NVarChar, 50),                           //5
                new SqlParameter("@State", SqlDbType.NVarChar, 2),                           //6
                new SqlParameter("@Zip", SqlDbType.NVarChar, 12),                            //7
                new SqlParameter("@Phone", SqlDbType.NVarChar, 20),                      //8
                new SqlParameter("@Email", SqlDbType.NVarChar, 255) ,                    //9
                new SqlParameter("@Fax", SqlDbType.NVarChar, 20) ,                         //10
                new SqlParameter("@Cell", SqlDbType.NVarChar, 20),                          //11
                 new SqlParameter("@CompanyId", SqlDbType.Int, 4)                           //12
                };
                parameters[0].Value = contactId;
                parameters[0].Direction = ParameterDirection.Output;
                parameters[1].Value = firstName;
                parameters[2].Value = lastName;
                parameters[3].Value = title;
                parameters[4].Value = adr.Street;
                parameters[5].Value = adr.City;
                parameters[6].Value = adr.State;
                parameters[7].Value = adr.Zip;
                parameters[8].Value = contact.BusinessPhone;
                parameters[9].Value = contact.Email;
                parameters[10].Value = contact.Fax;
                parameters[11].Value = contact.CellPhone;
                parameters[12].Value = companyId;
                int rows = 0;

                contactId = DbHelperSQL.RunProcedure("Save_Contact_Agents", parameters, out rows);

                if (contactId <= 0)
                {
                    err = "Failed to update  Contact, name=" + firstName + " " + lastName;
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                err = "Failed to update Cardex Contact, name=" + firstName + " " + lastName + ", Exception:" + e.Message;
                Trace.TraceError(err);
                return false;
            }

        }
        #endregion
        #region ConfigurationData
        public string GetCardexFilePath(ref string err)
        {
            err = "";
            string sqlCmd = "Select top 1 CardexFile from Company_Point";
            string CardexPath = "";
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    err = "No user accounts available.";
                    return CardexPath;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr[0] != DBNull.Value)
                    CardexPath = dr[0].ToString().Trim();
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
            return CardexPath;
        }

        public bool GetAlertDays(ref AlertDays alertDays, ref string err)
        {
            err = "";
            string sqlStr = "Select Top 1 from Company_Alerts";
            DataSet ds = null;

            try
            {
                ds = DbHelperSQL.Query(sqlStr);
                if ((ds == null) || (ds.Tables[0].Rows.Count == 0))
                {
                    err = "No record found in Company_Alerts";
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["AlertYellowDays"] != DBNull.Value)
                    alertDays.AlertYellowDays = (int)dr["AlertYellowDays"];
                else
                    alertDays.AlertYellowDays = 1;
                if (dr["AlertRedDays"] != DBNull.Value)
                    alertDays.AlertRedDays = (int)dr["AlertRedDays"];
                else
                    alertDays.AlertRedDays = 2;
                if (dr["TaskYellowDays"] != DBNull.Value)
                    alertDays.TaskYellowDays = (int)dr["TaskYellowDays"];
                else
                    alertDays.TaskYellowDays = 5;
                if (dr["TaskRedDays"] != DBNull.Value)
                    alertDays.TaskRedDays = (int)dr["TaskRedDays"];
                else
                    alertDays.TaskRedDays = 3;
                if (dr["RateLockYellowDays"] != DBNull.Value)
                    alertDays.RateLockYellowDays = (int)dr["RateLockYellowDays"];
                else
                    alertDays.RateLockYellowDays = 7;
                if (dr["RateLockRedDays"] != DBNull.Value)
                    alertDays.RateLockRedDays = (int)dr["RateLockRedDays"];
                else
                    alertDays.RateLockRedDays = 5;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                Trace.TraceError(err);
                return false;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
            return true;
        }

        public PointConfig GetPointConfigData(ref string err)
        {
            //string SQLString = "select TOP 1 WinpointIniPath, PointFieldIDMappingFile, CardexFile, PointImportintervalMinutes  from dbo.Company_Point";
            string SQLString = "select TOP 1 * from dbo.Company_Point";
            DataSet ds = null;
            err = "";
            PointConfig pc = null;

            try
            {
                ds = DbHelperSQL.Query(SQLString);
                if ((ds == null) || (ds.Tables[0].Rows.Count == 0))
                {
                    err = "No record found in Company_Point";
                    return pc;
                }
                pc = new PointConfig();
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["PointImportintervalMinutes"] == DBNull.Value)
                    pc.ImportInterval = 0;
                else
                    pc.ImportInterval = (short)dr["PointImportintervalMinutes"];
                if (pc.ImportInterval == 0)
                {
                    pc.ImportInterval = 15;
                }
                if (dr["WinpointIniPath"] == DBNull.Value)
                    pc.WinPointIni = "";
                else
                    pc.WinPointIni = dr["WinpointIniPath"].ToString();
                if (dr["PointFieldIDMappingFile"] == DBNull.Value)
                    pc.PointFldIdMap = "";
                else
                    pc.PointFldIdMap = dr["PointFieldIDMappingFile"].ToString();
                if (dr["CardexFile"] == DBNull.Value)
                    pc.CardexFile = "";
                else
                    pc.CardexFile = dr["CardexFile"].ToString();

                #region DataTrac

                pc.MasterSource = dr["MasterSource"] == DBNull.Value ? string.Empty : dr["MasterSource"].ToString();
                pc.TracToolsLogin = dr["TracToolsLogin"] == DBNull.Value ? string.Empty : dr["TracToolsLogin"].ToString();
                pc.TracToolsPwd = dr["TracToolsPwd"] == DBNull.Value ? string.Empty : dr["TracToolsPwd"].ToString();

                #endregion

                if (dr["Auto_ConvertLead"] == DBNull.Value)
                    pc.Auto_ConvertLead = false;
                else
                    pc.Auto_ConvertLead = Convert.ToBoolean(dr["Auto_ConvertLead"]);

                if (dr["AutoApplyProcessingWorkflow"] == DBNull.Value)
                    pc.AutoApplyProcessingWorkflow = false;
                else
                    pc.AutoApplyProcessingWorkflow = Convert.ToBoolean(dr["AutoApplyProcessingWorkflow"]);

                if (dr["AutoApplyProspectWorkflow"] == DBNull.Value)
                    pc.AutoApplyProspectWorkflow = false;
                else
                    pc.AutoApplyProspectWorkflow = Convert.ToBoolean(dr["AutoApplyProspectWorkflow"]);

                if (dr["Enable_MultiBranchFolders"] == DBNull.Value)
                    pc.Enable_MultiBranchFolders = false;
                else
                    pc.Enable_MultiBranchFolders = Convert.ToBoolean(dr["Enable_MultiBranchFolders"]);

                return pc;
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in Company_Point. " + ex.Message;
                Trace.TraceError(err);
                return pc;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public bool GetADConfigData(ref int ImportInterval, ref string err)
        {
            string SQLString = "select TOP 1 ImportUserInterval from dbo.Company_General";
            DataSet ds = null;
            err = "";

            try
            {
                ds = DbHelperSQL.Query(SQLString);
                if ((ds == null) || (ds.Tables[0].Rows.Count == 0))
                {
                    err = "No record found in Company_Point";
                    ImportInterval = 6;
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                if (dr["ImportUserInterval"] == DBNull.Value)
                    ImportInterval = 6;
                else
                    ImportInterval = (short)dr["ImportUserInterval"];

                if (ImportInterval == 0)
                {
                    ImportInterval = 6;
                }

                return true;
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in Company_General." + ex.Message;
                Trace.TraceError(err);
                return false;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public bool ShouldExportEmailAuditTrail(ref string err)
        {
            string SQLString = "select TOP 1 EnableEmailAuditTrail from dbo.Company_Web";
            DataSet ds = null;
            err = "";

            try
            {
                ds = DbHelperSQL.Query(SQLString);
                if ((ds == null) || (ds.Tables[0].Rows.Count == 0))
                {
                    err = "EnableEmailAuditTrail is not found in dbo.Company_Web table.";
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                if (dr["dbo.EnableEmailAuditTrail"] == DBNull.Value)
                    return false;
                else
                    return (bool)dr["EnableEmailAuditTrail"];
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in Company_Web." + ex.Message;
                Trace.TraceError(err);
                return false;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public int GetRuleMonitorInterval()
        {
            int RuleMonitorInterval = 4 * 60;             // default to every 4 hours in minutes
            try
            {
                string sqlCmd = "Select TOP 1 RuleMonitorInterval from Company_General";
                object tempInterval = DbHelperSQL.GetSingle(sqlCmd);
                if (tempInterval == null || tempInterval == DBNull.Value)
                    return RuleMonitorInterval;
                RuleMonitorInterval = (int)tempInterval <= 0 ? RuleMonitorInterval : (int)tempInterval;
            }
            catch (Exception ex)
            { }
            return RuleMonitorInterval;
        }

        public int GetRunningEdition()
        {
            int Edition = 0;             // default to free = 0
            try
            {
                string sqlCmd = "Select TOP 1 Edition from Company_General";
                object tempEdition = DbHelperSQL.GetSingle(sqlCmd);
                if (tempEdition == null || tempEdition == DBNull.Value)
                    return Edition;
                Edition = (int)tempEdition <= 0 ? Edition : (int)tempEdition;
            }
            catch (Exception ex)
            { }
            return Edition;
        }
        #endregion
        #region Email DB Hooks
        public List<Common.Table.EmailLog> GetEmailLogList(int fileId, string cond, ref string err)
        {
            bool logErr = false;
            var lstEmailLog = new List<Common.Table.EmailLog>();
            err = "";

            DataSet ds = null;
            if (fileId <= 0)
            {
                logErr = true;
                err = "GetEmailLogList Invalid FileId=" + fileId;
                return lstEmailLog;
            }
            string SQLString = "SELECT [EmailLogId],[Sender],[FromEmail],[Subject],[EmailBody],[FileId],[LastSent],[ToUser],[ToContact],[ToEmail], [Exported]  FROM [lpvw_GetEmailLog] Where FileId=" + fileId;
            if (cond != null && cond != string.Empty)
                SQLString += " AND " + cond;
            try
            {
                ds = DbHelperSQL.Query(SQLString);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    return lstEmailLog;
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    byte[] tempBody = (dr[4] == DBNull.Value) ? null : (byte[])dr[4];
                    lstEmailLog.Add(new Common.Table.EmailLog()
                    {
                        EmailLogId = (dr[0] == DBNull.Value) ? 0 : (int)dr[0],
                        Sender = (dr[1] == DBNull.Value) ? string.Empty : dr[1].ToString(),
                        FromEmail = (dr[2] == DBNull.Value) ? string.Empty : dr[2].ToString(),
                        Subject = (dr[3] == DBNull.Value) ? string.Empty : dr[3].ToString(),
                        EmailBody = (tempBody == null) ? string.Empty : Encoding.UTF8.GetString(tempBody),
                        FileId = (dr[5] == DBNull.Value) ? 0 : (int)dr[5],
                        LastSent = (dr[6] == DBNull.Value) ? DateTime.MinValue : DateTime.Parse(dr[6].ToString()),
                        ToUser = (dr[7] == DBNull.Value) ? string.Empty : dr[7].ToString(),
                        ToContact = (dr[8] == DBNull.Value) ? string.Empty : dr[8].ToString(),
                        ToEmail = (dr[9] == DBNull.Value) ? string.Empty : dr[9].ToString(),
                        Exported = (dr[10] == DBNull.Value) ? false : (bool)dr[10]
                    });
                }
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in Email_Log, Exception:" + ex.Message;
                logErr = true;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
            return lstEmailLog;

        }
        /// <summary>
        /// Gets the emal que list.
        /// </summary>
        /// <param name="err">The err.</param>
        /// <returns></returns>
        public List<EmailQue> GetEmalQueList(ref string err)
        {
            var lstEmailQue = new List<EmailQue>();
            string SQLString = "SELECT [EmailId],[ToUser],[ToContact],[ToBorrower],[EmailTmplId],[LoanAlertId],[FileId]  FROM [dbo].[EmailQue]";
            err = "";

            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                while (dataReader.Read())
                {
                    lstEmailQue.Add(new EmailQue()
                    {
                        EmailId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0),
                        ToUser = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1),
                        ToContact = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2),
                        ToBorrower = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3),
                        EmailTmplId = dataReader.IsDBNull(4) ? 0 : dataReader.GetInt32(4),
                        LoanAlertId = dataReader.IsDBNull(5) ? 0 : dataReader.GetInt32(5),
                        FileId = dataReader.IsDBNull(6) ? 0 : dataReader.GetInt32(6)
                    });

                }
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in Company_General." + ex.Message;
                Trace.TraceError(err);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return lstEmailQue;
        }
        public List<string> GetEmailList(int[] contractIds, int[] userIds, out string err)
        {
            var lstEmail = new List<string>();
            StringBuilder stringBuilder = new StringBuilder();

            string SQLString = string.Empty;
            string sContracts = string.Empty;
            string sUsers = string.Empty;

            stringBuilder.Append("select dbo.lpfn_GetContactName(contactid),Email from dbo.Contacts where ContactId in ({0}) ");
            stringBuilder.Append("union all ");
            stringBuilder.Append("select dbo.lpfn_GetUserName(userid),EmailAddress from dbo.Users where UserId in ({1})");

            if (contractIds != null && contractIds.Length > 0)
            {
                sContracts = contractIds.Select(query => query.ToString()).Aggregate((a, b) => a + "," + b);
            }
            else
            {
                sContracts = string.IsNullOrEmpty(sContracts) ? "0" : sContracts;
            }

            if (userIds != null && userIds.Length > 0)
            {
                sUsers = userIds.Select(query => query.ToString()).Aggregate((a, b) => a + "," + b);
            }
            else
            {
                sUsers = string.IsNullOrEmpty(sUsers) ? "0" : sUsers;
            }
            SQLString = string.Format(stringBuilder.ToString(), sContracts, sUsers);

            err = "";

            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                string emailAddress = string.Empty;
                string userName = string.Empty;
                while (dataReader.Read())
                {
                    if (!dataReader.IsDBNull(0))
                    {
                        userName = dataReader.GetString(0);
                        userName = userName.Replace(", ", " ");
                    }
                    if (!dataReader.IsDBNull(1))
                    {
                        emailAddress = dataReader.GetString(1);
                        if (!string.IsNullOrEmpty(emailAddress) && !lstEmail.Contains(emailAddress))
                        {
                            lstEmail.Add(string.Format("{0};{1}", userName, emailAddress));
                        }
                    }
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in Company_General." + ex.Message;
                Trace.TraceError(err);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return lstEmail;
        }

        public string GetUserEmailAddress(int userId, out string err)
        {
            err = "";
            string userEmailAddr = "";
            DataSet ds = null;
            try
            {
                string sqlCmd = "Select EmailAddress from Users where UserId=" + userId;
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                    return userEmailAddr;
                if (ds.Tables[0].Rows[0][0] == DBNull.Value)
                    return userEmailAddr;
                userEmailAddr = ds.Tables[0].Rows[0][0].ToString();
                return userEmailAddr;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                return userEmailAddr;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }
        public string GetUserEmailAddress(int userId)
        {
            string err = string.Empty;
            return this.GetUserEmailAddress(userId, out err);
        }
        public string GetContactEmailAddress(int contactId)
        {
            string returnVal;
            string err = "";
            string userEmailAddr = "";
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query("select Email from dbo.Contacts where ContactId =" + contactId);
                if (((ds == null) || (ds.Tables.Count <= 0)) || (ds.Tables[0].Rows.Count <= 0))
                {
                    return userEmailAddr;
                }
                if (ds.Tables[0].Rows[0][0] == DBNull.Value)
                {
                    return userEmailAddr;
                }
                userEmailAddr = ds.Tables[0].Rows[0][0].ToString();
                returnVal = userEmailAddr;
            }
            catch (Exception ex)
            {
                err = ex.Message;
                returnVal = userEmailAddr;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
            return returnVal;
        }


        public void SaveEmailLog(Table.PendingEmailLog emailLogInfo)
        {
            string err = "";

            try
            {
                SqlParameter[] parameters = new SqlParameter[] {                    
                    new SqlParameter("@ToUser", SqlDbType.NVarChar, 255 )
                   ,new SqlParameter("@ToContact", SqlDbType.NVarChar, 255 )
                   ,new SqlParameter("@EmailTmplId", SqlDbType.Int, 4 )
                   ,new SqlParameter("@Success", SqlDbType.Bit )
                   ,new SqlParameter("@Error", SqlDbType.NVarChar, 500 )
                   ,new SqlParameter("@LastSent", SqlDbType.DateTime )
                   ,new SqlParameter("@LoanAlertId", SqlDbType.Int, 4 )
                   ,new SqlParameter("@Retries", SqlDbType.SmallInt )                
                   ,new SqlParameter("@FileId", SqlDbType.Int, 4 )
                   ,new SqlParameter("@FromEmail", SqlDbType.NVarChar, 255 )
                   ,new SqlParameter("@FromUser", SqlDbType.Int, 4 )
                   ,new SqlParameter("@Created", SqlDbType.DateTime )
                   ,new SqlParameter("@AlertEmailType", SqlDbType.SmallInt )
                   ,new SqlParameter("@EmailBody", SqlDbType.VarBinary)
                   ,new SqlParameter("@Subject", SqlDbType.NVarChar, 500 )
                   ,new SqlParameter("@Exported", SqlDbType.Bit )
                   ,new SqlParameter("@ToEmail", SqlDbType.NVarChar, 255 )
                   ,new SqlParameter("@ProspectId", SqlDbType.Int, 4 )                 
                   ,new SqlParameter("@ProspectAlertId", SqlDbType.Int, 4 )
                   ,new SqlParameter("@CCUser", SqlDbType.NVarChar,255)
                   ,new SqlParameter("@CCContact", SqlDbType.NVarChar,255)
                   ,new SqlParameter("@ChainId", SqlDbType.NVarChar, 255)
                   ,new SqlParameter("@SequenceNumber", SqlDbType.Int, 4)
                   ,new SqlParameter("@EwsImported", SqlDbType.Bit)  
                   ,new SqlParameter("@CcEmails", SqlDbType.NVarChar,255)
                   ,new SqlParameter("@DateTimeReceived", SqlDbType.DateTime) 
                   ,new SqlParameter("@EmailUniqueId", SqlDbType.NVarChar, 255)
                };

                if (!string.IsNullOrEmpty(emailLogInfo.ToUser))
                {
                    parameters[0].Value = emailLogInfo.ToUser;
                }
                else
                {
                    parameters[0].Value = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(emailLogInfo.ToContact))
                {
                    parameters[1].Value = emailLogInfo.ToContact;
                }
                else
                {
                    parameters[1].Value = DBNull.Value;
                }

                if (emailLogInfo.EmailTmplId > 0)
                {
                    parameters[2].Value = emailLogInfo.EmailTmplId;
                }
                else
                {
                    parameters[2].Value = DBNull.Value;
                }

                parameters[3].Value = emailLogInfo.Success;

                if (!string.IsNullOrEmpty(emailLogInfo.Error))
                {
                    parameters[4].Value = emailLogInfo.Error;
                }
                else
                {
                    parameters[4].Value = DBNull.Value;
                }

                parameters[5].Value = emailLogInfo.LastSent;
                parameters[6].Value = emailLogInfo.LoanAlertId;
                parameters[7].Value = emailLogInfo.Retries;
                parameters[8].Value = emailLogInfo.FileId;

                if (!string.IsNullOrEmpty(emailLogInfo.FromEmail))
                {
                    parameters[9].Value = emailLogInfo.FromEmail;
                }
                else
                {
                    parameters[9].Value = DBNull.Value;
                }

                parameters[10].Value = emailLogInfo.FromUser;
                parameters[11].Value = DateTime.Now;
                parameters[12].Value = emailLogInfo.AlertEmailType;

                if ((emailLogInfo.EmailBody != null) && (emailLogInfo.EmailBody.Length > 0))
                {
                    parameters[13].Value = emailLogInfo.EmailBody;
                }
                else
                {
                    parameters[13].Value = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(emailLogInfo.Subject))
                {
                    parameters[14].Value = emailLogInfo.Subject;
                }
                else
                {
                    parameters[14].Value = DBNull.Value;
                }

                parameters[15].Value = emailLogInfo.Exported;

                if (!string.IsNullOrEmpty(emailLogInfo.ToEmail))
                {
                    parameters[16].Value = emailLogInfo.ToEmail;
                }
                else
                {
                    parameters[16].Value = DBNull.Value;
                }

                parameters[17].Value = emailLogInfo.ProspectId;
                parameters[18].Value = emailLogInfo.ProspectAlertId;

                if (!string.IsNullOrEmpty(emailLogInfo.CCUser))
                {
                    parameters[19].Value = emailLogInfo.CCUser;
                }
                else
                {
                    parameters[19].Value = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(emailLogInfo.CCContact))
                {
                    parameters[20].Value = emailLogInfo.CCContact;
                }
                else
                {
                    parameters[20].Value = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(emailLogInfo.ChainId))
                {
                    parameters[21].Value = emailLogInfo.ChainId;
                }
                else
                {
                    parameters[21].Value = DBNull.Value;
                }

                parameters[22].Value = emailLogInfo.SequenceNumber;
                parameters[23].Value = emailLogInfo.EwsImported;

                if (!string.IsNullOrEmpty(emailLogInfo.CCEmail))
                {
                    parameters[24].Value = emailLogInfo.CCEmail;
                }
                else
                {
                    parameters[24].Value = DBNull.Value;
                }

                parameters[25].Value = emailLogInfo.DateTimeReceived;

                if (!string.IsNullOrEmpty(emailLogInfo.EmailUniqueId))
                {
                    parameters[26].Value = emailLogInfo.EmailUniqueId;
                }
                else
                {
                    parameters[26].Value = DBNull.Value;
                }

                string sqlString = @"INSERT INTO [dbo].[EmailLog]
                                       ([ToUser]
                                       ,[ToContact]
                                       ,[EmailTmplId]
                                       ,[Success]
                                       ,[Error]
                                       ,[LastSent]
                                       ,[LoanAlertId]
                                       ,[Retries]
                                       ,[FileId]
                                       ,[FromEmail]
                                       ,[FromUser]
                                       ,[Created]
                                       ,[AlertEmailType]
                                       ,[EmailBody]
                                       ,[Subject]
                                       ,[Exported]
                                       ,[ToEmail]
                                       ,[ProspectId]
                                       ,[ProspectAlertId]
                                       ,[CCUser]
                                       ,[CCContact]
                                       ,[ChainId]
                                       ,[SequenceNumber]
                                       ,[EwsImported]
                                       ,[CCEmail] 
                                       ,[DateTimeReceived]
                                       ,[EmailUniqueId]      
                                        )
                                 VALUES
                                       (@ToUser
                                       ,@ToContact
                                       ,@EmailTmplId
                                       ,@Success
                                       ,@Error
                                       ,GetDate()
                                       ,@LoanAlertId
                                       ,@Retries
                                       ,@FileId
                                       ,@FromEmail
                                       ,@FromUser
                                       ,GetDate()
                                       ,@AlertEmailType
                                       ,@EmailBody
                                       ,@Subject
                                       ,@Exported
                                       ,@ToEmail
                                       ,@ProspectId
                                       ,@ProspectAlertId
                                       ,@CCUser
                                       ,@CCContact
                                       ,@ChainId
                                       ,@SequenceNumber
                                       ,@EwsImported
                                       ,@CcEmails
                                       ,@DateTimeReceived
                                       ,@EmailUniqueId     
                                        )";

                DbHelperSQL.ExecuteSql(sqlString, parameters);

            }
            catch (Exception e)
            {
                err = "Failed to log for email log, Exception:" + e.Message;
                Trace.TraceError(err);
                return;
            }

        }

        public string GetLoanPointFieldValue(string pointFieldId, FieldType fieldType, int FileId, string actValue, int userID, int prospectId, int propsectTaskId, int loanAlertId, int propsectAlertId, out string err)
        {
            string sqlCmd = string.Empty;
            string fieldValue = "";
            DataSet ds = null;
            err = string.Empty;
            switch (fieldType)
            {
                case FieldType.Unknown:
                    //
                    break;
                case FieldType.Previous:
                    sqlCmd = string.Format("SELECT TOP 1 PrevValue [retValue] FROM dbo.LoanPointFields WHERE PointFieldId={0} AND FileId={1}", pointFieldId, FileId);
                    break;
                case FieldType.DB:
                    //get act value via actValue
                    bool isProspect = false;
                    bool isProsepctID = false;
                    bool isLoanAlert = false;
                    if (string.IsNullOrEmpty(actValue))
                    {
                        err = "invalid db field";
                        return fieldValue;
                    }

                    string[] requiredLoanAlertFields = new string[]
                                                         {
                                                                "Task Owner First name",
                                                                "Task Owner Last name",
                                                                "Task Owner Full name",
                                                                "Task Owner Email",
                                                                "Task Owner Signature",
                                                                 "Task Description",
                                                                 "Task Due Date",
                                                                 "Task Alert Link",
                                                                 "Rule Alert Link"
                                                             
                                                        };

                    if (requiredLoanAlertFields.Contains(actValue))
                    {
                        if (loanAlertId == 0)
                            return fieldValue;

                        isLoanAlert = true;
                        actValue = actValue + "LA";
                    }
                    else
                    {
                        if (FileId == 0 && (prospectId != 0 || propsectTaskId != 0 || propsectAlertId != 0))
                        {
                            isProspect = true;

                            if (propsectAlertId != 0)
                            {
                                actValue = actValue + "PA";
                            }
                            else if (propsectTaskId != 0)
                            {
                                actValue = actValue + "T";
                            }
                            else if (prospectId != 0)
                            {
                                isProsepctID = true;
                                actValue = actValue + "P";
                            }
                        }
                    }
                    if (DbFieldMapping.ContainsKey(actValue))
                    {
                        sqlCmd = DbFieldMapping[actValue];
                        if (actValue == "Sender Picture" || actValue == "Sender Signature")
                        {
                            sqlCmd = string.Format(sqlCmd, userID);
                        }
                        else
                        {
                            if (isLoanAlert)
                            {
                                sqlCmd = string.Format(sqlCmd, loanAlertId);
                            }
                            else
                            {
                                sqlCmd = string.Format(sqlCmd, FileId);
                            }

                        }

                        if (isProspect)
                        {
                            if (isProsepctID)
                            {
                                sqlCmd = string.Format(sqlCmd, prospectId);
                            }
                            else
                            {
                                sqlCmd = string.Format(sqlCmd, propsectTaskId);
                            }
                        }
                    }
                    else
                    {
                        err = "invalid db field" + actValue + " specified.";
                        return fieldValue;
                    }
                    break;
                case FieldType.Default:
                    sqlCmd = string.Format("SELECT  TOP 1  CurrentValue [retValue] FROM dbo.LoanPointFields WHERE PointFieldId={0} AND FileId={1}", pointFieldId, FileId);
                    break;
                default:
                    err = "invalid fieldValue" + fieldType + " specified.";
                    return fieldValue;
            }
            if (FileId <= 0)
            {
                err = "invalid FileId" + FileId + " specified.";
                return fieldValue;
            }

            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "Unable to find field value for FileId " + FileId;
                    return fieldValue;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["retValue"] == DBNull.Value)
                    return fieldValue;
                //todo:check Image binary data
                if (!string.IsNullOrEmpty(actValue))
                {

                    if (actValue == "Branch Logo" || actValue == "Loan Officer Picture" || actValue == "Company Subpage Logo" || actValue == "Company Homepage Logo" || actValue == "Sender Picture" || actValue == "Loan Officer Picture" || actValue == "Loan Officer Assistant Picture" || actValue == "Processor Picture" || actValue == "Underwriter Picture" || actValue == "Closer Picture" || actValue == "Shipper Picture" || actValue == "Doc Prep Picture"
)
                    {
                        byte[] imageData = (byte[])dr["retValue"];
                        fieldValue = Convert.ToBase64String(imageData);
                        fieldValue = string.Format("<img src=\"cid:{0}\" data=\"{1}\" />", Guid.NewGuid().ToString(), fieldValue);
                    }
                    else
                    {
                        fieldValue = dr["retValue"].ToString().Trim();
                    }
                }
                return fieldValue;
            }
            catch (Exception ex)
            {
                err = "Exception:" + ex.Message;
                Trace.TraceError(err);
                return fieldValue;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        //mark gdc 20120804 
        public Common.Table.TemplateEmail GetEmailTemplateByTemplateId(int templateId, out string err)
        {
            string sqlCmd = string.Format("select [TemplEmailId],[Enabled],[Name],[Desc],[FromUserRoles],[FromEmailAddress],[Content],[Subject],[SenderName],[EmailSkinId] from dbo.Template_Email where enabled=1 and TemplEmailId={0}", templateId);
            Common.Table.TemplateEmail templateEmail = new Common.Table.TemplateEmail();
            DataSet ds = null;
            err = string.Empty;
            if (templateId <= 0)
            {
                err = "invalid templateId" + templateId + " specified.";
                return templateEmail;
            }

            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "Unable to find Content value for template ID " + templateId;
                    return templateEmail;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (!dr.IsNull("Name"))
                {
                    templateEmail.Name = dr["Name"].ToString();
                }
                if (!dr.IsNull("Desc"))
                {
                    templateEmail.Desc = dr["Desc"].ToString();
                }
                if (!dr.IsNull("FromUserRoles"))
                {
                    templateEmail.FromUserRoles = Convert.ToInt32(dr["FromUserRoles"]);
                }
                if (!dr.IsNull("FromEmailAddress"))
                {
                    templateEmail.FromEmailAddress = dr["FromEmailAddress"].ToString();
                }
                if (!dr.IsNull("Content"))
                {
                    templateEmail.Content = dr["Content"].ToString();
                    var EmailSkinId = dr.IsNull("EmailSkinId") ? 0 : Convert.ToInt32(dr["EmailSkinId"]);

                    templateEmail.Content = GetEmailTemplateContentWithEmailSkin(EmailSkinId, templateEmail.Content);
                }
                if (!dr.IsNull("Subject"))
                {
                    templateEmail.Subject = dr["Subject"].ToString();
                }
                if (!dr.IsNull("SenderName"))
                {
                    templateEmail.SenderName = dr["SenderName"].ToString();
                }
                //if (!dr.IsNull("FromUserName"))
                //{
                //    templateEmail.FromUserName = dr["FromUserName"].ToString();
                //}
                return templateEmail;
            }
            catch (Exception ex)
            {
                err = "Exception:" + ex.Message;
                Trace.TraceError(err);
                return templateEmail;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        //gdc CR32 20120804
        public string GetEmailTemplateContentWithEmailSkin(int emailSkinId, string emailTemplateContent)
        {
            string content = "";

            #region Skin Read To content
            if (emailSkinId <= 0)
            {
                DataSet dsDefaultSkin = null;
                string sqlCmd = string.Format("SELECT [EmailSkinId],[Name],[Desc],[HTMLBody],[Enabled],[Default] FROM [Template_EmailSkins] WHERE [Default] =1 AND [Enabled] = 1");
                dsDefaultSkin = DbHelperSQL.Query(sqlCmd);
                if ((dsDefaultSkin == null) || (dsDefaultSkin.Tables[0].Rows.Count <= 0) || dsDefaultSkin.Tables[0].Rows[0]["HTMLBody"] == DBNull.Value)
                {
                    content = "";
                }
                else
                {
                    content = dsDefaultSkin.Tables[0].Rows[0]["HTMLBody"].ToString();
                }
            }
            else
            {
                DataSet dsSkin = null;
                string sqlCmd = string.Format("SELECT [EmailSkinId],[Name],[Desc],[HTMLBody],[Enabled],[Default] FROM [Template_EmailSkins] WHERE EmailSkinId ={0} AND [Enabled] = 1", emailSkinId);
                dsSkin = DbHelperSQL.Query(sqlCmd);
                if ((dsSkin == null) || (dsSkin.Tables[0].Rows.Count <= 0) || dsSkin.Tables[0].Rows[0]["HTMLBody"] == DBNull.Value)
                {
                    content = "";
                }
                else
                {
                    content = dsSkin.Tables[0].Rows[0]["HTMLBody"].ToString();
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(content) && System.Text.RegularExpressions.Regex.IsMatch(content, "&lt;@EmailBody@&gt;"))  //<@EmailBody@> == &lt;@EmailBody@&gt;   (Encoded)
            {

                content = System.Text.RegularExpressions.Regex.Replace(content, "&lt;@EmailBody@&gt;", emailTemplateContent);

            }
            else
            {
                content = emailTemplateContent;
            }

            return content;
        }

        public Common.Table.CompanyWeb GetEmailServerSetting(out string err)
        {
            string sqlCmd = string.Format("select  top 1 * from Company_Web");
            Common.Table.CompanyWeb emailServerSetting = new Common.Table.CompanyWeb();
            DataSet ds = null;
            err = string.Empty;

            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "Unable to find email server setting.";
                    return emailServerSetting;
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["EmailAlertsEnabled"].ToString() != "")
                    {
                        if ((ds.Tables[0].Rows[0]["EmailAlertsEnabled"].ToString() == "1") ||
                            (ds.Tables[0].Rows[0]["EmailAlertsEnabled"].ToString().ToLower() == "true"))
                        {
                            emailServerSetting.EmailAlertsEnabled = true;
                        }
                        else
                        {
                            emailServerSetting.EmailAlertsEnabled = false;
                        }
                    }
                    emailServerSetting.EmailRelayServer = ds.Tables[0].Rows[0]["EmailRelayServer"].ToString();
                    emailServerSetting.DefaultAlertEmail = ds.Tables[0].Rows[0]["DefaultAlertEmail"].ToString();
                    if (ds.Tables[0].Rows[0]["EmailInterval"].ToString() != "")
                    {
                        emailServerSetting.EmailInterval = int.Parse(ds.Tables[0].Rows[0]["EmailInterval"].ToString());
                    }
                    emailServerSetting.LPCompanyURL = ds.Tables[0].Rows[0]["LPCompanyURL"].ToString();
                    emailServerSetting.BorrowerURL = ds.Tables[0].Rows[0]["BorrowerURL"].ToString();
                    emailServerSetting.BorrowerGreeting = ds.Tables[0].Rows[0]["BorrowerGreeting"].ToString();
                    emailServerSetting.HomePageLogo = ds.Tables[0].Rows[0]["HomePageLogo"].ToString();
                    emailServerSetting.LogoForSubPages = ds.Tables[0].Rows[0]["LogoForSubPages"].ToString();
                    if (ds.Tables[0].Rows[0]["HomePageLogoData"].ToString() != "")
                    {
                        emailServerSetting.HomePageLogoData = (byte[])ds.Tables[0].Rows[0]["HomePageLogoData"];
                    }
                    if (ds.Tables[0].Rows[0]["SubPageLogoData"].ToString() != "")
                    {
                        emailServerSetting.SubPageLogoData = (byte[])ds.Tables[0].Rows[0]["SubPageLogoData"];
                    }
                    emailServerSetting.BackgroundLoanAlertPage =
                        ds.Tables[0].Rows[0]["BackgroundLoanAlertPage"].ToString();
                    if (ds.Tables[0].Rows[0]["EnableEmailAuditTrail"] == DBNull.Value)
                    {
                        emailServerSetting.EnableEmailAuditTrail = false;
                    }
                    else
                    {
                        if (ds.Tables[0].Rows[0]["EnableEmailAuditTrail"].ToString() == "True")
                        {
                            emailServerSetting.EnableEmailAuditTrail = true;
                        }
                        else
                        {
                            emailServerSetting.EnableEmailAuditTrail = false;
                        }
                    }

                    emailServerSetting.BackgroundWCFURL = ds.Tables[0].Rows[0]["BackgroundWCFURL"].ToString();

                    if (ds.Tables[0].Rows[0]["SendEmailViaEWS"] == DBNull.Value)
                    {
                        emailServerSetting.SendEmailViaEWS = false;
                    }
                    else
                    {
                        if (ds.Tables[0].Rows[0]["SendEmailViaEWS"].ToString() == "True")
                        {
                            emailServerSetting.SendEmailViaEWS = true;
                        }
                        else
                        {
                            emailServerSetting.SendEmailViaEWS = false;
                        }
                    }

                    emailServerSetting.EwsUrl = ds.Tables[0].Rows[0]["EwsUrl"].ToString();


                    emailServerSetting.SMTP_Port = ds.Tables[0].Rows[0]["SMTP_Port"] != DBNull.Value
                                                       ? Convert.ToInt32(ds.Tables[0].Rows[0]["SMTP_Port"])
                                                       : 25;

                    emailServerSetting.AuthReq = ds.Tables[0].Rows[0]["AuthReq"] != DBNull.Value
                                                     ? Convert.ToBoolean(ds.Tables[0].Rows[0]["AuthReq"])
                                                     : false;

                    emailServerSetting.AuthEmailAccount = ds.Tables[0].Rows[0]["AuthEmailAccount"] != DBNull.Value
                                                              ? ds.Tables[0].Rows[0]["AuthEmailAccount"].ToString()
                                                              : "";

                    emailServerSetting.AuthPassword = ds.Tables[0].Rows[0]["AuthPassword"] != DBNull.Value
                                                          ? ds.Tables[0].Rows[0]["AuthPassword"].ToString()
                                                          : "";

                    emailServerSetting.SMTP_EncryptMethod = ds.Tables[0].Rows[0]["SMTP_EncryptMethod"] != DBNull.Value
                                                                ? ds.Tables[0].Rows[0]["SMTP_EncryptMethod"].ToString()
                                                                : "";

                    emailServerSetting.EWS_Version = ds.Tables[0].Rows[0]["EWS_Version"] != DBNull.Value
                                                         ? ds.Tables[0].Rows[0]["EWS_Version"].ToString()
                                                         : "";
                    emailServerSetting.EWS_Domain = ds.Tables[0].Rows[0]["EWS_Domain"] != DBNull.Value
                                                         ? ds.Tables[0].Rows[0]["EWS_Domain"].ToString()
                                                         : "";
                }
                return emailServerSetting;
            }
            catch (Exception ex)
            {
                err = "Exception:" + ex.Message;
                Trace.TraceError(err);
                return emailServerSetting;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        /// <summary>
        /// Gets the template email recipient.
        /// </summary>
        /// <param name="emailTemplateId">The email template id.</param>
        /// <param name="err">The err.</param>
        /// <returns></returns>
        public List<Common.Table.TemplateEmailRecipient> GetTemplateEmailRecipient(int emailTemplateId, out string err)
        {
            List<Common.Table.TemplateEmailRecipient> emailRecipients = new List<Common.Table.TemplateEmailRecipient>();
            string SQLString = "SELECT [TemplRecipientId],[TemplEmailId],[EmailAddr],[UserRoles],[ContactRoles],[RecipientType],[TaskOwner]  FROM [dbo].[Template_Email_Recipients] WHERE TemplEmailId={0}";

            err = "";
            if (emailTemplateId < 1)
            {
                err = "invalid emailTemplateId";
                return emailRecipients;
            }
            SQLString = string.Format(SQLString, emailTemplateId);

            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                Table.TemplateEmailRecipient emailRecipient;
                while (dataReader.Read())
                {
                    emailRecipient = new Common.Table.TemplateEmailRecipient();
                    emailRecipient.TemplRecipientId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    emailRecipient.TemplEmailId = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt32(1);
                    emailRecipient.EmailAddr = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
                    emailRecipient.UserRoles = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3);
                    emailRecipient.ContactRoles = dataReader.IsDBNull(4) ? string.Empty : dataReader.GetString(4);
                    emailRecipient.RecipientType = dataReader.IsDBNull(5) ? string.Empty : dataReader.GetString(5);
                    emailRecipient.TaskOwner = dataReader.IsDBNull(6) ? false : dataReader.GetBoolean(6);
                    emailRecipients.Add(emailRecipient);
                }
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in Company_General." + ex.Message;
                Trace.TraceError(err);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return emailRecipients;
        }
        /// <summary>
        /// Gets the email recipients.
        /// </summary>
        /// <param name="templEmailId">The templ email id.</param>
        /// <param name="fileId">The file id.</param>
        /// <param name="emailId">The email id.</param>
        /// <returns></returns>
        public List<Common.Table.TemplateEmailRecipient> GetEmailRecipients(int templEmailId, int fileId, int emailId)
        {
            List<Common.Table.TemplateEmailRecipient> emailRecipients = new List<Common.Table.TemplateEmailRecipient>();
            SqlParameter[] parameters = {
					new SqlParameter("@TemplEmailId", SqlDbType.Int),
					new SqlParameter("@FileId", SqlDbType.Int),
                    new SqlParameter("@EmailId", SqlDbType.Int)
					};
            parameters[0].Value = templEmailId;
            parameters[1].Value = fileId;
            parameters[2].Value = emailId;
            DataSet dataSet = DbHelperSQL.RunProcedure("lpsp_GetEmailRecipients", parameters, "ds");
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                Table.TemplateEmailRecipient emailRecipient;

                foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                {
                    emailRecipient = new Common.Table.TemplateEmailRecipient();
                    emailRecipient.TemplRecipientId = dataRow.IsNull(0) ? 0 : Convert.ToInt32(dataRow[0]);
                    emailRecipient.UserName = dataRow.IsNull(1) ? string.Empty : Convert.ToString(dataRow[1]);
                    //emailRecipient.EmailAddr = dataRow.IsNull(2) ? string.Empty : Convert.ToString(dataRow[2]);
                    if (!dataRow.IsNull(2))
                    {
                        emailRecipient.ToEmails = new string[1];
                        emailRecipient.ToEmails[0] = dataRow.IsNull(2) ? string.Empty : Convert.ToString(dataRow[2]);
                    }
                    emailRecipient.RecipientType = dataRow.IsNull(3) ? string.Empty : Convert.ToString(dataRow[3]);
                    emailRecipient.RoleType = dataRow.IsNull(5) ? string.Empty : Convert.ToString(dataRow[5]);
                    int toUserId = 0;
                    if (!dataRow.IsNull(4))
                    {
                        toUserId = Convert.ToInt32(dataRow[4]);
                    }
                    if (emailRecipient.RoleType.ToUpper().Contains("CONTRACT"))
                    {
                        emailRecipient.ToContactIds = new int[1];
                        emailRecipient.ToContactIds[0] = toUserId;
                    }
                    if (emailRecipient.RoleType.ToUpper().Contains("USER"))
                    {
                        emailRecipient.ToUserIds = new int[1];
                        emailRecipient.ToUserIds[0] = toUserId;
                    }

                    if (!dataRow.IsNull(6))
                        emailRecipient.TaskOwner = dataRow.IsNull(6) ? false : Convert.ToBoolean(dataRow[6]);
                    emailRecipients.Add(emailRecipient);
                }
            }
            return emailRecipients;
        }

        /// <summary>
        /// Gets the email recipients.
        /// </summary>
        /// <param name="templEmailId">The templ email id.</param>
        /// <param name="fileId">The file id.</param>
        /// <param name="emailId">The email id.</param>
        /// <returns></returns>
        /// 

        public bool Check_ContactId(int ContactId, ref Table.TemplateEmailRecipient emailRecipientReply, out string err)
        {
            string SQLString = "select TOP 1 Email from Contacts where ContactId={0}";
            DataSet ds = null;
            err = "";
            int cnt = 0;
            string str = "";
            int[] ToContactIds = null;
            string[] ToEmails = null;
            int[] CCContactIds = null;
            string[] CCEmails = null;

            SQLString = string.Format(SQLString, ContactId);

            try
            {
                ds = DbHelperSQL.Query(SQLString);
                if ((ds == null) || (ds.Tables[0].Rows.Count == 0))
                {
                    err = "No record found in Contacts.";
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                if (dr["Email"] == DBNull.Value)
                {
                    err = "Email equal to Null";
                    return false;
                }

                str = (string)dr["Email"];

                if (str == string.Empty)
                {
                    err = "Email equal to Empty";
                    return false;
                }

                if (emailRecipientReply.RecipientType.ToString().Trim().ToUpper() == "TO")
                {
                    if (emailRecipientReply.ToContactIds == null)
                    {
                        ToContactIds = new int[1];
                        ToContactIds[0] = ContactId;
                        emailRecipientReply.ToContactIds = ToContactIds;
                    }
                    else
                    {
                        cnt = emailRecipientReply.ToContactIds.Length;
                        ToContactIds = new int[cnt + 1];
                        ToContactIds = emailRecipientReply.ToContactIds;
                        ToContactIds[cnt] = ContactId;
                        emailRecipientReply.ToContactIds = ToContactIds;
                    }

                    if (emailRecipientReply.ToEmails == null)
                    {
                        ToEmails = new string[1];
                        ToEmails[0] = str;
                        emailRecipientReply.ToEmails = ToEmails;
                    }
                    else
                    {
                        cnt = emailRecipientReply.ToEmails.Length;
                        ToEmails = new string[cnt + 1];
                        ToEmails = emailRecipientReply.ToEmails;
                        ToEmails[cnt] = str;
                        emailRecipientReply.ToEmails = ToEmails;
                    }
                }

                if (emailRecipientReply.RecipientType.ToString().Trim().ToUpper() == "CC")
                {
                    if (emailRecipientReply.CCContactIds == null)
                    {
                        CCContactIds = new int[1];
                        CCContactIds[0] = ContactId;
                        emailRecipientReply.CCContactIds = CCContactIds;
                    }
                    else
                    {
                        cnt = emailRecipientReply.CCContactIds.Length;
                        CCContactIds = new int[cnt + 1];
                        CCContactIds = emailRecipientReply.CCContactIds;
                        CCContactIds[cnt] = ContactId;
                        emailRecipientReply.CCContactIds = CCContactIds;
                    }

                    if (emailRecipientReply.CCEmails == null)
                    {
                        CCEmails = new string[1];
                        CCEmails[0] = str;
                        emailRecipientReply.CCEmails = CCEmails;
                    }
                    else
                    {
                        cnt = emailRecipientReply.CCEmails.Length;
                        CCEmails = new string[cnt + 1];
                        CCEmails = emailRecipientReply.CCEmails;
                        CCEmails[cnt] = str;
                        emailRecipientReply.CCEmails = CCEmails;
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in Contacts." + ex.Message;
                Trace.TraceError(err);
                return false;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public bool Check_UserId(int UserId, ref Table.TemplateEmailRecipient emailRecipientReply, out string err)
        {
            string SQLString = "select TOP 1 EmailAddress from Users where UserId={0}";
            DataSet ds = null;
            err = "";
            string str = "";
            int cnt = 0;
            int[] ToUserIds = null;
            string[] ToEmails = null;
            int[] CCUserIds = null;
            string[] CCEmails = null;

            SQLString = string.Format(SQLString, UserId);

            try
            {
                ds = DbHelperSQL.Query(SQLString);
                if ((ds == null) || (ds.Tables[0].Rows.Count == 0))
                {
                    err = "No record found in Users.";
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                if (dr["EmailAddress"] == DBNull.Value)
                {
                    err = "EmailAddress equal to Null";
                    return false;
                }

                str = (string)dr["EmailAddress"];

                if (str == string.Empty)
                {
                    err = "EmailAddress equal to Empty";
                    return false;
                }

                if (emailRecipientReply.RecipientType.ToString().Trim().ToUpper() == "TO")
                {
                    if (emailRecipientReply.ToUserIds == null)
                    {
                        ToUserIds = new int[1];
                        ToUserIds[0] = UserId;
                        emailRecipientReply.ToUserIds = ToUserIds;
                    }
                    else
                    {
                        cnt = emailRecipientReply.ToUserIds.Length;
                        ToUserIds = new int[cnt + 1];
                        ToUserIds = emailRecipientReply.ToUserIds;
                        ToUserIds[cnt] = UserId;
                        emailRecipientReply.ToUserIds = ToUserIds;
                    }

                    if (emailRecipientReply.ToEmails == null)
                    {
                        ToEmails = new string[1];
                        ToEmails[0] = str;
                        emailRecipientReply.ToEmails = ToEmails;
                    }
                    else
                    {
                        cnt = emailRecipientReply.ToEmails.Length;
                        ToEmails = new string[cnt + 1];
                        ToEmails = emailRecipientReply.ToEmails;
                        ToEmails[cnt] = str;
                        emailRecipientReply.ToEmails = ToEmails;
                    }
                }

                if (emailRecipientReply.RecipientType.ToString().Trim().ToUpper() == "CC")
                {
                    if (emailRecipientReply.CCUserIds == null)
                    {
                        CCUserIds = new int[1];
                        CCUserIds[0] = UserId;
                        emailRecipientReply.CCUserIds = CCUserIds;
                    }
                    else
                    {
                        cnt = emailRecipientReply.CCUserIds.Length;
                        CCUserIds = new int[cnt + 1];
                        CCUserIds = emailRecipientReply.CCUserIds;
                        CCUserIds[cnt] = UserId;
                        emailRecipientReply.CCUserIds = CCUserIds;
                    }

                    if (emailRecipientReply.CCEmails == null)
                    {
                        CCEmails = new string[1];
                        CCEmails[0] = str;
                        emailRecipientReply.CCEmails = CCEmails;
                    }
                    else
                    {
                        cnt = emailRecipientReply.CCEmails.Length;
                        CCEmails = new string[cnt + 1];
                        CCEmails = emailRecipientReply.CCEmails;
                        CCEmails[cnt] = str;
                        emailRecipientReply.CCEmails = CCEmails;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in Users." + ex.Message;
                Trace.TraceError(err);
                return false;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public bool Check_UserRoles(int FileId, int RoleId, ref Table.TemplateEmailRecipient emailRecipientReply, out string err)
        {
            string SQLString = "select TOP 1 UserId from LoanTeam where FileId={0} and RoleId={1}";
            DataSet ds = null;
            err = "";
            bool status = true;
            int UserId = 0;

            SQLString = string.Format(SQLString, FileId, RoleId);

            try
            {
                ds = DbHelperSQL.Query(SQLString);
                if ((ds == null) || (ds.Tables[0].Rows.Count == 0))
                {
                    err = "No record found in LoanTeam";
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                if (dr["UserId"] == DBNull.Value)
                {
                    err = "UserId equal to Null";
                    return false;
                }

                UserId = (int)dr["UserId"];

                status = Check_UserId(UserId, ref emailRecipientReply, out err);

                return status;
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in Users." + ex.Message;
                Trace.TraceError(err);
                return false;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public bool Check_ContactRoles(int FileId, int ContactRoleId, ref Table.TemplateEmailRecipient emailRecipientReply, out string err)
        {
            string SQLString = "select TOP 1 ContactId from dbo.LoanContacts where FileId={0} and ContactRoleId={1}";
            DataSet ds = null;
            err = "";
            int ContactId = 0;
            bool status = true;

            SQLString = string.Format(SQLString, FileId, ContactRoleId);

            try
            {
                ds = DbHelperSQL.Query(SQLString);
                if ((ds == null) || (ds.Tables[0].Rows.Count == 0))
                {
                    err = "No record found in LoanContacts";
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                if (dr["ContactId"] == DBNull.Value)
                {
                    err = "ContactId equal to Null";
                    return false;
                }

                ContactId = (int)dr["ContactId"];

                status = Check_ContactId(ContactId, ref emailRecipientReply, out err);

                return status;
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in LoanContacts." + ex.Message;
                Trace.TraceError(err);
                return false;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public List<int> GetFileId_FromUserId(int[] UserId, ref string err)
        {
            List<int> Fid_List = new List<int>();
            List<int> UserId_List = new List<int>();
            int idx = 0;

            int FileId = 0;
            DataSet ds = null;
            DataRow dr = null;

            UserId_List = UserId.ToList();

            try
            {
                foreach (int Ud in UserId_List)
                {
                    if (Ud > 0)
                    {
                        string SQLString = "select FileId from dbo.LoanTeam where UserId={0} and (RoleId=3)";
                        SQLString = string.Format(SQLString, Ud);
                        ds = DbHelperSQL.Query(SQLString);
                        if ((ds == null) || (ds.Tables[0].Rows.Count == 0))
                        {
                        }
                        else
                        {
                            idx = ds.Tables[0].Rows.Count;
                            for (int i = 0; i < idx; i++)
                            {
                                dr = ds.Tables[0].Rows[i];
                                if (dr["FileId"] != DBNull.Value)
                                {
                                    FileId = (int)dr["FileId"];
                                    Fid_List.Add(FileId);
                                }
                            }
                        }
                    }
                }
                return Fid_List;
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in LoanTeam. " + ex.Message;
                Trace.TraceError(err);
                return Fid_List;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public List<int> GetFileId_FromContactId(int[] ContactId, ref string err)
        {
            List<int> Fid_List = new List<int>();
            List<int> ContactId_List = new List<int>();
            int idx = 0;

            int FileId = 0;
            DataSet ds = null;
            DataRow dr = null;

            ContactId_List = ContactId.ToList();

            try
            {

                foreach (int Cd in ContactId_List)
                {
                    if (Cd > 0)
                    {
                        string SQLString = "select FileId from dbo.LoanContacts where ContactId={0} and ((ContactRoleId=1) or (ContactRoleId=2))";
                        SQLString = string.Format(SQLString, Cd);
                        ds = DbHelperSQL.Query(SQLString);
                        if ((ds == null) || (ds.Tables[0].Rows.Count == 0))
                        {
                        }
                        else
                        {
                            idx = ds.Tables[0].Rows.Count;
                            for (int i = 0; i < idx; i++)
                            {
                                dr = ds.Tables[0].Rows[i];
                                if (dr["FileId"] != DBNull.Value)
                                {
                                    FileId = (int)dr["FileId"];
                                    Fid_List.Add(FileId);
                                }
                            }
                        }
                    }
                }
                return Fid_List;
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in LoanContacts." + ex.Message;
                Trace.TraceError(err);
                return Fid_List;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public List<Common.Table.TemplateEmailRecipient> GetEmailRecipients_cs(int EmailTmplId, int fileId, int emailId, ref bool no_tos, out string err)
        {
            no_tos = true;
            List<string> strlist1 = new List<string>();
            List<Common.Table.TemplateEmailRecipient> emailRecipients = new List<Common.Table.TemplateEmailRecipient>();
            string SQLString = "SELECT [TemplRecipientId],[TemplEmailId],[EmailAddr],[UserRoles],[ContactRoles],[RecipientType],[TaskOwner]  FROM [dbo].[Template_Email_Recipients] WHERE TemplEmailId={0}";

            err = "";
            bool Add_flag = false;

            if (EmailTmplId < 1)
            {
                err = "invalid emailTemplateId";
                return emailRecipients;
            }

            SQLString = string.Format(SQLString, EmailTmplId);

            SqlDataReader dataReader = null;
            try
            {
                int RoleId = 0;
                int ContactRoleId = 0;
                bool status = true;
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                Table.TemplateEmailRecipient emailRecipient;
                Table.TemplateEmailRecipient emailRecipientReply;
                while (dataReader.Read())
                {
                    Add_flag = false;
                    emailRecipient = new Common.Table.TemplateEmailRecipient();
                    emailRecipientReply = new Common.Table.TemplateEmailRecipient();
                    emailRecipient.TemplRecipientId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    emailRecipient.TemplEmailId = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt32(1);
                    emailRecipient.EmailAddr = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
                    emailRecipient.UserRoles = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3);
                    emailRecipient.ContactRoles = dataReader.IsDBNull(4) ? string.Empty : dataReader.GetString(4);
                    emailRecipient.RecipientType = dataReader.IsDBNull(5) ? string.Empty : dataReader.GetString(5);
                    emailRecipient.TaskOwner = dataReader.IsDBNull(6) ? false : dataReader.GetBoolean(6);

                    if (emailRecipient.RecipientType.ToString().Trim().ToUpper() == "TO")
                    {
                        Add_flag = false;
                        emailRecipientReply = emailRecipient;
                        if (emailRecipient.EmailAddr != string.Empty)
                        {
                            strlist1.Add(emailRecipient.EmailAddr);
                            emailRecipientReply.ToEmails = strlist1.ToArray();
                            Add_flag = true;
                        }

                        status = int.TryParse(emailRecipient.UserRoles, out RoleId);

                        if (status == true)
                        {
                            if ((Check_UserRoles(fileId, RoleId, ref emailRecipientReply, out err) == true))
                            {
                                Add_flag = true;
                            }
                        }

                        status = int.TryParse(emailRecipient.ContactRoles, out ContactRoleId);

                        if (status == true)
                        {
                            if ((Check_ContactRoles(fileId, ContactRoleId, ref emailRecipientReply, out err) == true))
                            {
                                Add_flag = true;
                            }
                        }

                        if (Add_flag == true)
                        {
                            no_tos = false;
                            emailRecipients.Add(emailRecipientReply);
                        }
                        else
                        {
                            no_tos = true;
                        }
                    }

                    if (emailRecipient.RecipientType.ToString().Trim().ToUpper() == "CC")
                    {
                        Add_flag = false;
                        emailRecipientReply = emailRecipient;
                        if (emailRecipient.EmailAddr != string.Empty)
                            Add_flag = true;

                        status = int.TryParse(emailRecipient.UserRoles, out RoleId);

                        if (status == true)
                        {
                            if ((Check_UserRoles(fileId, RoleId, ref emailRecipientReply, out err) == true))
                            {
                                Add_flag = true;
                            }
                        }

                        status = int.TryParse(emailRecipient.ContactRoles, out ContactRoleId);

                        if (status == true)
                        {
                            if ((Check_ContactRoles(fileId, ContactRoleId, ref emailRecipientReply, out err) == true))
                            {
                                Add_flag = true;
                            }
                        }

                        if (Add_flag == true)
                        {
                            emailRecipients.Add(emailRecipientReply);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in Company_General." + ex.Message;
                Trace.TraceError(err);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }

            return emailRecipients;
        }

        public bool Check_TaskOwner(int FileId, int LoanAlertId, ref Table.TemplateEmailRecipient emailRecipientReply, out string err)
        {
            string SQLString = "select TOP 1 OwnerId from LoanAlerts where FileId={0} and LoanAlertId={1}";
            DataSet ds = null;
            err = "";
            bool status = true;
            int UserId = 0;

            SQLString = string.Format(SQLString, FileId, LoanAlertId);

            try
            {
                ds = DbHelperSQL.Query(SQLString);
                if ((ds == null) || (ds.Tables[0].Rows.Count == 0))
                {
                    err = "No record found in LoanAlerts";
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                if (dr["OwnerId"] == DBNull.Value)
                {
                    err = "UserId equal to Null";
                    return false;
                }

                UserId = (int)dr["OwnerId"];

                status = Check_UserId(UserId, ref emailRecipientReply, out err);

                return status;
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in LoanAlerts." + ex.Message;
                Trace.TraceError(err);
                return false;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public List<Common.Table.TemplateEmailRecipient> GetEmailQueRecipients_cs(int EmailTmplId, int LoanAlertId, int fileId, int emailId, ref bool no_tos, out string err)
        {
            no_tos = true;
            List<string> strlist1 = new List<string>();
            List<Common.Table.TemplateEmailRecipient> emailRecipients = new List<Common.Table.TemplateEmailRecipient>();
            string SQLString = "SELECT [TemplRecipientId],[TemplEmailId],[EmailAddr],[UserRoles],[ContactRoles],[RecipientType],[TaskOwner]  FROM [dbo].[Template_Email_Recipients] WHERE TemplEmailId={0}";

            err = "";
            bool Add_flag = false;

            if (EmailTmplId < 1)
            {
                err = "invalid emailTemplateId";
                return emailRecipients;
            }

            SQLString = string.Format(SQLString, EmailTmplId);

            SqlDataReader dataReader = null;
            try
            {
                int RoleId = 0;
                int ContactRoleId = 0;
                bool status = true;
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                Table.TemplateEmailRecipient emailRecipient;
                Table.TemplateEmailRecipient emailRecipientReply;
                while (dataReader.Read())
                {
                    Add_flag = false;
                    emailRecipient = new Common.Table.TemplateEmailRecipient();
                    emailRecipientReply = new Common.Table.TemplateEmailRecipient();
                    emailRecipient.TemplRecipientId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    emailRecipient.TemplEmailId = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt32(1);
                    emailRecipient.EmailAddr = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
                    emailRecipient.UserRoles = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3);
                    emailRecipient.ContactRoles = dataReader.IsDBNull(4) ? string.Empty : dataReader.GetString(4);
                    emailRecipient.RecipientType = dataReader.IsDBNull(5) ? string.Empty : dataReader.GetString(5);
                    emailRecipient.TaskOwner = dataReader.IsDBNull(6) ? false : dataReader.GetBoolean(6);

                    if (emailRecipient.RecipientType.ToString().Trim().ToUpper() == "TO")
                    {
                        Add_flag = false;
                        emailRecipientReply = emailRecipient;
                        if (!string.IsNullOrEmpty(emailRecipient.EmailAddr))
                        {
                            strlist1.Add(emailRecipient.EmailAddr);
                            emailRecipientReply.ToEmails = strlist1.ToArray();
                            Add_flag = true;
                        }

                        status = int.TryParse(emailRecipient.UserRoles, out RoleId);

                        if (status == true)
                        {
                            if ((Check_UserRoles(fileId, RoleId, ref emailRecipientReply, out err) == true))
                            {
                                Add_flag = true;
                            }
                        }

                        status = int.TryParse(emailRecipient.ContactRoles, out ContactRoleId);

                        if (status == true)
                        {
                            if ((Check_ContactRoles(fileId, ContactRoleId, ref emailRecipientReply, out err) == true))
                            {
                                Add_flag = true;
                            }
                        }

                        if (emailRecipient.TaskOwner == true)
                        {
                            if ((Check_TaskOwner(fileId, LoanAlertId, ref emailRecipientReply, out err) == true))
                            {
                                Add_flag = true;
                            }

                        }

                        if (Add_flag == true)
                        {
                            no_tos = false;
                            emailRecipients.Add(emailRecipientReply);
                        }
                        else
                        {
                            no_tos = true;
                        }
                    }

                    if (emailRecipient.RecipientType.ToString().Trim().ToUpper() == "CC")
                    {
                        Add_flag = false;
                        emailRecipientReply = emailRecipient;
                        if (!string.IsNullOrEmpty(emailRecipient.EmailAddr))
                            Add_flag = true;

                        status = int.TryParse(emailRecipient.UserRoles, out RoleId);

                        if (status == true)
                        {
                            if ((Check_UserRoles(fileId, RoleId, ref emailRecipientReply, out err) == true))
                            {
                                Add_flag = true;
                            }
                        }

                        status = int.TryParse(emailRecipient.ContactRoles, out ContactRoleId);

                        if (status == true)
                        {
                            if ((Check_ContactRoles(fileId, ContactRoleId, ref emailRecipientReply, out err) == true))
                            {
                                Add_flag = true;
                            }
                        }

                        if (Add_flag == true)
                        {
                            emailRecipients.Add(emailRecipientReply);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in Company_General." + ex.Message;
                Trace.TraceError(err);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }

            return emailRecipients;
        }

        /// <summary>
        /// Handles the email que log.
        /// </summary>
        /// <param name="emailId">The email id.</param>
        /// <param name="sentStatus">if set to <c>true</c> [sent status].</param>
        /// <param name="error">The error.</param>
        /// <param name="lastSent">The last sent.</param>
        /// <param name="retries">The retries.</param>
        /// <param name="fromEmail">From email.</param>
        /// <param name="fromUser">From user.</param>
        /// <param name="removeQueOnFailed">if set to <c>true</c> [remove que on failed].</param>
        /// <param name="emailBody">The email body.</param>
        /// <param name="toEmails">To emails.</param>
        /// <param name="toUserIds">To user ids.</param>
        /// <param name="toContacts">To contacts.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="prospectId">The prospect id.</param>
        /// <param name="prospectAlertId">The prospect alert id.</param>
        /// <param name="chainId">The chain id.</param>
        /// <param name="sequenceNumber">The sequence number.</param>
        /// <param name="ewsImported">if set to <c>true</c> [ews imported].</param>
        /// <param name="err">The err.</param>
        public bool HandleEmailQueLog(int emailId, bool sentStatus, string error, DateTime lastSent, int retries, string fromEmail, int fromUser, bool removeQueOnFailed, string emailBody, string[] toEmails, int[] toUserIds, int[] toContacts, string subject, int prospectId, int prospectAlertId, string chainId, int sequenceNumber, bool ewsImported, string[] CCEmails, int[] CCUserIds, int[] CCContactIds, out string err, ref int emailLogId, ref int fileId)
        {
            //@EmailId int,@SentStatus bit,@Error nvarchar(500),@LastSent datetime,@Retries smallint,@FromEmail nvarchar(255),@FromUser int
            err = "";

            if (emailId <= 0)
            {
                err = string.Format("HandleEmailQueLog, emailId {0} is invalid.", emailId);
                return false;
            }

            try
            {
                SqlParameter[] parameters = {                    
                new SqlParameter("@EmailId", SqlDbType.Int, 4 ),
                new SqlParameter("@SentStatus", SqlDbType.Bit),
                new SqlParameter("@Error", SqlDbType.NVarChar, 500), 
                new SqlParameter("@LastSent", SqlDbType.DateTime),
                new SqlParameter("@Retries", SqlDbType.Int, 4),
                new SqlParameter("@FromEmail", SqlDbType.NVarChar, 255),
                new SqlParameter("@FromUser", SqlDbType.Int, 4),
                new SqlParameter("@RemoveQueOnFailed", SqlDbType.Bit),
                new SqlParameter("@EmailBody", SqlDbType.VarBinary),
                new SqlParameter("@ToEmails", SqlDbType.NVarChar, 255),
                new SqlParameter("@ToUserIds", SqlDbType.NVarChar, 255),
                new SqlParameter("@ToContactIds", SqlDbType.NVarChar, 255),
                new SqlParameter("@Subject", SqlDbType.NVarChar, 255),
                new SqlParameter("@ProspectId", SqlDbType.Int, 4),
                new SqlParameter("@ProspectAlertId", SqlDbType.Int, 4),
                new SqlParameter("@ChainId", SqlDbType.NVarChar, 255),
                new SqlParameter("@SequenceNumber", SqlDbType.Int, 4),
                new SqlParameter("@EwsImported", SqlDbType.Bit),
                new SqlParameter("@CCEmails", SqlDbType.NVarChar, 255),
                new SqlParameter("@CCUserIds", SqlDbType.NVarChar, 255),
                new SqlParameter("@CCContactIds", SqlDbType.NVarChar, 255),
                new SqlParameter("@oEmailLogId", SqlDbType.Int, 4),
                new SqlParameter("@oFileId", SqlDbType.Int, 4)
                };
                parameters[0].Value = emailId;
                parameters[1].Value = sentStatus;
                if (string.IsNullOrEmpty(error))
                {
                    parameters[2].Value = DBNull.Value;
                }
                else
                {
                    parameters[2].Value = error;
                }
                parameters[3].Value = lastSent;
                parameters[4].Value = retries;
                parameters[5].Value = fromEmail;
                parameters[6].Value = fromUser;
                parameters[7].Value = removeQueOnFailed;
                parameters[8].Value = Encoding.UTF8.GetBytes(emailBody);

                string tempEmails = "";
                if ((toEmails != null) && (toEmails.Length > 0))
                {
                    foreach (string str in toEmails)
                    {
                        if ((str != String.Empty) &&
                            (str != null))
                        {
                            tempEmails += str + ";";
                        }
                    }
                    if (tempEmails.Length > 0)
                        tempEmails = tempEmails.Substring(0, tempEmails.Length - 1);
                }
                parameters[9].Value = tempEmails;

                string tempUserIds = "";
                if ((toUserIds != null) && (toUserIds.Length > 0))
                {
                    foreach (int userId in toUserIds)
                        if (userId > 0)
                            tempUserIds += userId.ToString() + ";";
                }
                parameters[10].Value = tempUserIds;

                string tempContactIds = "";
                if ((toContacts != null) && (toContacts.Length > 0))
                {
                    foreach (int contactid in toContacts)
                    {
                        if (contactid > 0)
                            tempContactIds += contactid.ToString() + ";";
                    }
                }
                parameters[11].Value = tempContactIds;

                if (string.IsNullOrEmpty(subject))
                    parameters[12].Value = DBNull.Value;
                else
                    parameters[12].Value = subject;

                parameters[13].Value = prospectId;
                parameters[14].Value = prospectAlertId;


                if (!string.IsNullOrEmpty(chainId))
                {
                    parameters[15].Value = chainId;
                }
                else
                {
                    parameters[15].Value = DBNull.Value;
                }

                if (sequenceNumber != 0)
                {
                    parameters[16].Value = sequenceNumber;
                }
                else
                {
                    parameters[16].Value = DBNull.Value;
                }

                parameters[17].Value = ewsImported;


                string ccEmail = string.Empty;
                if (CCEmails != null && CCEmails.Length > 0)
                {
                    ccEmail = CCEmails.Select(a => a.ToString()).Aggregate((a, b) => a + ";" + b);
                }
                if (!string.IsNullOrEmpty(ccEmail))
                {
                    parameters[18].Value = ccEmail;
                }
                else
                {
                    parameters[18].Value = DBNull.Value;
                }

                string ccUser = string.Empty;
                if (CCUserIds != null && CCUserIds.Length > 0)
                {
                    ccUser = CCUserIds.Select(a => a.ToString()).Aggregate((a, b) => a + ";" + b);
                }

                if (!string.IsNullOrEmpty(ccUser))
                {
                    parameters[19].Value = ccUser;
                }
                else
                {
                    parameters[19].Value = DBNull.Value;
                }

                string ccContact = string.Empty;
                if (CCContactIds != null && CCContactIds.Length > 0)
                {
                    ccContact = CCContactIds.Select(a => a.ToString()).Aggregate((a, b) => a + ";" + b);
                }

                if (!string.IsNullOrEmpty(ccContact))
                {
                    parameters[20].Value = ccContact;
                }
                else
                {
                    parameters[20].Value = DBNull.Value;
                }

                parameters[21].Direction = ParameterDirection.Output;
                parameters[22].Direction = ParameterDirection.Output;

                int rows = 0;
                int returnValue = DbHelperSQL.RunProcedure("lpsp_EmailQueLog", parameters, out rows);
                var objEmailLogID = parameters[21].Value;
                if (objEmailLogID != null)
                    emailLogId = (int)objEmailLogID;

                var objField = parameters[22].Value;
                if (objField != null)
                    fileId = (int)objField;

                if (returnValue < 0)
                {
                    err = "Failed to log for email Que:" + emailId;
                    return false;
                }
            }
            catch (Exception e)
            {
                err = "Failed to log for email Que:" + emailId + ", Exception:" + e.ToString();
                return false;
            }
            return true;
        }
        /// <summary>
        /// Emails the log.
        /// </summary>
        /// <param name="ToUser">To user.</param>
        /// <param name="ToContact">To contact.</param>
        /// <param name="EmailTmplId">The email TMPL id.</param>
        /// <param name="Success">if set to <c>true</c> [success].</param>
        /// <param name="Error">The error.</param>
        /// <param name="LoanAlertId">The loan alert id.</param>
        /// <param name="Retries">The retries.</param>
        /// <param name="FileId">The file id.</param>
        /// <param name="FromEmail">From email.</param>
        /// <param name="FromUser">From user.</param>
        /// <param name="AlertEmailType">Type of the alert email.</param>
        /// <param name="EmailBody">The email body.</param>
        /// <param name="Subject">The subject.</param>
        /// <param name="Exported">if set to <c>true</c> [exported].</param>
        /// <param name="ToEmail">To email.</param>
        /// <param name="prospectId">The prospect id.</param>
        /// <param name="prospectAlertId">The prospect alert id.</param>
        /// <param name="chainId">The chain id.</param>
        /// <param name="sequenceNumber">The sequence number.</param>
        /// <param name="ewsImported">if set to <c>true</c> [ews imported].</param>
        /// <param name="ccEmails">The cc emails.</param>
        /// <param name="ccUser">The cc user.</param>
        /// <param name="ccContact">The cc contact.</param>
        /// <param name="err">The err.</param>
        public bool EmailLog(string ToUser, string ToContact, int EmailTmplId, bool Success, string Error, int LoanAlertId, int Retries, int FileId, string FromEmail, int FromUser, int AlertEmailType, string EmailBody, string Subject, bool Exported, string ToEmail, int prospectId, int prospectAlertId, string chainId, int sequenceNumber, bool ewsImported, string ccEmails, string ccUser, string ccContact, string EmailUniqueId, ref string err, ref int emailLogId)
        {
            err = "";

            try
            {
                SqlParameter[] parameters = {                    
		            new SqlParameter("@ToUser", SqlDbType.NVarChar, 255 )
                   ,new SqlParameter("@ToContact", SqlDbType.NVarChar, 255 )
                   ,new SqlParameter("@EmailTmplId", SqlDbType.Int, 4 )
                   ,new SqlParameter("@Success", SqlDbType.Bit )
                   ,new SqlParameter("@Error", SqlDbType.NVarChar, 500 )
                   //,new SqlParameter("@LastSent", SqlDbType.DateTime )
                   ,new SqlParameter("@LoanAlertId", SqlDbType.Int, 4 )
                   ,new SqlParameter("@Retries", SqlDbType.SmallInt )
                   ,new SqlParameter("@FileId", SqlDbType.Int, 4 )
                   ,new SqlParameter("@FromEmail", SqlDbType.NVarChar, 255 )
                   ,new SqlParameter("@FromUser", SqlDbType.Int, 4 )
                   //,new SqlParameter("@Created", SqlDbType.DateTime )
                   ,new SqlParameter("@AlertEmailType", SqlDbType.SmallInt )
                   ,new SqlParameter("@EmailBody", SqlDbType.VarBinary)
                   ,new SqlParameter("@Subject", SqlDbType.NVarChar, 500 )
                   ,new SqlParameter("@Exported", SqlDbType.Bit )
                   ,new SqlParameter("@ToEmail", SqlDbType.NVarChar, 255 )
                   ,new SqlParameter("@ProspectId", SqlDbType.Int, 4 )
                   ,new SqlParameter("@ProspectAlertId", SqlDbType.Int, 4 )
                   ,new SqlParameter("@ChainId", SqlDbType.NVarChar, 255)
                   ,new SqlParameter("@SequenceNumber", SqlDbType.Int, 4)
                   ,new SqlParameter("@EwsImported", SqlDbType.Bit)
                   ,new SqlParameter("@CcEmails", SqlDbType.NVarChar,255)
                   ,new SqlParameter("@CCUser", SqlDbType.NVarChar,255)
                   ,new SqlParameter("@CCContact", SqlDbType.NVarChar,255)
                   ,new SqlParameter("@EmailUniqueId", SqlDbType.NVarChar,255)
                };
                if (!string.IsNullOrEmpty(ToUser))
                {
                    parameters[0].Value = ToUser;
                }
                else
                {
                    parameters[0].Value = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(ToContact))
                {
                    parameters[1].Value = ToContact;
                }
                else
                {
                    parameters[1].Value = DBNull.Value;
                }

                parameters[2].Value = EmailTmplId;
                parameters[3].Value = Success;
                if (!string.IsNullOrEmpty(Error))
                {
                    parameters[4].Value = Error;
                }
                else
                {
                    parameters[4].Value = DBNull.Value;
                }
                //parameters[5].Value = LastSent;
                parameters[5].Value = LoanAlertId;
                parameters[6].Value = DBNull.Value;
                parameters[7].Value = FileId;
                if (!string.IsNullOrEmpty(FromEmail))
                {
                    parameters[8].Value = FromEmail;
                }
                else
                {
                    parameters[8].Value = DBNull.Value;
                }
                parameters[9].Value = FromUser;
                //parameters[11].Value = Created;
                parameters[10].Value = AlertEmailType;
                if (!string.IsNullOrEmpty(EmailBody))
                {
                    parameters[11].Value = Encoding.UTF8.GetBytes(EmailBody);
                }
                else
                {
                    parameters[11].Value = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(Subject))
                {
                    parameters[12].Value = Subject;
                }
                else
                {
                    parameters[12].Value = DBNull.Value;
                }
                parameters[13].Value = DBNull.Value;
                if (!string.IsNullOrEmpty(ToEmail))
                {
                    parameters[14].Value = ToEmail;
                }
                else
                {
                    parameters[14].Value = DBNull.Value;
                }
                parameters[15].Value = prospectId;
                parameters[16].Value = prospectAlertId;
                if (!string.IsNullOrEmpty(chainId))
                {
                    parameters[17].Value = chainId;
                }
                else
                {
                    parameters[17].Value = DBNull.Value;
                }
                if (sequenceNumber != 0)
                {
                    parameters[18].Value = sequenceNumber;
                }
                else
                {
                    parameters[18].Value = DBNull.Value;
                }
                parameters[19].Value = ewsImported;
                if (!string.IsNullOrEmpty(ccEmails))
                {
                    parameters[20].Value = ccEmails;
                }
                else
                {
                    parameters[20].Value = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(ccUser))
                {
                    parameters[21].Value = ccUser;
                }
                else
                {
                    parameters[21].Value = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(ccContact))
                {
                    parameters[22].Value = ccContact;
                }
                else
                {
                    parameters[22].Value = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(EmailUniqueId))
                {
                    parameters[23].Value = EmailUniqueId;
                }
                else
                {
                    parameters[23].Value = DBNull.Value;
                }

                string sqlString = @"INSERT INTO [dbo].[EmailLog]
                                       ([ToUser]
                                       ,[ToContact]
                                       ,[EmailTmplId]
                                       ,[Success]
                                       ,[Error]
                                       ,[LastSent]
                                       ,[LoanAlertId]
                                       ,[Retries]
                                       ,[FileId]
                                       ,[FromEmail]
                                       ,[FromUser]
                                       ,[Created]
                                       ,[AlertEmailType]
                                       ,[EmailBody]
                                       ,[Subject]
                                       ,[Exported]
                                       ,[ToEmail]
                                       ,[ProspectId]
                                       ,[ProspectAlertId]
                                       ,[ChainId]
                                       ,[SequenceNumber]
                                       ,[EwsImported]
                                       ,[CCEmail]
                                       ,[CCUser]
                                       ,[CCContact]
                                       ,[EmailUniqueId]
                                        )
                                 VALUES
                                       (@ToUser
                                       ,@ToContact
                                       ,@EmailTmplId
                                       ,@Success
                                       ,@Error
                                       ,GetDate()
                                       ,@LoanAlertId
                                       ,@Retries
                                       ,@FileId
                                       ,@FromEmail
                                       ,@FromUser
                                       ,GetDate()
                                       ,@AlertEmailType
                                       ,@EmailBody
                                       ,@Subject
                                       ,@Exported
                                       ,@ToEmail
                                       ,@ProspectId
                                       ,@ProspectAlertId
                                       ,@ChainId
                                       ,@SequenceNumber
                                       ,@EwsImported
                                       ,@CcEmails
                                       ,@CCUser
                                       ,@CCContact
                                       ,@EmailUniqueId
                                        ) 
                                    select SCOPE_IDENTITY() ";
                SqlCommand sqlCmd = new SqlCommand(sqlString);
                sqlCmd.Parameters.AddRange(parameters);
                emailLogId = Convert.ToInt32(DbHelperSQL.ExecuteScalar(sqlCmd));
                return true;
            }
            catch (Exception e)
            {
                err = "Failed to log for email log, Exception:" + e.ToString();
                Trace.TraceError(err);
                return false;
            }
        }

        #endregion

        #region Loan Rules
        public DataSet ProcessLoanRules()
        {
            SqlParameter[] parameters = {
					new SqlParameter("@tid", SqlDbType.Int)
					};
            parameters[0].Value = 0;
            DataSet dataSet = DbHelperSQL.RunProcedure("[lpsp_RuleManager]", parameters, "ds");
            return dataSet;
        }

        public List<Table.PendingRuleInfo> GetPendgingRuleInfo()
        {
            var pendgingRuleInfo = new List<Table.PendingRuleInfo>();
            //string SQLString = "SELECT * FROM lpvw_PendingRuleInfos ORDER BY RuleID asc ,RuleCondID asc";
            string SQLString = "lpsp_GetPendgingRuleInfo";
            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                Table.PendingRuleInfo RuleInfo = null;
                while (dataReader.Read())
                {
                    RuleInfo = new Table.PendingRuleInfo();
                    RuleInfo.RuleCondId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    RuleInfo.RuleId = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt32(1);
                    RuleInfo.RuleGroupId = dataReader.IsDBNull(2) ? 0 : dataReader.GetInt32(2);
                    RuleInfo.FileId = dataReader.IsDBNull(3) ? 0 : dataReader.GetInt32(3);
                    RuleInfo.RuleCondValue = dataReader.IsDBNull(4) ? 0 : dataReader.GetInt32(4);
                    RuleInfo.RuleType = dataReader.IsDBNull(5) ? string.Empty : dataReader.GetString(5);
                    pendgingRuleInfo.Add(RuleInfo);
                }
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return pendgingRuleInfo;
        }

        public List<Table.TemplateRules> GetPendgingTemplateRuleInfo()
        {
            var pendgingTemplateRuleInfo = new List<Table.TemplateRules>();
            string SQLString = "select * from dbo.Template_Rules";
            string err = string.Empty;
            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                Table.TemplateRules templateRules = null;
                while (dataReader.Read())
                {
                    templateRules = new Table.TemplateRules();
                    templateRules.RuleId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    templateRules.Name = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1);
                    templateRules.Desc = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
                    templateRules.Enabled = dataReader.IsDBNull(3) ? false : dataReader.GetBoolean(3);
                    templateRules.AlertEmailTemplId = dataReader.IsDBNull(4) ? 0 : dataReader.GetInt32(4);
                    templateRules.AckReq = dataReader.IsDBNull(5) ? false : dataReader.GetBoolean(5);
                    templateRules.RecomEmailTemplid = dataReader.IsDBNull(6) ? 0 : dataReader.GetInt32(6);
                    templateRules.AdvFormula = dataReader.IsDBNull(7) ? string.Empty : dataReader.GetString(7);
                    templateRules.RuleScope = dataReader.IsDBNull(8) ? 0 : dataReader.GetInt16(8);
                    templateRules.LoanTarget = dataReader.IsDBNull(9) ? 0 : dataReader.GetInt16(9);
                    templateRules.AutoCampaignId = dataReader.IsDBNull(10) ? 0 : dataReader.GetInt32(10);

                    pendgingTemplateRuleInfo.Add(templateRules);
                }
            }
            catch (Exception ex)
            {
                err = "RuleManager:GetPendgingTemplateRuleInfo, Exception:" + ex.Message;
                int Event_id = 3099;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return pendgingTemplateRuleInfo;
        }


        /// <summary>
        /// Gets the adv formula value.
        /// </summary>
        /// <param name="advFormula">The adv formula.</param>
        /// <returns></returns>
        public bool GetAdvFormulaValue(string advFormula)
        {
            string SQLString = string.Format("SELECT COUNT(*) WHERE {0}", advFormula);

            object obj = DbHelperSQL.GetSingle(SQLString);
            if (obj == null)
            {
                return false;
            }
            int rValue = Convert.ToInt32(obj);
            return rValue > 0 ? true : false;

            return false;
        }

        public void UpdateEmailBody(int emailId, int loanAlertId, int alertEmailType, string alertEmailBody, byte[] emailBody)
        {
            //@EmailId int,@LoanAlertId int,@AlertEmailType int ,@AlertEmailBody nvarchar(max),@EmailBody varbinary(max)
            SqlParameter[] parameters = {
					new SqlParameter("@EmailId", SqlDbType.Int),
					new SqlParameter("@LoanAlertId", SqlDbType.Int),
					new SqlParameter("@AlertEmailType", SqlDbType.Int),
					new SqlParameter("@AlertEmailBody", SqlDbType.NVarChar),
					new SqlParameter("@EmailBody", SqlDbType.VarBinary)
					};
            parameters[0].Value = emailId;
            parameters[1].Value = loanAlertId;
            parameters[2].Value = alertEmailType;
            parameters[3].Value = alertEmailBody;
            parameters[4].Value = emailBody;
            int rowsAffected;
            DbHelperSQL.RunProcedure("lpsp_UpdateEmailBody", parameters, out rowsAffected);
        }

        public DataSet AcknowledgeAlert(int currentLoanAlertId, int userId, out bool status)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CurrentAlertId", SqlDbType.Int),
					new SqlParameter("@UserId", SqlDbType.Int),
                    new SqlParameter("@ReturnValue",SqlDbType.Int)
					};
            parameters[0].Value = currentLoanAlertId;
            parameters[1].Value = userId;
            parameters[2].Direction = ParameterDirection.ReturnValue;
            DataSet dataSet = DbHelperSQL.RunProcedure("[lpsp_AcknowledgeAlert]", parameters, "ds");
            int returnValue = Convert.ToInt32(parameters[2].Value);
            status = returnValue == 0 ? true : false;
            return dataSet;
        }

        public void AcknowledgeAlertEmailQue(int currentLoanAlertId, int userId, string alertEmailBody, byte[] emailBody)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CurrentAlertId", SqlDbType.Int),
					new SqlParameter("@UserId", SqlDbType.Int),
                    new SqlParameter("@EmailBody",SqlDbType.VarBinary),
                    new SqlParameter("@AlertEmailBody", SqlDbType.NVarChar)
					};
            parameters[0].Value = currentLoanAlertId;
            parameters[1].Value = userId;
            parameters[2].Value = emailBody;
            parameters[3].Value = alertEmailBody;
            int rowsAffected;
            DbHelperSQL.RunProcedure("lpsp_AcknowledgeAlertEmailQue", parameters, out rowsAffected);
        }

        public void SendingEmailLog(string toUser, string toContact, int emailTemplId, bool status, string err, DateTime lastSent, int fileId, string fromEmail, int userId, DateTime created, string emailBody)
        {
            string sqlCmd =
                "Insert into dbo.EmailLog([ToUser],[ToContact],[EmailTmplId],[Success],[Error],[LastSent],[FileId],[FromEmail],[FromUser],[Created],[EmailBody]) values (@ToUser,@ToContact,@EmailTmplId,@Success,@Error,@LastSent,@FileId,@FromEmail,@FromUser,@Created,@EmailBody)";
            SqlParameter[] parameters = {
                                            new SqlParameter("@ToUser", SqlDbType.NVarChar),
                                            new SqlParameter("@ToContact", SqlDbType.NVarChar),
                                            new SqlParameter("@EmailTmplId", SqlDbType.Int),
                                            new SqlParameter("@Success", SqlDbType.Bit),
                                            new SqlParameter("@Error", SqlDbType.NVarChar),
                                            new SqlParameter("@LastSent", SqlDbType.DateTime),
                                            new SqlParameter("@FileId", SqlDbType.Int),
                                            new SqlParameter("@FromEmail", SqlDbType.NVarChar),
                                            new SqlParameter("@FromUser", SqlDbType.Int),
                                            new SqlParameter("@Created", SqlDbType.DateTime),
                                            new SqlParameter("@EmailBody", SqlDbType.VarBinary)
                                        };
            parameters[0].Value = toUser;
            parameters[1].Value = toContact;
            parameters[2].Value = emailTemplId;
            parameters[3].Value = status;
            parameters[4].Value = err;
            parameters[5].Value = lastSent;
            parameters[6].Value = fileId;
            parameters[7].Value = fromEmail;
            parameters[8].Value = userId;
            parameters[9].Value = created;
            parameters[10].Value = System.Text.Encoding.UTF8.GetBytes(emailBody);
            DbHelperSQL.ExecuteSql(sqlCmd, parameters);
        }

        #endregion

        #region GetUpdateContactList
        public bool GetContactCompanyInfo(ref Table.Contacts contactInfo, ref string err)
        {
            if (contactInfo == null || contactInfo.ContactId <= 0)
            {
                err = "GetContactCompanyInfo:: contactInfo is null or ContactId is invalid.";
                return false;
            }
            if (contactInfo.ContactCompanyId <= 0 && contactInfo.ContactBranchId <= 0)
            {
                err = string.Format("GetContactCompanyInfo:: invalid ContactcompanyId={0} or invalid ContactBranchId={1}.", contactInfo.ContactCompanyId, contactInfo.ContactBranchId);
                return false;
            }
            if (contactInfo.ContactBranchId > 0 && contactInfo.ContactCompanyId <= 0)
            {
                object obj = DbHelperSQL.GetSingle(string.Format("Select dbo.lpfn_GetContactCompanyId({0})", contactInfo.ContactId));
                if (obj != null)
                {
                    contactInfo.ContactCompanyId = (int)obj;
                }
            }
            DataSet ds = null;
            try
            {
                string sqlCmd = string.Empty;
                if (contactInfo.ContactBranchId > 0)
                {
                    sqlCmd = string.Format("Select TOP 1 * from ContactBranches where ContactBranchId={0}", contactInfo.ContactBranchId);
                    ds = DbHelperSQL.Query(sqlCmd);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drb = ds.Tables[0].Rows[0];
                        contactInfo.Fax = drb["Fax"] == DBNull.Value ? contactInfo.Fax : drb["Fax"].ToString();
                    }
                }
                else if (contactInfo.ContactCompanyId > 0)
                {
                    sqlCmd = string.Format("Select TOP 1 * from ContactCompanies where ContactCompanyId={0}", contactInfo.ContactCompanyId);
                    ds = DbHelperSQL.Query(sqlCmd);
                }
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                    return true;
                DataRow dr = ds.Tables[0].Rows[0];
                contactInfo.CompanyName = dr["Name"] == DBNull.Value ? string.Empty : dr["Name"].ToString();
                contactInfo.MailingAddr = dr["Address"] == DBNull.Value ? contactInfo.MailingAddr : dr["Address"].ToString();
                contactInfo.MailingCity = dr["City"] == DBNull.Value ? contactInfo.MailingCity : dr["City"].ToString();
                contactInfo.MailingState = dr["State"] == DBNull.Value ? contactInfo.MailingState : dr["State"].ToString();
                contactInfo.MailingZip = dr["Zip"] == DBNull.Value ? contactInfo.MailingZip : dr["Zip"].ToString();
                return true;
            }
            catch (Exception ex)
            {
                err = "GetContactCompanyInfo failed to get Contact Company/Branch Info, exception:" + ex.Message;
                return false;
            }
        }

        public bool GetContactDetail_Assets(int ContactId, ref Table.Contacts contactInfo, ref string err)
        {
            int idx = 0;

            if (contactInfo == null)
            {
                return true;
            }

            contactInfo.assets = new List<Table.Assets>();

            Table.Assets Assets_temp = new Table.Assets();

            err = "";

            try
            {
                string sqlCmd = string.Format("Select * from ProspectAssets where ContactId={0}", ContactId);
                DataSet ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    err = string.Format("Unable to find ProspectAssets for the specified ContactId={0}", ContactId);
                    return true;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                idx = ds.Tables[0].Rows.Count;

                for (int i = 0; i < idx; i++)
                {
                    Assets_temp = null;
                    Assets_temp = new Table.Assets();

                    dr = ds.Tables[0].Rows[i];

                    Assets_temp.ProspectAssetId = (int)dr["ProspectAssetId"];
                    Assets_temp.ContactId = (int)dr["ContactId"];

                    if (dr["Name"] == DBNull.Value)
                    {
                        Assets_temp.Name = string.Empty;
                    }
                    else
                    {
                        Assets_temp.Name = dr["Name"].ToString();
                    }

                    if (dr["Account"] == DBNull.Value)
                    {
                        Assets_temp.Account = string.Empty;
                    }
                    else
                    {
                        Assets_temp.Account = dr["Account"].ToString();
                    }

                    if (dr["Amount"] == DBNull.Value)
                    {
                        Assets_temp.Amount = 0.0m;
                    }
                    else
                    {
                        Assets_temp.Amount = (decimal)dr["Amount"];
                    }

                    if (dr["Type"] == DBNull.Value)
                    {
                        Assets_temp.Type = string.Empty;
                    }
                    else
                    {
                        Assets_temp.Type = dr["Type"].ToString();
                    }

                    contactInfo.assets.Add(Assets_temp);
                }

                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("GetContactDetail_Assets ContactId={0}, error: {1}", ContactId, ex.Message);
                return false;
            }

        }

        public bool GetContactDetail_Employment(int ContactId, ref Table.Contacts contactInfo, ref string err)
        {
            int idx = 0;

            if (contactInfo == null)
            {
                return true;
            }

            contactInfo.employment = new List<Table.Employment>();

            Table.Employment Employment_temp = new Table.Employment();

            err = "";

            try
            {
                string sqlCmd = string.Format("Select * from ProspectEmployment where ContactId={0}", ContactId);
                DataSet ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    err = string.Format("Unable to find ProspectEmployment for the specified ContactId={0}", ContactId);
                    return true;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                idx = ds.Tables[0].Rows.Count;

                for (int i = 0; i < idx; i++)
                {
                    Employment_temp = null;
                    Employment_temp = new Table.Employment();

                    dr = ds.Tables[0].Rows[i];

                    Employment_temp.EmplId = (int)dr["EmplId"];
                    Employment_temp.ContactId = (int)dr["ContactId"];

                    if (dr["SelfEmployed"] == DBNull.Value)
                    {
                        Employment_temp.SelfEmployed = false;
                    }
                    else
                    {
                        Employment_temp.SelfEmployed = (bool)dr["SelfEmployed"];
                    }

                    if (dr["Position"] == DBNull.Value)
                    {
                        Employment_temp.Position = string.Empty;
                    }
                    else
                    {
                        Employment_temp.Position = dr["Position"].ToString();
                    }

                    if (dr["StartYear"] == DBNull.Value)
                    {
                        Employment_temp.StartYear = 0.0m;
                    }
                    else
                    {
                        Employment_temp.StartYear = (decimal)dr["StartYear"];
                    }

                    if (dr["StartMonth"] == DBNull.Value)
                    {
                        Employment_temp.StartMonth = 0.0m;
                    }
                    else
                    {
                        Employment_temp.StartMonth = (decimal)dr["StartMonth"];
                    }

                    if (dr["EndYear"] == DBNull.Value)
                    {
                        Employment_temp.EndYear = 0.0m;
                    }
                    else
                    {
                        Employment_temp.EndYear = (decimal)dr["EndYear"];
                    }

                    if (dr["EndMonth"] == DBNull.Value)
                    {
                        Employment_temp.EndMonth = 0.0m;
                    }
                    else
                    {
                        Employment_temp.EndMonth = (decimal)dr["EndMonth"];
                    }

                    if (dr["YearsOnWork"] == DBNull.Value)
                    {
                        Employment_temp.YearsOnWork = 0.0m;
                    }
                    else
                    {
                        Employment_temp.YearsOnWork = (decimal)dr["YearsOnWork"];
                    }

                    if (dr["Phone"] == DBNull.Value)
                    {
                        Employment_temp.Phone = string.Empty;
                    }
                    else
                    {
                        Employment_temp.Phone = dr["Phone"].ToString();
                    }

                    if (dr["ContactBranchId"] == DBNull.Value)
                    {
                        Employment_temp.ContactBranchId = 0;
                    }
                    else
                    {
                        Employment_temp.ContactBranchId = (int)dr["ContactBranchId"];
                    }

                    if (dr["CompanyName"] == DBNull.Value)
                    {
                        Employment_temp.CompanyName = string.Empty;
                    }
                    else
                    {
                        Employment_temp.CompanyName = dr["CompanyName"].ToString();
                    }

                    if (dr["Address"] == DBNull.Value)
                    {
                        Employment_temp.Address = string.Empty;
                    }
                    else
                    {
                        Employment_temp.Address = dr["Address"].ToString();
                    }

                    if (dr["City"] == DBNull.Value)
                    {
                        Employment_temp.City = string.Empty;
                    }
                    else
                    {
                        Employment_temp.City = dr["City"].ToString();
                    }

                    if (dr["State"] == DBNull.Value)
                    {
                        Employment_temp.State = string.Empty;
                    }
                    else
                    {
                        Employment_temp.State = dr["State"].ToString();
                    }

                    if (dr["Zip"] == DBNull.Value)
                    {
                        Employment_temp.Zip = string.Empty;
                    }
                    else
                    {
                        Employment_temp.Zip = dr["Zip"].ToString();
                    }

                    contactInfo.employment.Add(Employment_temp);
                }

                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("GetContactDetail_Employment ContactId={0}, error: {1}", ContactId, ex.Message);
                return false;
            }

        }

        public bool GetContactDetail_Income(int ContactId, ref Table.Contacts contactInfo, ref string err)
        {
            err = "";

            if (contactInfo == null)
            {
                return true;
            }

            contactInfo.income = new Table.Income();

            Table.Income Income_temp = new Table.Income();

            try
            {
                string sqlCmd = string.Format("Select * from ProspectIncome where ContactId={0}", ContactId);
                DataSet ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    err = string.Format("Unable to find ProspectIncome for the specified ContactId={0}", ContactId);
                    return true;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                Income_temp.ProspectIncomeId = (int)dr["ProspectIncomeId"];
                Income_temp.ContactId = (int)dr["ContactId"];

                if (dr["Salary"] == DBNull.Value)
                {
                    Income_temp.Salary = 0.0m;
                }
                else
                {
                    Income_temp.Salary = (decimal)dr["Salary"];
                }

                if (dr["Overtime"] == DBNull.Value)
                {
                    Income_temp.Overtime = 0.0m;
                }
                else
                {
                    Income_temp.Overtime = (decimal)dr["Overtime"];
                }

                if (dr["Bonuses"] == DBNull.Value)
                {
                    Income_temp.Bonuses = 0.0m;
                }
                else
                {
                    Income_temp.Bonuses = (decimal)dr["Bonuses"];
                }

                if (dr["Commission"] == DBNull.Value)
                {
                    Income_temp.Commission = 0.0m;
                }
                else
                {
                    Income_temp.Commission = (decimal)dr["Commission"];
                }

                if (dr["Div_Int"] == DBNull.Value)
                {
                    Income_temp.Div_Int = 0.0m;
                }
                else
                {
                    Income_temp.Div_Int = (decimal)dr["Div_Int"];
                }

                if (dr["NetRent"] == DBNull.Value)
                {
                    Income_temp.NetRent = 0.0m;
                }
                else
                {
                    Income_temp.NetRent = (decimal)dr["NetRent"];
                }

                if (dr["Other"] == DBNull.Value)
                {
                    Income_temp.Other = 0.0m;
                }
                else
                {
                    Income_temp.Other = (decimal)dr["Other"];
                }

                if (dr["EmplId"] == DBNull.Value)
                {
                    Income_temp.EmplId = 0;
                }
                else
                {
                    Income_temp.EmplId = (int)dr["EmplId"];
                }

                contactInfo.income = Income_temp;

                Income_temp = null;

                return true;
            }
            catch (Exception ex)
            {
                err = string.Format("GetContactDetail_Income ContactId={0}, error: {1}", ContactId, ex.Message);
                return false;
            }

        }

        public bool GetContactDetail_OtherIncome(int ContactId, ref Table.Contacts contactInfo, ref string err)
        {
            int idx = 0;
            int NewContactId = 0;
            int FileId = 0;
            int ContactRoleId = 0;
            int NewContactRoleId = 0;
            int errcode = 0;
            string RoleType = "";
            string NewRoleType = "";
            err = "";

            try
            {
                string sqlCmd = string.Format("Select Top 1 * from LoanContacts where ContactId={0}", ContactId);
                DataSet ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    err = string.Format("Unable to find LoanContacts for the specified ContactId={0}", ContactId);
                    errcode = 1;
                }

                if (errcode == 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    FileId = (int)dr["FileId"];
                    ContactRoleId = (int)dr["ContactRoleId"];
                    if (ContactRoleId == 1)
                    {
                        RoleType = "B";
                        NewRoleType = "C";
                        NewContactRoleId = 2;
                    }
                    else
                    {
                        RoleType = "C";
                        NewRoleType = "B";
                        NewContactRoleId = 1;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            if (contactInfo == null)
            {
                return true;
            }

            contactInfo.otherincome = new List<Table.OtherIncome>();

            Table.OtherIncome OtherIncome_temp = new Table.OtherIncome();

            try
            {
                string sqlCmd = string.Format("Select * from ProspectOtherIncome where ContactId={0}", ContactId);
                DataSet ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    err = string.Format("Unable to find ProspectOtherIncome for the specified ContactId={0}", ContactId);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                idx = ds.Tables[0].Rows.Count;

                for (int i = 0; i < idx; i++)
                {
                    OtherIncome_temp = null;
                    OtherIncome_temp = new Table.OtherIncome();

                    dr = ds.Tables[0].Rows[i];

                    OtherIncome_temp.ProspectOtherIncomeId = (int)dr["ProspectOtherIncomeId"];
                    OtherIncome_temp.ContactId = (int)dr["ContactId"];

                    if (dr["Type"] == DBNull.Value)
                    {
                        OtherIncome_temp.Type = String.Empty;
                    }
                    else
                    {
                        OtherIncome_temp.Type = dr["Type"].ToString();
                    }

                    if (dr["MonthlyIncome"] == DBNull.Value)
                    {
                        OtherIncome_temp.MonthlyIncome = 0.0m;
                    }
                    else
                    {
                        OtherIncome_temp.MonthlyIncome = (decimal)dr["MonthlyIncome"];
                    }

                    OtherIncome_temp.RoleType = RoleType;

                    contactInfo.otherincome.Add(OtherIncome_temp);

                }
            }
            catch (Exception ex)
            {
                err = string.Format("GetContactDetail_OtherIncome ContactId={0}, error: {1}", ContactId, ex.Message);
                return false;
            }

            string sqlCmd1 = "";
            sqlCmd1 = "select Top 1 ContactId from LoanContacts where FileId=" + FileId + " AND ContactRoleId=" + NewContactRoleId;
            object obj = DbHelperSQL.GetSingle(sqlCmd1);
            NewContactId = obj == null ? 0 : (int)obj;
            if (NewContactId <= 0)
            {
                return true;
            }

            try
            {
                string sqlCmd = string.Format("Select * from ProspectOtherIncome where ContactId={0}", NewContactId);
                DataSet ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    return true;
                }

                DataRow dr = ds.Tables[0].Rows[0];

                idx = ds.Tables[0].Rows.Count;

                for (int i = 0; i < idx; i++)
                {
                    OtherIncome_temp = null;
                    OtherIncome_temp = new Table.OtherIncome();

                    dr = ds.Tables[0].Rows[i];

                    OtherIncome_temp.ProspectOtherIncomeId = (int)dr["ProspectOtherIncomeId"];
                    OtherIncome_temp.ContactId = (int)dr["ContactId"];

                    if (dr["Type"] == DBNull.Value)
                    {
                        OtherIncome_temp.Type = String.Empty;
                    }
                    else
                    {
                        OtherIncome_temp.Type = dr["Type"].ToString();
                    }

                    if (dr["MonthlyIncome"] == DBNull.Value)
                    {
                        OtherIncome_temp.MonthlyIncome = 0.0m;
                    }
                    else
                    {
                        OtherIncome_temp.MonthlyIncome = (decimal)dr["MonthlyIncome"];
                    }

                    OtherIncome_temp.RoleType = NewRoleType;

                    contactInfo.otherincome.Add(OtherIncome_temp);

                }

                return true;
            }
            catch (Exception ex)
            {
                return true;
            }

        }

        public Table.Contacts GetContactDetail(int ContactId, ref string err)
        {
            bool status = true;

            err = "";
            Table.Contacts contactInfo = null;

            if (ContactId <= 0)
            {
                err = string.Format("GetContactDetail, Invalid ContactId ={0}", ContactId);
                return contactInfo;
            }

            try
            {
                string sqlCmd = string.Format("Select * from Contacts where ContactId={0}", ContactId);
                DataSet ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    err = string.Format("Unable to find contact detail for the specified ContactId={0}", ContactId);
                    return contactInfo;
                }
                DataRow dr = ds.Tables[0].Rows[0];

                if (dr["FirstName"] == DBNull.Value || dr["LastName"] == DBNull.Value)
                {
                    err = string.Format("Missing firstname or lastname for ContactId={0}", ContactId);
                    return contactInfo;
                }

                contactInfo = new Table.Contacts();
                contactInfo.ContactId = ContactId;
                contactInfo.FirstName = dr["FirstName"].ToString();
                contactInfo.LastName = dr["LastName"].ToString();
                contactInfo.MiddleName = dr["MiddleName"] == DBNull.Value ? string.Empty : dr["MiddleName"].ToString();
                contactInfo.MailingAddr = dr["MailingAddr"] == DBNull.Value ? String.Empty : dr["MailingAddr"].ToString();
                contactInfo.MailingCity = dr["MailingCity"] == DBNull.Value ? String.Empty : dr["MailingCity"].ToString();
                contactInfo.MailingState = dr["MailingState"] == DBNull.Value ? String.Empty : dr["MailingState"].ToString();
                contactInfo.MailingZip = dr["MailingZip"] == DBNull.Value ? String.Empty : dr["MailingZip"].ToString();
                contactInfo.NickName = dr["NickName"] == DBNull.Value ? String.Empty : dr["NickName"].ToString();
                contactInfo.Title = dr["Title"] == DBNull.Value ? String.Empty : dr["Title"].ToString();
                contactInfo.SSN = dr["SSN"] == DBNull.Value ? String.Empty : dr["SSN"].ToString();
                contactInfo.GenerationCode = dr["GenerationCode"] == DBNull.Value ? String.Empty : dr["GenerationCode"].ToString();
                contactInfo.HomePhone = dr["HomePhone"] == DBNull.Value ? String.Empty : dr["HomePhone"].ToString();
                contactInfo.BusinessPhone = dr["BusinessPhone"] == DBNull.Value ? String.Empty : dr["BusinessPhone"].ToString();
                contactInfo.CellPhone = dr["CellPhone"] == DBNull.Value ? String.Empty : dr["CellPhone"].ToString();
                contactInfo.Fax = dr["Fax"] == DBNull.Value ? String.Empty : dr["Fax"].ToString();
                contactInfo.Email = dr["Email"] == DBNull.Value ? String.Empty : dr["Email"].ToString();
                contactInfo.DOB = dr["DOB"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["DOB"];
                contactInfo.ContactBranchId = dr["ContactBranchId"] == DBNull.Value ? 0 : (int)dr["ContactBranchId"];
                contactInfo.ContactCompanyId = dr["ContactCompanyId"] == DBNull.Value ? 0 : (int)dr["ContactCompanyId"];

                return contactInfo;
            }
            catch (Exception ex)
            {
                err = string.Format("GetContactDetail ContactId={0}, error: {1}", ContactId, ex.Message);
                return contactInfo;
            }

        }

        public List<string> GetUpdateContactList(int ContactId, ref bool hasPartnerContact, ref string err)
        {
            bool logErr = false;
            List<string> FileContactRoles = null;
            err = "";
            hasPartnerContact = false;

            DataSet ds = null;
            if (ContactId <= 0)
            {
                logErr = true;
                err = "GetUpdateContactList Invalid ContactId=" + ContactId;
                return FileContactRoles;
            }
            FileContactRoles = new List<string>();
            string SQLString = string.Format("SELECT DISTINCT v.[FileId], v.[RoleName]  from [lpvw_GetLoanContactwRoles] v inner join Loans l on v.FileId=l.FileId where ContactId = {0} and (l.Status='Processing' OR l.Status='Prospect')", ContactId);

            try
            {
                ds = DbHelperSQL.Query(SQLString);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    return FileContactRoles;
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["RoleName"] == DBNull.Value || dr["FileId"] == DBNull.Value)
                        continue;
                    string tempstr = string.Empty;

                    tempstr = dr["RoleName"].ToString().Trim();
                    if (tempstr != ContactRoles.ContactRole_Borrower && tempstr != ContactRoles.ContactRole_Coborrower)
                        hasPartnerContact = true;
                    tempstr += ";" + dr["FileId"].ToString();
                    FileContactRoles.Add(tempstr);
                }
            }
            catch (Exception ex)
            {
                err = "Failed to fetch record in Email_Log, Exception:" + ex.Message;
                logErr = true;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                }
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
            return FileContactRoles;
        }

        #endregion

        /// <summary>
        /// Updates the loan rule last check.
        /// </summary>
        /// <param name="ruleId">The rule id.</param>
        public void UpdateLoanRuleLastCheck(int ruleId)
        {
            string sqlCmd = "UPDATE dbo.LoanRules SET LastCheck=GETDATE() WHERE LoanRuleId=@RuleId";
            SqlParameter[] parameters = {
                                            new SqlParameter("@RuleId", SqlDbType.Int)
                                        };
            parameters[0].Value = ruleId;

            try
            {
                int rows = 0;
                rows = DbHelperSQL.ExecuteSql(sqlCmd, parameters);
            }
            catch (Exception exception)
            {
                string err = MethodBase.GetCurrentMethod() + string.Format(" update rule last check faild for ruleid {0}. Exception: {1}", ruleId, exception);
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// Updates the rule manager service status.
        /// </summary>
        public void UpdateRuleManagerServiceStatus()
        {
            string sqlCmd = "lpsp_UpdateRuleManagerStatus";
            SqlParameter[] parameters = { };
            DbHelperSQL.RunProcedure(sqlCmd, parameters);
        }

        /// <summary>
        /// Creates the rule alert.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="ruleId">The rule id.</param>
        /// <param name="alertEmailType">Type of the alert email.</param>
        /// <param name="desc">The desc.</param>
        /// <param name="ackReq">if set to <c>true</c> [ack req].</param>
        /// <param name="ruleAlert">The rule alert.</param>
        /// <param name="emailBody">The email body.</param>
        /// <returns></returns>
        public int CreateRuleAlert(int fileId, int ruleId, int alertEmailType, string desc, bool ackReq, string ruleAlert, string emailBody)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@FileId", SqlDbType.Int),
					new SqlParameter("@LoanRuleId", SqlDbType.Int),
					new SqlParameter("@AlertEmailType", SqlDbType.Int),
					new SqlParameter("@Desc", SqlDbType.NVarChar,255),
					new SqlParameter("@AcknowlegeReq", SqlDbType.Bit),
					new SqlParameter("@AlertType", SqlDbType.NVarChar,50),
					new SqlParameter("@AlertEmailBody", SqlDbType.NVarChar),
                    new SqlParameter("@ReturnValue",SqlDbType.Int)
					};
            parameters[0].Value = fileId;
            parameters[1].Value = ruleId;
            parameters[2].Value = alertEmailType;
            parameters[3].Value = desc;
            parameters[4].Value = ackReq;
            parameters[5].Value = ruleAlert;
            parameters[6].Value = emailBody;
            parameters[7].Direction = ParameterDirection.ReturnValue;
            int rowsAffected;
            DbHelperSQL.RunProcedure("[dbo].[lpsp_CreateRuleAlert]", parameters, out rowsAffected);
            int returnValue = Convert.ToInt32(parameters[7].Value);
            return returnValue;
        }

        /// <summary>
        /// Creates the email que.
        /// </summary>
        /// <param name="emailTemplId">The email templ id.</param>
        /// <param name="loanAlertID">The loan alert ID.</param>
        /// <param name="fileId">The file id.</param>
        /// <param name="alertEmailType">Type of the alert email.</param>
        /// <param name="emailBody">The email body.</param>
        /// <returns></returns>
        public void CreateEmailQue(int emailTemplId, int loanAlertID, int fileId, int alertEmailType, string emailBody)
        {
            string sqlCmd = @"INSERT dbo.EmailQue(EmailTmplId,LoanAlertId,FileId,AlertEmailType,EmailBody)
				                        VALUES(@EmailTmplId,@LoanAlertId,@FileId,@AlertEmailType,@EmailBody)";
            SqlParameter[] parameters = {
                                            new SqlParameter("@EmailTmplId", SqlDbType.Int),
                                            new SqlParameter("@LoanAlertId", SqlDbType.Int),
                                            new SqlParameter("@FileId", SqlDbType.Int),
                                            new SqlParameter("@AlertEmailType", SqlDbType.Int),
                                            new SqlParameter("@EmailBody", SqlDbType.VarBinary),
                                           
                                        };
            parameters[0].Value = emailTemplId;
            parameters[1].Value = loanAlertID;
            parameters[2].Value = fileId;
            parameters[3].Value = alertEmailType;
            parameters[4].Value = Encoding.UTF8.GetBytes(emailBody);
            DbHelperSQL.ExecuteSql(sqlCmd, parameters);
        }

        /// <summary>
        /// Updates the rule alert email body.
        /// </summary>
        /// <param name="loanAlertID">The loan alert ID.</param>
        /// <param name="alertEmailType">Type of the alert email.</param>
        /// <param name="emailBody">The email body.</param>
        public void UpdateRuleAlertEmailBody(int loanAlertID, int alertEmailType, string emailBody)
        {
            string sqlCmd = "UPDATE dbo.LoanAlerts SET AlertEmail=@emailBody  WHERE LoanAlertId=@LoanAlertId";

            if (alertEmailType == 2)
            {
                sqlCmd = "UPDATE dbo.LoanAlerts SET RecomEmail=@emailBody  WHERE LoanAlertId=@LoanAlertId";
            }

            SqlParameter[] parameters = {
                                            new SqlParameter("@LoanAlertId", SqlDbType.Int),
                                            new SqlParameter("@emailBody", SqlDbType.NVarChar)
                                        };
            parameters[0].Value = loanAlertID;
            parameters[1].Value = emailBody;
            DbHelperSQL.ExecuteSql(sqlCmd, parameters);
        }

        /// <summary>
        /// Gets the prospect alert id.
        /// </summary>
        /// <param name="propsectTaskId">The propsect task id.</param>
        /// <returns></returns>
        public int GetProspectAlertId(int propsectTaskId)
        {

            string sqlCmd = "select top 1 ProspectAlertId from dbo.ProspectAlerts where ProspectTaskId={0}";
            sqlCmd = string.Format(sqlCmd, propsectTaskId);
            int prospectAlertId = 0;
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return prospectAlertId;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr[0] != DBNull.Value)
                    prospectAlertId = Convert.ToInt32(dr[0]);
            }

            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
            return prospectAlertId;
        }
        /// <summary>
        /// Gets the pendging email log.
        /// </summary>
        /// <returns></returns>
        public List<Table.PendingEmailLog> GetPendgingEmailLog()
        {
            var pendgingEmailLog = new List<Table.PendingEmailLog>();
            string SQLString = "SELECT * FROM lpvw_PendingGetReply";
            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                Table.PendingEmailLog emailLog = null;
                while (dataReader.Read())
                {
                    emailLog = new Table.PendingEmailLog();
                    emailLog.EmailLogId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    emailLog.ToUser = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1);
                    emailLog.ToContact = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
                    emailLog.EmailTmplId = dataReader.IsDBNull(3) ? 0 : dataReader.GetInt32(3);
                    emailLog.Success = dataReader.IsDBNull(4) ? false : dataReader.GetBoolean(4);
                    emailLog.Error = dataReader.IsDBNull(5) ? string.Empty : dataReader.GetString(5);
                    emailLog.LastSent = dataReader.IsDBNull(6) ? DateTime.MinValue : dataReader.GetDateTime(6);
                    emailLog.LoanAlertId = dataReader.IsDBNull(7) ? 0 : dataReader.GetInt32(7);
                    emailLog.Retries = dataReader.IsDBNull(8) ? (short)0 : dataReader.GetInt16(8);
                    emailLog.FileId = dataReader.IsDBNull(9) ? 0 : dataReader.GetInt32(9);
                    emailLog.FromEmail = dataReader.IsDBNull(10) ? string.Empty : dataReader.GetString(10);
                    emailLog.FromUser = dataReader.IsDBNull(11) ? 0 : dataReader.GetInt32(11);
                    emailLog.Created = dataReader.IsDBNull(12) ? DateTime.MinValue : dataReader.GetDateTime(12);
                    emailLog.AlertEmailType = dataReader.IsDBNull(13) ? (short)0 : dataReader.GetInt16(13);
                    //emailLog.EmailBody = dataReader.IsDBNull(14) ? null : dataReader.GetBytes(14);
                    emailLog.Subject = dataReader.IsDBNull(15) ? string.Empty : dataReader.GetString(15);
                    emailLog.Exported = dataReader.IsDBNull(16) ? false : dataReader.GetBoolean(16);
                    emailLog.ToEmail = dataReader.IsDBNull(17) ? string.Empty : dataReader.GetString(17);
                    emailLog.ProspectId = dataReader.IsDBNull(18) ? 0 : dataReader.GetInt32(18);
                    emailLog.ProspectAlertId = dataReader.IsDBNull(19) ? 0 : dataReader.GetInt32(19);
                    emailLog.CCUser = dataReader.IsDBNull(20) ? string.Empty : dataReader.GetString(20);
                    emailLog.CCContact = dataReader.IsDBNull(21) ? string.Empty : dataReader.GetString(21);
                    emailLog.ChainId = dataReader.IsDBNull(22) ? string.Empty : dataReader.GetString(22);
                    emailLog.SequenceNumber = dataReader.IsDBNull(23) ? 0 : dataReader.GetInt32(23);
                    emailLog.EwsImported = dataReader.IsDBNull(24) ? false : dataReader.GetBoolean(24);
                    emailLog.CCEmail = dataReader.IsDBNull(25) ? string.Empty : dataReader.GetString(25);
                    pendgingEmailLog.Add(emailLog);
                }
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return pendgingEmailLog;
        }

        /// <summary>
        /// Saves the email reply to db.
        /// </summary>
        /// <param name="emailLogId">The email log id.</param>
        /// <param name="fromEmail">From email.</param>
        /// <param name="emailBody">The email body.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="toEmail">To email.</param>
        /// <param name="ccEmail">The cc email.</param>
        /// <param name="dateTimeReceived">The date time received.</param>
        /// <param name="uniqueId">The unique id.</param>
        public void SaveEmailReplyToDb(int emailLogId, string fromEmail, byte[] emailBody, string subject, string toEmail, string ccEmail, DateTime dateTimeReceived, string uniqueId)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EmailLogId", SqlDbType.Int),
					new SqlParameter("@FromEmail", SqlDbType.NVarChar,255),
					new SqlParameter("@EmailBody", SqlDbType.VarBinary),
					new SqlParameter("@Subject", SqlDbType.NVarChar,500),
					new SqlParameter("@ToEmail", SqlDbType.NVarChar,255),
					new SqlParameter("@CCEmail", SqlDbType.NVarChar,255),
					new SqlParameter("@DateTimeReceived", SqlDbType.DateTime),
                    new SqlParameter("@EmailUniqueId",SqlDbType.NVarChar,255)
					};
            parameters[0].Value = emailLogId;
            if (!string.IsNullOrEmpty(fromEmail))
            {
                parameters[1].Value = fromEmail;
            }
            else
            {
                parameters[1].Value = DBNull.Value;
            }
            if (emailBody != null && emailBody.Length > 0)
            {
                parameters[2].Value = emailBody;
            }
            else
            {
                parameters[2].Value = DBNull.Value;
            }
            if (!string.IsNullOrEmpty(subject))
            {
                parameters[3].Value = subject;
            }
            else
            {
                parameters[3].Value = DBNull.Value;
            }

            if (!string.IsNullOrEmpty(toEmail))
            {
                parameters[4].Value = toEmail;
            }
            else
            {
                parameters[4].Value = DBNull.Value;
            }

            if (!string.IsNullOrEmpty(ccEmail))
            {
                parameters[5].Value = ccEmail;
            }
            else
            {
                parameters[5].Value = DBNull.Value;
            }
            if (DateTime.Now.AddDays(-3) <= dateTimeReceived && dateTimeReceived <= DateTime.Now.AddDays(3))
            {
                parameters[6].Value = dateTimeReceived;
            }
            else
            {
                parameters[6].Value = DBNull.Value;
            }


            if (!string.IsNullOrEmpty(uniqueId))
            {
                parameters[7].Value = uniqueId;
            }
            else
            {
                parameters[7].Value = DBNull.Value;
            }
            int rowsAffected;
            DbHelperSQL.RunProcedure("lpsp_CreateReplyEmailLog", parameters, out rowsAffected);

        }

        public Table.PendingEmailLog GetEmailLogInfo(int replyToEmailLogId)
        {

            //string SQLString = "SELECT TOP 1 * FROM lpvw_PendingGetReply WHERE [EmailLogId]=" + replyToEmailLogId.ToString();
            Byte[] ByteA = null;
            System.Data.SqlTypes.SqlBytes SB = new System.Data.SqlTypes.SqlBytes();
            string SQLString = "SELECT EmailLogId, ToUser, ToContact, EmailTmplId, Success, Error, LastSent, LoanAlertId, Retries, FileId, FromEmail, FromUser, Created, AlertEmailType, EmailBody, Subject, Exported, ToEmail, ProspectId, ProspectAlertId, CCUser, CCContact, ChainId, SequenceNumber, EwsImported, CCEmail, DateTimeReceived, EmailUniqueId FROM dbo.EmailLog WHERE [EmailLogId]=" + replyToEmailLogId;
            SqlDataReader dataReader = null;
            Table.PendingEmailLog emailLog = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                while (dataReader.Read())
                {
                    emailLog = new Table.PendingEmailLog();
                    emailLog.EmailLogId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    emailLog.ToUser = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1);
                    emailLog.ToContact = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
                    emailLog.EmailTmplId = dataReader.IsDBNull(3) ? 0 : dataReader.GetInt32(3);
                    emailLog.Success = dataReader.IsDBNull(4) ? false : dataReader.GetBoolean(4);
                    emailLog.Error = dataReader.IsDBNull(5) ? string.Empty : dataReader.GetString(5);
                    emailLog.LastSent = dataReader.IsDBNull(6) ? DateTime.MinValue : dataReader.GetDateTime(6);
                    emailLog.LoanAlertId = dataReader.IsDBNull(7) ? 0 : dataReader.GetInt32(7);
                    emailLog.Retries = dataReader.IsDBNull(8) ? (short)0 : dataReader.GetInt16(8);
                    emailLog.FileId = dataReader.IsDBNull(9) ? 0 : dataReader.GetInt32(9);
                    emailLog.FromEmail = dataReader.IsDBNull(10) ? string.Empty : dataReader.GetString(10);
                    emailLog.FromUser = dataReader.IsDBNull(11) ? 0 : dataReader.GetInt32(11);
                    emailLog.Created = dataReader.IsDBNull(12) ? DateTime.MinValue : dataReader.GetDateTime(12);
                    emailLog.AlertEmailType = dataReader.IsDBNull(13) ? (short)0 : dataReader.GetInt16(13);
                    //emailLog.EmailBody = dataReader.IsDBNull(14) ? null : dataReader.GetBytes(14);
                    if (dataReader.IsDBNull(14))
                    {
                        emailLog.EmailBody = null;
                    }
                    else
                    {
                        SB = dataReader.GetSqlBytes(14);
                        ByteA = SB.Value;
                        emailLog.EmailBody = ByteA;
                    }
                    emailLog.Subject = dataReader.IsDBNull(15) ? string.Empty : dataReader.GetString(15);
                    emailLog.Exported = dataReader.IsDBNull(16) ? false : dataReader.GetBoolean(16);
                    emailLog.ToEmail = dataReader.IsDBNull(17) ? string.Empty : dataReader.GetString(17);
                    emailLog.ProspectId = dataReader.IsDBNull(18) ? 0 : dataReader.GetInt32(18);
                    emailLog.ProspectAlertId = dataReader.IsDBNull(19) ? 0 : dataReader.GetInt32(19);
                    emailLog.CCUser = dataReader.IsDBNull(20) ? string.Empty : dataReader.GetString(20);
                    emailLog.CCContact = dataReader.IsDBNull(21) ? string.Empty : dataReader.GetString(21);
                    emailLog.ChainId = dataReader.IsDBNull(22) ? string.Empty : dataReader.GetString(22);
                    emailLog.SequenceNumber = dataReader.IsDBNull(23) ? 0 : dataReader.GetInt32(23);
                    emailLog.EwsImported = dataReader.IsDBNull(24) ? false : dataReader.GetBoolean(24);
                    emailLog.CCEmail = dataReader.IsDBNull(25) ? string.Empty : dataReader.GetString(25);
                    emailLog.DateTimeReceived = dataReader.IsDBNull(26) ? DateTime.MinValue : dataReader.GetDateTime(26);
                    emailLog.EmailUniqueId = dataReader.IsDBNull(27) ? string.Empty : dataReader.GetString(27);
                }
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return emailLog;
        }

        /// <summary>
        /// Pictures the signature.
        /// </summary>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="actValue">The act value.</param>
        /// <param name="fromUser">From user.</param>
        /// <param name="err">The err.</param>
        /// <returns></returns>
        public string PictureSignature(FieldType fieldType, string actValue, int fromUser, out string err)
        {
            string sqlCmd = string.Empty;
            string fieldValue = "";
            DataSet ds = null;
            err = string.Empty;
            switch (fieldType)
            {
                case FieldType.Unknown:
                    //
                    break;
                case FieldType.Previous:
                    //sqlCmd = string.Format("SELECT TOP 1 PrevValue [retValue] FROM dbo.LoanPointFields WHERE PointFieldId={0} AND FileId={1}", pointFieldId, FileId);
                    break;
                case FieldType.DB:
                    //get act value via actValue
                    bool isProspect = false;
                    bool isProsepctID = false;
                    if (string.IsNullOrEmpty(actValue))
                    {
                        err = "invalid db field";
                        return fieldValue;
                    }

                    if (DbFieldMapping.ContainsKey(actValue))
                    {
                        sqlCmd = DbFieldMapping[actValue];
                        if (actValue == "Sender Picture" || actValue == "Sender Signature")
                        {
                            sqlCmd = string.Format(sqlCmd, fromUser);
                        }
                    }
                    else
                    {
                        err = "invalid db field" + actValue + " specified.";
                        return fieldValue;
                    }
                    break;
                case FieldType.Default:
                    //sqlCmd = string.Format("SELECT  TOP 1  CurrentValue [retValue] FROM dbo.LoanPointFields WHERE PointFieldId={0} AND FileId={1}", pointFieldId, FileId);
                    break;
                default:
                    err = "invalid fieldValue" + fieldType + " specified.";
                    return fieldValue;
            }


            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "Unable to find field value for UserID " + fromUser;
                    return fieldValue;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["retValue"] == DBNull.Value)
                    return fieldValue;
                //todo:check Image binary data
                if (!string.IsNullOrEmpty(actValue))
                {

                    if (actValue == "Loan Officer Picture" || actValue == "Company Subpage Logo" || actValue == "Company Homepage Logo" || actValue == "Sender Picture")
                    {
                        byte[] imageData = (byte[])dr["retValue"];
                        fieldValue = Convert.ToBase64String(imageData);
                        fieldValue = string.Format("<img src=\"cid:{0}\" data=\"{1}\" />", Guid.NewGuid().ToString(), fieldValue);
                    }
                    else
                    {
                        fieldValue = dr["retValue"].ToString().Trim();
                    }
                }
                return fieldValue;
            }
            catch (Exception ex)
            {
                err = "Exception:" + ex.Message;
                Trace.TraceError(err);
                return fieldValue;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        /// <summary>
        /// Gets the prospect task id.
        /// </summary>
        /// <param name="prospectAlertId">The prospect alert id.</param>
        /// <returns></returns>
        public int GetProspectTaskId(int prospectAlertId)
        {
            if (prospectAlertId == 0)
                return default(int);

            string SQLString = string.Format("SELECT TOP 1 ProspectTaskId FROM  dbo.ProspectAlerts WHERE ProspectAlertId ={0}", prospectAlertId);

            object obj = null;
            try
            {
                obj = DbHelperSQL.GetSingle(SQLString);

                if (obj != null)
                {
                    return Convert.ToInt32(obj);

                }
            }
            catch (Exception e)
            {
                return default(int);
            }

            return default(int);
        }

        public Table.TemplateEmailRecipient GetLoanAlertOwnerEmail(int loanAlertId)
        {
            if (loanAlertId <= 0)
            {
                return null;
            }

            Table.TemplateEmailRecipient ownerRecipient = null;
            string SQLString = string.Format("SELECT U.EmailAddress FROM LoanAlerts LA INNER JOIN Users U ON LA.OwnerId=U.UserId where LA.LoanAlertId={0}", loanAlertId);

            object obj = null;
            try
            {
                obj = DbHelperSQL.GetSingle(SQLString);

                if (obj != null)
                {
                    ownerRecipient = new Table.TemplateEmailRecipient();
                    string ownerEmail = Convert.ToString(obj);
                    ownerRecipient.ToEmails = new string[] { ownerEmail };
                    ownerRecipient.RoleType = "Owner";
                    ownerRecipient.RecipientType = "TO";
                    ownerRecipient.TaskOwner = true;
                    return ownerRecipient;
                }
            }
            catch (Exception e)
            {
                return ownerRecipient;
            }
            return ownerRecipient;
        }

        public Common.Table.ReportBranchInfo GetReportBranchInfoByFileId(int FileID, out string err)
        {
            string sqlCmd = string.Format("SELECT TOP 1 [Name],[BranchAddress],[Phone],[WebURL],[Email],[WebsiteLogo],City + ' ' + BranchState +' '+ Zip as CityBranchStateZip FROM [dbo].[Branches] where [BranchID] =(select [BranchID] from [dbo].[Loans] where [FileId]=" + FileID + ")");
            Common.Table.ReportBranchInfo reportBranchInfo = new Common.Table.ReportBranchInfo();
            DataSet ds = null;
            err = string.Empty;

            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "Unable to find loan's branch information.";
                    return reportBranchInfo;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (!dr.IsNull("Name"))
                {
                    reportBranchInfo.Name = dr["Name"].ToString();
                }
                if (!dr.IsNull("BranchAddress"))
                {
                    reportBranchInfo.BranchAddress = dr["BranchAddress"].ToString();
                }
                if (!dr.IsNull("Phone"))
                {
                    reportBranchInfo.Phone = dr["Phone"].ToString();
                }
                if (!dr.IsNull("WebURL"))
                {
                    reportBranchInfo.WebURL = dr["WebURL"].ToString();
                }
                if (!dr.IsNull("Email"))
                {
                    reportBranchInfo.Email = dr["Email"].ToString();
                }
                if (!dr.IsNull("WebsiteLogo"))
                {
                    reportBranchInfo.WebsiteLogo = (byte[])dr["WebsiteLogo"];
                }
                if (!dr.IsNull("CityBranchStateZip"))
                {
                    reportBranchInfo.CityBranchStateZip = dr["CityBranchStateZip"].ToString();
                }
                return reportBranchInfo;
            }
            catch (Exception ex)
            {
                err = "Exception:" + ex.Message;
                Trace.TraceError(err);
                return reportBranchInfo;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public Common.Table.ReportLoanDetailInfo GetReportLoanDetailInfoByFileId(int FileID, out string err)
        {
            string sqlCmd = string.Format("SELECT TOP 1 Loans.[FileId],[PropertyAddr],[PropertyCity],[PropertyState],[PropertyZip],[SalesPrice]  ,[LoanAmount],[Rate],[Program],[Purpose],[EstCloseDate], dbo.lpfn_GetProgress(Loans.FileId) as [Progress],(v1.FirstName + ' ' +v1.LastName) as BorrowerName,(v2.FirstName + ' ' +v2.LastName) as CoBorrowerName,v1.MailingAddr,v1.MailingCity,v1.MailingState,v1.MailingZip,v1.BusinessPhone, v1.Fax,v1.Email FROM [dbo].[Loans] left join dbo.lpvw_GetLoanContactwRoles v1 on v1.FileId=Loans.FileId and v1.RoleName='Borrower'  left join dbo.lpvw_GetLoanContactwRoles v2 on v2.FileId=Loans.FileId and v2.RoleName='CoBorrower'   where Loans.[FileId]=" + FileID + "");
            Common.Table.ReportLoanDetailInfo reportLoanDetailInfo = new Common.Table.ReportLoanDetailInfo();
            DataSet ds = null;
            err = string.Empty;

            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "Unable to find loan information.";
                    return reportLoanDetailInfo;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (!dr.IsNull("FileId"))
                {
                    reportLoanDetailInfo.FileId = Convert.ToInt32(dr["FileId"]);
                }
                if (!dr.IsNull("PropertyAddr"))
                {
                    reportLoanDetailInfo.PropertyAddr = dr["PropertyAddr"].ToString();
                }
                if (!dr.IsNull("PropertyCity"))
                {
                    reportLoanDetailInfo.PropertyCity = dr["PropertyCity"].ToString();
                }
                if (!dr.IsNull("PropertyState"))
                {
                    reportLoanDetailInfo.PropertyState = dr["PropertyState"].ToString();
                }
                if (!dr.IsNull("PropertyZip"))
                {
                    reportLoanDetailInfo.PropertyZip = dr["PropertyZip"].ToString();
                }
                if (!dr.IsNull("SalesPrice"))
                {
                    reportLoanDetailInfo.SalesPrice = dr["SalesPrice"].ToString();
                }
                if (!dr.IsNull("LoanAmount"))
                {
                    reportLoanDetailInfo.LoanAmount = dr["LoanAmount"].ToString();
                }
                if (!dr.IsNull("Rate"))
                {
                    reportLoanDetailInfo.Rate = dr["Rate"].ToString();
                }
                if (!dr.IsNull("Program"))
                {
                    reportLoanDetailInfo.Program = dr["Program"].ToString();
                }
                if (!dr.IsNull("Purpose"))
                {
                    reportLoanDetailInfo.Purpose = dr["Purpose"].ToString();
                }
                if (!dr.IsNull("EstCloseDate"))
                {
                    reportLoanDetailInfo.EstCloseDate = Convert.ToDateTime(dr["EstCloseDate"]);
                }
                if (!dr.IsNull("Progress"))
                {
                    reportLoanDetailInfo.Progress = dr["Progress"].ToString();
                }
                if (!dr.IsNull("BorrowerName"))
                {
                    reportLoanDetailInfo.BorrowerName = dr["BorrowerName"].ToString();
                }
                if (!dr.IsNull("CoBorrowerName"))
                {
                    reportLoanDetailInfo.CoBorrowerName = dr["CoBorrowerName"].ToString();
                }
                if (!dr.IsNull("MailingAddr"))
                {
                    reportLoanDetailInfo.MailingAddr = dr["MailingAddr"].ToString();
                }
                if (!dr.IsNull("MailingCity"))
                {
                    reportLoanDetailInfo.MailingCity = dr["MailingCity"].ToString();
                }
                if (!dr.IsNull("MailingState"))
                {
                    reportLoanDetailInfo.MailingState = dr["MailingState"].ToString();
                }
                if (!dr.IsNull("MailingZip"))
                {
                    reportLoanDetailInfo.MailingZip = dr["MailingZip"].ToString();
                }
                if (!dr.IsNull("BusinessPhone"))
                {
                    reportLoanDetailInfo.BusinessPhone = dr["BusinessPhone"].ToString();
                }
                if (!dr.IsNull("Fax"))
                {
                    reportLoanDetailInfo.Fax = dr["Fax"].ToString();
                }
                if (!dr.IsNull("Email"))
                {
                    reportLoanDetailInfo.Email = dr["Email"].ToString();
                }
                return reportLoanDetailInfo;
            }
            catch (Exception ex)
            {
                err = "Exception:" + ex.Message;
                Trace.TraceError(err);
                return reportLoanDetailInfo;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        public Common.Table.ReportLoanOfficerInfo GetReportLoanOfficerInfoByFileId(int FileID, out string err)
        {
            string sqlCmd = string.Format("SELECT TOP 1 (FirstName + ' ' +LastName) as [Name],[EmailAddress],[Phone],[Fax],[UserPictureFile],[NMLS] FROM [dbo].[Users] where [UserID] =(select top 1 [UserId] from [dbo].[LoanTeam] where [FileId]=" + FileID + " and RoleId=(select RoleId from dbo.Roles where Name='Loan Officer'))");
            Common.Table.ReportLoanOfficerInfo reportLoanOfficerInfo = new Common.Table.ReportLoanOfficerInfo();
            DataSet ds = null;
            err = string.Empty;

            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if ((ds == null) || (ds.Tables[0].Rows.Count <= 0))
                {
                    err = "Unable to find loan's LoanOfficer information.";
                    return reportLoanOfficerInfo;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                if (!dr.IsNull("Name"))
                {
                    reportLoanOfficerInfo.Name = dr["Name"].ToString();
                }
                if (!dr.IsNull("Phone"))
                {
                    reportLoanOfficerInfo.Phone = dr["Phone"].ToString();
                }
                if (!dr.IsNull("Fax"))
                {
                    reportLoanOfficerInfo.Fax = dr["Fax"].ToString();
                }
                if (!dr.IsNull("EmailAddress"))
                {
                    reportLoanOfficerInfo.EmailAddress = dr["EmailAddress"].ToString();
                }
                if (!dr.IsNull("UserPictureFile"))
                {
                    reportLoanOfficerInfo.UserPictureFile = (byte[])dr["UserPictureFile"];
                }
                if (!dr.IsNull("NMLS"))
                {
                    reportLoanOfficerInfo.NMLS = dr["NMLS"].ToString();
                }
                return reportLoanOfficerInfo;
            }
            catch (Exception ex)
            {
                err = "Exception:" + ex.Message;
                Trace.TraceError(err);
                return reportLoanOfficerInfo;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Clear();
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        #region Add User LicenesNumber

        public string GetReportLoanOfficerUserLicenesNumber(int FileID, out string err)
        {
            var sqlCmd =
                string.Format(
                    "SELECT [LicenseNumber] FROM [dbo].[UserLicenses] WHERE [UserID] =(select top 1 [UserId] from [dbo].[LoanTeam] where [FileId]=" +
                    FileID + " and RoleId=(select RoleId from dbo.Roles where Name='Loan Officer'))");
            var strLicenesNumber = string.Empty;
            DataTable dt = null;
            err = string.Empty;
            try
            {
                dt = DbHelperSQL.ExecuteDataTable(sqlCmd);
                if ((dt == null) || (dt.Rows.Count <= 0))
                {
                    err = "Unable to find loan's LoanOfficer User LicenesNumber.";
                    return strLicenesNumber;
                }
                var q = (from item in dt.AsEnumerable() select item.Field<string>("LicenseNumber")).ToArray();
                strLicenesNumber = string.Join("<br />", q);
                return strLicenesNumber;
            }
            catch (Exception ex)
            {
                err = "Exception:" + ex.Message;
                Trace.TraceError(err);
                return strLicenesNumber;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Clear();
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        #endregion


        public Table.LoanAutoEmails GetLoanAutoEmailsByFileId(int FileId, int LoanAutoEmailId, out string err)
        {
            Table.LoanAutoEmails loanAutoEmails = null;
            string str = string.Format("Select * from LoanAutoEmails where FileId={0} and LoanAutoEmailid={1}", FileId, LoanAutoEmailId);
            DataSet ds = null;
            err = string.Empty;
            try
            {
                ds = DbHelperSQL.Query(str);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                {
                    return loanAutoEmails;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                loanAutoEmails = new Table.LoanAutoEmails()
                {
                    LoanAutoEmailId = (int)dr["LoanAutoEmailId"],
                    FileId = (int)dr["FileId"],
                    ToContactId = dr["ToContactId"] == DBNull.Value ? 0 : (int)dr["ToContactId"],
                    TouserId = dr["TouserId"] == DBNull.Value ? 0 : (int)dr["TouserId"],
                    Enabled = dr["Enabled"] == DBNull.Value ? false : (bool)dr["Enabled"],
                    External = dr["External"] == DBNull.Value ? false : (bool)dr["External"],
                    TempReportId = dr["TemplReportId"] == DBNull.Value ? 0 : (int)dr["TemplReportId"],
                    Applied = dr["Applied"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["Applied"],
                    AppliedBy = dr["AppliedBy"] == DBNull.Value ? 0 : (int)dr["AppliedBy"],
                    LastRun = dr["LastRun"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["LastRun"],
                    ScheduleType = dr["ScheduleType"] == DBNull.Value ? 0 : (short)dr["ScheduleType"]
                };

            }
            catch (Exception ex)
            {
                err = "GetLoanAutoEmailsByFileId, Exception: " + ex.Message;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
            }
            return loanAutoEmails;
        }

        public List<Table.ReportTaskInfo> GetReportLoanTaskInfoByFileId(int FileID, out string err, bool bExternal, bool bLoanTasks)
        {
            #region Conditions

            string sSql1 = string.Empty;
            string sSql2 = string.Empty;
            sSql1 = string.Format("select LoanCondId as LoanTaskId, CondName as Name, null as ICON, Due from LoanConditions where FileId={0} ", FileID);
            sSql1 += bExternal ? " AND Received IS NULL AND ExternalViewing=1 " : " AND Cleared IS NULL";

            #endregion

            sSql2 = "SELECT LoanTaskId, [Name],[dbo].[lpfn_GetTaskIcon](LoanTaskId) as [ICON],Due FROM [dbo].[LoanTasks] where [FileId]=" + FileID + " and isnull(Completed,'')='' ";
            if (bExternal)
            {
                sSql2 += " AND ExternalViewing=1 ";
            }
            Common.Table.ReportTaskInfo reportTaskInfo = new Common.Table.ReportTaskInfo();
            List<Table.ReportTaskInfo> lstInfos = new List<Table.ReportTaskInfo>();
            err = string.Empty;

            SqlDataReader dataReader = null;
            Table.ReportTaskInfo lsrInfo = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(sSql1);
                while (dataReader.Read())
                {
                    lsrInfo = new Table.ReportTaskInfo();
                    lsrInfo.LoanTaskId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    lsrInfo.Name = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1);
                    lsrInfo.ICON = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
                    lsrInfo.Due = dataReader.IsDBNull(3) ? Convert.ToDateTime("1900-1-1") : dataReader.GetDateTime(3);
                    lstInfos.Add(lsrInfo);
                }

                if (!bLoanTasks)
                    return lstInfos;

                dataReader = DbHelperSQL.ExecuteReader(sSql2);
                while (dataReader.Read())
                {
                    lsrInfo = new Table.ReportTaskInfo();
                    lsrInfo.LoanTaskId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    lsrInfo.Name = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1);
                    lsrInfo.ICON = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
                    lsrInfo.Due = dataReader.IsDBNull(3) ? Convert.ToDateTime("1900-1-1") : dataReader.GetDateTime(3);
                    lstInfos.Add(lsrInfo);
                }
            }
            catch (Exception ex)
            {
                err = "Exception:" + ex.Message;
                Trace.TraceError(err);
                return lstInfos;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return lstInfos;
        }

        public List<Table.ReportLoanContactInfo> GetReportLoanContactInfoByFileId(int FileID, out string err)
        {
            string sqlCmd = string.Format(@"SELECT     dbo.LoanContacts.ContactId, dbo.ContactRoles.Name AS RolesName, dbo.Contacts.FirstName + ' ' + dbo.Contacts.LastName AS Name, 
                      dbo.Contacts.BusinessPhone, dbo.Contacts.Fax, dbo.Contacts.Email, dbo.Contacts.Picture, dbo.ContactCompanies.Website, 
                      dbo.lpfn_GetContactCompanyName(dbo.Contacts.ContactId) AS CompanyName
FROM         dbo.Contacts LEFT OUTER JOIN
                      dbo.ContactCompanies ON dbo.Contacts.ContactCompanyId = dbo.ContactCompanies.ContactCompanyId RIGHT OUTER JOIN
                      dbo.LoanContacts ON dbo.Contacts.ContactId = dbo.LoanContacts.ContactId RIGHT OUTER JOIN
                      dbo.ContactRoles ON dbo.LoanContacts.ContactRoleId = dbo.ContactRoles.ContactRoleId
WHERE     (dbo.LoanContacts.FileId = " + FileID + ") AND  dbo.LoanContacts.ContactRoleId<>dbo.lpfn_GetBorrowerRoleId() AND dbo.LoanContacts.ContactRoleId<>dbo.lpfn_GetCoBorrowerRoleId()");
            Common.Table.ReportLoanContactInfo reportTaskInfo = new Common.Table.ReportLoanContactInfo();
            List<Table.ReportLoanContactInfo> lstInfos = new List<Table.ReportLoanContactInfo>();
            err = string.Empty;

            SqlDataReader dataReader = null;
            Table.ReportLoanContactInfo lsrInfo = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(sqlCmd);
                while (dataReader.Read())
                {
                    lsrInfo = new Table.ReportLoanContactInfo();
                    lsrInfo.ContactId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    lsrInfo.RolesName = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1);
                    lsrInfo.Name = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
                    lsrInfo.BusinessPhone = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3);
                    lsrInfo.Fax = dataReader.IsDBNull(4) ? string.Empty : dataReader.GetString(4);
                    lsrInfo.Email = dataReader.IsDBNull(5) ? string.Empty : dataReader.GetString(5);

                    if (!dataReader.IsDBNull(6))
                    {
                        lsrInfo.Picture = (byte[])dataReader[6];
                    }
                    lsrInfo.Website = dataReader.IsDBNull(7) ? string.Empty : dataReader.GetString(7);
                    lsrInfo.CompanyName = dataReader.IsDBNull(8) ? string.Empty : dataReader.GetString(8);
                    lstInfos.Add(lsrInfo);
                }
            }
            catch (Exception ex)
            {
                err = "Exception:" + ex.Message;
                Trace.TraceError(err);
                return lstInfos;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return lstInfos;
        }

        public List<Table.LSRInfo> GetLSRInfo()
        {
            List<Table.LSRInfo> lstInfos = new List<Table.LSRInfo>();
            string SQLString = @"SELECT TemplReportId 
	                                   ,ToContactId
	                                   ,dbo.lpfn_GetContactName(ToContactId) ToContactUserName
	                                   ,(select Email FROM dbo.Contacts WHERE ContactId=ToContactId) ToContactEmail
	                                   ,ToUserId
	                                   ,dbo.lpfn_GetUserName(ToUserId) ToUserUserName
	                                   ,dbo.lpfn_GetUserEmail(ToUserId) ToUserEmail
	                                   ,DATEDIFF(DAY,LastRun, GetDate()) XTD
	                                   ,dbo.lpfn_GetBorrower(Fileid) Borrower
	                                   ,ScheduleType
	                                   ,FileId
                                       ,LoanAutoEmailid
                                       ,LastRun
                                        FROM lpvw_GetEnableAutoEmailLoan";
            SqlDataReader dataReader = null;
            Table.LSRInfo lsrInfo = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                while (dataReader.Read())
                {
                    lsrInfo = new Table.LSRInfo();
                    lsrInfo.TemplReportId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    lsrInfo.ToContactId = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt32(1);
                    lsrInfo.ToContactUserName = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
                    lsrInfo.ToContactEmail = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3);
                    lsrInfo.ToUserId = dataReader.IsDBNull(4) ? 0 : dataReader.GetInt32(4);
                    lsrInfo.ToUserUserName = dataReader.IsDBNull(5) ? string.Empty : dataReader.GetString(5);
                    lsrInfo.ToUserEmail = dataReader.IsDBNull(6) ? string.Empty : dataReader.GetString(6);
                    lsrInfo.XTD = dataReader.IsDBNull(7) ? 0 : dataReader.GetInt32(7);
                    lsrInfo.Borrower = dataReader.IsDBNull(8) ? string.Empty : dataReader.GetString(8);
                    lsrInfo.ScheduleType = dataReader.IsDBNull(9) ? 0 : dataReader.GetInt16(9);
                    lsrInfo.FileId = dataReader.IsDBNull(10) ? 0 : dataReader.GetInt32(10);
                    lsrInfo.LoanAutoEmailid = dataReader.IsDBNull(11) ? 0 : dataReader.GetInt32(11);
                    if (!dataReader.IsDBNull(12))
                    {
                        lsrInfo.LastRun = Convert.ToDateTime(dataReader[12]);
                    }
                    lstInfos.Add(lsrInfo);
                }
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return lstInfos;
        }

        public void UpdateLoanAutoEmailStatus(int loanAutoEmailid, ref string err)
        {
            err = "";
            string sqlCmd = string.Format(@"UPDATE [dbo].[LoanAutoEmails]
                                            SET [LastRun] = GetDate()      
                                            WHERE [LoanAutoEmailId] = {0}", loanAutoEmailid);
            try
            {
                int rows = 0;
                rows = DbHelperSQL.ExecuteSql(sqlCmd);

            }
            catch (Exception ex)
            {
                err = "Failed to update LoanAutoEmails stats. Exception:" + ex.Message;
                Trace.TraceError(err);
            }
        }

        public string GetCompanyName()
        {
            string sSql = "SELECT TOP 1 [Name] FROM dbo.Company_General";
            string sCompanyName = DbHelperSQL.ExecuteScalar(sSql).ToString();
            return sCompanyName;
        }

        #region DataTrac
        string strUpdateLoanPointFieldSqlPattern = @"
IF EXISTS (SELECT 1 FROM LoanPointFields WHERE FileId='{0}' AND PointFieldId='{1}')
	BEGIN 
		UPDATE LoanPointFields set PrevValue=CurrentValue, CurrentValue='{2}', ChangeTime=GETDATE() WHERE FileId='{0}' and PointFieldId='{1}'
	END
ELSE 
	BEGIN 
		INSERT INTO LoanPointFields(FileId, PointFieldId, CurrentValue, PrevValue, ChangeTime)VALUES('{0}', '{1}', '{2}', null, null) 
	END";

        /// <summary>
        /// update Pulse Loans data with DataTrac loan data
        /// neo 2011-06-23
        /// </summary>
        /// <param name="LoanItem"></param>
        /// <param name="Borrower"></param>
        /// <param name="CoBorrower"></param>
        public void UpdateLoanInfo(Common.Table.Loans LoanItem, Common.Table.Contacts Borrower, Common.Table.Contacts CoBorrower)
        {
            #region Build SqlCommand - Update Loans
            /*
            string sSql = "update Loans set PropertyAddr=@PropertyAddr, PropertyCity=@PropertyCity, PropertyState=@PropertyState, PropertyZip=@PropertyZip, "
                        + "County=@County, LoanAmount=@LoanAmount, Rate=@Rate, LienPosition=@LienPosition, Purpose=@Purpose, Program=@Program, "
                        + "DownPay=@DownPay, EstCloseDate=@EstCloseDate, RateLockExpiration=@RateLockExpiration, "
                        + "SalesPrice=@SalesPrice, AppraisedValue=@AppraisedValue, LoanType=@LoanType, MonthlyPayment=@MonthlyPayment, LTV=@LTV, "
                        + "CLTV=@CLTV, Term=@Term, Due=@Due, Occupancy=@Occupancy, CCScenario=@CCScenario, Lender=@Lender "
                        + "where FileId=" + LoanItem.FileId;
             */
            string sSql = @"update Loans set 
AppraisedValue=@AppraisedValue, 
CLTV=@CLTV, 
LienPosition=@LienPosition, 
LoanAmount=@LoanAmount,
LTV=@LTV,
PropertyAddr=@PropertyAddr, 
PropertyCity=@PropertyCity, 
PropertyState=@PropertyState, 
PropertyZip=@PropertyZip, 
Rate=@Rate, 
Term=@Term, 
Occupancy=@Occupancy, 
County=@County, 
EstCloseDate=@EstCloseDate, 
SalesPrice=@SalesPrice, 
Program=@Program,
Purpose=@Purpose, 
RateLockExpiration=@RateLockExpiration,
DateHMDA=@DateHMDA,
DateSubmit=@DateSubmit,
DateDocs=@DateDocs,
DateDocsOut=@DateDocsOut,
DateDocsReceived=@DateDocsReceived,
DateClose=@DateClose
where FileId=" + LoanItem.FileId;
            SqlCommand SqlCmd = new SqlCommand(sSql);

            DbHelperSQL.AddSqlParameter(SqlCmd, "@AppraisedValue", SqlDbType.Money, LoanItem.AppraisedValue);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@CLTV", SqlDbType.SmallMoney, LoanItem.CLTV);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@LienPosition", SqlDbType.NVarChar, CheckSqlServerString(LoanItem.LienPosition));
            DbHelperSQL.AddSqlParameter(SqlCmd, "@LoanAmount", SqlDbType.Money, LoanItem.LoanAmount);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@LTV", SqlDbType.SmallMoney, LoanItem.LTV);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@PropertyAddr", SqlDbType.NVarChar, CheckSqlServerString(LoanItem.PropertyAddr));
            DbHelperSQL.AddSqlParameter(SqlCmd, "@PropertyCity", SqlDbType.NVarChar, CheckSqlServerString(LoanItem.PropertyCity));
            DbHelperSQL.AddSqlParameter(SqlCmd, "@PropertyState", SqlDbType.Char, CheckSqlServerChar(LoanItem.PropertyState, 2));
            DbHelperSQL.AddSqlParameter(SqlCmd, "@PropertyZip", SqlDbType.NVarChar, CheckSqlServerString(LoanItem.PropertyZip));
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Rate", SqlDbType.SmallMoney, LoanItem.Rate);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Term", SqlDbType.SmallInt, LoanItem.Term);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Occupancy", SqlDbType.NVarChar, CheckSqlServerString(LoanItem.Occupancy));
            DbHelperSQL.AddSqlParameter(SqlCmd, "@County", SqlDbType.NVarChar, CheckSqlServerString(LoanItem.County));
            DbHelperSQL.AddSqlParameter(SqlCmd, "@EstCloseDate", SqlDbType.DateTime, CheckSqlServerDateTime(LoanItem.EstCloseDate));
            DbHelperSQL.AddSqlParameter(SqlCmd, "@SalesPrice", SqlDbType.Money, LoanItem.SalesPrice);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Program", SqlDbType.NVarChar, CheckSqlServerString(LoanItem.Program));
            DbHelperSQL.AddSqlParameter(SqlCmd, "@Purpose", SqlDbType.NVarChar, CheckSqlServerString(LoanItem.Purpose));
            DbHelperSQL.AddSqlParameter(SqlCmd, "@RateLockExpiration", SqlDbType.DateTime, CheckSqlServerDateTime(LoanItem.RateLockExpiration));

            DbHelperSQL.AddSqlParameter(SqlCmd, "@DateHMDA", SqlDbType.DateTime, CheckSqlServerDateTime(LoanItem.DateHMDA));
            DbHelperSQL.AddSqlParameter(SqlCmd, "@DateSubmit", SqlDbType.DateTime, CheckSqlServerDateTime(LoanItem.DateSubmit));
            DbHelperSQL.AddSqlParameter(SqlCmd, "@DateDocs", SqlDbType.DateTime, CheckSqlServerDateTime(LoanItem.DateDocs));
            DbHelperSQL.AddSqlParameter(SqlCmd, "@DateDocsOut", SqlDbType.DateTime, CheckSqlServerDateTime(LoanItem.DateDocsOut));
            DbHelperSQL.AddSqlParameter(SqlCmd, "@DateDocsReceived", SqlDbType.DateTime, CheckSqlServerDateTime(LoanItem.DateDocsReceived));
            DbHelperSQL.AddSqlParameter(SqlCmd, "@DateClose", SqlDbType.DateTime, CheckSqlServerDateTime(LoanItem.DateClose));
            //DbHelperSQL.AddSqlParameter(SqlCmd, "@CurrentStage", SqlDbType.NVarChar, CheckSqlServerString(LoanItem.CurrentStage));

            //DbHelperSQL.AddSqlParameter(SqlCmd, "@DownPay", SqlDbType.Money, LoanItem.DownPay);
            //DbHelperSQL.AddSqlParameter(SqlCmd, "@LoanType", SqlDbType.NVarChar, CheckSqlServerString(LoanItem.LoanType));
            //DbHelperSQL.AddSqlParameter(SqlCmd, "@MonthlyPayment", SqlDbType.Money, LoanItem.MonthlyPayment); 
            //DbHelperSQL.AddSqlParameter(SqlCmd, "@Due", SqlDbType.SmallInt, LoanItem.Due);
            //DbHelperSQL.AddSqlParameter(SqlCmd, "@CCScenario", SqlDbType.NVarChar, CheckSqlServerString(LoanItem.CCScenario));
            //DbHelperSQL.AddSqlParameter(SqlCmd, "@Lender", SqlDbType.Int, LoanItem.Lender);

            #endregion

            #region Get Borrower Info

            string sSql11 = "select * from LoanContacts where FileId=" + LoanItem.FileId + " and ContactRoleId=dbo.lpfn_GetBorrowerRoleId()";
            DataTable BorrowerInfo = DbHelperSQL.ExecuteDataTable(sSql11);

            #endregion

            #region Get CoBorrower Info

            string sSql12 = "select * from LoanContacts where FileId=" + LoanItem.FileId + " and ContactRoleId=dbo.lpfn_GetCoBorrowerRoleId()";
            DataTable CoBorrowerInfo = DbHelperSQL.ExecuteDataTable(sSql12);

            #endregion

            #region Build SqlCommand - Update Borrower

            string sSql2 = "update Contacts set DOB=@DOB, TransUnion=@TransUnion, Experian=@Experian, Equifax=@Equifax, SSN=@SSN where ContactId=@ContactId";
            SqlCommand SqlCmd2 = new SqlCommand(sSql2);

            if (Borrower != null && BorrowerInfo.Rows.Count > 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd2, "@DOB", SqlDbType.DateTime, CheckSqlServerDateTime(Borrower.DOB));
                DbHelperSQL.AddSqlParameter(SqlCmd2, "@TransUnion", SqlDbType.SmallInt, Borrower.TransUnion);
                DbHelperSQL.AddSqlParameter(SqlCmd2, "@Experian", SqlDbType.SmallInt, Borrower.Experian);
                DbHelperSQL.AddSqlParameter(SqlCmd2, "@Equifax", SqlDbType.SmallInt, Borrower.Equifax);
                DbHelperSQL.AddSqlParameter(SqlCmd2, "@SSN", SqlDbType.NVarChar, CheckSqlServerString(Borrower.SSN));

                DbHelperSQL.AddSqlParameter(SqlCmd2, "@ContactId", SqlDbType.Int, Convert.ToInt32(BorrowerInfo.Rows[0]["ContactId"]));
            }

            #endregion

            #region Build SqlCommand - Update CoBorrower

            string sSql3 = "update Contacts set DOB=@DOB, TransUnion=@TransUnion, Experian=@Experian, Equifax=@Equifax, SSN=@SSN where ContactId=@ContactId";

            SqlCommand SqlCmd3 = new SqlCommand(sSql3);

            if (CoBorrower != null && CoBorrowerInfo.Rows.Count > 0)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd3, "@DOB", SqlDbType.DateTime, CheckSqlServerDateTime(CoBorrower.DOB));
                DbHelperSQL.AddSqlParameter(SqlCmd3, "@TransUnion", SqlDbType.SmallInt, CoBorrower.TransUnion);
                DbHelperSQL.AddSqlParameter(SqlCmd3, "@Experian", SqlDbType.SmallInt, CoBorrower.Experian);
                DbHelperSQL.AddSqlParameter(SqlCmd3, "@Equifax", SqlDbType.SmallInt, CoBorrower.Equifax);
                DbHelperSQL.AddSqlParameter(SqlCmd3, "@SSN", SqlDbType.NVarChar, CheckSqlServerString(CoBorrower.SSN));

                DbHelperSQL.AddSqlParameter(SqlCmd3, "@ContactId", SqlDbType.Int, Convert.ToInt32(CoBorrowerInfo.Rows[0]["ContactId"]));
            }

            #endregion

            #region Build SqlCommand - Update loan field in LoanPointField table --Peter
            StringBuilder sbSql = new StringBuilder();
            int nTempFieldId = 0;
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 801, LoanItem.AppraisedValue);   //gen.appras_val
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 541, LoanItem.CLTV);             //gen.cltv

            // LienPosition ??
            if (!string.IsNullOrEmpty(LoanItem.LienPosition))
            {
                switch (LoanItem.LienPosition.Trim().ToUpper())
                {
                    case "FIRST":
                        nTempFieldId = 915;
                        break;
                    case "SECOND":
                        nTempFieldId = 916;
                        break;
                    default:
                        nTempFieldId = 917;
                        break;
                }
            }
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, nTempFieldId, CheckSqlServerString(LoanItem.LienPosition));

            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 11, LoanItem.LoanAmount);        //gen.bloan_amt
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 540, LoanItem.LTV);              //gen.ltv
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 31, CheckSqlServerString(LoanItem.PropertyAddr));      //gen.prop_no
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 32, CheckSqlServerString(LoanItem.PropertyCity));      //gen.prop_city
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 33, CheckSqlServerString(LoanItem.PropertyState));     //gen.prop_state
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 34, CheckSqlServerString(LoanItem.PropertyZip));       //gen.prop_zip
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 12, LoanItem.Rate);              //gen.int_rate
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 13, LoanItem.Term);              //gen.loan_term

            // Occupancy
            if (!string.IsNullOrEmpty(LoanItem.Occupancy))
            {
                if (LoanItem.Occupancy.Trim().ToUpper().Contains("PRIMARY"))
                    nTempFieldId = 921;
                else if (LoanItem.Occupancy.Trim().ToUpper().Contains("SECONDARY"))
                    nTempFieldId = 923;
                else if (LoanItem.Occupancy.Trim().ToUpper().Contains("INVESTMENT"))
                    nTempFieldId = 924;
                else
                    nTempFieldId = 921;
            }
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, nTempFieldId, CheckSqlServerString(LoanItem.Occupancy));

            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 35, CheckSqlServerString(LoanItem.County));            //County
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 6075, CheckSqlServerDateTime(LoanItem.EstCloseDate));    //EstCloseDate
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 800, LoanItem.SalesPrice);       //SalesPrice
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 7403, CheckSqlServerString(LoanItem.Program));         //Program

            // Purpose
            if (!string.IsNullOrEmpty(LoanItem.Purpose))
            {
                switch (LoanItem.Purpose.Trim().ToUpper())
                {
                    case "PURCHASE":
                        nTempFieldId = 1190;
                        break;
                    case "CONSTRUCTION-PERMANENT":  // ?? DataTrac没有返回
                        nTempFieldId = 1191;
                        break;
                    case "CONSTRUCTION":
                        nTempFieldId = 1192;
                        break;
                    case "CASH-OUT REFINANCE":  // ?? DataTrac没有返回
                        nTempFieldId = 1193;
                        break;
                    case "OTHER":
                        nTempFieldId = 1194;
                        break;
                    case "NO CASH-OUT REFINANCE":  // ?? DataTrac没有返回
                        nTempFieldId = 1198;
                        break;
                    default: nTempFieldId = 1190;
                        break;
                }
            }
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, nTempFieldId, CheckSqlServerString(LoanItem.Purpose));

            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 6063, CheckSqlServerDateTime(LoanItem.RateLockExpiration));  //RateLockExpiration

            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 4997, CheckSqlServerDateTime(LoanItem.DateHMDA));
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 6025, CheckSqlServerDateTime(LoanItem.DateSubmit));
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 6035, CheckSqlServerDateTime(LoanItem.DateDocs));
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 11440, CheckSqlServerDateTime(LoanItem.DateDocsOut));
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 11442, CheckSqlServerDateTime(LoanItem.DateDocsReceived));
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 6055, CheckSqlServerDateTime(LoanItem.DateClose));
            //sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 6063, CheckSqlServerString(LoanItem.CurrentStage));

            // Borrower Info
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 108, CheckSqlServerString(Borrower.SSN));
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 118, CheckSqlServerDateTime(Borrower.DOB));
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 5032, Borrower.Experian);
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 5034, Borrower.TransUnion);
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 5036, Borrower.Equifax);

            // Co-Borrower Info
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 158, CheckSqlServerString(CoBorrower.SSN));
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 168, CheckSqlServerDateTime(CoBorrower.DOB));
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 5033, CoBorrower.Experian);
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 5035, CoBorrower.TransUnion);
            sbSql.AppendFormat(strUpdateLoanPointFieldSqlPattern, LoanItem.FileId, 5037, CoBorrower.Equifax);

            SqlCommand cmdUpdateLoanPointField = new SqlCommand(sbSql.ToString());

            #endregion

            #region 批量执行SQL语句

            SqlConnection SqlConn = null;
            SqlTransaction SqlTrans = null;

            try
            {
                SqlConn = DbHelperSQL.GetOpenConnection();
                SqlTrans = SqlConn.BeginTransaction();

                DbHelperSQL.ExecuteNonQuery(SqlCmd, SqlTrans);

                if (Borrower != null && BorrowerInfo.Rows.Count > 0)
                {
                    DbHelperSQL.ExecuteNonQuery(SqlCmd2, SqlTrans);
                }

                if (CoBorrower != null && CoBorrowerInfo.Rows.Count > 0)
                {
                    DbHelperSQL.ExecuteNonQuery(SqlCmd3, SqlTrans);
                }

                // update loan field in LoanPointField table
                DbHelperSQL.ExecuteNonQuery(cmdUpdateLoanPointField, SqlTrans);

                SqlTrans.Commit();
            }
            catch (Exception ex)
            {
                SqlTrans.Rollback();
                throw ex;
            }
            finally
            {
                if (SqlConn != null)
                {
                    SqlConn.Close();
                }
            }

            #endregion
        }

        private object CheckSqlServerDateTime(DateTime? dt)
        {
            if (!dt.HasValue || dt.Value < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
                return DBNull.Value;
            else
                return dt;
        }

        private object CheckSqlServerString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return DBNull.Value;
            else
                return str;
        }

        private object CheckSqlServerChar(string str, int nLen)
        {
            if (string.IsNullOrEmpty(str) || str.Length > nLen)
                return DBNull.Value;
            else
                return str;
        }

        public void Update_Pointfile_LastModified(int iFileID, Nullable<DateTime> LastModified)
        {
            string Sql1 = "update PointFiles set [LastModified]=@LastModified where [FileId]=" + iFileID;

            SqlCommand SqlCmd1 = new SqlCommand(Sql1);

            if ((LastModified == null) || (LastModified == DateTime.MinValue))
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@LastModified", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@LastModified", SqlDbType.DateTime, LastModified);
            }

            DbHelperSQL.ExecuteNonQuery(SqlCmd1);
        }

        public void UpdateDataTracLoanStages(int iFileID, Table.Loans loans_table)
        {
            string StageName = "Application";

            string Sql1 = "update LoanStages set [Completed]=@DateOpen where [FileId]=" + iFileID + " and [StageName]='" + StageName + "'";

            SqlCommand SqlCmd1 = new SqlCommand(Sql1);

            if ((loans_table.DateOpen == null) || (loans_table.DateOpen == DateTime.MinValue))
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@DateOpen", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd1, "@DateOpen", SqlDbType.DateTime, loans_table.DateOpen);
            }

            DbHelperSQL.ExecuteNonQuery(SqlCmd1);

            StageName = "Submit";

            string Sql2 = "update LoanStages set [Completed]=@DateSubmit where [FileId]=" + iFileID + " and [StageName]='" + StageName + "'";

            SqlCommand SqlCmd2 = new SqlCommand(Sql2);

            if ((loans_table.DateSubmit == null) || (loans_table.DateSubmit == DateTime.MinValue))
            {
                DbHelperSQL.AddSqlParameter(SqlCmd2, "@DateSubmit", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd2, "@DateSubmit", SqlDbType.DateTime, loans_table.DateSubmit);
            }

            DbHelperSQL.ExecuteNonQuery(SqlCmd2);

            StageName = "Approve";

            string Sql3 = "update LoanStages set [Completed]=@DateApprove where [FileId]=" + iFileID + " and [StageName]='" + StageName + "'";

            SqlCommand SqlCmd3 = new SqlCommand(Sql3);

            if ((loans_table.DateApprove == null) || (loans_table.DateApprove == DateTime.MinValue))
            {
                DbHelperSQL.AddSqlParameter(SqlCmd3, "@DateApprove", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd3, "@DateApprove", SqlDbType.DateTime, loans_table.DateApprove);
            }

            DbHelperSQL.ExecuteNonQuery(SqlCmd3);

            StageName = "Clear To Close";

            string Sql4 = "update LoanStages set [Completed]=@DateClearToClose where [FileId]=" + iFileID + " and [StageName]='" + StageName + "'";

            SqlCommand SqlCmd4 = new SqlCommand(Sql4);

            if ((loans_table.DateClearToClose == null) || (loans_table.DateClearToClose == DateTime.MinValue))
            {
                DbHelperSQL.AddSqlParameter(SqlCmd4, "@DateClearToClose", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd4, "@DateClearToClose", SqlDbType.DateTime, loans_table.DateClearToClose);
            }

            DbHelperSQL.ExecuteNonQuery(SqlCmd4);

            StageName = "Docs Drawn";

            string Sql5 = "update LoanStages set [Completed]=@DateDocs where [FileId]=" + iFileID + " and [StageName]='" + StageName + "'";

            SqlCommand SqlCmd5 = new SqlCommand(Sql5);

            if ((loans_table.DateDocs == null) || (loans_table.DateDocs == DateTime.MinValue))
            {
                DbHelperSQL.AddSqlParameter(SqlCmd5, "@DateDocs", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd5, "@DateDocs", SqlDbType.DateTime, loans_table.DateDocs);
            }

            DbHelperSQL.ExecuteNonQuery(SqlCmd5);

            StageName = "Fund";

            string Sql6 = "update LoanStages set [Completed]=@DateFund where [FileId]=" + iFileID + " and [StageName]='" + StageName + "'";

            SqlCommand SqlCmd6 = new SqlCommand(Sql6);

            if ((loans_table.DateFund == null) || (loans_table.DateFund == DateTime.MinValue))
            {
                DbHelperSQL.AddSqlParameter(SqlCmd6, "@DateFund", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd6, "@DateFund", SqlDbType.DateTime, loans_table.DateFund);
            }

            DbHelperSQL.ExecuteNonQuery(SqlCmd6);

            StageName = "Record";

            string Sql7 = "update LoanStages set [Completed]=@DateRecord where [FileId]=" + iFileID + " and [StageName]='" + StageName + "'";

            SqlCommand SqlCmd7 = new SqlCommand(Sql7);

            if ((loans_table.DateRecord == null) || (loans_table.DateRecord == DateTime.MinValue))
            {
                DbHelperSQL.AddSqlParameter(SqlCmd7, "@DateRecord", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd7, "@DateRecord", SqlDbType.DateTime, loans_table.DateRecord);
            }

            DbHelperSQL.ExecuteNonQuery(SqlCmd7);

            StageName = "Close";

            string Sql8 = "update LoanStages set [Completed]=@DateClose where [FileId]=" + iFileID + " and [StageName]='" + StageName + "'";

            SqlCommand SqlCmd8 = new SqlCommand(Sql8);

            if ((loans_table.DateClose == null) || (loans_table.DateClose == DateTime.MinValue))
            {
                DbHelperSQL.AddSqlParameter(SqlCmd8, "@DateClose", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd8, "@DateClose", SqlDbType.DateTime, loans_table.DateClose);
            }

            DbHelperSQL.ExecuteNonQuery(SqlCmd8);

        }

        public void UpdateDataTracStatusDate_All(int iFileID, List<StatusDate> StatusDateList)
        {

            string Sql = "update Loans set [DateNote]=@DateNote, [DateResubmit]=@DateResubmit, [DateApprove]=@DateApprove, [DateClearToClose]=@DateClearToClose, " +
                         "[DateFund]=@DateFund, [DateSuspended]=@DateSuspended, [DateDenied]=@DateDenied, [DateCanceled]=@DateCanceled, " +
                         "[DateOpen]=@DateOpen, [DateSubmit]=@DateSubmit, [DateDocs]=@DateDocs, [DateRecord]=@DateRecord, " +
                         "[DateClose]=@DateClose, [DateHMDA]=@DateHMDA, [DateProcessing]=@DateProcessing, [DateDocsOut]=@DateDocsOut, " +
                         "[DateDocsReceived]=@DateDocsReceived, [Status]=@Status where FileId=" + iFileID;


            SqlCommand SqlCmd = new SqlCommand(Sql);

            DbHelperSQL.AddSqlParameter(SqlCmd, "@DateHMDA", SqlDbType.DateTime, DBNull.Value);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@DateProcessing", SqlDbType.DateTime, DBNull.Value);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@DateDocsOut", SqlDbType.DateTime, DBNull.Value);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@DateDocsReceived", SqlDbType.DateTime, DBNull.Value);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@DateRecord", SqlDbType.DateTime, DBNull.Value);
            DbHelperSQL.AddSqlParameter(SqlCmd, "@DateClose", SqlDbType.DateTime, DBNull.Value);

            Nullable<DateTime> DateApprove = new Nullable<DateTime>();
            Nullable<DateTime> DateClearToClose = new Nullable<DateTime>();
            Nullable<DateTime> DateOpen = new Nullable<DateTime>();
            Nullable<DateTime> DateSubmit = new Nullable<DateTime>();
            Nullable<DateTime> DateDocs = new Nullable<DateTime>();

            DateApprove = null;
            DateClearToClose = null;
            DateOpen = null;
            DateSubmit = null;
            DateDocs = null;

            string Status = "Processing";

            foreach (StatusDate StatusDateItem in StatusDateList)
            {

                if (StatusDateItem.StatusName == "Note")
                {
                    if (StatusDateItem.CompletionDate == null)
                    {
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@DateNote", SqlDbType.DateTime, DBNull.Value);
                    }
                    else
                    {
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@DateNote", SqlDbType.DateTime, StatusDateItem.CompletionDate);
                    }
                }
                else if (StatusDateItem.StatusName == "Re-Submit")
                {
                    if (StatusDateItem.CompletionDate == null)
                    {
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@DateResubmit", SqlDbType.DateTime, DBNull.Value);
                    }
                    else
                    {
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@DateResubmit", SqlDbType.DateTime, StatusDateItem.CompletionDate);
                    }
                }
                else if (StatusDateItem.StatusName == "Approve")
                {
                    if (StatusDateItem.CompletionDate == null)
                    {
                        DateApprove = null;
                        //DbHelperSQL.AddSqlParameter(SqlCmd, "@DateApprove", SqlDbType.DateTime, DBNull.Value);
                    }
                    else
                    {
                        //DbHelperSQL.AddSqlParameter(SqlCmd, "@DateApprove", SqlDbType.DateTime, StatusDateItem.CompletionDate);                        
                        DateOpen = StatusDateItem.CompletionDate;
                        DateSubmit = StatusDateItem.CompletionDate;
                        DateApprove = StatusDateItem.CompletionDate;
                    }
                }
                else if (StatusDateItem.StatusName == "Clear to Close")
                {
                    if (StatusDateItem.CompletionDate == null)
                    {
                        DateClearToClose = null;
                        //DbHelperSQL.AddSqlParameter(SqlCmd, "@DateClearToClose", SqlDbType.DateTime, DBNull.Value);
                    }
                    else
                    {
                        //DbHelperSQL.AddSqlParameter(SqlCmd, "@DateClearToClose", SqlDbType.DateTime, StatusDateItem.CompletionDate);
                        if (DateOpen == null)
                            DateOpen = StatusDateItem.CompletionDate;
                        if (DateSubmit == null)
                            DateSubmit = StatusDateItem.CompletionDate;
                        if (DateApprove == null)
                            DateApprove = StatusDateItem.CompletionDate;
                        DateClearToClose = StatusDateItem.CompletionDate;
                    }
                }
                else if (StatusDateItem.StatusName == "Fund")
                {
                    if (StatusDateItem.CompletionDate == null)
                    {
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@DateFund", SqlDbType.DateTime, DBNull.Value);
                    }
                    else
                    {
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@DateFund", SqlDbType.DateTime, StatusDateItem.CompletionDate);
                        if (DateOpen == null)
                            DateOpen = StatusDateItem.CompletionDate;
                        if (DateSubmit == null)
                            DateSubmit = StatusDateItem.CompletionDate;
                        if (DateApprove == null)
                            DateApprove = StatusDateItem.CompletionDate;
                        if (DateClearToClose == null)
                            DateClearToClose = StatusDateItem.CompletionDate;
                        if (DateDocs == null)
                            DateDocs = StatusDateItem.CompletionDate;
                    }
                }
                else if (StatusDateItem.StatusName == "Suspended")
                {
                    if (StatusDateItem.CompletionDate == null)
                    {
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@DateSuspended", SqlDbType.DateTime, DBNull.Value);
                    }
                    else
                    {
                        Status = "Suspended";
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@DateSuspended", SqlDbType.DateTime, StatusDateItem.CompletionDate);
                    }
                }
                else if (StatusDateItem.StatusName == "Denied")
                {
                    if (StatusDateItem.CompletionDate == null)
                    {
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@DateDenied", SqlDbType.DateTime, DBNull.Value);
                    }
                    else
                    {
                        Status = "Denied";
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@DateDenied", SqlDbType.DateTime, StatusDateItem.CompletionDate);
                    }
                }
                else if (StatusDateItem.StatusName == "Canceled")
                {
                    if (StatusDateItem.CompletionDate == null)
                    {
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@DateCanceled", SqlDbType.DateTime, DBNull.Value);
                    }
                    else
                    {
                        Status = "Canceled";
                        DbHelperSQL.AddSqlParameter(SqlCmd, "@DateCanceled", SqlDbType.DateTime, StatusDateItem.CompletionDate);
                    }
                }

            }

            DbHelperSQL.AddSqlParameter(SqlCmd, "@Status", SqlDbType.NVarChar, Status);

            if (DateApprove == null)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@DateApprove", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@DateApprove", SqlDbType.DateTime, DateOpen);
            }

            if (DateClearToClose == null)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@DateClearToClose", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@DateClearToClose", SqlDbType.DateTime, DateOpen);
            }

            if (DateOpen == null)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@DateOpen", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@DateOpen", SqlDbType.DateTime, DateOpen);
            }

            if (DateSubmit == null)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@DateSubmit", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@DateSubmit", SqlDbType.DateTime, DateOpen);
            }

            if (DateDocs == null)
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@DateDocs", SqlDbType.DateTime, DBNull.Value);
            }
            else
            {
                DbHelperSQL.AddSqlParameter(SqlCmd, "@DateDocs", SqlDbType.DateTime, DateOpen);
            }

            DbHelperSQL.ExecuteNonQuery(SqlCmd);

            string Sql2 = "delete LoanStages where FileId=" + iFileID;


            SqlCommand SqlCmd2 = new SqlCommand(Sql2);
            DbHelperSQL.ExecuteNonQuery(SqlCmd2);
        }

        /// <summary>
        /// get loan number from FileId
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>

        public string GetLoanNumber(int fileId)
        {
            DataTable dtLoanNumbers = this.GetLoanNumberInfo(new int[] { fileId });
            string[] arrLoanNumbers = dtLoanNumbers.AsEnumerable().Select(item => item.Field<string>(1)).ToArray();
            if (arrLoanNumbers.Length > 0)
                return arrLoanNumbers[0];
            else
                return null;
        }

        /// <summary>
        /// get loan numbers from FileIds
        /// </summary>
        /// <param name="fileIds"></param>
        /// <returns></returns>
        public DataTable GetLoanNumberInfo(int[] fileIds)
        {
            StringBuilder sbFileIds = new StringBuilder();
            foreach (int n in fileIds)
            {
                if (sbFileIds.Length > 0)
                    sbFileIds.Append(",");
                sbFileIds.Append(n);
            }

            string strSql = "";
            if (sbFileIds.Length > 0)
            {
                strSql = string.Format("SELECT FileId, LoanNumber FROM Loans WHERE FileId IN ({0})", sbFileIds.ToString());
                DataTable dtLoanNumber = DbHelperSQL.ExecuteDataTable(strSql);
                return dtLoanNumber;
            }
            else
                return null;
        }

        /// <summary>
        /// get loan number from loan status
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public DataTable GetLoanNumberInfo(string strStatus)
        {
            string strSql = "SELECT FileId, LoanNumber, DT_FileId FROM Loans WHERE ";
            if (string.IsNullOrEmpty(strStatus))
            {
                strSql += " Status <>'Prospect' AND (DT_FileId IS NOT NULL) ";
            }
            else
            {
                strSql += string.Format(" Status='{0}' AND (DT_FileID IS NOT NULL) ", strStatus.ToString());
            }
            DataTable dtLoanNumber = DbHelperSQL.ExecuteDataTable(strSql);
            return dtLoanNumber;
        }

        /// <summary>
        /// get current value of point file field for specified loan
        /// </summary>
        /// <param name="nFileId"></param>
        /// <param name="nFieldId"></param>
        /// <returns></returns>
        public string GetLoanFieldValue(int nFileId, int nFieldId)
        {
            string strSql = string.Format("SELECT TOP 1 CurrentValue FROM LoanPointFields WHERE FileId={0} AND PointFieldId={1}", nFileId, nFieldId);
            Object objRt = DbHelperSQL.ExecuteScalar(strSql);
            if (objRt == null)
                return string.Empty;

            return string.Format("{0}", objRt);
        }

        #endregion

        public List<Table.Template_Workflow> GetTemplateWorkflow()
        {
            var templateWorkflow = new List<Table.Template_Workflow>();
            string SQLString = @"SELECT [WflTemplId]
                                          ,[Name]
                                          ,[Enabled]
                                          ,[Desc]
                                          ,[WorkflowType]
                                          ,[Custom]
                                          ,[Default]
                                          ,[CalculationMethod]
                                      FROM [dbo].[Template_Workflow]";
            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                Table.Template_Workflow workflow = null;
                while (dataReader.Read())
                {
                    workflow = new Table.Template_Workflow();
                    workflow.WflTemplId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    workflow.Name = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1);
                    workflow.Enabled = dataReader.IsDBNull(2) ? false : dataReader.GetBoolean(2);
                    workflow.Desc = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3);
                    workflow.WorkflowType = dataReader.IsDBNull(4) ? string.Empty : dataReader.GetString(4);
                    workflow.Custom = dataReader.IsDBNull(5) ? false : dataReader.GetBoolean(5);
                    workflow.Default = dataReader.IsDBNull(6) ? false : dataReader.GetBoolean(6);
                    workflow.CalculationMethod = dataReader.IsDBNull(7) ? 0 : dataReader.GetInt32(7);
                    templateWorkflow.Add(workflow);
                }
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return templateWorkflow;
        }
        public int GetDefaultWorkflowTemplate(int FileId)
        {
            string err = string.Empty;
            string LoanStatus = string.Empty;
            string Sqlcmd = string.Empty;
            int DefWflTemplId = 0;
            int LoanWflTemplId = 0;

            try
            {
                // If there are already tasks, don't generate the default workflow
                //Sqlcmd = string.Format("Select count(1) from LoanTasks where FileId='{0}'", FileId);
                //object obj = DbHelperSQL.GetSingle(Sqlcmd);
                //if (obj != null && obj != DBNull.Value && (int)obj > 0)
                //    return DefWflTemplId;      
                Sqlcmd = string.Format("Select top 1 Status from Loans where FileId='{0}'", FileId);
                object obj = DbHelperSQL.GetSingle(Sqlcmd);
                if (obj == null || obj == DBNull.Value)
                    return DefWflTemplId;
                LoanStatus = (string)obj;
                if (LoanStatus != "Processing" && LoanStatus != "Prospect")
                    return DefWflTemplId;

                Sqlcmd = string.Format("Select top 1 WflTemplId from Template_Workflow where [Enabled]=1 and [Default]=1 and WorkflowType='{0}'", LoanStatus);
                object obj1 = DbHelperSQL.GetSingle(Sqlcmd);
                if (obj1 == null || obj1 == DBNull.Value)
                    return DefWflTemplId;

                DefWflTemplId = (int)obj1;

                // if the loan already has a Workflow Template applied, don't do it.
                Sqlcmd = string.Format("Select top 1 WflTemplId from LoanWflTempl lwt where FileId={0} and ApplyDate IS NOT NULL", FileId, LoanStatus);
                object obj2 = DbHelperSQL.GetSingle(Sqlcmd);
                LoanWflTemplId = (obj2 == null || obj2 == DBNull.Value) ? 0 : (int)obj2;
                if (LoanWflTemplId <= 0)
                    return DefWflTemplId;

                Sqlcmd = string.Format("Select top 1 WflTemplId from Template_Workflow where [Enabled]=1 and WflTemplid={0} and WorkflowType='{1}'", LoanWflTemplId, LoanStatus);
                obj = DbHelperSQL.GetSingle(Sqlcmd);
                if (obj == null || obj == DBNull.Value || (int)obj <= 0)
                    return DefWflTemplId;

                return 0;
            }
            catch (Exception ex)
            {
                err = string.Format("GetDefaultWorkflowTempalte, FileId {0}, LoanStatus {1}, Exception: ", FileId, LoanStatus, ex.Message + "\n" + ex.StackTrace);
                Trace.TraceError(err);
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                return DefWflTemplId;
            }
        }
        public List<Table.AutoCampaigns> GetAutoCampaigns()
        {
            var autoCampaigns = new List<Table.AutoCampaigns>();
            string SQLString = @"SELECT [CampaignId]
                                          ,[PaidBy]
                                          ,[Enabled]
                                          ,[SelectedBy]
                                          ,[Started]
                                          ,[LoanType]
                                          ,[TemplStageId]
                                      FROM [dbo].[AutoCampaigns]";
            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                Table.AutoCampaigns campaigns = null;
                while (dataReader.Read())
                {
                    campaigns = new Table.AutoCampaigns();
                    campaigns.CampaignId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    campaigns.PaidBy = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt16(1);
                    campaigns.Enabled = dataReader.IsDBNull(2) ? false : dataReader.GetBoolean(2);
                    campaigns.SelectedBy = dataReader.IsDBNull(3) ? 0 : dataReader.GetInt32(3);
                    campaigns.Started = dataReader.IsDBNull(4) ? DateTime.MinValue : dataReader.GetDateTime(4);
                    campaigns.LoanType = dataReader.IsDBNull(5) ? string.Empty : dataReader.GetString(5);
                    campaigns.TemplStageId = dataReader.IsDBNull(6) ? 0 : dataReader.GetInt32(6);

                    autoCampaigns.Add(campaigns);
                }
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return autoCampaigns;
        }
        public List<Table.TemplateEmail> GetTemplateEmail()
        {
            var templateEmails = new List<Table.TemplateEmail>();
            string SQLString = @"SELECT [TemplEmailId]
                                          ,[Enabled]
                                          ,[Name]
                                          ,[Desc]
                                          ,[FromUserRoles]
                                          ,[FromEmailAddress]
                                          ,[Content]
                                          ,[Subject]
                                          ,[Target]
                                          ,[Custom]
                                          ,[SendTrigger]
                                      FROM [dbo].[Template_Email] WHERE SendTrigger='LeadCreated' AND [Enabled]=1";
            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                Table.TemplateEmail templateEmail = null;
                while (dataReader.Read())
                {
                    templateEmail = new Table.TemplateEmail();
                    templateEmail.TemplEmailId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                    templateEmail.Enabled = dataReader.IsDBNull(1) ? false : dataReader.GetBoolean(1);
                    templateEmail.Name = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
                    templateEmail.Desc = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3);
                    templateEmail.FromUserRoles = dataReader.IsDBNull(4) ? 0 : dataReader.GetInt32(4);
                    templateEmail.FromEmailAddress = dataReader.IsDBNull(5) ? string.Empty : dataReader.GetString(5);
                    templateEmail.Content = dataReader.IsDBNull(6) ? string.Empty : dataReader.GetString(6);
                    templateEmail.Subject = dataReader.IsDBNull(7) ? string.Empty : dataReader.GetString(7);
                    templateEmail.Target = dataReader.IsDBNull(8) ? string.Empty : dataReader.GetString(8);
                    templateEmail.Custom = dataReader.IsDBNull(9) ? false : dataReader.GetBoolean(9);
                    templateEmail.SendTrigger = dataReader.IsDBNull(10) ? string.Empty : dataReader.GetString(10);

                    templateEmails.Add(templateEmail);
                }
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return templateEmails;
        }


        public int GetBorrowerContactID(int fieldID)
        {
            string err = "";
            int contactId = 0;
            bool logErr = false;
            string sqlCmd = string.Format("select dbo.lpfn_GetBorrowerContactId({0})", fieldID);
            try
            {
                object obj = DbHelperSQL.GetSingle(sqlCmd);
                contactId = obj == null ? 0 : Convert.ToInt32(obj);
                return contactId;
            }
            catch (Exception ex)
            {
                err = "GetBorrowerContactID, Exception: " + ex.Message;
                logErr = true;
                return contactId;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }
        public int Sync_Loan_Table(int FileId, ref string err)
        {
            err = "";
            int ErrCnt = 0;

            try
            {
                string sqlCmd1 = string.Format("select top 1 * from lpvw_PipelineInfo where FileId={0}", FileId);

                DataSet ds1_view = DbHelperSQL.Query(sqlCmd1);
                if (ds1_view == null || ds1_view.Tables[0] == null || ds1_view.Tables.Count <= 0 || ds1_view.Tables[0].Rows.Count <= 0)
                    return 0;
                DataRow dr = ds1_view.Tables[0].Rows[0];
                string FieldList = string.Format(" [Stage]='{0}', [RateLockIcon]='{1}', [AlertIcon]='{2}', RuleAlertIcon='{3}', Amount='{4}', Lien='{5}'," +
                                    " Lender='{6}', [Lock Expiration Date]='{7}', Rate='{8}', Progress='{9}', [Task Count]='{10}'," +
                                    " [Status]='{11}', LenderId='{12}', CurrentStage='{13}', LastCompletedStage='{14}', Program='{15}', LoanProgram='{16}', Purpose='{17}' ",
                                    dr["Stage"] == DBNull.Value ? string.Empty : dr["Stage"].ToString(),                                           // 0
                                    dr["RateLockIcon"] == DBNull.Value ? string.Empty : dr["RateLockIcon"].ToString(), // 1
                                    dr["AlertIcon"] == DBNull.Value ? string.Empty : dr["AlertIcon"].ToString(),  // 2
                                    dr["RuleAlertIcon"] == DBNull.Value ? string.Empty : dr["RuleAlertIcon"].ToString(), // 3
                                    dr["Amount"] == DBNull.Value ? string.Empty : dr["Amount"].ToString(), // 4
                                    dr["Lien"] == DBNull.Value ? string.Empty : dr["Lien"].ToString(), // 5
                                    dr["Lender"] == DBNull.Value ? string.Empty : dr["Lender"].ToString(), // 6
                                    dr["Lock Expiration Date"] == DBNull.Value ? string.Empty : dr["Lock Expiration Date"].ToString(), // 7
                                    dr["Rate"] == DBNull.Value ? string.Empty : dr["Rate"].ToString(),  // 8
                                    dr["Progress"] == DBNull.Value ? string.Empty : dr["Progress"].ToString(),  // 9
                                    dr["Task Count"] == DBNull.Value ? string.Empty : dr["Task Count"].ToString(), // 10
                                    dr["Status"] == DBNull.Value ? string.Empty : dr["Status"].ToString(), // 11
                                    dr["LenderId"] == DBNull.Value ? string.Empty : dr["LenderId"].ToString(),  // 12
                                    dr["CurrentStage"] == DBNull.Value ? string.Empty : dr["CurrentStage"].ToString(),  // 13
                                    dr["LastCompletedStage"] == DBNull.Value ? string.Empty : dr["LastCompletedStage"].ToString(),  // 14
                                    dr["Program"] == DBNull.Value ? string.Empty : dr["Program"].ToString(),  // 15
                                    dr["LoanProgram"] == DBNull.Value ? string.Empty : dr["LoanProgram"].ToString(),  // 16
                                    dr["Purpose"] == DBNull.Value ? string.Empty : dr["Purpose"].ToString());  // 17

                sqlCmd1 = string.Format("Update V_ProcessingPipelineInfo Set {0} where FileId={1}", FieldList, FileId);
                ErrCnt = DbHelperSQL.ExecuteSql(sqlCmd1);
                return ErrCnt;
            }
            catch (Exception ex)
            {
                ErrCnt = -1;
                err = ex.Message;
                return ErrCnt;
            }
        }
        public int Sync_Loan_Table(ref string err)
        {
            err = "";
            int ErrCnt = 0;

            try
            {

                string sqlCmd1 = string.Format("select [FileId], [Status], [BranchID], [Borrower], [Stage], [EstClose], [RateLockicon], " +
                    "[ImportErrorIcon], [AlertIcon], [RuleAlertIcon], [Loan Officer], [Amount], [Lien], [Rate], [Lender], " +
                    "[Lock Expiration Date], [Branch], [Progress], [Processor], [Task Count], [Point Folder], [Filename], [ProspectLoanStatus], [Program] " +
                    " from lpvw_PipelineInfo where Status<>'Prospect' ");

                DataSet ds1_view = DbHelperSQL.Query(sqlCmd1);
                foreach (DataRow dr1_view in ds1_view.Tables[0].Rows)
                {
                    int view1_FileId = 0;
                    if (dr1_view["FileId"] != DBNull.Value)
                        view1_FileId = (int)dr1_view["FileId"];

                    string view1_Status = string.Empty;
                    if (dr1_view["Status"] != DBNull.Value)
                        view1_Status = (string)dr1_view["Status"];

                    int view1_BranchID = 0;
                    if (dr1_view["BranchID"] != DBNull.Value)
                        view1_BranchID = (int)dr1_view["BranchID"];

                    string view1_Borrower = string.Empty;
                    if (dr1_view["Borrower"] != DBNull.Value)
                        view1_Borrower = (string)dr1_view["Borrower"];

                    string view1_Stage = string.Empty;
                    if (dr1_view["Stage"] != DBNull.Value)
                        view1_Stage = (string)dr1_view["Stage"];

                    Nullable<DateTime> view1_EstClose = null;
                    if (dr1_view["EstClose"] != DBNull.Value)
                        view1_EstClose = (DateTime)dr1_view["EstClose"];

                    string view1_RateLockicon = string.Empty;
                    if (dr1_view["RateLockicon"] != DBNull.Value)
                        view1_RateLockicon = (string)dr1_view["RateLockicon"];

                    string view1_ImportErrorIcon = string.Empty;
                    if (dr1_view["ImportErrorIcon"] != DBNull.Value)
                        view1_ImportErrorIcon = (string)dr1_view["ImportErrorIcon"];

                    string view1_AlertIcon = string.Empty;
                    if (dr1_view["AlertIcon"] != DBNull.Value)
                        view1_AlertIcon = (string)dr1_view["AlertIcon"];

                    string view1_RuleAlertIcon = string.Empty;
                    if (dr1_view["RuleAlertIcon"] != DBNull.Value)
                        view1_RuleAlertIcon = (string)dr1_view["RuleAlertIcon"];

                    string view1_LoanOfficer = string.Empty;
                    if (dr1_view["Loan Officer"] != DBNull.Value)
                        view1_LoanOfficer = (string)dr1_view["Loan Officer"];

                    Decimal view1_Amount = 0.0m;
                    if (dr1_view["Amount"] != DBNull.Value)
                        view1_Amount = (Decimal)dr1_view["Amount"];

                    string view1_Lien = string.Empty;
                    if (dr1_view["Lien"] != DBNull.Value)
                        view1_Lien = (string)dr1_view["Lien"];

                    Decimal view1_Rate = 0.0m;
                    if (dr1_view["Rate"] != DBNull.Value)
                        view1_Rate = (Decimal)dr1_view["Rate"];

                    string view1_Lender = string.Empty;
                    if (dr1_view["Lender"] != DBNull.Value)
                        view1_Lender = (string)dr1_view["Lender"];

                    Nullable<DateTime> view1_LockExpirationDate = null;
                    if (dr1_view["Lock Expiration Date"] != DBNull.Value)
                        view1_LockExpirationDate = (DateTime)dr1_view["Lock Expiration Date"];

                    string view1_Branch = string.Empty;
                    if (dr1_view["Branch"] != DBNull.Value)
                        view1_Branch = (string)dr1_view["Branch"];

                    Decimal view1_Progress = 0.0m;
                    if (dr1_view["Progress"] != DBNull.Value)
                        view1_Progress = (Decimal)dr1_view["Progress"];

                    string view1_Processor = string.Empty;
                    if (dr1_view["Processor"] != DBNull.Value)
                        view1_Processor = (string)dr1_view["Processor"];

                    int view1_TaskCount = 0;
                    if (dr1_view["Task Count"] != DBNull.Value)
                        view1_TaskCount = (int)dr1_view["Task Count"];

                    string view1_PointFolder = string.Empty;
                    if (dr1_view["Point Folder"] != DBNull.Value)
                        view1_PointFolder = (string)dr1_view["Point Folder"];

                    string view1_Filename = string.Empty;
                    if (dr1_view["Filename"] != DBNull.Value)
                        view1_Filename = (string)dr1_view["Filename"];

                    string view1_ProspectLoanStatus = string.Empty;
                    if (dr1_view["ProspectLoanStatus"] != DBNull.Value)
                        view1_ProspectLoanStatus = (string)dr1_view["ProspectLoanStatus"];

                    string view1_Program = string.Empty;
                    if (dr1_view["Program"] != DBNull.Value)
                        view1_ProspectLoanStatus = (string)dr1_view["Program"];

                    string sqlCmd2 = string.Format("select [FileId], [Status], [BranchID], [Borrower], [Stage], [EstClose], [RateLockicon], " +
                    "[ImportErrorIcon], [AlertIcon], [RuleAlertIcon], [Loan Officer], [Amount], [Lien], [Rate], [Lender], " +
                    "[Lock Expiration Date], [Branch], [Progress], [Processor], [Task Count], [Point Folder], [Filename], [ProspectLoanStatus] " +
                    " from V_ProcessingPipelineInfo where [FileId]='{0}' ", view1_FileId);

                    DataSet ds1_table = DbHelperSQL.Query(sqlCmd2);

                    if ((ds1_table == null) || (ds1_table.Tables.Count == 0) || (ds1_table.Tables[0].Rows.Count == 0))
                    {
                        if (view1_Rate > 99)
                            view1_Rate = 0;

                        string Sql3 = string.Format("Insert into V_ProcessingPipelineInfo ( [FileId], [Status], [BranchID], [Borrower], [Stage], [EstClose], " +
                        "[RateLockicon], [ImportErrorIcon], [AlertIcon], [RuleAlertIcon], [Loan Officer], [Amount], [Lien], [Rate], [Lender], " +
                        "[Lock Expiration Date], [Branch], [Progress], [Processor], [Task Count], [Point Folder], [Filename], [ProspectLoanStatus], [Program] ) " +
                        "Values ('{0}', '{1}', '{2}', @Borrower, '{3}', @EstClose, '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', @LockExpirationDate, '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}' )",
                        view1_FileId, view1_Status, view1_BranchID, view1_Stage, view1_RateLockicon,
                        view1_ImportErrorIcon, view1_AlertIcon, view1_RuleAlertIcon, view1_LoanOfficer, view1_Amount, view1_Lien, view1_Rate, view1_Lender,
                        view1_Branch, view1_Progress, view1_Processor, view1_TaskCount, view1_PointFolder, view1_Filename, view1_ProspectLoanStatus, view1_Program);

                        SqlCommand SqlCmd3 = new SqlCommand(Sql3);

                        if (view1_Borrower == null)
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd3, "@Borrower", SqlDbType.NVarChar, DBNull.Value);
                        }
                        else
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd3, "@Borrower", SqlDbType.NVarChar, view1_Borrower);
                        }

                        if (view1_EstClose == null)
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd3, "@EstClose", SqlDbType.DateTime, DBNull.Value);
                        }
                        else
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd3, "@EstClose", SqlDbType.DateTime, view1_EstClose);
                        }

                        if (view1_LockExpirationDate == null)
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd3, "@LockExpirationDate", SqlDbType.DateTime, DBNull.Value);
                        }
                        else
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd3, "@LockExpirationDate", SqlDbType.DateTime, view1_LockExpirationDate);
                        }

                        DbHelperSQL.ExecuteNonQuery(SqlCmd3);
                        continue;
                    }

                    DataRow dr1_table = ds1_table.Tables[0].Rows[0];

                    int table1_FileId = 0;
                    if (dr1_table["FileId"] != DBNull.Value)
                        table1_FileId = (int)dr1_table["FileId"];

                    string table1_Status = string.Empty;
                    if (dr1_table["Status"] != DBNull.Value)
                        table1_Status = (string)dr1_table["Status"];

                    if (table1_Status != view1_Status)
                    {
                        string sqlCmd4 = string.Format("Update V_ProcessingPipelineInfo set [Status]='{0}' Where [FileId]='{1}'", view1_Status, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd4);
                    }

                    int table1_BranchID = 0;
                    if (dr1_table["BranchID"] != DBNull.Value)
                        table1_BranchID = (int)dr1_table["BranchID"];

                    if (table1_BranchID != view1_BranchID)
                    {
                        string sqlCmd5 = string.Format("Update V_ProcessingPipelineInfo set [BranchID]='{0}' Where [FileId]='{1}'", view1_BranchID, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd5);
                    }

                    string table1_Borrower = string.Empty;
                    if (dr1_table["Borrower"] != DBNull.Value)
                        table1_Borrower = (string)dr1_table["Borrower"];

                    if (table1_Borrower != view1_Borrower)
                    {
                        string Sql6 = string.Format("Update V_ProcessingPipelineInfo set [Borrower]=@Borrower Where [FileId]='{0}'", view1_FileId);

                        SqlCommand SqlCmd6 = new SqlCommand(Sql6);

                        if (view1_Borrower == null)
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd6, "@Borrower", SqlDbType.NVarChar, DBNull.Value);
                        }
                        else
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd6, "@Borrower", SqlDbType.NVarChar, view1_Borrower);
                        }

                        DbHelperSQL.ExecuteNonQuery(SqlCmd6);
                    }

                    string table1_Stage = string.Empty;
                    if (dr1_table["Stage"] != DBNull.Value)
                        table1_Stage = (string)dr1_table["Stage"];

                    if (table1_Stage != view1_Stage)
                    {
                        string sqlCmd7 = string.Format("Update V_ProcessingPipelineInfo set [Stage]='{0}' Where [FileId]='{1}'", view1_Stage, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd7);
                    }

                    Nullable<DateTime> table1_EstClose = null;
                    if (dr1_table["EstClose"] != DBNull.Value)
                        table1_EstClose = (DateTime)dr1_table["EstClose"];

                    if (table1_EstClose != view1_EstClose)
                    {
                        string Sql8 = string.Format("Update V_ProcessingPipelineInfo set EstClose=@EstClose Where [FileId]='{0}'", view1_FileId);

                        SqlCommand SqlCmd8 = new SqlCommand(Sql8);

                        if (view1_EstClose == null)
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd8, "@EstClose", SqlDbType.DateTime, DBNull.Value);
                        }
                        else
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd8, "@EstClose", SqlDbType.DateTime, view1_EstClose);
                        }

                        DbHelperSQL.ExecuteNonQuery(SqlCmd8);
                    }

                    string table1_RateLockicon = string.Empty;
                    if (dr1_table["RateLockicon"] != DBNull.Value)
                        table1_RateLockicon = (string)dr1_table["RateLockicon"];

                    if (table1_RateLockicon != view1_RateLockicon)
                    {
                        string sqlCmd9 = string.Format("Update V_ProcessingPipelineInfo set [RateLockicon]='{0}' Where [FileId]='{1}'", view1_RateLockicon, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd9);
                    }

                    string table1_ImportErrorIcon = string.Empty;
                    if (dr1_table["ImportErrorIcon"] != DBNull.Value)
                        table1_ImportErrorIcon = (string)dr1_table["ImportErrorIcon"];

                    if (table1_ImportErrorIcon != view1_ImportErrorIcon)
                    {
                        string sqlCmd10 = string.Format("Update V_ProcessingPipelineInfo set [ImportErrorIcon]='{0}' Where [FileId]='{1}'", view1_ImportErrorIcon, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd10);
                    }

                    string table1_AlertIcon = string.Empty;
                    if (dr1_table["AlertIcon"] != DBNull.Value)
                        table1_AlertIcon = (string)dr1_table["AlertIcon"];

                    if (table1_AlertIcon != view1_AlertIcon)
                    {
                        string sqlCmd11 = string.Format("Update V_ProcessingPipelineInfo set [AlertIcon]='{0}' Where [FileId]='{1}'", view1_AlertIcon, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd11);
                    }

                    string table1_RuleAlertIcon = string.Empty;
                    if (dr1_table["RuleAlertIcon"] != DBNull.Value)
                        table1_RuleAlertIcon = (string)dr1_table["RuleAlertIcon"];

                    if (table1_RuleAlertIcon != view1_RuleAlertIcon)
                    {
                        string sqlCmd12 = string.Format("Update V_ProcessingPipelineInfo set [RuleAlertIcon]='{0}' Where [FileId]='{1}'", view1_RuleAlertIcon, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd12);
                    }

                    string table1_LoanOfficer = string.Empty;
                    if (dr1_table["Loan Officer"] != DBNull.Value)
                        table1_LoanOfficer = (string)dr1_table["Loan Officer"];

                    if (table1_LoanOfficer != view1_LoanOfficer)
                    {
                        string sqlCmd13 = string.Format("Update V_ProcessingPipelineInfo set [Loan Officer]='{0}' Where [FileId]='{1}'", view1_LoanOfficer, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd13);
                    }

                    Decimal table1_Amount = 0.0m;
                    if (dr1_table["Amount"] != DBNull.Value)
                        table1_Amount = (Decimal)dr1_table["Amount"];

                    if (table1_Amount != view1_Amount)
                    {
                        string sqlCmd14 = string.Format("Update V_ProcessingPipelineInfo set [Amount]='{0}' Where [FileId]='{1}'", view1_Amount, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd14);
                    }

                    string table1_Lien = string.Empty;
                    if (dr1_table["Lien"] != DBNull.Value)
                        table1_Lien = (string)dr1_table["Lien"];

                    if (table1_Lien != view1_Lien)
                    {
                        string sqlCmd15 = string.Format("Update V_ProcessingPipelineInfo set [Lien]='{0}' Where [FileId]='{1}'", view1_Lien, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd15);
                    }

                    Decimal table1_Rate = 0.0m;
                    if (dr1_table["Rate"] != DBNull.Value)
                        table1_Rate = (Decimal)dr1_table["Rate"];

                    if (table1_Rate != view1_Rate)
                    {
                        if (view1_Rate < 100)
                        {
                            string sqlCmd16 = string.Format("Update V_ProcessingPipelineInfo set [Rate]='{0}' Where [FileId]='{1}'", view1_Rate, view1_FileId);
                            DbHelperSQL.ExecuteSql(sqlCmd16);
                        }
                    }

                    string table1_Lender = string.Empty;
                    if (dr1_table["Lender"] != DBNull.Value)
                        table1_Lender = (string)dr1_table["Lender"];

                    if (table1_Lender != view1_Lender)
                    {
                        string sqlCmd17 = string.Format("Update V_ProcessingPipelineInfo set [Lender]='{0}' Where [FileId]='{1}'", view1_Lender, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd17);
                    }

                    Nullable<DateTime> table1_LockExpirationDate = null;
                    if (dr1_table["Lock Expiration Date"] != DBNull.Value)
                        table1_LockExpirationDate = (DateTime)dr1_table["Lock Expiration Date"];

                    if (table1_LockExpirationDate != view1_LockExpirationDate)
                    {
                        string Sql18 = string.Format("Update V_ProcessingPipelineInfo set [Lock Expiration Date]=@LockExpirationDate Where [FileId]='{0}'", view1_FileId);

                        SqlCommand SqlCmd18 = new SqlCommand(Sql18);

                        if (view1_LockExpirationDate == null)
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd18, "@LockExpirationDate", SqlDbType.DateTime, DBNull.Value);
                        }
                        else
                        {
                            DbHelperSQL.AddSqlParameter(SqlCmd18, "@LockExpirationDate", SqlDbType.DateTime, view1_LockExpirationDate);
                        }

                        DbHelperSQL.ExecuteNonQuery(SqlCmd18);
                    }

                    string table1_Branch = string.Empty;
                    if (dr1_table["Branch"] != DBNull.Value)
                        table1_Branch = (string)dr1_table["Branch"];

                    if (table1_Branch != view1_Branch)
                    {
                        string sqlCmd19 = string.Format("Update V_ProcessingPipelineInfo set [Branch]='{0}' Where [FileId]='{1}'", view1_Branch, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd19);
                    }

                    Decimal table1_Progress = 0.0m;
                    if (dr1_table["Progress"] != DBNull.Value)
                        table1_Progress = (Decimal)dr1_table["Progress"];

                    if (table1_Progress != view1_Progress)
                    {
                        string sqlCmd20 = string.Format("Update V_ProcessingPipelineInfo set [Progress]='{0}' Where [FileId]='{1}'", view1_Progress, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd20);
                    }

                    string table1_Processor = string.Empty;
                    if (dr1_table["Processor"] != DBNull.Value)
                        table1_Processor = (string)dr1_table["Processor"];

                    if (table1_Processor != view1_Processor)
                    {
                        string sqlCmd21 = string.Format("Update V_ProcessingPipelineInfo set [Processor]='{0}' Where [FileId]='{1}'", view1_Processor, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd21);
                    }

                    int table1_TaskCount = 0;
                    if (dr1_table["Task Count"] != DBNull.Value)
                        table1_TaskCount = (int)dr1_table["Task Count"];

                    if (table1_TaskCount != view1_TaskCount)
                    {
                        string sqlCmd22 = string.Format("Update V_ProcessingPipelineInfo set [Task Count]='{0}' Where [FileId]='{1}'", view1_TaskCount, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd22);
                    }

                    string table1_PointFolder = string.Empty;
                    if (dr1_table["Point Folder"] != DBNull.Value)
                        table1_PointFolder = (string)dr1_table["Point Folder"];

                    if (table1_PointFolder != view1_PointFolder)
                    {
                        string sqlCmd23 = string.Format("Update V_ProcessingPipelineInfo set [Point Folder]='{0}' Where [FileId]='{1}'", view1_PointFolder, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd23);
                    }

                    string table1_Filename = string.Empty;
                    if (dr1_table["Filename"] != DBNull.Value)
                        table1_Filename = (string)dr1_table["Filename"];

                    if (table1_Filename != view1_Filename)
                    {
                        string sqlCmd24 = string.Format("Update V_ProcessingPipelineInfo set [Filename]='{0}' Where [FileId]='{1}'", view1_Filename, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd24);
                    }

                    string table1_ProspectLoanStatus = string.Empty;
                    if (dr1_table["ProspectLoanStatus"] != DBNull.Value)
                        table1_ProspectLoanStatus = (string)dr1_table["ProspectLoanStatus"];

                    if (table1_ProspectLoanStatus != view1_ProspectLoanStatus)
                    {
                        string sqlCmd25 = string.Format("Update V_ProcessingPipelineInfo set [ProspectLoanStatus]='{0}' Where [FileId]='{1}'", view1_ProspectLoanStatus, view1_FileId);
                        DbHelperSQL.ExecuteSql(sqlCmd25);
                    }

                }

                string sqlCmd26 = string.Format("select [FileId], [Status], [BranchID], [Borrower], [Stage], [EstClose], [RateLockicon], " +
                                "[ImportErrorIcon], [AlertIcon], [RuleAlertIcon], [Loan Officer], [Amount], [Lien], [Rate], [Lender], " +
                                "[Lock Expiration Date], [Branch], [Progress], [Processor], [Task Count], [Point Folder], [Filename], [ProspectLoanStatus]" +
                                " from V_ProcessingPipelineInfo ");

                DataSet ds2_table = DbHelperSQL.Query(sqlCmd26);

                foreach (DataRow dr2_table in ds2_table.Tables[0].Rows)
                {
                    int table2_FileId = 0;
                    if (dr2_table["FileId"] != DBNull.Value)
                        table2_FileId = (int)dr2_table["FileId"];

                    string sqlCmd27 = string.Format("select [FileId], [Status], [BranchID], [Borrower], [Stage], [EstClose], [RateLockicon], " +
                    "[ImportErrorIcon], [AlertIcon], [RuleAlertIcon], [Loan Officer], [Amount], [Lien], [Rate], [Lender], " +
                    "[Lock Expiration Date], [Branch], [Progress], [Processor], [Task Count], [Point Folder], [Filename], [ProspectLoanStatus] " +
                    " from lpvw_PipelineInfo where [FileId]='{0}' ", table2_FileId);

                    DataSet ds2_view = DbHelperSQL.Query(sqlCmd27);

                    if ((ds2_view == null) || (ds2_view.Tables.Count == 0) || (ds2_view.Tables[0].Rows.Count == 0))
                    {
                        string sqlCmd9 = "Delete V_ProcessingPipelineInfo Where [FileId]=" + table2_FileId;
                        DbHelperSQL.ExecuteSql(sqlCmd9);
                        continue;
                    }
                }

                return ErrCnt;
            }
            catch (Exception ex)
            {
                ErrCnt = -1;
                err = ex.Message;
                return ErrCnt;
            }
        }

        public DataTable GetLoanBasicDocumentList(string sWhere)
        {
            string sSql = "select * from LoanBasicDocs where 1=1 " + sWhere;
            return DbHelperSQL.Query(sSql).Tables[0];
        }

        public DataTable GetMilestoneList(int iFileID)
        {
            string sSql = "select dbo.lpfn_GetStageAlias(LoanStageId) as StageAlias, * from LoanStages where FileId=" + iFileID + " order by SequenceNumber";
            //string sSql = "select * from LoanStages as a inner join Template_Wfl_Stages b on a.WflStageId=b.WflStageId inner join Template_Stages c on b.TemplStageId=c.TemplStageId where a.Fileid=" + iFileID + " order by a.SequenceNumber asc";
            return DbHelperSQL.Query(sSql).Tables[0];
        }

        public string GetLenderName(int iFileID)
        {
            string sSql = "select dbo.lpfn_GetLender(" + iFileID + ")";
            return DbHelperSQL.ExecuteScalar(sSql).ToString();
        }

        public List<Table.CompanyReport> GetCompanyReportInfo()
        {
            var companyReport = new List<Table.CompanyReport>();
            string SQLString = @"SELECT [DOW]
                                          ,[TOD]
                                          ,[SenderRoleId]
                                          ,[SenderEmail]
                                          ,[SenderName]
                                      FROM [dbo].[Company_Report]";
            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                Table.CompanyReport company = null;
                while (dataReader.Read())
                {
                    company = new Table.CompanyReport();
                    company.DOW = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt16(0);
                    company.TOD = dataReader.IsDBNull(1) ? 0 : dataReader.GetInt16(1);
                    company.SenderRoleId = dataReader.IsDBNull(2) ? 0 : dataReader.GetInt32(2);
                    company.SenderEmail = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3);
                    company.SenderName = dataReader.IsDBNull(4) ? string.Empty : dataReader.GetString(4);

                    companyReport.Add(company);
                }
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
            return companyReport;
        }

        public string GetSendRoleUserEmail(int SenderRoleID, int FileId)
        {
            string sSql = "select EmailAddress from Users where UserID=(select UserID from LoanTeam where RoleId=" + SenderRoleID + " and FileId=" + FileId + ")";
            object obj = DbHelperSQL.GetSingle(sSql);
            string sEmailAddress = "";
            if (obj != null)
            {
                sEmailAddress = Convert.ToString(obj);
            }
            return sEmailAddress;
        }

        public string GetSendRoleUserName(int SenderRoleID, int FileId)
        {
            string sSql = "select ISNULL(LastName,'') + ', ' + ISNULL(FirstName,'') from Users where UserID=(select UserID from LoanTeam where RoleId=" + SenderRoleID + " and FileId=" + FileId + ")";
            object obj = DbHelperSQL.GetSingle(sSql);
            string sEmailAddress = "";
            if (obj != null)
            {
                sEmailAddress = Convert.ToString(obj);
            }
            return sEmailAddress;
        }

        //public bool NeedToRegenerateWorkflow_OLD(int FileId, int WorkflowTemplId, out int existingWflTemplId)
        //{
        //    // first check to see if there is an existing workflow applied
        //    string sSql = string.Format("select WflTemplId from LoanWflTempl where FileId={0}", FileId);
        //    object obj = DbHelperSQL.GetSingle(sSql);
        //    // if not, the loan does not have a workflow yet, we'll generate new workflow
        //    existingWflTemplId = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
        //    int defWflTemplid = 0;
        //    defWflTemplid = GetDefaultWorkflowTemplate(FileId);

        //    if ((existingWflTemplId > 0 && existingWflTemplId == WorkflowTemplId) || existingWflTemplId <= 0)
        //    {
        //        sSql = string.Format("select Count(1) from LoanTasks where FileId={0}", FileId);
        //        obj = DbHelperSQL.GetSingle(sSql);
        //        int taskCount = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
        //        if (taskCount > 0)
        //            return false;
        //        if (defWflTemplid == WorkflowTemplId)
        //            return false;
        //        else
        //            return true;
        //    }
        //    // then check to see if the existing workflow and the new one are of the same workflow type "Processing" or "Prospect"
        //    sSql = string.Format("select WorkflowType from Template_Workflow where WflTemplId={0}", WorkflowTemplId);
        //    obj = DbHelperSQL.GetSingle(sSql);
        //    string newWorkflowType = (obj == null || obj == DBNull.Value) ? string.Empty : (string)obj;
        //    sSql = string.Format("select WorkflowType from Template_Workflow where WflTemplId={0}", existingWflTemplId);
        //    obj = DbHelperSQL.GetSingle(sSql);
        //    string existingWorkflowType = (obj == null || obj == DBNull.Value) ? string.Empty : (string)obj;
        //    // if they're not of the same type, we'll generate new workflow
        //    if (string.IsNullOrEmpty(existingWorkflowType) || string.IsNullOrEmpty(newWorkflowType)
        //        || (existingWorkflowType.Trim().ToUpper() != newWorkflowType.Trim().ToUpper()))
        //        return true;

        //    return false;
        //}

        public bool NeedToRegenerateWorkflow(int FileId, int WorkflowTemplId, out int existingWflTemplId)
        {
            // first check to see if there is an existing workflow applied
            string sSql = string.Format("select WflTemplId from LoanWflTempl where FileId={0}", FileId);
            object obj = DbHelperSQL.GetSingle(sSql);
            // if not, the loan does not have a workflow yet, we'll generate new workflow
            existingWflTemplId = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
            int defWflTemplid = 0;
            defWflTemplid = GetDefaultWorkflowTemplate(FileId);

            if ((existingWflTemplId > 0 && existingWflTemplId == WorkflowTemplId) || existingWflTemplId <= 0)
            {

                sSql = string.Format("select Count(1) from LoanTasks where FileId={0}", FileId);
                obj = DbHelperSQL.GetSingle(sSql);
                int taskCount = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
                if (taskCount > 0)
                    return false;
                sSql = string.Format("Select Count(1) from LoanStages where FileId={0}", FileId);
                obj = DbHelperSQL.GetSingle(sSql);
                int stageCount = (obj == null || obj == DBNull.Value) ? 0 : (int)obj;
                if (stageCount < 1)
                    return true;
                if (defWflTemplid == WorkflowTemplId)
                    return false;
                else
                    return true;
            }

            // then check to see if the existing workflow and the new one are of the same workflow type "Processing" or "Prospect"
            sSql = string.Format("select WorkflowType from Template_Workflow where WflTemplId={0}", WorkflowTemplId);
            obj = DbHelperSQL.GetSingle(sSql);
            string newWorkflowType = (obj == null || obj == DBNull.Value) ? string.Empty : (string)obj;
            sSql = string.Format("select WorkflowType from Template_Workflow where WflTemplId={0}", existingWflTemplId);
            obj = DbHelperSQL.GetSingle(sSql);
            string existingWorkflowType = (obj == null || obj == DBNull.Value) ? string.Empty : (string)obj;
            // if they're not of the same type, we'll generate new workflow
            if (string.IsNullOrEmpty(existingWorkflowType) || string.IsNullOrEmpty(newWorkflowType)
            || (existingWorkflowType.Trim().ToUpper() != newWorkflowType.Trim().ToUpper()))
                return true;

            return false;
        }

        #region GetImportantMessages
        public DataSet GetImportantMessagesPreview(int FileId, bool ExternalViewing, out string err)
        {
            var ds = new DataSet();
            err = string.Empty;
            try
            {
                string sql = "SELECT Note, Created from LoanNotes where FileId=" + FileId;
                if (ExternalViewing)
                    sql += " AND ExternalViewing=1 ";
                sql += " Order by Created desc ";
                ds = DbHelperSQL.Query(sql);
                return ds;
            }
            catch (Exception ex)
            {
                err = "Exception:" + ex.ToString();
                Trace.TraceError(err);
                return ds;
            }
        }
        public DataSet GetImportantMessages(int loanAutoEmailid, out string err)
        {
            var ds = new DataSet();
            err = string.Empty;
            try
            {
                SqlParameter[] parameters = {
                                                new SqlParameter("@LoanAutoEmailid", SqlDbType.Int)
                                            };
                parameters[0].Value = loanAutoEmailid;
                ds = DbHelperSQL.RunProcedure("[lpsp_GetDisplayImportantMessages]", parameters, "ds");
                return ds;
            }
            catch (Exception ex)
            {
                err = "Exception:" + ex.Message;
                Trace.TraceError(err);
                return ds;
            }
        }

        #endregion

        public DataSet GetPointFieldIdAndValue(int fieldId, int ruleCondId)
        {
            var pendgingRuleInfo = new List<Table.PendingRuleInfo>();
            string SQLString = @"select * from LoanPointFields lpf 
	where lpf.fileId = {0} 
	and PointFieldId=(select top 1 pointfieldid from Template_RuleConditions tr where tr.rulecondid={1})
";
            SQLString = string.Format(SQLString, fieldId, ruleCondId);

            return DbHelperSQL.Query(SQLString);

        }

        public int GetConditionValue(int fileId, int ruleCondId)
        {
            string sSql = string.Format("SELECT [dbo].[lpfn_GetConditionValue]({0},{1})", fileId, ruleCondId);
            object obj = DbHelperSQL.ExecuteScalar(sSql);
            if (obj != null)
                return Convert.ToInt32(obj);

            return 0;
        }

        public DataTable GetUserInfo(int iUserID)
        {
            string sSql = "select * from Users where UserId=" + iUserID;
            return DbHelperSQL.ExecuteDataTable(sSql);
        }

        public Record.LoanLocks GetLoanLock(int fileId, ref string err)
        {
            err = string.Empty;
            Record.LoanLocks loanLock = null;
            bool logErr = false;
            if (fileId <= 0)
            {
                err = string.Format("GetLoanLock, FileId {0} is invalid.", fileId);
                return loanLock;
            }

            string sqlCmd = string.Format("select top 1 * from dbo.LoanLocks where FileId={0}", fileId);
            DataSet ds = null;
            try
            {
                ds = DbHelperSQL.Query(sqlCmd);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                    return loanLock;
                DataRow dr = ds.Tables[0].Rows[0];
                loanLock = new Record.LoanLocks()
                {
                    FileId = dr["FileId"] == DBNull.Value || dr["FileId"] == null ? 0 : (int)dr["FileId"],
                    ConfirmedBy = dr["ConfirmedBy"] == DBNull.Value || dr["ConfirmedBy"] == null ? string.Empty : dr["ConfirmedBy"].ToString(),
                    ConfirmTime = dr["ConfirmTime"] == DBNull.Value || dr["ConfirmTime"] == null ? string.Empty : dr["ConfirmTime"].ToString(),

                    LockOption = dr["LockOption"] == DBNull.Value || dr["LockOption"] == null ? string.Empty : dr["LockOption"].ToString(),
                    LockedBy = dr["LockedBy"] == DBNull.Value || dr["LockedBy"] == null ? string.Empty : dr["LockedBy"].ToString(),
                    LockTime = dr["LockTime"] == DBNull.Value || dr["LockTime"] == null ? string.Empty : dr["LockTime"].ToString(),
                    LockTerm = dr["LockTerm"] == DBNull.Value || dr["LockTerm"] == null ? string.Empty : dr["LockTerm"].ToString(),
                    LockExpirationDate = dr["LockExpirationDate"] == DBNull.Value || dr["LockExpirationDate"] == null ? string.Empty : dr["LockExpirationDate"].ToString(),

                    Ext1ConfirmTime = dr["Ext1ConfirmTime"] == DBNull.Value || dr["Ext1ConfirmTime"] == null ? string.Empty : dr["Ext1ConfirmTime"].ToString(),
                    Ext1LockTime = dr["Ext1LockTime"] == DBNull.Value || dr["Ext1LockTime"] == null ? string.Empty : dr["Ext1LockTime"].ToString(),
                    Ext1LockedBy = dr["Ext1LockedBy"] == DBNull.Value || dr["Ext1LockedBy"] == null ? string.Empty : dr["Ext1LockedBy"].ToString(),
                    Ext1LockExpDate = dr["Ext1LockExpDate"] == DBNull.Value || dr["Ext1LockExpDate"] == null ? string.Empty : dr["Ext1LockExpDate"].ToString(),
                    Ext1Term = dr["Ext1Term"] == DBNull.Value || dr["Ext1Term"] == null ? string.Empty : dr["Ext1Term"].ToString(),

                    Ext2ConfirmTime = dr["Ext2ConfirmTime"] == DBNull.Value || dr["Ext2ConfirmTime"] == null ? string.Empty : dr["Ext2ConfirmTime"].ToString(),
                    Ext2LockTime = dr["Ext2LockTime"] == DBNull.Value || dr["Ext2LockTime"] == null ? string.Empty : dr["Ext2LockTime"].ToString(),
                    Ext2LockedBy = dr["Ext2LockedBy"] == DBNull.Value || dr["Ext2LockedBy"] == null ? string.Empty : dr["Ext2LockedBy"].ToString(),
                    Ext2LockExpDate = dr["Ext2LockExpDate"] == DBNull.Value || dr["Ext2LockExpDate"] == null ? string.Empty : dr["Ext2LockExpDate"].ToString(),
                    Ext2Term = dr["Ext2Term"] == DBNull.Value || dr["Ext2Term"] == null ? string.Empty : dr["Ext2Term"].ToString(),

                    Ext3ConfirmTime = dr["Ext3ConfirmTime"] == DBNull.Value || dr["Ext3ConfirmTime"] == null ? string.Empty : dr["Ext3ConfirmTime"].ToString(),
                    Ext3LockTime = dr["Ext3LockTime"] == DBNull.Value || dr["Ext3LockTime"] == null ? string.Empty : dr["Ext3LockTime"].ToString(),
                    Ext3LockedBy = dr["Ext3LockedBy"] == DBNull.Value || dr["Ext3LockedBy"] == null ? string.Empty : dr["Ext3LockedBy"].ToString(),
                    Ext3LockExpDate = dr["Ext3LockExpDate"] == DBNull.Value || dr["Ext3LockExpDate"] == null ? string.Empty : dr["Ext3LockExpDate"].ToString(),
                    Ext3Term = dr["Ext3Term"] == DBNull.Value || dr["Ext3Term"] == null ? string.Empty : dr["Ext3Term"].ToString()
                };

                return loanLock;
            }
            catch (Exception ex)
            {
                err = "GetLoanLock, Exception: " + ex.Message;
                logErr = true;
                return loanLock;
            }
            finally
            {
                if (logErr)
                {
                    Trace.TraceError(err);
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning);
                }
            }
        }
        public void SaveLoanLocks_Profit(int fileId, Record.LoanLocks loanLocks, Record.LoanProfit loanProfit, ref string err)
        {
            #region LoanLocks
            if (loanLocks == null)
            {
                return;
            }
            var sSql = @"IF EXISTS ( SELECT  FileId
                                    FROM    dbo.LoanLocks
                                    WHERE   FileId = @FileId ) 
                            BEGIN
		                        UPDATE dbo.LoanLocks SET
				                            LockedBy =@LockedBy, 
                                            LockTime =@LockTime,
                                            LockTerm =@LockTerm,
                                            LockExpirationDate =@LockExpirationDate,
                                            Ext1LockExpDate =@Ext1LockExpDate,
                                            Ext1LockTime =@Ext1LockTime,
                                            Ext2LockExpDate =@Ext2LockExpDate,
                                            Ext2LockTime =@Ext2LockTime
                                WHERE      FileId = @FileId
                            END
                        ELSE 
                            BEGIN
                                INSERT  INTO dbo.LoanLocks
                                        ( FileId ,
                                          LockedBy ,
                                          LockTime ,
                                          LockTerm ,
                                          LockExpirationDate ,
                                          Ext1LockExpDate ,
                                          Ext1LockTime ,
                                          Ext2LockExpDate ,
                                          Ext2LockTime
                                        )
                                VALUES  ( @FileId ,
                                          @LockedBy ,
                                          @LockTime ,
                                          @LockTerm ,
                                          @LockExpirationDate ,
                                          @Ext1LockExpDate ,
                                          @Ext1LockTime ,
                                          @Ext2LockExpDate ,
                                          @Ext2LockTime
                                        )
                            END";

            SqlParameter[] parameters = {
                                            new SqlParameter("@LockedBy",SqlDbType.NVarChar,255),
                                            new SqlParameter("@LockTime",SqlDbType.DateTime),
                                            new SqlParameter("@LockTerm",SqlDbType.SmallInt),
                                            new SqlParameter("@LockExpirationDate",SqlDbType.Date),
                                            new SqlParameter("@Ext1LockExpDate",SqlDbType.Date),
                                            new SqlParameter("@Ext1LockTime",SqlDbType.DateTime),
                                            new SqlParameter("@Ext2LockExpDate",SqlDbType.Date),
                                            new SqlParameter("@Ext2LockTime",SqlDbType.DateTime),
                                            new SqlParameter("@FileId",SqlDbType.Int)
                                            };
            if (string.IsNullOrEmpty(loanLocks.LockedBy))
            {
                parameters[0].Value = DBNull.Value;
            }
            else
            {
                parameters[0].Value = loanLocks.LockedBy;
            }

            if (string.IsNullOrEmpty(loanLocks.LockTime))
            {
                parameters[1].Value = DBNull.Value;
            }
            else
            {
                DateTime dateTime;
                if (DateTime.TryParse(loanLocks.LockTime, out dateTime))
                    parameters[1].Value = dateTime;
            }
            if (string.IsNullOrEmpty(loanLocks.LockTerm))
            {
                parameters[2].Value = DBNull.Value;
            }
            else
            {
                Int16 iLockTerm;
                if (Int16.TryParse(loanLocks.LockTerm, out iLockTerm))
                    parameters[2].Value = iLockTerm;
            }
            if (string.IsNullOrEmpty(loanLocks.LockExpirationDate))
            {
                parameters[3].Value = DBNull.Value;
            }
            else
            {
                DateTime dateTime;
                if (DateTime.TryParse(loanLocks.LockExpirationDate, out dateTime))
                    parameters[3].Value = dateTime;
            }
            if (string.IsNullOrEmpty(loanLocks.Ext1LockExpDate))
            {
                parameters[4].Value = DBNull.Value;
            }
            else
            {
                DateTime dateTime;
                if (DateTime.TryParse(loanLocks.Ext1LockExpDate, out dateTime))
                    parameters[4].Value = dateTime;
            }
            if (string.IsNullOrEmpty(loanLocks.Ext1LockTime))
            {
                parameters[5].Value = DBNull.Value;
            }
            else
            {
                DateTime dateTime;
                if (DateTime.TryParse(loanLocks.Ext1LockTime, out dateTime))
                    parameters[5].Value = dateTime;
            }
            if (string.IsNullOrEmpty(loanLocks.Ext2LockExpDate))
            {
                parameters[6].Value = DBNull.Value;
            }
            else
            {
                DateTime dateTime;
                if (DateTime.TryParse(loanLocks.Ext2LockExpDate, out dateTime))
                    parameters[6].Value = dateTime;
            }
            if (string.IsNullOrEmpty(loanLocks.Ext2LockTime))
            {
                parameters[7].Value = DBNull.Value;
            }
            else
            {
                DateTime dateTime;
                if (DateTime.TryParse(loanLocks.Ext2LockTime, out dateTime))
                    parameters[7].Value = dateTime;
            }

            parameters[8].Value = fileId;
            #endregion
            try
            {
                DbHelperSQL.ExecuteSql(sSql, parameters);
                #region loan profit
                if (loanProfit == null)
                    return;
                sSql = @"IF EXISTS ( SELECT  FileId
                                    FROM    dbo.LoanProfit
                                    WHERE   FileId = @FileId ) 
                            BEGIN
		                        UPDATE dbo.LoanProfit SET
				                            LenderCredit =@LenderCredit 
                                WHERE      FileId = @FileId
                            END
                        ELSE 
                            BEGIN
                                INSERT  INTO dbo.LoanProfit
                                        ( FileId ,
                                          LenderCredit
                                        )
                                VALUES  ( @FileId ,
                                          @LenderCredit
                                        )
                            END";
                SqlParameter[] param1 = {
                                            new SqlParameter("@FileId",SqlDbType.Int),
                                            new SqlParameter("@LenderCredit",SqlDbType.Decimal)
                                            
                                        };
                param1[0].Value = fileId;
                decimal tempDecimal = 0;

                if (string.IsNullOrEmpty(loanProfit.LenderCredit))
                {
                    param1[1].Value = DBNull.Value;
                }
                else
                {
                    if (decimal.TryParse(loanProfit.LenderCredit, out tempDecimal))
                        param1[1].Value = tempDecimal;
                }
                DbHelperSQL.ExecuteSql(sSql, param1);
                #endregion

            }
            catch (Exception ex)
            {
                err = "Exception:" + ex.Message;
                Trace.TraceError(err);
            }
        }

        public void GetCompanyMCT(ref Table.Company_MCT company_MCT)
        {
            string SQLString = @"SELECT TOP 1  ClientID ,
                                                PostURL ,
                                                PostDataEnabled ,
                                                ActiveLoanInterval ,
                                                ArchivedLoanInterval ,
                                                ArchivedLoanDisposeMonth ,
                                                ArchivedLoanStatuses FROM dbo.Company_MCT";
            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                while (dataReader.Read())
                {
                    if (company_MCT == null)
                        company_MCT = new Table.Company_MCT();
                    company_MCT.ClientID = dataReader.IsDBNull(0) ? string.Empty : dataReader.GetString(0);
                    company_MCT.PostURL = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1);
                    company_MCT.PostDataEnabled = !dataReader.IsDBNull(2) && dataReader.GetBoolean(2);
                    company_MCT.ActiveLoanInterval = dataReader.IsDBNull(3) ? 0 : dataReader.GetInt32(3);
                    company_MCT.ArchivedLoanInterval = dataReader.IsDBNull(4) ? 0 : dataReader.GetInt32(4);
                    company_MCT.ArchivedLoanDisposeMonth = dataReader.IsDBNull(5) ? 0 : dataReader.GetInt32(5);
                    company_MCT.ArchivedLoanStatuses = dataReader.IsDBNull(6) ? string.Empty : dataReader.GetString(6);
                }
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }
        }

        public DataSet GetPostActiveLoan()
        {
            string SQLString = @"[lpsp_GetActiveLoan]";
            return DbHelperSQL.Query(SQLString);
        }

        public DataSet GetPostArchivedLoan()
        {
            string SQLString = @"[lpsp_GetArchivedLoan]";
            return DbHelperSQL.Query(SQLString);
        }

        public Dictionary<string, byte[]> GetEmailAttachments(int emailTemplId)
        {
            var attachments = new Dictionary<string, byte[]>();
            string SQLString = string.Format(@"SELECT [TemplAttachId]
                                  ,[TemplEmailId]
                                  ,[Enabled]
                                  ,[Name]
                                  ,[FileType]
                                  ,[FileImage]
                              FROM [dbo].[Template_Email_Attachments] 
                              WHERE TemplEmailId ={0}  and [Enabled] = 1", emailTemplId);
            SqlDataReader dataReader = null;
            try
            {
                dataReader = DbHelperSQL.ExecuteReader(SQLString);
                while (dataReader.Read())
                {
                    var name = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetString(3);
                    var type = dataReader.IsDBNull(4) ? string.Empty : dataReader.GetString(4);
                    var data = (dataReader.IsDBNull(5) ? null : dataReader[5]) as byte[];
                    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type) || data == null || data.Length == 0)
                    {
                        continue;
                    }
                    var fileName = string.Format("{0}.{1}", name, type);
                    if (attachments.ContainsKey(fileName))
                    {
                        continue;
                    }
                    attachments.Add(fileName, data);
                }
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
            }

            return attachments;
        }

        public void SaveEmailLogAttachment(int emailLogId, int fileId, string name, string ext, byte[] data)
        {
            string err = string.Empty;
            var sql = @"INSERT INTO [dbo].[EmailLog_Attachments]
                                    ([EmailLogId]
                                    ,[FileId]
                                    ,[Name]
                                    ,[FileType]
                                    ,[FileImage])
                                VALUES
                                    (@EmailLogId
                                    ,@FileId
                                    ,@Name
                                    ,@FileType
                                    ,@FileImage)";
            SqlParameter[] parameters = {                    
                    new SqlParameter("@EmailLogId", SqlDbType.Int, 4 ),                               //0
                    new SqlParameter("@FileId", SqlDbType.Int, 4),                           //1
                    new SqlParameter("@Name", SqlDbType.NVarChar, 255),               //2
                    new SqlParameter("@FileType", SqlDbType.NVarChar, 255),               //3
                    new SqlParameter("@FileImage", SqlDbType.VarBinary, int.MaxValue )     //4
                   };

            parameters[0].Value = emailLogId;
            parameters[1].Value = fileId;

            if (string.IsNullOrEmpty(name))
            {
                parameters[2].Value = DBNull.Value;
            }
            else
            {
                parameters[2].Value = name;
            }
            if (string.IsNullOrEmpty(ext))
            {
                parameters[3].Value = DBNull.Value;
            }
            else
            {
                parameters[3].Value = ext;
            }
            if (data != null)
            {
                parameters[4].Value = data;
            }
            else
            {
                parameters[4].Value = DBNull.Value;
            }
            try
            {
                DbHelperSQL.ExecuteSql(sql, parameters);
            }
            catch (Exception ex)
            {
                err = "Exception:" + ex.Message;
                Trace.TraceError(err);
            }
        }
    }
}

