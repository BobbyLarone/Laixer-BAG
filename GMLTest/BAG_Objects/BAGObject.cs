using LaixerGMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.BAG_Objects
{
    /// <summary>
    /// Base class for all the other objects
    /// </summary>
    class BAGObject
    {
        private Dictionary<string, BAGAttribute> dictionaryBAGObjects;
        private List<BAGAttribute> attributeList;
        private List<BAGrelationAttribute> relations;

        private string _objectType;
        private string _tag;
        private string _name;

        private string originalObj = "";
        private string processId = "";
        private int BAGObjectId;


        public BAGObject(string tag ="",string name ="", string objecType = "")
        {
            // The XML tag of this object
            _tag = tag;

            // The name of this object
            _name = name;

            // the object type of this object
            _objectType = objecType;

            // Dictionary to acces the attributes based on key pair values
            dictionaryBAGObjects = new Dictionary<string, BAGAttribute>();

            // list of all the attributes that this object contains
            attributeList = new List<BAGAttribute>();

            // This holds the relations to other attributes
            relations = new List<BAGrelationAttribute>();

            Add(new BAGstringAttribute  (16, "identificatie", "identificatie"));
            Add(new BAGbooleanAttribute ("aanduidingRecordInactief", "bag_LVC:aanduidingRecordInactief"));
            Add(new BAGintegerAttribute ("aanduidingRecordCorrectie", "bag_LVC:aanduidingRecordCorrectie"));
            Add(new BAGbooleanAttribute ("officieel", "bag_LVC:officieel"));
            Add(new BAGbooleanAttribute ("inOnderzoek", "bag_LVC:inOnderzoek"));
            Add(new BAGdatetimeAttribute("begindatumTijdvakGeldigheid", "bag_LVC:tijdvakgeldigheid/bagtype:begindatumTijdvakGeldigheid"));
            Add(new BAGdatetimeAttribute("einddatumTijdvakGeldigheid", "bag_LVC:tijdvakgeldigheid/bagtype:einddatumTijdvakGeldigheid"));
            Add(new BAGstringAttribute  (20, "documentnummer", "bag_LVC:bron/bagtype:documentnummer"));
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
            // Add a relation to the object
            relations.Add(relation);
        }

        public string GetObjectType()
        {
            return _objectType;
        }
        
        /// <summary>
        /// Get the relations
        /// </summary>
        /// <param name="relationName"></param>
        public List<BAGrelationAttribute> GetRelations(string relationName = "")
        {
            var result = new List<BAGrelationAttribute>();

            foreach(var relation in relations)
            {
                if(relation == null || relationName == relation.GetRelationName())
                {
                    result.Add(relation);
                }
            }
            return result;
        }

        public string GetTag()
        {
            return _tag;
        }

        /// <summary>
        /// Get the unique id of the BAG_Object
        /// </summary>
        /// <returns></returns>
        public string GetIdentification()
        {
            // Find the attribute by Id
            var result = GetAttribute("identificatie");
            if(result == null)
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
            if(HasAttribute(name))
            {
                return attributeList.Find(x => x.GetName() == name);
            }
            return null;
        }

        public List<BAGAttribute> GetListOfAttributes()
        {
            return attributeList;
        }

        public void SetAttribute(string attributeName, string value)
        {
            if (HasAttribute(attributeName))
            {
                foreach(var item in attributeList)
                {
                    if(item.GetName() == attributeName)
                    {
                        item.SetValue(value);
                    }
                }
            }
        }

    }
}
