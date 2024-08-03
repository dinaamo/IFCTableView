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
            PropertyOject();
        }

        public IfcProject Project => database.Project;

        public object ItemTreeView
        {
            get
            {
                return database.SourceFilePath;
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

        private void PropertyOject()
        {
            _PropertyElement = new Dictionary<string, HashSet<string>>
            {
                { "Путь к файлу", new HashSet<string>() { database.SourceFilePath } },
                { "Версия", new HashSet<string>() { Convert.ToString(database.Release) } },
                { "Формат", new HashSet<string>() { Convert.ToString(database.Format) } },
                { "Модельный вид", new HashSet<string>() { Convert.ToString(database.ModelView) } },
                { "Приложение", new HashSet<string>() { Convert.ToString(Project.OwnerHistory?.OwningApplication?.ApplicationFullName) } },
                { "Автор проекта", new HashSet<string>() { Convert.ToString(Project.OwnerHistory?.OwningUser?.ThePerson?.Name) }},
                { "Организация", new HashSet<string>() { Convert.ToString(Project.OwnerHistory?.OwningUser?.TheOrganization?.Name) }},
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