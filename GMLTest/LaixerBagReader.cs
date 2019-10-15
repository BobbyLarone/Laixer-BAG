using LaixerGMLTest.BAG_Objects;
using LaixerGMLTest.Object_Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;



namespace LaixerGMLTest
{

    //TODO: Rewrite this to make less calls to the XML reader.
    /// <summary>
    /// A custom reader for BAG XML Files
    /// </summary>
    class LaixerBagReader
    {
        public List<BAGObject> listOfBAGObjects;
        private BAGObjectFactory BAGObjectFactory = new BAGObjectFactory();

        public LaixerBagReader()
        {
            listOfBAGObjects = new List<BAGObject>();
        }

        /// <summary>
        /// Reads an XML file
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        public void ReadXML(string filePath)
        {
            //read the xml file Async
            WithXMLReaderAsync(filePath).Wait();
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
            using XmlReader reader = XmlReader.Create(xmlFile, settings);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        await CheckRootElement(reader).ConfigureAwait(false);
                        break;

                    default:
                        break;
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
            switch (reader.LocalName)
            {
                case "BAG-Extract-Deelbestand-LVC":
                case "BAG-GWR-Deelbestand-LVC":
                    await ReadXMLBody(reader).ConfigureAwait(false);
                    break;
                    // removed the break statements so that the default behaviour is called
                case "BAG-Mutaties-Deelbestand-LVC":
                case "BAG-Extract-Levering":
                default:
                    break;
            }
        }

