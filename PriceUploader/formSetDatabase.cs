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
            Cursor.Current = Cursors.WaitCursor;
            
            string StrDatabase = this.textBoxDatabase.Text;
            string StrPassword = this.textBoxPassword.Text;
            string StrPort = this.textBoxPort.Text;
            string StrServer = this.textBoxServer.Text;
            string StrUserId = this.textBoxUserID.Text;

            if (this.priceModel.CheckConnect(StrDatabase, StrServer, StrUserId, StrPassword, StrPort) < 0)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Ошибка в настройках доступа к БД", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.priceModel.StrDatabase = StrDatabase;
            this.priceModel.StrPassword = StrPassword;
            this.priceModel.StrPort = StrPort;
            this.priceModel.StrServer = StrServer;
            this.priceModel.StrUserId = StrUserId;

            if (this.priceModel.SaveDatabaseSettings() < 0)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Ошибка сохранения настроек доступа к БД", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Настройки доступа к БД сохранены успешно");
                this.Close();
            }
                
            
        }
       
    }
}
