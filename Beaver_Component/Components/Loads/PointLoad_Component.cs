using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model;
using Beaver.Properties;
using Rhino.Geometry;

namespace Beaver.Model.Loads
{
	// Token: 0x02000019 RID: 25
	public class PointLoad_Component : GH_Component, IGH_PreviewObject
	{
		// Token: 0x060000CE RID: 206 RVA: 0x00006874 File Offset: 0x00004A74
		public PointLoad_Component() : base("Pointload", "PL", "Creates a Pointload", "Beaver", "04 Loads")
		{
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000068C8 File Offset: 0x00004AC8
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddPointParameter("Point", "P", "Location of load", GH_ParamAccess.list);
			pManager.AddVectorParameter("Forces [MN]", "F", "Forces XYZ as Vector", 0, Vector3d.Zero);
			pManager.AddNumberParameter("ScaleLoad", "sc", "scales displayed load", 0, 1.0);
			pManager[1].Optional = true;
			pManager[2].Optional = true;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006945 File Offset: 0x00004B45
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("PointLoad", "PL", "", GH_ParamAccess.list);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00006960 File Offset: 0x00004B60
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			List<Point3d> list = new List<Point3d>();
			Vector3d force = default(Vector3d);   //受力

			DA.GetDataList<Point3d>(0, list);
			DA.GetData<Vector3d>(1, ref force);
			DA.GetData<double>(2, ref this.scaleFactor);

			this.Locations.Clear();
			List<PointLoad> list2 = new List<PointLoad>();
			foreach (Point3d item in list)
			{
				Node node = new Node(item.X, item.Y, item.Z);
				PointLoad item2 = new PointLoad(node, force.X, force.Y, force.Z, 0.0, 0.0, 0.0);

				this.Locations.Add(item);
				list2.Add(item2);
			}
			this.Force = force;
			DA.SetDataList(0, list2);
		}

		// 画出受力的框线，力越大线越长
		public override void DrawViewportWires(IGH_PreviewArgs args)
		{
			Color limeGreen = Color.LimeGreen;
			for (int i = 0; i < this.Locations.Count; i++)
			{
				Point3d point3d = this.Locations[i];
				Vector3d force = this.Force;
				Plane plane;
				plane = new Plane(point3d, new Vector3d(-force.X + 1.0, 0.0, 0.0));
				Cone cone;
				cone = new Cone(plane, 1.0 * this.scaleFactor, 0.25 * this.scaleFactor);
				Line line;
				line = new Line(point3d, new Vector3d((-force.X + 1.0) * this.scaleFactor, 0.0, 0.0));

				Plane plane2;
				plane2 = new Plane(point3d, new Vector3d(0.0, -force.Y + 1.0, 0.0));
				Cone cone2;
				cone2 = new Cone(plane2, 1.0 * this.scaleFactor, 0.25 * this.scaleFactor);
				Line line2;
				line2 = new Line(point3d, new Vector3d(0.0, (-force.Y + 1.0) * this.scaleFactor, 0.0));

				Plane plane3;
				plane3 = new Plane(point3d, new Vector3d(0.0, 0.0, -force.Z + 1.0));
				Cone cone3;
				cone3 = new Cone(plane3, 1.0 * this.scaleFactor, 0.25 * this.scaleFactor);
				Line line3;
				line3 = new Line(point3d, new Vector3d(0.0, 0.0, (-force.Z + 1.0) * this.scaleFactor));

				bool flag = force.X != 0.0;
				if (flag)
				{
					args.Display.DrawCone(cone, limeGreen);
					args.Display.DrawLine(line, limeGreen);
				}
				bool flag2 = force.Y != 0.0;
				if (flag2)
				{
					args.Display.DrawCone(cone2, limeGreen);
					args.Display.DrawLine(line2, limeGreen);
				}
				bool flag3 = force.Z != 0.0;
				if (flag3)
				{
					args.Display.DrawCone(cone3, limeGreen);
					args.Display.DrawLine(line3, limeGreen);
				}
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00006D20 File Offset: 0x00004F20
		public override void DrawViewportMeshes(IGH_PreviewArgs args)
		{
			Color forestGreen = Color.ForestGreen;
			for (int i = 0; i < this.Locations.Count; i++)
			{
				Point3d point3d = this.Locations[i];
				Vector3d force = this.Force;
				Plane plane;
				plane = new Plane(point3d, new Vector3d(-force.X + 1.0, 0.0, 0.0));
				Cone cone;
				cone = new Cone(plane, 1.0 * this.scaleFactor, 0.25 * this.scaleFactor);
				Line line;
				line = new Line(point3d, new Vector3d((-force.X + 1.0) * this.scaleFactor, 0.0, 0.0));

				Plane plane2;
				plane2 = new Plane(point3d, new Vector3d(0.0, -force.Y + 1.0, 0.0));
				Cone cone2;
				cone2 = new Cone(plane2, 1.0 * this.scaleFactor, 0.25 * this.scaleFactor);
				Line line2;
				line2 = new Line(point3d, new Vector3d(0.0, (-force.Y + 1.0) * this.scaleFactor, 0.0));

				Plane plane3;
				plane3 = new Plane(point3d, new Vector3d(0.0, 0.0, -force.Z + 1.0));
				Cone cone3;
				cone3 = new Cone(plane3, 1.0 * this.scaleFactor, 0.25 * this.scaleFactor);
				Line line3;
				line3 = new Line(point3d, new Vector3d(0.0, 0.0, (-force.Z + 1.0) * this.scaleFactor));

				bool flag = force.X != 0.0;
				if (flag)
				{
					args.Display.DrawCone(cone, forestGreen);
					args.Display.DrawLine(line, forestGreen);
				}
				bool flag2 = force.Y != 0.0;
				if (flag2)
				{
					args.Display.DrawCone(cone2, forestGreen);
					args.Display.DrawLine(line2, forestGreen);
				}
				bool flag3 = force.Z != 0.0;
				if (flag3)
				{
					args.Display.DrawCone(cone3, forestGreen);
					args.Display.DrawLine(line3, forestGreen);
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00006FD3 File Offset: 0x000051D3
		protected override Bitmap Icon
		{
			get
			{
				return Resources.PointLoad_BeaverIcon;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00005907 File Offset: 0x00003B07
		public override GH_Exposure Exposure
		{
			get
			{
				return GH_Exposure.secondary;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00006FDA File Offset: 0x000051DA
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("ea675e48-ec6a-4934-a228-4430f2f592b2");
			}
		}

		// Token: 0x04000008 RID: 8
		private List<Point3d> Locations = new List<Point3d>();

		// Token: 0x04000009 RID: 9
		private Vector3d Force = Vector3d.Zero;

		// Token: 0x0400000A RID: 10
		private double scaleFactor = 1.0;
	}
}
