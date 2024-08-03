using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GeometryGym.Ifc;


namespace Edit_PropertyIFC4
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Open_File_Click(object sender, RoutedEventArgs e)
        {
            if (!HelperIFC.Load())
            {
                treeView.ItemsSource = null;
                treeView.Items.Clear();

                dgPropertySet.ItemsSource = null;
                dgPropertySet.Items.Clear();
                dgTable.Items.Clear();
                dgProperty.Items.Clear();
                return;
            }

            WindowIFC.Title = Globals.FilePath;
            treeView.ItemsSource = Globals.ModelItems;
            
            Globals.DataBase = new DatabaseIfc(Globals.FilePath);

            if (Globals.DataBase != null)
            {
                Add_Table1.IsEnabled = true;
                Delete_Table.IsEnabled = true;
            }
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

            if (treeView.ItemsSource == null) return;
            //FillTreeView(((ModelItem)treeView.SelectedItem).CollectionPropertySet);

            dgPropertySet.ItemsSource = ((ModelItem)treeView.SelectedItem).CollectionPropertySet;

            tableName.Text = null;
            dgTable.DataContext = null;

            FillDataGridTable(((ModelItem)treeView.SelectedItem).Table);
        }

        //void FillTreeView(IEnumerable<IfcPropertySet> collPropSet)
        //{



        //    //treeViewPrSet.Items.Clear();

        //    if (collPropSet == null)
        //    {
        //        return;
        //    }


        //    TreeList treeList = new TreeList();

        //    //treeList.Items.Clear();


        //    foreach (IfcPropertySet propSet in collPropSet)
        //    {

        //        var treeListitem = new TreeListItem();
                

        //        //var treeItem = new TreeViewItem()
        //        //{
        //        //    Header = propSet.Name
        //        //};



        //        foreach (KeyValuePair<string, IfcProperty> prop in propSet.HasProperties)
        //        {
        //            var value = prop.Value as IfcPropertySingleValue;

        //            //if (value != null)
        //            //{
        //            //    treeItem.Items.Add(new TreeViewItem()
        //            //    {
        //            //        Header = prop.Key + " <-::-> " +value.NominalValue?.ValueString
        //            //    });
        //            //}



        //        }
        //        //treeViewPrSet.Items.Add(treeItem);
        //    }
        //}

        void FillDataGridTable(IfcTable Table)
        {
            
            DataTable dt = new DataTable();
            if (Table != null)
            {
                if (Table.Rows.Count == 0)
                {
                    return;
                }
              tableName.Text = Table.Name;
              for (int i = 0; i < Table.Rows[0].RowCells.Count(); i++)
                {
                    string nameColumn = Table.Rows[0].RowCells[i].Value.ToString();
                    ReplacingCharacters(ref nameColumn);

                    dt.Columns.Add(nameColumn);
                }

                for (int i = 1; i < Table.Rows.Count; i++)
                {
                    DataRow row = dt.NewRow();

                    for (int j = 0; j < Table.Rows[i].RowCells.Count(); j++)
                    {
                        row[dt.Columns[j].ColumnName] = Table.Rows[i].RowCells[j].Value.ToString();
                    }
                    dt.Rows.Add(row);
                }
                dgTable.DataContext = dt;


                foreach (DataGridColumn column in dgTable.Columns)
                {
                    //column.MinWidth = column.ActualWidth;
                    column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                }
            }
        }

        private void ReplacingCharacters(ref string nameAssembl)
        {
            string characters = @"\./:{}[]|;<>?'~";

            for (int i = 0; i < characters.Length; i++)
            {
                if (nameAssembl.Contains(characters[i]))
                {
                    nameAssembl = nameAssembl.Replace(characters[i], '_');

                }
            }

            nameAssembl = nameAssembl.Replace("__", "_");

            if (nameAssembl[nameAssembl.Length - 1] == '_')
            {
                nameAssembl = nameAssembl.Remove(nameAssembl.Length - 1);
            }
        }

        private void dg_SelectedItemChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            dgProperty.ItemsSource = ((GeometryGym.Ifc.IfcPropertySet)dgPropertySet?.SelectedItem)?.HasProperties.Select(it => it.Value);
        }


        private void Add_Table_Click(object sender, RoutedEventArgs e)
        {
            FormAddTable NewTable = new FormAddTable();

            NewTable.ShowDialog();
        }

        private void Delete_Table_Click(object sender, RoutedEventArgs e)
        {
            HelperIFC.DeleteTable((ModelItem)treeView.SelectedItem);
        }

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            HelperIFC.SaveIFCFile(Globals.FilePath);
            this.IsEnabled = true;
        }

        private void MenuItem_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            Globals.FilePath = FileIFC.SaveAsIFC_File(); 
            if (Globals.FilePath == null)
            {
                return;
            }

            HelperIFC.SaveIFCFile(Globals.FilePath);
            this.IsEnabled = true;
            WindowIFC.Title = Globals.FilePath;
        }

        private void MenuItem_SaveAsIFCXML_Click(object sender, RoutedEventArgs e)
        {
            //this.IsEnabled = false;
            //Globals.FilePath = FileIFC.SaveAsIFCXML_File();
            //if (Globals.FilePath == null)
            //{
            //    return;
            //}
            //HelperIFC.SaveIFCFile(Globals.FilePath);
            //this.IsEnabled = true;
            //WindowIFC.Title = Globals.FilePath;
        }



    }

 
}
