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
    /// Логика взаимодействия для WindowLoad.xaml
    /// </summary>
    public partial class LoadWindow : Window
    {
        public LoadWindow(string message)
        {
            InitializeComponent();
            DataContext = new LoadWindowViewModel(message);
        }

    }

    
}
