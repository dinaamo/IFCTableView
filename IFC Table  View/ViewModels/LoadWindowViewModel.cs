using GeometryGym.Ifc;
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

namespace IFC_Table_View.ViewModels
{
    internal class LoadWindowViewModel : BaseViewModel
    {

        public string Message { get; set; }

        public LoadWindowViewModel()
        {

        }

        //public LoadWindowViewModel(string message)
        //{
        //    Message = message;
        //}
    }
}
