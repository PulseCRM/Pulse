using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PulseInstallation
{
    public partial class BaseFrm : Form
    {
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public BaseFrm()
        {
            InitializeComponent();
        }

        BaseFrm backFrm, nextFrm;

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

        public InstallContext Context { get; set; }
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

        public static WizardParams Params = new WizardParams();
        public string _windowsClassName = "MsiDialogCloseClass";
        public string _windowsName = "PulseServiceSetup";

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (backFrm.Visible)
            {
                this.Visible = false;
                backFrm.ShowDialog();
            }
            else
            {
                this.Visible = false;
                backFrm.Visible = true;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (nextFrm != null)
            {
                if (OnNext())
                {
                    this.Visible = false;
                    nextFrm.ShowDialog();
                }
            }
        }

        public virtual bool OnNext()
        {
            return true;
        }

        public virtual bool OnCancel()
        {
            ShowSetupWindow();
            return true;
        }

        private void ShowSetupWindow()
        {
            IntPtr setupWindow = FindWindow(_windowsClassName, _windowsName);
            if (setupWindow != IntPtr.Zero)
            {
                ShowWindow(setupWindow, 1);
            }
            Application.Exit();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancel();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            ShowSetupWindow();
        }
    }
}
