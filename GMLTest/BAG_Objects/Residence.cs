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
    }
}
