using GeometryGym.Ifc;
using IFC_Table_View.IFC.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace IFC_Table_View.IFC.ModelItem
{
    public class ModelItemIFCFile : BaseModelItemIFC
    {
        private DatabaseIfc database;
        

        public ModelItemIFCFile(DatabaseIfc database, ModelIFC modelIFC) : base(modelIFC)
        {
            this.database = database;
            PropertyObject();
        }

        public IfcProject Project => database.Project;

        public override object ItemTreeView
        {
            get
            {
                return database.SourceFilePath;
            }
        }

        /// <summary>
        /// IsExpanded
        /// </summary>
        private bool _IsExpanded { get; set; } = false;
        public override bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
                _IsExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        public override Dictionary<string, HashSet<object>> PropertyElement
        {
            get
            {
                return _PropertyElement;
            }
            protected set
            {
                _PropertyElement = value;
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

        private ObservableCollection<BaseModelItemIFC> _ModelItems;

        public override ObservableCollection<BaseModelItemIFC> ModelItems
        {
            get
            {
                if (_ModelItems == null)
                {
                    _ModelItems = new ObservableCollection<BaseModelItemIFC>();
                }
                return _ModelItems;
            }
        }

    }
}