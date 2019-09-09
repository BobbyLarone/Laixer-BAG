using System;
using System.Collections.Generic;
using System.Text;

namespace GMLTest.BAG_Attributes
{
    class BAGbooleanAttribute : BAGAttribute
    {
        private string _name;
        private string _tag;
        private string _value;
        public BAGbooleanAttribute(string name, string tag) : base(0,name,tag)
        {
            _name = name;
            _tag = tag;
            _value = "";
        }
    }
}
