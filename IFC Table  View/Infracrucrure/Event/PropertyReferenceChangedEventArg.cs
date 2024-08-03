using System;

namespace IFC_Table_View.Infracrucrure
{
    public class PropertyReferenceChangedEventArg : EventArgs
    {
        public bool IsContainPropertyReference;

        public PropertyReferenceChangedEventArg(bool IsContainPropertyReference)
        { this.IsContainPropertyReference = IsContainPropertyReference; }
    }
}