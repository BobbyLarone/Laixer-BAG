using LaixerGMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.BAG_Objects
{
    class BAGAddressableObject : BAGObject
    {
        public BAGAddressableObject(string tag, string name, string objectType)
            : base(tag,name,objectType)
        {
            Console.WriteLine("Created a new Adressable Object");
            Add(new BAGstringAttribute(16, "hoofdadres", "bag_LVC:gerelateerdeAdressen/bag_LVC:hoofdadres/bag_LVC:identificatie"));
            AddRelation(new BAGrelationAttribute(
                this, 
                "adresseerbaarobjectnevenadres",
                16, 
                "nevenadres",
                "bag_LVC:gerelateerdeAdressen/bag_LVC:nevenadres/bag_LVC:identificatie",
                new List<string>()
                {
                    "ligplaatsStatus", "standplaatsStatus", "verblijfsobjectStatus", "geom_valid"
                }));
        }
    }
}
