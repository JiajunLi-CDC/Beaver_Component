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
using Beaver3D.Reuse;
using Beaver.Properties;
using Rhino.Geometry;

namespace Beaver.Optimization
{
	// Token: 0x0200000A RID: 10
	public class DiscreteStockConstrainedOptimization_Component : GH_Component
	{
		// Token: 0x06000063 RID: 99 RVA: 0x00003660 File Offset: 0x00001860
		public DiscreteStockConstrainedOptimization_Component() : base("Discrete Stock Constrained Optimization", "DSCopt", "Perform a MILP truss stock constrained optimization with discrete stock", "Beaver", "06 Optimization")
		{
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000036CC File Offset: 0x000018CC
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

		// Token: 0x06000065 RID: 101 RVA: 0x000037B0 File Offset: 0x000019B0
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Structure", "S", "The optimized structure", 0);
			pManager.AddNumberParameter("Objective value", "ObjVal", "The optimal objective function value", 0);
			pManager.AddNumberParameter("Bar areas", "A", "The cross section areas of the optimized members", GH_ParamAccess.list);
			pManager.AddNumberParameter("Bar normal forces", "F", "The bar normal forces for each load case", GH_ParamAccess.list);
			pManager.AddPointParameter("LowerBounds", "LB", "LowerBounds", GH_ParamAccess.list);
			pManager.AddPointParameter("UpperBounds", "UB", "UpperBounds", GH_ParamAccess.list);
			pManager.AddGenericParameter("Stock", "Stock", "The stock", 0);
			pManager.AddNumberParameter("Runtime", "T", "Runtime", 0);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003878 File Offset: 0x00001A78
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			bool flag = false;
			Structure structure = null;
			Stock stock = null;
			List<string> list = new List<string>();
			int num = 0;
			OptimOptions options = new OptimOptions();
			List<GH_Point> list2 = new List<GH_Point>();
			List<GH_Point> list3 = new List<GH_Point>();
			DA.GetData<Structure>(0, ref structure);
			DA.GetData<Stock>(1, ref stock);
			DA.GetData<int>(2, ref num);
			DA.GetDataList<string>(3, list);
			DA.GetData<OptimOptions>(4, ref options);
			DA.GetData<bool>(5, ref flag);
			structure = structure.Clone();
			stock = stock.Clone();
			num--;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			DiscreteStockConstrainedOptimization discreteStockConstrainedOptimization = new DiscreteStockConstrainedOptimization((Objective)num, options);

			bool flag2 = flag;
			if (flag2)
			{
				discreteStockConstrainedOptimization.Solve(structure, list, stock, new GHGFrontiers());   //gurobi计算

				bool interrupted = discreteStockConstrainedOptimization.Interrupted;  //如果计算被终止
				if (interrupted)
				{
					this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "The optimization has been terminated by the user");
				}

				//输出结果

				List<GH_Number> list4 = new List<GH_Number>();
				DataTree<GH_Number> dataTree = new DataTree<GH_Number>();
				foreach (IMember member in structure.Members)
				{
					Bar bar = (Bar)member;
					list4.Add(new GH_Number(bar.CrossSection.Area));
					foreach (LoadCase key in structure.GetLoadCasesFromNames(list))
					{
						dataTree.Add(new GH_Number(bar.Nx[key][0]), new GH_Path(bar.Number));
					}
				}


				for (int i = 0; i < discreteStockConstrainedOptimization.LowerBounds.Count; i++)
				{
					bool flag3 = discreteStockConstrainedOptimization.UpperBounds[i].Item2 > 1E+99 && discreteStockConstrainedOptimization.LowerBounds[i].Item2 > -1E+99;
					if (flag3)
					{
						list2.Add(new GH_Point(new Point3d(discreteStockConstrainedOptimization.LowerBounds[i].Item1, discreteStockConstrainedOptimization.LowerBounds[i].Item2, 0.0)));
					}
					else
					{
						bool flag4 = discreteStockConstrainedOptimization.LowerBounds[i].Item2 < -1E+99 && discreteStockConstrainedOptimization.UpperBounds[i].Item2 < 1E+99;
						if (flag4)
						{
							list3.Add(new GH_Point(new Point3d(discreteStockConstrainedOptimization.UpperBounds[i].Item1, discreteStockConstrainedOptimization.UpperBounds[i].Item2, 0.0)));
						}
						else
						{
							list2.Add(new GH_Point(new Point3d(discreteStockConstrainedOptimization.LowerBounds[i].Item1, discreteStockConstrainedOptimization.LowerBounds[i].Item2, 0.0)));
							list3.Add(new GH_Point(new Point3d(discreteStockConstrainedOptimization.UpperBounds[i].Item1, discreteStockConstrainedOptimization.UpperBounds[i].Item2, 0.0)));
						}
					}
				}
				DA.SetData(0, structure);
				DA.SetData(1, new GH_Number(discreteStockConstrainedOptimization.ObjectiveValue));
				DA.SetDataList(2, list4);
				DA.SetDataTree(3, dataTree);
				DA.SetDataList(4, list2);
				DA.SetDataList(5, list3);
				DA.SetData(6, stock);
				DA.SetData(7, stopwatch.ElapsedMilliseconds);
				stopwatch.Stop();
			}
			else
			{
				DA.SetData(0, structure);
				DA.SetData(6, stock);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003C7C File Offset: 0x00001E7C
		protected override Bitmap Icon
		{
			get
			{
				return Resources.DSCO_Solver_BeaverIcon;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003C83 File Offset: 0x00001E83
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("651c8323-b797-47d3-ba81-340f2b9a9c03");
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003C90 File Offset: 0x00001E90
		protected override void BeforeSolveInstance()
		{
			bool flag = base.Params.Input[2].SourceCount <= 0 || base.Params.Input[2].SourceCount != 1 || !(base.Params.Input[2].Sources[0] is GH_ValueList);
			if (!flag)
			{
				GH_ValueList gh_ValueList = base.Params.Input[2].Sources[0] as GH_ValueList;
				// 在gh_ValueList输入的情况下选择下拉列表的模式：
				gh_ValueList.ListMode = GH_ValueListMode.DropDown;
				bool flag2 = gh_ValueList.ListItems.Count != this.Objectives.Count;
				if (flag2)
				{
					gh_ValueList.ListItems.Clear();
					for (int i = 0; i < this.Objectives.Count; i++)
					{
						gh_ValueList.ListItems.Add(new GH_ValueListItem(this.Objectives[i], i.ToString()));
					}
					gh_ValueList.ExpireSolution(true);
				}
				else
				{
					bool flag3 = true;
					for (int j = 0; j < this.Objectives.Count; j++)
					{
						bool flag4 = gh_ValueList.ListItems[j].Name != this.Objectives[j];
						if (flag4)
						{
							flag3 = false;
							break;
						}
					}
					bool flag5 = !flag3;
					if (flag5)
					{
						gh_ValueList.ListItems.Clear();
						for (int k = 0; k < this.Objectives.Count; k++)
						{
							gh_ValueList.ListItems.Add(new GH_ValueListItem(this.Objectives[k], (k + 1).ToString()));
						}
						gh_ValueList.ExpireSolution(true);
					}
				}
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00003E6C File Offset: 0x0000206C
		public override GH_Exposure Exposure
		{
			get
			{
				return GH_Exposure.quarternary;
			}
		}

		// Token: 0x04000003 RID: 3
		private List<string> Objectives = new List<string>
		{
			"MinStructureMass",
			"MinStockMass",
			"MinWaste",
			"MinLCA"
		};
	}
}
