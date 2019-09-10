using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.BAG_Attributes
{
    class BAGmultiPolygon : BAGpolygon
    {
        public BAGmultiPolygon(int length, string name, string tag) : base(length, name, tag)
        {

        }

        public string GetType()
        {
            return "MULTIPOLYGON";
        }
    }
}
