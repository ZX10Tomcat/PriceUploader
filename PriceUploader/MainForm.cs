using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriceUploader
{
    public partial class MainForm : Form
    {
        private PriceModel Model = null;
        private DataTable TableCategoryCharge = null; 
        //private DataTable TableImportSettings = null;
        private DataTable TablePriceCategory = null;
        private DataTable TableProduct = null;
        private DataTable TableProductAlias = null;
        private DataTable TableProductCategory = null;
        private DataTable TableProductPrice = null;
        private DataTable TableSupplier = null; 

        public MainForm()
        {
            InitializeComponent();
            Init();
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void toolStripMenuItemSettings_Click(object sender, EventArgs e)
        {
            this.tabControlMain.SelectedIndex = 1;
            
        }

        private void toolStripMenuItemImportPrices_Click(object sender, EventArgs e)
        {
            this.tabControlMain.SelectedIndex = 0;
        }

        public void Init()
        {
            Model = new PriceModel();

            DateTime d1 = DateTime.Now;

            //int rowsCategoryCharge = Model.Load_category_charge(ref TableCategoryCharge);
            //int rowsImportSettings = Model.Load_import_settings(ref TableImportSettings);
            //int rowsPriceCategory = Model.Load_price_category(ref TablePriceCategory);
            //int rowsProduct = Model.Load_product(ref TableProduct);
            //int rowsProductAlias = Model.Load_product_alias(ref TableProductAlias);
            //int rowsProductCategory = Model.Load_product_category(ref TableProductCategory);
            //int rowsProductPrice = Model.Load_product_price(ref TableProductPrice);
            //int rowsSupplier = Model.Load_supplier(ref TableSupplier);

            Model.Load_category_charge().ContinueWith(res => 
            {
                TableCategoryCharge = res.Result;
                Debug.WriteLine("           TableCategoryCharge: " + TableCategoryCharge.Rows.Count.ToString());
            });

            Model.Load_import_settings().ContinueWith(res =>
            {
                SetDataTableByRows(res, "Table_import_settings");                
            });

            Model.Load_price_category().ContinueWith(res =>
            {
                TablePriceCategory = res.Result;
                Debug.WriteLine("           TablePriceCategory: " + TablePriceCategory.Rows.Count.ToString());
            });

            Model.Load_product().ContinueWith(res =>
            {
                TableProduct = res.Result;
                Debug.WriteLine("           TableProduct: " + TableProduct.Rows.Count.ToString());
            });

            Model.Load_product_alias().ContinueWith(res =>
            {
                TableProductAlias = res.Result;
                Debug.WriteLine("           TableProductAlias: " + TableProductAlias.Rows.Count.ToString());
            });

            Model.Load_product_category().ContinueWith(res =>
            {
                TableProductCategory = res.Result;
                Debug.WriteLine("           TableProductCategory: " + TableProductCategory.Rows.Count.ToString());
            });

            Model.Load_product_price().ContinueWith(res =>
            {
                TableProductPrice = res.Result;
                Debug.WriteLine("           TableProductPrice: " + TableProductPrice.Rows.Count.ToString());
            });

            Model.Load_supplier().ContinueWith(res =>
            {
                TableSupplier = res.Result;
                Debug.WriteLine("           TableSupplier: " + TableSupplier.Rows.Count.ToString());
            });

            DateTime d2 = DateTime.Now;
            TimeSpan timeout = d2 - d1;

            Debug.WriteLine("time data loaded: " + timeout.ToString());
            return;
        }

        private void SetDataTableByRows(Task<DataTable> res, string tableName)
        {
            dataSet.Tables[tableName].Clear();
            for (int i = 0; i < res.Result.Rows.Count; i++)
            {
                DataRow dr = dataSet.Tables[tableName].NewRow();
                for (int n = 0; n < dr.ItemArray.ToList().Count; n++)
                    dr[n] = res.Result.Rows[i].ItemArray[n];

                dataSet.Tables[tableName].Rows.Add(dr);
            }
        }



        private void toolStripMenuItemDatabaseSettings_Click(object sender, EventArgs e)
        {
            OpenSetDatabaseDialog();
        }

       
        private void buttonDatabaseSettings_Click(object sender, EventArgs e)
        {
            OpenSetDatabaseDialog();
        }

        private void OpenSetDatabaseDialog()
        {
            FormSetDatabase formSetDatabase = new FormSetDatabase();
            formSetDatabase.Init(ref Model);
            formSetDatabase.ShowDialog();
        }

        private void bindingSource__import_settings_CurrentChanged(object sender, EventArgs e)
        {

        }


        private void dataGrid_import_settings_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e != null)
            {
                var v = dataSet.Tables["Table_import_settings"].Rows[e.RowIndex];                
                textBoxFirstRow.Text = v.ItemArray[0].ToString();
                textBoxName.Text = v.ItemArray[1].ToString();
                textBoxCode.Text = v.ItemArray[2].ToString();
                textBoxPrice.Text = v.ItemArray[3].ToString();
                textBoxProductName.Text = v.ItemArray[4].ToString();
                textBoxAvailability1.Text = v.ItemArray[5].ToString();
                textBoxAvailability2.Text = v.ItemArray[6].ToString();
                textBoxCurrency.Text = v.ItemArray[7].ToString();
                textBoxAvailSign.Text = v.ItemArray[8].ToString();
                textBoxPriceGrn.Text = v.ItemArray[9].ToString();
                textBoxActuality.Text = v.ItemArray[10].ToString();

            }
            
            return;
        }


    }
}
