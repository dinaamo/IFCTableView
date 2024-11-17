using GeometryGym.Ifc;
using IFC_Table_View.IFC.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows;
using IFC_Table_View.Infracrucrure.Commands;

namespace IFC_Table_View.IFC.ModelItem
{
    public abstract class BaseModelReferenceIFC : BaseModelItemIFC
    {
        public BaseModelReferenceIFC(IfcObjectReferenceSelect ifcObjectReferenceSelect, ModelIFC modelIFC) : base(modelIFC, ifcObjectReferenceSelect)
        {
            DeleteReferenceCommand = new ActionCommand(
                        OnDeleteReferenceCommandExecuted,
                        CanDeleteReferenceCommandExecute);
            this.ifcObjectReferenceSelect = ifcObjectReferenceSelect;
        }

        IfcObjectReferenceSelect ifcObjectReferenceSelect;

        public ICommand DeleteReferenceCommand { get; }
        protected virtual void OnDeleteReferenceCommandExecuted(object o)
        {
            IEnumerable<ModelItemIFCObject> referenceToDelete = PropertyElement
                                        .SelectMany(it => it.Value)
                                        .Cast<ModelItemIFCObject>();

            foreach (ModelItemIFCObject modelObject in referenceToDelete.ToArray())
            {
                modelObject.DeleteReferenceToTheObjectReference(new List<BaseModelReferenceIFC>() { this });
                DeleteReferenceToTheElement(modelObject);
            }
        }


        protected bool CanDeleteReferenceCommandExecute(object o)
        {
            if (modelIFC == null)
            {
                return false;
            }
            else if (o is BaseModelReferenceIFC)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public abstract string NameReference { get; }

        public ObservableCollection<BaseModelItemIFC> ModelItems { get; }

        public HashSet<object> referenceObjectCollection { get; set; }
        public virtual IfcObjectReferenceSelect GetReferense()
        {
            return ifcObjectReferenceSelect;
        }

        /// <summary>
        /// Коллекция ссылок на объекты
        /// </summary>
        /// 
        private Dictionary<string, HashSet<object>> _PropertyElement;
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

        public void AddReferenceToTheElement(ModelItemIFCObject referenceObject)
        {
            if (PropertyElement == null)
            {
                PropertyElement = new Dictionary<string, HashSet<object>>();
                referenceObjectCollection = new HashSet<object>();
                PropertyElement.Add("Ссылки на объекты", referenceObjectCollection);
            }

            referenceObjectCollection.Add(referenceObject);
        }

        public void DeleteReferenceToTheElement(ModelItemIFCObject deleteReferenceObject)
        {
            PropertyElement["Ссылки на объекты"].Remove(deleteReferenceObject);
        }
    }
}
