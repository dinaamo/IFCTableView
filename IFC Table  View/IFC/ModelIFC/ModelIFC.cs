
using GeometryGym.Ifc;
using IFC_Table_View.HelperIFC;
using IFC_Table_View.IFC.ModelItem;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace IFC_Table_View.IFC.ModelIFC
{
    public class ModelIFC
    { 
        private DatabaseIfc DataBase = null;

        public ObservableCollection<IModelItemIFC> ModelItems { get; private set; }

        public string FilePath
        {
            get { return DataBase?.SourceFilePath; }
        }

        public ModelIFC SaveFile(string filePath)
        {
            if (filePath == null)
            {
                return null;
            }
            
            if (DataBase.WriteFile(filePath))
            {  
                return Load(filePath);
            }
            else
            {
                return null;
            }

        }
        public ModelIFC SaveXMLFile(string filePath)
        {
            if (DataBase.WriteXmlFile(filePath))
            {
                return Load(filePath);
            }
            else
            {
                return null;
            }

        }

        //Загружаем базу данных
        public ModelIFC Load(string filePath=null)
        {
            
            if (filePath == null)
            {
                return null;
            }
            LoadDataBase(filePath);

            if (DataBase == null || DataBase.Project == null)
            {
                return null;
            }

            FillCollectionModelItem();

            return this;

        }

        /// <summary>
        /// Открываем файл
        /// </summary>
        /// <param name="filePath"></param>
        private void LoadDataBase(string filePath)
        {
            try
            {
                DataBase = new DatabaseIfc(filePath);

                if (DataBase.Project == null)
                {
                    throw new Exception("В файле нет объекта IFCProject");
                }
            }
            catch (AggregateException aex)
            {
                string msg = "";
                foreach (var innerException in aex.InnerExceptions)
                {
                    msg += $"{innerException.InnerException.Message}\n";
                }
                System.Windows.Forms.MessageBox.Show($"Ошибка чтения файла\n Произошла одна из нескольких ошибок:\n {msg}");
                
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка чтения файла\n" + $"{ex.Message}");
                
            }
            return;

        }
        ModelItemIFCFile FileItem;


        ObservableCollection<ModelItemIFCTable> tempTableItemSet;

        /// <summary>
        /// Заполняем коллекцию элементов дерева модели
        /// </summary>
        private void FillCollectionModelItem()
        {
            ModelItems = new ObservableCollection<IModelItemIFC>();


            //Добавляем в дерево первым элементом файл 
            FileItem = new ModelItemIFCFile(DataBase);
            ModelItems.Add(FileItem);

            //Ищем все таблицы в файле и заполняем временную коллекцию элементов дерева с таблицами
            IEnumerable<IfcTable> tableSet = DataBase.Where(it => it.GetType() == typeof(IfcTable)).Cast<IfcTable>();
            tempTableItemSet = AddIFCTables(tableSet);

            //Составляем дерево объектов модели
            CreationHierarchyIFCObjects(FileItem.Project, FileItem.ModelItems, null);

            //После того как составили дерево объектов к нему добавляем таблицы 
            foreach (IModelItemIFC tableItem in tempTableItemSet)
            {
                FileItem.ModelItems.Add(tableItem);
            }
 
        }

        /// <summary>
        /// Составляем дерево элементов модели
        /// </summary>
        /// <param name="objDef"></param>
        /// <param name="collection"></param>
        /// <param name="topElement"></param>
        private void CreationHierarchyIFCObjects(IfcObjectDefinition objDef, ObservableCollection<IModelItemIFC> collection, ModelItemIFCObject topElement)
        {
            ModelItemIFCObject nestItem = new ModelItemIFCObject(objDef, topElement);

            //Проверяем, что в элементе есть ссылки на таблицы. Если есть то добавляем к таблицам ссылку на элемент
            nestItem.AddToTheTableReferenceElement(tempTableItemSet);

            collection.Add(nestItem);

            IfcSpatialStructureElement spatialElement = objDef as IfcSpatialStructureElement;

            if (spatialElement != null)
            {
                foreach (IfcObjectDefinition obj in spatialElement.ContainsElements.SelectMany(it => it.RelatedElements))
                {
                    CreationHierarchyIFCObjects(obj, nestItem.ModelItems, nestItem);
                }
            }

            foreach (IfcObjectDefinition obj in objDef.IsDecomposedBy.SelectMany(r => r.RelatedObjects))
            {
                CreationHierarchyIFCObjects(obj, nestItem.ModelItems, nestItem);
            }


        }

        /// <summary>
        /// Добавляем к дереву элементов таблицы
        /// </summary>
        /// <param name="tableSet"></param>
        /// <returns></returns>
        private ObservableCollection<ModelItemIFCTable> AddIFCTables(IEnumerable<IfcTable> tableSet)
        {
            ObservableCollection<ModelItemIFCTable> tempTableItemSet = new ObservableCollection<ModelItemIFCTable>();
            if (tableSet != null)
            {
                foreach (IfcTable table in tableSet)
                {
                    tempTableItemSet.Add(new ModelItemIFCTable(table));
                }

                return tempTableItemSet;
            }
            return null;

        }

        /// <summary>
        /// Удаляем таблицу
        /// </summary>
        /// <param name="tableToDelete"></param>
        public void DeleteTable(IfcTable tableToDelete)
        {
            
            foreach (IfcTableRow row in tableToDelete.Rows)
            {
                DataBase.DeleteElement(row);
            }


            DataBase.DeleteElement(tableToDelete);

            bool res = ModelItems[0].ModelItems.Remove(ModelItems[0].ModelItems.OfType<ModelItemIFCTable>().FirstOrDefault(it =>
            {
                if (it.IFCTable != null)
                {
                    return it.IFCTable.Equals(tableToDelete);
                }
                return false;
            }));

            IEnumerable<IfcPropertySet> PropertySetCollection = DataBase.OfType<IfcObject>().
                SelectMany(it => it.IsDefinedBy.
                SelectMany(obj => obj.RelatingPropertyDefinition).
                OfType<IfcPropertySet>()).
                Where(it => it.HasProperties.Select(pr => pr.Value.GetType() == typeof(IfcPropertyReferenceValue) ) != null).
                Where(it => it.HasProperties.Select(pr => ((IfcPropertyReferenceValue)pr.Value).PropertyReference == tableToDelete) != null);

            foreach (IfcPropertySet PropertySet in PropertySetCollection)
            {
                IfcProperty deletedroperty = PropertySet.HasProperties.
                    Where(it => it.Value.GetType() == typeof(IfcPropertyReferenceValue)).
                    FirstOrDefault(pr => ((IfcPropertyReferenceValue)pr.Value).PropertyReference == tableToDelete).Value;
                PropertySet.RemoveProperty(deletedroperty);
            }

            
        }





        /// <summary>
        /// Добавляем в модель таблицу
        /// </summary>
        /// <param name="dataTable"></param>
        public void CreateNewIFCTable(DataTable dataTable)
        {
            IfcTable ifcTable = new IfcTable(DataBase);
            ifcTable.Name = dataTable.TableName;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                List<IfcValue> rowSet = new List<IfcValue>();
                foreach (object cell in dataTable.Rows[i].ItemArray)
                {
                    rowSet.Add(new IfcText(cell.ToString()));
                }

                if (i == 0)
                {
                    ifcTable.Rows.Add(new IfcTableRow(DataBase, rowSet, true));
                }
                else
                {
                    ifcTable.Rows.Add(new IfcTableRow(DataBase, rowSet, false));
                }
            }


            AddIFCTables(new List<IfcTable>() { ifcTable });
        }


    }
}
