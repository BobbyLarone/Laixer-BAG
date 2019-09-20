using LaixerGMLTest.BAG_Attributes;
using LaixerGMLTest.BAG_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.Object_Relations
{
    /// <summary>
    /// In Dutch : Gemeente woonplaats relatie
    /// </summary>
    class MunicipalityResidenceRelation : BAGObject
    {
        public string Begindatumtijdvakgeldigheid { get => GetAttribute("begindatumtijdvakgeldigheid").GetValue(); }
        public string Einddatumtijdvakgeldigheid { get => GetAttribute("einddatumtijdvakgeldigheid").GetValue(); }
        public string Woonplaatscode { get => GetAttribute("woonplaatscode").GetValue(); }
        public string Gemeentecode { get => GetAttribute("gemeentecode").GetValue(); }
        public string status { get => GetAttribute("status").GetValue(); }

        private List<string> statusEnum = new List<string>()
        {
            "voorlopig",
            "definitief"
        };

        public MunicipalityResidenceRelation() : base("gwr_LVC:GemeenteWoonplaatsRelatie", "gemeente_woonplaats", "GWR")
        {
            Add(new BAGdatetimeAttribute("begindatumtijdvakgeldigheid", "gwr_LVC:tijdvakgeldigheid/bagtype:begindatumTijdvakGeldigheid"));
            Add(new BAGdatetimeAttribute("einddatumtijdvakgeldigheid", "gwr_LVC:tijdvakgeldigheid/bagtype:einddatumTijdvakGeldigheid"));
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
    }
}
