using IFC_Table_View.IFC.ModelItem;
using IFC_Table_View.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IFC_Table_View.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow(IEnumerable<BaseModelItemIFC> ModelElementsForSearch)
        {
            InitializeComponent();

            DataContext = new SearchWindowViewModel(ModelElementsForSearch);
        }

        void OnComboboxTextChanged(object sender, RoutedEventArgs e)
        {
            //CB.IsDropDownOpen = true;
            //// убрать selection, если dropdown только открылся
            //var tb = (TextBox)e.OriginalSource;
            //tb.Select(tb.SelectionStart + tb.SelectionLength, 0);
            //CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(CB.ItemsSource);
            //cv.Filter = s =>
            //    ((string)s).IndexOf(CB.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }
}
