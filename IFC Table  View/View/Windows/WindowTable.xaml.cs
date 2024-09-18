using GeometryGym.Ifc;
using IFC_Table_View.IFC.ModelItem;
using System.Data;
using System.Windows;


namespace IFC_Table_View.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowTable.xaml
    /// </summary>
    public partial class WindowTable : Window
    {
        DataTable dataTable;
        public WindowTable(IfcTable ifcTable)
        {
            InitializeComponent();

            dataTable = ModelItemIFCTable.FiilDataTable(ifcTable);
            DataContext = dataTable;

            MaxWidth = SystemParameters.PrimaryScreenWidth;
            MaxHeight = SystemParameters.PrimaryScreenHeight;
        }
    }
}
