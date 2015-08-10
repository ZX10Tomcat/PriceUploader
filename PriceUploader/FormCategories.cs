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
    public partial class FormCategories : Form
    {
        public FormCategories()
        {
            InitializeComponent();
        }
                
        internal void Init(DataTable TableProductCategory)
        {
            foreach (var item in TableProductCategory.AsEnumerable())
            {

            }
        }
    }
}
