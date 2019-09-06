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

        private enum enumStatus
        {
            Verblijfsobject_gevormd,
            Niet_gerealiseerd_verblijfsobject,
            Verblijfsobject_in_gebruik_niet_ingemeten,
            Verblijfsobject_in_gebruik,
            Verblijfsobject_ingetrokken,
            Verblijfsobject_buiten_gebruik
        }

        private enum enumPurpose
        {
            woonfunctie,
            bijeenkomstfunctie,
            celfunctie,
            gezondheidszorgfunctie,
            industriefunctie,
            kantoorfunctie,
            logiesfunctie,
            onderwijsfunctie,
            sportfunctie,
            winkelfunctie,
            overige_gebruiksfunctie
        }

        public Accommodation()
        {

        }
    }
}
