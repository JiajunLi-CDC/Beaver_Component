using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Beaver3D.Model;
using Beaver.Properties;
using Rhino.Geometry;

namespace Beaver.Components.Model
{
	// Token: 0x02000014 RID: 20
	public class Support_Component : GH_Component, IGH_PreviewObject
	{
		// Token: 0x060000AB RID: 171 RVA: 0x00005918 File Offset: 0x00003B18
		public Support_Component() : base("Support", "Sp", "Creates a support for truss elements", "Beaver", "01 Geometry")
		{
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005974 File Offset: 0x00003B74
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddPointParameter("Location", "P", "Location of support", GH_ParamAccess.list);
			pManager.AddBooleanParameter("Fix TX", "X", "Fix translation in X-Direction", 0, true);
			pManager.AddBooleanParameter("Fix TY", "Y", "Fix translation in Y-Direction", 0, true);
			pManager.AddBooleanParameter("Fix TZ", "Z", "Fix translation in Z-Direction", 0, true);
			pManager.AddNumberParameter("ScaleSupport", "sc", "scales displayed support", 0, 1.0);
			pManager[0].Optional = true;
			pManager[1].Optional = true;
			pManager[2].Optional = true;
			pManager[3].Optional = true;
			pManager[4].Optional = true;
	
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005A47 File Offset: 0x00003C47
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Support", "Sp", "A support fixing selected translations and rotations", GH_ParamAccess.list);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005A64 File Offset: 0x00003C64
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			List<Point3d> list = new List<Point3d>();
			List<Support> list2 = new List<Support>();
			GH_Boolean gh_Boolean = new GH_Boolean();
			GH_Boolean gh_Boolean2 = new GH_Boolean();
			GH_Boolean gh_Boolean3 = new GH_Boolean();
			DA.GetDataList<Point3d>(0, list);
			DA.GetData<GH_Boolean>(1, ref gh_Boolean);
			DA.GetData<GH_Boolean>(2, ref gh_Boolean2);
			DA.GetData<GH_Boolean>(3, ref gh_Boolean3);
			DA.GetData<double>(4, ref this.scaleFactor);


			this.Locations.Clear();
			this.DOFs = new Tuple<bool, bool, bool, bool, bool, bool>(gh_Boolean.Value, gh_Boolean2.Value, gh_Boolean3.Value, true, true, true);
			foreach (Point3d item in list)
			{
				Node n = new Node(item.X, item.Y, item.Z);
				Support item2 = new Support(n, gh_Boolean.Value, gh_Boolean2.Value, gh_Boolean3.Value, true, true, true);
				this.Locations.Add(item);
				list2.Add(item2);
			}
			DA.SetDataList(0, list2);
		}

		// 画出每个预览的基座线框
		public override void DrawViewportWires(IGH_PreviewArgs args)
		{
			Color limeGreen = Color.LimeGreen;
			for (int i = 0; i < this.Locations.Count; i++)
			{
				bool item = this.DOFs.Item1;
				bool item2 = this.DOFs.Item2;
				bool item3 = this.DOFs.Item3;
				Point3d point3d = this.Locations[i];
				Cone cone = default(Cone);
				Cone cone2 = default(Cone);
				Cone cone3 = default(Cone);
				bool flag = item;
                if (flag)
                {
                    Plane plane;
                    plane = new Plane(point3d, new Vector3d(-1.0, 0.0, 0.0));
                    cone = new Cone(plane, 1.0 * this.scaleFactor, 0.5 * this.scaleFactor);
                    args.Display.DrawCone(cone, limeGreen);
                }
                bool flag2 = item2;
                if (flag2)
                {
                    Plane plane2;
                    plane2 = new Plane(point3d, new Vector3d(0.0, -1.0, 0.0));
                    cone2 = new Cone(plane2, 1.0 * this.scaleFactor, 0.5 * this.scaleFactor);
                    args.Display.DrawCone(cone2, limeGreen);
                }
                bool flag3 = item3;
                if (flag3)
                {
                    Plane plane3;
                    plane3 = new Plane(point3d, new Vector3d(0.0, 0.0, -1.0));
                    cone3 = new Cone(plane3, 1.0 * this.scaleFactor, 0.5 * this.scaleFactor);
                    args.Display.DrawCone(cone3, limeGreen);
                }
            }
		}

		// 画出每个预览的基座mesh
		public override void DrawViewportMeshes(IGH_PreviewArgs args)
		{
			Color forestGreen = Color.ForestGreen;
			for (int i = 0; i < this.Locations.Count; i++)
			{
				bool item = this.DOFs.Item1;
				bool item2 = this.DOFs.Item2;
				bool item3 = this.DOFs.Item3;
				Point3d point3d = this.Locations[i];
				Cone cone = default(Cone);
				Cone cone2 = default(Cone);
				Cone cone3 = default(Cone);
				bool flag = item;
                if (flag)
                {
                    Plane plane;
					plane = new Plane(point3d, new Vector3d(-1.0, 0.0, 0.0));
                    cone = new Cone(plane, 1.0 * this.scaleFactor, 0.5 * this.scaleFactor);
                    args.Display.DrawCone(cone, forestGreen);
                }
                bool flag2 = item2;
                if (flag2)
                {
                    Plane plane2;
					plane2 = new Plane(point3d, new Vector3d(0.0, -1.0, 0.0));
                    cone2 = new Cone(plane2, 1.0 * this.scaleFactor, 0.5 * this.scaleFactor);
                    args.Display.DrawCone(cone2, forestGreen);
                }
                bool flag3 = item3;
                if (flag3)
                {
                    Plane plane3;
					plane3 = new Plane(point3d, new Vector3d(0.0, 0.0, -1.0));
                    cone3 = new Cone(plane3, 1.0 * this.scaleFactor, 0.5 * this.scaleFactor);
                    args.Display.DrawCone(cone3, forestGreen);
                }
            }
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00005F16 File Offset: 0x00004116
		protected override Bitmap Icon
		{
			get
			{
				return Resources.Support_BeaverIcon;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00005907 File Offset: 0x00003B07
		public override GH_Exposure Exposure
		{
			get
			{
				return GH_Exposure.secondary;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00005F1D File Offset: 0x0000411D
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("50aaa93a-9ee8-43f3-9539-2c1749d0460f");
			}
		}

		// Token: 0x04000005 RID: 5
		private Tuple<bool, bool, bool, bool, bool, bool> DOFs = new Tuple<bool, bool, bool, bool, bool, bool>(false, false, false, false, false, false);

		// Token: 0x04000006 RID: 6
		private List<Point3d> Locations = new List<Point3d>();

		// Token: 0x04000007 RID: 7
		private double scaleFactor = 1.0;
	}
}
