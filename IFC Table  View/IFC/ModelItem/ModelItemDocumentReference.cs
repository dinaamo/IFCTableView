using GeometryGym.Ifc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using IFC_Table_View.HelperIFC;

using System.Collections;
using Microsoft.Win32;
using System.Reflection;
using System.Xml.Linq;
using System.ComponentModel;
using System.Windows;
using System.Text.RegularExpressions;
using IFC_Table_View.IFC.Model;
using System.Windows.Input;
using IFC_Table_View.Infracrucrure.Commands;

namespace IFC_Table_View.IFC.ModelItem
{
    public class ModelItemDocumentReference : BaseModelReferenceIFC
    {
        public ModelItemDocumentReference(IfcDocumentReference documentReference, ModelIFC modelIFC) : base(documentReference, modelIFC)
        {
            this.documentReference = documentReference;
        }

        private IfcDocumentReference documentReference;

        public override string NameReference => documentReference.Name;

       
    }
}
