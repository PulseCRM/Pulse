using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using LP2.Service.Common;

namespace DataTracManager
{
    public interface IDataTracMrg
    {
        bool Login(string Username, string Password);

        List<Table.Loans> GetLoanInfoList(string LoanStatus, ref string err);

        List<Table.Loans> GetLoanInfoList(int[] FileIds, ref string err);

        Table.Loans GetLoanInfo (int FileId, ref string err);

        List<StatusDate> GetDates(int FileId, ref string err);

        RateLock GetRateLock(int FileId, ref string err);

        Table.Contacts GetBorrowerInfo(int iFileId, ref string err);

        Table.Contacts GetCoBorrowerInfo(int iFileId, ref string err);

        List<DT_LoanProgram> GetLoanPrograms(ref string err);

        bool SubmitLoan(int FieldId, string orig_type, string loan_program, ref string err);

        bool UpdateHMDAFields(int FileId, ref string err);

        bool UpdateLoanContact(int FileId, string Role, ref string err);
    }
}
