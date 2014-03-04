using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPWeb.BLL
{
    public class RuleManager
    {
        #region RuleAPI

        ///// <summary>
        ///// Acknowledges the alert.
        ///// </summary>
        ///// <param name="currentLoanAlertId">The current loan alert id.</param>
        ///// <param name="userId">The user id.</param>
        ///// <param name="name">The name.</param>
        ///// <returns></returns>
        //public static bool AcknowledgeAlert(int currentLoanAlertId, int userId, string name)
        //{
        //    return DAL.RuleManager.AcknowledgeAlert(currentLoanAlertId, userId,name,string.Empty);
        //}


        /// <summary>
        /// Accepts the alert.
        /// </summary>
        /// <param name="loanAlertId">The loan alert id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static bool AcceptAlert(int loanAlertId, int userId, string name)
        {
            return DAL.RuleManager.AcceptAlert(loanAlertId, userId, name);
        }


        /// <summary>
        /// Declines the alert.
        /// </summary>
        /// <param name="loanAlertId">The loan alert id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static bool DeclineAlert(int loanAlertId, int userId, string name)
        {
            return DAL.RuleManager.DeclineAlert(loanAlertId, userId, name);
        }


        /// <summary>
        /// Dismisses the alert.
        /// </summary>
        /// <param name="currentLoanAlertId">The current loan alert id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static bool DismissAlert(int currentLoanAlertId, int userId)
        {
            return DAL.RuleManager.DismissAlert(currentLoanAlertId, userId);
        }

        #endregion
    }
}
