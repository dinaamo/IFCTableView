using IFC_Table_View.IFC.ModelItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFC_Table_View.Infracrucrure.FindObjectException
{
    internal class FindObjectException : Exception
    {
        public ModelItemIFCObject findObject;
        public FindObjectException(ModelItemIFCObject findObject)
        {
            this.findObject = findObject;
        }
 
    }
}
