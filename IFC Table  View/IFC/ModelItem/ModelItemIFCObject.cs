﻿using GeometryGym.Ifc;
using IFC_Table_View.Infracrucrure;
using IFC_Table_View.Infracrucrure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace IFC_Table_View.IFC.ModelItem
{
    public class ModelItemIFCObject : BaseModelItemIFC
    {
        private IfcObjectDefinition _IFCObjectDefinition;

        public event EventHandler<PropertyReferenceChangedEventArg> PropertyReferenceChanged;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ModelItemIFCObject(IfcObjectDefinition IFCObject, ModelItemIFCObject TopElement)
        {
            //Если есть элемент выше по дереву то подключаем к нему обработчик события изменения состояния элемента
            if (TopElement != null)
            {
                PropertyReferenceChanged += TopElement.ChangePropertyReference;
            }

            _IFCObjectDefinition = IFCObject;


            InitializationModelObject();
        }

        /// <summary>
        /// Инициализация элементов объекта модели
        /// </summary>
        private void InitializationModelObject()
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
                IsContainPropertyReference = true;
            }
            else
            {
                IsContainPropertyReference = false;
            }

            PropertyReferenceChanged?.Invoke(this, new PropertyReferenceChangedEventArg(IsContainPropertyReference));
        }

        /// <summary>
        /// Прокидываем по дереву вверх состояние элемента
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        public void ChangePropertyReference(object obj, PropertyReferenceChangedEventArg e)
        {
            if (e.IsContainPropertyDownTreeReference)
            {
                IsContainPropertyReferenceDownTree = true;
            }
            else if (!e.IsContainPropertyDownTreeReference)
            {
                bool searchResult = ModelItems.
                        Cast<ModelItemIFCObject>().
                        Select(mi => mi.IsContainPropertyReferenceValue()).Any(pr => pr == true);

                if (searchResult)
                {
                    IsContainPropertyReferenceDownTree = true;
                }
                else
                {
                    IsContainPropertyReferenceDownTree = false;
                }
            }
            else
            {
                IsContainPropertyReferenceDownTree = false;
            }

            PropertyReferenceChanged?.Invoke(this, new PropertyReferenceChangedEventArg(IsContainPropertyReferenceDownTree));

        }

        private bool _IsContainPropertyReference { get; set; } = false;
        /// <summary>
        /// Наличие в элементе ссылки
        /// </summary>
        public bool IsContainPropertyReference
        {
            get { return _IsContainPropertyReference; }
            set
            {
                if (!value && !IsContainPropertyReferenceDownTree)
                {
                    IsNotContainAnyReferenceProperty = true;
                }
                else
                {
                    IsNotContainAnyReferenceProperty = false;
                }

                _IsContainPropertyReference = value;
                OnPropertyChanged("IsContainPropertyReference");
            }
        }

        private bool _IsContainPropertyReferenceDownTree { get; set; }
        /// <summary>
        /// Наличие в ниже по дереву элементов в ссылками
        /// </summary>
        public bool IsContainPropertyReferenceDownTree
        {
            get { return _IsContainPropertyReferenceDownTree; }
            set
            {
                if (!value && !IsContainPropertyReference)
                {
                    IsNotContainAnyReferenceProperty = true;
                }
                else
                {
                    IsNotContainAnyReferenceProperty = false;
                }

                _IsContainPropertyReferenceDownTree = value;
                OnPropertyChanged("IsContainPropertyReferenceDownTree");
            }
        }

        private bool _IsNotContainAnyReferenceProperty { get; set; }
        /// <summary>
        /// Не содержит ни в себе, ни ниже по дереву ссылок
        /// </summary>
        public bool IsNotContainAnyReferenceProperty
        {
            get { return _IsNotContainAnyReferenceProperty; }
            set
            {
                _IsNotContainAnyReferenceProperty = value;
                OnPropertyChanged("IsNotContainAnyReferenceProperty");
            }
        }
        
        ModelObjectHelper modelHelper;

  

        /// <summary>
        /// Добавить ссылку на таблицу
        /// </summary>
        /// <param name="tableItemSet"></param>
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
                PropertyReferenceChanged?.Invoke(this, new PropertyReferenceChangedEventArg(IsContainPropertyReference));
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
                        IfcRelDefinesByProperties relDefProp = new IfcRelDefinesByProperties(_IFCObjectDefinition, PropSetTableReference);
                        obj.IsDefinedBy.Add(relDefProp);
                    }
                    else
                    {
                        PropSetTableReference.AddProperty(pref);
                    }
                    //Добавляем в таблицу ссылки
                    RefTable.AddReferenceToTheElement(this);

                    IsContainPropertyReference = true;
                    //Прокидываем наверх по дереву событие добавления ссылки
                    PropertyReferenceChanged?.Invoke(this, new PropertyReferenceChangedEventArg(IsContainPropertyReference));

                }

                modelHelper.FillCollectionPropertySet(CollectionPropertySet);
            }
        }

        /// <summary>
        /// Получение элемента вложенного дерева
        /// </summary>
        public override object ItemTreeView
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
        public override Dictionary<string, HashSet<object>> PropertyElement
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

        private ObservableCollection<BaseModelItemIFC> _ModelItems;
        /// <summary>
        /// Элемент дерева
        /// </summary>
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


        /// <summary>
        /// Проверка наличия ниже по дереву ссылок
        /// </summary>
        /// <returns></returns>
        public bool IsContainPropertyReferenceValue()
        {
            if (IsContainPropertyReference)
            { 
                return true;
            }
            else
            {
                foreach (ModelItemIFCObject modelItem in ModelItems.OfType<ModelItemIFCObject>())
                {
                    bool searchResult = modelItem.ModelItems.
                        Cast<ModelItemIFCObject>().
                        Select(mi => mi.IsContainPropertyReferenceValue()).Any(pr => pr == true);

                    if (searchResult)
                    { 
                        return true; 
                    }
                }
                return false;
            }
        }
    }




    /// <summary>
    /// Вспомогательный класс для объекта дерева
    /// </summary>
    class ModelObjectHelper
    {
        IfcObjectDefinition ifcObjectDefinition;

        public ModelObjectHelper(IfcObjectDefinition IFCObjectDefinition)
        {
            this.ifcObjectDefinition = IFCObjectDefinition;
        }

        #region Заполнение свойств элемента
        public Dictionary<string, HashSet<object>> GetPropertyObject()
        {

            Dictionary<string, HashSet<object>> PropertyElement = new Dictionary<string, HashSet<object>>();
            if (this.ifcObjectDefinition == null)
            { 
                return PropertyElement; 
            }

            //Материал
            if (ifcObjectDefinition is IfcElement IFCElement)
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

            if (ifcObjectDefinition.Description != string.Empty)
            {
                PropertyElement.Add("Описание", new HashSet<object>() { ifcObjectDefinition.Description });
            }

            //Вложенные объекты
            HashSet<object> listobjectIsNestedBy = new HashSet<object>();
            foreach (IfcRelNests relNest in ifcObjectDefinition.IsNestedBy)
            {
                foreach (IfcObjectDefinition obj in relNest.RelatedObjects)
                {
                    if (obj == ifcObjectDefinition)
                    {
                        continue;
                    }
                    else
                    {
                        listobjectIsNestedBy.Add(
                            $"Наименование связи: {relNest.Name}\n" +
                            $"Описание связи: {relNest.Description}\n" +
                            $"Наименование элемента: {obj.Name}\n" +
                            $"Класс IFC: {obj.GetType().Name}\n" +
                            $"GUID: {obj.Guid}");
                    }
                }
            }
            if (listobjectIsNestedBy.Count > 0)
            {
                PropertyElement.Add("Вложенные объекты (IsNestedBy)", listobjectIsNestedBy);
            }

            //Разлагается на объекты
            HashSet<object> listobjectIsDecomposedBy = new HashSet<object>();
            foreach (IfcRelAggregates relDecomp in ifcObjectDefinition.IsDecomposedBy)
            {
                foreach (IfcObjectDefinition obj in relDecomp.RelatedObjects)
                {
                    if (obj == ifcObjectDefinition)
                    {
                        continue;
                    }
                    else
                    {
                        listobjectIsDecomposedBy.Add(
                            $"Наименование связи: {relDecomp.Name}\n" +
                            $"Описание связи: {relDecomp.Description}\n" +
                            $"Наименование элемента: {obj.Name}\n" +
                            $"Класс IFC: {obj.GetType().Name}\n" +
                            $"GUID: {obj.Guid}");
                    }
                }
            }
            if (listobjectIsDecomposedBy.Count > 0)
            {
                PropertyElement.Add("Раскладывается на объекты (IsDecomposedBy)", listobjectIsDecomposedBy);
            }

            if (ifcObjectDefinition is IfcObject IFCObject)
            {


                //Тип объекта
                if (IFCObject.ObjectType != string.Empty)
                {
                    PropertyElement.Add("Тип", new HashSet<object>() { IFCObject.ObjectType });
                }

                //Связанные объекты
                HashSet<object> listObjectIsDefinedBy = new HashSet<object>();
                foreach (IfcRelDefinesByProperties relDef in IFCObject.IsDefinedBy)
                {
                    foreach (IfcObjectDefinition obj in relDef.RelatedObjects)
                    {
                        if (obj == ifcObjectDefinition)
                        {
                            continue;
                        }
                        else
                        {
                            listObjectIsDefinedBy.Add(
                            $"Наименование связи: {relDef.Name}\n" +
                            $"Описание связи: {relDef.Description}\n" +
                            $"Наименование элемента: {obj.Name}\n" +
                            $"Класс IFC: {obj.GetType().Name}\n" +
                            $"GUID: {obj.Guid}");
                        }

                    }
                }

                if (listObjectIsDefinedBy.Count > 0)
                {
                    PropertyElement.Add("Связанные объекты (IsDefinedBy)", listObjectIsDefinedBy);
                }
            }

            //Содержит объекты
            HashSet<object> listObjectContainElement = new HashSet<object>();

            if (ifcObjectDefinition is IfcSpatialStructureElement IFCStrElem)
            {
                foreach (IfcRelContainedInSpatialStructure spartialStucture in IFCStrElem.ContainsElements)
                {
                    foreach (IfcProduct obj in spartialStucture.RelatedElements)
                    {
                        if (obj == ifcObjectDefinition)
                        {
                            continue;
                        }
                        else
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
            if (this.ifcObjectDefinition == null)
            {
                return false;
            }

            if (ifcObjectDefinition is IfcObject obj)
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