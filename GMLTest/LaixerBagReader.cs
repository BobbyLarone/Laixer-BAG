﻿using LaixerGMLTest.BAG_Objects;
using LaixerGMLTest.Object_Relations;
using NetTopologySuite.IO.GML2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;



namespace LaixerGMLTest
{
    /// <summary>
    /// A custom reader for BAG XML Files
    /// </summary>
    class LaixerBagReader
    {
        string filename = @"C:\Users\Workstation\Documents\MyProjects\Documents\XML\9999LIG08082019-000001-reformatted.xml";

        public string logText;
        public string xmlOutput;

        public List<BAGObject> listOfBAGObjects;
        BAGObjectFactory BAGObjectFactory = new BAGObjectFactory();

        public LaixerBagReader()
        {
            listOfBAGObjects = new List<BAGObject>();
        }

        /// <summary>
        /// This can read the Whole XML file within +-35 seconds
        /// </summary>
        public void ReadXML()
        {
            WithXMLReaderAsync(filename).Wait();
        }

        /// <summary>
        /// Reads an XML file
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        public void ReadXML(string filePath)
        {
            //read the xml file Async
            WithXMLReaderAsync(filePath).Wait();
            printAllAttributes();

        }

        /// <summary>
        /// Reads a XML document in a async way
        /// </summary>
        /// <param name="xmlFile">Path to the XML file</param>
        /// <returns></returns>
        private async Task WithXMLReaderAsync(string xmlFile)
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                Async = true,
                IgnoreWhitespace = true,
            };

            // used to start reading the file from top to bottom. 
            using (XmlReader reader = XmlReader.Create(xmlFile, settings))
            {
                while (await reader.ReadAsync())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                await CheckRootElement(reader).ConfigureAwait(false);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Checks the roottype of the XML file
        /// </summary>
        /// <param name="reader">The xml reader</param>
        /// <returns></returns>
        private async Task CheckRootElement(XmlReader reader)
        {
            Console.WriteLine($"Root element: {reader.LocalName}");
            switch (reader.LocalName)
            {
                case "BAG-Extract-Deelbestand-LVC":
                    {
                        await ReadXMLBody(reader).ConfigureAwait(false);
                        break;
                    }
                case "BAG-Mutaties-Deelbestand-LVC":
                    {
                        break;
                    }
                case "BAG-Extract-Levering":
                    {
                        break;
                    }

                default:
                    break;
            }
        }

        private async Task ReadXMLBody(XmlReader reader)
        {
            while (await reader.ReadAsync())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            await PrefixReader(reader).ConfigureAwait(false);
                            break;
                        }

                    case XmlNodeType.Text:
                        {
                            break;
                        }

                    case XmlNodeType.EndElement:
                        {
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }
        }

        public async Task PrefixReader(XmlReader reader)
        {
            switch (reader.Prefix)
            {
                case "xb - remove this to use it -":
                    {
                        break;
                    }
                case "selecties-extract":
                    {
                        break;
                    }

                case "bag_LVC":
                    {
                        await BAGObjectGenerator(reader).ConfigureAwait(false);
                        break;
                    }
                default:
                    break;
            }
        }


