using System;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model.CrossSections;
using Beaver.Properties;

namespace Beaver.Model.CrossSection
{
	// Token: 0x02000025 RID: 37
	public class GenericSection_Component : GH_Component
	{
		// Token: 0x0600011D RID: 285 RVA: 0x00007DD9 File Offset: 0x00005FD9
		public GenericSection_Component() : base("GenericSection", "GenericSection", "Creates a generic cross section", "Beaver", "02 CrossSections")
		{
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00007DFC File Offset: 0x00005FFC
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddNumberParameter("Area", "A [mm2]", "Cross section area in [mm2]", 0, 2500.0);
			pManager.AddNumberParameter("Iy", "Iy [mm4]", "Area moment of inertia about y-Axis in [mm4]", 0, 520833.0);
			pManager.AddNumberParameter("Iz", "Iz [mm4]", "Area moment of inertia about z-Axis in [mm4]", 0, 520833.0);
			pManager.AddNumberParameter("Wy", "Wy [mm3]", "Section modulus about y-Axis in [mm3]", 0, 20833.0);
			pManager.AddNumberParameter("Wz", "Wz [mm3]", "Section modulus about z-Axis in [mm3]", 0, 20833.0);
			pManager.AddNumberParameter("It", "It [mm3]", "Torsional moment of inertia in [mm4]", 0, 881249.99999999977);
			pManager.AddNumberParameter("Wt", "Wt [mm3]", "Section modulus about x-Axis in [mm3]", 0, 20833.0);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000078A6 File Offset: 0x00005AA6
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Cross Section", "CS", "Cross Section", 0);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00007EEC File Offset: 0x000060EC
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			double num = 2500.0;
			double num2 = 520833.0;
			double iz = num2;
			double num3 = 20833.0;
			double wz = num3;
			double num4 = 881249.99999999977;
			double num5 = num3;
			DA.GetData<double>(0, ref num);
			DA.GetData<double>(1, ref num2);
			DA.GetData<double>(2, ref iz);
			DA.GetData<double>(3, ref num3);
			DA.GetData<double>(4, ref wz);
			DA.GetData<double>(5, ref num4);
			DA.GetData<double>(6, ref num5);
			num /= 1000000.0;
			num2 /= 1000000000000.0;
			iz = num2;
			num3 /= 1000000000.0;
			wz = num3;
			num4 /= 1000000000000.0;
			num5 /= 1000000000.0;
			string name = "GenericSection";
			GenericSection genericSection = new GenericSection(name, num, num2, iz, num3, wz, num5, num4, 0.0, 0.0);
			DA.SetData(0, genericSection);
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00007FEC File Offset: 0x000061EC
		protected override Bitmap Icon
		{
			get
			{
				return Resources.GenericSection_BeaverIcon;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00007FF3 File Offset: 0x000061F3
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("75dbd239-149c-490d-953e-098502486527");
			}
		}
	}
}
