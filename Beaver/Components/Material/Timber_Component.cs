using System;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model.Materials;
using Beaver.Properties;

namespace Beaver.Model.Materials
{
	// Token: 0x02000015 RID: 21
	public class Timber_Component : GH_Component
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x00005F29 File Offset: 0x00004129
		public Timber_Component() : base("Timber", "Timber", "Timber material (longitudinal only)", "Beaver", "03 Materials")
		{
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005F4C File Offset: 0x0000414C
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddNumberParameter("Density", "D [kg/m3]", "Material density in [kg/m3]", 0, 500.0);
			pManager.AddNumberParameter("Young's Modulus", "E0 [N/mm2]", "Material Young's Modulus = Elastic Modulus parallel to fibers in [MPa] = [N/mm2]", 0, 12000.0);
			pManager.AddNumberParameter("Poisson Ratio", "nu [-]", "Material density in [kg/m3]", 0, 0.25);
			pManager.AddNumberParameter("Tension Strength", "ft [N/mm2]", "Material tension strength (positive) in [MPa] = [N/mm2]", 0, 12.0);
			pManager.AddNumberParameter("Compression Strength", "fc [N/mm2]", "Material compression strength (here positive) in [MPa] = [N/mm2]", 0, 8.0);
			pManager.AddNumberParameter("Material safety factor", "y0 [-]", "Material safety factor for yield strength fd0 = fy/y0", 0, 1.0);
			pManager.AddNumberParameter("Material safety factor for stability", "y1 [-]", "Material safety factor for stability fd1 = fy/y1", 0, 1.1);
			pManager.AddNumberParameter("kmod", "kmod [-]", "Factor for environment", 0, 0.8);

		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000060CA File Offset: 0x000042CA
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Material", "Mat", "Material", 0);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000060E4 File Offset: 0x000042E4
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			double value = 500.0;
			double value2 = 12000.0;
			double value3 = 0.25;
			double value4 = 12.0;
			double value5 = 8.0;
			double value6 = 1.0;
			double value7 = 1.1;
			double value8 = 0.8;
			DA.GetData<double>(0, ref value);
			DA.GetData<double>(1, ref value2);
			DA.GetData<double>(2, ref value3);
			DA.GetData<double>(3, ref value4);
			DA.GetData<double>(4, ref value5);
			DA.GetData<double>(5, ref value6);
			DA.GetData<double>(6, ref value7);
			DA.GetData<double>(7, ref value8);
			DA.SetData(0, new Timber(Math.Abs(value), Math.Abs(value2), Math.Abs(value3), Math.Abs(value4), Math.Abs(value5), Math.Abs(value6), Math.Abs(value7), Math.Abs(value8)));
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000061DC File Offset: 0x000043DC
		protected override Bitmap Icon
		{
			get
			{
				return Resources.Timber_BeaverIcon;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000061E3 File Offset: 0x000043E3
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("ca2edb42-1d35-4ca9-a18c-81213d9b411e");
			}
		}
	}
}
