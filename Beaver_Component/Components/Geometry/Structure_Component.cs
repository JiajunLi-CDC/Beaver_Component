using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model;
using Beaver.Properties;

namespace Beaver.Components.Model
{
	// Token: 0x02000013 RID: 19
	public class Structure_Component : GH_Component
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x0000572B File Offset: 0x0000392B
		public Structure_Component() : base("Structure", "S", "Structure", "Beaver", "01 Geometry")
		{
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005750 File Offset: 0x00003950
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddGenericParameter("Members", "M", "Members of the structure", GH_ParamAccess.list);
			pManager.AddGenericParameter("Supports", "Sp", "Supports of the structure", GH_ParamAccess.list);
			pManager.AddGenericParameter("LoadCases", "LC", "Load Cases acting on the structure", GH_ParamAccess.list);
			pManager.AddBooleanParameter("Reset", "Reset", "Reset", GH_ParamAccess.item, false);
			pManager[1].Optional = true;
			pManager[2].Optional = true;
			
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000057BF File Offset: 0x000039BF
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Structure", "S", "Structure", 0);
			pManager.AddGenericParameter("barcopy", "S", "Structure", GH_ParamAccess.list);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000057DC File Offset: 0x000039DC
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			bool ifRest = false;
			List<IMember> list = new List<IMember>();
			List<Support> list2 = new List<Support>();
			List<LoadCase> list3 = new List<LoadCase>();
			List<Bar> list4 = new List<Bar>();

			DA.GetDataList<Bar>(0, list4);     //获取杆件
			DA.GetDataList<Support>(1, list2);      //获取支座
			DA.GetDataList<LoadCase>(2, list3);       //获取荷载
			DA.GetData(3, ref ifRest);

			


			Structure structure = new Structure();

			if (!ifRest)
            {
				structure = new Structure();

				for (int i = 0; i < list4.Count; i++)
				{
					structure.original_Members.Add(list4[i].CloneBar());   //储存原始的bar
				}

				for (int i = 0; i < list4.Count; i++)
				{
					list.Add(list4[i].Clone());  //转化为Imember
				}

				foreach (IMember m in list)
				{
					structure.AddMember(m);
				}
				foreach (LoadCase lc in list3)
				{
					structure.AddLoadcase(lc);
				}
				foreach (Support s in list2)
				{
					structure.AddSupport(s);
				}

			}
				
			DA.SetData(0, structure.Clone());
			DA.SetDataList(1, structure.Clone().original_Members);
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00005900 File Offset: 0x00003B00
		protected override Bitmap Icon
		{
			get
			{
				return Resources.Structure_BeaverIcon;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00005907 File Offset: 0x00003B07
		public override GH_Exposure Exposure
		{
			get
			{
				return GH_Exposure.secondary;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000AA RID: 170 RVA: 0x0000590A File Offset: 0x00003B0A
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("2313f09f-392a-44ac-8322-767d1e173461");
			}
		}
	}
}
