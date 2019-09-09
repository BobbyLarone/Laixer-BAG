using System;
using System.Collections.Generic;
using System.Text;

namespace GMLTest.BAG_Attributes
{
    class BAGenumAttribute : BAGAttribute
    {
        private List<string> _enumList;
        private int _length;
        private string _name;
        private string _tag;
        private string value = "";


        public BAGenumAttribute(List<string> list, int length, string name, string tag) : base(length, name, tag)
        {
            _enumList = list;
            _length = length;
            _name = name;
            _tag = tag;


        }
    }
}
