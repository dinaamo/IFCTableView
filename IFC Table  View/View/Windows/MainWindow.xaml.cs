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


        //bool mSuppressRequestBringIntoView;
        //private void TreeViewItem_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        //{

        //    // Ignore re-entrant calls
        //    if (mSuppressRequestBringIntoView)
        //        return;

        //    // Cancel the current scroll attempt
        //    e.Handled = true;

        //    //Вызовите BringIntoView, используя прямоугольник, который расширяется в "отрицательное пространство" слева от нашего 
        //    // фактического элемента управления.
        //    // Это позволяет вертикальной прокрутке работать без негативного 
        //    // влияния на текущую горизонтальную позицию прокрутки.
        //    mSuppressRequestBringIntoView = true;

        //    TreeViewItem tvi = sender as TreeViewItem;
        //    if (tvi != null)
        //    {
        //        Rect newTargetRect = new Rect(-1000, 0, tvi.ActualWidth + 1000, tvi.ActualHeight);
        //        tvi.BringIntoView(newTargetRect);
        //    }

        //    mSuppressRequestBringIntoView = false;
        //}








        //Обработка события наведения мыши
        //private void TextBlock_IsMouseDirectlyOverChanged(object sender, MouseEventArgs e)
        //{
        //    if (sender is TextBlock textBlock)
        //    {
        //        //Находим искомый элемент
        //        ModelItemIFCObject findObject = textBlock.DataContext as ModelItemIFCObject;

        //        findObject.IsFocusReference = true;

        //        //Находим корневой элемент
        //        TreeViewItem topElement = treeViewIFC.ItemContainerGenerator.ContainerFromItem(treeViewIFC.Items[0]) as TreeViewItem;

        //        try
        //        {
        //            FindTreeViewItem(topElement, findObject);
        //        }
        //        catch (FindObjectException findExeption)
        //        {


        //        }

        //    }
        //}


        #region FINDElEMENT

        //private TreeViewItem FindTreeViewItem(TreeViewItem topElement, ModelItemIFCObject findObject)
        //{


        //    foreach (object item in topElement.Items)
        //    {
        //        DependencyObject dependencyObject = topElement.ItemContainerGenerator.ContainerFromItem(item);

        //        if (dependencyObject is TreeViewItem treeViewItem)
        //        {
        //            if (item == findObject)
        //            {
        //                treeViewItem.IsSelected = true;
        //                throw new FindObjectException(findObject);
        //            }
        //            else
        //            {
        //                FindTreeViewItem(treeViewItem, findObject);
        //            }
        //        }
        //    }
        //    return null;
        //}



        #endregion


    }
}