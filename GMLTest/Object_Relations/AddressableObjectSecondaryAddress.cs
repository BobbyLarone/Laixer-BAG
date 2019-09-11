using LaixerGMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.Object_Relations
{
    /// <summary>
    /// Verblijfs object neven addres
    /// </summary>
    class AddressableObjectSecondaryAddress : BAGRelation
    {
        public AddressableObjectSecondaryAddress() : base("", "adresseerbaarobjectnevenadres", "")
        {
            Add(new BAGstringAttribute(16, "nevenadres", "bag_LVC:nevenadres"));
        }
    }
}
