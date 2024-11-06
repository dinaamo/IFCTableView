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

            SearchWindowViewModel searchWindowViewModel = new SearchWindowViewModel(ModelElementsForSearch);

            DataContext = searchWindowViewModel;


            CBGUID.ItemsSource = searchWindowViewModel.SearchItems.Select(it => it.GUID).Distinct();
            CBClassElement.ItemsSource = searchWindowViewModel.SearchItems.Select(it => it.IFCClass).Distinct();
            CBNameElement.ItemsSource = searchWindowViewModel.SearchItems.Select(it => it.Name).Distinct();
            CBPropertySet.ItemsSource = searchWindowViewModel.SearchItems.SelectMany(it => it.PropertySetCollection).Select(it => it.Name).Distinct();
            CBPropertyName.ItemsSource = searchWindowViewModel.SearchItems.SelectMany(it => it.PropertiesName).Cast<string>().Distinct();
            CBPropertyValue.ItemsSource = searchWindowViewModel.SearchItems.SelectMany(it => it.Values).Cast<string>().Distinct();
        
        }



        void OnComboboxTextChanged(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
