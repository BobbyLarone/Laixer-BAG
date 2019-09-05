using System;
using System.Collections.Generic;
using System.Text;

namespace GMLTest.BAG_Objects
{
    /// <summary>
    /// Translation to Dutch: Ligplaats
    /// </summary>
    class Berth : BAGAddressableObject
    {
        private string abbreviation = "LIG";

        public Berth()
        {
            Console.WriteLine("******CREATED A NEW BERTH ... XD THAT NAME THO");
        }

        public bool hasGeometry()
        {
            return true;
        }
    }
}
