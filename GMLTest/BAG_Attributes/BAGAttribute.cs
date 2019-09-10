using LaixerGMLTest.BAG_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.BAG_Attributes
{
    /// <summary>
    /// Base class for all the other attributes
    /// </summary>
    class BAGAttribute
    {
        private int _length;
        private string _name;
        private string _tag;
        private string _value;
        private string _relationName;

        public BAGObject parentObject; 

        public BAGAttribute(int length, string name, string tag)
        {
            _tag = tag;
            _value = "";
            _name = name;
            _length = length;
            _relationName = "";
            parentObject = null;
        }

        /// <summary>
        /// Get the length of the object
        /// </summary>
        /// <returns></returns>
        public int GetLength()
        {
            return _length;
        }

        /// <summary>
        /// Get the name of the object
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return _name;
        }

        /// <summary>
        /// Returns if the object has a relationName
        /// </summary>
        /// <returns></returns>
        public string GetSingle()
        {
            return _relationName;
        }

        /// <summary>
        /// Get the tag of the object
        /// </summary>
        /// <returns></returns>
        public string GetTag()
        {
            return _tag;
        }

        /// <summary>
        /// Return the value of the object
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {
            return _value;
        }

        /// <summary>
        /// Set the value for the object
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(string value)
        {
            _value = value;
        }
    }
}
