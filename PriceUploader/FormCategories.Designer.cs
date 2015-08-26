namespace PriceUploader
{
    partial class FormCategories
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
            this.treeViewCategories = new System.Windows.Forms.TreeView();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeViewCategories
            // 
            this.treeViewCategories.Location = new System.Drawing.Point(1, 2);
            this.treeViewCategories.Name = "treeViewCategories";
            this.treeViewCategories.Size = new System.Drawing.Size(503, 494);
            this.treeViewCategories.TabIndex = 0;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(375, 502);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(119, 23);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Отмена";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(3, 502);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(119, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Выбрать";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // FormCategories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 533);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.treeViewCategories);
            this.Name = "FormCategories";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Категории";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewCategories;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSave;
    }
}