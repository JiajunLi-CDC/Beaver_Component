using System;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model.CrossSections;
using Beaver.Properties;

namespace Beaver.Components.CrossSection
{
	// Token: 0x02000023 RID: 35
	public class CircularHollowSection_Component : GH_Component
	{
		// Token: 0x06000111 RID: 273 RVA: 0x00007C64 File Offset: 0x00005E64
		public CircularHollowSection_Component() : base("CircularHollowSection", "CHS", "Defines a circular hollow section with diameter D and wall thickness T", "Beaver", "02 CrossSections")
		{
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00007C88 File Offset: 0x00005E88
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddNumberParameter("Diameter [mm]", "D [mm]", "Circular section diameter in [mm]", 0, 100.0);
			pManager.AddNumberParameter("WallThickness [mm]", "T [mm]", "Wall thickness in [mm]", 0, 10.0);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000078A6 File Offset: 0x00005AA6
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Cross Section", "CS", "Cross Section", 0);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00007CD8 File Offset: 0x00005ED8
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			double num = 100.0;
			double num2 = 10.0;
			DA.GetData<double>(0, ref num);
			DA.GetData<double>(1, ref num2);
			DA.SetData(0, new CircularHollowSection(num / 1000.0, num2 / 1000.0));
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00007D31 File Offset: 0x00005F31
		protected override Bitmap Icon
		{
			get
			{
				return Resources.CircularHollowSection_BeaverIcon;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00007D38 File Offset: 0x00005F38
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("8e617b0c-b658-464e-958e-a27b6cea0f14");
			}
		}
	}
}
