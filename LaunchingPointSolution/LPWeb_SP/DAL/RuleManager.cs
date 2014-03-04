using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LPWeb.DAL
{
    /// <summary>
    /// The Workflow Manager
    /// </summary>
    public partial class RuleManager
    {
        #region RuleAPI

        ///// <summary>
        ///// Acknowledges the alert.
        ///// </summary>
        ///// <param name="currentLoanAlertId">The current loan alert id.</param>
        ///// <param name="userId">The user id.</param>
        ///// <param name="name">The name.</param>
        ///// <param name="emailContent">Content of the email.</param>
        ///// <returns></returns>
        //public static bool AcknowledgeAlert(int currentLoanAlertId, int userId, string name, string emailContent)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("lpsp_AcknowledgeAlert");

        //    SqlParameter[] parameters = {
        //            new SqlParameter("@CurrentLoanAlertId", SqlDbType.Int,4),
        //            new SqlParameter("@UserId", SqlDbType.Int,4),
        //            new SqlParameter("@Name", SqlDbType.NVarChar,100)
        //                                };
        //    parameters[0].Value = currentLoanAlertId;
        //    parameters[1].Value = userId;
        //    parameters[2].Value = name;

        //    int rowsAffected = 0;

        //    int returnValue = -1;
        //    returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

        //    return returnValue == 0;
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
            return AcceptOrDeclineAlert(loanAlertId, userId, name, true);
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
            return AcceptOrDeclineAlert(loanAlertId, userId, name, false);
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_DismissAlert");

            SqlParameter[] parameters = {
					new SqlParameter("@CurrentAlertId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4)
                    //,new SqlParameter("@Name", SqlDbType.NVarChar,100)
                                        };
            parameters[0].Value = currentLoanAlertId;
            parameters[1].Value = userId;
            //parameters[2].Value = name;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }

        /// <summary>
        /// Accepts the or decline alert.
        /// </summary>
        /// <param name="loanAlertId">The loan alert id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="name">The name.</param>
        /// <param name="type">if set to <c>true</c> [type].</param>
        /// <returns></returns>
        private static bool AcceptOrDeclineAlert(int loanAlertId, int userId, string name, bool type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("lpsp_AcceptOrDeclineAlert");

            SqlParameter[] parameters = {
					new SqlParameter("@LoanAlertId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
                    new SqlParameter("@AlertType", SqlDbType.Bit)
                                        };
            parameters[0].Value = loanAlertId;
            parameters[1].Value = userId;
            parameters[2].Value = name;
            parameters[3].Value = type;

            int rowsAffected = 0;

            int returnValue = -1;
            returnValue = DbHelperSQL.RunProcedure(strSql.ToString(), parameters, out rowsAffected);

            return returnValue == 0;
        }
        #endregion
    }
}
