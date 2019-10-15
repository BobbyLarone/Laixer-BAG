using LaixerGMLTest.BAG_Attributes;

namespace LaixerGMLTest.BAG_Objects
{
    internal class BAGAddressableObject : BAGObject
    {
        public string Hoofdadres => GetAttribute("hoofdadres").GetValue();

        public BAGAddressableObject(string tag, string name, string objectType)
            : base(tag, name, objectType)
        {
            Add(new BAGstringAttribute(16, "hoofdadres", "bag_LVC:gerelateerdeAdressen/bag_LVC:hoofdadres/bag_LVC:identificatie"));
            Add(new BAGstringAttribute(16, "nevenadres", "bag_LVC:nevenadres/bag_LVC:identificatie"));
        }
    }
}
