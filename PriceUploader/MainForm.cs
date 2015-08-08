using LOffice;
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
        private DataTable TableExcelData = null; 
        private string[] Columns = new string[27];

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
                SetDataBindings();
                SetFormatComboBox(res);
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
                SetSupplierComboBox(res);                
            });

            DateTime d2 = DateTime.Now;
            TimeSpan timeout = d2 - d1;

            FillComboBoxes();            
            Debug.WriteLine("time data loaded: " + timeout.ToString());
            return;
        }

        private void FillComboBoxes()
        {
            Columns[0] = "-";
            Columns[1] = "A";
            Columns[2] = "B";
            Columns[3] = "C";
            Columns[4] = "D";
            Columns[5] = "E";
            Columns[6] = "F";
            Columns[7] = "G";
            Columns[8] = "H";
            Columns[9] = "I";
            Columns[10] = "J";
            Columns[11] = "K";
            Columns[12] = "L";
            Columns[13] = "M";
            Columns[14] = "N";
            Columns[15] = "O";
            Columns[16] = "P";
            Columns[17] = "Q";
            Columns[18] = "R";
            Columns[19] = "S";
            Columns[20] = "T";
            Columns[21] = "U";
            Columns[22] = "V";
            Columns[23] = "W";
            Columns[24] = "X";
            Columns[25] = "Y";
            Columns[26] = "Z";

            for (int i = 0; i < Columns.Length; i++)
            {
                comboBoxCode.Items.Add(Columns[i]);
                comboBoxPrice.Items.Add(Columns[i]);
                comboBoxProductName.Items.Add(Columns[i]);
                comboBoxAvailability1.Items.Add(Columns[i]);
                comboBoxAvailability2.Items.Add(Columns[i]);
                comboBoxCurrency.Items.Add(Columns[i]);
            }           
        }

        private void SetFormatComboBox(Task<DataTable> res)
        {   
            comboBoxFormat.Invoke(new Action(() => {
                comboBoxFormat.Items.Clear();
                for (int i = 0; i < res.Result.Rows.Count; i++)
                {
                    comboBoxFormat.Items.Add(res.Result.Rows[i].ItemArray[1]);
                }
            }));
        }

        private void SetSupplierComboBox(Task<DataTable> res)
        {
            comboBoxSupplier.Invoke(new Action(() =>
            {
                comboBoxSupplier.Items.Clear();
                for (int i = 0; i < res.Result.Rows.Count; i++)
                {
                    comboBoxSupplier.Items.Add(res.Result.Rows[i].ItemArray[1]);
                }
            }));
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
            
            dataSet.Tables[tableName].AcceptChanges();            
        }

        private void SetDataTableByRowsSync(DataTable dt, string tableName)
        {
            dataSet.Tables[tableName].Clear();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dataSet.Tables[tableName].NewRow();
                for (int n = 0; n < dr.ItemArray.ToList().Count; n++)
                    dr[n] = dt.Rows[i].ItemArray[n];

                dataSet.Tables[tableName].Rows.Add(dr);
            }

            dataSet.Tables[tableName].AcceptChanges();
        }

        private void SetDataBindings()
        {
            textBoxFirstRow.Invoke(new Action(()=>
            {
                textBoxFirstRow.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource__import_settings, "is_start_row", true, DataSourceUpdateMode.OnPropertyChanged));
                
            }));

            textBoxName.Invoke(new Action(()=>
            {
                textBoxName.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource__import_settings, "is_name", true, DataSourceUpdateMode.OnPropertyChanged));
            }));

            comboBoxCode.Invoke(new Action(()=>
            {
                comboBoxCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource__import_settings, "is_code_col", true, DataSourceUpdateMode.OnPropertyChanged));
            }));

            comboBoxPrice.Invoke(new Action(()=>
            {
                comboBoxPrice.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource__import_settings, "is_price_col", true, DataSourceUpdateMode.OnPropertyChanged));
            }));
            comboBoxProductName.Invoke(new Action(()=>
            {
                comboBoxProductName.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource__import_settings, "is_name_col", true, DataSourceUpdateMode.OnPropertyChanged));
            }));

            comboBoxAvailability1.Invoke(new Action(()=>
            {
                comboBoxAvailability1.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource__import_settings, "is_presense1_col", true, DataSourceUpdateMode.OnPropertyChanged));
            }));

            comboBoxAvailability2.Invoke(new Action(()=>
            {
                comboBoxAvailability2.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource__import_settings, "is_presense2_col", true, DataSourceUpdateMode.OnPropertyChanged));
            }));

            comboBoxCurrency.Invoke(new Action(()=>
            {
                comboBoxCurrency.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource__import_settings, "is_currency_col", true, DataSourceUpdateMode.OnPropertyChanged));
            }));

            textBoxAvailSign.Invoke(new Action(()=>
            {
                textBoxAvailSign.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource__import_settings, "is_presense_symbol", true, DataSourceUpdateMode.OnPropertyChanged));
            }));

            textBoxPriceGrn.Invoke(new Action(()=>
            {
                textBoxPriceGrn.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource__import_settings, "is_uah_flag", true, DataSourceUpdateMode.OnPropertyChanged));
            }));

            textBoxActuality.Invoke(new Action(()=>
            {
                textBoxActuality.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource__import_settings, "is_actuality", true, DataSourceUpdateMode.OnPropertyChanged));
            }));
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

        
        private void buttonSave_Click(object sender, EventArgs e)
        {
            bindingSource__import_settings.MoveNext();
            bindingSource__import_settings.MovePrevious();
            
            var table = dataSet.Tables["Table_import_settings"];
            Model.Update_import_settings(ref table);

            Model.Load_import_settings().ContinueWith(res =>
            {
                table.Clear();
                this.dataGrid_import_settings.Invoke(new Action(()=>
                {
                    this.dataGrid_import_settings.DataSource = null;
                    this.dataGrid_import_settings.Rows.Clear();
                    this.dataGrid_import_settings.DataSource = bindingSource__import_settings;
                    this.SetDataTableByRows(res, "Table_import_settings");
                }));
            });
        }


        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы хотите удалить выбраную строку?", "Удаление", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                if (this.dataGrid_import_settings.CurrentRow != null)
                {
                    int currentRow = this.dataGrid_import_settings.CurrentRow.Index;
                    bindingSource__import_settings.RemoveAt(currentRow);
                }
            }
        }

        private void buttonOpenExcel_Click(object sender, EventArgs e)
        {
            label_file_name.Text = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = openFileDialog.FileName;
                    label_file_name.Text = openFileDialog.SafeFileName;
                    DataTable dt = new DataTable();
                    int res = this.Model.ImportExcel(fileName, ref dt);
                    if (res > 0)
                    {
                        string tableName = "Table_import_excel";
                        dataSet.Tables[tableName].Clear();

                        this.dataGrid_import_excel.DataSource = null;
                        this.dataGrid_import_excel.Rows.Clear();
                        this.dataGrid_import_excel.DataSource = this.bindingSource_import_excel;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = dataSet.Tables[tableName].NewRow();
                            dr[0] = dt.Rows[i].ItemArray.GetValue(3);
                            dr[1] = dt.Rows[i].ItemArray.GetValue(4);
                            dr[2] = dt.Rows[i].ItemArray.GetValue(6);
                            dr[3] = i;
                            dataSet.Tables[tableName].Rows.Add(dr);
                        }
                        dataSet.Tables[tableName].AcceptChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    label_file_name.Text = "Oшибка";
                }
            }
            else
                label_file_name.Text = "Файл не выбран";

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.dataGrid_import_settings.DataSource = null;
            this.dataGrid_import_settings.Rows.Clear();
            this.dataGrid_import_settings.DataSource = bindingSource__import_settings;

            string tableName = "Table_import_settings";
            DataRow dr = dataSet.Tables[tableName].NewRow();
            dr["is_id"] = "0";
            dr["is_name"] = string.Empty;
            dr["is_start_row"] = "0";
            dr["is_code_col"] = "A";
            dr["is_price_col"] = "B";
            dr["is_name_col"] = "C";
            dr["is_actuality"] = "1";
            dr["is_presense1_col"] = "-";
            dr["is_presense2_col"] = "-";
            dr["is_presense_symbol"] = "-";
            dr["is_currency_col"] = string.Empty;
            dr["is_uah_flag"] = string.Empty;

            dataSet.Tables[tableName].Rows.Add(dr);
            bindingSource__import_settings.MoveLast();
            
        }



    }
}
