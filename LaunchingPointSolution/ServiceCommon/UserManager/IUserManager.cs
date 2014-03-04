using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using LP2.Service.Common;

namespace LP2.Service
{
     public class UserManagerEvent : EventArgs
    {
        Common.UserMgrCommandType m_reqType;
        int m_userId;   // this is the user who initiates the request
        int m_reqId;
        User m_user;

        public UserManagerEvent()
        {
            m_reqType = Common.UserMgrCommandType.Unknown;
        }
        public UserManagerEvent(int reqid, Common.UserMgrCommandType req)
        {
            m_reqId = reqid;
            m_reqType = req;
            m_userId = 0;
            m_user = null;
        }
        public UserManagerEvent(int reqid, Common.UserMgrCommandType req, int userId)
        {
            m_reqId = reqid;
            m_reqType = req;
            m_userId = userId;
            m_user = null;
        }

        public UserManagerEvent(int reqid, Common.UserMgrCommandType req, int userId, User u)
        {
            m_reqId = reqid;
            m_reqType = req;
            m_userId = userId;
            m_user = u;
        }
        public UserManagerEvent(int reqid, Common.UserMgrCommandType req, User u)
        {
            m_reqId = reqid;
            m_reqType = req;
            m_user = u;
            m_userId = 0;
        }
        public Common.UserMgrCommandType RequestType
        {
            get { return m_reqType; }
            set { m_reqType = value; }
        }

        public User ADUser
        {
            get { return m_user; }
            set { m_user = value; }
        }
        public int RequestId
        {
            get { return m_reqId; }
            set { m_reqId = value; }
        }

        public int UserId
        {
            get { return m_userId; }
            set { m_userId = value; }
        }
    }
    public interface IUserManager
    {
        bool CreateUser(User user, int requestor, int reqId, ref string err);
        bool UpdateUser(User user, int requestor, int reqId, ref string err);
        bool ChangeUserPassword(string login, string pwd, int requestor, int reqId, ref string err);
        bool DisableUser(User user, int requestor, int reqId, ref string err);
        bool EnableUser(User user, int requestor, int reqId, ref string err);
        bool DeleteUser(User user, int requestor, int reqId, ref string err);
        bool ImportUsers(string OU_Filter, int requestor, int reqId, ref string err); 
    }
}
