using LaixerGMLTest.BAG_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.BAG_Attributes
{
    class BAGenumRelationAttribute : BAGrelationAttribute
    {
        public List<string> enumList;
        private int _length;

        public BAGenumRelationAttribute(BAGObject parent, string relationName, string name, string tag, List<string> exraAttributes, List<string> list) 
            : base(parent, relationName, list.Count, name, tag, exraAttributes)
        {
            enumList = list;
            _length = list.Count;
        }
    }
}
