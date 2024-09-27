using BH.Adapter.HTTP;
using BH.Engine.Adapters.OpenStreetMap;
using BH.oM.Adapters.HTTP;
using BH.oM.Adapters.OpenStreetMap;
using BH.oM.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BuildingClusterCC
{
    public static partial class Query
    {
        public static async Task PlaceNodes(Dictionary<string,string> areaProperties, Dictionary<string, string> placeProperties )
        {
            TaggedArea taggedArea = new TaggedArea()
            {
                KeyValues = areaProperties,
            };

            Node node = new Node()
            {
                KeyValues = placeProperties
            };

            QueryBuilder qb = Create.QueryBuilder(taggedArea, new List<IOpenStreetMapElement>() { node });


            using (var client = new HttpClient())
            {
                try
                {
                    // Send a GET request to the specified Uri  
                    HttpResponseMessage response = await client.GetAsync(qb.QueryString);

                    // Ensure we receive a successful response.  
                    response.EnsureSuccessStatusCode();

                    // Read the response content as a string  
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Parse the response body using Newtonsoft.Json  
                    var json = JObject.Parse(responseBody);
                    using (StreamWriter sw = new StreamWriter("placesTowns.json"))
                    {
                        // Iterate through the "elements" property of the parsed JSON  
                        foreach (var element in json["elements"])
                        {
                            sw.WriteLine(element.ToString(Formatting.None));
                        }
                    }
                    
                }
                catch (HttpRequestException e)
                {
                    // Handle any errors that occur during the request  
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }

            //CustomObject custom = objs.First() as CustomObject; 
        }
    }
}
