using LaixerGMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.BAG_Objects
{
    /// <summary>
    /// Translation to Dutch: Woonplaats
    /// </summary>
    class Residence : BAGObject
    {
        public string WoonplaatsNaam { get => GetAttribute("woonplaatsNaam").GetValue(); }
        public string WoonplaatsStatus { get => GetAttribute("woonplaatsStatus").GetValue(); }
        public string Geovlak { get => GetAttribute("geovlak").GetValue(); }
        public string Geom_valid { get => GetAttribute("geom_valid").GetValue() == ""? null : GetAttribute("geom_valid").GetValue(); }



        public List<string> residenceStatusType = new List<string>()
        {
            "Woonplaats aangewezen", "Woonplaats ingetrokken"
        };

        public Residence() : base("bag_LVC:Woonplaats", "woonplaats","WPL")
        {
            Add(new BAGstringAttribute(80, "woonplaatsNaam", "bag_LVC:woonplaatsNaam"));
            Add(new BAGenumAttribute(residenceStatusType, residenceStatusType.Count, "woonplaatsStatus", "bag_LVC:woonplaatsStatus"));
            Add(new BAGmultiPolygon(2, "geovlak", "bag_LVC:woonplaatsGeometrie"));
            Add(new BAGgeometryValidation("geom_valid", "geovlak"));
        }

        public bool HasGeometry()
        {
            return true;
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
