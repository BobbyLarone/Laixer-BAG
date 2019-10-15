using LaixerGMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;


namespace LaixerGMLTest.BAG_Objects
{
    /// <summary>
    /// Translation to Dutch: Ligplaats
    /// </summary>
    internal class Berth : BAGAddressableObject
    {
        public string LigplaatsStatus => GetAttribute("ligplaatsStatus").GetValue();
        public string Geovlak => GetAttribute("geovlak").GetValue();
        public string Geom_valid => GetAttribute("geom_valid").GetValue() == "" ? null : GetAttribute("geom_valid").GetValue();

        public List<string> berthStatusType = new List<string>()
        {
            "Plaats aangewezen", "Plaats ingetrokken"
        };

        public Berth(string tag = "bag_LVC:Ligplaats", string name = "ligplaats", string objectType = "LIG")
            : base(tag, name, objectType)
        {
            Add(new BAGenumAttribute(berthStatusType, berthStatusType.Count, "ligplaatsStatus", "bag_LVC:ligplaatsStatus"));
            Add(new BAGpolygon(3, "geovlak", "bag_LVC:ligplaatsGeometrie"));
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
