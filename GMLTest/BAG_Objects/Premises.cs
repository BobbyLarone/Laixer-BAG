using LaixerGMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;

namespace LaixerGMLTest.BAG_Objects
{
    /// <summary>
    /// Translation to Dutch: Pand
    /// </summary>
    internal class Premises : BAGObject
    {

        public string PandStatus => GetAttribute("pandstatus").GetValue();
        public string Bouwjaar => GetAttribute("bouwjaar").GetValue();
        public string Geovlak => GetAttribute("geovlak").GetValue();
        public string Geom_valid => GetAttribute("geom_valid").GetValue() == "" ? null : GetAttribute("geom_valid").GetValue();

        public List<string> statusEnum = new List<string>()
        {
            "Bouwvergunning verleend",
            "Niet gerealiseerd pand",
            "Bouw gestart",
            "Pand in gebruik (niet ingemeten)",
            "Pand in gebruik",
            "Sloopvergunning verleend",
            "Pand gesloopt",
            "Pand buiten gebruik"
        };

        /// <summary>
        /// Translation to Dutch: Pand.
        /// Create a new premise object.
        /// </summary>
        public Premises() : base("bag_LVC:Pand", "pand", "PND")
        {
            Add(new BAGenumAttribute(statusEnum, statusEnum.Count, "pandstatus", "bag_LVC:pandstatus"));
            Add(new BAGnumericAttribute(4, "bouwjaar", "bag_LVC:bouwjaar"));
            Add(new BAGpolygon(3, "geovlak", "bag_LVC:pandGeometrie"));
            Add(new BAGgeometryValidation("geom_valid", "geovlak"));
        }

        /// <summary>
        /// Returns a value if the object has geometry
        /// </summary>
        /// <returns></returns>
        public bool HasGeometry() => true;

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
