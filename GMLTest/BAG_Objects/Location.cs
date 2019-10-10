using LaixerGMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.BAG_Objects
{
    /// <summary>
    /// Translation to Dutch: standplaats
    /// </summary>
    class Location : BAGAddressableObject
    {

        public string StandplaatsStatus { get => GetAttribute("standplaatsStatus").GetValue(); }
        public string Geovlak { get => GetAttribute("geovlak").GetValue(); }
        public string Geom_valid { get => GetAttribute("geom_valid").GetValue() == "" ? null : GetAttribute("geom_valid").GetValue(); }

        public List<string> locationStatusType = new List<string>()
        {
            "Plaats aangewezen", "Plaats ingetrokken"
        };

        public Location(string tag = "bag_LVC:Standplaats", string name = "standplaats", string objectType = "STA")
            : base(tag, name, objectType)
        {
            Add(new BAGenumAttribute(locationStatusType, locationStatusType.Count, "standplaatsStatus", "bag_LVC:standplaatsStatus"));
            Add(new BAGpolygon(3, "geovlak", "bag_LVC:standplaatsGeometrie"));
            Add(new BAGgeometryValidation("geom_valid", "geovlak"));
        }

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
