using System;
using System.Drawing;
using Rhino.Geometry;

namespace Beaver.Display
{
	// Token: 0x02000030 RID: 48
	public static class Profile
	{
		// Token: 0x06000157 RID: 343 RVA: 0x0000B370 File Offset: 0x00009570
		public static Mesh LSection(Point3d anchor, double w, double h, double t, double l, Color color)
		{
			Mesh mesh = new Mesh();
			mesh.Vertices.Add(anchor);
			mesh.Vertices.Add(anchor + new Vector3d(w, 0.0, 0.0));
			mesh.Vertices.Add(anchor + new Vector3d(w, t, 0.0));
			mesh.Vertices.Add(anchor + new Vector3d(t, t, 0.0));
			mesh.Vertices.Add(anchor + new Vector3d(t, h, 0.0));
			mesh.Vertices.Add(anchor + new Vector3d(0.0, h, 0.0));
			mesh.Vertices.Add(anchor + new Vector3d(0.0, 0.0, l));
			mesh.Vertices.Add(anchor + new Vector3d(w, 0.0, l));
			mesh.Vertices.Add(anchor + new Vector3d(w, t, l));
			mesh.Vertices.Add(anchor + new Vector3d(t, t, l));
			mesh.Vertices.Add(anchor + new Vector3d(t, h, l));
			mesh.Vertices.Add(anchor + new Vector3d(0.0, h, l));
			foreach (Point3f point3f in mesh.Vertices)
			{
				mesh.VertexColors.Add(color);
			}
			mesh.Faces.AddFace(1, 0, 3, 2);
			mesh.Faces.AddFace(0, 5, 4, 3);
			mesh.Faces.AddFace(6, 7, 8, 9);
			mesh.Faces.AddFace(6, 9, 10, 11);
			mesh.Faces.AddFace(0, 1, 7, 6);
			mesh.Faces.AddFace(1, 2, 8, 7);
			mesh.Faces.AddFace(2, 3, 9, 8);
			mesh.Faces.AddFace(3, 4, 10, 9);
			mesh.Faces.AddFace(4, 5, 11, 10);
			mesh.Faces.AddFace(5, 0, 6, 11);
			return mesh;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000B608 File Offset: 0x00009808
		public static Mesh CircularSection(Point3d anchor, double r, double l, Color color)
		{
			Mesh mesh = new Mesh();
			int num = 8;
			for (int i = 0; i < num; i++)
			{
				mesh.Vertices.Add(anchor + new Vector3d(r * Math.Sin((double)(i * 2) * 3.1415926535897931 / (double)num), r * Math.Cos((double)(i * 2) * 3.1415926535897931 / (double)num), 0.0));
			}
			for (int j = 0; j < num; j++)
			{
				mesh.Vertices.Add(anchor + new Vector3d(r * Math.Sin((double)(j * 2) * 3.1415926535897931 / (double)num), r * Math.Cos((double)(j * 2) * 3.1415926535897931 / (double)num), l));
			}
			foreach (Point3f point3f in mesh.Vertices)
			{
				mesh.VertexColors.Add(color);
			}
			mesh.Faces.AddFace(0, 1, 9, 8);
			mesh.Faces.AddFace(1, 2, 10, 9);
			mesh.Faces.AddFace(2, 3, 11, 10);
			mesh.Faces.AddFace(3, 4, 12, 11);
			mesh.Faces.AddFace(4, 5, 13, 12);
			mesh.Faces.AddFace(5, 6, 14, 13);
			mesh.Faces.AddFace(6, 7, 15, 14);
			mesh.Faces.AddFace(7, 0, 8, 15);
			mesh.Faces.AddFace(0, 1, 2, 3);
			mesh.Faces.AddFace(0, 3, 4, 7);
			mesh.Faces.AddFace(7, 4, 5, 6);
			mesh.Faces.AddFace(num, 1 + num, 2 + num, 3 + num);
			mesh.Faces.AddFace(num, 3 + num, 4 + num, 7 + num);
			mesh.Faces.AddFace(7 + num, 4 + num, 5 + num, 6 + num);
			return mesh;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000B83C File Offset: 0x00009A3C
		public static Mesh CircularHollowSection(Point3d anchor, double r1, double r2, double l, Color color)
		{
			Mesh mesh = new Mesh();
			int num = 8;
			int num2 = 2 * num;
			for (int i = 0; i < num; i++)
			{
				mesh.Vertices.Add(anchor + new Vector3d(r1 * Math.Sin((double)(i * 2) * 3.1415926535897931 / (double)num), r1 * Math.Cos((double)(i * 2) * 3.1415926535897931 / (double)num), 0.0));
			}
			for (int j = 0; j < num; j++)
			{
				mesh.Vertices.Add(anchor + new Vector3d(r1 * Math.Sin((double)(j * 2) * 3.1415926535897931 / (double)num), r1 * Math.Cos((double)(j * 2) * 3.1415926535897931 / (double)num), l));
			}
			for (int k = 0; k < num; k++)
			{
				mesh.Vertices.Add(anchor + new Vector3d(r2 * Math.Sin((double)(k * 2) * 3.1415926535897931 / (double)num), r2 * Math.Cos((double)(k * 2) * 3.1415926535897931 / (double)num), 0.0));
			}
			for (int m = 0; m < num; m++)
			{
				mesh.Vertices.Add(anchor + new Vector3d(r2 * Math.Sin((double)(m * 2) * 3.1415926535897931 / (double)num), r2 * Math.Cos((double)(m * 2) * 3.1415926535897931 / (double)num), l));
			}
			foreach (Point3f point3f in mesh.Vertices)
			{
				mesh.VertexColors.Add(color);
			}
			mesh.Faces.AddFace(0, 1, 9, 8);
			mesh.Faces.AddFace(1, 2, 10, 9);
			mesh.Faces.AddFace(2, 3, 11, 10);
			mesh.Faces.AddFace(3, 4, 12, 11);
			mesh.Faces.AddFace(4, 5, 13, 12);
			mesh.Faces.AddFace(5, 6, 14, 13);
			mesh.Faces.AddFace(6, 7, 15, 14);
			mesh.Faces.AddFace(7, 0, 8, 15);
			mesh.Faces.AddFace(num2, 1 + num2, 9 + num2, 8 + num2);
			mesh.Faces.AddFace(1 + num2, 2 + num2, 10 + num2, 9 + num2);
			mesh.Faces.AddFace(2 + num2, 3 + num2, 11 + num2, 10 + num2);
			mesh.Faces.AddFace(3 + num2, 4 + num2, 12 + num2, 11 + num2);
			mesh.Faces.AddFace(4 + num2, 5 + num2, 13 + num2, 12 + num2);
			mesh.Faces.AddFace(5 + num2, 6 + num2, 14 + num2, 13 + num2);
			mesh.Faces.AddFace(6 + num2, 7 + num2, 15 + num2, 14 + num2);
			mesh.Faces.AddFace(7 + num2, num2, 8 + num2, 15 + num2);
			for (int n = 0; n < num; n++)
			{
				mesh.Faces.AddFace(n, 1 + n, 17 + n, 16 + n);
				mesh.Faces.AddFace(n + num, 1 + n + num, 17 + n + num, 16 + n + num);
			}
			mesh.Faces.AddFace(0, 16, 23, 7);
			mesh.Faces.AddFace(num, 16 + num, 23 + num, 7 + num);
			return mesh;
		}
	}
}
