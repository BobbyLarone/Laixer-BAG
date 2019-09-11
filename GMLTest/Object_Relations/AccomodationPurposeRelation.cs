using LaixerGMLTest.BAG_Attributes;
using LaixerGMLTest.BAG_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaixerGMLTest.Object_Relations
{
    /// <summary>
    /// In dutch : Verblijfsobjecten gebruiksdoel
    /// </summary>
    class AccomodationPurposeRelation : BAGRelation
    {
        public List<string> accomodationPurpose = new List<string>()
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

        public AccomodationPurposeRelation() : base("", "verblijfsobjectgebruiksdoel", "")
        {
            Add(new BAGenumAttribute(accomodationPurpose,accomodationPurpose.Count, "gebruiksdoelverblijfsobject", "bag_LVC:gebruiksdoelVerblijfsobject"));
        }
    }
}