        /// <summary>
        /// Read the XML Body
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private async Task ReadXMLBody(XmlReader reader)
        {
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        await PrefixReader(reader).ConfigureAwait(false);
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Read the prefix of an element
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public async Task PrefixReader(XmlReader reader)
        {
            switch (reader.Prefix)
            {
                case "bag_LVC":
                case "gwr_LVC":
                        await BAGObjectGenerator(reader).ConfigureAwait(false);
                        break;

                case "xb - remove this to use it -":
                case "selecties-extract":
                default:
                    break;
            }
        }

        /// <summary>
        /// Generate a BAG Object Based on the name
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
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

                                        await FillStandardAttributes(reader, elementName, reader.LocalName, myObject).ConfigureAwait(false);

                                        if (reader.LocalName.ToLower() == "hoofdadres")
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

                        // Fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        // Store the name of the current element
                                        elementName = reader.LocalName;
                                        #region test
                                        //if (reader.LocalName.ToLower() == "polygon")
                                        //{
                                        //    var value = await reader.ReadOuterXmlAsync();
                                        //    // Store the value in the property geovlak of this object
                                        //    myObject.SetAttribute("geovlak", value);
                                        //}
                                        //// Transform the date-time string to a DateTime object when these two names are found
                                        //if (reader.LocalName.ToLower() == "begindatumtijdvakgeldigheid" || reader.LocalName.ToLower() == "einddatumtijdvakgeldigheid")
                                        //{
                                        //    // Go to next part
                                        //    reader.Read();
                                        //    // Read the value and transform it into a DateTime object
                                        //    var r = normalizeDateTime(await reader.GetValueAsync());
                                        //    // Set the attribute
                                        //    myObject.SetAttribute(elementName, r);
                                        //}
                                        //// Transform the date string to a DateTime object
                                        //if(reader.LocalName.ToLower() == "documentdatum")
                                        //{
                                        //    // Go to next part
                                        //    reader.Read();
                                        //    // Get the date string
                                        //    var r = normalizeDate(await reader.GetValueAsync());
                                        //    // Set the attribute
                                        //    myObject.SetAttribute(elementName, r);
                                        //}
                                        #endregion
                                        await FillStandardAttributes(reader, elementName, reader.LocalName, myObject).ConfigureAwait(false);

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
                                        if (reader.LocalName == "gerelateerdPand" || reader.LocalName == "hoofdadres")
                                        {
                                            // skip once to get to the id
                                            reader.Read();
                                        }

                                        if (reader.LocalName.ToLower() == "point")
                                        {
                                            string value = await reader.ReadOuterXmlAsync().ConfigureAwait(false);
                                            // Store the value in the property geovlak and point of this object
                                            var value2 = value.Replace("<gml:pos>", "<gml:pos srsDimension=\"3\">");
                                            myObject.SetAttribute("geopunt", value2);
                                        }

                                        #region test
                                        //// Transform the date-time string to a DateTime object when these two names are found
                                        //if (reader.LocalName.ToLower() == "begindatumtijdvakgeldigheid" || reader.LocalName.ToLower() == "einddatumtijdvakgeldigheid")
                                        //{
                                        //    // Go to next part
                                        //    reader.Read();
                                        //    // Read the value and transform it into a DateTime object
                                        //    var r = normalizeDateTime(await reader.GetValueAsync());
                                        //    // Set the attribute
                                        //    myObject.SetAttribute(elementName, r);
                                        //}
                                        //if (reader.LocalName.ToLower() == "polygon")
                                        //{
                                        //    string value = await reader.ReadOuterXmlAsync();
                                        //    // Store the value in the property geovlak of this object
                                        //    myObject.SetAttribute("geovlak", value);
                                        //}
                                        //// Transform the date string to a DateTime object
                                        //if (reader.LocalName.ToLower() == "documentdatum")
                                        //{
                                        //    // Go to next part
                                        //    reader.Read();
                                        //    // Get the date string
                                        //    var r = normalizeDate(await reader.GetValueAsync());
                                        //    // Set the attribute
                                        //    myObject.SetAttribute(elementName, r);
                                        //}
                                        #endregion
                                        await FillStandardAttributes(reader, elementName, reader.LocalName, myObject);

                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // Retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);

                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
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
                                        if (reader.LocalName.ToLower() == "gerelateerdewoonplaats")
                                        {
                                            // read next node
                                            reader.Read();
                                        }

                                        #region test
                                        //// Transform the date-time string to a DateTime object when these two names are found
                                        //if (reader.LocalName.ToLower() == "begindatumtijdvakgeldigheid" || reader.LocalName.ToLower() == "einddatumtijdvakgeldigheid")
                                        //{
                                        //    // Go to next part
                                        //    reader.Read();
                                        //    // Read the value and transform it into a DateTime object
                                        //    var r = normalizeDateTime(await reader.GetValueAsync());
                                        //    // Set the attribute
                                        //    myObject.SetAttribute(elementName, r);
                                        //}

                                        //// Transform the date string to a DateTime object
                                        //if (reader.LocalName.ToLower() == "documentdatum")
                                        //{
                                        //    // Go to next part
                                        //    reader.Read();
                                        //    // Get the date string
                                        //    var r = normalizeDate(await reader.GetValueAsync());
                                        //    // Set the attribute
                                        //    myObject.SetAttribute(elementName, r);
                                        //}
                                        #endregion
                                        await FillStandardAttributes(reader, elementName, reader.LocalName, myObject).ConfigureAwait(false);

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
                                        #region test
                                        //// Transform the date-time string to a DateTime object when these two names are found
                                        //if (reader.LocalName.ToLower() == "begindatumtijdvakgeldigheid" || reader.LocalName.ToLower() == "einddatumtijdvakgeldigheid")
                                        //{
                                        //    // Go to next part
                                        //    reader.Read();
                                        //    // Read the value and transform it into a DateTime object
                                        //    var r = normalizeDateTime(await reader.GetValueAsync());
                                        //    // Set the attribute
                                        //    myObject.SetAttribute(elementName, r);
                                        //}

                                        //// Transform the date string to a DateTime object
                                        //if (reader.LocalName.ToLower() == "documentdatum")
                                        //{
                                        //    // Go to next part
                                        //    reader.Read();
                                        //    // Get the date string
                                        //    var r = normalizeDate(await reader.GetValueAsync());
                                        //    // Set the attribute
                                        //    myObject.SetAttribute(elementName, r);
                                        //}
                                        #endregion

                                        await FillStandardAttributes(reader, elementName, reader.LocalName, myObject).ConfigureAwait(false);

                                        if (reader.LocalName == "gerelateerdeOpenbareRuimte" || reader.LocalName == "gerelateerdeWoonplaats")
                                        {
                                            // Go to next part of the element
                                            reader.Read();

                                            var value = await reader.GetValueAsync().ConfigureAwait(false);
                                            myObject.SetAttribute(elementName, value);
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
                                        #region test
                                        //if (reader.LocalName.ToLower() == "polygon")
                                        //{
                                        //    var value = await reader.ReadOuterXmlAsync();
                                        //    // Store the value in the property geovlak of this object
                                        //    myObject.SetAttribute("geovlak", value);
                                        //}
                                        //// Transform the date-time string to a DateTime object when these two names are found
                                        //if (reader.LocalName.ToLower() == "begindatumtijdvakgeldigheid" || reader.LocalName.ToLower() == "einddatumtijdvakgeldigheid")
                                        //{
                                        //    // Go to next part
                                        //    reader.Read();
                                        //    // Read the value and transform it into a DateTime object
                                        //    var r = normalizeDateTime(await reader.GetValueAsync());
                                        //    // Set the attribute
                                        //    myObject.SetAttribute(elementName, r);
                                        //}
                                        //// Transform the date string to a DateTime object
                                        //if (reader.LocalName.ToLower() == "documentdatum")
                                        //{
                                        //    // Go to next part
                                        //    reader.Read();
                                        //    // Get the date string
                                        //    var r = normalizeDate(await reader.GetValueAsync());
                                        //    // Set the attribute
                                        //    myObject.SetAttribute(elementName, r);
                                        //}
                                        #endregion
                                        await FillStandardAttributes(reader, elementName, reader.LocalName, myObject);

                                        if (reader.LocalName.ToLower() == "hoofdadres")
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
                        // store the name of the element to read.
                        var elementName = "";
                        // Store the name of the Object
                        var nameOfelement = reader.LocalName;
                        // Create a new bag object based on the name of the object
                        var myObject = (Premises)BAGObjectFactory.GetBagObjectByXML(nameOfelement);
                        // Add the object to the list 
                        listOfBAGObjects.Add(myObject);

                        // Fill the object with al the stuff that we can find in the xml file
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    {
                                        elementName = reader.LocalName;

                                        await FillStandardAttributes(reader, elementName, reader.LocalName, myObject).ConfigureAwait(false);
                                        break;
                                    }
                                case XmlNodeType.Text:
                                    {
                                        // Retrieve the value in the node
                                        string value = await reader.GetValueAsync().ConfigureAwait(false);

                                        myObject.SetAttribute(elementName, value);
                                        break;
                                    }
                                case XmlNodeType.EndElement:
                                    {
                                        // Write the end element name. For testing purpouse
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
                        // Create a new BAG Object
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

                                        await FillStandardAttributes(reader, elementName, reader.LocalName, myObject).ConfigureAwait(false);

                                        if(reader.LocalName.ToLower() == "gerelateerdewoonplaats")
                                        {
                                            //skip to "identificatie" node
                                            reader.Read();
                                            reader.Read();
                                            // retrieve the value in the node
                                            string value = await reader.GetValueAsync().ConfigureAwait(false);

                                            // set the value for this node in the attribute "woonplaatscode"
                                            myObject.SetAttribute("woonplaatscode", value);
                                        }
                                        if (reader.LocalName.ToLower() == "gerelateerdegemeente")
                                        {
                                            //skip to "identificatie" node
                                            reader.Read();
                                            reader.Read();
                                            // retrieve the value in the node
                                            string value = await reader.GetValueAsync().ConfigureAwait(false);

                                            // set the value for this node in the attribute "gemeentecode"
                                            myObject.SetAttribute("gemeentecode", value);
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
                default:
                    break;
            }
        }

        /// <summary>
        /// Fill the standard attributes
        /// </summary>
        /// <param name="reader">The xml reader that is used to read the xml file</param>
        /// <param name="elementName">The main name of the element</param>
        /// <param name="name">The name of the element that the current node is</param>
        /// <param name="myObject">The BAGobject for the current element</param>
        /// <returns></returns>
        private async Task FillStandardAttributes(XmlReader reader, string elementName, string name, BAGObject myObject)
        {
            name = name.ToLower();
            switch (name)
            {
                case "polygon":
                    {
                        // Insert the list of position data into the attribute :geovlak
                        var value = await reader.ReadOuterXmlAsync().ConfigureAwait(false);
                        // Set the attribute
                        myObject.SetAttribute("geovlak", value);
                        break;
                    }
                case "begindatumtijdvakgeldigheid":
                case "einddatumtijdvakgeldigheid":
                    {
                        // Go to next part
                        reader.Read();
                        // Read the value and transform it into a DateTime object
                        var r = normalizeDateTime(await reader.GetValueAsync().ConfigureAwait(false));
                        // Set the attribute
                        myObject.SetAttribute(elementName, r);
                        break;
                    }
                case "documentdatum":
                    {
                        // Go to next part
                        reader.Read();
                        // Get the date string
                        var r = normalizeDate(await reader.GetValueAsync().ConfigureAwait(false));
                        // Set the attribute
                        myObject.SetAttribute(elementName, r);
                        break;
                    }

                default:
                    break;
            }
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

        /// <summary>
        /// normalize the datetime string ( ISO8106)
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private DateTime normalizeDateTime(string time)
        {
            // Split and store the date and time separate
            var year = int.Parse(time.Substring(0, 4));
            var month = int.Parse(time.Substring(4, 2));
            var day = int.Parse(time.Substring(6, 2));
            var Hour = int.Parse(time.Substring(8, 2));
            var minute = int.Parse(time.Substring(10, 2));
            var seconds = int.Parse(time.Substring(12, 2));
            var microseconds = int.Parse(time.Substring(14, 2));

            // Create a new DateTime variable with the variables from above
            return new DateTime(year: year, month: month, day: day, hour: Hour, minute: minute, second: seconds, millisecond: microseconds);
        }

        /// <summary>
        /// normalize the date string ( ISO8106)
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private DateTime normalizeDate(string time)
        {
            // Split and store
            var year = int.Parse(time.Substring(0, 4));
            var month = int.Parse(time.Substring(4, 2));
            var day = int.Parse(time.Substring(6, 2));

            // Create a new DateTime object
            return new DateTime(year: year, month: month, day: day);
        }

    }
}
