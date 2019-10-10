using LaixerGMLTest.Object_Relations;
using System.Collections.Generic;
using System.Xml;

namespace LaixerGMLTest.BAG_Objects
{
    //TODO: Transform this into an singleton class

    /// <summary>
    /// Factory to create new BAG objects
    /// </summary>
    internal class BAGObjectFactory
    {
        /// <summary>
        /// Get a BagObject by specifying a type. This could be WPL or NUM or LIG or something else
        /// </summary>
        /// <param name="type">The type of the object</param>
        /// <returns></returns>
        public BAGObject GetBAGObjectByType(string type) => type switch
        {
            "WPL" => new Residence(),
            "OPR" => new PublicSpace(),
            "NUM" => new NumberIndication(),
            "LIG" => new Berth(),
            "STA" => new Location(),
            "VBO" => new Accommodation(),
            "PND" => new Premises(),
            "GWR" => new MunicipalityResidenceRelation(),
            _ => null,
        };

        /// <summary>
        /// Get a BAG object by name of the node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public BAGObject GetBagObjectByXML(string node) => node switch
        {
            "Ligplaats" => new Berth(),
            "Woonplaats" => new Residence(),
            "Verblijfsobject" => new Accommodation(),
            "OpenbareRuimte" => new PublicSpace(),
            "Nummeraanduiding" => new NumberIndication(),
            "Standplaats" => new Location(),
            "Pand" => new Premises(),
            "GemeenteWoonplaatsRelatie" => new MunicipalityResidenceRelation(),
            _ => null,
        };

        /// <summary>
        /// Get a BAG object by identification number
        /// </summary>
        /// <param name="id">The Identification number</param>
        /// <returns>A Bag object</returns>
        public BAGObject GetBagObjectByIdentificationNumber(int id)
        {
            string stringId = id.ToString();
            if (stringId.Length == 4)
            {
                return new Residence();
            }

            // Split the string at the 3th index and after 2 characters split the string.
            string temp = stringId.Substring(3, 2);
            return temp switch
            {
                "30" => new PublicSpace(),
                "20" => new NumberIndication(),
                "10" => new Premises(),
                "03" => new Location(),
                "02" => new Berth(),
                "01" => new Accommodation(),
                _ => null,
            };
        }

        /// <summary>
        /// Generate a list of BAG objects.
        /// </summary>
        /// <param name="nodeList">XML Node list for the objects</param>
        /// <returns>Returns a list of BAG Objects</returns>
        public List<BAGObject> GetListOfBAGObjects(List<XmlNode> nodeList)
        {
            if (nodeList is null)
            {
                throw new System.ArgumentNullException(nameof(nodeList));
            }

            List<BAGObject> list = new List<BAGObject>();

            foreach (var node in nodeList)
            {
                var bagObject = GetBagObjectByXML(node.LocalName);
                if (bagObject != null)
                {
                    list.Add(bagObject);
                }
            }
            return list;
        }
    }
}
