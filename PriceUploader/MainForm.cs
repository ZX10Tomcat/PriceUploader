using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriceUploader
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void smiExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {




        }

        private void smiSettings_Click(object sender, EventArgs e)
        {
            this.tcMain.SelectedIndex = 1;
            
        }

        private void smiImportPrices_Click(object sender, EventArgs e)
        {
            this.tcMain.SelectedIndex = 0;
        }


    }
}
