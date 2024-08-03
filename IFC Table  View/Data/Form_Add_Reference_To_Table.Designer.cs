namespace IFC_Table_View.Data
{
    partial class Form_Add_Reference_To_Table
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
            this.button_Add_Reference = new System.Windows.Forms.Button();
            this.dataGridViewTable = new System.Windows.Forms.DataGridView();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.labelGUID = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTable)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Add_Reference
            // 
            this.button_Add_Reference.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_Add_Reference.Location = new System.Drawing.Point(16, 107);
            this.button_Add_Reference.Margin = new System.Windows.Forms.Padding(4);
            this.button_Add_Reference.Name = "button_Add_Reference";
            this.button_Add_Reference.Size = new System.Drawing.Size(161, 33);
            this.button_Add_Reference.TabIndex = 2;
            this.button_Add_Reference.Text = "Задать ссылку";
            this.button_Add_Reference.UseVisualStyleBackColor = true;
            this.button_Add_Reference.Click += new System.EventHandler(this.button_Add_Reference_Click);
            // 
            // dataGridViewTable
            // 
            this.dataGridViewTable.AllowUserToAddRows = false;
            this.dataGridViewTable.AllowUserToDeleteRows = false;
            this.dataGridViewTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnSelect});
            this.dataGridViewTable.Location = new System.Drawing.Point(8, 25);
            this.dataGridViewTable.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewTable.Name = "dataGridViewTable";
            this.dataGridViewTable.RowHeadersWidth = 51;
            this.dataGridViewTable.Size = new System.Drawing.Size(621, 232);
            this.dataGridViewTable.TabIndex = 3;
            // 
            // ColumnName
            // 
            this.ColumnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnName.FillWeight = 149.2386F;
            this.ColumnName.HeaderText = "Наименование таблицы";
            this.ColumnName.MinimumWidth = 6;
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            // 
            // ColumnSelect
            // 
            this.ColumnSelect.FillWeight = 50.76142F;
            this.ColumnSelect.HeaderText = "Выбор";
            this.ColumnSelect.MinimumWidth = 6;
            this.ColumnSelect.Name = "ColumnSelect";
            this.ColumnSelect.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColumnSelect.Width = 76;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dataGridViewTable);
            this.groupBox1.Location = new System.Drawing.Point(16, 148);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(637, 264);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Таблицы в текущем файле";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(13, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Наименование элемента";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(13, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "GUID";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelName.Location = new System.Drawing.Point(13, 35);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(221, 20);
            this.labelName.TabIndex = 5;
            this.labelName.Text = "Наименование элемента";
            // 
            // labelGUID
            // 
            this.labelGUID.AutoSize = true;
            this.labelGUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelGUID.Location = new System.Drawing.Point(13, 80);
            this.labelGUID.Name = "labelGUID";
            this.labelGUID.Size = new System.Drawing.Size(51, 20);
            this.labelGUID.TabIndex = 5;
            this.labelGUID.Text = "GUID";
            // 
            // Form_Add_Reference_To_Table
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 427);
            this.Controls.Add(this.labelGUID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_Add_Reference);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form_Add_Reference_To_Table";
            this.ShowIcon = false;
            this.Text = "Задать ссылку на таблицу";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTable)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_Add_Reference;
        private System.Windows.Forms.DataGridView dataGridViewTable;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelGUID;
    }
}