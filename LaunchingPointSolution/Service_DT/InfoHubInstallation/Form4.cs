using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PulseInstallation
{
    public partial class Form4 : BaseFrm
    {
        public Form4()
        {
            InitializeComponent();
            BackFrm = null;
            NextFrm = new Form5();
            NextFrm.Context = this.Context;
            NextFrm.BackFrm = this;
            this.nav1.HightLight(2);
        }
        public override bool OnNext()
        {
            Params.ADServerHost = txtADServerHost.Text;
            Params.Domain = txtDomain.Text;
            Params.ADAdminLogin = txtADAdminLogin.Text;
            Params.AdminPassword = txtAdminPassword.Text;
            if (!string.IsNullOrEmpty(txtADImportInterval.Text.Trim()))
            {
                Params.ADImportInterval = Convert.ToDecimal(txtADImportInterval.Text);
            }
            return base.OnNext();
        }
    }
}
