using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Beaver3D.Model;
using Beaver3D.Model.CrossSections;
using Beaver.Properties;
using Rhino.Geometry;

namespace Beaver.Display
{
	// Token: 0x0200002F RID: 47
	public class ResultsComponent : GH_Component
	{
		// Token: 0x0600014F RID: 335 RVA: 0x0000AC3C File Offset: 0x00008E3C
		public ResultsComponent() : base("Display Results", "Disp Res", "shows results of optimization", "Beaver", "07 Display")
		{
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000AC9C File Offset: 0x00008E9C
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddGenericParameter("Stucture", "S", "Structure", 0);
			pManager.AddBooleanParameter("ShowResultsOnStructure", "disp", "displays information of members on the members in the structure", 0, true);
			pManager.AddTextParameter("LoadCase", "LC", "Name of the load case to visualize", 0);
			pManager[1].Optional = true;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000AD00 File Offset: 0x00008F00
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddTextParameter("OptimizationResults", "R", "key facts of optimization", 0);
			pManager.AddNumberParameter("Utilization", "U", "Utilization of each member in the structure", GH_ParamAccess.list);
			pManager.AddNumberParameter("Normal Forces", "Nx", "Normal Forces of members of the structure", GH_ParamAccess.tree);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000AD54 File Offset: 0x00008F54
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			Structure structure = new Structure();
			string item = null;
			DA.GetData<Structure>(0, ref structure);
			DA.GetData<bool>(1, ref this.showOnStructure);
			DA.GetData<string>(2, ref item);
			Structure structure2 = structure.Clone();
			this.Structure = structure2;
			string text = structure2.Results.ToString();
			List<LoadCase> loadCasesFromNames = this.Structure.GetLoadCasesFromNames(new List<string>
			{
				item
			});
			this.loadcaseNumber = loadCasesFromNames[0].Number;
			bool flag = this.loadcaseNumber < 0 && this.loadcaseNumber > structure2.LoadCases.Count;
			if (flag)
			{
				throw new ArgumentException("Choose a valid LoadCase!");
			}
			this.utilization.Clear();
			foreach (double item2 in structure2.Results.Utilization)
			{
				this.utilization.Add(item2);
			}
			int num = 0;
			this.normalForces.Clear();
			foreach (IMember member in structure2.Members)
			{
				IMember1D member1D = (IMember1D)member;
				foreach (KeyValuePair<LoadCase, List<double>> keyValuePair in member1D.Nx)
				{
					this.normalForces.Add(keyValuePair.Value[0], new GH_Path(num));
				}
				num++;
			}
			DA.SetData(0, text);
			DA.SetDataList(1, this.utilization);
			DA.SetDataTree(2, this.normalForces);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000AF2C File Offset: 0x0000912C
		public override void DrawViewportWires(IGH_PreviewArgs args)
		{
			bool flag = this.showOnStructure;
			if (flag)
			{
				Color black = Color.Black;
				int num = 0;
				foreach (IMember member in this.Structure.Members)
				{
					IMember1D member1D = (IMember1D)member;
					Line line;
					line = new Line(new Point3d(member1D.From.X, member1D.From.Y, member1D.From.Z), new Point3d(member1D.To.X, member1D.To.Y, member1D.To.Z));
					Point3d point3d;
					point3d = new Point3d(line.PointAt(0.5));
					double value = member1D.Nx[this.Structure.LoadCases[this.loadcaseNumber]][0];
					decimal num2 = Math.Round(new decimal(value), 3);
					decimal d = Math.Round(new decimal(this.Structure.Results.Utilization[num]), 2) * 100m;
					d = Math.Round(d, 0);
					string text = "N = " + num2.ToString() + "\n";
					text = text + "ID: " + member1D.Number.ToString() + "\n";
					text = text + "Util: " + d.ToString() + "%\n";
					string str = text;
					string str2 = "Sec: ";
					ICrossSection crossSection = member1D.Assignment.ElementGroups[0].CrossSection;
					text = str + str2 + ((crossSection != null) ? crossSection.ToString() : null);
					args.Display.Draw2dText(text, black, point3d, true);
					num++;
				}
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000B144 File Offset: 0x00009344
		public override void DrawViewportMeshes(IGH_PreviewArgs args)
		{
			bool flag = this.showOnStructure;
			if (flag)
			{
				Color black = Color.Black;
				int num = 0;
				foreach (IMember member in this.Structure.Members)
				{
					IMember1D member1D = (IMember1D)member;
					Line line;
					line = new Line(new Point3d(member1D.From.X, member1D.From.Y, member1D.From.Z), new Point3d(member1D.To.X, member1D.To.Y, member1D.To.Z));
					Point3d point3d;
					point3d = new Point3d(line.PointAt(0.5));
					double value = member1D.Nx[this.Structure.LoadCases[this.loadcaseNumber]][0];
					decimal num2 = Math.Round(new decimal(value), 3);
					decimal d = Math.Round(new decimal(this.Structure.Results.Utilization[num]), 2) * 100m;
					d = Math.Round(d, 0);
					string text = "N = " + num2.ToString() + "\n";
					text = text + "ID: " + member1D.Number.ToString() + "\n";
					text = text + "Util: " + d.ToString() + "%\n";
					string str = text;
					string str2 = "Sec: ";
					ICrossSection crossSection = member1D.Assignment.ElementGroups[0].CrossSection;
					text = str + str2 + ((crossSection != null) ? crossSection.ToString() : null);
					args.Display.Draw2dText(text, black, point3d, true);
					num++;
				}
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000B35C File Offset: 0x0000955C
		protected override Bitmap Icon
		{
			get
			{
				return Resources.DisplayResult_BeaverIcon;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000156 RID: 342 RVA: 0x0000B363 File Offset: 0x00009563
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("94e4dcf9-f958-41ab-baef-5fe805a8251b");
			}
		}

		// Token: 0x04000024 RID: 36
		private List<double> utilization = new List<double>();

		// Token: 0x04000025 RID: 37
		private DataTree<double> normalForces = new DataTree<double>();

		// Token: 0x04000026 RID: 38
		private bool showOnStructure = true;

		// Token: 0x04000027 RID: 39
		private Structure Structure = new Structure();

		// Token: 0x04000028 RID: 40
		private int loadcaseNumber = 0;
	}
}
