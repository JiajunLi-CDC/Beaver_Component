using System;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model.CrossSections;
using Beaver.Properties;

namespace Beaver.Model.CrossSection
{
	// Token: 0x0200001E RID: 30
	public class HEASection_Component : GH_Component
	{
		// Token: 0x060000F3 RID: 243 RVA: 0x00007867 File Offset: 0x00005A67
		public HEASection_Component() : base("HEASection", "HEA", "Standard HEA-Section profile after EN 16828; 2015-4", "Beaver", "02 CrossSections")
		{
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000788A File Offset: 0x00005A8A
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddIntegerParameter("Profile size", "Size", "HEA-Section size", 0, 100);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000078A6 File Offset: 0x00005AA6
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Cross Section", "CS", "Cross Section", 0);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000078C0 File Offset: 0x00005AC0
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			int size = 100;
			DA.GetData<int>(0, ref size);
			DA.SetData(0, new HEASection(size));
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x000078E9 File Offset: 0x00005AE9
		protected override Bitmap Icon
		{
			get
			{
				return Resources.HEASection_BeaverIcon;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000078F0 File Offset: 0x00005AF0
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("12a218ca-4dcf-4446-999b-84cecf514612");
			}
		}
	}
}
