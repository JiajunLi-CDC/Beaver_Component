using System;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model.CrossSections;
using Beaver.Properties;

namespace Beaver.Model.CrossSection
{
	// Token: 0x02000020 RID: 32
	public class RectangularSection_Component : GH_Component
	{
		// Token: 0x060000FF RID: 255 RVA: 0x00007978 File Offset: 0x00005B78
		public RectangularSection_Component() : base("RectangularSection", "R", "Rectangular cross-section with specified height and width", "Beaver", "02 CrossSections")
		{
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000799C File Offset: 0x00005B9C
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddNumberParameter("Height [mm]", "H [mm]", "Rectangular section height in [mm]", 0, 50.0);
			pManager.AddNumberParameter("Width [mm]", "W [mm]", "Rectangular section width in [mm]", 0, 30.0);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000078A6 File Offset: 0x00005AA6
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Cross Section", "CS", "Cross Section", 0);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000079EC File Offset: 0x00005BEC
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			double num = 50.0;
			double num2 = 30.0;
			DA.GetData<double>(0, ref num);
			DA.GetData<double>(1, ref num2);
			DA.SetData(0, new RectangularSection(num / 1000.0, num2 / 1000.0));
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00007A45 File Offset: 0x00005C45
		protected override Bitmap Icon
		{
			get
			{
				return Resources.Rectangle_section_BeaverIcon;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00007A4C File Offset: 0x00005C4C
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("12a218ca-4dcf-4446-999b-84cecf516404");
			}
		}
	}
}
