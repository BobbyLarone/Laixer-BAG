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

        public string GetType()
        {
            return "POINT";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
