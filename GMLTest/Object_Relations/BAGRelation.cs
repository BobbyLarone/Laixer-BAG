using LaixerGMLTest.BAG_Attributes;
using LaixerGMLTest.BAG_Objects;
using System.Collections.Generic;

namespace LaixerGMLTest.Object_Relations
{
    /// <summary>
    /// Relation from BAG to BAG objects
    /// </summary>
    internal class BAGRelation : BAGObject
    {
        private readonly List<BAGAttribute> attributeList = new List<BAGAttribute>();
        private readonly Dictionary<string, BAGAttribute> dictionaryBAGObjects = new Dictionary<string, BAGAttribute>();
        private readonly List<BAGrelationAttribute> relations = new List<BAGrelationAttribute>();

        // The XML tag of this object
        private readonly string _tag;

        // The name of this object
        private readonly string _name;

        // the object type of this object
        private readonly string _objectType;

        private readonly BAGObject originalObj;

        public BAGRelation(string tag = "", string name = "", string objectType = "")
        {
            _tag = tag;
            _name = name;
            _objectType = objectType;
            originalObj = null;

            Add(new BAGstringAttribute(16, "identificatie", "identificatie"));
            Add(new BAGbooleanAttribute("aanduidingRecordInactief", "bag_LVC:aanduidingRecordInactief"));
            Add(new BAGintegerAttribute("aanduidingRecordCorrectie", "bag_LVC:aanduidingRecordCorrectie"));
            Add(new BAGdatetimeAttribute("begindatumTijdvakGeldigheid", "bag_LVC:tijdvakgeldigheid/bagtype:begindatumTijdvakGeldigheid"));
            Add(new BAGdatetimeAttribute("einddatumTijdvakGeldigheid", "bag_LVC:tijdvakgeldigheid/bagtype:einddatumTijdvakGeldigheid"));
        }
    }
}
