using LaixerGMLTest.BAG_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.BAG_Attributes
{
    /// <summary>
    /// Base class for all the other attributes
    /// </summary>
    public class BAGAttribute
    {
        private int _length;
        private string _name;
        private string _tag;
        private string _value;
        private string _relationName;
        private DateTime dateTime;

        // The parent of this object
        /// <summary>
        /// 
        /// </summary>
        public BAGObject parentObject; 

        public BAGAttribute(int length, string name, string tag)
        {
            _tag = tag;
            _name = name;
            _length = length;
            _relationName = "";
            parentObject = null;
        }

        /// <summary>
        /// Get the length of the object
        /// </summary>
        /// <returns></returns>
        public int GetLength() => _length;

        /// <summary>
        /// Get the name of the object
        /// </summary>
        /// <returns></returns>
        public string GetName() => _name;

        /// <summary>
        /// Returns if the object has a relationName
        /// </summary>
        /// <returns></returns>
        public string GetSingle() => _relationName;

        /// <summary>
        /// Get the tag of the object
        /// </summary>
        /// <returns></returns>
        public string GetTag() => _tag;

        /// <summary>
        /// Return the value of the object
        /// </summary>
        /// <returns></returns>
        public string GetValue() => _value;

        /// <summary>
        /// Get the datetime
        /// </summary>
        /// <returns></returns>
        public DateTime GetDateTime() => dateTime;


        /// <summary>
        /// Set the value for the object
        /// </summary>
        /// <param name="value">the string value</param>
        public void SetValue(string value) => _value = value;

        /// <summary>
        /// Set the date time for the object
        /// </summary>
        /// <param name="datetime">The time and date for the object</param>
        public void SetDateTime(DateTime datetime) => dateTime = datetime;
    }
}
