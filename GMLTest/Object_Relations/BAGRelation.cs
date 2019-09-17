using LaixerGMLTest.BAG_Attributes;
using LaixerGMLTest.BAG_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.Object_Relations
{
    /// <summary>
    /// Relation from BAG to BAG objects
    /// </summary>
    class BAGRelation : BAGObject
    {
        private List<BAGAttribute> attributeList;
        private Dictionary<string, BAGAttribute> dictionaryBAGObjects;
        private List<BAGrelationAttribute> relations;

        // The XML tag of this object
        private string _tag;

        // The name of this object
        private string _name;

        // the object type of this object
        private string _objectType;

        private BAGObject originalObj;


        public BAGRelation(string tag = "",string name = "", string objectType = "")
        {
            _tag = tag;
            _name = name;
            _objectType = objectType;

            originalObj = null;

            // Dictionary to acces the attributes based on key pair values
            dictionaryBAGObjects = new Dictionary<string, BAGAttribute>();

            // list of all the attributes that this object contains
            attributeList = new List<BAGAttribute>();

            // This holds the relations to other attributes
            relations = new List<BAGrelationAttribute>();

            Add(new BAGstringAttribute(16, "identificatie", "identificatie"));
            Add(new BAGbooleanAttribute("aanduidingRecordInactief", "bag_LVC:aanduidingRecordInactief"));
            Add(new BAGintegerAttribute("aanduidingRecordCorrectie", "bag_LVC:aanduidingRecordCorrectie"));
            Add(new BAGdatetimeAttribute("begindatumTijdvakGeldigheid", "bag_LVC:tijdvakgeldigheid/bagtype:begindatumTijdvakGeldigheid"));
            Add(new BAGdatetimeAttribute("einddatumTijdvakGeldigheid", "bag_LVC:tijdvakgeldigheid/bagtype:einddatumTijdvakGeldigheid"));
        }
    }
}
