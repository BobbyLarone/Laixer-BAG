using GMLTest.BAG_Attributes;
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

        public string mainAddress = "";
        public string relatedAddress = "";
        public string relatedAddress_Identification = "";
        public string indicationRecordInactive = "";
        public string indicationRecordCorrection = "";
        public string official = "";
        public string berthStatus= "";
        public string startDataPeriodValidity = "";
        public string inResearch = "";
        public string source_DocumentDate = "";
        public string source_DocumentNumber = "";

        public List<string> berthStatusType = new List<string>()
        {
            "Plaats aangewezen", "Plaats ingetrokken"
        };

        public Berth(string tag = "bag_LVC:Ligplaats", string name = "ligplaats", string objectType = "LIG")
            : base (tag,name,objectType)
        {
            Console.WriteLine("******CREATED A NEW BERTH ... XD THAT NAME THO");

            Add(new BAGenumAttribute(berthStatusType, berthStatusType.Count, "ligplaatsStatus", "bag_LVC:ligplaatsStatus"));
            Add(new BAGpolygon(3, "geovlak", "bag_LVC:ligplaatsGeometrie"));
            Add(new BAGgeometryValidation("geom_valid","geovlak"));
        }

        public bool HasGeometry()
        {
            return true;
        }
    }

    // Maybe create a class to store all the geometry data? 
    // A Lot of stuff is indented and categorized... soooooooooooooo


}
