using System;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Testes
{
    [TestClass]
    public class GeoCoordisServiceTest
    {
        [TestMethod]
        public async System.Threading.Tasks.Task TestMethod1Async()
        {
            var result = new GeoCoordsResult();
            var name = "Atlanta, GA";
            var apiKey = "AmHl3lAb1PZtu5xW4CciCDCG0Hwia6kuOx2DsHAzW7W75oe4yQwSWsS_ZZst7zo1";
            var encodeName = WebUtility.UrlDecode(name);
            var url = $"http://dev.virtualearth.net/REST/v1/Locations?q={encodeName}&key={apiKey}";

            var client = new HttpClient();

            var json = await client.GetStringAsync(url);

            var results = JObject.Parse(json);
            var resources = results["resourceSets"][0]["resources"];
            if (!resources.HasValues)
            {
                result.Message = $"Could not find '{name}' as a location";
            }
            else
            {
                var confidence = (string)resources[0]["confidence"];
                if (confidence != "High")
                {
                    result.Message = $"Could not find a confident match for '{name}' as a location";
                }
                else
                {
                    var coords = resources[0]["geocodePoints"][0]["coordinates"];
                    result.Latitude = (double)coords[0];
                    result.Longitude = (double)coords[1];
                    result.Sucess = true;
                    result.Message = "Success";
                }
            }

            Assert.AreEqual("Success", result.Message);
        }
    }

    public class GeoCoordsResult
    {
        public bool Sucess { get; set; }
        public string Message { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
