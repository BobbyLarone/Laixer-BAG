using LaixerGMLTest.BAG_Attributes;
using LaixerGMLTest.BAG_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.Object_Relations
{
    /// <summary>
    /// In Dutch : Gemeente-Woonplaats relatie
    /// </summary>
    class MunicipalityResidenceRelation : BAGObject
    {
        public string Woonplaatscode { get => GetAttribute("woonplaatscode").GetValue(); }
        public string Gemeentecode { get => GetAttribute("gemeentecode").GetValue(); }
        public string Status { get => GetAttribute("status").GetValue(); }

        private List<string> statusEnum = new List<string>()
        {
            "voorlopig",
            "definitief"
        };

        public MunicipalityResidenceRelation() : base("gwr_LVC:GemeenteWoonplaatsRelatie", "gemeente_woonplaats", "GWR")
        {
            Add(new BAGnumericAttribute(4, "woonplaatscode", "gwr_LVC:gerelateerdeWoonplaats/gwr_LVC:identificatie"));
            Add(new BAGnumericAttribute(4, "gemeentecode", "gwr_LVC:gerelateerdeGemeente/gwr_LVC:identificatie"));
            Add(new BAGenumAttribute(statusEnum,statusEnum.Count, "status", "gwr_LVC:status"));
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
