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
    public partial class FormSetDatabase : Form
    {
        private PriceModel priceModel = null;

        public FormSetDatabase()
        {
            InitializeComponent();
        }

        public void Init(ref PriceModel priceModel)
        {
            this.priceModel = priceModel;

            this.textBoxDatabase.Text = this.priceModel.StrDatabase;
            this.textBoxPassword.Text = this.priceModel.StrPassword;
            this.textBoxPort.Text = this.priceModel.StrPort;
            this.textBoxServer.Text = this.priceModel.StrServer;
            this.textBoxUserID.Text = this.priceModel.StrUserId;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.priceModel.StrDatabase = this.textBoxDatabase.Text;
            this.priceModel.StrPassword = this.textBoxPassword.Text;
            this.priceModel.StrPort = this.textBoxPort.Text;
            this.priceModel.StrServer = this.textBoxServer.Text;
            this.priceModel.StrUserId = this.textBoxUserID.Text;
            
            this.priceModel.SaveDatabaseSettings();
            
            this.Close();
        }

       
    }
}
