using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Text;
using System.Threading;
using LP2.Service.ADHelper;
using LP2.Service.Common;
using DataAccess;
using Utilities;
using System.Diagnostics;

namespace LP2.Service
{
    public class UserManager : IUserManager
    {
        #region UserManager Properties
        short Category = 40;
        static ActiveDirectoryHelper m_adHelper = null;
        static UserManager m_Instance;
        static DataAccess.DataAccess m_dataAccess = null;
        SingleThreadedContext m_ThreadContext = null;
        string m_OU_Filter = "";
        static int m_refCount;

        private UserManager()
        {
                if (m_adHelper == null)
                    m_adHelper = new ActiveDirectoryHelper();
                m_OU_Filter = m_adHelper.OU;

                if (m_dataAccess == null)
                    m_dataAccess = new DataAccess.DataAccess();
                if (m_ThreadContext == null)
                {
                    m_ThreadContext = new SingleThreadedContext();
                    m_ThreadContext.ExceptionEvent += new ExceptionEventHandler(ThreadExceptionHandler);
                    m_ThreadContext.Init("User Manager");
                }
               
                if (m_Instance != null)
                    return;
        }

        public static UserManager Instance
        {
            get 
            {
                if (m_Instance == null)
                {
                   m_Instance = new UserManager();
                }
                lock (m_Instance)
                {
                    m_refCount++;
                }

                return m_Instance;
            }
        }
        public void Dispose()
        {
            lock (m_Instance)
            {
                m_refCount--;
            }
            if (m_refCount <= 0)
            {
                m_adHelper.Close();
                m_ThreadContext.Exit();
                m_dataAccess = null;
                m_Instance = null;
            }
        }
        #endregion
        #region UserManager Methods

        public void ThreadExceptionHandler(object sender, ExceptionEventArgs args)
        {
            int Event_id = 6010;
            EventLog.WriteEntry(InfoHubEventLog.LogSource, args.Exception.Message, EventLogEntryType.Warning, Event_id, Category);             
         }

        private bool CheckData(User user, ref string err)
        {
            err = "";
            if (user == null)
            {
                err = "User account info is empty, NULL";
                return false;
            }
            if (user == null)
            {
                err = "User account info is empty, NULL";
                return false;
            }
            user.Lastname = user.Lastname.Trim();
            user.Firstname = user.Firstname.Trim();
            user.Email = user.Email.Trim();
            user.Username = user.Username.Trim();
            if (user.Username == String.Empty)
                err += "Username is empty.\n";
            if (user.Firstname == String.Empty)
                err += "User First Name is empty.\n";
            if (user.Lastname == String.Empty)
                err += "User Last Name is empty.\n";
            //if (user.Email == String.Empty)
            //    err += "User Email Address is empty.\n";
            if (err.Length > 0)
                return false;
            return true;
        }

        public String OU_Filter
        {
            get {return m_OU_Filter;}
            set {
                lock (m_Instance)
                {
                    m_OU_Filter = value;
                }
            }
        }

        private bool SetUpADHelper(ref string err)
        {
            err = "";
            try 
            {
                lock (m_Instance)
                {
                    if (m_adHelper == null)
                        m_adHelper = new ActiveDirectoryHelper();
                    if (OU_Filter.Trim().Length <= 0)
                    {
                        OU_Filter = m_adHelper.OU;
                        if (OU_Filter == null)
                            OU_Filter = "";
                    }
                    if (OU_Filter.Trim() != m_adHelper.OU)
                        m_adHelper.OU = OU_Filter.Trim();
                }
            } catch (Exception ex)
            {
                err = "SetUpADHelper, exception: " + ex.Message;
                int Event_id = 6011;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);             
            }
            return true;
        }

        private void UpdateRequest(int requestor, int reqId, UserMgrCommandType cmd, bool success, string errMsg, ref string err)
        {
            m_dataAccess.UpdateRequestQueue(requestor, cmd.ToString(), ref reqId, success, errMsg, ref err);
        }

        private void ProcessRequest(object o)
        {
            bool logErr = false;
            bool fatal = false;
            bool status = false;
            string err = "";
            ADUserDetail adUser = null;
            UserManagerEvent e = o as UserManagerEvent;
            if (e == null)
            {
                err = "ProcessRequest, UserManagerEvent argument is empty, OU " + m_OU_Filter;
                int Event_id = 6012;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);            
            }

