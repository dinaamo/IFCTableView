﻿using GeometryGym.Ifc;
using IFC_Table_View.Data;
using IFC_Table_View.HelperIFC;
using IFC_Table_View.IFC.Model;
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
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace IFC_Table_View.ViewModels
{
    internal class TableWindowViewModel : BaseViewModel
    {

        public DataTable dataTable {get; set;}

        public string[] СonditionsSearch { get; private set; } = { "Равно", "Не равно", "Содержит", "Не содержит" };

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

        #region Поиск
        public ICommand SearchCellsCommand { get; }

        private void OnSearchCellsCommandExecuted(object o)
        {
            object[] ControlArray = (object[])o;

            DataGrid dataGrid = ControlArray[0] as DataGrid;
            bool isFullText = (bool)ControlArray[1];
            bool isIgnorRegister = (bool)ControlArray[2];
            string seachString = (string)ControlArray[3];


            if (seachString.Equals(string.Empty)) { return; }

            int CountFound = 0;

            foreach (DataRowView rowView in dataGrid.Items)
            {
                DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromItem(rowView) as DataGridRow;
                
                foreach (var column in dataGrid.Columns)
                {
                    TextBlock cell = column.GetCellContent(row) as TextBlock;
                    string cellstring = cell.Text;

                    if (!isIgnorRegister)
                    {
                        cellstring = cellstring.ToLower();
                        seachString = seachString.ToLower();
                    }

                    if (isFullText)
                    {
                        if(cellstring.Equals(seachString))
                        {
                            cell.Background = Brushes.Tomato;
                            ++CountFound;
                        }
                    }
                    else
                    {
                        if (cellstring.Contains(seachString))
                        {
                            cell.Background = Brushes.Tomato;
                            ++CountFound;
                        }
                    }
                }
            }
            ((TextBlock)ControlArray[4]).Text = $"Найдено ячеек: {CountFound}";
        }


        private bool CanSearchCellsCommandExecute(object o)
        { 
            return true;
        }
        #endregion

        #region Сброс
        public ICommand ResetSearchCommand { get; }

        private void OnResetSearchCommandExecuted(object o)
        {
            object[] ControlArray = (object[])o;

            DataGrid dataGrid = ControlArray[0] as DataGrid;
            ((CheckBox)ControlArray[1]).IsChecked = false;
            ((CheckBox)ControlArray[2]).IsChecked = false;
            ((TextBox)ControlArray[3]).Text = string.Empty;

            foreach (DataRowView rowView in dataGrid.Items)
            {
                DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromItem(rowView) as DataGridRow;

                foreach (var column in dataGrid.Columns)
                {
                    TextBlock cell = column.GetCellContent(row) as TextBlock;
                    
                    cell.Background = null;
                }

            }

            ((TextBlock)ControlArray[4]).Text = string.Empty;
        }


        private bool CanResetSearchCommandExecute(object o)
        {
            return true;
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


        public TableWindowViewModel(DataTable dataTable)
        {
            this.dataTable = dataTable;

            Title = this.dataTable.TableName;

            #region Комманды

            MoreSizeFontCommand = new ActionCommand(
                OnMoreSizeFontCommandExecuted,
                CanMoreSizeFontCommandExecute);

            LessSizeFontCommand = new ActionCommand(
                OnLessSizeFontCommandExecuted,
                CanLessSizeFontCommandExecute);

            SearchCellsCommand = new ActionCommand(
                OnSearchCellsCommandExecuted,
                CanSearchCellsCommandExecute);

            ResetSearchCommand = new ActionCommand(
                OnResetSearchCommandExecuted,
                CanResetSearchCommandExecute);

            ExportToExcelCommand = new ActionCommand(
                OnExportToExcelCommandExecuted,
                CanExportToExcelCommandExecute);

            #endregion


        }
    }
}
