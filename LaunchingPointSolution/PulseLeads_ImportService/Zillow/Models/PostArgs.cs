namespace PulseLeads.Zillow.Models
{
    public class PostArgs
    {
        public int LoanId { get; set; }
        public int LoanOfficerId { get; set; }
        public int ContactId { get; set; }
        public int BranchId { get; set; }
        public int CoBorrowerContactId { get; set; }
    }
}