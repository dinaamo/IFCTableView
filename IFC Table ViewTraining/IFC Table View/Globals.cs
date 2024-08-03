using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryGym.Ifc;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Edit_PropertyIFC4
{
    class Globals
    {
        public static string FilePath { get; set; }
        public static DatabaseIfc DataBase { get; set; }
        public static ObservableCollection<ModelItem> ModelItems { get; set; }


    }

    public enum FormatIFC
    {
        IFC,
        IFCXML
    }
}
