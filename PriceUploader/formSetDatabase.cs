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
    public partial class formSetDatabase : Form
    {
        private PriceModel priceModel = null;

        public formSetDatabase()
        {
            InitializeComponent();
        }

        public void Init(ref PriceModel priceModel)
        {
            this.priceModel = priceModel;

            this.textBoxDatabase.Text = this.priceModel.GetDatabase();
            this.textBoxPassword.Text = this.priceModel.GetPass();
            this.textBoxPort.Text = this.priceModel.GetPort();
            this.textBoxServer.Text = this.priceModel.GetServer();
            this.textBoxUserID.Text = this.priceModel.GetUserId();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.priceModel.GetStrConn( 
                this.textBoxServer.Text,
                this.textBoxUserID.Text,
                this.textBoxPassword.Text,
                this.textBoxPort.Text);

            this.priceModel.GetStrDataBase( this.textBoxDatabase.Text);

            this.priceModel.SaveDatabasSettings();
            
            this.Close();
        }

       
    }
}
