﻿using IFC_Table_View.IFC.ModelItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IFC_Table_View.Infracrucrure.FindObjectException
{
    internal class FindObjectException : Exception
    {
        public ModelItemIFCObject FindObject { get;set;}
        public FindObjectException(ModelItemIFCObject FindObject)
        {
            this.FindObject = FindObject;
        }
 
    }
}
