using System;
using System.Collections.Generic;
using System.Configuration;
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
            BackFrm = null;
            NextFrm = new Form7();
            NextFrm.BackFrm = this;
            this.nav1.HightLight(4);
        }


        /// <summary>
        /// Called when [next].
        /// </summary>
        /// <returns></returns>
        public override bool OnNext()
        {
            string strDataTracPath = string.Empty;
            string strDataTracLogin = string.Empty;
            string strDataTracPassword = string.Empty;
            strDataTracPath = txtDataTracServerPath.Text.Trim();
            strDataTracLogin = txtDataTracLogin.Text.Trim();
            strDataTracPassword = txtDataTracPassword.Text.Trim();
            SaveConfig(strDataTracPath, strDataTracLogin, strDataTracPassword);
            return true;
        }
        /// <summary>
        /// Saves the config.
        /// </summary>
        /// <param name="strDataTracPath">The DataTrac Path.</param>
        /// <param name="strDataTracLogin">The DataTrac Login.</param>
        /// <param name="strDataTracPassword">The DataTrac Password.</param>
        private void SaveConfig(string strDataTracPath, string strDataTracLogin, string strDataTracPassword)
        {
            Configuration config = GetConfig();
            if (config != null)
            {
                config.AppSettings.Settings["DataTracServerPath"].Value = strDataTracPath;
                config.AppSettings.Settings["DataTracLoginName"].Value = strDataTracLogin;
                config.AppSettings.Settings["DataTracLoginPwd"].Value = strDataTracPassword;
                config.Save();
            }
        }

    }
}
