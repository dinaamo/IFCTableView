using GeometryGym.Ifc;
using IFC_Table_View.View.Windows;
using IFC_Table_View.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IFC_Table_View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
        }

        

        public void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                if (sender is TextBlock textBlock)
                {
                    if (textBlock.DataContext is KeyValuePair<string, IfcProperty> property)
                    {
                        if (property.Value is IfcPropertyReferenceValue propertyRefVal)
                        {
                            if (propertyRefVal.PropertyReference is IfcTable ifcTable)
                            {
                                new WindowTable(ifcTable).ShowDialog();
                            }
                        }
                    }
                }
            }
        }

        private void MainWindowIFC_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new MainWindowViewModel(this);
        }
    }
}