using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PulseInstallation
{
    public partial class BaseFrm : Form
    {
        BaseFrm backFrm, nextFrm;

        public static WizardParams Params = new WizardParams();
        public string _windowsClassName = "MsiDialogCloseClass";
        public string _windowsName = "Pulse";
        public static DialogResult FrmDialogResult = DialogResult.None;
        private bool fakeC = false;
        private static bool finishfakeC = false;
        public virtual bool OnBack()
        {
            return true;
        }

        public virtual bool OnNext()
        {
            return true;
        }

        public virtual bool OnFinished()
        {
            return true;
        }

        public virtual bool OnCancel()
        {
            return true;
        }

        public BaseFrm BackFrm
        {
            get { return backFrm; }
            set { backFrm = value; }
        }

        public BaseFrm NextFrm
        {
            get { return nextFrm; }
            set { nextFrm = value; }
        }

        public static InstallContext Context { get; set; }

        public BaseFrm()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, uint hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        public static extern void SetForegroundWindow(IntPtr hwnd);

        private void BaseFrm_Load(object sender, EventArgs e)
        {
            if (backFrm == null)//first page
            {
                btnNext.Enabled = btnCancel.Enabled = true;
            }

            if (nextFrm == null)//end page
            {
                btnBack.Enabled = btnFinish.Enabled = true;
            }

            if (backFrm != null && nextFrm != null)
            {
                btnBack.Enabled = btnNext.Enabled = btnCancel.Enabled = true;
            }
        }
        public static IntPtr MakeLParam(int wLow, int wHigh)
        {
            return (IntPtr)(((short)wHigh << 16) | (wLow & 0xffff));
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (OnBack())
            {
                if (backFrm != null)
                {
                    this.Visible = false;
                    backFrm.Visible = true;
                    fakeC = true;
                }
                //if (backFrm != null)
                //{
                //    if (backFrm.Visible)
                //    {
                //        this.Visible = false;
                //        backFrm.ShowDialog();
                //    }
                //    else
                //    {
                //        this.Visible = false;
                //        backFrm.Visible = true;
                //    }
                //}
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (OnNext())
            {
                if (nextFrm != null)
                {
                    this.Visible = false;
                    nextFrm.ShowDialog();
                }
            }
        }

        private void ShowSetupWindow()
        {
            IntPtr setupWindow = FindWindow(_windowsClassName, _windowsName);
            if (setupWindow != IntPtr.Zero)
            {
                ShowWindow(setupWindow, 1);
            }
            this.Visible = false;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancel())
            {
                string message = "You have not completed the installation process. Are you sure you want to cancel?";
                if (MessageBox.Show(message, "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    FrmDialogResult = DialogResult.Cancel;
                    ShowSetupWindow();
                }
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            if (OnFinished())
            {
                FrmDialogResult = DialogResult.OK;
                ShowSetupWindow();
                finishfakeC = true;
            }
            
        }

        private void BaseFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = fakeC || finishfakeC;
            if (fakeC || finishfakeC)
            {
                if (fakeC)
                fakeC = false;

                return;
            }

            //MessageBox.Show("Form Closing");
            FrmDialogResult = DialogResult.Cancel;
            ShowSetupWindow();
        }

        public Configuration GetConfig()
        {
            string contextPath = string.Empty;
            if (Context != null && Context.Parameters.ContainsKey("assemblypath"))
            {
                contextPath = Context.Parameters["assemblypath"];
            }
            if (string.IsNullOrEmpty(contextPath))
            {
                //todo:
                return null;
            }
            string exePath = Path.Combine(Path.GetDirectoryName(contextPath), "InfoHubService.exe");
            return ConfigurationManager.OpenExeConfiguration(exePath);

        }
    }
}
