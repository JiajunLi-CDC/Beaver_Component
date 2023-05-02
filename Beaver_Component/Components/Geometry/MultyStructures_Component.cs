using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model;
using Beaver.Properties;

namespace Beaver.Components.Model
{
	// Token: 0x02000013 RID: 19
	public class MultyStructures_Component : GH_Component
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x0000572B File Offset: 0x0000392B
		public MultyStructures_Component() : base("MultyStructures", "MS", "MultyStructures", "Beaver", "01 Geometry")
		{
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005750 File Offset: 0x00003950
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddGenericParameter("Structures", "MS", "List of the structures", GH_ParamAccess.list);
			pManager.AddNumberParameter("ProductionLenth", "PC", "Length of Production", GH_ParamAccess.list);
			pManager.AddNumberParameter("Tolerance", "t", "Tolerance of Production between each cluster", 0,0.1);
			pManager.AddBooleanParameter("Reset", "Reset", "Reset", GH_ParamAccess.item,false);

			pManager[0].Optional = true;
			pManager[1].Optional = true;

		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000057BF File Offset: 0x000039BF
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Structure", "S", "Structure", 0);
			pManager.AddGenericParameter("each_Structure", "杆件所属结构", "Structure", GH_ParamAccess.list);
			pManager.AddGenericParameter("productionLenth", "PL", "最终生产杆件长度List", GH_ParamAccess.list);
			pManager.AddGenericParameter("each_productionLenth", "EPL", "最终每个杆件的生产杆件长度List", GH_ParamAccess.list);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000057DC File Offset: 0x000039DC
		protected override void SolveInstance(IGH_DataAccess DA)
		{

			List<Structure> Structurelist = new List<Structure>();
			List<double> iProductionLength = new List<double>();
			double iTolerance = new double();
			bool ifRest = false;

			DA.GetDataList<Structure>(0, Structurelist);     //获取多个结构	
			DA.GetDataList<double>(1, iProductionLength);   //获取生产长度
			DA.GetData(2, ref iTolerance);   //获取误差
			DA.GetData(3, ref ifRest);     //获取布尔值	

			Structure structure = new Structure();

			if (!ifRest)
            {
				structure = new Structure();

                structure.addMultyStructureInformation(Structurelist);

				structure.member_prductionLenth = iProductionLength;  //赋予生产长度
				structure.merge_structure_num = Structurelist.Count;  //赋予结构数量值
				structure.setMemberProductionLength(iTolerance);  //赋予每个杆件生产长度
			}


			List<int> attribute = new List<int>();
			List<double> each_productionLen = new List<double>();

			for (int i = 0; i < structure.Members.Count; i++)
            {
				int num = structure.Members[i].structure_num;
				double len = structure.Members[i].Production_length;
				attribute.Add(num);
				each_productionLen.Add(len);
			}

			DA.SetData(0, structure.Clone());
			DA.SetDataList(1, attribute);
			DA.SetDataList(2, structure.Clone().member_prductionLenth);
			DA.SetDataList(3, each_productionLen);
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00005900 File Offset: 0x00003B00
		protected override Bitmap Icon
		{
			get
			{
				return Resources.MultyStructure_BeaverIcon;
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
				return new Guid("2313f09f-392a-44ac-8322-767d1e173261");
			}
		}
	}
}
