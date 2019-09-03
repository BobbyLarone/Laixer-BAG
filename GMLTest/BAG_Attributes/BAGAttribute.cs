using GMLTest.BAG_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMLTest.BAG_Attributes
{
    class BAGAttribute
    {
        private int _length;
        private string _name;
        private string _tag;
        private string _value { get; set; }
        private string _relationName;

        public BAGObject parentObject; 

        public BAGAttribute(int length, string name, string tag)
        {
            _length = length;
            _name = name;
            _tag = tag;
            _relationName = String.Empty;
        }

        public int GetLength()
        {
            return _length;
        }

        public string GetName()
        {
            return _name;
        }

        public string GetSingle()
        {
            return _relationName;
        }

        public string GetTag()
        {
            return _tag;
        }


    }
}
