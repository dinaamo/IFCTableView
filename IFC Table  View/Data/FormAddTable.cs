using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace IFC_Table_View.Data
{
    public partial class FormCreateTable : Form
    {
        private Action<DataTable> CereaTetable;

        public FormCreateTable(Action<DataTable> CereaTetable)
        {
            InitializeComponent();
            this.CereaTetable = CereaTetable;
        }

        private void PasteClipboard()
        {
            try
            {
                string s = Clipboard.GetText();
                char[] spl = { '\r' };
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
                                        //val = val.Replace("\"\"", "");

                                        oCell.Value = val.Trim('\n', '\r').Replace('\"', '\0');
                                        //  oCell.Style.BackColor = Color.Tomato;
                                    }
                                    else
                                        iFail++;
                                    //only traps a fail if the data has changed  \"
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
                MessageBox.Show("Данные имеют неверный формат", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        public DataTable dataTable { get; private set; }

        private void WriteTableToFile()
        {
            if (dgData.Rows.Count < 1)
            { return; }
            int cnt = textBoxNameTeble.Text.Count();
            if (cnt < 1)
            {
                MessageBox.Show("Задайте имя таблицы");
                return;
            }

            dataTable = new DataTable(textBoxNameTeble.Text);

            for (int c = 0; c < (dgData.Rows[0].Cells?.Count ?? 0); c++)
            {
                string headerName = dgData.Rows[0].Cells[c]?.Value?.ToString() ?? "";

                while (dataTable.Columns.Contains(headerName))
                {
                    headerName = headerName + "_" + c.ToString();
                }

                dataTable.Columns.Add(headerName);
            }

            for (int r = 0; r < dgData.Rows.Count; r++)
            {
                DataRow row = dataTable.NewRow();
                for (int c = 0; c < dgData.Columns.Count; c++)
                {
                    if (r == 0)
                    {
                        row[dataTable.Columns[c].ColumnName] = dataTable.Columns[c];
                    }
                    else
                    {
                        row[dataTable.Columns[c].ColumnName] = Convert.ToString(dgData.Rows[r].Cells[c].Value.ToString()) ?? "";
                    }
                }
                dataTable.Rows.Add(row);
            }

            CereaTetable(dataTable);

            ClearDG();
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            ClearDG();
        }

        private void ClearDG()
        {
            dgData.Columns.Clear();
            dgData.Rows.Clear();
        }
    }
}