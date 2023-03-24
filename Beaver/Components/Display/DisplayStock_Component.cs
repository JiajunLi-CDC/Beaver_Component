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
	// Token: 0x0200002A RID: 42
	public class DisplayStock_Component : GH_Component
	{
		// Token: 0x06000139 RID: 313 RVA: 0x00009CFC File Offset: 0x00007EFC
		public DisplayStock_Component() : base("Display Stock", "Disp Stock", "displays the stock", "Beaver", "07 Display")
		{
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00009DAC File Offset: 0x00007FAC
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddGenericParameter("Structure", "S", "Structure", 0);
			pManager.AddGenericParameter("Stock", "Stock", "Stock", 0);
			pManager.AddPlaneParameter("Plane", "Plane", " plane on which the stock elements are displayed", 0, default(Plane));
			pManager.AddVectorParameter("SpacingVector", "dx", "spacing between stock elements", 0, new Vector3d(1.0, 1.0, 0.0));
			pManager.AddIntegerParameter("StockType", "StockType", "connect a value list for options", 0);
			pManager.AddIntegerParameter("ResultsType", "Type", "connect a value list for options", 0);
			pManager.AddTextParameter("LoadCase", "LC", "", 0);
			pManager[0].Optional = true;
			pManager[2].Optional = true;
			pManager[3].Optional = true;
			pManager[6].Optional = true;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00009EBC File Offset: 0x000080BC
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddMeshParameter("CutOff", "CutOff", "StockElements", GH_ParamAccess.tree);
			pManager.AddMeshParameter("UsedElements", "E", "UsedElements", GH_ParamAccess.tree);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00009EF0 File Offset: 0x000080F0
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			Stock stock = null;
			Structure structure = null;
			Plane anchorPlane = default(Plane);
			Vector3d vector3d = default(Vector3d);
			int num = 2;
			int num2 = 1;
			string item = null;
			LoadCase lc = null;
			DA.GetData<Stock>(1, ref stock);
			bool data = DA.GetData<Structure>(0, ref structure);
			DA.GetData<Plane>(2, ref anchorPlane);
			DA.GetData<Vector3d>(3, ref vector3d);
			DA.GetData<int>(4, ref num);
			DA.GetData<int>(5, ref num2);
			bool data2 = DA.GetData<string>(6, ref item);
			if (data2)
			{
				lc = structure.GetLoadCasesFromNames(new List<string>
				{
					item
				})[0];
			}
			bool flag = num2 != 1;
			if (flag)
			{
				num2 += 2;
			}
			Stock stock2 = stock;
			stock2.ResetNext();
			stock2.ResetStacks();
			stock2.ResetRemainLenghtsTemp();
			stock2.ResetRemainLenghts();
			DataTree<GH_Mesh> dataTree = new DataTree<GH_Mesh>();
			DataTree<GH_Mesh> dataTree2 = new DataTree<GH_Mesh>();
			bool flag2 = !data;
			if (flag2)
			{
				dataTree = DisplayHelper.GetStockMeshesOnly(stock2, (DisplayResultsType)num2, anchorPlane.Clone(), vector3d.X, vector3d.Y);
			}
			else
			{
				Structure structure2 = structure;
				switch (num)
				{
				case 0:
					dataTree = DisplayHelper.GetStockMeshesOnly(stock2, (DisplayResultsType)num2, anchorPlane, vector3d.X, vector3d.Y);
					break;
				case 1:
				{
					DataTree<GH_Mesh>[] stockMeshesFull = DisplayHelper.GetStockMeshesFull(stock2, structure2, (DisplayResultsType)num2, lc, anchorPlane, vector3d.X, vector3d.Y);
					dataTree = stockMeshesFull[0];
					dataTree2 = stockMeshesFull[1];
					break;
				}
				case 2:
				{
					DataTree<GH_Mesh>[] stockMeshesUsed = DisplayHelper.GetStockMeshesUsed(stock2, structure2, (DisplayResultsType)num2, lc, anchorPlane, vector3d.X, vector3d.Y);
					dataTree = stockMeshesUsed[0];
					dataTree2 = stockMeshesUsed[1];
					break;
				}
				default:
					dataTree = DisplayHelper.GetStockMeshesOnly(stock2, (DisplayResultsType)num2, anchorPlane, vector3d.X, vector3d.Y);
					break;
				}
			}
			DA.SetDataTree(0, dataTree);
			DA.SetDataTree(1, dataTree2);
			stock2.ResetNext();
			stock2.ResetStacks();
			stock2.ResetRemainLenghtsTemp();
			stock2.ResetRemainLenghts();
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600013D RID: 317 RVA: 0x0000A0E5 File Offset: 0x000082E5
		protected override Bitmap Icon
		{
			get
			{
				return Resources.DisplayStock_BeaverIcon;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600013E RID: 318 RVA: 0x0000A0EC File Offset: 0x000082EC
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("d8ef9052-8623-4b4a-a7d4-c1baa3350cee");
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000A0F8 File Offset: 0x000082F8
		protected override void BeforeSolveInstance()
		{
			bool flag = base.Params.Input[5].SourceCount <= 0 || base.Params.Input[5].SourceCount != 1 || !(base.Params.Input[5].Sources[0] is GH_ValueList);
			if (!flag)
			{
				GH_ValueList gh_ValueList = base.Params.Input[5].Sources[0] as GH_ValueList;
				gh_ValueList.ListMode = GH_ValueListMode.DropDown;
				bool flag2 = gh_ValueList.ListItems.Count != this.ResultType.Count;
				if (flag2)
				{
					gh_ValueList.ListItems.Clear();
					for (int i = 0; i < this.ResultType.Count; i++)
					{
						gh_ValueList.ListItems.Add(new GH_ValueListItem(this.ResultType[i], i.ToString()));
					}
					gh_ValueList.ExpireSolution(true);
				}
				else
				{
					bool flag3 = true;
					for (int j = 0; j < this.ResultType.Count; j++)
					{
						bool flag4 = gh_ValueList.ListItems[j].Name != this.ResultType[j];
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
						for (int k = 0; k < this.ResultType.Count; k++)
						{
							gh_ValueList.ListItems.Add(new GH_ValueListItem(this.ResultType[k], (k + 1).ToString()));
						}
						gh_ValueList.ExpireSolution(true);
					}
				}
				bool flag6 = base.Params.Input[4].SourceCount <= 0 || base.Params.Input[4].SourceCount != 1 || !(base.Params.Input[4].Sources[0] is GH_ValueList);
				if (!flag6)
				{
					GH_ValueList gh_ValueList2 = base.Params.Input[4].Sources[0] as GH_ValueList;
					gh_ValueList.ListMode = GH_ValueListMode.DropDown;
					bool flag7 = gh_ValueList2.ListItems.Count != this.StockType.Count;
					if (flag7)
					{
						gh_ValueList2.ListItems.Clear();
						for (int l = 0; l < this.StockType.Count; l++)
						{
							gh_ValueList2.ListItems.Add(new GH_ValueListItem(this.StockType[l], l.ToString()));
						}
						gh_ValueList2.ExpireSolution(true);
					}
					else
					{
						bool flag8 = true;
						for (int m = 0; m < this.StockType.Count; m++)
						{
							bool flag9 = gh_ValueList2.ListItems[m].Name != this.StockType[m];
							if (flag9)
							{
								flag8 = false;
								break;
							}
						}
						bool flag10 = !flag8;
						if (flag10)
						{
							gh_ValueList2.ListItems.Clear();
							for (int n = 0; n < this.StockType.Count; n++)
							{
								gh_ValueList2.ListItems.Add(new GH_ValueListItem(this.StockType[n], (n + 1).ToString()));
							}
							gh_ValueList2.ExpireSolution(true);
						}
					}
				}
			}
		}

		// Token: 0x0400001C RID: 28
		private List<string> ResultType = new List<string>
		{
			"Blank",
			"Forces",
			"Utilization",
			"Reuse New",
			"Mass",
			"Environmental Impact"
		};

		// Token: 0x0400001D RID: 29
		private List<string> StockType = new List<string>
		{
			"Full Stock Blank",
			"Full Stock",
			"Used Stock"
		};
	}
}
