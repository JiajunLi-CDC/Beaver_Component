using System;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.LinearAlgebra;
using Beaver3D.Model;
using Beaver3D.Model.CrossSections;
using Beaver3D.Model.Materials;
using Beaver.Properties;
using Rhino.Geometry;

namespace Beaver.Components.Model
{
	// Token: 0x02000011 RID: 17
	public class Bar_Component : GH_Component
	{
		// Token: 0x06000096 RID: 150 RVA: 0x000051EB File Offset: 0x000033EB
		public Bar_Component() : base("Bar", "Bar (Truss)", "Creates a truss bar element that has pinned ends and only carries normal forces", "Beaver", "01 Geometry")
		{
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005210 File Offset: 0x00003410
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddLineParameter("Lines", "Line", "Centerline of the bar", 0);
			pManager.AddGenericParameter("Material", "Mat", "Material of the bar", 0);
			pManager.AddGenericParameter("CrossSection", "CS", "Bar cross section", 0);
			pManager.AddVectorParameter("Normal", "Normal", "Normal direction of the cross section. Default (0,0,1)", 0, Vector3d.ZAxis);
			pManager[1].Optional = true;
	
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005005 File Offset: 0x00003205
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Bar", "M", "Bar member", 0);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00005290 File Offset: 0x00003490
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			Line line = default(Line);
			IMaterial material = default(Steel);
			ICrossSection crossSection = new CircularSection(0.05);
			Vector3d zaxis = Vector3d.ZAxis;
			DA.GetData<Line>(0, ref line);
			DA.GetData<IMaterial>(1, ref material);
			DA.GetData<ICrossSection>(2, ref crossSection);
			DA.GetData<Vector3d>(3, ref zaxis);
			Bar bar = new Bar(new Node(line.FromX, line.FromY, line.FromZ), new Node(line.ToX, line.ToY, line.ToZ));
			bar.SetMaterial(material);
			bar.SetCrossSection(crossSection);
			bar.SetNormal(new Vector(new double[]
			{
				zaxis.X,
				zaxis.Y,
				zaxis.Z
			}));
			bool normalOverwritten = bar.NormalOverwritten;
			if (normalOverwritten)
			{
				this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "The provided member normal " + zaxis.ToString() + " coincides with the member direction. Normal has been overwritten to " + bar.Normal.ToString());
			}
			DA.SetData(0, bar.CloneBar());
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000053BB File Offset: 0x000035BB
		protected override Bitmap Icon
		{
			get
			{
				return Resources.Bar_BeaverIcon;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000053C2 File Offset: 0x000035C2
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("ce4c1b75-be42-49ba-b34e-96cf4e41d989");
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600009C RID: 156 RVA: 0x000053D0 File Offset: 0x000035D0
		public override GH_Exposure Exposure
		{
			get
			{
				return GH_Exposure.secondary;
			}
		}
	}
}
