using System;
using System.Drawing;
using Grasshopper.Kernel;
using Beaver3D.Model.CrossSections;
using Beaver3D.Model.Materials;
using Beaver3D.Reuse;
using Beaver.Properties;

namespace Beaver.Reuse
{
	// Token: 0x02000005 RID: 5
	public class NewElementGroup_Component : GH_Component
	{
		// Token: 0x06000011 RID: 17 RVA: 0x0000220B File Offset: 0x0000040B
		public NewElementGroup_Component() : base("New Element", "NewElement", "Creates a new element candidate", "Beaver", "05 Reuse")
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002230 File Offset: 0x00000430
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddGenericParameter("Material", "Mat", "Element material", 0);
			pManager.AddGenericParameter("CrossSection", "CS", "Element cross section", 0);
			pManager.AddTextParameter("Name", "Name", "Element group name", 0);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002283 File Offset: 0x00000483
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("NewElement", "E", "Group of new elements", 0);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000022A0 File Offset: 0x000004A0
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			IMaterial material = default(Steel);
			ICrossSection crossSection = new CircularSection(0.05);
			string name = null;
			DA.GetData<IMaterial>(0, ref material);
			DA.GetData<ICrossSection>(1, ref crossSection);
			DA.GetData<string>(2, ref name);
			DA.SetData(0, new ElementGroup(material, crossSection, name));
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000022FC File Offset: 0x000004FC
		protected override Bitmap Icon
		{
			get
			{
				return Resources.NewElement_BeaverIcon;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002303 File Offset: 0x00000503
		public override Guid ComponentGuid
		{
			get
			{
				return new Guid("45a74b75-6eef-47b1-9f93-b24dddc760c7");
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002310 File Offset: 0x00000510
		public override GH_Exposure Exposure
		{
			get
			{
				return GH_Exposure.secondary;
			}
		}
	}
}
