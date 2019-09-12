using LaixerGMLTest.BAG_Objects;
using LaixerGMLTest.Object_Relations;
using NetTopologySuite.IO.GML2;
using System;
using System.Collections.Generic;
using System.IO;
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
        const string filename = @"C:\Users\Workstation\Documents\MyProjects\Documents\XML\9999LIG08082019-000001-reformatted.xml";

        private XmlReader reader;
        private XmlNamespaceManager manager;

        public string logText = "";
        public string xmlOutput = "";

        public List<BAGObject> listOfBAGObjects;
        BAGObjectFactory BAGObjectFactory = new BAGObjectFactory();

        public LaixerBagReader()
        {
            Console.WriteLine("Starting to open the file...........");
            //gmlReader = new GMLReader();
            listOfBAGObjects = new List<BAGObject>();

            XmlReaderSettings settings = new XmlReaderSettings
            {
                Async = true
            };
            reader = XmlReader.Create(filename, settings);
            var table = reader.NameTable;

            manager = new XmlNamespaceManager(table);
            FillXMLNSManager();
        }

        /// <summary>
        /// Fill the XML namespace manager with namespaces
        /// </summary>
        private void FillXMLNSManager()
        {
            manager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            manager.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            manager.AddNamespace("xb", "http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-deelbestand-lvc/v20090901");
            manager.AddNamespace("bag_LVC", "http://www.kadaster.nl/schemas/imbag/lvc/v20090901");
            manager.AddNamespace("gml", "http://www.opengis.net/gml");
            manager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
            manager.AddNamespace("bagtype", "http://www.kadaster.nl/schemas/imbag/imbag-types/v20090901");
            manager.AddNamespace("nen5825", "http://www.kadaster.nl/schemas/imbag/nen5825/v20090901");
            manager.AddNamespace("product_LVC", "http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-producten-lvc/v20090901");
            manager.AddNamespace("selecties-extract", "http://www.kadaster.nl/schemas/bag-verstrekkingen/extract-selecties/v20090901");
        }

        /// <summary>
        /// Test Function to check if the namespaces are correctly places inside the XML namespace manager
        /// </summary>
        public void TheNameSpaces()
        {
            Console.WriteLine("The namespace reader is called :");

            var result = manager.GetNamespacesInScope(XmlNamespaceScope.All);
            Console.WriteLine($"Amount of namespaces: {result.Count}");

            foreach (KeyValuePair<string, string> entry in result)
            {
                Console.WriteLine($"Entry Key: {entry.Key} and value: {entry.Value}");
            }
        }


        //TODO: check for 
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
                            Console.WriteLine("Other node {0} with value {1}", reader.NodeType, reader.Value);
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
                            Console.WriteLine("reading the element now: ");
                            await PrefixReader(reader).ConfigureAwait(false);
                            break;
                        }

                    case XmlNodeType.Text:
                        {
                            Console.WriteLine($"Text Node: {await reader.GetValueAsync().ConfigureAwait(false)}");
                            break;
                        }

                    case XmlNodeType.EndElement:
                        {
                            Console.WriteLine($"End Element {reader.Name} \n");
                            break;
                        }

                    default:
                        {
                            Console.WriteLine("Other node {0} with value {1}", reader.NodeType, reader.Value);
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
                        Console.WriteLine($"Root element: {reader.LocalName}");
                        break;
                    }
                case "selecties-extract":
                    {
                        Console.WriteLine($"Start Element {reader.Name}");
                        break;
                    }

                case "bag_LVC":
                    {
                        Console.WriteLine($"Start Element {reader.Name}");

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
                        while(reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        elementName = reader.LocalName;
                                        Console.WriteLine($"reading the element now: {reader.Name}");
                                        if(reader.LocalName == "ligplaatsGeometrie")
                                        {
                                            var geoData = ReadGMLAttributes(reader);
                                            myObject.SetAttribute("geovlak", geoData);

                                        }

                                        if(reader.LocalName == "hoofdadres")
                                        {
                                            reader.Read();
                                        }
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);
                                        Console.WriteLine($"Text Node: {value}");

                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // write the end element name. For testing purpouse
                                        Console.WriteLine($"End Element {reader.Name} \n");
                                        if(reader.LocalName == nameOfelement)
                                        {
                                            // We can get out of this function, because we reached the end tag of this element
                                            myObject.ShowAllAttributes();
                                            return;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Other node {0} with value {1}", reader.NodeType, reader.Value);
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
                                        Console.WriteLine($"reading the element now: {reader.Name}");
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);
                                        Console.WriteLine($"Text Node: {value}");

                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // write the end element name. For testing purpouse
                                        Console.WriteLine($"End Element {reader.Name} \n");
                                        if (reader.LocalName == nameOfelement)
                                        {
                                            // We can get out of this function, because we reached the end tag of this element
                                            return;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Other node {0} with value {1}", reader.NodeType, reader.Value);
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
                                        Console.WriteLine($"reading the element now: {reader.Name}");
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);
                                        Console.WriteLine($"Text Node: {value}");

                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // write the end element name. For testing purpouse
                                        Console.WriteLine($"End Element {reader.Name} \n");
                                        if (reader.LocalName == nameOfelement)
                                        {
                                            // We can get out of this function, because we reached the end tag of this element
                                            return;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Other node {0} with value {1}", reader.NodeType, reader.Value);
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
                                        Console.WriteLine($"reading the element now: {reader.Name}");
                                        //if (reader.LocalName == "gerelateerdeWoonplaats")
                                        {
                                            reader.Read();
                                        }
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);
                                        Console.WriteLine($"Text Node: {value}");

                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // write the end element name. For testing purpouse
                                        Console.WriteLine($"End Element {reader.Name} \n");
                                        if (reader.LocalName == nameOfelement)
                                        {
                                            // We can get out of this function, because we reached the end tag of this element
                                            return;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Other node {0} with value {1}", reader.NodeType, reader.Value);
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

                                        Console.WriteLine($"reading the element now: {reader.Name}");
                                        if(reader.LocalName == "gerelateerdeOpenbareRuimte")
                                        {
                                            reader.Read();
                                        }
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);
                                        Console.WriteLine($"Text Node: {value}");

                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // write the end element name. For testing purpouse
                                        Console.WriteLine($"End Element {reader.Name} \n");
                                        if (reader.LocalName == nameOfelement)
                                        {
                                            // We can get out of this function, because we reached the end tag of this element
                                            return;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Other node {0} with value {1}", reader.NodeType, reader.Value);
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

                                        Console.WriteLine($"reading the element now: {reader.Name}");
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);
                                        Console.WriteLine($"Text Node: {value}");

                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // write the end element name. For testing purpouse
                                        Console.WriteLine($"End Element {reader.Name} \n");
                                        if (reader.LocalName == nameOfelement)
                                        {
                                            // We can get out of this function, because we reached the end tag of this element
                                            return;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Other node {0} with value {1}", reader.NodeType, reader.Value);
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

                                        Console.WriteLine($"reading the element now: {reader.Name}");
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);
                                        Console.WriteLine($"Text Node: {value}");

                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // write the end element name. For testing purpouse
                                        Console.WriteLine($"End Element {reader.Name} \n");
                                        if (reader.LocalName == nameOfelement)
                                        {
                                            // We can get out of this function, because we reached the end tag of this element
                                            return;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Other node {0} with value {1}", reader.NodeType, reader.Value);
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

                                        Console.WriteLine($"reading the element now: {reader.Name}");
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);
                                        Console.WriteLine($"Text Node: {value}");

                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // write the end element name. For testing purpouse
                                        Console.WriteLine($"End Element {reader.Name} \n");
                                        if (reader.LocalName == nameOfelement)
                                        {
                                            // We can get out of this function, because we reached the end tag of this element
                                            return;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Other node {0} with value {1}", reader.NodeType, reader.Value);
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
//            Console.WriteLine(result.SRID);
//            Console.WriteLine(result);


            foreach (var item in temp)
            {
                Console.WriteLine(item.CoordinateValue);
                if(gmlString == "") { gmlString = $"{item.CoordinateValue}"; }

                gmlString = $"{gmlString},{item.CoordinateValue}";
            }
            return gmlString;
        }

        private void convertToGPS()
        {
            //var csWgs84 = ProjNet.CoordinateSystems.GeographicCoordinateSystems.WGS84;
            //const string epsg27700 = "..."; // see http://epsg.io/27700
            //var cs27700 = ProjNet.Converters.WellKnownText.CoordinateSystemWktReader.Parse(epsg27700);
            //var ctFactory = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
            //var ct = ctFactory.CreateFromCoordinateSystems(csWgs84, cs27700);
            //var mt = ct.MathTransform;

            //var gf = new NetTopologySuite.Geometries.GeometryFactory(27700);

            //// BT2 8HB
            //var myPostcode = gf.CreatePoint(mt.Transform(new Coordinate(-5.926223, 54.592395)));
            //// DT11 0DB
            //var myMatesPostcode = gf.CreatePoint(mt.Transform(new Coordinate(-2.314507, 50.827157)));

            //double distance = myPostcode.Distance(myMatesPostcode);
        }

        private void printAllAttributes()
        {
            if(listOfBAGObjects[0].GetType() == typeof(Berth))
            {
                var item = (Berth)listOfBAGObjects[0];
                item.ShowAllAttributes();
            }
            else if(listOfBAGObjects[0].GetType() == typeof(Location))
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
