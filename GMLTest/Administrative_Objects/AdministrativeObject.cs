using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace LaixerGMLTest.Administrative_Objects
{
    class AdministrativeObject
    {
        public string id { get; set; }

        /// <summary>
        /// Get the date from the node ( I think )
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public string GetDate(XmlNode node)
        {
            string date = "This must be implemented";

            if(node.NodeType == XmlNodeType.Text)
            {
                // MAYBE use a xmlReader and read the node
                return date;
            }

            return date;
        }


        /// <summary>
        /// Seriously WTF is this function ...............................
        /// </summary>
        public void GetNumber()
        {

        }
    }
}
