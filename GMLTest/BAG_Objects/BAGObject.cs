using LaixerGMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;

namespace LaixerGMLTest.BAG_Objects
{
    public static class StringExtensions
    {
        public static bool AsBoolean(this string str)
        {
            return str[0] == 'J';
        }
    }

    /// <summary>
    /// Base class for all the other objects
    /// </summary>
    public class BAGObject
    {
        private readonly Dictionary<string, BAGAttribute> dictionaryBAGObjects;
        private readonly List<BAGAttribute> attributeList;
        private readonly List<BAGrelationAttribute> relations;

        // The XML tag of this object
        private readonly string _tag;

        // The name of this object
        private readonly string _name;

        // the object type of this object
        private readonly string _objectType;

        public string Identificatie => GetAttribute("identificatie").GetValue();
        public bool AanduidingRecordInactief => GetAttribute("aanduidingRecordInactief").GetValue().AsBoolean();
        public string AanduidingRecordCorrectie => GetAttribute("aanduidingRecordCorrectie").GetValue();
        public bool Officieel => GetAttribute("officieel").GetValue().AsBoolean();
        public bool InOnderzoek => GetAttribute("inOnderzoek").GetValue().AsBoolean();
        public DateTime BegindatumTijdvakGeldigheid => GetAttribute("begindatumTijdvakGeldigheid").GetDateTime();
        public DateTime EinddatumTijdvakGeldigheid => GetAttribute("einddatumTijdvakGeldigheid").GetDateTime();
        public string DocumentNummer => GetAttribute("documentnummer").GetValue();
        public DateTime DocumentDatum => GetAttribute("documentdatum").GetDateTime();

        public BAGObject(string tag = "", string name = "", string objectType = "")
        {
            _tag = tag;
            _name = name;
            _objectType = objectType;

            // Dictionary to acces the attributes based on key pair values
            dictionaryBAGObjects = new Dictionary<string, BAGAttribute>();

            // list of all the attributes that this object contains
            attributeList = new List<BAGAttribute>();

            // This holds the relations to other attributes
            relations = new List<BAGrelationAttribute>();

            Add(new BAGstringAttribute(16, "identificatie", "identificatie"));
            Add(new BAGbooleanAttribute("aanduidingRecordInactief", "bag_LVC:aanduidingRecordInactief"));
            Add(new BAGintegerAttribute("aanduidingRecordCorrectie", "bag_LVC:aanduidingRecordCorrectie"));
            Add(new BAGbooleanAttribute("officieel", "bag_LVC:officieel"));
            Add(new BAGbooleanAttribute("inOnderzoek", "bag_LVC:inOnderzoek"));
            Add(new BAGdatetimeAttribute("begindatumTijdvakGeldigheid", "bag_LVC:tijdvakgeldigheid/bagtype:begindatumTijdvakGeldigheid"));
            Add(new BAGdatetimeAttribute("einddatumTijdvakGeldigheid", "bag_LVC:tijdvakgeldigheid/bagtype:einddatumTijdvakGeldigheid"));
            Add(new BAGstringAttribute(20, "documentnummer", "bag_LVC:bron/bagtype:documentnummer"));
            Add(new BAGdatetimeAttribute("documentdatum", "bag_LVC:bron/bagtype:documentdatum"));
        }

        /// <summary>
        /// Add a BAG attribute to the object
        /// </summary>
        /// <param name="attribute"></param>
        public void Add(BAGAttribute attribute)
        {
            attribute.parentObject = this;

            // Add to the dictionary to acces it later as an key value pair
            dictionaryBAGObjects.Add(attribute.GetName(), attribute);

            // Add to the list
            attributeList.Add(attribute);
        }

        /// <summary>
        /// Add a relation to the object
        /// </summary>
        /// <param name="relation">The relation attribute</param>
        public void AddRelation(BAGrelationAttribute relation)
        {
            relations.Add(relation);
        }


        /// <summary>
        /// Checks if this object has the specified attribute
        /// </summary>
        /// <param name="name">The name of the attribute</param>
        /// <returns>Boolean if the attribute exists</returns>
        public bool HasAttribute(string name)
        {
            return attributeList.Exists(x => x.GetName() == name);
        }

        /// <summary>
        /// Get a BAG attribute that matches the name
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <returns></returns>
        public BAGAttribute GetAttribute(string name)
        {
            return HasAttribute(name) ? attributeList.Find(x => x.GetName() == name) : null;
        }

        public List<BAGAttribute> GetListOfAttributes() { return attributeList; }

        /// <summary>
        /// Set the value for the attribute for this object
        /// </summary>
        /// <param name="attributeName">Name of the attribute</param>
        /// <param name="value">The value for the object</param>
        public void SetAttribute(string attributeName, string value)
        {
            if (HasAttribute(attributeName))
            {
                GetAttribute(attributeName).SetValue(value);
            }
        }

        /// <summary>
        /// Set the value for the attribute for this object
        /// </summary>
        /// <param name="attributeName">Name of the attribute</param>
        /// <param name="value">The value for the object</param>
        public void SetAttribute(string attributeName, DateTime value)
        {
            if (HasAttribute(attributeName))
            {
                GetAttribute(attributeName).SetDateTime(value);
            }
        }
    }
}
