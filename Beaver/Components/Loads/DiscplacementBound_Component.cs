using System;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model;
using Beaver.Properties;
using Rhino.Geometry;

namespace Beaver.Model.Loads
{
	// Token: 0x02000017 RID: 23
	public class DiscplacementBound_Component : GH_Component
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x0000645C File Offset: 0x0000465C
		public DiscplacementBound_Component() : base("Displacement Bounds", "Displacements", "Set a limit on allowable node displacements for optimization", "Beaver", "04 Loads")
		{
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00006480 File Offset: 0x00004680
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddPointParameter("Node Position", "P", "Position of the constrained node", 0);
			pManager.AddVectorParameter("LowerBound", "LB", "Lower bound for the node displacements (translations XYZ, usually negative number or 0)", 0, new Vector3d(-1E+100, -1E+100, -1E+100));
			pManager.AddVectorParameter("UpperBound", "UB", "Upper bound for the node displacements (translations XYZ, usually positive number or 0)", 0, new Vector3d(1E+100, 1E+100, 1E+100));
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006513 File Offset: 0x00004713
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("DisplacementBound", "DB", "Displacement bound", 0);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00006530 File Offset: 0x00004730
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			Point3d point3d = default(Point3d);
			Vector3d vector3d;
			vector3d = new Vector3d(-1E+100, -1E+100, -1E+100);
			Vector3d vector3d2;
			vector3d2 = new Vector3d(1E+100, 1E+100, 1E+100);
			DA.GetData<Point3d>(0, ref point3d);
			DA.GetData<Vector3d>(1, ref vector3d);
			DA.GetData<Vector3d>(2, ref vector3d2);
			DA.SetData(0, new DisplacementBound(new Node(point3d.X, point3d.Y, point3d.Z), vector3d.X, vector3d.Y, vector3d.Z, vector3d2.X, vector3d2.Y, vector3d2.Z));
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000065F9 File Offset: 0x000047F9
		protected override Bitmap Icon
		{
			get
			{
				return Resources.Displacement_Bounds_BeaverIcon;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00006600 File Offset: 0x00004800
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("54e9919d-2147-47f0-b885-7d84c6787a56");
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x0000660C File Offset: 0x0000480C
		public override GH_Exposure Exposure
		{
			get
			{
				return GH_Exposure.secondary;
			}
		}
	}
}
