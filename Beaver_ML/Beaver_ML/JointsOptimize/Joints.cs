using Beaver_ML.JointsOptimize.Info;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beaver_ML.JointsOptimize
{
    public class Joints
    {
        //每个结构的分节点的孔洞点
        public Dictionary<int, List<Point3d>> holes = new Dictionary<int, List<Point3d>>();

        //每个结构的分节点的中心点
        public Dictionary<int,Point3d> centers = new Dictionary<int, Point3d>();

        //每个节点的最终基准点
        public Point3d center { get; private set; } 

        public Joints(List<List<Point3d>> points)
        {
            for(int i = 0; i < points.Count; i++) 
            {
                holes.Add(i, points[i]);   //每个结构中添加一个节点
            }
        }

        public void removeToOneCenter()  //将所有节点的基准放在节点最多的单个结构上
        {

        }
    }
}
