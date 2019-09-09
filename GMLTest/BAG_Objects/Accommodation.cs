using System;
using System.Collections.Generic;
using System.Text;

namespace GMLTest.BAG_Objects
{
    /// <summary>
    /// Translation to Dutch: Verblijfs object
    /// </summary>
    class Accommodation : BAGObject
    {
        private string abreviation = "VBO";

        public List<string> accomodationStatus = new List<string>()
        {
            "Verblijfsobject gevormd",
            "Niet gerealiseerd verblijfsobject",
            "Verblijfsobject in gebruik (niet ingemeten)",
            "Verblijfsobject in gebruik",
            "Verblijfsobject ingetrokken",
            "Verblijfsobject buiten gebruik"
        };


        public List<string> accomodationPurpose= new List<string>()
        {
            "woonfunctie",
            "bijeenkomstfunctie",
            "celfunctie",
            "gezondheidszorgfunctie",
            "industriefunctie",
            "kantoorfunctie",
            "logiesfunctie",
            "onderwijsfunctie",
            "sportfunctie",
            "winkelfunctie",
            "overige gebruiksfunctie"
        };

        public Accommodation()
        {

        }
    }
}
