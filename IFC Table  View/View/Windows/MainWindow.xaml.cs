using GeometryGym.Ifc;
using IFC_Table_View.IFC.ModelItem;
using IFC_Table_View.Infracrucrure.FindObjectException;
using IFC_Table_View.View.Windows;
using IFC_Table_View.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

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

        /// Двойной клик
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
                                new TableWindow(ifcTable).ShowDialog();
                            }
                        }
                    }
                    else if (textBlock.DataContext is ModelItemIFCObject searchModelObject)
                    {
                        try
                        {
                            IEnumerable<ModelItemIFCObject> secondLevelCollection = ((BaseModelItemIFC)treeViewIFC.Items[0]).ModelItems.
                                OfType<ModelItemIFCObject>();

                            foreach (ModelItemIFCObject modelObject in secondLevelCollection)
                            {
                                modelObject.IsExpanded = true;
                                ModelItemIFCObject.FindSingleTreeObject(modelObject, searchModelObject);
                            }
                        }
                        catch (FindObjectException findObj)
                        {
                            findObj.FindObject.IsSelected = true;
                            findObj.FindObject.IsFocusReference = false;
                        }

                    }
                }
            }
        }



        /// Загрузка формы
        private void MainWindowIFC_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new MainWindowViewModel(this);

        }
        
        //Обработка события потери фокуса мыши
        private void IsMouseLostFocus(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                ModelItemIFCObject findObject = textBlock.DataContext as ModelItemIFCObject;

                if (findObject != null)
                {
                    findObject.IsFocusReference = false;
                }
                else if (textBlock.DataContext is KeyValuePair<string, IfcProperty> pairPropertyRefVal)
                {
                    IfcPropertyReferenceValue PropertyRefVa = pairPropertyRefVal.Value as IfcPropertyReferenceValue;

                    ObservableCollection<BaseModelItemIFC> collectionObjectModel = treeViewIFC.ItemsSource as ObservableCollection<BaseModelItemIFC>;

                    ModelItemIFCTable targetModelTable = collectionObjectModel[0].ModelItems.
                                                                    OfType<ModelItemIFCTable>().
                                                                    FirstOrDefault(it => it.ItemTreeView == PropertyRefVa?.PropertyReference);

                    if (targetModelTable != null)
                    {
                        targetModelTable.IsFocusReference = false;
                    }
                }
            }
        }

        //Обработка события получения фокуса мыши
        private void IsMouseFocus(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                if (textBlock.DataContext is ModelItemIFCObject findObject)
                {
                    findObject.IsFocusReference = true;
                }
                else if (textBlock.DataContext is KeyValuePair<string, IfcProperty> pairPropertyRefVal)
                {
                    IfcPropertyReferenceValue PropertyRefVa = pairPropertyRefVal.Value as IfcPropertyReferenceValue;

                    ObservableCollection<BaseModelItemIFC> collectionObjectModel = treeViewIFC.ItemsSource as ObservableCollection<BaseModelItemIFC>;

                    ModelItemIFCTable targetModelTable = collectionObjectModel[0].ModelItems.
                                                                    OfType<ModelItemIFCTable>().
                                                                    FirstOrDefault(it => it.ItemTreeView == PropertyRefVa?.PropertyReference);

                    if (targetModelTable != null)
                    {
                        targetModelTable.IsFocusReference = true;
                    }
                }

            }
        }

        //Установка стиля для колонок dataGrid
        private void SetColumnStyle()
        {
            Style columnStyle = new Style(typeof(TextBlock));
            columnStyle.Setters.Add(new Setter(
                                                TextBlock.TextWrappingProperty,
                                                TextWrapping.Wrap
                                                ));
            foreach (DataGridTextColumn column in dgTable.Columns)
            {
                column.ElementStyle = columnStyle;
            }
        }
   
        //Подбор содержимого колонок по контексту
        private void ResizeColumns()
        {
            foreach (var column in dgTable.Columns)
            {
                column.Width = DataGridLength.SizeToHeader;
                double sizeToHeader = column.Width.DesiredValue;

                column.Width = DataGridLength.SizeToCells;
                double sizeToCells = column.Width.DesiredValue;

                if (sizeToHeader > sizeToCells || sizeToCells < 50 || sizeToCells > 300)
                {
                    column.Width = DataGridLength.SizeToHeader;
                }
                else if (sizeToHeader < sizeToCells || sizeToHeader < 50 || sizeToHeader > 300)
                {
                    column.Width = DataGridLength.SizeToCells;
                }
                else
                {
                    column.Width = new DataGridLength(dgTable.Width / dgTable.Columns.Count, DataGridLengthUnitType.Auto);
                }
            }
        }


        //Событие загрузка датагрид
        private void dgTable_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            ResizeColumns();
            SetColumnStyle();
        }

    }
}