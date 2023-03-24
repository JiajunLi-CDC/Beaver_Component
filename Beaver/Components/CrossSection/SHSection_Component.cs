using System;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model.CrossSections;
using Beaver.Properties;

namespace Beaver.Model.CrossSection
{
	// Token: 0x02000022 RID: 34
	public class SHSection_Component : GH_Component
	{
		// Token: 0x0600010B RID: 267 RVA: 0x00007B77 File Offset: 0x00005D77
		public SHSection_Component() : base("SquareHollowSection", "SHS", "Standard SH-Section profile after EN 10210", "Beaver", "02 CrossSections")
		{
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00007B9C File Offset: 0x00005D9C
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddNumberParameter("Width [mm]", "W [mm]", "SHSection width in [mm]", 0, 40.0);
			pManager.AddNumberParameter("Thickness [mm]", "T [mm]", "SHSection wall thickness [mm]", 0, 4.0);

		}

		// Token: 0x0600010D RID: 269 RVA: 0x000078A6 File Offset: 0x00005AA6
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Cross Section", "CS", "Cross Section", 0);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00007BF8 File Offset: 0x00005DF8
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			double num = 40.0;
			double num2 = 4.0;
			DA.GetData<double>(0, ref num);
			DA.GetData<double>(1, ref num2);
			DA.SetData(0, new SHSection(num / 1000.0, num2 / 1000.0));
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00007C51 File Offset: 0x00005E51
		protected override Bitmap Icon
		{
			get
			{
				return Resources.SquareHollowSection_BeaverIcon;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00007C58 File Offset: 0x00005E58
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("12a218ca-4dcf-4446-999b-84cecf514501");
			}
		}
	}
}
