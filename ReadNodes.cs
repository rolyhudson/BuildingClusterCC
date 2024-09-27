using BH.oM.Adapters.OpenStreetMap;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingClusterCC
{
    public static partial class Query 
    {
        public static List<Node> ReadNodes(string path)
        {
            List<Node> nodes = new List<Node>();
            using (StreamReader sr = new StreamReader(path))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    var json = JObject.Parse(line);
                    Node n = new Node();
                    n.Latitude = Convert.ToDouble(json["lat"]);
                    n.Longitude= Convert.ToDouble(json["lon"]);
                    var tags = json["tags"];
                    n.Name = tags["name"].ToString();
                    nodes.Add(n);
                    
                    line = sr.ReadLine();
                }

            }
            return nodes;
        }
    }
}
