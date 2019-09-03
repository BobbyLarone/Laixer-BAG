using System;
using System.Collections.Generic;
using System.Text;

namespace GMLTest.BAG_Attributes
{
    class BAGenumRelationAttribute : BAGrelationAttribute
    {
        public BAGenumRelationAttribute(string relationName, int length, string name, string tag, string exraAttributes, List<BAGrelationAttribute> list) 
            : base(relationName, list.Count, name, tag, exraAttributes)
        {

        }
    }
}
