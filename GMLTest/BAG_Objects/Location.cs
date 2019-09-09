using GMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMLTest.BAG_Objects
{
    /// <summary>
    /// Translation to Dutch: standplaats
    /// </summary>
    class Location : BAGAddressableObject
    {
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

        public bool HasGeometry()
        {
            return true;
        }
    }
}
