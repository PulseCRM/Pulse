using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LP2.Service.ADHelper;
using LP2.Service;
using LP2.Service.Common;
using DataAccess;

namespace TestADMgr
{
    public partial class TestADImport : Form
    {
        ActiveDirectoryHelper adHelper = null;
        UserManager um = null;
        DataAccess.DataAccess da = null;
        public TestADImport()
        {
            InitializeComponent();
            cbCommand.Items.Clear();
            cbCommand.Items.Add("Add");
            cbCommand.Items.Add("ChangePassword");
            cbCommand.Items.Add("Delete");
            cbCommand.Items.Add("Disable");
            cbCommand.Items.Add("Enable");
            cbCommand.Items.Add("Import");
            cbCommand.Items.Add("Update");
            cbCommand.Items.Add("List");
            rtbMsg.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                adHelper = new ActiveDirectoryHelper();
                um = UserManager.Instance;
                da = new DataAccess.DataAccess();
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText("Exception: " + ex.Message);  
            }
        }

        private bool checkData()
        {
            string cmd = (string)cbCommand.SelectedItem;
            if ((cmd != "List") && (cmd != "Import"))
            {
                if (tbUser.Text == String.Empty)
                {
                    MessageBox.Show("No user name.", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;                
                }
            }

            if ((cmd == "Add") ||  (cmd == "Update"))
            {
                if ((tbEmail.Text == String.Empty) ||  (tbFirst.Text == String.Empty) ||  (tbLast.Text == String.Empty))
                {
                    MessageBox.Show("User's First and Last Names and Email must be present.", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            rtbMsg.Clear();
            return true;
        }
        private bool execUserCmd()
        {
            bool Disabled = (!cbEnabled.Checked);
            um.OU_Filter = tbOU.Text.Trim();
            int reqId = 0;
            
            User user = new User(tbUser.Text, tbFirst.Text, tbLast.Text, tbEmail.Text, tbPwd.Text, cbEnabled.Checked);
            string cmd = (string)cbCommand.SelectedItem;
            string err = "";
            bool status = false;
           
            da.AddRequestQueue(100, cmd, ref reqId, ref err);          
            switch (cmd.ToLower())
            {
                case "add":
                    status = um.CreateUser(user, 100,  reqId, ref err);
                    break;
                case "changepassword":
                    status = um.ChangeUserPassword(user.Username, user.Password, 100, reqId, ref err);
                    break;
                case "delete":
                    status = um.DeleteUser(user, 100, reqId, ref err);
                    break;
                case "disable":
                    status = um.DisableUser(user, 100, reqId, ref err);
                    break;
                case "enable":
                    status = um.EnableUser(user, 100, reqId, ref err);
                    break;
                case "import":
                    status = um.ImportUsers(tbOU.Text, 100, reqId, ref err);
                    break;
                case "update":
                    status = um.UpdateUser(user, 100, reqId, ref err);
                    break;
                case "list":
                    List<ADUserDetail> userList = adHelper.GetUsers();
                    if (userList.Count <= 0)
                    {
                        rtbMsg.AppendText("No users found.");
                        return false;
                    }
                    foreach (ADUserDetail u in userList)
                    {
                        rtbMsg.AppendText("Username: " + u.LoginName + ", Disabled=" + u.AccountDisabled.ToString() + ", FirstName=" + u.FirstName + ",LastName=" + u.LastName + ", Email=" + u.EmailAddress + "\r\n");
                        rtbMsg.AppendText("===================================================================================================================\r\n");
                    }
                    break;
                default:
                    MessageBox.Show("Unknown command.");
                    return false;
            }
            if (status == false)
                rtbMsg.AppendText(err);
            else
                rtbMsg.AppendText("Successfully completed " + cmd + "!");
            return true;     
        }

        private bool execADCmd()
        {
            bool Disabled = (!cbEnabled.Checked);
            if (adHelper.OU != tbOU.Text)
                adHelper.OU = tbOU.Text.Trim();
            ADUserDetail user = new ADUserDetail(tbUser.Text, Disabled, tbFirst.Text, tbLast.Text, tbEmail.Text, tbPwd.Text);
            string cmd = (string)cbCommand.SelectedItem;
            string err = "";
            switch (cmd.ToLower())
            { 
                case "add":
                    adHelper.AddUserByLogin(user, ref err);
                    break;
                case "delete":
                    adHelper.DeleteUserByLogin(user, ref err);
                    break;
                case "disable":
                    adHelper.DisableUserByLogin(user, ref err);
                    break;
                case "enable":
                    adHelper.EnableUserByLogin(user, ref err);
                    break;
                case "import":
                    break;
                case "update":
                    adHelper.UpdateUserByLogin(user, false, ref err);
                    break;
                case "list":
                    List<ADUserDetail> userList = adHelper.GetUsers();
                    if (userList.Count <= 0)
                    {
                        rtbMsg.AppendText("No users found.");
                        return false;
                    }
                    foreach (ADUserDetail u in userList)
                    {
                        rtbMsg.AppendText("Username: " + u.LoginName +", Disabled="+ u.AccountDisabled.ToString() + ", FirstName=" + u.FirstName + ",LastName=" + u.LastName + ", Email=" + u.EmailAddress + "\r\n");
                        rtbMsg.AppendText("===================================================================================================================\r\n");
                    }
                    break;
                default:
                    MessageBox.Show("Unknown command.");
                    return false;
            }
            
            rtbMsg.AppendText("Successfully completed " + cmd + "!");
            return true;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!checkData())
                return;
            try
            {
                //execADCmd();
                execUserCmd();
            }
            catch (Exception ex)
            {
                rtbMsg.AppendText(ex.Message);
            }
        }

    }
}
