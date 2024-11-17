using GeometryGym.Ifc;
using IFC_Table_View.HelperExcel;
using IFC_Table_View.IFC.ModelItem;
using IFC_Table_View.ViewModels;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace IFC_Table_View.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowTable.xaml
    /// </summary>
    public partial class TableWindow : Window
    {
       

        public TableWindow(DataTable dataTable)
        {
            InitializeComponent();

            DataContext = new TableWindowViewModel(dataTable);

            MaxWidth = SystemParameters.PrimaryScreenWidth;
            MaxHeight = SystemParameters.PrimaryScreenHeight;
        }


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
                    column.Width = new DataGridLength(dgTable.Width/ dgTable.Columns.Count, DataGridLengthUnitType.Auto);
                }
            }   
        }

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

        int countItems = 0;
        private void dgTable_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            
            DataGrid dataGrid = (DataGrid)sender;

            if (countItems < dataGrid.Items.Count)
            {
                ++countItems;
            }


            if (countItems == dataGrid.Items.Count)
            {
                ResizeColumns();
                SetColumnStyle();
                countItems = 0;
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResizeColumns();
        }
    }
}
