using BH.oM.Data.Collections;
using BH.oM.Dimensional;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Engine.Data;
using Accord.Collections;
using BH.oM.Geospatial;

namespace BuildingClusterCC
{
    public static partial class Compute
    {
        public static void FindClustersDomainTree(double tolerance, int minMembers)
        {
            List<BH.oM.Geometry.Point> points = new List<BH.oM.Geometry.Point>();
            using (StreamReader sr = new StreamReader("utmCentroids.json"))
            {
                
                string line = sr.ReadLine();
                while (line != null)
                {
                    var parts = line.Split(',');
                    double x = double.Parse(parts[1]);
                    double y = double.Parse(parts[2]);
                    points.Add(new BH.oM.Geometry.Point() { X = x, Y = y });
                    line = sr.ReadLine();
                }
            }


            //Func<IElement, DomainBox> toDomainBox = x => x.IBounds().DomainBox();
            
            var clusters = BH.Engine.Data.Compute.DomainTreeClusters(points, 
                x => x.IBounds().DomainBox(),
                (a, b) => a.SquareDistance(b) < tolerance * tolerance,
                (x, y) => x.SquareDistance(y) < tolerance * tolerance,
                minMembers);
         }

        public static void FindClustersKDTree(double tolerance, int minMembers)
        {
            
            Dictionary<Guid, BH.oM.Geometry.Point> utmDict = Compute.ReadBuildingCentroids();
            List<BH.oM.Geometry.Point> points = utmDict.Values.ToList();   
            List<Feature> features = Compute.ReadBuildingFeatures();
            List<double[]> kdpoints = new List<double[]>();

            foreach (BH.oM.Geometry.Point p in points)
            {
                if (p == null)
                {
                    //utmDict.Remove()
                    continue;
                }
                double[] pt = new double[] { p.X, p.Y };
                kdpoints.Add(pt);
            }
            m_KDTree = KDTree.FromData<BH.oM.Geometry.Point>(kdpoints.ToArray(), points.ToArray(), true);

            double[] query = { points[0].X, points[0].Y };
            //first get the neighbourhood around the current spec
            var neighbours = m_KDTree.Nearest(query, radius: tolerance);

        }
        private static KDTree<BH.oM.Geometry.Point> m_KDTree;
    }
}
