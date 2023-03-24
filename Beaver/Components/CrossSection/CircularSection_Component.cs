using System;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model.CrossSections;
using Beaver.Properties;

namespace Beaver.Model.CrossSection
{
	// Token: 0x02000024 RID: 36
	public class CircularSection_Component : GH_Component
	{
		// Token: 0x06000117 RID: 279 RVA: 0x00007D44 File Offset: 0x00005F44
		public CircularSection_Component() : base("CircularSection", "CS", "Defines a circular section with diameter D", "Beaver", "02 CrossSections")
		{
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00007D67 File Offset: 0x00005F67
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddNumberParameter("Diameter [mm]", "D [mm]", "Circular section diameter in [mm]", 0, 100.0);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000078A6 File Offset: 0x00005AA6
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Cross Section", "CS", "Cross Section", 0);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00007D8C File Offset: 0x00005F8C
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			double num = 100.0;
			DA.GetData<double>(0, ref num);
			DA.SetData(0, new CircularSection(num / 1000.0));
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00007DC6 File Offset: 0x00005FC6
		protected override Bitmap Icon
		{
			get
			{
				return Resources.CircularSection_BeaverIcon;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00007DCD File Offset: 0x00005FCD
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("8e617b0c-b658-464e-958e-a27b6cea0f54");
			}
		}
	}
}
