using GMLTest.ObjectRelations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GMLTest.BAG_Objects
{
    //TODO: Transform this into an singleton class
    class BAGObjectFactory
    {
        /// <summary>
        /// Factory to create new BAG objects
        /// </summary>
        public BAGObjectFactory()
        {   
        }

        /// <summary>
        /// Get a BagObject by specifying a type. This could be WPL or NUM or LIG or something else
        /// </summary>
        /// <param name="type">The type of the object</param>
        /// <returns></returns>
        public BAGObject GetBAGObjectByType(string type)
        {
            switch (type)
            {
                case "WPL":
                    {
                        return new Residence();
                    }
                case "OPR":
                    {
                        return new PublicSpace();
                    }
                case "NUM":
                    {
                        return new NumberIndication();
                    }
                case "LIG":
                    {
                        return new Berth();
                    }
                case "STA":
                    {
                        return new Location();
                    }
                case "VBO":
                    {
                        return new Accommodation();
                    }
                case "PND":
                    {
                        return new Premises();
                    }
                case "GWR":
                    {
                        return new TownResidenceRelation();
                    }

                default:
                    return null;
            }
        }

        /// <summary>
        /// Get a BAG object by name of the node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public BAGObject GetBagObjectByXML(string node)
        {
            switch (node)
            {
                case "Ligplaats":
                    {
                        return new Berth();
                    }
                case "Woonplaats":
                    {
                        return new Residence();
                    }
                case "Verblijfsobject":
                    {
                        return new Accommodation();
                    }
                case "OpenbareRuimte":
                    {
                        return new PublicSpace();
                    }
                case "Nummeraanduiding":
                    {
                        return new NumberIndication();
                    }
                case "Standplaats":
                    {
                        return new Location();
                    }
                case "Pand":
                    {
                        return new Premises();
                    }
                case "GemeenteWoonplaatsRelatie":
                    {
                        return new TownResidenceRelation();
                    }
                default:
                    return null;
            }
        }

        /// <summary>
        /// Get a BAG object by identification number
        /// </summary>
        /// <param name="id">The Identification number</param>
        /// <returns></returns>
        public BAGObject GetBagObjectByIdentificationNumber(int id)
        {
            string stringId = id.ToString();
            if(stringId.Length == 4)
            {
                return new Residence();
            }

            string temp = stringId.Substring(3, 2);
            switch (temp)
            {
                case "30":
                    {
                        return new PublicSpace();
                    }
                case "20":
                    {
                        return new NumberIndication();
                    }
                case "10":
                    {
                        return new Premises();
                    }
                case "03":
                    {
                        return new Location();
                    }
                case "02":
                    {
                        return new Berth();
                    }
                case "01":
                    {
                        return new Accommodation();
                    }
                default:
                    return null;
            }
        }

        /// <summary>
        /// Generate a list of BAG objects.
        /// </summary>
        /// <param name="nodeList">XML Node list for the objects</param>
        /// <returns>Returns a list of BAG Objects</returns>
        public List<BAGObject> GetListOfBAGObjects(List<XmlNode> nodeList)
        {
            List<BAGObject> list = new List<BAGObject>();

            foreach(var node in nodeList)
            {
                var bagObject = GetBagObjectByXML(node.Name);
                if(bagObject != null)
                {
                    list.Add(bagObject);
                }
            }
            return list;
        }
    }
}
