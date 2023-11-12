using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;
using Beaver3D.Model;
using Beaver3D.Reuse;
using Beaver.Properties;
using Rhino.Geometry;

namespace Beaver.Display
{
    public class MoveNodes_Component : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public MoveNodes_Component()
          : base("MoveNodes", "MoveNodes",
              "移动节点",
              "Beaver", "07 Display")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Structure", "S", "Structure", 0);
            pManager.AddVectorParameter("MoveVector", "V", "移动的向量", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Line", "Line", "杆件Debug", GH_ParamAccess.list);
            pManager.AddNumberParameter("StockMemberLenth", "StockMemberLenth", "杆件对应库存杆件长度", GH_ParamAccess.list);
            pManager.AddNumberParameter("MemberSection", "MemberSection", "杆件对应截面", GH_ParamAccess.list);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Structure structure = null;
           
            List<Vector3d> moveVec = new List<Vector3d>();
            List<double> memberSection = new List<double>();  //截面

            bool data = DA.GetData<Structure>(0, ref structure);  
            DA.GetDataList<Vector3d>(1, moveVec);  //移动向量获取
            Structure structure_new = structure.Clone3();

            List<Line> members = new List<Line>();
            List<double> StockChooseLength = new List<double>();
            if (data)
            {
                for (int i = 0; i < structure_new.Nodes.Count; i++)
                {
                    if (!structure_new.Nodes[i].FixTx)
                    {
                        structure_new.Nodes[i].X += moveVec[i].X;
                    }
                    if (!structure_new.Nodes[i].FixTy)
                    {
                        structure_new.Nodes[i].Y += moveVec[i].Y;
                    }
                    if (!structure_new.Nodes[i].FixTz)
                    {
                        structure_new.Nodes[i].Z += moveVec[i].Z;
                    }
                }
                for (int i = 0; i < structure_new.Members.Count; i++)
                {
                    Bar bar = structure_new.Members[i] as Bar;
                    int nodeIndex_start = bar.From.Number;
                    Node startNode = structure_new.Nodes[nodeIndex_start];
                    int nodeIndex_end = bar.To.Number;
                    Node endNode = structure_new.Nodes[nodeIndex_end];

                    Point3d startPoint = new Point3d(startNode.X, startNode.Y, startNode.Z);
                    Point3d endPoint = new Point3d(endNode.X, endNode.Y, endNode.Z);

                    Line line = new Line(startPoint, endPoint);

                    double StockMemberLenth = bar.Assignment.ElementGroups[0].Length; //原始库存构件长度，这里是计算之后保存在一个list中的
                    double section = bar.CrossSection.dy;

                    StockChooseLength.Add(StockMemberLenth);
                    members.Add(line);
                    memberSection.Add(section);
                }
            }

            DA.SetDataList(0, members);
            DA.SetDataList(1, StockChooseLength);
            DA.SetDataList(2, memberSection);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
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
            get { return new Guid("9f200971-b5c6-4c97-8b20-7601c8433fc9"); }
        }
    }
}