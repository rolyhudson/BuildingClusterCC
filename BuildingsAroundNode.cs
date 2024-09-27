using BH.Adapter.OpenStreetMap;
using BH.oM.Adapter;
using BH.oM.Adapters.OpenStreetMap;
using BH.oM.Data.Library;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BuildingClusterCC
{
    public static partial class Query
    {
        public static async Task BuildingsAroundNode(List<Node> nodes)
        {
            using (var client = new HttpClient())
            {
                foreach (var node in nodes)
                {
                    BH.oM.Geospatial.Point p = new BH.oM.Geospatial.Point();
                    p.Longitude = node.Longitude;
                    p.Latitude = node.Latitude;

                    BH.oM.Geospatial.Circle circle = new BH.oM.Geospatial.Circle();
                    circle.Centre = p;
                    circle.Radius = 1000;

                    try
                    {
                        
                        OpenStreetMapAdapter adapter = new OpenStreetMapAdapter();
                        OverpassRequest request = new OverpassRequest();
                        request.Category = "building";
                        request.GeospatialRegion = circle;
                        OpenStreetMapConfig config = new OpenStreetMapConfig();
                        config.ElementsToSearch = ElementsToSearch.WaysAndRelations;
                        config.OverpassEndpoint = OverpassEndpoint.Main;
                        Response osmResponse = adapter.Pull(request, actionConfig: config).First() as Response;
                        using(StreamWriter sw = new StreamWriter($"placeBuildings\\{node.Name.Replace(" ","_")}.json"))
                        {
                            foreach (var obj in osmResponse.FeatureCollection.Features)
                            {
                                sw.WriteLine(BH.Engine.Serialiser.Convert.ToJson(obj));
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
            }
        }
    }
}
