using LaixerGMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.Object_Relations
{
    /// <summary>
    /// In Dutch : Verblijfsobject-pand relatie
    /// </summary>
    class AccomodationPremiseRelation : BAGRelation 
    {
        public AccomodationPremiseRelation() : base("", "verblijfsobjectpand", "")
        {
            Add(new BAGstringAttribute(16, "gerelateerdpand", "bag_LVC:gerelateerdPand"));
        }
    }
}
