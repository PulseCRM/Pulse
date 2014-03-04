using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using LPWeb.Model;
using Microsoft.SharePoint.BusinessData.Administration;
using Users = LPWeb.BLL.Users;

namespace LPWeb.Common
{
    [Serializable]
    public class LoginUser
    {
        #region Current login user properties

        private readonly bool _bUserEnabled = true;
        private  bool _bIsCompanyExecutive = false;
        private  bool _bIsRegionExecutive = false;
        private  bool _bIsDivisionExecutive = false;
        private  bool _bIsBranchManager = false;

        private  bool _bIsCompanyUser = false;
        private  bool _bIsRegionUser = false;
        private  bool _bIsDivisionUser = false;
        private  bool _bIsBranchUser = false;
        private  readonly bool _bAccessOtherLoans = false;

        private readonly int _iRoleID;
        private readonly int _iUserID;
        private readonly string _sEmail = string.Empty;
        private readonly string _sFirstName = string.Empty;
        private readonly string _sFullName = string.Empty;
        private readonly string _sLastName = string.Empty;
        private readonly string _sRoleName = string.Empty;
        private readonly string _sUserName = string.Empty;
        private readonly List<Int32> _recentItems=new List<Int32>();

        private readonly Roles _userRole = new Roles();
        private string userLoanList = string.Empty;             //FileIds separated by ","
        //private readonly DataTable _dtUserInfo;
        private Model.Users userInfo = new Model.Users();


        // CR48
        private bool _QuickLeadForm = false;
        private bool _RemindTaskDue = false;
        private int? _TaskReminder;
        private string _SortTaskPickList;
        private Dictionary<int, LPWeb.BLL.TaskReminder> _TaskListDueToday = null;

        //CR38
        private bool _ViewLockInfo = false;
        private bool _LockRate = false;

        //CR52
        private bool _AccessProfitability = false;
        private bool _ViewCompensation = false;
        //CR64
        private bool _UpdateCondition = false;

        public int iUserID
        {
            get { return _iUserID; }
        }

        public bool bUserEnabled
        {
            get { return _bUserEnabled; }
        }
        public bool bIsCompanyExecutive
        {
            get { return _bIsCompanyExecutive; }
        }
        public bool bIsRegionExecutive
        {
            get { return _bIsRegionExecutive; }
        }
        public bool bIsDivisionExecutive
        {
            get { return _bIsDivisionExecutive; }
        }
        public bool bIsBranchManager
        {
            get { return _bIsBranchManager; }
        }

        public bool bIsCompanyUser
        {
            get { return _bIsCompanyUser; }
        }
        public bool bIsRegionUser
        {
            get { return _bIsRegionUser; }
        }
        public bool bIsDivisionUser
        {
            get { return _bIsDivisionUser; }
        }
        public bool bIsBranchUser
        {
            get { return _bIsBranchUser; }
        }

        public string sUserName
        {
            get { return _sUserName; }
        }

        public string sEmail
        {
            get { return _sEmail; }
        }

        public string sFirstName
        {
            get { return _sFirstName; }
        }

        public string sLastName
        {
            get { return _sLastName; }
        }

        public string sFullName
        {
            get { return _sFullName; }
        }

        public int iRoleID
        {
            get { return _iRoleID; }
        }

        public string sRoleName
        {
            get { return _sRoleName; }
        }

        public Roles userRole
        {
            get { return _userRole; }
        }

        public bool bAccessOtherLoans
        {
            get { return _bAccessOtherLoans; }
        }


        #region CR48

        public bool QuickLeadForm
        {
            get { return _QuickLeadForm; }
        }

        public bool RemindTaskDue
        {
            get { return _RemindTaskDue; }
        }

        public int? TaskReminder
        {
            get { return _TaskReminder; }
        }

        public string SortTaskPickList
        {
            get { return _SortTaskPickList; }
        }

        public Dictionary<int, LPWeb.BLL.TaskReminder> TaskListDueToday
        {
            get { return _TaskListDueToday; }
        }

        #endregion

        //CR38
        public bool ViewLockInfo
        {
            get { return _ViewLockInfo; }
        }
        public bool LockRate
        {
            get { return _LockRate; }
        }
        //CR52
        public bool AccessProfitability
        {
            get { return _AccessProfitability; }
        }
        public bool ViewCompensation
        {
            get { return _ViewCompensation; }
        }

        public List<Int32> RecentItems
        {
            get { return _recentItems; }
        }
        //CR65
        public bool UpdateCondition
        {
            get { return _UpdateCondition; }
        }
        #endregion

