using LaixerGMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.BAG_Objects
{
    /// <summary>
    /// Translation to Dutch: Nummer indicatie
    /// </summary>
    class NumberIndication : BAGObject
    {
        public List<string> numberIndicationTypes = new List<string>()
        {
            "Naamgeving uitgegeven",
            "Naamgeving ingetrokken"
        };

        public List<string> residenceTypes = new List<string>()
        {
            "Verblijfsobject",
            "Standplaats",
            "Ligplaats"
        };

        public NumberIndication() : base ("bag_LVC:Nummeraanduiding", "nummeraanduiding", "NUM")
        {
            Add(new BAGnumericAttribute(5, "huisnummer", "bag_LVC:huisnummer"));
            // Attributes
            Add(new BAGAttribute(1, "huisletter", "bag_LVC:huisletter"));
            Add(new BAGAttribute(4, "huisnummertoevoeging", "bag_LVC:huisnummertoevoeging"));
            Add(new BAGAttribute(6, "postcode", "bag_LVC:postcode"));
            // Enums
            Add(new BAGenumAttribute(numberIndicationTypes, numberIndicationTypes.Count, "nummeraanduidingStatus","bag_LVC:nummeraanduidingStatus"));
            Add(new BAGenumAttribute(residenceTypes, residenceTypes.Count, "typeAdresseerbaarObject","bag_LVC:typeAdresseerbaarObject"));
            // String
            Add(new BAGstringAttribute(16, "gerelateerdeOpenbareRuimte","bag_LVC:gerelateerdeOpenbareRuimte/bag_LVC:identificatie"));
            Add(new BAGstringAttribute(16, "gerelateerdeWoonplaats","bag_LVC:gerelateerdeWoonplaats/bag_LVC:identificatie"));
        }

        /// <summary>
        /// Get the addressable object
        /// </summary>
        /// <returns> A bag object</returns>
        public BAGObject GetAdressableObject()
        {
            string typeAddressableObject = GetAttribute("typeAdresseerbaarObject").GetValue();

            switch (typeAddressableObject)
            {
                case "ligplaats": { return new Berth(); }

                case "standplaats": { return new Location(); }

                case "verblijfsobject": {return new Accommodation(); }

                default:
                    return null;
            }
        }

        public void ShowAllAttributes()
        {
            var myList = GetListOfAttributes();
            Console.WriteLine($"{myList.Count} Attributes were found");
            foreach (var att in myList)
            {
                Console.WriteLine($"Found: {att.GetName()} Value: {att.GetValue()}");
            }
        }
    }
}
