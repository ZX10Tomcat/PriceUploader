﻿using LOffice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using log4net.Config;

namespace PriceUploader
{
    public partial class PriceUploader : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PriceUploader));

        private PriceModel Model = null;
        public string[] Columns = new string[27];
        private FormLoad formLoad = null;
        private DataTable table = new DataTable();
        private DataTable tableExcel = new DataTable();
        private int countRowsExcel = 0;
        private int indexColumnName = 0;
        private int indexColumnCode = 0;
        private int indexColumnPrice = 0;
        private int indexColumnPresense1 = 0;
        private int indexColumnPresense2 = 0;
        private int indexColumnCurrency = 0;
        private DateTime timeBeg = DateTime.Now;
        private int beginRows = 0;
        private string tableName = "Table_import_excel";
        public const string TABLE_IMPORT_SETTINGS = "Table_import_settings";
        FormCode formCode = new FormCode();
        FormCategories formCategories = new FormCategories();
        private string fileName;

        public PriceUploader()
        {
            BasicConfigurator.Configure();

            InitializeComponent();
            Init();
            log.Info("Start app");
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
            string appPath = Application.ExecutablePath;
            Model.InsertDataError += Model_InsertDataError;
            Model.OnAddRow += Model_OnAddRow;
            Model.OnSaveAll += Model_OnSaveAll;

            Model.LoadCurrency();

            DateTime d1 = DateTime.Now;

            ReceiveData.instance.OnLoaded += new ReceiveData.OnLoadedEventHandler(instance_OnLoaded);

            ReceiveData.instance.BegQuery();
            Model.Load_import_settings().ContinueWith(res =>
            {
                SetDataTableByRows(res, TABLE_IMPORT_SETTINGS);
                SetDataBindings();
                SetFormatComboBox(res);
                ReceiveData.instance.EndQuery();
            });

            LoadData(true);

            DateTime d2 = DateTime.Now;
            TimeSpan timeout = d2 - d1;

            FillComboBoxes();
            comboBoxImportCurrency.SelectedIndex = 0;

            SetGridLayout();

            //dataGrid_import_excel.Columns["isChecked"].ReadOnly = false;
            Debug.WriteLine("time data loaded: " + timeout.ToString());
            return;
        }

        void Model_OnSaveAll(object sender, EventArgs e)
        {
            log.Info("Model_OnSaveAll(object sender, EventArgs e)");
            ReceiveData.instance.OnLoaded += saveData_OnLoaded;
            LoadData(true);
        }

        private void LoadData(bool showMessage)
        {
            log.Info("LoadData(bool showMessage)");

            buttonOpenExcel.Enabled = false;
            buttonDownloadFile.Enabled = false;
            buttonSaveData.Enabled = false;

            log.Info("Start Model.Load_product_and_alias()");
            ReceiveData.instance.BegQuery();

            Model.Load_product_and_alias().ContinueWith(res =>
            {
                Model.TableProductAndAlias = res.Result;

                Model.products = new List<Product>();
                int count = Model.TableProductAndAlias.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    Model.products.Add(new Product()
                    {
                        prod_id = Model.TableProductAndAlias.Rows[i].ItemArray[0],
                        prod_name = Model.TableProductAndAlias.Rows[i].ItemArray[1],
                        prod_income_price = Model.TableProductAndAlias.Rows[i].ItemArray[2],
                        prod_text = Model.TableProductAndAlias.Rows[i].ItemArray[3],
                        prod_client_price = Model.TableProductAndAlias.Rows[i].ItemArray[4],
                        prod_price_col1 = Model.TableProductAndAlias.Rows[i].ItemArray[5],
                        prod_price_col2 = Model.TableProductAndAlias.Rows[i].ItemArray[6],
                        prod_price_col3 = Model.TableProductAndAlias.Rows[i].ItemArray[7],
                        prod_fixed_price = Model.TableProductAndAlias.Rows[i].ItemArray[8],
                        pa_code = Model.TableProductAndAlias.Rows[i].ItemArray[9] == null ? "" : Model.TableProductAndAlias.Rows[i].ItemArray[9].ToString(),
                        pa_code_lower = Model.TableProductAndAlias.Rows[i].ItemArray[9] == null ? "" : Model.TableProductAndAlias.Rows[i].ItemArray[9].ToString().ToLower(),
                        prod_pc_id = Model.TableProductAndAlias.Rows[i].ItemArray[10],
                        prod_qty = Model.TableProductAndAlias.Rows[i].ItemArray[11],
                    });
                }

                string str = "Table ProductAndAlias: " + Model.TableProductAndAlias.Rows.Count.ToString();
                FormLoadMessage(str, showMessage);
                ReceiveData.instance.EndQuery();
                
                log.Info("Finished Model.Load_product_and_alias()");
            });


            log.Info("Start Model.Load_category_charge()");
            ReceiveData.instance.BegQuery();
            Model.Load_category_charge().ContinueWith(res =>
            {
                Model.TableCategoryCharge = res.Result;

                Model.categoryCharge = new List<CategoryCharge>();
                int count = Model.TableCategoryCharge.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    Model.categoryCharge.Add(new CategoryCharge()
                    {
                        cc_pc_id = System.Convert.ToInt32(Model.TableCategoryCharge.Rows[i].ItemArray[0]),
                        cc_price_from = System.Convert.ToInt32(Model.TableCategoryCharge.Rows[i].ItemArray[1]),
                        cc_price_to = System.Convert.ToInt32(Model.TableCategoryCharge.Rows[i].ItemArray[2]),
                        cc_charge = System.Convert.ToInt32(Model.TableCategoryCharge.Rows[i].ItemArray[3]),
                    });
                }

                string str = "Table CategoryCharge: " + Model.TableCategoryCharge.Rows.Count.ToString();
                FormLoadMessage(str, showMessage);
                ReceiveData.instance.EndQuery();

                log.Info("Finished Model.Load_category_charge()");
            });

            log.Info("Start Model.Load_price_category()");
            ReceiveData.instance.BegQuery();
            Model.Load_price_category().ContinueWith(res =>
            {
                Model.TablePriceCategory = res.Result;
                string str = "Table PriceCategory: " + Model.TablePriceCategory.Rows.Count.ToString();
                FormLoadMessage(str, showMessage);
                ReceiveData.instance.EndQuery();

                log.Info("Finished Model.Load_price_category()");
            });

            log.Info("Start Model.Load_product()");
            ReceiveData.instance.BegQuery();
            Model.Load_product().ContinueWith(res =>
            {
                Model.TableProduct = res.Result;
                string str = "Table Product: " + Model.TableProduct.Rows.Count.ToString();
                FormLoadMessage(str, showMessage);
                ReceiveData.instance.EndQuery();

                log.Info("Finished Model.Load_product()");
            });

            log.Info("Start Model.Load_product_alias()");
            ReceiveData.instance.BegQuery();
            Model.Load_product_alias().ContinueWith(res =>
            {
                Model.TableProductAlias = res.Result;
                string str = "Table ProductAlias: " + Model.TableProductAlias.Rows.Count.ToString();
                FormLoadMessage(str, showMessage);
                ReceiveData.instance.EndQuery();

                log.Info("Finished Model.Load_product_alias()");
            });

            log.Info("Start Model.Load_product_category()");
            ReceiveData.instance.BegQuery();
            Model.Load_product_category().ContinueWith(res =>
            {
                Model.TableProductCategory = res.Result;
                string str = "Table ProductCategory: " + Model.TableProductCategory.Rows.Count.ToString();
                FormLoadMessage(str, showMessage);

                Model.categories = new List<Category>();
                foreach (var item in Model.TableProductCategory.AsEnumerable())
                {
                    Model.categories.Add(new Category
                    {
                        pc_id = Convert.ToInt32(item.ItemArray[0]),
                        pc_parent_id = Convert.ToInt32(item.ItemArray[1]),
                        pc_name = Convert.ToString(item.ItemArray[2]),
                        pc_description = Convert.ToString(item.ItemArray[8])
                    });
                }

                ReceiveData.instance.EndQuery();

                log.Info("Finished Model.Load_product_category()");
            });

            //Model.Load_product_price().ContinueWith(res =>
            //{
            //    TableProductPrice = res.Result;
            //    Debug.WriteLine("           TableProductPrice: " + TableProductPrice.Rows.Count.ToString());
            //});

            log.Info("Start Model.Load_supplier()");
            ReceiveData.instance.BegQuery();
            Model.Load_supplier().ContinueWith(res =>
            {
                Model.TableSupplier = res.Result;
                SetSupplierComboBox(res);
                string str = "Table Supplier: " + Model.TableSupplier.Rows.Count.ToString();
                FormLoadMessage(str, showMessage);
                ReceiveData.instance.EndQuery();

                log.Info("Finished Model.Load_supplier()");
            });

            
        }
        
        void Model_OnAddRow(object sender, EventArgs e)
        {
            log.Info("Start Model_OnAddRow()");

            var v = sender as AddRowInfo;

            if (v != null)
            
            {
                lbl_Counter.Invoke(new Action(() =>
                {
                    lbl_Counter.Text = (v.index + 1).ToString();
                    lbl_Counter.Refresh();
                }));

                label_TimeSpan.Invoke(new Action(() =>
                {
                    //double calc = v.timeRuning.TotalMilliseconds / 1000;
                    //label_TimeSpan.Text = calc.ToString();
                    label_TimeSpan.Text = string.Format("{0}:{1}:{2}.{3}", v.timeRuning.Hours, v.timeRuning.Minutes, v.timeRuning.Seconds, v.timeRuning.Milliseconds);
                    label_TimeSpan.Refresh();
                }));
            }

            log.Info("Finished Model_OnAddRow()");
        }
        
        void Model_InsertDataError(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format("Вы не указали категорию в строке №{0}", sender.ToString() ));
        }

        private void SetGridLayout()
        {
            // Columns width
            dataGrid_import_excel.Columns[0].Width = 50;
            dataGrid_import_excel.Columns[1].Width = 80;
            dataGrid_import_excel.Columns[2].Width = 80;
            dataGrid_import_excel.Columns[3].Width = 60;
            //dataGrid_import_excel.Columns[4].Width = 600;
            //dataGrid_import_excel.Columns[5].Width = 100;
            //dataGrid_import_excel.Columns[6].Width = 150;
            //dataGrid_import_excel.Columns[7].Width = 100;
            //dataGrid_import_excel.Columns[8].Width = 100;
            //dataGrid_import_excel.Columns[9].Width = 100;
            //dataGrid_import_excel.Columns[10].Width = 100;
            //dataGrid_import_excel.Columns[11].Width = 60;
            //dataGrid_import_excel.Columns[12].Width = 60;


            // Set ReadOnly columns
            dataGrid_import_excel.Columns[0].ReadOnly = false;
            dataGrid_import_excel.Columns[1].ReadOnly = true;
            dataGrid_import_excel.Columns[2].ReadOnly = true;
            dataGrid_import_excel.Columns[3].ReadOnly = true;
            dataGrid_import_excel.Columns[4].ReadOnly = false;
            dataGrid_import_excel.Columns[5].ReadOnly = false;
            dataGrid_import_excel.Columns[6].ReadOnly = true;
            dataGrid_import_excel.Columns[7].ReadOnly = true;
            dataGrid_import_excel.Columns[8].ReadOnly = true;
            dataGrid_import_excel.Columns[9].ReadOnly = true;
            dataGrid_import_excel.Columns[10].ReadOnly = true;
            dataGrid_import_excel.Columns[11].ReadOnly = true;
            dataGrid_import_excel.Columns[12].ReadOnly = true;
        }

        private void instance_OnLoaded()
        {
            buttonOpenExcel.Invoke(new Action(() =>
            {
                buttonOpenExcel.Enabled = true;
                buttonDownloadFile.Enabled = true;
                buttonSaveData.Enabled = true;

                if (this.formLoad != null)
                    this.formLoad.Close();

                ReceiveData.instance.OnLoaded -= new ReceiveData.OnLoadedEventHandler(instance_OnLoaded);
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
                comboBoxFormat.SelectedIndex = 0;
            }));
        }

        private void SetSupplierComboBox(Task<DataTable> res)
        {
            comboBoxSupplier.Invoke(new Action(() =>
            {
                comboBoxSupplier.Items.Clear();
                for (int i = 0; i < res.Result.Rows.Count; i++)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = res.Result.Rows[i].ItemArray[1].ToString();
                    item.Value = res.Result.Rows[i].ItemArray[0];
                    comboBoxSupplier.Items.Add(item);
                }
                comboBoxSupplier.SelectedIndex = 0;
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
            //bindingSource__import_settings.MoveNext();
            //bindingSource__import_settings.MovePrevious();

            var table = dataSet.Tables[TABLE_IMPORT_SETTINGS];
            if (table.GetChanges() != null)
            {
                Model.Update_import_settings(ref table);
                Model.Load_import_settings().ContinueWith(res =>
                {
                    table.Clear();
                    this.dataGrid_import_settings.Invoke(new Action(() =>
                    {
                        this.dataGrid_import_settings.DataSource = null;
                        this.dataGrid_import_settings.Rows.Clear();
                        this.dataGrid_import_settings.DataSource = bindingSource__import_settings;
                        this.SetDataTableByRows(res, TABLE_IMPORT_SETTINGS);
                    }));
                });
            }
        }
        
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы хотите удалить выбраную строку?", "Удаление", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                if (this.dataGrid_import_settings.CurrentRow != null)
                {
                    var dataRowView = this.dataGrid_import_settings.CurrentRow.DataBoundItem as DataRowView;
                    if (dataRowView != null)
                    {
                        int is_id = dataRowView.Row.Field<int>("is_id");
                        Model.Delete_row_import_settings(is_id);
                        int currentRow = this.dataGrid_import_settings.CurrentRow.Index;
                        bindingSource__import_settings.RemoveAt(currentRow);
                        //dataSet.Tables[TABLE_IMPORT_SETTINGS].AcceptChanges();
                    }

                }
            }
        }
        
        private void buttonOpenExcel_Click(object sender, EventArgs e)
        {
            CategoriesTreeView categoriesTreeView = new CategoriesTreeView();
            categoriesTreeView.Init(Model.TableProductCategory, formCategories, Model);

            label_file_name.Text = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Excel files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;
                label_file_name.Text = openFileDialog.SafeFileName;
            }
        }

        private void buttonDownloadFile_Click(object sender, EventArgs e)
        {
            if (fileName !="")
            {
                try
                {
                    buttonDownloadFile.Enabled = false;
                    
                    dataGrid_import_excel.CellContentClick -= dataGrid_import_excel_CellContentClick;
                    dataGrid_import_excel.SelectionChanged -= dataGrid_import_excel_SelectionChanged;

                    table = new DataTable();
                    tableExcel = new DataTable();

                    countRowsExcel = this.Model.ImportExcel(fileName, ref tableExcel, this.comboBoxFormat.Text);
                    Model.listImportToDB = new List<ImportToDB>();

                    if (countRowsExcel > 0)
                    {
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
                            from settings in dataSet.Tables[TABLE_IMPORT_SETTINGS].AsEnumerable()
                            select settings;

                        DataRow importSettings = null;
                        importSettings = importSettingsQuery.FirstOrDefault(s => s.Field<string>("is_name") == format);


                        indexColumnName = LetterNumber(importSettings["is_name_col"].ToString()) - 1;
                        indexColumnCode = LetterNumber(importSettings["is_code_col"].ToString()) - 1;
                        indexColumnPrice = LetterNumber(importSettings["is_price_col"].ToString()) - 1;
                        indexColumnPresense1 = LetterNumber(importSettings["is_presense1_col"].ToString()) - 1;
                        indexColumnPresense2 = LetterNumber(importSettings["is_presense2_col"].ToString()) - 1;
                        indexColumnCurrency = LetterNumber(importSettings["is_currency_col"].ToString()) - 1;

                        if (importSettings["is_actuality"] != null && !string.IsNullOrEmpty(importSettings["is_actuality"].ToString()))
                            Model.ProdActuality = Convert.ToInt32(importSettings["is_actuality"].ToString());
                        else
                            Model.ProdActuality = 1;  //just in case

                        Model.GRNsign = importSettings["is_uah_flag"].ToString();

                        if (importSettings["is_start_row"] != null
                            && !string.IsNullOrEmpty(importSettings["is_start_row"].ToString())
                            && Convert.ToInt32(importSettings["is_start_row"].ToString()) > 0)
                            beginRows = Convert.ToInt32(importSettings["is_start_row"].ToString()) - 1;
                        else
                            beginRows = 0;

                        Model.PresenseValue = string.Empty;
                        if (importSettings["is_actuality"] != null && !string.IsNullOrEmpty(importSettings["is_actuality"].ToString()))
                            Model.PresenseValue = importSettings["is_presense_symbol"].ToString();

                        // Set columns
                        //table.Columns.Add("№", typeof(int));
                        //table.Columns.Add("V", typeof(bool));
                        //table.Columns.Add("Наименование", typeof(string));
                        //table.Columns.Add("Код", typeof(string));
                        //table.Columns.Add("Цена", typeof(string));
                        //table.Columns.Add("prod_presense1", typeof(string));
                        //table.Columns.Add("prod_presense2", typeof(string));
                        //table.Columns.Add("prod_currency", typeof(string));
                        //table.Columns.Add("prod_client_price", typeof(string));
                        //table.Columns.Add("prod_pc_id", typeof(string));
                        //table.Columns.Add("prod_id", typeof(string));

                        int calcRowsCount = countRowsExcel - beginRows;
                        lbl_TotalCount.Text = (calcRowsCount).ToString();
                        lbl_TotalCount.Refresh();

                        //string code = string.Empty;
                        //Product prod = null;
                        //int countFound = 0;

                        //int count = countRowsExcel;
                        //for (int i = 0; i < count; i++)
                        //{

                        //    code = string.Empty;
                        //    prod = null;
                        //    countFound = 0;

                        //    var v = tableExcel.Rows[i].ItemArray.GetValue(4);
                        //    if (v != null)
                        //    {
                        //        code = v as string;
                        //        //countFound = products.Count(a => a.pa_code.ToString() == code);
                        //        prod = products.FirstOrDefault(a => a.pa_code.ToString() == code);
                        //    }

                        //    excelList.Add(new ImportToDB()
                        //    {
                        //        prod_name = GetValue(ref tableExcel, i, indexColumnPresense1),               //tableExcel.Rows[i].ItemArray.GetValue(indexColumnName).ToString(),
                        //        prod_code = GetValue(ref tableExcel, i, indexColumnCode),               //tableExcel.Rows[i].ItemArray.GetValue(indexColumnCode).ToString(),
                        //        prod_income_price = GetValue(ref tableExcel, i, indexColumnPrice),                                  //tableExcel.Rows[i].ItemArray.GetValue(indexColumnPrice).ToString(),
                        //        prod_presense1 = GetValue(ref tableExcel, i, indexColumnPresense1),
                        //        prod_presense2 =  GetValue(ref tableExcel, i, indexColumnPresense2),        //tableExcel.Rows[i].ItemArray.GetValue(indexColumnPresense2).ToString(),
                        //        prod_currency = GetValue(ref tableExcel, i, indexColumnCurrency),           //tableExcel.Rows[i].ItemArray.GetValue(indexColumnCurrency).ToString(),
                        //        prod_client_price = GetValue(ref tableExcel, i, indexColumnPrice),          //tableExcel.Rows[i].ItemArray.GetValue(indexColumnPrice).ToString(),
                        //        prod_pc_id = "code",                                                        //tableExcel.Rows[i].ItemArray.GetValue(4).ToString(), // Откуда єто брать?
                        //        number = i
                        //    });
                        //}

                        timeBeg = DateTime.Now;
                        AddRows(beginRows, countRowsExcel, indexColumnName, indexColumnCode, indexColumnPrice,
                            indexColumnPresense1, indexColumnPresense2, indexColumnCurrency);

                        //dataGrid1.SelectionMode = SourceGrid.GridSelectionMode.Row;
                        //dataGrid1.DataSource = new DevAge.ComponentModel.BoundDataView(table.DefaultView);
                        dataGrid_import_excel.DataSource = dataSet.Tables[tableName];
                        //dataGrid1.Columns.AutoSizeView();
                        //dataGrid1.DefaultWidth = 50;

                        dataGrid_import_excel.Invoke(new Action(() =>
                        {
                            dataGrid_import_excel.ResumeLayout();
                            buttonDownloadFile.Enabled = true;
                            //dataGrid1.RecalcCustomScrollBars();

                            // Add datagrid Events

                            dataGrid_import_excel.CellContentClick += dataGrid_import_excel_CellContentClick;
                            dataGrid_import_excel.SelectionChanged += dataGrid_import_excel_SelectionChanged;
                        }));

                        //new Thread(CreateData).Start();
                    }
                    else
                    {
                        buttonDownloadFile.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    label_file_name.Text = "Oшибка";
                    buttonDownloadFile.Enabled = true;
                }
            }
            else
                label_file_name.Text = "Файл не выбран";

        }
        
        private string GetValue(ref DataTable tbl, int row, int column)
        {
            string temp = string.Empty;
            if (column >= 0)
                temp = tbl.Rows[row].ItemArray.GetValue(column) != null ? tbl.Rows[row].ItemArray.GetValue(column).ToString() : string.Empty;
            return temp;
        }


        private string GetValueDouble(ref DataTable tbl, int row, int column)
        {
            string temp = GetValue(ref tbl, row, column);
            if (!string.IsNullOrEmpty(temp))
                temp = Math.Round(Convert.ToDouble(temp), 2).ToString();
            return temp;
        }



        private void UpdateTime()
        {
            label_TimeSpan.Invoke(new Action(() =>
            {
                TimeSpan ts = DateTime.Now - timeBeg;
                //double calc = ts.TotalMilliseconds / 1000;
                //label_TimeSpan.Text = calc.ToString();
                label_TimeSpan.Text = string.Format("{0}:{1}:{2}.{3}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            }));
        }
        
        private void UpdateCounter(int index)
        {
            lbl_Counter.Invoke(new Action(() =>
            {
                lbl_Counter.Text = (index+1).ToString();
                lbl_Counter.Refresh();
            }));
        }
        
        private void CreateData()
        {
            dataGrid_import_excel.SuspendLayout();

            //AddRows(beginRows, countRowsExcel, indexColumnName, indexColumnCode, indexColumnPrice, indexColumnPresense1, indexColumnPresense2, indexColumnCurrency);

            dataGrid_import_excel.Invoke(new Action(() =>
            {
                dataGrid_import_excel.ResumeLayout();
                //dataGrid1.RecalcCustomScrollBars();
            }));
        }

        private void AddRows(int indexBeg, int indexEnd, int indexColumnName, 
            int indexColumnCode, int indexColumnPrice, int indexColumnPresense1,
            int indexColumnPresense2, int indexColumnCurrency)
        {
            //string code = string.Empty;
            Product prod = null;

            Model.listImportToDB = new List<ImportToDB>();

            for (int i = indexBeg; i < indexEnd; i++)
            {
                Model.listImportToDB.Add(
                    new ImportToDB()
                    {
                        number = i,
                        prod_code = GetValue(ref tableExcel, i, indexColumnCode),
                        prod_name = GetValue(ref tableExcel, i, indexColumnName),
                        prod_income_price = GetValueDouble(ref tableExcel, i, indexColumnPrice),
                        prod_presense1 = GetValue(ref tableExcel, i, indexColumnPresense1),
                        prod_presense2 = GetValue(ref tableExcel, i, indexColumnPresense2),
                        prod_currency = GetValue(ref tableExcel, i, indexColumnCurrency)
                    });
            }

            //var importQuery = (
            //    from l in listImportToDB
            //    from p in products.Where(p => p.pa_code == l.prod_code && p.prod_name.ToString() == l.prod_name.ToString()).DefaultIfEmpty()
            //    select new
            //    {
            //        number = l.number,
            //        prod_code = l.prod_code,
            //        prod_name = l.prod_name,
            //        prod_income_price = l.prod_income_price,
            //        prod_presense1 = l.prod_presense1,
            //        prod_presense2 = l.prod_presense2,
            //        prod_currency =  l.prod_currency, 
            //        product_pa_code = p == null ? "" : p.pa_code,
            //        product_prod_pc_id = p == null ? "" : p.prod_pc_id,
            //        prod_pc_id = p == null ? "" : p.prod_pc_id,
            //        prod_id = p == null ? "" : p.prod_id,
            //    }).ToList();


            //var import = (importQuery).ToList();
            //for (int i = indexBeg; i < import.Count() /* indexEnd */; i++)

            string importCurrency = comboBoxImportCurrency.Text;
            

            for (int i = indexBeg; i < indexEnd; i++)
            {
                //if(i==87)
                    Debug.Print("i" + i.ToString());
                
                //Debug.Print("i = " + i.ToString());
                
                ImportToDB importToDB = new ImportToDB();
                importToDB.number = i;

                prod = null;
                importToDB.prod_code = GetValue(ref tableExcel, i, indexColumnCode);


                string tmp_prod_code = string.IsNullOrEmpty(importToDB.prod_code) == false ? importToDB.prod_code.ToLower() : string.Empty;

                if (!string.IsNullOrEmpty(tmp_prod_code))
                    prod = Model.products.FirstOrDefault(a => a.pa_code_lower == tmp_prod_code);

                importToDB.prod_name = GetValue(ref tableExcel, i, indexColumnName);
                importToDB.prod_presense1 = GetValue(ref tableExcel, i, indexColumnPresense1);
                importToDB.prod_presense2 = GetValue(ref tableExcel, i, indexColumnPresense2);
                importToDB.prod_currency = GetValue(ref tableExcel, i, indexColumnCurrency);
                importToDB.product_pa_code = importToDB.prod_code == null ? "" : importToDB.prod_code;
                
                if (prod != null)
                {
                    importToDB.product_prod_pc_id = prod.prod_pc_id == null ? "" : prod.prod_pc_id.ToString();
                    importToDB.prod_pc_id = prod.prod_pc_id == null ? "" : prod.prod_pc_id.ToString();
                    importToDB.prod_id = prod.prod_id == null ? "" : prod.prod_id.ToString();
                    importToDB.prod_qty = prod.prod_qty == null ? "" : prod.prod_qty.ToString();

                    //проверка что цена в прайсе больше чем в БД    
                    //double priceExcel = 0;
                    //double priceDB = 0;

                    importToDB.prod_income_price = GetValueDouble(ref tableExcel, i, indexColumnPrice);

                    //if (!string.IsNullOrEmpty(importToDB.prod_income_price))
                    //{
                    //    priceExcel = System.Convert.ToDouble(PriceModel.ConvertSeparator(importToDB.prod_income_price));
                    //}

                    //if (prod.prod_income_price != null)
                    //{
                    //    priceDB = System.Convert.ToDouble(PriceModel.ConvertSeparator(prod.prod_income_price.ToString()));
                    //}

                    //if (priceDB > priceExcel)
                    //    importToDB.prod_income_price = priceDB.ToString();
                    
                    //проверка что цена в прайсе больше чем в БД    
                }
                else
                {
                    importToDB.product_prod_pc_id = "";
                    importToDB.prod_pc_id = "";
                    importToDB.prod_id = "";
                    importToDB.prod_qty = "";
                    importToDB.prod_income_price = GetValueDouble(ref tableExcel, i, indexColumnPrice);  //string.Empty;
                }

                //object recived_price = tableExcel.Rows[i].ItemArray.GetValue(indexColumnPrice);
                //if (prod != null
                //    && prod.prod_pc_id != null
                //    && categoryCharge != null
                //    && recived_price != null)
                //{
                //    int prod_pc_id = System.Convert.ToInt32(prod.prod_pc_id);
                //    double price = System.Convert.ToDouble(recived_price);
                //    importToDB.prod_client_price = CalcClientPrice(prod_pc_id, price).ToString();
                //}
                
                //На всякий случай/////////////////////////////////////////////////////////////////////////////////////////////

                //dataSet.Tables[tableName].Rows.Add(
                //    new object[] {                                                                     
                //                    import[i].prod_name,                                   
                //                    import[i].prod_code,
                //                    import[i].prod_income_price,
                //                    import[i].number +1, 
                //                    import[i].prod_presense1,
                //                    import[i].prod_presense2,
                //                    import[i].prod_currency,
                //                    CalcClientPrice(ref categoryCharge, tableExcel.Rows[i].ItemArray.GetValue(indexColumnPrice), import[i].prod_pc_id)    /* prod_client_price */,
                //                    import[i].prod_pc_id,
                //                    import[i].prod_id,
                //                    (import[i].prod_pc_id.ToString() == "" && import[i].prod_id.ToString() == "")
                //                    //(import[i].prod_pc_id == null && import[i].prod_id == null) ? "red" : "green"
                //                });


                //Check product presense
                //if (!is_null($presense1_col) && mb_strpos($row[$presense1_col], $preset['is_presense_symbol'], 0, 'UTF-8') !== false) $presense_found = true;
                //  if (!is_null($presense2_col) && mb_strpos($row[$presense2_col], $preset['is_presense_symbol'], 0, 'UTF-8') !== false) $presense_found = true;
                //  if ($presense_found == false) continue;
                //Check product presense

                //Debug.Print("importToDB.prod_code = " + importToDB.prod_code.ToString());

                bool presense_found = false;

                if (!string.IsNullOrEmpty(importToDB.prod_presense1)
                    && importToDB.prod_presense1.Contains(Model.PresenseValue))
                    presense_found = true;
                
                if (!string.IsNullOrEmpty(importToDB.prod_presense2)
                    && importToDB.prod_presense2.Contains(Model.PresenseValue))
                    presense_found = true;

                //if (!presense_found) 
                //    continue;

                object prod_pa_code = null;

                if (prod != null && !string.IsNullOrEmpty(prod.pa_code))
                    prod_pa_code = prod.pa_code;
                else
                    prod_pa_code = string.Empty;

                if ( (!string.IsNullOrEmpty(importToDB.prod_name) || !string.IsNullOrWhiteSpace(importToDB.prod_name))
                    && (!string.IsNullOrEmpty(importToDB.prod_code) || !string.IsNullOrWhiteSpace(importToDB.prod_code))
                    && (!string.IsNullOrEmpty(importToDB.prod_income_price) || !string.IsNullOrWhiteSpace(importToDB.prod_income_price)) )
                {

                    if (importCurrency != PriceModel.RRC 
                        && Model.GRNsign.ToLower() == importToDB.prod_currency.ToLower()
                        && !string.IsNullOrEmpty(importToDB.prod_currency))
                    {
                        double? d = System.Double.Parse(importToDB.prod_income_price);
                        if (d != null)
                        {
                            d = d / Model.Currency_rate_cash;
                            importToDB.prod_income_price = Math.Round(d.GetValueOrDefault(0), 2).ToString();
                        }
                    }

                    string calcClientPrice = this.Model.CalcClientPrice(ref Model.categoryCharge, tableExcel.Rows[i].ItemArray.GetValue(indexColumnPrice), importToDB.prod_pc_id, importToDB.prod_qty, importCurrency, prod_pa_code) /* prod_client_price */;

                    dataSet.Tables[tableName].Rows.Add(
                        new object[] {
                            importToDB.prod_name,
                            importToDB.prod_code,
                            importToDB.prod_income_price,
                            importToDB.number +1,
                            importToDB.prod_presense1,
                            importToDB.prod_presense2,
                            importToDB.prod_currency,
                            calcClientPrice,
                            importToDB.prod_pc_id,
                            importToDB.prod_id,
                            (importToDB.prod_pc_id.ToString() == "" && importToDB.prod_id.ToString() == ""),
                            false,
                            "",
                            "",
                            false,
                            importToDB.prod_qty,
                            presense_found
                    });
                }


                ////if ( (countRowsExcel < 50 && i == countRowsExcel) || (i == 50) )
                //if (i == 0)
                //    this.dataGrid_import_excel.DataSource = this.bindingSource_import_excel;

                if (i % 100 == 0)
                {
                    UpdateCounter(i - indexBeg);
                    UpdateTime();
                }

                if (i % 500 == 0)
                    dataGrid_import_excel.Invoke(new Action(() =>
                    {
                        dataGrid_import_excel.ResumeLayout();
                        //dataGrid1.RecalcCustomScrollBars();
                    }));

            }

            UpdateCounter(indexEnd - indexBeg - 1);
            UpdateTime();

            dataGrid_import_excel.Invoke(new Action(() =>
            {
                dataGrid_import_excel.ResumeLayout();
                //dataGrid1.RecalcCustomScrollBars();
            }));

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
            //FormCategories formCategories = new FormCategories();
            //formCategories.Init(TableProductCategory);
            
            formCategories.ShowDialog();
        }


        private void dataGrid_import_excel_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int index = e.RowIndex; index <= e.RowIndex + e.RowCount - 1; index++)
            {
                DataGridViewRow row = dataGrid_import_excel.Rows[index];
                // Set Row Color

                //string prod_id = row.Field<string>("prod_id");
                
                if (row.Cells[11].Value.ToString() != "" && row.Cells[12].Value.ToString() != "")
                {
                    //dr["typeFoundProduct"] = TypeFoundProduct.Exist;
                    SetGreenRow(index);
                }
                else
                {
                    //dr["typeFoundProduct"] = TypeFoundProduct.New;
                    SetRedRow(index);

                }
                
                //dataSet.Tables[tableName].Rows[index].Field<string>("color").  = dataGrid_import_excel.Rows[index].DefaultCellStyle.BackColor.Name.ToLower();
                //dataSet.Tables[tableName].AcceptChanges(); 

                dataSet.Tables[tableName].Rows[index]["color"] = dataGrid_import_excel.Rows[index].DefaultCellStyle.BackColor.Name.ToLower();

            }
        }

        private void SetRedRow(int index)
        {
            dataGrid_import_excel.Rows[index].DefaultCellStyle.BackColor = Color.Red;

            dataGrid_import_excel.Rows[index].Cells[0].Value = null;
            dataGrid_import_excel.Rows[index].Cells[0] = new DataGridViewCheckBoxCell();
            dataGrid_import_excel.Rows[index].Cells[0].Value = false;

            dataGrid_import_excel.Rows[index].Cells[1].Value = null;
            dataGrid_import_excel.Rows[index].Cells[1] = new DataGridViewButtonCell();
            dataGrid_import_excel.Rows[index].Cells[1].Value = "Сопоставить";
            dataGrid_import_excel.Rows[index].Cells[1].ToolTipText = "Сопоставить";

            dataGrid_import_excel.Rows[index].Cells[2].Value = null;
            dataGrid_import_excel.Rows[index].Cells[2] = new DataGridViewButtonCell();
            dataGrid_import_excel.Rows[index].Cells[2].Value = "Категория";
            dataGrid_import_excel.Rows[index].Cells[2].ToolTipText = "Категория";
        }

        private void SetGreenRow(int index)
        {
            dataGrid_import_excel.Rows[index].DefaultCellStyle.BackColor = Color.Green;

            dataGrid_import_excel.Rows[index].Cells[0].Value = null;
            dataGrid_import_excel.Rows[index].Cells[0] = new DataGridViewTextBoxCell();
            dataGrid_import_excel.Rows[index].Cells[0].Value = "";

            dataGrid_import_excel.Rows[index].Cells[1].Value = null;
            dataGrid_import_excel.Rows[index].Cells[1] = new DataGridViewTextBoxCell();
            dataGrid_import_excel.Rows[index].Cells[1].Value = "";

            dataGrid_import_excel.Rows[index].Cells[2].Value = null;
            dataGrid_import_excel.Rows[index].Cells[2] = new DataGridViewTextBoxCell();
            dataGrid_import_excel.Rows[index].Cells[2].Value = "";
        }
        
        private void dataGrid_import_excel_SelectionChanged(object sender, EventArgs e)
        {
            int row_Index = 0;
            if (dataGrid_import_excel.SelectedRows.Count > 0)
                row_Index = dataGrid_import_excel.SelectedRows[0].Index;

            //dataSet.Tables[tableName].Rows[row_Index].ItemArray[12] = dataGrid_import_excel.Rows[row_Index].DefaultCellStyle.BackColor.Name.ToLower();

            switch (dataGrid_import_excel.Rows[row_Index].DefaultCellStyle.BackColor.Name.ToLower())
            {
                case "green":
                    dataGrid_import_excel.Rows[row_Index].DefaultCellStyle.BackColor = Color.Blue;
                    break;

                case "blue":
                    dataGrid_import_excel.Rows[row_Index].DefaultCellStyle.BackColor = Color.Green;
                    break;
            }

            dataSet.Tables[tableName].Rows[row_Index]["color"] = dataGrid_import_excel.Rows[row_Index].DefaultCellStyle.BackColor.Name.ToLower();
        }

        private int currenRowIndex = 0;
        private void dataGrid_import_excel_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                currenRowIndex = e.RowIndex;

                dataGrid_import_excel.Rows[currenRowIndex].Cells[0].Value = true;

                switch (e.ColumnIndex)
                {
                    case 1:
                        
                        //formCode.Init(TableProductCategory);
                        formCode.FormClosed += formCode_FormClosed;
                        formCode.ShowDialog();
                        var newCode = formCode.CodeValue;                        
                        break;

                    case 2:
                        formCategories.FormClosed -= formCategories_FormClosed;
                        formCategories.FormClosed += formCategories_FormClosed;
                        formCategories.Init(Model.TableProductCategory);                        
                        formCategories.ShowDialog();
                    break;
                
                }
            }
        }

        void formCategories_FormClosed(object sender, FormClosedEventArgs e)
        {
            string newCategory = "";
            if (formCategories.DialogResult == DialogResult.OK)
            {
                newCategory = formCategories.CategoryValue;
                int row_Index = currenRowIndex;
                //8=prod_pc_id
                //dataSet.Tables[tableName].Rows[row_Index].ItemArray[8] = newCategory; 

                dataSet.Tables[tableName].Rows[row_Index]["prod_pc_id"] = newCategory;
                dataGrid_import_excel.Rows[row_Index].DefaultCellStyle.BackColor = Color.Green;

                foreach (DataGridViewRow row in dataGrid_import_excel.Rows)
                {
                    if (row.Cells[0].GetType() == typeof(DataGridViewCheckBoxCell))
                    {
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                        if (chk.Value.ToString() == "True" || chk.Value == null)
                        {
                            SetGreenRow(row.Index);
                            dataSet.Tables[tableName].Rows[row.Index]["prod_pc_id"] = newCategory;
                            dataSet.Tables[tableName].Rows[row.Index]["color"] = dataGrid_import_excel.Rows[row.Index].DefaultCellStyle.BackColor.Name.ToLower();
                            //dataGrid_import_excel.Rows[row.Index].DefaultCellStyle.BackColor = Color.Green;

                            //dataGrid_import_excel.Rows[row.Index].Cells[0].Value = null;
                            //dataGrid_import_excel.Rows[row.Index].Cells[0] = new DataGridViewTextBoxCell();
                            //dataGrid_import_excel.Rows[row.Index].Cells[0].Value = "";

                            //dataGrid_import_excel.Rows[row.Index].Cells[1].Value = null;
                            //dataGrid_import_excel.Rows[row.Index].Cells[1] = new DataGridViewTextBoxCell();
                            //dataGrid_import_excel.Rows[row.Index].Cells[1].Value = "";

                            //dataGrid_import_excel.Rows[row.Index].Cells[2].Value = null;
                            //dataGrid_import_excel.Rows[row.Index].Cells[2] = new DataGridViewTextBoxCell();
                            //dataGrid_import_excel.Rows[row.Index].Cells[2].Value = "";
                        }
                    }
                }
                //dataSet.Tables[tableName].AcceptChanges();

                //dataGrid_import_excel.Rows[row_Index].DefaultCellStyle.BackColor = Color.Green;
           
            }
                
            formCategories.FormClosed -= formCategories_FormClosed;
        }

        void formCode_FormClosed(object sender, FormClosedEventArgs e)
        {
            string newCode = "";
            if (formCode.DialogResult == DialogResult.OK)
            {
                newCode = formCode.CodeValue;
                int row_Index = currenRowIndex;
                //13=prod_new_code
                //dataSet.Tables[tableName].Rows[row_Index].ItemArray[13] = newCode;
                dataSet.Tables[tableName].Rows[row_Index]["prod_new_code"] = newCode;
                //dataSet.Tables[tableName].AcceptChanges();

                dataGrid_import_excel.Rows[row_Index].DefaultCellStyle.BackColor = Color.Green;

                foreach (DataGridViewRow row in dataGrid_import_excel.Rows)
                {
                    if (row.Cells[0].GetType() == typeof(DataGridViewCheckBoxCell))
                    {
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                        if (chk.Value == chk.TrueValue || chk.Value == null)
                        {
                            SetGreenRow(row.Index);                            
                            dataSet.Tables[tableName].Rows[row.Index]["color"] = dataGrid_import_excel.Rows[row.Index].DefaultCellStyle.BackColor.Name.ToLower();
                        }
                    }
                }
            }

            formCode.FormClosed -= formCode_FormClosed;
        }

        private void buttonSaveData_Click(object sender, EventArgs e)
        {
            log.Info("buttonSaveData_Click");

            buttonSaveData.Invoke(new Action(() =>
            {
                log.Info("buttonSaveData.Invoke");

                buttonOpenExcel.Enabled = false;
                buttonDownloadFile.Enabled = false;
                buttonSaveData.Enabled = false;

                log.Info("button disabled");

                this.formLoad = new FormLoad();

                Model.InsertData(dataSet.Tables[tableName], (this.comboBoxSupplier.SelectedItem as ComboboxItem).Value.ToString(), comboBoxImportCurrency.Text);

                log.Info("Model.InsertData was finished");

                this.dataGrid_import_excel.DataSource = null;
                this.dataGrid_import_excel.Rows.Clear();
                dataSet.Tables[tableName].Clear();
                lbl_Counter.Text = "0";
                lbl_TotalCount.Text = "0";
                label_TimeSpan.Text = "";
                log.Info("Clear grid");
                
                this.formLoad.ShowDialog();
            }));
        }

        private void saveData_OnLoaded()
        {
            buttonOpenExcel.Invoke(new Action(() =>
            {
                buttonOpenExcel.Enabled = true;
            }));

            buttonDownloadFile.Invoke(new Action(() =>
            {
                buttonDownloadFile.Enabled = true;
            }));

            buttonSaveData.Invoke(new Action(() =>
            {
                buttonSaveData.Enabled = true;
            }));

            if (this.formLoad != null)
            {
                formLoad.Invoke(new Action(() =>
                {
                    this.formLoad.Close();
                }));
            }

            ReceiveData.instance.OnLoaded -= saveData_OnLoaded;
        }
        
        private void PriceUploader_Shown(object sender, EventArgs e)
        {
            formLoad = new FormLoad();
            formLoad.ShowDialog();
        }

        private void FormLoadMessage(string message, bool showMessage)
        {
            if (!showMessage)
                return;

            try
            {
                formLoad.Invoke(new Action(() =>
                {
                    formLoad.CurrentTask = message;
                }));
            }
            catch { }

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

    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

}
