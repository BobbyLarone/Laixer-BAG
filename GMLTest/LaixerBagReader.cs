using NetTopologySuite.IO.GML2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace GMLTest
{
    class LaixerBagReader
    {
        static int maxDepth = 1;
        const string filename = @"C:\Users\Workstation\Documents\MyProjects\Documents\XML\9999LIG08082019-000001-reformatted.xml";

        XmlReader reader;
        XmlNamespaceManager manager;
        GMLReader gmlReader; // its here... just for testing purpouses

        public string logText = "";
        public string xmlOutput = "";

        public LaixerBagReader()
        {
            Console.WriteLine("Starting to open the file...........");
            gmlReader = new GMLReader();

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
        /// <returns></returns>
        public void withXML()
        {
            WithXMLReaderAsync(filename).Wait();
        }

        /// <summary>
        /// Reads a XML document Async
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

            using (XmlReader reader = XmlReader.Create(xmlFile, settings))
            {
                while (await reader.ReadAsync())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                        {
                                Console.WriteLine($"Start Element {reader.Name} with prefix: {reader.Prefix}");
                                Console.WriteLine(manager.LookupNamespace(reader.Prefix)); 
                                if(reader.LocalName == "identificatie")
                                {
                                    Console.WriteLine("***************************************");
                                    Console.WriteLine("I FOUND THE IDENTIFACATION NUMBER !!!");
                                    Console.WriteLine("***************************************");
                                }
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
