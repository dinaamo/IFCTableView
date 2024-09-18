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

        public class ConvertItemIFCValue : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
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

        public class ConvertItemProperiesIFC : IValueConverter
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
                object element = ((IModelItemIFC)value)?.ItemTreeView;

                if (element != null)
                {
                    return System.Convert.ToString(element.GetType().Name);
                }
                else
                {
                    return "";
                }


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
                object element = ((IModelItemIFC)value)?.ItemTreeView;

                if (element is IfcObjectDefinition ifcObj)
                {
                    return System.Convert.ToString(ifcObj.Guid);
                }
                else if (element is IfcTable ifcTable)
                {
                    return "-";
                }
                else
                {
                    return "";
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
                object element = ((IModelItemIFC)value)?.ItemTreeView;

                if (element is IfcObjectDefinition ifcObjectDef)
                {
                    return System.Convert.ToString(ifcObjectDef.Name);
                }
                else if(element is IfcTable ifcTable)
                {
                    return System.Convert.ToString(ifcTable.Name);
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



