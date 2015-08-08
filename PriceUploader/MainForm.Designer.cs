namespace PriceUploader
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemWork = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImportPrices = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDatabaseSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageImport = new System.Windows.Forms.TabPage();
            this.dataGrid_import_excel = new System.Windows.Forms.DataGridView();
            this.prodrewritenameoldDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prodnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prodincomepriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource_import_excel = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet = new System.Data.DataSet();
            this.dataTable_import_settings = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.dataColumn5 = new System.Data.DataColumn();
            this.dataColumn6 = new System.Data.DataColumn();
            this.dataColumn7 = new System.Data.DataColumn();
            this.dataColumn8 = new System.Data.DataColumn();
            this.dataColumn9 = new System.Data.DataColumn();
            this.dataColumn10 = new System.Data.DataColumn();
            this.dataColumn11 = new System.Data.DataColumn();
            this.dataColumn12 = new System.Data.DataColumn();
            this.dataTable_import_excel = new System.Data.DataTable();
            this.dataColumn13 = new System.Data.DataColumn();
            this.dataColumn14 = new System.Data.DataColumn();
            this.dataColumn15 = new System.Data.DataColumn();
            this.dataColumn16 = new System.Data.DataColumn();
            this.label_file_name = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonOpenExcel = new System.Windows.Forms.Button();
            this.comboBoxImportCurrency = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.comboBoxFormat = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.comboBoxSupplier = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonDatabaseSettings = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.dataGrid_import_settings = new System.Windows.Forms.DataGridView();
            this.isnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isstartrowDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iscodecolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ispricecolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isnamecolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isactualityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ispresense1colDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ispresense2colDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ispresensesymbolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iscurrencycolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isuahflagDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource__import_settings = new System.Windows.Forms.BindingSource(this.components);
            this.comboBoxCurrency = new System.Windows.Forms.ComboBox();
            this.comboBoxAvailability2 = new System.Windows.Forms.ComboBox();
            this.comboBoxAvailability1 = new System.Windows.Forms.ComboBox();
            this.comboBoxProductName = new System.Windows.Forms.ComboBox();
            this.comboBoxPrice = new System.Windows.Forms.ComboBox();
            this.comboBoxCode = new System.Windows.Forms.ComboBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBoxActuality = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxPriceGrn = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxAvailSign = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFirstRow = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageImport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_import_excel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_import_excel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable_import_settings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable_import_excel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPageSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_import_settings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource__import_settings)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemWork,
            this.toolStripMenuItemExit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1186, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItemWork
            // 
            this.toolStripMenuItemWork.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemImportPrices,
            this.toolStripMenuItemSettings,
            this.toolStripMenuItemDatabaseSettings});
            this.toolStripMenuItemWork.Name = "toolStripMenuItemWork";
            this.toolStripMenuItemWork.Size = new System.Drawing.Size(57, 20);
            this.toolStripMenuItemWork.Text = "Работа";
            // 
            // toolStripMenuItemImportPrices
            // 
            this.toolStripMenuItemImportPrices.Name = "toolStripMenuItemImportPrices";
            this.toolStripMenuItemImportPrices.Size = new System.Drawing.Size(207, 22);
            this.toolStripMenuItemImportPrices.Text = "Импорт прайсов";
            this.toolStripMenuItemImportPrices.Click += new System.EventHandler(this.toolStripMenuItemImportPrices_Click);
            // 
            // toolStripMenuItemSettings
            // 
            this.toolStripMenuItemSettings.Name = "toolStripMenuItemSettings";
            this.toolStripMenuItemSettings.Size = new System.Drawing.Size(207, 22);
            this.toolStripMenuItemSettings.Text = "Hастройки импорта";
            this.toolStripMenuItemSettings.Click += new System.EventHandler(this.toolStripMenuItemSettings_Click);
            // 
            // toolStripMenuItemDatabaseSettings
            // 
            this.toolStripMenuItemDatabaseSettings.Name = "toolStripMenuItemDatabaseSettings";
            this.toolStripMenuItemDatabaseSettings.Size = new System.Drawing.Size(207, 22);
            this.toolStripMenuItemDatabaseSettings.Text = "Настройки доступа к БД";
            this.toolStripMenuItemDatabaseSettings.Click += new System.EventHandler(this.toolStripMenuItemDatabaseSettings_Click);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(53, 20);
            this.toolStripMenuItemExit.Text = "Выход";
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.toolStripMenuItemExit_Click);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageImport);
            this.tabControlMain.Controls.Add(this.tabPageSettings);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 24);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1186, 604);
            this.tabControlMain.TabIndex = 1;
            // 
            // tabPageImport
            // 
            this.tabPageImport.Controls.Add(this.dataGrid_import_excel);
            this.tabPageImport.Controls.Add(this.label_file_name);
            this.tabPageImport.Controls.Add(this.button2);
            this.tabPageImport.Controls.Add(this.buttonOpenExcel);
            this.tabPageImport.Controls.Add(this.comboBoxImportCurrency);
            this.tabPageImport.Controls.Add(this.label14);
            this.tabPageImport.Controls.Add(this.comboBoxFormat);
            this.tabPageImport.Controls.Add(this.label13);
            this.tabPageImport.Controls.Add(this.comboBoxSupplier);
            this.tabPageImport.Controls.Add(this.label12);
            this.tabPageImport.Controls.Add(this.dataGridView1);
            this.tabPageImport.Location = new System.Drawing.Point(4, 22);
            this.tabPageImport.Name = "tabPageImport";
            this.tabPageImport.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageImport.Size = new System.Drawing.Size(1178, 578);
            this.tabPageImport.TabIndex = 0;
            this.tabPageImport.Text = "Импорт";
            this.tabPageImport.UseVisualStyleBackColor = true;
            // 
            // dataGrid_import_excel
            // 
            this.dataGrid_import_excel.AllowUserToAddRows = false;
            this.dataGrid_import_excel.AllowUserToDeleteRows = false;
            this.dataGrid_import_excel.AutoGenerateColumns = false;
            this.dataGrid_import_excel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid_import_excel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.prodrewritenameoldDataGridViewTextBoxColumn,
            this.prodnameDataGridViewTextBoxColumn,
            this.prodincomepriceDataGridViewTextBoxColumn,
            this.numberDataGridViewTextBoxColumn});
            this.dataGrid_import_excel.DataSource = this.bindingSource_import_excel;
            this.dataGrid_import_excel.Location = new System.Drawing.Point(8, 36);
            this.dataGrid_import_excel.Name = "dataGrid_import_excel";
            this.dataGrid_import_excel.ReadOnly = true;
            this.dataGrid_import_excel.Size = new System.Drawing.Size(1167, 536);
            this.dataGrid_import_excel.TabIndex = 10;
            // 
            // prodrewritenameoldDataGridViewTextBoxColumn
            // 
            this.prodrewritenameoldDataGridViewTextBoxColumn.DataPropertyName = "prod_rewrite_name_old";
            this.prodrewritenameoldDataGridViewTextBoxColumn.FillWeight = 200F;
            this.prodrewritenameoldDataGridViewTextBoxColumn.HeaderText = "prod_rewrite_name_old";
            this.prodrewritenameoldDataGridViewTextBoxColumn.Name = "prodrewritenameoldDataGridViewTextBoxColumn";
            this.prodrewritenameoldDataGridViewTextBoxColumn.ReadOnly = true;
            this.prodrewritenameoldDataGridViewTextBoxColumn.Width = 200;
            // 
            // prodnameDataGridViewTextBoxColumn
            // 
            this.prodnameDataGridViewTextBoxColumn.DataPropertyName = "prod_name";
            this.prodnameDataGridViewTextBoxColumn.FillWeight = 600F;
            this.prodnameDataGridViewTextBoxColumn.HeaderText = "prod_name";
            this.prodnameDataGridViewTextBoxColumn.Name = "prodnameDataGridViewTextBoxColumn";
            this.prodnameDataGridViewTextBoxColumn.ReadOnly = true;
            this.prodnameDataGridViewTextBoxColumn.Width = 600;
            // 
            // prodincomepriceDataGridViewTextBoxColumn
            // 
            this.prodincomepriceDataGridViewTextBoxColumn.DataPropertyName = "prod_income_price";
            this.prodincomepriceDataGridViewTextBoxColumn.FillWeight = 150F;
            this.prodincomepriceDataGridViewTextBoxColumn.HeaderText = "prod_income_price";
            this.prodincomepriceDataGridViewTextBoxColumn.Name = "prodincomepriceDataGridViewTextBoxColumn";
            this.prodincomepriceDataGridViewTextBoxColumn.ReadOnly = true;
            this.prodincomepriceDataGridViewTextBoxColumn.Width = 150;
            // 
            // numberDataGridViewTextBoxColumn
            // 
            this.numberDataGridViewTextBoxColumn.DataPropertyName = "number";
            this.numberDataGridViewTextBoxColumn.HeaderText = "number";
            this.numberDataGridViewTextBoxColumn.Name = "numberDataGridViewTextBoxColumn";
            this.numberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bindingSource_import_excel
            // 
            this.bindingSource_import_excel.DataMember = "Table_import_excel";
            this.bindingSource_import_excel.DataSource = this.dataSet;
            // 
            // dataSet
            // 
            this.dataSet.DataSetName = "dataSet";
            this.dataSet.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTable_import_settings,
            this.dataTable_import_excel});
            // 
            // dataTable_import_settings
            // 
            this.dataTable_import_settings.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2,
            this.dataColumn3,
            this.dataColumn4,
            this.dataColumn5,
            this.dataColumn6,
            this.dataColumn7,
            this.dataColumn8,
            this.dataColumn9,
            this.dataColumn10,
            this.dataColumn11,
            this.dataColumn12});
            this.dataTable_import_settings.TableName = "Table_import_settings";
            // 
            // dataColumn1
            // 
            this.dataColumn1.Caption = "is_id";
            this.dataColumn1.ColumnName = "is_id";
            this.dataColumn1.DataType = typeof(int);
            // 
            // dataColumn2
            // 
            this.dataColumn2.Caption = "is_name";
            this.dataColumn2.ColumnName = "is_name";
            // 
            // dataColumn3
            // 
            this.dataColumn3.Caption = "is_start_row";
            this.dataColumn3.ColumnName = "is_start_row";
            this.dataColumn3.DataType = typeof(int);
            // 
            // dataColumn4
            // 
            this.dataColumn4.Caption = "is_code_col";
            this.dataColumn4.ColumnName = "is_code_col";
            // 
            // dataColumn5
            // 
            this.dataColumn5.Caption = "is_price_col";
            this.dataColumn5.ColumnName = "is_price_col";
            // 
            // dataColumn6
            // 
            this.dataColumn6.Caption = "is_name_col";
            this.dataColumn6.ColumnName = "is_name_col";
            // 
            // dataColumn7
            // 
            this.dataColumn7.Caption = "is_actuality";
            this.dataColumn7.ColumnName = "is_actuality";
            this.dataColumn7.DataType = typeof(int);
            // 
            // dataColumn8
            // 
            this.dataColumn8.Caption = "is_presense1_col";
            this.dataColumn8.ColumnName = "is_presense1_col";
            // 
            // dataColumn9
            // 
            this.dataColumn9.Caption = "is_presense2_col";
            this.dataColumn9.ColumnName = "is_presense2_col";
            // 
            // dataColumn10
            // 
            this.dataColumn10.Caption = "is_presense_symbol";
            this.dataColumn10.ColumnName = "is_presense_symbol";
            // 
            // dataColumn11
            // 
            this.dataColumn11.Caption = "is_currency_col";
            this.dataColumn11.ColumnName = "is_currency_col";
            // 
            // dataColumn12
            // 
            this.dataColumn12.Caption = "is_uah_flag";
            this.dataColumn12.ColumnName = "is_uah_flag";
            // 
            // dataTable_import_excel
            // 
            this.dataTable_import_excel.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn13,
            this.dataColumn14,
            this.dataColumn15,
            this.dataColumn16});
            this.dataTable_import_excel.TableName = "Table_import_excel";
            // 
            // dataColumn13
            // 
            this.dataColumn13.Caption = "prod_name";
            this.dataColumn13.ColumnName = "prod_name";
            // 
            // dataColumn14
            // 
            this.dataColumn14.Caption = "prod_rewrite_name_old";
            this.dataColumn14.ColumnName = "prod_rewrite_name_old";
            // 
            // dataColumn15
            // 
            this.dataColumn15.Caption = "prod_income_price";
            this.dataColumn15.ColumnName = "prod_income_price";
            // 
            // dataColumn16
            // 
            this.dataColumn16.Caption = "number";
            this.dataColumn16.ColumnName = "number";
            // 
            // label_file_name
            // 
            this.label_file_name.AutoSize = true;
            this.label_file_name.Location = new System.Drawing.Point(734, 13);
            this.label_file_name.Name = "label_file_name";
            this.label_file_name.Size = new System.Drawing.Size(92, 13);
            this.label_file_name.TabIndex = 9;
            this.label_file_name.Text = "Файл не выбран";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1056, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(114, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Загрузить файл";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // buttonOpenExcel
            // 
            this.buttonOpenExcel.Location = new System.Drawing.Point(652, 6);
            this.buttonOpenExcel.Name = "buttonOpenExcel";
            this.buttonOpenExcel.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenExcel.TabIndex = 7;
            this.buttonOpenExcel.Text = "Файл";
            this.buttonOpenExcel.UseVisualStyleBackColor = true;
            this.buttonOpenExcel.Click += new System.EventHandler(this.buttonOpenExcel_Click);
            // 
            // comboBoxImportCurrency
            // 
            this.comboBoxImportCurrency.FormattingEnabled = true;
            this.comboBoxImportCurrency.Items.AddRange(new object[] {
            "USD",
            "EUR",
            "РРЦ"});
            this.comboBoxImportCurrency.Location = new System.Drawing.Point(502, 9);
            this.comboBoxImportCurrency.Name = "comboBoxImportCurrency";
            this.comboBoxImportCurrency.Size = new System.Drawing.Size(121, 21);
            this.comboBoxImportCurrency.TabIndex = 6;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(450, 13);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(45, 13);
            this.label14.TabIndex = 5;
            this.label14.Text = "Валюта";
            // 
            // comboBoxFormat
            // 
            this.comboBoxFormat.FormattingEnabled = true;
            this.comboBoxFormat.Location = new System.Drawing.Point(293, 8);
            this.comboBoxFormat.Name = "comboBoxFormat";
            this.comboBoxFormat.Size = new System.Drawing.Size(151, 21);
            this.comboBoxFormat.TabIndex = 4;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(240, 12);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(49, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "Формат";
            // 
            // comboBoxSupplier
            // 
            this.comboBoxSupplier.FormattingEnabled = true;
            this.comboBoxSupplier.Location = new System.Drawing.Point(81, 7);
            this.comboBoxSupplier.Name = "comboBoxSupplier";
            this.comboBoxSupplier.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSupplier.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(11, 11);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Поставщик";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 35);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1169, 538);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.splitContainer1);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(1178, 578);
            this.tabPageSettings.TabIndex = 1;
            this.tabPageSettings.Text = "Настройки";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonDelete);
            this.splitContainer1.Panel1.Controls.Add(this.buttonDatabaseSettings);
            this.splitContainer1.Panel1.Controls.Add(this.buttonAdd);
            this.splitContainer1.Panel1.Controls.Add(this.dataGrid_import_settings);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.comboBoxCurrency);
            this.splitContainer1.Panel2.Controls.Add(this.comboBoxAvailability2);
            this.splitContainer1.Panel2.Controls.Add(this.comboBoxAvailability1);
            this.splitContainer1.Panel2.Controls.Add(this.comboBoxProductName);
            this.splitContainer1.Panel2.Controls.Add(this.comboBoxPrice);
            this.splitContainer1.Panel2.Controls.Add(this.comboBoxCode);
            this.splitContainer1.Panel2.Controls.Add(this.buttonSave);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxActuality);
            this.splitContainer1.Panel2.Controls.Add(this.label11);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxPriceGrn);
            this.splitContainer1.Panel2.Controls.Add(this.label10);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxAvailSign);
            this.splitContainer1.Panel2.Controls.Add(this.label9);
            this.splitContainer1.Panel2.Controls.Add(this.label8);
            this.splitContainer1.Panel2.Controls.Add(this.label7);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxFirstRow);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxName);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(1172, 572);
            this.splitContainer1.SplitterDistance = 885;
            this.splitContainer1.TabIndex = 0;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(270, 6);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(259, 23);
            this.buttonDelete.TabIndex = 25;
            this.buttonDelete.Text = "Удалить";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonDatabaseSettings
            // 
            this.buttonDatabaseSettings.Location = new System.Drawing.Point(623, 6);
            this.buttonDatabaseSettings.Name = "buttonDatabaseSettings";
            this.buttonDatabaseSettings.Size = new System.Drawing.Size(259, 23);
            this.buttonDatabaseSettings.TabIndex = 22;
            this.buttonDatabaseSettings.Text = "Настройки доступа к БД";
            this.buttonDatabaseSettings.UseVisualStyleBackColor = true;
            this.buttonDatabaseSettings.Click += new System.EventHandler(this.buttonDatabaseSettings_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(3, 6);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(259, 23);
            this.buttonAdd.TabIndex = 24;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // dataGrid_import_settings
            // 
            this.dataGrid_import_settings.AutoGenerateColumns = false;
            this.dataGrid_import_settings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid_import_settings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.isnameDataGridViewTextBoxColumn,
            this.isstartrowDataGridViewTextBoxColumn,
            this.iscodecolDataGridViewTextBoxColumn,
            this.ispricecolDataGridViewTextBoxColumn,
            this.isnamecolDataGridViewTextBoxColumn,
            this.isactualityDataGridViewTextBoxColumn,
            this.ispresense1colDataGridViewTextBoxColumn,
            this.ispresense2colDataGridViewTextBoxColumn,
            this.ispresensesymbolDataGridViewTextBoxColumn,
            this.iscurrencycolDataGridViewTextBoxColumn,
            this.isuahflagDataGridViewTextBoxColumn,
            this.isidDataGridViewTextBoxColumn});
            this.dataGrid_import_settings.DataSource = this.bindingSource__import_settings;
            this.dataGrid_import_settings.Location = new System.Drawing.Point(3, 35);
            this.dataGrid_import_settings.Name = "dataGrid_import_settings";
            this.dataGrid_import_settings.Size = new System.Drawing.Size(879, 531);
            this.dataGrid_import_settings.TabIndex = 0;
            this.dataGrid_import_settings.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGrid_import_settings_RowEnter);
            this.dataGrid_import_settings.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGrid_import_settings_RowsAdded);
            // 
            // isnameDataGridViewTextBoxColumn
            // 
            this.isnameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.isnameDataGridViewTextBoxColumn.DataPropertyName = "is_name";
            this.isnameDataGridViewTextBoxColumn.HeaderText = "Имена форматов";
            this.isnameDataGridViewTextBoxColumn.Name = "isnameDataGridViewTextBoxColumn";
            // 
            // isstartrowDataGridViewTextBoxColumn
            // 
            this.isstartrowDataGridViewTextBoxColumn.DataPropertyName = "is_start_row";
            this.isstartrowDataGridViewTextBoxColumn.HeaderText = "is_start_row";
            this.isstartrowDataGridViewTextBoxColumn.Name = "isstartrowDataGridViewTextBoxColumn";
            this.isstartrowDataGridViewTextBoxColumn.Visible = false;
            // 
            // iscodecolDataGridViewTextBoxColumn
            // 
            this.iscodecolDataGridViewTextBoxColumn.DataPropertyName = "is_code_col";
            this.iscodecolDataGridViewTextBoxColumn.HeaderText = "is_code_col";
            this.iscodecolDataGridViewTextBoxColumn.Name = "iscodecolDataGridViewTextBoxColumn";
            this.iscodecolDataGridViewTextBoxColumn.Visible = false;
            // 
            // ispricecolDataGridViewTextBoxColumn
            // 
            this.ispricecolDataGridViewTextBoxColumn.DataPropertyName = "is_price_col";
            this.ispricecolDataGridViewTextBoxColumn.HeaderText = "is_price_col";
            this.ispricecolDataGridViewTextBoxColumn.Name = "ispricecolDataGridViewTextBoxColumn";
            this.ispricecolDataGridViewTextBoxColumn.Visible = false;
            // 
            // isnamecolDataGridViewTextBoxColumn
            // 
            this.isnamecolDataGridViewTextBoxColumn.DataPropertyName = "is_name_col";
            this.isnamecolDataGridViewTextBoxColumn.HeaderText = "is_name_col";
            this.isnamecolDataGridViewTextBoxColumn.Name = "isnamecolDataGridViewTextBoxColumn";
            this.isnamecolDataGridViewTextBoxColumn.Visible = false;
            // 
            // isactualityDataGridViewTextBoxColumn
            // 
            this.isactualityDataGridViewTextBoxColumn.DataPropertyName = "is_actuality";
            this.isactualityDataGridViewTextBoxColumn.HeaderText = "is_actuality";
            this.isactualityDataGridViewTextBoxColumn.Name = "isactualityDataGridViewTextBoxColumn";
            this.isactualityDataGridViewTextBoxColumn.Visible = false;
            // 
            // ispresense1colDataGridViewTextBoxColumn
            // 
            this.ispresense1colDataGridViewTextBoxColumn.DataPropertyName = "is_presense1_col";
            this.ispresense1colDataGridViewTextBoxColumn.HeaderText = "is_presense1_col";
            this.ispresense1colDataGridViewTextBoxColumn.Name = "ispresense1colDataGridViewTextBoxColumn";
            this.ispresense1colDataGridViewTextBoxColumn.Visible = false;
            // 
            // ispresense2colDataGridViewTextBoxColumn
            // 
            this.ispresense2colDataGridViewTextBoxColumn.DataPropertyName = "is_presense2_col";
            this.ispresense2colDataGridViewTextBoxColumn.HeaderText = "is_presense2_col";
            this.ispresense2colDataGridViewTextBoxColumn.Name = "ispresense2colDataGridViewTextBoxColumn";
            this.ispresense2colDataGridViewTextBoxColumn.Visible = false;
            // 
            // ispresensesymbolDataGridViewTextBoxColumn
            // 
            this.ispresensesymbolDataGridViewTextBoxColumn.DataPropertyName = "is_presense_symbol";
            this.ispresensesymbolDataGridViewTextBoxColumn.HeaderText = "is_presense_symbol";
            this.ispresensesymbolDataGridViewTextBoxColumn.Name = "ispresensesymbolDataGridViewTextBoxColumn";
            this.ispresensesymbolDataGridViewTextBoxColumn.Visible = false;
            // 
            // iscurrencycolDataGridViewTextBoxColumn
            // 
            this.iscurrencycolDataGridViewTextBoxColumn.DataPropertyName = "is_currency_col";
            this.iscurrencycolDataGridViewTextBoxColumn.HeaderText = "is_currency_col";
            this.iscurrencycolDataGridViewTextBoxColumn.Name = "iscurrencycolDataGridViewTextBoxColumn";
            this.iscurrencycolDataGridViewTextBoxColumn.Visible = false;
            // 
            // isuahflagDataGridViewTextBoxColumn
            // 
            this.isuahflagDataGridViewTextBoxColumn.DataPropertyName = "is_uah_flag";
            this.isuahflagDataGridViewTextBoxColumn.HeaderText = "is_uah_flag";
            this.isuahflagDataGridViewTextBoxColumn.Name = "isuahflagDataGridViewTextBoxColumn";
            this.isuahflagDataGridViewTextBoxColumn.Visible = false;
            // 
            // isidDataGridViewTextBoxColumn
            // 
            this.isidDataGridViewTextBoxColumn.DataPropertyName = "is_id";
            this.isidDataGridViewTextBoxColumn.FillWeight = 5F;
            this.isidDataGridViewTextBoxColumn.HeaderText = "is_id";
            this.isidDataGridViewTextBoxColumn.Name = "isidDataGridViewTextBoxColumn";
            this.isidDataGridViewTextBoxColumn.ReadOnly = true;
            this.isidDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.isidDataGridViewTextBoxColumn.Visible = false;
            this.isidDataGridViewTextBoxColumn.Width = 5;
            // 
            // bindingSource__import_settings
            // 
            this.bindingSource__import_settings.DataMember = "Table_import_settings";
            this.bindingSource__import_settings.DataSource = this.dataSet;
            // 
            // comboBoxCurrency
            // 
            this.comboBoxCurrency.FormattingEnabled = true;
            this.comboBoxCurrency.Location = new System.Drawing.Point(12, 347);
            this.comboBoxCurrency.Name = "comboBoxCurrency";
            this.comboBoxCurrency.Size = new System.Drawing.Size(259, 21);
            this.comboBoxCurrency.TabIndex = 29;
            // 
            // comboBoxAvailability2
            // 
            this.comboBoxAvailability2.FormattingEnabled = true;
            this.comboBoxAvailability2.Location = new System.Drawing.Point(12, 301);
            this.comboBoxAvailability2.Name = "comboBoxAvailability2";
            this.comboBoxAvailability2.Size = new System.Drawing.Size(259, 21);
            this.comboBoxAvailability2.TabIndex = 28;
            // 
            // comboBoxAvailability1
            // 
            this.comboBoxAvailability1.FormattingEnabled = true;
            this.comboBoxAvailability1.Location = new System.Drawing.Point(12, 254);
            this.comboBoxAvailability1.Name = "comboBoxAvailability1";
            this.comboBoxAvailability1.Size = new System.Drawing.Size(259, 21);
            this.comboBoxAvailability1.TabIndex = 27;
            // 
            // comboBoxProductName
            // 
            this.comboBoxProductName.FormattingEnabled = true;
            this.comboBoxProductName.Location = new System.Drawing.Point(12, 210);
            this.comboBoxProductName.Name = "comboBoxProductName";
            this.comboBoxProductName.Size = new System.Drawing.Size(259, 21);
            this.comboBoxProductName.TabIndex = 26;
            // 
            // comboBoxPrice
            // 
            this.comboBoxPrice.FormattingEnabled = true;
            this.comboBoxPrice.Location = new System.Drawing.Point(12, 163);
            this.comboBoxPrice.Name = "comboBoxPrice";
            this.comboBoxPrice.Size = new System.Drawing.Size(259, 21);
            this.comboBoxPrice.TabIndex = 25;
            // 
            // comboBoxCode
            // 
            this.comboBoxCode.FormattingEnabled = true;
            this.comboBoxCode.Location = new System.Drawing.Point(12, 115);
            this.comboBoxCode.Name = "comboBoxCode";
            this.comboBoxCode.Size = new System.Drawing.Size(259, 21);
            this.comboBoxCode.TabIndex = 24;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(12, 516);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(259, 50);
            this.buttonSave.TabIndex = 23;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxActuality
            // 
            this.textBoxActuality.Location = new System.Drawing.Point(12, 490);
            this.textBoxActuality.Name = "textBoxActuality";
            this.textBoxActuality.Size = new System.Drawing.Size(259, 20);
            this.textBoxActuality.TabIndex = 21;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 473);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(139, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Актуальность цены (дней)";
            // 
            // textBoxPriceGrn
            // 
            this.textBoxPriceGrn.Location = new System.Drawing.Point(12, 442);
            this.textBoxPriceGrn.Name = "textBoxPriceGrn";
            this.textBoxPriceGrn.Size = new System.Drawing.Size(259, 20);
            this.textBoxPriceGrn.TabIndex = 19;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 425);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(132, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Признак цены в гривнах";
            // 
            // textBoxAvailSign
            // 
            this.textBoxAvailSign.Location = new System.Drawing.Point(12, 395);
            this.textBoxAvailSign.Name = "textBoxAvailSign";
            this.textBoxAvailSign.Size = new System.Drawing.Size(259, 20);
            this.textBoxAvailSign.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 378);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Признак наличия";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 331);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(137, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Колонка отметки валюты";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 285);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Колонка наличия 2";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 238);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Колонка наличия 1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 194);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Колонка названия";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Колонка цены";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Колонка кода товара";
            // 
            // textBoxFirstRow
            // 
            this.textBoxFirstRow.Location = new System.Drawing.Point(12, 68);
            this.textBoxFirstRow.Name = "textBoxFirstRow";
            this.textBoxFirstRow.Size = new System.Drawing.Size(259, 20);
            this.textBoxFirstRow.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Первая строка прайса";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(12, 23);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(259, 20);
            this.textBoxName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Название";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 628);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Импорт прайс-листов";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageImport.ResumeLayout(false);
            this.tabPageImport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_import_excel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_import_excel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable_import_settings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable_import_excel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPageSettings.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_import_settings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource__import_settings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemWork;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImportPrices;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageImport;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGrid_import_settings;
        private System.Windows.Forms.TextBox textBoxActuality;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxPriceGrn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxAvailSign;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxFirstRow;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button buttonDatabaseSettings;
        private System.Windows.Forms.Label label_file_name;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonOpenExcel;
        private System.Windows.Forms.ComboBox comboBoxImportCurrency;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboBoxFormat;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboBoxSupplier;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDatabaseSettings;
        private System.Windows.Forms.BindingSource bindingSource__import_settings;
        private System.Data.DataSet dataSet;
        private System.Data.DataTable dataTable_import_settings;
        private System.Data.DataColumn dataColumn1;
        private System.Data.DataColumn dataColumn2;
        private System.Data.DataColumn dataColumn3;
        private System.Data.DataColumn dataColumn4;
        private System.Data.DataColumn dataColumn5;
        private System.Data.DataColumn dataColumn6;
        private System.Data.DataColumn dataColumn7;
        private System.Data.DataColumn dataColumn8;
        private System.Data.DataColumn dataColumn9;
        private System.Data.DataColumn dataColumn10;
        private System.Data.DataColumn dataColumn11;
        private System.Data.DataColumn dataColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn isnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn isstartrowDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn iscodecolDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ispricecolDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn isnamecolDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn isactualityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ispresense1colDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ispresense2colDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ispresensesymbolDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn iscurrencycolDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn isuahflagDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn isidDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonSave;

        private System.Windows.Forms.DataGridView dataGrid_import_excel;
        private System.Windows.Forms.BindingSource bindingSource_import_excel;
        private System.Data.DataTable dataTable_import_excel;
        private System.Data.DataColumn dataColumn13;
        private System.Data.DataColumn dataColumn14;
        private System.Data.DataColumn dataColumn15;
        private System.Data.DataColumn dataColumn16;
        private System.Windows.Forms.DataGridViewTextBoxColumn prodrewritenameoldDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn prodnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn prodincomepriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberDataGridViewTextBoxColumn;

        private System.Windows.Forms.ComboBox comboBoxCode;
        private System.Windows.Forms.ComboBox comboBoxCurrency;
        private System.Windows.Forms.ComboBox comboBoxAvailability2;
        private System.Windows.Forms.ComboBox comboBoxAvailability1;
        private System.Windows.Forms.ComboBox comboBoxProductName;
        private System.Windows.Forms.ComboBox comboBoxPrice;

    }
}