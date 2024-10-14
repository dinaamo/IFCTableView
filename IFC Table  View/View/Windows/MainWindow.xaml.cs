using GeometryGym.Ifc;
using IFC_Table_View.IFC.ModelItem;
using IFC_Table_View.Infracrucrure.FindObjectException;
using IFC_Table_View.View.Windows;
using IFC_Table_View.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        /// <summary>
        /// Двойной клик на ссылке на таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    else if (textBlock.DataContext is ModelItemIFCObject findModelObject)
                    {
                        
                        try
                        {
                            IEnumerable<ModelItemIFCObject> secondLevelCollection = ((IModelItemIFC)treeViewIFC.Items[0]).ModelItems.
                                OfType<ModelItemIFCObject>();

                            foreach (ModelItemIFCObject modelObject in secondLevelCollection)
                            {
                                modelObject.IsExpanded = true;
                                FindTreeViewItem(modelObject, findModelObject);
                            } 

                            
                        }
                        catch (FindObjectException find)
                        {
                            //TreeViewItem treeViewItem = topElement.ItemContainerGenerator.ContainerFromItem(find) as TreeViewItem;
                            //targetElement.IsSelected = true;
                        }
                        
                    }
                }
            }
        }
        
        /// <summary>
        /// Ищем элемент в дереве
        /// </summary>
        /// <param name="topElement"></param>
        /// <param name="findObject"></param>
        /// <returns></returns>
        /// <exception cref="FindObjectException"></exception>
        private void FindTreeViewItem(ModelItemIFCObject topElement, ModelItemIFCObject findObject)
        {
            foreach (ModelItemIFCObject item in topElement.ModelItems)
            {
                //TreeViewItem treeViewItem = topElement.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (item == findObject)
                {
                    throw new FindObjectException(item);
                }
                else
                {
                    item.IsExpanded = true;
                    FindTreeViewItem(item, findObject);
                }
                
            }
        }

        /// <summary>
        /// Загрузка формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                    ObservableCollection<IModelItemIFC> collectionObjectModel = treeViewIFC.ItemsSource as ObservableCollection<IModelItemIFC>;

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
                if(textBlock.DataContext is ModelItemIFCObject findObject)
                {
                    findObject.IsFocusReference = true;
                }
                else if(textBlock.DataContext is KeyValuePair<string, IfcProperty> pairPropertyRefVal)
                {
                    IfcPropertyReferenceValue PropertyRefVa = pairPropertyRefVal.Value as IfcPropertyReferenceValue;

                    ObservableCollection<IModelItemIFC> collectionObjectModel = treeViewIFC.ItemsSource as ObservableCollection<IModelItemIFC>;

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

    }
}