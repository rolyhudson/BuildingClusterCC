using BH.Engine.Geospatial;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.oM.Geospatial;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace BuildingClusterCC
{
    public static partial class Compute
    {
        public static Dictionary<Guid, BH.oM.Geometry.Point> ReadBuildingCentroids()
        {
            List < Feature > features = new List < Feature >();
            ConcurrentDictionary<Guid, BH.oM.Geometry.Point> utmPointDict = new ConcurrentDictionary<Guid, BH.oM.Geometry.Point> ();
            //Parallel.ForEach(Directory.GetFiles("placeBuildings"), place =>
            foreach(var place in Directory.GetFiles("placeBuildings"))
            {
                using (StreamReader sr = new StreamReader(place))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        if (line.Contains("County Square Shopping Centre"))
                        {
                            var a = 0;
                        }
                        Feature feature = BH.Engine.Serialiser.Convert.FromJson(line) as Feature;
                        IGeometry geo = feature.ToUTM(30);
                        List<BH.oM.Geometry.Point> points = new List<BH.oM.Geometry.Point>();
                        if (geo is CompositeGeometry)
                        {
                            CompositeGeometry composite = (CompositeGeometry)geo;

                            foreach (var g in composite.Elements)
                            {
                                if (g is ICurve || g is IPolyline)
                                {
                                    Polyline c = (Polyline)g;
                                    points.Add(c.ControlPoints.Average());
                                }
                                if (g is CompositeGeometry)
                                {
                                    CompositeGeometry composite2 = (CompositeGeometry)g;
                                    foreach (var g2 in composite2.Elements)
                                    {
                                        if (g2 is ICurve)
                                        {
                                            Polyline c = (Polyline)g2;
                                            points.Add(c.ControlPoints.Average());
                                        }
                                    }
                                }
                            }
                        }
                        if(geo is BH.oM.Geometry.Point)
                        {
                            points.Add(geo as BH.oM.Geometry.Point);
                        }
                        if (geo is Polyline)
                        {
                            Polyline poly = (Polyline)geo;
                            points.Add(poly.ICentroid());
                        }
                        var centroid = points.Average();
                        if (centroid != null)
                            utmPointDict.TryAdd(feature.BHoM_Guid, centroid);
                        else
                        {
                            var b = 0;
                        }

                        line = sr.ReadLine();
                    }
                }
            }//);
            
            using(StreamWriter sw = new StreamWriter("utmCentroids.json"))
            {
                foreach (var kvp in utmPointDict)
                {
                    if(kvp.Value == null)
                    {
                        continue;
                    }
                    sw.WriteLine($"{kvp.Key},{kvp.Value.X},{kvp.Value.Y}");
                }
            }
            
            return utmPointDict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public static List<Feature> ReadBuildingFeatures()
        {
            
            ConcurrentBag<Feature> features = new ConcurrentBag<Feature>();
            Parallel.ForEach(Directory.GetFiles("placeBuildings"), place =>
            {
                using (StreamReader sr = new StreamReader(place))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        Feature feature = BH.Engine.Serialiser.Convert.FromJson(line) as Feature;
                        IGeometry geo = feature.ToUTM(30);
                        features.Add(feature);

                        line = sr.ReadLine();
                    }
                }
            });

            return features.ToList();
        }
    }
}
