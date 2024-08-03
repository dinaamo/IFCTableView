using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GeometryGym.Ifc;

namespace Edit_PropertyIFC4
{
    public interface IModelItem
    {
        IEnumerable<IfcPropertySet> CollectionPropertySet { get; }
        string GUID { get; }
        string ItemTreeView { get; }
        ObservableCollection<ModelItem> ModelItems { get; set; }
        IfcTable Table { get; }

        IfcTable GetIFCTable();
        IfcObjectDefinition GetObject();
    }

    public class ModelItem : IModelItem
    {
        private IfcObjectDefinition ifcObjectDef;
        private IfcTable ifcTable;
        Type ifcType;

        public ModelItem(IfcObjectDefinition IfcObject)
        {
            ifcObjectDef = IfcObject;
            ifcType = typeof(IfcObjectDefinition);
            ModelItems = new ObservableCollection<ModelItem>();

            IfcObject obj = ifcObjectDef as IfcObject;

            CollectionPropertySet = obj?.IsDefinedBy.SelectMany(it => it.RelatingPropertyDefinition).OfType<IfcPropertySet>();


        }

        public IfcTable GetIFCTable()
        {
            return ifcTable;
        }

        public IfcObjectDefinition GetObject()
        {
            return ifcObjectDef;
        }

        public ModelItem(IfcTable Table)
        {
            ifcType = typeof(IfcTable);
            ifcTable = Table;
            //DataTable = new List<TableRow>();

            //FillDataTable();
        }
        public ObservableCollection<ModelItem> ModelItems { get; set; }

        public string ItemTreeView
        {
            get
            {
                if (ifcType == typeof(IfcObjectDefinition))
                {
                    return ifcObjectDef.Name + $" ({ifcObjectDef.GetType().Name})";
                }
                else if (ifcType == typeof(IfcTable))
                {
                    return ifcTable.Name + $" ({ifcTable.GetType().Name})";
                }
                else
                {
                    return "";
                }
            }
        }
        public string GUID
        {
            get
            {
                if (ifcType == typeof(IfcObjectDefinition))
                {
                    return ifcObjectDef.GlobalId;
                }
                else if (ifcType == typeof(IfcTable))
                {
                    return ifcTable.Name;
                }
                else
                {
                    return "";
                }

            }
        }

        public IEnumerable<IfcPropertySet> CollectionPropertySet
        {
            get;
            private set;
        }


        public IfcTable Table
        {
            get { return ifcTable; }
        }


    }
}
