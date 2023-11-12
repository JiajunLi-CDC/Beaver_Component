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
    public class DisplayMember_Component : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public DisplayMember_Component()
          : base("DisplayMember", "DisplayMember",
              "杆件的基础显示",
              "Beaver", "07 Display")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Structure", "S", "Structure", 0);
            pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Line", "Line", "杆件Debug", GH_ParamAccess.list);
            pManager.AddNumberParameter("StockMemberLenth", "StockMemberLenth", "杆件对应库存杆件长度", GH_ParamAccess.list);
        }

       
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Structure structure = null;
            bool data = DA.GetData<Structure>(0, ref structure);

            List<Line> members = new List<Line>();
            List<double> StockChooseLength = new List<double>();
            if (data)
            {     
                for (int i = 0; i < structure.Members.Count; i++)
                {
                    Bar bar = structure.Members[i] as Bar;
                    int nodeIndex_start = bar.From.Number;
                    Node startNode = structure.Nodes[nodeIndex_start];
                    int nodeIndex_end = bar.To.Number;
                    Node endNode = structure.Nodes[nodeIndex_end];

                    Point3d startPoint = new Point3d(startNode.X, startNode.Y, startNode.Z);
                    Point3d endPoint = new Point3d(endNode.X, endNode.Y, endNode.Z);

                    Line line = new Line(startPoint, endPoint);

                    double StockMemberLenth = bar.Assignment.ElementGroups[0].Length; //原始库存构件长度，这里是计算之后保存在一个list中的

                    StockChooseLength.Add(StockMemberLenth);
                    members.Add(line);
                } 
            }

            DA.SetDataList(0, members);
            DA.SetDataList(1, StockChooseLength);
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
            get { return new Guid("9f200971-b5c6-4c97-8b20-7601c8953fc9"); }
        }
    }
}