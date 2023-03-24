using System;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model.CrossSections;
using Beaver.Properties;

namespace Beaver.Model.CrossSection
{
	// Token: 0x02000026 RID: 38
	public class LSection_Component : GH_Component
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00007FFF File Offset: 0x000061FF
		public LSection_Component() : base("LSection", "L", "Standard L-Section profile after EN 10056-1:2017", "Beaver", "02 CrossSections")
		{
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00008024 File Offset: 0x00006224
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddNumberParameter("Height [mm]", "H [mm]", "LSection height in [mm]", 0, 50.0);
			pManager.AddNumberParameter("Width [mm]", "W [mm]", "LSection width in [mm]. If not profided, value H is taken.", 0);
			pManager.AddNumberParameter("Thickness [mm]", "T [mm]", "LSection flange thickness [mm]", 0, 6.0);		
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000078A6 File Offset: 0x00005AA6
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Cross Section", "CS", "Cross Section", 0);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00008098 File Offset: 0x00006298
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			double num = 50.0;
			double num2 = 50.0;
			double num3 = 6.0;
			DA.GetData<double>(0, ref num);
			bool flag = !DA.GetData<double>(1, ref num2);
			if (flag)
			{
				num2 = num;
			}
			DA.GetData<double>(2, ref num3);
			DA.SetData(0, new LSection(num / 1000.0, num2 / 1000.0, num3 / 1000.0));
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000127 RID: 295 RVA: 0x0000811A File Offset: 0x0000631A
		protected override Bitmap Icon
		{
			get
			{
				return Resources.LSection_BeaverIcon;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00008121 File Offset: 0x00006321
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("12a218ca-4dcf-4446-999b-84cecf519150");
			}
		}
	}
}
