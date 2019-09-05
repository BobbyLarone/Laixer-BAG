using GMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMLTest.BAG_Objects
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

        public BAGObject(string tag ="",string name ="", string objecType = "")
        {
            _tag = tag;
            _name = name;
            _objectType = objecType;

            dictionaryBAGObjects = new Dictionary<string, BAGAttribute>();
            attributeList = new List<BAGAttribute>();
            relations = new List<BAGrelationAttribute>();
        }


        public void AddToList(BAGAttribute attribute)
        {
            // Add to the dictionary to acces it later as an key value pair
            dictionaryBAGObjects.Add(attribute.GetName(), attribute);
            // Add to the list
            attributeList.Add(attribute);
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


    }
}
