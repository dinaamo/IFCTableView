using IFC_Table_View.IFC.ModelItem;
using System;
using System.Globalization;
using System.Windows.Data;

namespace IFC_Table_View.Infracrucrure.Converter
{
    public class ConvertItemIFC : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IModelItemIFC element)
            {
                if (element.PropertyElement != null)
                {
                    return element.PropertyElement;
                }
                else
                {
                    return null;
                }
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

    //public class ConvertColorItemObject : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        return null;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        return null;
    //    }
    //}
}