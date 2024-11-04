using GeometryGym.Ifc;
using IFC_Table_View.Data;
using IFC_Table_View.HelperIFC;
using IFC_Table_View.IFC.ModelIFC;
using IFC_Table_View.IFC.ModelItem;
using IFC_Table_View.Infracrucrure.Commands;
using IFC_Table_View.Infracrucrure.FindObjectException;
using IFC_Table_View.View.Windows;
using IFC_Table_View.ViewModels;
using IFC_Table_View.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Media3D;


namespace IFC_Table_View.ViewModels
{
    //public static class StartArguments
    //{
    //    public static string StartPath { get; set; }
    //}
    internal class MainWindowViewModel : BaseViewModel 
    {


        private ModelIFC _modelIFC;
        public ModelIFC modelIFC 
        { 
            get 
            { return _modelIFC; } 
            set 
            {
                Set(ref _modelIFC, value);
                Title = _modelIFC.FilePath;
                mainWindow.treeViewIFC.ItemsSource = modelIFC.ModelItems;
            } 
        }

        #region Заголовок
        private string _Title;

        ///<summary>Заголовок окна</summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        #region Выборка элементов
        private List<BaseModelItemIFC> SelectionElements(BaseModelItemIFC modelItem)
        {
            List< BaseModelItemIFC > list = new List< BaseModelItemIFC >();

            if (!(modelItem is ModelItemIFCFile) && !(modelItem is ModelItemIFCTable))
            {
                list.Add(modelItem);
            }

            if (modelItem.ModelItems != null)
            {
                foreach (BaseModelItemIFC nestModelItem in modelItem.ModelItems)
                {
                    list.AddRange(SelectionElements(nestModelItem));
                }
            }
            return list;

        }

        #endregion


        #region Дерево элементов
        private ObservableCollection<ModelItemIFCTable> _ModelItems;

        ///<summary>Дерево элементов</summary>
        public ObservableCollection<ModelItemIFCTable> ModelItems
        {
            get => _ModelItems;
            set => Set(ref _ModelItems, value);
        }
        #endregion

        #region Загрузка модели с анимацией

        void LoadModelAsync(string path)
        {
            if (path == null)
            {
                return;
            }

            mainWindow.IsEnabled = false;

            LoadWindow windowLoad = new LoadWindow("Загрузка");
            if (mainWindow.WindowState == WindowState.Maximized)
            {
                windowLoad.Left = (System.Windows.SystemParameters.PrimaryScreenWidth / 2) - (windowLoad.Width / 2);
                windowLoad.Top = (System.Windows.SystemParameters.PrimaryScreenHeight / 2) - (windowLoad.Height / 2);
            }
            else
            {
                windowLoad.Left = mainWindow.Left + (mainWindow.Width / 2) - (windowLoad.Width / 2);
                windowLoad.Top = mainWindow.Top + (mainWindow.Height / 2) - (windowLoad.Height / 2);
            }

            ModelIFC TempModel = new ModelIFC();
            using ManualResetEvent signal = new ManualResetEvent(false);

            Task.Run(() =>
            {

                TempModel.Load(path);
                signal.WaitOne();
                windowLoad.Dispatcher.BeginInvoke(() =>
                {
                    windowLoad.Close();
                });
                
            });

            signal.Set();
            windowLoad.ShowDialog();

            mainWindow.IsEnabled = true;
            if (TempModel != null)
            {
                modelIFC = TempModel;
                
            }
        }

        #endregion

     

        #region Комманды


        #region Открыть_файл
        public ICommand LoadApplicationCommand { get; }

        private void OnLoadApplicationCommandExecuted(object o)
        {
            LoadModelAsync(HelperFileIFC.OpenIFC_File());

            //Task.Run(() =>
            //{
            //    UpdateTreeView();
            //});
            
        }



        private bool CanLoadApplicationCommandExecute(object o)
        {
            return true;
        }

        #endregion

        #region Добавить_таблицу
        public ICommand AddIFCTableCommand { get; }

        private void OnAddIFCTableCommandExecuted(object o)
        {
            FormCreateTable tableForm = new FormCreateTable(modelIFC.CreateNewIFCTable);
            tableForm.ShowDialog();
        }




