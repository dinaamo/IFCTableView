using GeometryGym.Ifc;
using IFC_Table_View.Infracrucrure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Media;

namespace IFC_Table_View.IFC.ModelItem
{
    public class ModelItemIFCObject : IModelItemIFC, INotifyPropertyChanged
    {
        private IfcObjectDefinition _IFCObjectdefinition;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string PropertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        public event EventHandler<PropertyReferenceChangedEventArg> PropertyReferenceChanged;

        public void ChangePropertyReference(object obj, PropertyReferenceChangedEventArg e)
        {
            bool containRefObject = ModelItems.
                                        OfType<ModelItemIFCObject>().
                                        Select(it => it.IsContainPropertyReference).
                                        Any(it => it == true);

            if (e.IsContainPropertyReference == true || containRefObject)
            {
                IsContainPropertyReference = true;
                BrushImageForeground = System.Windows.Media.Brushes.Green;
                PropertyReferenceChanged?.Invoke(this, new PropertyReferenceChangedEventArg(true));
            }
            else
            {
                IsContainPropertyReference = false;
                BrushImageForeground = System.Windows.Media.Brushes.DarkRed;
                PropertyReferenceChanged?.Invoke(this, new PropertyReferenceChangedEventArg(false));
            }
        }

        public bool IsContainPropertyReference { get; set; }

        private Brush _BrushImageForeground = System.Windows.Media.Brushes.DarkRed;

        public Brush BrushImageForeground
        {
            get { return _BrushImageForeground; }
            set
            {
                _BrushImageForeground = value;
                OnPropertyChanged("BrushImageForeground");
            }
        }

        public ModelItemIFCObject(IfcObjectDefinition IFCObject, ModelItemIFCObject TopElement)
        {
            if (TopElement != null)
            {
                PropertyReferenceChanged += TopElement.ChangePropertyReference;
            }

            _IFCObjectdefinition = IFCObject;

            CollectionPropertySet = new ObservableCollection<IfcPropertySetDefinition>();

            FillCollectionPropertySet();
            PropertyesOject();

            int? countPropRef = CollectionPropertySet?.OfType<IfcPropertySet>().
                SelectMany(it => it.HasProperties.Values).
                OfType<IfcPropertyReferenceValue>().Count();

            if (countPropRef > 0)
            {
                ChangePropertyReference(this, new PropertyReferenceChangedEventArg(true));
            }
            else
            {
                ChangePropertyReference(this, new PropertyReferenceChangedEventArg(false));
            }
        }

        private void FillCollectionPropertySet()
        {
            if (_IFCObjectdefinition is IfcObject obj)
            {
                CollectionPropertySet.Clear();
                foreach (IfcPropertySetDefinition propSetIsObj in obj.IsDefinedBy.SelectMany(it => it.RelatingPropertyDefinition).OfType<IfcPropertySetDefinition>())
                {
                    CollectionPropertySet.Add(propSetIsObj);
                }

                IEnumerable<IfcPropertySetDefinition> collectionTypeProperty = obj.IsTypedBy?.RelatingType?.HasPropertySets?.Cast<IfcPropertySetDefinition>();

                if (collectionTypeProperty != null)
                {
                    foreach (IfcPropertySetDefinition propSetIsType in collectionTypeProperty)
                    {
                        CollectionPropertySet.Add(propSetIsType);
                    }
                }
            }
        }

        public void AddReferenseTable(List<ModelItemIFCTable> RefTableSet)
        {
            if (_IFCObjectdefinition is IfcObject obj)
            {
                IfcPropertySet PropSetTableReference = CollectionPropertySet.
                                                    OfType<IfcPropertySet>().
                                                    FirstOrDefault(it => it.Name == "RZDP_Ссылки");

                foreach (ModelItemIFCTable RefTable in RefTableSet)
                {
                    IfcPropertyReferenceValue pref = new IfcPropertyReferenceValue(_IFCObjectdefinition.Database, RefTable.IFCTable.Name);

                    pref.PropertyReference = RefTable.IFCTable;
                    pref.Name = RefTable.IFCTable.Name;

                    if (PropSetTableReference == null)
                    {
                        PropSetTableReference = new IfcPropertySet("RZDP_Ссылки", pref);
                        IfcRelDefinesByProperties reldefProp = new IfcRelDefinesByProperties(_IFCObjectdefinition, PropSetTableReference);
                        obj.IsDefinedBy.Add(reldefProp);
                    }
                    else
                    {
                        PropSetTableReference.AddProperty(pref);
                    }
                    ChangePropertyReference(this, new PropertyReferenceChangedEventArg(true));
                }

                FillCollectionPropertySet();
            }
        }

