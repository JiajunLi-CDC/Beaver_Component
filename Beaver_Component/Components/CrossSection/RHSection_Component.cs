using System;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model.CrossSections;
using Beaver.Properties;

namespace Beaver.Model.CrossSection
{
	// Token: 0x02000021 RID: 33
	public class RHSection_Component : GH_Component
	{
		// Token: 0x06000105 RID: 261 RVA: 0x00007A58 File Offset: 0x00005C58
		public RHSection_Component() : base("RectangularHollowSection", "RHS", "Standard RH-Section profile after EN 10210", "Beaver", "02 CrossSections")
		{
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00007A7C File Offset: 0x00005C7C
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddNumberParameter("Height [mm]", "H [mm]", "RHSection height in [mm]", 0, 50.0);
			pManager.AddNumberParameter("Width [mm]", "W [mm]", "RHSection width in [mm]", 0, 30.0);
			pManager.AddNumberParameter("Thickness [mm]", "T [mm]", "RHSection wall thickness [mm]", 0, 4.0);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000078A6 File Offset: 0x00005AA6
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Cross Section", "CS", "Cross Section", 0);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00007AEC File Offset: 0x00005CEC
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			double num = 50.0;
			double num2 = 30.0;
			double num3 = 4.0;
			DA.GetData<double>(0, ref num);
			DA.GetData<double>(1, ref num2);
			DA.GetData<double>(2, ref num3);
			DA.SetData(0, new RHSection(num / 1000.0, num2 / 1000.0, num3 / 1000.0));
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00007B64 File Offset: 0x00005D64
		protected override Bitmap Icon
		{
			get
			{
				return Resources.Rectangle_hollow_section_BeaverIcon;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00007B6B File Offset: 0x00005D6B
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("12a218ca-4dcf-4446-999b-84cecf51df77");
			}
		}
	}
}
