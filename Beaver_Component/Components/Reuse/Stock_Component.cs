using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Reuse;
using Beaver.Properties;

namespace Beaver.Reuse
{
	// Token: 0x02000004 RID: 4
	public class Stock_Component : GH_Component
	{
		// Token: 0x0600000A RID: 10 RVA: 0x0000212F File Offset: 0x0000032F
		public Stock_Component() : base("Stock", "Stock", "Stock of reclaimed elements and new element candidates", "Beaver", "05 Reuse")
		{
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002152 File Offset: 0x00000352
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddGenericParameter("Elements reuse and new", "E", "List of stock and new elements", GH_ParamAccess.list);
			pManager.AddIntegerParameter("Order elements", "Order", "Sort elements: 0 = Off, 1 = Type (Reuse then New), 2 = Type then ForceCapacity then Length (desc.), 3 = Type then Length then ForceCapacity (desc.)", 0, 0);
			pManager[1].Optional = true;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002192 File Offset: 0x00000392
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Stock", "Stock", "Stock of reclaimed elements and new element candidates", 0);
			pManager.AddGenericParameter("CrossSectionType_num", "CrossSectionType_num", "number of different Cross Section type", GH_ParamAccess.list);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021AC File Offset: 0x000003AC
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			List<ElementGroup> list = new List<ElementGroup>();
			int sortBy = 0;
			DA.GetDataList<ElementGroup>(0, list);
			DA.GetData<int>(1, ref sortBy);

			Stock stock = new Stock(list, (SortStockElementsBy)sortBy);
			stock.GetDifferentCrossSectionGroup();

			List<double> debug= new List<double>();
			for(int i = 0; i < stock.crossSectionType.Count; i++)
            {
				debug.Add(stock.crossSectionType[i].Area);
            }

			DA.SetData(0,stock);
			DA.SetDataList(1, debug);
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000021E4 File Offset: 0x000003E4
		protected override Bitmap Icon
		{
			get
			{
				return Resources.Stock_BeaverIcon;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000021EB File Offset: 0x000003EB
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("b80eb706-ed47-4a1e-a78b-97a894215b5f");
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000021F8 File Offset: 0x000003F8
		public override GH_Exposure Exposure
		{
			get
			{
				return GH_Exposure.quarternary;
			}
		}
	}
}