        /// <summary>
        /// get login user info
        /// </summary>
        public LoginUser()
        {
            // do not check authorization for Unauthorize page
            if (System.IO.Path.GetFileName(HttpContext.Current.Request.PhysicalPath).ToLower() == "unauthorize.aspx")
                return;

            string sLoginUserId = HttpContext.Current.User.Identity.Name;
            if (sLoginUserId.IndexOf("\\") >= 0)
            {
                sLoginUserId = sLoginUserId.Substring(sLoginUserId.LastIndexOf("\\") + 1);
            }

            // 加载用户数据
            var userManager = new Users();
            //DataTable dtUserInfo = userManager.GetModel(" AND Username='" + sLoginUserId + "'");
            //if (dtUserInfo == null || dtUserInfo.Rows.Count == 0)
            //{
            //    HttpContext.Current.Response.Redirect("~/_layouts/LPWeb/Unauthorize.aspx");
            //    return;
            //}
            string sqlCmd = string.Format("Select top 1 UserId from Users where username='{0}'", sLoginUserId);
            object obj = DAL.DbHelperSQL.GetSingle(sqlCmd);
            int userId = (obj == null || obj == DBNull.Value) ? 0 : (int) obj;
            if (userId <= 0)
            {
                HttpContext.Current.Response.Redirect("~/_layouts/LPWeb/Unauthorize.aspx");
                return;
            }
            int userid = (int)obj;
            userInfo = userManager.GetModel_WithoutPicture(userid);

            _iUserID = userid;
            _bUserEnabled = userInfo.UserEnabled;
            _sUserName = userInfo.Username;
            _sFirstName = userInfo.FirstName;
            _sLastName = userInfo.LastName;
            _sEmail = userInfo.EmailAddress;
            _sFullName = _sFirstName + ' '+ _sLastName;
            _iRoleID = userInfo.RoleId;
            if (_iRoleID <= 0)
            {
                HttpContext.Current.Response.Redirect("~/_layouts/LPWeb/Unauthorize.aspx");
                return;
            }

            var roleMgr = new BLL.Roles();
            _userRole = roleMgr.GetModel(_iRoleID);
            _sRoleName = _userRole.Name;
            _bAccessOtherLoans = _userRole.OtherLoanAccess;
            GetUserOrganizationInfo(userManager);
            GetRecentItems(userid);
            #region CR48

            LPWeb.BLL.UserHomePref UserHomePref1 = new LPWeb.BLL.UserHomePref();
            LPWeb.Model.UserHomePref UserPrefInfo = UserHomePref1.GetModel(userid);
            if (UserPrefInfo != null)
            {
                this._QuickLeadForm = UserPrefInfo.QuickLeadForm;

                this._RemindTaskDue = userInfo.RemindTaskDue;
                this._TaskReminder = userInfo.TaskReminder;
                this._SortTaskPickList = userInfo.SortTaskPickList;
            }

            // init TaskListDueToday
            this.InitTaskListDueToday();

            #endregion

            //CR38
            this._ViewLockInfo = _userRole.ViewLockInfo;
            this._LockRate = _userRole.LockRate;
            //CR52
            this._AccessProfitability = _userRole.AccessProfitability;
            this._ViewCompensation = _userRole.ViewCompensation;
            //CR65
            this._UpdateCondition = _userRole.UpdateCondition;
        }
        public string GetUserLoanList()
        {
            string result = "";
            string temp = string.Format("Select LoanID from dbo.[lpfn_GetUserLoans2] ('{0}', '{1}') ", _iUserID, _bAccessOtherLoans);
            DataTable dt = DAL.DbHelperSQL.ExecuteDataTable(temp);
            if (dt == null || dt.Rows.Count <= 0)
                return result;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["LoanID"] == null || dr["LoanID"] == DBNull.Value)
                    continue;
                if (result.Length > 0)
                    result += "," + dr["LoanID"].ToString();
                else
                    result = dr["LoanID"].ToString();
            }
            return result;
        }

        public void InitTaskListDueToday() 
        {
            if (this._RemindTaskDue == true)
            {
                this._TaskListDueToday = new Dictionary<int, BLL.TaskReminder>();
                
                LPWeb.BLL.LoanTasks TaskMgr = new BLL.LoanTasks();
                DataTable TaskList = TaskMgr.GetUserTaskDueToday(this._iUserID);
                foreach (DataRow TaskRow in TaskList.Rows)
                {
                    int iLoanTaskId = Convert.ToInt32(TaskRow["LoanTaskId"]);
                    int iFileId = Convert.ToInt32(TaskRow["FileId"]);
                    DateTime Due = Convert.ToDateTime(TaskRow["Due"]);
                    string sTaskName = TaskRow["Name"].ToString();
                    string sBorrower = TaskRow["Borrower"].ToString();

                    LPWeb.BLL.TaskReminder reminder = new LPWeb.BLL.TaskReminder();
                    reminder.LoanTaskId = iLoanTaskId;
                    reminder.FileId = iFileId;
                    reminder.Due = Due;
                    reminder.TaskName = sTaskName;
                    reminder.Borrower = sBorrower;

                    this._TaskListDueToday.Add(iLoanTaskId, reminder);
                }
            }
        }

        private void GetUserOrganizationInfo(BLL.Users userManager)
        { 
        
                    // is company executive
            this._bIsCompanyExecutive = userManager.IsCompanyExecutive(this._iUserID);
            if (this.bIsCompanyExecutive)
            {
                this._bIsRegionExecutive = false;
                this._bIsDivisionExecutive = false;
                this._bIsCompanyUser = false;
                this._bIsBranchManager = false;
                this._bIsRegionUser = false;
                this._bIsDivisionUser = false;
                this._bIsBranchUser = false;
                return;
            }
            // is region executive
            this._bIsRegionExecutive = userManager.IsRegionExecutive(this._iUserID);
            if (this._bIsRegionExecutive)
            {
                this._bIsCompanyExecutive = false;
                this._bIsDivisionExecutive = false;
                this._bIsCompanyUser = false;
                this._bIsBranchManager = false;
                this._bIsRegionUser = false;
                this._bIsDivisionUser = false;
                this._bIsBranchUser = false;
                return;
            }
            // is division executive
            this._bIsDivisionExecutive = userManager.IsDivisionExecutive(this._iUserID);
            if (this._bIsDivisionExecutive)
            {
                this._bIsCompanyExecutive = false;
                this._bIsRegionExecutive = false;
                this._bIsCompanyUser = false;
                this._bIsBranchManager = false;
                this._bIsRegionUser = false;
                this._bIsDivisionUser = false;
                this._bIsBranchUser = false;
                return;
            }
            // is Branch Manager
            this._bIsBranchManager = userManager.IsBranchManager(this._iUserID);
            if (this._bIsBranchManager)
            {
                this._bIsCompanyExecutive = false;
                this._bIsRegionExecutive = false;
                this._bIsCompanyUser = false;
                this._bIsDivisionExecutive = false;
                this._bIsRegionUser = false;
                this._bIsDivisionUser = false;
                this._bIsBranchUser = false;
                return;
            }
            // is company User
            this._bIsCompanyUser = userManager.IsCompanyUser(this._iUserID);
            if (this._bIsCompanyUser)
            {
                this._bIsCompanyExecutive = false;
                this._bIsRegionExecutive = false;
                this._bIsBranchManager = false;
                this._bIsDivisionExecutive = false;
                this._bIsRegionUser = false;
                this._bIsDivisionUser = false;
                this._bIsBranchUser = false;
                return;
            }
            // is region User
            this._bIsRegionUser = userManager.IsRegionUser(this._iUserID);
            if (this._bIsRegionUser)
            {
                this._bIsCompanyExecutive = false;
                this._bIsRegionExecutive = false;
                this._bIsBranchManager = false;
                this._bIsDivisionExecutive = false;
                this._bIsCompanyUser = false;
                this._bIsDivisionUser = false;
                this._bIsBranchUser = false;
                return;
            }
            // is division User
            this._bIsDivisionUser = userManager.IsDivisionUser(this._iUserID);
            if (this._bIsDivisionUser)
            {
                this._bIsCompanyExecutive = false;
                this._bIsRegionExecutive = false;
                this._bIsBranchManager = false;
                this._bIsDivisionExecutive = false;
                this._bIsCompanyUser = false;
                this._bIsRegionUser = false;
                this._bIsBranchUser = false;
                return;
            }
            // is Branch User
            this._bIsBranchUser = userManager.IsBranchUser(this._iUserID);
            if (this._bIsBranchUser)
            {
                this._bIsCompanyExecutive = false;
                this._bIsRegionExecutive = false;
                this._bIsBranchManager = false;
                this._bIsDivisionExecutive = false;
                this._bIsCompanyUser = false;
                this._bIsRegionUser = false;
                this._bIsDivisionUser = false;
                return;
            }
        }

        private void GetRecentItems(int iUserID)
        {
            BLL.UserRecentItems _UserRecentItems = new BLL.UserRecentItems();
            DataSet dsUserRecentItems = _UserRecentItems.GetUserRecentItems(iUserID);
            if(dsUserRecentItems!=null && dsUserRecentItems.Tables[0].Rows.Count>0)
            {
                foreach (DataRow dr in dsUserRecentItems.Tables[0].Rows)
                {
                    _recentItems.Add(Convert.ToInt32(dr["FileId"]));
                }
            }
        }

        /// <summary>
        /// 验证页面的访问权限，如果没有访问权限则重定向提示无权限页面
        /// </summary>
        /// <param name="permissionName">权限控制字段名称</param>
        /// <returns>返回是否有访问该页面的权限</returns>
        public bool ValidatePageVisitPermission(string permissionName)
        {
            Dictionary<string, bool> dic = ValidatePermission(new string[1] { permissionName });

            if (dic == null || dic.Count == 0)
                throw new ArgumentNullException("permissionName", "The permission name is invaild!");

            if (dic[permissionName])
            {
                return true;
            }

            HttpContext.Current.Response.Redirect("~/_layouts/LPWeb/Unauthorize.aspx");//todo:shoud be redirect login page
            return false;
        }

        /// <summary>
        /// 验证按钮显示的权限,并控制增，删，改按钮的显示或隐藏
        /// </summary>
        /// <param name="permissionName">权限控制字段名称</param>
        /// <param name="btnControls">需要控制的按钮</param>
        /// <returns>返回按钮操作权限</returns>
        public bool ValidateButtonStatePermission(string permissionName, params WebControl[] btnControls)
        {
            //这四个字段都是smallint类型，分别用来标识该项的create，edit，delete权限的
            var specialField = new List<string>
                                   {    
                                       "SetUserGoals", 
                                       "SetOwnGoals", 
                                       "ImportLoan", 
                                       "RemoveLoan"
                                   };
            if (!specialField.Contains(permissionName))
                throw new ArgumentNullException("permissionName", "The permission name is not button validation permission!");//todo:modify the message

            Dictionary<string, bool> dic = ValidatePermission(new string[1] { permissionName });

            if (dic == null || dic.Count == 0)
                throw new ArgumentNullException("permissionName", "The permission name is invaild!");

            if (btnControls == null || btnControls.Length == 0)
                return dic[permissionName];

            foreach (var btnControl in btnControls)
            {
                btnControl.Visible = dic[permissionName];
            }

            return dic[permissionName];
        }

        /// <summary>
        /// 验证按钮显示的权限
        /// </summary>
        /// <param name="permissionName">权限控制字段名称</param>
        /// <returns>返回按钮操作权限</returns>
        public bool ValidateButtonStatePermission(string permissionName)
        {
            return ValidateButtonStatePermission(permissionName, null);
        }

        /// <summary>
        /// 验证参数中每项功能是否权限
        /// </summary>
        /// <param name="validateParms">需要验证的功能参数列表</param>
        /// <returns>返回与参数各项对应的true或false的验证结果(注：当参数是"WorkflowTempl","CustomTask","AlertRules"或"AlertRuleTempl"时，
        /// 将返回对应的增删改三项的权限。 例：WorkflowTempl将返回"Create","Edit","Delete"三项权限)</returns>
        private Dictionary<string, bool> ValidatePermission(string[] validateParms)
        {
            if (validateParms == null || validateParms.Length == 0)
            {
                throw new ArgumentNullException("validateParms", "Parameter is empty!");
            }

            if (userInfo == null || userInfo.UserId == 0 || userInfo.RoleId <= 0)
            {
                throw new Exception("load user failed!");
            }

            var permissionResult = new Dictionary<string, bool>();

            //这四个字段都是smallint类型，分别用来标识该项的create，edit，delete权限的(在项目一期中不会验证这些权限)
            var specialField = new List<string>
                                   {    
                                       "WorkflowTempl", 
                                       "CustomTask", 
                                       "AlertRules", 
                                       "AlertRuleTempl"
                                   };

            var roleMgr = new BLL.Roles();
            var dsRole = roleMgr.GetList(string.Format(" RoleId={0}", _iRoleID));
            if(dsRole==null || dsRole.Tables.Count==0 || dsRole.Tables[0].Rows.Count==0)
                throw new Exception("load user role failed!");

            //根据查询结果检查参数中各项的权限，并加入返回结果集
            foreach (var validateParm in validateParms)
            {
                if (!dsRole.Tables[0].Columns.Contains(validateParm)) //table中不包含要验证的字段
                    continue;

                if (!specialField.Contains(validateParm))
                    permissionResult.Add(validateParm, dsRole.Tables[0].Rows[0].Field<bool>(validateParm));
            }

            return permissionResult;
        }

        ///// <summary>
        ///// 验证增删改的权限
        ///// </summary>
        ///// <param name="ret"></param>
        ///// <param name="validatParm"></param>
        ///// <param name="fieldValue"></param>
        //private static void ValidaeCudPermission(IDictionary<string, bool> ret, string validatParm, Int16 fieldValue)
        //{
        //    //            string create = validatParm + "_Create";
        //    //            string edit = validatParm + "_Edit";
        //    //            string delete = validatParm + "_Delete";
        //    string create = "Create";
        //    string edit = "Edit";
        //    string delete = "Delete";

        //    ret.Add(create, fieldValue.ToString().Contains("1"));
        //    ret.Add(edit, fieldValue.ToString().Contains("2"));
        //    ret.Add(delete, fieldValue.ToString().Contains("3"));
        //}
    }
}