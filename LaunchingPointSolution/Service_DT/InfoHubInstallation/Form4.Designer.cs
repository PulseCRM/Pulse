namespace PulseInstallation
{
    partial class Form4
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
            this.txtADImportInterval = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAdminPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtADAdminLogin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtADServerHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
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
            // txtADImportInterval
            // 
            this.txtADImportInterval.Location = new System.Drawing.Point(145, 214);
            this.txtADImportInterval.Name = "txtADImportInterval";
            this.txtADImportInterval.Size = new System.Drawing.Size(81, 20);
            this.txtADImportInterval.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "AD Import Interval:";
            // 
            // txtAdminPassword
            // 
            this.txtAdminPassword.Location = new System.Drawing.Point(145, 178);
            this.txtAdminPassword.Name = "txtAdminPassword";
            this.txtAdminPassword.Size = new System.Drawing.Size(270, 20);
            this.txtAdminPassword.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 182);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Admin Password:";
            // 
            // txtADAdminLogin
            // 
            this.txtADAdminLogin.Location = new System.Drawing.Point(145, 145);
            this.txtADAdminLogin.Name = "txtADAdminLogin";
            this.txtADAdminLogin.Size = new System.Drawing.Size(270, 20);
            this.txtADAdminLogin.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "AD Admin Login:";
            // 
            // txtDomain
            // 
            this.txtDomain.Location = new System.Drawing.Point(145, 112);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(270, 20);
            this.txtDomain.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Domain:";
            // 
            // txtADServerHost
            // 
            this.txtADServerHost.Location = new System.Drawing.Point(145, 79);
            this.txtADServerHost.Name = "txtADServerHost";
            this.txtADServerHost.Size = new System.Drawing.Size(270, 20);
            this.txtADServerHost.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "AD Server/IP:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(237, 218);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Hours";
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 382);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtADImportInterval);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtAdminPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtADAdminLogin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDomain);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtADServerHost);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nav1);
            this.Name = "Form4";
            this.Controls.SetChildIndex(this.nav1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtADServerHost, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtDomain, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtADAdminLogin, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txtAdminPassword, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtADImportInterval, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Nav nav1;
        private System.Windows.Forms.TextBox txtADImportInterval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAdminPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtADAdminLogin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDomain;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtADServerHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
    }
}