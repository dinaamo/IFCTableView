﻿using GeometryGym.Ifc;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IFC_Table_View.Infracrucrure.Commands.Base
{
    public interface IModelItem
    {
        IEnumerable<IfcPropertySet> CollectionPropertySet { get; }
        string GUID { get; }
        string ItemTreeView { get; }
        string IFCType { get; }
    }

    internal class ModelItem
    {
        private ObservableCollection<ModelItem> ModelItems { get; set; }
    }
}