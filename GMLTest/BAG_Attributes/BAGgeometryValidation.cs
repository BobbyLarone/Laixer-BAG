using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.BAG_Attributes
{
    class BAGgeometryValidation : BAGAttribute
    {
        private string nameGeometryAttribute;

        public BAGgeometryValidation(string name, string nameGeoAttribute ,int length = -1, string tag = "") 
            : base(length, name, tag)
        {
            nameGeometryAttribute = nameGeoAttribute;
        }

        public string GetType() => "";
    }
}
