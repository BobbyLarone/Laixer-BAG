using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.BAG_Attributes
{
    class BAGpoint : BAGgeoAttribute
    {
        public BAGpoint(int length, string name, string tag) : base(length, name, tag)
        {

        }

        public string GetType() => "POINT";

        public bool IsValid() => true;
    }
}
