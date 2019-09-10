using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.BAG_Objects
{
    class BAGAddressableObject : BAGObject
    {
        public BAGAddressableObject(string tag, string name, string objectType)
        {
            Console.WriteLine("Created a new Adressable Object");
            var bagObject = new BAGObject();
        }
    }
}
