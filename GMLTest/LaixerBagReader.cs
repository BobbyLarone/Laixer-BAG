using GMLTest.BAG_Objects;
using NetTopologySuite.IO.GML2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace GMLTest
{
    /// <summary>
    /// A custom reader for BAG XML Files
    /// </summary>
    class LaixerBagReader
    {
        const string filename = @"C:\Users\Workstation\Documents\MyProjects\Documents\XML\9999LIG08082019-000001-reformatted.xml";

        private XmlReader reader;
        private XmlNamespaceManager manager;
        private GMLReader gmlReader; // its here... just for testing purpouses

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
            var firstItem = (Berth)listOfBAGObjects[0];
            firstItem.ShowAllAttributes();
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
                case "gml":
                    {
                        //Console.WriteLine("I found a GML element !!!");
                        break;
                    }

                case "bagtype":
                    {
                        //Console.WriteLine("I found a BagType");
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
                        var elementValue = "";

                        var nameOfelement = reader.LocalName;
                        Berth myObject = (Berth)BAGObjectFactory.GetBagObjectByXML(nameOfelement);
                        listOfBAGObjects.Add(myObject);

                        myObject.ShowAllAttributes();


                        // fill the object with al the stuff that we can find in the xml file
                        while(reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        elementName = reader.LocalName;
                                        Console.WriteLine($"reading the element now: {reader.Name}");
                                        if(reader.LocalName == "ligplaatsStatus")
                                        {
                                            Console.WriteLine("*************FOUND LIGLPAAATS**************");
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
                        var nameOfelement = reader.LocalName;
                        listOfBAGObjects.Add(BAGObjectFactory.GetBagObjectByXML(nameOfelement));

                        // fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        Console.WriteLine($"reading the element now: {reader.Name}");
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        Console.WriteLine($"Text Node: {await reader.GetValueAsync().ConfigureAwait(false)}");
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
                        var nameOfelement = reader.LocalName;
                        listOfBAGObjects.Add(BAGObjectFactory.GetBagObjectByXML(nameOfelement));

                        // fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        Console.WriteLine($"reading the element now: {reader.Name}");
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        Console.WriteLine($"Text Node: {await reader.GetValueAsync().ConfigureAwait(false)}");
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
                        var nameOfelement = reader.LocalName;
                        listOfBAGObjects.Add(BAGObjectFactory.GetBagObjectByXML(nameOfelement));

                        // fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        Console.WriteLine($"reading the element now: {reader.Name}");
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        Console.WriteLine($"Text Node: {await reader.GetValueAsync().ConfigureAwait(false)}");
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
                        var nameOfelement = reader.LocalName;
                        listOfBAGObjects.Add(BAGObjectFactory.GetBagObjectByXML(nameOfelement));

                        // fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        Console.WriteLine($"reading the element now: {reader.Name}");
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        Console.WriteLine($"Text Node: {await reader.GetValueAsync().ConfigureAwait(false)}");
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
                        var nameOfelement = reader.LocalName;
                        listOfBAGObjects.Add(BAGObjectFactory.GetBagObjectByXML(nameOfelement));

                        // fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        Console.WriteLine($"reading the element now: {reader.Name}");
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        Console.WriteLine($"Text Node: {await reader.GetValueAsync().ConfigureAwait(false)}");
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
                        var nameOfelement = reader.LocalName;
                        listOfBAGObjects.Add(BAGObjectFactory.GetBagObjectByXML(nameOfelement));

                        // fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        Console.WriteLine($"reading the element now: {reader.Name}");
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        Console.WriteLine($"Text Node: {await reader.GetValueAsync().ConfigureAwait(false)}");
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
                        var nameOfelement = reader.LocalName;
                        listOfBAGObjects.Add(BAGObjectFactory.GetBagObjectByXML(nameOfelement));

                        // fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        Console.WriteLine($"reading the element now: {reader.Name}");
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // retrieve the value in the node
                                        Console.WriteLine($"Text Node: {await reader.GetValueAsync().ConfigureAwait(false)}");
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


        public void FillObject(Berth myObject)
        {

        }


        /// <summary>
        /// Still WIP, loads Doc in mem i think
        /// </summary>
        public void TheFkingFile()
        {
            int docDept = 0;
            Console.WriteLine("Reading with document");
            //setup and load the XML document
            XmlDocument document = new XmlDocument();
            document.Load(filename);

            var rootName = document.DocumentElement; // read the root name
            var begin = rootName.FirstChild; // read the next node in the xml file
            var rootEndName = rootName.ParentNode.LastChild; // go back to the previous node and read the last child to determin the end element

            Console.WriteLine($"XML depth = {docDept}");
            Console.WriteLine($"Root element is: {rootName.Name} and end element is: {rootEndName.Name} of type: {rootEndName.NodeType}");
            Console.WriteLine("This element has the following child nodes: ");

            while (begin.HasChildNodes)
            {
                docDept++;
                Console.WriteLine("*******************************");
                Console.WriteLine($"XML depth = {docDept}");
                Console.WriteLine($"Data: {begin.Name}");

                if (begin.HasChildNodes && begin.FirstChild.NodeType == XmlNodeType.Element)
                {
                    Console.WriteLine($"Next child is: {begin.FirstChild.Name} and last child is: {begin.LastChild.Name} with {begin.ChildNodes.Count} inbetween");
                }
                begin = begin.FirstChild;
            }

            Console.WriteLine($"Inner data from {begin.ParentNode.Name} = {begin.InnerText}");
        }
    }
}
