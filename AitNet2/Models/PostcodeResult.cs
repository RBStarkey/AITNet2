using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AitNet2.Models
{
    [Serializable]
    public class PostcodeResult
    {
        public string Postcode { get; set; }
        public int Quality { get; set; }
        public int Eastings { get; set; }
        public int Northings { get; set; }
        public string Country { get; set; }

        [JsonProperty("nhs_ha")]
        public string NHSHealthAuthority { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string ParliamentaryConstituency { get; set; }
        public string EuropeanElectoralRegion { get; set; }
        public string PrimaryCareTrust { get; set; }
        public string Region { get; set; }
        public string LSOA { get; set; }
        public string MSOA { get; set; }
        public string NUTS { get; set; }
        public string InCode { get; set; }
        public string OutCode { get; set; }
        public string AdminDistrict { get; set; }
        public string Parish { get; set; }
        public string AdminCounty { get; set; }
        public string AdminWard { get; set; }
        public string CCG { get; set; }
        public Codes Codes { get; set; }
        public List<LineItem> ResultList { get; set; }
        public void LoadResults()
        {
            ResultList = new List<LineItem>();
            ResultList.Add(new LineItem() { Caption = "Post Code", Data = Postcode ?? "No data available" });
            ResultList.Add(new LineItem() { Caption = "Parish", Data = Parish ?? "-" });
            ResultList.Add(new LineItem() { Caption = "Eastings", Data = Eastings.ToString()?? "-" });
            ResultList.Add(new LineItem() { Caption = "Northings", Data = Northings.ToString() ?? "-" });
            ResultList.Add(new LineItem() { Caption = "Longitude", Data = Longitude.ToString() ?? "-" });
            ResultList.Add(new LineItem() { Caption = "Latitude", Data = Latitude.ToString() ?? "-" });

            ResultList.Add(new LineItem() { Caption = "NHS Health Authority ", Data = NHSHealthAuthority ?? "-" });
            ResultList.Add(new LineItem() { Caption = "Parliamentary Constituency", Data = ParliamentaryConstituency ?? "-" });
            ResultList.Add(new LineItem() { Caption = "European Electoral Region", Data = EuropeanElectoralRegion ?? "-" });
            ResultList.Add(new LineItem() { Caption = "Primary Care Trust", Data = PrimaryCareTrust ?? "-" });

            ResultList.Add(new LineItem() { Caption = "Census lower layer SOA code", Data = LSOA ?? "-" });
            ResultList.Add(new LineItem() { Caption = "Census middle layer SOA (MSOA) code", Data = MSOA ?? "-" });

            //Console.WriteLine("Region: " + Region);
            if (!string.IsNullOrEmpty(NUTS))
            {
                ResultList.Add(new LineItem() { Caption = "NUTS (EU term)", Data = NUTS ?? "-" });
            }
            
            //Console.WriteLine("InCode: " + InCode);
            //Console.WriteLine("OutCode: " + OutCode);
            //ResultList.Add(new LineItem() { Caption = "Admin District", Data = AdminDistrict ?? "-" });
            //Console.WriteLine("AdminDistrict: " + AdminDistrict);


            //Console.WriteLine("Admin County: " + AdminCounty);
            //Console.WriteLine("AdminWard: " + AdminWard);
            //Console.WriteLine("Clinical Commissioning Group (CCG): " + CCG);
            ResultList.Add(new LineItem() { Caption = "Clinical Commissioning Group (CCG)", Data = CCG ?? "-" });

            if (Codes != null)
            {
                if (!string.IsNullOrEmpty(Codes.AdminDistrict))
                {
                    ResultList.Add(new LineItem() { Caption = "Admin District", Data = Codes.AdminDistrict ?? "-" });
                }

                if (!string.IsNullOrEmpty(Codes.AdminCounty))
                {
                    ResultList.Add(new LineItem() { Caption = "Admin County", Data = Codes.AdminCounty ?? "-" });
                }
                if (!string.IsNullOrEmpty(Codes.AdminWard))
                {
                    Console.WriteLine("Admin Ward: null");
                    ResultList.Add(new LineItem() { Caption = "Admin Ward", Data = Codes.AdminWard ?? "-" });
                }

                if (!string.IsNullOrEmpty(Codes.CCG))
                {
                    ResultList.Add(new LineItem() { Caption = "CCG Code", Data = Codes.CCG ?? "-" });
                    Console.WriteLine("CCG Code: null");
                }
                if (string.IsNullOrEmpty(Codes.Parish))
                {
                    ResultList.Add(new LineItem() { Caption = "Parish Code", Data = Codes.Parish ?? "-" });
                    Console.WriteLine("Parish Code: null");
                }
            }

        }
    }

    [Serializable]
    [JsonObjectAttribute]
    public class Codes
    {
        public string AdminDistrict { get; set; }
        public string AdminCounty { get; set; }
        public string AdminWard { get; set; }
        public string Parish { get; set; }
        public string CCG { get; set; }
    }
}

