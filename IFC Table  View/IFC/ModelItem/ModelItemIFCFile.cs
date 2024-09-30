using GeometryGym.Ifc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IFC_Table_View.IFC.ModelItem
{
    public class ModelItemIFCFile : IModelItemIFC
    {
        private DatabaseIfc database;

        public ModelItemIFCFile(DatabaseIfc database)
        {
            this.database = database;
            PropertyObject();
        }

        public IfcProject Project => database.Project;

        public object ItemTreeView
        {
            get
            {
                return database.SourceFilePath;
            }
        }

        public Dictionary<string, HashSet<object>> PropertyElement
        {
            get
            {
                return _PropertyElement;
            }
        }

        private Dictionary<string, HashSet<object>> _PropertyElement;

        private void PropertyObject()
        {
            _PropertyElement = new Dictionary<string, HashSet<object>>
            {
                { "Путь к файлу", new HashSet<object>() { database.SourceFilePath } },
                { "Версия", new HashSet<object>() { Convert.ToString(database.Release) } },
                { "Формат", new HashSet<object>() { Convert.ToString(database.Format) } },
                { "Модельный вид", new HashSet<object>() { Convert.ToString(database.ModelView) } },
                { "Приложение", new HashSet<object>() { Convert.ToString(Project.OwnerHistory?.OwningApplication?.ApplicationFullName) } },
                { "Автор проекта", new HashSet<object>() { Convert.ToString(Project.OwnerHistory?.OwningUser?.ThePerson?.Name) }},
                { "Организация", new HashSet<object>() { Convert.ToString(Project.OwnerHistory?.OwningUser?.TheOrganization?.Name) }},
            };
        }

        private ObservableCollection<IModelItemIFC> _ModelItems;

        public ObservableCollection<IModelItemIFC> ModelItems
        {
            get
            {
                if (_ModelItems == null)
                {
                    _ModelItems = new ObservableCollection<IModelItemIFC>();
                }
                return _ModelItems;
            }
        }
    }
}