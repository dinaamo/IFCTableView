﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IFC_Table_View.IFC.ModelItem
{
    public interface IModelItemIFC
    {
        object ItemTreeView { get; }

        Dictionary<string, HashSet<string>> PropertyElement { get; }

        ObservableCollection<IModelItemIFC> ModelItems { get; }
    }
}