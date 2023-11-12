using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;
using Beaver3D.LCA;
using Beaver3D.Model;
using Beaver3D.Optimization;
using Beaver3D.Optimization.TopologyOptimization;
using Beaver3D.Optimization.GeometryOptimization;
using Beaver3D.Reuse;
using Beaver.Properties;
using Rhino.Geometry;

namespace Beaver.Optimization
{
    public class GeogmetryReuseOptimize_Component : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GeogmetryReuseOptimize class.
        /// </summary>
        public GeogmetryReuseOptimize_Component()
          : base("GeogmetryReuseOptimize", "GRO",
              "结构优化",
              "Beaver", "08 Test")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Structure", "S", "Structure to work on", 0);
            pManager.AddGenericParameter("Stock", "Stock", "Cross Section Catalog", 0);
            pManager.AddIntegerParameter("Objective", "Obj", "Objective function: 0 = Min structure mass, 1 = compliance", 0, 0);
            pManager.AddTextParameter("LoadCase Names", "LC", "Name of the loadcases to consider. Use 'all' to consider all load cases in the structure.", GH_ParamAccess.list, new List<string>
            {
                "all"
            });
            pManager.AddGenericParameter("DSCO Options", "DSCOOpt", "Discret Stock Constrained Optimization Options", 0);
            pManager.AddBooleanParameter("Run", "Run", "Run", 0, false);
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Structure_new", "S", "The optimized structure", 0);
            pManager.AddNumberParameter("Objective value", "ObjVal", "The optimal objective function value", 0);
            pManager.AddGenericParameter("Structure_origin", "S", "The optimized structure", 0);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool flag = false;
            Structure structure = null;
            Stock stock = null;
            List<string> list = new List<string>();
            int num = 0;  //默认优化目标为最小结构质量
            OptimOptions options = new OptimOptions();

            DA.GetData<Structure>(0, ref structure);
            DA.GetData<Stock>(1, ref stock);
            DA.GetData<int>(2, ref num);
            DA.GetDataList<string>(3, list);  //荷载的信息
            DA.GetData<OptimOptions>(4, ref options);
            DA.GetData<bool>(5, ref flag);  //是否计算

            Structure structure_new = structure.Clone3();
            stock = stock.Clone();
            num--;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            JJGeometryOptimization geogmetryOptimization = new JJGeometryOptimization((Objective)num, options);

            bool flag2 = flag;
            if (flag2)
            {
                geogmetryOptimization.Solve(structure_new,  stock, new GHGFrontiers());   //gurobi计算

                bool interrupted = geogmetryOptimization.Interrupted;  //如果计算被终止
                if (interrupted)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "The optimization has been terminated by the user");
                }

                //for (int i = 0; i < structure_new.Nodes.Count; i++)
                //{
                //    //    //Node node = Structure.Nodes[i];

                //    structure_new.Nodes[i].X = 0;      //输出结果,改变每个node的位置
                //    structure_new.Nodes[i].Y = 0;
                //    //structure.Nodes[i].Z = 2;

                ////    //Structure.Nodes[i] = node;
                //}
                //输出结果

                DA.SetData(0, structure_new);
                DA.SetData(1, new GH_Number(geogmetryOptimization.ObjectiveValue));
                DA.SetData(2, structure);

                stopwatch.Stop();
            }
            else
            {
                DA.SetData(0, structure);
                DA.SetData(1, 0);
                DA.SetData(2, structure);
            }
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
            get { return new Guid("00bffe48-f398-45d5-bb0e-33b8a1c9014b"); }
        }
    }
}