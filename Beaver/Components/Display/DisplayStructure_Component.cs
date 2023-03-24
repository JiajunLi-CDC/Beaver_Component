using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;
using Beaver3D.Model;
using Beaver.Properties;

namespace Beaver.Display
{
	// Token: 0x0200002B RID: 43
	public class DisplayStructure_Component : GH_Component
	{
		// Token: 0x06000140 RID: 320 RVA: 0x0000A4AC File Offset: 0x000086AC
		public DisplayStructure_Component() : base("Display Structure", "Disp Struct", "displays a structure", "Beaver", "07 Display")
		{
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000A530 File Offset: 0x00008730
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddGenericParameter("Structure", "S", "Structure", 0);
			pManager.AddIntegerParameter("DisplayResultsType", "Type", "Type of results to show. Connect a ValueList component for options.", 0);
			pManager.AddTextParameter("LoadCase", "LC", "Name of the load case to visualize", 0);
			pManager.AddNumberParameter("DisplacementScale", "Dsc", "Scale factor for nodal displacement (default = 0)", 0, 0.0);
			pManager[2].Optional = true;
			pManager[3].Optional = true;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000A5BF File Offset: 0x000087BF
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddMeshParameter("CrossSection Meshes", "E", "Cross-sections as Meshes", GH_ParamAccess.tree);
			pManager.AddBooleanParameter("Debug", "Debug", "Debug,this should be true", 0);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000A5DC File Offset: 0x000087DC
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			Structure structure = null;
			int num = 0;
			string text = null;
			LoadCase lc = null;
			double lcscale = 0.0;
			DA.GetData<Structure>(0, ref structure);
			DA.GetData<int>(1, ref num);
			DA.GetData<string>(2, ref text);
			DA.GetData<double>(3, ref lcscale);

			List<LoadCase> loadCasesFromNames = structure.GetLoadCasesFromNames(new List<string>
			{
				text
			});
			bool flag = text != null;
			if (flag)
			{
				lc = structure.GetLoadCasesFromNames(new List<string>
				{
					text
				})[0];
			}
			bool flag2 = num != 1;
			if (flag2)
			{
				num += 2;
			}
			bool test = false;
			DataTree<GH_Mesh> structureMeshes = DisplayHelper.GetStructureMeshes(structure, (DisplayResultsType)num, lc, lcscale, ref test);

			DA.SetDataTree(0, structureMeshes);
			DA.SetData(1, test);
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000A685 File Offset: 0x00008885
		protected override Bitmap Icon
		{
			get
			{
				return Resources.DisplayStructure_BeaverIcon;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000145 RID: 325 RVA: 0x0000A68C File Offset: 0x0000888C
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("104f0eb5-83c1-4ccf-afb2-b67cd8dafe61");
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000A698 File Offset: 0x00008898
		protected override void BeforeSolveInstance()
		{
			bool flag = base.Params.Input[1].SourceCount <= 0 || base.Params.Input[1].SourceCount != 1 || !(base.Params.Input[1].Sources[0] is GH_ValueList);
			if (!flag)
			{
				GH_ValueList gh_ValueList = base.Params.Input[1].Sources[0] as GH_ValueList;
				gh_ValueList.ListMode = GH_ValueListMode.DropDown;
				bool flag2 = gh_ValueList.ListItems.Count != this.ResultType.Count;
				if (flag2)
				{
					gh_ValueList.ListItems.Clear();
					for (int i = 0; i < this.ResultType.Count; i++)
					{
						gh_ValueList.ListItems.Add(new GH_ValueListItem(this.ResultType[i], (i + 1).ToString()));
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
			}
		}

		// Token: 0x0400001E RID: 30
		private List<string> ResultType = new List<string>
		{
			"Blank",
			"Forces",
			"Utilization",
			"Reuse New",
			"Mass",
			"Environmental Impact"
		};
	}
}
