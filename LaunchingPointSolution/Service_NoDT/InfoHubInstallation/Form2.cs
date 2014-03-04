using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PulseInstallation
{
    public partial class Form2 : BaseFrm
    {
        public Form2()
        {
            InitializeComponent();
            BackFrm = null;
            NextFrm = new Form3();
            this.nav1.HightLight(0);
            NextFrm.BackFrm = this;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (DialogResult.OK == dr)
            {
                txtInstallFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        public override bool OnNext()
        {
            Params.InstallFolder = txtInstallFolder.Text;
            return base.OnNext();
        }
    }
}
