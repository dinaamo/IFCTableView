using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edit_PropertyIFC4
{
    public partial class FormAddTable : Form
    {
        public FormAddTable()
        {
            InitializeComponent();
        }

        private void PasteClipboard()
        {
            try
            {
                string s = Clipboard.GetText();
                char[] spl = { '\r'  };
                string[] lines = s.Split(spl);
                string[] columns = lines[0].Split('\t');

                for (int i = 0; i < columns.Count(); i++)
                {
                    dgData.Columns.Add(i.ToString(), i.ToString());
                }

                int iFail = 0;
                int iRow = 0;
                int iCol = 0;

                DataGridViewCell oCell;
                if (dgData.Rows.Count < lines.Length)
                    dgData.Rows.Add(lines.Length - 1);
                foreach (string line in lines)
                {
                    if (iRow < dgData.RowCount && line.Length > 0)
                    {
                        string[] sCells = line.Split('\t');
                        for (int i = 0; i < sCells.GetLength(0); ++i)
                        {
                            if (iCol + i < this.dgData.ColumnCount)
                            {
                                oCell = dgData[iCol + i, iRow];
                                if (!oCell.ReadOnly)
                                {
                                    if (oCell.Value == null || oCell.Value.ToString() != sCells[i])
                                    {
                                        string val = (string)Convert.ChangeType(sCells[i],
                                                              oCell.ValueType);
                                        oCell.Value = val.Trim('\n', '\r');
                                        //  oCell.Style.BackColor = Color.Tomato;
                                    }
                                    else
                                        iFail++;
                                    //only traps a fail if the data has changed 
                                    //and you are pasting into a read only cell
                                }
                            }
                            else
                            { break; }
                        }
                        iRow++;
                    }
                    else
                    { break; }
                    if (iFail > 0)
                        MessageBox.Show(string.Format("{0} updates failed due" +
                                        " to read only column setting", iFail));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Данные имеют неверный формат.");
                ClearDG();
                return;
            }
        }

        private void button_Paste_Clipboard_Click(object sender, EventArgs e)
        {
            dgData.ReadOnly = false;
            PasteClipboard();
            dgData.ReadOnly = true;

            foreach (DataGridViewColumn item in dgData.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void button_WriteFile_Click(object sender, EventArgs e)
        {
            WriteTableToFile();
            
        }

        void WriteTableToFile()
        {
            int cnt = textBoxNameTeble.Text.Count();
            if (cnt < 1)
            {
                MessageBox.Show("Задайте имя таблицы");
                return;
            }

            DataTable dt = new DataTable(textBoxNameTeble.Text);
            
            for (int c = 0; c < dgData.Rows[0].Cells.Count; c++)
            {
                string headerName = dgData.Rows[0].Cells[c]?.Value?.ToString() ?? "";


                while (dt.Columns.Contains(headerName))
                {
                    headerName = headerName + "_" + c.ToString();
                }

                dt.Columns.Add(headerName);

            }

            for (int r = 0; r < dgData.Rows.Count; r++)
            {
                DataRow row = dt.NewRow();
                for (int c = 0; c < dgData.Columns.Count; c++)
                {
                    if (r == 0)
                    {
                        row[dt.Columns[c].ColumnName] = dt.Columns[c].ColumnName;
                    }
                    else
                    {
                        row[dt.Columns[c].ColumnName] = Convert.ToString(dgData.Rows[r].Cells[c].Value.ToString()) ?? "";
                    }
                }
                dt.Rows.Add(row);
            }

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    foreach (object cell in dt.Rows[i].ItemArray)
            //    {
            //        string text = cell.ToString();
            //    }
            //}

                HelperIFC.AddIFCTable(dt);

                ClearDG();
            }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            ClearDG();
        }

        void ClearDG()
        {
            dgData.Columns.Clear();
            dgData.Rows.Clear();
        }
    }
}
