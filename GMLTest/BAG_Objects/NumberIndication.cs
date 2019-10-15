using LaixerGMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;

namespace LaixerGMLTest.BAG_Objects
{
    /// <summary>
    /// Translation to Dutch: Nummer indicatie
    /// </summary>
    internal class NumberIndication : BAGObject
    {
        public string Huisnummer => GetAttribute("huisnummer").GetValue();
        public string Huisletter => GetAttribute("huisletter").GetValue();
        public string Huisnummertoevoeging => GetAttribute("huisnummertoevoeging").GetValue();
        public string Postcode => GetAttribute("postcode").GetValue();
        public string NummeraanduidingStatus => GetAttribute("nummeraanduidingStatus").GetValue();
        public string TypeAdresseerbaarObject => GetAttribute("typeAdresseerbaarObject").GetValue();
        public string GerelateerdeOpenbareRuimte => GetAttribute("gerelateerdeOpenbareRuimte").GetValue();
        public string GerelateerdeWoonplaats => GetAttribute("gerelateerdeWoonplaats").GetValue();

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

        public NumberIndication() : base("bag_LVC:Nummeraanduiding", "nummeraanduiding", "NUM")
        {
            Add(new BAGnumericAttribute(5, "huisnummer", "bag_LVC:huisnummer"));
            // Attributes
            Add(new BAGAttribute(1, "huisletter", "bag_LVC:huisletter"));
            Add(new BAGAttribute(4, "huisnummertoevoeging", "bag_LVC:huisnummertoevoeging"));
            Add(new BAGAttribute(6, "postcode", "bag_LVC:postcode"));
            // Enums
            Add(new BAGenumAttribute(numberIndicationTypes, numberIndicationTypes.Count, "nummeraanduidingStatus", "bag_LVC:nummeraanduidingStatus"));
            Add(new BAGenumAttribute(residenceTypes, residenceTypes.Count, "typeAdresseerbaarObject", "bag_LVC:typeAdresseerbaarObject"));
            // String
            Add(new BAGstringAttribute(16, "gerelateerdeOpenbareRuimte", "bag_LVC:gerelateerdeOpenbareRuimte/bag_LVC:identificatie"));
            Add(new BAGstringAttribute(16, "gerelateerdeWoonplaats", "bag_LVC:gerelateerdeWoonplaats/bag_LVC:identificatie"));
        }

        /// <summary>
        /// Get the addressable object
        /// </summary>
        /// <returns> A bag object</returns>
        public BAGObject GetAdressableObject()
        {
            string typeAddressableObject = GetAttribute("typeAdresseerbaarObject").GetValue();

            return typeAddressableObject switch
            {
                "ligplaats" => new Berth(),
                "standplaats" => new Location(),
                "verblijfsobject" => new Accommodation(),
                _ => null,
            };
        }

        public void ShowAllAttributes()
        {
            var myList = GetListOfAttributes();
            Console.WriteLine($"{myList.Count} Attributes were found");
            foreach (var att in myList)
            {
                if (att.GetType() == typeof(BAGdatetimeAttribute))
                {
                    Console.WriteLine($"Found: {att.GetName()} Value: {att.GetDateTime()}");
                }
                else
                {
                    Console.WriteLine($"Found: {att.GetName()} Value: {att.GetValue()}");
                }
            }
        }
    }
}
