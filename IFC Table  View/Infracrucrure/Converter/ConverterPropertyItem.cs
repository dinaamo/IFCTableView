using GeometryGym.Ifc;
using IFC_Table_View.IFC.ModelItem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

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

    public class ConvertConvPropertyesItem : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HashSet<object> collectionProperty)
            {
                HashSet<string> collectionTableReference = new HashSet<string>();
                foreach (object element in collectionProperty)
                {
                    if (element is ModelItemIFCObject elementObject)
                    {
                        IfcObjectDefinition ifcObject = (IfcObjectDefinition)elementObject.ItemTreeView;
                        collectionTableReference.Add((
                                        $"Guid:{ifcObject.Guid} " +
                                        $"\nКласс: {ifcObject.StepClassName} " +
                                        $"\nИмя: {ifcObject.Name}"));
                    }
                }
                return collectionTableReference;
            }
            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}