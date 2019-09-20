using LaixerGMLTest.BAG_Objects;
using System.Collections.Generic;

namespace LaixerGMLTest.BAG_Attributes
{
    public class BAGrelationAttribute : BAGAttribute
    {
        private string _relationName;
        private BAGObject _parent;
        private List<string> values;
        private List<string> _extraAttributes;

        //idk yet what kind of type these things are... sooo default for me is string

        public BAGrelationAttribute(BAGObject parent, string relationName, int length, string name, string tag, List<string> extraAttributes)
            : base(length, name, tag)
        {
            _parent = parent;
            _relationName = relationName;
            _extraAttributes = extraAttributes;
            values = new List<string>();
        }

        /// <summary>
        /// Get the name of the relation of this object
        /// </summary>
        /// <returns></returns>
        public string GetRelationName()
        {
            return _relationName;
        }

        /// <summary>
        /// Set the value for this object. This will be added to the list of values of this object
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(string value)
        {
            values.Add(value);
        }

        /// <summary>
        /// Get a list of values 
        /// </summary>
        /// <returns></returns>
        public List<string> GetValue()
        {
            return values;
        }

        /// <summary>
        /// uuuhm? returns false since this object can have multiple values
        /// </summary>
        /// <returns></returns>
        public bool IsSingle()
        {
            return false;
        }

    }
}
