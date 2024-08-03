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

namespace IFC_Table_View.IFC.ModelItem
{
    public class ModelItemIFCTable : IModelItemIFC
    {
        public IfcTable IFCTable { get; private set; }



        public ModelItemIFCTable(IfcTable IFCTable)
        {
            this.IFCTable = IFCTable;
            dataTable = FiilDataTable(this.IFCTable);
            //test();
        }
        
        void test()
        {
            //IfcReference refer = new IfcReference(IFCTable.Database);

            //refer.TypeIdentifier = $"";

            //IFCTable.Columns[0].ReferencePath = refer;
        }

        public object ItemTreeView
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
        public Dictionary<string, HashSet<string>> PropertyElement
        {
            get
            {
                return _PropertyElement;
            }
        }
        private Dictionary<string, HashSet<string>> _PropertyElement;

        void PropertyOject() 
        {
            _PropertyElement = new Dictionary<string, HashSet<string>>();

        }

        public DataTable dataTable { get; private set; }

        public ObservableCollection<IModelItemIFC> ModelItems => null;

        public bool t => throw new NotImplementedException();

        public static DataTable FiilDataTable(IfcTable IFCTable)
        {
            if (IFCTable == null && IFCTable.Rows.Count == 0)
            {
                return null;
            }

            DataTable dataTable = new DataTable();

            dataTable.TableName = IFCTable.Name;
            for (int i = 0; i < IFCTable.Rows[0].RowCells.Count(); i++)
            {
                string nameColumn = IFCTable.Rows[0].RowCells[i].Value.ToString();
                HelreptReplaceSymbols.ReplacingSymbols(ref nameColumn);

                dataTable.Columns.Add(nameColumn);
            }

            for (int i = 1; i < IFCTable.Rows.Count; i++)
            {
                DataRow row = dataTable.NewRow();

                for (int j = 0; j < IFCTable.Rows[i].RowCells.Count(); j++)
                {
                    row[dataTable.Columns[j].ColumnName] = IFCTable.Rows[i].RowCells[j].Value.ToString();
                }
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }

    }
}
