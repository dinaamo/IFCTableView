using GeometryGym.Ifc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using IFC_Table_View.HelperIFC;

using System.Collections;
using Microsoft.Win32;
using System.Reflection;
using System.Xml.Linq;
using System.ComponentModel;
using System.Windows;
using System.Text.RegularExpressions;
using IFC_Table_View.IFC.Model;
using System.Windows.Input;
using IFC_Table_View.Infracrucrure.Commands;

namespace IFC_Table_View.IFC.ModelItem
{
    public class ModelItemIFCTable : BaseModelReferenceIFC
    {


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="IFCTable"></param>
        /// <param name="modelIFC"></param>
        public ModelItemIFCTable(IfcTable IFCTable, ModelIFC modelIFC) : base(IFCTable, modelIFC)
        {
            this.IFCTable = ReplaceSymbols(IFCTable);
            dataTable = FillDataTable(this.IFCTable);
        }


        #region Свойства
        private IfcTable IFCTable { get; set; }

        public string IFCTableName => IFCTable.Name; 

        public DataTable dataTable { get; private set; }





        public override string NameReference => IFCTable.Name;


        #endregion


        #region Методы
        /// <summary>
        /// Заполнение DataTable
        /// </summary>
        /// <param name="ifcTable"></param>
        /// <returns></returns>
        public static DataTable FillDataTable(IfcTable ifcTable)
        {
            if (ifcTable == null && ifcTable.Rows.Count == 0)
            {
                return null;
            }

            DataTable dataTable = new DataTable();

            dataTable.TableName = ifcTable.Name;
            for (int i = 0; i < ifcTable.Rows[0].RowCells.Count(); i++)
            {
                string nameColumn = ifcTable.Rows[0].RowCells[i].Value.ToString();
                HelreptReplaceSymbols.ReplacingSymbols(ref nameColumn);

                dataTable.Columns.Add(nameColumn);
            }

            for (int i = 1; i < ifcTable.Rows.Count; i++)
            {
                DataRow row = dataTable.NewRow();

                for (int j = 0; j < ifcTable.Rows[i].RowCells.Count(); j++)
                {
                    row[dataTable.Columns[j].ColumnName] = ifcTable.Rows[i].RowCells[j].Value.ToString();
                }
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }

        public IfcTable GetIFCTable()
        {
            return IFCTable;
        }



        /// <summary>
        /// Замена запрещенных символов
        /// </summary>
        private IfcTable ReplaceSymbols(IfcTable ifcTable)
        {

            for (int row = 0; row < ifcTable.Rows.Count; row++)
            {
                IfcTableRow tt = ifcTable.Rows[row];

                for (int cell = 0; cell < ifcTable.Rows[row].RowCells.Count; cell++)
                {
                    string valueString = ifcTable.Rows[row].RowCells[cell].ValueString;

                    string newValueString = Regex.Replace(valueString, @"\s+", " ");

                    newValueString = newValueString.Trim((char)32);

                    newValueString = newValueString.Replace("\0", "");

                    newValueString = newValueString.Replace("измере-ния", "измерения");

                    newValueString = newValueString.Replace("изме- рения", "измерения");

                    newValueString = newValueString.Replace("оли-чество", "оличество");

                    newValueString = newValueString.Replace("ед_,", "ед");

                    newValueString = newValueString.Replace("Ед_", "Ед");

                    ifcTable.Rows[row].RowCells[cell] = new IfcText(newValueString);
                }
            }

            return ifcTable;
        }
        #endregion


        #region Комманды

        #region Удалить_таблицу
        protected override void OnDeleteReferenceCommandExecuted(object o)
        {
            MessageBoxResult result = MessageBox.Show("Удалить таблицу?", "Внимание!", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (result != MessageBoxResult.OK) { return; }

            base.OnDeleteReferenceCommandExecuted(o);

            modelIFC.DeleteIFCTable(IFCTable);
            
        }

        #endregion


        #endregion


    }
}
