using System;
using System.Collections.Generic;
using System.Text;

namespace GMLTest.BAG_Objects
{
    class BAGAddressableObject : BAGObject
    {
        public BAGAddressableObject()
        {
            Console.WriteLine("Created a new Adressable Object");
            var bagObject = new BAGObject();
        }
    }
}
