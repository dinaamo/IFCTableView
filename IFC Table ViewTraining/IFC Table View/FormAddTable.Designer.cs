
namespace Edit_PropertyIFC4
{
    partial class FormAddTable
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
            this.dgData = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button_Clear = new System.Windows.Forms.Button();
            this.textBoxNameTeble = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).BeginInit();
            this.SuspendLayout();
            // 
            // dgData
            // 
            this.dgData.AllowUserToAddRows = false;
            this.dgData.AllowUserToResizeRows = false;
            this.dgData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgData.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgData.Location = new System.Drawing.Point(0, 41);
            this.dgData.Name = "dgData";
            this.dgData.ReadOnly = true;
            this.dgData.Size = new System.Drawing.Size(981, 619);
            this.dgData.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(216, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Вставить данные из буфера обмена";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button_Paste_Clipboard_Click);
            // 
            // button_Clear
            // 
            this.button_Clear.Location = new System.Drawing.Point(228, 12);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(65, 23);
            this.button_Clear.TabIndex = 1;
            this.button_Clear.Text = "Очистить";
            this.button_Clear.UseVisualStyleBackColor = true;
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // textBoxNameTeble
            // 
            this.textBoxNameTeble.Location = new System.Drawing.Point(462, 12);
            this.textBoxNameTeble.Name = "textBoxNameTeble";
            this.textBoxNameTeble.Size = new System.Drawing.Size(258, 20);
            this.textBoxNameTeble.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(381, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Имя таблицы";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(726, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(243, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "Записать таблицу в текущий IFC файл";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button_WriteFile_Click);
            // 
            // FormAddTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 660);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxNameTeble);
            this.Controls.Add(this.button_Clear);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dgData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormAddTable";
            this.ShowIcon = false;
            this.Text = "Добавление таблицы";
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgData;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button_Clear;
        private System.Windows.Forms.TextBox textBoxNameTeble;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
    }
}