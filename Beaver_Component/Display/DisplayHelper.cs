using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Grasshopper;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Beaver3D.Model;
using Beaver3D.Model.CrossSections;
using Beaver3D.Optimization;
using Beaver3D.Reuse;
using Rhino.Geometry;

namespace Beaver.Display
{
	// Token: 0x02000027 RID: 39
	public static class DisplayHelper
	{
		// Token: 0x06000129 RID: 297 RVA: 0x00008130 File Offset: 0x00006330
		public static DataTree<GH_Mesh> GetStructureMeshes(Structure Structure, DisplayResultsType DisplayResultsType, LoadCase LC, double LCScale, ref bool test)
		{
			DataTree<GH_Mesh> dataTree = new DataTree<GH_Mesh>();
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				List<GH_Mesh> memberMeshes = DisplayHelper.GetMemberMeshes(member1D, Structure, DisplayResultsType, LC, LCScale, ref test);
				dataTree.AddRange(memberMeshes, new GH_Path(member1D.Number));
			}
			foreach (IMember member2 in Structure.Members)
			{
				IMember1D member1D2 = (IMember1D)member2;
				bool flag = member1D2.Assignment != null;
				if (flag)
				{
					foreach (ElementGroup elementGroup in member1D2.Assignment.ElementGroups)
					{
						elementGroup.ResetAlreadyCounted();
					}
				}
			}
			return dataTree;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000825C File Offset: 0x0000645C
		public static DataTree<GH_Mesh> GetStockMeshesOnly(Stock Stock, DisplayResultsType DisplayResultsType, Plane AnchorPlane = default(Plane), double dx = 1.0, double dy = 1.0)
		{
			Plane plane = AnchorPlane.Clone();
			Stock.ResetNext();
			bool flag = !plane.IsValid;
			if (flag)
			{
				plane = Plane.WorldXY;
			}
			double originX = plane.OriginX;
			DataTree<GH_Mesh> dataTree = new DataTree<GH_Mesh>();
			for (int i = 0; i < Stock.ElementGroups.Count; i++)
			{
				for (int j = 0; j < Stock.ElementGroups[i].NumberOfElements; j++)
				{
					dataTree.Add(DisplayHelper.GetElementMeshOnly(Stock.ElementGroups[i], plane, DisplayResultsType), new GH_Path(i));
					plane.Origin += plane.XAxis * dx;
				}
				plane.Origin += plane.YAxis * dy;
				plane.OriginX = originX;
			}
			return dataTree;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00008364 File Offset: 0x00006564
		public static DataTree<GH_Mesh>[] GetStockMeshesFull(Stock Stock, Structure Structure, DisplayResultsType DisplayResultsType, LoadCase LC, Plane AnchorPlane = default(Plane), double dx = 1.0, double dy = 1.0)
		{
			Plane plane = AnchorPlane.Clone();
			Stock.ResetNext();
			Stock.ResetStacks();
			bool flag = !plane.IsValid;
			if (flag)
			{
				plane = Plane.WorldXY;
			}
			double originX = plane.OriginX;
			Plane plane2 = plane;
			DataTree<GH_Mesh> dataTree = new DataTree<GH_Mesh>();
			DataTree<GH_Mesh> dataTree2 = new DataTree<GH_Mesh>();
			for (int i = 0; i < Stock.ElementGroups.Count; i++)
			{
				for (int j = 0; j < Stock.ElementGroups[i].NumberOfElements; j++)
				{
					bool flag2 = Stock.ElementGroups[i].Type == ElementType.Reuse;
					if (flag2)
					{
						dataTree.Add(DisplayHelper.GetElementMeshOnly(Stock.ElementGroups[i], plane2, DisplayResultsType.Blank), new GH_Path(i));
					}
					plane2.Origin += plane2.XAxis * dx;
				}
				plane2.Origin += plane2.YAxis * dy;
				plane2.OriginX = originX;
			}
			foreach (IMember member in Structure.Members)
			{
				IMember1D member1D = (IMember1D)member;
				for (int k = 0; k < member1D.Assignment.ElementGroups.Count; k++)
				{
					ElementGroup elementGroup = member1D.Assignment.ElementGroups[k];
					int num = member1D.Assignment.ElementIndices[k];
					bool flag3 = elementGroup.Type == ElementType.Reuse;
					if (flag3)
					{
						Plane plane3 = plane;
						plane3.Translate(plane3.XAxis * (double)num * dx + plane3.YAxis * (double)elementGroup.Number * dy + plane3.ZAxis * elementGroup.Stack[num]);
						GH_Mesh memberMesh = DisplayHelper.GetMemberMesh(member1D, elementGroup, num, Structure, plane3, DisplayResultsType, LC);
						dataTree2.Add(memberMesh, new GH_Path(elementGroup.Number));
						elementGroup.Stack[num] += member1D.Length;
						Plane plane4 = plane3;
						plane4.Translate(plane4.ZAxis * member1D.Length);
						GH_Mesh remainMesh = DisplayHelper.GetRemainMesh(elementGroup, elementGroup.Length - elementGroup.Stack[num], plane4, DisplayResultsType.Blank);
						dataTree[new GH_Path(elementGroup.Number), num] = remainMesh;
					}
					else
					{
						bool flag4 = elementGroup.Type == ElementType.New;
						if (flag4)
						{
							Plane plane5 = plane;
							plane5.Translate(new Vector3d((double)elementGroup.Next * dx, (double)elementGroup.Number * dy, 0.0));
							GH_Mesh memberMesh2 = DisplayHelper.GetMemberMesh(member1D, elementGroup, num, Structure, plane5, DisplayResultsType, null);
							dataTree2.Add(memberMesh2, new GH_Path(elementGroup.Number));
						}
					}
				}
			}
			Stock.ResetNext();
			Stock.ResetStacks();
			return new DataTree<GH_Mesh>[]
			{
				dataTree,
				dataTree2
			};
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000086DC File Offset: 0x000068DC
		public static DataTree<GH_Mesh>[] GetStockMeshesUsed(Stock Stock, Structure Structure, DisplayResultsType DisplayResultsType, LoadCase LC, Plane AnchorPlane = default(Plane), double dx = 1.0, double dy = 1.0)
		{
			Plane plane = AnchorPlane.Clone();
			Stock.ResetNext();
			bool flag = !plane.IsValid;
			if (flag)
			{
				plane = Plane.WorldXY;
			}
			double originX = plane.OriginX;
			DataTree<GH_Mesh> dataTree = new DataTree<GH_Mesh>();
			DataTree<GH_Mesh> dataTree2 = new DataTree<GH_Mesh>();
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < Stock.ElementGroups.Count; i++)
			{
				bool flag2 = false;
				ElementGroup elementGroup = Stock.ElementGroups[i];
				Plane plane2 = plane;
				plane2.Translate(plane2.YAxis * (double)num * dy);
				int j = 0;
				while (j < elementGroup.NumberOfElements)
				{
					bool flag3 = elementGroup.Type == ElementType.Reuse;
					if (flag3)
					{
						bool flag4 = elementGroup.AssignedMembers[j] == null;
						if (!flag4)
						{
							foreach (IMember1D member1D in elementGroup.AssignedMembers[j])
							{
								GH_Mesh memberMesh = DisplayHelper.GetMemberMesh(member1D, elementGroup, j, Structure, plane2, DisplayResultsType, LC);
								dataTree2.Add(memberMesh, new GH_Path(elementGroup.Number));
								plane2.Translate(plane2.ZAxis * member1D.Length);
								elementGroup.Stack[j] += member1D.Length;
							}
							bool flag5 = elementGroup.AssignedMembers[j].Count > 0;
							if (flag5)
							{
								GH_Mesh remainMesh = DisplayHelper.GetRemainMesh(elementGroup, elementGroup.Length - elementGroup.Stack[j], plane2, DisplayResultsType.Blank);
								dataTree.Add(remainMesh, new GH_Path(i));
								plane2.Translate(plane2.ZAxis * -elementGroup.Stack[j]);
								plane2.Translate(plane2.XAxis * dx);
								flag2 = true;
								num2++;
							}
						}
					}
					else
					{
						bool flag6 = elementGroup.Type == ElementType.New;
						if (flag6)
						{
							bool flag7 = elementGroup.AssignedMembers[j] == null;
							if (!flag7)
							{
								foreach (IMember1D m in elementGroup.AssignedMembers[j])
								{
									GH_Mesh memberMesh2 = DisplayHelper.GetMemberMesh(m, elementGroup, j, Structure, plane2, DisplayResultsType, LC);
									dataTree2.Add(memberMesh2, new GH_Path(elementGroup.Number));
									plane2.Translate(plane2.XAxis * dx);
									num2++;
								}
								bool flag8 = elementGroup.AssignedMembers[j].Count > 0;
								if (flag8)
								{
									flag2 = true;
									num2++;
								}
							}
						}
					}
					IL_2A7:
					j++;
					continue;
					goto IL_2A7;
				}
				plane2.Translate(plane.XAxis * (-dx * (double)num2));
				num2 = 0;
				bool flag9 = flag2;
				if (flag9)
				{
					num++;
				}
			}
			Stock.ResetNext();
			Stock.ResetStacks();
			return new DataTree<GH_Mesh>[]
			{
				dataTree,
				dataTree2
			};
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00008A38 File Offset: 0x00006C38
		public static List<GH_Mesh> GetMemberMeshes(IMember1D M, Structure S, DisplayResultsType DisplayResultsType, LoadCase LC, double DispScale , ref bool test1)
		{
			List<GH_Mesh> list = new List<GH_Mesh>();
			Vector3d vector3d;
			vector3d = new Vector3d(M.Normal[0], M.Normal[1], M.Normal[2]);
			Point3d point3d;
			point3d = new Point3d(M.From.X, M.From.Y, M.From.Z);
			Point3d point3d2;
			point3d2 = new Point3d(M.To.X, M.To.Y, M.To.Z);
			double length = M.Length;

			//这里有问题,m的这个displacements什么时候设置了？
			bool flaggg = M.To.Displacements.ContainsKey(LC);
			
			bool flag = LC != null && S.LoadCases.Contains(LC) && M.From.Displacements.ContainsKey(LC) && M.To.Displacements.ContainsKey(LC);
			if (flag)
			{
				point3d += DispScale * new Vector3d(M.From.Displacements[LC][0], M.From.Displacements[LC][1], M.From.Displacements[LC][2]);
				point3d2 += DispScale * new Vector3d(M.To.Displacements[LC][0], M.To.Displacements[LC][1], M.To.Displacements[LC][2]);
			}
			test1 = flaggg;

			Line line;
			line = new Line(point3d, point3d2);
			length = line.Length;
			Plane plane;
			plane = new Plane(line.From, line.Direction, vector3d);
			Plane plane2;
			plane2 = new Plane(line.From, -plane.ZAxis, plane.YAxis);
			bool flag2 = M.Assignment != null;
			if (flag2)
			{
				int count = M.Assignment.ElementGroups.Count;
				int num = count;
				if (num != 1)
				{
					if (num != 2)
					{
						for (int i = 0; i < M.Assignment.ElementGroups.Count; i++)
						{
							Mesh meshFromPolygon = DisplayHelper.GetMeshFromPolygon(M.Assignment.ElementGroups[i].CrossSection, length);
							DisplayHelper.ColorMemberMesh(ref meshFromPolygon, M, M.Assignment.ElementGroups[i], M.Assignment.ElementIndices[i], S, DisplayResultsType, LC);
							meshFromPolygon.Transform(Transform.PlaneToPlane(Plane.WorldXY, plane2));
							meshFromPolygon.Transform(Transform.Translation(new Vector3d(plane2.XAxis * M.Assignment.ElementGroups[i].CrossSection.cy / plane2.XAxis.Length) + new Vector3d(plane2.YAxis * M.Assignment.ElementGroups[i].CrossSection.cz / plane2.YAxis.Length)));
							meshFromPolygon.Transform(Transform.Rotation((double)i * 3.1415926535897931 * 2.0 / (double)M.Assignment.ElementGroups.Count, plane2.ZAxis, plane2.Origin));
							double num2 = 6.2831853071795862 / (double)(2 * M.Assignment.ElementGroups.Count) + (double)i * 3.1415926535897931 * 2.0 / (double)M.Assignment.ElementGroups.Count;
							meshFromPolygon.Transform(Transform.Translation(0.005 * Math.Cos(num2) * plane2.XAxis + 0.005 * Math.Sin(num2) * plane2.YAxis));
							meshFromPolygon.Transform(Transform.Translation((length - length) / 2.0 * line.Direction / line.Direction.Length));
							list.Add(new GH_Mesh(meshFromPolygon));
						}
					}
					else
					{
						Plane plane3 = plane2;
						plane3.Origin += new Vector3d(plane2.XAxis * 0.005 / plane2.XAxis.Length) + new Vector3d(plane2.XAxis * M.Assignment.ElementGroups[0].CrossSection.cy / plane2.XAxis.Length);
						Plane plane4 = plane2;
						plane4.Origin -= new Vector3d(plane2.XAxis * 0.005 / plane2.XAxis.Length) + new Vector3d(plane2.XAxis * M.Assignment.ElementGroups[0].CrossSection.cy / plane2.XAxis.Length);
						Mesh meshFromPolygon2 = DisplayHelper.GetMeshFromPolygon(M.Assignment.ElementGroups[0].CrossSection, length);
						DisplayHelper.ColorMemberMesh(ref meshFromPolygon2, M, M.Assignment.ElementGroups[0], M.Assignment.ElementIndices[0], S, DisplayResultsType, LC);
						meshFromPolygon2.Transform(Transform.PlaneToPlane(Plane.WorldXY, plane3));
						meshFromPolygon2.Transform(Transform.Translation((length - length) / 2.0 * line.Direction / line.Direction.Length));
						list.Add(new GH_Mesh(meshFromPolygon2));
						Mesh meshFromPolygon3 = DisplayHelper.GetMeshFromPolygon(M.Assignment.ElementGroups[1].CrossSection, length);
						DisplayHelper.ColorMemberMesh(ref meshFromPolygon3, M, M.Assignment.ElementGroups[1], M.Assignment.ElementIndices[1], S, DisplayResultsType, LC);
						meshFromPolygon3.Transform(Transform.Mirror(new Plane(Plane.WorldYZ)));
						meshFromPolygon3.Transform(Transform.PlaneToPlane(Plane.WorldXY, plane4));
						meshFromPolygon3.Transform(Transform.Translation((length - length) / 2.0 * line.Direction / line.Direction.Length));
						list.Add(new GH_Mesh(meshFromPolygon3));
					}
				}
				else
				{
					Mesh meshFromPolygon4 = DisplayHelper.GetMeshFromPolygon(M.Assignment.ElementGroups[0].CrossSection, length);
					DisplayHelper.ColorMemberMesh(ref meshFromPolygon4, M, M.Assignment.ElementGroups[0], M.Assignment.ElementIndices[0], S, DisplayResultsType, LC);
					meshFromPolygon4.Transform(Transform.PlaneToPlane(Plane.WorldXY, plane2));
					meshFromPolygon4.Transform(Transform.Translation((length - length) / 2.0 * line.Direction / line.Direction.Length));
					list.Add(new GH_Mesh(meshFromPolygon4));
				}
			}
			else
			{
				Mesh meshFromPolygon5 = DisplayHelper.GetMeshFromPolygon(M.CrossSection, length);
				DisplayHelper.ColorMemberMesh(ref meshFromPolygon5, M, null, 0, S, DisplayResultsType, LC);
				meshFromPolygon5.Transform(Transform.PlaneToPlane(Plane.WorldXY, plane2));
				list.Add(new GH_Mesh(meshFromPolygon5));
			}
			return list;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00009244 File Offset: 0x00007444
		public static GH_Mesh GetMemberMesh(IMember1D M, ElementGroup EG, int ElementIndex, Structure S, Plane Plane, DisplayResultsType DisplayResultsType, LoadCase LC = null)
		{
			double length = M.Length;
			bool flag = EG != null;
			GH_Mesh result;
			if (flag)
			{
				Mesh meshFromPolygon = DisplayHelper.GetMeshFromPolygon(EG.CrossSection, length);
				DisplayHelper.ColorMemberMesh(ref meshFromPolygon, M, EG, ElementIndex, S, DisplayResultsType, LC);
				meshFromPolygon.Transform(Transform.PlaneToPlane(Plane.WorldXY, Plane));
				result = new GH_Mesh(meshFromPolygon);
			}
			else
			{
				Mesh meshFromPolygon2 = DisplayHelper.GetMeshFromPolygon(M.CrossSection, length);
				DisplayHelper.ColorMemberMesh(ref meshFromPolygon2, M, null, 0, S, DisplayResultsType, LC);
				meshFromPolygon2.Transform(Transform.PlaneToPlane(Plane.WorldXY, Plane));
				result = new GH_Mesh(meshFromPolygon2);
			}
			return result;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000092DC File Offset: 0x000074DC
		public static GH_Mesh GetElementMeshOnly(ElementGroup EG, Plane Plane, DisplayResultsType DisplayResultsType)
		{
			Mesh meshFromPolygon = DisplayHelper.GetMeshFromPolygon(EG.CrossSection, EG.Length);
			DisplayHelper.ColorElementMesh(ref meshFromPolygon, EG, DisplayResultsType);
			meshFromPolygon.Transform(Transform.PlaneToPlane(Plane.WorldXY, Plane));
			return new GH_Mesh(meshFromPolygon);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00009324 File Offset: 0x00007524
		public static GH_Mesh GetRemainMesh(ElementGroup EG, double RemainLength, Plane Plane, DisplayResultsType DisplayResultsType)
		{
			Mesh meshFromPolygon = DisplayHelper.GetMeshFromPolygon(EG.CrossSection, RemainLength);
			DisplayHelper.ColorElementMesh(ref meshFromPolygon, EG, DisplayResultsType);
			meshFromPolygon.Transform(Transform.PlaneToPlane(Plane.WorldXY, Plane));
			return new GH_Mesh(meshFromPolygon);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00009368 File Offset: 0x00007568
		public static Mesh GetMeshFromPolygon(ICrossSection CroSec, double Length)
		{
			Mesh mesh = new Mesh();
			for (int i = 0; i < CroSec.Polygon.Count; i++)
			{
				mesh.Vertices.Add(CroSec.Polygon[i].Item1, CroSec.Polygon[i].Item2, 0.0);
			}
			for (int j = 0; j < CroSec.Polygon.Count; j++)
			{
				mesh.Vertices.Add(CroSec.Polygon[j].Item1, CroSec.Polygon[j].Item2, Length);
			}
			for (int k = 0; k < CroSec.Polygon.Count - 1; k++)
			{
				mesh.Faces.AddFace(new MeshFace(k, k + 1, CroSec.Polygon.Count + k + 1, CroSec.Polygon.Count + k));
			}
			mesh.Faces.AddFace(new MeshFace(CroSec.Polygon.Count - 1, 0, CroSec.Polygon.Count, 2 * CroSec.Polygon.Count - 1));
			return mesh;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000094B8 File Offset: 0x000076B8
		public static void ColorMemberMesh(ref Mesh Mesh, IMember1D M, ElementGroup EG, int ElementIndex, Structure S, DisplayResultsType DisplayResultsType, LoadCase LC)
		{
			Mesh.VertexColors.Clear();
			Color memberColor = DisplayHelper.GetMemberColor(M, EG, ElementIndex, S, DisplayResultsType, LC);
			for (int i = 0; i < Mesh.Vertices.Count; i++)
			{
				Mesh.VertexColors.Add(memberColor);
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00009510 File Offset: 0x00007710
		public static void ColorElementMesh(ref Mesh Mesh, ElementGroup EG, DisplayResultsType DisplayResultsType)
		{
			Mesh.VertexColors.Clear();
			Color elementColor = DisplayHelper.GetElementColor(EG, DisplayResultsType);
			for (int i = 0; i < Mesh.Vertices.Count; i++)
			{
				Mesh.VertexColors.Add(elementColor);
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00009560 File Offset: 0x00007760
		public static Color GetMemberColor(IMember1D M, ElementGroup EG, int ElementIndex, Structure S, DisplayResultsType DRType, LoadCase LC)
		{
			switch (DRType)
			{
			case DisplayResultsType.Blank:
				return ColorPalette.CBlank;
			case DisplayResultsType.BarBeam:
			{
				bool flag = M is Bar;
				if (flag)
				{
					return Color.Aqua;
				}
				return Color.Khaki;
			}
			case DisplayResultsType.Forces:
			{
				bool flag2 = LC == null;
				if (flag2)
				{
					return ColorPalette.CBlank;
				}
				bool flag3 = !S.LoadCases.Contains(LC);
				if (flag3)
				{
					throw new ArgumentException("LoadCase does not exist!");
				}
				double num = Result.GetMemberUtilization(M, LC);
				bool flag4 = num == 0.0;
				if (flag4)
				{
					return Color.White;
				}
				bool flag5 = M.Nx[LC].Min() > 0.0;
				if (flag5)
				{
					bool flag6 = num > 1.0;
					if (flag6)
					{
						num = 1.0;
					}
					return DisplayHelper.ColorFromHSV(0.0, num, 1.0);
				}
				bool flag7 = num > 1.0;
				if (flag7)
				{
					num = 1.0;
				}
				return DisplayHelper.ColorFromHSV(240.0, num, 1.0);
			}
			case DisplayResultsType.Utilization:
			{
				double memberUtilization = Result.GetMemberUtilization(M, LC);
				return DisplayHelper.ColorFromHSV(120.0 - memberUtilization * 120.0, 1.0, 1.0);
			}
			case DisplayResultsType.ReuseNew:
			{
				bool flag8 = EG == null;
				if (flag8)
				{
					return ColorPalette.CBlank;
				}
				switch (EG.Type)
				{
				case ElementType.Zero:
					return ColorPalette.CBlank;
				case ElementType.Reuse:
					return ColorPalette.CReuseUsed;
				case ElementType.New:
					return ColorPalette.CNewUsed;
				default:
					return ColorPalette.CBlank;
				}
				break;
			}
			case DisplayResultsType.Mass:
			{
				bool flag9 = S.Results != null;
				if (!flag9)
				{
					double num2 = M.Length * M.CrossSection.Area * M.Material.Density / S.Results.MaxMemberMass;
					return DisplayHelper.ColorFromHSV(120.0 - num2 * 120.0, 1.0, 1.0);
				}
				bool flag10 = EG == null;
				if (flag10)
				{
					double num3 = M.Length * M.CrossSection.Area * M.Material.Density / S.Results.MaxMemberMass;
					return DisplayHelper.ColorFromHSV(120.0 - num3 * 120.0, 1.0, 1.0);
				}
				double num4 = M.Length * EG.CrossSection.Area * EG.Material.Density / S.Results.MaxMemberMass;
				return DisplayHelper.ColorFromHSV(120.0 - num4 * 120.0, 1.0, 1.0);
			}
			case DisplayResultsType.Impact:
			{
				bool flag11 = S.Results != null && S.LCA != null;
				if (!flag11)
				{
					return ColorPalette.CBlank;
				}
				bool flag12 = EG == null;
				if (flag12)
				{
					double num5 = S.LCA.ReturnMemberImpact(M) / S.Results.MaxMemberImpact;
					return DisplayHelper.ColorFromHSV(120.0 - num5 * 120.0, 1.0, 1.0);
				}
				double num6 = S.LCA.ReturnElementMemberImpact(EG, EG.AlreadyCounted[ElementIndex], M) / S.Results.MaxMemberImpact;
				EG.AlreadyCounted[ElementIndex] = true;
				return DisplayHelper.ColorFromHSV(120.0 - num6 * 120.0, 1.0, 1.0);
			}
			}
			return ColorPalette.CBlank;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000099A0 File Offset: 0x00007BA0
		public static Color GetElementColor(ElementGroup EG, DisplayResultsType DRType)
		{
			switch (DRType)
			{
			case DisplayResultsType.Blank:
				return ColorPalette.CBlank;
			case DisplayResultsType.BarBeam:
				return ColorPalette.CBlank;
			case DisplayResultsType.Forces:
				return ColorPalette.CBlank;
			case DisplayResultsType.Utilization:
				return ColorPalette.CBlank;
			case DisplayResultsType.ReuseNew:
			{
				bool flag = EG.Type == ElementType.Reuse;
				if (flag)
				{
					return ColorPalette.CReuse;
				}
				bool flag2 = EG.Type == ElementType.New;
				if (flag2)
				{
					return ColorPalette.CNew;
				}
				return ColorPalette.CBlank;
			}
			case DisplayResultsType.Mass:
				return ColorPalette.CBlank;
			case DisplayResultsType.Impact:
				return ColorPalette.CBlank;
			}
			return ColorPalette.CBlank;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00009A54 File Offset: 0x00007C54
		public static Color ColorFromHSV(double hue, double saturation, double value)
		{
			int num = Convert.ToInt32(Math.Floor(hue / 60.0)) % 6;
			double num2 = hue / 60.0 - Math.Floor(hue / 60.0);
			value *= 255.0;
			int num3 = Convert.ToInt32(value);
			int num4 = Convert.ToInt32(value * (1.0 - saturation));
			int num5 = Convert.ToInt32(value * (1.0 - num2 * saturation));
			int num6 = Convert.ToInt32(value * (1.0 - (1.0 - num2) * saturation));
			bool flag = num == 0;
			Color result;
			if (flag)
			{
				result = Color.FromArgb(255, num3, num6, num4);
			}
			else
			{
				bool flag2 = num == 1;
				if (flag2)
				{
					result = Color.FromArgb(255, num5, num3, num4);
				}
				else
				{
					bool flag3 = num == 2;
					if (flag3)
					{
						result = Color.FromArgb(255, num4, num3, num6);
					}
					else
					{
						bool flag4 = num == 3;
						if (flag4)
						{
							result = Color.FromArgb(255, num4, num5, num3);
						}
						else
						{
							bool flag5 = num == 4;
							if (flag5)
							{
								result = Color.FromArgb(255, num6, num4, num3);
							}
							else
							{
								result = Color.FromArgb(255, num3, num4, num5);
							}
						}
					}
				}
			}
			return result;
		}
	}
}
