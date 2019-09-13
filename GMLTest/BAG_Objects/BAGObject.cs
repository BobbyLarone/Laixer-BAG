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

        public string Identification { get => GetAttribute("identificatie").GetValue(); }
        public bool AanduidingRecordInactief { get => GetAttribute("aanduidingRecordInactief").GetValue().AsBoolean(); }
        public string AanduidingRecordCorrectie { get => GetAttribute("aanduidingRecordCorrectie").GetValue(); }
        public bool Officieel { get => GetAttribute("officieel").GetValue().AsBoolean(); }
        public bool InOnderzoek { get => GetAttribute("inOnderzoek").GetValue().AsBoolean(); }
        public string BegindatumTijdvakGeldigheid { get => GetAttribute("begindatumTijdvakGeldigheid").GetValue(); }
        public string EinddatumTijdvakGeldigheid { get => GetAttribute("einddatumTijdvakGeldigheid").GetValue(); }
        public string DocumentNummer { get => GetAttribute("documentnummer").GetValue(); }
        public string DocumentDatum { get => GetAttribute("documentdatum").GetValue(); }

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
        public void AddRelation(BAGrelationAttribute relation) { relations.Add(relation); }

        public string GetObjectType() { return _objectType; }

        /// <summary>
        /// Get the relations of this object
        /// </summary>
        /// <param name="relationName"></param>
        public List<BAGrelationAttribute> GetRelations(string relationName = "")
        {
            var result = new List<BAGrelationAttribute>();

            foreach (var relation in relations)
            {
                if (relation == null || relationName == relation.GetRelationName())
                {
                    result.Add(relation);
                }
            }
            return result;
        }

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
        /// <returns></returns>
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
            #region original if statement
            /*
             *  if(HasAttribute(name))
             *  {
             *      return attributeList.Find(x => x.GetName() == name);
             *  }
             *  return null;
            */
            #endregion

            // Ternaery operator style for the if statement above
            return HasAttribute(name) ? attributeList.Find(x => x.GetName() == name) : null;

        }

        public List<BAGAttribute> GetListOfAttributes() { return attributeList; }

        /// <summary>
        /// Fill the attributes of this object
        /// </summary>
        /// <param name="values"></param>
        public void SetAttributes(List<string> values)
        {
            int i = 0;

            int max = values.Count;
            foreach (var attribute in attributeList)
            {
                SetAttribute(attribute.GetName(), values[i]);
                i++;
            }

        }

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
                GetAttribute(attributeName).SetValue(value);
            }
        }

        /// <summary>
        /// Return the name of this object
        /// </summary>
        /// <returns></returns>
        public string GetName() { return _name; }

    }
}
