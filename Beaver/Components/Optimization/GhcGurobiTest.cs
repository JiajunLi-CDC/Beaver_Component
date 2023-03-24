using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Grasshopper.Kernel.Data;
using Grasshopper;
using Beaver.ReuseScripts.Optimization;

namespace Beaver.Components.Optimization
{
    public class GhcGurobiTest : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GhcCheckPanelCluster class.
        /// </summary>
        public GhcGurobiTest()
            : base("GurobiTest", "GurobiTest", "GurobiTest测试", "Beaver", "Optimization")
        {
        }


        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Solve", "Solve", "重置计算", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("OptimizationResults", "R", "key facts of optimization", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            BilinearTest bilinearTest;
            bool iSolve = true;

            DA.GetData("Solve", ref iSolve);


            //if (iSolve == false)
            //{
            //    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Failed to collect any valid GyresMesh.");
            //}

            //=============================================================================================
            bilinearTest = new BilinearTest();
            List<string> result = new List<string>();

            if (iSolve)
            {
                bilinearTest.Solve(ref result);
            }

            //=============================================================================================

            DA.SetDataList("OptimizationResults", result);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Gurobi_BeaverIcon;
            }
        }


        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("a1cb546a-2030-4d23-b439-144fd448f1cd");
            }
        }
    }
}
