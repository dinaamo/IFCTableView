using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;


namespace IFC_Table_View.HelperExcel
{
    class ExcelHelper : IDisposable
    {

        Excel.Application appExcel;
        Excel.Workbook workbook;
        Excel.Worksheet worksheet;

        public void WriteData(System.Data.DataTable dataTable)
        {
            appExcel = new Excel.Application();
            appExcel.Visible = false;

            workbook = appExcel.Workbooks.Add();

            FillTable(dataTable);

            //Содержимое по контенту
            Excel.Range range = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[dataTable.Rows.Count + 1, dataTable.Columns.Count+1]];
            range.EntireColumn.AutoFit();
            range.EntireRow.AutoFit();

            appExcel.Visible = true;

        }

        void FillTable(System.Data.DataTable dataTable)
        {
            worksheet = workbook.ActiveSheet;
            
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                string columnName = ReplacingCharacters(dataTable.Columns[i].ColumnName);
                worksheet.Cells[1, i + 1] = columnName;
            }

            for (int row = 0; row < dataTable.Rows.Count; row++)
            {
                for (int col = 0; col < dataTable.Columns.Count; col++)
                {
                    string cellValue = ReplacingCharacters(dataTable.Rows[row][col].ToString());
                    worksheet.Cells[row + 2, col + 1] = cellValue;
                }
            }
        }
        public void Dispose()
        {
            appExcel = null;
            workbook = null;
            worksheet = null;
            GC.Collect();

            //workbook.Close();
            //appExcel.Quit();
        }

       string ReplacingCharacters(string targetString)
        {
            //string prohibitedСharacters = @"\:{}[]|;<>?'~";

            //for (int i = 0; i < prohibitedСharacters.Length; i++)
            
            //if (targetString.Contains(prohibitedСharacters[i]))
                
            targetString = targetString.Replace("\0", "");

            return targetString;
        }
    }
}

