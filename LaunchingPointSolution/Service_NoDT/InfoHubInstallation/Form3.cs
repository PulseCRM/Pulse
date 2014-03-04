using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Windows.Forms;

namespace PulseInstallation
{
    public partial class Form3 : BaseFrm
    {
        public Form3()
        {
            InitializeComponent();
            BackFrm = null;
            NextFrm = new Form4();
            NextFrm.Context = this.Context;
            NextFrm.BackFrm = this;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.nav1.HightLight(1);
        }
        public override bool OnNext()
        {
            Params.SQLServer = this.txtSQLServer.Text;
            Params.PulseDBLogin = this.txtPulseDBLogin.Text;
            Params.PulseDBPassword = this.txtPulseDBPassword.Text;
            Params.PulseDBName = this.txtPulseDBName.Text;
            Params.ServiceHost = this.txtServiceHost.Text;
            Params.ServicePortNumber = this.txtServicePortNumber.Text;
            MessageBox.Show(Context.Parameters["assemblypath"]);
            return base.OnNext();
        }
        private void CreateConnectionString()
        {

            //Assembly ass = Assembly.GetExecutingAssembly();
            //Stream stmConfig = ass.GetManifestResourceStream(
            //               ass.GetName().Name + ".Web.config");

            //if (!Directory.Exists(Context.Parameters["Folder"]))
            //    Directory.CreateDirectory(Context.Parameters["Folder"]);

            //FileStream stmPhysical = new FileStream(
            //    Context.Parameters["Folder"] + @"\Web.config",
            //    FileMode.Create);
            //StreamReader srConfig = new StreamReader(stmConfig);
            //StreamWriter swConfig = new StreamWriter(stmPhysical);

            //string strConfig = srConfig.ReadToEnd();
            //stmConfig.Close();
            //strConfig = strConfig.Replace("server=(local);database" +
            //          "=DatabaseName;User ID=sa;Password=;" +
            //          "trusted_connection=false", NewConnection());


            //swConfig.Write(strConfig);
            //swConfig.Close();
            //stmPhysical.Close();

        }
    }
}