        public async Task BAGObjectGenerator(XmlReader reader)
        {
            switch (reader.LocalName)
            {
                case "Ligplaats":
                    {
                        var elementName = "";
                        var nameOfelement = reader.LocalName;
                        var myObject = (Berth)BAGObjectFactory.GetBagObjectByXML(nameOfelement);
                        listOfBAGObjects.Add(myObject);

                        // fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        elementName = reader.LocalName;
                                        if (reader.LocalName == "ligplaatsGeometrie")
                                        {
                                            // insert the list of position data into the attribute :geovlak
                                            var geoData = ReadGMLAttributes(reader);
                                            myObject.SetAttribute("geovlak", geoData);
                                        }

                                        if (reader.LocalName == "hoofdadres")
                                        {
                                            //skip one node to read the text
                                            reader.Read();
                                        }
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);
                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // If the end element is reached
                                        if (reader.LocalName == nameOfelement)
                                        {
                                            // We can get out of this function, because we reached the end tag of this element
                                            return;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case "Woonplaats":
                    {
                        var elementName = "";
                        var nameOfelement = reader.LocalName;
                        var myObject = (Residence)BAGObjectFactory.GetBagObjectByXML(nameOfelement);
                        listOfBAGObjects.Add(myObject);

                        // fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        elementName = reader.LocalName;
                                        if (reader.LocalName.ToLower() == "polygon")
                                        {
                                            var value = await reader.ReadOuterXmlAsync();
                                            myObject.SetAttribute("geovlak", value);
                                        }
                                        if(reader.LocalName == "begindatumTijdvakGeldigheid" || reader.LocalName == "einddatumTijdvakGeldigheid")
                                        {
                                            
                                            // go to next part
                                            reader.Read();
                                            string value = await reader.GetValueAsync();
                                            var timeLenght = value.Length;

                                            var year = int.Parse(value.Substring(0, 4));
                                            var month = int.Parse(value.Substring(4, 2));
                                            var day = int.Parse(value.Substring(6, 2));
                                            var Hour = int.Parse(value.Substring(8, 2));
                                            var minute = int.Parse(value.Substring(10, 2));
                                            var seconds = int.Parse(value.Substring(12, 2));
                                            var microseconds = int.Parse(value.Substring(14,2));

                                            string date = $"{day}{month}{year}{Hour}{minute}{seconds}{microseconds}";
                                            var r = new DateTime(year: year, month: month, day: day, hour: Hour, minute: minute, second: seconds, millisecond: microseconds);

                                            //string[] formats = { "yyyyMMddHHmmss" };
                                            //DateTime date;

                                            //DateTime.TryParseExact(value, formats, null, System.Globalization.DateTimeStyles.AllowWhiteSpaces |
                                            //System.Globalization.DateTimeStyles.AdjustToUniversal,out date);

                                            myObject.SetAttribute(elementName, r);

                                        }
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);
                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // write the end element name. For testing purpouse
                                        if (reader.LocalName == nameOfelement)
                                        {
                                            // We can get out of this function, because we reached the end tag of this element
                                            return;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case "Verblijfsobject":
                    {
                        var elementName = "";
                        var nameOfelement = reader.LocalName;
                        var myObject = (Accommodation)BAGObjectFactory.GetBagObjectByXML(nameOfelement);
                        listOfBAGObjects.Add(myObject);

                        // fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        elementName = reader.LocalName;
                                        if(reader.LocalName == "gerelateerdPand" || reader.LocalName == "hoofdadres")
                                        {
                                            // skip once to get to the id
                                            reader.Read();
                                        }
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);

                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // write the end element name. For testing purpouse
                                        if (reader.LocalName == nameOfelement)
                                        {
                                            // We can get out of this function, because we reached the end tag of this element
                                            return;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case "OpenbareRuimte":
                    {
                        var elementName = "";
                        var nameOfelement = reader.LocalName;
                        var myObject = (PublicSpace)BAGObjectFactory.GetBagObjectByXML(nameOfelement);
                        listOfBAGObjects.Add(myObject);

                        // fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        elementName = reader.LocalName;
                                        if (reader.LocalName == "gerelateerdeWoonplaats")
                                        {
                                            reader.Read();
                                        }
                                        if (reader.LocalName == "VerkorteOpenbareruimteNaam")
                                        {

                                        }
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);

                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // write the end element name. For testing purpouse
                                        if (reader.LocalName == nameOfelement)
                                        {
                                            // We can get out of this function, because we reached the end tag of this element
                                            myObject.ShowAllAttributes();
                                            return;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case "Nummeraanduiding":
                    {
                        var elementName = "";
                        var nameOfelement = reader.LocalName;
                        var myObject = (NumberIndication)BAGObjectFactory.GetBagObjectByXML(nameOfelement);
                        listOfBAGObjects.Add(myObject);

                        // fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        elementName = reader.LocalName;
                                        if (reader.LocalName == "gerelateerdeOpenbareRuimte")
                                        {
                                            reader.Read();
                                        }
                                        if (reader.LocalName == "gerelateerdeWoonplaats")
                                        {
                                            reader.Read();
                                        }
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);
                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // write the end element name. For testing purpouse
                                        if (reader.LocalName == nameOfelement)
                                        {
                                            // We can get out of this function, because we reached the end tag of this element
                                            return;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case "Standplaats":
                    {
                        var elementName = "";
                        var nameOfelement = reader.LocalName;
                        var myObject = (Location)BAGObjectFactory.GetBagObjectByXML(nameOfelement);
                        listOfBAGObjects.Add(myObject);

                        // fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        elementName = reader.LocalName;
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);
                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // write the end element name. For testing purpouse
                                        if (reader.LocalName == nameOfelement)
                                        {
                                            // We can get out of this function, because we reached the end tag of this element
                                            return;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case "Pand":
                    {
                        var elementName = "";
                        var nameOfelement = reader.LocalName;
                        var myObject = (Premises)BAGObjectFactory.GetBagObjectByXML(nameOfelement);
                        listOfBAGObjects.Add(myObject);

                        // fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        elementName = reader.LocalName;
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);

                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // write the end element name. For testing purpouse
                                        if (reader.LocalName == nameOfelement)
                                        {
                                            // We can get out of this function, because we reached the end tag of this element
                                            return;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case "GemeenteWoonplaatsRelatie":
                    {
                        var elementName = "";
                        var nameOfelement = reader.LocalName;
                        var myObject = (MunicipalityResidenceRelation)BAGObjectFactory.GetBagObjectByXML(nameOfelement);
                        listOfBAGObjects.Add(myObject);

                        // fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        elementName = reader.LocalName;

                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);

                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // write the end element name. For testing purpouse
                                        if (reader.LocalName == nameOfelement)
                                        {
                                            // We can get out of this function, because we reached the end tag of this element
                                            return;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        private string ReadGMLAttributes(XmlReader reader)
        {
            var myReader = reader;
            var gmlReader = new GMLReader(new NetTopologySuite.Geometries.GeometryFactory(new NetTopologySuite.Geometries.PrecisionModel(), 0));
            var result = gmlReader.Read(myReader);
            var temp = result.Coordinates.ToList();

            string gmlString = "";
            foreach (var item in temp)
            {
                if (gmlString == "") { gmlString = $"{item.CoordinateValue}"; }

                gmlString = $"{gmlString},{item.CoordinateValue}";
            }
            return gmlString;
        }

        private void convertToGPS()
        {
            // WIP

            /*
             * This function will take a coordinate in the epsg28996 format 
             * and then calculate the gps position based on epgs4326
            */
        }

        private void printAllAttributes()
        {
            if (listOfBAGObjects[0].GetType() == typeof(Berth))
            {
                var item = (Berth)listOfBAGObjects[0];
                item.ShowAllAttributes();
            }
            else if (listOfBAGObjects[0].GetType() == typeof(Location))
            {
                var item = (Location)listOfBAGObjects[0];
                item.ShowAllAttributes();
            }
            else if (listOfBAGObjects[0].GetType() == typeof(Premises))
            {
                var item = (Premises)listOfBAGObjects[0];
                item.ShowAllAttributes();
            }
            else if (listOfBAGObjects[0].GetType() == typeof(NumberIndication))
            {
                var item = (NumberIndication)listOfBAGObjects[0];
                item.ShowAllAttributes();
            }
            else if (listOfBAGObjects[0].GetType() == typeof(Residence))
            {
                var item = (Residence)listOfBAGObjects[0];
                item.ShowAllAttributes();
            }
            else if (listOfBAGObjects[0].GetType() == typeof(PublicSpace))
            {
                var item = (PublicSpace)listOfBAGObjects[0];
                item.ShowAllAttributes();
            }
            else if (listOfBAGObjects[0].GetType() == typeof(Accommodation))
            {
                var item = (Accommodation)listOfBAGObjects[0];
                item.ShowAllAttributes();
            }

        }

    }
}
