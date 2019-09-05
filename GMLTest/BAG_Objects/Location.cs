using System;
using System.Collections.Generic;
using System.Text;

namespace GMLTest.BAG_Objects
{
    /// <summary>
    /// Translation to Dutch: standplaats
    /// </summary>
    class Location : BAGObject
    {
        private string abbreviation = "STA";
        enum LocationStatusType
        {
            LocationAppointed,
            LocationRevoked
        }
    }
}
