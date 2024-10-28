using IFC_Table_View.IFC.ModelItem;
using IFC_Table_View.View.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IFC_Table_View.Resourses
{
    public partial class DictionaryClass
    {
        public void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                if (sender is TextBlock textBlock)
                {
                    if (textBlock.DataContext is ModelItemIFCTable table)
                    {
                        new WindowTable(table.IFCTable).ShowDialog();
                    }
                }
            }
        }

        //double angleRotate = 0;
        //public void Mouse_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    Button btn = sender as Button;
        //    if (btn != null)
        //    {
        //        angleRotate += 180;
        //        RotateTransform transform = new RotateTransform(angleRotate);
        //        btn.RenderTransform = transform;
        //    }
        //}

    }



}