            if (e.UserId <= 0)
                e.UserId = 0;

            if (e.RequestType == Common.UserMgrCommandType.Unknown)
            {
                err = "UserManagerEvent RequestType is Unknown, ReqType=" + e.RequestType;
                int Event_id = 6014;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);            
             }
            if (e.RequestId <= 0)
            {
                err = "UserManagerEvent RequestId is invalid, RequestId=" + e.RequestId.ToString();
                int Event_id = 6015;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);            
            }
            try
            {
                if ((e.RequestType != Common.UserMgrCommandType.ImportUsers) &&
                    (e.RequestType != Common.UserMgrCommandType.StartImport) &&
                    (e.RequestType != Common.UserMgrCommandType.StopImport))
                {
                    if (e.ADUser == null)
                    {
                        err = "Received invalid UserManagerEvent RequestType, ReqType=" + e.RequestType + ", OU=" + m_OU_Filter;
                        int Event_id = 6016;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);            
                    }

                    adUser = new ADUserDetail(e.ADUser.Username, !e.ADUser.Enabled, e.ADUser.Firstname, e.ADUser.Lastname, e.ADUser.Email, e.ADUser.Password);
                }
                if (SetUpADHelper(ref err) == false)
                {
                    int Event_id = 6017;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);            
                }
                switch (e.RequestType)
                {
                    case Common.UserMgrCommandType.CreateUser:
                        status = m_adHelper.AddUserByLogin(adUser, false, ref err);
                        break;
                    case Common.UserMgrCommandType.DeleteUser:
                        status = m_adHelper.DeleteUserByLogin(adUser, ref err);
                        break;
                    case Common.UserMgrCommandType.DisableUser:
                        status = m_adHelper.DisableUserByLogin(adUser, ref err);
                        break;
                    case Common.UserMgrCommandType.EnableUser:
                        status = m_adHelper.EnableUserByLogin(adUser, ref err);
                        break;
                    case Common.UserMgrCommandType.ImportUsers:
                        status = ImportADUsers(e.UserId, e.RequestId, ref err);
                        break;
                    case Common.UserMgrCommandType.StartImport:
                        //status = StartImport();
                        break;
                    case Common.UserMgrCommandType.StopImport:
                        //status = StopImport();
                        break;
                }
                if (status == false)
                {
                    int Event_id = 6018;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);            
                }
             }
            catch (Exception ex)
            {
                err = "Failed to  " + e.RequestType.ToString() + ", OU " + m_OU_Filter + ", Exception:" + ex.Message;
                int Event_id = 6019;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);            
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 6020;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);            
                }
                string errMsg = "";
                UpdateRequest(e.UserId, e.RequestId, e.RequestType, status, err, ref errMsg);                
            }
        }

        public bool ChangeUserPassword(string login, string pwd, int requestor, int reqId, ref string err)
        {
            err = "";
            bool logErr = false;
            bool fatal = false;
            try
            {
                if (String.IsNullOrEmpty(login))
                {
                    err = "ChangeUserPassword, Username is empty, OU=" + m_OU_Filter;
                    int Event_id = 6021;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);            
                    return false;
                }

                if (String.IsNullOrEmpty(pwd))
                {
                    err = "ChangeUserPassword, Password is empty, OU=" + m_OU_Filter;
                    int Event_id = 6022;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);            
                    return false;
                }

                if (reqId <= 0)
                {
                    err = "ChangeUserPassword, RequestId must be greater than 0, reqId=" + reqId.ToString() + ", OU=" + m_OU_Filter;
                    int Event_id = 6023;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);            
                    return false;
                }
                if (requestor <= 0)  // if there is no user id of the requestor is not given
                    requestor = 0;

                User user = m_dataAccess.GetUserAccount(login, ref err);
                if (user == null)
                {
                    err = "ChangeUserPassword, err: " + err;
                    return false;
                }
                if ((user.Firstname == string.Empty) || (user.Lastname == string.Empty))
                {
                    err = "ChangeUserPassword, username=" + user.Username + " missing First Name or Last Name in the database.";
                    return false;
                }
                //if (user.Password == string.Empty)
                //{
                    user.Password = pwd;
                //}
                ADUserDetail adUser = new ADUserDetail(user.Username, !user.Enabled, user.Firstname, user.Lastname, user.Email, user.Password);
                bool status = m_adHelper.ChangeUserPassword(adUser, ref err);
                if (status == false)
                {
                    err += ", OU " + m_OU_Filter;
                    int Event_id = 6024;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);            
                }
                else
                    err = "";
                return status;
            }
            catch (Exception ex)
            {
                //UpdateRequest(requestor, reqId, UserMgrCommandType.ChangePassword, false, ex.Message, ref err);
                err = "Failed to ChangeUserPassword the user account, username=" + login + ", OU " + m_OU_Filter + ", Exception:" + ex.Message;
                int Event_id = 6025;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);            
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 6026;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                 
                }
            
            }
        }

        public bool UpdateUser(User user, int requestor,  int reqId, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (CheckData(user, ref err) == false)
                {
                    int Event_id = 6027;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                 
                    return false;
                }
                ADUserDetail aduser = new ADUserDetail(user.Username, !user.Enabled, user.Firstname, user.Lastname, user.Email, user.Password);

                bool status = m_adHelper.UpdateUserByLogin(aduser, false, ref err);
                if (status == true)
                {
                    err = "";
                }
                else
                {
                    err += ", OU " + m_OU_Filter;
                    int Event_id = 6028;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                 
                }
                return status;

            }
            catch (Exception ex)
            {
                err = "Failed to update the user account, username=" + user.Username + ", OU " + m_OU_Filter + ", Exception:" + ex.Message;
                int Event_id = 6029;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                 
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 6030;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                          
                }
            }
        }

        public bool CreateUser(User user, int requestor, int reqId, ref string err)
        {
            bool logErr = false;
            try
            {
                if (CheckData(user, ref err) == false)
                {
                    int Event_id = 6031;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                          
                    return false;
                }
                ADUserDetail aduser = new ADUserDetail(user.Username, !user.Enabled, user.Firstname, user.Lastname, user.Email, user.Password);
                bool status = m_adHelper.AddUserByLogin(aduser, false, ref err);
                if (status == true)
                    err = "";
                else
                {
                    err += " OU Filter=" + OU_Filter;
                    int Event_id = 6032;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                          
                }
                return status;
            }
            catch (Exception ex)
            {
                err = "Failed to create the user account, username=" + user.Username + ", OU " + m_OU_Filter + ", Exception:" + ex.Message;
                int Event_id = 6033;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                          
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 6034;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                   
                }
            }
        }

        public bool DisableUser(User user, int requestor, int reqId, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (CheckData(user, ref err) == false)
                {
                    int Event_id = 6035;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                         
                    return false;
                }

                ADUserDetail aduser = new ADUserDetail(user.Username, !user.Enabled, user.Firstname, user.Lastname, user.Email);
                bool status = m_adHelper.DisableUserByLogin(aduser, ref err);
                if (status == true)
                    err = "";
                else
                {
                    err += ", OU "+m_OU_Filter;
                    int Event_id = 6036;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                         
                }
                return status;
            }
            catch (Exception ex)
            {
                err = "Failed to disable the user account, username=" + user.Username + ", OU " + m_OU_Filter+", Exception:" + ex.Message;
                int Event_id = 6037;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                         
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 6038;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                         
                }
            }
        }

        public bool EnableUser(User user, int requestor, int reqId, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (CheckData(user, ref err) == false)
                {
                    int Event_id = 6039;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                              
                    return false;
                }
                 ADUserDetail aduser = new ADUserDetail(user.Username, !user.Enabled, user.Firstname, user.Lastname, user.Email);
                 bool status = m_adHelper.EnableUserByLogin(aduser, ref err);
                 if (status == true)
                     err = "";
                 else
                 {
                     err += ", OU="+m_OU_Filter;
                     int Event_id = 6040;
                     EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                                 
                 }
                 return status;
             }
            catch (Exception ex)
            {
                err = "Failed to enable the user account, username=" + user.Username + m_OU_Filter + ", Exception:" + ex.Message;
                int Event_id = 6041;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                                 
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 6042;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                                 
                }
            }
        }

        public bool DeleteUser(User user, int requestor, int reqId, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                if (CheckData(user, ref err) == false)
                {
                    int Event_id = 6043;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                                 
                    return false;
                }
                ADUserDetail aduser = new ADUserDetail(user.Username, !user.Enabled, user.Firstname, user.Lastname, user.Email);
                bool status = m_adHelper.DeleteUserByLogin(aduser, ref err);
                if (status == true)
                    err = "";
                else
                {
                    err += ", OU=" + m_OU_Filter + ", AD Path=";
                    int Event_id = 6044;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                                 
                }
                return status;
            }
            catch (Exception ex)
            {
                err = "Failed to delete the user account, username=" + user.Username + ", OU="+m_OU_Filter+ ", Exception:" + ex.Message;
                int Event_id = 6045;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                                 
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 6046;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                                 
                }
            }
        }

        public bool ImportUsers(string OU_Filter, int requestor,  int reqId, ref string err)
        {
            err = "";
            bool logErr = false;
            try
            {
                UserManagerEvent e = new UserManagerEvent(reqId, UserMgrCommandType.ImportUsers, requestor);
                m_ThreadContext.Post(new SendOrPostCallback(ProcessRequest), e);
                err = "Your request is being processed. It'll take a few minutes to complete.";
            }
            catch (Exception ex)
            {
                err = "Failed to import user accounts, Exception:" + ex.Message;
                int Event_id = 6047;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                                 
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 6048;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                                 
                }
            }
            return true;
        }

        private bool ImportADUsers(int userId, int reqId, ref string err)
        {
            err = "";
            bool logErr = false;
            try 
            {
                List<ADUserDetail> adUserList = null;
                adUserList = m_adHelper.GetUsers();
                if (adUserList == null)
                {
                    err = "ImportADUsers, User List is empty for OU "+m_OU_Filter;
                    int Event_id = 6049;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                                 
                    return false;                
                }
                if (adUserList.Count <= 0)
                {
                    err = "ImportADUsers, User List is empty for OU ::  " + m_OU_Filter;
                    //int Event_id = 6050;
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                                 
                    return false;
                }
                User u = new User();
                int total = adUserList.Count;
                int proc = 0;
                int failed = 0;
                m_dataAccess.UpdateReqProgress(userId, Common.UserMgrCommandType.ImportUsers.ToString(), ref reqId, total, proc, failed, ref err);
                int newUserId = -1;
                foreach (ADUserDetail adUser in adUserList)
                {
                    u.Firstname = adUser.FirstName;
                    u.Lastname = adUser.LastName;
                    u.Email = adUser.EmailAddress;
                    u.Username = adUser.LoginName;
                    u.Enabled = !adUser.AccountDisabled;
                    try
                    {
                        newUserId = m_dataAccess.UpdateUser(u, ref err);
                        if (newUserId <= 0)
                            failed++;
                        proc++;
                    }
                    catch (Exception ex)
                    {
                        failed++;
                        proc++;
                        err = "ImportADUsers, Exception:"+ex.Message;
                        int Event_id = 6051;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                                 
                    }
                    m_dataAccess.UpdateReqProgress(userId, Common.UserMgrCommandType.ImportUsers.ToString(), ref reqId, total, proc, failed, ref err);                
                }

                err = "Successfully imported " + proc + " user accounts from AD OU " + m_OU_Filter;
                Trace.TraceInformation(err);
                int event_id = 2001;                
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Information, event_id, Category);

                m_dataAccess.UpdateRequestQueue(userId, Common.UserMgrCommandType.ImportUsers.ToString(), ref reqId, true, " ", ref err);
                List<User> userList = m_dataAccess.GetUserAccounts(ref err);
                if (userList == null)
                {
                    return false;
                }
                if (userList.Count <= 0)
                    return false;
                int i;
                bool found = false;
                foreach (User ua in userList)
                {
                    found = false;
                    for (i = 0; i < adUserList.Count; i++)
                    {
                        if (ua.Username == adUserList[i].LoginName)
                        {
                            found = true;
                            break;
                        }
                     }
                     if (!found)
                     {
                        m_dataAccess.DisableUser(ua, ref err);
                     }
                }
            }
            catch (Exception ex)
            {
                err = "Failed to import AD users, OU:" + m_OU_Filter + ", Exception:" + ex.Message;
                int Event_id = 6052;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                                 
                return false;
            }
            finally
            {
                if (logErr)
                {
                    int Event_id = 6053;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);                                                 
                }
            }

            return true;
        }

        #endregion
    }
}
