using GeometryGym.Ifc;
using IFC_Table_View.Data;
using IFC_Table_View.HelperIFC;
using IFC_Table_View.IFC.ModelIFC;
using IFC_Table_View.IFC.ModelItem;
using IFC_Table_View.Infracrucrure.Commands;
using IFC_Table_View.View.Windows;
using IFC_Table_View.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Data;
using IFC_Table_View.HelperExcel;

namespace IFC_Table_View.ViewModels
{
    internal class TableWindowViewModel : BaseViewModel
    {

        public DataTable dataTable {get; set;}
            
        #region Заголовок
        private string _Title;

        ///<summary>Заголовок окна</summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        private int _FontSizeTable  = 12;

        public int FontSizeTable
        {
            get => _FontSizeTable;
            set 
            {
                if (value < 10)
                {
                    Set(ref _FontSizeTable, 10);
                }
                else if (value > 30)
                {
                    Set(ref _FontSizeTable, 30);
                }
                else
                {
                    Set(ref _FontSizeTable, value);
                }
            }
        }


        #region Комманды


        #region Шрифт больше
        public ICommand MoreSizeFontCommand { get; }

        private void OnMoreSizeFontCommandExecuted(object o)
        {
            ++FontSizeTable;
        }


        private bool CanMoreSizeFontCommandExecute(object o)
        {
            if (FontSizeTable >= 30)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Шрифт меньше
        public ICommand LessSizeFontCommand { get; }

        private void OnLessSizeFontCommandExecuted(object o)
        {
            --FontSizeTable;
        }


        private bool CanLessSizeFontCommandExecute(object o)
        {
            if (FontSizeTable <= 10)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Экспорт в Excel
        public ICommand ExportToExcelCommand { get; }

        private void OnExportToExcelCommandExecuted(object o)
        {
            using (ExcelHelper excel = new ExcelHelper())
            {
                try
                {
                    excel.WriteData(dataTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private bool CanExportToExcelCommandExecute(object o)
        {
            return true;
        }
        #endregion

        #endregion


        public TableWindowViewModel()
        {

        }


        public TableWindowViewModel(IfcTable ifcTable)
        {
            dataTable = ModelItemIFCTable.FillDataTable(ifcTable);

            Title= dataTable.TableName;

            #region Комманды

            MoreSizeFontCommand = new ActionCommand(
                OnMoreSizeFontCommandExecuted,
                CanMoreSizeFontCommandExecute);

            LessSizeFontCommand = new ActionCommand(
                OnLessSizeFontCommandExecuted,
                CanLessSizeFontCommandExecute);

            ExportToExcelCommand = new ActionCommand(
                OnExportToExcelCommandExecuted,
                CanExportToExcelCommandExecute);

            #endregion


        }
    }
}
