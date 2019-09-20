using LaixerGMLTest.BAG_Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.BAG_Objects
{
    /// <summary>
    /// Translation to Dutch: Verblijfs object
    /// </summary>
    class Accommodation : BAGAddressableObject
    {
        public string VerblijfsobjectStatus { get => GetAttribute("verblijfsobjectStatus").GetValue(); }
        public string OppervlakteVerblijfsobject { get => GetAttribute("oppervlakteVerblijfsobject").GetValue(); }
        public string LigplaatsStatus { get => GetAttribute("verblijfsobjectStatus").GetValue(); }
        public string GebruiksdoelVerblijfsobject { get => GetAttribute("gebruiksdoelVerblijfsobject").GetValue(); }
        public string GerelateerdPand { get => GetAttribute("gerelateerdPand").GetValue(); }
        public string NevenAdres { get => GetAttribute("nevenadres").GetValue() == "" ? null : GetAttribute("nevenadres").GetValue(); }
        public string Geopunt { get => GetAttribute("geopunt").GetValue(); }
        public string Geovlak { get => GetAttribute("geovlak").GetValue(); }
        public string Geom_valid { get => GetAttribute("geom_valid").GetValue() == "" ? null : GetAttribute("geom_valid").GetValue(); }


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

        public Accommodation() : base("bag_LVC:Verblijfsobject", "verblijfsobject", "VBO")
        {
            Add(new BAGenumAttribute(accomodationStatus, accomodationStatus.Count, "verblijfsobjectStatus", "bag_LVC:verblijfsobjectStatus"));
            Add(new BAGenumAttribute(accomodationPurpose, accomodationPurpose.Count, "gebruiksdoelVerblijfsobject", "bag_LVC:gebruiksdoelVerblijfsobject"));
            Add(new BAGnumericAttribute(6, "oppervlakteVerblijfsobject", "bag_LVC:oppervlakteVerblijfsobject"));
            Add(new BAGstringAttribute(16, "gerelateerdPand","bag_LVC:gerelateerdPand/bag_LVC:identificatie"));
            Add(new BAGpoint(3, "geopunt", "bag_LVC:verblijfsobjectGeometrie"));
            Add(new BAGpolygon(3, "geovlak", "bag_LVC:verblijfsobjectGeometrie"));
            Add(new BAGgeometryValidation("geom_valid", "geovlak"));

            AddRelation(new BAGenumRelationAttribute(this, 
                "verblijfsobjectgebruiksdoel",
                "gebruiksdoelVerblijfsobject",
                "bag_LVC:gebruiksdoelVerblijfsobject", 
                new List<string>() { "verblijfsobjectStatus", "geom_valid"} , 
                accomodationPurpose));

            AddRelation(new BAGrelationAttribute(this, 
                "verblijfsobjectpand",
                16, "gerelateerdPand",
                "bag_LVC:gerelateerdPand/bag_LVC:identificatie",
                new List<string>(){ "verblijfsobjectStatus", "geom_valid" }));
        }

        /// <summary>
        /// Returns a value if the object has geometry
        /// </summary>
        /// <returns></returns>
        public bool HasGeometry()
        {
            return true;
        }

        public void ShowAllAttributes()
        {
            var myList = GetListOfAttributes();
            Console.WriteLine($"{myList.Count} Attributes were found.");
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
