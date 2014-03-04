using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
            NextFrm.BackFrm = this;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.nav1.HightLight(1);
#if test
            txtSQLServer.Text = @".\sqlexpress";
            txtPulseDBLogin.Text = "sa";
            txtPulseDBPassword.Text = "test@123";
            txtServiceHost.Text = "localhost";
            txtPulseDBName.Text = "LP";
#endif

        }

        public override bool OnNext()
        {
            Params.SQLServer = this.txtSQLServer.Text;
            Params.PulseDBLogin = this.txtPulseDBLogin.Text;
            Params.PulseDBPassword = this.txtPulseDBPassword.Text;
            Params.PulseDBName = this.txtPulseDBName.Text;
            Params.ServiceHost = this.txtServiceHost.Text;
            Params.ServicePortNumber = this.txtServicePortNumber.Text;
            string newConnString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;user={2};password={3};Pooling=true", Params.SQLServer, Params.PulseDBName, Params.PulseDBLogin, Params.PulseDBPassword);
            string newServiceBaseAddress = string.Format("http://{0}:{1}/InfoHubService/", Params.ServiceHost, Params.ServicePortNumber);
#if test
            MessageBox.Show(newConnString);
            MessageBox.Show(newServiceBaseAddress);
#endif
            if (CheckDb(newConnString))
            {
                UpdateConfig(newConnString, newServiceBaseAddress);
                Form4 formNext=new Form4();
                formNext.OUFilter= GetOuFilter(newConnString);
                NextFrm = formNext;
                return true;
            }
            MessageBox.Show("Database settings are invalid.");
            return false;
        }
        private bool CheckDb(string conn)
        {
            var sqlConnection = new SqlConnection(conn);
            try
            {
                sqlConnection.Open();
                return true;
            }
            catch (Exception exception)
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        private string GetOuFilter(string strConn)
        {
            //select top 1 AD_OU_Filter from dbo.Company_General
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.CommandText = "select top 1 AD_OU_Filter from dbo.Company_General";
                    command.CommandType = CommandType.Text;
                    object val = command.ExecuteScalar();
                    if (val != null)
                        return Convert.ToString(val);
                }
            }
            return string.Empty;
        }
        private void UpdateConfig(string newConnString, string newServiceBaseAddress)
        {
            Configuration config = GetConfig();

            if (config != null)
            {
                //start config
                //string newConnString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;user={2};password={3};Pooling=true", Params.SQLServer, Params.PulseDBLogin, Params.PulseDBLogin, Params.PulseDBPassword);
                config.ConnectionStrings.ConnectionStrings["focusITConnectionString"].ConnectionString = newConnString;

                //start service url
                //"http://localhost:8731/InfoHubService/"
                //string newServiceBaseAddress = string.Format("{0}:{1}/InfoHubService/", Params.ServiceHost, Params.ServicePortNumber);
                config.AppSettings.Settings["ServiceBaseAddress"].Value = newServiceBaseAddress;

                config.Save();
            }
        }

        private void txtServicePortNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
