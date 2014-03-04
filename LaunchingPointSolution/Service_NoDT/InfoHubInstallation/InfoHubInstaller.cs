using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Windows.Forms;


namespace PulseInstallation
{
    [RunInstaller(true)]
    public partial class PulseInstaller : System.Configuration.Install.Installer
    {
        public PulseInstaller()
        {
            InitializeComponent();
        }
        protected override void OnBeforeInstall(IDictionary savedState)
        {
           
            base.OnBeforeInstall(savedState);
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            Form1 frm = new Form1(Context);
            frm.ShowDialog();
            base.OnAfterInstall(savedState);
        }

        protected override void OnCommitted(IDictionary savedState)
        {
            //Form1 frm = new Form1();
            //frm.ShowDialog();
            base.OnCommitted(savedState);
        }
    }
}
