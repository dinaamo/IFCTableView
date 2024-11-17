using GeometryGym.Ifc;
using IFC_Table_View.IFC.ModelItem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Xml.Linq;

namespace IFC_Table_View.Infracrucrure.Converter
{
    public class ConvertItemIFC : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BaseModelItemIFC elementFile)
            {
                return elementFile.PropertyElement;
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

    public class ConvertConvPropertyItem : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ConvertConvReferenceToObject : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ModelItemIFCObject elementObject)
            {
                return $"Guid:{elementObject.IFCObjectGUID} " +
                        $"\nКласс: {elementObject.IFCObjectClass} " +
                        $"\nИмя: {elementObject.IFCObjectName}";
            }
            else
            {
                return value;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}