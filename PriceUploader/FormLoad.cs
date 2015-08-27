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
    public partial class FormLoad : Form
    {
        public FormLoad()
        {
            InitializeComponent();
        }

        public string CurrentTask
        {
            get 
            { 
                return this.labelTask.Text; 
            }

            set
            {
                this.labelTask.Text = value;
                this.Refresh();
            }
        }
    }
}
