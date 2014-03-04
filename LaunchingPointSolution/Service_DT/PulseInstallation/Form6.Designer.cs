namespace PulseInstallation
{
    partial class Form6
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
            this.txtDataTracPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDataTracLogin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDataTracServerPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // nav1
            // 
            this.nav1.Location = new System.Drawing.Point(13, 13);
            this.nav1.Name = "nav1";
            this.nav1.Size = new System.Drawing.Size(480, 30);
            this.nav1.TabIndex = 0;
            // 
            // txtDataTracPassword
            // 
            this.txtDataTracPassword.Location = new System.Drawing.Point(143, 134);
            this.txtDataTracPassword.Name = "txtDataTracPassword";
            this.txtDataTracPassword.Size = new System.Drawing.Size(270, 20);
            this.txtDataTracPassword.TabIndex = 27;
            this.txtDataTracPassword.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "DataTrac Password:";
            // 
            // txtDataTracLogin
            // 
            this.txtDataTracLogin.Location = new System.Drawing.Point(143, 101);
            this.txtDataTracLogin.Name = "txtDataTracLogin";
            this.txtDataTracLogin.Size = new System.Drawing.Size(270, 20);
            this.txtDataTracLogin.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "DataTrac Login:";
            // 
            // txtDataTracServerPath
            // 
            this.txtDataTracServerPath.Location = new System.Drawing.Point(143, 68);
            this.txtDataTracServerPath.Name = "txtDataTracServerPath";
            this.txtDataTracServerPath.Size = new System.Drawing.Size(270, 20);
            this.txtDataTracServerPath.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "DataTrac Server Path:";
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 382);
            this.Controls.Add(this.txtDataTracPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDataTracLogin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDataTracServerPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nav1);
            this.Name = "Form6";
            this.Text = "Form6";
            this.Controls.SetChildIndex(this.nav1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtDataTracServerPath, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtDataTracLogin, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtDataTracPassword, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Nav nav1;
        private System.Windows.Forms.TextBox txtDataTracPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDataTracLogin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDataTracServerPath;
        private System.Windows.Forms.Label label1;
    }
}