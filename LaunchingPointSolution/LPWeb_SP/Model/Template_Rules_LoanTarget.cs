using System;
namespace LPWeb.Model
{
    [Serializable]
    public class Template_Rules_LoanTarget
    {
        public Template_Rules_LoanTarget()
        {
            this.ActiveLoans = this.ActiveLeads = this.ArchivedLoans = this.ArchivedLeads = false;
        }

        public Template_Rules_LoanTarget(Int16 iLoanTargetValue)
            : this()
        {
            if (iLoanTargetValue == _constOldProcessing) { this.ActiveLoans = true; }
            else if (iLoanTargetValue == _constOldProspect) { this.ActiveLeads = true; }
            else if (iLoanTargetValue == _constOldProcessingAndProspect) { this.ActiveLoans = this.ActiveLeads = true; }
            else
            {
                if ((iLoanTargetValue & _constActiveLoans) == _constActiveLoans) { this.ActiveLoans = true; }
                if ((iLoanTargetValue & _constActiveLeads) == _constActiveLeads) { this.ActiveLeads = true; }
                if ((iLoanTargetValue & _constArchivedLoans) == _constArchivedLoans) { this.ArchivedLoans = true; }
                if ((iLoanTargetValue & _constArchivedLeads) == _constArchivedLeads) { this.ArchivedLeads = true; }
            }
        }

        #region Model

        public static readonly Int16 _constOldProcessing = 0;
        public static readonly Int16 _constOldProspect = 1;
        public static readonly Int16 _constOldProcessingAndProspect = 2;
        public static readonly Int16 _constActiveLoans = Convert.ToInt16("10001", 2);
        public static readonly Int16 _constActiveLeads = Convert.ToInt16("10010", 2);
        public static readonly Int16 _constArchivedLoans = Convert.ToInt16("10100", 2);
        public static readonly Int16 _constArchivedLeads = Convert.ToInt16("11000", 2);

        public bool ActiveLoans { get; set; }
        public bool ActiveLeads { get; set; }
        public bool ArchivedLoans { get; set; }
        public bool ArchivedLeads { get; set; }

        public Int16 LoanTargetValue
        {
            get
            {
                Int16 returnValue = 0;

                if (this.ActiveLoans) { returnValue = (Int16)(returnValue | _constActiveLoans); }
                if (this.ActiveLeads) { returnValue = (Int16)(returnValue | _constActiveLeads); }
                if (this.ArchivedLoans) { returnValue = (Int16)(returnValue | _constArchivedLoans); }
                if (this.ArchivedLeads) { returnValue = (Int16)(returnValue | _constArchivedLeads); }

                return returnValue;
            }
        }

        #endregion Model
    }
}
