using GMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMLTest.BAG_Objects
{
    class BAGObject
    {
        private Dictionary<string, BAGAttribute> dictionaryBAGObjects;
        private List<BAGAttribute> attributeList;

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
        }


        public void AddToList(BAGAttribute attribute)
        {
            // Add to the dictionary to acces it later as an key value pair
            dictionaryBAGObjects.Add(attribute.GetName(), attribute);
            // Add to the list
            attributeList.Add(attribute);
        }

        public string GetTag()
        {
            return _tag;
        }


    }
}
