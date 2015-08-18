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
        //private DataTable TableProductPrice = null;
        private DataTable TableSupplier = null;
        //private DataTable TableExcelData = null;
        private DataTable TableProductAndAlias = null;
        private string[] Columns = new string[27];
        private List<Product> products = new List<Product>();
        private List<CategoryCharge> categoryCharge = new List<CategoryCharge>();

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

            ReceiveData.instance.OnLoaded += new ReceiveData.OnLoadedEventHandler(instance_OnLoaded);

            ReceiveData.instance.BegQuery();


            Model.Load_product_and_alias().ContinueWith(res =>
            {
                TableProductAndAlias = res.Result;
                
                products = new List<Product>();
                int count = TableProductAndAlias.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    products.Add(new Product() 
                    {
                        prod_id = TableProductAndAlias.Rows[i].ItemArray[0],
                        prod_name = TableProductAndAlias.Rows[i].ItemArray[1],
                        prod_income_price = TableProductAndAlias.Rows[i].ItemArray[2],
                        prod_text = TableProductAndAlias.Rows[i].ItemArray[3],
                        prod_client_price = TableProductAndAlias.Rows[i].ItemArray[4],
                        prod_price_col1 = TableProductAndAlias.Rows[i].ItemArray[5],
                        prod_price_col2 = TableProductAndAlias.Rows[i].ItemArray[6],
                        prod_price_col3 = TableProductAndAlias.Rows[i].ItemArray[7],
                        prod_fixed_price = TableProductAndAlias.Rows[i].ItemArray[8],
                        pa_code = TableProductAndAlias.Rows[i].ItemArray[9],
                        prod_pc_id = TableProductAndAlias.Rows[i].ItemArray[10],
                    });  
                }

                ReceiveData.instance.EndQuery();

                Debug.WriteLine("           TableProductAndAlias: " + TableProductAndAlias.Rows.Count.ToString());
            });


            ReceiveData.instance.BegQuery();
            Model.Load_category_charge().ContinueWith(res => 
            {
                TableCategoryCharge = res.Result;

                categoryCharge = new List<CategoryCharge>();
                int count = TableCategoryCharge.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    categoryCharge.Add(new CategoryCharge()
                    {
                        cc_pc_id = System.Convert.ToInt32(TableCategoryCharge.Rows[i].ItemArray[0]),
                        cc_price_from = System.Convert.ToInt32(TableCategoryCharge.Rows[i].ItemArray[1]),
                        cc_price_to = System.Convert.ToInt32(TableCategoryCharge.Rows[i].ItemArray[2]),
                        cc_charge = System.Convert.ToInt32(TableCategoryCharge.Rows[i].ItemArray[3]),
                    });
                }

                ReceiveData.instance.EndQuery();

                Debug.WriteLine("           TableCategoryCharge: " + TableCategoryCharge.Rows.Count.ToString());
            });

            ReceiveData.instance.BegQuery();
            Model.Load_import_settings().ContinueWith(res =>
            {
                SetDataTableByRows(res, "Table_import_settings");
                SetDataBindings();
                SetFormatComboBox(res);
                ReceiveData.instance.EndQuery();
            });

            ReceiveData.instance.BegQuery();
            Model.Load_price_category().ContinueWith(res =>
            {
                TablePriceCategory = res.Result;
                Debug.WriteLine("           TablePriceCategory: " + TablePriceCategory.Rows.Count.ToString());
                ReceiveData.instance.EndQuery();
            });

            ReceiveData.instance.BegQuery();
            Model.Load_product().ContinueWith(res =>
            {
                TableProduct = res.Result;
                Debug.WriteLine("           TableProduct: " + TableProduct.Rows.Count.ToString());
                ReceiveData.instance.EndQuery();
            });

            ReceiveData.instance.BegQuery();
            Model.Load_product_alias().ContinueWith(res =>
            {
                TableProductAlias = res.Result;
                Debug.WriteLine("           TableProductAlias: " + TableProductAlias.Rows.Count.ToString());
                ReceiveData.instance.EndQuery();
            });

            ReceiveData.instance.BegQuery();
            Model.Load_product_category().ContinueWith(res =>
            {
                TableProductCategory = res.Result;
                Debug.WriteLine("           TableProductCategory: " + TableProductCategory.Rows.Count.ToString());
                ReceiveData.instance.EndQuery();
            });

            //Model.Load_product_price().ContinueWith(res =>
            //{
            //    TableProductPrice = res.Result;
            //    Debug.WriteLine("           TableProductPrice: " + TableProductPrice.Rows.Count.ToString());
            //});

            ReceiveData.instance.BegQuery();
            Model.Load_supplier().ContinueWith(res =>
            {
                TableSupplier = res.Result;
                SetSupplierComboBox(res);
                ReceiveData.instance.EndQuery();
            });

            DateTime d2 = DateTime.Now;
            TimeSpan timeout = d2 - d1;

            FillComboBoxes();
            dataGrid_import_excel.Columns["isChecked"].ReadOnly = false;
            Debug.WriteLine("time data loaded: " + timeout.ToString());
            return;
        }


        private void instance_OnLoaded()
        {
            buttonOpenExcel.Invoke(new Action(() =>
            {
                buttonOpenExcel.Enabled = true;
            }));
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
                    int countRowsExcel = this.Model.ImportExcel(fileName, ref dt);
                    if (countRowsExcel > 0)
                    {
                        string tableName = "Table_import_excel";

                        this.dataGrid_import_excel.DataSource = null;
                        this.dataGrid_import_excel.Rows.Clear();

                        dataSet.Tables[tableName].Clear();

                        //IEnumerable<DataRow> aliasQuery =
                        //    from productAlias in TableProductAlias.AsEnumerable()
                        //    select productAlias;


                        string format = comboBoxFormat.Text;

                        if (string.IsNullOrEmpty(format))
                        {
                            MessageBox.Show("Выберите формат импорта!", "Импорт");
                            return;
                        }

                        IEnumerable<DataRow> importSettingsQuery =
                            from settings in dataSet.Tables["Table_import_settings"].AsEnumerable()
                            select settings;

                        DataRow importSettings = null;
                        importSettings = importSettingsQuery.FirstOrDefault(s => s.Field<string>("is_name") == format);
                        
                        int indexColumnName = LetterNumber(importSettings["is_name_col"].ToString()) - 1;
                        int indexColumnCode = LetterNumber(importSettings["is_code_col"].ToString()) - 1;
                        int indexColumnPrice = LetterNumber(importSettings["is_price_col"].ToString()) - 1;
                        int indexColumnPresense1 = LetterNumber(importSettings["is_presense1_col"].ToString()) - 1;
                        int indexColumnPresense2 = LetterNumber(importSettings["is_presense2_col"].ToString()) - 1;
                        int indexColumnCurrency = LetterNumber(importSettings["is_currency_col"].ToString()) - 1;

                        //DataTable dataTable = null; 
                        int countFound = -1;
                        string code = string.Empty;
                        Product prod = null;

                        dataGrid1.SelectionMode = SourceGrid.GridSelectionMode.Row;
                        //dataGrid1.Selection.EnableMultiSelection = true;
                        dataGrid1.DataSource = new DevAge.ComponentModel.BoundDataView(dataSet.Tables[tableName].DefaultView);
                        dataGrid1.Columns.AutoSizeView();
                        dataGrid1.DefaultWidth = 50;

                        for (int i = 0; i < countRowsExcel; i++)
                        {
                            //IEnumerable<DataRow> results = null;
                            prod = null;
                            var v = dt.Rows[i].ItemArray.GetValue(4);
                            if (v != null)
                            {
                                code = v as string;
                                //countFound = aliasQuery.Count(a => a.Field<string>("pa_code") == pa_code);

                                //pa_code = v as string;
                                //dataTable = new DataTable();
                                //countFound = this.Model.Load_product_and_alias(ref dataTable, pa_code);

                                countFound = products.Count(a => a.pa_code.ToString() == code);

                                prod = products.FirstOrDefault(a => a.pa_code.ToString() == code);
                            }

                            DataRow dr = dataSet.Tables[tableName].NewRow();
                            if (indexColumnName >= 0)
                                dr["prod_name"] = dt.Rows[i].ItemArray.GetValue(indexColumnName); // наименование
                            else
                                dr["prod_name"] = null; // наименование

                            if(indexColumnCode >=0)
                                dr["prod_code"] = dt.Rows[i].ItemArray.GetValue(indexColumnCode); //код
                            else
                                dr["prod_code"] = null; //код

                            if(indexColumnPrice>=0)
                                dr["prod_income_price"] = dt.Rows[i].ItemArray.GetValue(indexColumnPrice);
                            else
                                dr["prod_income_price"] = null;

                            dr["number"] = i;

                            //if (results != null
                            //    && results.Count<DataRow>() > 0)
                           

                            if(indexColumnPresense1 >= 0)
                                dr["prod_presense1"] = dt.Rows[i].ItemArray.GetValue(indexColumnPresense1);
                            else
                                dr["prod_presense1"] = null;
                            
                            if (indexColumnPresense2 >= 0)
                                dr["prod_presense2"] = dt.Rows[i].ItemArray.GetValue(indexColumnPresense1);
                            else
                                dr["prod_presense2"] = null;

                            if(indexColumnCurrency>=0)
                                dr["prod_currency"] = dt.Rows[i].ItemArray.GetValue(indexColumnCurrency);
                            else
                                dr["prod_currency"] = null;

                            dr["prod_client_price"] = null;
                            
                            object recived_price = dt.Rows[i].ItemArray.GetValue(indexColumnPrice);
                            if (prod != null
                                && prod.prod_pc_id != null
                                && countFound > 0
                                && categoryCharge != null
                                && recived_price != null)
                            {
                                int prod_pc_id = System.Convert.ToInt32(prod.prod_pc_id);
                                double price = System.Convert.ToDouble(recived_price);
                                dr["prod_client_price"] = CalcClientPrice(prod_pc_id, price);
                            }

                            if(prod != null)
                                dr["prod_pc_id"] = prod.prod_pc_id;
                            else
                                dr["prod_pc_id"] = null;

                            ///////////////////////////////////////////////////////////////////////////////////////////////////
                            dataSet.Tables[tableName].Rows.Add(dr);
                            
                            ////if ( (countRowsExcel < 50 && i == countRowsExcel) || (i == 50) )
                            //if (i == 0)
                            //    this.dataGrid_import_excel.DataSource = this.bindingSource_import_excel;

                            //// Set Row Color
                            //if (countFound > 0)
                            //{
                            //    dr["typeFoundProduct"] = TypeFoundProduct.Exist;
                            //    dataGrid_import_excel.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                            //}
                            //else
                            //{
                            //    dr["typeFoundProduct"] = TypeFoundProduct.New;
                            //    dataGrid_import_excel.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                            //}

                            Application.DoEvents();
                            dataGrid1.ResumeLayout();
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





        private double? CalcClientPrice(int prod_pc_id, double price)
        {
            double? res = null;
            CategoryCharge findCharge = categoryCharge.FirstOrDefault(
                f => (System.Convert.ToInt32(f.cc_pc_id) == prod_pc_id && (double)price >= System.Convert.ToDouble(f.cc_price_from) && (double)price < System.Convert.ToDouble(f.cc_price_to))
                    || (System.Convert.ToInt32(f.cc_pc_id) == prod_pc_id && (double)price > System.Convert.ToDouble(f.cc_price_from) && (double)price >= System.Convert.ToDouble(f.cc_price_to)) );
            
            if (findCharge != null)
                res = price + ((price * System.Convert.ToInt32(findCharge.cc_charge)) / 100);
            
            return res;
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


        public int LetterNumber(string charValue)
        {
            char c = Convert.ToChar(charValue);
            int index = char.ToUpper(c) - 64;
            return index;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormCategories formCategories = new FormCategories();
            formCategories.Init(TableProductCategory);
            formCategories.ShowDialog();
        }

    }


    public enum TypeFoundProduct
    { 
        New = 0,
        Exist = 1,
    }


    public class ReceiveData
    {
        private int runQueryes = 0;

        public static ReceiveData instance = new ReceiveData();

        public delegate void OnLoadedEventHandler();
        public event OnLoadedEventHandler OnLoaded;

        public void BegQuery()
        {
            runQueryes++;
        }

        public void EndQuery()
        {
            if (runQueryes > 0)
            {
                runQueryes--;
            }

            if (runQueryes == 0)
                OnLoaded();
        }

    }

}
