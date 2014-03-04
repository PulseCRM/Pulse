namespace PulseInstallation
{
    partial class Form5
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.nav1 = new PulseInstallation.Nav();
            this.txtScheduledImportInterval = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtWinPointIni = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPointCentralDBName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPointCentralDBPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPointCentralDBLogin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPontCentralSQLServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCardexFile = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkPointCentralEnabled = new System.Windows.Forms.CheckBox();
            this.btnWinPointIni = new System.Windows.Forms.Button();
            this.btnCardexFile = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.label8 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // nav1
            // 
            this.nav1.Location = new System.Drawing.Point(13, 24);
            this.nav1.Name = "nav1";
            this.nav1.Size = new System.Drawing.Size(480, 30);
            this.nav1.TabIndex = 0;
            // 
            // txtScheduledImportInterval
            // 
            this.txtScheduledImportInterval.Location = new System.Drawing.Point(162, 257);
            this.txtScheduledImportInterval.Name = "txtScheduledImportInterval";
            this.txtScheduledImportInterval.Size = new System.Drawing.Size(113, 20);
            this.txtScheduledImportInterval.TabIndex = 27;
            this.txtScheduledImportInterval.Text = "30";
            this.txtScheduledImportInterval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtScheduledImportInterval_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 261);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "Scheduled Import Interval:";
            // 
            // txtWinPointIni
            // 
            this.txtWinPointIni.Location = new System.Drawing.Point(162, 224);
            this.txtWinPointIni.Name = "txtWinPointIni";
            this.txtWinPointIni.Size = new System.Drawing.Size(186, 20);
            this.txtWinPointIni.TabIndex = 25;
            this.txtWinPointIni.Text = "C:\\WINDOWS\\WINPOINT.INI";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 228);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "WinPoint.INI:";
            // 
            // txtPointCentralDBName
            // 
            this.txtPointCentralDBName.Location = new System.Drawing.Point(162, 191);
            this.txtPointCentralDBName.Name = "txtPointCentralDBName";
            this.txtPointCentralDBName.Size = new System.Drawing.Size(270, 20);
            this.txtPointCentralDBName.TabIndex = 23;
            this.txtPointCentralDBName.Text = "PDS";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Point Central DB Name:";
            // 
            // txtPointCentralDBPassword
            // 
            this.txtPointCentralDBPassword.Location = new System.Drawing.Point(162, 158);
            this.txtPointCentralDBPassword.Name = "txtPointCentralDBPassword";
            this.txtPointCentralDBPassword.Size = new System.Drawing.Size(270, 20);
            this.txtPointCentralDBPassword.TabIndex = 21;
            this.txtPointCentralDBPassword.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Point Central DB Password:";
            // 
            // txtPointCentralDBLogin
            // 
            this.txtPointCentralDBLogin.Location = new System.Drawing.Point(162, 125);
            this.txtPointCentralDBLogin.Name = "txtPointCentralDBLogin";
            this.txtPointCentralDBLogin.Size = new System.Drawing.Size(270, 20);
            this.txtPointCentralDBLogin.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Point Central  DB Login:";
            // 
            // txtPontCentralSQLServer
            // 
            this.txtPontCentralSQLServer.Location = new System.Drawing.Point(162, 92);
            this.txtPontCentralSQLServer.Name = "txtPontCentralSQLServer";
            this.txtPontCentralSQLServer.Size = new System.Drawing.Size(270, 20);
            this.txtPontCentralSQLServer.TabIndex = 17;
            this.txtPontCentralSQLServer.Text = "(local)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Point Central SQL Server:";
            // 
            // txtCardexFile
            // 
            this.txtCardexFile.Location = new System.Drawing.Point(91, 289);
            this.txtCardexFile.Name = "txtCardexFile";
            this.txtCardexFile.Size = new System.Drawing.Size(257, 20);
            this.txtCardexFile.TabIndex = 32;
            this.txtCardexFile.Text = "C:\\PNTTEMPL\\database\\POINTCDX.MDB";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 293);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 31;
            this.label7.Text = "Cardex File:";
            // 
            // chkPointCentralEnabled
            // 
            this.chkPointCentralEnabled.AutoSize = true;
            this.chkPointCentralEnabled.Location = new System.Drawing.Point(26, 61);
            this.chkPointCentralEnabled.Name = "chkPointCentralEnabled";
            this.chkPointCentralEnabled.Size = new System.Drawing.Size(128, 17);
            this.chkPointCentralEnabled.TabIndex = 33;
            this.chkPointCentralEnabled.Text = "Point Central Enabled";
            this.chkPointCentralEnabled.UseVisualStyleBackColor = true;
            this.chkPointCentralEnabled.CheckedChanged += new System.EventHandler(this.chkPointCentralEnabled_CheckedChanged);
            // 
            // btnWinPointIni
            // 
            this.btnWinPointIni.Location = new System.Drawing.Point(357, 222);
            this.btnWinPointIni.Name = "btnWinPointIni";
            this.btnWinPointIni.Size = new System.Drawing.Size(75, 23);
            this.btnWinPointIni.TabIndex = 34;
            this.btnWinPointIni.Text = "Broswe";
            this.btnWinPointIni.UseVisualStyleBackColor = true;
            this.btnWinPointIni.Click += new System.EventHandler(this.btnWinPointIni_Click);
            // 
            // btnCardexFile
            // 
            this.btnCardexFile.Location = new System.Drawing.Point(357, 289);
            this.btnCardexFile.Name = "btnCardexFile";
            this.btnCardexFile.Size = new System.Drawing.Size(75, 23);
            this.btnCardexFile.TabIndex = 35;
            this.btnCardexFile.Text = "Broswe";
            this.btnCardexFile.UseVisualStyleBackColor = true;
            this.btnCardexFile.Click += new System.EventHandler(this.btnCardexFile_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(281, 261);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "minutes";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.AutoUpgradeEnabled = false;
            this.openFileDialog1.Filter = "WinPoint.ini|*.ini|All files|*.*";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.AutoUpgradeEnabled = false;
            this.openFileDialog2.Filter = "Cardex File|*.MDB|All files|*.*";
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 382);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnCardexFile);
            this.Controls.Add(this.btnWinPointIni);
            this.Controls.Add(this.chkPointCentralEnabled);
            this.Controls.Add(this.txtCardexFile);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtScheduledImportInterval);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtWinPointIni);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPointCentralDBName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPointCentralDBPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPointCentralDBLogin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPontCentralSQLServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nav1);
            this.Name = "Form5";
            this.Controls.SetChildIndex(this.nav1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtPontCentralSQLServer, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtPointCentralDBLogin, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtPointCentralDBPassword, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txtPointCentralDBName, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtWinPointIni, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtScheduledImportInterval, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.txtCardexFile, 0);
            this.Controls.SetChildIndex(this.chkPointCentralEnabled, 0);
            this.Controls.SetChildIndex(this.btnWinPointIni, 0);
            this.Controls.SetChildIndex(this.btnCardexFile, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Nav nav1;
        private System.Windows.Forms.TextBox txtScheduledImportInterval;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtWinPointIni;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPointCentralDBName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPointCentralDBPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPointCentralDBLogin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPontCentralSQLServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCardexFile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkPointCentralEnabled;
        private System.Windows.Forms.Button btnWinPointIni;
        private System.Windows.Forms.Button btnCardexFile;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
    }
}