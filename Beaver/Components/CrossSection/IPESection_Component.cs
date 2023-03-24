using System;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model.CrossSections;
using Beaver.Properties;

namespace Beaver.Model.CrossSection
{
	// Token: 0x0200001F RID: 31
	public class IPESection_Component : GH_Component
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x000078FC File Offset: 0x00005AFC
		public IPESection_Component() : base("IPESection", "IPE", "Standard IPE-Section profile after EN 16828; 2015-4", "Beaver", "02 CrossSections")
		{
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000791F File Offset: 0x00005B1F
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddIntegerParameter("Size", "Size", "IPE-Section size", 0, 100);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000078A6 File Offset: 0x00005AA6
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Cross Section", "CS", "Cross Section", 0);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000793C File Offset: 0x00005B3C
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			int size = 100;
			DA.GetData<int>(0, ref size);
			DA.SetData(0, new IPESection(size));
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00007965 File Offset: 0x00005B65
		protected override Bitmap Icon
		{
			get
			{
				return Resources.IPESection_BeaverIcon;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000796C File Offset: 0x00005B6C
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("12a218ca-4dcf-4446-999b-84cecf546011");
			}
		}
	}
}
