using System;
using System.Collections.Generic;
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
	// Token: 0x02000010 RID: 16
	public class Bar_Component_Extended : GH_Component
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00004ED7 File Offset: 0x000030D7
		public Bar_Component_Extended() : base("Bar Extended", "BarExt (Truss)", "Creates a truss bar element that has pinned ends and only carries normal forces", "Beaver", "01 Geometry")
		{
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004EFC File Offset: 0x000030FC
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddLineParameter("Lines", "Line", "Centerline of the bar", 0);
			pManager.AddGenericParameter("Material", "Mat", "Material of the bar", 0);
			pManager.AddGenericParameter("CrossSection", "CS", "Bar cross section", 0);
			pManager.AddVectorParameter("Normal", "Normal", "Normal direction of the cross section. Default (0,0,1)", 0, Vector3d.ZAxis);
			pManager.AddIntegerParameter("BucklingType", "Buckling", "BucklingType: 0 = Off;  1 = Euler; 2 = Eurocode 2", 0, 0);
			pManager.AddNumberParameter("BucklingLength", "BLength", "BucklingLength", 0);
			pManager.AddTextParameter("AllowedCrossSections", "allowedCS", "List of allowed cross-sections to be assigned", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
            pManager[6].Optional = true;
        }

		// Token: 0x06000091 RID: 145 RVA: 0x00005005 File Offset: 0x00003205
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Bar", "M", "Bar member", 0);
			pManager.AddBooleanParameter("Bar", "M", "Bar member", 0);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005020 File Offset: 0x00003220
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			Line line = default(Line);
			IMaterial material = default(Steel);
			ICrossSection crossSection = new CircularSection(0.05);
			Vector3d zaxis = Vector3d.ZAxis;   //杆件截面法向量
			double bucklingLength = 0.0;
			int bucklingType = 0;
			int minCompound = 1;
			int maxCompound = 1;
			int groupNumber = -1;
			List<string> list = new List<string>();

			DA.GetData<Line>(0, ref line);
			DA.GetData<IMaterial>(1, ref material);
			DA.GetData<ICrossSection>(2, ref crossSection);
			DA.GetData<Vector3d>(3, ref zaxis);
			DA.GetData<int>(4, ref bucklingType);
			DA.GetDataList<string>(6, list);

			bool flag = !DA.GetData<double>(5, ref bucklingLength);
			if (flag)  //如果没有获取到bucklingLength数据
			{
				bucklingLength = line.Length;  //屈曲长度为杆件长度
			}

			Bar bar = new Bar(new Node(line.FromX, line.FromY, line.FromZ), new Node(line.ToX, line.ToY, line.ToZ));
			bar.SetMaterial(material);
			bar.SetCrossSection(crossSection);
			//设置截面法向量
			bar.SetNormal(new Vector(new double[]   
			{
				zaxis.X,
				zaxis.Y,
				zaxis.Z
			}));   
			//设置屈曲类型和屈曲长度
			bar.SetBuckling((BucklingType)bucklingType, bucklingLength);
			//设置最小最大复合部分，默认为1
			bar.SetMinMaxCompoundSection(minCompound, maxCompound);
			//设置杆件所属的结构种类编号，默认为-1
			bar.SetGroupNumber(groupNumber);
			//设置允许的截面大小list
			bar.SetAllowedCrossSections(list);

			bool normalOverwritten = bar.NormalOverwritten;  //默认为false，r如果出现设置的法向量和杆件方向相同则为true
			if (normalOverwritten)
			{
				this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "The provided member normal " + zaxis.ToString() + " coincides with the member direction. Normal has been overwritten to " + bar.Normal.ToString());
			}


			DA.SetData(0, bar);
			DA.SetData(1, normalOverwritten);
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000051C4 File Offset: 0x000033C4
		protected override Bitmap Icon
		{
			get
			{
				return Resources.Bar_Extended_BeaverIcon;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000094 RID: 148 RVA: 0x000051CB File Offset: 0x000033CB
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("ce4c1b75-be42-49ba-b34e-96cf4e41d964");
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000051D8 File Offset: 0x000033D8
		public override GH_Exposure Exposure
		{
			get
			{
				return GH_Exposure.secondary;
			}
		}
	}
}
