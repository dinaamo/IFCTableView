using System;

namespace IFC_Table_View.Infracrucrure
{
    public class PropertyReferenceChangedEventArg : EventArgs
    {
        public bool IsContainPropertyDownTreeReference { get; set; }

        //public bool IsContainPropertyReferenceDownTree { get; set; }

        //public bool IsNotContainAnyReferenceProperty { get; set; }



        public PropertyReferenceChangedEventArg(bool IsContainPropertyDownTreeReference)
        {
            this.IsContainPropertyDownTreeReference = IsContainPropertyDownTreeReference;
            //this.IsNotContainAnyReferenceProperty = isNotContainAnyReferenceProperty;    

        }
    }
}