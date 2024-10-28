using GeometryGym.Ifc;
using IFC_Table_View.IFC.ModelItem;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;


namespace IFC_Table_View.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowTable.xaml
    /// </summary>
    public partial class WindowTable : Window, INotifyPropertyChanged
    {
        public DataTable dataTable {get; set;}
        public WindowTable(IfcTable ifcTable)
        {
            InitializeComponent();

            dataTable = ModelItemIFCTable.FillDataTable(ifcTable);
            //DataContext = dataTable;

            MaxWidth = SystemParameters.PrimaryScreenWidth;
            MaxHeight = SystemParameters.PrimaryScreenHeight;

            
        }


        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Событие изменения элемента
        /// </summary>
        /// <param name = "PropertyName" ></ param >
        protected virtual void OnPropertyChanged(string PropertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }


        private int _FontSizeTable { get; set; } = 12;

        public int FontSizeTable
        {
            get { return _FontSizeTable; }
            set
            {
                if (value < 10)
                {
                    _FontSizeTable = 10;
                }
                else if (value > 20)
                {
                    _FontSizeTable = 20;
                }
                else
                {
                    _FontSizeTable = value;
                }

                OnPropertyChanged("FontSizeTable");
            }
        }

        private void ResizeColumns()
        {
            foreach (DataGridColumn column in dgTable.Columns)
            {
                //column.CellStyle = new Style();
            }
        }


        private void Button_More_Click(object sender, RoutedEventArgs e)
        {
            ++FontSizeTable;
            ResizeColumns();
        }

        private void Button_Less_Click(object sender, RoutedEventArgs e)
        {
            --FontSizeTable;
            ResizeColumns();
        }

        //private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        //{
        //    Style style = new Style(typeof(DataGridCell));
        //    style.Setters.Add(new Setter(DataGridCell.ContentTemplateProperty, Resources["templ"]));
        //    e.Column.CellStyle = style;
        //}
    }
}
