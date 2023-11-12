using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Beaver3D;
using Beaver3D.Model;
using Grasshopper;
using Grasshopper.Kernel.Data;
using System.Drawing;

namespace Beaver.Components.Joints
{
    public class GetJoints_Component : GH_Component
    {

        public GetJoints_Component()
         : base("GetJoints", "GetJoints",
              "Get the points on the sphere joints in the structure",
              "Beaver", "08 JointsOptimize")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Structure", "S", "Structure to work on", 0);
            pManager.AddNumberParameter("Radius", "R", "Radius of Joints", 0, 1.0);
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Lines", "Line", "Lines on JointsCenter", GH_ParamAccess.tree);
            pManager.AddPointParameter("Points", "P1", "Points on Joints", GH_ParamAccess.tree);
            pManager.AddPointParameter("Points_Node", "P2", "Points on JointsCenter", GH_ParamAccess.tree);
        }

        public List<List<Node>> node_eachStructure ;
        public double iradius = 1;
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Structure istructure = null;
            double iiradius = 0;
            DA.GetData(0, ref istructure);
            DA.GetData(1, ref iiradius);

            this.iradius = iiradius;

            if (istructure == null)
            {
                return;
            }

            node_eachStructure = new List<List<Node>>();  //节点分组

            for(int i = 0; i < istructure.merge_structure_num; i++)
            {
                List<Node> eachStructure = new List<Node>();
                foreach (Node node in istructure.Nodes)
                {
                    if (node.structure_num == i)
                    {
                        eachStructure.Add(node);
                    }
                }
                node_eachStructure.Add(eachStructure);
            }

            List<List<List<Point3d>>> points_eachNode01 = new List<List<List<Point3d>>>();

            for (int i = 0; i < node_eachStructure.Count; i++)  //对于每个结构
            {
                List<List<Point3d>> points_eachNode02 = new List<List<Point3d>>();  //记录每个节点的球形节点表面点
                for (int j = 0; j < node_eachStructure[i].Count; j++)  //对于每个结构的节点
                {
                    Node node = node_eachStructure[i][j];
                    Dictionary<IMember, double> connectInfomation = node.ConnectedMembers;   //获取连接信息

                    List<IMember> members_node = new List<IMember>(connectInfomation.Keys);
                    List<double> values = new List<double>(connectInfomation.Values);

                    List<Point3d> points_eachNode03 = new List<Point3d>();  //记录每个节点的球形节点表面点
                    for (int k = 0; k < members_node.Count; k++)
                    {
                        IMember member = members_node[k];
                        Bar bar = member as Bar;
                        double oren = values[k];

                        if (Math.Abs(1 - oren) < 0.01)  //杆件的结束点等于节点
                        {
                            Point3d p = new Point3d(bar.From.X, bar.From.Y, bar.From.Z);
                            Point3d p2 = new Point3d(bar.To.X, bar.To.Y, bar.To.Z);
                            Vector3d vec = new Vector3d(p - p2);
                            vec.Unitize();
                            p2 += vec * iradius;
                            points_eachNode03.Add(p2);
                        }
                        else  //杆件出发点等于节点
                        {
                            Point3d p = new Point3d(bar.To.X, bar.To.Y, bar.To.Z);
                            Point3d p2 = new Point3d(bar.From.X, bar.From.Y, bar.From.Z);
                            Vector3d vec = new Vector3d(p - p2);
                            vec.Unitize();
                            p2 += vec * iradius;
                            points_eachNode03.Add(p2);
                        }                   
                    }
                    points_eachNode02.Add(points_eachNode03);
                }
                points_eachNode01.Add(points_eachNode02);
            }

            DataTree<Point3d> pointsTree_output = new DataTree<Point3d>();  //节点表面点
            for (int i = 0; i < points_eachNode01.Count; i++)  //对于每个结构
            {
                for (int j = 0; j < points_eachNode01[i].Count; j++)  //对于每个结构的节点
                {
                    GH_Path path = new GH_Path(i, j);
                    pointsTree_output.AddRange(points_eachNode01[i][j], path);
                }
            }

            DataTree<Point3d> pointsTree_output2 = new DataTree<Point3d>();  //节点中心点
            for (int i = 0; i < node_eachStructure.Count; i++)  //对于每个结构
            {
                for (int j = 0; j < node_eachStructure[i].Count; j++)  //对于每个结构的节点
                {
                    GH_Path path = new GH_Path(i, j);
                    Node node = node_eachStructure[i][j];
                    Point3d p = new Point3d(node.X, node.Y, node.Z);
                    List<Point3d> points = new List<Point3d>();
                    points.Add(p);
                    pointsTree_output2.AddRange(points, path);
                }
            }

            DataTree<Line> curves_output = new DataTree<Line>();  //  输出线
            for (int i = 0; i < points_eachNode01.Count; i++)  //对于每个结构
            {
                for (int j = 0; j < points_eachNode01[i].Count; j++)  //对于每个结构的节点
                {
                    GH_Path path = new GH_Path(i, j);

                    List<Line> lines = new List<Line>();
                    Node node = node_eachStructure[i][j];
                    Point3d p = new Point3d(node.X, node.Y, node.Z); //中心点
                    for (int k = 0; k < points_eachNode01[i][j].Count; k++)  //对于每个结构的节点(表面)
                    {
                        Point3d pa = points_eachNode01[i][j][k];
                        Line line = new Line(p, pa);
                        lines.Add(line);
                    }

                    curves_output.AddRange(lines, path);
                }
            }

            DA.SetDataTree(0, curves_output);
            DA.SetDataTree(1, pointsTree_output);
            DA.SetDataTree(2, pointsTree_output2);
        }

        // 画出节点球
        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            Color limeGreen = Color.LimeGreen;
            for (int i = 0; i < node_eachStructure.Count; i++)  //对于每个结构
            {
                for (int j = 0; j < node_eachStructure[i].Count; j++)  //对于每个结构的节点
                {
                    Node node = node_eachStructure[i][j];
                    Point3d p = new Point3d(node.X, node.Y, node.Z);
                    Sphere sphere = new Sphere(p, this.iradius);
                    args.Display.DrawSphere(sphere, limeGreen);
                }
            }        
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c02ba5f5-bd80-4497-ade3-36a9b558fb74"); }
        }
    }
}