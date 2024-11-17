using GeometryGym.Ifc;
using IFC_Table_View.Data;
using IFC_Table_View.IFC.Model;
using IFC_Table_View.Infracrucrure;
using IFC_Table_View.Infracrucrure.Commands;
using IFC_Table_View.Infracrucrure.Commands.Base;
using IFC_Table_View.Infracrucrure.FindObjectException;
using IFC_Table_View.View.Windows;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace IFC_Table_View.IFC.ModelItem
{
    public class ModelItemIFCObject : BaseModelItemIFC
    {

        /// <summary>
        /// Конструктор
        /// </summary>
        public ModelItemIFCObject(IfcObjectDefinition IFCObject, ModelItemIFCObject TopElement, ModelIFC modelIFC) : base(modelIFC, IFCObject)
        {
            //Если есть элемент выше по дереву то подключаем к нему обработчик события изменения состояния элемента
            if (TopElement != null)
            {
                PropertyReferenceChanged += TopElement.ChangePropertyReference;
            }

            IFCObjectDefinition = IFCObject;

            PaintElementsCommand = new ActionCommand(
                   OnPaintElementsCommandExecuted,
                   CanPaintElementsCommandExecute);

            SearchElementsCommand = new ActionCommand(
                   OnSearchElementsCommandExecuted,
                   CanSearchElementsCommandExecute);

            ResetSearchCommand = new ActionCommand(
                OnResetSearchCommandExecuted,
                CanResetSearchCommandExecute);

            AddReferenceToTheTable = new ActionCommand(
                    OnAddReferenceToTheTable,
                    CanAddReferenceToTheTable);

            DeleteReferenceToTheTable = new ActionCommand(
                OnDeleteReferenceToTheTable,
                CanDeleteReferenceToTheTable);

            InitializationModelObject();
        }

        #region Комманды

        #region Выделить элемент
        public ICommand PaintElementsCommand { get; }

        private void OnPaintElementsCommandExecuted(object o)
        {
            IsPaint = true;
        }


        private bool CanPaintElementsCommandExecute(object o)
        {
            return true;
        }
        #endregion

        #region Поиск элементов
        public ICommand SearchElementsCommand { get;}

        private void OnSearchElementsCommandExecuted(object o)
        {
            if (o is ModelItemIFCObject modelItem)
            {
                SearchWindow.CreateWindowSearch(SelectionElements(modelItem));
            }
        }


        private bool CanSearchElementsCommandExecute(object o)
        {
            return true;
        }
        #endregion

        #region Сброс выделения
        public ICommand ResetSearchCommand { get;}

        private void OnResetSearchCommandExecuted(object o)
        {
            if (o is ModelItemIFCObject modelItem)
            {
                SelectionElements(modelItem).ForEach(it => it.IsPaint = false);
            }
        }

        private bool CanResetSearchCommandExecute(object o)
        {
            return true;

        }
        #endregion

        #region Добавить к элементу связь с элементом 
        public ICommand AddReferenceToTheTable { get; }

        private void OnAddReferenceToTheTable(object o)
        {

            if (o is ModelItemIFCObject modelObject)
            {

                List<BaseModelReferenceIFC> collectionModelTable = modelIFC.ModelItems[0].ModelItems.
                                                    OfType<BaseModelReferenceIFC>().
                                                    ToList();


                Form_Add_Reference_To_Table form_Add_Reference_To_Table = new Form_Add_Reference_To_Table(new List<ModelItemIFCObject> { this } , collectionModelTable);

                form_Add_Reference_To_Table.ShowDialog();


                AddReferenceToTheObjectReference(form_Add_Reference_To_Table.CollectionTableToAdd);

            }
        }

        private bool CanAddReferenceToTheTable(object o)
        {
            return true;
        }
        #endregion

        #region Удалить ссылки на документ или таблицу
        public ICommand DeleteReferenceToTheTable { get; }

        private void OnDeleteReferenceToTheTable(object o)
        {
            List<BaseModelReferenceIFC> collectionModelTable = modelIFC.ModelItems[0].ModelItems.
                                                OfType<BaseModelReferenceIFC>().
                                                ToList();

            Form_Delete_Reference_To_Table form_Delete_Reference_To_Table = new Form_Delete_Reference_To_Table(new List<ModelItemIFCObject>() { this }, collectionModelTable);

            form_Delete_Reference_To_Table.ShowDialog();



            DeleteReferenceToTheObjectReference(form_Delete_Reference_To_Table.CollectionModelitemTableToDelete);

        }

        private bool CanDeleteReferenceToTheTable(object o)
        {
            return true;
        }
        #endregion

        #endregion

        #region Методы

        public IfcObjectDefinition GetIFCObject()
        {
            return IFCObjectDefinition;
        }

        /// <summary>
        /// Инициализация элементов объекта модели
        /// </summary>
        private void InitializationModelObject()
        {

            modelHelper = new ModelObjectHelper(IFCObjectDefinition);

            CollectionPropertySet = new ObservableCollection<IfcPropertySetDefinition>();

            modelHelper.FillCollectionPropertySet(CollectionPropertySet);

            PropertyElement = modelHelper.GetPropertyObject();

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

        public event EventHandler<PropertyReferenceChangedEventArg> PropertyReferenceChanged;
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
                //Проверка наличия ниже по дереву ссылок
                bool searchResult = SelectionElements(this)
                                        .Where(it => it != this)
                                        .Any(it => it.IsContainPropertyReference);

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

        ModelObjectHelper modelHelper;

        /// <summary>
        /// Удаление ссылок на таблицы
        /// </summary>
        /// <param name="CollectionModelitemTableToDelete"></param>
        public void DeleteReferenceToTheObjectReference(List<BaseModelReferenceIFC> CollectionModelitemTableToDelete)
        {
            List<BaseModelReferenceIFC> collectionModelTable = modelIFC.ModelItems[0].ModelItems.
                                                OfType<BaseModelReferenceIFC>().
                                                ToList();

            if (CollectionModelitemTableToDelete.Count == 0)
            {
                return;
            }

            //Получаем необходимый набор характеристик
            IfcPropertySet PropSetTableReference = CollectionPropertySet.
                                                   OfType<IfcPropertySet>().
                                                   FirstOrDefault(it => it.Name == "RZDP_Ссылки");
            if (PropSetTableReference == null)
            {
                return;
            }

            //Удаляем ссылки
            foreach (BaseModelReferenceIFC modelTable in CollectionModelitemTableToDelete)
            {
                BaseModelReferenceIFC findModelTable = collectionModelTable.FirstOrDefault(it => it == modelTable);
                if (findModelTable == null) {continue;}
                
                findModelTable.DeleteReferenceToTheElement(this);


                IfcPropertyReferenceValue propertyToDelete = CollectionPropertySet?.OfType<IfcPropertySet>()
                                                                                    .SelectMany(it => it.HasProperties.Values)
                                                                                    .OfType<IfcPropertyReferenceValue>()
                                                                                    .FirstOrDefault(it => it.PropertyReference == modelTable.GetReferense());
                
                PropSetTableReference?.RemoveProperty(propertyToDelete);
            }

            //Проверяем, остались ли еще ссылки на другие таблицы
            if (PropSetTableReference.HasProperties.Count == 0)
            {
                GeometryGym.STEP.SET<IfcRelDefinesByProperties> ifcRelDefinesByProperties = null;
                if (IFCObjectDefinition is IfcObject ifcObject)
                {
                    ifcRelDefinesByProperties = ifcObject.IsDefinedBy;
                }
                else if (IFCObjectDefinition is IfcContext ifcContext)
                {

                    ifcRelDefinesByProperties = ifcContext.IsDefinedBy;
                }

                IfcRelDefinesByProperties FindRelDef = ifcRelDefinesByProperties
                       .FirstOrDefault(it => it.RelatingPropertyDefinition.Contains(PropSetTableReference));

                //Удаляем пустой набор свойств
                FindRelDef.RelatingPropertyDefinition.Remove(PropSetTableReference);

                //Удаляем пустой промежуточный класс
                if (FindRelDef.RelatingPropertyDefinition.Count == 0)
                {
                    ifcRelDefinesByProperties.Remove(FindRelDef);
                }
                IsContainPropertyReference = false;
                //Прокидываем наверх по дереву событие удаления ссылок
                PropertyReferenceChanged?.Invoke(this, new PropertyReferenceChangedEventArg(IsContainPropertyReference));
            }
            //Заполняем заново коллекцию характеристик
            modelHelper.FillCollectionPropertySet(CollectionPropertySet);
        }

        /// <summary>
        /// При заполнении дерева, если в элементе есть ссылка на таблицу,
        /// то добавляем к таблице обратную ссылку на элемент
        /// </summary>
        /// <param name="tableItemSet"></param>
        public void AddReferenceToTheObjectReferenceToload(ObservableCollection<BaseModelReferenceIFC> ReferenceElementSet)
        {
            IEnumerable<IfcPropertyReferenceValue> propertyReferenceSet = CollectionPropertySet?.OfType<IfcPropertySet>().
                SelectMany(it => it.HasProperties.Values).
                OfType<IfcPropertyReferenceValue>();

            foreach (IfcPropertyReferenceValue propertyReference in propertyReferenceSet)
            {
                foreach (ModelItemIFCTable tableItem in ReferenceElementSet)
                {
                    if (propertyReference.PropertyReference == tableItem.GetReferense())
                    {
                        tableItem.AddReferenceToTheElement(this);
                    }
                }
            }
        }

        /// <summary>
        /// Добавление ссылок на таблицы от элемента
        /// </summary>
        /// <param name="modelReferenceSet"></param>
        public void AddReferenceToTheObjectReference(List<BaseModelReferenceIFC> modelReferenceSet)
        {
            IfcPropertySet PropSetTableReference = CollectionPropertySet.
                                                OfType<IfcPropertySet>().
                                                FirstOrDefault(it => it.Name == "RZDP_Ссылки");

            foreach (BaseModelReferenceIFC modelReference in modelReferenceSet)
            {
                IfcPropertyReferenceValue pref = new IfcPropertyReferenceValue(IFCObjectDefinition.Database, modelReference.NameReference);

                pref.PropertyReference = modelReference.GetReferense();
  
                if (PropSetTableReference == null)
                {
                    PropSetTableReference = new IfcPropertySet("RZDP_Ссылки", pref);
                    IfcRelDefinesByProperties relDefProp = new IfcRelDefinesByProperties(IFCObjectDefinition, PropSetTableReference);
                    if (IFCObjectDefinition is IfcObject obj)
                    {
                        obj.IsDefinedBy.Add(relDefProp);
                    }
                    else if (IFCObjectDefinition is IfcContext context)
                    {
                        context.IsDefinedBy.Add(relDefProp);
                    }
                }
                else
                {
                    PropSetTableReference.AddProperty(pref);
                }
                //Добавляем в таблицу ссылку на текущий элемент
                modelReference.AddReferenceToTheElement(this);

                IsContainPropertyReference = true;
                //Прокидываем наверх по дереву событие добавления ссылки
                PropertyReferenceChanged?.Invoke(this, new PropertyReferenceChangedEventArg(IsContainPropertyReference));

            }

            //Обновляем коллекцию характеристик
            modelHelper.FillCollectionPropertySet(CollectionPropertySet);
            
        }


        /// <summary>
        /// Ищем все элементы в дереве по критериям
        /// </summary>
        /// <param name="topElement"></param>
        /// <param name="foundObjects"></param>
        public static void FindMultiplyTreeObject(ModelItemIFCObject topElement, IEnumerable<ModelItemIFCObject> foundObjects)
        {
            if (foundObjects.Any(it => it == topElement))
            {
                topElement.IsPaint = true;
            }

            topElement.IsExpanded = true;

            foreach (ModelItemIFCObject item in topElement.ModelItems)
            {
                FindMultiplyTreeObject(item, foundObjects);
            }
        }

        /// <summary>
        /// Ищем все элементы в дереве по критериям
        /// </summary>
        /// <param name="topElement"></param>
        /// <param name="foundObjects"></param>
        public static List<ModelItemIFCObject> FindPaintObject(ModelItemIFCObject topElement)
        {
            return topElement.SelectionElements(topElement).Where(it => it.IsPaint).ToList();
        }

        /// <summary>
        /// Ищем один элемент в дереве
        /// </summary>
        public static void FindSingleTreeObject(ModelItemIFCObject topElement, ModelItemIFCObject foundObject)
        {
            foreach (ModelItemIFCObject item in topElement.ModelItems)
            {
                if (item == foundObject)
                {
                    throw new FindObjectException(item);
                }
            }

            foreach (ModelItemIFCObject item in topElement.ModelItems)
            {
                item.IsExpanded = true;
                FindSingleTreeObject(item, foundObject);
            }
        }

        /// <summary>
        /// Выборка элементов ниже по дереву
        /// </summary>
        /// <param name="modelItem"></param>
        /// <returns></returns>
        private List<ModelItemIFCObject> SelectionElements(ModelItemIFCObject modelItem)
        {
            List<ModelItemIFCObject> list = new List<ModelItemIFCObject>();

            list.Add(modelItem);

            foreach (ModelItemIFCObject nestModelItem in modelItem.ModelItems)
            {
                list.AddRange(SelectionElements(nestModelItem));
            }

            return list;

        }

        #endregion

        #region Свойства

        private IfcObjectDefinition IFCObjectDefinition;

        public string IFCObjectGUID => IFCObjectDefinition.Guid.ToString();

        public string IFCObjectName => IFCObjectDefinition.Name;

        public string IFCObjectClass => IFCObjectDefinition.StepClassName;


        
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


        /// <summary>
        /// Покрасить элемент
        /// </summary>
        private bool _IsPaint { get; set; }
        public bool IsPaint
        {
            get { return _IsPaint; }
            set
            {
                _IsPaint = value;
                OnPropertyChanged("IsPaint");
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
            protected set 
            {
                _PropertyElement = value;
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


        #endregion 

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

            CollectionPropertySet.Clear();
            if (ifcObjectDefinition is IfcObject obj)
            {
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
            else if (ifcObjectDefinition is IfcContext context)
            {
                IEnumerable<IfcPropertySetDefinition> collectionProperty = context.IsDefinedBy.SelectMany(it => it.RelatingPropertyDefinition).OfType<IfcPropertySetDefinition>();
                
                foreach (IfcPropertySetDefinition propSetIsObj in collectionProperty)
                {
                    CollectionPropertySet.Add(propSetIsObj);
                }           
            }

            return true;
        }
        #endregion
    }
}