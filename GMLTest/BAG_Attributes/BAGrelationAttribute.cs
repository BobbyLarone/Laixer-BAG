using System;
using System.Collections.Generic;
using System.Text;

namespace GMLTest.BAG_Attributes
{
    class BAGrelationAttribute : BAGAttribute
    {
        private string _relationName;

        //idk yet what kind of type these things are... sooo default for me is string
        private string _extraAttributes;

        public BAGrelationAttribute(string relationName, int length, string name, string tag, string extraAttributes) 
            : base(length, name, tag)
        {
            _relationName = relationName;
            _extraAttributes = extraAttributes;
        }

        public string GetRelationName()
        {
            return _relationName;
        }

    }
}
