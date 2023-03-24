using System;
using System.Drawing;
using Beaver.Properties;
using Grasshopper.Kernel;
using Beaver3D.Model.Materials;


namespace Beaver.Model.Materials
{
	// Token: 0x02000016 RID: 22
	public class Steel_Component : GH_Component
	{
		// Token: 0x060000BA RID: 186 RVA: 0x000061EF File Offset: 0x000043EF
		public Steel_Component() : base("Steel", "Steel", "Isotropic steel material", "Beaver", "03 Materials")
		{
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00006214 File Offset: 0x00004414
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddNumberParameter("Density", "D [kg/m3]", "Material density in [kg/m3]", 0, 7850.0);
			pManager.AddNumberParameter("Young's Modulus", "E [N/mm2]", "Material Young's Modulus = Elastic Modulus in [MPa] = [N/mm2]", 0, 210000.0);
			pManager.AddNumberParameter("Poisson Ratio", "nu [-]", "Material density in [kg/m3]", 0, 0.3);
			pManager.AddNumberParameter("Tension Strength", "ft [N/mm2]", "Material tension (positive) in [MPa] = [N/mm2]", 0, 235.0);
			pManager.AddNumberParameter("Compression Strength", "fc [N/mm2]", "Material compression strength (here positive) in [MPa] = [N/mm2]", 0, 235.0);
			pManager.AddNumberParameter("Material safety factor", "y0 [-]", "Material safety factor for yield strength fd0 = fy/y0", 0, 1.0);
			pManager.AddNumberParameter("Material safety factor for stability", "y1 [-]", "Material safety factor for stability fd1 = fy/y1", 0, 1.1);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000060CA File Offset: 0x000042CA
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Material", "Mat", "Material", 0);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00006364 File Offset: 0x00004564
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			double value = 7850.0;
			double value2 = 210000.0;
			double value3 = 0.3;
			double value4 = 235.0;
			double value5 = 235.0;
			double value6 = 1.0;
			double value7 = 1.1;
			DA.GetData<double>(0, ref value);
			DA.GetData<double>(1, ref value2);
			DA.GetData<double>(2, ref value3);
			DA.GetData<double>(3, ref value4);
			DA.GetData<double>(4, ref value5);
			DA.GetData<double>(5, ref value6);
			DA.GetData<double>(6, ref value7);


			DA.SetData(0, new Steel(Math.Abs(value), Math.Abs(value2), Math.Abs(value3), Math.Abs(value4), Math.Abs(value5), Math.Abs(value6), Math.Abs(value7), 1.0));
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00006449 File Offset: 0x00004649
		protected override Bitmap Icon
		{
			get
			{
				return Resources.Steel_BeaverIcon;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00006450 File Offset: 0x00004650
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("ca2edb42-1d35-4ca9-a18c-81213d9b402e");
			}
		}
	}
}
