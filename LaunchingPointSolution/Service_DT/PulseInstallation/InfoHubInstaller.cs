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
            BaseFrm frm = new Form1(Context);
            frm.ShowDialog();
#if test
            MessageBox.Show(BaseFrm.FrmDialogResult.ToString());
#endif
            if (BaseFrm.FrmDialogResult == DialogResult.Cancel)
            {
                DisposeForms(frm);
                throw new Exception("You cancelled installation");
            }
            DisposeForms(frm);
            base.OnAfterInstall(savedState);
        }

        private void DisposeForms(BaseFrm frm)
        {
            List<BaseFrm> frms = new List<BaseFrm>();
            GetAllForm(frm, frms);
            for (int i = frms.Count - 1; i >= 0; i--)
            {
                frms[i].Close();
                frms[i].Dispose();
            }
        }

        private void GetAllForm(BaseFrm start, List<BaseFrm> frms)
        {
            if (start != null)
            {
                BaseFrm baseFrm = start.NextFrm;
                if (baseFrm != null)
                {
                    frms.Add(baseFrm);
                    GetAllForm(baseFrm, frms);
                }
            }
        }
        protected override void OnCommitted(IDictionary savedState)
        {
            //Form1 frm = new Form1();
            //frm.ShowDialog();
            base.OnCommitted(savedState);
        }

    }
}
