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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.smiWork = new System.Windows.Forms.ToolStripMenuItem();
            this.smiImportPrices = new System.Windows.Forms.ToolStripMenuItem();
            this.smiSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.smiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpImport = new System.Windows.Forms.TabPage();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxFirstRow = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPrice = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxProductName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxAvailability1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxAvailability2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxCurrency = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxAvailSign = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxPriceGrn = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxActuality = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.buttonDatabaseSettings = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpImport.SuspendLayout();
            this.tpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smiWork,
            this.smiExit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1186, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // smiWork
            // 
            this.smiWork.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smiImportPrices,
            this.smiSettings});
            this.smiWork.Name = "smiWork";
            this.smiWork.Size = new System.Drawing.Size(57, 20);
            this.smiWork.Text = "Работа";
            // 
            // smiImportPrices
            // 
            this.smiImportPrices.Name = "smiImportPrices";
            this.smiImportPrices.Size = new System.Drawing.Size(185, 22);
            this.smiImportPrices.Text = "Импорт прайсов";
            this.smiImportPrices.Click += new System.EventHandler(this.smiImportPrices_Click);
            // 
            // smiSettings
            // 
            this.smiSettings.Name = "smiSettings";
            this.smiSettings.Size = new System.Drawing.Size(185, 22);
            this.smiSettings.Text = "Hастройки импорта";
            this.smiSettings.Click += new System.EventHandler(this.smiSettings_Click);
            // 
            // smiExit
            // 
            this.smiExit.Name = "smiExit";
            this.smiExit.Size = new System.Drawing.Size(53, 20);
            this.smiExit.Text = "Выход";
            this.smiExit.Click += new System.EventHandler(this.smiExit_Click);
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpImport);
            this.tcMain.Controls.Add(this.tpSettings);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 24);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(1186, 603);
            this.tcMain.TabIndex = 1;
            // 
            // tpImport
            // 
            this.tpImport.Controls.Add(this.label15);
            this.tpImport.Controls.Add(this.button2);
            this.tpImport.Controls.Add(this.button1);
            this.tpImport.Controls.Add(this.comboBox3);
            this.tpImport.Controls.Add(this.label14);
            this.tpImport.Controls.Add(this.comboBox2);
            this.tpImport.Controls.Add(this.label13);
            this.tpImport.Controls.Add(this.comboBox1);
            this.tpImport.Controls.Add(this.label12);
            this.tpImport.Controls.Add(this.dataGridView1);
            this.tpImport.Location = new System.Drawing.Point(4, 22);
            this.tpImport.Name = "tpImport";
            this.tpImport.Padding = new System.Windows.Forms.Padding(3);
            this.tpImport.Size = new System.Drawing.Size(1178, 577);
            this.tpImport.TabIndex = 0;
            this.tpImport.Text = "Импорт";
            this.tpImport.UseVisualStyleBackColor = true;
            // 
            // tpSettings
            // 
            this.tpSettings.Controls.Add(this.splitContainer1);
            this.tpSettings.Location = new System.Drawing.Point(4, 22);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettings.Size = new System.Drawing.Size(1178, 577);
            this.tpSettings.TabIndex = 1;
            this.tpSettings.Text = "Настройки";
            this.tpSettings.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 36);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1169, 538);
            this.dataGridView1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.buttonDatabaseSettings);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxActuality);
            this.splitContainer1.Panel2.Controls.Add(this.label11);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxPriceGrn);
            this.splitContainer1.Panel2.Controls.Add(this.label10);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxAvailSign);
            this.splitContainer1.Panel2.Controls.Add(this.label9);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxCurrency);
            this.splitContainer1.Panel2.Controls.Add(this.label8);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxAvailability2);
            this.splitContainer1.Panel2.Controls.Add(this.label7);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxAvailability1);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxProductName);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxPrice);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxCode);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxFirstRow);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxName);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(1172, 571);
            this.splitContainer1.SplitterDistance = 885;
            this.splitContainer1.TabIndex = 0;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(3, 3);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(879, 563);
            this.dataGridView2.TabIndex = 0;
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
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(12, 23);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(259, 20);
            this.textBoxName.TabIndex = 1;
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
            // textBoxCode
            // 
            this.textBoxCode.Location = new System.Drawing.Point(12, 115);
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(259, 20);
            this.textBoxCode.TabIndex = 5;
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
            // textBoxPrice
            // 
            this.textBoxPrice.Location = new System.Drawing.Point(12, 164);
            this.textBoxPrice.Name = "textBoxPrice";
            this.textBoxPrice.Size = new System.Drawing.Size(259, 20);
            this.textBoxPrice.TabIndex = 7;
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
            // textBoxProductName
            // 
            this.textBoxProductName.Location = new System.Drawing.Point(12, 211);
            this.textBoxProductName.Name = "textBoxProductName";
            this.textBoxProductName.Size = new System.Drawing.Size(259, 20);
            this.textBoxProductName.TabIndex = 9;
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
            // textBoxAvailability1
            // 
            this.textBoxAvailability1.Location = new System.Drawing.Point(12, 255);
            this.textBoxAvailability1.Name = "textBoxAvailability1";
            this.textBoxAvailability1.Size = new System.Drawing.Size(259, 20);
            this.textBoxAvailability1.TabIndex = 11;
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
            // textBoxAvailability2
            // 
            this.textBoxAvailability2.Location = new System.Drawing.Point(12, 302);
            this.textBoxAvailability2.Name = "textBoxAvailability2";
            this.textBoxAvailability2.Size = new System.Drawing.Size(259, 20);
            this.textBoxAvailability2.TabIndex = 13;
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
            // textBoxCurrency
            // 
            this.textBoxCurrency.Location = new System.Drawing.Point(12, 348);
            this.textBoxCurrency.Name = "textBoxCurrency";
            this.textBoxCurrency.Size = new System.Drawing.Size(259, 20);
            this.textBoxCurrency.TabIndex = 15;
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
            // textBoxPriceGrn
            // 
            this.textBoxPriceGrn.Location = new System.Drawing.Point(12, 443);
            this.textBoxPriceGrn.Name = "textBoxPriceGrn";
            this.textBoxPriceGrn.Size = new System.Drawing.Size(259, 20);
            this.textBoxPriceGrn.TabIndex = 19;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 426);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(132, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Признак цены в гривнах";
            // 
            // textBoxActuality
            // 
            this.textBoxActuality.Location = new System.Drawing.Point(12, 493);
            this.textBoxActuality.Name = "textBoxActuality";
            this.textBoxActuality.Size = new System.Drawing.Size(259, 20);
            this.textBoxActuality.TabIndex = 21;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 476);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(139, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Актуальность цены (дней)";
            // 
            // buttonDatabaseSettings
            // 
            this.buttonDatabaseSettings.Location = new System.Drawing.Point(12, 543);
            this.buttonDatabaseSettings.Name = "buttonDatabaseSettings";
            this.buttonDatabaseSettings.Size = new System.Drawing.Size(259, 23);
            this.buttonDatabaseSettings.TabIndex = 22;
            this.buttonDatabaseSettings.Text = "Настройки доступа к БД";
            this.buttonDatabaseSettings.UseVisualStyleBackColor = true;
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
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(81, 7);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 2;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(293, 8);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 4;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(241, 12);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(49, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "Формат";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(502, 9);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 21);
            this.comboBox3.TabIndex = 6;
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(652, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Файл";
            this.button1.UseVisualStyleBackColor = true;
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
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(734, 13);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(92, 13);
            this.label15.TabIndex = 9;
            this.label15.Text = "Файл не выбран";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 627);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Импорт прайс-листов";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tcMain.ResumeLayout(false);
            this.tpImport.ResumeLayout(false);
            this.tpImport.PerformLayout();
            this.tpSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem smiWork;
        private System.Windows.Forms.ToolStripMenuItem smiImportPrices;
        private System.Windows.Forms.ToolStripMenuItem smiSettings;
        private System.Windows.Forms.ToolStripMenuItem smiExit;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpImport;
        private System.Windows.Forms.TabPage tpSettings;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.TextBox textBoxActuality;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxPriceGrn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxAvailSign;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxCurrency;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxAvailability2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxAvailability1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxProductName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxPrice;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxFirstRow;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button buttonDatabaseSettings;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}