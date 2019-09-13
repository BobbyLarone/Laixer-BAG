using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.BAG_Attributes
{
    class BAGgeoAttribute : BAGAttribute
    {
        private int _dimension;
        public BAGgeoAttribute(int dimension, string name, string tag) : base(-1, name, tag)
        {
            _dimension = dimension;
        }

        public int GetDimension()
        {
            return _dimension;
        }

        public bool IsGeometry()
        {
            return true;
        }
    }
}
