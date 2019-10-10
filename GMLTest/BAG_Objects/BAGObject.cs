using LaixerGMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;

namespace LaixerGMLTest.BAG_Objects
{
    public static class StringExtensions
    {
        public static bool AsBoolean(this string str) => str[0] == 'J';
    }

    /// <summary>
    /// Base class for all the other objects
    /// </summary>
    public class BAGObject
    {
        private Dictionary<string, BAGAttribute> dictionaryBAGObjects;
        private List<BAGAttribute> attributeList;
        private List<BAGrelationAttribute> relations;

        // The XML tag of this object
        private string _tag;

        // The name of this object
        private string _name;

        // the object type of this object
        private string _objectType;

        public string Identificatie { get => GetAttribute("identificatie").GetValue(); }
        public bool AanduidingRecordInactief { get => GetAttribute("aanduidingRecordInactief").GetValue().AsBoolean(); }
        public string AanduidingRecordCorrectie { get => GetAttribute("aanduidingRecordCorrectie").GetValue(); }
        public bool Officieel { get => GetAttribute("officieel").GetValue().AsBoolean(); }
        public bool InOnderzoek { get => GetAttribute("inOnderzoek").GetValue().AsBoolean(); }
        public DateTime BegindatumTijdvakGeldigheid { get => GetAttribute("begindatumTijdvakGeldigheid").GetDateTime(); }
        public DateTime EinddatumTijdvakGeldigheid { get => GetAttribute("einddatumTijdvakGeldigheid").GetDateTime(); }
        public string DocumentNummer { get => GetAttribute("documentnummer").GetValue(); }
        public DateTime DocumentDatum { get => GetAttribute("documentdatum").GetDateTime(); }

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
        public void AddRelation(BAGrelationAttribute relation) => relations.Add(relation);

        public string GetObjectType() { return _objectType; }


        public string GetTag() { return _tag; }

        /// <summary>
        /// Get the unique id of the BAG_Object
        /// </summary>
        /// <returns></returns>
        public string GetIdentification()
        {
            // Find the attribute by Id
            var result = GetAttribute("identificatie");
            if (result == null)
            {
                return ""; // return an empty string if there is no Id
            }
            // else we can return the value
            return result.GetValue();
        }

        /// <summary>
        /// Checks if this object has the specified attribute
        /// </summary>
        /// <param name="name">The name of the attribute</param>
        /// <returns>Boolean if the attribute exists</returns>
        public bool HasAttribute(string name) => attributeList.Exists(x => x.GetName() == name);

        /// <summary>
        /// check if this object has the specific relation attribute
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Boolean if the relation attribute exists</returns>
        public bool HasRelationAttribute(string name) => relations.Exists(x => x.GetName() == name);

        /// <summary>
        /// Get a BAG attribute that matches the name
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <returns></returns>
        public BAGAttribute GetAttribute(string name) => HasAttribute(name) ? attributeList.Find(x => x.GetName() == name) : null;

        /// <summary>
        /// Get a BAG relation attribute that matches the name
        /// </summary>
        /// <param name="name">Name of the relation attribute</param>
        /// <returns></returns>
        public BAGrelationAttribute GetRelationAttribute(string name) => HasRelationAttribute(name) ? relations.Find(x => x.GetName() == name) : null;

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

        public void SetRelationAttribute(string relationAttributeName, string value)
        {
            if(HasRelationAttribute(relationAttributeName))
            {
                GetRelationAttribute(relationAttributeName).SetValue(value);
            }
        }


        /// <summary>
        /// Return the name of this object
        /// </summary>
        /// <returns></returns>
        public string GetName() { return _name; }

    }
}
