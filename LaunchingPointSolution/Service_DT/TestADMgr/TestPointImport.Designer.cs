namespace TestADMgr
{
    partial class TestPointImport
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
            this.btnImport = new System.Windows.Forms.Button();
            this.rtbMsg = new System.Windows.Forms.RichTextBox();
            this.comboCommand = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txFileId = new System.Windows.Forms.TextBox();
            this.comboStages = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbReqData = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(603, 112);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(81, 30);
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "Go";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // rtbMsg
            // 
            this.rtbMsg.Location = new System.Drawing.Point(24, 168);
            this.rtbMsg.Name = "rtbMsg";
            this.rtbMsg.Size = new System.Drawing.Size(691, 549);
            this.rtbMsg.TabIndex = 4;
            this.rtbMsg.Text = "";
            // 
            // comboCommand
            // 
            this.comboCommand.FormattingEnabled = true;
            this.comboCommand.Location = new System.Drawing.Point(187, 112);
            this.comboCommand.Name = "comboCommand";
            this.comboCommand.Size = new System.Drawing.Size(381, 28);
            this.comboCommand.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "File Id:";
            // 
            // txFileId
            // 
            this.txFileId.Location = new System.Drawing.Point(86, 16);
            this.txFileId.Name = "txFileId";
            this.txFileId.Size = new System.Drawing.Size(100, 26);
            this.txFileId.TabIndex = 7;
            // 
            // comboStages
            // 
            this.comboStages.FormattingEnabled = true;
            this.comboStages.Location = new System.Drawing.Point(315, 11);
            this.comboStages.Name = "comboStages";
            this.comboStages.Size = new System.Drawing.Size(253, 28);
            this.comboStages.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(237, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Stage:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "Command:";
            // 
            // tbReqData
            // 
            this.tbReqData.Location = new System.Drawing.Point(315, 62);
            this.tbReqData.Name = "tbReqData";
            this.tbReqData.Size = new System.Drawing.Size(253, 26);
            this.tbReqData.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(177, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "Request Data:";
            // 
            // TestPointImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 746);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbReqData);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboStages);
            this.Controls.Add(this.txFileId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboCommand);
            this.Controls.Add(this.rtbMsg);
            this.Controls.Add(this.btnImport);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TestPointImport";
            this.Text = "Form3";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.RichTextBox rtbMsg;
        private System.Windows.Forms.ComboBox comboCommand;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txFileId;
        private System.Windows.Forms.ComboBox comboStages;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbReqData;
        private System.Windows.Forms.Label label4;
    }
}