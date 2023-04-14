using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model;
using Beaver.Properties;

namespace Beaver.Model.Loads
{
	// Token: 0x02000018 RID: 24
	public class LoadCase_Component : GH_Component
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x0000661F File Offset: 0x0000481F
		public LoadCase_Component() : base("LoadCase", "LoadCase", "Creates a LoadCase", "Beaver", "04 Loads")
		{
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00006644 File Offset: 0x00004844
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddTextParameter("Name", "N", "Loadcase Name", 0, "LC1");
			pManager.AddGenericParameter("PointLoads", "PL", "AllPointLoads", GH_ParamAccess.list);
			pManager.AddGenericParameter("DisplacementBounds", "DB", "All Displacement Bounds", GH_ParamAccess.list);
			pManager.AddNumberParameter("SelfWeightFactor", "SW", "Factor for self-weight of members (lumped at nodes; 0 = off; 1.0 -> 10 kg/ms2)", 0, 1.0);
			pManager[0].Optional = true;
			pManager[1].Optional = true;
			pManager[2].Optional = true;
			pManager[3].Optional = true;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000066F4 File Offset: 0x000048F4
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("LoadCase", "LC", "Load Case", 0);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00006710 File Offset: 0x00004910
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			List<PointLoad> list = new List<PointLoad>();
			List<DisplacementBound> list2 = new List<DisplacementBound>();    //位移界限
			string name = "";
			double selfWeightFactor = 1.0;  //自重系数

			DA.GetData<string>(0, ref name);  //获取荷载名称
			DA.GetDataList<PointLoad>(1, list);     //获取点荷载
			DA.GetDataList<DisplacementBound>(2, list2);    //获取位移界限
			DA.GetData<double>(3, ref selfWeightFactor);

			LoadCase loadCase = new LoadCase(name, selfWeightFactor);    //实例化


			foreach (PointLoad pointLoad in list)
			{
				bool flag = loadCase.Loads.Contains(pointLoad);    //如果该节点已经包含该荷载定义
				if (flag)
				{
					this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Multiple force or moment vectors are defined for the same Node. Force and moment vectors are added!");
				}
				loadCase.AddLoad(pointLoad);  //增加点荷载
			}
			foreach (DisplacementBound displacementBound in list2)
			{
				bool flag2 = loadCase.DisplacementBounds.Contains(displacementBound);
				if (flag2)
				{
					this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Multiple displacement bound vectors are defined for the same Node. The last item is taken!");
				}
				loadCase.AddDisplacementBound(displacementBound);  // 增加位移界限
			}
			DA.SetData(0, loadCase);
		}

		
		protected override Bitmap Icon
		{
			get
			{
				return Resources.LoadCase_BeaverIcon;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00006853 File Offset: 0x00004A53
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("6c97a116-a198-4f10-a808-dfec635fd6d5");
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00006860 File Offset: 0x00004A60
		public override GH_Exposure Exposure
		{
			get
			{
				return GH_Exposure.quinary;
			}
		}
	}
}