        private bool CanAddIFCTableCommandExecute(object o)
        {
            if (modelIFC != null)
            { 
                return true; 
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Сохранить_файл
        public ICommand SaveFileCommand { get; }

        private void OnSaveFileCommandExecuted(object o)
        {
            mainWindow.IsEnabled = false;

            LoadWindow windowLoad = new LoadWindow("Сохранение");
            if (mainWindow.WindowState == WindowState.Maximized)
            {
                windowLoad.Left = (System.Windows.SystemParameters.PrimaryScreenWidth / 2) - (windowLoad.Width / 2);
                windowLoad.Top = (System.Windows.SystemParameters.PrimaryScreenHeight / 2) - (windowLoad.Height / 2);
            }
            else
            {
                windowLoad.Left = mainWindow.Left + (mainWindow.Width / 2) - (windowLoad.Width / 2);
                windowLoad.Top = mainWindow.Top + (mainWindow.Height / 2) - (windowLoad.Height / 2);
            }

            using ManualResetEvent signal = new ManualResetEvent(false);

            Task.Run(() =>
            {

                modelIFC.SaveFile(modelIFC.FilePath);
                signal.WaitOne();
                windowLoad.Dispatcher.BeginInvoke(() =>
                {
                    windowLoad.Close();
                });

            });

            signal.Set();
            windowLoad.ShowDialog();

            mainWindow.IsEnabled = true;
            
        }

        private bool CanSaveFileCommandExecute(object o)
        {
            if (modelIFC != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Сохранить_файл_как_ifc
        public ICommand SaveAsFileCommand { get; }

        private void OnSaveAsIFCFileCommandExecuted(object o)
        {
            ModelIFC TempModel = modelIFC.SaveFile(HelperFileIFC.SaveAsIFC_File("ifc"));
            if (TempModel == null)
            {
                MessageBox.Show("Не удалось сохранить файл", 
                                "Внимание!!!", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                modelIFC = TempModel;
            }          
        }

        private bool CanSaveAsIFCFileCommandExecute(object o)
        {
            if (modelIFC != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Сохранить_файл_как_ifcxml
        public ICommand SaveAsIFCXMLFileCommand { get; }

        private void OnSaveAsIFCXMLFileCommandExecuted(object o)
        {
            ModelIFC TempModel = modelIFC.SaveXMLFile(HelperFileIFC.SaveAsIFC_File("ifcxml"));
            if (TempModel == null)
            {
                MessageBox.Show("Не удалось сохранить файл",
                                "Внимание!!!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                modelIFC = TempModel;
            }
        }

        private bool CanSaveAsIFCXMLFileCommandExecute(object o)
        {
            if (modelIFC != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Удалить_таблицу
        public ICommand DeleteTableCommand { get; }

        private void OnDeleteTableCommandExecuted(object o)
        {
            object element = ((ModelItemIFCTable)o)?.ItemTreeView;

            if (element is IfcTable Table)
            {
                modelIFC.DeleteTable(Table);
            }
        }

        private bool CanDeleteTableCommandExecute(object o)
        {
            if (modelIFC == null)
            {
                return false;
            }
            else if(((BaseModelItemIFC)o)?.ItemTreeView is IfcTable Table)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Добавить к элементу связь с таблицей 
        public ICommand AddReferenceToTheTable { get; }

        private void OnAddReferenceToTheTable(object o)
        {

            if (o is ModelItemIFCObject modelObject)
            {

                List<ModelItemIFCTable> collectionModelTable = mainWindow.treeViewIFC.ItemsSource.
                                                Cast<BaseModelItemIFC>().
                                                ToList()[0].
                                                ModelItems.
                                                OfType<ModelItemIFCTable>().
                                                ToList();


                Form_Add_Reference_To_Table form_Add_Reference_To_Table = new Form_Add_Reference_To_Table(modelObject, collectionModelTable);

                form_Add_Reference_To_Table.ShowDialog();

                modelObject.AddReferenceTable(form_Add_Reference_To_Table.TableNameCollection);

            }
        }

        private bool CanAddReferenceToTheTable(object o)
        {
            if (o is ModelItemIFCObject obj)
            {
                if (obj.ItemTreeView is IfcObject)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Удалить ссылку на таблицу
        public ICommand DeleteReferenceToTheTable { get; }

        private void OnDeleteReferenceToTheTable(object o)
        {

            if (o is ModelItemIFCObject modelObject)
            {
                Form_Delete_Reference_To_Table form_Delete_Reference_To_Table = new Form_Delete_Reference_To_Table(modelObject);

                form_Delete_Reference_To_Table.ShowDialog();

                List<ModelItemIFCTable> collectionModelTable = mainWindow.treeViewIFC.ItemsSource.
                                                Cast<BaseModelItemIFC>().
                                                ToList()[0].
                                                ModelItems.
                                                OfType<ModelItemIFCTable>().
                                                ToList();

                modelObject.DeleteReferenceTable(form_Delete_Reference_To_Table.ifcPropertyReferenceValueDictionaryToDelete, collectionModelTable);

            }
        }

        private bool CanDeleteReferenceToTheTable(object o)
        {
            //if (modelIFC == null)
            //{
            //    return false;
            //}
            //else if (((IModelItemIFC)o) is ModelItemIFCObject obj)
            if (o is ModelItemIFCObject obj)
            {
                if (obj.ItemTreeView is IfcObject)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Открыть справку
        public ICommand OpenHelp { get; }

        private void OnOpenHelpCommandExecuted(object o)
        {
            string fileHelpPath = "IFC Table View.chm";
            if(System.IO.File.Exists(fileHelpPath))
            {
                Process.Start(fileHelpPath);

            }
        }

        private bool CanOpenHelpCommandExecute(object o)
        {
            return true;
        }
        #endregion

        #region Действие с раскрывающимися списками
        public ICommand ActionExpanders { get; }

        private void OnActionExpandedCommandExecuted(object o)
        {
            if (IsExpandedPropertySet)
            {
                IsExpandedPropertySet = false;
            }
            else
            {
                IsExpandedPropertySet = true;
            }
        }

        private bool _IsExpandedPropertySet { get; set; }
        public bool IsExpandedPropertySet
        {
            get { return _IsExpandedPropertySet; }
            set
            {
                _IsExpandedPropertySet = value;
                OnPropertyChanged("IsExpandedPropertySet");
            }
        }


        private bool CanActionExpandedCommandExecute(object o)
        {
            return true;
        }
        #endregion

        #region Поиск элементов
        public ICommand SearchElements { get; }

        private void OnSearchElementsCommandExecuted(object o)
        {
            if (o is BaseModelItemIFC modelItem)
            {
                new SearchWindow(SelectionElements(modelItem)).Show();
            }   
        }


        private bool CanSearchElementsCommandExecute(object o)
        {
            if (o is BaseModelItemIFC)
            {
                return true;
            }
            else
            {
                return false; 
            }
        }
        #endregion

        #region Сброс поиска
        public ICommand ResetSearch { get; }

        private void OnResetSearchCommandExecuted(object o)
        {

        }


        private bool CanResetSearchCommandExecute(object o)
        {
            if (o is BaseModelItemIFC)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        #endregion

        public MainWindowViewModel()
        {

        }

        private readonly MainWindow mainWindow;
        public MainWindowViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            var strArray = System.Environment.GetCommandLineArgs();
            if (strArray.Length > 1)
            {
                //MessageBox.Show(strArray[1]);
                LoadModelAsync(Environment.GetCommandLineArgs()[1]);
            }

            #region Комманды
            LoadApplicationCommand = new ActionCommand(
                OnLoadApplicationCommandExecuted,
                CanLoadApplicationCommandExecute);

            AddIFCTableCommand = new ActionCommand(
                OnAddIFCTableCommandExecuted,
                CanAddIFCTableCommandExecute);

            DeleteTableCommand = new ActionCommand(
                OnDeleteTableCommandExecuted,
                CanDeleteTableCommandExecute);

            SaveFileCommand = new ActionCommand(
                OnSaveFileCommandExecuted,
                CanSaveFileCommandExecute);

            SaveAsFileCommand = new ActionCommand(
                OnSaveAsIFCFileCommandExecuted,
                CanSaveAsIFCFileCommandExecute);

            SaveAsIFCXMLFileCommand = new ActionCommand(
                OnSaveAsIFCXMLFileCommandExecuted,
                CanSaveAsIFCXMLFileCommandExecute);

            AddReferenceToTheTable = new ActionCommand(
                OnAddReferenceToTheTable,
                CanAddReferenceToTheTable);

            DeleteReferenceToTheTable = new ActionCommand(
                OnDeleteReferenceToTheTable,
                CanDeleteReferenceToTheTable);

            OpenHelp = new ActionCommand(
                OnOpenHelpCommandExecuted,
                CanOpenHelpCommandExecute);

            ActionExpanders = new ActionCommand(
                OnActionExpandedCommandExecuted,
                CanActionExpandedCommandExecute);

            SearchElements = new ActionCommand(
                OnSearchElementsCommandExecuted,
                CanSearchElementsCommandExecute);

            ResetSearch = new ActionCommand(
                OnResetSearchCommandExecuted,
                CanResetSearchCommandExecute);

            #endregion
        }

    }
}
