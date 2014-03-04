using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PulseInstallation
{
    public partial class Nav : UserControl
    {
        public Nav()
        {
            InitializeComponent();
        }

        private void Nav_Load(object sender, EventArgs e)
        {

        }
        public void HightLight(int index)
        {
            ++index;
            Font nfont = null;
            switch (index)
            {
                case 1:
                    nfont = this.L1.Font;
                    nfont = new Font(nfont, FontStyle.Bold);
                    this.L1.Font = nfont;
                    break;
                case 2:
                    nfont = this.L2.Font;
                    nfont = new Font(nfont, FontStyle.Bold);
                    this.L2.Font = nfont;
                    break;
                case 3:
                    nfont = this.L3.Font;
                    nfont = new Font(nfont, FontStyle.Bold);
                    this.L3.Font = nfont;
                    break;
                case 4:
                    nfont = this.L4.Font;
                    nfont = new Font(nfont, FontStyle.Bold);
                    this.L4.Font = nfont;
                    break;
                case 5:
                    nfont = this.L5.Font;
                    nfont = new Font(nfont, FontStyle.Bold);
                    this.L5.Font = nfont;
                    break;
                case 6:
                    nfont = this.L6.Font;
                    nfont = new Font(nfont, FontStyle.Bold);
                    this.L6.Font = nfont;
                    break;

            }
        }
    }
}
