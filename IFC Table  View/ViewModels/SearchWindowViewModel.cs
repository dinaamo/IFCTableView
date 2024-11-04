using GeometryGym.Ifc;
using GeometryGym.STEP;
using IFC_Table_View.IFC.ModelItem;
using IFC_Table_View.Infracrucrure.Commands;
using IFC_Table_View.Infracrucrure.Converter;
using IFC_Table_View.ViewModels.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Windows.Input;

namespace IFC_Table_View.ViewModels
{
    internal class SearchWindowViewModel : BaseViewModel
    {
        public List<SearchItem> SearchItems { get; private set; }

        #region Комманды

        #region Найти элементы


        public ICommand SelectElement { get; }

        private void OnSelectElementCommandExecuted(object o)
        {
            
        }


        private bool CanSelectElementCommandExecute(object o)
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
            SearchItems = new List<SearchItem>();
            foreach (BaseModelItemIFC modelItem in modelElementsForSearch)
            {
                SearchItems.Add(new SearchItem(modelItem));
            }

            #region Комманды

            SelectElement = new ActionCommand(
                OnSelectElementCommandExecuted,
                CanSelectElementCommandExecute);

            

            #endregion


        }
    }

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

        private List<object> _PropertiesName;

        public IEnumerable<object> PropertiesName 
        { 
            get
            {
                return _PropertiesName;
            }
            
        }

        private List<object> _Values;

        public List<object> Values
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

            _PropertiesName = new List<object>();
            _Values = new List<object>();

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
                            _PropertiesName.Add(convertItemIFCNameProperty.Convert(property, null, null, null));
                            _Values.Add(convertItemIFCValue.Convert(property, null, null, null));
                        }
                    }
                    else if (PropertiesDef is Dictionary<string, IfcPhysicalQuantity> quantities)
                    {
                        foreach (object quantity in quantities)
                        {
                            _PropertiesName.Add(convertItemIFCNameProperty.Convert(quantity, null, null, null));
                            _Values.Add(convertItemIFCValue.Convert(quantity, null, null, null));
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
