using LaixerGMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;

namespace LaixerGMLTest.BAG_Objects
{
    /// <summary>
    /// Translation to Dutch: Openbare ruimte
    /// </summary>
    internal class PublicSpace : BAGObject
    {
        public string OpenbareRuimteNaam => GetAttribute("openbareRuimteNaam").GetValue();
        public string OpenbareruimteStatus => GetAttribute("openbareruimteStatus").GetValue();
        public string OpenbareRuimteType => GetAttribute("openbareRuimteType").GetValue();
        public string GerelateerdeWoonplaats => GetAttribute("gerelateerdeWoonplaats").GetValue();
        public string VerkorteOpenbareruimteNaam => GetAttribute("VerkorteOpenbareruimteNaam").GetValue() == "" ?
                null : GetAttribute("VerkorteOpenbareruimteNaam").GetValue();


        public List<string> publicSpaceTypes = new List<string>()
        {
            "Weg",
            "Water",
            "Spoorbaan",
            "Terrein",
            "Kunstwerk",
            "Landschappelijk gebied",
            "Administratief gebied"
        };

        public List<string> publicSpaceStatusTypes = new List<string>()
        {
            "Naamgeving uitgegeven",
            "Naamgeving ingetrokken"
        };

        public PublicSpace() : base("bag_LVC:OpenbareRuimte", "openbareruimte", "OPR")
        {
            Add(new BAGstringAttribute(80, "openbareRuimteNaam", "bag_LVC:openbareRuimteNaam"));
            Add(new BAGenumAttribute(publicSpaceStatusTypes, publicSpaceStatusTypes.Count, "openbareruimteStatus", "bag_LVC:openbareruimteStatus"));
            Add(new BAGenumAttribute(publicSpaceTypes, publicSpaceTypes.Count, "openbareRuimteType", "bag_LVC:openbareRuimteType"));
            Add(new BAGstringAttribute(16, "gerelateerdeWoonplaats", "bag_LVC:gerelateerdeWoonplaats/bag_LVC:identificatie"));
            Add(new BAGAttribute(80, "VerkorteOpenbareruimteNaam", "nen5825:VerkorteOpenbareruimteNaam"));
        }

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
