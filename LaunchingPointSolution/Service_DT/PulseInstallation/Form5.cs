using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
            NextFrm.BackFrm = this;
            this.nav1.HightLight(3);
            chkPointCentralEnabled_CheckedChanged(new object(), new EventArgs());
        }
        public override bool OnNext()
        {
            Params.PointCentralEnabled = chkPointCentralEnabled.Checked;
            Params.PontCentralSQLServer = txtPontCentralSQLServer.Text.Trim();
            Params.PointCentralDBLogin = txtPointCentralDBLogin.Text.Trim();
            Params.PointCentralDBPassword = txtPointCentralDBPassword.Text.Trim();
            Params.PointCentralDBName = txtPointCentralDBName.Text.Trim();
            Params.WinPointIni = txtWinPointIni.Text.Trim();
            if (!string.IsNullOrEmpty(txtScheduledImportInterval.Text.Trim()))
            {
                Params.ScheduledImportInterval = Convert.ToDecimal(txtScheduledImportInterval.Text.Trim());
            }
            Params.CardexFile = txtCardexFile.Text.Trim();

            string newConnString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;user={2};password={3};Pooling=true", Params.SQLServer, Params.PulseDBName, Params.PulseDBLogin, Params.PulseDBPassword);
            try
            {
                UpdateInformation2Db(newConnString);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            if (chkPointCentralEnabled.Checked)
            {
                string conn = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;user={2};password={3};Pooling=true", Params.PontCentralSQLServer, Params.PointCentralDBName, Params.PointCentralDBLogin, Params.PointCentralDBPassword);
                UpdateConfig(conn);
            }

            return true;
        }

        private void chkPointCentralEnabled_CheckedChanged(object sender, EventArgs e)
        {
            txtPontCentralSQLServer.Enabled = chkPointCentralEnabled.Checked;
            txtPointCentralDBLogin.Enabled = chkPointCentralEnabled.Checked;
            txtPointCentralDBPassword.Enabled = chkPointCentralEnabled.Checked;
            txtPointCentralDBName.Enabled = chkPointCentralEnabled.Checked;

            txtWinPointIni.Enabled = btnWinPointIni.Enabled = !chkPointCentralEnabled.Checked;

            txtScheduledImportInterval.Enabled = chkPointCentralEnabled.Checked;
            txtCardexFile.Enabled = btnCardexFile.Enabled = chkPointCentralEnabled.Checked;
        }

        private void btnWinPointIni_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\WINDOWS\";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FilterIndex = 1;
            DialogResult dr = openFileDialog1.ShowDialog();
            if (DialogResult.OK == dr)
            {
                txtWinPointIni.Text = openFileDialog1.FileName;
            }
        }

        private void btnCardexFile_Click(object sender, EventArgs e)
        {
            openFileDialog2.InitialDirectory = @"C:\PNTTEMPL\database\";
            openFileDialog2.RestoreDirectory = true;
            openFileDialog1.FilterIndex = 1;
            DialogResult dr = openFileDialog2.ShowDialog();
            if (DialogResult.OK == dr)
            {
                txtCardexFile.Text = openFileDialog2.FileName;
            }
        }

        private void UpdateInformation2Db(string strConn)
        {

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"IF EXISTS(SELECT * FROM dbo.Company_Point)
	                                            BEGIN
		                                            UPDATE dbo.Company_Point set WinpointIniPath=@WinpointIniPath
	                                            END
                                            ELSE
                                                BEGIN
		                                            INSERT INTO dbo.Company_Point(WinpointIniPath) VALUES (@WinpointIniPath)
                                                END";
                    SqlParameter parameter = new SqlParameter("@WinpointIniPath", SqlDbType.NVarChar);
                    parameter.Value = Params.WinPointIni;
                    command.Parameters.Add(parameter);

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Save WinpointIniPath faild");
                    }

                    command.CommandText = @"IF EXISTS(SELECT * FROM dbo.Company_Point)
	                                            BEGIN
		                                            UPDATE DBO.COMPANY_POINT SET PointImportIntervalMinutes=@PointImportIntervalMinutes
	                                            END
                                            ELSE
                                                BEGIN
		                                            INSERT INTO dbo.Company_Point(PointImportIntervalMinutes) VALUES (@PointImportIntervalMinutes)
                                                END";
                    parameter = new SqlParameter("@PointImportIntervalMinutes", SqlDbType.Int);
                    parameter.Value = Convert.ToInt32(Params.ScheduledImportInterval);
                    command.Parameters.Clear();
                    command.Parameters.Add(parameter);

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Save PointImportIntervalMinutes faild");
                    }

                    command.CommandText = @"IF EXISTS(SELECT * FROM dbo.Company_Point)
	                                        BEGIN		    
		                                        UPDATE dbo.Company_Point set CardexFile=@CardexFile
	                                        END
                                        ELSE
                                            BEGIN
		                                        INSERT INTO dbo.Company_Point(CardexFile) VALUES (@CardexFile)
                                            END
                                        ";
                    parameter = new SqlParameter("@CardexFile", SqlDbType.NVarChar);
                    parameter.Value = Params.CardexFile;
                    command.Parameters.Clear();
                    command.Parameters.Add(parameter);

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Save CardexFile faild");
                    }
                }
            }
        }
        private void UpdateConfig(string newConnString)
        {
            Configuration config = GetConfig();

            if (config != null)
            {
                //start config
                //string newConnString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;user={2};password={3};Pooling=true", Params.SQLServer, Params.PulseDBLogin, Params.PulseDBLogin, Params.PulseDBPassword);
                config.ConnectionStrings.ConnectionStrings["PointCentralDBString"].ConnectionString = newConnString;

                config.Save();
            }
        }

        private void txtScheduledImportInterval_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
