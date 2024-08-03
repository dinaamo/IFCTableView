using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using GeometryGym.Ifc;
using GeometryGym.STEP;
using System.Collections.ObjectModel;
using System.Data;

namespace Edit_PropertyIFC4
{
    public class HelperIFC
    {
        public static bool Load()
        {
            Globals.ModelItems = null;
            Globals.FilePath = FileIFC.OpenIFC_File();
            if (Globals.FilePath == null)
            {
                return false;
            }
            LoadDataBase();

            if (Globals.DataBase == null)
            {
                return false;
            }

            if(Globals.DataBase.Project == null)
            {
                return false;
            }

            return FillCollectionModelItem();

        }

        private static void LoadDataBase()
        {
            try
             {
                Globals.DataBase = new DatabaseIfc(Globals.FilePath);

                if (Globals.DataBase.Project == null)
                {
                    throw new Exception("В файле не найдет класс IFCProject");
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
                Globals.DataBase = null;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка чтения файла\n" + $"{ex.Message}");
                Globals.DataBase = null;
            }

        }

        private static bool FillCollectionModelItem()
        {
            Globals.ModelItems = new ObservableCollection<ModelItem>();

            IfcProject project = Globals.DataBase.FirstOrDefault(it => it.GetType() == typeof(IfcProject)) as IfcProject;

            if(project != null)
            {
                CreationHierarchyIFCObjects(project, Globals.ModelItems);
            }

            IEnumerable<IfcTable> tableSet = Globals.DataBase.Where(it => it.GetType() == typeof(IfcTable)).Cast<IfcTable>();

            if (tableSet != null)
            {
                AddIFCTables(tableSet);
            }

            if (tableSet == null && project == null)
            {
                return false;
            }

            return true;
        }

        static void AddIFCTables(IEnumerable<BaseClassIfc> tableSet)
        {
            foreach (IfcTable table in tableSet)
            {
                Globals.ModelItems.Add(new ModelItem(table));
            }
        }
        static void CreationHierarchyIFCObjects(IfcObjectDefinition objDef, ObservableCollection<ModelItem> collection)
        {
            ModelItem NestItem = new ModelItem(objDef);

            collection.Add(NestItem);

            IfcSpatialStructureElement spatialElement = objDef as IfcSpatialStructureElement;

            if (spatialElement != null)
            {
                foreach (IfcObjectDefinition obj in spatialElement.ContainsElements.SelectMany(it => it.RelatedElements))
                {
                    CreationHierarchyIFCObjects(obj, NestItem.ModelItems);
                }
            }

            foreach (IfcObjectDefinition obj in objDef.IsDecomposedBy.SelectMany(r => r.RelatedObjects))
            {
                CreationHierarchyIFCObjects(obj, NestItem.ModelItems);
            }
        }

        public static void AddIFCTable(DataTable dataTable)
        {
            IfcTable ifcTable = new IfcTable(Globals.DataBase);
            ifcTable.Name = dataTable.TableName;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                List<IfcValue> rowSet = new List<IfcValue>();
                foreach (object cell in dataTable.Rows[i].ItemArray)
                {
                    rowSet.Add(new IfcText(cell.ToString()));
                }

                if (i==0)
                {
                    ifcTable.Rows.Add(new IfcTableRow(Globals.DataBase, rowSet, true));
                }
                else
                {
                    ifcTable.Rows.Add(new IfcTableRow(Globals.DataBase, rowSet, false));
                }
            }
            AddIFCTables(new List<IfcTable>() { ifcTable });
        }

        public static void DeleteTable(ModelItem modelItem)
        {
            IfcTable table = modelItem.GetIFCTable();
            if (table != null)
            {
                foreach (IfcTableRow row in table.Rows)
                {
                    Globals.DataBase.DeleteElement(row);
                }


                Globals.DataBase.DeleteElement(table);

                Globals.ModelItems.Remove(modelItem);
            }
        }

        public static void SaveIFCFile(string filePath)
        {
            if (Globals.DataBase == null)
            {
                return;
            }
            
             Globals.DataBase.WriteFile(filePath);
            
        }


    }
}
