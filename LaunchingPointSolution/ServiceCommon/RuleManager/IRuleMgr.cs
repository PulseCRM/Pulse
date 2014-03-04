namespace RuleManager
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRuleMgr
    {
        /// <summary>
        /// Processes the loan rules.
        /// </summary>
        void ProcessLoanRules();

        /// <summary>
        /// Acknowledges the alert.
        /// </summary>
        /// <param name="currentLoanAlertId">The current loan alert id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        bool AcknowledgeAlert(int currentLoanAlertId, int userId);
    }
}