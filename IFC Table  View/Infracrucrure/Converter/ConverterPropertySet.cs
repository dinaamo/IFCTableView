using GeometryGym.Ifc;
using IFC_Table_View.IFC.ModelItem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace IFC_Table_View.Infracrucrure.Converter
{
    // <summary>
    /// Возвращаем наименование характеристики
    /// </summary>
    public class ConvertItemIFCNameProperty : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is KeyValuePair<string, IfcProperty> ifcPairProperty)
            {
                return ifcPairProperty.Key;
            }
            else if (value is KeyValuePair<string, IfcPhysicalQuantity> ifcPairPhysicalQuantity)
            {
                return ifcPairPhysicalQuantity.Key;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// Возвращаем значение характеристики
    /// </summary>
    public class ConvertItemIFCValue : IValueConverter
        {
            public object Convert(object valueObject, Type targetType, object parameter, CultureInfo culture)
            {
                object value = null;

                if (valueObject is KeyValuePair<string, IfcProperty> ifcPairProperty)
                {
                    value = ifcPairProperty.Value;
                }
                else if (valueObject is KeyValuePair<string, IfcPhysicalQuantity> ifcPairPhysicalQuantity)
                {
                    value = ifcPairPhysicalQuantity.Value;
                }

                if (value is IfcPropertySingleValue ifcValue)
                {
                    return System.Convert.ToString(ifcValue?.NominalValue?.ValueString);
                }
                else if(value is IfcPropertyReferenceValue ifcRefValue)
                {
                    return System.Convert.ToString(ifcRefValue.PropertyReference);
                }
                else if (value is IfcQuantityArea quantityArea)
                {
                    return System.Convert.ToString(quantityArea.AreaValue);
                }
                else if (value is IfcQuantityCount quantityCount)
                {
                    return System.Convert.ToString(quantityCount.CountValue);
                }
                else if (value is IfcQuantityLength quantityLength)
                {
                    return System.Convert.ToString(quantityLength.LengthValue);
                }
                else if (value is IfcQuantityTime quantityTime)
                {
                        return System.Convert.ToString(quantityTime.TimeValue);
                }
                else if (value is IfcQuantityVolume quantityVolume)
                {
                    return System.Convert.ToString(quantityVolume.VolumeValue);
                }
                else if (value is IfcQuantityWeight quantityWeight)
                {
                    return System.Convert.ToString(quantityWeight.WeightValue);
                }
                else
                {
                    return null;
                }        
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }
        }

        public class ConvertItemPropertiesIFC : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
            
                if (value is IfcPropertySet PropertySet)
                {
                    return PropertySet.HasProperties;
                }
                else if (value is IfcElementQuantity PropertySetQuantity)
                {
                    return PropertySetQuantity.Quantities;
                }
                else
                {
                    return null;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }
        }

        public class ConvertItemPropSetIFC : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is ModelItemIFCObject element)
                {
                    return element.CollectionPropertySet;
                }
                else
                {
                    return null;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }
        }

        public class ConvertItemStatusClassIFC : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return System.Convert.ToString(value?.GetType().Name);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }
        }

        public class ConvertItemStatusGUID : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is ModelItemIFCObject modelObject)
                {
                    return System.Convert.ToString(modelObject.IFCObjectGUID);
                }
                else
                {
                    return "-";
                }

            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }
        }

        public class ConvertItemStatusName : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {

                if (value is ModelItemIFCObject modelItem)
                {
                    return System.Convert.ToString(modelItem.IFCObjectName);
                }
                else if(value is BaseModelReferenceIFC referenceObject)
                {
                    return System.Convert.ToString(referenceObject.NameReference);
                }
                else
                {
                    return string.Empty;
                }

            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }
        }

        public class ConvertItemTreeName : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is IfcObjectDefinition ifcObj)
                {
                    if (ifcObj.Name == "")
                    {
                        return ifcObj.StepClassName;
                    }
                    else
                    {
                        return ifcObj.Name;
                    }
                }
                else if (value is IfcTable ifcTable)
                {
                    return ifcTable.Name;
                }
                else
                {
                    return "?????";
                }

            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }
        }

        public class ConvertItemTypeClassIFC : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {

                return value?.GetType().Name;

            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }
        }
}



