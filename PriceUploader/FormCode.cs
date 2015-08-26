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
    public partial class FormCode : Form
    {
        public string CodeValue
        {
            get { return textBoxCode.Text; }
        }

        public FormCode()
        {
            InitializeComponent();          
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBoxCode_VisibleChanged(object sender, EventArgs e)
        {
            textBoxCode.Text = "";
        }
    }
}
