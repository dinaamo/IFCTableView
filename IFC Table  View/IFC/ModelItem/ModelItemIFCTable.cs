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

namespace IFC_Table_View.IFC.ModelItem
{
    public class ModelItemIFCTable : BaseModelItemIFC
    {
        public IfcTable IFCTable { get; private set; }



        public ModelItemIFCTable(IfcTable IFCTable)
        {
            this.IFCTable = ReplaceSymbols(IFCTable);
            dataTable = FillDataTable(this.IFCTable);
        }



        /// <summary>
        /// Коллекция ссылок на объекты
        /// </summary>
        public override Dictionary<string, HashSet<object>> PropertyElement
        {
            get
            {
                return _PropertyElement;
            }
        }
        private Dictionary<string, HashSet<object>> _PropertyElement;

        HashSet<object> referenceObjectCollection;

        public void AddReferenceToTheElement(ModelItemIFCObject referenceObject)
        {
            if (_PropertyElement == null)
            {
                _PropertyElement = new Dictionary<string, HashSet<object>>();
                referenceObjectCollection = new HashSet<object>();
                _PropertyElement.Add("Ссылки на объекты", referenceObjectCollection);
            }
            
            referenceObjectCollection.Add(referenceObject);
        }

        public void DeleteReferenceToTheElement(ModelItemIFCObject deleteReferenceObject)
        {
            _PropertyElement["Ссылки на объекты"].Remove(deleteReferenceObject);
        }

        public override object ItemTreeView
        {
            get
            {
                if (dataTable != null)
                {
                    return IFCTable;
                }
                else
                {
                    return null;
                }
            }
        }


        void PropertyObject() 
        {
            _PropertyElement = new Dictionary<string, HashSet<object>>();

        }

        public DataTable dataTable { get; private set; }

        public override ObservableCollection<BaseModelItemIFC> ModelItems => null;

        //public bool t => throw new NotImplementedException();


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

                    newValueString = newValueString.Replace("измере-ния", "измерения");

                    newValueString = newValueString.Replace("оли-чество", "оличество");

                    newValueString = newValueString.Replace("ед_,", "ед");

                    newValueString = newValueString.Replace("Ед_", "Ед");

                    ifcTable.Rows[row].RowCells[cell] = new IfcText(newValueString);
                }
            }

            return ifcTable;
        }
    }
}
