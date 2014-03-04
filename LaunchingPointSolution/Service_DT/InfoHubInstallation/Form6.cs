using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PulseInstallation
{
    public partial class Form6 : BaseFrm
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            lblPulseSQLServer.Text = Params.SQLServer;
            lblDbName.Text = Params.PulseDBName;
            lblADServer.Text = Params.ServiceHost;
            lblADAdminUsername.Text = Params.ADAdminLogin;
            lblServiceHostPort.Text = Params.ServicePortNumber;
            lblImportUserInterval.Text = Params.ADImportInterval.ToString();
            lblADDomain.Text = Params.Domain;
            lblPointImportInterval.Text = Params.ScheduledImportInterval.ToString();
            lblCardexFile.Text = Params.CardexFile;
            lblPointCentralDBName.Text = Params.PointCentralDBName;
            lblPointCentralSQLServer.Text = Params.PontCentralSQLServer;
            lblCardexFile.Text = Params.CardexFile;
            chkPointCentralEnabled.Checked = Params.PointCentralEnabled;
        }

        private void lblCardexFile_Click(object sender, EventArgs e)
        {

        }
    }
}
