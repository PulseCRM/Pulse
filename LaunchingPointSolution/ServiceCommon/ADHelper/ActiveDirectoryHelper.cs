using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.DirectoryServices;
using System.Diagnostics;
using System.Reflection;
using LP2.Service.Common;

namespace LP2.Service.ADHelper
{
    public class ActiveDirectoryHelper
    {
        short Category = 41;
        string m_LDAPPath;
        string m_LDAPUser;
        string m_LDAPPassword;
        string m_LDAPDomain;
        string m_DomainPrefix;
        string m_DomainSuffix;
        string m_OU_Filter;
        DirectoryEntry m_DirectoryEntry = null;
      
        public  ActiveDirectoryHelper()
        {
            if (m_DirectoryEntry == null)
            {
                try
                {
                    m_LDAPPath = ConfigurationManager.AppSettings["LDAPPath"];
                    m_LDAPUser = ConfigurationManager.AppSettings["LDAPUser"];
                    m_LDAPPassword = ConfigurationManager.AppSettings["LDAPPassword"];
                    m_LDAPDomain = ConfigurationManager.AppSettings["Domain"];
                    string tempOu = ConfigurationManager.AppSettings["OU"];
                    string[] dc1 = m_LDAPDomain.Split('.');
                    if (dc1.Length <= 1)
                    {
                        string err = "Invalid Domain Name for Active Directory, Domain=" + m_LDAPDomain;
                        int Event_id = 1111;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return;
                    }
                    m_DomainPrefix = dc1[0];
                    m_DomainSuffix = dc1[1];
                    string[] OUs = tempOu.Split(',');
                    if (OUs.Length == 0)
                    {
                        string err = "No OU specified, Domain=" + m_LDAPDomain;
                        int Event_id = 1114;
                        EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                        return;
                    }
                    int i = 0;
                    m_OU_Filter = "";
                    for (i = OUs.Length - 1; i >= 0; i--)
                    {
                        if (string.IsNullOrEmpty(OUs[i]) || OUs[i].Trim().Length == 0)
                        {
                            string err = string.Format("Missing one of the OUs, Index={0}, the OU config parameter={1}", i + 1, OUs);
                            int Event_id = 1116;
                            EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category); 
                        }
                        m_OU_Filter += "OU="+OUs[i].Trim();
                        if (i > 0)
                            m_OU_Filter += ",";                       
                    }
                    string LDAP_Path = string.Format(@"{0}/OU={1};DC={2};DC={3}", m_LDAPPath, m_OU_Filter, m_DomainPrefix, m_DomainSuffix);
                    //int event_id = 2001;                
                    //EventLog.WriteEntry(InfoHubEventLog.LogSource, LDAP_Path, EventLogEntryType.Information, Event_id, Category);
                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                    int Event_id = 1117;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category); 
                }
            }
        }

        public void Close()
        {
            if (m_DirectoryEntry != null)
                m_DirectoryEntry.Close();
            m_DirectoryEntry = null;
        }

        private DirectoryEntry GetRoot()
        {
            string path = m_LDAPPath + @"/" + OU + ";DC=" + m_DomainPrefix + ";DC=" + m_DomainSuffix; 
            if (m_DirectoryEntry != null)
            {
                  if (m_DirectoryEntry.Path != path)
                  {
                       m_DirectoryEntry.Close();
                       m_DirectoryEntry = null;
                   } else
                       return m_DirectoryEntry;
             }
            try
            {
                m_DirectoryEntry = new DirectoryEntry(path, LDAPUser, LDAPPassword, AuthenticationTypes.Secure);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                int Event_id = 1118;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category); 
            }
            return m_DirectoryEntry;
        }

        private String LDAPPath
        {
            get
            {
                return m_LDAPPath;
            }
        }

        private String LDAPUser
        {
            get
            {
                return m_LDAPUser;
            }
        }

        private String LDAPPassword
        {
            get
            {
                return m_LDAPPassword;
            }
        }

        public String OU
        {
            get { return m_OU_Filter; }
            set { m_OU_Filter = value; }
        }
 
        internal  ADUserDetail GetUserByFullName(String userName)
        {
            try
            {
                DirectoryEntry de = GetRoot();
                DirectorySearcher directorySearch = new DirectorySearcher(de);
                directorySearch.Filter = "(&(objectClass=user)(cn=" + userName + "))";
                SearchResult results = directorySearch.FindOne();

                if (results == null)
                    return null;
                DirectoryEntry user = new DirectoryEntry(results.Path, LDAPUser, LDAPPassword);
                return ADUserDetail.GetUser(user);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                string err = ex.Message;
                int Event_id = 1118;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                ADUserDetail ADuser = null;
                return ADuser;
            }
        }

        public  ADUserDetail GetUserByLoginName(String userName)
        {
           try
            {
                DirectoryEntry de = GetRoot();
                DirectorySearcher directorySearch = new DirectorySearcher(de);
                directorySearch.Filter = "(&(objectClass=user)(SAMAccountName=" + userName + "))";
                SearchResult results = directorySearch.FindOne();
                if (results != null)
                {
                    DirectoryEntry user = new DirectoryEntry(results.Path, LDAPUser, LDAPPassword);
                    return ADUserDetail.GetUser(user);
                }
                return null;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                string err = ex.Message;
                int Event_id = 1119;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return null;
            }
        }

        /// <summary>

        /// This function will take a DL or Group name and return list of users

        /// </summary>

        /// <param name="groupName"></param>

        /// <returns></returns>
        public List<ADUserDetail> GetUsers()
        { 
            List<ADUserDetail> userlist = new List<ADUserDetail>();
            DirectoryEntry user = null;
            try
            {
                DirectoryEntry de = GetRoot();
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.Filter = "(objectCategory=user)";
                SearchResultCollection res = ds.FindAll();
                if (res != null)
                {
                    foreach (SearchResult r in res)
                    {
                        user = r.GetDirectoryEntry();

                        ADUserDetail userobj = ADUserDetail.GetUser(user);
                        userlist.Add(userobj);
                        user.Close();
                    }
                }
                return userlist;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                string err = "GetUser from AD user server,  error: " + ex.Message;
                //int Event_id = 1120;
                //EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return userlist;
            }
            finally 
            {
                if (user != null)
                    user.Close();
            }
 
        }
        
        public  List<ADUserDetail> GetUserFromGroup(String groupName)
        {
            List<ADUserDetail> userlist = new List<ADUserDetail>();
            try
            {
                DirectoryEntry de = GetRoot(); 
                DirectorySearcher directorySearch = new DirectorySearcher(de);
                directorySearch.Filter = "(&(objectClass=group)(SAMAccountName=" + groupName + "))";
                SearchResult results = directorySearch.FindOne();
                if (results != null)
                {
                    DirectoryEntry deGroup = new DirectoryEntry(results.Path, LDAPUser, LDAPPassword);
                    System.DirectoryServices.PropertyCollection pColl = deGroup.Properties;
                    int count = pColl["member"].Count;
                    for (int i = 0; i < count; i++)
                    {
                        string respath = results.Path;
                        string[] pathnavigate = respath.Split("CN".ToCharArray());
                        respath = pathnavigate[0];
                        string objpath = pColl["member"][i].ToString();
                        string path = respath + objpath;
                        DirectoryEntry user = new DirectoryEntry(path, LDAPUser, LDAPPassword);
                        ADUserDetail userobj = ADUserDetail.GetUser(user);
                        userlist.Add(userobj);
                        user.Close();
                    }
                }
                return userlist;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                string err = ex.Message;
                int Event_id = 1122;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return userlist;
            }
        }

        #region Get user with First Name

        public  List<ADUserDetail>GetUsersByFirstName(string fName)
        {
            //UserProfile user;
            List<ADUserDetail> userlist = new List<ADUserDetail>();
            string filter = "";

            m_DirectoryEntry = null;
            DirectoryEntry de = GetRoot(); 
            DirectorySearcher directorySearch = new DirectorySearcher(de);
            directorySearch.Asynchronous = true;
            directorySearch.CacheResults = true;
            filter = string.Format("(givenName={0}*", fName);
            //            filter = "(&(objectClass=user)(objectCategory=person)(givenName="+fName+ "*))";
 
            directorySearch.Filter = filter;
            SearchResultCollection userCollection = directorySearch.FindAll();
            foreach (SearchResult users in userCollection)
            {
                DirectoryEntry userEntry = new DirectoryEntry(users.Path, LDAPUser, LDAPPassword);
                ADUserDetail userInfo =  ADUserDetail.GetUser(userEntry);
                userlist.Add(userInfo);               
            }
            directorySearch.Filter = "(&(objectClass=group)(SAMAccountName=" +fName  + "*))";
            SearchResultCollection results = directorySearch.FindAll();
            if (results != null)
            {
                    foreach (SearchResult r in results)
                    {
                        DirectoryEntry deGroup = new DirectoryEntry(r.Path, LDAPUser, LDAPPassword);                     
                        ADUserDetail agroup = ADUserDetail.GetUser(deGroup);
                        userlist.Add(agroup);
                    } 
             }
            return userlist;             
        }

        #endregion
        #region ChangeUserPassword
        public bool ChangeUserPassword(ADUserDetail u, ref string err)
        {
            err = MethodBase.GetCurrentMethod().ToString();
            if (u == null)
            {
                err += "ADUserDetail is empty.";
                return false;
            }
            if (u.LoginName == "")
                err +=  " Invalid username (empty).";
            if (u.Password == "")
                err += " Invalid password (empty).";
            if ((u.LoginName == String.Empty) || (u.Password == String.Empty))
                return false;

            try
            {
                DirectoryEntry de = GetRoot();
                DirectorySearcher directorySearch = new DirectorySearcher(de);
                directorySearch.Filter = "(&(objectClass=user)(SAMAccountName=" + u.LoginName + "))";
                SearchResult results = directorySearch.FindOne();
                if (results == null)
                {
                    return AddUserByLogin(u, ref err);
                }
                DirectoryEntry user = new DirectoryEntry(results.Path, LDAPUser, LDAPPassword);
                user.Invoke("SetPassword", u.Password);

                user.CommitChanges();
                user.Close();
            }
            catch (System.Runtime.InteropServices.COMException ce)
            {
                if (String.Format("0:X", ce.ErrorCode) == "80072035")
                {
                    err = "Failed to change password due to a password policy issue";
                    int Event_id = 1123;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
                else
                {                    
                    err = ce.InnerException.Message;
                    int Event_id = 1123;
                    EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                }
            }
            catch (Exception ex)
            {
                err = MethodBase.GetCurrentMethod() + ", Exception: " + ex.Message;
                if (null != ex.InnerException)
                    err += ", " + ex.InnerException.Message;
                int Event_id = 1125;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                Trace.TraceError(err);
                return false;
            }
            return true; 
        }
        #endregion ChangeUserPassword

        #region UpdateUserByLogin
        public bool UpdateUserByLogin(ADUserDetail userUpdate, bool fromAdd, ref string err)
        {
            err = "";
            try
            {
                DirectoryEntry de = GetRoot();
                DirectorySearcher directorySearch = new DirectorySearcher(de);
                directorySearch.Filter = "(&(objectClass=user)(SAMAccountName=" + userUpdate.LoginName + "))";
                SearchResult results = directorySearch.FindOne();
                if (results == null)
                {
                    err = MethodBase.GetCurrentMethod() + ", user account " + userUpdate.LoginName + ", does not exist in AD.";
                    if (fromAdd)
                        return false;
                    return AddUserByLogin(userUpdate, true, ref err);
                }
                return UpdateUserByLogin(results.Path, userUpdate, ref err);
            }
            catch (Exception ex)
            {
                err = MethodBase.GetCurrentMethod() + ", Exception:" + ex.Message;
                return false;
            }
        }

        public bool UpdateUserByLogin(string path, ADUserDetail userUpdate, ref string err)
        {
            DirectoryEntry user = null;
            try
            {
                err = "";
                user = new DirectoryEntry(path, LDAPUser, LDAPPassword);
                user.Properties[ADProperties.FIRSTNAME].Value = userUpdate.FirstName;
                user.Properties[ADProperties.LASTNAME].Value = userUpdate.LastName;
                if (userUpdate.EmailAddress != String.Empty)
                    user.Properties[ADProperties.EMAILADDRESS].Value = userUpdate.EmailAddress;
                if ((userUpdate.Password != null) && (userUpdate.Password != ""))
                    user.Invoke("SetPassword", userUpdate.Password);
                  user.CommitChanges();
                  if (userUpdate.AccountDisabled)
                      DisableUserByLogin(userUpdate, ref err);
                  else
                      EnableUserByLogin(userUpdate, ref err);
            }
            catch (Exception ex)
            {
                err = MethodBase.GetCurrentMethod() + ", Exception:" + ex.Message;
                Trace.TraceError(err);
                return false;
            }
            finally
            { 
                if (user != null)
                    user.Close();
                user = null;
            }
            return true;
        }
        #endregion
        #region DisableUserByLogin
        public bool DisableUserByLogin(ADUserDetail userDisable, ref string err)
        {
            err = "";
            try
            {
                 DirectoryEntry de = GetRoot();
                DirectorySearcher directorySearch = new DirectorySearcher(de);
                directorySearch.Filter = "(&(objectClass=user)(SAMAccountName=" + userDisable.LoginName + "))";
                SearchResult results = directorySearch.FindOne();
                if (results == null)
                {
                    return false;
                }
                DirectoryEntry user = new DirectoryEntry(results.Path, LDAPUser, LDAPPassword);
                int val = (int)user.Properties["userAccountControl"].Value;
                user.Properties["userAccountControl"].Value = val | 0x2; 
                user.CommitChanges();
            }
            catch (Exception ex)
            {
                err = MethodBase.GetCurrentMethod() + ", Exception:" + ex.Message;
                Trace.TraceError(err);
                return false;
            }
            return true;
        }

        #endregion
        #region EnableUserByLogin
        public bool EnableUserByLogin(ADUserDetail userEnable, ref string err)
        {
            err = "";
            DirectoryEntry user = null;
            try
            {
                DirectoryEntry de = GetRoot();
                DirectorySearcher directorySearch = new DirectorySearcher(de);
                directorySearch.Filter = "(&(objectClass=user)(SAMAccountName=" + userEnable.LoginName + "))";
                SearchResult results = directorySearch.FindOne();
                if (results == null)
                {
                    err = MethodBase.GetCurrentMethod() + ", User account " + userEnable.LoginName + ", does not exist in AD.";
                    return AddUserByLogin(userEnable, ref err);
                }
                user = new DirectoryEntry(results.Path, LDAPUser, LDAPPassword);
                int val = (int)user.Properties["userAccountControl"].Value;
                user.Properties["userAccountControl"].Value = val & ~0x2;
                user.CommitChanges();

                return true;
            }
            catch (Exception ex)
            {
                err = MethodBase.GetCurrentMethod() + ", Exception:" + ex.Message;
                Trace.TraceError(err);
                return false;
            }
            finally
            { 
                if (user != null)
                    user.Close();
                user = null;
            }
        }
        
        #endregion
        #region AddUserByLogin
        public bool AddUserByLogin(ADUserDetail userAdd, bool fromUpdate, ref string err)
        {
           err = "";
            DirectoryEntry de = null;
             try
            {
                de = GetRoot();
                DirectorySearcher directorySearch = new DirectorySearcher(de);
                directorySearch.Filter = "(&(objectClass=user)(SAMAccountName=" + userAdd.LoginName + "))";
                SearchResult results = directorySearch.FindOne();
                if (results != null)
                {
                    err = MethodBase.GetCurrentMethod() + ", user account " + userAdd.LoginName + " already exists in AD.";
                    if (fromUpdate)
                        return false;

                    return UpdateUserByLogin(results.Path, userAdd, ref err);
                }
                return AddUserByLogin(userAdd, ref err);
            }
            catch (Exception ex)
            { 
                err = MethodBase.GetCurrentMethod()+", Exception:"+ex.Message;
                return false;
            }
        }

        public bool AddUserByLogin(ADUserDetail userAdd, ref string err)
        {
            err = "";
            DirectoryEntry de = null;
            DirectoryEntry user = null;
            DirectoryEntry deuser = null;
            try
            {
                de = GetRoot();
                user =  de.Children.Add("CN="+userAdd.LoginName, "user");
                using (user)
                {
                    //sAMAccountName is required for W2k AD, we would not use
                    //this for ADAM, however.
                    user.Properties[ADProperties.LOGINNAME].Value = userAdd.LoginName;
                    user.Properties[ADProperties.FIRSTNAME].Value = userAdd.FirstName;
                    user.Properties[ADProperties.LASTNAME].Value = userAdd.LastName;
                    if (userAdd.EmailAddress != String.Empty)
                        user.Properties[ADProperties.EMAILADDRESS].Value = userAdd.EmailAddress;
                    user.Properties[ADProperties.DISPLAYNAME].Value = userAdd.FirstName + " " + userAdd.LastName;
                    //user.Properties["FullName"].Value = userAdd.FirstName + " " + userAdd.LastName;
                    //userPrincipalName is not required, but recommended
                    //for ADAM. AD also contains this, so we can use it.
                    //user.Properties[ADProperties.USERPRINCIPALNAME].Value = userAdd.LoginName;
                    user.CommitChanges();
                }

                UpdateUserByLogin(userAdd, true, ref err);
                return true;
            }
            catch (Exception ex)
            {
                err = MethodBase.GetCurrentMethod() + ", Exception: " + ex.Message;
                return false;
            }
            finally
            {
                if (user != null)
                    user.Close();
                if (deuser != null)
                    deuser.Close();
                de = null;
                user = null;
                deuser = null;
            }
        }
        #endregion
        #region DeleteUserByLogin
        public bool DeleteUserByLogin(ADUserDetail userDelete, ref string err)
        {
            err = "";
            try
            {
                DirectoryEntry de = GetRoot();
                de = GetRoot();
                DirectorySearcher directorySearch = new DirectorySearcher(de);
                directorySearch.Filter = "(&(objectClass=user)(SAMAccountName=" + userDelete.LoginName + "))";
                SearchResult results = directorySearch.FindOne();
                if (results == null)
                {
                    //err = MethodBase.GetCurrentMethod() + ", user account " + userDelete.LoginName + " does not exist in AD.";
                    return true;
                }
                DirectoryEntry user = new DirectoryEntry(results.Path, LDAPUser, LDAPPassword);
                
                de.Children.Remove(user);
                de.CommitChanges();
            }
            catch (Exception ex)
            {
                err = MethodBase.GetCurrentMethod() + " Exception, " + ex.Message;
               Trace.TraceError(err);
                //throw ex;
               return false;
            }
 
            return true;
        }

        #endregion
        #region AddUserToGroup
        public  bool AddUserToGroup(ADUserDetail user, string groupName, ref string err)
        {
            err = "";
            try
            {
                ADManager admanager = new ADManager(LDAPPath, LDAPUser, LDAPPassword);
                if (admanager.AddUserToGroup(user.LoginName, groupName) == false)
                    return false;

                return UpdateUserByLogin(user, false, ref err);
            }
            catch (Exception ex)
            {
                err = MethodBase.GetCurrentMethod() + ", Exception:" + ex.Message;
                Trace.TraceError(err);
                return false;
            }
        }

        #endregion

        #region RemoveUserToGroup
        public  bool RemoveUserToGroup(string userlogin, string groupName)
        {
            try
            {
                m_DirectoryEntry = null;
                ADManager admanager = new ADManager(LDAPPath, LDAPUser, LDAPPassword);
                admanager.RemoveUserFromGroup(userlogin, groupName);
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                string err = ex.Message;
                int Event_id = 1128;
                EventLog.WriteEntry(InfoHubEventLog.LogSource, err, EventLogEntryType.Warning, Event_id, Category);
                return false;
            }
        }

        #endregion     
     
    }
}
