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
        private IfcObjectDefinition _IFCObjectDefinition;

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

        public event EventHandler<PropertyReferenceChangedEventArg> PropertyReferenceChanged;


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

        private bool _IsExpanded { get; set; } = false;
        /// <summary>
        /// IsExpanded
        /// </summary>
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
                _IsExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        private bool _IsSelected { get; set; } = false;
        /// <summary>
        /// IsSelected 
        /// </summary>
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (value)
                {
                    FontSize = 15;
                }
                else
                {
                    FontSize = 12;
                }

                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }


        private int _FontSize { get; set; } = 12;
        /// <summary>
        /// IsSelected 
        /// </summary>
        public int FontSize
        {
            get { return _FontSize; }
            set
            {
                _FontSize = value;
                OnPropertyChanged("FontSize");
            }
        }

        /// <summary>
        /// Прокидываем по дереву вверх состояние элемента
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        public void ChangePropertyReference(object obj, PropertyReferenceChangedEventArg e)
        {
            if (IsContainPropertyReference == true || e.IsContainPropertyReference == true)
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

        public bool _IsContainPropertyReference { get; set; }
        /// <summary>
        /// Наличие в элементе ссылки на таблицы
        /// </summary>
        public bool IsContainPropertyReference
        {
            get { return _IsContainPropertyReference; }
            set
            {
                ////Изменяем цвет маркера НЕ НАДО МЕНЯЕТСЯ ЧЕРЕЗ СОБЫТИЕ
                //if (value)
                //{
                //    _BrushImageForeground = System.Windows.Media.Brushes.Green;
                //}
                //else
                //{
                //    _BrushImageForeground = System.Windows.Media.Brushes.DarkRed;
                //}
                _IsContainPropertyReference = value;
            }
        }

        private Brush _BrushImageForeground = System.Windows.Media.Brushes.DarkRed;
        /// <summary>
        /// Кисть для маркера
        /// </summary>
        public Brush BrushImageForeground
        {
            get { return _BrushImageForeground; }
            set
            {
                _BrushImageForeground = value;
                OnPropertyChanged("BrushImageForeground");
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="IFCObject"></param>
        /// <param name="TopElement"></param>
        public ModelItemIFCObject(IfcObjectDefinition IFCObject, ModelItemIFCObject TopElement)
        {
            //Если есть элемент выше по дереву то подключаем к нему обработчик события изменения состояния элемента
            if (TopElement != null)
            {
                PropertyReferenceChanged += TopElement.ChangePropertyReference;
            }
           
            _IFCObjectDefinition = IFCObject;


            InitializationElementModelObject();
        }

        public void AddToTheTableReferenceElement(ObservableCollection<ModelItemIFCTable> tableItemSet)
        {
            IEnumerable<IfcPropertyReferenceValue> propertyReferenceSet = CollectionPropertySet?.OfType<IfcPropertySet>().
                SelectMany(it => it.HasProperties.Values).
                OfType<IfcPropertyReferenceValue>();

            foreach (IfcPropertyReferenceValue propertyReference in propertyReferenceSet)
            {
                foreach (ModelItemIFCTable tableItem in tableItemSet)
                {
                    if (propertyReference.PropertyReference == tableItem.ItemTreeView as IfcTable)
                    {
                        tableItem.AddReferenceToTheElement(this);
                    }
                }
            }
        }
        
        ModelObjectHelper modelHelper;

        /// <summary>
        /// Инициализация элементов объекта модели
        /// </summary>
        private void InitializationElementModelObject()
        {
            modelHelper = new ModelObjectHelper(_IFCObjectDefinition);

            CollectionPropertySet = new ObservableCollection<IfcPropertySetDefinition>();

            modelHelper.FillCollectionPropertySet(CollectionPropertySet);

            _PropertyElement = modelHelper.GetPropertyObject();

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

        /// <summary>
        /// Удаление ссылок на таблицы
        /// </summary>
        /// <param name="ifcPropertyReferenceValueDictionaryToDelete"></param>
        public void DeleteReferenceTable(Dictionary<string, IfcPropertyReferenceValue> ifcPropertyReferenceValueDictionaryToDelete, List<ModelItemIFCTable> collectionModelTable)
        {
            //Получаем необходимый набор характеристик
            IfcPropertySet PropSetTableReference = CollectionPropertySet.
                                                   OfType<IfcPropertySet>().
                                                   FirstOrDefault(it => it.Name == "RZDP_Ссылки");

            //Удаляем ссылки
            foreach (KeyValuePair<string, IfcPropertyReferenceValue> Pair in ifcPropertyReferenceValueDictionaryToDelete)
            {
                ModelItemIFCTable findModelTable = collectionModelTable.FirstOrDefault(it => it.ItemTreeView == Pair.Value.PropertyReference);
                findModelTable.DeleteReferenceToTheElement(this);
                PropSetTableReference.HasProperties.Remove(Pair.Key);
            }

            //Проверяем, остались ли еще ссылки на другие таблицы
            if (PropSetTableReference.HasProperties.Count == 0)
            {
                IfcObject ifcObject = ((IfcObject)_IFCObjectDefinition);
                //Находим промежуточный класс 
                IfcRelDefinesByProperties FindRelDef = ifcObject.IsDefinedBy
                    .FirstOrDefault(it => it.RelatingPropertyDefinition.Contains(PropSetTableReference));

                //Удаляем пустой набор свойств
                FindRelDef.RelatingPropertyDefinition.Remove(PropSetTableReference);

                //Удаляем пустой промежуточный класс
                if (FindRelDef.RelatingPropertyDefinition.Count == 0)
                {
                    ifcObject.IsDefinedBy.Remove(FindRelDef);
                }
                IsContainPropertyReference = false;
                //Прокидываем наверх по дереву событие удаления ссылок
                ChangePropertyReference(this, new PropertyReferenceChangedEventArg(false));
            }
            //Заполняем заново коллекцию характеристик
            modelHelper.FillCollectionPropertySet(CollectionPropertySet);
        }

        /// <summary>
        /// Добавление ссылок на таблицы
        /// </summary>
        /// <param name="RefTableSet"></param>
        public void AddReferenceTable(List<ModelItemIFCTable> RefTableSet)
        {
            if (_IFCObjectDefinition is IfcObject obj)
            {
                IfcPropertySet PropSetTableReference = CollectionPropertySet.
                                                    OfType<IfcPropertySet>().
                                                    FirstOrDefault(it => it.Name == "RZDP_Ссылки");

                foreach (ModelItemIFCTable RefTable in RefTableSet)
                {
                    IfcPropertyReferenceValue pref = new IfcPropertyReferenceValue(_IFCObjectDefinition.Database, RefTable.IFCTable.Name);

                    pref.PropertyReference = RefTable.IFCTable;
                    pref.Name = RefTable.IFCTable.Name;

                    if (PropSetTableReference == null)
                    {
                        PropSetTableReference = new IfcPropertySet("RZDP_Ссылки", pref);
                        IfcRelDefinesByProperties reldefProp = new IfcRelDefinesByProperties(_IFCObjectDefinition, PropSetTableReference);
                        obj.IsDefinedBy.Add(reldefProp);
                    }
                    else
                    {
                        PropSetTableReference.AddProperty(pref);
                    }
                    //Добавляем в таблицу ссылки
                    RefTable.AddReferenceToTheElement(this);

                    //Прокидываем наверх по дереву событие добавления ссылки
                    ChangePropertyReference(this, new PropertyReferenceChangedEventArg(true));
                }

                modelHelper.FillCollectionPropertySet(CollectionPropertySet);
            }
        }

        /// <summary>
        /// Получение элемента вложенного дерева
        /// </summary>
        public object ItemTreeView
        {
            get
            {
                if (_IFCObjectDefinition != null)
                {
                    return _IFCObjectDefinition;
                }
                else
                {
                    return null;
                }
            }
        }

        private Dictionary<string, HashSet<object>> _PropertyElement;
        /// <summary>
        /// Свойства элемента
        /// </summary>
        public Dictionary<string, HashSet<object>> PropertyElement
        {
            get
            {
                return _PropertyElement;
            }
        }

        private ObservableCollection<IfcPropertySetDefinition> _CollectionPropertySet;
        /// <summary>
        /// Наборы характеристик
        /// </summary>
        public ObservableCollection<IfcPropertySetDefinition> CollectionPropertySet
        {
            get
            {
                return _CollectionPropertySet;
            }

            set
            {
                OnPropertyChanged("CollectionPropertySet");
                _CollectionPropertySet = value;
            }
        }

        private ObservableCollection<IModelItemIFC> _ModelItems;
        /// <summary>
        /// Элемент дерева
        /// </summary>
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




    /// <summary>
    /// Вспомогательный класс для объекта дерева
    /// </summary>
    class ModelObjectHelper
    {
        IfcObjectDefinition IFCObjectDefinition;

        public ModelObjectHelper(IfcObjectDefinition IFCObjectDefinition)
        {
            this.IFCObjectDefinition = IFCObjectDefinition;
        }

        #region Заполнение свойств элемента
        public Dictionary<string, HashSet<object>> GetPropertyObject()
        {

            Dictionary<string, HashSet<object>> PropertyElement = new Dictionary<string, HashSet<object>>();
            if (this.IFCObjectDefinition == null)
            { 
                return PropertyElement; 
            }

            //Материал
            if (IFCObjectDefinition is IfcElement IFCElement)
            {
                IfcMaterialSelect ifcMaterialSelect = IFCElement.MaterialSelect();

                if (ifcMaterialSelect != null)
                {
                    if (ifcMaterialSelect is IfcMaterial materialSingle)
                    {
                        PropertyElement.Add("Материал", new HashSet<object>() { materialSingle.Name });
                    }
                    else if (ifcMaterialSelect is IfcMaterialLayer materiaLayer)
                    {
                        PropertyElement.Add("Материал", new HashSet<object>() { materiaLayer.Material.Name });
                    }
                    else if (ifcMaterialSelect is IfcMaterialLayerSet materiaLayerSet)
                    {
                        HashSet<object> materialSet = new HashSet<object>();
                        foreach (IfcMaterial material in materiaLayerSet.MaterialLayers.Select(it => it.Material))
                        {
                            materialSet.Add(material.Name);
                        }
                        PropertyElement.Add("Материалы", materialSet);
                    }
                    else if (ifcMaterialSelect is IfcMaterialProfile materiaProfile)
                    {
                        PropertyElement.Add("Материал", new HashSet<object>() { materiaProfile.Material.Name });
                    }
                    else if (ifcMaterialSelect is IfcMaterialProfileSet materiaProfileSet)
                    {
                        HashSet<object> materialSet = new HashSet<object>();
                        foreach (IfcMaterial material in materiaProfileSet.MaterialProfiles.Select(it => it.Material))
                        {
                            materialSet.Add(material.Name);
                        }
                        PropertyElement.Add("Материалы", materialSet);
                    }
                    else if (ifcMaterialSelect is IfcMaterialConstituent materiaConstituen)
                    {
                        PropertyElement.Add("Материал", new HashSet<object>() { materiaConstituen.Material.Name });
                    }
                    else if (ifcMaterialSelect is IfcMaterialConstituentSet materiaConstituentSet)
                    {
                        HashSet<object> materialSet = new HashSet<object>();
                        foreach (IfcMaterial material in materiaConstituentSet.MaterialConstituents.Select(it => it.Value.Material))
                        {
                            materialSet.Add(material.Name);
                        }
                        PropertyElement.Add("Материалы", materialSet);
                    }
                    else if (ifcMaterialSelect is IfcMaterialLayerSetUsage materiaLayerSetUsage)
                    {
                        HashSet<object> materialSet = new HashSet<object>();
                        foreach (IfcMaterial material in materiaLayerSetUsage.ForLayerSet.MaterialLayers.Select(it => it.Material))
                        {
                            materialSet.Add(material.Name);
                        }
                        PropertyElement.Add("Материалы", materialSet);
                    }
                    else if (ifcMaterialSelect is IfcMaterialProfileSetUsage materiaProfileSetUsag)
                    {
                        HashSet<object> materialSet = new HashSet<object>();
                        foreach (IfcMaterial material in materiaProfileSetUsag.ForProfileSet.MaterialProfiles.Select(it => it.Material))
                        {
                            materialSet.Add(material.Name);
                        }
                        PropertyElement.Add("Материалы", materialSet);
                    }
                    else if (ifcMaterialSelect is IfcMaterialList materialList)
                    {
                        HashSet<object> materialListValue = new HashSet<object>();

                        foreach (IfcMaterial material in materialList.Materials)
                        {
                            materialListValue.Add(material.Name);
                        }
                        PropertyElement.Add("Материалы", materialListValue);
                    }
                }
            }

            if (IFCObjectDefinition.Description != string.Empty)
            {
                PropertyElement.Add("Описание", new HashSet<object>() { IFCObjectDefinition.Description });
            }

            //Вложенные объекты
            HashSet<object> listobjectIsNestedBy = new HashSet<object>();
            foreach (IfcRelNests relNest in IFCObjectDefinition.IsNestedBy)
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
                PropertyElement.Add("Вложенные объекты (IsNestedBy)", listobjectIsNestedBy);
            }

            //Разлагается на объекты
            HashSet<object> listobjectIsDecomposedBy = new HashSet<object>();
            foreach (IfcRelAggregates relDecomp in IFCObjectDefinition.IsDecomposedBy)
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
                PropertyElement.Add("Раскладывается на объекты (IsDecomposedBy)", listobjectIsDecomposedBy);
            }

            if (IFCObjectDefinition is IfcObject IFCObject)
            {


                //Тип объекта
                if (IFCObject.ObjectType != string.Empty)
                {
                    PropertyElement.Add("Тип", new HashSet<object>() { IFCObject.ObjectType });
                }

                //Связанные объекты
                HashSet<object> listobjectIsDefinedBy = new HashSet<object>();
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
                    PropertyElement.Add("Связанные объекты (IsDefinedBy)", listobjectIsDefinedBy);
                }
            }

            //Содержит объекты
            HashSet<object> listObjectContainElement = new HashSet<object>();

            if (IFCObjectDefinition is IfcSpatialStructureElement IFCStrElem)
            {
                foreach (IfcRelContainedInSpatialStructure spartialStucture in IFCStrElem.ContainsElements)
                {
                    foreach (IfcProduct obj in spartialStucture.RelatedElements)
                    {
                        listObjectContainElement.Add(
                            $"Наименование связи: {spartialStucture.Name}\n" +
                            $"Описание связи: {spartialStucture.Description}\n" +
                            $"Наименование элемента: {obj.Name}\n" +
                            $"Класс IFC: {obj.GetType().Name}\n" +
                            $"GUID: {obj.Guid} ");
                    }
                }
            }

            if (listObjectContainElement.Count > 0)
            {
                PropertyElement.Add("Содержит объекты (ContainsElements)", listObjectContainElement);
            }


            return PropertyElement;
        }
        #endregion

        #region Заполнение характеристик элемента
        public bool FillCollectionPropertySet(ObservableCollection<IfcPropertySetDefinition> CollectionPropertySet)
        {
            if (this.IFCObjectDefinition == null)
            {
                return false;
            }

            if (IFCObjectDefinition is IfcObject obj)
            {
                CollectionPropertySet.Clear();

                IEnumerable<IfcPropertySetDefinition> collectionProperty = obj.IsDefinedBy.SelectMany(it => it.RelatingPropertyDefinition).OfType<IfcPropertySetDefinition>();

                foreach (IfcPropertySetDefinition propSetIsObj in collectionProperty)
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

            return true;
        }
        #endregion
    }
}