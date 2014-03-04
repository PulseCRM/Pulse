namespace PulseInstallation
{
    partial class Form3
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtSQLServer = new System.Windows.Forms.TextBox();
            this.txtPulseDBLogin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPulseDBPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPulseDBName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtServiceHost = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtServicePortNumber = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // nav1
            // 
            this.nav1.Location = new System.Drawing.Point(12, 23);
            this.nav1.Name = "nav1";
            this.nav1.Size = new System.Drawing.Size(480, 30);
            this.nav1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "SQL Server:";
            // 
            // txtSQLServer
            // 
            this.txtSQLServer.Location = new System.Drawing.Point(145, 84);
            this.txtSQLServer.Name = "txtSQLServer";
            this.txtSQLServer.Size = new System.Drawing.Size(270, 20);
            this.txtSQLServer.TabIndex = 2;
            // 
            // txtPulseDBLogin
            // 
            this.txtPulseDBLogin.Location = new System.Drawing.Point(145, 117);
            this.txtPulseDBLogin.Name = "txtPulseDBLogin";
            this.txtPulseDBLogin.Size = new System.Drawing.Size(270, 20);
            this.txtPulseDBLogin.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Pulse DB Login:";
            // 
            // txtPulseDBPassword
            // 
            this.txtPulseDBPassword.Location = new System.Drawing.Point(145, 150);
            this.txtPulseDBPassword.Name = "txtPulseDBPassword";
            this.txtPulseDBPassword.Size = new System.Drawing.Size(270, 20);
            this.txtPulseDBPassword.TabIndex = 6;
            this.txtPulseDBPassword.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Pulse DB Password:";
            // 
            // txtPulseDBName
            // 
            this.txtPulseDBName.Location = new System.Drawing.Point(145, 183);
            this.txtPulseDBName.Name = "txtPulseDBName";
            this.txtPulseDBName.Size = new System.Drawing.Size(270, 20);
            this.txtPulseDBName.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Pulse DB Name:";
            // 
            // txtServiceHost
            // 
            this.txtServiceHost.Location = new System.Drawing.Point(145, 216);
            this.txtServiceHost.Name = "txtServiceHost";
            this.txtServiceHost.Size = new System.Drawing.Size(270, 20);
            this.txtServiceHost.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Service Host/IP:";
            // 
            // txtServicePortNumber
            // 
            this.txtServicePortNumber.Location = new System.Drawing.Point(145, 249);
            this.txtServicePortNumber.Name = "txtServicePortNumber";
            this.txtServicePortNumber.Size = new System.Drawing.Size(270, 20);
            this.txtServicePortNumber.TabIndex = 12;
            this.txtServicePortNumber.Text = "8731";
            this.txtServicePortNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtServicePortNumber_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 251);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Service Port Number:";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 382);
            this.Controls.Add(this.txtServicePortNumber);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtServiceHost);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPulseDBName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPulseDBPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPulseDBLogin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSQLServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nav1);
            this.Name = "Form3";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.Controls.SetChildIndex(this.nav1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtSQLServer, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtPulseDBLogin, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtPulseDBPassword, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txtPulseDBName, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtServiceHost, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtServicePortNumber, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Nav nav1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSQLServer;
        private System.Windows.Forms.TextBox txtPulseDBLogin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPulseDBPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPulseDBName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtServiceHost;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtServicePortNumber;
        private System.Windows.Forms.Label label6;
    }
}