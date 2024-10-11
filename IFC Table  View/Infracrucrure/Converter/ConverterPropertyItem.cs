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
            if (value is IModelItemIFC elementFile)
            {
                return elementFile?.PropertyElement;
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
                IfcObjectDefinition ifcObject = (IfcObjectDefinition)elementObject.ItemTreeView;

                return $"Guid:{ifcObject.Guid} " +
                        $"\nКласс: {ifcObject.StepClassName} " +
                        $"\nИмя: {ifcObject.Name}";
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