using GeometryGym.Ifc;
using GeometryGym.STEP;
using IFC_Table_View.IFC.ModelItem;
using IFC_Table_View.Infracrucrure.Commands;
using IFC_Table_View.Infracrucrure.Converter;
using IFC_Table_View.ViewModels.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace IFC_Table_View.ViewModels
{

    internal class SearchWindowViewModel : BaseViewModel
    {

        public string[] СonditionsSearch { get; private set; } = { "Равно", "Не равно", "Содержит", "Не содержит"};
        public ObservableCollection<SearchItem> SearchItems { get; private set; }

        private ObservableCollection<SearchItem> _FilteredSearchItems;

        public ObservableCollection<SearchItem> FilteredSearchItems
        {
            get
            {
                return _FilteredSearchItems;
            }

            private set
            {
                OnPropertyChanged("FilteredSearchItems");
                _FilteredSearchItems = value;
            }
        }

        #region Комманды

        #region Покрасить элементы
        public ICommand PaintElements { get; }

        private void OnPaintElementCommandExecuted(object o)
        {
            var topElement = SearchItems[0].GetModelItem();
            var foundItems = FilteredSearchItems.Select(it => it.GetModelItem());
            ModelItemIFCObject.FindMultiplyTreeObject(topElement, foundItems);
        }


        private bool CanPaintElementCommandExecute(object o)
        {
            return FilteredSearchItems != null && FilteredSearchItems.Count() > 0;
        }
        #endregion

        #region Сбросить условия поиска
        public ICommand ResetSeachСonditions { get; }

        private void OnResetSeachСonditionsCommandExecuted(object o)
        {
            object[] ControlArray = (object[])o;

            ((ComboBox)ControlArray[0]).SelectedIndex = 2;
            ((ComboBox)ControlArray[1]).Text = string.Empty;

            ((ComboBox)ControlArray[2]).SelectedIndex = 2;
            ((ComboBox)ControlArray[3]).Text = string.Empty;

            ((ComboBox)ControlArray[4]).SelectedIndex = 2;
            ((ComboBox)ControlArray[5]).Text = string.Empty;

            ((ComboBox)ControlArray[6]).SelectedIndex = 2;
            ((ComboBox)ControlArray[7]).Text = string.Empty;

            ((ComboBox)ControlArray[8]).SelectedIndex = 2;
            ((ComboBox)ControlArray[9]).Text = string.Empty;

            ((ComboBox)ControlArray[10]).SelectedIndex = 2;
            ((ComboBox)ControlArray[11]).Text = string.Empty;

            DataGrid dataGrid = ControlArray[12] as DataGrid;

            dataGrid.ItemsSource = FilteredSearchItems = new ObservableCollection<SearchItem>(SearchItems);
        }


        private bool CanResetSearchСonditionsCommandExecute(object o)
        {
            return true;
        }
        #endregion

        #region Фильтр
        public ICommand FilteredElements { get; }

        private void OnFilteredElementsCommandExecuted(object o)
        {
            object[] ControlArray = (object[])o;

            string FilterSearchValueGUID = ((ComboBox)ControlArray[0]).Text;
            string textGUID = ((ComboBox)ControlArray[1]).Text;

            string FilterSearchValueClassElement = ((ComboBox)ControlArray[2]).Text;
            string textClassElement = ((ComboBox)ControlArray[3]).Text;

            string FilterSearchValueNameElement = ((ComboBox)ControlArray[4]).Text;
            string textNameElement = ((ComboBox)ControlArray[5]).Text;

            string FilterSearchValuePropertySet = ((ComboBox)ControlArray[6]).Text;
            string textPropertySet = ((ComboBox)ControlArray[7]).Text;

            string FilterSearchValuePropertyName = ((ComboBox)ControlArray[8]).Text;
            string textPropertyName = ((ComboBox)ControlArray[9]).Text;

            string FilterSearchValuePropertyValue = ((ComboBox)ControlArray[10]).Text;
            string textPropertyValue = ((ComboBox)ControlArray[11]).Text;

            DataGrid dataGrid = ControlArray[12] as DataGrid;

            dataGrid.ItemsSource = null;

            var col1 = SearchItems.Where(it => IsFilterString(new List<string>() {it.GUID}, textGUID, FilterSearchValueGUID));
            var col2 = col1.Where(it => IsFilterString(new List<string>() { it.IFCClass }, textClassElement, FilterSearchValueClassElement));
            var col3 = col2.Where(it => IsFilterString(new List<string>() { it.Name },textNameElement, FilterSearchValueNameElement));
            var col4 = col3.Where(it => IsFilterString(it.PropertySetCollection?.Select(it => it.Name), textPropertySet, FilterSearchValuePropertySet));
            var col5 = col4.Where(it => IsFilterString(it.PropertiesName, textPropertyName, FilterSearchValuePropertyValue));
            var col6 = col5.Where(it => IsFilterString(it.Values, textPropertyValue, FilterSearchValuePropertyValue));

            FilteredSearchItems = new ObservableCollection<SearchItem>(col6);

            dataGrid.ItemsSource = FilteredSearchItems;
        }


        bool IsFilterString(IEnumerable<string> stringCollection, string seachString, string seachingFilter)
        {
            if (seachString == string.Empty)
            {
                return true;
            }
            else 
            {
                if(seachingFilter == "Равно")
                {
                    return stringCollection.Any(str => str.Equals(seachString));
                }
                else if (seachingFilter == "Не равно")
                {
                    return stringCollection.Any(str => !str.Equals(seachString));
                }
                else if (seachingFilter == "Содержит")
                {
                    return stringCollection.Any(str => str.Contains(seachString));
                }
                else if (seachingFilter == "Не содержит")
                {
                    return stringCollection.Any(str => !str.Contains(seachString));
                }
                else
                {
                    return false;
                }
            } 
        }

        private bool CanFilteredElementsCommandExecute(object o)
        {
            return true;
        }
        #endregion

        #endregion


        public SearchWindowViewModel()
        {
        }

        

        public SearchWindowViewModel(IEnumerable<ModelItemIFCObject> modelElementsForSearch)
        {
            SearchItems = new ObservableCollection<SearchItem>();
            foreach (ModelItemIFCObject modelItem in modelElementsForSearch)
            {
                SearchItems.Add(new SearchItem(modelItem));
            }

            FilteredSearchItems = new ObservableCollection<SearchItem>(SearchItems);
            #region Комманды

            PaintElements = new ActionCommand(
                OnPaintElementCommandExecuted,
                CanPaintElementCommandExecute);

            FilteredElements = new ActionCommand(
                OnFilteredElementsCommandExecuted,
                CanFilteredElementsCommandExecute);

            ResetSeachСonditions = new ActionCommand(
                OnResetSeachСonditionsCommandExecuted,
                CanResetSearchСonditionsCommandExecute);


            #endregion


        }
    }




    /// <summary>
    /// Элемент для поска
    /// </summary>
    public class SearchItem
    {
        ModelItemIFCObject modelItem;

        public SearchItem(ModelItemIFCObject modelItem)
        {
            this.modelItem = modelItem;
            SetParameters();

        }
        public string GUID { get; private set; }

        public string IFCClass { get; private set; }

        public string Name { get; private set; }

        public List<IfcPropertySetDefinition> PropertySetCollection { get; private set; }

        private List<string> _PropertiesName;

        public List<string> PropertiesName 
        { 
            get
            {
                return _PropertiesName;
            }
            private set { _PropertiesName = value; }
        }

        private List<string> _Values;

        public List<string> Values
        {
            get
            {
                return _Values;
            }
            private set { _Values = value; }
        }


        void SetParameters()
        {
            ConvertItemPropertiesIFC convertItemPropertiesIFC = new ConvertItemPropertiesIFC();
            ConvertItemIFCNameProperty convertItemIFCNameProperty = new ConvertItemIFCNameProperty();
            ConvertItemIFCValue convertItemIFCValue = new ConvertItemIFCValue();

            //Получаем класс
            IFCClass = ((IfcObjectDefinition)modelItem.ItemTreeView).StepClassName;

            //Получаем Имя
            Name = ((NamedObjectIfc)modelItem.ItemTreeView).Name;

            if (modelItem is ModelItemIFCObject modelObject)
            { 
                //Получаем GUID
                GUID = ((IfcObjectDefinition)modelItem.ItemTreeView).Guid.ToString();

                //Получаем наборы
                PropertySetCollection = modelObject.CollectionPropertySet.Select(prSet => prSet).ToList();

                PropertiesName = new List<string>();
                Values = new List<string>();

                //Получаем характеристики
                foreach (IfcPropertySetDefinition prSet in PropertySetCollection)
                {
                    object PropertiesDef = convertItemPropertiesIFC.Convert(prSet, null, null, null);

                    if (PropertiesDef is Dictionary<string, IfcProperty> properties)
                    {
                        foreach (object property in properties)
                        {
                            _PropertiesName.Add(Convert.ToString(convertItemIFCNameProperty.Convert(property, null, null, null)));
                            _Values.Add(Convert.ToString(convertItemIFCValue.Convert(property, null, null, null)));
                        }
                    }
                    else if (PropertiesDef is Dictionary<string, IfcPhysicalQuantity> quantities)
                    {
                        foreach (object quantity in quantities)
                        {
                            _PropertiesName.Add(Convert.ToString(convertItemIFCNameProperty.Convert(quantity, null, null, null)));
                            _Values.Add(Convert.ToString(convertItemIFCValue.Convert(quantity, null, null, null)));
                        }
                    }
                }
            }
        }

        public ModelItemIFCObject GetModelItem()
        {
            return modelItem;
        }
    }
}
