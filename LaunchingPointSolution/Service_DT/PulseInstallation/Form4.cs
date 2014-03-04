using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PulseInstallation
{
    public partial class Form4 : BaseFrm
    {
        private string _newConnString;
        private string _oUFilter;

        public Form4()
        {
            InitializeComponent();
            BackFrm = null;
            NextFrm = new Form5();
            NextFrm.BackFrm = this;
            nav1.HightLight(2);

            _newConnString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;user={2};password={3};Pooling=true", Params.SQLServer, Params.PulseDBName, Params.PulseDBLogin, Params.PulseDBPassword);
            //OUFilter = GetOuFilter(_newConnString);
            this.txtOUFilter.Text = OUFilter;
        }

        public string OUFilter
        {
            get { return _oUFilter; }
            set { _oUFilter = value; }
        }

        /// <summary>
        /// Called when [next].
        /// </summary>
        /// <returns></returns>
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
#if test
            MessageBox.Show(newConnString);
#endif
            if (string.IsNullOrEmpty(OUFilter))
            {
                MessageBox.Show("Can't not get OU Filter");
                return false;
            }

            if (Params.ADImportInterval > 0)
            {
                UpdateInterval(_newConnString, Params.ADImportInterval);
            }

            SaveConfig(OUFilter);

            if (!string.Equals(txtOUFilter.Text, OUFilter, StringComparison.CurrentCultureIgnoreCase))
            {
                UpdateOUFilter(_newConnString, txtOUFilter.Text);
            }

            return true;
        }
        /// <summary>
        /// Gets the ou filter.
        /// </summary>
        /// <param name="strConn">The STR conn.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Updates the interval.
        /// </summary>
        /// <param name="strConn">The STR conn.</param>
        /// <param name="val">The val.</param>
        private void UpdateInterval(string strConn, decimal val)
        {
            //select top 1 AD_OU_Filter from dbo.Company_General
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.CommandText = "update dbo.Company_General set ImportUserInterval=@ImportUserInterval";
                    SqlParameter parameter = new SqlParameter("@ImportUserInterval", SqlDbType.Decimal);
                    parameter.Value = val;
                    command.Parameters.Add(parameter);
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }
        private void UpdateOUFilter(string strConn, string oUfilter)
        {
            //select top 1 AD_OU_Filter from dbo.Company_General
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.CommandText = "update dbo.Company_General set AD_OU_Filter=@AD_OU_Filter";
                    SqlParameter parameter = new SqlParameter("@AD_OU_Filter", SqlDbType.NVarChar);
                    parameter.Value = oUfilter;
                    command.Parameters.Add(parameter);
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Saves the config.
        /// </summary>
        /// <param name="oUFilter">The o U filter.</param>
        private void SaveConfig(string oUFilter)
        {
            Configuration config = GetConfig();
            if (config != null)
            {
                config.AppSettings.Settings["LDAPPassword"].Value = Params.AdminPassword;
                config.AppSettings.Settings["LDAPPath"].Value = string.Format("LDAP://{0}", Params.ADServerHost);
                config.AppSettings.Settings["LDAPUser"].Value = Params.ADAdminLogin;
                config.AppSettings.Settings["Domain"].Value = Params.Domain;
                config.AppSettings.Settings["OU"].Value = oUFilter;

                //<add key="LDAPPassword" value="$PS2010"/>
                //<add key="LDAPPath" value="LDAP://192.168.253.16"/>
                //<add key="LDAPUser" value="SPSADMIN"/>
                //<add key="Domain" value="focus.internal"/>
                //<add key="OU" value="SPSTEST"/>
                config.Save();
            }
        }

        private void txtADImportInterval_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void Form4_VisibleChanged(object sender, EventArgs e)
        {
            this.txtOUFilter.Text = OUFilter;
        }
    }
}
