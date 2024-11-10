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
        private static SearchWindow instance;

        public static void CreateWindowSearch(IEnumerable<ModelItemIFCObject> ModelElementsForSearch)
        {
            if(instance == null)
            {
                instance = new SearchWindow(ModelElementsForSearch);
                instance.Show();
            }
            else
            {
                return;
            }
        }
        private SearchWindow(IEnumerable<ModelItemIFCObject> ModelElementsForSearch)
        {
            InitializeComponent();

            SearchWindowViewModel searchWindowViewModel = new SearchWindowViewModel(ModelElementsForSearch);

            DataContext = searchWindowViewModel;

            CBGUIDValue.ItemsSource = searchWindowViewModel.SearchItems.Select(it => it.GUID).Distinct();
            CBClassElementValue.ItemsSource = searchWindowViewModel.SearchItems.Select(it => it.IFCClass).Distinct();
            CBNameElementValue.ItemsSource = searchWindowViewModel.SearchItems.Select(it => it.Name).Distinct();
            CBPropertySetValue.ItemsSource = searchWindowViewModel.SearchItems.Where(it => it.PropertySetCollection != null).SelectMany(it => it.PropertySetCollection).Select(it => it?.Name).Distinct();
            CBPropertyNameValue.ItemsSource = searchWindowViewModel.SearchItems.Where(it => it.PropertiesName != null).SelectMany(it => it.PropertiesName).Cast<string>().Distinct();
            CBPropertyValue.ItemsSource = searchWindowViewModel.SearchItems.Where(it => it.Values != null).SelectMany(it => it.Values).Cast<string>().Distinct();
        }

        

        void OnComboboxTextChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            instance = null;
        }
    }
}
