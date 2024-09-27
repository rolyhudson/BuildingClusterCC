using BH.oM.Geometry;
using BuildingClusterCC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingClusterCC
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Modify.CombineJsonL();
            //Compute.FindClustersKDTree(25,4);
            //Compute.ReadBuildingCentroids();
            //var nodes = Query.ReadNodes("placesTowns.json");

            //await Query.BuildingsAroundNode(nodes);
            //Dictionary<string, string> areaProperties = new Dictionary<string, string>();
            //Dictionary<string, string> placeProperties = new Dictionary<string, string>();

            //areaProperties.Add("name", "Kent");
            //areaProperties.Add("boundary", "administrative");
            //areaProperties.Add("admin_level", "6");

            //placeProperties.Add("place", "town");
            //await Query.PlaceNodes(areaProperties, placeProperties);
        }
    }
}