        public object ItemTreeView
        {
            get
            {
                if (_IFCObjectdefinition != null)
                {
                    return _IFCObjectdefinition;
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

        private void PropertyesOject()
        {
            _PropertyElement = new Dictionary<string, HashSet<string>>();

            //Материал
            if (_IFCObjectdefinition is IfcElement IFCElement)
            {
                IfcMaterialSelect ifcMaterialSelect = IFCElement.MaterialSelect();

                if (ifcMaterialSelect != null)
                {
                    if (ifcMaterialSelect is IfcMaterial materialSingle)
                    {
                        _PropertyElement.Add("Материал", new HashSet<string>() { materialSingle.Name });
                    }
                    else if (ifcMaterialSelect is IfcMaterialLayer materiaLayer)
                    {
                        _PropertyElement.Add("Материал", new HashSet<string>() { materiaLayer.Material.Name });
                    }
                    else if (ifcMaterialSelect is IfcMaterialLayerSet materiaLayerSet)
                    {
                        HashSet<string> materialSet = new HashSet<string>();
                        foreach (IfcMaterial material in materiaLayerSet.MaterialLayers.Select(it=>it.Material))
                        {
                            materialSet.Add(material.Name);
                        }
                        _PropertyElement.Add("Материалы", materialSet);
                    }
                    else if (ifcMaterialSelect is IfcMaterialProfile materiaProfile)
                    {
                        _PropertyElement.Add("Материал", new HashSet<string>() { materiaProfile.Material.Name });
                    }
                    else if (ifcMaterialSelect is IfcMaterialProfileSet materiaProfileSet)
                    {
                        HashSet<string> materialSet = new HashSet<string>();
                        foreach (IfcMaterial material in materiaProfileSet.MaterialProfiles.Select(it => it.Material))
                        {
                            materialSet.Add(material.Name);
                        }
                        _PropertyElement.Add("Материалы", materialSet);
                    }
                    else if (ifcMaterialSelect is IfcMaterialConstituent materiaConstituen)
                    {
                        _PropertyElement.Add("Материал", new HashSet<string>() { materiaConstituen.Material.Name });
                    }
                    else if (ifcMaterialSelect is IfcMaterialConstituentSet materiaConstituentSet)
                    {
                        HashSet<string> materialSet = new HashSet<string>();
                        foreach (IfcMaterial material in materiaConstituentSet.MaterialConstituents.Select(it => it.Value.Material))
                        {
                            materialSet.Add(material.Name);
                        }
                        _PropertyElement.Add("Материалы", materialSet);
                    }
                    else if (ifcMaterialSelect is IfcMaterialLayerSetUsage materiaLayerSetUsage)
                    {
                        HashSet<string> materialSet = new HashSet<string>();
                        foreach (IfcMaterial material in materiaLayerSetUsage.ForLayerSet.MaterialLayers.Select(it => it.Material))
                        {
                            materialSet.Add(material.Name);
                        }
                        _PropertyElement.Add("Материалы", materialSet);
                    }
                    else if (ifcMaterialSelect is IfcMaterialProfileSetUsage materiaProfileSetUsag)
                    {
                        HashSet<string> materialSet = new HashSet<string>();
                        foreach (IfcMaterial material in materiaProfileSetUsag.ForProfileSet.MaterialProfiles.Select(it => it.Material))
                        {
                            materialSet.Add(material.Name);
                        }
                        _PropertyElement.Add("Материалы", materialSet);
                    }
                    else if(ifcMaterialSelect is IfcMaterialList materialList)
                    {
                        HashSet<string> materialListValue = new HashSet<string>();

                        foreach (IfcMaterial material in materialList.Materials)
                        {
                            materialListValue.Add(material.Name);
                        }
                        _PropertyElement.Add("Материалы", materialListValue);
                    }
                }               
            }

            if (_IFCObjectdefinition.Description != string.Empty)
            {
                _PropertyElement.Add("Описание", new HashSet<string>() { _IFCObjectdefinition.Description });
            }

            //Вложенные объекты
            HashSet<string> listobjectIsNestedBy = new HashSet<string>();
            foreach (IfcRelNests relNest in _IFCObjectdefinition.IsNestedBy)
            {
                foreach (IfcObjectDefinition obj in relNest.RelatedObjects)
                {
                    listobjectIsNestedBy.Add(
                        $"Наименование связи: {relNest.Name}\n" +
                        $"Описание связи: {relNest.Description}\n" +
                        $"Наименование элемента: {obj.Name}\n" +
                        $"Класс IFC: {obj.GetType().Name}\n" +
                        $"GUID: {obj.Guid}");
                }
            }
            if (listobjectIsNestedBy.Count > 0)
            {
                _PropertyElement.Add("Вложенные объекты (IsNestedBy)", listobjectIsNestedBy);
            }

            //Разлагается на объекты
            HashSet<string> listobjectIsDecomposedBy = new HashSet<string>();
            foreach (IfcRelAggregates relDecomp in _IFCObjectdefinition.IsDecomposedBy)
            {
                foreach (IfcObjectDefinition obj in relDecomp.RelatedObjects)
                {
                    listobjectIsDecomposedBy.Add(
                        $"Наименование связи: {relDecomp.Name}\n" +
                        $"Описание связи: {relDecomp.Description}\n" +
                        $"Наименование элемента: {obj.Name}\n" +
                        $"Класс IFC: {obj.GetType().Name}\n" +
                        $"GUID: {obj.Guid}");
                }
            }
            if (listobjectIsDecomposedBy.Count > 0)
            {
                _PropertyElement.Add("Раскладывается на объекты (IsDecomposedBy)", listobjectIsDecomposedBy);
            }

            if (_IFCObjectdefinition is IfcObject IFCObject)
            {
                

                //Тип объекта
                if (IFCObject.ObjectType != string.Empty)
                {
                    _PropertyElement.Add("Тип", new HashSet<string>() { IFCObject.ObjectType });
                }

                //Связанные объекты
                HashSet<string> listobjectIsDefinedBy = new HashSet<string>();
                foreach (IfcRelDefinesByProperties relDef in IFCObject.IsDefinedBy)
                {
                    foreach (IfcObjectDefinition obj in relDef.RelatedObjects)
                    {
                        listobjectIsDefinedBy.Add(
                            $"Наименование связи: {relDef.Name}\n" +
                            $"Описание связи: {relDef.Description}\n" +
                            $"Наименование элемента: {obj.Name}\n" +
                            $"Класс IFC: {obj.GetType().Name}\n" +
                            $"GUID: {obj.Guid}");
                    }
                }

                if (listobjectIsDefinedBy.Count > 0)
                {
                    _PropertyElement.Add("Связанные объекты (IsDefinedBy)", listobjectIsDefinedBy);
                }
            }

            //Содержит объекты
            HashSet<string> listobjectContainElement = new HashSet<string>();

            if (_IFCObjectdefinition is IfcSpatialStructureElement IFCStrElem)
            {
                foreach (IfcRelContainedInSpatialStructure spartialStucture in IFCStrElem.ContainsElements)
                {
                    foreach (IfcProduct obj in spartialStucture.RelatedElements)
                    {
                        listobjectContainElement.Add(
                            $"Наименование связи: {spartialStucture.Name}\n" +
                            $"Описание связи: {spartialStucture.Description}\n" +
                            $"Наименование элемента: {obj.Name}\n" +
                            $"Класс IFC: {obj.GetType().Name}\n" +
                            $"GUID: {obj.Guid} ");
                    }
                }
            }

            if (listobjectContainElement.Count > 0)
            {
                _PropertyElement.Add("Содержит объекты (ContainsElements)", listobjectContainElement);
            }
        }

        private ObservableCollection<IfcPropertySetDefinition> _CollectionPropertySet;

        public ObservableCollection<IfcPropertySetDefinition> CollectionPropertySet
        {
            get
            {
                return _CollectionPropertySet;
            }

            private set
            {
                OnPropertyChanged("CollectionPropertySet");
                _CollectionPropertySet = value;
            }
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