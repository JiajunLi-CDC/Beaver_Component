using System;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model.CrossSections;
using Beaver3D.Model.Materials;
using Beaver3D.Reuse;
using Beaver.Properties;

namespace Beaver.Reuse
{
	// Token: 0x02000006 RID: 6
	public class ReuseElementGroup_Component : GH_Component
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00002323 File Offset: 0x00000523
		public ReuseElementGroup_Component() : base("Reused Element", "ReusedElement", "Creates a group of identical stock elements for reuse", "Beaver", "05 Reuse")
		{
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002348 File Offset: 0x00000548
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddGenericParameter("Material", "Mat", "Element material", 0);
			pManager.AddGenericParameter("CrossSection", "CS", "Element cross section", 0);
			pManager.AddNumberParameter("Length", "Length", "Element length", 0);
			pManager.AddIntegerParameter("Amount of elements", "n", "The number of available identical elements", 0);
			pManager.AddBooleanParameter("Elements can be cut", "Cut", "True = elements can be cut, False = elements cannot be cut", 0, true);
			pManager.AddTextParameter("Name", "Name", "Name of the element group", 0, "default");
			pManager[0].Optional = true;
			pManager[1].Optional = true;
			pManager[2].Optional = true;
			pManager[3].Optional = true;
			pManager[4].Optional = true;
			pManager[5].Optional = true;
			
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000243A File Offset: 0x0000063A
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("ReuseElementGroup", "E", "Group of identical stock elements", 0);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002454 File Offset: 0x00000654
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			IMaterial material = default(Steel);
			ICrossSection crossSection = new CircularSection(0.05);
			double length = 0.0;
			int numberOfElements = 0;
			bool canBeCut = true;
			string name = null;
			DA.GetData<IMaterial>(0, ref material);
			DA.GetData<ICrossSection>(1, ref crossSection);
			DA.GetData<double>(2, ref length);
			DA.GetData<int>(3, ref numberOfElements);
			DA.GetData<bool>(4, ref canBeCut);
			DA.GetData<string>(5, ref name);
			DA.SetData(0, new ElementGroup(ElementType.Reuse, material, crossSection, length, numberOfElements, canBeCut, name));
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000024E5 File Offset: 0x000006E5
		protected override Bitmap Icon
		{
			get
			{
				return Resources.ReuseElement_BeaverIcon;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000024EC File Offset: 0x000006EC
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("45a74b75-6eef-47b1-9f93-b34dddc730a9");
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000024F8 File Offset: 0x000006F8
		public override GH_Exposure Exposure
		{
			get
			{
				return GH_Exposure.secondary;
			}
		}
	}
}
