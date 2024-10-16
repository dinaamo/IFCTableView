﻿using GeometryGym.Ifc;
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

namespace IFC_Table_View.IFC.ModelItem
{
    public class ModelItemIFCTable : IModelItemIFC, INotifyPropertyChanged
    {
        public IfcTable IFCTable { get; private set; }


        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Событие изменения элемента
        /// </summary>
        /// <param name="PropertyName"></param>
        protected virtual void OnPropertyChanged(string PropertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        public ModelItemIFCTable(IfcTable IFCTable)
        {
            this.IFCTable = IFCTable;
            dataTable = FillDataTable(this.IFCTable);
            //test();
        }


        private bool _IsFocusReference { get; set; } = false;
        /// <summary>
        /// Фокус элемента
        /// </summary>
        public bool IsFocusReference
        {
            get { return _IsFocusReference; }
            set
            {
                _IsFocusReference = value;
                OnPropertyChanged("IsFocusReference");
            }
        }

        /// <summary>
        /// Коллекция ссылок на объекты
        /// </summary>
        public Dictionary<string, HashSet<object>> PropertyElement
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


        void PropertyObject() 
        {
            _PropertyElement = new Dictionary<string, HashSet<object>>();

        }

        public DataTable dataTable { get; private set; }

        public ObservableCollection<IModelItemIFC> ModelItems => null;

        //public bool t => throw new NotImplementedException();


        /// <summary>
        /// Заполнение DataTable
        /// </summary>
        /// <param name="IFCTable"></param>
        /// <returns></returns>
        public static DataTable FillDataTable(IfcTable IFCTable)
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
