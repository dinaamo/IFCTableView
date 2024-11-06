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

        #region Найти элементы
        public ICommand SelectElements { get; }

        private void OnSelectElementCommandExecuted(object o)
        {
            
        }


        private bool CanSelectElementCommandExecute(object o)
        {
            return true;
        }
        #endregion

        #region Фильтр
        public ICommand FilteredElements { get; }

        private void OnFilteredElementsCommandExecuted(object o)
        {
            object[] ComboBoxArray = (object[])o;

            string textGUID = ((ComboBox)ComboBoxArray[0]).Text;
            string textClassElement = ((ComboBox)ComboBoxArray[1]).Text;
            string textNameElement = ((ComboBox)ComboBoxArray[2]).Text;
            string textPropertySet = ((ComboBox)ComboBoxArray[3]).Text;
            string textPropertyName = ((ComboBox)ComboBoxArray[4]).Text;
            string textPropertyValue = ((ComboBox)ComboBoxArray[5]).Text;


            FilteredSearchItems = new ObservableCollection<SearchItem>(SearchItems.
                        Where(it => it.GUID.Contains(textGUID)).
                        Where(it => it.IFCClass.Contains(textClassElement)).
                        Where(it => it.Name.Contains(textNameElement)).
                        Where(it => it.PropertySetCollection.Select(prSet => prSet.Name).
                                        Any(namePrSet => namePrSet.Contains(textPropertySet))).
                        Where(it => it.PropertiesName.Any(namePr => namePr.Contains(textPropertyName))).
                        Where(it => it.Values.Any(valuePr => valuePr.Contains(textPropertyValue))));
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

        

        public SearchWindowViewModel(IEnumerable<BaseModelItemIFC> modelElementsForSearch)
        {
            SearchItems = new ObservableCollection<SearchItem>();
            foreach (BaseModelItemIFC modelItem in modelElementsForSearch)
            {
                SearchItems.Add(new SearchItem(modelItem));
            }

            FilteredSearchItems = new ObservableCollection<SearchItem>(SearchItems);

            #region Комманды

            SelectElements = new ActionCommand(
                OnSelectElementCommandExecuted,
                CanSelectElementCommandExecute);

            FilteredElements = new ActionCommand(
                OnFilteredElementsCommandExecuted,
                CanFilteredElementsCommandExecute);



            #endregion


        }
    }




    /// <summary>
    /// Элемент для поска
    /// </summary>
    public class SearchItem
    {
        BaseModelItemIFC modelItem;

        public SearchItem(BaseModelItemIFC modelItem)
        {
            this.modelItem = modelItem;
            SetParameters();

        }
        public string GUID { get; private set; }

        public string IFCClass { get; private set; }

        public string Name { get; private set; }

        public List<IfcPropertySetDefinition> PropertySetCollection { get; private set; }

        private List<string> _PropertiesName;

        public IEnumerable<string> PropertiesName 
        { 
            get
            {
                return _PropertiesName;
            }
            
        }

        private List<string> _Values;

        public List<string> Values
        {
            get
            {
                return _Values;
            }
        }


        void SetParameters()
        {
            ConvertItemPropertiesIFC convertItemPropertiesIFC = new ConvertItemPropertiesIFC();
            ConvertItemIFCNameProperty convertItemIFCNameProperty = new ConvertItemIFCNameProperty();
            ConvertItemIFCValue convertItemIFCValue = new ConvertItemIFCValue();

            _PropertiesName = new List<string>();
            _Values = new List<string>();

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

                //Получаем характеристики
                foreach (IfcPropertySetDefinition prSet in PropertySetCollection)
                {
                    object PropertiesDef = convertItemPropertiesIFC.Convert(prSet, null, null, null);

                    if (PropertiesDef is Dictionary<string, IfcProperty> properties)
                    {
                        foreach (object property in properties)
                        {
                            _PropertiesName.Add(convertItemIFCNameProperty.Convert(property, null, null, null).ToString());
                            _Values.Add(convertItemIFCValue.Convert(property, null, null, null).ToString());
                        }
                    }
                    else if (PropertiesDef is Dictionary<string, IfcPhysicalQuantity> quantities)
                    {
                        foreach (object quantity in quantities)
                        {
                            _PropertiesName.Add(convertItemIFCNameProperty.Convert(quantity, null, null, null).ToString());
                            _Values.Add(convertItemIFCValue.Convert(quantity, null, null, null).ToString());
                        }
                    }
                }
            }
        }

        public BaseModelItemIFC GetModelItem()
        {
            return modelItem;
        }
    }
}
