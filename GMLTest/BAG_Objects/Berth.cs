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

        public string[] berthStatusType = {"Plaats aangewezen", "Plaats ingetrokken" };

        public Berth()
        {
            Console.WriteLine("******CREATED A NEW BERTH ... XD THAT NAME THO");


        }

        public bool hasGeometry()
        {
            return true;
        }
    }

    // Maybe create a class to store all the geometry data? 
    // A Lot of stuff is indented and categorized... soooooooooooooo


}
