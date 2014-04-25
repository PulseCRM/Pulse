using System;
namespace LPWeb.Model
{
    [Serializable]
    public class Template_Rules_LoanTarget
    {
        public Template_Rules_LoanTarget() { }
        public Template_Rules_LoanTarget(Int16 iLoanTargetValue)
        {
            if (iLoanTargetValue == this._constOldProcessing) { this.ActiveLoans = true; }
            else if (iLoanTargetValue == this._constOldProspect) { this.ActiveLeads = true; }
            else if (iLoanTargetValue == this._constOldProcessingAndProspect) { this.ActiveLoans = this.ActiveLeads = true; }
            else
            {
                if ((iLoanTargetValue & this._constActiveLoans) == this._constActiveLoans) { this.ActiveLoans = true; }
                if ((iLoanTargetValue & this._constActiveLeads) == this._constActiveLeads) { this.ActiveLeads = true; }
                if ((iLoanTargetValue & this._constArchivedLoans) == this._constArchivedLoans) { this.ArchivedLoans = true; }
                if ((iLoanTargetValue & this._constArchivedLeads) == this._constArchivedLeads) { this.ArchivedLeads = true; }
            }
        }

        #region Model

        private readonly Int16 _constOldProcessing = 0;
        private readonly Int16 _constOldProspect = 1;
        private readonly Int16 _constOldProcessingAndProspect = 2;
        private readonly Int16 _constActiveLoans = Convert.ToInt16("10001", 2);
        private readonly Int16 _constActiveLeads = Convert.ToInt16("10010", 2);
        private readonly Int16 _constArchivedLoans = Convert.ToInt16("10100", 2);
        private readonly Int16 _constArchivedLeads = Convert.ToInt16("11000", 2);

        public bool ActiveLoans { get; set; }
        public bool ActiveLeads { get; set; }
        public bool ArchivedLoans { get; set; }
        public bool ArchivedLeads { get; set; }

        public Int16 LoanTargetValue
        {
            get
            {
                Int16 returnValue = 0;

                if (this.ActiveLoans) { returnValue = (Int16)(returnValue | this._constActiveLoans); }
                if (this.ActiveLeads) { returnValue = (Int16)(returnValue | this._constActiveLeads); }
                if (this.ArchivedLoans) { returnValue = (Int16)(returnValue | this._constArchivedLoans); }
                if (this.ArchivedLeads) { returnValue = (Int16)(returnValue | this._constArchivedLeads); }

                return returnValue;
            }
        }

        #endregion Model
    }
}
