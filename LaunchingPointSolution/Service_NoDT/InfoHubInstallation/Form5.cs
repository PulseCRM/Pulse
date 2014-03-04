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
    public partial class Form5 : BaseFrm
    {
        public Form5()
        {
            InitializeComponent();
            BackFrm = null;
            NextFrm = new Form6();
            NextFrm.Context = this.Context;
            NextFrm.BackFrm = this;
            this.nav1.HightLight(3);
        }
        public override bool OnNext()
        {
            Params.PointCentralEnabled = chkPointCentralEnabled.Checked;
            Params.PontCentralSQLServer = txtPontCentralSQLServer.Text.Trim();
            Params.PointCentralDBLogin=txtPointCentralDBLogin.Text.Trim();
            Params.PointCentralDBPassword = txtPointCentralDBPassword.Text.Trim();
            Params.PointCentralDBName = txtPointCentralDBName.Text.Trim();
            Params.WinPointIni = txtWinPointIni.Text.Trim();
            if (!string.IsNullOrEmpty(txtScheduledImportInterval.Text.Trim()))
            {
                Params.ScheduledImportInterval = Convert.ToDecimal(txtScheduledImportInterval.Text.Trim());
            }
            Params.CardexFile = txtCardexFile.Text.Trim();
            return base.OnNext();
        }
    }
}
