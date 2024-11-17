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
                        new TableWindow(table.dataTable).ShowDialog();
                    }
                }
            }
        }


        private void TreeViewSelectedItemChanged(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                item.BringIntoView();
                e.Handled = true;
            }
        }


    }



}