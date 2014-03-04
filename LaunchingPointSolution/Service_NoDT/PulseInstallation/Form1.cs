using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using PulseInstallation;

namespace PulseInstallation
{
    public partial class Form1 : BaseFrm
    {
        public Form1(InstallContext context)
        {
            InitializeComponent();
            IntPtr setupWindow = FindWindow(_windowsClassName, _windowsName);
            if (setupWindow != IntPtr.Zero)
            {
                ShowWindow(setupWindow, 0);
            }
            Context = context;
            BackFrm = null;
            NextFrm = new Form3();
            NextFrm.BackFrm = this;
        }
        public Form1()
        {

        }
    }
}
